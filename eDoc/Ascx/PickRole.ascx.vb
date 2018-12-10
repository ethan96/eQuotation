Imports System.Reflection
Public Class PickRole
    Inherits System.Web.UI.UserControl

    'Dim myRole As New Role("EQ", "tbRole")
    'Public Sub getData()
    '    Dim dt As DataTable = Pivot.GetDT("", "Value")
    '    Me.GridView1.DataSource = dt
    'End Sub

    'Public Sub ShowData()
    '    getData()
    '    Me.GridView1.DataBind()
    'End Sub
    'Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
    '    Me.GridView1.PageIndex = e.NewPageIndex
    '    getData()
    '    Me.GridView1.DataBind()
    'End Sub


    'Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim o As LinkButton = CType(sender, LinkButton)
    '    Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
    '    Dim key As Object = Me.GridView1.DataKeys(row.RowIndex).Value
    '    Dim P As Page = Me.Parent.Page
    '    Dim TP As Type = P.GetType()
    '    Dim MI As MethodInfo = TP.GetMethod("PickRoleEnd")
    '    Dim para(0) As Object
    '    para(0) = key
    '    MI.Invoke(P, para)
    'End Sub

End Class