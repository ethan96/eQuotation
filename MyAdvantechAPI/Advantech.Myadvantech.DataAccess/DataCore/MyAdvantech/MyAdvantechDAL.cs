using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace Advantech.Myadvantech.DataAccess
{
    public class MyAdvantechDAL
    {
        /// <summary>
        /// ICC 2015/9/22 Get minimun order qty datatable. Use order ID and org ID paramete.
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="plant"></param>
        /// <param name="org_id"></param>
        /// <returns>DataTable</returns>
        public static DataTable getBelowMOQLineItem(string order_id, string org_id)
        {
            //ICC 2015/9/21 Modify minimun order qty sql. Change SAP_PRODUCT_ABC to SAP_PRODUCT_ORG, and use org id to get min_ord_qty
            StringBuilder sql = new StringBuilder();
            //sql.AppendLine(" select b.PART_NO,b.TotalQty,c.MIN_LOT_SIZE from  ");
            sql.AppendLine(" select b.PART_NO, b.TotalQty, c.MIN_ORDER_QTY from  ");
            sql.AppendLine("    (  ");
            sql.AppendLine(" 		select a.PART_NO,SUM(a.QTY) as TotalQty  ");
            sql.AppendLine(" 		From ORDER_DETAIL a ");
            sql.AppendLine(" 		where  a.ORDER_ID='" + order_id + "' ");
            sql.AppendLine(" 		group by a.PART_NO ");
            //sql.AppendLine(" 	) b inner join SAP_PRODUCT_ABC c on b.PART_NO=c.PART_NO	");
            //sql.AppendLine(" where c.PLANT='" + plant + "' ");
            //sql.AppendLine(" and c.MIN_LOT_SIZE>0 ");
            //sql.AppendLine(" and b.TotalQty<c.MIN_LOT_SIZE ");
            sql.AppendLine("      ) b inner join SAP_PRODUCT_STATUS c on b.PART_NO = c.PART_NO");
            sql.AppendFormat("  where c.SALES_ORG = '{0}' ", org_id);
            sql.AppendLine("    and c.MIN_ORDER_QTY > 0 ");
            sql.AppendLine("    and b.TotalQty < c.MIN_ORDER_QTY ");

            return SqlProvider.dbGetDataTable("MY", sql.ToString());

        }

        public List<PRODUCT_DEPENDENCY> getProductDependencyByPartNo(string PartNo)
        {
            var result = MyAdvantechContext.Current.PRODUCT_DEPENDENCY.Where(x => x.PART_NO == PartNo).ToList();
            return result;
        }
        public CARTMASTERV2 getCartMasterV2ByCartID(string cartID)
        {
            var result = MyAdvantechContext.Current.CARTMASTERV2.Where(x => x.CartID == cartID).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Update CARTMASTERV2 optyID if Opprotunity ID is in Null, empty and new ID
        /// </summary>
        /// <param name="quoteID"></param>
        /// <param name="optyID"></param>
        public static void UpdateOptyID(string quoteID, string optyID)
        {
            //SQL server defualt不管大小寫, 所以new ID不需要特別處理 
            MyAdvantechContext.Current.Database.ExecuteSqlCommand(string.Format(" UPDATE CARTMASTERV2 SET OpportunityID = '{0}'  WHERE ISNULL(OpportunityID, '') in ('new ID', '')  AND QuoteID = '{1}' ", optyID, quoteID));
        }

        public static SAP_PRODUCT GetSAPProduct(string _PartNo)
        {
            return MyAdvantechContext.Current.SAP_PRODUCT.Where(d => d.PART_NO.Equals(_PartNo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public static SAP_PRODUCT_ABC GetSAPProductABC(string _PartNo, string _Plant = "")
        {
            return MyAdvantechContext.Current.SAP_PRODUCT_ABC.Where(d => d.PART_NO.Equals(_PartNo, StringComparison.OrdinalIgnoreCase) && (String.IsNullOrEmpty(_Plant) ? true : d.PLANT.Equals(_Plant, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
        }

        public static SAP_PRODUCT GetSAP_ProductByOrg(string PartNo, string ORGID)
        {
            var result = from sp in MyAdvantechContext.Current.SAP_PRODUCT
                         join spo in MyAdvantechContext.Current.SAP_PRODUCT_ORG
                             on sp.PART_NO equals spo.PART_NO
                         where sp.PART_NO == PartNo && spo.ORG_ID == ORGID
                         select sp;
            return result.FirstOrDefault();

            //return null;

        }

        /// <summary>
        /// Get Siebel Sales Position by email
        /// </summary>
        /// <param name="emial"></param>
        /// <returns>POSITION</returns>
        public static object GetSiebelSalesPositionByEmail(string emial)
        {
            //return SqlProvider.dbExecuteScalar("MY", string.Format(" select top 1 POSITION from SIEBEL_SALES_HIERARCHY where EMAIL = '{0}' ", emial));
            //ICC 2015/11/11 SIEBEL_SALES_HIERARCHY data is not accurate, so change to SIEBEL_POSITION for instead.
            return SqlProvider.dbExecuteScalar("MY", string.Format(" select top 1 PRIMARY_POSITION_NAME from SIEBEL_POSITION where EMAIL_ADDR = '{0}' order by CREATED desc", emial));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sales_code"></param>
        /// <returns></returns>
        public static DataTable GetSaleseManager(string sales_code)
        {
            return SqlProvider.dbGetDataTable("MY", string.Format(" select id_sap as sales_code,id_rbu,id_email,id_sector from EAI_IDMAP where id_sap = '{0}' and id_fact_zone='North America' and id_sector like '%AOnline%' ", sales_code));
        }


        public static ORDER_MASTER GetOrderMaster(String order_id)
        {
            var result = MyAdvantechContext.Current.ORDER_MASTER.Where(x => x.ORDER_ID == order_id).FirstOrDefault();
            return result;
        }


        public static List<ORDER_DETAIL> GetOrderDetail(String order_id)
        {
            return MyAdvantechContext.Current.ORDER_DETAIL.Where(d => d.ORDER_ID == order_id).OrderBy(d => d.LINE_NO).ToList();
        }

        public List<SAP_PRODUCT_ABC> GetSAPProductABC_CDTPPart()
        {
            return MyAdvantechContext.Current.SAP_PRODUCT_ABC.Where(d => d.PLANT == "USH1" && (d.ABC_INDICATOR.StartsWith("C") || d.ABC_INDICATOR.StartsWith("D") || d.ABC_INDICATOR.StartsWith("T") || d.ABC_INDICATOR.StartsWith("P"))).ToList();
        }

        public List<SAP_PRODUCT> GetSAPProduct_XYPart()
        {
            return MyAdvantechContext.Current.SAP_PRODUCT.Where(d => d.PRODUCT_TYPE == "ZPER" && (d.PART_NO.StartsWith("X") || d.PART_NO.StartsWith("Y"))).ToList();
        }

        public List<cart_DETAIL_V2> GetCartDetailV2ByCartID(String cart_id)
        {
            return MyAdvantechContext.Current.cart_DETAIL_V2.Where(d => d.Cart_Id == cart_id).ToList();
        }

        public List<SAP_EMPLOYEE> GetSAPEmployeeBySalesEmail(String sales_email)
        {
            return MyAdvantechContext.Current.SAP_EMPLOYEE.Where(d => d.EMAIL == sales_email).ToList();
        }

        public List<SAP_EMPLOYEE> GetSAPEmployeeBySalesCode(String sales_code)
        {
            return MyAdvantechContext.Current.SAP_EMPLOYEE.Where(d => d.SALES_CODE == sales_code).ToList();
        }

        public static List<SIEBEL_ACCOUNT> GetSiebelAccountByERPID(String _ERPID)
        {
            return MyAdvantechContext.Current.SIEBEL_ACCOUNT.Where(d => d.ERP_ID.Equals(_ERPID)).ToList();
        }

        public static List<SIEBEL_ACCOUNT> GetSiebelAccountByRowID(String _RowID)
        {
            return MyAdvantechContext.Current.SIEBEL_ACCOUNT.Where(d => d.ROW_ID.Equals(_RowID)).ToList();
        }

        public static List<SIEBEL_ACCOUNT> GetSiebelAccountChannelPartnerByERPID(String erpid)
        {
            return MyAdvantechContext.Current.SIEBEL_ACCOUNT.Where(d => d.ERP_ID == erpid &&
                (d.ACCOUNT_STATUS == "01-Premier Channel Partner" ||
                 d.ACCOUNT_STATUS == "02-Gold Channel Partner" ||
                 d.ACCOUNT_STATUS == "03-Certified Channel Partner")).ToList();
        }

        /// <summary>
        /// ICC 2016/4/1 GetSiebelContactByERPID
        /// </summary>
        /// <param name="erpid"></param>
        /// <returns></returns>
        public static List<SIEBEL_CONTACT> GetSiebelContactByERPID(string erpid)
        {
            if (string.IsNullOrEmpty(erpid))
            {
                return new List<SIEBEL_CONTACT>();
            }
            try
            {
                return MyAdvantechContext.Current.SIEBEL_CONTACT.Where(s => s.ERPID.ToUpper() == erpid.ToUpper()).ToList();
            }
            catch
            {
                return new List<SIEBEL_CONTACT>();
            }

        }

        /// <summary>
        /// Alex 2017/04/20 Get Siebel Contact By AccountRowId
        /// </summary>
        /// <param name="accountRowId"></param>
        /// <returns></returns>
        public static List<SIEBEL_CONTACT> GetSiebelContactByAccountRowId(string accountRowId)
        {
            if (string.IsNullOrEmpty(accountRowId))
            {
                return new List<SIEBEL_CONTACT>();
            }
            try
            {
                return MyAdvantechContext.Current.SIEBEL_CONTACT.Where(s => s.ACCOUNT_ROW_ID.ToUpper() == accountRowId.ToUpper()).ToList();
            }
            catch
            {
                return new List<SIEBEL_CONTACT>();
            }

        }

        public static DataTable GetBBWebPrice(String PartNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT PartNo,isnull(L1Price,0) as L1Price ");
            sql.AppendLine(" FROM BBESTORE_WEB_PRICE ");
            sql.AppendLine(" WHERE PartNo='" + PartNo + "' ");

            return SqlProvider.dbGetDataTable("MY", sql.ToString());

        }

        public static List<SAP_DIMCOMPANY> GetSAPDIMCompanyByERPID(String _erpid)
        {
            return MyAdvantechContext.Current.SAP_DIMCOMPANY.Where(s => s.COMPANY_ID == _erpid).ToList();
        }

        public static List<SAP_COMPANY_PARTNERS> GetSAPPARTNERSByERPID(String _erpid)
        {
            return MyAdvantechContext.Current.SAP_COMPANY_PARTNERS.Where(s => s.COMPANY_ID == _erpid).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static DataTable GetEmployeeDetail(String email)
        {
            //DataTable _dt = new DataTable();


            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT A.[EMAIL_ADDR],A.[FST_NAME],A.[LST_NAME],A.[LOCAL_NAME] ");
            sql.AppendLine(" ,A.[EMPLR_ID],A.[EXTENSION],isnull(B.SALES_CODE,'') as Sales_Code ");
            sql.AppendLine(" FROM  EZ_EMPLOYEE A LEFT JOIN SAP_EMPLOYEE B ");
            sql.AppendLine(" ON A.EMAIL_ADDR=B.EMAIL ");
            sql.AppendLine(" WHERE A.EMAIL_ADDR='" + email + "' ");

            return SqlProvider.dbGetDataTable("MY", sql.ToString());


            //return _dt;

        }

        public static List<PRODUCT_COMPATIBILITY> GetProductCompatibility(Compatibility compatibility)
        {
            return MyAdvantechContext.Current.PRODUCT_COMPATIBILITY
                .Where(p => p.RELATION == (int)compatibility)
                .ToList();
        }

        public static List<FreightOption> GetAllFreightOptions()
        {
            return MyAdvantechContext.Current.FreightOptions.ToList();
        }

        public static OrderForwarderService GetOrderForwarderServiceByOrderId(string orderId)
        {
            return MyAdvantechContext.Current.OrderForwarderServices.SingleOrDefault(s => s.OrderId == orderId);
        }


        public static void RemoveOrderForwarderServiceByID(String orderId)
        {
            OrderForwarderService item = GetOrderForwarderServiceByOrderId(orderId);
            MyAdvantechContext.Current.OrderForwarderServices.Remove(item);
            MyAdvantechContext.Current.SaveChanges();
        }

        public static void AddOrUpdateOrderForwarderService(OrderForwarderService orderForwarderService)
        {
            OrderForwarderService existedorderForwarderService = GetOrderForwarderServiceByOrderId(orderForwarderService.OrderId);
            if (existedorderForwarderService != null)
            {
                existedorderForwarderService.CustomChargeBy = orderForwarderService.CustomChargeBy;
                existedorderForwarderService.FreightChargeBy = orderForwarderService.FreightChargeBy;
                existedorderForwarderService.FreightOption = orderForwarderService.FreightOption;
            }
            else
            {
                MyAdvantechContext.Current.OrderForwarderServices.Add(orderForwarderService);
            }
            MyAdvantechContext.Current.SaveChanges();
        }

        public static List<BB_ESTORE_ORDER> GetBBorderWithStatus(List<string> orderStatus)
        {
            return MyAdvantechContext.Current.BB_ESTORE_ORDER.Where(p => orderStatus.Contains(p.ORDER_STATUS)).ToList();
        }

        public static Tuple<bool, string> CreateBBorderRecord(BB_ESTORE_ORDER order)
        {
            try
            {
                MyAdvantechContext.Current.BB_ESTORE_ORDER.Add(order);
                MyAdvantechContext.Current.SaveChanges();
                return new Tuple<bool, string>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.ToString());
            }
        }

        public static BB_ESTORE_ORDER GetBBorderRecord(string orderNo)
        {
            try
            {
                return MyAdvantechContext.Current.BB_ESTORE_ORDER.Where(p => p.ORDER_NO == orderNo).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public static Tuple<bool, string> CreateBBCreditCardOrder(BB_CREDITCARD_ORDER order)
        {
            try
            {
                MyAdvantechContext.Current.BB_CREDITCARD_ORDER.Add(order);
                MyAdvantechContext.Current.SaveChanges();
                return new Tuple<bool, string>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.ToString());
            }
        }


        public static List<BB_CREDITCARD_ORDER> GetAllBBCreditCardOrder()
        {
            try
            {
                return MyAdvantechContext.Current.BB_CREDITCARD_ORDER.ToList();
            }
            catch
            {
                return null;
            }
        }

        public static List<BB_CREDITCARD_ORDER> GetBBCreditCardOrdersByTranType(List<string> tranTypes)
        {
            try
            {
                return MyAdvantechContext.Current.BB_CREDITCARD_ORDER.Where(o => tranTypes.Contains(o.TRANSACTION_TYPE)).ToList();
            }
            catch
            {
                return null;
            }
        }

        public static BB_CREDITCARD_ORDER GetBBCreditCardOrderByOrderNo(string orderNo)
        {
            try
            {
                return MyAdvantechContext.Current.BB_CREDITCARD_ORDER.Where(o => o.ORDER_NO == orderNo).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public static List<HubConfiguredResult> GetHubConfiguredResults(string sourceId)
        {
            try
            {
                return MyAdvantechContext.Current.HubConfiguredResults.Where(d => d.ID.Equals(sourceId, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static HubConfiguredResult GetHubConfiguredResultsWithLineNo(string sourceId, int lineNo)
        {
            try
            {
                return MyAdvantechContext.Current.HubConfiguredResults.Where(d => d.ID.Equals(sourceId, StringComparison.OrdinalIgnoreCase) && d.ParentLineNo == lineNo).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }


        public static void AddConfigurationHubConfiguredRecords(HubConfiguredResult objInput)
        {
            try
            {
                if (MyAdvantechContext.Current.HubConfiguredResults.Any(e => e.ID.Equals(objInput.ID, StringComparison.OrdinalIgnoreCase) && e.ParentLineNo == objInput.ParentLineNo))
                {
                    MyAdvantechContext.Current.HubConfiguredResults.Attach(objInput);
                    MyAdvantechContext.Current.Entry(objInput).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    MyAdvantechContext.Current.HubConfiguredResults.Add(objInput);
                }
                MyAdvantechContext.Current.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public static List<SAP_COMPANY_ORG> GetSAPCompanyOrgList(string companyId)
        {
            return MyAdvantechContext.Current.SAP_COMPANY_ORG.Where(d => d.COMPANY_ID.Equals(companyId)).ToList();
        }

        public static void AddSAPCompanyOrg(SAP_COMPANY_ORG input)
        {
            MyAdvantechContext.Current.SAP_COMPANY_ORG.Add(input);
            MyAdvantechContext.Current.SaveChanges();
        }

        public static Dictionary<string, string> GetPaymentTermByOrg(string orgId)
        {
            var groups = MyAdvantechContext.Current.SAP_DIMCOMPANY.Where(s => s.ORG_ID == orgId && !string.IsNullOrEmpty(s.CREDIT_TERM)).GroupBy(c => new { c.PAYMENT_TERM_CODE, c.PAYMENT_TERM_NAME });
            var result = new Dictionary<string, string>();
            foreach (var group in groups)
            {
                var name = string.IsNullOrEmpty(group.Key.PAYMENT_TERM_NAME.Trim()) ? group.Key.PAYMENT_TERM_CODE : group.Key.PAYMENT_TERM_NAME;
                var value = group.Key.PAYMENT_TERM_CODE;
                result.Add(name, value);
            }
            return result;
        }

        public static List<ExtendedWarrantyPartNo_V2> GetExtendedWarrantyByPlant(string plant)
        {
            var orgSubTwo = plant.Substring(0, 2);
            return MyAdvantechContext.Current.ExtendedWarrantyPartNo_V2.Where(w => w.Plant.StartsWith(orgSubTwo)).OrderBy(w => w.SeqNO).ToList();
        }

        public static List<ExtendedWarrantyPartNo_V2> GetExtendedWarrantyByOrg(string org)
        {
            //var plant = SAPDAL.GetPlantByOrg(org);
            var orgSubTwo = org.Substring(0, 2);
            return MyAdvantechContext.Current.ExtendedWarrantyPartNo_V2.Where(w => w.Plant.StartsWith(orgSubTwo)).OrderBy(w => w.SeqNO).ToList();
        }

        public static ExtendedWarrantyPartNo_V2 GetExtendedWarrantyById(int warrantyId)
        {
            return MyAdvantechContext.Current.ExtendedWarrantyPartNo_V2.Where(w => w.ID == warrantyId).FirstOrDefault();
        }

        public static ExtendedWarrantyPartNo_V2 GetExtendedWarrantyPartNoAndPlant(string warrantyPartNo, string plant)
        {
            var orgSubTwo = plant.Substring(0, 2);
            return MyAdvantechContext.Current.ExtendedWarrantyPartNo_V2.Where(w => w.EW_PartNO == warrantyPartNo && w.Plant.StartsWith(orgSubTwo)).FirstOrDefault();
        }

        public static void AddASGBtosInstruction(string cartId, int lineNo, string instructionText)
        {
            ASG_BTOSInstruction newItem = new ASG_BTOSInstruction();
            newItem.ID = cartId;
            newItem.LineNo = lineNo;
            newItem.Text = instructionText;

            if (MyAdvantechContext.Current.ASG_BTOSInstruction.Any(e => e.ID.Equals(cartId, StringComparison.OrdinalIgnoreCase) && e.LineNo == lineNo))
            {
                MyAdvantechContext.Current.ASG_BTOSInstruction.Attach(newItem);
                MyAdvantechContext.Current.Entry(newItem).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                MyAdvantechContext.Current.ASG_BTOSInstruction.Add(newItem);
            }
            MyAdvantechContext.Current.SaveChanges();
        }
    }
}
