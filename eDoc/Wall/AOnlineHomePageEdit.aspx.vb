Imports AOnlineWall.Entity
Imports AOnlineWall.POCOS
Public Class AOnlineHomePageEdit
    Inherits AOnlineWall.Business.BaseControls.AonlineBasePage

#Region "Property"
    Private _selectedMenu As WallMenu
    Public Property selectedMenu() As WallMenu
        Get
            If _selectedMenu Is Nothing Then
                If Not String.IsNullOrEmpty(tvWallMenu.SelectedValue) Then
                    _selectedMenu = New WallMenuService().GetWallMenuById(Integer.Parse(tvWallMenu.SelectedValue))
                Else
                    _selectedMenu = Nothing
                End If
            End If

            Return _selectedMenu
        End Get
        Set(ByVal value As WallMenu)
            _selectedMenu = value
        End Set
    End Property
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ExistsAdmin()
            BindWallMenuTree()
        End If

    End Sub

#Region "Bind Data"
    '权限
    Private Sub ExistsAdmin()
        Dim isExistsAmin As Boolean = False
        isExistsAmin = New WallAdminService().AOnlineHomePageIsAdmin(Pivot.CurrentProfile.UserId)
        If Not isExistsAmin Then
            Response.Redirect("~/Home_MyAOnlineWall.aspx")
        End If
    End Sub

    '绑定menu tree node
    Private Sub BindWallMenuTree()
        Me.tvWallMenu.Nodes.Clear()
        Dim root As New TreeNode()
        root.Text = "Root"
        root.Value = "0000"
        root.ToolTip = "0000"
        root.Expanded = True
        tvWallMenu.Nodes.Add(root)

        Dim wmList As List(Of WallMenu) = New WallMenuService().GetWallMenu(0, False)
        If wmList IsNot Nothing And wmList.Count > 0 Then
            wmList = wmList.OrderBy(Function(p) p.MenuName).ToList()
            For Each wmItem As WallMenu In wmList.Where(Function(p) p.ParentMenuId = 0)
                Dim node As New TreeNode
                node.Text = wmItem.MenuName
                node.Value = wmItem.Id.ToString()
                node.ToolTip = wmItem.MenuName + "__" + wmItem.Id.ToString()
                node.Expanded = False
                root.ChildNodes.Add(node)

                GetCategoryNode(node, wmItem, wmList)
            Next
        End If
    End Sub

    '循环绑定 menu 子节点
    Private Sub GetCategoryNode(ByVal parentNode As TreeNode, ByVal wmParent As WallMenu, ByVal wmList As List(Of WallMenu))
        Dim subMenuList As List(Of WallMenu) = wmList.Where(Function(p) p.ParentMenuId = wmParent.Id).OrderBy(Function(p) p.Id).ToList()
        If subMenuList IsNot Nothing And subMenuList.Count > 0 Then
            For Each item As WallMenu In subMenuList
                Dim node As New TreeNode
                node.Text = item.MenuName
                node.Value = item.Id.ToString()
                node.ToolTip = item.MenuName + "__" + item.Id.ToString()
                parentNode.ChildNodes.Add(node)
                GetCategoryNode(node, item, wmList)
            Next
        End If
    End Sub

    '绑定Menu detail
    Private Sub BindMenuDetail()
        If selectedMenu IsNot Nothing Then
            lblMenuId.Text = selectedMenu.Id
            txtMenuName.Text = selectedMenu.MenuName
            rblPublish.SelectedValue = IIf(String.IsNullOrEmpty(selectedMenu.PublishStatus.ToString()), "False", selectedMenu.PublishStatus.ToString())
            txtUrl.Text = selectedMenu.Url
        End If
    End Sub

    Private Sub BindChildMenu()
        Dim parentId As Integer = Integer.Parse(tvWallMenu.SelectedValue)

        Dim wmList As List(Of WallMenu) = New List(Of WallMenu)
        Try
            wmList = New WallMenuService().GetWallMenu(parentId, True)
        Catch ex As Exception
            wmList = New WallMenuService().GetWallMenu(parentId, True)
        End Try

        If wmList IsNot Nothing Then
            rptWallMenu.DataSource = wmList
        End If
        rptWallMenu.DataBind()
    End Sub
#End Region

