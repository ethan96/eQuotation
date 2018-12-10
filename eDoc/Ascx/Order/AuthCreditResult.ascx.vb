Public Class AuthCreditResult
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property PNReference As String
        Get
            Return Label2.Text
        End Get
    End Property
    Public Function Auth(ByVal Amount As Decimal, _
                         ByVal FirstName As String, ByVal LastName As String, _
                         ByVal BillToStreet As String, ByVal City As String, ByVal State As String, ByVal BillToZip As String, _
                         ByVal PoNo As String, ByVal CreditCardNum As String, _
                         ByVal CvvCode As String, ByVal ExpDate As Date) As Boolean
        Label1.Text = "" : Label2.Text = "" : Label3.Text = "" : Label4.Text = "" : Label5.Text = "" : Label6.Text = "" : Label7.Text = ""
        Label8.Text = "" : Label9.Text = "" : Label10.Text = "" : Label11.Text = "" : Label12.Text = ""
        trFraudRow.Visible = False
        Dim rlt As Boolean = False
        Dim validresult As String = "Invalid"
        Dim strFraudAlert As List(Of String) = New List(Of String)
        If String.IsNullOrEmpty(FirstName) Or String.IsNullOrEmpty(FirstName) Then
            strFraudAlert.Add("Missing card holder name")
            validresult = "Invalid"
        End If
        If String.IsNullOrEmpty(BillToStreet) Then
            strFraudAlert.Add("Missing bill address")
            validresult = "Invalid"
        End If
        If String.IsNullOrEmpty(BillToZip) Then
            strFraudAlert.Add("Missing bill zip code")
            validresult = "Invalid"
        End If
        If strFraudAlert.Count > 0 Then
            Label3.Text = String.Format("{0}<br /><fieldset style=""color:Red;""><legend>Fraud Alert</legend>{1}</fieldset>", validresult, String.Join("<br />", strFraudAlert.ToArray()))
            trAuthInfo.Visible = True
            SaveLog(Amount, FirstName, LastName, BillToStreet, City, State, BillToZip, PoNo, CreditCardNum, CvvCode, ExpDate, validresult, strFraudAlert)
            Return rlt
        End If
        Dim User As New PayPal.Payments.DataObjects.UserInfo("Advantech", "Advantech", "verisign", "2ws3ed4rf")
        Dim Connection As New PayPal.Payments.DataObjects.PayflowConnectionData("payflowpro.paypal.com")
        'Dim Connection As New PayPal.Payments.DataObjects.PayflowConnectionData("pilot-payflowpro.paypal.com")

        Dim Inv As New PayPal.Payments.DataObjects.Invoice
        ' Set Amount.
        Dim Amt As New PayPal.Payments.DataObjects.Currency(0, "USD")
        Inv.Amt = Amt : Inv.PoNum = PoNo : Inv.InvNum = ""

        ' Set the Billing Address details.
        Dim Bill As New PayPal.Payments.DataObjects.BillTo
        Bill.FirstName = FirstName
        Bill.LastName = LastName

        Bill.Street = BillToStreet : Bill.City = City : Bill.State = State : Bill.Zip = BillToZip : Inv.BillTo = Bill

        ' Create a new Payment Device - Credit Card data object.
        ' The input parameters are Credit Card No. and Expiry Date for the Credit Card.
        Dim CC As New PayPal.Payments.DataObjects.CreditCard(CreditCardNum, ExpDate.ToString("MM") + Right(ExpDate.Year.ToString(), 2))
        CC.Cvv2 = CvvCode

        ' Create a new Tender - Card Tender data object.
        Dim Card As New PayPal.Payments.DataObjects.CardTender(CC)
        '/////////////////////////////////////////////////////////////////

        ' Create a new Auth Transaction.
        Dim Trans As New PayPal.Payments.Transactions.AuthorizationTransaction(User, Connection, Inv, Card, PayPal.Payments.Common.Utility.PayflowUtility.RequestId)

        ' Submit the transaction.
        Dim Resp As PayPal.Payments.DataObjects.Response = Trans.SubmitTransaction()
        If Not Resp Is Nothing Then
            Dim TrxnResponse As PayPal.Payments.DataObjects.TransactionResponse = Resp.TransactionResponse
            'Dim sbAuthResponse As New System.Text.StringBuilder
            If Not TrxnResponse Is Nothing Then
                trAuthInfo.Visible = True
                Label1.Text = TrxnResponse.Result.ToString
                Label2.Text = TrxnResponse.Pnref
                Label3.Text = TrxnResponse.RespMsg
                Label4.Text = TrxnResponse.AuthCode
                Label5.Text = TrxnResponse.AVSAddr
                Label6.Text = TrxnResponse.AVSZip
                Label7.Text = TrxnResponse.IAVS
                Label8.Text = TrxnResponse.CVV2Match
                Label9.Text = TrxnResponse.Duplicate

                'Dim validresult As String = ""
                If TrxnResponse.Result = 0 Then
                    'fraud checking
                    If TrxnResponse.AVSAddr IsNot Nothing AndAlso TrxnResponse.AVSAddr.Equals("N") Then
                        strFraudAlert.Add("AVS Street mismatch")
                        validresult = "Fraud Alert"
                    End If
                    If TrxnResponse.AVSZip IsNot Nothing AndAlso TrxnResponse.AVSZip.Equals("N") Then
                        strFraudAlert.Add("AVS Zip mismatch")
                        validresult = "Fraud Alert"
                    End If
                    If TrxnResponse.CVV2Match IsNot Nothing AndAlso TrxnResponse.CVV2Match.Equals("N") Then
                        strFraudAlert.Add("Card Security Code mismatch")
                        validresult = "Invalid"
                    End If
                    If strFraudAlert.Count > 0 Then
                        Label3.Text = String.Format("{0}<br /><div style=""color:Red;"">{1}</div>", validresult, String.Join("<br />", strFraudAlert.ToArray()))
                    Else
                        'checking expire date, charge $1 then void
                        Dim AuthInv As New PayPal.Payments.DataObjects.Invoice
                        Dim AuthAmt As New PayPal.Payments.DataObjects.Currency(1, "USD")
                        AuthInv.Amt = AuthAmt : AuthInv.PoNum = PoNo : AuthInv.InvNum = ""
                        Dim AuthTrans As New PayPal.Payments.Transactions.AuthorizationTransaction(User, Connection, AuthInv, Card, PayPal.Payments.Common.Utility.PayflowUtility.RequestId)
                        ' Submit the transaction.
                        Dim AuthResp As PayPal.Payments.DataObjects.Response = AuthTrans.SubmitTransaction()
                        If Not AuthResp Is Nothing Then
                            Dim AuthTrxnResponse As PayPal.Payments.DataObjects.TransactionResponse = AuthResp.TransactionResponse

                            If Not AuthTrxnResponse Is Nothing Then
                                If AuthTrxnResponse.Result = 0 Then
                                    validresult = "Valid"
                                    rlt = True
                                Else
                                    strFraudAlert.Add(AuthTrxnResponse.RespMsg)
                                    validresult = "Invalid"
                                End If
                                If Not String.IsNullOrEmpty(AuthTrxnResponse.Pnref) Then
                                    Dim VoidTrans As PayPal.Payments.Transactions.VoidTransaction = _
                                   New PayPal.Payments.Transactions.VoidTransaction(AuthTrxnResponse.Pnref, User, Connection, PayPal.Payments.Common.Utility.PayflowUtility.RequestId)

                                    ' Submit the transaction.                                                                                
                                    Dim VoidResp As PayPal.Payments.DataObjects.Response = VoidTrans.SubmitTransaction()

                                    If Not VoidResp Is Nothing Then
                                        ' Get the Transaction Response parameters.                                                
                                        Dim VoidTrxnResponse As PayPal.Payments.DataObjects.TransactionResponse = VoidResp.TransactionResponse
                                        If VoidTrxnResponse IsNot Nothing Then
                                            If VoidTrxnResponse.Result = 0 Then
                                                'validresult = "Valid"
                                            Else
                                                'need log  AuthTrxnResponse.Pnref
                                                'strFraudAlert.Add("void Transaction failed, please Contact Cathee to void this Transaction manually. Pnref: " + AuthTrxnResponse.Pnref)
                                                'validresult = "Valid"
                                                Dim voiderror As List(Of String) = New List(Of String)
                                                voiderror.Add(String.Format("Auth Pnref: {0}", AuthTrxnResponse.Pnref))
                                                voiderror.Add(String.Format("Void Pnref: {0}, Void RespMsg: {1}", VoidTrxnResponse.Pnref, VoidTrxnResponse.RespMsg))
                                                SaveLog(Amount, FirstName, LastName, BillToStreet, City, State, BillToZip, PoNo, CreditCardNum, CvvCode, ExpDate _
                                                        , "Void Transaction Faild", voiderror)
                                            End If

                                        End If
                                    Else
                                        Dim voiderror As List(Of String) = New List(Of String)
                                        voiderror.Add(String.Format("Auth Pnref: {0}", AuthTrxnResponse.Pnref))
                                        voiderror.Add(String.Format("Void RespMsg: {0}", "No response"))
                                        SaveLog(Amount, FirstName, LastName, BillToStreet, City, State, BillToZip, PoNo, CreditCardNum, CvvCode, ExpDate _
                                                , "Void Transaction Faild", voiderror)
                                    End If
                                End If
                            End If
                        Else
                            strFraudAlert.Add("no response, please try again.")
                            validresult = "Invalid"
                        End If
                        If strFraudAlert.Count > 0 Then
                            Label3.Text = String.Format("{0}<br /><div style=""color:Red;"">{1}</div>", validresult, String.Join("<br />", strFraudAlert.ToArray()))
                        Else
                            Label3.Text = validresult
                        End If
                    End If
                Else
                    Label3.Text = "Invalid"
                End If

                Dim FraudResp As PayPal.Payments.DataObjects.FraudResponse = Resp.FraudResponse
                If Not FraudResp Is Nothing Then
                    Label10.Text = FraudResp.PreFpsMsg : Label11.Text = FraudResp.PostFpsMsg
                    'trFraudRow.Visible = True
                Else
                    trFraudRow.Visible = False
                End If
                '' Get the Transaction Context and check for any contained SDK specific errors (optional code).
                Dim TransCtx As PayPal.Payments.Common.Context = Resp.TransactionContext
                If (Not TransCtx Is Nothing) And (TransCtx.getErrorCount() > 0) Then
                    Label12.Text = "Transaction Errors:" + TransCtx.ToString()
                End If
            Else
                trAuthInfo.Visible = False
            End If

        End If

        'Save Log
        SaveLog(Amount, FirstName, LastName, BillToStreet, City, State, BillToZip, PoNo, CreditCardNum, CvvCode, ExpDate, validresult, strFraudAlert)

        'Dim aptOrderDetail As New MyOrderDSTableAdapters.ORDER_DETAILTableAdapter, aptOrderPartner As New MyOrderDSTableAdapters.ORDER_PARTNERSTableAdapter
        'Dim decTotalAmount As Decimal = aptOrderDetail.getTotalAmount(CartId)
        'Dim txtBillToStreet As String = "", txtBillToZip As String = ""
        'Dim BillSoldToDt As MyOrderDS.ORDER_PARTNERSDataTable = aptOrderPartner.GetPartnerByOrderIDAndType(CartId, "B")
        'If BillSoldToDt.Count = 0 OrElse String.IsNullOrEmpty(BillSoldToDt(0).STREET) OrElse String.IsNullOrEmpty(BillSoldToDt(0).ZIPCODE) Then
        '    BillSoldToDt = aptOrderPartner.GetPartnerByOrderIDAndType(CartId, "SOLDTO")
        '    If BillSoldToDt.Count = 0 Then
        '        Return False
        '    End If
        'End If
        'txtBillToStreet = BillSoldToDt(0).STREET : txtBillToZip = BillSoldToDt(0).ZIPCODE
        'Dim CreditCardAuthResult1 As CreditCardAuthResult = AuthUtil.VerifyCreditCardByPayPalService( _
        '    PoNo, "", decTotalAmount, txtBillToStreet, txtBillToZip, CreditCardNum, _
        '    ExpDate, CvvCode, "USD")
        'With CreditCardAuthResult1
        '    Label1.Text = .cAUTHCODE
        '    Label2.Text = .cAVSADDR
        '    Label3.Text = .cAVSZIP
        '    Label4.Text = .cCVV2MATCH
        '    Label5.Text = .cDUPLICATE
        '    Label6.Text = .cFraud_POSTFPSMSG
        '    Label7.Text = .cFraud_PREFPSMSG
        '    Label8.Text = .cIAVS
        '    Label9.Text = .cPNREF
        '    Label10.Text = .cRESPMSG
        '    Label11.Text = .cRESULT
        '    Label12.Text = .cTransactionErrors
        'End With
        'If Label1.Text.Trim = "0" Then
        '    Return True
        'End If
        Return rlt
    End Function

    Sub SaveLog(ByVal Amount As Decimal, ByVal FirstName As String, ByVal LastName As String, ByVal BillToStreet As String, ByVal City As String, ByVal State As String, _
                ByVal BillToZip As String, ByVal PoNo As String, ByVal CreditCardNum As String, ByVal CvvCode As String, ByVal ExpDate As Date, ByVal validresult As String, _
                ByVal strFraudAlert As List(Of String))
        Dim strCmd As String = _
            " INSERT INTO MY_CC_LOG " + _
            " VALUES (@SESSIONID, @USERID, @AMOUNT, @FIRST_NAME, @LAST_NAME, @BILL_TO_STREET, @CITY, @STATE, @BILL_TO_ZIP, @PO_NO, @CARD_NUM, @CVV_CODE, @EXPIRED_DATE" + _
            " , @RESPONSE_RESULT, @PN_REFERENCE, @RESPONSE_MSG, @AUTHCODE, @AVS_ADDRESS, @AVS_ZIP, @IAVS, @CVV2MATCH, @DUPLICATE, @VALIDATE_TYPE, @FRAUD_MSG, @TIMESTAMP, @CLIENT_IP) "
        Dim cmd As New SqlClient.SqlCommand(strCmd, New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MYLOCAL").ConnectionString))
        With cmd.Parameters
            .AddWithValue("SESSIONID", HttpContext.Current.Session.SessionID) : .AddWithValue("USERID", HttpContext.Current.User.Identity.Name)
            .AddWithValue("AMOUNT", Amount) : .AddWithValue("FIRST_NAME", FirstName) : .AddWithValue("LAST_NAME", LastName)
            .AddWithValue("BILL_TO_STREET", BillToStreet) : .AddWithValue("CITY", City) : .AddWithValue("STATE", State)
            .AddWithValue("BILL_TO_ZIP", BillToZip) : .AddWithValue("PO_NO", PoNo) : .AddWithValue("CARD_NUM", CreditCardNum)
            .AddWithValue("CVV_CODE", CvvCode) : .AddWithValue("EXPIRED_DATE", ExpDate) : .AddWithValue("RESPONSE_RESULT", Label1.Text)
            .AddWithValue("PN_REFERENCE", Label2.Text) : .AddWithValue("RESPONSE_MSG", Label3.Text) : .AddWithValue("AUTHCODE", Label4.Text)
            .AddWithValue("AVS_ADDRESS", Label5.Text) : .AddWithValue("AVS_ZIP", Label6.Text) : .AddWithValue("IAVS", Label7.Text)
            .AddWithValue("CVV2MATCH", Label8.Text) : .AddWithValue("DUPLICATE", Label9.Text) : .AddWithValue("VALIDATE_TYPE", validresult)
            .AddWithValue("FRAUD_MSG", IIf(strFraudAlert.Count > 0, String.Join("<br />", strFraudAlert.ToArray()), ""))
            .AddWithValue("TIMESTAMP", Now) : .AddWithValue("CLIENT_IP", Util.GetClientIP())
        End With
        cmd.Connection.Open() : cmd.ExecuteNonQuery() : cmd.Connection.Close()
    End Sub

End Class