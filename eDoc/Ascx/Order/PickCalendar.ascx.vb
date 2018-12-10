Public Class PickCalendar
    Inherits System.Web.UI.UserControl
    Dim xDT As New System.Data.DataTable, Dt_sap_company_calendar As System.Data.DataTable
    Dim xDTcust As New System.Data.DataTable
    Dim Arr01 As New ArrayList
    Public Orgbysession As String = ""
 
    ' ming get local time
    Shared Function getCalendarbyOrg(ByVal org As String) As String
        Dim plant As String = org & "H1"
        Dim str As String = String.Format("select LAND1 from saprdp.t001w where WERKS='{0}' and mandt='168' and rownum=1", plant)
        Dim CID As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
        If Not IsNothing(CID) AndAlso CID.ToString <> "" Then
            Return CID.ToString
        End If
        Return "TW"
    End Function
    Dim Today_Date As Date = Today.Date

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Orgbysession = getCalendarbyOrg(Left(Pivot.CurrentProfile.getCurrOrg.ToString().ToUpper, 2))
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim localtime As DateTime = Pivot.NewObjDoc.GetLocalTime(Pivot.CurrentProfile.getCurrOrg.ToString.Substring(0, 2))
        Today_Date = localtime.Date
        'Me.Calendar1.Caption = "<font size='3pt'><b>" & "Calendar" & "</b></font>"
        If Not Page.IsPostBack Then
            Try
                Me.txtType.Text = Request("Type").ToString.ToLower
            Catch ex As Exception
                Me.txtType.Text = ""
            End Try
            Try
                Me.txtElement.Text = Request("Element").ToString.ToLower
            Catch ex As Exception
                Me.txtElement.Text = ""
            End Try
            Try
                Me.txtFormat.Text = Request("Format").ToString.ToLower
                If Me.txtFormat.Text = "" Then
                    Me.txtFormat.Text = "dd/MM/yyyy"
                End If
            Catch ex As Exception
                Me.txtFormat.Text = "dd/MM/yyyy"
            End Try
            Try
                Me.txtSalesOrg.Text = Pivot.CurrentProfile.getCurrOrg.ToString.ToLower
                If Me.txtSalesOrg.Text = "" Then
                    Me.txtSalesOrg.Text = "EU10"
                End If
            Catch ex As Exception
                Me.txtSalesOrg.Text = "EU10"
            End Try
            Try
                Me.txtCustomerId.Text = Me.hCompany.Value.ToString.ToLower
                If Me.txtCustomerId.Text = "" Then
                    'Me.txtCustomerId.Text = Request("CustomerId")
                End If
                'If Me.txtCustomerId.Text = "" Then
                '    Me.txtCustomerId.Text = "Default"
                'End If
            Catch ex As Exception
                Me.txtCustomerId.Text = "Default"
            End Try
            Try
                Me.txtPlant.Text = Request("Plant").ToString.ToLower
                If Me.txtPlant.Text = "" Then
                    Me.txtPlant.Text = "Default"
                End If
            Catch ex As Exception
                Me.txtPlant.Text = "Default"
            End Try
            Me.Calendar1.SelectedDate = Today_Date
            '--
            'Response.Write(Me.Calendar1.VisibleDate.Month.ToString + "<hr>")
            Me.Calendar1.VisibleDate = Today_Date



        End If
        '--
        If Page.IsPostBack Then
            If Request("Flag") = "YES" Then
                Dim FromDate As Date = CDate(Me.ddYear.SelectedValue & "/" & Me.ddMonth.SelectedValue & "/1")
                Me.Calendar1.VisibleDate = FromDate
            End If
        End If
        If Page.IsPostBack Then
            ' Response.Write(Me.Calendar1.VisibleDate.Year.ToString() + "<hr/>")
            ' xDT = dbUtil.dbGetDataTable("B2B", "Select * from ShippingCalendar_new WHERE JAHR = '" + ddYear.SelectedValue + "' and IDENT ='" + Orgbysession.ToString.Trim.ToUpper() + "'")
        Else
            xDT = tbOPBase.dbGetDataTable("B2B", "Select * from ShippingCalendar_new WHERE JAHR = '" + Date.Now.Year.ToString + "'  and IDENT ='" + Orgbysession.ToString.Trim.ToUpper() + "'")

            For i As Integer = 0 To Me.ddMonth.Items.Count - 1
                If Me.ddMonth.Items(i).Value = Date.Now.Month.ToString Then
                    Me.ddMonth.SelectedIndex = i
                    Exit For
                End If
            Next
        End If


    End Sub

    Private Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        Dim o As Calendar = CType(sender, Calendar)
        Dim DateV As Date = o.SelectedDate.ToShortDateString
        Dim P As Page = Me.Parent.Page
        Dim TP As Type = P.GetType()
        Dim MI As System.Reflection.MethodInfo = Nothing
        MI = TP.GetMethod("PickCalEnd")
        Dim para(0) As Object
        para(0) = DateV
        If Me.hDesC.Value <> "" Then
            MI.Invoke(P, New Object() {DateV, CStr(Me.hDesC.Value)})
        Else
            MI.Invoke(P, para)
        End If
    End Sub

    Protected Sub Calendar1_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles Calendar1.VisibleMonthChanged
        Me.Calendar1.VisibleDate = e.NewDate
        For i As Integer = 0 To Me.ddYear.Items.Count - 1
            If Me.ddYear.Items(i).Value = Me.Calendar1.VisibleDate.Year Then
                Me.ddYear.SelectedIndex = i
                Exit For
            End If
        Next
        For i As Integer = 0 To Me.ddMonth.Items.Count - 1
            If Me.ddMonth.Items(i).Value = Me.Calendar1.VisibleDate.Month Then
                Me.ddMonth.SelectedIndex = i
                Exit For
            End If
        Next
        ' Response.Write("<hr>" + e.NewDate.ToLongDateString)
    End Sub
    Protected Sub Calendar1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.PreRender
        'Response.Write(Me.Calendar1.VisibleDate.Year.ToString() + "<hr/>")
        xDT = tbOPBase.dbGetDataTable("B2B", "Select * from ShippingCalendar_new WHERE JAHR = '" + Me.Calendar1.VisibleDate.Year.ToString() + "' and IDENT ='" + Orgbysession.ToString.Trim.ToUpper() + "'")
        Dim monthstr As String = ""
        If Me.Calendar1.VisibleDate.Month.ToString.Trim.Length = 1 Then
            monthstr = "0" + Me.Calendar1.VisibleDate.Month.ToString.Trim
        Else
            monthstr = Me.Calendar1.VisibleDate.Month.ToString.Trim
        End If
        ' Response.Write("<hr>" + monthstr + "<hr>")
        If xDT.Rows.Count > 0 Then
            ' OrderUtilities.showDT(xDT)
            If Not IsDBNull(xDT.Rows(0).Item("MON" + monthstr)) AndAlso xDT.Rows(0).Item("MON" + monthstr).ToString <> "" Then
                Dim thiscolumn As String = xDT.Rows(0).Item("MON" + monthstr).ToString
                For i As Integer = 0 To thiscolumn.Length - 1
                    Arr01.Add(Mid(thiscolumn, i + 1, 1))
                Next
            End If

        End If
        Dim sql As New StringBuilder
        sql.AppendLine("select rtrim(MOAB1)+rtrim(MOBI1)+rtrim(MOAB2)+rtrim(MOBI2)  as Monday,")
        sql.AppendLine("rtrim(DIAB1)+rtrim(DIBI1)+rtrim(DIAB2)+rtrim(DIBI2)  as Tuesday,")
        sql.AppendLine("rtrim(MIAB1)+rtrim(MIBI1)+rtrim(MIAB2)+rtrim(MIBI2)  as Wednesday,")
        sql.AppendLine("rtrim(DOAB1)+rtrim(DOBI1)+rtrim(DOAB2)+rtrim(DOBI2)  as Thursday,")
        sql.AppendLine("rtrim(FRAB1)+rtrim(FRBI1)+rtrim(FRAB2)+rtrim(FRBI2)  as Friday,")
        sql.AppendLine("rtrim(SAAB1)+rtrim(SABI1)+rtrim(SAAB2)+rtrim(SABI2)  as Saturday,")
        sql.AppendLine("rtrim(SOAB1)+rtrim(SOBI1)+rtrim(SOAB2)+rtrim(SOBI2)  as Sunday")
        sql.AppendLine("from SAP_COMPANY_CALENDAR")
        sql.AppendLine(String.Format("where KUNNR='{0}'", Me.hCompany.Value))
        Dt_sap_company_calendar = tbOPBase.dbGetDataTable("B2B", sql.ToString)
        'For i As Integer = 0 To Arr01.Count - 1
        '    Response.Write("<br>" + Arr01(i))
        'Next
    End Sub

    Protected Sub Calendar1_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles Calendar1.DayRender
        If e.Day.IsOtherMonth = True Then
            e.Cell.Text = ""
        Else
            If Arr01.Count > 0 Then
                If e.Day.Date < Today_Date Then
                    e.Cell.Text = "<font color=""Silver"">" & e.Day.Date.Day & "</font>"
                ElseIf e.Day.Date = Today_Date Then
                    e.Cell.Text = e.Day.Date.Day
                Else
                    ' Dim dateit As New DateTime(Me.Calendar1.VisibleDate.Year, Me.Calendar1.VisibleDate.Month, i)
                    'Response.Write("<br>" + Arr01.Count.ToString() + ":" + (e.Day.Date.Day - 1).ToString)
                    Try
                        If Arr01.Count >= Integer.Parse(e.Day.Date.Day - 1) AndAlso Arr01(e.Day.Date.Day - 1).ToString = "1" Then
                            'e.Cell.Attributes.Add("onclick", "PickDate('" & Me.txtElement.Text & "','" & FormatDate(Me.txtFormat.Text, e.Day.Date) & "')")
                        Else
                            e.Cell.Text = "<font color=""Silver"">" & e.Day.Date.Day & "</font>"
                        End If
                    Catch ex As Exception
                        e.Cell.Text = "<font color=""Silver"">" & e.Day.Date.Day & "</font>"
                    End Try
                End If

            Else

                'e.Cell.Attributes.Add("onclick", "PickDate('" & Me.txtElement.Text & "','" & FormatDate(Me.txtFormat.Text, e.Day.Date) & "')")
            End If

            ' MING ADD  FOR SAP_COMPANY_Clendar
            If Dt_sap_company_calendar IsNot Nothing AndAlso Dt_sap_company_calendar.Rows.Count > 0 Then
                Dim checknum As Int64 = 0
                With Dt_sap_company_calendar.Rows(0)
                    Select Case e.Day.Date.DayOfWeek
                        Case DayOfWeek.Monday
                            If Isabilityday(.Item("Monday").ToString.Trim) Then
                                checknum = 1
                            End If
                        Case DayOfWeek.Tuesday
                            If Isabilityday(.Item("Tuesday").ToString.Trim) Then
                                checknum = 1
                            End If
                        Case DayOfWeek.Wednesday
                            If Isabilityday(.Item("Wednesday").ToString.Trim) Then
                                checknum = 1
                            End If
                        Case DayOfWeek.Thursday
                            If Isabilityday(.Item("Thursday").ToString.Trim) Then
                                checknum = 1
                            End If
                        Case DayOfWeek.Friday
                            If Isabilityday(.Item("Friday").ToString.Trim) Then
                                checknum = 1
                            End If
                        Case DayOfWeek.Saturday
                            If Isabilityday(.Item("Saturday").ToString.Trim) Then
                                checknum = 1
                            End If
                        Case DayOfWeek.Sunday
                            If Isabilityday(.Item("Sunday").ToString.Trim) Then
                                checknum = 1
                            End If
                    End Select
                End With
                If checknum = 1 Then
                    'e.Cell.Attributes.Add("onclick", "PickDate('" & Me.txtElement.Text & "','" & FormatDate(Me.txtFormat.Text, e.Day.Date) & "')")

                ElseIf e.Day.Date = Today_Date Then
                    e.Cell.Text = e.Day.Date.Day
                Else
                    e.Cell.Text = "<font color=""Silver"">" & e.Day.Date.Day & "</font>"
                End If
            End If
            'end
            If CDate("12/27/2010") <= e.Day.Date AndAlso e.Day.Date <= CDate("12/31/2010") Then
                e.Cell.Text = "<font color=""Silver"">" & e.Day.Date.Day & "</font>"
            End If
            If CDate("12/2/2010") <= e.Day.Date AndAlso e.Day.Date <= CDate("12/3/2010") Then
                e.Cell.Text = "<font color=""Silver"">" & e.Day.Date.Day & "</font>"
            End If
            If CDate("11/17/2011") <= e.Day.Date AndAlso e.Day.Date <= CDate("11/18/2011") Then
                e.Cell.Text = "<font color=""Silver"">" & e.Day.Date.Day & "</font>"
            End If
        End If
    End Sub
    Public Function Isabilityday(ByVal str As String) As Boolean
        Static Num() As String = New String() {"1", "2", "3", "4", "5", "6", "7", "8", "9"}
        For Each numb As String In Num
            If str.Contains(numb) Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function
    Protected Function FormatDate(ByVal xFormat As String, ByVal xDate As String)
        Dim RetDate As String = ""

        RetDate = CDate(xDate).Date.ToString("yyyy/MM/dd")

        Return RetDate
    End Function

   
    Private Sub ddYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddYear.SelectedIndexChanged
        Me.Calendar1.VisibleDate = CDate(CType(sender, DropDownList).SelectedValue & "/01" & "/01")

    End Sub

    Private Sub ddMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddMonth.SelectedIndexChanged
        Me.Calendar1.VisibleDate = CDate(Calendar1.VisibleDate.Year & "/" & CType(sender, DropDownList).SelectedValue & "/01")
    End Sub
End Class