#Region "Tree Node"
    Private Sub tvWallMenu_SelectedNodeChanged(sender As Object, e As System.EventArgs) Handles tvWallMenu.SelectedNodeChanged
        messageDiv.Visible = False
        If tvWallMenu.SelectedValue = "0000" Then
            fieldDetail.Visible = False
            trAddUrl.Visible = False
        Else
            fieldDetail.Visible = True
            trAddUrl.Visible = True

            If tvWallMenu.SelectedNode.Parent IsNot Nothing AndAlso tvWallMenu.SelectedNode.Parent.Parent Is Nothing Then
                trEditUrl.Visible = False
            Else
                trEditUrl.Visible = True
            End If


            If selectedMenu IsNot Nothing Then
                BindMenuDetail()
            Else
                MessageBoxs.Add("Message", "Please refresh page.")
            End If

        End If

        '现在暂时只能有2级 menu
        If tvWallMenu.SelectedNode.Parent IsNot Nothing AndAlso tvWallMenu.SelectedNode.Parent.Parent IsNot Nothing AndAlso tvWallMenu.SelectedNode.Parent.Parent.Parent IsNot Nothing Then
            fieldAddMenu.Visible = False
        Else
            fieldAddMenu.Visible = True
            BindChildMenu()
        End If
    End Sub
#End Region

#Region "Button"
    '更新menu detail
    Protected Sub btnUpdateWallMenu_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not String.IsNullOrEmpty(txtMenuName.Text.Trim()) Then
            If selectedMenu IsNot Nothing Then
                Dim wm As WallMenu = selectedMenu
                wm.MenuName = txtMenuName.Text.Trim()
                wm.PublishStatus = Boolean.Parse(rblPublish.SelectedValue)
                wm.Url = txtUrl.Text.Trim()
                wm.Owner = Pivot.CurrentProfile.UserId
                Dim wmService As New WallMenuService()
                If wmService.Update(wm) Then
                    MessageBoxs.Add("Message", "Success!")
                Else
                    MessageBoxs.Add("Message", "Faild!")
                End If
            Else
                MessageBoxs.Add("Message", "Please refresh page.")
            End If
        Else
            MessageBoxs.Add("Message", "Please enter menuName!")
        End If
    End Sub
    '添加menu
    Protected Sub btnAddMenuName_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not String.IsNullOrEmpty(txtAddMenuName.Text) Then
            Dim wm As New WallMenu()
            wm.ParentMenuId = Integer.Parse(tvWallMenu.SelectedValue)
            wm.MenuName = txtAddMenuName.Text.Trim()
            wm.PublishStatus = True
            wm.Url = txtAddUrl.Text.Trim()
            wm.Owner = Pivot.CurrentProfile.UserId
            Dim wmService As New WallMenuService()
            Dim wid As Integer = wmService.Save(wm)
            If wid <> 0 Then
                Dim node As New TreeNode
                node.Text = txtAddMenuName.Text
                node.Value = wid.ToString()
                node.ToolTip = txtAddMenuName.Text + wid.ToString()
                tvWallMenu.SelectedNode.ChildNodes.Add(node)

                txtAddMenuName.Text = ""
                txtAddUrl.Text = ""
                BindChildMenu()
            Else
                MessageBoxs.Add("Message", "Faild!")
            End If
        Else
            MessageBoxs.Add("Message", "Please enter menuName!")
        End If
    End Sub
    '删除menu
    Protected Sub btnDeleteMenu_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim wid As Integer = CType(sender, Button).CommandArgument
        If wid > 0 Then
            Dim wmService As New WallMenuService()
            If wmService.getChildMenuCountById(wid) = 0 Then
                If wmService.DeleteWallMenu(wid) > 0 Then
                    BindChildMenu()
                    For Each item As TreeNode In tvWallMenu.SelectedNode.ChildNodes
                        If item.Value = wid Then
                            tvWallMenu.SelectedNode.ChildNodes.Remove(item)
                            Exit For
                        End If
                    Next
                Else
                    MessageBoxs.Add("Message", "Faild!")
                End If
            Else
                MessageBoxs.Add("Message", "This menu has child node,You need to remove child menu.")
            End If

        Else
            MessageBoxs.Add("Message", "Please refresh page.")
        End If

    End Sub
#End Region

    
End Class