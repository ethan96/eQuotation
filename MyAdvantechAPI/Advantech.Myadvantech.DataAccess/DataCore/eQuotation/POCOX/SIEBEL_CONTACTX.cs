using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class SIEBEL_CONTACT
    {
        private string[] ips;
        private string[] baa;
        private bool isNeverEmail;
        private List<SIEBEL_CONTACT_PRIVILEGE> privilege;
        public string[] InterestedProduct 
        {
            get
            {
                if (ips != null)
                    return this.ips;
                else
                    return new string[] { };
            }
            set
            {
                ips = value;
            }
        }

        public string[] BAA
        {
            get
            {
                if (baa != null)
                    return this.baa;
                else
                    return new string[] { };
            }
            set
            {
                baa = value;
            }
        }

        public bool IsNeverEmail
        {
            get
            {
                if (!string.IsNullOrEmpty(this.NeverEmail))
                {
                    return string.Equals(this.NeverEmail, SiebelContactNeverEmail.Y.ToString()) ? true : false;
                }
                return false;
            }
        }

        public List<SIEBEL_CONTACT_PRIVILEGE> PRIVILEGE
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ROW_ID))
                    return MyAdvantechContext.Current.SIEBEL_CONTACT_PRIVILEGE.Where(p => p.ROW_ID == this.ROW_ID).ToList();
                else
                    return null;
            }
        }
    }
}
