using eQuotation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eQuotation.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {

        AppDbContext DbContext { get; }

        IRepository<AppAction> AppAction { get; }
        IRepository<AppError> AppError { get; }
        IRepository<AppRoleAction> AppRoleAction { get; }
        IRepository<AppRole> AppRole { get; }
        IRepository<AppUser> AppUser { get; }
        IRepository<AppLogEvent> AppLogEvent { get; }
        IRepository<MenuCategory> MenuCategory { get; }
        IRepository<MenuGroup> MenuGroup { get; }
        IRepository<MenuElement> MenuElement { get; }
        IRepository<MenuControl> MenuControl { get; }
        IRepository<Product> Product { get; }

        IRepository<ProductItem> ProductItem { get; }


        void Save();

    }
}
