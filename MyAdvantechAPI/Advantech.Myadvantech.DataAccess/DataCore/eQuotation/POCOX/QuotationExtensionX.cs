using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
 public partial    class QuotationExtension:IRepository<QuotationExtension>
    {
     public eQApprovalFlowType ApprovalFlowTypeX
     {
         get
         {

             if (this.ApprovalFlowType != null && Enum.IsDefined(typeof(eQApprovalFlowType), this.ApprovalFlowType))
             {
                 return (eQApprovalFlowType)Enum.ToObject(typeof(eQApprovalFlowType), this.ApprovalFlowType);
             }
             return eQApprovalFlowType.Normal;
         }
     }


     public int delete(params object[] keyvalues)
     {
         string SQL = string.Format("delete from  Quotation_Approval_Expiration where quoteID in ('{0}')", string.Join("','", keyvalues));
         return eQuotationContext.Current.Database.ExecuteSqlCommand(SQL);
     }
     public DbContext CurrentContext
     {
         get
         {
             return eQuotationContext.Current;
         }
         set
         {
             throw new NotImplementedException();
         }
     }

     public void Add()
     {
         throw new NotImplementedException();
     }

     public void Update()
     {
         if (CurrentContext.Entry(this).State == EntityState.Detached)
         { CurrentContext.Set<QuotationExtension>().Attach(this); }
        // CurrentContext.Entry(this).State = EntityState.Modified;
         CurrentContext.SaveChanges();
     }

     public void Remove()
     {
         throw new NotImplementedException();
     }

    }
}
