using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public class CartDetailHelper
    {
        public static List<cart_DETAIL_V2> GetCartDetailByID(String _CartID)
        {
            return MyAdvantechContext.Current.cart_DETAIL_V2.Where(d => d.Cart_Id.Equals(_CartID)).ToList();
        }

        public static List<CARTMASTERV2> GetCartMasterByID(String _CartID)
        {
            return MyAdvantechContext.Current.CARTMASTERV2.Where(d => d.CartID.Equals(_CartID)).ToList();
        }

        public static void RemoveCartDetailByID(String _CartID)
        {
            List<cart_DETAIL_V2> items = GetCartDetailByID(_CartID);
            MyAdvantechContext.Current.cart_DETAIL_V2.RemoveRange(items);
            MyAdvantechContext.Current.SaveChanges();
        }

        public static void RemoveCartMasterByID(String _CartID)
        {
            List<CARTMASTERV2> items = GetCartMasterByID(_CartID);
            MyAdvantechContext.Current.CARTMASTERV2.RemoveRange(items);
            MyAdvantechContext.Current.SaveChanges();
        }

        public static void CartDetail2QuoteDetail(String _CartID, String _QuoteID)
        {
            List<cart_DETAIL_V2> CartDetails = MyAdvantechContext.Current.cart_DETAIL_V2.Where(d => d.Cart_Id.Equals(_CartID)).ToList();

            foreach (cart_DETAIL_V2 c in CartDetails)
            {
                QuotationDetail q = new QuotationDetail();
                q.quoteId = _QuoteID;
                q.line_No = c.Line_No;
                q.partNo = c.Part_No;
                q.description = c.Description;
                q.qty = c.Qty;
                q.listPrice = c.List_Price;
                q.unitPrice = c.oUnit_Price == null ? 0 : c.oUnit_Price;
                q.newUnitPrice = c.Unit_Price;
                q.itp = c.Itp;
                q.newItp = c.Itp;
                q.RecyclingFee = c.RecyclingFee == null ? 0 : c.RecyclingFee;
                q.deliveryPlant = c.Delivery_Plant;
                q.category = c.Category;
                q.classABC = c.@class;
                q.rohs = c.rohs;
                q.ewFlag = c.Ew_Flag;
                q.reqDate = c.req_date;
                q.dueDate = c.due_date;
                q.satisfyFlag = c.SatisfyFlag;
                q.canBeConfirmed = c.CanbeConfirmed;
                q.custMaterial = c.CustMaterial;
                q.inventory = c.inventory;
                //q.oType = c.otype;
                q.modelNo = c.Model_No;
                q.HigherLevel = c.higherLevel;
                q.ItemType = (c.Line_No >= 100 && (c.Line_No % 100 == 0)) ? 1 : 0; // weird... eQuotation only has 0/1 two kind of values.
                q.SequenceNo = 0; // not sure what is this field for.
                eQuotationContext.Current.QuotationDetail.Add(q);
            }
            eQuotationContext.Current.SaveChanges();
        }

        public static Order CartDetail2Order(List<cart_DETAIL_V2> _CartItems, String _ERPID, String _ORGID, String _Currency)
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
            foreach (cart_DETAIL_V2 c in _CartItems)
            {
                Product _part = new Product();
                _part.PartNumber = c.Part_No;
                _part.LineNumber = (int)c.Line_No;
                _part.Quantity = (int)c.Qty;
                _part.PlantID = c.Delivery_Plant;
                _part.UnitPrice = (decimal)c.Unit_Price;
                _part.ListPrice = (decimal)c.List_Price;

                switch ((int)c.otype)
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
                        _part.ParentLineNumber = (int)c.higherLevel;
                        break;
                }
                order.LineItems.Add(_part);
            }

            return order;
        }

    }
}
