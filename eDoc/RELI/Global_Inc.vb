Imports System.IO
Public Class Global_Inc
    Public Shared Function Format2SAPItem(ByVal Part_No As String) As String

        Try
            If IsNumericItem(Part_No) And Not Part_No.Substring(0, 1).Equals("0") Then
                Dim zeroLength As Integer = 18 - Part_No.Length
                For i As Integer = 0 To zeroLength - 1
                    Part_No = "0" & Part_No
                Next
                Return Part_No
            Else
                Return Part_No
            End If
        Catch ex As Exception
            Return Part_No
        End Try

    End Function
    Shared Function DateFormat(ByVal DATESTR As String, ByVal FF As String, ByVal TF As String, ByVal FSP As String, ByVal TSP As String) As String
        Dim Year As String = ""
        Dim Month As String = ""
        Dim Day As String = ""

        If FF.ToUpper = "YYYYMMDD" Then
            If FSP = "" Then
                Year = Left(DATESTR, 4)
                Month = Mid(DATESTR, 5, 2)
                Day = Right(DATESTR, 2)
            Else
                Year = DATESTR.Split(FSP)(0)
                Month = DATESTR.Split(FSP)(1)
                Day = DATESTR.Split(FSP)(2)
            End If
        End If
        If FF.ToUpper = "MMDDYYYY" Then
            If FSP = "" Then
                Year = Right(DATESTR, 4)
                Month = Left(DATESTR, 2)
                Day = Mid(DATESTR, 3, 2)
            Else
                Year = DATESTR.Split(FSP)(2)
                Month = DATESTR.Split(FSP)(0)
                Day = DATESTR.Split(FSP)(1)
            End If

        End If
        If FF.ToUpper = "DDMMYYYY" Then
            If FSP = "" Then
                Year = Right(DATESTR, 4)
                Month = Mid(DATESTR, 3, 2)
                Day = Left(DATESTR, 2)
            Else
                Year = DATESTR.Split(FSP)(2)
                Month = DATESTR.Split(FSP)(1)
                Day = DATESTR.Split(FSP)(0)
            End If
        End If
        If FF.ToUpper = "YYYYDDMM" Then
            If FSP = "" Then
                Year = Left(DATESTR, 4)
                Month = Right(DATESTR, 2)
                Day = Mid(DATESTR, 5, 2)
            Else
                Year = DATESTR.Split(FSP)(0)
                Month = DATESTR.Split(FSP)(2)
                Day = DATESTR.Split(FSP)(1)
            End If

        End If
        If FF.ToUpper = "MMYYYYDD" Then
            If FSP = "" Then
                Year = Mid(DATESTR, 3, 4)
                Month = Left(DATESTR, 2)
                Day = Right(DATESTR, 2)
            Else
                Year = DATESTR.Split(FSP)(1)
                Month = DATESTR.Split(FSP)(0)
                Day = DATESTR.Split(FSP)(2)
            End If
        End If
        If FF.ToUpper = "DDYYYYMM" Then
            If FSP = "" Then
                Year = Mid(DATESTR, 3, 4)
                Month = Right(DATESTR, 2)
                Day = Left(DATESTR, 2)
            Else
                Year = DATESTR.Split(FSP)(1)
                Month = DATESTR.Split(FSP)(2)
                Day = DATESTR.Split(FSP)(0)
            End If
        End If

        If TF.ToUpper = "YYYYMMDD" Then
            Return Year & TSP & Month & TSP & Day
        End If
        If TF.ToUpper = "MMDDYYYY" Then
            Return Month & TSP & Day & TSP & Year
        End If
        If TF.ToUpper = "DDMMYYYY" Then
            Return Day & TSP & Month & TSP & Year
        End If
        If TF.ToUpper = "YYYYDDMM" Then
            Return Year & TSP & Day & TSP & Month
        End If
        If TF.ToUpper = "MMYYYYDD" Then
            Return Month & TSP & Year & TSP & Day
        End If
        If TF.ToUpper = "DDYYYYMM" Then
            Return Day & TSP & Year & TSP & Month
        End If
        Return ""
    End Function
    Public Shared Function Format2SAPItem2(ByVal Part_No As String) As String

        Try
            If IsNumericItem(Part_No) And Not Part_No.Substring(0, 1).Equals("0") Then
                Dim zeroLength As Integer = 10 - Part_No.Length
                For i As Integer = 0 To zeroLength - 1
                    Part_No = "0" & Part_No
                Next
                Return Part_No
            Else
                Return Part_No
            End If
        Catch ex As Exception
            Return Part_No
        End Try

    End Function

    Public Shared Function SAPDate2StdDate(ByVal sapDateString As String) As Date

        If sapDateString.Length <> 8 Then
            Exit Function
        End If
        Dim Y, M, D As String

        Try
            Y = Left(sapDateString, 4)
            M = Mid(sapDateString, 5, 2)
            D = Right(sapDateString, 2)
            Dim stdDate As Date = CDate(Y & "/" & M & "/" & D)
            Return stdDate
        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Public Shared Function convertStr(ByRef obj)
        If InStr(obj, "'") > 0 Then
            obj = Replace(obj, "'", "''")
        End If
        Return 1
    End Function

    Public Shared Function IsNumericItem(ByVal part_no As String) As Boolean

        Dim pChar() As Char = part_no.ToCharArray()

        For i As Integer = 0 To pChar.Length - 1
            If Not IsNumeric(pChar(i)) Then
                Return False
                Exit Function
            End If
        Next

        Return True
    End Function

    Public Shared Function RemoveZeroString(ByVal NumericPart_No As String) As String

        If IsNumericItem(NumericPart_No) Then
            For i As Integer = 0 To NumericPart_No.Length - 1
                If Not NumericPart_No.Substring(i, 1).Equals("0") Then
                    Return NumericPart_No.Substring(i)
                    Exit For
                End If
            Next
            Return NumericPart_No
        Else
            Return NumericPart_No
        End If

    End Function
    Shared Sub Utility_EMailPage(ByVal FROM_Email As String, ByVal TO_Email As String, ByVal CC_Email As String, ByVal BCC_Email As String, ByVal Subject_Email As String, ByVal AttachFile As String, ByVal MailBody As String)

        Dim m1 As New System.Net.Mail.SmtpClient
        'm1.Host = Me.dbExecuteScalar("", "", "select para_value as SMTPServer1 from site_definition where site_parameter='SMTPServer1'")
        m1.Host = "172.21.34.21"
        Dim msg As New System.Net.Mail.MailMessage
        'msg.From = New System.Net.Mail.MailAddress(FROM_Email)
        msg.From = New System.Net.Mail.MailAddress("eBusiness.AEU@advantech.com")
        If TO_Email <> "" Then
            Dim ToArray As String() = Split(TO_Email, ";")
            For i As Integer = 0 To ToArray.Length - 1
                If ToArray(i) <> "" Then
                    msg.To.Add(ToArray(i))
                End If
            Next
        End If
        If CC_Email <> "" Then
            Dim CcArray As String() = Split(CC_Email, ";")
            For i As Integer = 0 To CcArray.Length - 1
                If CcArray(i) <> "" Then
                    msg.CC.Add(CcArray(i))
                End If
            Next
        End If
        If BCC_Email <> "" Then
            Dim BccArray As String() = Split(BCC_Email, ";")
            For i As Integer = 0 To BccArray.Length - 1
                If BccArray(i) <> "" Then
                    msg.Bcc.Add(BccArray(i))
                End If
            Next
        End If

        If InStr(MailBody, "<img") > 0 Then
            '20060316 TC: Handle MailBody image resources
            Try
                Dim send_mail_body As String = MailBody
                MailBody = "<xml>" & MailBody & "</xml>"
                Dim prefix As String = "<img", postfix As String = ">", imgarr As New ArrayList

                GetImgArr(MailBody, prefix, postfix, imgarr)

                Dim xml_img As String = "<xml>"
                For i As Integer = 0 To imgarr.Count - 1
                    If InStr(imgarr(i).ToString(), "/>") <= 0 Then
                        xml_img &= Replace(imgarr(i).ToString(), ">", " />")
                    Else
                        xml_img &= imgarr(i).ToString()
                    End If
                Next
                xml_img &= "</xml>"

                Dim xmlDoc As New System.Xml.XmlDataDocument
                xmlDoc.LoadXml(xml_img)

                Dim ImgLinkSrcArray(0) As System.Net.Mail.LinkedResource
                Dim ImageCounter As Integer = 0
                Dim n As System.Xml.XmlNode = xmlDoc.DocumentElement

                EmbedChildNodeImageSrc(n, ImageCounter, ImgLinkSrcArray)

                MailBody = send_mail_body

                Dim xn As System.Xml.XmlNode
                Dim count As Integer = 0
                Try
                    For Each xn In n.ChildNodes
                        'Response.Write(xn.Attributes("src").Value)
                        MailBody = Replace(MailBody, imgarr(count).ToString(), xn.OuterXml)
                        count += 1
                    Next
                Catch ex As Exception

                End Try

                Dim av1 As System.Net.Mail.AlternateView = _
                    System.Net.Mail.AlternateView.CreateAlternateViewFromString(MailBody, System.Text.Encoding.UTF8, System.Net.Mime.MediaTypeNames.Text.Html)
                For i As Integer = 0 To ImgLinkSrcArray.Length - 1
                    Try
                        av1.LinkedResources.Add(ImgLinkSrcArray(i))
                    Catch ex As Exception
                    End Try
                Next
                msg.AlternateViews.Add(av1)
            Catch ex As Exception

            End Try
        End If
        msg.Body = MailBody
        msg.IsBodyHtml = True
        msg.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8")
        msg.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8")
        msg.Subject = Subject_Email
        If Trim(AttachFile) <> "" Then
            Dim attArray As String() = Split(AttachFile, ";")
            For i As Integer = 0 To attArray.Length - 1
                Try
                    If attArray(i) <> "" And File.Exists(attArray(i)) Then
                        Dim Att As New System.Net.Mail.Attachment(attArray(i))
                        msg.Attachments.Add(Att)
                    End If
                Catch ex As Exception

                End Try
            Next
        End If
        m1.Send(msg)

        For i As Integer = 0 To msg.Attachments.Count - 1
            msg.Attachments.Item(i).ContentStream.Close()
        Next
        For i As Integer = 0 To msg.AlternateViews.Count - 1
            For j As Integer = 0 To msg.AlternateViews.Item(i).LinkedResources.Count - 1
                msg.AlternateViews.Item(i).LinkedResources.Item(j).ContentStream.Close()
            Next
        Next

    End Sub

    Shared Function GetImgArr(ByVal str As String, ByVal prefix As String, ByVal postfix As String, ByRef ImgArr As ArrayList)
        Dim len_prefix = InStr(str, prefix)
        str = Mid(str, InStr(str, prefix))
        Dim len_postfix = InStr(str, postfix)

        ImgArr.Add(Left(str, InStr(str, postfix)))
        str = Mid(str, len_postfix + 1)
        If InStr(str, prefix) > 0 Then
            GetImgArr(str, prefix, postfix, ImgArr)
        End If
        Return 1
    End Function
    Shared Sub EmbedChildNodeImageSrc(ByRef sn As System.Xml.XmlNode, ByRef ImageCounter As Integer, ByRef LinkSrcArray As System.Net.Mail.LinkedResource())

        Dim ssn As System.Xml.XmlNode
        Try
            For Each ssn In sn.ChildNodes

                If LCase(ssn.Name) = "img" Then

                    If File.Exists(ssn.Attributes("src").Value) Then

                        Dim ImgLinkSrc1 As System.Net.Mail.LinkedResource = Nothing

                        Try
                            ImgLinkSrc1 = New System.Net.Mail.LinkedResource(ssn.Attributes("src").Value)
                        Catch ex As Exception
                            'Response.Write(ex.Message)
                        End Try

                        ImgLinkSrc1.ContentId = "Img" & ImageCounter
                        ImgLinkSrc1.ContentType.Name = "Img" & ImageCounter
                        ssn.Attributes("src").Value = "cid:Img" & ImageCounter
                        ImageCounter += 1
                        ReDim Preserve LinkSrcArray(ImageCounter - 1)
                        LinkSrcArray(ImageCounter - 1) = ImgLinkSrc1

                    End If

                End If

                If ssn.ChildNodes.Count > 0 Then
                    EmbedChildNodeImageSrc(ssn, ImageCounter, LinkSrcArray)
                End If
            Next
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub
    Shared Function getDTHtml(ByVal ODT As DataTable) As String
        Dim str As String = "<table border=""1""><tr>"
        For i As Integer = 0 To ODT.Columns.Count - 1
            str &= "<td>" & ODT.Columns(i).Caption & "</td>"
        Next
        str &= "</tr>"

        For j As Integer = 0 To ODT.Rows.Count - 1
            str &= "<tr>"
            For k As Integer = 0 To ODT.Columns.Count - 1
                str &= "<td>"
                str &= ODT.Rows(j)(k)
                str &= "<br/></td>"
            Next
            str &= "</tr>"
        Next
        str &= "</table>"
        Return str
    End Function


End Class
