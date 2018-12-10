using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{

    public class EasyUITreeNode
    {
        public String id { get; set; }
        public String virtualid { get; set; }
        public String text { get; set; }
        public String desc { get; set; }
        public String hieid { get; set; }
        public String parentid { get; set; }
        public List<EasyUITreeNode> children { get; set; }
        public int seq { get; set; }
        public int type { get; set; }
        public int qty { get; set; }
        public int isdefault { get; set; }
        public int isrequired { get; set; }
        public int isexpand { get; set; }
        public int configurationrule { get; set; }
        private treeStates _state;
        private string iconcls;
        public string iconCls
        {
            get
            {
                return this.iconcls;
            }
        }
        public NodeCssType csstype
        {
            set
            {
                this.iconcls = value.ToString();
            }
        }

        public EasyUITreeNode()
        {
            id = "";
            virtualid = "";
            text = "";
            desc = "";
            hieid = "";
            parentid = "";
            this.children = new List<EasyUITreeNode>();
            seq = 0;
            type = -1;
            qty = 1;
            isdefault = 0;
            isrequired = 0;
            isexpand = 0;
            configurationrule = 0;
            _state = treeStates.open;
        }

        public EasyUITreeNode(String _id,String _virtualid, String _text, String _parentid, String _hieid, String _desc, int _type, int _seq, int _qty, int _expand, int _required, int _default, int _configurationrule)
        {
            id = _id;
            virtualid = _virtualid;
            text = _text;
            parentid = _parentid;
            hieid = _hieid;
            desc = _desc;
            type = _type;
            qty = _qty;
            seq = _seq;
            this.children = new List<EasyUITreeNode>();
            isexpand = _expand;
            isrequired = _required;
            isdefault = _default;
            configurationrule = _configurationrule;
            _state = treeStates.open;
        }
        public enum treeStates
        {
            open,
            closed,
        }
        public treeStates SetState
        {
            set
            {
                this._state = value;
            }
        }
        public string state
        {
            get
            {
                return this._state.ToString();
            }
        }
    }

    public enum NodeCssType
    {
        Tree_Node_Root,
        Tree_Node_Category,
        Tree_Node_Shared_Category,
        Tree_Node_Component,
        Tree_Node_Shared_Component
    }
    public enum CategoryTypes : int
    {
        Root = 0,
        Category = 1,
        Component = 2,
        SharedCategory = 3,
        SharedComponent = 4
    }

    public class CBOM_CATEGORY_RECORD
    {
        public string ID { get; set; }
        public string VIRTUAL_ID { get; set; }
        public string HIE_ID { get; set; }
        public string PAR_HIE_ID { get; set; }
        public string CATEGORY_ID { get; set; }
        public int SEQ_NO { get; set; }
        public string CATEGORY_NOTE { get; set; }
        public string SECTOR { get; set; }
        public int LEVEL { get; set; }
        public CategoryTypes CATEGORY_TYPE { get; set; }
        public string SHARED_CATEGORY_GUID { get; set; }
        public int QTY { get; set; }
        public int isDefault { get; set; }
        public int isRequired { get; set; }
        public int isExpand { get; set; }
        public int CONFIGURATION_RULE { get; set; }
        //public string iconCls { get; set; }
    }

    public class CBOM_CATALOG_RECORD
    {
        public string ID { get; set; }
        public string VIRTUAL_ID { get; set; }
        public string HIE_ID { get; set; }
        public string PAR_HIE_ID { get; set; }
        public string CATALOG_NAME { get; set; }
        public int SEQ_NO { get; set; }
        public string CATALOG_DESC { get; set; }
        public string SECTOR { get; set; }
        public int LEVEL { get; set; }
        public CategoryTypes CATALOG_TYPE { get; set; }
        public int QTY { get; set; }
    }

    public class UpdateDBResult
    {
        public bool IsUpdated { get; set; }
        public string ServerMessage { get; set; }
        public EasyUITreeNode Node { get; set; }
        public UpdateDBResult()
        {
            IsUpdated = true;
            ServerMessage = "";
            Node = new EasyUITreeNode();
        }
    }

    public enum CBOMAddType
    {
        AddNewCategory,
        AddSharedCategory,
        CopySharedCategory,
        AddNewComponent,
        AddSharedComponent,
        CopySharedComponent
    }

    [Serializable]
    public class ConfiguredItems
    {
        public string name;
        public string desc;
        public int qty;
        public string category;
        public Boolean isLooseItem;
    }

    public enum SRPORG
    {
        AU_SRP,
        BR_SRP,
        CN_SRP,
        EU_SRP,
        HK_SRP,
        ID_SRP,
        IN_SRP,
        JP_SRP,
        KR_SRP,
        MX_SRP,
        MY_SRP,
        SG_SRP,
        TL_SRP,
        TW_SRP,
        US_SRP
    }

    [Serializable]
    public class SRPBTO
    {
        public SRPBTO()
        {
            this.btosname = string.Empty;
            this.realpartno = string.Empty;
            this.defaultpackage = new EasyUITreeNode();
            this.optionpackage = new EasyUITreeNode();
        }

        public SRPBTO(string org)
        {
            this.btosname = string.Empty;
            this.realpartno = string.Empty;
            this.defaultpackage = new EasyUITreeNode();
            this.optionpackage = new EasyUITreeNode();

            switch (org.ToUpper())
            {
                case "AU":
                    this.Org = SRPORG.AU_SRP;
                    break;
                case "BR":
                    this.Org = SRPORG.BR_SRP;
                    break;
                case "CN":
                    this.Org = SRPORG.CN_SRP;
                    break;
                case "EU":
                    this.Org = SRPORG.EU_SRP;
                    break;
                case "HK":
                    this.Org = SRPORG.HK_SRP;
                    break;
                case "ID":
                    this.Org = SRPORG.ID_SRP;
                    break;
                case "IN":
                    this.Org = SRPORG.IN_SRP;
                    break;
                case "JP":
                    this.Org = SRPORG.JP_SRP;
                    break;
                case "KR":
                    this.Org = SRPORG.KR_SRP;
                    break;
                case "MX":
                    this.Org = SRPORG.MX_SRP;
                    break;
                case "MY":
                    this.Org = SRPORG.MY_SRP;
                    break;
                case "SG":
                    this.Org = SRPORG.SG_SRP;
                    break;
                case "TL":
                    this.Org = SRPORG.TL_SRP;
                    break;
                case "US":
                    this.Org = SRPORG.US_SRP;
                    break;
                case "TW":
                default:
                    this.Org = SRPORG.TW_SRP;
                    break;
            }

        }

        //ORG
        private SRPORG? org;
        public SRPORG Org
        {
            get
            {
                if (this.org.HasValue == false)
                    this.org = SRPORG.TW_SRP;
                return this.org.Value;
            }
            set
            {
                this.org = value;
            }
        }

        //XXX-BTO
        private string btosname;
        public string BTOSName
        {
            get
            {
                return this.btosname;
            }
            set
            {
                this.btosname = value;
            }
        }

        //Part No
        private string realpartno;
        public string RealPartNo
        {
            get
            {
                return this.realpartno;
            }
            set
            {
                this.realpartno = value;
            }
        }

        private EasyUITreeNode defaultpackage;
        public EasyUITreeNode DefaultPackage
        {
            get
            {
                return this.defaultpackage;
            }
            set
            {
                this.defaultpackage = value;
            }
        }

        private EasyUITreeNode optionpackage;
        public EasyUITreeNode OptionPackage
        {
            get
            {
                return this.optionpackage;
            }
            set
            {
                this.optionpackage = value;
            }
        }
    }

    [Serializable]
    public class MyPrice
    {
        [Newtonsoft.Json.JsonProperty("price")]
        public string Price { get; set; }
    }

    [Serializable]
    public class ProductCompatibility
    {
        [Newtonsoft.Json.JsonProperty("ID")]
        public int ID { get; set; }

        [Newtonsoft.Json.JsonProperty("pn1")]
        public string PART_NO1 { get; set; }

        [Newtonsoft.Json.JsonProperty("pn2")]
        public string PART_NO2 { get; set; }

        [Newtonsoft.Json.JsonProperty("relation")]
        public string RELATION { get; set; }

        [Newtonsoft.Json.JsonProperty("reason")]
        public string REASON { get; set; }

        [Newtonsoft.Json.JsonProperty("userID")]
        public string UPDATE_ID { get; set; }
    }

    [Serializable]
    public class ProjectCatalogCategory
    {
        [Newtonsoft.Json.JsonProperty("ID")]
        public string ID { get; set; }

        [Newtonsoft.Json.JsonProperty("ERPID")]
        public string COMPANY_ID { get; set; }

        [Newtonsoft.Json.JsonProperty("pn")]
        public string PART_NO { get; set; }

        [Newtonsoft.Json.JsonProperty("memo")]
        public string MEMO { get; set; }
    }

    [Serializable]
    public class AssignedCTOS_Master
    {
        [Newtonsoft.Json.JsonProperty("ID")]
        public int Row_ID { get; set; }

        [Newtonsoft.Json.JsonProperty("account")]
        public string CompanyID { get; set; }

        private List<AssignedCTOS_Detail> _deatils;

        [Newtonsoft.Json.JsonProperty("ctos")]
        public List<AssignedCTOS_Detail> Details
        {
            get
            {
                if (this._deatils == null)
                    this._deatils = new List<AssignedCTOS_Detail>();
                return this._deatils;
            }
            set
            {
                this._deatils = value;
            }
        }
        
    }

    [Serializable]
    public class AssignedCTOS_Detail
    {
        [Newtonsoft.Json.JsonProperty("ID")]
        public int Row_ID { get; set; }

        [Newtonsoft.Json.JsonProperty("name")]
        public string CTOSName { get; set; }

        [Newtonsoft.Json.JsonProperty("desc")]
        public string CTOSDescription { get; set; }

        [Newtonsoft.Json.JsonProperty("user")]
        public string UserID { get; set; }

        [Newtonsoft.Json.JsonProperty("time")]
        public string CreatedDate { get; set; }
    }
}
