using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class ProjectRegistration
    {
        public string Account_Row_ID { get; set; }

        public string Close_Date { get; set; }

        public string Currency { get; set; }

        public string Project_Name { get; set; }

        public string Revenue { get; set; }

        public string Contact_Row_ID { get; set; }

        public string RBU { get; set; }

        public string Owner_Email { get; set; }

        public string Description { get; set; }

        public string Competition { get; set; }

        #region Update page data
        public string RowID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string CompetitorName { get; set; }
        public string ModelNo { get; set; }
        public string SellingPrice { get; set; }
        public string Remark { get; set; }
        #endregion

        private List<ProjectRegistrationProduct> products = new List<ProjectRegistrationProduct>();
        public List<ProjectRegistrationProduct> Products 
        {
            get
            {
                return this.products;
            }
            set
            {
                this.products = value;
            }
        }
    }

    public class ProjectRegistrationProduct
    {
        public string Main_Product { get; set; }

        public string Main_Product_Qty { get; set; }
    }
}
