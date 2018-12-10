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
    public partial class TestCreateOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Order _order = new Order();
            _order.OrgID = "US01";
            _order.Currency = "USD";
            _order.DistChannel = "30";
            _order.Division = "20";
            _order.District = "D35";
            _order.SetOrderPartnet(new OrderPartner("UAGI5001", "US01", OrderPartnerType.SoldTo));
            _order.SetOrderPartnet(new OrderPartner("UAGI5001", "US01", OrderPartnerType.ShipTo));
            _order.RequireDate = DateTime.Now;
            _order.PODate = new DateTime(2015, 1, 19);

            //_order.AddLooseItem("PCA-6011G2-00A1E");
            //_order.AddLooseItem("ADAM-4520-EE", 2);

            Product _LineItem = new Product();
            _LineItem.PartNumber = "ADAM-4520-EE";
            _LineItem.SellingPrice = 90;
            _LineItem.Quantity = 2;
            _LineItem.PlantID = "USH1";
            _LineItem.RequireDate = DateTime.Now;
            
            _order.AddLooseItem(_LineItem);

            String _errmsg = string.Empty;


            bool _IsSuccess = OrderBusinessLogic.ConvertMyAdvantechCartToSAPOrder("AUSO018986", "US01", ref _errmsg, false, "", false);



            //SAPDAL.CreateOrder(ref _order, DateTime.Now, true, ref _errmsg);

            this.GVPart.DataSource = _order.LineItems;
            this.GVPart.DataBind();

            this.GVConvertOrderResult.DataSource = _order.ConvertToOrderResult;
            this.GVConvertOrderResult.DataBind();

            int a = 3;

        }
    }
}