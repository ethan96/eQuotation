using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("Advantech.Myadvantech.Business")]

namespace Advantech.Myadvantech.DataAccess
{
    public class CPDBDAL
    {
        public List<SO_HEADER> GetSoHeader()
        {
            return CPDBContext.Current.SO_HEADER.ToList();
        }

        public List<SO_HEADER> GetDistinctSoHeader(String so) 
        {
            return CPDBContext.Current.SO_HEADER.Where(d => d.SO == so).ToList();
        }

        public List<SO_HEADER> GetSoHeader(List<String> orders)
        {
            return CPDBContext.Current.SO_HEADER.Where(d => orders.Contains(d.SO) == true).ToList();
        }

        public List<SO_DETAIL> GetSoDetail(String so)
        {
            return CPDBContext.Current.SO_DETAIL.Where(d => d.SO == so).ToList();
        }

        public List<SO_PARTNERFUNC> GetSoPartnerFunc(String so)
        {
            return CPDBContext.Current.SO_PARTNERFUNC.Where(d => d.SO == so).ToList();
        }

        public List<CARTMASTERV2> GetCartMaster()
        {
            return MyAdvantechContext.Current.CARTMASTERV2.ToList();
        }

        public List<cart_DETAIL_V2> GetCartDetail()
        {
            return MyAdvantechContext.Current.cart_DETAIL_V2.ToList();
        }

        public List<CheckPointOrder2Cart> CheckPointOrder2Cart()
        {
            return MyAdvantechContext.Current.CheckPointOrder2Cart.ToList();
        }

        public List<Ship_to_Information> GetShiptoInfo(String so)
        {
            return CPDBContext.Current.Ship_to_Information.Where(d => d.SO_Number == so).ToList();
        }
    }
}
