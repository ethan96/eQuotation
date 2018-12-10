Public Class USVolumeDiscount
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

    Public Sub ShowData(ByVal _PartNo As String)
        Dim SAPClient1 As New Z_SD_USPRICELOOKUP.Z_SD_USPRICELOOKUP
        SAPClient1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        SAPClient1.Connection.Open()
        Dim p_error As String = "", p_maktx As String = "", p_head As New Z_SD_USPRICELOOKUP.ZSD_PRICE_HEAD, p_509 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_509Table
        Dim p_513 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_513Table, p_514 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_514Table, p_517 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_517Table
        Dim p_521 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_521Table, it_markup As New Z_SD_USPRICELOOKUP.ZSD_PRICE_MARKUPTable

        Dim _SAPDAL As New SAPDAL.SAPDAL
        Dim group As String = String.Empty
        If Role.IsUSAACSales() AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2100") Then
            group = "10"
        ElseIf Role.IsUsaAenc() Then
            group = "20"
        End If

        SAPClient1.Z_Sd_Uspricelookup("USH1", Now.ToString("yyyyMMdd"), group, SAPDAL.Global_Inc.Format2SAPItem2(_PartNo), "US01", "00", p_error, p_maktx, p_head, p_509, p_513, p_514, p_517, p_521, it_markup)
        SAPClient1.Connection.Close()

        'Header Settings
        Dim dtHeader As DataTable = New DataTable
        dtHeader.Columns.Add("PartNo")
        dtHeader.Columns.Add("ORG")
        dtHeader.Columns.Add("CustomerGroup")
        dtHeader.Columns.Add("Currency")
        dtHeader.Columns.Add("ListPrice")

        If p_head IsNot Nothing Then
            Dim r = dtHeader.NewRow()
            r.Item("PartNo") = _PartNo
            r.Item("ORG") = p_head.Vkorg
            r.Item("CustomerGroup") = p_head.Kdgrp
            r.Item("Currency") = p_head.Waerk
            r.Item("ListPrice") = p_head.Kbetr
            dtHeader.Rows.Add(r)
        End If

        Me.gv0.DataSource = dtHeader
        Me.gv0.DataBind()

        'Detail Settings
        Dim dtResult As DataTable = New DataTable
        dtResult.Columns.Add("PartNo")
        dtResult.Columns.Add("PriceGroup")
        dtResult.Columns.Add("Scale")
        dtResult.Columns.Add("Price")
        dtResult.Columns.Add("Valid_From")
        dtResult.Columns.Add("Valid_To")

        Dim Currency As String = IIf(p_head IsNot Nothing AndAlso Not String.IsNullOrEmpty(p_head.Waerk), p_head.Waerk, "USD")
        Dim CurrencySign As String = Util.getCurrSign(Currency)

        Dim dtSAPinfo As DataTable = New DataTable
        If group = "10" Then
            dtSAPinfo = p_521.ToADODataTable()
            For Each d As DataRow In dtSAPinfo.Rows
                Dim r = dtResult.NewRow()
                r.Item("PartNo") = _PartNo
                r.Item("PriceGroup") = d.Item("Konda521")
                r.Item("Scale") = Decimal.Round(Decimal.Parse(d.Item("Kstbm")), 0)
                r.Item("Price") = CurrencySign + d.Item("Sldpr").ToString
                r.Item("Valid_From") = d.Item("Datab521")
                r.Item("Valid_To") = d.Item("Datbi521")
                dtResult.Rows.Add(r)
            Next
        ElseIf group = "20" Then
            dtSAPinfo = p_517.ToADODataTable()
            For Each d As DataRow In dtSAPinfo.Rows
                Dim r = dtResult.NewRow()
                r.Item("PartNo") = _PartNo
                r.Item("PriceGroup") = d.Item("Konda517")
                r.Item("Scale") = Decimal.Round(Decimal.Parse(d.Item("Kstbm")), 0)
                r.Item("Price") = CurrencySign + d.Item("Sclpr").ToString
                r.Item("Valid_From") = d.Item("Datab517")
                r.Item("Valid_To") = d.Item("Datbi517")
                dtResult.Rows.Add(r)
            Next
        End If

        Me.gv1.DataSource = dtResult
        Me.gv1.DataBind()
    End Sub

End Class