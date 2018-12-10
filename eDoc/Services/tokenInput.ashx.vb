Imports System.Web
Imports System.Web.Services

Public Class tokenInput
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "application/json"
        Dim orgid As String = Trim(HttpContext.Current.Request("orgid"))
        Dim _IsUSAOnlineEP As String = Trim(HttpContext.Current.Request("IsUSAOnlineEP"))



        Dim txtKey As String = Trim(HttpContext.Current.Request("q")), pnList As New List(Of PNInfo)
        txtKey = Replace(Replace(Trim(txtKey), "'", "''"), "*", "%")
        'Dim dt As DataTable = Business.getProduct(orgid, txtKey, "")
        Dim dt As DataTable = Nothing

        If _IsUSAOnlineEP.Equals("true", StringComparison.InvariantCultureIgnoreCase) Then
            'dt = Business.getProduct(orgid, txtKey, "", 1)
            'Cathee 20160722 : they don't want to pick the BTO part in the auto-suggestion list
            dt = Business.getProduct(orgid, txtKey, "", 0)
        Else
            dt = Business.getProduct(orgid, txtKey, "")
        End If

        'tbOPBase.dbGetDataTable("MY", _
        '                                " select top 10 PART_NO from SAP_PRODUCT_ORG with (nolock) where ORG_ID='" + orgid + "' " + _
        '                                    " and  [status] in ('A','N','H','O') and PART_NO like '" + Replace(Replace(txtKey, "'", "''"), "*", "%") + "%' order by PART_NO")

        For Each r As DataRow In dt.Rows
            pnList.Add(New PNInfo(r.Item("Part_no"), r.Item("Part_no")))
        Next

        Dim jsr As New Script.Serialization.JavaScriptSerializer, retJson As String = jsr.Serialize(pnList)
        If HttpContext.Current.Request("callback") IsNot Nothing Then
            retJson = HttpContext.Current.Request("callback") + "(" + retJson + ")"
        End If
        HttpContext.Current.Response.Clear() : HttpContext.Current.Response.Write(retJson) : HttpContext.Current.Response.End()

    End Sub
    Public Class PNInfo
        Public Property id As String : Public Property name As String
        Public Sub New(ByVal k As String, ByVal v As String)
            Me.id = k : Me.name = v
        End Sub
    End Class
    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class