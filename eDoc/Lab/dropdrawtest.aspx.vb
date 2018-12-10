Public Class dropdrawtest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then


            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn() {New DataColumn("Item"), New DataColumn("Price")})
            dt.Rows.Add("Shirt", 450)
            dt.Rows.Add("Jeans", 3200)
            dt.Rows.Add("Trousers", 1900)
            dt.Rows.Add("Tie", 185)
            dt.Rows.Add("Cap", 100)
            dt.Rows.Add("Hat", 120)
            dt.Rows.Add("Scarf", 290)
            dt.Rows.Add("Belt", 150)
            gvSource.UseAccessibleHeader = True
            gvSource.DataSource = dt
            gvSource.DataBind()

            dt.Rows.Clear()
            dt.Rows.Add()

        End If

    End Sub

End Class