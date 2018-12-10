using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    [Serializable]
    public class SiebelAccountSearch
    {
        public SiebelAccountSearch()
        {
            this.accountname = string.Empty;
            this.erpID = string.Empty;
            this.primarysales = string.Empty;
            this.accountstatus = string.Empty;
            this.country = string.Empty;
            this.city = string.Empty;
            this.zipcode = string.Empty;
            this.state = string.Empty;
            this.region = string.Empty;
            this.accountRowId = string.Empty;
        }

        private string accountname;
        [Newtonsoft.Json.JsonProperty("name")]
        public string AccountName
        {
            get
            {
                return this.accountname;
            }
            set
            {
                this.accountname = value;
            }
        }

        private string erpID;
        [Newtonsoft.Json.JsonProperty("ID")]
        public string ERPID
        {
            get
            {
                return this.erpID;
            }
            set
            {
                this.erpID = value;
            }
        }

        private string primarysales;
        [Newtonsoft.Json.JsonProperty("sales")]
        public string PrimarySales
        {
            get
            {
                return this.primarysales;
            }
            set
            {
                this.primarysales = value;
            }
        }

        private string accountstatus;
        [Newtonsoft.Json.JsonProperty("status")]
        public string AccountStatus
        {
            get
            {
                return this.accountstatus;
            }
            set
            {
                this.accountstatus = value;
            }
        }

        private string country;
        [Newtonsoft.Json.JsonProperty("country")]
        public string Country
        {
            get
            {
                return this.country;
            }
            set
            {
                this.country = value;
            }
        }

        private string city;
        [Newtonsoft.Json.JsonProperty("city")]
        public string City
        {
            get
            {
                return this.city;
            }
            set
            {
                this.city = value;
            }
        }

        private string zipcode;
        [Newtonsoft.Json.JsonProperty("zip")]
        public string ZipCode
        {
            get
            {
                return this.zipcode;
            }
            set
            {
                this.zipcode = value;
            }
        }

        private string state;
        [Newtonsoft.Json.JsonProperty("state")]
        public string State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

        private string region;
        [Newtonsoft.Json.JsonProperty("region")]
        public string Region
        {
            get
            {
                return this.region;
            }
            set
            {
                this.region = value;
            }
        }

        private string accountRowId;
        [Newtonsoft.Json.JsonProperty("AccountRowID")]
        public string AccountRowId
        {
            get
            {
                return this.accountRowId;
            }
            set
            {
                this.accountRowId = value;
            }
        }

    }
}
