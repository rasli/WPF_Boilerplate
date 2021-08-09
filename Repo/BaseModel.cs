using Repo.Context;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace Repo
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

        public virtual void OnCreate(AppDBContext context) { }

        public virtual void OnDelete(AppDBContext context) { }

        public virtual void OnModify(AppDBContext context, DbPropertyValues originals) { }
        #region DbContext
        static AppDBContext _dbContext = null;
        public static AppDBContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {                    
                    _dbContext = new AppDBContext();
                }
                return _dbContext;
            }
        }
        #endregion
    }
}
