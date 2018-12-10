Public Class USPriceByLevel
    Inherits System.Web.UI.Page

    Function GetPrice() As DataTable
        Dim strSql As String = String.Format( _
            "select  top 100 PART_NO, UNIT_PRICE, DIVISION, PRICE_GROUP  " + _
            "from PRODUCT_US_PRICE_BY_DIVISION_PGRP  " + _
            "where PART_NO like '{0}%' " + _
            "order by DIVISION, PRICE_GROUP  ", Replace(Replace(Trim(txtPN.Text), "'", "''"), "*", "%"))
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        Dim dt As New DataTable
        Dim apt As New SqlClient.SqlDataAdapter(strSql, conn)
        apt.Fill(dt)
        conn.Close()
        Return dt
    End Function

    Protected Sub btnQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gv1.DataSource = GetPrice()
        gv1.DataBind()
    End Sub

End Class