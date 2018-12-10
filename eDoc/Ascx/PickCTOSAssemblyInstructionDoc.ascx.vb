Imports System.Reflection
Public Class PickCTOSAssemblyInstructionDoc
    Inherits System.Web.UI.UserControl

    Public Sub ShowData(ByVal erpid As String, ByVal docname As String)
        txtERPID.Text = erpid
        Dim dt As DataTable = SAPDAL.SAPDAL.GetCTOSAssemblyInstructionListByERPIdFromMyadvantech(erpid, docname)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            txtERPID.Text = ""
            dt = SAPDAL.SAPDAL.GetCTOSAssemblyInstructionListByERPIdFromMyadvantech("", docname)
        End If
        Me.GridView1.DataSource = dt : Me.GridView1.DataBind()
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(txtERPID.Text, txtDocTxt.Text)
    End Sub

    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim _url As Object = Me.GridView1.DataKeys(row.RowIndex).Values(0)
        Dim P As Page = Me.Parent.Page, TP As Type = P.GetType(), MI As MethodInfo = TP.GetMethod("PickCTOSAssemblyInstructionDocEnd")
        Dim para(1) As Object
        para(0) = _url.ToString
        para(1) = Me.h_system_name.Value
        MI.Invoke(P, para)
    End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowData(txtERPID.Text, txtDocTxt.Text)
    End Sub

End Class