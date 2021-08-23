using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Solution.Application.Common;
using Solution.Data.EF;
using Solution.Data.Entities;
using Solution.ViewModels.Categories;
using Solution.ViewModels.Common;
using Solution.ViewModels.ProductImages;
using Solution.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Solution.Application.Products
{
    public class ProductService : IProductService
    {
        private readonly SolutionDbContext _context;
        private readonly IStorageService _storageService;

        public ProductService(SolutionDbContext context, IStorageService storageService)
        {
            _storageService = storageService;
            _context = context;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> AddImage(ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.isDefault,
                ProductId = request.productId,
                SortOrder = request.SortOrder
            };
            if (request.Image != null)
            {
                productImage.ImagePath = await SaveFile(request.Image);
                productImage.FileSize = request.Image.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                DateCreated = DateTime.Now,
            };
            if (request.Image != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        DateCreated=DateTime.Now,
                        FileSize=request.Image.Length,
                        ImagePath=await SaveFile(request.Image),
                        IsDefault=true,
                        SortOrder=1
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<ApiResult<int>> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return new ApiErrorResult<int>("Không tìm thấy sản phẩm");
            var images = _context.ProductImages.Where(x => x.ProductId == productId);
            foreach (var img in images)
            {
                await _storageService.DeleteFileAsync(img.ImagePath);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int>();
        }

        public async Task<Pagination<ProductVM>> GetProductPaging(GetProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pc in _context.ProductCategories on p.Id equals pc.productId into ppc
                        from pc in ppc.DefaultIfEmpty()
                        join pm in _context.ProductImages on p.Id equals pm.ProductId into ppm
                        from pm in ppm.DefaultIfEmpty()
                        where pm.IsDefault == true
                        select new { p, pc, pm };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword));
            }
            if (request.categoryId != null && request.categoryId != 0)
            {
                query = query.Where(x => x.pc.categoryId == request.categoryId);
            }

            //Total row
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new ProductVM()
            {
                Id = x.p.Id,
                Name = x.p.Name,
                Description = x.p.Description,
                Price = x.p.Price,
                DateCreated = x.p.DateCreated,
                Image = x.pm.ImagePath,
            }).OrderBy(x => x.Id).ToListAsync();
            var pagedResult = new Pagination<ProductVM>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null) new ApiErrorResult<int>("Không tìm tháy sản phẩm");
            product.Name = request.Name;
            product.Price = request.Price;
            product.Description = request.Description;
            if (request.image != null)
            {
                var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == product.Id && x.IsDefault == true);
                if (image != null)
                {
                    await _storageService.DeleteFileAsync(image.ImagePath);
                    image.ImagePath = await SaveFile(request.image);
                    image.FileSize = request.image.Length;
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int>();
        }

        public async Task<ProductDetail> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return null;
            var category = await _context.ProductCategories.Where(x => x.productId.Equals(productId)).Select(x => x.Category.Name).ToListAsync();
            var image = await _context.ProductImages.Where(x => x.ProductId == productId).Select(x => x.ImagePath).ToListAsync();
            var productDetail = new ProductDetail()
            {
                Id=productId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                DateCreated = product.DateCreated,
                Categories = category,
                Image = image,
            };

            return productDetail;
        }

        public async Task<ProductImageViewModal> GetImageById(int productImageId)
        {
            var productImage = await _context.ProductImages.FindAsync(productImageId);
            if (productImage == null) return null;
            var image = new ProductImageViewModal()
            {
                Caption = productImage.Caption,
                FileSize = productImage.FileSize,
                ImagePath = productImage.ImagePath,
                IsDefault = productImage.IsDefault,
                DateCreated = productImage.DateCreated,
                SortOrder = productImage.SortOrder,
                Id = productImage.Id,
            };
            return image;
        }

        public async Task<ApiResult<bool>> RemoveImage(int imageId)
        {
            var result = await _context.ProductImages.FindAsync(imageId);
            if (result == null) return new ApiErrorResult<bool>("Ảnh không tồn tại");
            _context.ProductImages.Remove(result);
            await _storageService.DeleteFileAsync(result.ImagePath);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<ProductImageViewModal>> ListImagesByProductId(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(x => new ProductImageViewModal()
                {
                    Id = x.Id,
                    Caption = x.Caption,
                    IsDefault = x.IsDefault,
                    FileSize = x.FileSize,
                    DateCreated = x.DateCreated,
                    ImagePath = x.ImagePath,
                    SortOrder = x.SortOrder
                }).ToListAsync();
        }

        public async Task<ApiResult<bool>> CategoryAssign(CategoryAssignRequest request)
        {
            var productobj = await _context.Products.FindAsync(request.Id);
            if (productobj == null) return new ApiErrorResult<bool>("Sản phẩm không tồn tại");

            foreach (var category in request.Categories)
            {
                var productcategory = await _context.ProductCategories.
                    FirstOrDefaultAsync(x => x.categoryId == int.Parse(category.Id) && x.productId == productobj.Id);
                if (productcategory != null && category.Selected == true)
                {
                    await _context.ProductCategories.AddAsync(productcategory);
                }
                else if (productcategory != null && category.Selected == false)
                {
                    _context.ProductCategories.Remove(productcategory);
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> DefaultImage(int imageId, bool isDefault)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) return new ApiErrorResult<bool>("Không thể tìm thấy ảnh");
            productImage.IsDefault = isDefault;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}