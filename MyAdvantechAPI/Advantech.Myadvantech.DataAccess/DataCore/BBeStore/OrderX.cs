using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.Entities
{
    public partial class Order
    {
        private CultureInfo _culture;
        public CultureInfo cultureInfor
        {
            get
            {
                if (this._culture == null)
                {
                    this._culture = new CultureInfo("en-US");
                    this._culture.NumberFormat.CurrencySymbol = "$";
                }
                return this._culture;
            }
        }

        public string FreightX
        {
            get
            {
                if (this.Freight.HasValue == true)
                    return this.Freight.Value.ToString("C2", cultureInfor);
                else
                    return string.Empty;
            }
        }

        public string TaxX
        {
            get
            {
                if (this.Tax.HasValue == true)
                    return this.Tax.Value.ToString("C2", cultureInfor);
                else
                    return string.Empty;
            }
        }

        public string TaxRateX
        {
            get
            {
                if (this.TaxRate.HasValue == true)
                    return this.TaxRate.Value.ToString() + "%";
                else
                    return "N/A";
            }
        }

        public string TotalAmountX
        {
            get
            {
                if (this.TotalAmount.HasValue == true)
                    return this.TotalAmount.Value.ToString("C2", cultureInfor);
                else
                    return string.Empty;
            }
        }

        public bool Emergency { get; set; }
    }
}
