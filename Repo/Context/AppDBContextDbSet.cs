using Model;
using System.Data.Entity;

namespace Repo.Context
{
    public partial class AppDBContext
    {
        
        public virtual DbSet<Product.ProductDetails> Products { get; set; }
    }
}
