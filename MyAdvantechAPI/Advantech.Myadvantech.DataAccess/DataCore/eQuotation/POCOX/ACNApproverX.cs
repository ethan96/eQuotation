using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public partial class ACN_EQ_Sales
    {
        public string ApproverName {
            get
            {
                var name = this.ApproverEmail.Split('@')[0];
                if (name != null)
                    return name;
                return "";
            }
        }

        public bool IsExecutives {
            get
            {
                var executives = eQuotationContext.Current.ACN_Executive.Where(e=>e.Sector == this.Sector).ToList();
                if (executives != null)
                {
                    if (executives.Select(r => r.Level).Contains(this.Level))
                        return true;
                }
                return false;
            }
        }
    }

    public partial class ACN_EQ_PSM
    {
        public string PSMName
        {
            get
            {
                var name = this.PSM.Split('@')[0];
                if (name != null)
                    return name;
                return "";
            }
        }

    }
}
