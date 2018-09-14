using Domain.Entity;
using ExpressCheckout;
using FytMsys.Common;
using FytMsys.Helper;
using log4net;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FytMsys.Web.Controllers
{
    public class PaypalController : Controller
    {
        public readonly static string ReturnUrl = "http://www.51voy.com/Paypal/GetExpressCheckout";
        public readonly static string CancelUrl = "http://www.51voy.com/";
        public readonly static string LogoUrl = "http://www.51voy.com/lib/img/logo.png";
        public readonly static string SellerEmail = "go51voy@gmail.com";

        // Logs output statements, errors, debug info to a text file
        private static ILog logger = LogManager.GetLogger(typeof(SetExpressCheckout));

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Manager.ConfigManager.Instance.GetProperties();
        }

        public readonly static string RedirectUrl = GetConfig()["RedirectUrl"];

        public ActionResult Index()
        {
            Session.Clear();
            return View();
        }

        public ActionResult SetExpressCheckout()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Create the SetExpressCheckoutResponseType object
            SetExpressCheckoutResponseType responseSetExpressCheckoutResponseType = new SetExpressCheckoutResponseType();
            try
            {
                // Check if the EC methos is shorcut or mark
                string ecMethod = "ShorcutExpressCheckout";
                if (Request.QueryString["ExpressCheckoutMethod"] != null)
                {
                    ecMethod = Request.QueryString["ExpressCheckoutMethod"];
                }
                else if ((string)(Session["ExpressCheckoutMethod"]) != null)
                {
                    ecMethod = (string)(Session["ExpressCheckoutMethod"]);
                }
                string item_id = "";
                string item_name = "";
                string item_desc = "";
                string item_quantity = "1";
                string item_amount = "";
                string total_amount = "";
                string currency_code = "USD";
                string payment_type = "Authorization";

                // From Marck EC Page
                AddressType shipToAddress = new AddressType();
                if (ecMethod != null && ecMethod == "ShorcutExpressCheckout")
                {
                    // Get parameters from index page (shorcut express checkout)
                    item_id = FytRequest.GetQueryStringEncode("i");
                    item_name = FytRequest.GetQueryStringEncode("t");
                    item_desc = FytRequest.GetQueryStringEncode("o");
                    item_amount = FytRequest.GetQueryStringEncode("m");
                    total_amount = FytRequest.GetQueryStringEncode("m");
                    Session["Total_Amount"] = total_amount;
                    //logger.Info("录入订单号：" + item_desc + "/n");
                    Session["OrderNum"] = item_desc;
                }
                else if (ecMethod != null && ecMethod == "MarkExpressCheckout")
                {
                    // Get parameters from mark ec page 

                    item_id = FytRequest.GetQueryStringEncode("i");
                    item_name = FytRequest.GetQueryStringEncode("t");
                    item_desc = FytRequest.GetQueryStringEncode("o");
                    item_amount = FytRequest.GetQueryStringEncode("m");
                    total_amount = FytRequest.GetQueryStringEncode("m");

                    Double total_rate = Convert.ToDouble(total_amount);

                    // Calculate new order total based on shipping method selected

                    Session["Total_Amount"] = total_rate.ToString();
                    //logger.Info("录入订单号：" + item_desc + "/n");
                    Session["OrderNum"] = item_desc;
                }

                Session["SellerEmail"] = SellerEmail;
                CurrencyCodeType currencyCode_Type = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), currency_code, true);
                Session["currency_code_type"] = currencyCode_Type;
                PaymentActionCodeType payment_ActionCode_Type = (PaymentActionCodeType)Enum.Parse(typeof(PaymentActionCodeType), payment_type, true);
                Session["payment_action_type"] = payment_ActionCode_Type;
                // SetExpressCheckoutRequestDetailsType object
                SetExpressCheckoutRequestDetailsType setExpressCheckoutRequestDetails = new SetExpressCheckoutRequestDetailsType();
                // (Required) URL to which the buyer's browser is returned after choosing to pay with PayPal.
                setExpressCheckoutRequestDetails.ReturnURL = ReturnUrl;
                //(Required) URL to which the buyer is returned if the buyer does not approve the use of PayPal to pay you
                setExpressCheckoutRequestDetails.CancelURL = CancelUrl;
                // A URL to your logo image. Use a valid graphics format, such as .gif, .jpg, or .png
                setExpressCheckoutRequestDetails.cppLogoImage = LogoUrl;
                // To display the border in your principal identifying color, set the "cppCartBorderColor" parameter to the 6-digit hexadecimal value of that color
                // setExpressCheckoutRequestDetails.cppCartBorderColor = "0000CD";

                //Item details
                PaymentDetailsItemType itemDetails = new PaymentDetailsItemType();
                itemDetails.Name = item_name;
                itemDetails.Amount = new BasicAmountType(currencyCode_Type, item_amount);
                itemDetails.Quantity = Convert.ToInt32(item_quantity);
                itemDetails.Description = item_desc;
                itemDetails.Number = item_id;

                //Add more items if necessary by using the class 'PaymentDetailsItemType'

                // Payment Information
                List<PaymentDetailsType> paymentDetailsList = new List<PaymentDetailsType>();

                PaymentDetailsType paymentDetails = new PaymentDetailsType();
                paymentDetails.PaymentAction = payment_ActionCode_Type;
                paymentDetails.ItemTotal = new BasicAmountType(currencyCode_Type, item_amount);//item amount                
                paymentDetails.OrderTotal = new BasicAmountType(currencyCode_Type, total_amount); // order total amount

                paymentDetails.PaymentDetailsItem.Add(itemDetails);

                // Unique identifier for the merchant. 
                SellerDetailsType sellerDetails = new SellerDetailsType();
                sellerDetails.PayPalAccountID = SellerEmail;
                paymentDetails.SellerDetails = sellerDetails;

                if (ecMethod != null && ecMethod == "MarkExpressCheckout")
                {
                    paymentDetails.ShipToAddress = shipToAddress;
                }
                paymentDetailsList.Add(paymentDetails);
                setExpressCheckoutRequestDetails.PaymentDetails = paymentDetailsList;

                // Collect Shipping details if MARK express checkout

                SetExpressCheckoutReq setExpressCheckout = new SetExpressCheckoutReq();
                SetExpressCheckoutRequestType setExpressCheckoutRequest = new SetExpressCheckoutRequestType(setExpressCheckoutRequestDetails);
                setExpressCheckout.SetExpressCheckoutRequest = setExpressCheckoutRequest;

                // Create the service wrapper object to make the API call
                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();

                // API call
                // Invoke the SetExpressCheckout method in service wrapper object
                responseSetExpressCheckoutResponseType = service.SetExpressCheckout(setExpressCheckout);

                if (responseSetExpressCheckoutResponseType != null)
                {
                    // Response envelope acknowledgement
                    //string acknowledgement = "SetExpressCheckout API Operation - ";
                    //acknowledgement += responseSetExpressCheckoutResponseType.Ack.ToString();
                    //logger.Debug(acknowledgement + "\n");
                    //System.Diagnostics.Debug.WriteLine(acknowledgement + "\n");
                    // # Success values
                    if (responseSetExpressCheckoutResponseType.Ack.ToString().Trim().ToUpper().Equals("SUCCESS"))
                    {
                        // # Redirecting to PayPal for authorization
                        // Once you get the "Success" response, needs to authorise the
                        // transaction by making buyer to login into PayPal. For that,
                        // need to construct redirect url using EC token from response.
                        // Express Checkout Token
                        string EcToken = responseSetExpressCheckoutResponseType.Token;
                        //logger.Info("Express Checkout Token : " + EcToken + "\n");
                        //System.Diagnostics.Debug.WriteLine("Express Checkout Token : " + EcToken + "\n");
                        // Store the express checkout token in session to be used in GetExpressCheckoutDetails & DoExpressCheckout API operations
                        Session["EcToken"] = EcToken;
                        Response.Redirect(RedirectUrl + HttpUtility.UrlEncode(EcToken), false);
                        // Server.Execute(RedirectUrl + EcToken);
                    }
                    // # Error Values
                    else
                    {
                        List<ErrorType> errorMessages = responseSetExpressCheckoutResponseType.Errors;
                        string errorMessage = "";
                        foreach (ErrorType error in errorMessages)
                        {
                            logger.Debug("API Error Message : " + error.LongMessage);
                            System.Diagnostics.Debug.WriteLine("API Error Message : " + error.LongMessage + "\n");
                            errorMessage = errorMessage + error.LongMessage;
                        }
                        //Redirect to error page in case of any API errors
                        Server.Transfer("~/Response.aspx");
                    }
                }
            }
            catch (System.Exception ex)
            {
                // Log the exception message
                logger.Debug("Error Message : " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Error Message : " + ex.Message);
            }
            return View();
        }

        //通过支付方得到信息
        public ActionResult GetExpressCheckout()
        {
            // Create the GetExpressCheckoutDetailsResponseType object
            GetExpressCheckoutDetailsResponseType responseGetExpressCheckoutDetailsResponseType = new GetExpressCheckoutDetailsResponseType();
            try
            {
                // Create the GetExpressCheckoutDetailsReq object
                GetExpressCheckoutDetailsReq getExpressCheckoutDetails = new GetExpressCheckoutDetailsReq();
                // A timestamped token, the value of which was returned by `SetExpressCheckout` API response
                string EcToken = (string)(Session["EcToken"]);
                GetExpressCheckoutDetailsRequestType getExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType(EcToken);
                getExpressCheckoutDetails.GetExpressCheckoutDetailsRequest = getExpressCheckoutDetailsRequest;
                // Create the service wrapper object to make the API call
                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();
                // # API call
                // Invoke the GetExpressCheckoutDetails method in service wrapper object
                responseGetExpressCheckoutDetailsResponseType = service.GetExpressCheckoutDetails(getExpressCheckoutDetails);
                if (responseGetExpressCheckoutDetailsResponseType != null)
                {
                    // Response envelope acknowledgement
                    //string acknowledgement = "GetExpressCheckoutDetails API Operation - ";
                    //acknowledgement += responseGetExpressCheckoutDetailsResponseType.Ack.ToString();
                    //logger.Info(acknowledgement + "\n");
                    //System.Diagnostics.Debug.WriteLine(acknowledgement + "\n");
                    // # Success values
                    if (responseGetExpressCheckoutDetailsResponseType.Ack.ToString().Trim().ToUpper().Equals("SUCCESS"))
                    {
                        // Unique PayPal Customer Account identification number. This
                        // value will be null unless you authorize the payment by
                        // redirecting to PayPal after `SetExpressCheckout` call.
                        string PayerId = responseGetExpressCheckoutDetailsResponseType.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID;
                        // Store PayerId in session to be used in DoExpressCheckout API operation
                        Session["PayerId"] = PayerId;

                        List<PaymentDetailsType> paymentDetails = responseGetExpressCheckoutDetailsResponseType.GetExpressCheckoutDetailsResponseDetails.PaymentDetails;
                        foreach (PaymentDetailsType paymentdetail in paymentDetails)
                        {
                            AddressType ShippingAddress = paymentdetail.ShipToAddress;
                            if (ShippingAddress != null)
                            {
                                Session["Address_Name"] = ShippingAddress.Name;
                                Session["Address_Street"] = ShippingAddress.Street1 + " " + ShippingAddress.Street2;
                                Session["Address_CityName"] = ShippingAddress.CityName;
                                Session["Address_StateOrProvince"] = ShippingAddress.StateOrProvince;
                                Session["Address_CountryName"] = ShippingAddress.CountryName;
                                Session["Address_PostalCode"] = ShippingAddress.PostalCode;
                            }
                            Session["Currency_Code"] = paymentdetail.OrderTotal.currencyID;
                            Session["Order_Total"] = paymentdetail.OrderTotal.value;
                            Session["Shipping_Total"] = paymentdetail.ShippingTotal.value;
                            List<PaymentDetailsItemType> itemList = paymentdetail.PaymentDetailsItem;
                            foreach (PaymentDetailsItemType item in itemList)
                            {
                                Session["Product_Quantity"] = item.Quantity;
                                Session["Product_Name"] = item.Name;

                            }
                        }
                    }
                    // # Error Values
                    else
                    {
                        List<ErrorType> errorMessages = responseGetExpressCheckoutDetailsResponseType.Errors;
                        string errorMessage = "";
                        foreach (ErrorType error in errorMessages)
                        {
                            logger.Debug("API Error Message : " + error.LongMessage);
                            System.Diagnostics.Debug.WriteLine("API Error Message : " + error.LongMessage + "\n");
                            errorMessage = errorMessage + error.LongMessage;
                        }
                        //Redirect to error page in case of any API errors
                        Server.Transfer("~/Response.aspx");
                    }
                }
                //Redirect to DoExpressCheckoutPayment.aspx page if the method chosen is MarkExpressCheckout
                //The buyer need not review the shipping address and shipping method as it's already provided
                string ecMethod = (string)(Session["ExpressCheckoutMethod"]);
                if (ecMethod.Equals("MarkExpressCheckout"))
                {
                    Response.Redirect("/paypal/DoExpressCheckoutPayment");
                }

            }
            // # Exception log
            catch (System.Exception ex)
            {
                // Log the exception message
                logger.Debug("Error Message : " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Error Message : " + ex.Message);
            }
            return View();
        }

        public void callDoExpressCheckout()
        {
            //Get shippping rate
            // Calculate new order total based on shipping method selected

            Response.Redirect("/paypal/DoExpressCheckoutPayment");
        }

        public ActionResult DoExpressCheckoutPayment()
        {
            // Create the DoExpressCheckoutPaymentResponseType object
            DoExpressCheckoutPaymentResponseType responseDoExpressCheckoutPaymentResponseType = new DoExpressCheckoutPaymentResponseType();
            try
            {
                // Create the DoExpressCheckoutPaymentReq object
                DoExpressCheckoutPaymentReq doExpressCheckoutPayment = new DoExpressCheckoutPaymentReq();
                DoExpressCheckoutPaymentRequestDetailsType doExpressCheckoutPaymentRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
                // The timestamped token value that was returned in the
                // `SetExpressCheckout` response and passed in the
                // `GetExpressCheckoutDetails` request.
                doExpressCheckoutPaymentRequestDetails.Token = (string)(Session["EcToken"]);
                // Unique paypal buyer account identification number as returned in
                // `GetExpressCheckoutDetails` Response
                doExpressCheckoutPaymentRequestDetails.PayerID = (string)(Session["PayerId"]);

                // # Payment Information
                // list of information about the payment
                List<PaymentDetailsType> paymentDetailsList = new List<PaymentDetailsType>();
                // information about the payment
                PaymentDetailsType paymentDetails = new PaymentDetailsType();
                CurrencyCodeType currency_code_type = (CurrencyCodeType)(Session["currency_code_type"]);
                PaymentActionCodeType payment_action_type = (PaymentActionCodeType)(Session["payment_action_type"]);
                //Pass the order total amount which was already set in session
                string total_amount = (string)(Session["Total_Amount"]);
                BasicAmountType orderTotal = new BasicAmountType(currency_code_type, total_amount);
                paymentDetails.OrderTotal = orderTotal;
                paymentDetails.PaymentAction = payment_action_type;

                //BN codes to track all transactions
                paymentDetails.ButtonSource = "PP-DemoPortal-PPCredit-dotnet";

                // Unique identifier for the merchant. 
                SellerDetailsType sellerDetails = new SellerDetailsType();
                sellerDetails.PayPalAccountID = (string)(Session["SellerEmail"]);
                paymentDetails.SellerDetails = sellerDetails;

                paymentDetailsList.Add(paymentDetails);
                doExpressCheckoutPaymentRequestDetails.PaymentDetails = paymentDetailsList;

                DoExpressCheckoutPaymentRequestType doExpressCheckoutPaymentRequest = new DoExpressCheckoutPaymentRequestType(doExpressCheckoutPaymentRequestDetails);
                doExpressCheckoutPayment.DoExpressCheckoutPaymentRequest = doExpressCheckoutPaymentRequest;
                // Create the service wrapper object to make the API call
                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();
                // # API call
                // Invoke the DoExpressCheckoutPayment method in service wrapper object
                responseDoExpressCheckoutPaymentResponseType = service.DoExpressCheckoutPayment(doExpressCheckoutPayment);
                if (responseDoExpressCheckoutPaymentResponseType != null)
                {

                    // Response envelope acknowledgement
                    //string acknowledgement = "DoExpressCheckoutPayment API Operation - ";
                    //acknowledgement += responseDoExpressCheckoutPaymentResponseType.Ack.ToString();
                    //logger.Info(acknowledgement + "\n");
                    //System.Diagnostics.Debug.WriteLine(acknowledgement + "\n");
                    // # Success values
                    if (responseDoExpressCheckoutPaymentResponseType.Ack.ToString().Trim().ToUpper().Equals("SUCCESS"))
                    {
                        // Transaction identification number of the transaction that was
                        // created.
                        // This field is only returned after a successful transaction
                        // for DoExpressCheckout has occurred.
                        if (responseDoExpressCheckoutPaymentResponseType.DoExpressCheckoutPaymentResponseDetails.PaymentInfo != null)
                        {
                            IEnumerator<PaymentInfoType> paymentInfoIterator = responseDoExpressCheckoutPaymentResponseType.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.GetEnumerator();
                            while (paymentInfoIterator.MoveNext())
                            {
                                PaymentInfoType paymentInfo = paymentInfoIterator.Current;
                                //logger.Info("Transaction ID : " + paymentInfo.TransactionID + "\n");

                                Session["Transaction_Id"] = paymentInfo.TransactionID;
                                Session["Transaction_Type"] = paymentInfo.TransactionType;
                                Session["Payment_Status"] = paymentInfo.PaymentStatus;
                                Session["Payment_Type"] = paymentInfo.PaymentType;
                                Session["Payment_Total_Amount"] = paymentInfo.GrossAmount.value;

                                #region 业务逻辑代码
                                //logger.Info("业务逻辑开始\n");
                                var out_trade_no = (string)Session["OrderNum"];
                                //logger.Info("订单号：" + out_trade_no + "\n");
                                var header = out_trade_no.Substring(0, 2);
                                if (header == "WD")
                                {
                                    //我等你
                                    var oModel = OperateContext<lv_ProjectOrder>.SetServer.GetModel(m => m.Number == out_trade_no);
                                    if (oModel != null)
                                    {
                                        if (oModel.PayStatus == 1)
                                        {
                                            return RedirectToAction("PaySuccess", "ProJect", new { o = out_trade_no });
                                        }
                                        var total_fee = paymentInfo.GrossAmount.value;
                                        //var np = Math.Round(oModel.PayPrice, 0); //订单金额，转换成分，进行对比
                                        //判断金额是否相等
                                        if (oModel.PayPrice== decimal.Parse(total_fee))
                                        {
                                            oModel.PayStatus = 1; //更改支付状态
                                            oModel.Status = true; //更改订单状态
                                            OperateContext<lv_ProjectOrder>.SetServer.Update(oModel);
                                            //生成参与用户财务日志
                                            var uml = new tb_UserMoneyLog()
                                            {
                                                Number = UtilsHelper.GetRamCode(),
                                                UserId = oModel.UserId,
                                                Option = 1,
                                                Price = oModel.PayPrice,
                                                RealPrice = 0,
                                                NowPrice = 0,
                                                Status = 1,
                                                PayType = "Paypal",
                                                Summary = "支付我等你旅游项目",
                                                AddDate = DateTime.Now,
                                            };
                                            OperateContext<tb_UserMoneyLog>.SetServer.Add(uml);
                                            var project = OperateContext<lv_ProJect>.SetServer.GetModel(m => m.ID == oModel.ProJectId);
                                            //发送一条预约消息
                                            var message = new lv_Message()
                                            {
                                                SendUserId = oModel.UserId,
                                                GoUserId = project.UserId,
                                                Centents = "我预约了你的[" + project.Title + "]",
                                                IsRead = false,
                                                AddTime = DateTime.Now
                                            };
                                            OperateContext<lv_Message>.SetServer.Add(message);
                                            //成功跳转
                                            return RedirectToAction("PaySuccess", "ProJect", new { o = out_trade_no });
                                        }
                                        else
                                        {
                                            return Content("支付金额不符合");
                                        }
                                    }
                                    else
                                    {
                                        return Content("即时到账支付失败");
                                    }
                                }
                                else if (header == "QK")
                                {
                                    //去看看
                                    var oModel = OperateContext<lv_GoLookOrder>.SetServer.GetModel(m => m.Number == out_trade_no);
                                    if (oModel != null)
                                    {
                                        if (oModel.PayStatus == 1)
                                        {
                                            return RedirectToAction("PaySuccess", "ProJect", new { o = out_trade_no });
                                        }
                                        var total_fee = paymentInfo.GrossAmount.value;
                                        //var np = Math.Round(oModel.PayPrice, 0); //订单金额，转换成分，进行对比
                                        //判断金额是否相等
                                        if (oModel.PayPrice == decimal.Parse(total_fee))
                                        {
                                            oModel.PayStatus = 1; //更改支付状态
                                            OperateContext<lv_GoLookOrder>.SetServer.Update(oModel);
                                            //生成参与用户财务日志
                                            var uml = new tb_UserMoneyLog()
                                            {
                                                Number = UtilsHelper.GetRamCode(),
                                                UserId = oModel.UserId,
                                                Option = 1,
                                                Price = oModel.PayPrice,
                                                RealPrice = 0,
                                                NowPrice = 0,
                                                Status = 1,
                                                PayType = "Paypal",
                                                Summary = "支付去看看旅游项目",
                                                AddDate = DateTime.Now,
                                            };
                                            OperateContext<tb_UserMoneyLog>.SetServer.Add(uml);
                                            var look = OperateContext<lv_GoLook>.SetServer.GetModel(m => m.ID == oModel.LookId);
                                            //发送一条预约消息
                                            var message = new lv_Message()
                                            {
                                                SendUserId = oModel.UserId,
                                                GoUserId = look.UserId,
                                                Centents = "我参与了你的[" + look.Title + "]",
                                                IsRead = false,
                                                AddTime = DateTime.Now
                                            };
                                            OperateContext<lv_Message>.SetServer.Add(message);
                                            //成功跳转
                                            return RedirectToAction("PaySuccess", "GoLook", new { o = out_trade_no });
                                        }
                                        else
                                        {
                                            return Content("支付金额不符合");
                                        }
                                    }
                                    else
                                    {
                                        return Content("即时到账支付失败");
                                    }
                                }
                                #endregion

                                System.Diagnostics.Debug.WriteLine("Transaction ID : " + paymentInfo.TransactionID + "\n");
                            }
                        }
                    }
                    // # Error Values
                    else
                    {
                        var out_trade_no = (string)Session["OrderNum"];
                        var header = out_trade_no.Substring(0, 2);
                        if (header == "WD")
                        {
                            OperateContext<lv_ProjectOrder>.SetServer.DeleteBy(m => m.Number == out_trade_no);
                        }
                        else if (header == "QK")
                        {
                            OperateContext<lv_GoLookOrder>.SetServer.DeleteBy(m => m.Number == out_trade_no);
                        }

                        List<ErrorType> errorMessages = responseDoExpressCheckoutPaymentResponseType.Errors;
                        string errorMessage = "";
                        foreach (ErrorType error in errorMessages)
                        {
                            logger.Debug("API Error Message : " + error.LongMessage);
                            System.Diagnostics.Debug.WriteLine("API Error Message : " + error.LongMessage + "\n");
                            errorMessage = errorMessage + error.LongMessage;
                        }
                        //Redirect to error page in case of any API errors
                        Server.Transfer("~/Response.aspx");
                    }
                }
            }
            catch (System.Exception ex)
            {
                // Log the exception message
                logger.Debug("Error Message : " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Error Message : " + ex.Message);
            }
            return View();
        }

    }
}