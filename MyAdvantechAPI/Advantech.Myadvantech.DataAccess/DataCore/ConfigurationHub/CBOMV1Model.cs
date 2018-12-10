using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.DataCore.ConfigurationHub
{
    public static class CBOMV1Model
    {
        #region CatalogData

        public class CBOMV1Catalog
        {
            public string Id { get; set; }
            public string CategoryId { get; set; }
            public string ImageId { get; set; }
            public string CatalogName { get; set; }
            public string CatalogDesc { get; set; }
            public int SeqNo { get; set; }
            public string ParentName { get; set; }
            public CatalogType CatalogType { get; set; }
            public string SalesOrg { get; set; }
            public string CBOMOrg { get; set; }
        }

        public static List<CBOMV1Catalog> GetCBOMV1CatalogRecords(string salesOrg, string cbomOrg)
        {
            string str = string.Format(@" select a.UID as Id, d.UID as CategoryId, a.IMAGE_ID as ImageID, a.CATALOG_ID as CatalogName, a.CATALOG_DESC as CatalogDesc, 
                                        IsNull(b.LOCAL_NAME,a.Catalog_Type) as ParentName, '{1}' as CBOMOrg, '{0}' as SalesOrg, 2 as CatalogType
                                        from CBOM_Catalog a 
                                        left join CBOM_CATALOG_LOCALNAME b on a.Catalog_Type = b.catalog_type and a.Catalog_Org = b.org
                                        inner join SAP_PRODUCT_STATUS_ORDERABLE c on a.CATALOG_NAME = c.PART_NO 
                                        inner join CBOM_CATALOG_CATEGORY d on a.CATALOG_ID = d.CATEGORY_ID
                                        WHERE a.Catalog_Org = '{0}' and a.Catalog_Type <> '' and c.SALES_ORG = '{1}' and d.ORG = '{0}'
                                        order by a.CATALOG_TYPE ", cbomOrg, salesOrg);
            DataTable dtCatalog = SqlProvider.dbGetDataTable("MY", str);
            List<CBOMV1Catalog> catalogRecords = dtCatalog.DataTableToList<CBOMV1Catalog>();

            return catalogRecords;
        }

        #endregion



        #region ConfiguratorData

        public class CBOMV1Category
        {
            public string Id { get; set; }
            public int Version { get; set; }
            public string SalesOrg { get; set; }
            public string CBOMOrg { get; set; }
            public string CategoryName { get; set; }
            public string ParentId { get; set; }
            public string ParentName { get; set; }
            public int SeqNo { get; set; }
            public string CategoryDesc { get; set; }
            public int Level { get; set; }
            public CategoryType CategoryType { get; set; }
            public string SharedCategoryId { get; set; }
            public int Qty { get; set; }
            public int isDefault { get; set; }
            public int isRequired { get; set; }
            public int isExpand { get; set; }

        }

        public static List<CBOMV1Category> GetCBOMV1CategoryRecords(string rootId, string salesOrg, string cbomOrg)
        {
            string str = string.Format(@" SELECT TOP 1 a.UID as Id, '0' as ParentId, a.PARENT_CATEGORY_ID as ParentName, a.CATEGORY_ID as CategoryName, 
                          0 as CategoryType, IsNull(a.CATEGORY_DESC, '') as CategoryDesc, IsNull(a.SEQ_NO, 0) as SeqNo, 0 as ConfigurationRule, 
                          '{1}' as SalesOrg, a.ORG as CBOMOrg, 0 isDefault, 0 as isRequired, 1 as isExpand, 1 as Qty, 1 as Version
                          FROM CBOM_CATALOG_CATEGORY AS a
                          WHERE a.UID = N'{0}' and a.org = '{2}' ", rootId, salesOrg, cbomOrg);
            DataTable dtResult = SqlProvider.dbGetDataTable("MY", str);

            DataTable dtChild = GetCBOMV1SubCategory(rootId, salesOrg, cbomOrg);

            if (dtChild != null && dtChild.Rows.Count > 0)
            {
                dtResult.Merge(dtChild);
                foreach (DataRow drRoot in dtChild.Rows)
                {
                    DataTable dtGrandChild = GetCBOMV1SubCategory(drRoot["Id"].ToString(), salesOrg, cbomOrg);
                    dtResult.Merge(dtGrandChild);
                }
            }

            List<CBOMV1Category> categoryRecords = dtResult.DataTableToList<CBOMV1Category>();
            return categoryRecords;
        }

        private static DataTable GetCBOMV1SubCategory(string rootId, string salesOrg, string cbomOrg)
        {
            var objRoot = SqlProvider.dbExecuteScalar("MY", string.Format("select top 1 CATEGORY_ID from CBOM_CATALOG_CATEGORY where UID = '{0}' and ORG = '{1}'", rootId, cbomOrg));
            string rootName = (objRoot != null && !string.IsNullOrEmpty(objRoot.ToString())) ? objRoot.ToString() : "";

            string str = string.Format(@" SELECT distinct a.UID as Id, '{0}' as ParentId, a.PARENT_CATEGORY_ID as ParentName, a.CATEGORY_ID as CategoryName, 
                          case when a.CATEGORY_TYPE = 'Category' then 1 when a.CATEGORY_TYPE = 'Component' and a.PARENT_CATEGORY_ID = 'Root' then 0 else 2 end as CategoryType, 
                          IsNull(a.CATEGORY_DESC, '') as CategoryDesc, IsNull(a.SEQ_NO, 0) as SeqNo, 0 as ConfigurationRule, 
                          '{2}' as SalesOrg, a.ORG as CBOMOrg, case IsNull(a.DEFAULT_FLAG, 0) when '' then 0 else a.DEFAULT_FLAG end as isDefault, 
                          case IsNull(a.CONFIGURATION_RULE, 0) when 'REQUIRED' then 1 else 0 end as isRequired,
                          case IsNull(a.CONFIGURATION_RULE, 0) when 'REQUIRED' then 1 else 0 end as isExpand, 1 as Qty, 1 as Version
                          FROM CBOM_CATALOG_CATEGORY AS a 
                          WHERE a.PARENT_CATEGORY_ID = N'{1}' and a.org = '{3}' and a.CATEGORY_ID<> N'{1}' 
                          and(a.CATEGORY_TYPE = 'Category' or A.CATEGORY_TYPE = 'Component' or(a.CATEGORY_TYPE = 'Component' and(a.CATEGORY_ID = 'No Need' or a.CATEGORY_ID like '%|%')) or CATEGORY_TYPE = 'extendedcategory' or CATEGORY_TYPE = 'extendedComponent') 
                          ORDER BY SeqNo ", rootId, rootName, salesOrg, cbomOrg);
            DataTable dt = SqlProvider.dbGetDataTable("MY", str);

            return dt;
        }

        #endregion



    }
}
