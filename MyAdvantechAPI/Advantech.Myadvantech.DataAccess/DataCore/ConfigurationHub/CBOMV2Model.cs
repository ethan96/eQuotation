using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.DataCore.ConfigurationHub
{
    public static class CBOMV2Model
    {
        #region CatalogData

        public class CBOMV2Catalog
        {
            public string Id { get; set; }
            public string CategoryId { get; set; }
            public string ImageId { get; set; }
            public string CatalogName { get; set; }
            public string CatalogDesc { get; set; }
            public int SeqNo { get; set; }
            public string ParentHieId { get; set; }
            public string HieId { get; set; }
            public CatalogType CatalogType { get; set; }
            public int isVisible { get; set; }
            public string SalesOrg { get; set; }
            public string CBOMOrg { get; set; }
        }

        public static List<CBOMV2Catalog> GetCBOMV2CatalogRecords(string salesOrg, string cbomOrg, string companyId)
        {
            string str = string.Format(@" DECLARE @Root hierarchyid 
                                          SELECT @Root = HIE_ID FROM CBOM_CATALOG_V2 
                                          WHERE ID = '{0}_Root' 
                                          SELECT ID as [Id], CATEGORY_GUID as [CategoryId], '' as [ImageID], CATALOG_NAME as [CatalogName], CATALOG_DESC as [CatalogDesc],
						                  SEQ_NO as SeqNo, IsNull(cast(HIE_ID.GetAncestor(1) as nvarchar(100)),'') as [ParentHieId], HIE_ID.ToString() AS [HieId],
						                  '{0}' as [CBOMOrg], '{1}' as [SalesOrg], case when HIE_ID.GetLevel() = 2 then 1 else 2 end as [CatalogType],
                                          case when (select count(*) from ASSIGNED_CTOS where category_id = CBOM_CATALOG_V2.CATEGORY_GUID) = 0 then 1 
                                          else (case when (select count(*) from ASSIGNED_CTOS where COMPANY_ID = '{2}') > 0 then 1 else 0 end) end as isVisible
						                  FROM CBOM_CATALOG_V2 
                                          WHERE HIE_ID.GetAncestor(0) = @Root OR HIE_ID.GetAncestor(1) = @Root  or HIE_ID.GetAncestor(2) = @Root
                                          ORDER BY CATALOG_TYPE, SEQ_NO ", cbomOrg, salesOrg, companyId);
            DataTable dtCatalog = SqlProvider.dbGetDataTable("CBOMV2", str);
            List<CBOMV2Catalog> catalogRecords = dtCatalog.DataTableToList<CBOMV2Catalog>();

            return catalogRecords;
        }

        #endregion


        #region ConfiguratorData

        public class CBOMV2Category
        {
            public string Id { get; set; }
            public int Version { get; set; }
            public string SalesOrg { get; set; }
            public string CBOMOrg { get; set; }
            public string VirtualId { get; set; }
            public string HieId { get; set; }
            public string ParentHieId { get; set; }
            public string CategoryName { get; set; }
            public int SeqNo { get; set; }
            public string CategoryDesc { get; set; }
            public int Level { get; set; }
            public CategoryType CategoryType { get; set; }
            public string SharedCategoryId { get; set; }
            public int Qty { get; set; }
            public int isDefault { get; set; }
            public int isRequired { get; set; }
            public int isExpand { get; set; }
            public int isLooseItem { get; set; }
        }

        public static List<CBOMV2Category> GetCBOMV2CategoryData(string rootId, string salesOrg, string cbomOrg)
        {
            var objSharedID = SqlProvider.dbExecuteScalar("CBOMV2", string.Format("SELECT top 1 SHARED_CATEGORY_ID FROM [CBOMV2].[dbo].[CBOM_CATALOG_CATEGORY_V2] where ID = '{0}'", rootId));
            rootId = (objSharedID != null && !string.IsNullOrEmpty(objSharedID.ToString())) ? objSharedID.ToString() : rootId;

            string str = " DECLARE @Child hierarchyid " +
                         " SELECT @Child = HIE_ID FROM CBOM_CATALOG_CATEGORY_V2 " +
                         " WHERE ID = '" + rootId + "' AND ORG = '" + cbomOrg + "' " +
                         " SELECT IsNull(cast(HIE_ID.GetAncestor(1) as nvarchar(100)),'') as [ParentHieId], " +
                         " HIE_ID.GetLevel() AS [Level], ID AS [Id], ID AS [VirtualId], " +
                         " HIE_ID.ToString() AS [HieId], CATEGORY_ID as [CategoryName], CATEGORY_TYPE as [CategoryType], " +
                         " CATEGORY_NOTE as [CategoryDesc], SEQ_NO as [SeqNo], CONFIGURATION_RULE as [isLooseItem], ORG as [CBOMOrg], " +
                         " '" + salesOrg + "' as [SalesOrg], " +
                         " DEFAULT_FLAG as [isDefault], REQUIRED_FLAG as [isRequired], EXPAND_FLAG as [isExpand], " +
                         " SHARED_CATEGORY_ID AS [SharedCategoryId], MAX_QTY AS [Qty], 2 as [Version] " +
                         " FROM CBOM_CATALOG_CATEGORY_V2 " +
                         " WHERE HIE_ID.GetAncestor(0) = @Child or HIE_ID.GetAncestor(1) = @Child or HIE_ID.GetAncestor(2) = @Child " +
                         " AND ORG = '" + cbomOrg + "' ORDER BY HIE_ID.GetLevel() ";

            DataTable dtCategoryTree = SqlProvider.dbGetDataTable("CBOMV2", str);
            List<CBOMV2Category> CBOMCategoryRecords = dtCategoryTree.DataTableToList<CBOMV2Category>();
            return CBOMCategoryRecords;
        }

        public static void CheckSharedCategory(List<string> list, ref List<CBOMV2Category> cbomCategoryRecords)
        {
            List<CBOMV2Category> copy = new List<CBOMV2Category>();
            copy.AddRange(cbomCategoryRecords);

            foreach (CBOMV2Category c in copy)
            {
                if (c.CategoryType == CategoryType.SharedCategory && !list.Contains(c.VirtualId))
                {
                    list.Add(c.VirtualId);
                    GetSharedComponent(ref cbomCategoryRecords, c.Id.Substring(0, 5), c.HieId, c.SharedCategoryId, c.SalesOrg);
                }
            }

            // !!!!!!! Comment below out due to configurator only takes 3 levels, bypass recursive checking.
            //copy.RemoveAll(d => d.ID != String.Empty);
            //copy.AddRange(CBOMCategoryRecords);
            //if (copy.Where(d => (d.CATEGORY_TYPE == CategoryTypes.SharedCategory || d.CATEGORY_TYPE == CategoryTypes.SharedComponent) && !list.Contains(d.VIRTUAL_ID)).Any())
            //{
            //    CheckSharedCategory(list, ref CBOMCategoryRecords);
            //}
        }

        private static void GetSharedComponent(ref List<CBOMV2Category> cbomCategoryRecords, string virtualId, string hieId, string sharedId, string salesOrg)
        {
            string str = " DECLARE @ID  hierarchyid " +
                         " SELECT @ID  = HIE_ID " +
                         " FROM CBOM_CATALOG_CATEGORY_V2 WHERE ID = '" + sharedId + "' " +
                         " SELECT IsNull('" + virtualId + "_' + cast(HIE_ID.GetAncestor(1) as nvarchar(100)),'') as [ParentHieId], " +
                         " HIE_ID.GetLevel() AS [Level], ID AS [Id], '" + virtualId + "_' + ID AS [VirtualId], " +
                         " '" + virtualId + "_' + HIE_ID.ToString() AS [HieId], CATEGORY_ID as [CategoryName], CATEGORY_TYPE as [CategoryType], " +
                         " CATEGORY_NOTE as [CategoryDesc], SEQ_NO as [SeqNo], CONFIGURATION_RULE as [isLooseItem], ORG as CBOMOrg, " +
                         " '" + salesOrg + "' as SalesOrg, " +
                         " DEFAULT_FLAG as [isDefault], REQUIRED_FLAG as [isRequired], EXPAND_FLAG as [isExpand], " +
                         " SHARED_CATEGORY_ID AS [SharedCategoryId], MAX_QTY AS [Qty], 2 as [Version] " +
                         " FROM CBOM_CATALOG_CATEGORY_V2 WHERE HIE_ID.GetAncestor(1) = @ID " +
                         " ORDER BY HIE_ID.GetLevel() ";

            DataTable dtCategoryTree = SqlProvider.dbGetDataTable("CBOMV2", str);
            List<CBOMV2Category> SharedRecords = DataTableToList<CBOMV2Category>(dtCategoryTree) as List<CBOMV2Category>;

            SharedRecords.Remove(SharedRecords.Where(c => c.Level == 2).FirstOrDefault());
            SharedRecords.Where(c => c.Level == 3).ToList().ForEach(c => c.ParentHieId = hieId);

            cbomCategoryRecords.AddRange(SharedRecords);
        }

        private static List<T> DataTableToList<T>(this DataTable dt)
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

        #endregion




    }
}
