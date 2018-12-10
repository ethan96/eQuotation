Imports System.Web.SessionState
Imports Quartz
Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        'RegisterRoutes(System.Web.Routing.RouteTable.Routes)
        '2015/4/2 IC Add Schedule job function
        Dim mode As String = ConfigurationManager.AppSettings("QuartzJob")
        If Not mode Is Nothing AndAlso mode.ToUpper.Equals("ON") Then
            Dim myJob As New ScheduledJob()

            Try
                'Stop job first, then start job
                myJob.StopSiebelJob()
                myJob.StartSiebelJob()
            Catch ex As Exception
                Dim smtpClient1 As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient("172.20.0.76")
                smtpClient1.Send("myadvantech@advantech.com", "tc.chen@advantech.com.tw,Frank.Chung@advantech.com.tw,IC.Chen@advantech.com.tw,Ming.zhao@advantech.com.cn", "call WebJob Failed:" + Now.ToString(), ex.ToString())
            Finally
                myJob = Nothing
            End Try

        End If
        mode = Nothing
    End Sub

    Shared Sub RegisterRoutes(routes As System.Web.Routing.RouteCollection)
        'routes.Ignore("favicon.ico")
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
        'If Not _schedular Is Nothing Then _schedular.Shutdown()   'ICC 先註解掉
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        If HttpContext.Current.Request.ServerVariables("SERVER_PORT") <> "8200" Then
            Dim ex As Exception = Server.GetLastError().GetBaseException()
            'Server.ClearError()
            Dim msg As String = ex.Message : Server.ClearError()
            Dim uid As String = "Anonymous"

            'Frank 2012/10/15 Excluding this exception
            'If ex.ToString.StartsWith("System.Web.HttpException (0x80004005): File does not exist") Then Exit Sub
            If Not IsNothing(HttpContext.Current) AndAlso (Not IsNothing(HttpContext.Current.User)) AndAlso HttpContext.Current.User.Identity.IsAuthenticated Then
                uid = HttpContext.Current.User.Identity.Name
            End If
            If Not COMM.Util.IsTesting() Then
                'TC 2014/04/15:以下這種invalid part 可否不要視為Exception? 這應該只是提是給user的warning, 
                '不應該視為Program的runtime(exception才對)
                If Not ex.Message.StartsWith("Invalid PartNo.") Then
                    Util.InsertMyErrLog(ex.ToString, uid)
                    Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com;mike.liu@advantech.com.cn", "", "", "eQuotation Error Message by " + uid, "", ex.ToString, "")
                End If
            End If
            'If Not Request.ServerVariables("SERVER_NAME").ToString().Contains("localhost") Then Response.Redirect("~/err.htm")
            Response.Write(getErrMsgFormat(ex.Message.ToString, ex.ToString)) : Response.End()
        End If
    End Sub

    Private Function getErrMsgFormat(ByVal Msg As String, ByVal MsgDetail As String)
        If Msg.Length > 2000 Then
            Msg = Left(Msg, 2000)
        End If
        If MsgDetail.Length > 2000 Then
            MsgDetail = Left(MsgDetail, 1999)
        End If
        Dim str As String = "<fieldset style=""margin-top:3%;width:500px;""><legend style=""font-weight:bold;font-size:14px;font-family:Arial"">Error Message:</legend>"
        str &= "<table style=""width:496px;""><tr><td style=""font-size:12px;color:#666666;font-family:Arial""><br><span style=""color:#FF6633""> An error has occurred and IT has been informed."
        str &= "Please Contact <a href=""mailto:MyAdvantech@advantech.com"">MyAdvantech Team</a> for urgent issues,thank you.</span><hr/>"
        str &= Msg
        If MsgDetail.Length > 0 Then
            str &= "<hr/>" & MsgDetail
        End If
        str &= "</td></tr></table></fieldset>"
        Return str
    End Function

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Protected Sub Application_AcquireRequestState(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    Protected Sub Application_PreRequestHandlerExecute(ByVal sender As Object, ByVal e As System.EventArgs)
        If HttpContext.Current.Session IsNot Nothing Then

            Dim sUrl As String = Request.ServerVariables("SCRIPT_NAME")
            'myRoleManagement As New RoleManagement("EQ", "tbRoleManagement")
            'Dim dt As System.Data.DataTable = myRoleManagement.GetDT(String.Format("URL='{0}'", sUrl), "")
            Dim uid As String = "Anonymous"
            If Not IsNothing(HttpContext.Current) AndAlso (Not IsNothing(HttpContext.Current.User)) AndAlso HttpContext.Current.User.Identity.IsAuthenticated Then
                uid = HttpContext.Current.User.Identity.Name
            End If
            If (sUrl.IndexOf("Login.aspx") = -1 And sUrl.IndexOf(".axd") = -1) Then
                Dim cmd As New SqlClient.SqlCommand(
                     "insert into USER_LOG values(@SESSION,@TRANS,@USERID,GetDate(),@URL,@QUERY,@NOTE,@METHOD,@SERVERPORT,@CLIENT,@APPID,'N',@REFERRER)",
                     New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
                Try
                    Dim sServerName As String = Request.ServerVariables("SERVER_NAME"), sServerPort As String = Request.ServerVariables("SERVER_PORT")
                    Dim sMethod As String = Request.ServerVariables("REQUEST_METHOD")
                    Dim sReferrer As String = ""
                    If Request.ServerVariables("HTTP_REFERER") IsNot Nothing Then sReferrer = Request.ServerVariables("HTTP_REFERER")

                    Dim strSQL = "insert into USER_LOG values(@SESSION,@TRANS,@USERID,GetDate(),@URL,@QUERY,@NOTE,@METHOD,@SERVERPORT,@CLIENT,@APPID,'N',@REFERRER)"

                    With cmd.Parameters
                        .AddWithValue("SESSION", HttpContext.Current.Session.SessionID) : .AddWithValue("TRANS", "")
                        .AddWithValue("USERID", uid) : .AddWithValue("URL", Request.RawUrl)
                        .AddWithValue("QUERY", "") : .AddWithValue("NOTE", "") : .AddWithValue("METHOD", "") : .AddWithValue("SERVERPORT", sServerPort)
                        .AddWithValue("CLIENT", Util.GetClientIP()) : .AddWithValue("APPID", "EQ") : .AddWithValue("REFERRER", sReferrer)
                    End With
                    cmd.Connection.Open() : cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Throw ex
                Finally
                    If Not IsNothing(cmd) AndAlso Not IsNothing(cmd.Connection) Then
                        cmd.Connection.Close()
                    End If
                End Try
            End If
        End If
    End Sub

    Protected Sub Application_BeginRequest(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

End Class