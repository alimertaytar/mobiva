using Microsoft.EntityFrameworkCore;
using Objects;
using Objects.ViewModel;

namespace api.mobiva
{

    public class ContextApi : DbContext
    {
        public ContextApi()
        {
            //
        }
        public ContextApi(DbContextOptions<ContextApi> options) : base(options)
        {

        }
        public virtual DbSet<AppUser> AppUser { get; set; }
        public virtual DbSet<AppUserToDo> AppUserToDo { get; set; }
        public virtual DbSet<AppUserType> AppUserType { get; set; }
        public virtual DbSet<CashTransaction> CashTransaction { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrder { get; set; }
        public virtual DbSet<Dealer> Dealer { get; set; }
        public virtual DbSet<DealerAppUser> DealerAppUser { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductBrand> ProductBrand { get; set; }
        public virtual DbSet<ProductColor> ProductColor { get; set; }
        public virtual DbSet<ProductDetail> ProductDetail { get; set; }
        public virtual DbSet<ProductDocument> ProductDocument { get; set; }
        public virtual DbSet<ProductDocumentType> ProductDocumentType { get; set; }
        public virtual DbSet<ProductFeature> ProductFeature { get; set; }
        public virtual DbSet<ProductFeatureList> ProductFeatureList { get; set; }
        public virtual DbSet<ProductModel> ProductModel { get; set; }
        public virtual DbSet<ProductStatus> ProductStatus { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<ProductTypeSub> ProductTypeSub { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<Sales> Sales { get; set; }
        public virtual DbSet<TechnicalService> TechnicalService { get; set; }
        public virtual DbSet<TechnicalServiceStatus> TechnicalServiceStatus { get; set; }
        public virtual DbSet<TechnicalServiceHistory> TechnicalServiceHistory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CashTransaction>().Ignore(t => t.AppUser);
            modelBuilder.Entity<CashTransaction>().Ignore(t => t.AppUser1);

            modelBuilder.Entity<AppUser>().Ignore(t => t.CashTransaction);
            modelBuilder.Entity<AppUser>().Ignore(t => t.CashTransaction);
            modelBuilder.Entity<AppUser>().Ignore(t => t.Sale);
            modelBuilder.Entity<AppUser>().Ignore(t => t.Sale1);
            modelBuilder.Entity<AppUser>().Ignore(t => t.Sale2);

            modelBuilder.Entity<Sale>().Ignore(t => t.AppUser);
            modelBuilder.Entity<Sale>().Ignore(t => t.AppUser1);
            modelBuilder.Entity<Sale>().Ignore(t => t.AppUser2);

            modelBuilder.Entity<AppUser>().Ignore(t => t.AppUserToDo);
            modelBuilder.Entity<AppUser>().Ignore(t => t.AppUserToDo1);
            modelBuilder.Entity<AppUserToDo>().Ignore(t => t.AppUser);
            modelBuilder.Entity<AppUserToDo>().Ignore(t => t.AppUser1);
            base.OnModelCreating(modelBuilder);
        }


    }
}
