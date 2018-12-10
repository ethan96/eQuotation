Public Class CBOM_eStoreBTO_List5
    Inherits PageBase

    Public CATEGORY3_title As String = "", CATEGORY2_title As String = "", CATEGORY1_title As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
   
        If Request("CATEGORY2") IsNot Nothing AndAlso Request("CATEGORY3") IsNot Nothing AndAlso Request("CATEGORY1") IsNot Nothing Then
            SqlDataSource1.SelectCommand = "SELECT distinct DisplayPartno,ImageURL from ESTORE_BTOS where CATEGORY2='" + Request("CATEGORY2").ToString.Trim + "' AND CATEGORY3='" + Request("CATEGORY3").ToString.Trim + "'  AND CATEGORY1='" + Request("CATEGORY1").ToString.Trim + "' and storeid='AUS'"
            CATEGORY1_title = Request("CATEGORY1").ToString.Trim
            CATEGORY2_title = Request("CATEGORY2").ToString.Trim
            CATEGORY3_title = Request("CATEGORY3").ToString.Trim
        Else
            Response.End()
        End If
    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'DataList1.DataKeys(e.Item.ItemIndex).ToString().Trim()      
        Dim item As DataListItem = CType(CType(sender, Control).NamingContainer, DataListItem)
        Dim i As Integer = item.ItemIndex
        'Dim neirong As String = DirectCast(DataList1.Items(i).FindControl("textbox1"), TextBox).Text
        'Response.Write(DataList1.DataKeys(i).ToString().Trim())
        Response.Redirect("./QuotationDetail.aspx?VIEW=1&BTOITEM=" + DataList1.DataKeys(i).ToString().Trim() + "&QTY=1&UID=" + Request("UID") + "")
    End Sub

    Protected Sub DataList1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            ' Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            ' Dim Image1 As Image = DirectCast(e.Item.FindControl("Image1"), Image)
            '  Image1.ImageUrl ="http://buy.advantech.com/resource/ProductCategory/TREK-775_01-S.jpg" 'dv.Row("DisplayPartno").ToString()         
        End If
    End Sub
End Class