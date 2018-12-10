using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.Entities
{
    public partial class CartItem
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

        public string UnitPriceX
        {
            get
            {
                return this.UnitPrice.ToString("C2", cultureInfor);
            }
        }

        public string AdjustedPriceX
        {
            get
            {
                return this.AdjustedPrice.ToString("C2", cultureInfor);
            }
        }
    }
}
