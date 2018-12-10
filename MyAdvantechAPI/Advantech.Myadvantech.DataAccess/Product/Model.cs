using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Data.Common;

[assembly: InternalsVisibleTo("Advantech.Myadvantech.Business")]
[assembly: InternalsVisibleTo("Advantech.PIS.Business")]
namespace Advantech.Myadvantech.DataAccess
{

    /// <summary>
    /// PIS Model data entity
    /// </summary>
    public class Model
    {

        private List<Category> _Parent_Category = new List<Category>();
        /// <summary>
        /// Get model' parent category
        /// </summary>
        public List<Category> Parent_Category
        {
            get
            {
                if (this._Parent_Category.Count == 0)
                {
                    this.LoadModel_ParentCategory();
                }
                return _Parent_Category;
            }
            //set { this._Model_Langs = Model_Langs; }
        }

        public virtual string Model_ID
        {
            get;
            internal set;
        }


        public virtual string Model_Name
        {
            get;
            internal set;
        }

        public virtual string Display_Name
        {
            get;
            internal set;
        }

        public virtual string Description
        {
            get;
            protected set;
        }

        public virtual string Introduction
        {
            get;
            protected set;
        }

        public virtual ModelType Model_Type
        {
            get;
            protected set;
        }

        public virtual string Publish_status
        {
            get;
            protected set;
        }

        /// <summary>
        /// Get Model's model page in Corp. web site
        /// </summary>
        /// <param name="reg">return with regin's url</param>
        /// <returns></returns>
        public String GetModelURL(AOnlineRegion reg = AOnlineRegion.NA)
        {
            //http://www.advantech.com/products/gf-5u7m/adam-4520/mod_8dcee4b7-fbde-4c5d-9752-ed2f2fbd00bf
            StringBuilder _url = new StringBuilder();
            switch (reg)
            {
                case AOnlineRegion.AKR:
                    _url.Append("http://www.advantech.co.kr");
                    break;
                default:
                    _url.Append("http://www.advantech.com");
                    break;

            }

            _url.Append("/products");
            _url.Append("/" + this.Parent_Category[0].Category_ID);
            _url.Append("/" + this.Model_Name);
            _url.Append("/mod_" + this.Model_ID);

            return _url.ToString();

        }


        private List<Model_Lang> _Model_Langs = new List<Model_Lang>();
        public List<Model_Lang> Model_Langs
        {
            get
            {
                if (this._Model_Langs.Count == 0)
                {
                    this.LoadModel_LangInformation();
                }
                return _Model_Langs;
            }
            //set { this._Model_Langs = Model_Langs; }
        }

        private List<Literature> _Literatues = new List<Literature>();
        public List<Literature> Literatures
        {
            get
            {
                if (this._Literatues.Count == 0)
                {
                    this.LoadLiteraturesInformation();
                }
                return _Literatues;
            }
            //protected set { this._Literatues = Literatures; }
        }

        private List<Feature> _Features = new List<Feature>();
        public List<Feature> Features
        {
            get
            {
                if (this._Features.Count == 0)
                {
                    this.LoadFeaturesInformation();
                }
                return _Features;
            }
            //protected set { this._Features = Features; }
        }

        protected List<Feature> AllFeatures
        {
            get;
            set;
        }

        private List<ProductSpec> _ModelSpecs = new List<ProductSpec>();
        public List<ProductSpec> ModelSpecs
        {
            get
            {
                if (this._ModelSpecs.Count == 0)
                {
                    this.LoadModelSpecsInformation();
                }
                return _ModelSpecs;
            }
            //protected set { this._ModelSpecs = ModelSpecs; }
        }


        private List<Part> _Parts = new List<Part>();
        public List<Part> Parts
        {
            get
            {
                if (this._Parts.Count == 0)
                {
                    this.LoadPartsInformation();
                }
                return _Parts;
            }
            //protected set { this._Parts = Parts; }
        }

        public Model()
        { }

        public Model(string modelname)
        {
            this.Model_Name = modelname;
        }

