Imports System.Reflection
Public Class PickQuoteFinished
    Inherits System.Web.UI.UserControl

    Public Sub getData(ByVal desc As String)

        Dim SQLSTR As String = Business.getMySelfQuoteFinish(Pivot.CurrentProfile.UserId, desc.Replace("'", "''")) 'SiebelTools.GET_Siebel_Account_List(Name, RBU, erpid, country, location)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", SQLSTR)
        Me.GridView1.DataSource = dt
    End Sub

    Public Sub ShowData(ByVal desc As String)
        getData(desc)
        Me.GridView1.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(Me.txtName.Text.Trim.Replace("'", "''"))
    End Sub


    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As Object = Me.GridView1.DataKeys(row.RowIndex).Values
        Dim P As Page = Me.Parent.Page
        Dim TP As Type = P.GetType()
        Dim MI As MethodInfo = TP.GetMethod("PickQuoteFinishEnd")
        Dim para(0) As Object
        para(0) = key
        MI.Invoke(P, para)
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowData(Me.txtName.Text.Trim.Replace("'", "''"))
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ID As String = Me.GridView1.DataKeys(e.Row.RowIndex).Value
            If Not Business.isOrderable(ID) Then
                e.Row.Visible = False
            End If
        End If
    End Sub

End Class