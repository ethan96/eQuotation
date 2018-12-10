using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class QuotationDetailHelper : _eQuotationHelper
    {
        /// <summary>
        /// Get all QuotationDetail items by quoteID
        /// </summary>
        /// <param name="quoteID">quoteID</param>
        /// <returns>List<QuotationDetail></returns>
        public List<QuotationDetail> GetQuotationDetail(string quoteID)
        {
            return eQuotationContext.Current.QuotationDetail.Where(x => x.quoteId == quoteID).ToList();
        }

        /// <summary>
        /// Get all QuotationDetail_Extension_ABR items by quoteID
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        public List<QuotationDetail_Extension_ABR> GetQuotationDetail_Extension_ABR(string quoteID)
        {
            return eQuotationContext.Current.QuotationDetail_Extension_ABR.Where(x => x.quoteid == quoteID).ToList();
        }

        /// <summary>
        /// Get one QuotationDetail_Extension_ABR item by quoteID and line number
        /// </summary>
        /// <param name="quoteID"></param>
        /// <param name="line_no"></param>
        /// <returns></returns>
        public QuotationDetail_Extension_ABR GetQuotationDetail_Extension_ABRItem(string quoteID, int line_no)
        {
            return context.QuotationDetail_Extension_ABR.Where(x => x.quoteid == quoteID && x.line_No == line_no).FirstOrDefault();
        }



        /// <summary>
        /// Get one QuotationDetail item by quoteID and line number
        /// </summary>
        /// <param name="quoteID"></param>
        /// <param name="line_no"></param>
        /// <returns></returns>
        public QuotationDetail GetQuotationDetailItem(string quoteID,int line_no)
        {
            return context.QuotationDetail.Where(x => x.quoteId == quoteID && x.line_No == line_no).FirstOrDefault();
        }


        /// <summary>
        /// Update QuotationDetail
        /// </summary>
        /// <param name="detail">QuotationDetail</param>
        /// <returns>bool</returns>
        public bool UpdateQuotationDetail(QuotationDetail detail)
        {
            try
            {
                context.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static Order QuoteDetail2Order(List<QuotationDetail> _QuoteDetails, String _ERPID, String _ORGID, String _Currency)
        {
            Order order = new Order();
            order.OrderType = SAPOrderType.ZOR;

            // Set order partner
            OrderPartner partner = new OrderPartner();
            partner.Type = OrderPartnerType.SoldTo;
            partner.ErpID = _ERPID;

            // Set Order basic settings
            order.OrgID = _ORGID;
            order.SetOrderPartnet(partner);
            order.Currency = _Currency;
            order.DistChannel = "10";
            order.Division = "00";

            // Convert items from cart detail table to order
            foreach (QuotationDetail q in _QuoteDetails)
            {
                Product _part = new Product();
                _part.PartNumber = q.partNo;
                _part.LineNumber = (int)q.line_No;
                _part.Quantity = (int)q.qty;
                _part.PlantID = q.deliveryPlant;
                _part.UnitPrice = (decimal)q.newUnitPrice;
                _part.ListPrice = (decimal)q.listPrice;

                switch ((int)q.ItemType)
                {
                    // case -1 means BTOS parent
                    case -1:
                        _part.LineItemType = LineItemType.BTOSParent;
                        _part.ParentLineNumber = 0;
                        break;

                    // case 0 means Loose item
                    case 0:
                        _part.LineItemType = LineItemType.LooseItem;
                        _part.ParentLineNumber = 0;
                        break;

                    // case 1 means BTOS child
                    case 1:
                        _part.LineItemType = LineItemType.BTOSChild;
                        _part.ParentLineNumber = (int)q.HigherLevel;
                        break;
                }
                order.LineItems.Add(_part);
            }

            return order;
        }
    }
}
