using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions.UnitOfWork;

namespace qrwaiter_backend.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext() { }
        public async Task<ITransaction> BeginTransaction()
        {
            var transaction = await Database.BeginTransactionAsync();
            return new EfTransaction(transaction);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){        }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<QrCode> QrCode { get; set; }
        public DbSet<StatisticQrCode> StatisticQrCode { get; set; }
        public DbSet<NotifyDevice> NotifyDevice { get; set; }
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<Table> Table { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
                                       .HasOne(u => u.Restaurant)
                                       .WithOne(r => r.User)
                                       .HasForeignKey<ApplicationUser>(r => r.IdRestaurant);

            modelBuilder.Entity<Restaurant>()
                                       .HasOne(r => r.User)
                                       .WithOne(u => u.Restaurant)
                                       .HasForeignKey<Restaurant>(r => r.IdUser);

            modelBuilder.Entity<QrCode>().HasKey(qr => qr.Id);
            modelBuilder.Entity<QrCode>().HasIndex(qr => qr.Link);





            modelBuilder.Entity<QrCode>().HasOne(qr => qr.Table)
                                         .WithOne(t => t.QrCode)
                                         .HasForeignKey<QrCode>(qr => qr.IdTable);
                                         //.HasForeignKey<Table>(t => t.IdQrCode);
                                            

            modelBuilder.Entity<StatisticQrCode>().HasKey(sqr => sqr.Id);
            modelBuilder.Entity<StatisticQrCode>().HasOne(sqr => sqr.QrCode)
                                                  .WithMany(qr => qr.StatisticQrCodes)
                                                  .HasForeignKey(sqr => sqr.IdQrCode);


            modelBuilder.Entity<NotifyDevice>().HasKey(nd => nd.Id);
            modelBuilder.Entity<NotifyDevice>().HasIndex(nd => nd.DeviceToken);
            modelBuilder.Entity<NotifyDevice>().HasMany(nd => nd.QrCodes).WithMany(qr => qr.NotifyDevices);


            modelBuilder.Entity<Table>().HasKey(t => t.Id);
            modelBuilder.Entity<Table>().HasOne(t => t.Restaurant)
                                        .WithMany(r => r.Tabels)
                                        .HasForeignKey(t => t.IdResaurant);
            modelBuilder.Entity<Table>().HasOne(t => t.QrCode)
                                        .WithOne(r => r.Table)
                                        .HasForeignKey<Table>(t => t.IdQrCode);



            modelBuilder.Entity<Restaurant>().HasKey(r => r.Id);
            modelBuilder.Entity<Restaurant>().OwnsOne(r  => r.Address, nn =>
            {
                nn.ToJson();
            });



            base.OnModelCreating(modelBuilder);
        }

    }
}
