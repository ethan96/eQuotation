using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Advantech.Myadvantech.DataAccess;

namespace Ming.Test
{
    public partial class TestSimulateOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Order _order = new Order();
            _order.Currency = "USD";
            _order.DistChannel = "30";
            _order.Division = "20";
            _order.SetOrderPartnet(new OrderPartner("ULAI5005", "US01", OrderPartnerType.SoldTo));
            _order.SetOrderPartnet(new OrderPartner("ULAI5005", "US01", OrderPartnerType.ShipTo));
            _order.OrgID = "US01";

            _order.AddLooseItem("PCA-6011G2-00A1E");
            _order.AddLooseItem("ADAM-4520-EE", 2);

            _order.LineItems[0].PlantID = "USH1";
            _order.LineItems[1].PlantID = "USH1";

            String _errmsg = string.Empty;

            SAPDAL.SimulateOrder(ref _order, ref _errmsg);

            this.GVPart.DataSource = _order.LineItems;
            this.GVPart.DataBind();

            int a = 3;

        }
    }
}