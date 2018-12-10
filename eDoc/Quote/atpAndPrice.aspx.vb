Public Class atpAndPrice
    Inherits System.Web.UI.Page

    Protected Function getProductTable() As DataTable

        Dim dt As New DataTable

        dt.Columns.Add("Part_no") : dt.Columns.Add("qty") : dt.Columns.Add("listprice") : dt.Columns.Add("unitprice")

        Dim rr As DataRow = dt.NewRow
        rr.Item("Part_no") = Request("ID")
        rr.Item("qty") = "0"
        rr.Item("listprice") = "0"
        rr.Item("unitprice") = "0"
        dt.Rows.Add(rr)
        Dim RETPRICEDT As New DataTable
        'SAPTools.getSAPPriceByTable(Request("ID"), Request("ORG"), Request("COMPANY"), Request("COMPANY"), "", RETPRICEDT)
        Dim _SAPDAL As New SAPDAL.SAPDAL
        _SAPDAL.getSAPPriceByTable(Request("ID"), Request("ORG"), Request("COMPANY"), Request("COMPANY"), "", "", RETPRICEDT)
        Dim WSPTb As DataTable = RETPRICEDT
        For Each r As DataRow In WSPTb.Rows
            For Each dtr As DataRow In dt.Select("part_no='" & r.Item("MATNR") & "'")
                dtr.Item("unitprice") = FormatNumber(r.Item("Netwr"), 2).Replace(",", "")
                dtr.Item("listprice") = FormatNumber(r.Item("Kzwi1"), 2).Replace(",", "")
            Next
        Next
        Return dt
        Return Nothing
    End Function

    Protected Function getATPdetail(ByVal pn As String) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("date") : dt.Columns.Add("qty")
        ' Try
        Dim dttemp As New DataTable
        SAPTools.getInventoryAndATPTable(pn, Business.getPlantByOrgID(Request("Org")), 0, "", 0, dttemp)
        If dttemp.Rows.Count > 0 Then
            For i As Integer = 0 To dttemp.Rows.Count - 1
                If CInt(dttemp.Rows(i).Item("com_qty")) <> 0 Then
                    Dim r As DataRow = dt.NewRow
                    r.Item("date") = dttemp.Rows(i).Item("com_date") : r.Item("qty") = CInt(dttemp.Rows(i).Item("com_qty")) : dt.Rows.Add(r)
                End If
            Next
        End If
        If dt.Rows.Count = 0 Then
            Dim r As DataRow = dt.NewRow
            r.Item("date") = Now.ToString("yyyyMMdd") : r.Item("qty") = 0 : dt.Rows.Add(r)
        End If
        For Each r As DataRow In dt.Rows
            Dim strATPDate As String = r.Item("date").ToString(), dtATPDate As Date = Date.MaxValue
            If Date.TryParseExact(strATPDate, "yyyyMMdd", New Globalization.CultureInfo("en-US"), Globalization.DateTimeStyles.None, dtATPDate) Then
                r.Item("date") = dtATPDate.ToString("yyyy/MM/dd")
            End If
        Next
        Return dt
        'Catch ex As Exception

        'End Try

        Return Nothing
    End Function

    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim GV As GridView = e.Row.FindControl("gv2")
            Dim DBITEM As DataRowView = CType(e.Row.DataItem, DataRowView)
            GV.DataSource = getATPdetail(DBITEM.Item("PART_NO"))
            GV.DataBind()
            If CDbl(e.Row.Cells(3).Text) = 0 Then
                e.Row.Cells(3).Text = "TBD"
            Else
                e.Row.Cells(3).Text = e.Row.Cells(3).Text
            End If
            If CDbl(e.Row.Cells(4).Text) = 0 Then
                e.Row.Cells(4).Text = "TBD"
            Else
                e.Row.Cells(4).Text = e.Row.Cells(4).Text
            End If
        End If

    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not IsNothing(Request("COMPANY")) AndAlso Request("COMPANY") <> "" Then
                If Not IsNothing(Request("ID")) AndAlso Request("ID").Trim <> "" Then
                    Dim dt As DataTable = getProductTable()
                    Me.gv1.DataSource = dt
                    Me.gv1.DataBind()
                    Me.upContent.Update()
                End If
            End If

        End If
    End Sub

End Class