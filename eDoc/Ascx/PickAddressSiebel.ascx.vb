Imports System.Reflection
Public Class PickAddressSiebel
    Inherits System.Web.UI.UserControl

    Public Sub getData(ByVal RowId As String)
        Dim SQLSTR As String = ""

        If hType.Value = "SHIP" Then

            SQLSTR = SiebelTools.getSBListSiebel(RowId)
        End If
        If hType.Value = "BILL" Then
            SQLSTR = SiebelTools.getSBListSiebel(RowId)
        End If
        SQLSTR = SiebelTools.getSBListSiebel(RowId)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("CRM", SQLSTR)
        Me.GridView1.DataSource = dt
    End Sub

    Public Sub ShowData(ByVal RowID As String)
        getData(RowID)
        Me.GridView1.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(Me.hRowId.Value)
    End Sub

    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As Object = Me.GridView1.DataKeys(row.RowIndex).Values
        Dim P As Page = Me.Parent.Page
        Dim TP As Type = P.GetType()
        Dim MI As MethodInfo = Nothing
        If hType.Value = "SHIP" Then
            MI = TP.GetMethod("PickSSAddressEnd")
        End If
        If hType.Value = "BILL" Then
            MI = TP.GetMethod("PickSBAddressEnd")

        End If
        Dim para(0) As Object
        para(0) = key
        MI.Invoke(P, para)
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowData(Me.hRowId.Value)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class