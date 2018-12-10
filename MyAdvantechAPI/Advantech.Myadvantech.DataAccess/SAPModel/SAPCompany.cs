using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class SAPCompany
    {
        /// <summary>
        /// 
        /// </summary>
        public string Company_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ORG_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Company_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SAPCompany()
        {
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public SAPCompany(DataRow dr)
        {
            try
            {
                this.Company_ID = dr["KUNNR"].ToString();
                this.ORG_ID = dr["VKORG"].ToString();
            }
            catch
            {
                this.Company_ID = string.Empty;
                this.ORG_ID = string.Empty;
            }
        }
    }

    public class CLAcompany
    {
        public List<SAPCompany> ls106A { get; set; }
        public List<SAPCompany>ls106C { get; set; }

        public CLAcompany()
        { }

        public CLAcompany(List<SAPCompany> a, List<SAPCompany> c)
        {
            this.ls106A = a;
            this.ls106C = c;
        }
    }
}
