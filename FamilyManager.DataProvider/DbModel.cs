using FamilyManager.DataObject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyManager.DataProvider
{
    public class DbModel : DbContext
    {
        public DbModel()
           : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbModel, Migrations.Configuration>());
           // Database.SetInitializer(new CreateDatabaseIfNotExists<DbModel>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupFamily>()
                        .MapToStoredProcedures();
        }
        public DbSet<MemberFamily> MemberFamily { get; set; }
        public DbSet<GroupFamily> GroupFamily { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<CategoryProduct> CategoryProduct { get; set; }
        public DbSet<FamilyCategory> FamilyCategory { get; set; }
    }
    public class DbModelDBInitializer : CreateDatabaseIfNotExists<DbModel>
    {
       
        protected override void Seed(DbModel context)
        {
            IList<FamilyCategory> defaultCategory = new List<FamilyCategory>();

            defaultCategory.Add(new FamilyCategory() { Name = "Comida" });
            defaultCategory.Add(new FamilyCategory() { Name = "Limpieza" });
            defaultCategory.Add(new FamilyCategory() { Name = "Ropa" });
            defaultCategory.Add(new FamilyCategory() { Name = "GstosFijos" });
            defaultCategory.Add(new FamilyCategory() { Name = "Transporte" });
            defaultCategory.Add(new FamilyCategory() { Name = "Salud" });
            defaultCategory.Add(new FamilyCategory() { Name = "Otros" });

            IList<CategoryProduct> defaultCategoryProduct = new List<CategoryProduct>();
            defaultCategoryProduct.Add(new CategoryProduct() { Name = "Verduras_Frutas",FamilyCategory = defaultCategory .FirstOrDefault(x => x.Name == "Comida") });
            defaultCategoryProduct.Add(new CategoryProduct() { Name = "Carnes", FamilyCategory = defaultCategory.FirstOrDefault(x => x.Name == "Comida") });
            defaultCategoryProduct.Add(new CategoryProduct() { Name = "Comestibles", FamilyCategory = defaultCategory.FirstOrDefault(x => x.Name == "Comida") });
            defaultCategoryProduct.Add(new CategoryProduct() { Name = "Bebidas", FamilyCategory = defaultCategory.FirstOrDefault(x => x.Name == "Comida") });
            defaultCategoryProduct.Add(new CategoryProduct() { Name = "Otros", FamilyCategory = defaultCategory.FirstOrDefault(x => x.Name == "Comida") });

            foreach (FamilyCategory std in defaultCategory)
                context.FamilyCategory.Add(std);
            foreach (CategoryProduct std in defaultCategoryProduct)
                context.CategoryProduct.Add(std);

            base.Seed(context);
        }
    }
}
