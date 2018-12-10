Imports System.Net

Public Class TestMing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Response.Redirect("../quote/quoteModify.aspx?UID=deea7f2fddd3463")
            Dim u As String = Request.ServerVariables("SERVER_NAME")
            Response.Write(u + "<hr>")
            Dim IPs As IPAddress() = Dns.GetHostAddresses(u)
            For Each ip As IPAddress In IPs
                Response.Write(ip.ToString() & "<br>")
            Next
            Response.Write("<hr>")
            IPs = Dns.GetHostAddresses("eq.advantech.com")
            For Each ip As IPAddress In IPs
                Response.Write(ip.ToString() & "<br>")
            Next
            Response.Write("<hr>")
            Response.Write(IPs.FirstOrDefault().ToString())
            'Dim UID = "deea7f2fddd3463"
            'Dim Quotelist As List(Of QuoteItem) = MyQuoteX.GetQuoteList(UID)
            'Dim aa As Decimal = MyQuoteX.GetTotalPrice(UID)
            'Dim newDate As DateTime = Now
            'newDate = DateAdd(DateInterval.Day, EnumSetting.ExpiredDurationDay.USDuration, newDate)
            'Response.Write(newDate)
            ' Dim abc As Boolean = Role.IsUsaUser("cathee.cao@advantech.com")

            'Dim ws As New eQ25WS
            ' Response.Write(ws.GetAEUTemplateHtml("GQ034369", "linda.josquin@advantech.nl", "9b860346-a34c-41f1-8afa-3f539d7ac8c1-127001"))
            'Dim pageHolder As New TBBasePage()
            'pageHolder.IsVerifyRender = False
            'Dim ControlURL As String = "~/Ascx/AEUQuoteTemplate/QuoteMaster.ascx"
            'Dim cw1 As UserControl = DirectCast(pageHolder.LoadControl(ControlURL), UserControl)
            'Dim viewControlType As Type = cw1.GetType
            'Dim UID As Reflection.PropertyInfo = viewControlType.GetProperty("UID")
            'UID.SetValue(cw1, "94a35c0d1a2540b", Nothing)
            ''Dim _meth As Reflection.MethodInfo = viewControlType.GetMethod("LoadData")
            ''_meth.Invoke(cw1, Nothing)
            'pageHolder.Controls.Add(cw1)
            'Dim output As New IO.StringWriter()
            'HttpContext.Current.Server.Execute(pageHolder, output, False)
            'Literal1.Text = output.ToString()
        End If

        'If HttpContext.Current.Cache("PartNO_ITP") IsNot Nothing Then
        '    GridView1.DataSource = CType(HttpContext.Current.Cache("PartNO_ITP"), DataTable)
        '    GridView1.DataBind()
        '    Button1.Text = "清空ITP"
        '    Button1.Enabled = True
        'Else
        '    Button1.Text = "Cache还没有建立"
        '    Button1.Enabled = False
        'End If

    End Sub

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
    '    Dim myContentByte As Byte() = Nothing
    '    myContentByte = Util.DownloadQuotePDFforAEU("")
    '    Dim fname As String = "test"
    '   Response.Clear()
    '    Response.AddHeader("Content-Type", "binary/octet-stream")
    '    Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".pdf;size = " & myContentByte.Length.ToString())
    '    Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
    'End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If HttpContext.Current.Cache("PartNO_ITP") IsNot Nothing Then
            HttpContext.Current.Cache.Remove("PartNO_ITP")
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Business.isExpired("AUSQ013596", COMM.Fixer.eDocType.EQ)
    End Sub
End Class