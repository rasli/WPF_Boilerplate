using Model;
using Model.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Model.Context
{
    public partial class ShoppingCartContext : DbContext
    {

        public ShoppingCartContext() : base("mssqldb")
        {
            Database.Log = sql =>
            {
                Debug.WriteLine(sql);
            };

            Database.SetInitializer(new DatabaseInitializer());
        }

        private void handleUnimappedMTM<T, U>(DbModelBuilder modelBuilder, Expression<Func<T, ICollection<U>>> field, string tableName) where T : class where U : class
        {
            modelBuilder.Entity<T>().HasMany(field).WithMany().Map(a => a.ToTable(tableName));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(ConfigurationManager.AppSettings["DBConnectionSchema"]);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ManyToManyCascadeDeleteConvention>();
            Type baseType = typeof(BaseModel);
            var thisType = typeof(ShoppingCartContext);
            MethodInfo mtmMethod = thisType.GetMethod("handleUnimappedMTM", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var type in baseType.Assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(baseType)))
            {
                MethodInfo method = type.GetMethod("OnModelCreating", BindingFlags.Public | BindingFlags.Static);
                method?.Invoke(null, new object[] { this, modelBuilder });
                foreach (var prop in type.GetProperties())
                {
                    UnimappedManyToManyAttribute attr = prop.GetCustomAttribute<UnimappedManyToManyAttribute>();
                    if (attr != null)
                    {
                        Type fieldType = prop.PropertyType.GetTypeInfo().GenericTypeArguments[0];
                        var parameter = Expression.Parameter(type);
                        var member = Expression.Property(parameter, type, prop.Name);
                        string tableName = attr.Table ?? type.Name + prop.Name;
                        mtmMethod.MakeGenericMethod(type, fieldType).Invoke(this, new object[] { modelBuilder, Expression.Lambda(member, parameter), tableName });
                    }
                }
            }
        }

       // public int SaveChanges(bool createTransaction = false) { return SaveChanges(null, createTransaction); }

        public int SaveChanges(bool createTransaction = false)
        {
            //if (createTransaction || TransactionalAttribute.Transaction != null) {
            List<DbEntityEntry> copyChangeList = ChangeTracker.Entries().ToList();
            foreach (var entry in copyChangeList)
            {
                if (entry.State == EntityState.Unchanged)
                {
                    continue;
                }
                BaseModel entity = entry.Entity as BaseModel;
                if (entity == null)
                {
                    continue;
                }
                if (entry.State == EntityState.Deleted)
                {
                    entity.OnDelete(this);
                }
                if (entry.State == EntityState.Added)
                {
                    Type type = entity.GetType();
                    PropertyInfo info = type.GetProperty("CreateTime");
                    info?.SetValue(entity, DateTime.Now);
                    info = type.GetProperty("UpdateTime");
                    info?.SetValue(entity, DateTime.Now);

                    entity.OnCreate(this);
                }
                if (entry.State == EntityState.Modified)
                {
                    Type type = entity.GetType();
                    PropertyInfo info = type.GetProperty("UpdateTime");
                    info?.SetValue(entity, DateTime.Now);
                    entity.OnModify(this, entry.OriginalValues);
                    
                    //have to pull required lazy data, Need to check if latest EF can retrive 
                    entity.GetType().GetProperties().Where(p => p.GetGetMethod().IsVirtual && p.IsDefined(typeof(RequiredAttribute))).ForEach(p =>
                    {
                        p.GetValue(entity);
                    });
                }
            }
            return base.SaveChanges();
            //}
            //return 0;
        }
    }
    public class DatabaseConfiguration : DbMigrationsConfiguration<ShoppingCartContext>
    {
        public DatabaseConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "ShoppingCartContext";
        }
    }

    public class DatabaseInitializer : MigrateDatabaseToLatestVersion<ShoppingCartContext, DatabaseConfiguration>
    {

        //private static readonly ILog Log = LogManager.GetLogger(typeof(DatabaseInitializer));

        public override void InitializeDatabase(ShoppingCartContext context)
        {
            try
            {
                base.InitializeDatabase(context);
            }
            catch (AutomaticDataLossException)
            {
                DatabaseConfiguration dbMgConfig = new DatabaseConfiguration { AutomaticMigrationDataLossAllowed = true };
                var mg = new DbMigrator(dbMgConfig);
                var scriptor = new MigratorScriptingDecorator(mg);
                string script = scriptor.ScriptUpdate(null, null);
                //Log.Error("=========================================>>");
                //Log.Error("Attempted Update SQL for automigration");
                //Log.Error("=========================================>>");
                //Log.Error(script);
                //Log.Error("=========================================>>");
                throw;
            }
            catch (Exception ex)
            {
                // Show the error
            }
            Type baseType = typeof(BaseModel);
            var types = baseType.Assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
            types.ForEach(type =>
            {
                MethodInfo method = type.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
                method?.Invoke(null, new object[] { context });
            });
            context.SaveChanges(true);
        }
    }
}
