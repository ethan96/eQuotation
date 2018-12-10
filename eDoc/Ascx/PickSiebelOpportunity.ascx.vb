Imports System.Reflection
Public Class PickSiebelOpportunity
    Inherits System.Web.UI.UserControl

    Public Sub getData(ByVal sid As String, Optional ByVal OptyName As String = "", Optional ByVal PartNo As String = "")
        'Dim SQLSTR As String = SiebelTools.GET_Opty(sid, OptyName, PartNo)
        'Dim dt As DataTable = tbOPBase.dbGetDataTable("CRM", SQLSTR)
        Dim apt As New SiebelDSTableAdapters.SIEBEL_OPPORTUNITYTableAdapter
        Dim dt As SiebelDS.SIEBEL_OPPORTUNITYDataTable = Nothing
        Try
            If Role.IsTWAonlineSales() Then
                dt = apt.SearchOptyV3(OptyName, sid)
            ElseIf (Role.IsUsaUser() Or Role.IsMexicoAonlineSales()) Then
                dt = apt.SearchOptyForANA(OptyName, sid)
            Else
                dt = apt.SearchOptyV2(OptyName, sid)
            End If
        Catch ex As Exception
            Dim errormsg As String = ex.ToString + vbCrLf + "PickSiebelOpportunity.ascx.vb:line:20"
            Util.InsertMyErrLog(errormsg, Util.GetCurrentUserID())
        End Try
   
        ' Dim dt As SiebelDS.SIEBEL_OPPORTUNITYDataTable = apt.SearchOptyV2(OptyName, sid)
        Me.GridView1.DataSource = dt
    End Sub

    Public Sub ShowData(ByVal sid As String, Optional ByVal OptyName As String = "", Optional ByVal PartNo As String = "")
        getData(sid, OptyName, PartNo) : Me.GridView1.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(Me.h_rowid.Value.Trim.Replace("'", "''"), Util.ReplaceSQLChar(Me.txtSearchOptyName.Text.Trim))
    End Sub

    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim _optyID As Object = Me.GridView1.DataKeys(row.RowIndex).Values(0)
        Dim _optyName As Object = Me.GridView1.DataKeys(row.RowIndex).Values(1)
        Dim P As Page = Me.Parent.Page, TP As Type = P.GetType(), MI As MethodInfo = TP.GetMethod("PickOptyEnd")
        Dim para(2) As Object
        para(0) = _optyID.ToString : para(1) = _optyName : para(2) = ""
        MI.Invoke(P, para)
    End Sub

    Protected Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim _Name As String = Trim(Me.txtOptyName.Text)
        Me.LabelErrleMsg.Text = ""
        If String.IsNullOrEmpty(_Name) Then
            Me.LabelErrleMsg.Text = "Please input new opportunity name." : Me.txtOptyName.Focus() : Exit Sub
        End If
        'Check if the optyName already exists
        Dim apt As New SiebelDSTableAdapters.SIEBEL_OPPORTUNITYTableAdapter
        'Dim _dt As DataTable = tbOPBase.dbGetDataTable("CRM", SiebelTools.GETOptyByName(_Name, h_rowid.Value))
        If apt.CheckOptyNameExist(h_rowid.Value, _Name) > 0 Then
            Me.LabelErrleMsg.Text = "New opportunity name already exists." : Me.txtOptyName.Focus() : Exit Sub
        End If

        Dim _Stage As String = Me.DDLOptyStage.SelectedValue, P As Page = Me.Parent.Page, TP As Type = P.GetType()
        Dim MI As MethodInfo = TP.GetMethod("PickOptyEnd")
        Dim para(2) As Object
        para(0) = "new ID" : para(1) = _Name : para(2) = _Stage : MI.Invoke(P, para)
    End Sub

    Public Sub SetTabSelectedIndex(ByVal _DisplayTabIndex As Integer)
        If _DisplayTabIndex > Me.TabContainer1.Tabs.Count - 1 Then
            _DisplayTabIndex = Me.TabContainer1.Tabs.Count - 1
        End If
        Me.TabContainer1.ActiveTabIndex = _DisplayTabIndex
    End Sub

    Protected Sub btnSearchOpty_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowData(h_rowid.Value, txtSearchOptyName.Text.Replace("'", "''").Trim)
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim data As DataRowView = e.Row.DataItem
            Dim CURR_STG_ID As Object = data("CURR_STG_ID")
            Dim ddl As DropDownList = CType(e.Row.FindControl("ddl_OOP"), DropDownList)
            If CURR_STG_ID IsNot Nothing Then
                Dim curritem As ListItem = ddl.Items.FindByValue(CURR_STG_ID.ToString())
                If curritem IsNot Nothing Then
                    ddl.ClearSelection()
                    curritem.Selected = True
                End If
            End If
            Dim BTupdateOtp As Button = CType(e.Row.FindControl("BTupdateOtp"), Button)
            Dim PickLink As LinkButton = CType(e.Row.FindControl("lbtnPick"), LinkButton)
            If ddl.SelectedIndex >= 0 AndAlso (String.Equals(ddl.SelectedValue, "1-VXVAIE") OrElse String.Equals(ddl.SelectedValue, "1-VXVAID")) Then
                If BTupdateOtp IsNot Nothing Then BTupdateOtp.Visible = False
                ddl.Enabled = False : PickLink.Visible = False
            End If
            Dim LitOptyID As Literal = CType(e.Row.FindControl("LitOptyID"), Literal)
            LitOptyID.Visible = False
            If String.Equals(HFoptyID.Value, data("ROW_ID"), StringComparison.InvariantCultureIgnoreCase) Then
                PickLink.Text = "Picked"
                PickLink.Enabled = False
            End If
            'If Not COMM.Util.IsTesting() Then
            '    LitOptyID.Visible = False
            'End If
        End If
    End Sub
    Protected Sub btupdate(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(bt.NamingContainer, GridViewRow)
        Dim gv As GridView = CType(row.NamingContainer, GridView)
        Dim optyid As String = gv.DataKeys(row.RowIndex).Value
        Dim ddl As DropDownList = CType(row.FindControl("ddl_OOP"), DropDownList)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("CRM", String.Format("select TOP 1 O.ROW_ID, ISNULL(O.CURR_STG_ID,'1-VXVAIE') AS STG_ID,isnull(O.DESC_TEXT,'') as  DESC_TEXT ,isnull(O.SUM_MARGIN_AMT,0) as AMT from  S_OPTY O INNER JOIN S_STG  G ON O.CURR_STG_ID = G.ROW_ID WHERE O.ROW_ID='{0}'", optyid))
        If dt.Rows.Count = 1 Then
            Dim siebelWS As New SiebelWS.Siebel_WS
            Dim retbool As Boolean = False
            Try
                'Ming20150320調用新的WS.updateOpty
                Dim msg As String = String.Empty
                Dim AMT As Integer?
                Dim outAMT As Integer = 0
                If Integer.TryParse(dt.Rows(0).Item("AMT"), outAMT) Then
                    AMT = outAMT
                End If
                retbool = Advantech.Myadvantech.DataAccess.SiebelDAL.UpdateOptyStageV2(dt.Rows(0).Item("ROW_ID"), ddl.SelectedItem.Text, AMT, dt.Rows(0).Item("DESC_TEXT"), Nothing, msg)
                'retbool = siebelWS.UpdateOpportunityStage(dt.Rows(0).Item("ROW_ID"), ddl.SelectedItem.Text, dt.Rows(0).Item("DESC_TEXT"), dt.Rows(0).Item("AMT"), Nothing)

            Catch ex As Exception
                Dim errormsg As String = ex.ToString + vbCrLf + "siebelWS.UpdateOpportunityStage:" + dt.Rows(0).Item("ROW_ID").ToString()
                Util.InsertMyErrLog(errormsg, Util.GetCurrentUserID())
            End Try
            Dim up As UpdatePanel = CType(Me.Parent.FindControl("UPPickOpty"), UpdatePanel)
            If retbool Then
                Util.AjaxShowMsg(up, "Update is successful.", "outMsg")
            Else
                Util.AjaxShowMsg(up, "Update failed.", "outMsg")
                '    Util.AjaxShowLoading(up, "update fails.", 0)
            End If

        End If
    End Sub
    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Public Sub DisableTabsbyIndex(ByVal _Index As Integer)
        Me.TabContainer1.Tabs(_Index).Visible = False
    End Sub
End Class