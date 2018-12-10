using Advantech.Myadvantech.DataAccess;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Advantech.Myadvantech.Business
{
    public class CPDBBusinessLogic
    {
        public static List<SO_HEADER> GetSoHeaderWithWSResult(List<String> orders)
        {
            return new CPDBDAL().GetSoHeader(orders);
        }

        public static String GetSoPo_BySo(String so)
        {
            List<SO_HEADER> sh = new CPDBDAL().GetDistinctSoHeader(so);
            String result = (from d in sh
                             where d.SO == so
                             select d.CUST_PO_NO).FirstOrDefault();
            return !String.IsNullOrEmpty(result) ? result : "";
        }

        public static String GetSoPartnerFunc_Number(String so, String role_type)
        {
            List<SO_PARTNERFUNC> sp = new CPDBDAL().GetSoPartnerFunc(so);
            String result = "";
            // get ERPID from SO_Partnumber_function table depend on it's role
            if (role_type.Equals("AG"))
            {
                result = (from number in sp
                          where number.ROLE == "AG"
                          select number.NUMBER).FirstOrDefault();
            }
            else if (role_type.Equals("WE"))
            {
                result = (from number in sp
                          where number.ROLE == "WE"
                          select number.NUMBER).FirstOrDefault();
            }
            return !String.IsNullOrEmpty(result) ? result : "";
        }

        public static String GetSoDetail_Currency(String so)
        {
            List<SO_DETAIL> sd = new CPDBDAL().GetSoDetail(so);
            String result = (from d in sd
                             where d.SO == so
                             select d.CURRENCY).FirstOrDefault();
            return !String.IsNullOrEmpty(result) ? result : "USD";
        }

        public static void Detail2Cart(String so, String cart_id, String erp_id, String user_id, String currency, String org_id)
        {
            SqlProvider.dbExecuteNoQuery("MY", String.Format("delete from ORDER_MASTER where ORDER_ID = '{0}'", so));
            SqlProvider.dbExecuteNoQuery("MY", String.Format("delete from ORDER_DETAIL where ORDER_ID = '{0}'", so));
            SqlProvider.dbExecuteNoQuery("MY", String.Format("delete from ORDER_PARTNERS where ORDER_ID = '{0}'", so));
            SqlProvider.dbExecuteNoQuery("MY", String.Format("delete from order_Master_ExtensionV2 where ORDER_ID = '{0}'", so));

            // SO_Header to CartMaster
            CARTMASTERV2 CartMaster = new CARTMASTERV2();
            CartMaster.CartID = cart_id;
            CartMaster.ErpID = erp_id;
            CartMaster.CreatedDate = DateTime.Now;
            CartMaster.Currency = currency;
            CartMaster.CreatedBy = user_id;
            CartMaster.LastUpdatedDate = DateTime.Now;
            CartMaster.LastUpdatedBy = user_id;
            MyAdvantechContext.Current.CARTMASTERV2.Add(CartMaster);

            // SO_Detail to CartDetail
            List<SO_DETAIL> sd = new CPDBDAL().GetSoDetail(so); // get a list from SO_Detail table 

            // Simulate Product List to get the correct list price
            Order _order = new Order();
            GetPriceForCheckPoint(ref _order, so, currency, org_id);

            if (_order != null)
            {
                List<Product> pr = _order.LineItems;
                foreach (SO_DETAIL d in sd)
                {
                    cart_DETAIL_V2 CartDetail = new cart_DETAIL_V2();

                    CartDetail.Cart_Id = cart_id;
                    CartDetail.Line_No = int.Parse(d.LINE_NO);
                    CartDetail.Part_No = Advantech.Myadvantech.DataAccess.SAPDAL.RemovePrecedingZeros(d.MATERIAL);
                    CartDetail.Qty = int.Parse(d.QTY);
                    CartDetail.CustMaterial = d.CUST_MATERIAL;
                    CartDetail.Ew_Flag = 0;
                    CartDetail.SatisfyFlag = 0;
                    CartDetail.QUOTE_ID = "";

                    // higherlevel value validate, if is null then set it as 0 to avoid further error
                    if (d.HIGHER_LEVEL == null)
                        CartDetail.higherLevel = 0;
                    else
                        CartDetail.higherLevel = int.Parse(d.HIGHER_LEVEL);

                    // item type validate, set it's type, req_date, due_date
                    CartDetail.otype = 0;
                    CartDetail.req_date = DateTime.Now.AddDays(2);
                    if (CartDetail.Line_No < 100)
                    {
                        CartDetail.otype = (int)QuoteItemType.Part;
                    }
                    else if (CartDetail.Line_No % 100 == 0 && CartDetail.higherLevel == 0)
                    {
                        CartDetail.otype = (int)QuoteItemType.BtosParent;
                        CartDetail.req_date = getCompNextWorkDate(CartDetail.req_date.Value, org_id, getBTOWorkingDate(org_id));
                    }
                    else if (CartDetail.Line_No % 100 > 0 && CartDetail.Line_No > 100)
                    {
                        CartDetail.otype = (int)QuoteItemType.BtosPart;
                        CartDetail.req_date = getCompNextWorkDate(CartDetail.req_date.Value, org_id, getBTOWorkingDate(org_id));
                    }
                    CartDetail.due_date = CartDetail.req_date;

                    //Ryan 20160421 Revise Delivery Plant Logic, *Must behind partno & otype processing.
                    CartDetail.Delivery_Plant = PartBusinessLogic.GetDeliveryPlant(erp_id, org_id, CartDetail.Part_No, (QuoteItemType)Enum.Parse(typeof(QuoteItemType), CartDetail.otype.ToString()));

                    // set price according to it's item type
                    if (CartDetail.otype == (int)QuoteItemType.BtosParent)
                    {
                        CartDetail.List_Price = 0;
                        CartDetail.Unit_Price = 0;
                        CartDetail.Itp = 0;
                    }
                    else
                    {
                        // Get the list price only if CartDetail's Part_NO is equal to _Order's Part_NO
                        Decimal list_price = 0, unit_price = 0;
                        if ((from p in pr where p.PartNumber == CartDetail.Part_No select p.ListPrice).Any())
                            list_price = (from p in pr where p.PartNumber == CartDetail.Part_No select p.ListPrice).FirstOrDefault();
                        if ((from p in pr where p.PartNumber == CartDetail.Part_No select p.UnitPrice).Any())
                            unit_price = (from p in pr where p.PartNumber == CartDetail.Part_No select p.UnitPrice).FirstOrDefault();

                        CartDetail.List_Price = list_price;
                        CartDetail.Unit_Price = unit_price;
                        CartDetail.Itp = 0;
                    }
                    MyAdvantechContext.Current.cart_DETAIL_V2.Add(CartDetail);
                }
                MyAdvantechContext.Current.SaveChanges();
            }
        }

        public static void GetPriceForCheckPoint(ref Order _order, String so, String currency, String org_id)
        {
            List<SO_HEADER> sh = new CPDBDAL().GetDistinctSoHeader(so);
            List<SO_DETAIL> sd = new CPDBDAL().GetSoDetail(so);

            _order.Currency = currency;
            _order.OrgID = org_id;
            _order.DistChannel = (from d in sh select d.DIST_CHAN).FirstOrDefault().Trim();
            _order.Division = (from d in sh select d.DIVISION).FirstOrDefault().Trim();

            int i = 0;
            foreach (SO_DETAIL d in sd)
            {
                // Only check for list price, hence the qty is set to 1
                _order.AddLooseItem(Advantech.Myadvantech.DataAccess.SAPDAL.RemovePrecedingZeros(d.MATERIAL), 1);
                _order.LineItems[i].PlantID = "USH1";
                i++;
            }

            _order.SetOrderPartnet(new OrderPartner(GetSoPartnerFunc_Number(so, "AG"), org_id, OrderPartnerType.SoldTo));
            _order.SetOrderPartnet(new OrderPartner(GetSoPartnerFunc_Number(so, "WE"), org_id, OrderPartnerType.ShipTo));
            _order.SetOrderPartnet(new OrderPartner(GetSoPartnerFunc_Number(so, "AG"), org_id, OrderPartnerType.BillTo));

            String _errMsg = String.Empty;
            Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref _order, ref _errMsg);

            if (!String.IsNullOrEmpty(_errMsg))
                throw new Exception(_errMsg);
        }

        public static void SaveAllInfo(String so, String cart_id, String ERP_id, String user_id, String cust_pono)
        {
            CheckPointOrder2Cart CPO = new CheckPointOrder2Cart();
            CPO.ERPID = ERP_id;
            CPO.CheckPointOrderNo = so;
            CPO.CartID = cart_id;
            CPO.CreatedTime = DateTime.Now;
            CPO.Creator = user_id;
            CPO.OrderStatus = "On Cart";
            CPO.PO_NO = cust_pono;

            MyAdvantechContext.Current.CheckPointOrder2Cart.Add(CPO);
            MyAdvantechContext.Current.SaveChanges();
        }

        public static DateTime getCompNextWorkDate(DateTime req_date, String org_id, int days)
        {
            String LandStr = SAPDAL.SAPDAL.GetCalendarIDbyOrg(org_id.Substring(0, 1));
            SAPDAL.SAPDAL.Get_Next_WorkingDate_ByCode(ref req_date, days.ToString(), LandStr);
            return req_date;
        }

        public static int getBTOWorkingDate(String org_id)
        {
            if (org_id.Equals("US01"))
                return int.Parse(WebConfigurationManager.AppSettings["USBTOSWorkingDay"]);
            else if (org_id.Equals("TW01"))
                return int.Parse(WebConfigurationManager.AppSettings["TWBTOSWorkingDay"]);
            else
                return int.Parse(WebConfigurationManager.AppSettings["BTOSWorkingDay"]);
        }

        public static String CheckPointOrder2Cart_getOrderNo(String cart_id)
        {
            List<CheckPointOrder2Cart> cpoc = new CPDBDAL().CheckPointOrder2Cart();
            String result = (from d in cpoc
                             where d.CartID == cart_id
                             select d.CheckPointOrderNo).FirstOrDefault();
            return result;
        }

        public static Boolean CheckPointOrder2Cart_CartIDExist(String cart_id)
        {
            List<CheckPointOrder2Cart> cpoc = new CPDBDAL().CheckPointOrder2Cart();
            Boolean result = (from d in cpoc
                              where d.CartID == cart_id
                              select d.CheckPointOrderNo).Any();
            return result;
        }

        public static void EditCheckPointOrder2Cart_Status(String cart_id)
        {
            List<CheckPointOrder2Cart> cpoc = new CPDBDAL().CheckPointOrder2Cart();
            CheckPointOrder2Cart cart = (from d in cpoc
                                         where d.CartID == cart_id
                                         select d).FirstOrDefault();
            if (cart != null)
            {
                cart.OrderStatus = "Finished";
                cart.Remark = "Turned to finished at " + DateTime.Now;
                MyAdvantechContext.Current.SaveChanges();
            }
        }

        public static List<String> GetSOFromMYAWithWS()
        {
            Advantech.Myadvantech.DataAccess.CPTEST.general cp = new Advantech.Myadvantech.DataAccess.CPTEST.general();
            List<String> so = new List<string>();
            if (cp.GetSOPendingForMYA() != null)
            {
                so = cp.GetSOPendingForMYA().Select(c => c.ToString()).ToList();

            }
            return so;
        }

        public static String GetCustPONoByCartID(String cart_id)
        {
            List<CheckPointOrder2Cart> cpoc = new CPDBDAL().CheckPointOrder2Cart();
            String result = (from d in cpoc
                             where d.CartID == cart_id
                             select d.PO_NO).FirstOrDefault();
            return !String.IsNullOrEmpty(result) ? result : "";
        }
        public static String GetCustDNNoByCartID(String cart_id)
        {
            String result = String.Empty;

            List<CheckPointOrder2Cart> cpoc = new CPDBDAL().CheckPointOrder2Cart();
            String CPNO = (from d in cpoc
                             where d.CartID == cart_id
                             select d.CheckPointOrderNo).FirstOrDefault();
            if (CPNO != null && !String.IsNullOrEmpty(CPNO))
            {
                var objCPDNNo = SqlProvider.dbExecuteScalar("CheckPointDB", String.Format("SELECT top 1 b.DN FROM [CheckPointDB].[dbo].[SO_HEADER] a inner join [CheckPointDB].[dbo].[Shipment_Information] b on a.CUST_PO_NO = b.SO_Number where a.SO = '{0}'", CPNO));
                if (objCPDNNo != null && !String.IsNullOrEmpty(objCPDNNo.ToString()))
                    result = objCPDNNo.ToString();
            }
            return result;
        }

        public static Boolean IsCheckPointOrder(String cart_id, String user_id)
        {
            if (user_id.Equals("javian.tsai@advantech.com.tw", StringComparison.CurrentCultureIgnoreCase) || user_id.Equals("yl.huang@advantech.com.tw", StringComparison.CurrentCultureIgnoreCase))
            {
                if (String.IsNullOrEmpty(GetCustPONoByCartID(cart_id)))
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        #region CreateNewShipto

        public static String ProcessCPShipto(String _so, String _sopo, Boolean _istesting)
        {
            System.Net.Mail.SmtpClient smtpClient1 = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);

            Ship_to_Information ship_info = GetShipToInfo_BySOPO(_sopo).FirstOrDefault();
            DataAccess.SAPModel.SAPAccount s = new DataAccess.SAPModel.SAPAccount();

            s.SoldtoID = "UZISCHE01";
            s.ShiptoID = OrderBusinessLogic.GenerateNewUSShipToID(ship_info.ship_to_country_code, ship_info.ship_to_stateName, ship_info.ship_to_name, _istesting);
            s.CompanyName = ship_info.ship_to_name;
            s.CountryCode = ship_info.ship_to_country_code;
            s.OrgID = "US01";
            s.City = ship_info.ship_to_city;
            s.Address = ship_info.ship_to_address;
            s.Region = ship_info.ship_to_stateName;
            s.PostalCode = ship_info.ship_to_postal_code;
            s.TEL = ship_info.ship_to_telnumber;
            s.VatNumber = ship_info.ship_to_country_code.Equals("US", StringComparison.CurrentCultureIgnoreCase) ? GetVATNumber(ship_info.ship_to_stateName) : " ";
            s.Currency = GetSoDetail_Currency(_so);
            s.TaxJurisdiction = ship_info.ship_to_country_code.Equals("US", StringComparison.CurrentCultureIgnoreCase) ? ship_info.ship_to_stateName + ship_info.ship_to_postal_code.Substring(0, 5) : " ";
            s.Creator = "VALERIE.MO";
            s.CreateDate = DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            s.SalesGroupCode = " ";
            s.SalesOfficeCode = "2200";
            s.ContactPerson = ship_info.contact_person_first + ship_info.contact_person_last;

            DataAccess.SAPModel.SAPShipTo _objShipTo = CheckShiptoExistSAP(s.Address, s.ShiptoID);
            if (_objShipTo != null)
            {
                // Ship-to ID has already existed in SAP, return the exist one
                return _objShipTo.SHIPTOID;
            }
            else
            {
                // Ship-to ID is not existed, need to create a new one.
                try
                {
                    DataAccess.SAPDAL.CreateUSShiptoAccount(s, _istesting); // Auto Creation
                    System.Threading.Thread.Sleep(3000);
                    DataAccess.SAPDAL.UpdateSAPSalesAreaData(s.SoldtoID, s.ShiptoID, s.OrgID, _istesting);

                    // Send Mail to Javian
                    smtpClient1.Send("myadvantech@advantech.com", "Frank.Chung@advantech.com.tw,IC.Chen@advantech.com.tw,YL.Huang@advantech.com.tw,Javian.Tsai@advantech.com.tw",
                    string.Format("Check Point Auto-Create Shipto,  NewERPID: {0}", s.ShiptoID),
                    string.Format("NewERPID: {0}", s.ShiptoID) + "\r\nTime: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                catch (Exception ex)
                {
                    String errmsg = ex.ToString();
                }
                return !String.IsNullOrEmpty(s.ShiptoID) ? s.ShiptoID : "";
            }
        }

        public static List<Ship_to_Information> GetShipToInfo_BySOPO(String _sopo)
        {
            return new CPDBDAL().GetShiptoInfo(_sopo);
        }

        public static void UpdatePartnerFuncTable(String _so, String _newshiptoid)
        {
            SO_PARTNERFUNC sp1 = new SO_PARTNERFUNC();
            SO_PARTNERFUNC sp2 = new SO_PARTNERFUNC();

            sp1.SO = _so;
            sp1.ROLE = "AG";
            sp1.NUMBER = "UZISCHE01";

            sp2.SO = _so;
            sp2.ROLE = "WE";
            sp2.NUMBER = _newshiptoid;

            CPDBContext.Current.SO_PARTNERFUNC.Add(sp1);
            CPDBContext.Current.SO_PARTNERFUNC.Add(sp2);
            CPDBContext.Current.SaveChanges();
        }

        public static String GetVATNumber(String Key)
        {
            Dictionary<String, String> Dic = new Dictionary<String, String>();
            Dic.Add("AL", "6800 17277");
            Dic.Add("AR", "66364618-SLS");
            Dic.Add("AZ", "07 610849-M");
            Dic.Add("CA", "SR BH-097-480382");
            Dic.Add("CO", "07-44221-0000");
            Dic.Add("CT", "9732736-000");
            Dic.Add("DC", "350000035380");
            Dic.Add("FL", "AF1218350401");
            Dic.Add("GA", "175-442847");
            Dic.Add("HI", " ");
            Dic.Add("ID", "004781708-08");
            Dic.Add("IL", "2755-0931");
            Dic.Add("IA", "2943229135 001");
            Dic.Add("KS", "005-943229135-F01");
            Dic.Add("KY", "00351556");
            Dic.Add("ME", "1180798");
            Dic.Add("MD", "08792860");
            Dic.Add("MI", "ME-0233051");
            Dic.Add("MN", "4663234");
            Dic.Add("LA", "1922263-001-400");
            Dic.Add("MA", "94-3229135");
            Dic.Add("MO", "17020930");
            Dic.Add("NE", "8394989");
            Dic.Add("NV", "1067352981-001");
            Dic.Add("NJ", "943-229-135/000");
            Dic.Add("NM", " ");
            Dic.Add("NC", "600104079");
            Dic.Add("ND", "32385100");
            Dic.Add("OH", "99 045998");
            Dic.Add("OK", " ");
            Dic.Add("PA", "81-497 707");
            Dic.Add("RI", "94322913500");
            Dic.Add("SC", "09951350-1");
            Dic.Add("SD", " ");
            Dic.Add("TN", "103110929");
            Dic.Add("TX", "19432291359");
            Dic.Add("UT", "13936434");
            Dic.Add("VT", " ");
            Dic.Add("VA", "12-943229135F-001");
            Dic.Add("WA", "601-867-746");
            Dic.Add("WT", "005 0000593432 01");

            if (Dic.ContainsKey(Key))
            {
                return Dic[Key];
            }
            else
            {
                return " ";
            }
        }

        public static List<DataAccess.SAPModel.SAPShipTo> GetSAPShipToAddr(String ERPID)
        {
            String str = "select a.KUNNR,b.MC_STREET from saprdp.kna1 a inner join saprdp.adrc b on a.adrnr=b.addrnumber " +
                         " where a.kunnr like '" + ERPID + "%' order by a.kunnr desc ";

            DataTable dt = OracleProvider.GetDataTable("SAP_PRD", str);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<DataAccess.SAPModel.SAPShipTo> list = new List<DataAccess.SAPModel.SAPShipTo>();
                foreach (DataRow r in dt.Rows)
                {
                    DataAccess.SAPModel.SAPShipTo P = new DataAccess.SAPModel.SAPShipTo();
                    P.SHIPTOID = r["KUNNR"].ToString();
                    P.ADDR = r["MC_STREET"].ToString();
                    list.Add(P);
                }

                if (list.Count > 0)
                {
                    return list;
                }
            }
            return null;
        }

        public static DataAccess.SAPModel.SAPShipTo CheckShiptoExistSAP(string ShipAddr, string ERPID)
        {
            List<DataAccess.SAPModel.SAPShipTo> list = GetSAPShipToAddr(ERPID.Substring(0, 6));
            if (list != null && list.Count > 0)
            {
                String ShipAddr1 = RemoveSpecialChar(ShipAddr).ToUpper();
                foreach (DataAccess.SAPModel.SAPShipTo s in list)
                {
                    String SAPShipAddr = RemoveSpecialChar(s.ADDR).ToUpper();
                    if (ShipAddr1.Contains(SAPShipAddr))
                    {
                        return s;
                    }
                }
            }
            return null;
        }

        private static string RemoveSpecialChar(string str)
        {
            char[] C =  { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                          'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                          'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            ArrayList newstr = new ArrayList();
            if ((str.Trim().Length > 0))
            {
                char[] S = str.ToCharArray();
                foreach (char x in S)
                {
                    foreach (char t in C)
                    {
                        if (String.Equals(x.ToString(), t.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            newstr.Add(x);
                        }
                    }
                }
            }
            return String.Join("", newstr.ToArray());
        }
        #endregion

    }
}
