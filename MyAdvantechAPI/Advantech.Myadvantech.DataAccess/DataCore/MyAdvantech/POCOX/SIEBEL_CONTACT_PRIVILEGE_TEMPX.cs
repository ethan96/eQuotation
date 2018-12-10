using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class SIEBEL_CONTACT_PRIVILEGE_TEMP
    {
        #region Constructor
        public SIEBEL_CONTACT_PRIVILEGE_TEMP()
        {

        }

        public SIEBEL_CONTACT_PRIVILEGE_TEMP(string Row_ID, string Email_Address, string Privilege, string Action_Type, string CreatedBy, DateTime CreatedDate)
        {
            this.ROW_ID = Row_ID;
            this.EMAIL_ADDRESS = Email_Address;
            this.PRIVILEGE = Privilege;
            this.ACTION_TYPE = Action_Type;
            this.CREATED_BY = CreatedBy;
            this.CREATED_DATE = CreatedDate;
        }

        public SIEBEL_CONTACT_PRIVILEGE_TEMP(SIEBEL_CONTACT_PRIVILEGE Privilege, string Action_Type, string CreatedBy)
        {
            this.ROW_ID = Privilege.ROW_ID;
            this.EMAIL_ADDRESS = Privilege.EMAIL_ADDRESS;
            this.PRIVILEGE = Privilege.PRIVILEGE;
            this.ACTION_TYPE = Action_Type;
            this.CREATED_BY = CreatedBy;
            this.CREATED_DATE = DateTime.Now;
        }
        #endregion
        
        #region DB functions - only can be used in Business layer

        private DbContext CurrentContext
        {
            get
            {
                return MyAdvantechContext.Current;
            }
        }

        //internal void Update()
        //{
        //    CurrentContext.Entry(this).State = EntityState.Modified;
        //    CurrentContext.SaveChanges();
        //}

        //internal void Add()
        //{
        //    ((MyAdvantechGlobalEntities)CurrentContext).SIEBEL_CONTACT_PRIVILEGE_TEMP.Add(this);
        //    CurrentContext.Entry(this).State = EntityState.Added;
        //    CurrentContext.SaveChanges();
        //}

        //internal void Delete()
        //{
        //    ((MyAdvantechGlobalEntities)CurrentContext).SIEBEL_CONTACT_PRIVILEGE_TEMP.Remove(this);
        //    CurrentContext.Entry(this).State = EntityState.Deleted;
        //    CurrentContext.SaveChanges();
        //}
        #endregion
    }
}
