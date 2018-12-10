Imports Microsoft.VisualBasic
Imports Quartz
Imports Quartz.Impl
Imports Advantech.Myadvantech.Business
Imports Advantech.Myadvantech.DataAccess

Public Class ScheduledJob

    Public Sub StartSiebelJob()
        Dim scheduleFactory = New Quartz.Impl.StdSchedulerFactory()
        Dim schedular = scheduleFactory.GetScheduler()
        Dim job As IJobDetail = JobBuilder.Create(Of SiebelJob)().WithIdentity("SiebelJob").Build()
        Dim trigger As ITrigger = TriggerBuilder.Create().WithCronSchedule("0 0/3 * 1/1 * ? *").WithIdentity("SiebelJobTrigger").Build() 'Change web job time from per 5m to 3m
        schedular.ScheduleJob(job, trigger)
        schedular.Start()
    End Sub

    Public Sub StopSiebelJob()
        Dim schedulerFactory = New Quartz.Impl.StdSchedulerFactory().GetScheduler()
        schedulerFactory.UnscheduleJob(New TriggerKey("SiebelJobTrigger"))
        schedulerFactory.DeleteJob(New JobKey("SiebelJob"))
    End Sub
End Class

Public Class SiebelJob
    Implements IJob

    Public Sub Execute(context As IJobExecutionContext) Implements IJob.Execute
        Try
            'ICC 20170801 Add update product forecast job. Only in 9:00 AM will start this job.
            Dim _isUpdateProductForecast As Boolean = False
            If DateTime.Now.Hour = 9 AndAlso DateTime.Now.Minute < 3 Then _isUpdateProductForecast = True

            Dim _SleepTime As Integer = 3
            'Dim smtpClient1 As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient("172.20.0.76")
            'smtpClient1.Send("myadvantech@advantech.com", "tc.chen@advantech.com.tw,Frank.Chung@advantech.com.tw,IC.Chen@advantech.com.tw,Ming.zhao@advantech.com.cn", "SiebelJob start:" + Now.ToString(), Now.ToString())
            Dim count As Integer = 10
            Dim createOptyList As List(Of SiebelActive)
            Dim updateOptyList As List(Of SiebelActive)
            Dim createQuoteList As List(Of SiebelActive)
            Dim createActivityList As List(Of SiebelActive)
            Dim result As New List(Of SiebelActive)

            'Frank將執行時前十分鐘內失敗的再設為未執行，目地就是將上一次執行失敗的再執行一次。因此失敗的最多會重新執行一次
            tbOPBase.dbExecuteNoQuery("EQ", "Update SiebelActive set [status]='UnProcess',ProcessID='Retried' Where [status]='Failed' and CreatedDate>= DATEADD(minute,-12,GETDATE()) ") 'ICC 2015/4/27 Update failed time interval from 10m ago to 12m ago

            createOptyList = SiebelBusinessLogic.GetActiveList(SiebelActiveStatus.UnProcess, SiebelActiveType.CreateOpportunity, count)
            If Not createOptyList Is Nothing AndAlso createOptyList.Count > 0 Then
                If createOptyList.Count = count Then
                    '表示全部都是Create Opportunity
                    For Each active In createOptyList
                        SiebelBusinessLogic.BatchCreateSiebelOpportunity(active)
                        result.Add(active)
                        System.Threading.Thread.Sleep(_SleepTime * 1000)
                    Next
                    '發送Mail
                    SendResultMail(result)
                    '做完10次就退出
                    Exit Sub
                Else
                    '扣除Create Opportunity 剩下的數量，後面繼續使用
                    count = count - createOptyList.Count
                    For Each active In createOptyList
                        SiebelBusinessLogic.BatchCreateSiebelOpportunity(active)
                        result.Add(active)
                        System.Threading.Thread.Sleep(_SleepTime * 1000)
                    Next
                End If
            End If

            updateOptyList = SiebelBusinessLogic.GetActiveList(SiebelActiveStatus.UnProcess, SiebelActiveType.UpdateOpportunity, count)
            If Not updateOptyList Is Nothing AndAlso updateOptyList.Count > 0 Then
                If updateOptyList.Count = count Then
                    For Each active In updateOptyList
                        SiebelBusinessLogic.BatchUpdateSiebelOpportunity(active)
                        result.Add(active)
                        System.Threading.Thread.Sleep(_SleepTime * 1000)
                    Next
                    '發送Mail
                    SendResultMail(result)
                    '做完就退出
                    Exit Sub
                Else
                    count = count - updateOptyList.Count
                    For Each active In updateOptyList
                        SiebelBusinessLogic.BatchUpdateSiebelOpportunity(active)
                        result.Add(active)
                        System.Threading.Thread.Sleep(_SleepTime * 1000)
                    Next
                End If
            End If

            createQuoteList = SiebelBusinessLogic.GetActiveList(SiebelActiveStatus.UnProcess, SiebelActiveType.CreateQuote, count)
            If Not createQuoteList Is Nothing AndAlso createQuoteList.Count > 0 Then
                If createQuoteList.Count = count Then
                    For Each active In createQuoteList
                        SiebelBusinessLogic.BatchCreateSiebelQuote(active)
                        result.Add(active)
                        System.Threading.Thread.Sleep(_SleepTime * 1000)
                    Next
                    '發送Mail
                    SendResultMail(result)
                    '做完就退出
                    Exit Sub
                Else
                    count = count - createQuoteList.Count
                    For Each active In createQuoteList
                        SiebelBusinessLogic.BatchCreateSiebelQuote(active)
                        result.Add(active)
                        System.Threading.Thread.Sleep(_SleepTime * 1000)
                    Next
                End If
            End If

            createActivityList = SiebelBusinessLogic.GetActiveList(SiebelActiveStatus.UnProcess, SiebelActiveType.CreateActivity, count)
            If Not createActivityList Is Nothing AndAlso createActivityList.Count > 0 Then
                '不用再判斷 直接做到結束
                For Each active In createActivityList
                    SiebelBusinessLogic.BatchCreateSiebelActivity(active)
                    result.Add(active)
                    System.Threading.Thread.Sleep(_SleepTime * 1000)
                Next
            End If

            'ICC Getquotation master data
            If _isUpdateProductForecast = True Then
                Dim dt As DateTime = DateTime.Now.AddDays(-1) 'ICC 20170908 Change to get yesterday's quote 
                Dim forecast As List(Of SiebelActive) = Advantech.Myadvantech.Business.SiebelBusinessLogic.GetQuotationMasterForProductForecast(dt)
                SendResultMailWithProductForecast(forecast)
            End If

            '發送Mail
            SendResultMail(result)
        Catch ex As Exception
            Dim _SMTPServer As String = ConfigurationManager.AppSettings("SMTPServer")
            Dim sc As New System.Net.Mail.SmtpClient(_SMTPServer)
            Dim mail As New System.Net.Mail.MailMessage()
            mail.From = New Net.Mail.MailAddress("myadvantech@advantech.com")
            mail.To.Add(New System.Net.Mail.MailAddress("tc.chen@advantech.com.tw"))
            mail.To.Add(New System.Net.Mail.MailAddress("Frank.Chung@advantech.com.tw"))
            mail.To.Add(New System.Net.Mail.MailAddress("IC.Chen@advantech.com.tw"))
            mail.To.Add(New System.Net.Mail.MailAddress("yl.huang@advantech.com.tw"))
            mail.Subject = String.Format("Warring!!!Siebel job occurred exception at {0}", DateTime.Now.ToShortTimeString())
            mail.Body = ex.Message
            mail.IsBodyHtml = True
            sc.Send(mail)
        End Try

        'smtpClient1.Send("myadvantech@advantech.com", "myadvantech@advantech.com", "SiebelJob finished:" + Now.ToString(), Now.ToString())
    End Sub

    Public Sub SendResultMail(ByVal list As List(Of SiebelActive))

        Dim body As String = String.Empty

        Dim _IDs As New StringBuilder
        Dim _dt As DataTable = Nothing

        If list IsNot Nothing AndAlso list.Any() Then
            For Each _SA As SiebelActive In list
                _IDs.Append(_SA.ID & ",")
            Next
            Dim sql As New StringBuilder
            sql.AppendLine("Select [ID],[ActiveSource],[ActiveType],[Status],[QuoteID]")
            sql.AppendLine(",[OptyID],[OptyName],[OptyStage],[FailedLog],[CreatedDate],[CreateBy],[LastUpdatedDate]")
            sql.AppendLine(",[LastUpdatedBy],[WSParameters] From [SiebelActive] ")
            sql.AppendLine(" Where ID IN (" & _IDs.ToString.TrimEnd(",") & ")")
            _dt = tbOPBase.dbGetDataTable("EQ", sql.ToString)
        End If

        'If list.Any() Then
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            Dim gv As New GridView()
            'gv.DataSource = list
            gv.DataSource = _dt
            gv.DataBind()
            Dim sb As New StringBuilder()
            Dim sw As New System.IO.StringWriter(sb)
            Dim html As New System.Web.UI.HtmlTextWriter(sw)
            gv.RenderControl(html)
            body = sb.ToString()
        End If

        'Dim sc As New System.Net.Mail.SmtpClient("172.20.0.76")
        Dim _SMTPServer As String = ConfigurationManager.AppSettings("SMTPServer")
        Dim sc As New System.Net.Mail.SmtpClient(_SMTPServer)

        Dim mail As New System.Net.Mail.MailMessage()
        mail.From = New Net.Mail.MailAddress("myadvantech@advantech.com")
        'mail.To.Add(New System.Net.Mail.MailAddress("tc.chen@advantech.com.tw"))
        mail.To.Add(New System.Net.Mail.MailAddress("Frank.Chung@advantech.com.tw"))
        'mail.To.Add(New System.Net.Mail.MailAddress("IC.Chen@advantech.com.tw"))
        'mail.To.Add(New System.Net.Mail.MailAddress("Ming.zhao@advantech.com.cn"))
        'Frank IsTesting在job中呼叫會失敗
        'Dim _FromWhichSite As String = "(Production Site)"
        'If COMM.Util.IsTesting Then
        '    _FromWhichSite = "(Staging Site)"
        'End If
        mail.Subject = String.Format("Siebel job end at {0}", DateTime.Now.ToShortTimeString()) ' & _FromWhichSite
        mail.Body = body
        mail.IsBodyHtml = True
        sc.Send(mail)
    End Sub

    Public Sub SendResultMailWithProductForecast(ByVal list As List(Of SiebelActive))
        Dim body As String = String.Empty
        If list.Count > 0 Then
            Dim gv As New GridView()
            gv.DataSource = list.Select(Function(p) New With _
                                                  {.QuoteID = p.QuoteID, .OptyID = p.OptyID, .Message = p.FailedLog}).ToList()
            gv.DataBind()
            Dim sb As New StringBuilder()
            Dim sw As New System.IO.StringWriter(sb)
            Dim html As New System.Web.UI.HtmlTextWriter(sw)
            gv.RenderControl(html)
            body = sb.ToString()
        Else
            body = "No data"
        End If
        Dim _SMTPServer As String = ConfigurationManager.AppSettings("SMTPServer")
        Dim sc As New System.Net.Mail.SmtpClient(_SMTPServer)
        Dim mail As New System.Net.Mail.MailMessage()
        mail.From = New Net.Mail.MailAddress("myadvantech@advantech.com")
        mail.To.Add(New System.Net.Mail.MailAddress("Frank.Chung@advantech.com.tw"))
        mail.To.Add(New System.Net.Mail.MailAddress("JJ.lin@advantech.com.tw"))
        mail.To.Add(New System.Net.Mail.MailAddress("Alex.Chiu@advantech.com.tw"))
        mail.To.Add(New System.Net.Mail.MailAddress("IC.Chen@advantech.com.tw"))
        mail.Subject = String.Format("Siebel job update product forecast end at {0}", DateTime.Now.ToShortTimeString())
        mail.Body = body
        mail.IsBodyHtml = True
        sc.Send(mail)
    End Sub
End Class
