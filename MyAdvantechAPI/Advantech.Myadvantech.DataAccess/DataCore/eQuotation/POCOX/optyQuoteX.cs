using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class optyQuote
    {
        private DbContext CurrentContext
        {
            get
            {
                return eQuotationContext.Current;
            }
        }

        internal void Update()
        {
            //CurrentContext.Entry(this).State = EntityState.Modified;
          //  CurrentContext.SaveChanges();
            string sql = string.Format("UPDATE [optyQuote] SET [optyId] = '{0}',[optyName] = N'{1}',[optyStage] = N'{2}',[Opty_Owner_Email] = '{3}' ,[processId] = '{4}' WHERE [quoteId]='{5}'", 
                this.optyId,this.optyName,this.optyStage,this.Opty_Owner_Email,"",this.quoteId );
            CurrentContext.Database.ExecuteSqlCommand(sql);
        }

        internal void Add()
        {
            ((eQEntities)CurrentContext).optyQuote.Add(this);
           // CurrentContext.Entry(this).State = EntityState.Added;
            CurrentContext.SaveChanges();
        }
    }
}
