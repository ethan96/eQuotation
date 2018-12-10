using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.DataCore.ConfigurationHub
{
    public class CatalogModel
    {
        public string RuntimeGUID { get; set; }
        public string ID { get; set; }
        public string CategoryId { get; set; }
        public string ImageId { get; set; }
        public int Version { get; set; }
        public string SalesOrg { get; set; }
        public string CBOMOrg { get; set; }
        public string CatalogName { get; set; }
        public string CatalogDesc { get; set; }
        public CatalogType CatalogType { get; set; }
        public List<CatalogModel> Children { get; set; }
        public string CBOMV2HieID { get; set; }

        public CatalogModel()
        {
            this.RuntimeGUID = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
        }

        public CatalogModel(string id, string categoryId, string imageId, int version, string salesOrg, string cbomOrg, string catalogName, string catalogDesc, CatalogType catalogType, string hieId = "")
        {
            this.RuntimeGUID = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            this.ID = id;
            this.CategoryId = categoryId;
            this.ImageId = imageId;
            this.Version = version;
            this.SalesOrg = salesOrg;
            this.CBOMOrg = cbomOrg;
            this.CatalogName = catalogName;
            this.CatalogDesc = catalogDesc;
            this.CatalogType = catalogType;
            this.Children = new List<CatalogModel>();
            this.CBOMV2HieID = hieId;
        }
    }

    public class ConfiguratorModel
    {
        public string SalesOrg { get; set; }
        public string CBOMOrg { get; set; }
        public int Version { get; set; }             
        public string CompanyID { get; set; }
        public string Currency { get; set; }
        public string ReconfigID { get; set; }
        public string ReconfigData { get; set; }
        public ConfiguratorRecord RootNode { get; set; }
        public string SourceId { get; set; }
        public int SourceLineNo { get; set; }
        public SourceSite SourceSite { get; set; }

        public ConfiguratorModel()
        {
            
        }

        public ConfiguratorModel(string salesOrg, string cbomOrg, int version, string companyId, string currency, ConfiguratorRecord rootNode)
        {
            this.SalesOrg = salesOrg;
            this.CBOMOrg = cbomOrg;
            this.Version = version;
            this.CompanyID = companyId;
            this.Currency = currency;
            this.RootNode = rootNode;                        
        }
    }

    public class ConfiguratorRecord
    {
        public string RuntimeGUID { get; set; }
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public int SeqNo { get; set; }
        public CategoryType CategoryType { get; set; }
        public List<ConfiguratorRecord> Children { get; set; }
        public int MaxQty { get; set; }
        public int MinQty { get; set; }
        public bool isDefault { get; set; }
        public bool isRequired { get; set; }
        public bool isExpand { get; set; }
        public bool isLooseItem { get; set; }
        public string CBOMV2HieID { get; set; }


        public ConfiguratorRecord()
        {
            this.RuntimeGUID = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
        }

        public ConfiguratorRecord(string id, string categoryName, string categoryDesc, int seqNo, CategoryType categoryType, int maxQty, int minQty, bool isDefault, bool isRequired, bool isExpand, bool isLooseItem, string hieID)
        {
            this.RuntimeGUID = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            this.Id = id;
            this.CategoryName = categoryName;
            this.CategoryDesc = categoryDesc;
            this.SeqNo = seqNo;
            this.CategoryType = categoryType;
            this.Children = new List<ConfiguratorRecord>();
            this.MaxQty = maxQty;
            this.MinQty = minQty;
            this.isDefault = isDefault;
            this.isRequired = isRequired;
            this.isExpand = isExpand;
            this.isLooseItem = isLooseItem;
            this.CBOMV2HieID = hieID;
        }
    }

    public enum CatalogType : int
    {
        Root = 0,
        Catalog = 1,
        Component = 2
    }

    public enum CategoryType : int
    {
        Root = 0,
        Category = 1,
        Component = 2,
        SharedCategory = 3,
        SharedComponent = 4
    }


    [Serializable]
    public class UpdateResult
    {
        public bool IsUpdated { get; set; }
        public string ServerMessage { get; set; }
        public UpdateResult()
        {
            IsUpdated = true;
            ServerMessage = "";
        }
    }


    [Serializable]
    public class ConfiguredItems
    {
        public string name { get; set; }
        public string desc { get; set; }
        public int qty { get; set; }
        public string category { get; set; }
        public bool isLooseItem { get; set; }
    }

    public enum SourceSite
    {
        MyAdvantechCore = 0,
        MyAdvantech = 1,
        eQuotation2 = 2,
        eQuotation3 = 3        
    }
}
