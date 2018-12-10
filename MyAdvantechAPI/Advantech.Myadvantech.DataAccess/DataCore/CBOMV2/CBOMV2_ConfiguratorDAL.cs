using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.DataCore
{
    public static class CBOMV2_ConfiguratorDAL
    {
        public static List<EasyUITreeNode> GetConfigRecord(String rootid, String sales_org, String cbom_org, int nodetype)
        {
            List<EasyUITreeNode> TreeNodes = new List<EasyUITreeNode>();
            try
            {
                CategoryTypes type = (CategoryTypes)Enum.ToObject(typeof(CategoryTypes), nodetype);
                if (type == CategoryTypes.SharedComponent)
                {
                    rootid = SqlProvider.dbExecuteScalar("CBOMV2", string.Format("select top 1 ISNULL(SHARED_CATEGORY_ID,'') AS [SID] from CBOM_CATALOG_CATEGORY_V2 where ID= '{0}' ", rootid.Trim())).ToString();
                    if (string.IsNullOrEmpty(rootid))
                        return TreeNodes;
                }

                List<CBOM_CATEGORY_RECORD> CBOMCategoryRecords = GetCBOMCategoryRecordByRootId(rootid, cbom_org.Substring(0, 2));
                List<CBOM_CATEGORY_RECORD> RootRecord = (from q in CBOMCategoryRecords where q.ID == rootid select q).ToList();

                if (RootRecord.Count == 1)
                {
                    CheckSharedCategory(new List<String>(), ref CBOMCategoryRecords);
                    EasyUITreeNode RootTreeNode = new EasyUITreeNode(RootRecord.First().ID, RootRecord.First().ID, RootRecord.First().CATEGORY_ID, "", RootRecord.First().HIE_ID, "", 0, 0, 1, 0, 0, 0, 0);
                    RootTreeNode.csstype = NodeCssType.Tree_Node_Root;

                    //Ryan 20171120 Check components orderable or not for ACN.
                    CBOMCategoryRecords.RemoveAll(item => (item.CATEGORY_TYPE == CategoryTypes.Component || item.CATEGORY_TYPE == CategoryTypes.SharedComponent) && !CheckItemOrdereable(item.CATEGORY_ID, sales_org));
                    //if (cbomorg.ToUpper().StartsWith("CN"))
                    //    CBOMCategoryRecords.RemoveAll(item => (item.CATEGORY_TYPE == CategoryTypes.Component || item.CATEGORY_TYPE == CategoryTypes.SharedComponent) && !CheckItemOrdereable(item.CATEGORY_ID, org));
                    //else if (cbomorg.ToUpper().StartsWith("DL"))
                    //    CBOMCategoryRecords.RemoveAll(item => (item.CATEGORY_TYPE == CategoryTypes.Component || item.CATEGORY_TYPE == CategoryTypes.SharedComponent) && !CheckItemOrdereable(item.CATEGORY_ID, "US01"));

                    CBOMCategoryRecordsToEasyUITreeNode(CBOMCategoryRecords, RootTreeNode);

                    // Only have to add Special category such as EW and STD assembly if node is BTOS-Parent
                    //if (RootTreeNode.type == (int)CategoryTypes.Root && RootTreeNode.text.Contains("-BTO"))
                    //    RuntimeAddSpecialCategory(RootTreeNode, orgid);

                    TreeNodes.Add(RootTreeNode);
                }
            }
            catch
            {
                //save error
            }
            return TreeNodes;
        }

        public static List<CBOM_CATEGORY_RECORD> GetCBOMCategoryRecordByRootId(string RootId, string OrgId)
        {
            String str = " DECLARE @Child hierarchyid " +
                         " SELECT @Child = HIE_ID FROM CBOM_CATALOG_CATEGORY_V2 " +
                         " WHERE ID = '" + RootId + "' AND ORG = '" + OrgId + "' " +
                         " SELECT IsNull(cast(HIE_ID.GetAncestor(1) as nvarchar(100)),'') as PAR_HIE_ID, " +
                         " HIE_ID.GetLevel() AS [LEVEL], ID AS [ID], ID AS [VIRTUAL_ID], " +
                         " HIE_ID.ToString() AS [HIE_ID],CATEGORY_ID, CATEGORY_TYPE, " +
                         " CATEGORY_NOTE, SEQ_NO, CONFIGURATION_RULE as CONFIGURATION_RULE, ORG, " +
                         " DEFAULT_FLAG as isDefault, REQUIRED_FLAG as isRequired, EXPAND_FLAG as isExpand, " +
                         " SHARED_CATEGORY_ID AS [SHARED_CATEGORY_GUID], MAX_QTY AS [QTY] " +
                         " FROM CBOM_CATALOG_CATEGORY_V2 " +
                         " WHERE HIE_ID.GetAncestor(0) = @Child or HIE_ID.GetAncestor(1) = @Child or HIE_ID.GetAncestor(2) = @Child " +
                         " AND ORG = '" + OrgId + "' ORDER BY HIE_ID.GetLevel() ";

            DataTable dtCategoryTree = SqlProvider.dbGetDataTable("CBOMV2", str);
            List<CBOM_CATEGORY_RECORD> CBOMCategoryRecords = dtCategoryTree.DataTableToList<CBOM_CATEGORY_RECORD>();

            return CBOMCategoryRecords;
        }

        public static void CBOMCategoryRecordsToEasyUITreeNode(List<CBOM_CATEGORY_RECORD> CBOMCategoryRecords, EasyUITreeNode CurrentNode)
        {
            String CurrentNodeHieId = CurrentNode.hieid;
            List<CBOM_CATEGORY_RECORD> SubRecord = (from q in CBOMCategoryRecords where q.PAR_HIE_ID == CurrentNodeHieId orderby q.SEQ_NO, q.CATEGORY_ID select q).ToList();

            if (SubRecord.Count == 0)
                return;

            foreach (CBOM_CATEGORY_RECORD SubRecord_loopVariable in SubRecord)
            {
                EasyUITreeNode SubTreeNode = new EasyUITreeNode(SubRecord_loopVariable.ID, SubRecord_loopVariable.VIRTUAL_ID, SubRecord_loopVariable.CATEGORY_ID, CurrentNode.id, SubRecord_loopVariable.HIE_ID, SubRecord_loopVariable.CATEGORY_NOTE, (int)SubRecord_loopVariable.CATEGORY_TYPE, SubRecord_loopVariable.SEQ_NO, SubRecord_loopVariable.QTY
                    , SubRecord_loopVariable.isExpand, SubRecord_loopVariable.isRequired, SubRecord_loopVariable.isDefault, SubRecord_loopVariable.CONFIGURATION_RULE);

                switch (SubRecord_loopVariable.CATEGORY_TYPE)
                {
                    case CategoryTypes.Category:
                        SubTreeNode.csstype = NodeCssType.Tree_Node_Category;
                        break;
                    case CategoryTypes.Component:
                        SubTreeNode.csstype = NodeCssType.Tree_Node_Component;
                        break;
                    case CategoryTypes.SharedCategory:
                        SubTreeNode.csstype = NodeCssType.Tree_Node_Shared_Category;
                        break;
                    case CategoryTypes.SharedComponent:
                        SubTreeNode.csstype = NodeCssType.Tree_Node_Shared_Component;
                        break;
                    case CategoryTypes.Root:
                        SubTreeNode.csstype = NodeCssType.Tree_Node_Root;
                        break;
                    default:
                        break;
                }

                CurrentNode.children.Add(SubTreeNode);
                CBOMCategoryRecordsToEasyUITreeNode(CBOMCategoryRecords, SubTreeNode);
            }
        }

        public static void CheckSharedCategory(List<String> list, ref List<CBOM_CATEGORY_RECORD> CBOMCategoryRecords)
        {
            List<CBOM_CATEGORY_RECORD> copy = new List<CBOM_CATEGORY_RECORD>();
            copy.AddRange(CBOMCategoryRecords);

            // 對configurator來說，目前只要get包含root共三層的資料，因為component->category->component的架構
            // 只需檢查shared category並取他一層兒子即可
            foreach (CBOM_CATEGORY_RECORD c in copy)
            {
                if (c.CATEGORY_TYPE == CategoryTypes.SharedCategory && !list.Contains(c.VIRTUAL_ID))
                {
                    list.Add(c.ID);
                    GetSharedComponent(ref CBOMCategoryRecords, c.VIRTUAL_ID.Substring(0, 5), c.HIE_ID, c.SHARED_CATEGORY_GUID);
                }
            }
        }

        public static void GetSharedComponent(ref List<CBOM_CATEGORY_RECORD> CBOMCategoryRecords, String VirtualID, String HIEID, String SharedGUID)
        {
            // configurator only get 3 levels, so use HIE_ID.GetAncestor(1) to get 1 level children for shared category

            String str = " DECLARE @ID  hierarchyid " +
                         " SELECT @ID  = HIE_ID " +
                         " FROM CBOM_CATALOG_CATEGORY_V2 WHERE ID = '" + SharedGUID + "' " +
                         " SELECT IsNull('" + VirtualID + "_' + cast(HIE_ID.GetAncestor(1) as nvarchar(100)),'') as PAR_HIE_ID, " +
                         " HIE_ID.GetLevel() AS [LEVEL],ID AS [ID], '" + VirtualID + "_' + ID AS [VIRTUAL_ID], " +
                         " '" + VirtualID + "_' + HIE_ID.ToString() AS [HIE_ID], CATEGORY_ID, CATEGORY_TYPE, " +
                         " CATEGORY_NOTE, SEQ_NO, CONFIGURATION_RULE, ORG, " +
                         " DEFAULT_FLAG as [isDefault], REQUIRED_FLAG as [isRequired], EXPAND_FLAG as [isExpand], " +
                         " SHARED_CATEGORY_ID AS [SHARED_CATEGORY_GUID], MAX_QTY AS [QTY] " +
                         " FROM CBOM_CATALOG_CATEGORY_V2 WHERE HIE_ID.GetAncestor(1) = @ID" +
                         " ORDER BY HIE_ID.GetLevel() ";

            DataTable dtCategoryTree = SqlProvider.dbGetDataTable("CBOMV2", str);
            List<CBOM_CATEGORY_RECORD> SharedRecords = DataTableToList<CBOM_CATEGORY_RECORD>(dtCategoryTree) as List<CBOM_CATEGORY_RECORD>;

            SharedRecords.Remove(SharedRecords.Where(c => c.LEVEL == 2).FirstOrDefault());
            SharedRecords.Where(c => c.LEVEL == 3).ToList().ForEach(c => c.PAR_HIE_ID = HIEID);

            CBOMCategoryRecords.AddRange(SharedRecords);
        }

        public static void RuntimeAddSpecialCategory(EasyUITreeNode RootNode, String ORGID)
        {
            //Ryan 20170328 This function may be a bit ugly, can be improved someday.


            String RootName = RootNode.text;

            // Add Category "Std Assembly,Functional Testing, SW inst (General)"
            EasyUITreeNode NodeStdAssembly = new EasyUITreeNode("CATEGORY_STDASSEMBLY", "CATEGORY_STDASSEMBLY", "Std Assembly,Functional Testing, SW inst (General)",
                RootNode.hieid, "", "", 1, 99, 1, 1, 1, 0, 0);
            EasyUITreeNode NodeSYSA = new EasyUITreeNode("ComponentAGS-CTOS-SYS-B", "ComponentAGS-CTOS-SYS-B", "AGS-CTOS-SYS-B",
                "", "", "Standard Assembly + Functional Testing + Software", 2, 1, 1, 0, 0, 1, 0);
            NodeStdAssembly.children.Add(NodeSYSA);
            RootNode.children.Add(NodeStdAssembly);

            // Add Extended Warranty
            EasyUITreeNode NodeExtendedWarranty = new EasyUITreeNode("CATEGORY_EW", "CATEGORY_EW", " Extended Warranty for " + RootName,
                RootNode.hieid, "", "", 1, 100, 0, 0, 0, 0, 0);

            String str_getEW = String.Format(@"SELECT A.PART_NO, A.PRODUCT_DESC From SAP_PRODUCT A INNER JOIN SAP_PRODUCT_ORG B ON A.PART_NO=B.PART_NO  
                            WHERE  B.ORG_ID='{0}' and A.PART_NO in {1} order by A.PART_NO", ORGID, System.Configuration.ConfigurationManager.AppSettings["StdAGSEWPN"]);
            DataTable dtEW = SqlProvider.dbGetDataTable("MY", str_getEW);
            if (dtEW != null && dtEW.Rows.Count > 0)
            {
                for (int i = 0; i < dtEW.Rows.Count; i++)
                {
                    EasyUITreeNode NodeEW = new EasyUITreeNode(dtEW.Rows[i]["PART_NO"].ToString(), dtEW.Rows[i]["PART_NO"].ToString(), dtEW.Rows[i]["PART_NO"].ToString(),
                                                                "", "", dtEW.Rows[i]["PRODUCT_DESC"].ToString(), 2, i, 1, 0, 0, 0, 0);
                    NodeExtendedWarranty.children.Add(NodeEW);
                }
                RootNode.children.Add(NodeExtendedWarranty);
            }
        }

        public static List<T> DataTableToList<T>(this DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        public static UpdateDBResult Configurator2Cart(String SelectedItems, String ERPID, String CartID, String Currency, String Org_ID, bool isPrj = false)
        {
            UpdateDBResult updateresult = new UpdateDBResult();
            List<ConfiguredItems> items = new List<ConfiguredItems>();

            try
            {
                // Remove all items in current cart for ACN
                if (Org_ID.StartsWith("CN"))
                {
                    CartDetailHelper.RemoveCartDetailByID(CartID);
                }

                List<ConfiguredItems> toAdditems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConfiguredItems>>(SelectedItems);

                if (toAdditems == null || toAdditems.Count == 0)
                {
                    updateresult.IsUpdated = false;
                    updateresult.ServerMessage = "No item was selected.";
                    return updateresult;
                }

                // Split pipeline items
                foreach (ConfiguredItems c in toAdditems)
                {
                    if (c.name.Contains("|"))
                    {
                        //List<String> pipelineitems = c.name.Split('|').ToList();
                        //while (pipelineitems.Count > 0)
                        //{
                        //    ConfiguredItems newitem = new ConfiguredItems();
                        //    newitem.name = pipelineitems.First();
                        //    newitem.desc = c.desc;
                        //    newitem.qty = c.qty;
                        //    pipelineitems.RemoveAt(0);
                        //    toAdditems.Add(newitem);
                        //}
                        foreach (var i in c.name.Split('|').ToList())
                        {
                            SAP_PRODUCT sp = MyAdvantechDAL.GetSAPProduct(i);

                            ConfiguredItems newitem = new ConfiguredItems();
                            newitem.name = i;
                            newitem.desc = (sp != null && !String.IsNullOrEmpty(sp.PRODUCT_DESC)) ? sp.PRODUCT_DESC : c.desc;
                            newitem.qty = c.qty;
                            newitem.isLooseItem = c.isLooseItem;
                            items.Add(newitem);
                        }
                    }
                    else
                        items.Add(c);
                }
                //if (toAdditems.Count > 0)
                //{
                //    items.RemoveAll(d => d.name.Contains("|"));
                //    items.AddRange(toAdditems);
                //}

                //ICC 2017/4/7 Add product compatibility check. To make sure all items are not incompatible.
                List<PRODUCT_COMPATIBILITY> pcList = MyAdvantechDAL.GetProductCompatibility(Compatibility.Incompatible);
                foreach (ConfiguredItems c in items)
                {
                    List<ConfiguredItems> copy = new List<ConfiguredItems>(items);
                    copy.Remove(c);
                    Tuple<bool, string> result = CheckCompatibility(c.name, copy.Select(p => p.name).ToList(), pcList);
                    if (result.Item1 == true)
                    {
                        updateresult.IsUpdated = false;
                        updateresult.ServerMessage = string.Format("This part - {0} is {1} with {2}.", c.name, Compatibility.Incompatible.ToString().ToLower(), result.Item2);
                        return updateresult;
                    }
                }

                //111
                //var _result = "'" + String.Join("','", items) + "'";


            }
            catch (Exception ex)
            {
                updateresult.IsUpdated = false;
                updateresult.ServerMessage = ex.ToString();
                return updateresult;
            }

            int NextHigherLevel = 100;
            var objNextHigherLevel = SqlProvider.dbExecuteScalar("MY", "select max(line_no) from cart_DETAIL_V2 where cart_id = '" + CartID + "' and (otype = 0 or otype = 1)");
            if (objNextHigherLevel != null && !String.IsNullOrEmpty(objNextHigherLevel.ToString()))
                NextHigherLevel = (Convert.ToInt32(objNextHigherLevel.ToString()) / 100 + 1) * 100;
            int NextLooseItemLineNo = 1;
            var objNextLooseItemLineNo = SqlProvider.dbExecuteScalar("MY", "select max(line_no) from cart_DETAIL_V2 where cart_id = '" + CartID + "' and otype = 0 ");
            if (objNextLooseItemLineNo != null && !String.IsNullOrEmpty(objNextLooseItemLineNo.ToString()))
                NextLooseItemLineNo = Convert.ToInt32(objNextLooseItemLineNo.ToString()) + 1;

            if (items.Count > 0)
            {
                try
                {
                    // For ACN, need to split BOTS root name with "/", ex: IPC-510-BTO/P4
                    items.Where(c => c.name.Contains("-BTO/")).ToList().ForEach(c => c.name = c.name.Split(new string[] { "/" }, StringSplitOptions.None)[0]);

                    // Simulate Product List to get price
                    Order _order = new Order();
                    _order.Currency = Currency;
                    _order.OrgID = Org_ID;
                    _order.DistChannel = "10";
                    _order.Division = "00";
                    if (Org_ID.StartsWith("CN"))
                    {
                        if (System.Configuration.ConfigurationManager.AppSettings["ACNTaxRate"] != null)
                            _order.Tax = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["ACNTaxRate"]);
                        else
                            _order.Tax = (Decimal)0.16;
                    }

                    foreach (ConfiguredItems c in items)
                    {

                        //Frank 20171222, if part's material is BTO then identify it as BTOS parent item
                        SAP_PRODUCT sap_p = MyAdvantechDAL.GetSAPProduct(c.name);

                        if (Org_ID.Equals("EU80") && c.isLooseItem)
                        {
                            _order.AddLooseItem(c.name, SAPDAL.GetPlantByOrg(Org_ID), NextLooseItemLineNo, c.qty);
                            NextLooseItemLineNo++;
                        }
                        else if (sap_p != null && (sap_p.MATERIAL_GROUP.Equals("BTOS", StringComparison.InvariantCultureIgnoreCase) || c.name.EndsWith("-BTO", StringComparison.OrdinalIgnoreCase)))
                        {
                            _order.AddBTOSParentItem(c.name, SAPDAL.GetPlantByOrg(Org_ID), NextHigherLevel, c.qty);
                        }
                        else
                            _order.AddBTOSChildItem(c.name, NextHigherLevel, SAPDAL.GetPlantByOrg(Org_ID), c.qty);
                    }
                    _order.SetOrderPartnet(new OrderPartner(ERPID, Org_ID, OrderPartnerType.SoldTo));
                    _order.SetOrderPartnet(new OrderPartner(ERPID, Org_ID, OrderPartnerType.ShipTo));
                    _order.SetOrderPartnet(new OrderPartner(ERPID, Org_ID, OrderPartnerType.BillTo));

                    String _errMsg = String.Empty;
                    Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref _order, ref _errMsg);
                    // End order simulation


                    if (_order != null && String.IsNullOrEmpty(_errMsg))
                    {
                        foreach (Product p in _order.LineItems)
                        {
                            cart_DETAIL_V2 CartDetail = new cart_DETAIL_V2();
                            CartDetail.Cart_Id = CartID;

                            if (p.LineItemType == LineItemType.LooseItem)
                            {
                                CartDetail.Line_No = p.LineNumber;
                                CartDetail.higherLevel = 0;
                            }
                            else if (p.LineItemType == LineItemType.BTOSParent)
                            {
                                CartDetail.Line_No = NextHigherLevel;
                                CartDetail.higherLevel = 0;
                            }
                            else
                            {
                                CartDetail.Line_No = p.LineNumber;
                                CartDetail.higherLevel = NextHigherLevel;
                            }

                            CartDetail.Part_No = p.PartNumber;
                            CartDetail.Description = items.Where(i => i.name == p.PartNumber).FirstOrDefault().desc;
                            CartDetail.Qty = p.Quantity;
                            CartDetail.Ew_Flag = 0;
                            CartDetail.SatisfyFlag = 0;
                            CartDetail.QUOTE_ID = "";
                            CartDetail.Category = items.Where(i => i.name == p.PartNumber).FirstOrDefault().category;

                            //For ACN project, set customer material value = original customer part No.
                            if (isPrj == true && p.LineItemType == LineItemType.BTOSParent && !string.IsNullOrEmpty(CartDetail.Category) && CartDetail.Category.ToUpper().StartsWith("CM-"))
                                CartDetail.CustMaterial = CartDetail.Category;
                            else
                                CartDetail.CustMaterial = "";

                            CartDetail.otype = 0;
                            CartDetail.req_date = DateTime.Now.AddDays(2);
                            if (CartDetail.Line_No < 100)
                            {
                                CartDetail.otype = (int)QuoteItemType.Part;
                            }
                            else if (CartDetail.Line_No % 100 == 0 && CartDetail.higherLevel == 0)
                            {
                                CartDetail.otype = (int)QuoteItemType.BtosParent;
                            }
                            else if (CartDetail.Line_No % 100 > 0 && CartDetail.Line_No > 100)
                            {
                                CartDetail.otype = (int)QuoteItemType.BtosPart;
                            }
                            CartDetail.due_date = CartDetail.req_date;

                            CartDetail.Delivery_Plant = GetDeliveryPlant(ERPID, Org_ID, CartDetail.Part_No, (QuoteItemType)Enum.Parse(typeof(QuoteItemType), CartDetail.otype.ToString()));

                            // set price according to it's item type
                            if (CartDetail.otype == (int)QuoteItemType.BtosParent)
                            {
                                CartDetail.List_Price = 0;
                                CartDetail.Unit_Price = 0;
                                CartDetail.Itp = 0;
                            }
                            else
                            {
                                CartDetail.List_Price = p.ListPrice;
                                CartDetail.Unit_Price = p.UnitPrice;
                                CartDetail.Itp = 0;
                            }

                            // Get product info from sap_product table
                            DataTable sapproduct_dt = SqlProvider.dbGetDataTable("MY", "select * from sap_product where part_no = '" + p.PartNumber + "'");
                            if (sapproduct_dt != null && sapproduct_dt.Rows.Count > 0)
                            {
                                CartDetail.Model_No = sapproduct_dt.Rows[0]["Model_No"].ToString();
                            }


                            MyAdvantechContext.Current.cart_DETAIL_V2.Add(CartDetail);
                        }
                        MyAdvantechContext.Current.SaveChanges();
                        updateresult.IsUpdated = true;
                    }
                    else
                    {
                        updateresult.IsUpdated = false;
                        updateresult.ServerMessage = _errMsg;
                    }
                }
                catch (Exception ex)
                {
                    updateresult.IsUpdated = false;
                    updateresult.ServerMessage = ex.ToString();
                }
            }
            else
                updateresult.IsUpdated = false;

            return updateresult;
        }

        public static UpdateDBResult ConfiguratorSRP2Cart(String SelectedItems, String ERPID, String CartID, String Currency, String Org_ID, String LanguagePack)
        {
            UpdateDBResult updateresult = new UpdateDBResult();
            List<ConfiguredItems> items = new List<ConfiguredItems>();

            try
            {
                //Convert items
                items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConfiguredItems>>(SelectedItems);
                //Convert language pack
                if (!string.IsNullOrEmpty(LanguagePack))
                    LanguagePack = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(LanguagePack);
            }
            catch (Exception ex)
            {
                updateresult.IsUpdated = false;
                updateresult.ServerMessage = ex.ToString();
                return updateresult;
            }

            try
            {
                Order _order = new Order();
                _order.Currency = Currency;
                _order.OrgID = Org_ID;
                _order.DistChannel = "10";
                _order.Division = "00";
                if (Org_ID.StartsWith("CN"))
                {
                    var org = System.Configuration.ConfigurationManager.AppSettings["ACNTaxRate"] ?? "cn";
                    if (System.Configuration.ConfigurationManager.AppSettings["ACNTaxRate"] != null)
                        _order.Tax = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["ACNTaxRate"]);
                    else
                        _order.Tax = (Decimal)0.16;
                }


                foreach (ConfiguredItems c in items)
                    _order.AddLooseItem(c.name, c.qty);

                _order.SetOrderPartnet(new OrderPartner(ERPID, Org_ID, OrderPartnerType.SoldTo));
                _order.SetOrderPartnet(new OrderPartner(ERPID, Org_ID, OrderPartnerType.ShipTo));
                _order.SetOrderPartnet(new OrderPartner(ERPID, Org_ID, OrderPartnerType.BillTo));
                String _errMsg = String.Empty;
                Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref _order, ref _errMsg);

                if (_order != null && String.IsNullOrEmpty(_errMsg))
                {
                    //Get current max cart line no.
                    int line_no = 0;
                    object max = SqlProvider.dbExecuteScalar("MY", string.Format("select ISNULL(max(line_no), 0) from CART_DETAIL_V2 where cart_id='{0}' and otype=0 ", CartID));
                    if (max != null)
                        int.TryParse(max.ToString(), out line_no);
                    line_no = line_no + 1;

                    //Get SRP part in list
                    var srpItem = _order.LineItems.Where(p => p.PartNumber.ToUpper().StartsWith("SRP-")).FirstOrDefault();
                    if (srpItem == null)
                    {
                        updateresult.IsUpdated = false;
                        updateresult.ServerMessage = "No SRP- item in list";
                        return updateresult;
                    }

                    _order.LineItems.Remove(srpItem);
                    srpItem.LineNumber = line_no;
                    int srpLevel = line_no;
                    _order.LineItems.Insert(0, srpItem);

                    if (!string.IsNullOrEmpty(LanguagePack))
                        SqlProvider.dbExecuteNoQuery("MY", string.Format("DELETE FROM SRP_ORDER_LANGUAGE WHERE Cart_ID = '{0}' AND Line_No = {1}; INSERT INTO SRP_ORDER_LANGUAGE VALUES ('{0}', '{2}', {1}, 'SRP OS {3}');", CartID, srpLevel, srpItem.PartNumber, LanguagePack));

                    foreach (Product p in _order.LineItems)
                    {
                        cart_DETAIL_V2 CartDetail = new cart_DETAIL_V2();
                        CartDetail.Cart_Id = CartID;

                        CartDetail.Line_No = line_no;
                        CartDetail.higherLevel = 0;

                        if (!p.PartNumber.ToUpper().StartsWith("SRP-"))
                            CartDetail.higherLevel = srpLevel;

                        CartDetail.Part_No = p.PartNumber;
                        CartDetail.Description = items.Where(i => i.name == p.PartNumber).FirstOrDefault().desc;
                        CartDetail.Qty = p.Quantity;
                        CartDetail.CustMaterial = "";
                        CartDetail.Ew_Flag = 0;
                        CartDetail.SatisfyFlag = 0;
                        CartDetail.QUOTE_ID = "";

                        CartDetail.otype = 0;
                        CartDetail.req_date = DateTime.Now.AddDays(2);
                        if (CartDetail.Line_No < 100)
                        {
                            CartDetail.otype = (int)QuoteItemType.Part;
                        }
                        else if (CartDetail.Line_No % 100 == 0 && CartDetail.higherLevel == 0)
                        {
                            CartDetail.otype = (int)QuoteItemType.BtosParent;
                        }
                        else if (CartDetail.Line_No % 100 > 0 && CartDetail.Line_No > 100)
                        {
                            CartDetail.otype = (int)QuoteItemType.BtosPart;
                        }
                        CartDetail.due_date = CartDetail.req_date;

                        CartDetail.Delivery_Plant = GetDeliveryPlant(ERPID, Org_ID, CartDetail.Part_No, (QuoteItemType)Enum.Parse(typeof(QuoteItemType), CartDetail.otype.ToString()));

                        // set price according to it's item type
                        if (CartDetail.otype == (int)QuoteItemType.BtosParent)
                        {
                            CartDetail.List_Price = 0;
                            CartDetail.Unit_Price = 0;
                            CartDetail.Itp = 0;
                        }
                        else
                        {
                            CartDetail.List_Price = p.ListPrice;
                            CartDetail.Unit_Price = p.UnitPrice;
                            CartDetail.Itp = 0;
                        }

                        // Get product info from sap_product table
                        DataTable sapproduct_dt = SqlProvider.dbGetDataTable("MY", "select * from sap_product where part_no = '" + p.PartNumber + "'");
                        if (sapproduct_dt != null && sapproduct_dt.Rows.Count > 0)
                        {
                            CartDetail.Model_No = sapproduct_dt.Rows[0]["Model_No"].ToString();
                        }


                        MyAdvantechContext.Current.cart_DETAIL_V2.Add(CartDetail);
                        line_no = line_no + 1;
                    }
                    MyAdvantechContext.Current.SaveChanges();
                    updateresult.IsUpdated = true;
                }
                else
                {
                    updateresult.IsUpdated = false;
                    updateresult.ServerMessage = _errMsg;
                }
            }
            catch (Exception ex)
            {
                updateresult.IsUpdated = false;
                updateresult.ServerMessage = ex.ToString();
            }
            return updateresult;
        }

        public static String GetDeliveryPlant(String company_id, String org_id, String part_no, QuoteItemType type)
        {

            if (!String.IsNullOrEmpty(org_id))
            {
                if (org_id.Equals("TW01", StringComparison.OrdinalIgnoreCase))
                {
                    if (type == QuoteItemType.BtosParent || type == QuoteItemType.BtosPart)
                    {
                        return "TWH1";
                    }
                    else
                    {
                        String str = String.Format("select top 1 DELIVERYPLANT from SAP_PRODUCT_ORG where ORG_ID='TW01' and PART_NO = '{0}'", part_no);
                        DataTable dt = DataAccess.SqlProvider.dbGetDataTable("MY", str);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0][0].ToString();
                        else
                            return "TWH1";
                    }
                }
                else
                    return SAPDAL.GetPlantByOrg(org_id);
            }
            else
                return "TWH1";
        }

        public static SRPBTO GetSRPConfigRecord(string rootid, string orgid)
        {
            SRPBTO srp = new SRPBTO(orgid);

            object count = SqlProvider.dbExecuteScalar("CBOMV2", string.Format(@"DECLARE @Child hierarchyid SELECT @Child = HIE_ID 
                FROM CBOM_CATALOG_CATEGORY_V2 WHERE ID = '{0}' SELECT COUNT(*) FROM CBOM_CATALOG_CATEGORY_V2 
                WHERE HIE_ID.GetAncestor(1) = @Child AND ID = '{1}' ", srp.Org.ToString(), rootid));

            if (count != null && Convert.ToInt32(count) == 1)
            {
                List<CBOM_CATEGORY_RECORD> CBOMCategoryRecords = CBOMV2_EditorDAL.GetCBOMCategoryTreeByRootId(rootid, orgid);
                CBOM_CATEGORY_RECORD RootRecord = (from q in CBOMCategoryRecords where q.ID == rootid select q).FirstOrDefault();

                if (RootRecord != null)
                {
                    srp.BTOSName = RootRecord.CATEGORY_NOTE;
                    srp.RealPartNo = RootRecord.CATEGORY_ID;

                    List<CBOM_CATEGORY_RECORD> SubRecord = (from q in CBOMCategoryRecords
                                                            where q.PAR_HIE_ID == RootRecord.HIE_ID &&
                                                                q.CATEGORY_TYPE == CategoryTypes.Root
                                                            orderby q.SEQ_NO
                                                            select q).ToList();

                    foreach (var defaultoption in SubRecord)
                    {
                        EasyUITreeNode RootTreeNode = new EasyUITreeNode(defaultoption.ID, defaultoption.VIRTUAL_ID, defaultoption.CATEGORY_ID, "", defaultoption.HIE_ID, "", 0, 0, 1, 0, 0, 0, 0);
                        RootTreeNode.csstype = NodeCssType.Tree_Node_Root;
                        CBOMCategoryRecordsToEasyUITreeNode(CBOMCategoryRecords, RootTreeNode);

                        if (defaultoption.CATEGORY_ID.ToUpper().IndexOf("DEFAULT") > -1)
                            srp.DefaultPackage = RootTreeNode;
                        else if (defaultoption.CATEGORY_ID.ToUpper().IndexOf("OPTION") > -1)
                            srp.OptionPackage = RootTreeNode;
                    }
                }
            }
            return srp;
        }

        public static decimal CalculatePrice(string jsonString)
        {
            List<MyPrice> prices = new List<MyPrice>();
            decimal total = 0m;
            try
            {
                prices = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MyPrice>>(jsonString);
                foreach (var price in prices)
                {
                    decimal result = 0m;
                    decimal.TryParse(price.Price, out result);
                    total += result;
                }
            }
            catch
            {

            }
            finally
            {
                prices = null;
            }
            return total;
        }

        public static Tuple<bool, string> CheckCompatibility(string partNo, List<string> selectedItem, List<PRODUCT_COMPATIBILITY> pcList)
        {
            try
            {
                List<string> checkList = new List<string>();
                foreach (string item in partNo.Split('|'))
                {
                    var pns = pcList.Where(p => p.PART_NO1.IndexOf(item.Trim()) > -1)
                        .Select(p => new { pn1 = p.PART_NO1, pn2 = p.PART_NO2 })
                        .ToList();

                    foreach (var pn in pns)
                    {
                        foreach (var p in pn.pn1.Split('|'))
                            checkList.Add(p);
                        foreach (var p in pn.pn2.Split('|'))
                            checkList.Add(p);
                        checkList.Remove(item.Trim());
                        if (!checkList.Except(selectedItem).Any() == true)
                            return new Tuple<bool, string>(true, string.Join(",", checkList));
                        checkList.Clear();
                    }
                }
                return new Tuple<bool, string>(false, string.Empty);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(true, ex.Message);
            }

            //Keep old logic
            //DataTable dt = SqlProvider.dbGetDataTable("MY", string.Format("SELECT PART_NO1, PART_NO2 FROM PRODUCT_COMPATIBILITY WHERE PART_NO1 LIKE '%{0}%' AND RELATION = {1}", partNo, (int)compatibility));
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        foreach (string item in dr[0].ToString().Split('|'))
            //            if (!checkList.Contains(item))
            //                checkList.Add(item);
            //        foreach (string item in dr[1].ToString().Split('|'))
            //            if (!checkList.Contains(item))
            //                checkList.Add(item);
            //        checkList.Remove(partNo);
            //        if (!checkList.Except(selectedItem).Any() == true)
            //            return new Tuple<bool, string>(true, string.Join(",", checkList));
            //        checkList.Clear();
            //    }
            //}
        }

        public static DataTable ExpandBOM(string parentItem, string plant)
        {
            ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZPP_BOM_EXPL_MAT_V2_RFC_CKD rfc = new ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZPP_BOM_EXPL_MAT_V2_RFC_CKD();
            ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60Table bomTable = new ZPP_BOM_EXPL_MAT_V2_RFC_CKD.ZTPP_60Table();
            string msg = string.Empty;

            try
            {
                rfc.Connection = new SAP.Connector.SAPConnection(System.Configuration.ConfigurationManager.AppSettings["SAP_PRD"]);
                rfc.Connection.Open();
                rfc.Zpp_Bom_Expl_Mat_V2_Rfc("", " ", parentItem, plant, out msg, ref bomTable);
                rfc.Connection.Close();
                return bomTable.ToADODataTable();
            }
            catch
            {
                return null;
            }
        }

        public static UpdateDBResult AddProject2Cart(string mainItem, string ERPID, string cartID, string currency, string orgID)
        {
            UpdateDBResult udr = new UpdateDBResult();
            try
            {
                DataTable dt = DataCore.CBOMV2_ConfiguratorDAL.ExpandBOM(mainItem, "TWH1");
                if (dt == null || dt.Rows.Count == 0)
                {
                    udr.IsUpdated = false;
                    udr.ServerMessage = "No data";
                    return udr;
                }

                DataTable dt2 = OracleProvider.GetDataTable("SAP_PRD", string.Format("select distinct mast.matnr as Parent_item, stpo.idnrk as child_item, stpo.potx1 as memo from saprdp.mast inner join saprdp.stas  on stas.stlal = mast.stlal AND stas.stlnr = mast.stlnr INNER JOIN saprdp.stpo on stpo.stlkn = stas.stlkn AND stpo.stlnr = stas.stlnr AND stpo.stlty = stas.stlty where stas.LKENZ<>'X' and mast.matnr='{0}'", mainItem));
                List<string> exclude = new List<string>();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt2.Rows)
                    {
                        string pn = CBOMV2_EditorDAL.FormatSAPPartNoToNormal(dr["child_item"].ToString().Trim());
                        string m = dr["memo"].ToString();
                        if ((m.IndexOf("耗材") > -1 || m.IndexOf("客供") > -1) && !exclude.Contains(pn))
                            exclude.Add(pn);
                    }
                }

                List<ConfiguredItems> items = new List<ConfiguredItems>();
                string prjOriginalPartNo = mainItem; //保存原始料號 Ex. CM-10A3-T4A719901

                string[] pitem = mainItem.ToString().Split('-');
                if (pitem.Length != 3)
                {
                    udr.IsUpdated = false;
                    udr.ServerMessage = string.Format("{0} does not have BTO parent item", mainItem);
                    return udr;
                }

                mainItem = string.Format("{0}-{1}-BTO", pitem[0], pitem[1]);
                if (CheckItemOrdereable(mainItem, orgID) == false)
                {
                    udr.IsUpdated = false;
                    udr.ServerMessage = "BTO item can not be sold";
                    return udr;
                }

                object mainItemDesc = SqlProvider.dbExecuteScalar("MY", string.Format("SELECT TOP 1 PRODUCT_DESC FROM SAP_PRODUCT WHERE PART_NO ='{0}'", mainItem));
                ConfiguredItems mi = new ConfiguredItems();
                mi.name = mainItem;
                mi.desc = mainItemDesc != null ? mainItemDesc.ToString() : string.Empty;
                mi.qty = 1;
                mi.category = prjOriginalPartNo;
                items.Add(mi);

                foreach (DataRow dr in dt.Rows)
                {
                    string pn = CBOMV2_EditorDAL.FormatSAPPartNoToNormal(dr["IDNRK"].ToString().Trim());
                    if (!exclude.Contains(pn))
                    {
                        if (CheckItemOrdereable(pn, orgID) == false)
                        {
                            udr.IsUpdated = false;
                            udr.ServerMessage = string.Format("{0} can not be sold", pn);
                            return udr;
                        }

                        double qty = 1;
                        double.TryParse(dr["MENGE"].ToString().Trim(), out qty);
                        int q = Convert.ToInt32(Math.Floor(qty));
                        ConfiguredItems item = new ConfiguredItems();
                        item.name = pn;
                        item.desc = dr["OJTXP"].ToString().Trim();
                        item.qty = q;
                        items.Add(item);
                    }
                }

                //ICC 20170731 中科組裝單要在最後附上 assembly part No.- 目前鎖定 AGS-CTOS-SYS-C
                ConfiguredItems ags = new ConfiguredItems();
                object agsDesc = SqlProvider.dbExecuteScalar("MY", "SELECT TOP 1 PRODUCT_DESC FROM SAP_PRODUCT WHERE PART_NO ='AGS-CTOS-SYS-C'");
                ags.name = "AGS-CTOS-SYS-C";
                ags.desc = agsDesc != null ? agsDesc.ToString() : string.Empty;
                ags.qty = 1;
                items.Add(ags);

                if (string.IsNullOrEmpty(prjOriginalPartNo))
                {
                    udr.IsUpdated = false;
                    udr.ServerMessage = "Original part No. is null";
                    return udr;
                }

                udr = Configurator2Cart(Newtonsoft.Json.JsonConvert.SerializeObject(items), ERPID, cartID, currency, orgID, true);

                return udr;
            }
            catch (Exception ex)
            {
                udr.IsUpdated = false;
                udr.ServerMessage = ex.Message;
                return udr;
            }
        }

        public static bool CheckItemOrdereable(string partNo, string orgID)
        {
            List<String> parts = partNo.Split('|').ToList();

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.Append(" SELECT DISTINCT A.PART_NO AS [name], A.PRODUCT_DESC AS [desc] FROM dbo.SAP_PRODUCT A INNER JOIN SAP_PRODUCT_STATUS_ORDERABLE B ON A.PART_NO=B.PART_NO ");

            if (parts.Count > 1)
                sql.AppendFormat(" WHERE A.PART_NO in ({0}) ", String.Join(",", parts.Select(d => "'" + d + "'").ToArray()));
            else
                sql.AppendFormat(" WHERE A.PART_NO = '{0}' ", partNo);


            sql.AppendFormat(" AND B.PRODUCT_STATUS IN {0} ", System.Configuration.ConfigurationManager.AppSettings["CanOrderProdStatus"]);
            if (orgID.StartsWith("CN"))
            {
                sql.AppendFormat(" AND B.SALES_ORG in ('CN10','CN30','CN70') ");
                sql.AppendFormat(" AND B.PRODUCT_STATUS <> 'O' ");
            }
            else
                sql.AppendFormat("AND B.SALES_ORG ='{0}'", orgID);
            DataTable dt = SqlProvider.dbGetDataTable("MY", sql.ToString());

            if (dt == null || dt.Rows.Count != parts.Count)
                return false;
            else
                return true;
        }

        public static String GetCBOMORG(String _SAPORG)
        {
            if (!String.IsNullOrEmpty(_SAPORG))
            {
                if (_SAPORG.Equals("EU80", StringComparison.OrdinalIgnoreCase))
                    return "DL";
            }

            return _SAPORG;
        }
    }
}
