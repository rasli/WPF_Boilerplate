using System.Data.Entity;

namespace Model.Context
{
    public partial class ShoppingCartContext
    {
        public virtual DbSet<Cart> Carts { get; set; }
        //public virtual DbSet<CartItem> CartItems { get; set; }

        public virtual DbSet<Product.ProductDetails> Products { get; set; }
    }
}
