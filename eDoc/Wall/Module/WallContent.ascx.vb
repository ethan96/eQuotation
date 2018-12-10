Imports AOnlineWall.Entity
Imports AOnlineWall.POCOS
Imports System.IO

Public Class WallContent
    Inherits AOnlineWall.Business.BaseControls.AonlineBaseUserControl

    Public Delegate Sub dBindWall(ByVal wcList As List(Of WallCategory), ByVal actionType As String)
    Public Event eventBindData As dBindWall

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindWallCategory()
            BindWallFile()
            ExistsAdmin()
        End If

        If ckbShowCategory.Checked Then
            categoryContainer.Attributes.Add("class", "dottedBlue containerFirst center")
        Else
            categoryContainer.Attributes.Add("class", "dottedBlue containerFirst center hide")
        End If
    End Sub

#Region "Data Bind"
    Public Sub BindWallDetail(ByVal wc As WallCategory)
        ddlFileType.SelectedIndex = 0
        If wc IsNot Nothing Then
            btnUploadDialog.Enabled = True
            hfCategoryId.Value = wc.Id.ToString()
            lblWallCategoryName.Text = wc.CategoryName
        Else
            btnUploadDialog.Enabled = False
            hfCategoryId.Value = "0"
            lblWallCategoryName.Text = "Root"
        End If
        If IsPostBack Then
            BindWallCategory()
            BindWallFile()
        End If

    End Sub

    Private Function BindWallCategory() As List(Of WallCategory)
        Dim categoryId As Integer = Integer.Parse(hfCategoryId.Value)

        Dim wcList As List(Of WallCategory) = New WallCategoryService().GetWallCategory(categoryId, True)
        If wcList IsNot Nothing And wcList.Count > 0 Then
            lblCategoryMessage.Visible = False
            gvWallCategory.DataSource = wcList
        Else
            lblCategoryMessage.Visible = True
        End If
        gvWallCategory.DataBind()
        Return wcList
    End Function

    Private Sub BindWallFile()
        Dim categoryId As Integer = Integer.Parse(hfCategoryId.Value)
        Dim wfList As List(Of WallFile) = Nothing
        Dim wfService As New WallFileService()
        If categoryId = 0 Then
            wfList = wfService.GetAllWallFile()
        Else
            wfList = wfService.GetWallFileByCategory(categoryId)
        End If

        If wfList IsNot Nothing And wfList.Count > 0 Then
            lblWallMessage.Visible = False
            gvWall.DataSource = wfList
        Else
            lblWallMessage.Visible = True
        End If
        gvWall.DataBind()
    End Sub

    Private Sub ExistsAdmin()
        Dim isExistsAmin As Boolean = False
        isExistsAmin = New WallAdminService().AOnlineDocIsAdmin(Pivot.CurrentProfile.UserId)
        If isExistsAmin Then
            btnDeleteSelected.Visible = True
        Else
            btnDeleteSelected.Visible = False
        End If

    End Sub




#End Region

#Region "GridView"

#Region "WallCategory"
    Protected Sub gvWallCategory_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gvWallCategory.EditIndex = e.NewEditIndex
        BindWallCategory()
    End Sub

    Protected Sub gvWallCategory_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        gvWallCategory.EditIndex = -1
        BindWallCategory()
    End Sub
    
    Protected Sub gvWallCategory_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim categoryId As Integer = Integer.Parse(CType(gvWallCategory.Rows(e.RowIndex).FindControl("imgDelete"), ImageButton).CommandArgument)
    End Sub

    Protected Sub gvWallCategory_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim categoryId As Integer = Integer.Parse(CType(gvWallCategory.Rows(e.RowIndex).FindControl("imgUpdate"), ImageButton).CommandArgument)
        Dim wcService As New WallCategoryService()
        Dim wc As WallCategory = wcService.GetWallCategoryById(categoryId)
        If wc IsNot Nothing Then
            Dim txtCategoryName As String = CType(gvWallCategory.Rows(e.RowIndex).FindControl("txtCategoryName"), TextBox).Text.Trim()
            wc.CategoryName = IIf(String.IsNullOrEmpty(txtCategoryName), wc.CategoryName, txtCategoryName)
            wc.Owner = Pivot.CurrentProfile.UserId
            If wcService.UpdateWallCategory(wc) > 0 Then
                gvWallCategory.EditIndex = -1
                Dim wcList As List(Of WallCategory) = BindWallCategory()
                '绑定tree
                RaiseEvent eventBindData(wcList, "UPDATE")
            Else
                ShowMessage("Message", "Faild.")
            End If

        Else
            ShowMessage("Message", "Please refresh page.")
        End If

    End Sub
#End Region

