Public Class myMaster
    Inherits System.Web.UI.MasterPage

    Dim myTI As New TempInfo("EQ", "tbTempInfo"), myRoleManagement As New RoleManagement("EQ", "tbRoleManagement")

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsPostBack Then
            'initDrpReg()
        End If
    End Sub
    Public Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ShowMessage()
    End Sub
    Public Sub ShowMessage()
        If Not IsNothing(Page.Items("desturl")) Then
            Me.hDestUrl.Value = Page.Items("desturl")
        End If
        If Not IsNothing(Page.Items("err")) AndAlso Page.Items("err") <> "" Then
            If IsNothing(Page.Items("err")) Then Page.Items("err") = ""
            Dim Message As String = Page.Items("err")
            Me.DivGMsg.InnerHtml = Message
            upGMsg.Update()
            Me.MPGMsg.Show()
        End If
    End Sub

    Protected Sub lbLogIn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/login.aspx")
    End Sub

    Protected Sub lbLogOut_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session.Abandon() : FormsAuthentication.SignOut() : Util.ClearCookie_Login("ADEQCOOK")
        Response.Redirect("~/login.aspx")
    End Sub
  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim _isATWSales As Boolean = Role.IsTWAonlineSales
        If LCase(Request.ServerVariables("URL")).Contains("home_myaonlinewall.aspx") Then
            Me.imgLogo.ImageUrl = "~/Images/Logo_AOnlineWall.gif"
        End If
        If Not Page.IsPostBack Then
            If Not IsNothing(Pivot.CurrentProfile) Then
                lbUID.Text = Pivot.CurrentProfile.UserId.ToString()

                'Ryan 2016/09/13 Only show Aonline Wall link to US ANAonline
                'Frank 2013/02/26:Only show up the Aonline wall link for US Aonline sales
                If Role.IsAonlineUsa() Then
                    TableTopLink.Attributes.Add("Style", "width: 300px")
                    'TableTopLink.Style = "width: 300px"
                    tdMyAOnlineWall.Visible = True
                End If

            End If

            If Request("UID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request("UID")) Then
                tdQuoteid.Visible = True
                tdQuoteid.Attributes.Add("width", "150px")

                'lbQuoteid.Text = Request("UID")
                'Dim _IsOrderUID As Boolean = False
                'If LCase(Request.ServerVariables("URL")).Contains("/order/orderduedate.aspx") OrElse _
                '    LCase(Request.ServerVariables("URL")).Contains("/order/orderpreview.aspx") OrElse _
                '    LCase(Request.ServerVariables("URL")).Contains("/order/pi.aspx") Then
                '    _IsOrderUID = True
                'End If

                ' Dim QuoteDt As IBUS.iDocHeaderLine = Nothing
                'Dim QuoteDt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
                'If _IsOrderUID Then
                '    Dim OrderDT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.ORDER)
                '    QuoteDt = Pivot.NewObjDocHeader.GetByDocID(OrderDT.OriginalQuoteID, COMM.Fixer.eDocType.EQ)
                '    OrderDT = Nothing
                'Else
                ' QuoteDt = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
                'End If
                'lbQuoteid.Text = QuoteDt.quoteNo
                Dim _QuoteMaster As Quote_Master = MyQuoteX.GetQuoteMaster(Request("UID"))
                If _QuoteMaster IsNot Nothing Then
                    Me.HyperLinkQuoteNo.Text = _QuoteMaster.quoteNo
                    If _isATWSales Then
                        Me.HyperLinkQuoteNo.Text = Me.HyperLinkQuoteNo.Text & " V" & _QuoteMaster.Revision_Number.ToString
                        Me.tdQuoteDate.Visible = True : Me.lbQuoteDate.Text = Format(_QuoteMaster.quoteDate, "yyyy/MM/dd")
                    End If
                    'Dim ActiveQuoteID As String = Pivot.NewObjDocHeader.GetActiveRevisionQuoteIDByQuoteNo(_QuoteMaster.quoteNo, COMM.Fixer.eDocType.EQ)
                    Me.HyperLinkQuoteNo.NavigateUrl = "~\Quote\QuotationMaster.aspx?UID=" & _QuoteMaster.quoteId
                End If
            End If

        End If

    End Sub

    Protected Sub imgLogo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Response.Redirect("~/home.aspx")
    End Sub

    Protected Sub IdMyAdvantech_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim TID As String = Pivot.CurrentProfile.SSOID, USER As String = Pivot.CurrentProfile.UserId
        Response.Redirect(String.Format("http://my.advantech.com/ORDER/SSOENTER.ASPX?ID={0}&USER={1}", TID, USER))
    End Sub

 
    'Private Sub drpReg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpReg.SelectedIndexChanged
    '    Pivot.CurrentProfile.CurrDocReg = CType(sender, DropDownList).SelectedValue
    'End Sub
End Class