using Advantech.Myadvantech.DataAccess;
using Advantech.Myadvantech.DataAccess.bbeStoreFreightAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.Business
{
    /// <summary>
    /// Calculate freight
    /// </summary>
    public class FreightCalculateBusinessLogic
    {
        /// <summary>
        /// Use eStore B+B function to calcaulate freight
        /// </summary>
        /// <returns></returns>
        public static Tuple<bool,Response> CalculateBBFreight(SAP_DIMCOMPANY _Soldto, SAP_DIMCOMPANY _Shipto, SAP_DIMCOMPANY _Billto, List<cart_DETAIL_V2> _CartItems)
        {
            shippingrate target = new shippingrate();
            target.Timeout = 30000;
            //target.Url = "http://buy.advantech.com/services/shippingrate.asmx"; AUS eStore URL

            DataAccess.bbeStoreFreightAPI.Order order = new DataAccess.bbeStoreFreightAPI.Order();
            order.StoreId = "ABB";

            // Shipto settings
            Address shipto = new Address();            
            if (_Shipto != null)
            {
                shipto.Countrycode = _Shipto.COUNTRY;
                shipto.Zipcode = _Shipto.ZIP_CODE;
                shipto.StateCode = _Shipto.REGION_CODE;
                order.Shipto = shipto;
            }

            // Bill to settings
            Address billto = new Address();            
            if (_Billto != null)
            {
                billto.Countrycode = _Billto.COUNTRY;
                billto.Zipcode = _Billto.ZIP_CODE;
                billto.StateCode = _Billto.REGION_CODE;
                order.Billto = billto;
            }

            // Loose Items settings
            List<Item> items = new List<Item>();
            List<cart_DETAIL_V2> LooseItems = _CartItems.Where(d => d.otype == 0).ToList();
            if (LooseItems.Count > 0)
            {
                foreach (cart_DETAIL_V2 LooseItem in LooseItems)
                {
                    items.Add(new Item()
                    {
                        ProductID = LooseItem.Part_No,
                        Qty = (int)LooseItem.Qty
                    });
                }
            }

            // System Items settings
            List<ConfigSystem> systems = new List<ConfigSystem>();
            List<cart_DETAIL_V2> ParentItems = _CartItems.Where(d => d.otype == -1).ToList();
            foreach (cart_DETAIL_V2 ParentItem in ParentItems)
            {
                int _sys1Qty = 1;
                ConfigSystem _sys1 = new ConfigSystem();
                _sys1.Qty = (int)ParentItem.Qty;
                _sys1.ProductID = ParentItem.Part_No;
                _sys1Qty = _sys1.Qty;

                List<cart_DETAIL_V2> ChildItems = _CartItems.Where(d => d.otype == 1 && d.higherLevel == ParentItem.Line_No).ToList();               
                List<Item> _ds = new List<Item>();
                foreach (cart_DETAIL_V2 ChildItem in ChildItems)
                {
                    _ds.Add(new Item()
                    {
                        ProductID = ChildItem.Part_No,
                        Qty = (Int32)Math.Ceiling((double)(Convert.ToDouble(ChildItem.Qty / (double)_sys1Qty)))
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
                return new Tuple<bool, Response>(true, actual);
            else
                return new Tuple<bool, Response>(false, actual);
        }



        /// <summary>
        /// Use eStore B+B function to calcaulate freight(simple version for ajax)
        /// </summary>
        /// <returns></returns>
        public static ShippingResult CalculateBBFreight(string shipToCountry, string shipToZipCode, string shipToState,  string cartId, WebSource source = WebSource.Myadvantech)
        {
            ShippingResult shippingResult = new ShippingResult();
            List<ShippingMethod> shippingmethods = new List<ShippingMethod>();
            Response response;
            try
            {
                List<FreightOption> freightOptions = Advantech.Myadvantech.DataAccess.MyAdvantechDAL.GetAllFreightOptions();
                foreach (var option in freightOptions)
                {
                    ShippingMethod method = new ShippingMethod();
                    method.MethodName = option.SAPCode + ": " + option.Description;
                    method.MethodValue = option.CarrierCode + ": " + option.Description;
                    method.DisplayShippingCost = "N/A";
                    method.ErrorMessage = "";
                    if (option.EStoreServiceName != null)
                        method.EstoreServiceName = option.EStoreServiceName;
                    shippingmethods.Add(method);
                }


                try
                {
                    shippingrate target = new shippingrate();
                    target.Timeout = 30000;
                    //target.Url = "http://buy.advantech.com/services/shippingrate.asmx"; AUS eStore URL

                    DataAccess.bbeStoreFreightAPI.Order order = new DataAccess.bbeStoreFreightAPI.Order();
                    order.StoreId = "ABB";


                    // Shipto settings
                    Address shipto = new Address();
                    shipto.Countrycode = shipToCountry;
                    shipto.Zipcode = shipToZipCode;
                    shipto.StateCode = shipToState;
                    order.Shipto = shipto;
                    order.Billto = shipto;

                    if (source == WebSource.Myadvantech)
                    {
                        List<Advantech.Myadvantech.DataAccess.cart_DETAIL_V2> cartItems = Advantech.Myadvantech.DataAccess.CartDetailHelper.GetCartDetailByID(cartId);

                        // Loose Items settings
                        List<Item> items = new List<Item>();
                        List<cart_DETAIL_V2> LooseItems = cartItems.Where(d => d.otype == 0).ToList();
                        if (LooseItems.Count > 0)
                        {
                            foreach (cart_DETAIL_V2 LooseItem in LooseItems)
                            {
                                items.Add(new Item()
                                {
                                    ProductID = LooseItem.Part_No,
                                    Qty = (int)LooseItem.Qty
                                });
                            }
                        }

                        // System Items settings
                        List<ConfigSystem> systems = new List<ConfigSystem>();
                        List<cart_DETAIL_V2> ParentItems = cartItems.Where(d => d.otype == -1).ToList();
                        foreach (cart_DETAIL_V2 ParentItem in ParentItems)
                        {
                            int _sys1Qty = 1;
                            ConfigSystem _sys1 = new ConfigSystem();
                            _sys1.Qty = (int)ParentItem.Qty;
                            _sys1.ProductID = ParentItem.Part_No;
                            _sys1Qty = _sys1.Qty;

                            List<cart_DETAIL_V2> ChildItems = cartItems.Where(d => d.otype == 1 && d.higherLevel == ParentItem.Line_No).ToList();
                            List<Item> _ds = new List<Item>();
                            foreach (cart_DETAIL_V2 ChildItem in ChildItems)
                            {
                                _ds.Add(new Item()
                                {
                                    ProductID = ChildItem.Part_No,
                                    Qty = (Int32)Math.Ceiling((double)(Convert.ToDouble(ChildItem.Qty / (double)_sys1Qty)))
                                });
                            }

                            _sys1.Details = _ds.ToArray();
                            systems.Add(_sys1);
                        }
                        order.Items = items.ToArray();
                        order.Systems = systems.ToArray();
                    }
                    else if(source == WebSource.eQuotation)
                    {
                        var quotationMaster = QuoteBusinessLogic.GetQuotationMaster(cartId);
                        if (quotationMaster != null)
                        {
                            List<QuotationDetail> _QuoteDetails = quotationMaster.QuotationDetail;

                            // Loose Items settings
                            List<Item> items = new List<Item>();
                            List<QuotationDetail> LooseItems = _QuoteDetails.Where(q => q.ItemType == (int)LineItemType.LooseItem).ToList();
                            if (LooseItems.Count > 0)
                            {
                                foreach (QuotationDetail LooseItem in LooseItems)
                                {
                                    items.Add(new Item()
                                    {
                                        ProductID = LooseItem.partNo,
                                        Qty = (int)LooseItem.qty
                                    });
                                }
                            }

                            // System Items settings
                            List<ConfigSystem> systems = new List<ConfigSystem>();
                            List<QuotationDetail> ParentItems = _QuoteDetails.Where(q => q.ItemType == (int)LineItemType.BTOSParent).ToList();
                            foreach (QuotationDetail ParentItem in ParentItems)
                            {
                                int _sys1Qty = 1;
                                ConfigSystem _sys1 = new ConfigSystem();
                                _sys1.Qty = (int)ParentItem.qty;
                                _sys1.ProductID = ParentItem.partNo;
                                _sys1Qty = _sys1.Qty;

                                List<QuotationDetail> ChildItems = _QuoteDetails.Where(q => q.ItemType == (int)LineItemType.BTOSChild && q.HigherLevel == ParentItem.line_No).ToList();
                                List<Item> _ds = new List<Item>();
                                foreach (QuotationDetail ChildItem in ChildItems)
                                {
                                    _ds.Add(new Item()
                                    {
                                        ProductID = ChildItem.partNo,
                                        Qty = (Int32)Math.Ceiling((double)(Convert.ToDouble(ChildItem.qty / (double)_sys1Qty)))
                                    });
                                }

                                _sys1.Details = _ds.ToArray();
                                systems.Add(_sys1);
                            }
                            order.Items = items.ToArray();
                            order.Systems = systems.ToArray();
                        }
                    }



                    
                    response = target.getShippingRate(order);

                }
                catch (Exception ex)
                {
                    throw ex;
                }



                if (response != null)
                {
                    if (response.ShippingRates != null)
                    {
                        var normalShippingRatesList = new List<ShippingRate>();
                        foreach (var item in response.ShippingRates)
                        {
                            foreach (var method in shippingmethods)
                            {
                                if (method.EstoreServiceName == item.Nmae)
                                {
                                    method.ShippingCost = item.Rate;
                                    method.DisplayShippingCost = item.Rate.ToString();
                                    method.ErrorMessage = string.IsNullOrEmpty(item.ErrorMessage)? "" : item.ErrorMessage;

                                    //配對成功的就移除
                                    normalShippingRatesList.Add(item);
                                }
                            }

                        }
                        var unnormalShippingRatesList = response.ShippingRates.Where(p => !normalShippingRatesList.Any(p2 => p2.Nmae == p.Nmae));
                        if (unnormalShippingRatesList.Any())
                            shippingResult.Message = string.Join("<br/>", unnormalShippingRatesList.Select(s => s.Nmae).ToList());

                    }

                    if (response.Boxex[0] != null)
                    {
                        shippingResult.Weight = (double)Decimal.Round(response.Boxex[0].Weight, 2);
                    }





                    shippingResult.Status = response.Status;
                    //if (response.DetailMessages != null)
                    //    shippingResult.DetailMessage += string.Join(",", response.DetailMessages);
                }
                else
                {
                    shippingResult.Message = "No Response. Please select one freight option and manually enter cost.";
                    shippingResult.Status = "0";
                }
                
            }
            catch(Exception ex)
            {
                shippingResult.Message = "Exception occurs. Please contact Myadvantech team.";
                shippingResult.DetailMessage= ex.Message;
                shippingResult.Status = "0";
            }

            shippingResult.ShippingMethods = shippingmethods;


            return shippingResult;

        }



        /// <summary>
        /// Use eStore B+B function to calcaulate freight
        /// </summary>
        /// <returns></returns>
        public static Tuple<bool, Response> CalculateBBFreightByQuotationDetail(EQPARTNER _Shipto, EQPARTNER _Billto, List<QuotationDetail> _QuoteDetails)
        {
            shippingrate target = new shippingrate();
            target.Timeout = 30000;
            //target.Url = "http://buy.advantech.com/services/shippingrate.asmx"; AUS eStore URL

            DataAccess.bbeStoreFreightAPI.Order order = new DataAccess.bbeStoreFreightAPI.Order();
            order.StoreId = "ABB";

            // Shipto settings
            Address shipto = new Address();
            if (_Shipto != null)
            {
                shipto.Countrycode = _Shipto.COUNTRY;
                shipto.Zipcode = _Shipto.ZIPCODE;
                shipto.StateCode = _Shipto.STATE;
                order.Shipto = shipto;
            }

            // Bill to settings
            Address billto = new Address();
            if (_Billto != null)
            {
                billto.Countrycode = _Billto.COUNTRY;
                billto.Zipcode = _Billto.ZIPCODE;
                billto.StateCode = _Billto.STATE;
                order.Billto = billto;
            }

            // Loose Items settings
            List<Item> items = new List<Item>();
            List<QuotationDetail> LooseItems = _QuoteDetails.Where(q=> q.ItemType == (int)LineItemType.LooseItem).ToList();
            if (LooseItems.Count > 0)
            {
                foreach (QuotationDetail LooseItem in LooseItems)
                {
                    items.Add(new Item()
                    {
                        ProductID = LooseItem.partNo,
                        Qty = (int)LooseItem.qty
                    });
                }
            }

            // System Items settings
            List<ConfigSystem> systems = new List<ConfigSystem>();
            List<QuotationDetail> ParentItems = _QuoteDetails.Where(q => q.ItemType == (int)LineItemType.BTOSParent).ToList();
            foreach (QuotationDetail ParentItem in ParentItems)
            {
                int _sys1Qty = 1;
                ConfigSystem _sys1 = new ConfigSystem();
                _sys1.Qty = (int)ParentItem.qty;
                _sys1.ProductID = ParentItem.partNo;
                _sys1Qty = _sys1.Qty;

                List<QuotationDetail> ChildItems = _QuoteDetails.Where(q => q.ItemType == (int)LineItemType.BTOSChild &&q.HigherLevel == ParentItem.line_No).ToList();
                List<Item> _ds = new List<Item>();
                foreach (QuotationDetail ChildItem in ChildItems)
                {
                    _ds.Add(new Item()
                    {
                        ProductID = ChildItem.partNo,
                        Qty = (Int32)Math.Ceiling((double)(Convert.ToDouble(ChildItem.qty / (double)_sys1Qty)))
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
                return new Tuple<bool, Response>(true, actual);
            else
                return new Tuple<bool, Response>(false, actual);
        }
    }

    public class ShippingMethod
    {
        public string MethodName { get; set; }
        public string MethodValue { get; set; }
        public float ShippingCost { get; set; }
        public string DisplayShippingCost { get; set; }
        public string EstoreServiceName { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class ShippingResult
    {
        public string Status { get; set; }
        public double Weight { get; set; }

        public string Message { get; set; }

        public string DetailMessage { get; set; }

        public List<ShippingMethod> ShippingMethods { get; set; }
    }


}
