Imports System.IO

Public Class Signature
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadMySignatureList()
        End If
    End Sub

    Private Sub LoadMySignatureList()
        Dim str As New StringBuilder

        str.AppendLine(" SELECT SID,UserID,SignatureData,Active,FileName,LastUpdated FROM Signature ")
        str.AppendLine(" Where UserID='" & Pivot.CurrentProfile.UserId & "' Order by LastUpdated desc ")

        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", str.ToString)

        Me.GV1.DataSource = dt
        Me.GV1.DataBind()
    End Sub


    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Dim errmsg As String = String.Empty
        Me.lbUploadMessage.Text = ""
        Dim result As Boolean = upload(errmsg)
        If Not result Then
            Me.lbUploadMessage.Text = errmsg
            'preview(filename)
            'Button_Search_Click(sender, e)
            Exit Sub
        End If

        Me.LoadMySignatureList()
        'Response.Redirect("Signature.aspx")
    End Sub

    Function upload(ByRef errmsg As String) As Boolean

        Dim _FileName As String = Me.FileUpload1.PostedFile.FileName
        If String.IsNullOrEmpty(_FileName) Then
            errmsg = "Please choose a file"
            Return False
        End If
        Dim _FileType As String = Me.FileUpload1.PostedFile.ContentType
        Select Case _FileType.ToLower
            Case "image/png"
            Case "image/jpeg"
            Case "image/gif"
            Case Else
                errmsg = "Incorrect file format"
                Return Nothing
        End Select
        Dim stream As Stream = Me.FileUpload1.PostedFile.InputStream
        Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(stream)
        Dim _width As Integer = img.Width
        Dim _height As Integer = img.Height
        If _width > 500 Then
            errmsg = "Maximum Width: 500"
            Return False
        End If
        If _height > 150 Then
            errmsg = "Maximum Height : 150 "
            Return False
        End If
        '  tbOPBase.dbExecuteNoQuery("EQ", String.Format("update  Signature set Active =0 where UserID ='{0}'", Pivot.CurrentProfile.UserId))
        Dim _FileBytes() As Byte = Me.FileUpload1.FileBytes
        If _FileBytes IsNot Nothing AndAlso _FileBytes.Length > 0 Then
            Dim usl As IBUS.iUserSignatureLine = Pivot.NewLineUserSignature
            usl.Key = String.Empty : usl.UserID = Pivot.CurrentProfile.UserId
            usl.SignatureData = _FileBytes : usl.Active = True
            usl.FileName = _FileName : usl.LastUpdated = Now

            Dim MO As IBUS.iUserSignature = Pivot.NewObjUserSignature()
            Dim result As Integer = MO.Add(usl)
            MO = Nothing
            If result > 0 Then Return True
        End If
        errmsg = "Upload file process failed"
        Return False
    End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim SIDs As New ArrayList

        For i As Integer = 0 To Me.GV1.Rows.Count - 1
            Dim chk As CheckBox = Me.GV1.Rows(i).FindControl("chkKey")
            If chk.Checked Then
                SIDs.Add(GV1.DataKeys(i).Value)
            End If
        Next

        If SIDs.Count > 0 Then
            Dim str As New StringBuilder
            For i As Integer = 0 To SIDs.Count - 1
                str.AppendLine(" Delete FROM Signature ")
                str.AppendLine(" Where SID='" & SIDs.Item(i).ToString & "' ")
                str.AppendLine(" And UserID='" & Pivot.CurrentProfile.UserId & "' ")
                tbOPBase.dbExecuteNoQuery("EQ", str.ToString)
            Next
        End If

        Me.LoadMySignatureList()

    End Sub

    Private Sub GV1_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim DBITEM As DataRowView = CType(e.Row.DataItem, DataRowView)

            Dim SID As String = GV1.DataKeys(e.Row.RowIndex).Value
            Dim _rbl As RadioButtonList = CType(e.Row.FindControl("RBLIsDefaultSignature"), RadioButtonList)

            If DBITEM.Item("active") Then
                _rbl.SelectedIndex = 0
            Else
                _rbl.SelectedIndex = 1
            End If

            Dim _FileName As String = DBITEM.Item("FileName").ToString
            Dim _lbfilename As Label = CType(e.Row.FindControl("lbfilename"), Label)
            If Not String.IsNullOrEmpty(_FileName) Then
                Dim _file As New FileInfo(_FileName)
                _lbfilename.Text = _file.Name
            Else
                _lbfilename.Text = ""
            End If

        End If
    End Sub

    Protected Sub RBLIsDefaultSignature_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim obj As RadioButtonList = CType(sender, RadioButtonList), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim sid As String = Me.GV1.DataKeys(row.RowIndex).Value
        Dim str As New StringBuilder

        str.AppendLine(" Update Signature set active=0 where UserID='" & Pivot.CurrentProfile.UserId & "' ")
        If obj.SelectedValue = 1 Then
            str.AppendLine(";Update Signature set active=1 where UserID='" & Pivot.CurrentProfile.UserId & "' and SID='" & sid & "' ")
        End If
        tbOPBase.dbExecuteNoQuery("EQ", str.ToString)

        Me.LoadMySignatureList()
    End Sub
End Class