        private string GetModel_ParentCategorySQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" Select ch.parent_category_id1,ch.category_name1 ");
            sql.Append(" ,ch.parent_category_id2,ch.category_name2 ");
            sql.Append(" ,ch.parent_category_id3,ch.category_name3 ");
            sql.Append(" ,ch.parent_category_id4,ch.category_name4 ");
            sql.Append(" ,ch.parent_category_id5,ch.category_name5 ");
            sql.Append(" ,ch.parent_category_id6,ch.category_name6 ");
            sql.Append(" From CATEGORY_HIERARCHY ch  ");
            sql.Append(" Where ch.model_no=@model_name ");
            return sql.ToString();
        }


        private string GetModelSQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("  Select isnull(m.MODEL_ID , '') as MODEL_ID , isnull(m.MODEL_NAME , '') as MODEL_NAME, isnull(m.DISPLAY_NAME , '') as DISPLAY_NAME , ");
            sql.Append(" isnull(m.MODEL_DESC , '') as MODEL_DESC , isnull(m.EXTENDED_DESC, '') as EXTENDED_DESC, ");
            sql.Append(" isnull(m.MODEL_TYPE, '') as MODEL_TYPE From MODEL m Where m.model_name = @model_name ");
            return sql.ToString();
        }

        private string GetModel_LangSQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" Select ISNULL(mlang.LANGID , '') as LANGID, ISNULL(mlang.DISPLAY_NAME , '') AS DISPLAY_NAME , ISNULL(mlang.MODEL_DESC , '') AS MODEL_DESC , ISNULL(mlang.EXTENDED_DESC , '') as EXTENDED_DESC ");
            sql.Append(" From MODEL_LANG mlang  ");
            sql.Append(" Where mlang.model_name=@model_name ");
            return sql.ToString();
        }

        private string GetLiteraturesSQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("  Select ISNULL(l.LITERATURE_ID,'') as LITERATURE_ID , ISNULL(l.LIT_TYPE, '') as LIT_TYPE, ISNULL(l.LIT_NAME,'') as LIT_NAME ");
            sql.Append(" ,ISNULL(l.FILE_NAME, '') as FILE_NAME, ISNULL(l.LAST_UPDATED_BY, '') as LAST_UPDATED_BY, ISNULL(l.FILE_EXT, '') as FILE_EXT, ISNULL(l.LANG, '') as LANG ");
            sql.Append(" From Literature l inner join model_lit ml on l.literature_id=ml.literature_id  ");
            sql.Append(" Where ml.model_name=@model_name ");
            return sql.ToString();
        }

        private string GetModelSpecsSQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Clear();
            sql.Append(" Select isnull(AttrCatID,'') as AttrCatID, isnull(AttrCatName,'') as AttrCatName, isnull(AttrID,'') as AttrID, isnull(AttrName,'') as AttrName, ");
            sql.Append(" isnull(AttrValueID, '') as AttrValueID, isnull(AttrValueName, '') as AttrValueName, isnull(DataSheet_Sequence, '') as DataSheet_Sequence, isnull(Is_Filter, '') as Is_Filter  ");
            sql.Append(" From V_SPEC_V3  ");
            sql.Append(" Where ProductNo=@model_name and lang='ENU' and ItemType='Model'");
            sql.Append(" order by DataSheet_Sequence ");
            return sql.ToString();
        }


        private string GetPartsSQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Clear();
            sql.Append(" SELECT mp.part_no,mp.relation,mp.[status] as PIS_Status,sapprod.[STATUS] as SAP_Status ");
            sql.Append(" FROM model_product mp inner join PIS.dbo.PRODUCT_LOGISTICS_NEW sapprod ");
            sql.Append(" on mp.part_no=sapprod.PART_NO ");
            sql.Append(" where mp.[model_name]=@model_name ");
            sql.Append(" and sapprod.ORG_ID='TW01' ");
            sql.Append(" and mp.[status]='active' ");
            sql.Append(" and mp.relation='product' ");
            sql.Append(" and sapprod.[STATUS] in ('A','N','H','M1') ");
            sql.Append(" group by mp.part_no,mp.relation,mp.[status],sapprod.[STATUS] ");
            sql.Append(" order by mp.part_no ");
            return sql.ToString();
        }


        private string GetFeaturesSQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" Select isnull(mf.id,'') as FEATURE_ID, isnull(mf.FEATURE_SEQ,'') as FEATURE_SEQ, isnull(mf.FEATURE_DESC,'') as FEATURE_DESC, isnull(mf.LANG_ID,'') as LANG_ID ");
            sql.Append(" From MODEL_FEATURE mf  ");
            sql.Append(" Where mf.model_name=@model_name ");
            sql.Append(" Order by mf.LANG_ID,mf.FEATURE_SEQ ");
            return sql.ToString();
        }

        private string GetPublishSQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from Publish ");
            sql.Append("Where MODEL_NAME = @model_name ");
            return sql.ToString();
        }

        private void WriteToAttributeModel(DataTable dt)
        {
            foreach (DataRow _row in dt.Rows)
            {
                this.Model_ID = (string)_row["MODEL_ID"];
                this.Display_Name = (string)_row["DISPLAY_NAME"];
                this.Description = (string)_row["MODEL_DESC"];
                this.Introduction = (string)_row["EXTENDED_DESC"];
                switch ((string)_row["MODEL_TYPE"])
                {
                    case "standard_model":
                        this.Model_Type = ModelType.Model;
                        break;
                    case "bundle":
                        this.Model_Type = ModelType.Bundle;
                        break;
                    case "pre-config":
                        this.Model_Type = ModelType.PreconfigSystem;
                        break;
                }
            }
        }

        private void WriteToAttributeModel_ParentCategory(DataTable dt)
        {
            foreach (DataRow _row in dt.Rows)
            {

                if (_row["parent_category_id1"] is System.DBNull || (string)_row["parent_category_id1"] == "root")
                {
                    continue;
                }
                Category _category1 = new Category();
                _category1.Category_ID = (string)_row["parent_category_id1"];
                _category1.Display_Name = (string)_row["category_name1"];
                this._Parent_Category.Add(_category1);
                //this._Parent_Category = _category1;

                if (_row["parent_category_id2"] is System.DBNull || (string)_row["parent_category_id2"] == "root")
                {
                    continue;
                }
                Category _category2 = new Category();
                _category2.Category_ID = (string)_row["parent_category_id2"];
                _category2.Display_Name = (string)_row["category_name2"];
                _category1.Parent_Category = _category2;


                if (_row["parent_category_id3"] is System.DBNull || (string)_row["parent_category_id3"] == "root")
                {
                    continue;
                }
                Category _category3 = new Category();
                _category3.Category_ID = (string)_row["parent_category_id3"];
                _category3.Display_Name = (string)_row["category_name3"];
                _category2.Parent_Category = _category3;

                if (_row["parent_category_id4"] is System.DBNull || (string)_row["parent_category_id4"] == "root")
                {
                    continue;
                }
                Category _category4 = new Category();
                _category4.Category_ID = (string)_row["parent_category_id4"];
                _category4.Display_Name = (string)_row["category_name4"];
                _category3.Parent_Category = _category4;

                if (_row["parent_category_id5"] is System.DBNull || (string)_row["parent_category_id5"] == "root")
                {
                    continue;
                }
                Category _category5 = new Category();
                _category5.Category_ID = (string)_row["parent_category_id5"];
                _category5.Display_Name = (string)_row["category_name5"];
                _category4.Parent_Category = _category5;

                if (_row["parent_category_id6"] is System.DBNull || (string)_row["parent_category_id6"] == "root")
                {
                    continue;
                }
                Category _category6 = new Category();
                _category6.Category_ID = (string)_row["parent_category_id6"];
                _category6.Display_Name = (string)_row["category_name6"];
                _category5.Parent_Category = _category6;
            }
        }

        private void WriteToAttributeModel_Lang(DataTable dt)
        {
            foreach (DataRow _row in dt.Rows)
            {

                Model_Lang _model_lang = new Model_Lang();

                _model_lang.Description = (string)(_row["MODEL_DESC"] + "");
                _model_lang.Introduction = (string)(_row["EXTENDED_DESC"] + "");
                _model_lang.Language = MyExtension.GetLanguageCodeByPISLanguageID((string)_row["LANGID"]);
                this._Model_Langs.Add(_model_lang);
            }
        }

        private void WriteToAttributeLiteratures(DataTable dt)
        {
            PISLiteratureType _pislittype;
            foreach (DataRow _row in dt.Rows)
            {

                switch ((string)_row["LIT_TYPE"])
                {
                    case "Product - Photo(Main)":
                        _pislittype = PISLiteratureType.Product_Photo_Main;
                        break;
                    case "Product - Photo(Main) - Thumbnail":
                        _pislittype = PISLiteratureType.Product_Photo_Main_Thumbnail;
                        break;
                    case "Product - Photo(B)":
                        _pislittype = PISLiteratureType.Product_Photo_Big;
                        break;
                    case "Product - Photo(S)":
                        _pislittype = PISLiteratureType.Product_Photo_Small;
                        break;
                    case "Product - Datasheet":
                        _pislittype = PISLiteratureType.Product_Datasheet;
                        break;
                    default:
                        continue;
                }

                Literature _literaturel = new Literature();
                _literaturel.LIT_TYPE = _pislittype;
                _literaturel.LITERATURE_ID = (string)_row["LITERATURE_ID"];
                _literaturel.HTTP_URL = "http://downloadt.advantech.com/download/downloadlit.aspx?LIT_ID=" + (string)_row["LITERATURE_ID"];
                _literaturel.LIT_NAME = (string)_row["LIT_NAME"];
                _literaturel.FILE_NAME = (string)_row["FILE_NAME"];
                _literaturel.LAST_UPDATED_BY = (string)_row["LAST_UPDATED_BY"];
                _literaturel.FILE_EXT = (string)_row["FILE_EXT"];
                _literaturel.Language = MyExtension.GetLanguageCodeByPISLanguageID((string)_row["LANG"]);

                this._Literatues.Add(_literaturel);
            }
        }


        private void WriteToAttributeFeatures(DataTable dt)
        {
            foreach (DataRow _row in dt.Rows)
            {
                Feature _feature = new Feature();
                _feature.FEATURE_ID = (int)_row["FEATURE_ID"];
                _feature.FEATURE_SEQ = (long)_row["FEATURE_SEQ"];
                _feature.FEATURE_DESC = (string)_row["FEATURE_DESC"];
                _feature.Language = MyExtension.GetLanguageCodeByPISLanguageID((string)_row["LANG_ID"]);
                if (this.AllFeatures == null) { this.AllFeatures = new List<Feature>(); }
                this.AllFeatures.Add(_feature);
            }
            this._Features = this.GetFeaturesByDefault();
        }



        private void WriteToAttributeModelSpecs(DataTable dt)
        {

            foreach (DataRow _row in dt.Rows)
            {
                ProductSpec _spec = new ProductSpec();
                _spec.SpecCategoryID = (int)_row["AttrCatID"];
                _spec.SpecCategoryName = (string)_row["AttrCatName"];
                _spec.SpecItemID = (int)_row["AttrID"];
                _spec.SpecItemName = (string)_row["AttrName"];
                _spec.SpecItemValueID = (int)_row["AttrValueID"];
                _spec.SpecItemValueName = (string)_row["AttrValueName"];
                _spec.Sequence = (int)_row["DataSheet_Sequence"];
                _spec.IsFilterOption = (bool)_row["Is_Filter"];
                this._ModelSpecs.Add(_spec);
            }
        }

        private void WriteToAttributeParts(DataTable dt)
        {

            foreach (DataRow _row in dt.Rows)
            {
                Part _Part = new Part();
                _Part.PartNumber = (string)_row["Part_No"];
                _Part.LoadPartSpecsInformation();
                _Part.LoadPartDescriptionInformation();
                this._Parts.Add(_Part);
            }
        }

        private void WriteToAttributePublish(DataTable dt)
        {
            foreach (DataRow _row in dt.Rows)
            {
                this.Publish_status = (string)_row["STATUS"];
            }
        }

        /// <summary>
        /// Get model name by model ID
        /// </summary>
        /// <param name="model_id"></param>
        /// <returns></returns>
        internal string GetModelNameByModelID(string model_id)
        {
            if (string.IsNullOrEmpty(model_id)) { return ""; }

            StringBuilder sql = new StringBuilder();
            sql.Append(" Select model_name,model_id ");
            sql.Append(" From Model m  ");
            sql.Append(" Where m.model_id=@model_id ");

            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(sql.ToString(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_id";
            dp.Value = model_id;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                return (string)dt.Rows[0]["model_name"];
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category_id"></param>
        /// <param name="model_id"></param>
        /// <returns></returns>
        internal string GetModelNameByCategory_ModelID(string category_id, string model_id)
        {
            if (string.IsNullOrEmpty(model_id)) { return ""; }

            StringBuilder sql = new StringBuilder();
            sql.Append(" Select m.model_name,m.model_id ");
            sql.Append(" From Category_Model c inner join Model m on c.model_name=m.model_name ");
            sql.Append(" Where c.category_id=@category_id ");
            sql.Append(" And m.model_id=@model_id ");

            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null; IDbDataParameter dp = null; DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(sql.ToString(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "category_id";
            dp.Value = category_id;
            cmd.Parameters.Add(dp);

            dp = cmd.CreateParameter();
            dp.ParameterName = "model_id";
            dp.Value = model_id;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                return (string)dt.Rows[0]["model_name"];
            }
            return "";
        }

        /// <summary>
        /// Load only model master information
        /// </summary>
        /// <param name="modelname"></param>
        internal void LoadBasicModelInformation()
        {
            if (string.IsNullOrEmpty(this.Model_Name)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(this.GetModelSQL(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_name";
            dp.Value = this.Model_Name;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);

            this.WriteToAttributeModel(DsModel.Tables[0]);

        }

        /// <summary>
        /// Load model's parent categories
        /// </summary>
        private void LoadModel_ParentCategory()
        {
            if (string.IsNullOrEmpty(this.Model_Name)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(this.GetModel_ParentCategorySQL(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_name";
            dp.Value = this.Model_Name;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);
            this.WriteToAttributeModel_ParentCategory(DsModel.Tables[0]);
        }

        /// <summary>
        /// Load model master information in other language version
        /// </summary>
        private void LoadModel_LangInformation()
        {
            if (string.IsNullOrEmpty(this.Model_Name)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(this.GetModel_LangSQL(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_name";
            dp.Value = this.Model_Name;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);

            this.WriteToAttributeModel_Lang(DsModel.Tables[0]);
        }

        /// <summary>
        /// Load model's literatures
        /// </summary>
        private void LoadLiteraturesInformation()
        {
            if (string.IsNullOrEmpty(this.Model_Name)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(this.GetLiteraturesSQL(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_name";
            dp.Value = this.Model_Name;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);

            this.WriteToAttributeLiteratures(DsModel.Tables[0]);
        }

        /// <summary>
        /// Load model's specs
        /// </summary>
        private void LoadModelSpecsInformation()
        {
            if (string.IsNullOrEmpty(this.Model_Name)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection(); IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(this.GetModelSpecsSQL(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_name";
            dp.Value = this.Model_Name;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);

            this.WriteToAttributeModelSpecs(DsModel.Tables[0]);

        }

        /// <summary>
        /// Load model's parts
        /// </summary>
        private void LoadPartsInformation()
        {

            if (string.IsNullOrEmpty(this.Model_Name)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(this.GetPartsSQL(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_name";
            dp.Value = this.Model_Name;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);

            this.WriteToAttributeParts(DsModel.Tables[0]);
        }

        /// <summary>
        /// Load models features
        /// </summary>
        private void LoadFeaturesInformation()
        {
            if (string.IsNullOrEmpty(this.Model_Name)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(this.GetFeaturesSQL(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_name";
            dp.Value = this.Model_Name;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);

            this.WriteToAttributeFeatures(DsModel.Tables[0]);
        }


        /// <summary>
        /// Load complete model information
        /// </summary>
        /// <param name="modelname"></param>
        internal void LoadCompleteModelInformation()
        {
            if (string.IsNullOrEmpty(this.Model_Name)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;
            StringBuilder sql = new StringBuilder();

            //Frank 其它語系待處理
            sql.Clear();
            sql.AppendLine(this.GetModelSQL());

            //Model in different Langs
            sql.AppendLine(";");
            sql.AppendLine(this.GetModel_LangSQL());

            //Model literatures
            sql.AppendLine(";");
            sql.AppendLine(this.GetLiteraturesSQL());

            //Model Features
            sql.AppendLine(";");
            sql.AppendLine(this.GetFeaturesSQL());

            //Model Specs
            sql.AppendLine(";");
            sql.AppendLine(this.GetModelSpecsSQL());

            cmd = DatabaceFactory.CreateCommand(sql.ToString(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_name";
            dp.Value = this.Model_Name;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);

            //=====================Model=========================================
            if (DsModel.Tables[0] != null)
                this.WriteToAttributeModel(DsModel.Tables[0]);

            //=====================Model_Langs=========================================
            //Frank 其它語系待處理
            if (DsModel.Tables[1] != null)
                this.WriteToAttributeModel_Lang(DsModel.Tables[1]);

            //=====================Literatures=====================================
            if (DsModel.Tables[2] != null)
                this.WriteToAttributeLiteratures(DsModel.Tables[2]);

            //======================Features==========================================
            if (DsModel.Tables[3] != null)
                this.WriteToAttributeFeatures(DsModel.Tables[3]);

            //======================Features==========================================
            if (DsModel.Tables[4] != null)
                this.WriteToAttributeModelSpecs(DsModel.Tables[4]);

        }

        /// <summary>
        /// Get model's default features
        /// </summary>
        /// <returns></returns>
        List<Feature> GetFeaturesByDefault()
        {
            return GetFeaturesByLanguage(LanguageCode.en_us);
        }

        /// <summary>
        /// Get model's features in specify language
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public List<Feature> GetFeaturesByLanguage(LanguageCode lang)
        {
            if (this.AllFeatures == null) { return null; }
            List<Feature> _returnval = this.AllFeatures.OfType<Feature>().Where(o => o.Language == lang).ToList();
            if (_returnval.Count == 0)
            {
                return this.AllFeatures.OfType<Feature>().Where(o => o.Language == LanguageCode.en_us).ToList();
            }
            return _returnval;
        }

        /// <summary>
        /// Get model's description in specify language
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public string GetDescriptionByLanguage(LanguageCode lang)
        {
            Model_Lang _model_lang = GetModel_LangByLanguage(lang);
            if (_model_lang != null)
            {
                return _model_lang.Description;
            }
            return "";
        }

        /// <summary>
        /// Get model's introduction in specify language
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public string GetIntroductionByLanguage(LanguageCode lang)
        {
            Model_Lang _model_lang = GetModel_LangByLanguage(lang);
            if (_model_lang != null)
            {
                return _model_lang.Introduction;
            }
            return "";
        }

        private Model_Lang GetModel_LangByLanguage(LanguageCode lang)
        {
            List<Model_Lang> _returnval = this.Model_Langs.OfType<Model_Lang>().Where(o => o.Language == lang).ToList();
            if (_returnval.Count == 0)
            {
                _returnval = this.Model_Langs.OfType<Model_Lang>().Where(o => o.Language == LanguageCode.en_us).ToList();
            }
            return _returnval[0];
        }

        /// <summary>
        /// Get model's main pictures
        /// </summary>
        /// <returns></returns>
        public List<Literature> GetMainPictures()
        {
            List<Literature> _returnval = this.Literatures.OfType<Literature>().Where(
                o => o.LIT_TYPE == PISLiteratureType.Product_Photo_Main).ToList();

            return _returnval;
        }

        /// <summary>
        /// Get model's big pictures
        /// </summary>
        /// <returns></returns>
        public List<Literature> GetBigPictures()
        {
            List<Literature> _returnval = this.Literatures.OfType<Literature>().Where(
                o => o.LIT_TYPE == PISLiteratureType.Product_Photo_Big).ToList();

            return _returnval;
        }

        /// <summary>
        /// Get model's small pictures
        /// </summary>
        /// <returns></returns>
        public List<Literature> GetSmallPictures()
        {
            List<Literature> _returnval = this.Literatures.OfType<Literature>().Where(
                o => o.LIT_TYPE == PISLiteratureType.Product_Photo_Small).ToList();

            return _returnval;
        }

        /// <summary>
        /// Load only model master information
        /// </summary>
        /// <param name="modelname"></param>
        internal void LoadModelPublish()
        {
            if (string.IsNullOrEmpty(this.Model_Name)) { return; }
            IDbConnection cnn = DatabaceFactory.GetPISConnection();

            IDbCommand cmd = null;
            IDbDataParameter dp = null;
            DbDataAdapter da = null;

            cmd = DatabaceFactory.CreateCommand(this.GetPublishSQL(), DatabaseType.SQLServer, cnn);
            dp = cmd.CreateParameter();
            dp.ParameterName = "model_name";
            dp.Value = this.Model_Name;
            cmd.Parameters.Add(dp);

            da = DatabaceFactory.CreateAdapter(cmd, DatabaseType.SQLServer);
            DataSet DsModel = new DataSet();
            da.Fill(DsModel);

            this.WriteToAttributePublish(DsModel.Tables[0]);

        }
    }
}
