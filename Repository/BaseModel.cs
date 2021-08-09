using Model.Data.Context;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace Model
{
    public abstract class BaseModel
    {
        [Key]
        [Required]
        public string Id { get; set; } // Guid.NewGuid().ToString()
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public DateTime UpdateTime { get; set; }

        public virtual void OnCreate(ShoppingCartContext context) { }

        public virtual void OnDelete(ShoppingCartContext context) { }

        public virtual void OnModify(ShoppingCartContext context, DbPropertyValues originals) { }
        #region DbContext
        static ShoppingCartContext _dbContext = null;
        public static ShoppingCartContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {                    
                    _dbContext = new ShoppingCartContext();
                }
                return _dbContext;
            }
        }
        #endregion
    }
}
