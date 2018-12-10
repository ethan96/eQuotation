using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class SA_APPLICATION2COMPANY : IRepository<SA_APPLICATION2COMPANY>
    {
        
        public companyType CompanyTypeX
        {
            get
            {
                if (Enum.IsDefined(typeof(companyType), this.CompanyType))
                {
                    return (companyType)Enum.Parse(typeof(companyType), this.CompanyType.ToString());
                }
                return companyType.SholdTo;
            }
       }
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
            { CurrentContext.Set<SA_APPLICATION2COMPANY>().Attach(this); }
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
