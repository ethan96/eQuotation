Public Class ITPInquiry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then


            If Not Pivot.CurrentProfile.GroupBelTo.Contains(("MANAGER.ACL").ToUpper) And Not Pivot.CurrentProfile.GroupBelTo.Contains(("ITD.ACL").ToUpper) Then
                Response.Redirect("../home.aspx")
            End If
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lbMsg.Text = ""
        If Not String.IsNullOrEmpty(txtPN.Text) Then
            Dim dt As DataTable = GetProdITP()
            gv1.DataSource = dt : gv1.DataBind() : gv1.EmptyDataText = "No Data"
        Else
            lbMsg.Text = "Please input part number first"
        End If
    End Sub

    Function GetProdITP() As DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" select top 200 PART_NO, CURRENCY, ITP "))
            .AppendLine(String.Format(" from PRODUCT_ITP "))
            .AppendLine(String.Format(" where part_no like '{0}%' and currency='{1}' ", Replace(Replace(Trim(txtPN.Text), "'", "''"), "*", "%"), dlCurr.SelectedItem.Text))
            .AppendLine(String.Format(" order by part_no "))
        End With
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        Dim dt As New DataTable
        Dim apt As New SqlClient.SqlDataAdapter(sb.ToString(), conn)
        apt.Fill(dt)
        conn.Close()
        Return dt
    End Function

    Protected Sub btnXls_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim dt As DataTable = GetProdITP()
        Util.DataTable2ExcelDownload(dt, "AEU_ITP.xls")
    End Sub

End Class