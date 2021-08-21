using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Constants
{
    public class FacebookUserInfoResult
    {
        [JsonProperty("first_name")]
        public string FirstName { set; get; }

        [JsonProperty("last_name")]
        public string LastName { set; get; }

        [JsonProperty("picture")]
        public FacebookPicture Picture { set; get; }

        [JsonProperty("email")]
        public string Email { set; get; }

        [JsonProperty("id")]
        public string Id { set; get; }

        public class FacebookPicture
        {
            [JsonProperty("data")]
            public FacebookPictureData data { set; get; }
        }

        public class FacebookPictureData
        {
            [JsonProperty("height")]
            public long Height { set; get; }

            [JsonProperty("is_silhouette")]
            public bool Issilhouette { set; get; }

            [JsonProperty("url")]
            public Uri Url { set; get; }

            [JsonProperty("width")]
            public long Width { set; get; }
        }
    }
}