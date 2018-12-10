using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class QuoteApproval : IRepository<QuoteApproval>
    
    {
        public QuoteApprovalStatus StatusX
        {
            get
            {
                if (Enum.IsDefined(typeof(QuoteApprovalStatus), this.Status))
                {
                    return (QuoteApprovalStatus)Enum.ToObject(typeof(QuoteApprovalStatus), this.Status);
                }
                return QuoteApprovalStatus.Wait_for_review;
            }
        }
        private DbContext _CurrentContext; 
        public DbContext CurrentContext
        {
            get
            {
                if (_CurrentContext == null)
                {
                    _CurrentContext= eQuotationContext.Current;
                }
                return _CurrentContext;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add()
        {
           ((eQEntities)CurrentContext).QuoteApproval.Add(this);
           ((eQEntities)CurrentContext).SaveChanges();
        }

        public void Update()
        {
            ((eQEntities)CurrentContext).Entry(this).State = System.Data.Entity.EntityState.Modified;
            ((eQEntities)CurrentContext).SaveChanges();
        }

        public void Remove()
        {
            ((eQEntities)CurrentContext).Entry(this).State = System.Data.Entity.EntityState.Deleted;
            ((eQEntities)CurrentContext).SaveChanges();
        }


    }
}
