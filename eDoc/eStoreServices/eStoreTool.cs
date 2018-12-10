using eStoreServices.com.advantech.buy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;

namespace eStoreServices
{
    public static class eStoreTool
    {

        public static void getming()
        {


        }
        public static bool GetShippingRate(ref Hashtable Head)
        {

         ///   shippingrate sr = new shippingrate();
            shippingrate target = new shippingrate(); // TODO: Initialize to an appropriate value
            target.Timeout = 10000;   //设置10秒 , 提高反应速度
           // target.Url = "   http://buyqa.advantech.com:9000/services/shippingrate.asmx";
            Order order = new Order(); // TODO: Initialize to an appropriate value
            order.StoreId = Head["StoreId"].ToString();
            Address shipto = new Address();
            shipto.Countrycode = Head["shiptoCountrycode"].ToString();
            shipto.Zipcode = Head["shiptoZipcode"].ToString();
            shipto.StateCode = Head["shiptoStateCode"].ToString();
            Address billto = new Address();
            billto.Countrycode = Head["billtoCountrycode"].ToString();
            billto.StateCode = Head["billtoStateCode"].ToString();
            billto.Zipcode = Head["billtoZipcode"].ToString();
            order.Shipto = shipto;
            order.Billto = billto;
            DataTable dt = (DataTable)Head["items"];

            List<Item> items = new List<Item>();
            DataRow[] drs = dt.Select(string.Format("type={0}", 0));
            if (drs.Length > 0)
            {
                for (int i = 0; i <= drs.Length - 1; i++)
                {
                    items.Add(new Item() { ProductID = drs[i]["partno"].ToString(), Qty = Convert.ToInt32(drs[i]["Qty"]) });
                }
            }

            List<ConfigSystem> systems = new List<ConfigSystem>();
            DataRow[] drsP = dt.Select(string.Format("type={0}", -1));
            for (int i = 0; i <= drsP.Length - 1; i++)
            {
                int _sys1Qty = 1;
                ConfigSystem _sys1 = new ConfigSystem();
                _sys1.Qty = Convert.ToInt32(drsP[i]["Qty"]);
                _sys1.ProductID = drsP[i]["partno"].ToString();
                _sys1Qty = _sys1.Qty;

                DataRow[] drsC = dt.Select(string.Format("type={0} and HigherLevel ={1}", 1, drsP[i]["lineno"]));
                List<Item> _ds = new List<Item>();
                for (int j = 0; j <= drsC.Length - 1; j++)
                {
                    _ds.Add(new Item() { ProductID = drsC[j]["partno"].ToString(),
                                         Qty =(Int32) Math.Ceiling((double)(Convert.ToDouble(drsC[j]["Qty"]) / (double)_sys1Qty))
                    });

                }
                _sys1.Details = _ds.ToArray();
                systems.Add(_sys1);

            }



            order.Items = items.ToArray();
            order.Systems = systems.ToArray();
            Response actual;
         
            actual = target.getShippingRate(order);

            if (actual != null && actual.Status == "1")
            {
                //List <string,string>  = new     List()<string,string>;
                DataTable shipdt = new DataTable();
                shipdt.Columns.Add("name", typeof(string));
                shipdt.Columns.Add("freight", typeof(float));
                shipdt.Columns.Add("TextStr", typeof(string));
                //   Dictionary<string, float> shiplist = new Dictionary<string, float>();
                 
                foreach (ShippingRate _ShippingRate in actual.ShippingRates)
                {
                    DataRow dr = shipdt.NewRow();
                    dr["name"] = _ShippingRate.Nmae;
                    dr["freight"] = _ShippingRate.Rate;
                    dr["TextStr"] = String.Format("{0}  ({1})", dr["name"], dr["freight"]);
                    //   shiplist[_ShippingRate.Nmae] = _ShippingRate.Rate;
                    shipdt.Rows.Add(dr);
                }
                shipdt.AcceptChanges();
                Head["ShippingRates"] = shipdt;
                //foreach (var item in xxx.Keys)
                //{
                //    string name = item;
                //    string value = xxx[item];
                //}
                // ShipList = actual.ShippingRates;
                //this.GridView1.DataSource = actual.ShippingRates;
                //this.GridView1.DataBind();
                //this.rpPackingBox.DataSource = actual.Boxex;
                //this.rpPackingBox.DataBind();
                return true;
            }
            // ShipList = null;
            return false;
        }
    }
}