#Region "WallFile"

    Protected Sub gvWall_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim currentWf As WallFile = CType(e.Row.DataItem, WallFile)

            Dim imgType As Image = CType(e.Row.FindControl("imgType"), Image)
            If currentWf.Type.Equals("Web Link") Then
                imgType.ImageUrl = "~/Images/webIE.jpg"
            End If

            Dim lblFileSize As Label = CType(e.Row.FindControl("lblFileSize"), Label)
            If currentWf.FileSize >= 1024 Then
                lblFileSize.Text = String.Format("{0:N2}", ((currentWf.FileSize * 1000 / 1024) / 1000)) + "MB"
            Else
                lblFileSize.Text = currentWf.FileSize.ToString() + "KB"
            End If

            If e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Alternate Then
                Dim extName As String = ""
                Try
                    'url 或者 文件名错误.
                    extName = System.IO.Path.GetExtension(currentWf.FileName).ToLower()
                Catch ex As Exception

                End Try
                If currentWf.Type.Equals("Web Link") Then
                    Dim hlName As HyperLink = CType(e.Row.FindControl("hlName"), HyperLink)
                    hlName.NavigateUrl = currentWf.FileName
                    hlName.Target = "_blank"
                    hlName.Visible = True

                    Dim imgDown As ImageButton = CType(e.Row.FindControl("imgDown"), ImageButton)
                    imgDown.Visible = False

                    Dim hlDown As HyperLink = CType(e.Row.FindControl("hlDown"), HyperLink)
                    hlDown.NavigateUrl = currentWf.FileName
                    hlDown.Target = "_blank"
                    hlDown.Visible = True
                ElseIf (AOnlineWall.Business.Utilities.AOnlineUrlExtension.Contains(extName)) Then
                    Dim hlName As HyperLink = CType(e.Row.FindControl("hlName"), HyperLink)
                    hlName.NavigateUrl = "~/resource/" + currentWf.FileName
                    hlName.Target = "_blank"
                    hlName.Visible = True
                Else
                    Dim lbName As LinkButton = CType(e.Row.FindControl("lbName"), LinkButton)
                    lbName.ToolTip = currentWf.Name
                    lbName.CommandArgument = currentWf.FileName
                    lbName.Visible = True
                End If

            End If

        End If

    End Sub

    Protected Sub lbDown_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim fileName As String = CType(sender, LinkButton).CommandArgument
        Dim name As String = CType(sender, LinkButton).ToolTip
        DownFile(fileName, name)
    End Sub

    Protected Sub gvWall_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gvWall.EditIndex = e.NewEditIndex
        BindWallFile()
    End Sub

    Protected Sub gvWall_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        gvWall.EditIndex = -1
        BindWallFile()
    End Sub

    Protected Sub gvWall_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvWall.PageIndex = e.NewPageIndex
        BindWallFile()
    End Sub

    Protected Sub gvWall_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim fid As Integer = Integer.Parse(CType(gvWall.Rows(e.RowIndex).FindControl("imgUpdate"), ImageButton).CommandArgument)
        Dim wfService As New WallFileService()
        Dim wf As WallFile = wfService.GetWallFileById(fid)

        If wf IsNot Nothing Then
            Dim txtName As String = CType(gvWall.Rows(e.RowIndex).FindControl("txtName"), TextBox).Text.Trim()
            'If editorSubject.Content.Trim().Length > 2000 Then
            '    ShowMessage("Message", "Subject max length 2000 character!")
            '    Return
            'End If
            wf.Name = IIf(String.IsNullOrEmpty(txtName), wf.Name, txtName)
            wf.Owner = Pivot.CurrentProfile.UserId
            If wfService.Update(wf) > 0 Then
                gvWall.EditIndex = -1
                BindWallFile()
            Else
                ShowMessage("Message", "Faild.")
            End If
        Else
            ShowMessage("Message", "Please refresh page.")
        End If
    End Sub

    '编辑Subject Name
    Protected Sub imgEdit_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim fid As Integer = Integer.Parse(CType(sender, ImageButton).CommandArgument)
        If fid > 0 Then
            Dim wfService As New WallFileService()
            Dim wf As WallFile = wfService.GetWallFileById(fid)
            If wf IsNot Nothing Then
                editorHtmlSubject.Content = wf.Name
                btnEditSubjectName.CommandArgument = wf.Id
                ScriptManager.RegisterStartupScript(Page, Me.GetType(), "ShowEditDialog", "showDialogById('editSubjectContainer','Edit Doc',615,'auto');", True)
            End If
        End If
    End Sub

    Protected Sub imgDown_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim fileName As String = CType(sender, ImageButton).CommandArgument
        Dim name As String = CType(sender, ImageButton).AlternateText
        DownFile(fileName, name)
    End Sub
#End Region

#End Region

