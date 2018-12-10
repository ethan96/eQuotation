Imports EDOC.CBOMWS.CBOMDS

Public Class BtosSearch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("UID") Is Nothing Then
            Response.Redirect("./BtosSearch.aspx?" + "UID=" + Business.GetNewUID(), True)
        End If
        Dim FM As HtmlForm = CType(Me.Master.FindControl("form1"), HtmlForm)
        FM.DefaultButton = BTsearch.UniqueID
        If Not IsPostBack Then
            Dim BtosDT As CBOM_CATALOGDataTable = Business.getCatalogByUID(Request("UID"), COMM.Fixer.eDocType.EQ)

            If Role.IsUsaUser() Then
                Dim dr As CBOM_CATALOGRow = BtosDT.NewCBOM_CATALOGRow()
                dr.CATALOG_TYPE = "eStore BTOS"
                BtosDT.Rows.Add(dr)
                dr = BtosDT.NewRow()
                dr.CATALOG_TYPE = "Custom BTOS"
                BtosDT.Rows.Add(dr)
                BtosDT.AcceptChanges()
            End If
            Dim dv As New DataView(BtosDT)
            dv.Sort = "CATALOG_TYPE"
            ddlcatname.DataTextField = "CATALOG_TYPE" : ddlcatname.DataValueField = "CATALOG_TYPE"
            ddlcatname.DataSource = dv.ToTable()
            ddlcatname.DataBind()
            ddlcatname.Items.Insert(0, New ListItem("Select…", ""))

        End If
        If ddlcatname.SelectedValue.Equals("Custom BTOS", StringComparison.OrdinalIgnoreCase) Then
            initSBC()
            AdxGrid1.Visible = False
        Else
            AdxGrid1.Visible = True
        End If
    End Sub
    Protected Sub btnConfig_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim CATEGORY_ID As String = AdxGrid1.DataKeys(row.RowIndex).Values(0)
        Dim intQty As Integer = 1
        intQty = CType(row.FindControl("txtQty"), TextBox).Text
        Dim str As String = "~/quote/QuotationDetail.aspx?VIEW=1&BTOITEM=" & CATEGORY_ID & "&QTY=" & intQty & "&UID=" & Request("UID")
        Response.Redirect(str)
    End Sub

    Protected Sub BTsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim org As String = "US"

        Dim quoteDt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
        If Not IsNothing(quoteDt) Then
            org = quoteDt.org.ToString.Substring(0, 2)
        End If
        Dim sb As New StringBuilder
        sb.AppendLine(" select distinct top 200 CATALOG_ID, CATALOG_TYPE  from dbo.CBOM_CATALOG where  CATALOG_ID <> ''")
        sb.AppendFormat(" and CATALOG_ORG='{0}' ", org)
        sb.AppendFormat(" and (CATALOG_TYPE  like '%{0}%' or CATALOG_ID like '%{0}%') ", TBkey.Text.Trim.Replace("'", "''").Replace("*", "%"))
        If ddlcatname.SelectedIndex > 0 Then
            sb.AppendFormat(" and CATALOG_TYPE ='{0}'", ddlcatname.SelectedValue)
        End If
        sb.AppendLine(" order by CATALOG_ID")
        If ddlcatname.SelectedValue.Equals("eStore BTOS", StringComparison.OrdinalIgnoreCase) Then
            sb.Clear()
            sb.AppendLine(" select distinct DisplayPartno as CATALOG_ID,CategoryName as CATALOG_TYPE ")
            sb.AppendFormat(" FROM ESTORE_BTOS_CATEGORY  where storeid ='AUS' and (CategoryName like '%{0}%' or DisplayPartno like '%{0}%') order by DisplayPartno", TBkey.Text.Trim.Replace("'", "''").Replace("*", "%"))
        End If

        AdxGrid1.DataSource = tbOPBase.dbGetDataTable("B2B", sb.ToString)
        AdxGrid1.DataBind()
    End Sub
    Sub initSBC()
        Dim STR() As String = {"ACP-BTO", "TPC-BTO", "IPPC-BTO", "IPC-BTO", "UNO-BTO", "ARK-BTO", "TREK-BTO", "PPC-BTO", "SBC-BTO"}
        For Each X As String In STR
            Dim hlSBC As New LinkButton
            hlSBC.Text = X
            hlSBC.Font.Bold = True
            AddHandler hlSBC.Click, AddressOf SBCCLICK
            plSBC.Controls.Add(hlSBC)
            plSBC.Controls.Add(New LiteralControl("<BR/><BR/>"))
        Next
    End Sub
    Sub SBCCLICK(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim obj As LinkButton = CType(sender, LinkButton)
        Dim Apt As New EQDSTableAdapters.QuotationDetailTableAdapter
        Apt.DeleteQuoteDetailById(Request("UID"))
        tbOPBase.adddblog("delete from quotationDetail where quoteid='" & Request("UID") & "'")
        'Business.ADD2QUOTE_V2(Request("UID"), obj.Text.ToUpper, 1, 0, COMM.Fixer.eItemType.Parent, "", 0, 0, Now, 0, 0, "")
        Business.ADD2QUOTE_V2_1(Request("UID"), obj.Text.ToUpper, 1, 0, COMM.Fixer.eItemType.Parent, "", 0, 0, Now, 0, "", 0, "", Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
        Response.Redirect("~/quote/QuotationDetail.aspx?VIEW=0&UID=" & Request("UID"))
    End Sub

End Class