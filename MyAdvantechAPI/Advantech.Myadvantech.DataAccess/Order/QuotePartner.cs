using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    [Serializable]
    public class QuotePartner
    {
        public QuotePartner() { }
        
        public QuotePartner(System.Data.DataRow row, string firstName, string lastName)
        {
            this.QuoteID = row[0] == DBNull.Value ? string.Empty : row[0].ToString();
            this.RowID = row[1] == DBNull.Value ? string.Empty : row[1].ToString();
            this.ERPID = row[2] == DBNull.Value ? string.Empty : row[2].ToString();
            this.Name = row[3] == DBNull.Value ? string.Empty : row[3].ToString();
            this.Address = row[4] == DBNull.Value ? string.Empty : row[4].ToString();
            this.Type = row[5] == DBNull.Value ? string.Empty : row[5].ToString();
            this.Attention = row[6] == DBNull.Value ? string.Empty : row[6].ToString();
            this.Tel = row[7] == DBNull.Value ? string.Empty : row[7].ToString();
            this.Mobile = row[8] == DBNull.Value ? string.Empty : row[8].ToString();
            this.ZipCode = row[9] == DBNull.Value ? string.Empty : row[9].ToString();
            this.Country = row[10] == DBNull.Value ? string.Empty : row[10].ToString();
            this.City = row[11] == DBNull.Value ? string.Empty : row[11].ToString();
            this.Street = row[12] == DBNull.Value ? string.Empty : row[12].ToString();
            this.State = row[13] == DBNull.Value ? string.Empty : row[13].ToString();
            this.District = row[14] == DBNull.Value ? string.Empty : row[14].ToString();
            this.Street2 = row[15] == DBNull.Value ? string.Empty : row[15].ToString();
            this.Fax = row[16] == DBNull.Value ? string.Empty : row[16].ToString();
            this.FirstName = firstName;
            this.LasteName = lastName;
        }
        
        public string QuoteID { get; set; }

        public string RowID { get; set; }

        public string ERPID { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LasteName { get; set; }

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
