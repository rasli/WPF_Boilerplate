using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Product
    {
        public int ProductId { get; set; }
        public string CategoryId { get; set; }
        public string JanCode { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public string NetPrice { get; set; }
        public string UpdatedAt { get; set; }
    }
}
