Public Class QuoteMaster
    Inherits System.Web.UI.UserControl

    Private _UID As String = String.Empty
    Public Property UID As String
        Get
            Return _UID
        End Get
        Set(ByVal value As String)
            _UID = value
        End Set
    End Property
    Private _QM As IBUS.iDocHeaderLine = Nothing
    Public ReadOnly Property QM As IBUS.iDocHeaderLine
        Get
            Return Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        End Get
    End Property

    Public logoImg As String = "heard.jpg" 'Frank:Sorry I really don't who named this as header file name @@

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        If Not IsPostBack Then
            If Not IsNothing(QM) Then

                Dim _SalesCode As String = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetSalesCodeByQuoteID(UID)
                If Advantech.Myadvantech.Business.UserRoleBusinessLogic.IsBBIrelandBySalesID(_SalesCode) Then
                    logoImg = "heardBB.jpg"
                End If

                LitQuoteNo.Text = QM.quoteNo

                LitCustomerNo.Text = QM.AccRowId
                LitExpiredDate.Text = QM.expiredDate.ToString("dd.MM.yyyy")
                LitCreatedDate.Text = QM.CreatedDate.ToString("dd.MM.yyyy")
                'LitPaymentTerms.Text = QM.paymentTerm
                LitPaymentTerms.Text = SAPTools.GetPaymentMethodNameByValue(QM.paymentTerm)
                'LitShippingMethod.Text = "EX-works Eindhoven" 'QM.shipTerm
                LitShippingMethod.Text = SAPTools.GetShipMethodNameByValue(QM.shipTerm)
                LitERPID.Text = QM.AccErpId
                LitPONumber.Text = QM.PO_NO

                If Not String.IsNullOrEmpty(QM.salesEmail) AndAlso QM.salesEmail.IndexOf("@") > 0 Then
                    LitAccountManager.Text = QM.salesEmail.Substring(0, QM.salesEmail.IndexOf("@"))
                    LitSales.Text = LitAccountManager.Text
                    Dim dt As DataTable = Relics.dbUtil.dbGetDataTable("MY", String.Format("SELECT top 1 ISNULL(workphone,'') as tel , ISNULL(FaxNumber,'') as fax FROM  SIEBEL_CONTACT where EMAIL_ADDRESS ='{0}'", QM.salesEmail))
                    If dt.Rows.Count = 1 Then
                        With dt.Rows(0)
                            If Not String.IsNullOrEmpty(.Item("tel")) Then
                                'LitAccountManagerTel.Text = "Tel: " + vbTab + .Item("tel")
                                Dim _telarr() As String = .Item("tel").ToString.Split(vbLf)
                                LitAccountManagerTel.Text = "Tel: " + vbTab + "<font class='QMtext2'>" + _telarr(0) + "</font>"
                            End If
                            If Not String.IsNullOrEmpty(.Item("fax")) Then
                                'LitAccountManagerTel.Text += vbCrLf + "Fax: " + vbTab + .Item("fax")
                                Dim _faxarr() As String = .Item("fax").ToString.Split(vbLf)
                                LitAccountManagerTel.Text += "<br/>" + "Fax: " + vbTab + "<font class='QMtext2'>" + _faxarr(0) + "</font>"
                            End If
                        End With
                    End If
                    LitAccountManager.Text += "<br/>" + QM.salesEmail
                End If
                Dim QuoteExtensionline As IBUS.iDOCHeaderExtensionLine = Pivot.NewObjDocHeaderExtension.GetQuoteExtension(UID)
                If Not IsNothing(QuoteExtensionline) Then
                    With QuoteExtensionline

                        If String.IsNullOrEmpty(.EmailGreeting.Trim) Then
                            LitGreeting.Visible = False
                        Else
                            'LitGreeting.Text = .EmailGreeting.Trim.Replace(Environment.NewLine, "<br/>")
                            LitGreeting.Text = .EmailGreeting.Trim
                        End If


                        'oSpecialTandC = .SpecialTandC
                        If String.IsNullOrEmpty(.SignatureRowID.Trim) Then
                            ImgSignature.Visible = False
                        Else
                            ImgSignature.ImageUrl = String.Format("{0}/Ascx/DisplayImageHandler.ashx?Type=signature&ImageID={1}", Util.GetRuntimeSiteIP(), .SignatureRowID)
                        End If
                    End With
                End If
                Dim P As List(Of IBUS.iPartnerLine) = Pivot.NewObjPartner.GetListAll(QM.Key, COMM.Fixer.eDocType.EQ)
                If P IsNot Nothing Then
                    For Each r As IBUS.iPartnerLine In P
                        If r.TYPE.Equals("SOLDTO", StringComparison.OrdinalIgnoreCase) Then
                            'litsoldto.Text = r.ERPID
                            'litsoldtocompany.Text = r.NAME
                            LitCustomerName.Text = r.NAME
                            'Litcompany.Text = r.NAME
                            litsoldtoaddress.Text = r.STREET + " " + r.CITY + " " + r.STATE + " " + r.COUNTRY + " " + r.ZIPCODE
                            'litsoldtotel.Text = r.TEL
                            'litsoldtoattention.Text = r.ATTENTION
                        End If
                        If r.TYPE.Equals("S", StringComparison.OrdinalIgnoreCase) Then
                            'litshipto.Text = r.ERPID
                            litshiptocompany.Text = r.NAME
                            litshiptoaddress.Text = r.STREET + " " + r.CITY + " " + r.STATE + " " + r.COUNTRY + " " + r.ZIPCODE
                            'litshiptoaddress2.Text = r.STREET2
                            'litshiptotel.Text = r.TEL
                            'litshiptoattention.Text = r.ATTENTION
                        End If
                        If r.TYPE.Equals("B", StringComparison.OrdinalIgnoreCase) Then
                            'litshipto.Text = r.ERPID
                            litBilltocompany.Text = r.NAME
                            litBilltoaddress.Text = r.STREET + " " + r.CITY + " " + r.STATE + " " + r.COUNTRY + " " + r.ZIPCODE
                            'litshiptoaddress2.Text = r.STREET2
                            'litshiptotel.Text = r.TEL
                            'litshiptoattention.Text = r.ATTENTION
                        End If
                    Next
                End If

            End If
        End If
    End Sub

End Class