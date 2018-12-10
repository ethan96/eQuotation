Imports System.Reflection
Public Class PickShipToBillToSAP
    Inherits System.Web.UI.UserControl

    Public Sub ShowData()
        '    hPERPID.Value = PERPID : hORG.Value = ORG
        '    If Not String.IsNullOrEmpty(hPERPID.Value) Then txtERPID.Text = hPERPID.Value
        Me.GridView1.DataSource = SAPTools.SearchAllSAPCompanySoldBillShipTo(txtERPID.Text, hORG.Value, txtAccountName.Text, "", "", hdDivision.Value, "", "", hType.Value, Me.hIsAll.Value)
        Me.GridView1.DataBind()
    End Sub
    'Function getComType() As String
    '    Dim comType As String = ""
    '    If hType.Value = "SHIP" Then
    '        comType = "Z002"
    '    Else
    '        comType = "Z003"
    '    End If
    '    Return comType
    'End Function
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        Me.GridView1.PageIndex = e.NewPageIndex
        Me.GridView1.DataSource = SAPTools.SearchAllSAPCompanySoldBillShipTo(txtERPID.Text, hORG.Value, txtAccountName.Text, "", "", hdDivision.Value, "", "", hType.Value, Me.hIsAll.Value)
        Me.GridView1.DataBind()
    End Sub

    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton), row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As Object = Me.GridView1.DataKeys(row.RowIndex).Values, P As Page = Me.Parent.Page
        Dim TP As Type = P.GetType(), MI As MethodInfo = Nothing
        If hType.Value = "SHIP" Then
            MI = TP.GetMethod("PickSHIPEnd")
        Else
            If hType.Value = "BILL" Then
                MI = TP.GetMethod("PickBILLEnd")
            ElseIf hType.Value = "EM" Then
                MI = TP.GetMethod("PickEMEnd")
            End If
        End If
        Dim para(0) As Object
        para(0) = key : MI.Invoke(P, para)
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.hIsAll.Value = "Y"
        Me.GridView1.DataSource = SAPTools.SearchAllSAPCompanySoldBillShipTo(txtERPID.Text, hORG.Value, txtAccountName.Text, txtAddress.Text, "", hdDivision.Value, "", "", hType.Value, Me.hIsAll.Value)
        Me.GridView1.DataBind()
        GridView1.Visible = True
    End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    GridView1.Visible = False
    '    If Not Page.IsPostBack Then

    '    End If
    'End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lBtnPick As LinkButton = e.Row.FindControl("lbtnPick")
            Dim hdRowOffice As HiddenField = e.Row.FindControl("hdRowSOffice"), hdRowGrp As HiddenField = e.Row.FindControl("hdRowSGroup")
            '20120718 TC: If company's office/group not equal to current quote-to office/group then hide pick button, un-mark below code to make it effective
            'If lBtnPick IsNot Nothing And hdRowOffice IsNot Nothing And hdRowGrp IsNot Nothing Then
            '    If Not String.IsNullOrEmpty(hdSalesGroup.Value) And Not String.IsNullOrEmpty(hdRowGrp.Value) Then
            '        If Not String.Equals(hdRowGrp.Value, hdSalesGroup.Value, StringComparison.CurrentCultureIgnoreCase) Then
            '            lBtnPick.Enabled = False
            '        End If
            '    End If
            '    If Not String.IsNullOrEmpty(hdSalesOffice.Value) And Not String.IsNullOrEmpty(hdRowOffice.Value) Then
            '        If Not String.Equals(hdSalesOffice.Value, hdRowOffice.Value, StringComparison.CurrentCultureIgnoreCase) Then
            '            lBtnPick.Enabled = False
            '        End If
            '    End If
            'End If
        End If
    End Sub

    Public Sub PreSettings()
        'If is selecting EM and is not JP01, only allow to select the end customers which are under sold-to id, hence, needs to disable three txtbox 
        If Me.hType.Value.Equals("EM", StringComparison.OrdinalIgnoreCase) AndAlso Not hORG.Value.Equals("JP01") Then
            txtAccountName.ReadOnly = True
            txtAccountName.BackColor = Drawing.ColorTranslator.FromHtml("#ebebe4")
            txtERPID.ReadOnly = True
            txtERPID.BackColor = Drawing.ColorTranslator.FromHtml("#ebebe4")
            txtAddress.ReadOnly = True
            txtAddress.BackColor = Drawing.ColorTranslator.FromHtml("#ebebe4")
        Else
            txtAccountName.ReadOnly = False
            txtAccountName.BackColor = Drawing.Color.White
            txtERPID.ReadOnly = False
            txtERPID.BackColor = Drawing.Color.White
            txtAddress.ReadOnly = False
            txtAddress.BackColor = Drawing.Color.White
        End If
    End Sub

End Class