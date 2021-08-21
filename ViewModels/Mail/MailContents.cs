using Microsoft.AspNetCore.Http;

namespace Solution.ViewModels.Mail
{
    public class MailContents
    {
        public string From { set; get; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile Attachments { get; set; }
    }
}