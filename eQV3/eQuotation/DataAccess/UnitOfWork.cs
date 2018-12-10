using eQuotation.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace eQuotation.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        public AppDbContext DbContext { get; private set; }

        public UnitOfWork()
        {
            this.DbContext = new AppDbContext();
        }

        #region private properties
        private IRepository<AppAction> actionRepo;
        private IRepository<AppError> errorRepo;
        private IRepository<AppRoleAction> roleActionRepo;
        private IRepository<AppRole> roleRepo;
        private IRepository<AppUser> userRepo;
        private IRepository<AppLogEvent> logEventRepo;
        private IRepository<MenuCategory> mnCategoryRepo;
        private IRepository<MenuGroup> mnGroupRepo;
        private IRepository<MenuElement> mnElementRepo;
        private IRepository<MenuControl> mnControlRepo;
        private IRepository<Product> product;
        private IRepository<ProductItem> ProductItem;

        #endregion

        public IRepository<Product> Product
        {
            get
            {
                if (this.product == null)
                    this.product = new Repository<Product>(DbContext);

                return this.product;
            }
        }




        public IRepository<AppAction> AppAction
        {
            get
            {
                if (this.actionRepo == null)
                    this.actionRepo = new Repository<AppAction>(DbContext);

                return this.actionRepo;
            }
        }
        public IRepository<AppError> AppError
        {
            get
            {
                if (this.errorRepo == null)
                    this.errorRepo = new Repository<AppError>(DbContext);

                return this.errorRepo;
            }
        }
        public IRepository<AppRoleAction> AppRoleAction
        {
            get
            {
                if (this.roleActionRepo == null)
                    this.roleActionRepo = new Repository<AppRoleAction>(DbContext);

                return this.roleActionRepo;
            }
        }
        public IRepository<AppRole> AppRole
        {
            get
            {
                if (this.roleRepo == null)
                    this.roleRepo = new Repository<AppRole>(DbContext);

                return this.roleRepo;
            }
        }
        public IRepository<AppUser> AppUser
        {
            get
            {
                if (this.userRepo == null)
                    this.userRepo = new Repository<AppUser>(DbContext);

                return this.userRepo;
            }
        }
        public IRepository<AppLogEvent> AppLogEvent
        {
            get
            {
                if (this.logEventRepo == null)
                    this.logEventRepo = new Repository<AppLogEvent>(DbContext);

                return this.logEventRepo;
            }
        }
        public IRepository<MenuCategory> MenuCategory
        {
            get
            {
                if (this.mnCategoryRepo == null)
                    this.mnCategoryRepo = new Repository<MenuCategory>(DbContext);

                return this.mnCategoryRepo;
            }
        }
        public IRepository<MenuGroup> MenuGroup
        {
            get
            {
                if (this.mnGroupRepo == null)
                    this.mnGroupRepo = new Repository<MenuGroup>(DbContext);

                return this.mnGroupRepo;
            }
        }
        public IRepository<MenuElement> MenuElement
        {
            get
            {
                if (this.mnElementRepo == null)
                    this.mnElementRepo = new Repository<MenuElement>(DbContext);

                return this.mnElementRepo;
            }
        }
        public IRepository<MenuControl> MenuControl
        {
            get
            {
                if (this.mnControlRepo == null)
                    this.mnControlRepo = new Repository<MenuControl>(DbContext);

                return this.mnControlRepo;
            }
        }

        IRepository<ProductItem> IUnitOfWork.ProductItem
        {
            get
            {
                if (this.ProductItem == null)
                    this.ProductItem = new Repository<ProductItem>(DbContext);

                return this.ProductItem;
            }
        }

        /* --START ENTITY DEFINITION-- */

        /* --END ENTITY DEFINITION-- */

        public void Save()
        {
            try
            {
                DbContext.ReservedKeys.Clear();
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //get all error messages
                var errmsg = string.Concat(ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage));

                throw new HttpException(608, errmsg);
            }
            catch (DbUpdateException ex)
            {
                var err = new StringBuilder("Error Updating Entity: ");

                foreach (var result in ex.Entries)
                {
                    err.AppendFormat("{0}-$$-", result.Entity.GetType().Name);
                }

                err.AppendFormat("Detail Exception-{0}-", ex.ToString());
                throw new HttpException(608, err.ToString());
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}