using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Advantech.Myadvantech.DataAccess;
using System.Configuration;

namespace Advantech.Myadvantech.Business
{
    public class AuthorizeNetSolution
    {
        public bool supportDirectAccess()
        {
            return true;
        }

        private static string apiLoginId = ConfigurationManager.AppSettings["AuthorizeNet.BB.Login.US"];
        private static string apiTransactionKey = ConfigurationManager.AppSettings["AuthorizeNet.BB.TransactionKey.US"];
        private static string apiLoginIdSandbox = ConfigurationManager.AppSettings["AuthorizeNet.BB.Sanbox.Login.US"];
        private static string apiTransactionKeySandbox = ConfigurationManager.AppSettings["AuthorizeNet.BB.Sanbox.TransactionKey.US"];

        private static void InitEnvironmentAndAccount(bool simulation = false)
        {
            if (simulation)
            {
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
                // define the merchant information (authentication / transaction id)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                {
                    name = apiLoginIdSandbox,
                    ItemElementName = ItemChoiceType.transactionKey,
                    Item = apiTransactionKeySandbox,
                };
            }
            else
            {
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
                // define the merchant information (authentication / transaction id)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                {
                    name = apiLoginId,
                    ItemElementName = ItemChoiceType.transactionKey,
                    Item = apiTransactionKey,
                };
            }


        }

        public static AuthorizeNetResponse AuthorizePaymentAmount(string ordero, decimal amount, string firstName, string lastName, string billToStreet, string city, string state, string billToZipCode, string country,
            string cardNo, string cardExporedDate, string securityCode,  bool simulation = false)
        {
            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();
            try
            {
                InitEnvironmentAndAccount(simulation);
                //if (simulation)
                //{
                //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
                //}
                //else
                //{
                //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
                //}

                //// define the merchant information (authentication / transaction id)
                //ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                //{
                //    name = apiLoginId,
                //    ItemElementName = ItemChoiceType.transactionKey,
                //    Item = apiTransactionKey,
                //};

                var creditCard = new creditCardType();
                creditCard = new creditCardType
                {
                    cardNumber = cardNo,
                    expirationDate = cardExporedDate,
                    cardCode = securityCode
                };


                var billingAddress = new customerAddressType
                {
                    firstName = string.IsNullOrEmpty(firstName) ? "" : firstName,
                    lastName = string.IsNullOrEmpty(lastName) ? "" : lastName,
                    address = billToStreet,
                    city = city,
                    zip = billToZipCode,
                    country = country
                };

                //standard api call to retrieve response
                var paymentType = new paymentType { Item = creditCard };

                var requestOrder = new orderType { invoiceNumber = ordero };

                var transactionRequest = new transactionRequestType
                {
                    transactionType = transactionTypeEnum.authOnlyTransaction.ToString(),    // charge the card
                    amount = amount,
                    payment = paymentType,
                    billTo = billingAddress,
                    order = requestOrder
                };

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .net3.5沒有Tls12

                var request = new createTransactionRequest { transactionRequest = transactionRequest };

                // instantiate the contoller that will call the service
                var controller = new createTransactionController(request);
                controller.Execute();

                // get the response from the service (errors contained if any)
                var response = controller.GetApiResponse();

                finalResponse = ParseResponse(response);
            }
            catch (Exception ex)
            {
                finalResponse = GenerateExceptionResponse(ex.Message);
            }
            return finalResponse;
        }

        public static AuthorizeNetResponse VoidPayment(string refTransId, string cardNo, string cardExporedDate, string securityCode, bool simulation = false)
        {
            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();
            try
            {
                InitEnvironmentAndAccount(simulation);
                //if (simulation)
                //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
                //else
                //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;

                //// define the merchant information (authentication / transaction id)
                //ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                //{
                //    name = apiLoginId,
                //    ItemElementName = ItemChoiceType.transactionKey,
                //    Item = apiTransactionKey,
                //};

                var creditCard = new creditCardType();
                creditCard = new creditCardType
                {
                    cardNumber = cardNo,
                    expirationDate = cardExporedDate,
                    cardCode = securityCode
                };

                //standard api call to retrieve response
                var paymentType = new paymentType { Item = creditCard };



                var transactionRequest = new transactionRequestType
                {
                    transactionType = transactionTypeEnum.voidTransaction.ToString(),    // void  successful authorization
                    payment = paymentType,
                    refTransId = refTransId
                };

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .net3.5沒有Tls12

                var request = new createTransactionRequest { transactionRequest = transactionRequest };

                // instantiate the contoller that will call the service
                var controller = new createTransactionController(request);
                controller.Execute();

                // get the response from the service (errors contained if any)
                var response = controller.GetApiResponse();


                finalResponse = ParseResponse(response);
            }
            catch (Exception ex)
            {
                finalResponse = GenerateExceptionResponse(ex.Message);
            }
            return finalResponse;
        }

        public static AuthorizeNetResponse CapturePreviouslyAuthorizeAmount(decimal amount, string transactionId, bool simulation = false)
        {
            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();

            try
            {
                InitEnvironmentAndAccount(simulation);
                //if (simulation)
                //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
                //else
                //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;


                //// define the merchant information (authentication / transaction id)
                //ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                //{
                //    name = apiLoginId,
                //    ItemElementName = ItemChoiceType.transactionKey,
                //    Item = apiTransactionKey
                //};


                var transactionRequest = new transactionRequestType
                {
                    transactionType = transactionTypeEnum.priorAuthCaptureTransaction.ToString(),    // capture prior only
                    amount = amount,
                    refTransId = transactionId
                };

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .net3.5沒有Tls12

                var request = new createTransactionRequest { transactionRequest = transactionRequest };

                // instantiate the controller that will call the service
                var controller = new createTransactionController(request);
                controller.Execute();

                // get the response from the service (errors contained if any)
                var response = controller.GetApiResponse();

                finalResponse = ParseResponse(response);

            }
            catch (Exception ex)
            {
                finalResponse = GenerateExceptionResponse(ex.Message);
            }
            return finalResponse;
        }

        public static AuthorizeNetResponse RefundTransaction(decimal amount, string transactionId,
            string cardNo, string cardExporedDate, bool simulation = false)
        {
            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();

            try
            {
                InitEnvironmentAndAccount(simulation);
                //if (simulation)
                //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
                //else
                //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;

                //// define the merchant information (authentication / transaction id)
                //ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                //{
                //    name = apiLoginId,
                //    ItemElementName = ItemChoiceType.transactionKey,
                //    Item = apiTransactionKey
                //};

                var creditCard = new creditCardType
                {
                    cardNumber = cardNo,
                    expirationDate = cardExporedDate
                };

                //standard api call to retrieve response
                var paymentType = new paymentType { Item = creditCard };

                var transactionRequest = new transactionRequestType
                {
                    transactionType = transactionTypeEnum.refundTransaction.ToString(),    // refund type
                    payment = paymentType,
                    amount = amount,
                    refTransId = transactionId
                };

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .net3.5沒有Tls12

                var request = new createTransactionRequest { transactionRequest = transactionRequest };

                // instantiate the controller that will call the service
                var controller = new createTransactionController(request);
                controller.Execute();

                // get the response from the service (errors contained if any)
                var response = controller.GetApiResponse();

                finalResponse = ParseResponse(response);

            }
            catch (Exception ex)
            {
                finalResponse = GenerateExceptionResponse(ex.Message);
            }
            return finalResponse;
        }

        public static AuthorizeNetResponse CreatePaymentProfileForCustomerFromTransaction(string transactionId, string customerProfileId, bool simulation = false)
        {
            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();
            try
            {
                InitEnvironmentAndAccount(simulation);

                var request = new createCustomerProfileFromTransactionRequest
                {
                    transId = transactionId,
                    // You can either specify the customer information in form of customerProfileBaseType object
                    //customer = customerProfile
                    //  OR   
                    // You can just provide the customer Profile ID
                    customerProfileId = customerProfileId,
                };

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .net3.5沒有Tls12

                var controller = new createCustomerProfileFromTransactionController(request);
                controller.Execute();

                createCustomerProfileResponse response = controller.GetApiResponse();

                finalResponse = ParseResponse(response);
            }
            catch (Exception ex)
            {
                finalResponse = GenerateExceptionResponse(ex.Message);
            }

            return finalResponse;
        }


        public static List<AuthorizeNetResponse> GetSettledList(bool simulation = false)
        {
            List<AuthorizeNetResponse> finalResponseList = new List<AuthorizeNetResponse>();
            InitEnvironmentAndAccount(simulation);
            //if (simulation)
            //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            //else
            //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;


            //// define the merchant information (authentication / transaction id)
            //ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            //{
            //    name = apiLoginId,
            //    ItemElementName = ItemChoiceType.transactionKey,
            //    Item = apiTransactionKey,
            //};

            var request = new getUnsettledTransactionListRequest();
            request.status = TransactionGroupStatusEnum.any;
            request.statusSpecified = true;
            request.paging = new Paging
            {
                limit = 1000,
                offset = 1
            };
            request.sorting = new TransactionListSorting
            {
                orderBy = TransactionListOrderFieldEnum.id,
                orderDescending = true
            };
            // instantiate the controller that will call the service
            var controller = new getUnsettledTransactionListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactions == null)
                    return finalResponseList;

                foreach (var item in response.transactions)
                {
                    var response1 = new AuthorizeNetResponse();
                    response1.TransactionID = item.transId;
                    response1.Result = item.transactionStatus;
                }
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return finalResponseList;


        }

        public static AuthorizeNetResponse GetUnsettledList(bool simulation = false)
        {

            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();

            try
            {
                InitEnvironmentAndAccount(simulation);


                var request = new getUnsettledTransactionListRequest();
                request.status = TransactionGroupStatusEnum.any;
                request.statusSpecified = true;
                request.paging = new Paging
                {
                    limit = 1000,
                    offset = 1
                };
                request.sorting = new TransactionListSorting
                {
                    orderBy = TransactionListOrderFieldEnum.submitTimeUTC,
                    orderDescending = true
                };

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .net3.5沒有Tls12

                // instantiate the controller that will call the service
                var controller = new getUnsettledTransactionListController(request);
                controller.Execute();

                // get the response from the service (errors contained if any)
                var response = controller.GetApiResponse();

                if (response != null)
                {

                    if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
                    {

                        if (response.messages != null)
                        {
                            finalResponse.Result = "Success";
                            finalResponse.TransactionID = "NA";
                            finalResponse.AuthCode = response.messages.message[0].code;
                            finalResponse.Message = response.messages.message[0].text;
                        }
                        else
                        {
                            finalResponse.Result = "Success";
                            finalResponse.TransactionID = "NA";
                            finalResponse.AuthCode = "NA";
                            finalResponse.Message = "NA";
                        }

                        foreach (var item in response.transactions)
                        {
                            var tranRecord = new TransactionRecord();
                            tranRecord.TransactionID = item.transId;
                            tranRecord.OrderNo = item.invoiceNumber;
                            tranRecord.Status = item.transactionStatus;
                            tranRecord.SubmitTime = item.submitTimeLocal;
                            finalResponse.TransactionRecords.Add(tranRecord);
                        }
                    }
                    else if (response != null)
                    {
                        finalResponse.Result = "Fail";
                        finalResponse.TransactionID = "NA";
                        finalResponse.AuthCode = response.messages.message[0].code;
                        finalResponse.Message = response.messages.message[0].text;
                    }
                }
                else
                {
                    finalResponse.Result = "Fail";
                    finalResponse.TransactionID = "NA";
                    finalResponse.AuthCode = "NA";
                    finalResponse.Message = "Null Response.";
                }
            }
            catch (Exception ex)
            {
                finalResponse = GenerateExceptionResponse(ex.Message);

            }

            return finalResponse;

        }

        private static AuthorizeNetResponse ParseResponse(createTransactionResponse response)
        {
            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();

            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {

                    if (response.transactionResponse.messages != null)
                    {
                        finalResponse.Result = "Success";
                        finalResponse.TransactionID = response.transactionResponse.transId;
                        finalResponse.AuthCode = response.transactionResponse.authCode;
                        finalResponse.Message = response.transactionResponse.messages[0].description;
                    }
                    else
                    {
                        finalResponse.Result = "Fail";
                        finalResponse.TransactionID = "NA";
                        if (response.transactionResponse.errors != null)
                        {
                            finalResponse.AuthCode = response.transactionResponse.errors[0].errorCode;
                            finalResponse.Message = response.transactionResponse.errors[0].errorText;
                        }
                    }
                }
                else
                {
                    finalResponse.Result = "Fail";
                    finalResponse.TransactionID = "NA";
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        finalResponse.AuthCode = response.transactionResponse.errors[0].errorCode;
                        finalResponse.Message = response.transactionResponse.errors[0].errorText;
                    }
                    else
                    {
                        finalResponse.AuthCode = response.messages.message[0].code;
                    }
                }
            }
            else
            {
                finalResponse.Result = "Fail";
                finalResponse.TransactionID = "NA";
                finalResponse.AuthCode = "NA";
                finalResponse.Message = "Null Response.";
            }

            return finalResponse;
        }

        private static AuthorizeNetResponse ParseResponse(createCustomerProfileResponse response)
        {
            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();

            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {

                    if (response.messages.message != null)
                    {
                        finalResponse.Result = "Success";
                        finalResponse.TransactionID = "NA";
                        finalResponse.AuthCode = response.messages.message[0].code;
                        finalResponse.Message = response.messages.message[0].text;
                        if (response.customerProfileId != null)
                            finalResponse.CustomerProfileId = response.customerProfileId;
                    }
                    else
                    {
                        finalResponse.Result = "Fail";
                        finalResponse.TransactionID = "NA";
                        finalResponse.AuthCode = "NA";
                        finalResponse.Message = "NA";

                    }
                }
                else
                {
                    finalResponse.Result = "Fail";
                    finalResponse.TransactionID = "NA";
                    finalResponse.AuthCode = response.messages.message[0].code;
                    finalResponse.Message = response.messages.message[0].text;

                }
            }
            else
            {
                finalResponse.Result = "Fail";
                finalResponse.TransactionID = "NA";
                finalResponse.AuthCode = "NA";
                finalResponse.Message = "Null Response.";
            }

            return finalResponse;
        }

        public static AuthorizeNetResponse CreateCustomerProfile(string customerId, string desc, bool simulation = false)
        {
            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();
            try
            {
                InitEnvironmentAndAccount(simulation);


                customerProfileType customerProfile = new customerProfileType();
                customerProfile.merchantCustomerId = customerId;
                customerProfile.description = desc;


                var request = new createCustomerProfileRequest { profile = customerProfile, validationMode = validationModeEnum.none };

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .net3.5沒有Tls12

                // instantiate the controller that will call the service
                var controller = new createCustomerProfileController(request);

                //createCustomerProfileTransactionRequest request2 = new createCustomerProfileTransactionRequest();
                controller.Execute();

                // get the response from the service (errors contained if any)
                createCustomerProfileResponse response = controller.GetApiResponse();

                // validate response 
                finalResponse = ParseResponse(response);

            }
            catch (Exception ex)
            {
                finalResponse = GenerateExceptionResponse(ex.Message);
            }
            return finalResponse;
        }

        private static AuthorizeNetResponse GenerateExceptionResponse(string message)
        {
            AuthorizeNetResponse finalResponse = new AuthorizeNetResponse();
            finalResponse.Result = "Fail";
            finalResponse.AuthCode = "NA";
            finalResponse.TransactionID = "NA";
            finalResponse.Message = message;

            return finalResponse;
        }

        public static string GetOrCreateCustomerProfileId(string ERPId, string ORG, bool simulation = false)
        {
            string customerProfileId = "";

            if (!string.IsNullOrEmpty(ERPId))
            {
                string str = string.Format("select CUSTOMER_PROFILE_ID from BB_CIM_ProfileMapping where COMPANY_ID = '{0}'", ERPId);


                var objCustomerProfileId = SqlProvider.dbExecuteScalar("MY", str);

                if (objCustomerProfileId != null)
                {
                    customerProfileId = objCustomerProfileId.ToString();

                }
                else
                {
                    string desc = Advantech.Myadvantech.DataAccess.DataCore.MyAdvantech.SAPCompanyHelper.GetSAPDIMCompanyByID(ERPId, ORG).Where(s=>s.COMPANY_TYPE == "Z001").FirstOrDefault().COMPANY_NAME;
                    var respones = CreateCustomerProfile(ERPId, desc, simulation);
                    if (respones != null && respones.Result == "Success" && !string.IsNullOrEmpty(respones.CustomerProfileId))
                    {
                        customerProfileId = respones.CustomerProfileId;
                        str = string.Format("Insert into BB_CIM_ProfileMapping values(N'{0}',N'{1}')", ERPId, customerProfileId);
                        SqlProvider.dbExecuteNoQuery("MY", str);
                    }


                }
            }
            return customerProfileId;
        }

    }
        

    public  class AuthorizeNetResponse
    {
        public string TransactionID { get; set; }
        public string Result { get; set; }
        public string AuthCode { get; set; }
        public string Message { get; set; }
        public string CustomerProfileId { get; set; }

        public AuthorizeNetResponse()
        {
            this.TransactionRecords = new List<TransactionRecord>();
        }

        public List<TransactionRecord> TransactionRecords { get; set; }
    }

    public class TransactionRecord
    {
        public string TransactionID { get; set; }
        public string Status { get; set; }
        public string OrderNo { get; set; }
        public DateTime SubmitTime { get; set; }
    }
        

}