#Region "Button"
    '添加category
    Protected Sub btnNewCategory_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not String.IsNullOrEmpty(txtCategoryName.Text.Trim()) Then
            Dim wc As New WallCategory()
            wc.CategoryName = txtCategoryName.Text.Trim()
            wc.ParentCategoryId = Integer.Parse(hfCategoryId.Value)
            wc.Owner = Pivot.CurrentProfile.UserId
            If New WallCategoryService().Save(wc) <> 0 Then
                txtCategoryName.Text = ""
                Dim wcList As List(Of WallCategory) = BindWallCategory()
                '绑定tree
                RaiseEvent eventBindData(wcList, "ADD")
            Else
                ShowMessage("Message", "Faild.")
            End If

        Else
            ShowMessage("Message", "Please enter category name.")
        End If

    End Sub

    '添加 file or web link
    Protected Sub btnUploadFile_Click(ByVal sender As Object, ByVal e As EventArgs)
        If editorSubject.Content.Trim().Length > 2000 Then
            ShowMessage("Message", "Subject max length 2000 character!")
            Return
        End If

        Dim wf As New WallFile()
        If ddlFileType.SelectedIndex = 0 Then
            If fuFile.HasFile Then
                Dim filePath As String = System.Configuration.ConfigurationManager.AppSettings.Get("WallFile_Path")
                Dim fileName As String = New AOnlineWall.Business.Common().UploadFile(filePath, fuFile)

                If Not String.IsNullOrEmpty(fileName) Then
                    wf.Name = IIf(String.IsNullOrEmpty(editorSubject.Content.Trim()), fuFile.FileName, editorSubject.Content.Trim())
                    wf.FileName = fileName
                    wf.FileSize = (fuFile.PostedFile.ContentLength * 1000.0 / 1024) / 1000.0
                    If wf.FileSize > 20480 Then
                        ShowMessage("Message", "File Max Size 20MB!")
                        Return
                    End If
                Else
                    ddlFileType.SelectedIndex = 0
                    ShowMessage("Message", "Please upload file.")
                    ScriptManager.RegisterStartupScript(Page, Me.GetType(), "ShowDialog", "showDialogById('containerUpload','Add Doc',615,'auto');", True)
                    Return
                End If
            Else
                ddlFileType.SelectedIndex = 0
                ShowMessage("Message", "Please upload file.")
                ScriptManager.RegisterStartupScript(Page, Me.GetType(), "ShowDialog", "showDialogById('containerUpload','Add Doc',615,'auto');", True)
                Return
            End If
        Else
            wf.Name = IIf(String.IsNullOrEmpty(editorSubject.Content.Trim()), txtWebUrlLink.Text.Trim(), editorSubject.Content.Trim())
            wf.FileName = txtWebUrlLink.Text.Trim()
        End If

        wf.Type = ddlFileType.SelectedValue
        wf.Owner = Pivot.CurrentProfile.UserId
        wf.CategoryId = Integer.Parse(hfCategoryId.Value)

        ddlFileType.SelectedIndex = 0
        If New WallFileService().Save(wf) > 0 Then
            editorSubject.Content = ""
            txtWebUrlLink.Text = ""
            BindWallFile()
        Else
            ShowMessage("Message", "Faild.")
        End If
    End Sub

    '修改Subject Name
    Protected Sub btnEditSubjectName_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtName As String = editorHtmlSubject.Content.Trim()
        If txtName.Length > 2000 Then
            ShowMessage("Message", "Subject max length 2000 character!")
            Return
        End If

        Dim fid As Integer = Integer.Parse(CType(sender, Button).CommandArgument)
        Dim wfService As New WallFileService()
        Dim wf As WallFile = wfService.GetWallFileById(fid)

        If wf IsNot Nothing Then
            wf.Name = IIf(String.IsNullOrEmpty(txtName), wf.Name, txtName)
            wf.Owner = Pivot.CurrentProfile.UserId
            If wfService.Update(wf) > 0 Then
                'gvWall.EditIndex = -1
                BindWallFile()
            Else
                ShowMessage("Message", "Faild.")
            End If
        Else
            ShowMessage("Message", "Please refresh page.")
        End If
    End Sub

    Protected Sub btnDeleteSelected_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim removeIdList As New List(Of Integer)
        For Each row As GridViewRow In gvWall.Rows
            Dim ckbDelete As CheckBox = CType(row.FindControl("ckbDelete"), CheckBox)
            If ckbDelete.Checked Then
                Dim hfFileId As HiddenField = CType(row.FindControl("hfFileId"), HiddenField)
                removeIdList.Add(Integer.Parse(hfFileId.Value))
            End If
        Next

        If removeIdList.Count > 0 Then
            Dim filePath As String = System.Configuration.ConfigurationManager.AppSettings.Get("WallFile_Path")
            If New WallFileService().DeleteWallFileList(removeIdList, filePath) Then
                BindWallFile()
            Else
                ShowMessage("Message", "Faild.")
            End If
        End If

    End Sub

#End Region

#Region "Business"
    '下载文件
    Private Sub DownFile(ByVal fileName As String, ByVal name As String)
        Dim filePath As String = System.Configuration.ConfigurationManager.AppSettings.Get("WallFile_Path")
        Dim fileAddress As String = filePath + fileName
        Dim extName As String = Path.GetExtension(fileName)
        '防止出现html代码
        Try
            fileName = Path.GetFileNameWithoutExtension(name) + extName
        Catch ex As Exception
        End Try

        If File.Exists(fileAddress) Then
            Response.Buffer = True
            Response.ContentType = "application/octet-stream"
            Response.ContentEncoding = System.Text.Encoding.UTF8
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.GetEncoding("utf-8")))
            Response.WriteFile(fileAddress)
            Response.Flush()
            Response.End()
        End If
    End Sub
#End Region

End Class