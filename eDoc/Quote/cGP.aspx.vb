Public Class cGP
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim QD As New quotationDetail("EQ", "QUOTATIONDETAIL")
        Dim DT As New DataTable
        DT = QD.GetDT(String.Format("QUOTEID='{0}'", Me.TextBox1.Text), "line_no")
        Dim sp As String = ""
        Dim si As String = ""
        For Each ROW As DataRow In DT.Rows
            Dim f As Boolean = Business.IsPTD(ROW.Item("partno"))
            Response.Write(ROW.Item("line_no") & ";&nbsp;;&nbsp;;&nbsp;;&nbsp;" & ROW.Item("partno") & ";&nbsp;;&nbsp;;&nbsp;;&nbsp;" & IIf(f, "True", "<font color=""red"">False</font>") & ";&nbsp;;&nbsp;;&nbsp;;&nbsp;" & ROW.Item("newunitprice") & ";&nbsp;;&nbsp;;&nbsp;;&nbsp;" & ROW.Item("newitp"))
            Response.Write("<br/>")
            If Not f Then
                sp = sp & " + " & ROW.Item("newunitprice")
                si = si & " + " & ROW.Item("newitp")
            End If
        Next
        Response.Write("total price: " & sp)
        Response.Write("<br/>")
        Response.Write("total itp: " & si)

    End Sub
End Class