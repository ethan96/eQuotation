Imports AOnlineWall.Entity
'Imports AOnlineWall.POCOS

Public Class WallHome
    Inherits AOnlineWall.Business.BaseControls.AonlineBasePage

    Public Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Me.Theme = ""
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AddHandler WallContent1.eventBindData, AddressOf resetBind
        AddHandler CategoryTree1.eventBindData, AddressOf LoadDefaultCategory
    End Sub

    Protected Overrides Sub OnInit(e As System.EventArgs)


        AddHandler CategoryTree1.SelectedNodeChanged, AddressOf WallCategoryTree_SelectedNodeChanged
        MyBase.OnInit(e)
    End Sub

    Private Sub resetBind(ByVal wcList As List(Of WallCategory), Optional ByVal actionType As String = "UPDATE")
        Me.CategoryTree1.resetCategoryTree(wcList, actionType)
    End Sub


    Private Sub LoadDefaultCategory()
        Dim wc As WallCategory = Me.CategoryTree1.selectedCategory
        Me.WallContent1.BindWallDetail(wc)
    End Sub

    Protected Sub WallCategoryTree_SelectedNodeChanged(sender As Object, e As System.EventArgs)
        LoadDefaultCategory()
    End Sub

End Class