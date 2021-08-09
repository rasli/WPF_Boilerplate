using Newtonsoft.Json;
using Repo;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Repo
{

    public class Product : BaseModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public List<ProductDetails> ProductDetail { get; set; }

        public class ProductDetails : BaseModel
        {
            //[Key]
            //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ProductId { get; set; }

            [JsonProperty("categoryId")]
            public string CategoryId { get; set; }

            [JsonProperty("janCode")]
            public string JanCode { get; set; }

            [JsonProperty("productName")]
            public string ProductName { get; set; }

            //[JsonProperty("productShortName")]
            //public object ProductShortName { get; set; }

            //[JsonProperty("productKanaName")]
            //public object ProductKanaName { get; set; }

            [JsonProperty("isAgeConfirmed")]
            public bool IsAgeConfirmed { get; set; }

            [JsonProperty("imageUrl")]
            public string ImageUrl { get; set; }

            [JsonProperty("netPrice")]
            public string NetPrice { get; set; }

            [JsonProperty("updatedAt")]
            public string UpdatedAt { get; set; }
            
            public bool IsLocal { get; set; }
        }
    }

}
