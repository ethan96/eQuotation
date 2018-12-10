Public Class CreateNewCust
    Inherits System.Web.UI.Page

    Public Function getDist_Chann() As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "select distinct DIST_CHANN from MyAdvantechGlobal.dbo.SAP_COMPANY_LOV WHERE ORG_ID='US01' ORDER BY DIST_CHANN")
        Return dt
    End Function

    Public Function getDivision() As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "select distinct DIVISION from MyAdvantechGlobal.dbo.SAP_COMPANY_LOV WHERE ORG_ID='US01' ORDER BY DIVISION")
        Return dt
    End Function

    Public Function getOfficeGroup() As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "SELECT distinct sales_office_code,sales_office,sales_group_code,sales_group FROM SAP_ORG_OFFICE_GRP WHERE SALES_org='US01' ORDER BY SALES_OFFICE,SALES_GROUP")
        Return dt
    End Function

    Public Function getShipCon() As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "select distinct shipterm , shiptermname from sap_company_lov where org_id='US01' AND SHIPTERM<>'' order by shiptermname")
        Return dt
    End Function


    Public Function getINCO() As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "select distinct isnull(INCO1,'') as INCO1 from SAP_DIMCOMPANY ORDER BY INCO1")
        Return dt
    End Function

    Public Function getPayment() As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "select distinct CREDIT_TERM from SAP_DIMCOMPANY WHERE org_id='US01' and CREDIT_TERM is not null and credit_term<>'' ORDER BY CREDIT_TERM")
        Return dt
    End Function

    Public Function getCountryRegion() As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "select distinct Country_Code,Country_Name,Region_Code,Region_Name from sap_country_region_lov order by Country_Name,Region_Name")
        Return dt

    End Function

    Public Function getDistrict() As DataTable
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", "select distinct BZIRK from SAPRDP.T171 WHERE MANDT='168' order by BZIRK")

        Return dt
    End Function
    Sub setDefault(ByVal CustName As String, ByVal Division As String, ByVal SalesOffice As String,
                   ByVal SalesGroup As String, ByVal District As String,
                   ByVal ICON1 As String, ByVal ICON2 As String,
                   ByVal TaxExempt As Integer, ByVal PaymentTerm As String,
                   ByVal ShipCon As String, ByVal Phone As String,
                   ByVal email As String, ByVal SCountry As String,
                   ByVal SZipCode As String, ByVal SRegion As String,
                   ByVal SAddress As String, ByVal SCity As String)

        Me.txtName.Text = CustName
        'Me.txtCustID.Text = Advantech.Myadvantech.Business.OrderBusinessLogic.GenerateNewUSShipToID(SCountry, SRegion, CustName, True)

        Dim dtDistChann As DataTable = getDist_Chann()
        Me.drpDistChannel.Items.Clear()
        Me.drpDistChannel.Items.Add(New ListItem("00", "00"))
        'If Not IsNothing(dtDistChann) Then
        '    For Each r As DataRow In dtDistChann.Rows
        '        Me.drpDistChannel.Items.Add(New ListItem(r.Item("DIST_CHANN"), r.Item("DIST_CHANN")))
        '    Next
        'End If

        Dim _reg As Advantech.Myadvantech.DataAccess.AOnlineRegion = Advantech.Myadvantech.DataAccess.AOnlineRegion.AUS_AOnline
        If Role.IsAonlineUsa Then
            _reg = Advantech.Myadvantech.DataAccess.AOnlineRegion.AUS_AOnline
        Else Role.IsAonlineUsaIag
            _reg = Advantech.Myadvantech.DataAccess.AOnlineRegion.AUS_AOnline_IAG
        End If

        Dim _defaultERPID As String = Advantech.Myadvantech.Business.OrderBusinessLogic.GenerateNewSoldToID_ForANASalesTeam(CustName, _reg, True)
        Me.txtCustID.Text = _defaultERPID

        Dim dtDivision As DataTable = getDivision()
        Me.drpDivision.Items.Clear()
        If Not IsNothing(dtDivision) Then
            For Each r As DataRow In dtDivision.Rows
                Me.drpDivision.Items.Add(New ListItem(r.Item("DIVISION"), r.Item("DIVISION")))
            Next
            For Each li As ListItem In Me.drpDivision.Items
                If li.Value = Division Then
                    li.Selected = True
                End If
            Next
        End If


        Dim dtOfficeGroup As DataTable = getOfficeGroup()
        Me.drpSalesOffice.Items.Clear()
        If Not IsNothing(dtOfficeGroup) Then
            For Each r As DataRow In dtOfficeGroup.Rows
                If IsNothing(Me.drpSalesOffice.Items.FindByValue(r.Item("sales_office_code").ToString)) Then
                    Me.drpSalesOffice.Items.Add(New ListItem(r.Item("sales_office") + " " + r.Item("sales_office_code"), r.Item("sales_office_code")))
                End If
            Next
            For Each li As ListItem In Me.drpSalesOffice.Items
                If li.Value = SalesOffice Then
                    li.Selected = True
                End If
            Next
        End If
        Me.drpSalesGroup.Items.Clear()
        If Not IsNothing(dtOfficeGroup) Then
            For Each r As DataRow In dtOfficeGroup.Rows
                If IsNothing(Me.drpSalesGroup.Items.FindByValue(r.Item("sales_group_code").ToString)) Then
                    Me.drpSalesGroup.Items.Add(New ListItem(r.Item("sales_group") + " " + r.Item("sales_group_code"), r.Item("sales_group_code")))
                End If
            Next

            For Each li As ListItem In Me.drpSalesGroup.Items
                If li.Value = SalesGroup Then
                    li.Selected = True
                End If
            Next
        End If

        Dim dtShipCon As DataTable = getShipCon()
        Me.drpShipCondition.Items.Clear()
        If Not IsNothing(dtShipCon) Then
            For Each r As DataRow In dtShipCon.Rows
                Me.drpShipCondition.Items.Add(New ListItem(r.Item("shiptermname"), r.Item("shipterm")))
            Next

            For Each li As ListItem In Me.drpShipCondition.Items
                If li.Value = ShipCon Then
                    li.Selected = True
                End If
            Next
        End If

        Dim dtINCO As DataTable = getINCO()
        Me.drpIncoterm.Items.Clear()
        If Not IsNothing(dtINCO) Then
            For Each r As DataRow In dtINCO.Rows
                Me.drpIncoterm.Items.Add(New ListItem(r.Item("INCO1"), r.Item("INCO1")))
            Next

            For Each li As ListItem In Me.drpIncoterm.Items
                If li.Value = ICON1 Then
                    li.Selected = True
                End If
            Next
        End If
        Dim dtPayment As DataTable = getPayment()
        Me.drpPaymentTerm.Items.Clear()
        If Not IsNothing(dtPayment) Then
            For Each r As DataRow In dtPayment.Rows
                Me.drpPaymentTerm.Items.Add(New ListItem(r.Item("CREDIT_TERM"), r.Item("CREDIT_TERM")))
            Next

            For Each li As ListItem In Me.drpPaymentTerm.Items
                If li.Value = PaymentTerm Then
                    li.Selected = True
                End If
            Next
        End If

        Dim dtCountryRegion As DataTable = getCountryRegion()
        Me.drpCountry.Items.Clear()
        Me.drpRegion.Items.Clear()

        If Not IsNothing(dtCountryRegion) Then
            For Each r As DataRow In dtCountryRegion.Rows
                If IsNothing(Me.drpCountry.Items.FindByValue(r.Item("country_code").ToString)) Then
                    Me.drpCountry.Items.Add(New ListItem(r.Item("Country_Name"), r.Item("Country_code")))
                End If
                For Each li As ListItem In Me.drpCountry.Items
                    If li.Value = SCountry Then
                        li.Selected = True
                    End If
                Next
                If IsNothing(Me.drpRegion.Items.FindByValue(r.Item("Region_code").ToString)) Then
                    Me.drpRegion.Items.Add(New ListItem(r.Item("Region_Name"), r.Item("Region_code")))
                End If
                For Each li As ListItem In Me.drpRegion.Items
                    If li.Value = SRegion Then
                        li.Selected = True
                    End If
                Next
            Next
        End If

        Dim dtDistrict As DataTable = getDistrict()
        Me.drpSalesDistrict.Items.Clear()
        If Not IsNothing(dtDistrict) Then
            For Each r As DataRow In dtDistrict.Rows
                Me.drpSalesDistrict.Items.Add(New ListItem(r.Item("BZIRK"), r.Item("BZIRK")))
            Next
            For Each li As ListItem In Me.drpSalesDistrict.Items
                If li.Value = District Then
                    li.Selected = True
                End If
            Next
        End If
        Me.txtStreet.Text = SAddress
        Me.txtCity.Text = SCity
        Me.txtPostalCode.Text = SZipCode
        Me.txtJurisdiction.Text = SRegion & SZipCode
        Me.txtPhone.Text = Phone
        Me.txtIncoterms2.Text = ICON2
    End Sub

    Function getSiebelAcc(ByVal RowID As String) As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "select * from Siebel_Account where ROW_ID = '" & RowID & "'")
        Return dt
    End Function
    Function getCountryCode(ByVal RowId As String) As String
        Dim str As String = "select top 1 b.country_code from siebel_account A inner join SAP_COUNTRY_REGION_LOV B on a.country=b.country_name where A.row_id='" & RowId & "'"
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", str)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If Not IsDBNull(dt.Rows(0).Item("country_code")) Then
                Return dt.Rows(0).Item("country_code")
            End If
        End If
        Return ""
    End Function

    Function getRegion(ByVal RowId As String) As String
        Dim str As String = "select [State] from siebel_account where row_id='" & RowId & "'"
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", str)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If Not IsDBNull(dt.Rows(0).Item("State")) Then
                Return dt.Rows(0).Item("State")
            End If
        End If
        Return ""
    End Function
    Function getZipCode(ByVal RowId As String) As String
        Dim str As String = "select [ZIPCODE] from siebel_account where row_id='" & RowId & "'"
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", str)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If Not IsDBNull(dt.Rows(0).Item("ZIPCODE")) Then
                Return dt.Rows(0).Item("ZIPCODE")
            End If
        End If
        Return ""
    End Function
    Function getAddress(ByVal RowId As String) As String
        Dim str As String = "select [Address] from siebel_account where row_id='" & RowId & "'"
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", str)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If Not IsDBNull(dt.Rows(0).Item("Address")) Then
                Return dt.Rows(0).Item("Address")
            End If
        End If
        Return ""
    End Function

    Function getCity(ByVal RowId As String) As String
        Dim str As String = "select [City] from siebel_account where row_id='" & RowId & "'"
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", str)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If Not IsDBNull(dt.Rows(0).Item("City")) Then
                Return dt.Rows(0).Item("City")
            End If
        End If
        Return ""
    End Function
    Dim _QID As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not String.IsNullOrEmpty(Request("QuoteID")) Then
            _QID = Request("QuoteID")
        End If
        If Not IsPostBack Then
            Dim QM As Advantech.Myadvantech.DataAccess.QuotationMaster = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMaster(_QID)
            Dim SiebelInfo As DataTable = getSiebelAcc(QM.quoteToRowId)

            setDefault(QM.quoteToName, QM.DIVISION, QM.SALESOFFICE, QM.SALESGROUP, QM.DISTRICT, QM.INCO1, QM.INCO2, QM.isExempt, QM.paymentTerm, QM.shipTerm, QM.directPhone, "", getCountryCode(QM.quoteToRowId), getZipCode(QM.quoteToRowId), getRegion(QM.quoteToRowId), getAddress(QM.quoteToRowId), getCity(QM.quoteToRowId))



        End If
    End Sub

    Function createCust() As String
        Dim AccType As String = Me.drpCustAccGroup.SelectedValue
        Dim CustId As String = Me.txtCustID.Text.Trim
        Dim SalesOrg As String = Me.txtSalesOrg.Text.Trim
        Dim DistChannel As String = Me.drpDistChannel.SelectedValue
        Dim Division As String = Me.drpDivision.SelectedValue
        Dim Name1 As String = Me.txtName.Text.Trim
        Dim Street As String = Me.txtStreet.Text.Trim
        Dim City As String = Me.txtCity.Text.Trim
        Dim Region As String = Me.drpRegion.SelectedValue
        Dim PostCode As String = Me.txtPostalCode.Text.Trim
        Dim Country As String = Me.drpCountry.SelectedValue
        Dim TaxJuris As String = Me.txtJurisdiction.Text.Trim
        Dim Tel As String = Me.txtPhone.Text.Trim
        Dim Email As String = Me.txtEmail.Text.Trim
        Dim Vat As String = Me.txtVAT.Text.Trim
        Dim SDistrict As String = Me.drpSalesDistrict.SelectedValue
        Dim SOffice As String = Me.drpSalesOffice.SelectedValue
        Dim SGroup As String = Me.drpSalesGroup.SelectedValue
        Dim Currency As String = Me.txtCurrency.Text.Trim
        Dim ShipCon As String = Me.drpShipCondition.SelectedValue
        Dim Inco As String = Me.drpIncoterm.SelectedValue
        Dim Inco2 As String = Me.txtIncoterms2.Text.Trim
        Dim PTerm As String = Me.drpPaymentTerm.SelectedValue
        Dim TaxExempt As Integer = Me.drpTaxExempt.SelectedValue
        'Dim pipeline As New Advantech.Myadvantech.DataAccess.Pipeline.general()
        Dim sapnewcust As New SAPDAL.SAPDAL()
        Dim msg As String = ""
        Try
            'pipeline.CreateNewCust(AccType, CustId, SalesOrg, DistChannel, Division, Name1, "", "", Name1, Name1, Street,
            '"", "", City, Region, PostCode, Country, TaxJuris, "",
            '"", Tel, "", Email, "", "", "", "", Vat, "",
            '"", "", "", SDistrict, SOffice, SGroup, "", Currency, 0, "", ShipCon, "", Inco,
            'Inco2, PTerm, "", TaxExempt, "", msg)

            'Frank 20180710 move Nada's create new SAP account from web service to SAPDAL.SAPDAL
            SAPDAL.SAPDAL.CreateNewCust(AccType, CustId, SalesOrg, DistChannel, Division, Name1, "", "", Name1, Name1, Street,
            "", "", City, Region, PostCode, Country, TaxJuris, "",
            "", Tel, "", Email, "", "", "", "", Vat, "",
            "", "", "", SDistrict, SOffice, SGroup, "", Currency, 0, "", ShipCon, "", Inco,
            Inco2, PTerm, "", TaxExempt, "", msg)

            Return msg
        Catch ex As Exception
            msg = ex.Message : Return msg
        End Try

        'Me.lbMsg.Text = msg
    End Function
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim msg As String = ""
        Dim msg1 As String = ""
        Dim msg2 As String = ""
        If Advantech.Myadvantech.Business.OrderBusinessLogic.isERPIDExist(Me.txtCustID.Text.Trim) Then
            msg = "Customer ID already exists."
        End If
        If msg = "" Then
            msg = createCust()
            If msg.ToUpper.Contains(("ErrType:S").ToUpper) Then
                msg1 = updateQuoteBack(Me.txtCustID.Text.Trim, _QID)
                Dim QM As Advantech.Myadvantech.DataAccess.QuotationMaster = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMaster(_QID)
                msg2 = updateSiebelAcc(Me.txtCustID.Text.Trim, QM.quoteToRowId)
            End If

        End If

        Me.lbMsg.Text = msg & "<br/>" & msg1 & "<br/>" & msg2
    End Sub

    Function updateQuoteBack(ByVal ERPID As String, ByVal QUOTEID As String)
        Try
            Dim int As Integer = tbOPBase.dbExecuteNoQuery("EQ", "UPDATE QUOTATIONMASTER SET QUOTETOERPID='" & ERPID & "' WHERE QUOTEID='" & QUOTEID & "'")
            If int = 1 Then
                Return "Update Quotation Success."
            Else
                Return "Update Quotation Failed."
            End If
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    Public Function updateSiebelAcc(ByVal ERPID As String, ByVal ROWID As String)
        Dim strMeth = New UpdateSiebelAcc.ADVWebServiceUpdAccount()
        Dim strAccount = New UpdateSiebelAcc.ACC()
        Dim AccInput = New UpdateSiebelAcc.UpdAccount_Input()
        Dim AccOutput = New UpdateSiebelAcc.UpdAccount_Output()

        strAccount.ROW_ID = ROWID ' It 's Required.
        strAccount.ERPID = ERPID
        AccInput.ACC = strAccount
        AccInput.SOURCE = "MTL" 'It's Required.

        Try
            AccOutput = strMeth.UpdAccount(AccInput)
            Dim strStatus As String = AccOutput.STATUS
            If (strStatus = "SUCCESS") Then
                Return "Update Siebel Account Success."
            Else
                Return String.Format("Update Siebel account failed! Error message: {0}, {1}", AccOutput.Error_spcCode, AccOutput.Error_spcMessage)

            End If

        Catch ex As Exception
            Return String.Format("Message: {0}", ex.ToString())
        End Try
    End Function
End Class