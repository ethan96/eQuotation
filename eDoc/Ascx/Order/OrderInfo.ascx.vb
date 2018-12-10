Public Class OrderInfo1
    Inherits System.Web.UI.UserControl

     Private _QM As IBUS.iDocHeaderLine = Nothing
    Public Property QM As IBUS.iDocHeaderLine
        Get
            Return _QM
        End Get
        Set(ByVal value As IBUS.iDocHeaderLine)
            _QM = value
        End Set
    End Property
    Public Shared Function GetPaymentMethodNameByValue(ByVal PaymentMethodValue As String) As String
        'If PaymentMethodValue.Equals("0") Then Return "TBD"
        Dim retObj As Object = Nothing
        Dim cmd As New SqlClient.SqlCommand("select top 1 PAYMENTTERMNAME from SAP_COMPANY_LOV where PAYMENTTERM=@SV", _
                                            New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
        cmd.Parameters.AddWithValue("SV", PaymentMethodValue)
        cmd.Connection.Open() : retObj = cmd.ExecuteScalar() : cmd.Connection.Close()
        If retObj IsNot Nothing Then
            Return retObj.ToString()
        End If
        Return PaymentMethodValue
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Me.trEUOPN.Visible = False

            If QM.org IsNot Nothing AndAlso QM.org.ToString.Equals("EU10", StringComparison.OrdinalIgnoreCase) Then
                Me.trEUOPN.Visible = True
            End If
            If Role.IsUsaUser Then Me.trPartial.Visible = False
            If Not IsNothing(QM) Then
                With QM

                    Dim QuoteDT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(.OriginalQuoteID, COMM.Fixer.eDocType.EQ)
                    Me.lbQuoteNo.Text = QuoteDT.quoteNo
                    If QuoteDT.quoteNo <> QuoteDT.Key Then
                        Me.lbQuoteNo.Text &= " Version " & QuoteDT.Revision_Number
                    End If
                    QuoteDT = Nothing

                    Me.lbPO.Text = .PO_NO
                    Dim SONO As String = .Key
                    Me.lbOrderDate.Text = CDate(.DocDate).ToString("MM/dd/yyyy")
                    Me.lbPayTerm.Text = GetPaymentMethodNameByValue(.paymentTerm)
                    Me.lbReqdate.Text = CDate(.ReqDate).ToString("MM/dd/yyyy")
                    Me.lbIncoterm.Text = GetIncotermName(.Inco1) + " " + .Inco2
                    Me.lbPlacedBy.Text = .CreatedBy
                    Me.lbFreight.Text = .freight
                    Dim OP As IBUS.iPartner = Pivot.NewObjPartner
                    Dim LOPDT As List(Of IBUS.iPartnerLine) = OP.GetListAll(.Key, COMM.Fixer.eDocType.ORDER)
                    Dim OPdt As IBUS.iPartnerLine = LOPDT.Where(Function(x) x.TYPE = "E").FirstOrDefault
                    Dim Sales_Code As String = String.Empty
                    If Not IsNothing(OPdt) Then Sales_Code = OPdt.ERPID
                    Me.lbSalesRep.Text = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Sales_Code, .CreatedBy)
                    If Double.TryParse(Me.lbFreight.Text, 0) AndAlso CDbl(Me.lbFreight.Text) = 0 Then Me.lbFreight.Text = "TBD"
                    If CInt(.PartialF) = 1 Then
                        Me.lbisPartial.Text = "Yes"
                    ElseIf CInt(.PartialF) = 0 Then
                        Me.lbisPartial.Text = "No"
                    End If
                    Me.lbShipCond.Text = Pivot.NewObjDoc.shipCode2Txt(.shipTerm)
                    Dim txt As List(Of IBUS.iDocTextLine) = Pivot.NewObjDocText.GetListAll(.Key)
                    If Not IsNothing(txt) Then
                        For Each r As IBUS.iDocTextLine In txt
                            If r.Type = "0002" Then
                                Me.lbOrderNote.Text = r.Txt
                            End If
                            If r.Type = "0001" Then
                                Me.lbSalesNote.Text = r.Txt
                            End If
                            If r.Type = "ZEOP" Then
                                Me.lbOPNote.Text = r.Txt
                            End If
                        Next
                    End If
                End With
                Me.lbSO.Text = QM.quoteNo
                If QM.DocType = CInt(COMM.Fixer.eDocType.ORDER) Then
                    litSO.Text = "Advantech Order No:"
                End If
            End If
        End If
    End Sub
    Public Shared Function GetIncotermName(ByVal IncotermID As String) As String
        Dim SQL As String = String.Format(" select tinct.BEZEI from  saprdp.tinct where inco1='{0}' AND SPRAS='E' AND ROWNUM =1", IncotermID)
        Dim Incoterm As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", SQL)
        If Incoterm IsNot Nothing Then
            Return Incoterm
        End If
        Return ""
    End Function
End Class