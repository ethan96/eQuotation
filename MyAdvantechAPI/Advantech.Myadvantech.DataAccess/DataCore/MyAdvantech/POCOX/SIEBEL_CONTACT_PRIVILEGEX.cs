using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class SIEBEL_CONTACT_PRIVILEGE
    {
        #region Constructor
        public SIEBEL_CONTACT_PRIVILEGE()
        {
        
        }
        public SIEBEL_CONTACT_PRIVILEGE(string Row_ID, string Email_Address, string Privilege)
        {
            this.ROW_ID = ROW_ID;
            this.EMAIL_ADDRESS = Email_Address;
            this.PRIVILEGE = Privilege;
        }
        
        #endregion
        
        #region DB functions - only can be used in Business layer

        //private DbContext CurrentContext
        //{
        //    get
        //    {
        //        return MyAdvantechContext.Current;
        //    }
        //}

        //internal void Update()
        //{
        //    CurrentContext.Entry(this).State = EntityState.Modified;
        //    CurrentContext.SaveChanges();
        //}

        //internal void Add()
        //{
        //    ((MyAdvantechGlobalEntities)CurrentContext).SIEBEL_CONTACT_PRIVILEGE.Add(this);
        //    CurrentContext.Entry(this).State = EntityState.Added;
        //    CurrentContext.SaveChanges();
        //}

        //internal void Delete()
        //{
        //    ((MyAdvantechGlobalEntities)CurrentContext).SIEBEL_CONTACT_PRIVILEGE.Remove(this);
        //    CurrentContext.Entry(this).State = EntityState.Deleted;
        //    CurrentContext.SaveChanges();
        //}
        #endregion
    }
}
