using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class WorkFlowApproval : IRepository<WorkFlowApproval>
    {
        private DbContext _CurrentContext;
        public DbContext CurrentContext
        {
            get
            {
                if (_CurrentContext == null)
                {
                    _CurrentContext = eQuotationContext.Current;
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
            ((eQEntities)CurrentContext).WorkFlowApproval.Add(this);
            ((eQEntities)CurrentContext).SaveChanges();
        }

        public void Remove()
        {
            ((eQEntities)CurrentContext).Entry(this).State = System.Data.Entity.EntityState.Deleted;
            ((eQEntities)CurrentContext).SaveChanges();
        }

        public void Update()
        {
            ((eQEntities)CurrentContext).Entry(this).State = System.Data.Entity.EntityState.Modified;
            ((eQEntities)CurrentContext).SaveChanges();
        }




    }
}
