Public Class QuotationPreview
    Inherits System.Web.UI.Page

    'TC:QuotePreview這頁，所有user 都default看到external version
    'Frank 2012/07/26:Please do not remove the _IsANAUser, I think some day we will need it.
    'Dim _IsANAUser As Boolean = True

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Me._IsANAUser = GetLoginUserIsANAUser()
        If Not IsPostBack Then
            getPageStr(Request("UID"), Me.RadioButtonList_PriviewMode.SelectedValue)
            'Me.view_type_option.Visible = Me._IsANAUser
        End If
    End Sub

    Public Function GetLoginUserIsANAUser() As Boolean


        Return Role.IsUsaUser()
       
    End Function

    Sub getPageStr(ByVal UID As String, Optional ByVal IsInternalUser As Boolean = True)
        Dim M As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        Me.divContent.InnerHtml = Business.getPageStrInternal(UID, M.DocReg, IsInternalUser)
    End Sub

    Protected Sub RadioButtonList_PriviewMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        getPageStr(Request("UID"), Me.RadioButtonList_PriviewMode.SelectedValue)
        Me.UP_QuotationPreview.Update()
    End Sub
End Class