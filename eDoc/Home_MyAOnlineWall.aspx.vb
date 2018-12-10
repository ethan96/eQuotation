Imports AOnlineWall.Entity
Imports AOnlineWall.POCOS

Public Class Home_MyAOnlineWall
    Inherits System.Web.UI.Page

    Protected httpUrl As String = "Wall/WallHome.aspx"

    Private _homePageList As List(Of WallMenu)
    Public Property HomePageList() As List(Of WallMenu)
        Get
            If _homePageList Is Nothing Then
                _homePageList = New WallMenuService().GetWallMenu(0, False, True)
            End If
            Return _homePageList
        End Get
        Set(ByVal value As List(Of WallMenu))
            _homePageList = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindHomeAOnlineWall()
        End If
    End Sub

    Private Sub BindHomeAOnlineWall()
        If HomePageList IsNot Nothing AndAlso HomePageList.Count > 0 Then
            HomePageList = HomePageList.OrderBy(Function(p) p.MenuName).ToList()
            Dim objMenuList As New List(Of WallMenuColumn)
            Dim parentMenuList As List(Of WallMenu) = HomePageList.Where(Function(p) p.ParentMenuId = 0).ToList()
            If parentMenuList IsNot Nothing AndAlso parentMenuList.Count > 0 Then
                '把key Account加入到Sales Guideline中
                Dim salesGuidelineMenu As WallMenu = HomePageList.FirstOrDefault(Function(p) p.ParentMenuId = 0 And p.MenuName = "Sales Guideline")
                If salesGuidelineMenu IsNot Nothing Then
                    Dim keyAccountMenu As New WallMenu()
                    keyAccountMenu.MenuName = "Key Account Transfer Agreement Template"
                    keyAccountMenu.Url = String.Format("http://aasc-scan/ComReportWeb/default.aspx?id={0}&lang={1}&tempid={2}&returnURL={3}?year={4}",
                                                            Pivot.CurrentProfile.UserId.ToString(), "EN", Pivot.CurrentProfile.SSOID,
                                                            Request.Url.AbsoluteUri, Date.Now.Year.ToString())
                    keyAccountMenu.Id = 1000000
                    keyAccountMenu.ParentMenuId = salesGuidelineMenu.Id
                    HomePageList.Add(keyAccountMenu)
                End If

                Dim menuRow As Integer = parentMenuList.Count / 4
                Dim menuRowYu As Double = parentMenuList.Count / 4
                If menuRow < menuRowYu Then
                    menuRow = menuRow + 1
                End If

                Dim columnIndex As Integer = 3
                If menuRow = 0 Then
                    menuRow = 1
                    columnIndex = parentMenuList.Count - 1
                End If

                For i = 0 To columnIndex
                    Dim columnMenuList As New List(Of WallMenu)
                    For j = 0 To menuRow - 1
                        Dim currentIndex As Integer = i + (j * 4)
                        If currentIndex < parentMenuList.Count AndAlso parentMenuList(currentIndex) IsNot Nothing Then
                            columnMenuList.Add(parentMenuList(currentIndex))
                        End If

                    Next
                    Dim objMenu As New WallMenuColumn()
                    objMenu.MenuList = columnMenuList
                    objMenuList.Add(objMenu)
                Next

                rptRootMenu.DataSource = objMenuList
            End If

        End If
        rptRootMenu.DataBind()
    End Sub

    Protected Sub rptRootMenu_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptRootMenu.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim currentMenu As WallMenuColumn = CType(e.Item.DataItem, WallMenuColumn)

            Dim firstRepeater As Repeater = CType(e.Item.FindControl("rptFirstMenu"), Repeater)
            AddHandler firstRepeater.ItemDataBound, AddressOf rptFirstMenu_ItemDataBound

            firstRepeater.DataSource = currentMenu.MenuList
            firstRepeater.DataBind()
        End If

    End Sub

    Private Sub rptFirstMenu_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim parentMenu As WallMenu = CType(e.Item.DataItem, WallMenu)
            Dim secondRepeater As Repeater = CType(e.Item.FindControl("rptSecondMenu"), Repeater)
            AddHandler secondRepeater.ItemDataBound, AddressOf rptSecondMenu_ItemDataBound

            Dim secondMenuList As List(Of WallMenu) = HomePageList.Where(Function(p) p.ParentMenuId = parentMenu.Id).ToList()
            secondRepeater.DataSource = secondMenuList
            secondRepeater.DataBind()
        End If
    End Sub

    Private Sub rptSecondMenu_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim parentMenu As WallMenu = CType(e.Item.DataItem, WallMenu)
            Dim threeRepeater As Repeater = CType(e.Item.FindControl("rptThreeMenu"), Repeater)

            Dim threeMenuList As List(Of WallMenu) = HomePageList.Where(Function(p) p.ParentMenuId = parentMenu.Id).ToList()
            If threeMenuList IsNot Nothing AndAlso threeMenuList.Count > 0 Then
                Dim secondLi As HtmlGenericControl = CType(e.Item.FindControl("secondLi"), HtmlGenericControl)
                If secondLi IsNot Nothing Then
                    secondLi.Attributes.Add("class", "blod")
                End If

            End If

            threeRepeater.DataSource = threeMenuList
            threeRepeater.DataBind()
        End If
    End Sub

End Class

Public Class WallMenuColumn
    Private _menuList As List(Of WallMenu)
    Public Property MenuList() As List(Of WallMenu)
        Get
            Return _menuList
        End Get
        Set(ByVal value As List(Of WallMenu))
            _menuList = value
        End Set
    End Property

End Class