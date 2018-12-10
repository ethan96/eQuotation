Imports AOnlineWall.Entity
Imports AOnlineWall.POCOS

Public Class CategoryTree
    Inherits System.Web.UI.UserControl

    Public Delegate Sub dBindWall()
    Public Event eventBindData As dBindWall
    Public Event SelectedNodeChanged As EventHandler

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindRootCategory()
            If Request.QueryString("Id") IsNot Nothing Then
                LoadDefaultCategory()
            End If
        End If
    End Sub

    Private Sub tvCategory_SelectedNodeChanged(sender As Object, e As System.EventArgs) Handles tvCategory.SelectedNodeChanged
        RaiseEvent SelectedNodeChanged(sender, e)
    End Sub

    Public Sub BindRootCategory(Optional ByVal wcList As List(Of WallCategory) = Nothing)
        If wcList Is Nothing Then
            wcList = New WallCategoryService().GetWallCategory()
        End If

        Me.tvCategory.Nodes.Clear()
        Dim root As New TreeNode()
        root.Text = "Root"
        root.Value = "0000"
        root.ToolTip = "0000"
        root.Expanded = True
        tvCategory.Nodes.Add(root)

        If wcList IsNot Nothing And wcList.Count > 0 Then
            For Each wcItem As WallCategory In wcList.Where(Function(p) p.ParentCategoryId = 0).OrderBy(Function(p) p.Id)
                Dim node As New TreeNode
                node.Text = wcItem.CategoryName
                node.Value = wcItem.Id.ToString()
                node.ToolTip = wcItem.CategoryName + "__" + wcItem.Id.ToString()
                node.Expanded = False
                root.ChildNodes.Add(node)

                GetCategoryNode(node, wcItem, wcList)
            Next
        End If
    End Sub

    Private Sub GetCategoryNode(ByVal parentNode As TreeNode, ByVal wcParent As WallCategory, ByVal wcList As List(Of WallCategory))
        Dim subCategoryList As List(Of WallCategory) = wcList.Where(Function(p) p.ParentCategoryId = wcParent.Id).OrderBy(Function(p) p.Id).ToList()
        If subCategoryList IsNot Nothing And subCategoryList.Count > 0 Then
            For Each item As WallCategory In subCategoryList
                Dim node As New TreeNode
                node.Text = item.CategoryName
                node.Value = item.Id.ToString()
                node.ToolTip = item.CategoryName + "__" + item.Id.ToString()
                parentNode.ChildNodes.Add(node)
                GetCategoryNode(node, item, wcList)
            Next
        End If
    End Sub

    Private _selectedCategory As WallCategory
    Public Property selectedCategory() As WallCategory
        Get
            If _selectedCategory Is Nothing Then
                Dim categoryId As String = tvCategory.SelectedValue
                If Not String.IsNullOrEmpty(categoryId) Then
                    _selectedCategory = New WallCategoryService().GetWallCategoryById(categoryId)
                Else
                    _selectedCategory = Nothing
                End If
            End If

            Return _selectedCategory
        End Get
        Set(ByVal value As WallCategory)
            _selectedCategory = value
        End Set
    End Property

    Public Sub resetCategoryTree(ByVal wcList As List(Of WallCategory), Optional ByVal actionType As String = "UPDATE")
        Dim currentNode As TreeNode = tvCategory.SelectedNode
        If currentNode Is Nothing Then
            currentNode = tvCategory.FindNode("0000")
        End If

        If currentNode IsNot Nothing Then
            If wcList Is Nothing Then
                wcList = New WallCategoryService().GetWallCategory()
            End If
            If actionType.Equals("UPDATE") Then
                '循环选中 节点下面所有 一级 子节点
                For Each childNode As TreeNode In currentNode.ChildNodes
                    Dim childItem As WallCategory = wcList.FirstOrDefault(Function(p) p.Id = Integer.Parse(childNode.Value))
                    If childItem IsNot Nothing Then
                        childNode.Text = childItem.CategoryName
                    End If
                Next
            ElseIf (actionType.Equals("DELETE")) Then
                '循环选中 节点下面所有 一级 子节点
                For Each childNode As TreeNode In currentNode.ChildNodes
                    Dim childItem As WallCategory = wcList.FirstOrDefault(Function(p) p.Id = Integer.Parse(childNode.Value))
                    If childItem Is Nothing Then
                        currentNode.ChildNodes.Remove(childNode)
                        Return
                    End If
                Next
            ElseIf (actionType.Equals("ADD")) Then
                For Each wcItem As WallCategory In wcList.Where(Function(p) p.ParentCategoryId = Integer.Parse(currentNode.Value)).OrderBy(Function(p) p.Id)
                    Dim isAdd As Boolean = True
                    For Each childNode As TreeNode In currentNode.ChildNodes
                        If childNode.Value = wcItem.Id.ToString() Then
                            isAdd = False
                            Exit For
                        End If
                    Next

                    If isAdd Then
                        Dim node As New TreeNode()
                        node.Text = wcItem.CategoryName
                        node.Value = wcItem.Id.ToString()
                        node.Expanded = False
                        currentNode.ChildNodes.Add(node)
                    End If
                Next

            End If

        End If

    End Sub

    Private Sub LoadDefaultCategory()
        Dim categoryId As String = Request.QueryString("id").Trim()
        For Each parentNode As TreeNode In tvCategory.Nodes
            If SelectNodeByValue(parentNode, categoryId) Then
                RaiseEvent eventBindData()
                Return
            End If
        Next
    End Sub

    Private Function SelectNodeByValue(ByVal parentNode As TreeNode, ByVal categoryId As String) As Boolean
        If parentNode.ChildNodes.Count > 0 Then
            For Each nodeItem As TreeNode In parentNode.ChildNodes
                If nodeItem.Value = categoryId Then
                    nodeItem.Selected = True
                    ExpandedParentNode(nodeItem)
                    Return True
                Else
                    If SelectNodeByValue(nodeItem, categoryId) Then
                        Return True
                    End If
                End If
            Next
        End If
        Return False
    End Function

    ''' <summary>
    ''' 打开父类 节点
    ''' </summary>
    ''' <param name="nodeItem"></param>
    ''' <remarks></remarks>
    Private Sub ExpandedParentNode(ByVal nodeItem As TreeNode)
        If nodeItem.Parent IsNot Nothing Then
            nodeItem.Parent.Expanded = True
            ExpandedParentNode(nodeItem.Parent)
        End If

    End Sub

    
End Class