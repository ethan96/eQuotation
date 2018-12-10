using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Advantech.Myadvantech.Business;
using Advantech.Myadvantech.DataAccess;


namespace Ming.Test
{
    public partial class PartToModelSample : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
           
            string _partno = "";
            //_partno = "ADAM-4520I-AE";
            //_partno = "ADAM-5510KW-A1E";
            _partno = "AIMB-762G2-00A1E";

            //透過Business Layer來取得Part物件，並且包含了Part在PIS中所對應的Model
            //而Model的相關資訊(Description, Features, Images, Specs)一開始不會載入，在調用該屬性時再動態載入
            Part part1 = PartBusinessLogic.GetPartWithBasicModelInformation(_partno);

            //透過Business Layer來取得Part物件，
            //並且包含了Part在PIS中所對應的Model與Model完整的資訊，例如：Description, Features, Images, Specs
            //Part part1 = PartBusinessLogic.GetPartWithCompleteModelInformation(_partno);

            //調用Part的規格
            GVSpec.DataSource = part1.PartSpecs;
            GVSpec.DataBind();

            //調用Part在SAP中所維護的描述
            this.SAPDescription.Text = part1.SAPDescription;
            //調用Part在PIS中所維護的描述，空白表示沒有被維護，請取用part1.SAPDescription
            this.PISDescription.Text = part1.PISDescription;

            //從Part中取得此Part所對應的Model實體
            List<Model> _models = part1.Models;

            //將此料號所屬的model主檔顯示在GridView
            GridView2.DataSource = _models;
            GridView2.DataBind();

            //示範將第一個model下的預設features取出並顯示在GridView
            Model _model = _models[0];
            List<Feature> _defaultFeature = _model.Features;
            GridView3.DataSource = _defaultFeature;
            GridView3.DataBind();

            //語系代碼轉換方式
            Advantech.Myadvantech.DataAccess.LanguageCode _lang_zh_cn = LanguageCode.en_us;
            Enum.TryParse<LanguageCode>("zh_cn", true, out _lang_zh_cn);

            //示範將第一個model下的簡體中文版本features取出並顯示在GridView
            List<Feature> Simplified = _model.GetFeaturesByLanguage(_lang_zh_cn);
            GVFeature.DataSource = Simplified;
            GVFeature.DataBind();

            //調用第一個model下的主圖顯示在GridView
            List<Literature> _mainpic = _model.GetMainPictures();
            GVMainPIC.DataSource = _mainpic;
            GVMainPIC.DataBind();

            //調用第一個model下的大圖顯示在GridView
            List<Literature> _bigpic = _model.GetBigPictures();
            GVBigPIC.DataSource = _bigpic;
            GVBigPIC.DataBind();

            //調用第一個model下的小圖取出並顯示在GridView
            List<Literature> _smallpic = _model.GetSmallPictures();
            GVSmallPIC.DataSource = _smallpic;
            GVSmallPIC.DataBind();

        }
    }
}