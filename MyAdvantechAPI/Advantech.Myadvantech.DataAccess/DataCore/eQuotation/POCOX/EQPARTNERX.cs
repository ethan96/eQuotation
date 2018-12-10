using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class EQPARTNERX
    {
        public EQPARTNERX()
        {

        }

        public EQPARTNERX(System.Data.DataRow dr)
        {
            this.QuoteID = dr[0] == DBNull.Value ? string.Empty : dr[0].ToString();
            this.RowID = dr[1] == DBNull.Value ? string.Empty : dr[1].ToString();
            this.ERPID = dr[2] == DBNull.Value ? string.Empty : dr[2].ToString();
            this.Name = dr[3] == DBNull.Value ? string.Empty : dr[3].ToString();
            this.Address = dr[4] == DBNull.Value ? string.Empty : dr[4].ToString();
            this.Type = dr[5] == DBNull.Value ? string.Empty : dr[5].ToString();
            this.Attention = dr[6] == DBNull.Value ? string.Empty : dr[6].ToString();
            this.Tel = dr[7] == DBNull.Value ? string.Empty : dr[7].ToString();
            this.Mobile = dr[8] == DBNull.Value ? string.Empty : dr[8].ToString();
            this.ZipCode = dr[9] == DBNull.Value ? string.Empty : dr[9].ToString();
            this.Country = dr[10] == DBNull.Value ? string.Empty : dr[10].ToString();
            this.City = dr[11] == DBNull.Value ? string.Empty : dr[11].ToString();
            this.Street = dr[12] == DBNull.Value ? string.Empty : dr[12].ToString();
            this.State = dr[13] == DBNull.Value ? string.Empty : dr[13].ToString();
            this.District = dr[14] == DBNull.Value ? string.Empty : dr[14].ToString();
            this.Street2 = dr[15] == DBNull.Value ? string.Empty : dr[15].ToString();
            this.Fax = dr[16] == DBNull.Value ? string.Empty : dr[16].ToString();
        }
        public string QuoteID { get; set; }
        
        public string RowID { get; set; }
        
        public string ERPID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Type { get; set; }

        public string Attention { get; set; }

        public string Tel { get; set; }
        
        public string Mobile { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }
        
        public string Street2 { get; set; }

        public string State { get; set; }

        public string District { get; set; }

        public string Fax { get; set; }
    }
}
