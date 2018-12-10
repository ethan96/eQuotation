using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.DataCore.ConfigurationHub
{
    public class ConfigurationHubDAL
    {
        public static UpdateResult SaveConfiguredRecords(string sourceId, string sourceLineNo, string sourceSite, string rootId, List<Advantech.Myadvantech.DataAccess.DataCore.ConfigurationHub.ConfiguredItems> selectedItems, string companyId, string currency, string reconfigData)
        {
            UpdateResult result = new UpdateResult();

            try
            {
                HubConfiguredResult myEntity = new HubConfiguredResult();
                myEntity.ID = sourceId;
                myEntity.Source = sourceSite;
                myEntity.ParentLineNo = int.Parse(sourceLineNo);
                myEntity.RootID = rootId;
                myEntity.Result = Convert.ToInt32(true);
                myEntity.SelectedItems = Newtonsoft.Json.JsonConvert.SerializeObject(selectedItems);
                myEntity.CompanyID = companyId;
                myEntity.Currency = currency;
                myEntity.ReConfigData = reconfigData;
                myEntity.CreatedTime = System.DateTime.Now;
                MyAdvantechDAL.AddConfigurationHubConfiguredRecords(myEntity);
                result.IsUpdated = true;
            }
            catch (Exception ex)
            {
                result.IsUpdated = false;
                result.ServerMessage = ex.ToString();
            }

            return result;
        }

        public static UpdateResult Configurator2Cart(string cartId, int parentLineNo, string orgId)
        {
            UpdateResult updateResult = new UpdateResult();
            List<ConfiguredItems> items = new List<ConfiguredItems>();

            try
            {
                HubConfiguredResult hcr = MyAdvantechDAL.GetHubConfiguredResultsWithLineNo(cartId, parentLineNo);
                if (hcr != null)
                {
                    // Remove all items in current cart for ACN
                    if (orgId.StartsWith("CN"))
                    {
                        CartDetailHelper.RemoveCartDetailByID(cartId);
                    }

                    List<ConfiguredItems> toAdditems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConfiguredItems>>(hcr.SelectedItems);

                    if (toAdditems == null || toAdditems.Count == 0)
                    {
                        updateResult.IsUpdated = false;
                        updateResult.ServerMessage = "No item was selected.";
                        return updateResult;
                    }

                    // Split pipeline items
                    foreach (ConfiguredItems c in toAdditems)
                    {
                        if (c.name.Contains("|"))
                        {
                            foreach (var i in c.name.Split('|').ToList())
                            {
                                SAP_PRODUCT sp = MyAdvantechDAL.GetSAPProduct(i);

                                ConfiguredItems newitem = new ConfiguredItems();
                                newitem.name = i;
                                newitem.desc = (sp != null && !String.IsNullOrEmpty(sp.PRODUCT_DESC)) ? sp.PRODUCT_DESC : c.desc;
                                newitem.qty = c.qty;
                                //newitem.isLooseItem = c.isLooseItem;
                                items.Add(newitem);
                            }
                        }
                        else
                            items.Add(c);
                    }

                    // Get next higherlevel & next loose line no.
                    int NextHigherLevel = hcr.ParentLineNo;
                    var objNextHigherLevel = SqlProvider.dbExecuteScalar("MY", "select max(line_no) from cart_DETAIL_V2 where cart_id = '" + cartId + "' and (otype = 0 or otype = 1)");
                    if (objNextHigherLevel != null && !String.IsNullOrEmpty(objNextHigherLevel.ToString()))
                        NextHigherLevel = (Convert.ToInt32(objNextHigherLevel.ToString()) / 100 + 1) * 100;
                    int NextLooseItemLineNo = 1;
                    var objNextLooseItemLineNo = SqlProvider.dbExecuteScalar("MY", "select max(line_no) from cart_DETAIL_V2 where cart_id = '" + cartId + "' and otype = 0 ");
                    if (objNextLooseItemLineNo != null && !String.IsNullOrEmpty(objNextLooseItemLineNo.ToString()))
                        NextLooseItemLineNo = Convert.ToInt32(objNextLooseItemLineNo.ToString()) + 1;

                    if (items.Count > 0)
                    {
                        // For ACN, need to split BOTS root name with "/", ex: IPC-510-BTO/P4
                        items.Where(c => c.name.Contains("-BTO/")).ToList().ForEach(c => c.name = c.name.Split(new string[] { "/" }, StringSplitOptions.None)[0]);

                        // Simulate Product List to get price
                        Order _order = new Order();
                        _order.Currency = hcr.Currency;
                        _order.OrgID = orgId;
                        _order.DistChannel = "10";
                        _order.Division = "00";
                        if (orgId.StartsWith("CN"))
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

                            if (orgId.Equals("EU80") && c.isLooseItem)
                            {
                                _order.AddLooseItem(c.name, SAPDAL.GetPlantByOrg(orgId), NextLooseItemLineNo, c.qty);
                                NextLooseItemLineNo++;
                            }
                            else if (sap_p != null && (sap_p.MATERIAL_GROUP.Equals("BTOS", StringComparison.InvariantCultureIgnoreCase) || c.name.EndsWith("-BTO", StringComparison.OrdinalIgnoreCase)))
                            {
                                _order.AddBTOSParentItem(c.name, SAPDAL.GetPlantByOrg(orgId), NextHigherLevel, c.qty);
                            }
                            else
                                _order.AddBTOSChildItem(c.name, NextHigherLevel, SAPDAL.GetPlantByOrg(orgId), c.qty);
                        }
                        _order.SetOrderPartnet(new OrderPartner(hcr.CompanyID, orgId, OrderPartnerType.SoldTo));
                        _order.SetOrderPartnet(new OrderPartner(hcr.CompanyID, orgId, OrderPartnerType.ShipTo));
                        _order.SetOrderPartnet(new OrderPartner(hcr.CompanyID, orgId, OrderPartnerType.BillTo));

                        String _errMsg = String.Empty;
                        Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref _order, ref _errMsg);
                        // End order simulation


                        if (_order != null && String.IsNullOrEmpty(_errMsg))
                        {
                            foreach (Product p in _order.LineItems)
                            {
                                cart_DETAIL_V2 CartDetail = new cart_DETAIL_V2();
                                CartDetail.Cart_Id = cartId;

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

                                CartDetail.Delivery_Plant = SAPDAL.GetDeliveryPlant(hcr.CompanyID, orgId, CartDetail.Part_No, (QuoteItemType)Enum.Parse(typeof(QuoteItemType), CartDetail.otype.ToString()));

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
                            updateResult.IsUpdated = true;
                        }
                        else
                        {
                            updateResult.IsUpdated = false;
                            updateResult.ServerMessage = _errMsg;
                        }

                    }

                }
                else
                {
                    updateResult.IsUpdated = false;
                    updateResult.ServerMessage = "No matched record found.";
                }
            }
            catch (Exception ex)
            {
                updateResult.IsUpdated = false;
                updateResult.ServerMessage = ex.ToString();
            }

            return updateResult;
        }


        #region CBOMV1 

        public static CatalogModel GetCBOMV1CatalogModel(string salesOrg, string cbomOrg, string companyId)
        {
            // 1. Get DB records
            List<CBOMV1Model.CBOMV1Catalog> catalogRecords = CBOMV1Model.GetCBOMV1CatalogRecords(salesOrg, cbomOrg);
            CatalogModel rootModel = new CatalogModel("Root", "", "", 1, salesOrg, cbomOrg, "CatalogRoot", "", CatalogType.Root);

            // 2. Convert to standard viewmodel CatalogModel
            // Bottom-up, consider to V1 schema, get all components first, then add catalog records from its parent name.
            if (catalogRecords != null && catalogRecords.Count > 0)
            {
                foreach (CBOMV1Model.CBOMV1Catalog distinctCatalog in catalogRecords.GroupBy(d => d.ParentName).Select(d => d.First()))
                {
                    CatalogModel catalog = new CatalogModel(distinctCatalog.ParentName, "", "", 1, salesOrg, cbomOrg, distinctCatalog.ParentName, "", CatalogType.Catalog);

                    foreach (CBOMV1Model.CBOMV1Catalog catalogRecord in catalogRecords.Where(d => d.ParentName == catalog.CatalogName && (catalog.CatalogName.Equals("CTOS") ? d.CatalogName.Contains(companyId) : true)).OrderBy(d => d.CatalogName))
                    {
                        CatalogModel component = new CatalogModel(catalogRecord.Id, catalogRecord.CategoryId, catalogRecord.ImageId, 1, salesOrg, cbomOrg, catalogRecord.CatalogName, catalogRecord.CatalogDesc, CatalogType.Component);
                        catalog.Children.Add(component);
                    }

                    rootModel.Children.Add(catalog);
                }
            }

            return rootModel;
        }

        public static ConfiguratorModel GetCBOMV1ConfiguratorModel(string rootId, string salesOrg, string cbomOrg)
        {
            // 1. Get all records include Root, categories, components from DB
            List<CBOMV1Model.CBOMV1Category> categoryRecords = CBOMV1Model.GetCBOMV1CategoryRecords(rootId, salesOrg, cbomOrg);
            categoryRecords.RemoveAll(item => (item.CategoryType == CategoryType.Component || item.CategoryType == CategoryType.SharedComponent) && !isPartOrdereable(item.CategoryName, salesOrg));
            CBOMV1Model.CBOMV1Category rootRecord = (from q in categoryRecords where q.Id == rootId select q).ToList().FirstOrDefault();

            // 2. Convert DB records to ConfiguratorRecord which contains whole tree structure
            ConfiguratorRecord rootNode = new ConfiguratorRecord(rootRecord.Id, rootRecord.CategoryName, rootRecord.CategoryDesc, 0, CategoryType.Root, 1, 1, true, false, true, false, "");
            CBOMV1Category2Model(categoryRecords, rootNode);

            // 3. Add rootNode to standard ViewModel ConfiguratorModel
            ConfiguratorModel rootModel = new ConfiguratorModel(salesOrg, cbomOrg, 1, "", "", rootNode);

            return rootModel;
        }

        private static void CBOMV1Category2Model(List<CBOMV1Model.CBOMV1Category> categoryRecords, ConfiguratorRecord currentNode)
        {
            List<CBOMV1Model.CBOMV1Category> subRecords = (from q in categoryRecords where q.ParentId == currentNode.Id orderby q.SeqNo, q.CategoryName select q).ToList();
            if (subRecords.Count == 0) return;

            foreach (CBOMV1Model.CBOMV1Category subRecord in subRecords)
            {
                ConfiguratorRecord subModel = new ConfiguratorRecord(subRecord.Id, subRecord.CategoryName, subRecord.CategoryDesc, subRecord.SeqNo, subRecord.CategoryType, subRecord.Qty, 1, Convert.ToBoolean(subRecord.isDefault), Convert.ToBoolean(subRecord.isRequired), Convert.ToBoolean(subRecord.isExpand), false, "");

                currentNode.Children.Add(subModel);
                CBOMV1Category2Model(categoryRecords, subModel);
            }
        }

        #endregion


        #region CBOMV2

        public static CatalogModel GetCBOMV2CatalogModel(string salesOrg, string cbomOrg, string companyId)
        {
            // 1. Get all records include Root, catalogs, components from DB
            List<CBOMV2Model.CBOMV2Catalog> catalogRecords = CBOMV2Model.GetCBOMV2CatalogRecords(salesOrg, cbomOrg, companyId);
            CBOMV2Model.CBOMV2Catalog rootRecord = catalogRecords.Where(d => d.CatalogName.Equals(cbomOrg + "_Root")).FirstOrDefault();
            catalogRecords.RemoveAll(d => d.CatalogType == CatalogType.Component && !isPartOrdereable((d.CatalogName.Contains("/") ? d.CatalogName.Split('/')[0] : d.CatalogName), salesOrg));

            // 2. Convert DB records to standard ViewModel CatalogModel
            CatalogModel rootModel = new CatalogModel(rootRecord.Id, "", "", 2, salesOrg, cbomOrg, rootRecord.CatalogName, "", CatalogType.Root, rootRecord.HieId);
            if (catalogRecords != null && catalogRecords.Count > 0)
            {
                CBOMV2CatalogData2Model(catalogRecords, rootModel);
            }

            return rootModel;
        }

        private static void CBOMV2CatalogData2Model(List<CBOMV2Model.CBOMV2Catalog> catalogRecords, CatalogModel currentNode)
        {
            List<CBOMV2Model.CBOMV2Catalog> subRecords = (from q in catalogRecords where q.ParentHieId == currentNode.CBOMV2HieID && Convert.ToBoolean(q.isVisible) orderby q.SeqNo, q.CatalogName select q).ToList();
            if (subRecords.Count == 0) return;

            foreach (CBOMV2Model.CBOMV2Catalog subRecord in subRecords)
            {
                CatalogModel subModel = new CatalogModel(subRecord.Id, subRecord.CategoryId, subRecord.ImageId, 2, subRecord.SalesOrg, subRecord.CBOMOrg, subRecord.CatalogName, subRecord.CatalogDesc, subRecord.CatalogType, subRecord.HieId);

                currentNode.Children.Add(subModel);
                CBOMV2CatalogData2Model(catalogRecords, subModel);
            }
        }


        public static ConfiguratorModel GetCBOMV2ConfiguratorModel(string rootId, string salesOrg, string cbomOrg)
        {
            // 1. Get all records include Root, categories, components from DB
            List<CBOMV2Model.CBOMV2Category> categoryRecords = CBOMV2Model.GetCBOMV2CategoryData(rootId, salesOrg, cbomOrg);
            CBOMV2Model.CheckSharedCategory(new List<String>(), ref categoryRecords);
            categoryRecords.RemoveAll(item => (item.CategoryType == CategoryType.Component || item.CategoryType == CategoryType.SharedComponent) && !isPartOrdereable(item.CategoryName, salesOrg));
            CBOMV2Model.CBOMV2Category rootRecord = (from q in categoryRecords where (q.Id.Equals(rootId) || q.Level == 2) select q).ToList().FirstOrDefault();

            // 2. Convert DB records to ConfiguratorRecord which contains whole tree structure
            ConfiguratorRecord rootNode = new ConfiguratorRecord(rootRecord.Id, rootRecord.CategoryName, rootRecord.CategoryDesc, 0, CategoryType.Root, 1, 1, true, false, true, false, rootRecord.HieId);
            CBOMV2ConfiguratorData2Model(categoryRecords, rootNode);

            // 3. Add rootNode to standard ViewModel ConfiguratorModel
            ConfiguratorModel rootModel = new ConfiguratorModel(salesOrg, cbomOrg, 2, "", "", rootNode);

            return rootModel;
        }

        private static void CBOMV2ConfiguratorData2Model(List<CBOMV2Model.CBOMV2Category> categoryRecords, ConfiguratorRecord currentNode)
        {
            List<CBOMV2Model.CBOMV2Category> subRecords = (from q in categoryRecords where q.ParentHieId == currentNode.CBOMV2HieID orderby q.SeqNo, q.CategoryName select q).ToList();
            if (subRecords.Count == 0) return;

            foreach (CBOMV2Model.CBOMV2Category subRecord in subRecords)
            {
                ConfiguratorRecord subModel = new ConfiguratorRecord(subRecord.Id, subRecord.CategoryName, subRecord.CategoryDesc, subRecord.SeqNo, subRecord.CategoryType, subRecord.Qty, 1, Convert.ToBoolean(subRecord.isDefault), Convert.ToBoolean(subRecord.isRequired), Convert.ToBoolean(subRecord.isExpand), Convert.ToBoolean(subRecord.isLooseItem), subRecord.HieId);

                currentNode.Children.Add(subModel);
                CBOMV2ConfiguratorData2Model(categoryRecords, subModel);
            }
        }

        #endregion




        public static bool isPartOrdereable(string partNo, string salesOrg)
        {
            List<String> parts = partNo.Split('|').ToList();

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.Append(" SELECT DISTINCT A.PART_NO AS [name], A.PRODUCT_DESC AS [desc] FROM dbo.SAP_PRODUCT A INNER JOIN SAP_PRODUCT_STATUS_ORDERABLE B ON A.PART_NO=B.PART_NO ");

            if (parts.Count > 1)
                sql.AppendFormat(" WHERE A.PART_NO in ({0}) ", String.Join(",", parts.Select(d => "'" + d + "'").ToArray()));
            else
                sql.AppendFormat(" WHERE A.PART_NO = '{0}' ", partNo);

            sql.AppendFormat(" AND B.PRODUCT_STATUS IN ('A','N','H','O','M1','C','P','S2','S5','T','V') ");
            if (salesOrg.StartsWith("CN"))
            {
                sql.AppendFormat(" AND B.SALES_ORG in ('CN10','CN30') ");
                sql.AppendFormat(" AND B.PRODUCT_STATUS <> 'O' ");
            }
            else
                sql.AppendFormat("AND B.SALES_ORG ='{0}'", salesOrg);
            DataTable dt = SqlProvider.dbGetDataTable("MY", sql.ToString());

            if (dt == null || dt.Rows.Count != parts.Count)
                return false;
            else
                return true;
        }

        public static string GetCBOMOrgBySalesOrg(string salesOrg)
        {
            string result = string.Empty;

            switch (salesOrg)
            {
                case "TW01":
                case "JP01":
                case "KR01":
                case "SG01":
                case "MY01":
                    result = "TW";
                    break;

                case "EU10":
                    result = "EU";
                    break;

                case "EU80":
                    result = "DL";
                    break;

                case "US01":
                case "US10":
                    result = "US";
                    break;

                case "CN10":
                case "CN30":
                case "CN70":
                    result = "CN";
                    break;
            }
            return result;
        }

        public static int GetCBOMVersionByCBOMOrg(string cbomOrg)
        {
            switch (cbomOrg)
            {
                case "TW":
                    return 1;
                case "EU":
                    return 1;
                case "US":
                    return 1;
                case "DL":
                    return 21;
                case "CN":
                    return 2;
                default:
                    return 0;
            }
        }

    }
}
