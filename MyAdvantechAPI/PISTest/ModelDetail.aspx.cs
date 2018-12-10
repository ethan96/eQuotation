using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Advantech.PIS.Business;
using Advantech.Myadvantech.DataAccess;

namespace PISTest
{
    public partial class ModelDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //LanguageCode     Enum.TryParse<LanguageCode>("zh-cn");

            string _modelid = "8DCEE4B7-FBDE-4C5D-9752-ED2F2FBD00BF";// "ADAM-4520";
            string _categoryid = "Data_Acquisition_Modules";
            
            //Model _model = ModelBusinessLogic.GetCompleteModelByModelID(_modelid);
            Model _model = ModelBusinessLogic.GetModelByCategory_ModelID(_categoryid, _modelid);

            List<Category> _ParentCategory = _model.Parent_Category;
            StringBuilder sb = new StringBuilder();
            foreach (Category _category in _ParentCategory)
            {
                Category _ca = _category;
                
                for (int i = 1; i < 7; i++)
                {
                    sb.Insert(0, _ca.Display_Name);
                    if(_ca.Parent_Category != null){
                        sb.Insert(0, " > ");
                        _ca = _ca.Parent_Category;
                    }
                    else
                    {
                        break;
                    }
                }
                sb.Insert(0, "<br />");
            }
            this.CategoryPath.Text = sb.ToString();

            this.ModelName.Text = _model.Model_Name;
            this.ModelDescription.Text = _model.Description;
            this.ModelIntroduction.Text = _model.Introduction;

            //PartBusinessLogic.GetModel(part1.PartNumber);

            //將此料號所屬的model主檔顯示在GridView
            //GridView2.DataSource = _models;
            //GridView2.DataBind();



            //示範將第一個model下的預設features取出並顯示在GridView
            //Model _model = _models[0];
            List<Feature> _defaultFeature = _model.Features;
            GridView3.DataSource = _defaultFeature;
            GridView3.DataBind();

            //示範將第一個model下的簡體中文版本features取出並顯示在GridView
            List<Feature> Simplified = _model.GetFeaturesByLanguage(LanguageCode.zh_cn);
            GVFeature.DataSource = Simplified;
            GVFeature.DataBind();

            //示範將第一個model下的主圖，大圖與小圖取出並顯示在GridView
            List<Literature> _mainpic = _model.GetMainPictures();
            List<Literature> _bigpic = _model.GetBigPictures();
            List<Literature> _smallpic = _model.GetSmallPictures();


            GVMainPIC.DataSource = _mainpic;
            GVMainPIC.DataBind();

            GVBigPIC.DataSource = _bigpic;
            GVBigPIC.DataBind();

            GVSmallPIC.DataSource = _smallpic;
            GVSmallPIC.DataBind();

            GVSpec.DataSource = _model.ModelSpecs;
            GVSpec.DataBind();

            GVParts.DataSource = _model.Parts;
            GVParts.DataBind();

            if (_model.Parts.Count > 0)
            {
                GVFistPartSpec.DataSource = _model.Parts[0].PartSpecs;
                GVFistPartSpec.DataBind();
            }
            //this.SAPDescription.Text = part1.SAPDescription;
            //this.PISDescription.Text = part1.PISDescription;
       }
    }
}