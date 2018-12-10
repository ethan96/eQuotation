Imports System.Web.Script.Serialization


Public Class Products
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'drpOrg.ClearSelection()
            'For Each i As ListItem In drpOrg.Items
            '    If i.Value = Pivot.CurrentProfile.getCurrOrg Then
            '        i.Selected = True
            '    End If
            'Next
            'drpOrg.Enabled = False
            Horg.Value = Pivot.CurrentProfile.getCurrOrg
            LitOrgid.Text = Horg.Value
            If Not Request.QueryString("Btos") Is Nothing Then
                BTOS.Value = Request.QueryString("Btos").ToString()
            End If
            If Not Request.QueryString("currency") Is Nothing Then
                Hcurrency.Value = Request.QueryString("currency").ToString()
            End If
        End If
    End Sub
    <Services.WebMethod(enablesession:=True)> _
<Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetData(ByVal ipage As String, ByVal irows As String, ByVal org As String, ByVal partNo As String, ByVal Desc As String, ByVal Btos As String) As String
        Dim serializer As New JavaScriptSerializer
        Dim dt As DataTable = Business.getProduct(org, partNo, Desc, Btos)
        If Not IsNumeric(ipage) Then ipage = 1
        If Not IsNumeric(irows) Then irows = 10
        Dim returnstr As String = GetJson(dt, Integer.Parse(ipage), Integer.Parse(irows))
        Return returnstr
    End Function
    Public Shared Function GetJson(ByVal dt As DataTable, ByVal ipage As Integer, ByVal irows As Integer) As String
        Dim serializer As New JavaScriptSerializer
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object) = Nothing
        Dim result As New Dictionary(Of String, Object)
        Dim startRow As Integer = 0, endRow As Integer = 0
        Dim rowtotal As Integer = 0

        '最後一頁的判斷
        rowtotal = ipage * irows
        If (dt.Rows.Count - rowtotal) > 0 Then
            endRow = rowtotal '非最後一頁就為每頁筆數*第幾頁
            startRow = endRow - irows
        Else
            endRow = dt.Rows.Count '最後一頁就為原總筆數
            startRow = (ipage - 1) * irows '最後一頁的起始筆數= (第幾頁-1)* 每頁筆數
        End If

        '總筆數
        result.Add("total", dt.Rows.Count)

        For i As Integer = startRow To endRow - 1
            Dim dr As DataRow = dt.Rows(i)
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dt.Columns
                row.Add(col.ColumnName.Trim(), dr(col))
            Next
            rows.Add(row)
        Next

        result.Add("rows", rows)
        Return serializer.Serialize(result)
    End Function
End Class