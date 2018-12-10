Imports System.Web
Imports System.Web.Services

Public Class DisplayImageHandler
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        'context.Response.ContentType = "text/plain"
        'context.Response.Write("Hello World!")

        Dim _ContentType As String = context.Request("Type"), ImageID As String = context.Request("ImageID")

        Dim obj As Byte() = Nothing

        Select Case _ContentType.ToLower
            Case "signature"
                obj = GetSignatureData(ImageID)
        End Select

        If obj Is Nothing Then
            HttpContext.Current.ApplicationInstance.CompleteRequest()
            Exit Sub
        End If

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.ContentType = "image/Jpeg"
        HttpContext.Current.Response.BinaryWrite(obj)

        'TC:HttpContext.Current.ApplicationInstance.CompleteRequest() instead of context.Response.End() to prevent Thread Abort Exception
        'context.Response.End()
        HttpContext.Current.ApplicationInstance.CompleteRequest()

    End Sub


    Public Function GetSignatureData(ByVal SID As String) As Byte()

        Dim _SQL As String = String.Empty
        _SQL = "Select SignatureData From Signature where SID=@SID "

        Dim apt As New SqlClient.SqlDataAdapter(_SQL, ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        apt.SelectCommand.Parameters.AddWithValue("SID", SID)
        Dim _dt As New DataTable
        apt.Fill(_dt)
        apt.SelectCommand.Connection.Close()

        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            Return _dt.Rows(0).Item("SignatureData")
        End If

        Return Nothing

    End Function


    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property






    'Public Shared Function GenerateThumbnail_V2(ByVal url As String, ByVal File_Ext As String, ByVal width As Integer, ByVal height As Integer, ByRef ErrMsg As String) As Byte()
    '    Try

    '        'Dim bmp As Drawing.Bitmap = WebsiteThumbnail.GetThumbnail(url, 600, 900, width, height)
    '        Dim bmp As Drawing.Bitmap = WebsiteThumbnail.GetThumbnail(url, width, height, width, height)

    '        Dim ms As New System.IO.MemoryStream()

    '        Select Case File_Ext.ToUpper
    '            Case "PNG"
    '                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
    '            Case "JPG"
    '                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
    '            Case "BMP"
    '                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp)
    '            Case "GIF"
    '                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Gif)
    '            Case "TIF"
    '                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff)
    '        End Select

    '        Return ms.ToArray()
    '    Catch ex As Exception
    '        ErrMsg = ex.ToString : Return Nothing
    '    End Try
    'End Function


    'Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

    '    'Get input parameters
    '    Dim _ContentType As String = context.Request("Type"), _ContentID As String = context.Request("ContentId")

    '    Dim obj As Byte() = Nothing

    '    Select Case _ContentType.ToUpper

    '        Case "CMS"

    '            'Frank:Get image cache from database
    '            obj = GetThumbnailCache(_ContentType, _ContentID)

    '            If obj Is Nothing Then

    '                Dim _dt As DataTable = dbUtil.dbGetDataTable("MY", String.Format("Select CMS_CONTENT from WWW_RESOURCES_DETAIL where RECORD_ID='{0}'", _ContentID))
    '                Dim _CMSContent As String = ""

    '                If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
    '                    _CMSContent = _dt.Rows(0).Item("CMS_CONTENT") & ""
    '                End If

    '                Dim webClient As New HtmlWeb

    '                'Dim sms() As Byte = System.Text.Encoding.Default.GetBytes(_CMSContent)
    '                Dim _encoding As System.Text.Encoding = New System.Text.UTF8Encoding

    '                Dim sms() As Byte = _encoding.GetBytes(_CMSContent)

    '                Dim ms As MemoryStream = New MemoryStream(sms)
    '                Dim doc As HtmlDocument = New HtmlDocument
    '                doc.Load(ms, True)

    '                Dim _IsFound As Boolean = False, _errmsg As String = String.Empty

    '                Dim _HttpWebReq As HttpWebRequest = Nothing, _HttpWebRespon As HttpWebResponse = Nothing

    '                Dim _SearchNodes As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//img")

    '                Dim _url As String = String.Empty, _urlstart As String = String.Empty, _urlarr() As String = Nothing, _IsHttpUrl As Boolean = False

    '                If _SearchNodes IsNot Nothing Then

    '                    For Each _node As HtmlNode In _SearchNodes

    '                        For Each _att As HtmlAttribute In _node.Attributes

    '                            If _att.Name.Equals("SRC", StringComparison.InvariantCultureIgnoreCase) Then

    '                                Try

    '                                    _IsHttpUrl = False
    '                                    'Frank 2012/06/13:To prevent the double / in URL to make http exception
    '                                    '_HttpWebReq = CType(System.Net.WebRequest.Create(_att.Value), HttpWebRequest)
    '                                    If _att.Value.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) Then
    '                                        _IsHttpUrl = True
    '                                        _urlstart = "http://"
    '                                    End If
    '                                    If _att.Value.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase) Then
    '                                        _IsHttpUrl = True
    '                                        _urlstart = "https://"
    '                                    End If
    '                                    If Not _IsHttpUrl Then Continue For

    '                                    _url = _att.Value.Substring(_urlstart.Length, _att.Value.Length - _urlstart.Length)
    '                                    _url = _urlstart & _url.Replace("//", "/")

    '                                    _HttpWebReq = CType(System.Net.WebRequest.Create(_url), HttpWebRequest)
    '                                    _HttpWebReq.Timeout = 30000
    '                                    _HttpWebRespon = CType(_HttpWebReq.GetResponse(), HttpWebResponse)

    '                                    If _HttpWebRespon.StatusCode = HttpStatusCode.OK Then

    '                                        Dim _st As IO.Stream = _HttpWebRespon.GetResponseStream
    '                                        Dim _memstream As MemoryStream = StreamToMemory(_st)
    '                                        obj = GetStreamAsByteArray(_memstream)

    '                                        _IsFound = True
    '                                        Exit For
    '                                    End If
    '                                Catch ex As WebException
    '                                    Dim _exstring As String = "Creating thumbnail error for <img> tag in CMS page:"
    '                                    _exstring &= Environment.NewLine & "Type=" & _ContentType & "&contentid=" & _ContentID
    '                                    _exstring &= Environment.NewLine & "URL=" & _att.Value
    '                                    _exstring &= Environment.NewLine & Environment.NewLine & ex.Message
    '                                    Util.InsertMyErrLog(_exstring)
    '                                End Try
    '                            End If

    '                        Next

    '                        'Frnak:If got the bytes of image the exit for loop
    '                        If _IsFound Then Exit For

    '                    Next
    '                    _SearchNodes = Nothing
    '                End If

    '                Dim _err As String = String.Empty

    '                'Frnak:If can not get img tag then get thumbnail image by CMS page
    '                If Not _IsFound Then

    '                    Dim _CMSURL As String = "http://resources.advantech.com/Resources/Details.aspx?rid="

    '                    Try

    '                        _HttpWebReq = CType(System.Net.WebRequest.Create(_CMSURL & _ContentID), HttpWebRequest)
    '                        _HttpWebRespon = CType(_HttpWebReq.GetResponse(), HttpWebResponse)

    '                        If _HttpWebRespon.StatusCode = HttpStatusCode.OK Then
    '                            obj = GenerateThumbnail_V2(_CMSURL & _ContentID, "jpg", Me.ImageWitdh, Me.ImageHeight, _err)
    '                            _IsFound = True
    '                        End If
    '                    Catch ex As WebException
    '                        Dim _exstring As String = "Creating thumbnail error for CMS page:"
    '                        _exstring &= Environment.NewLine & "Type=" & _ContentType & "&contentid=" & _ContentID
    '                        _exstring &= Environment.NewLine & "URL=" & _CMSURL & _ContentID
    '                        _exstring &= Environment.NewLine & Environment.NewLine & ex.Message
    '                        Util.InsertMyErrLog(_exstring)
    '                    End Try

    '                End If

    '                _HttpWebReq = Nothing
    '                _HttpWebRespon = Nothing

    '                'Frnak:If can not get image by CMS page then get default image
    '                If _IsFound And obj IsNot Nothing Then
    '                    Me.MaintainThumbnailCache(_ContentType, _ContentID, obj)
    '                End If

    '            End If

    '        Case "ECAMPAIGN"

    '            'Frnak:Get image cache from database
    '            obj = dbUtil.dbExecuteScalar("MY", String.Format("Select thumbnail from campaign_thumbnail where campaign_row_id='{0}'", _ContentID))

    '        Case "PIS"

    '            'Frnak:Get image cache from database
    '            obj = GetThumbnailCache(_ContentType, _ContentID)

    '            If obj Is Nothing Then

    '                Dim _dt As DataTable = dbUtil.dbGetDataTable("PIS", String.Format("Select Literature_ID,FILE_EXT from PIS.dbo.Literature where Literature_ID='{0}'", _ContentID))

    '                If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then

    '                    'http://downloadt.advantech.com/download/downloadlit.aspx?LIT_ID=224a9922-3b70-401a-ab99-846c20859533
    '                    Dim _url As String = "http://downloadt.advantech.com/download/downloadlit.aspx?LIT_ID="

    '                    Dim _literID As String = String.Empty, _sql As String = String.Empty, _err As String = String.Empty, _file_ext As String = String.Empty

    '                    Select Case _dt.Rows(0).Item("FILE_EXT").ToString.ToLower

    '                        Case "jpg", "gif", "png", "jpeg", "bmp", "tif", "tiff"
    '                            _literID = _dt.Rows(0).Item("Literature_ID")
    '                            _file_ext = _dt.Rows(0).Item("FILE_EXT").ToString

    '                            obj = GenerateThumbnail_V2(_url & _literID, _file_ext, Me.ImageWitdh, Me.ImageHeight, _err)

    '                        Case Else 'Frnak:maybe pdf or another file type not in "jpg", "gif", "png", "jpeg", "bmp", "tif", "tiff"

    '                            Dim _dt1 As DataTable = Nothing
    '                            'Frnak:Search the literature under which model, then get the display image of the model
    '                            _sql = "Select a.model_name, "
    '                            _sql &= " isnull((select top 1 LITERATURE.LITERATURE_ID from PIS.dbo.Model_lit inner join PIS.dbo.LITERATURE"
    '                            _sql &= " ON MODEL_LIT.LITERATURE_ID = LITERATURE.LITERATURE_ID"
    '                            _sql &= " WHERE(MODEL_LIT.MODEL_NAME = a.model_name)"
    '                            _sql &= " and literature.FILE_EXT in ('jpg','JPG','gif', 'GIF','png','tif','tiff')"
    '                            _sql &= " and literature.LIT_TYPE in ('Product - Photo(Main)','Product - Photo(B)','Product - Photo(S)')"
    '                            _sql &= " order by (case when LITERATURE.LIT_TYPE='Product - Photo(Main)' then 0 else 1 end)"
    '                            _sql &= " ),'') as tumbnail_image_id"
    '                            _sql &= " From PIS.dbo.Model_lit a"
    '                            _sql &= String.Format(" Where a.literature_id='{0}' ", _ContentID)

    '                            _dt1 = dbUtil.dbGetDataTable("PIS", _sql)

    '                            If _dt1 IsNot Nothing AndAlso _dt1.Rows.Count > 0 Then

    '                                _literID = _dt1.Rows(0).Item("tumbnail_image_id")

    '                                _sql = "Select literature_id,File_Ext From Literature"
    '                                _sql &= String.Format(" Where literature_id='{0}' ", _literID)
    '                                _dt1 = dbUtil.dbGetDataTable("PIS", _sql)

    '                                If _dt1 IsNot Nothing AndAlso _dt1.Rows.Count > 0 Then
    '                                    _file_ext = _dt1.Rows(0).Item("File_Ext").ToString
    '                                End If

    '                                If String.IsNullOrEmpty(_file_ext) Then
    '                                    _file_ext = "jpg"
    '                                End If

    '                                obj = GenerateThumbnail_V2(_url & _literID, _file_ext, Me.ImageWitdh, Me.ImageHeight, _err)

    '                            End If

    '                            _dt1 = Nothing

    '                    End Select

    '                    If obj IsNot Nothing Then
    '                        Me.MaintainThumbnailCache(_ContentType, _ContentID, obj)
    '                    End If

    '                End If

    '                _dt = Nothing

    '            End If

    '        Case "TEMPLATE"

    '            Dim _ForwardTempDetailTAP As New ForwardTemplateDALTableAdapters.AONLINE_FORWARD_CONTENT_TEMPLATE_IMAGE_CACHETableAdapter
    '            Dim _FileDT As ForwardTemplateDAL.AONLINE_FORWARD_CONTENT_TEMPLATE_IMAGE_CACHEDataTable = _ForwardTempDetailTAP.GetDataByFileID(_ContentID)
    '            If _FileDT.Rows.Count > 0 Then
    '                Dim _FileRow As ForwardTemplateDAL.AONLINE_FORWARD_CONTENT_TEMPLATE_IMAGE_CACHERow = _FileDT.Rows(0)
    '                obj = _FileRow.FILE_BYTES
    '            End If
    '            _FileDT = Nothing : _ForwardTempDetailTAP = Nothing
    '            If obj Is Nothing OrElse obj.Length = 0 Then obj = GetDefaultImageInBytes()
    '            HttpContext.Current.Response.Clear()
    '            HttpContext.Current.Response.ContentType = "image/Jpeg"
    '            HttpContext.Current.Response.BinaryWrite(obj)

    '            'TC:HttpContext.Current.ApplicationInstance.CompleteRequest() instead of context.Response.End() to prevent Thread Abort Exception
    '            'context.Response.End()
    '            HttpContext.Current.ApplicationInstance.CompleteRequest()

    '    End Select

    '    'Frnak:If can not get image bytes then showing up the default image
    '    If obj Is Nothing OrElse obj.Length = 0 Then
    '        obj = GetDefaultImageInBytes()
    '        Me.MaintainThumbnailCache(_ContentType, _ContentID, obj)
    '    End If

    '    HttpContext.Current.Response.Clear()
    '    HttpContext.Current.Response.ContentType = "image/Jpeg"
    '    HttpContext.Current.Response.BinaryWrite(obj)

    '    'TC:HttpContext.Current.ApplicationInstance.CompleteRequest() instead of context.Response.End() to prevent Thread Abort Exception
    '    'context.Response.End()
    '    HttpContext.Current.ApplicationInstance.CompleteRequest()

    'End Sub

    'Private Function GetDefaultImageInBytes() As Byte()

    '    Dim _defaultlogo As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images\publisher.png")

    '    If File.Exists(_defaultlogo) Then
    '        Return Me.ConvertImageFiletoBytes(_defaultlogo)
    '    End If

    '    Return Nothing

    'End Function

    'Public Function GetThumbnailCache(ByVal _SOURCE_TYPE As String, ByVal _SOURCE_ID As String) As Byte()

    '    Dim _sql As String = String.Empty
    '    _sql = String.Format("Select top 1 THUMBNAIL_BYTES From THUMBNAIL_CACHE where SOURCE_TYPE='{0}' and SOURCE_ID='{1}' and CACHED_DATE>=GETDATE()-30 and THUMBNAIL_BYTES is not null", _SOURCE_TYPE, _SOURCE_ID)

    '    Dim _dt As DataTable = dbUtil.dbGetDataTable("MY", _sql)
    '    _sql.Remove(0, _sql.Length)
    '    If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
    '        Return _dt.Rows(0).Item("THUMBNAIL_BYTES")
    '    End If

    '    Return Nothing

    'End Function

    'Public Sub MaintainThumbnailCache(ByVal _SOURCE_TYPE As String, ByVal _SOURCE_ID As String, ByVal _THUMBNAIL_CACHE As Byte())

    '    Dim _MyCon As New SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
    '    If _MyCon.State <> ConnectionState.Open Then _MyCon.Open()

    '    Dim cmd As New SqlClient.SqlCommand()
    '    cmd.Connection = _MyCon

    '    Dim _DeleteSQL As String = String.Format("Delete From THUMBNAIL_CACHE Where SOURCE_TYPE='{0}' and SOURCE_ID='{1}'", _SOURCE_TYPE, _SOURCE_ID)

    '    'cmd.CommandText = "insert into THUMBNAIL_CACHE (SOURCE_TYPE, SOURCE_ID, THUMBNAIL_BYTES) values(@SOURCE_TYPE,@SOURCE_ID,@THUMBNAIL_BYTES)"
    '    cmd.CommandText = _DeleteSQL & ";insert into THUMBNAIL_CACHE (SOURCE_TYPE, SOURCE_ID, THUMBNAIL_BYTES) values(@SOURCE_TYPE,@SOURCE_ID,@THUMBNAIL_BYTES)"
    '    cmd.Parameters.AddWithValue("SOURCE_TYPE", _SOURCE_TYPE) : cmd.Parameters.AddWithValue("SOURCE_ID", _SOURCE_ID) : cmd.Parameters.AddWithValue("THUMBNAIL_BYTES", _THUMBNAIL_CACHE)
    '    cmd.ExecuteNonQuery()

    'End Sub

    ' ''' <summary>
    ' ''' Converts the Image File to array of Bytes
    ' ''' </summary>
    ' ''' <param name="ImageFilePath">The path of the image file</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function ConvertImageFiletoBytes(ByVal ImageFilePath As String) As Byte()
    '    Dim _tempByte() As Byte = Nothing
    '    If String.IsNullOrEmpty(ImageFilePath) = True Then
    '        Throw New ArgumentNullException("Image File Name Cannot be Null or Empty", "ImageFilePath")
    '        Return Nothing
    '    End If
    '    Try
    '        Dim _fileInfo As New IO.FileInfo(ImageFilePath)
    '        Dim _NumBytes As Long = _fileInfo.Length
    '        Dim _FStream As New IO.FileStream(ImageFilePath, IO.FileMode.Open, IO.FileAccess.Read)
    '        Dim _BinaryReader As New IO.BinaryReader(_FStream)
    '        _tempByte = _BinaryReader.ReadBytes(Convert.ToInt32(_NumBytes))
    '        _fileInfo = Nothing
    '        _NumBytes = 0
    '        _FStream.Close()
    '        _FStream.Dispose()
    '        _BinaryReader.Close()
    '        Return _tempByte
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="stream"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetStreamAsByteArray(ByVal stream As System.IO.Stream) As Byte()

    '    Dim streamLength As Integer = Convert.ToInt32(stream.Length)

    '    Dim fileData As Byte() = New Byte(streamLength) {}

    '    ' Read the file into a byte array
    '    stream.Read(fileData, 0, streamLength)
    '    stream.Close()

    '    Return fileData

    'End Function

    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="input"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function StreamToMemory(ByVal input As Stream) As IO.MemoryStream

    '    Dim buffer(1023) As Byte
    '    Dim count As Integer = 1024
    '    Dim output As MemoryStream

    '    ' build a new stream
    '    If input.CanSeek Then
    '        output = New MemoryStream(input.Length)
    '    Else
    '        output = New MemoryStream
    '    End If

    '    ' iterate stream and transfer to memory stream
    '    Do
    '        count = input.Read(buffer, 0, count)
    '        If count = 0 Then Exit Do
    '        output.Write(buffer, 0, count)
    '    Loop

    '    ' rewind stream
    '    output.Position = 0

    '    ' pass back
    '    Return output

    'End Function
    'Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
    '    Get
    '        Return False
    '    End Get
    'End Property




End Class