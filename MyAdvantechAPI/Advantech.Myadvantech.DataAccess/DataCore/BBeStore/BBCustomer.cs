using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class BBCustomer
    {
        public string CustomerID { get; set; }
        public string OrgID { get; set; }
        public string UserID { get; set; }
        public string NetTerm { get; set; }
        public string IncotermText { get; set; }

        public BBCustomer()
        { }

        public BBCustomer(string customerID, string orgID, string userID)
        {
            this.CustomerID = customerID;
            this.OrgID = orgID;
            this.UserID = userID;
        }

        public BBCustomer(string userID)
        {
            this.UserID = userID;
        }
    }
}
