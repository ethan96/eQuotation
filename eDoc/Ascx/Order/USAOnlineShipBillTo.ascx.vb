Public Class USAOnlineShipBillTo
    Inherits System.Web.UI.UserControl
    Private _QM As IBUS.iDocHeaderLine = Nothing
    Public Property QM As IBUS.iDocHeaderLine
        Get
            Return _QM
        End Get
        Set(ByVal value As IBUS.iDocHeaderLine)
            _QM = value
        End Set
    End Property
    Public Shared Function SearchAllSAPCompanySoldBillShipTo( _
          ByVal ERPID As String, ByVal Org_id As String, ByVal CompanyName As String, ByVal Address As String, ByVal State As String, _
          ByVal Division As String, ByVal SalesGroup As String, ByVal SalesOffice As String, Optional ByVal WhereStr As String = "", Optional ByVal Type As String = "") As DataTable
        Dim dt As New DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            ' .AppendLine(" SELECT A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME,  D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| D.country AS Address, ") 'B.STRAS AS ADDRESS,
            .AppendLine(" SELECT A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME, " + _
                        " D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1) AS Address, ") 'B.STRAS AS ADDRESS,
            .AppendLine(" B.Land1 AS  COUNTRY,B.Ort01 AS CITY,B.STRAS as STREET,")
            .AppendLine(" B.PSTLZ AS ZIP_CODE, D.region AS STATE,  C.smtp_addr AS CONTACT_EMAIL,B.TELF1 AS TEL_NO,B.TELFX AS FAX_NO, D.NAME_CO as Attention, ")
            .AppendLine(" case A.PARVW when 'WE' then 'Ship-To' when 'AG' then 'Sold-To' when 'RE' then 'Bill-To' end as PARTNER_FUNCTION, ")
            .AppendLine(" E.VKBUR as SalesOffice, E.VKGRP as SalesGroup, E.SPART as division,D.STR_SUPPL3  ")
            .AppendLine(" FROM saprdp.knvp A  ")
            .AppendLine(" INNER JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR left join saprdp.adr6 C on B.adrnr=C.addrnumber ")
            .AppendLine(" inner join saprdp.adrc D on  D.country=B.land1 and D.addrnumber=B.adrnr inner join saprdp.knvv E on B.KUNNR=E.KUNNR  ")
            .AppendLine(" where rownum<=60 ")
            If Not String.IsNullOrEmpty(State) Then .AppendFormat(" and Upper(D.region) LIKE '%{0}%' ", UCase(State.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(Address) Then .AppendFormat(" and Upper(B.STRAS) LIKE '%{0}%' ", UCase(Address.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(CompanyName) Then .AppendFormat(" and (Upper(B.NAME1) LIKE '%{0}%' or B.NAME2 like '%{0}%') ", UCase(CompanyName.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(ERPID) Then .AppendFormat(" and (A.Kunnr LIKE '%{0}%' or A.KUNN2 like '%{0}%') ", UCase(ERPID.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(Org_id) Then .AppendFormat(" and A.VKORG = '{0}' ", UCase(Org_id.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(Division) Then
                .AppendFormat(" and E.SPART = '{0}' ", UCase(Division.Replace("'", "''").Trim))
            End If
            If Not String.IsNullOrEmpty(SalesGroup) Then
                .AppendFormat(" and E.VKGRP = '{0}' ", UCase(SalesGroup.Replace("'", "''").Trim))
            End If
            If Not String.IsNullOrEmpty(SalesOffice) Then
                .AppendFormat(" and E.VKBUR = '{0}' ", UCase(SalesOffice.Replace("'", "''").Trim))
            End If
            If Not String.IsNullOrEmpty(Type) Then
                Select Case Type
                    Case "S"
                        .Append(" AND A.PARVW = 'WE' ")
                    Case "B"
                        .Append(" AND A.PARVW ='RE' ")
                    Case "EM"
                        .Append(" AND A.PARVW ='EM' ")
                End Select
            End If
            If Not String.IsNullOrEmpty(WhereStr) Then
                .AppendFormat(" AND B.ktokd in ({0})", WhereStr)
            End If
            .AppendFormat(" AND A.PARVW in ('WE','AG','RE','EM') ORDER BY A.Kunn2 ", Org_id)
        End With

        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        dt.TableName = "SAPPF"
        Return dt
    End Function


    Sub GetData(Optional ByVal WhereStr As String = "", Optional ByVal Type As String = "")

        Dim retDt As DataTable = SearchAllSAPCompanySoldBillShipTo(txtShipID.Text, QM.org, txtShipName.Text, "", "", "", "", "", WhereStr, Type)
        gvBillShipTo.DataSource = retDt : gvBillShipTo.DataBind()
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvBillShipTo.PageIndex = e.NewPageIndex
        GetData()
    End Sub

    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(0)
        Dim p As Control = Me.Parent
        'Dim dt As DataTable = myCompany.GetDT(String.Format("company_Id='{0}'", id), "")
        'If dt.Rows.Count > 0 Then
        CType(p.FindControl("txtShipTo"), TextBox).Text = id
        CType(p.FindControl("txtShipToName"), TextBox).Text = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(9)
        'txtShipToAddr
        CType(p.FindControl("txtShipToStreet"), TextBox).Text = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(1)
        CType(p.FindControl("txtShipToStreet2"), TextBox).Text = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(8)
        CType(p.FindControl("txtShipToCity"), TextBox).Text = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(2)
        CType(p.FindControl("txtShipToState"), TextBox).Text = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(3)
        CType(p.FindControl("txtShipToZipcode"), TextBox).Text = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(4)
        CType(p.FindControl("txtShipToCountry"), TextBox).Text = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(5)
        CType(p.FindControl("txtShipToAttention"), TextBox).Text = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(6)
        CType(p.FindControl("txtShipToTel"), TextBox).Text = Me.gvBillShipTo.DataKeys(row.RowIndex).Values(7)
        'End If
        CType(p.FindControl("upShipTo"), UpdatePanel).Update()
        CType(p.FindControl("MP_shipto"), AjaxControlToolkit.ModalPopupExtender).Hide()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetData()
    End Sub

    Protected Sub lnkCloseBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim p As Control = Me.Parent
        CType(p.FindControl("MP_shipto"), AjaxControlToolkit.ModalPopupExtender).Hide()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not IsNothing(QM) Then
                txtShipID.Text = QM.AccErpId
            End If
        End If
    End Sub

End Class