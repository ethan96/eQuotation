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
    public partial class TestAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //多个Part的调用方法
            List<Part> parts = new List<Part>();
            
            Part part = new Part();
            //part.SoldToErpID = "EFESAI04";
            part.OrgID = "EU10";
            part.PartNumber = "PCI-1220U-AE";
            //part.PlantID = "EUH1";
            //part.Currency = "EUR";
            parts.Add(part);

            Part part2 = new Part();
            // part2.Insert();
            //part2.SoldToErpID = "EFESAI04";
            part2.OrgID = "EU10";
            part2.PartNumber = "ADAM-4502-AE";
            part2.PlantID = "EUH1";
            part2.Currency = "EUR";
            parts.Add(part2);
            Part part3 = new Part();
            //part3.SoldToErpID = "EFESAI04";
            part3.OrgID = "EU10";
            part3.PartNumber = "IPC-100-60SE";
            part3.PlantID = "EUH1";
           // part3.ListPrice
            part3.Currency = "EUR";
            parts.Add(part3);
            string errorMsg = "";
            PartBusinessLogic.GetPrice(parts, ref errorMsg);
            GridView1.DataSource = parts;
            GridView1.DataBind();
            //单一Part的调用方法
            Part part1 = new Part("ADAM-4520-EE");
            //part1.SoldToErpID = "ESTORE";
            part1.OrgID = "CN01";
            //part1.PartNo = "1654002538";
            part1.PlantID = "CNH1";
            //part1.Currency = "EUR";
            PartBusinessLogic.GetPrice(part1, ref errorMsg);
            List<Part> parts1 = new List<Part>();
            parts1.Add(part1);
            DetailsView1.DataSource = parts1;
            DetailsView1.DataBind();

            GridView2.DataSource = part1.Models;
            GridView2.DataBind();

            //part1.g



            Model _model = new Model(part1.Models[0].Model_Name);
            List<Feature> _defaultFeature = _model.Features;
            GridView3.DataSource = _defaultFeature;
            GridView3.DataBind();


            List<Feature> aaa = _model.GetFeaturesByLanguage(LanguageCode.zh_cn);

            GVFeature.DataSource = aaa;
            GVFeature.DataBind();


            List<Literature> _mainpic = _model.GetMainPictures();
            List<Literature> _bigpic = _model.GetBigPictures();
            List<Literature> _smallpic = _model.GetSmallPictures();


            GVMainPIC.DataSource = _mainpic;
            GVMainPIC.DataBind();

            GVBigPIC.DataSource = _bigpic;
            GVBigPIC.DataBind();

            GVSmallPIC.DataSource = _smallpic;
            GVSmallPIC.DataBind();

        }
    }
}