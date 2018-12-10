using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class SA_APPLICATION : IRepository<SA_APPLICATION>
    {
        public AccountWorkFlowStatus StatusX
        {
            get
            {
                if (Enum.IsDefined(typeof(AccountWorkFlowStatus), this.STATUS))
                {
                    return (AccountWorkFlowStatus)Enum.Parse(typeof(AccountWorkFlowStatus), this.STATUS.ToString());
                }
                return AccountWorkFlowStatus.NewRequest;
            }
        }
        public bool IsHaveShipToX()
        {
            ICollection<SA_APPLICATION2COMPANY> thisA2C = this.SA_APPLICATION2COMPANY;

            foreach(SA_APPLICATION2COMPANY item in thisA2C )
            {
                if (item.CompanyTypeX == companyType.ShipTo)
                {
                    return true;
                  
                }
            }
            return false;
        }
        public bool IsHaveBillToX()
        {

            ICollection<SA_APPLICATION2COMPANY> thisA2C = this.SA_APPLICATION2COMPANY;

            foreach (SA_APPLICATION2COMPANY item in thisA2C)
            {
                if (item.CompanyTypeX == companyType.BillTo)
                {
                    return true;
            
                }
            }
            return false;
        }
        public SA_APPLICATION2COMPANY SholdToX()
        {
        
                int EnumVal = (int)companyType.SholdTo;
                SA_APPLICATION2COMPANY _SA_APPLICATION2COMPANY = this.SA_APPLICATION2COMPANY.FirstOrDefault(p => p.CompanyType == EnumVal);
                return _SA_APPLICATION2COMPANY;

        }
        public SA_APPLICATION2COMPANY ShipToX()
        {
            if (IsHaveShipToX())
            {
                int EnumVal = (int)companyType.ShipTo;
                SA_APPLICATION2COMPANY _SA_APPLICATION2COMPANY = this.SA_APPLICATION2COMPANY.FirstOrDefault(p => p.CompanyType == EnumVal);
                return  _SA_APPLICATION2COMPANY;
            }
            return null;
        }
        public SA_APPLICATION2COMPANY BillToX()
        {
            if (IsHaveBillToX())
            {
                int EnumVal = (int)companyType.BillTo;
                SA_APPLICATION2COMPANY _SA_APPLICATION2COMPANY = this.SA_APPLICATION2COMPANY.FirstOrDefault(p => p.CompanyType == EnumVal);
                return _SA_APPLICATION2COMPANY;
            }
            return null;
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
            ((MyAdminEntity)CurrentContext).SA_APPLICATION.Add(this);
            ((MyAdminEntity)CurrentContext).SaveChanges();
            //if (CurrentContext.Entry(this).State == EntityState.Detached)
            //{ CurrentContext.Set<SA_APPLICATION>().Attach(this); }
            //CurrentContext.Entry(this).State = EntityState.Added;
            //try
            //{
            //    CurrentContext.SaveChanges();
            //}
            //catch (DbEntityValidationException e)
            //{
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        sb.AppendFormat("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
            //    throw;
            //}
           
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
