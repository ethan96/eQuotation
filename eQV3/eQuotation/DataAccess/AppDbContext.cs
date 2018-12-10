using eQuotation.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace eQuotation.DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public AppDbContext()
            : base("EQ")
        {
            Database.SetInitializer<AppDbContext>(null);
            this.ReservedKeys = new Dictionary<string, string>();
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public Dictionary<string, string> ReservedKeys { get; set; }
        //public virtual IDbSet<AppRole> AppRoles { get; set; }
        public virtual DbSet<AppAction> AppActions { get; set; }
        public virtual DbSet<AppError> AppErrors { get; set; }
        public virtual DbSet<AppLogEvent> AppLogEvents { get; set; }
        public virtual DbSet<MenuCategory> MenuCategories { get; set; }
        public virtual DbSet<MenuGroup> MenuGroups { get; set; }
        public virtual DbSet<MenuElement> MenuElements { get; set; }
        public virtual DbSet<MenuControl> MenuControls { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().ToTable("AppUsers");
            modelBuilder.Entity<AppRole>().ToTable("AppRoles");
            modelBuilder.Entity<AppUserRole>().ToTable("AppUserRoles");
            modelBuilder.Entity<AppUserLogin>().ToTable("AppUserLogins");
            modelBuilder.Entity<AppUserClaim>().ToTable("AppUserClaims");



            //IDENTITY CONTEXT 
            //**********************************************************************
            //**********************************************************************
            //if (modelBuilder == null)
            //    throw new ArgumentNullException("modelBuilder");

            //keep this
            //modelBuilder.Entity<IdentityUser>().ToTable("AppUsers");

            // Change TUser to ApplicationUser everywhere else - IdentityUser 
            // and AppsUser essentially 'share' the AppsUser Table in the database:
            //EntityTypeConfiguration<AppUser> userTable = modelBuilder.Entity<AppUser>().ToTable("AppUsers");

            //userTable.Property((AppUser u) => u.UserName).IsRequired();

            //// EF won't let us swap out IdentityUserRole for AppsUserRole here:
            //modelBuilder.Entity<AppUser>().HasMany<IdentityUserRole>((AppUser u) => u.Roles);
            //modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) =>
            //    new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AppUserRoles");

            //modelBuilder.Entity<IdentityUserRole>().ToTable("AppUserRoles");

            //// Add this, so that IdentityRole can share a table with AppsRole:
            //modelBuilder.Entity<IdentityRole>().ToTable("AppRoles");

            // Change these from IdentityRole to AppsRole:
            //EntityTypeConfiguration<AppRole> entityTypeConfiguration1 = modelBuilder.Entity<AppRole>().ToTable("AppRoles");

            //entityTypeConfiguration1.Property((AppRole r) => r.Name).IsRequired();

            //add the action stuff here
            modelBuilder.Entity<AppRole>().HasMany<AppRoleAction>((AppRole r) => r.Actions);
            modelBuilder.Entity<AppRoleAction>().HasKey((AppRoleAction r) =>
                new { RoleId = r.RoleId, ActionId = r.ActionId }).ToTable("AppRoleActions");

            //and add here
            //EntityTypeConfiguration<AppAction> actionConfig = modelBuilder.Entity<AppAction>().ToTable("AppActions");
            //actionConfig.Property((AppAction a) => a.UriAction).IsRequired();

            // Leave this alone:
            //EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration =
            //modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) =>
            //    new
            //    {
            //        UserId = l.UserId,
            //        LoginProvider = l.LoginProvider,
            //        ProviderKey =
            //            l.ProviderKey
            //    }).ToTable("AppUserLogins");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("AppUserLogins");

            //entityTypeConfiguration.HasRequired<IdentityUser>((IdentityUserLogin u) => u.User);

            //EntityTypeConfiguration<IdentityUserClaim> table1 = modelBuilder.Entity<IdentityUserClaim>().ToTable("AppUserClaims");
            //table1.HasRequired<IdentityUser>((IdentityUserClaim u) => u.UserId);


            //**********************************************************************
            //**********************************************************************

            //modelBuilder.Entity<Vendor>().Property(x => x.ID)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
            //    .HasColumnName("ID")
            //    .HasColumnType("int");

            //modelBuilder.Entity<FileUpload>().Property(x => x.ID)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
            //    .HasColumnName("ID")
            //    .HasColumnType("int");
            //modelBuilder.Entity<Contact>().Property(x => x.ID)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
            //    .HasColumnName("ID")
            //    .HasColumnType("int");

        }
    }
}