using KAS.Uploading.DataAccess.Configurations;
using KAS.Uploading.DataAccess.Extensions;
using KAS.Uploading.Models.Entities;
using KAS.Uploading.Models.Structs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KAS.Uploading.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Contact> Contacts { set; get; }

        public DbSet<Footer> Footers { set; get; }

        public DbSet<Product> Product { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }

        public DbSet<XProduct_ProductCategory> XProduct_ProductCategory { get; set; }

        //public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            #region Identity Config
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(
                users =>
                {
                    users.HasMany(x => x.Claims)
                        .WithOne()
                        .HasForeignKey(x => x.UserId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    users.ToTable("ApplicationUsers").Property(p => p.Id).HasColumnName("UserId");
                }
            );
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("ApplicationUserClaims");
            //.HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("ApplicationRoleClaims");
            //.HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("ApplicationUserLogins");
            //.HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("ApplicationUserRoles");
            //.HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("ApplicationUserTokens");
            //.HasKey(x => new { x.UserId });
            builder.Entity<ApplicationRole>().ToTable("ApplicationRoles");

            builder.AddConfiguration(new FooterConfiguration());
            builder.HasDefaultSchema("kas");

            #endregion Identity Config



        }


        public override int SaveChanges()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (EntityEntry item in modified)
            {
                var changedOrAddedItem = item.Entity as IDateTracking;
                if (changedOrAddedItem != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.DateCreated = DateTime.Now;
                    }
                    changedOrAddedItem.DateModified = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }

    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    //{
    //    public ApplicationDbContext CreateDbContext(string[] args)
    //    {

    //        IConfiguration configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json", optional: true)
    //            .Build();
    //        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
    //        var connectionStrings = configuration.GetConnectionString("AppDbConnection");

    //        Console.WriteLine("co" + connectionStrings);
    //        builder.UseSqlServer(connectionStrings);
    //        return new ApplicationDbContext(builder.Options);
    //    }

}