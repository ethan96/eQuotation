Imports System.Reflection
Public Class PickAccount
    Inherits System.Web.UI.UserControl

    Private _IsAUSAENC As Boolean = False
    Private _IsCAPS As Boolean = False
    Private _IsAJP As Boolean = False
    Private _IsAUSAAC As Boolean = False
    Private _IsABR As Boolean = False
    Private _IsATWAiST As Boolean = False
    Private _UserID As String = String.Empty
    Public ReadOnly Property TxtAccountName As TextBox
        Get
            Return Me.txtName
        End Get
    End Property

    Public Sub getData(ByVal Name As String, ByVal RBU As String, ByVal erpid As String, _
                                            ByVal country As String, ByVal location As String, ByVal STATE As String, _
                                            ByVal PROVINCE As String, ByVal status As String, ByVal address1 As String, ByVal Zip As String, ByVal City As String, ByVal priSales As String)
        Dim SQLSTR As String = ""
        Dim dt As DataTable = Nothing
        Dim dt_AAC As DataTable = Nothing
        'Frank 2016/03/17
        If _IsAUSAENC OrElse _IsCAPS OrElse _IsAJP Then
            Dim _QueryORGID As String = String.Empty
            Dim _SalesOffice As String = String.Empty
            If _IsAUSAENC Then
                _QueryORGID = "US01"
                _SalesOffice = "2300"
                dt = SAPTools.SearchSAPCustomerForPickAccount(erpid, _QueryORGID, Name, "", _SalesOffice, "", address1, STATE, Zip)

                'Also include 2200 account data for AENC
                Dim dt_AENC As DataTable = Nothing
                _SalesOffice = "2200"
                dt_AENC = SAPTools.SearchSAPCustomerForPickAccount(erpid, _QueryORGID, Name, "", _SalesOffice, "", address1, STATE, Zip)
                dt.Merge(dt_AENC)
            End If
            If _IsCAPS Then
                _QueryORGID = "TW01"
                _SalesOffice = ""
                dt = SAPTools.SearchSAPCustomerForPickAccount(erpid, _QueryORGID, Name, "", _SalesOffice, "", address1, STATE, Zip)
            End If
            If _IsAJP Then
                _QueryORGID = "JP01"
                _SalesOffice = ""
                dt = SAPTools.SearchSAPCustomerForPickAccount(erpid, _QueryORGID, Name, "", _SalesOffice, "", address1, STATE, Zip)
            End If
        Else


            If hType.Value = "ALL" Then

                'SQLSTR = SiebelTools.GET_Siebel_Account_List(Name, RBU, erpid, country, location, STATE, PROVINCE, status, address1, Zip, City, priSales)
                'Frank 20160921: Get erpid if its tax number 1 matches the inputted query condition "TaxNumber1"
                If _IsABR Then
                    SQLSTR = SiebelTools.GET_Siebel_Account_ListForABR(Name, RBU, erpid, country, location, STATE, PROVINCE, status, address1, Zip, City, priSales, GetSafeStr(txtTaxNumber1.Text))
                ElseIf _IsATWAiST Then
                    SQLSTR = SiebelTools.GET_Siebel_Account_ListForAiST(Name, RBU, erpid, country, location, STATE, PROVINCE, status, address1, Zip, City, priSales)
                Else
                    SQLSTR = SiebelTools.GET_Siebel_Account_List(Name, RBU, erpid, country, location, STATE, PROVINCE, status, address1, Zip, City, priSales)
                End If

            End If
            'TEST.Text = SQLSTR
            'Dim dt As DataTable = tbOPBase.dbGetDataTable("CRM", SQLSTR)
            dt = tbOPBase.dbGetDataTable("CRM", SQLSTR)


            If _IsAUSAAC Then
                SQLSTR = SiebelTools.GET_Siebel_Account_List_ForAAC(Name, RBU, erpid, country, location, STATE, PROVINCE, status, address1, Zip, City, _UserID)
                dt_AAC = tbOPBase.dbGetDataTable("CRM", SQLSTR)
                dt.Merge(dt_AAC)
            End If

        End If

        Me.GridView1.DataSource = dt

    End Sub

    Public Sub ShowData(ByVal Name As String, ByVal RBU As String, ByVal erpid As String, _
                                            ByVal country As String, ByVal location As String, ByVal STATE As String, ByVal PROVINCE As String, _
                                            ByVal status As String, ByVal address1 As String, ByVal Zip As String, ByVal City As String, ByVal priSales As String)
        getData(Name, RBU, erpid, country, location, STATE, PROVINCE, status, address1, Zip, City, priSales)
        Me.GridView1.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        InvokeShowData()
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        'Frank 20160317
        If _IsAUSAENC OrElse _IsCAPS OrElse _IsAJP Then
            If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Then
                'AENC sales team does not need to see account status from Siebel
                e.Row.Cells(4).Visible = False
            End If
        End If
    End Sub


    Sub InvokeShowData()
        Dim RBU_Str As String = ""
        For i As Integer = 0 To cblRBU.Items.Count - 1
            If cblRBU.Items(i).Selected Then RBU_Str += GetSafeStr(cblRBU.Items(i).Value) + ","
        Next

        ''ICC 2015/5/4 Special rule for AENC user (CP & KA)
        'If Role.IsUsaAenc() Then
        '    GridView1.DataSource = Advantech.Myadvantech.Business.SiebelBusinessLogic.GetAencSiebelAccountList(GetSafeStr(Me.txtName.Text), RBU_Str.Trim(","), _
        '       GetSafeStr(Me.txtID.Text), GetSafeStr(Me.txtCounrty.Text), _
        '       GetSafeStr(Me.txtLocation.Text), GetSafeStr(Me.txtState.Text), _
        '       GetSafeStr(Me.txtProvince.Text), Me.drpStatus.SelectedValue, _
        '       GetSafeStr(Me.txtAddress1.Text), GetSafeStr(txtZip.Text), _
        '       GetSafeStr(txtcity.Text), GetSafeStr(txtPri.Text), Pivot.CurrentProfile.VisibleRBU)
        '    GridView1.DataBind()
        '    Exit Sub
        'End If
        'ShowData(GetSafeStr(Me.txtName.Text), RBU_Str.Trim(","), _
        '       GetSafeStr(Me.txtID.Text), GetSafeStr(Me.txtCounrty.Text), _
        '       GetSafeStr(Me.txtLocation.Text), GetSafeStr(Me.txtState.Text), _
        '       GetSafeStr(Me.txtProvince.Text), Me.drpStatus.SelectedValue, _
        '       GetSafeStr(Me.txtAddress1.Text), GetSafeStr(txtZip.Text), _
        '       GetSafeStr(txtcity.Text), GetSafeStr(txtPri.Text))

        Dim _account_status As String = Me.drpStatus.SelectedValue
        If _account_status.Equals("N/A", StringComparison.InvariantCultureIgnoreCase) Then
            _account_status = ""
        End If

        ShowData(GetSafeStr(Me.txtName.Text), RBU_Str.Trim(","), _
                GetSafeStr(Me.txtID.Text), GetSafeStr(Me.txtCounrty.Text), _
                GetSafeStr(Me.txtLocation.Text), GetSafeStr(Me.txtState.Text), _
                GetSafeStr(Me.txtProvince.Text), _account_status, _
                GetSafeStr(Me.txtAddress1.Text), GetSafeStr(txtZip.Text), _
                GetSafeStr(txtcity.Text), GetSafeStr(txtPri.Text))
    End Sub
    Protected Function GetSafeStr(ByVal Str As String) As String
        'If Str.Trim = "'" Then Return ""
        Return Replace(Str.Trim.Replace("'", "''"), "*", "%")
    End Function


    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtState.Text = Me.txtProvince.Text
        InvokeShowData()
    End Sub

    'Public leftSize As String = "-50px"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim MG As New PROF.MailGroup
        Dim GroupBelTo As ArrayList = MG.getMailGroupArray(Pivot.CurrentProfile.UserId)

        _IsAUSAENC = Role.IsUsaAenc()
        _IsCAPS = Role.IsCAPS
        _IsAJP = Role.IsJPAonlineSales
        _IsAUSAAC = Role.IsUSAACSales()
        _IsABR = Role.IsABRSales()
        _IsATWAiST = Role.IsATWAiSTSales(GroupBelTo)
        _UserID = Pivot.CurrentProfile.UserId

        If Not IsPostBack Then


            Dim RBUArrayList As ArrayList = Pivot.CurrentProfile.VisibleRBU
            If RBUArrayList.Count > 0 Then
                For i As Integer = 0 To RBUArrayList.Count - 1
                    cblRBU.Items.Add(New ListItem(UCase(RBUArrayList.Item(i))))
                Next
                If RBUArrayList.Count < 3 Then
                    cblRBU.RepeatColumns = RBUArrayList.Count
                    ViewState("leftSize") = "-20px"
                    If RBUArrayList.Count = 1 Then
                        ViewState("leftSize") = "-5px"
                    End If
                End If
            End If

            'ICC 2015/5/4 AENC users can not see GA account
            'If _IsAUSAENC Then
            If _IsAUSAENC OrElse _IsCAPS OrElse _IsAJP Then

                'Frank 2016/03/17 AENC team did not use Siebel, so disable any fields or functions related to Siebel.
                Me.drpStatus.Enabled = False

                'For i As Integer = 0 To drpStatus.Items.Count - 1
                '    If i >= 8 Then
                '        drpStatus.Items(i).Enabled = False
                '    End If
                'Next

                Me.rbutd.Visible = False : lbtnChoose.Visible = False

            End If

            If _IsABR Then
                Me.trTaxNumber1.Visible = True
            End If


            'gvSAPAccount.EmptyDataText = "No Account found"
            'dlOrg_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub
    'Public Function GetleftSize() As String
    '    If ViewState("leftSize") IsNot Nothing Then
    '        Return ViewState("leftSize").ToString()
    '    End If
    '    Return "-50px"
    'End Function
    'Protected Sub gvSAPAccount_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
    '    Me.gvSAPAccount.PageIndex = e.NewPageIndex
    '    Me.DoQuery()
    'End Sub
    'Protected Sub dlOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim apt As New SqlClient.SqlDataAdapter("select distinct DIVISION as Value from SAP_COMPANY_LOV where ORG_ID='" + dlOrg.SelectedValue + "' order by DIVISION", _
    '                                            ConfigurationManager.ConnectionStrings("B2B").ConnectionString)
    '    Dim dtDivision As New DataTable, dtGrp As New DataTable, dtOffice As New DataTable, _NewRow As DataRow
    '    apt.Fill(dtDivision)
    '    _NewRow = dtDivision.NewRow : _NewRow.Item("Value") = "Select..." : dtDivision.Rows.InsertAt(_NewRow, 0)

    '    apt.SelectCommand.CommandText = "select distinct SALESGROUP as Value from SAP_COMPANY_LOV where ORG_ID='" + dlOrg.SelectedValue + "' and SALESGROUP<>'' order by SALESGROUP"
    '    If apt.SelectCommand.Connection.State <> ConnectionState.Open Then apt.SelectCommand.Connection.Open()
    '    apt.Fill(dtGrp)
    '    _NewRow = dtGrp.NewRow : _NewRow.Item("Value") = "Select..." : dtGrp.Rows.InsertAt(_NewRow, 0)

    '    apt.SelectCommand.CommandText = "select distinct SALESOFFICE as Value from SAP_COMPANY_LOV where ORG_ID='" + dlOrg.SelectedValue + "' and SALESOFFICE<>'' order by SALESOFFICE"
    '    If apt.SelectCommand.Connection.State <> ConnectionState.Open Then apt.SelectCommand.Connection.Open()
    '    apt.Fill(dtOffice)
    '    _NewRow = dtOffice.NewRow : _NewRow.Item("Value") = "Select..." : dtOffice.Rows.InsertAt(_NewRow, 0)

    '    apt.SelectCommand.Connection.Close()
    '    dlDivision.DataSource = dtDivision : dlSalesGrp.DataSource = dtGrp : dlSalesOffice.DataSource = dtOffice
    '    dlDivision.DataBind() : dlSalesGrp.DataBind() : dlSalesOffice.DataBind()
    '    If dlOrg.SelectedValue = "US01" Then
    '        Dim liDivDoubleZero As ListItem = dlDivision.Items.FindByValue("00")
    '        If liDivDoubleZero IsNot Nothing Then dlDivision.Items.Remove(liDivDoubleZero)
    '    End If
    'End Sub


    'Protected Sub btnSearchSAP_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Me.DoQuery()
    'End Sub


    'Public Sub DoQuery()
    '    Dim _dlState As String = "", _dlDivision As String = "", _dlSalesOffice As String = "", _dlSalesGrp As String = ""
    '    If Me.dlState.SelectedIndex > 0 Then _dlState = dlState.SelectedValue
    '    If Me.dlDivision.SelectedIndex > 0 Then _dlDivision = dlDivision.SelectedValue
    '    If Me.dlSalesOffice.SelectedIndex > 0 Then _dlSalesOffice = dlSalesOffice.SelectedValue
    '    If Me.dlSalesGrp.SelectedIndex > 0 Then _dlSalesGrp = dlSalesGrp.SelectedValue

    '    gvSAPAccount.DataSource = SAPTools.SearchSAPCustomer(txtCompanyId.Text, dlOrg.SelectedValue, txtCompanyName.Text, _dlDivision, _
    '                               _dlSalesOffice, _dlSalesGrp, txtAddr.Text, _dlState, txtZip.Text)
    '    gvSAPAccount.DataBind()
    'End Sub

    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As Object = Me.GridView1.DataKeys(row.RowIndex).Values
        Dim P As Page = Me.Parent.Page
        Dim TP As Type = P.GetType()
        Dim MI As MethodInfo = Nothing
        If hType.Value = "ALL" Then
            MI = TP.GetMethod("PickAccountEnd")
        End If


        Dim para(0) As Object
        para(0) = key
        MI.Invoke(P, para)
    End Sub
    'Protected Sub lnkPickSapCompanyId_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim o As LinkButton = CType(sender, LinkButton)
    '    Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
    '    Dim key As Object = Me.gvSAPAccount.DataKeys(row.RowIndex).Values
    '    Dim P As Page = Me.Parent.Page
    '    Dim TP As Type = P.GetType()
    '    Dim MI As MethodInfo = Nothing
    '    If hType.Value = "ALL" Then
    '        MI = TP.GetMethod("PickAccountEnd")
    '    End If


    '    Dim para(0) As Object
    '    para(0) = key
    '    MI.Invoke(P, para)
    'End Sub

End Class