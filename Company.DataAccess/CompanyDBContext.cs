using Microsoft.EntityFrameworkCore;

namespace Company.DataAccess
{
    /// <summary>
    /// The Database Context to be used for the Company DB
    /// </summary>
    public class CompanyDBContext : DbContext
    {
        public CompanyDBContext(DbContextOptions<CompanyDBContext> options) : base(options)
        {
        }

        public DbSet<Models.Company> Companys { get; set; }
        public DbSet<Models.Exchange> Exchange { get; set; }
        public DbSet<Models.Ticker> Tickers { get; set; }
        public DbSet<Models.CompanyExchange> CompanyExchange { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Rename tables
            modelBuilder.Entity<Models.Company>().ToTable("company");
            modelBuilder.Entity<Models.Exchange>().ToTable("exchange");
            modelBuilder.Entity<Models.Ticker>().ToTable("ticker");
            modelBuilder.Entity<Models.CompanyExchange>().ToTable("company_exchange");

            //Setup the many to many relationship for company and exchange tables
            modelBuilder.Entity<Models.CompanyExchange>()
                .HasKey(bc => new { bc.CompanyId, bc.ExchangeId });
            modelBuilder.Entity<Models.CompanyExchange>()
                .HasOne(bc => bc.Company)
                .WithMany(b => b.CompanyExchange)
                .HasForeignKey(bc => bc.CompanyId);
            modelBuilder.Entity<Models.CompanyExchange>()
                .HasOne(bc => bc.Exchange)
                .WithMany(c => c.CompanyExchange)
                .HasForeignKey(bc => bc.ExchangeId);
        }
    }
}
