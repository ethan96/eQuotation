using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class BBeStoreDAL
    {
        public static List<Advantech.Myadvantech.DataAccess.Entities.Order> GetBBeStoreOrderByEmail(string email)
        {
            return BBeStoreContext.Current.Order.Where(o => o.UserID.Equals(email, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public static Advantech.Myadvantech.DataAccess.Entities.Order GetBBeStoreOrderByOrderNo(string orderNo)
        {
            try
            {
                var status = new List<string>() { "Confirmed", "Closed_Converted", "ConfirmdButNeedTaxIDReview", "ConfirmdButNeedFreightReview" };
                return BBeStoreContext.Current.Order
                    .Where(o => o.OrderNo == orderNo && status.Contains(o.OrderStatus)).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public static Advantech.Myadvantech.DataAccess.Entities.Payment GetBBeStorePaymentByOrderNo(string paymentID)
        {
            return BBeStoreContext.Current.Payment.Where(o => o.PaymentID == paymentID).FirstOrDefault();
        }
    }
}
