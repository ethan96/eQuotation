using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class SA_FKNBK : IRepository<SA_FKNBK>
    {
        public DbContext CurrentContext
        {
            get
            {
                return MyAdminContext.Current;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add()
        {
            if (CurrentContext.Entry(this).State == EntityState.Detached)
            { CurrentContext.Set<SA_FKNBK>().Attach(this); }
            CurrentContext.Entry(this).State = EntityState.Modified;
            CurrentContext.SaveChanges();
        }

        public void Update()
        {
            CurrentContext.Entry(this).State = EntityState.Modified;
            CurrentContext.SaveChanges();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}