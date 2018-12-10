Public Class ListPriceInquiry
    Inherits System.Web.UI.Page

    Protected Sub dlOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim apt As New SqlClient.SqlDataAdapter("select distinct currency from PRODUCT_LIST_PRICE_RBU where org='" + dlOrg.SelectedValue + "' order by currency", ConfigurationManager.ConnectionStrings("eQ").ConnectionString)
        Dim dt As New DataTable
        apt.Fill(dt)
        If dt.Rows.Count > 0 Then
            dlCurr.Items.Clear()
            For Each r As DataRow In dt.Rows
                dlCurr.Items.Add(New ListItem(r.Item("currency"), r.Item("currency")))
            Next
        End If
        thDivision.Visible = IIf(dlOrg.SelectedValue = "US01", True, False)
        tdDivision.Visible = IIf(dlOrg.SelectedValue = "US01", True, False)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            dlOrg_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub

    Function GetListPrice() As DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" select top 200 PART_NO, CURRENCY, LIST_PRICE "))
            .AppendLine(String.Format(" from PRODUCT_LIST_PRICE_RBU "))
            .AppendLine(String.Format(" where part_no like '{0}%' and currency='{1}' and division='{2}' and org='{3}' ", _
                                      Replace(Replace(Trim(txtPN.Text), "'", "''"), "*", "%"), dlCurr.SelectedValue, dlDivision.SelectedValue, dlOrg.SelectedValue))
            .AppendLine(String.Format(" order by part_no "))
        End With
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        Dim dt As New DataTable
        Dim apt As New SqlClient.SqlDataAdapter(sb.ToString(), conn)
        apt.Fill(dt)
        conn.Close()
        Return dt
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lbMsg.Text = ""
        If Not String.IsNullOrEmpty(txtPN.Text) Then
            Dim dt As DataTable = GetListPrice()
            gv1.DataSource = dt : gv1.DataBind() : gv1.EmptyDataText = "No Data"
        Else
            lbMsg.Text = "Please input part number first"
        End If
    End Sub

    Protected Sub btnXls_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim dt As DataTable = GetListPrice()
        Util.DataTable2ExcelDownload(dt, dlOrg.SelectedValue + " " + dlCurr.SelectedValue + " ListPrice.xls")
    End Sub

End Class