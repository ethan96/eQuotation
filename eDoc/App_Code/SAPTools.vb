Imports Microsoft.VisualBasic

Public Class SAPTools

    Public Shared Function GetIncotermName(ByVal IncotermID As String) As String
        Dim SQL As String = String.Format(" select tinct.BEZEI from  saprdp.tinct where inco1='{0}' AND SPRAS='E' AND ROWNUM =1", IncotermID)
        Dim Incoterm As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", SQL)
        If Incoterm IsNot Nothing Then
            Return Incoterm
        End If
        Return ""
    End Function

    Public Shared Function GetSAPPartnerTableByKunnr(ByVal Kunnr As String) As Relics.SalesOrder.SAP_BAPIPARNRDataTable
        Dim retTable As New Relics.SalesOrder.SAP_BAPIPARNRDataTable
        Dim strSql As String = _
            " select a.kunnr as PARTN_NUMB,a.anred as TITLE, a.NAME1 as NAME, a.NAME2 as NAME_2, b.NAME3 as NAME_3, b.NAME4 as NAME_4, " + _
            " a.STRAS as STREET, a.LAND1 as COUNTRY, a.LAND1 as COUNTRY_ISO, a.PSTLZ as POSTL_CODE, '' as POBX_PCD, ''as POBX_CTY,  " + _
            " b.CITY1 as CITY, '' as DISTRICT, a.REGIO as REGION, b.PO_BOX as PO_BOX, a.TELF1 as TELEPHONE, a.TELF2 as TELEPHONE2, a.TELBX as TELEBOX, " + _
            " a.TELFX as FAX_NUMBER, a.TELTX as TELEX_NO, a.SPRAS as LANGU, '' as LANGU_ISO, '' as UNLOAD_PT, b.TRANSPZONE, b.TAXJURCODE, " + _
            " '' as ADDRESS, '' as PRIV_ADDR, 1 as ADDR_TYPE, '' as ADDR_ORIG, '' as ADDR_LINK, '' as REFOBJTYPE, '' as REFOBJKEY, '' as REFLOGSYS " + _
            " from saprdp.kna1 a inner join saprdp.adrc b on a.adrnr=b.addrnumber and a.land1=b.country " + _
            " where a.mandt='168' and a.kunnr='" + UCase(Trim(Kunnr)) + "' "
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
        For Each r As DataRow In dt.Rows
            Dim PtnrRow As Relics.SalesOrder.SAP_BAPIPARNRRow = retTable.NewSAP_BAPIPARNRRow()
            PtnrRow.ADDR_LINK = "" : PtnrRow.ADDR_ORIG = "" : PtnrRow.ADDR_TYPE = r.Item("ADDR_TYPE") : PtnrRow.ADDRESS = ""
            PtnrRow.CITY = r.Item("CITY") : PtnrRow.COUNTRY_ISO = r.Item("COUNTRY_ISO") : PtnrRow.COUNTRY = r.Item("COUNTRY")
            PtnrRow.DISTRICT = "" : PtnrRow.FAX_NUMBER = r.Item("FAX_NUMBER") : PtnrRow.ITM_NUMBER = "" : PtnrRow.LANGU = r.Item("LANGU")
            PtnrRow.LANGU_ISO = "" : PtnrRow.NAME = r.Item("NAME") : PtnrRow.NAME_2 = r.Item("NAME_2") : PtnrRow.NAME_3 = r.Item("NAME_3")
            PtnrRow.NAME_4 = r.Item("NAME_4") : PtnrRow.PARTN_NUMB = Kunnr : PtnrRow.PARTN_ROLE = "" : PtnrRow.PO_BOX = r.Item("PO_BOX")
            PtnrRow.POBX_CTY = "" : PtnrRow.POBX_PCD = "" : PtnrRow.POSTL_CODE = r.Item("POSTL_CODE") : PtnrRow.PRIV_ADDR = ""
            PtnrRow.REFLOGSYS = "" : PtnrRow.REFOBJKEY = "" : PtnrRow.REFOBJTYPE = "" : PtnrRow.STREET = r.Item("STREET")
            PtnrRow.TAXJURCODE = r.Item("TAXJURCODE") : PtnrRow.TELEBOX = r.Item("TELEBOX") : PtnrRow.TELEPHONE = r.Item("TELEPHONE")
            PtnrRow.TELEPHONE2 = r.Item("TELEPHONE2") : PtnrRow.TELETEX_NO = "" : PtnrRow.TELEX_NO = r.Item("TELEX_NO")
            PtnrRow.TITLE = r.Item("TITLE") : PtnrRow.TRANSPZONE = r.Item("TRANSPZONE") : PtnrRow.UNLOAD_PT = "" : PtnrRow._REGION = r.Item("REGION")
            retTable.AddSAP_BAPIPARNRRow(PtnrRow)
        Next
        Return retTable
    End Function


    Public Shared Function IsCustomerCreditBlock( _
            ByVal Org As String, ByVal SoldToId As String, ByVal strDistChann As String, ByVal strDivision As String, ByRef ReturnTable As DataTable) As Boolean
        SoldToId = Trim(UCase(SoldToId))
        Dim ShipToId As String = SoldToId
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        With OrderHeader
            .Doc_Type = "ZOR" : .Sales_Org = Trim(UCase(Org)) : .Distr_Chan = strDistChann : .Division = strDivision
        End With
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = SoldToId : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = ShipToId
        Partners.Add(SoldTo) : Partners.Add(ShipTo)

        'Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        'item.Itm_Number = "1" : item.Material = "ADAM-4520-D2E" : item.Req_Qty = 1
        'ItemsIn.Add(item)
        proxy1.Connection.Open()
        proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "", _
                                            New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, _
                                            New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, _
                                            ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)
        proxy1.Connection.Close()
        ReturnTable = retDt.ToADODataTable()
        For Each retMsgRec As DataRow In ReturnTable.Rows
            If retMsgRec.Item("Type") = "E" Then
                Return False
            End If
        Next
        Return True
    End Function

    Shared Function getInventoryAndATPTable(ByVal PartNo As String, ByVal Plant As String, ByVal reqQty As Integer, Optional ByRef DueDate As String = "", _
                                            Optional ByRef Inventory As Integer = 0, _
                                            Optional ByRef ATPtable As DataTable = Nothing, _
                                            Optional ByVal reqDate As String = "", _
                                            Optional ByRef satisFlag As Integer = 1, _
                                            Optional ByRef qtyCanBeConfirm As Int32 = 0) As Integer

        Dim retDate As Date = DateAdd(DateInterval.Day, -1, Now), retQty As Integer = 0
        PartNo = Business.Format2SAPItem(Trim(UCase(PartNo)))
        'Dim dtO As New Relics.SAPDALDS.QueryInventory_OutputDataTable, dtI As New Relics.SAPDALDS.ProductInDataTable
        'Dim rowI As Relics.SAPDALDS.ProductInRow = dtI.NewRow

        Dim dtO As New SAPDAL.SAPDALDS.QueryInventory_OutputDataTable, dtI As New SAPDAL.SAPDALDS.ProductInDataTable
        Dim rowI As SAPDAL.SAPDALDS.ProductInRow = dtI.NewRow


        rowI.PART_NO = PartNo : rowI.QTY = reqQty : dtI.Rows.Add(rowI)
        Dim WS As New SAPDAL.SAPDAL
        WS.QueryInventory(dtI, UCase(Plant), dtO, "")

        ATPtable = dtO
        ATPtable.Columns("stock").ColumnName = "com_qty" : ATPtable.Columns("stock_date").ColumnName = "com_date"

        '.showDT(ATPtable) : HttpContext.Current.Response.End()
        If PartNo.ToUpper.StartsWith("AGS-") Then
            DueDate = Now.Date.ToString("yyyy-MM-dd")
        Else
            If ATPtable.Rows.Count > 0 Then
                For Each r As DataRow In ATPtable.Rows
                    qtyCanBeConfirm += CType(r.Item("com_qty"), Int64)
                Next
                Inventory = qtyCanBeConfirm
                If qtyCanBeConfirm > 0 Then
                    DueDate = Util.DateFormat(CDate(ATPtable.Rows(ATPtable.Rows.Count - 1).Item("Com_Date")).ToString("yyyyMMdd"), "YYYYMMDD", "YYYYMMDD", "", "-")
                Else
                    DueDate = "1900-01-01"
                End If
            Else
                DueDate = "1900-01-01"
            End If
        End If
        If DueDate = "1900-01-01" Then
            DueDate = Now.Date.AddDays(getLeadTime(PartNo, Plant))
        End If

        If reqQty > qtyCanBeConfirm Then
            satisFlag = 0
        Else
            satisFlag = 1
        End If
        Return 1
    End Function
    Public Shared Function getLeadTime(ByVal pn As String, ByVal plant As String) As Integer
        Dim N As Integer = 0
        Dim str As String = String.Format("select (PLANNED_DEL_TIME + GP_PROCESSING_TIME) from dbo.SAP_PRODUCT_ABC where PART_NO='{0}' AND PLANT='{1}'", pn, plant)
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("b2b", str)
        If dt.Rows.Count > 0 Then
            N = dt.Rows(0).Item(0)
        End If
        Return N
    End Function




    '<Obsolete("Deleted by Ming on 20150602", True)>
    'Shared Function getSAPPriceByTable(ByVal partNoStr As String, ByVal org As String, ByVal company As String, ByVal ShipTo As String, ByVal Currency As String, ByRef retTable As DataTable, Optional ByRef errMsg As String = "") As Integer
    '    'Dim ws As New Relics.SAPDAL
    '    'Return ws.getSAPPriceByTable(partNoStr, org, company, Currency, retTable, errMsg)

    '    'Frank 2013/12/06
    '    Dim _SAPDAL As New SAPDAL.SAPDAL
    '    Return _SAPDAL.getSAPPriceByTable(partNoStr, org, company, ShipTo, Currency, retTable, errMsg)

    'End Function

    Shared Function getGradePriceByTable(ByVal partNoStr As String, ByVal RBU As String, ByVal company As String, ByVal pGrade As String, ByVal CURR As String, ByRef retTable As DataTable) As Integer
        If pGrade.Length <> 8 Then Return Nothing
        Dim strKDGRP As String = "01", org As String = Pivot.CurrentProfile.getCurrOrg

        Select Case RBU.ToUpper()
            Case "ATW"
                strKDGRP = "03"
            Case "HQDC"
                strKDGRP = "D1"
            Case "ACN ", "ACN-E ", "ASH ", "AHZ ", "AWH ", "ACN-N ", "ABJ ", "ASY ", "AXA ", "ACN-S ", "AFZ ", "AGZ ", "AHK ", "ASZ ", "ACN-WS ", "ACD ", "ACQ "
                strKDGRP = "05"
            Case "ADL", "AFR", "AEE", "ABN", "AUK", "APL", "AIT"
                strKDGRP = "02"
            Case "AAC"
                strKDGRP = "10"
            Case "AENC"
                strKDGRP = "20"
            Case "ACL"
                strKDGRP = "01"
            Case "ABR"
                strKDGRP = "B1"
            Case "AKR"
                strKDGRP = "K1"
            Case "AJP"
                strKDGRP = "06"
            Case "SAP"
                strKDGRP = "07"
            Case "AAU"
                strKDGRP = "08"
            Case Else
                strKDGRP = "01"
        End Select
        'ACN','ACN-E','ASH','AHZ','AWH','ACN-N','ABJ','ASY','AXA','ACN-S','AFZ','AGZ','AHK','ASZ','ACN-WS','ACD','ACQ'
        Dim pg As New PRICE_GRADE.PRICE_GRADE
        Dim qin As New PRICE_GRADE.ZSSD_01_PGTable
        Dim qout As New PRICE_GRADE.ZSSD_02Table

        'C3V5P6L0

        pGrade = pGrade.Trim().ToUpper() : org = org.Trim().ToUpper()
        Dim part_noArr() As String = partNoStr.Trim().Trim("|").Split("|")
        For Each p As String In part_noArr

            Dim qinRow1 As New PRICE_GRADE.ZSSD_01_PG
            With qinRow1
                .Matnr = Business.Format2SAPItem(p.Trim) : .Mglme = 1
                .Kdkg1 = pGrade.Substring(0, 2) : .Kdkg2 = pGrade.Substring(2, 2)
                .Kdkg3 = pGrade.Substring(4, 2) : .Kdkg4 = pGrade.Substring(6, 2)
                .Mandt = "168" : .Vkorg = org : .Waerk = CURR.ToString().ToUpper  ' .Kunnr = "EDDEVI07"
                .Kdgrp = strKDGRP
            End With
            qin.Add(qinRow1)
        Next

        pg.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        pg.Connection.Open()
        'Try
        pg.Z_Sd_Priceinquery_Pg("1", qin, qout)
        'Catch ex As Exception
        '    pg.Connection.Close() : Return Nothing
        'End Try
        pg.Connection.Close()

        retTable = qout.ToADODataTable()
        Return 1
    End Function


    Shared Function getEpricerPrice(ByVal partNoStr As String, ByVal PRI_LST As String, ByVal pGrade As String, ByVal RBU As String, ByVal YEAR As String, ByVal QUARTER As String, ByVal CURR As String, ByVal org As String, ByRef retDT As DataTable) As Integer
        Business.GetCurrentPriceYearQuarter(YEAR, QUARTER, Left(org, 2))

        If retDT IsNot Nothing Then
            retDT.Columns.Clear()
        Else
            retDT = New DataTable
        End If
        retDT.Columns.Add("PART_NO") : retDT.Columns.Add("LIST_PRICE") : retDT.Columns.Add("UNIT_PRICE")

        If pGrade = "" Then
            pGrade = "L0L0L0L0"
        End If
        Dim part_noArr() As String = partNoStr.Trim().Trim("|").Split("|")
        For Each p As String In part_noArr
            'Dim DT As DataTable = dbUtil.dbGetDataTable("my", String.Format("select top 1 convert(decimal(10,2),AMT1) AS AMT1,convert(decimal(10,2),LIST_PRICE) AS LIST_PRICE from epricer_price where GRADE_NAME = '{0}' AND PROD_NAME='{1}' and org='{2}' and curcy_cd='{3}' AND AMT1 IS NOT NULL AND LIST_PRICE IS NOT NULL ORDER BY YEAR DESC,QUARTER DESC", GRADE, PN, org, CURR))

            Dim R As DataRow = retDT.NewRow()
            R.Item("PART_NO") = p : R.Item("LIST_PRICE") = 0 : R.Item("UNIT_PRICE") = 0
            If PRI_LST <> "" And YEAR <> "" And QUARTER <> "" Then
                Dim pdt As DataTable = tbOPBase.dbGetDataTable("MY", _
                " select top 1 LIST_PRICE, AMT1 from EPRICER_PRICE " + _
                String.Format(" where LIST_PRICE is not null and AMT1 is not null and PROD_NAME='{0}' and PRI_LST='{1}' and GRADE_NAME='{2}' ", _
                          p, PRI_LST, pGrade) + _
                String.Format(" and ORG='{0}' and YEAR={1} and QUARTER={2} and CURCY_CD='{3}'", IIf(RBU = "AEU" Or RBU = "ADL" Or RBU = "AIT" Or RBU = "AFR" Or RBU = "ABN" Or RBU = "AEE" Or RBU = "AUK", "AESC", RBU), YEAR, QUARTER, CURR))
                If pdt.Rows.Count > 0 Then
                    R.Item("LIST_PRICE") = pdt.Rows(0).Item("LIST_PRICE") : R.Item("UNIT_PRICE") = pdt.Rows(0).Item("AMT1")
                End If
            End If
            retDT.Rows.Add(R)
        Next

        Dim PartNoStrWithPriceZero As String = ""
        For Each R As DataRow In retDT.Rows()
            If R.Item("UNIT_PRICE") = 0 Then
                PartNoStrWithPriceZero &= R.Item("part_no") & "|"
            End If
        Next

        Dim RETTABLE As New DataTable
        getGradePriceByTable(PartNoStrWithPriceZero, RBU, "", pGrade, CURR, RETTABLE)
        If RETTABLE.Rows.Count > 0 Then
            For Each R As DataRow In RETTABLE.Rows()
                If R.Item("Netwr") > 0 Then
                    For Each rr As DataRow In retDT.Select(String.Format("PART_NO='{0}'", R.Item("MATNR").ToString.TrimStart("0")))
                        rr.Item("LIST_PRICE") = R.Item("Kzwi1") : rr.Item("UNIT_PRICE") = R.Item("Netwr")
                        retDT.AcceptChanges()
                    Next
                End If
            Next
        End If
        retDT.Columns("UNIT_PRICE").ColumnName = "Netwr"
        retDT.Columns("LIST_PRICE").ColumnName = "Kzwi1"
        retDT.AcceptChanges()
        Return 1
    End Function

    Public Shared Function GetSalesGrpOfficeDivisionDistrictByKunnr(ByVal Kunnr As String) As DataTable
        Kunnr = Trim(UCase(Kunnr))
        Dim strSql As String = _
            " select E.VKBUR as SalesOffice, E.VKGRP as SalesGroup, E.SPART as division, E.BZIRK as District " + _
            " from saprdp.knvv E " + _
            " where rownum<=30 and E.mandt='168' and E.kunnr='" + Kunnr + "'"
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
        Return dt
    End Function

    Public Shared Function SearchAllSAPCompanySoldBillShipTo( _
        ByVal ERPID As String, ByVal Org_id As String, ByVal CompanyName As String, ByVal Address As String, ByVal State As String, _
        ByVal Division As String, ByVal SalesGroup As String, ByVal SalesOffice As String, ByVal ComType As String, Optional ByVal isAll As String = "") As DataTable
        Dim dt As New DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            If ComType.Equals("EM", StringComparison.OrdinalIgnoreCase) Then
                'For AJP end customer searching, needs to select all JP01 customer ID, else only allowed to select EMs which are under Sold-to
                If Org_id.Equals("JP01", StringComparison.OrdinalIgnoreCase) Then
                    sb.Append(Advantech.Myadvantech.Business.OrderBusinessLogic.GetAJPAddressString(ERPID, CompanyName))
                Else
                    sb.Append(Advantech.Myadvantech.Business.OrderBusinessLogic.GetSAPPartnerAddressString("", ERPID, CompanyName, "EM"))
                End If
            Else
                ' .AppendLine(" SELECT A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME,  D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| D.country AS Address, ") 'B.STRAS AS ADDRESS,
                .AppendLine(" SELECT distinct A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME, " +
                            " D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1) AS Address, ") 'B.STRAS AS ADDRESS,
                .AppendLine(" B.Land1 AS  COUNTRY,B.Ort01 AS CITY,")
                .AppendLine(" B.PSTLZ AS ZIP_CODE, D.region AS STATE, B.TELF1 AS TEL_NO,B.TELFX AS FAX_NO, ")
                .AppendLine(" case A.PARVW when 'WE' then 'Ship-To' when 'AG' then 'Sold-To' when 'RE' then 'Bill-To' end as PARTNER_FUNCTION, ")
                .AppendLine(" E.VKBUR as SalesOffice, E.VKGRP as SalesGroup, E.SPART as division, E.BZIRK as District,A.PARVW   ")
                .AppendLine(" FROM saprdp.knvp A  ")
                .AppendLine(" INNER JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR ")
                .AppendLine(" inner join saprdp.adrc D on  D.country=B.land1 and D.addrnumber=B.adrnr inner join saprdp.knvv E on B.KUNNR=E.KUNNR  ")
                .AppendLine(" where rownum<=300 ")
                If Not String.IsNullOrEmpty(State) Then .AppendFormat(" and Upper(D.region) LIKE '%{0}%' ", UCase(State.Replace("'", "''").Trim))
                'If Not String.IsNullOrEmpty(Address) Then .AppendFormat(" and Upper(B.STRAS) LIKE '%{0}%' ", UCase(Address.Replace("'", "''").Trim))
                If Not String.IsNullOrEmpty(Address) Then .AppendFormat(" and B.STRAS ||' '|| B.ORT01 ||' '|| B.REGIO ||' '|| B.PSTLZ ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1) LIKE '%{0}%' ", Address.Replace("'", "''").Trim)
                If Not String.IsNullOrEmpty(CompanyName) Then .AppendFormat(" and (Upper(B.NAME1) LIKE '%{0}%' or B.NAME2 like '%{0}%') ", UCase(CompanyName.Replace("'", "''").Trim))
                If Not String.IsNullOrEmpty(ERPID) Then
                    '.AppendFormat(" and (A.Kunnr LIKE '%{0}%' or A.KUNN2 like '%{0}%') ", UCase(ERPID.Replace("'", "''").Trim))
                    If isAll = "Y" Then
                        .AppendFormat(" and (A.Kunnr LIKE '%{0}%' ) ", UCase(ERPID.Replace("'", "''").Trim))
                    Else
                        .AppendFormat(" and (A.Kunnr = '{0}' ) ", UCase(ERPID.Replace("'", "''").Trim))
                    End If
                End If
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
                'If Not String.IsNullOrEmpty(ComType) Then .AppendLine(" and (B.KTOKD='Z001' or B.KTOKD='" + ComType + "') ")
                If Not String.IsNullOrEmpty(ComType) Then
                    Select Case ComType
                        Case "SHIP"
                            .Append(" AND (A.PARVW = 'WE' OR A.PARVW = 'AG')")
                            .AppendFormat(" AND B.ktokd in ({0})", "'Z001','Z002'")
                        Case "BILL"
                            .Append(" AND (A.PARVW ='RE' OR A.PARVW = 'AG')")
                            .AppendFormat(" AND B.ktokd in ({0})", "'Z001','Z003'")
                    End Select
                End If
                .AppendFormat(" AND A.PARVW in ('WE','AG','RE') ORDER BY A.PARVW asc, A.KUNN2 desc", Org_id)
            End If
        End With
        'Util.SendEmail("eBusiness.AEU@advantech.eu", "nada.liu@advantech.com.cn,ming.zhao@advantech.com.cn,tc.chen@advantech.com.tw", "", "", "eQuotation Sql ", "", sb.ToString, "")
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        dt.TableName = "SAPPF"
        Return dt
    End Function

    Public Shared Function SearchSAPCustomer(ByVal CompanyId As String, ByVal OrgId As String, ByVal CompanyName As String, ByVal Division As String, _
                                             ByVal SalesOffice As String, ByVal SalesGroup As String, ByVal Address As String, ByVal State As String, _
                                             ByVal ZipCode As String) As DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(" select  a.kunnr as Row_ID, a.kunnr as company_id, b.vkorg as org_id, b.vtweg as dist_chann, b.SPART as division, b.VKBUR as SalesOffice, b.VKGRP as SalesGroup, ")
            .AppendLine(" a.name1 || a.name2 as Company_Name, ")
            .AppendLine(" (select adrc.street || adrc.str_suppl3 || adrc.location from saprdp.adrc where adrc.country=a.land1 and adrc.addrnumber=a.adrnr and rownum=1) as Address, ")
            .AppendLine(" a.telfx as fax_no, a.telf1 as tel_no, b.waers as Currency,  ")
            .AppendLine(" C.COUNTRY,C.REGION as region_code,c.post_code1 as Zip_Code,C.CITY1 as City,C.NAME_CO as Attention,  ")
            .AppendLine(" b.zterm as Credit_Term,  ")
            .AppendLine(" (select VTEXT from saprdp.tvsbt where tvsbt.vsbed=b.vsbed and rownum=1) as shipTermName, ")
            .AppendLine(" b.inco1, b.inco2, a.knurl as Url, a.erdat as CreatedDate, ")
            .AppendLine(" b.vsbed as ShipCondition, a.KATR4 as attribute4,  ")
            .AppendLine(" decode((select z.smtp_addr from saprdp.adr6 z where z.addrnumber=a.adrnr and z.client='168' and rownum=1),null,'',(select z.smtp_addr from saprdp.adr6 z where z.addrnumber=a.adrnr and z.client='168' and rownum=1)) as contact_email,  ")
            .AppendLine(" (select c.landx from saprdp.t005t c where c.land1=a.land1 and c.spras='E' and rownum=1) as country_name, a.KATR9 as CUST_IND,  ")
            .AppendLine(" b.KONDA as price_grp, b.PLTYP as price_list, b.inco1, b.inco2 ,b.zterm, ")
            .AppendLine(" (select VTEXT from saprdp.tvzbt where tvzbt.zterm=b.zterm and rownum=1) as paymentTermName ")
            .AppendLine(" from saprdp.kna1 a inner join saprdp.knvv b on a.kunnr=b.kunnr ")
            .AppendLine(" inner join saprdp.adrc c on a.ADRNR=C.ADDRNUMBER and A.LAND1=c.country ")
            .AppendLine(" where a.mandt='168' and b.mandt='168' and c.client='168' and b.vkorg not in ('EU20','EU30','EU31','EU32') and a.ktokd='Z001' and rownum<=30 ")
            If Not String.IsNullOrEmpty(CompanyId) Then .AppendLine(String.Format(" and a.kunnr like '%{0}%' ", Trim(UCase(Replace(Replace(CompanyId, "'", "''"), "*", "%")))))
            If Not String.IsNullOrEmpty(CompanyName) Then .AppendLine(String.Format(" and (upper(a.name1) like '%{0}%' or upper(a.name2) like '%{0}%') ", Trim(UCase(Replace(Replace(CompanyName, "'", "''"), "*", "%")))))
            If Not String.IsNullOrEmpty(Address) Then .AppendLine(String.Format(" and upper(c.STREET) like '%{0}%' ", Trim(Replace(Replace(UCase(Address), "'", "''"), "*", "%"))))
            If Not String.IsNullOrEmpty(State) Then .AppendLine(String.Format(" and upper(c.REGION) like '%{0}%' ", Trim(Replace(Replace(UCase(State), "*", "%"), "'", "''"))))
            If Not String.IsNullOrEmpty(ZipCode) Then .AppendLine(String.Format(" and upper(c.POST_CODE1) like '%{0}%' ", Trim(Replace(Replace(UCase(ZipCode), "*", "%"), "'", "''"))))
            If Not String.IsNullOrEmpty(OrgId) Then .AppendLine(" and b.vkorg='" + UCase(OrgId) + "' ")
            If Not String.IsNullOrEmpty(Division) Then .AppendLine(" and b.SPART='" + Division + "' ")
            If Not String.IsNullOrEmpty(SalesOffice) Then .AppendLine(" and b.VKBUR='" + SalesOffice + "' ")
            If Not String.IsNullOrEmpty(SalesGroup) Then .AppendLine(" and b.VKGRP='" + SalesGroup + "' ")
            .AppendLine(" order by a.kunnr ")
        End With
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        dt.TableName = "SAPCustomer"
        Return dt
    End Function

    Public Shared Function SearchSAPCustomerForPickAccount(ByVal CompanyId As String, ByVal OrgId As String, ByVal CompanyName As String, ByVal Division As String, _
                                         ByVal SalesOffice As String, ByVal SalesGroup As String, ByVal Address As String, ByVal State As String, _
                                         ByVal ZipCode As String) As DataTable

        'select distinct a.pernr as sales_code, a.werks as pers_area, a.persg as emp_group, a.persk as sub_emp_group, 
        '(select b.stras from saprdp.pa0006 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as address,
        '(select b.land1 from saprdp.pa0006 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as country,
        'a.sname, a.ename,
        '(select b.vorna from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as first_name,
        '(select b.nachn from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as last_name,
        'concat(concat((select b.vorna from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168'),'.'),(select b.nachn from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168')) as full_name,
        '(select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='0020' and rownum=1) as tel,
        'decode((select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='MAIL' and rownum=1),null,(select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='0010' and rownum=1),(select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='MAIL' and rownum=1)) as email,
        '(select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='CELL' and rownum=1) as cellphone,
        '(select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='MPHN' and rownum=1) as otherphone,
        'decode((select b.anzkd from saprdp.pa0002 b where b.mandt='168' and rownum=1 and b.pernr=a.pernr),null,0,(select b.anzkd from saprdp.pa0002 b where b.mandt='168' and rownum=1 and b.pernr=a.pernr)) as num_of_child,
        'a.Abkrs as payr_area
        ',(select b.vkbur from saprdp.pa0900 b where b.pernr=a.pernr and b.mandt='168' and rownum=1) as salesoffice
        ',(select b.vkgrp from saprdp.pa0900 b where b.pernr=a.pernr and b.mandt='168' and rownum=1) as salesgroup
        'from saprdp.pa0001 a
        'where a.mandt='168'



        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(" select  a.kunnr as Row_ID, a.kunnr as erpid, b.vkorg as org_id, b.vtweg as dist_chann, b.SPART as division, b.VKBUR as SalesOffice, b.VKGRP as SalesGroup, ")
            .AppendLine(" a.name1 || a.name2 as companyname, ")
            .AppendLine(" (select adrc.street || adrc.str_suppl3 || adrc.location from saprdp.adrc where adrc.country=a.land1 and adrc.addrnumber=a.adrnr and rownum=1) as Address, ")
            .AppendLine(" a.telfx as fax_no, a.telf1 as tel_no, b.waers as Currency,  ")
            .AppendLine(" C.COUNTRY,C.REGION as state,c.post_code1 as ZIPCODE,C.CITY1 as City,C.NAME_CO as Attention,  ")
            .AppendLine(" b.zterm as Credit_Term,  ")
            .AppendLine(" (select VTEXT from saprdp.tvsbt where tvsbt.vsbed=b.vsbed and rownum=1) as shipTermName, ")
            .AppendLine(" b.inco1, b.inco2, a.knurl as Url, a.erdat as CreatedDate, ")
            .AppendLine(" b.vsbed as ShipCondition, a.KATR4 as attribute4,  ")
            .AppendLine(" decode((select z.smtp_addr from saprdp.adr6 z where z.addrnumber=a.adrnr and z.client='168' and rownum=1),null,'',(select z.smtp_addr from saprdp.adr6 z where z.addrnumber=a.adrnr and z.client='168' and rownum=1)) as contact_email,  ")
            .AppendLine(" (select c.landx from saprdp.t005t c where c.land1=a.land1 and c.spras='E' and rownum=1) as country_name, a.KATR9 as CUST_IND,  ")
            .AppendLine(" b.KONDA as price_grp, b.PLTYP as price_list, b.inco1, b.inco2 ,b.zterm, ")
            .AppendLine(" (select VTEXT from saprdp.tvzbt where tvzbt.zterm=b.zterm and rownum=1) as paymentTermName ")
            .AppendLine(" ,(select bb.usrid_long from saprdp.knvp aa left join saprdp.pa0105 bb on aa.PERNR=bb.PERNR where aa.kunnr=a.kunnr and aa.PARVW='VE' and bb.subty='MAIL' and aa.mandt=168 and aa.VKORG='" & OrgId & "' and rownum=1) as priSales ")
            .AppendLine(" ,b.vkorg as RBU ,'' as Status ,'' as location ,'' as province ,'' as Address2")
            .AppendLine(" from saprdp.kna1 a inner join saprdp.knvv b on a.kunnr=b.kunnr ")
            .AppendLine(" inner join saprdp.adrc c on a.ADRNR=C.ADDRNUMBER and A.LAND1=c.country ")
            '.AppendLine(" left join saprdp.knvp d on a.kunnr=d.kunnr ")
            .AppendLine(" where a.mandt='168' and b.mandt='168' and c.client='168' and b.vkorg not in ('EU20','EU30','EU31','EU32') and a.ktokd='Z001' and rownum<=30 ")
            If Not String.IsNullOrEmpty(CompanyId) Then .AppendLine(String.Format(" and a.kunnr like '%{0}%' ", Trim(UCase(Replace(Replace(CompanyId, "'", "''"), "*", "%")))))
            If Not String.IsNullOrEmpty(CompanyName) Then .AppendLine(String.Format(" and (upper(a.name1) like '%{0}%' or upper(a.name2) like '%{0}%') ", Trim(UCase(Replace(Replace(CompanyName, "'", "''"), "*", "%")))))
            If Not String.IsNullOrEmpty(Address) Then .AppendLine(String.Format(" and upper(c.STREET) like '%{0}%' ", Trim(Replace(Replace(UCase(Address), "'", "''"), "*", "%"))))
            If Not String.IsNullOrEmpty(State) Then .AppendLine(String.Format(" and upper(c.REGION) like '%{0}%' ", Trim(Replace(Replace(UCase(State), "*", "%"), "'", "''"))))
            If Not String.IsNullOrEmpty(ZipCode) Then .AppendLine(String.Format(" and upper(c.POST_CODE1) like '%{0}%' ", Trim(Replace(Replace(UCase(ZipCode), "*", "%"), "'", "''"))))
            If Not String.IsNullOrEmpty(OrgId) Then .AppendLine(" and b.vkorg='" + UCase(OrgId) + "' ")
            If Not String.IsNullOrEmpty(Division) Then .AppendLine(" and b.SPART='" + Division + "' ")
            If Not String.IsNullOrEmpty(SalesOffice) Then .AppendLine(" and b.VKBUR='" + SalesOffice + "' ")
            If Not String.IsNullOrEmpty(SalesGroup) Then .AppendLine(" and b.VKGRP='" + SalesGroup + "' ")
            .AppendLine(" order by a.kunnr ")
        End With
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        dt.TableName = "SAPCustomer"
        Return dt
    End Function

    Public Shared Function GetOrderListFromSAP(ByVal PoNo As String, ByVal SoNo As String, ByVal CompanyID As String, ByVal CompanyName As String, ByVal OrgID As String, _
                                               ByVal OrderDateFrom As Object, ByVal OrderDateTo As Object, Optional ByVal TopNum As Integer = 10, Optional ByVal Qtype As Integer = 0) As DataTable

        'If DateTime.TryParse(OrderDateFrom, Date.Now()) = False OrElse DateTime.TryParse(OrderDateTo, Date.Now()) = False Then
        '    Return New DataTable("SAPPF")
        'End If
        Dim sb As New System.Text.StringBuilder
        With sb

            .AppendLine(" select VBAK.VBELN AS SoNo, VBAK.BSTNK AS PoNo, VBAK.KUNNR as SOLDTOID, ")
            .AppendLine("   (SELECT NAME1 FROM SAPRDP.KNA1 WHERE KUNNR=VBAK.KUNNR AND ROWNUM=1) AS COMPANYNAME, ")
            .AppendLine(" (select kunnr from saprdp.vbpa where vbpa.vbeln=vbak.vbeln and vbpa.parvw='RE' and rownum=1) AS BILLTOID, ")
            .AppendLine(" (select kunnr from saprdp.vbpa where vbpa.vbeln=vbak.vbeln and vbpa.parvw='WE' and rownum=1) AS SHIPTOID, ")
            .AppendLine(" VBAK.VKORG AS ORG_ID, VBAK.AUDAT AS ORDERDATE ")
            '.AppendFormat(" from (select VBAK.* from  SAPRDP.VBAK   where 1=1 ")
            .AppendFormat(" from (select VBAK.*, KNA1.NAME1 as CompanyName from  SAPRDP.VBAK left join SAPRDP.KNA1 on VBAK.KUNNR=KNA1.KUNNR where 1=1 ")
            If String.IsNullOrEmpty(PoNo) AndAlso String.IsNullOrEmpty(SoNo) AndAlso String.IsNullOrEmpty(CompanyID) _
              AndAlso Date.TryParse(OrderDateFrom, Now) AndAlso Date.TryParse(OrderDateFrom, Now) Then
                .AppendLine(String.Format(" and VBAK.AUDAT between '{0}' and '{1}'  ", Date.Parse(OrderDateFrom).ToString("yyyyMMdd"), Date.Parse(OrderDateTo).ToString("yyyyMMdd")))
            End If
            .Append("  order by VBAK.AUDAT desc,VBAK.VBELN desc) VBAK  ")
            .Append("  WHERE 1=1  ")
            If Qtype = 0 Then
                .AppendFormat(" and VBAK.AUART='{0}' ", "AG")
            ElseIf Qtype = 1 Then
                If OrgID.Equals("BR01", StringComparison.InvariantCultureIgnoreCase) Then
                    .AppendFormat(" and VBAK.AUART in ({0})", "'ZORI','ZORC','ZORR'")
                Else
                    .AppendFormat(" and VBAK.AUART in ({0})", "'ZOR','ZOR2'")
                End If
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and VBAK.KUNNR like '%{0}%' ", Trim(UCase(Replace(Replace(CompanyID, "*", "%"), "'", "''"))))
            End If

            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and VBAK.CompanyName like '%{0}%' ", Trim(UCase(Replace(Replace(CompanyName, "*", "%"), "'", "''"))))
            End If


            If Not String.IsNullOrEmpty(PoNo) Then
                .AppendFormat(" and upper(VBAK.BSTNK) like '%{0}%' ", Trim(UCase(Replace(Replace(PoNo, "*", "%"), "'", "''"))))
            End If
            If Not String.IsNullOrEmpty(SoNo) Then
                .AppendFormat(" and upper(VBAK.VBELN) like '%{0}%' ", Trim(UCase(Replace(Replace(SoNo, "*", "%"), "'", "''"))))
            End If
            If Not String.IsNullOrEmpty(OrgID) Then
                .AppendFormat(" and VBAK.VKORG='{0}' ", UCase(Replace(OrgID, "'", "''")))
            End If
            If Not String.IsNullOrEmpty(TopNum) Then
                .AppendFormat(" and rownum<={0} ", TopNum)
            Else
                .AppendFormat(" and rownum<=100 ")
            End If
        End With
        Dim dt As New DataTable("SAPOrders")
        Dim connstr As String = "SAP_PRD"
        If COMM.Util.IsTesting() Then
            connstr = "SAP_Test"
        End If
        dt = OraDbUtil.dbGetDataTable(connstr, sb.ToString())
        Return dt
    End Function

    Public Shared Function GetOrderMasterFromSAP(ByVal SoNo As String) As DataTable
        'If String.IsNullOrEmpty(OrgID) Then Return New DataTable("SAPPF")
        'PoNo = Replace(Trim(PoNo.ToUpper), "'", "")
        'SoNo = Replace(Trim(SoNo.ToUpper), "'", "")
        'CompanyID = Replace(Trim(CompanyID.ToUpper), "'", "")
        'OrgID = Replace(Trim(OrgID.ToUpper), "'", "")
        'If DateTime.TryParse(OrderDateFrom, Date.Now()) = False OrElse DateTime.TryParse(OrderDateTo, Date.Now()) = False Then
        '    Return New DataTable("SAPPF")
        'End If
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(" select VBAK.VBELN AS SoNo, VBAK.BSTNK AS PoNo, VBAK.KUNNR as SOLDTOID, ")
            .AppendLine(" (select kunnr from saprdp.vbpa where vbpa.vbeln=vbak.vbeln and vbpa.parvw='RE' and rownum=1) AS BILLTOID, ")
            .AppendLine(" (select kunnr from saprdp.vbpa where vbpa.vbeln=vbak.vbeln and vbpa.parvw='WE' and rownum=1) AS SHIPTOID, VBAK.BUKRS_VF AS ORG_ID,")
            .AppendFormat(" VBAK.AUDAT AS ORDERDATE  from SAPRDP.VBAK where VBAK.VBELN='{0}' ", SoNo.ToUpper.Trim())
        End With
        Dim dt As New DataTable("SAPOrders")
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return Nothing
    End Function

    Public Shared Function GetOrderDetailFromSAPBySoNo(ByVal SoNo As String) As DataTable
        If String.IsNullOrEmpty(SoNo) Then Return New DataTable("SAPDT")
        SoNo = Replace(Trim(SoNo.ToUpper), "'", "''")
        'If Relics.Global_Inc.IsNumericItem(SoNo) Then
        '    SoNo = Relics.Global_Inc.Format2SAPItem2(SoNo)
        'End If
        If SAPDAL.Global_Inc.IsNumericItem(SoNo) Then
            SoNo = SAPDAL.Global_Inc.Format2SAPItem2(SoNo)
        End If

        Dim objCurrency As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", "select WAERK from saprdp.VBAK Where VBAK.VBELN = '" & SoNo & "'")
        Dim CurrencyMarkUp As Decimal = 1

        If objCurrency IsNot Nothing Then
            Dim objCurrencyMarkUp As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", String.Format("SELECT CURRDEC FROM SAPRDP.TCURX WHERE CURRKEY = '{0}'", objCurrency.ToString()))
            If objCurrencyMarkUp IsNot Nothing AndAlso Int32.TryParse(objCurrencyMarkUp.ToString(), CurrencyMarkUp) Then
                CurrencyMarkUp = Convert.ToInt32(100 * Math.Pow(10, CurrencyMarkUp))
            End If
        End If



        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine("  select cast(VBAP.POSNR as integer) AS Lineno, VBAP.MATWA AS  Partno,  ")
            '.AppendLine("  VBAP.LSMENG AS  Qty, VBAP.ZZ_EDATU AS ReqDate, VBAP.NETPR AS UnitPrice,VBAP.NETWR AS  Amount ")
            .AppendLine("  VBAP.LSMENG AS  Qty, VBAP.ZZ_EDATU AS ReqDate, (VBAP.NETPR * " & CurrencyMarkUp & ") AS UnitPrice,(VBAP.NETWR * " & CurrencyMarkUp & ") AS Amount, ")
            .AppendLine("  VBAP.WAERK AS Currency ")
            'VBAP.WAERK

            .AppendFormat(" from   saprdp.VBAP where VBAP.VBELN ='{0}'  ", SoNo)
            .AppendLine(" order by Lineno ")
        End With
        Dim dt As New DataTable("SAPOrders")
        Dim connstr As String = "SAP_PRD"
        If COMM.Util.IsTesting() Then
            connstr = "SAP_Test"
        End If
        dt = OraDbUtil.dbGetDataTable(connstr, sb.ToString())
        Return dt
    End Function

    Public Shared Function GetShipMethodNameByValue(ByVal ShipMethodValue As String) As String
        If ShipMethodValue.Equals("0") Then Return "TBD"
        Dim retObj As Object = Nothing
        Dim cmd As New SqlClient.SqlCommand("select top 1 SHIPTERMNAME from SAP_COMPANY_LOV where SHIPTERM=@SV", _
                                            New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
        cmd.Parameters.AddWithValue("SV", ShipMethodValue)
        cmd.Connection.Open() : retObj = cmd.ExecuteScalar() : cmd.Connection.Close()
        If retObj IsNot Nothing Then
            Return retObj.ToString()
        End If
        Return ShipMethodValue
    End Function

    Public Shared Function GetPaymentMethodNameByValue(ByVal PaymentMethodValue As String) As String
        If PaymentMethodValue.Equals("0") Then Return "TBD"
        Dim retObj As Object = Nothing
        Dim cmd As New SqlClient.SqlCommand("select top 1 PAYMENTTERMNAME from SAP_COMPANY_LOV where PAYMENTTERM=@SV", _
                                            New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
        cmd.Parameters.AddWithValue("SV", PaymentMethodValue)
        cmd.Connection.Open() : retObj = cmd.ExecuteScalar() : cmd.Connection.Close()
        If retObj IsNot Nothing Then
            Return retObj.ToString()
        End If
        Return PaymentMethodValue
    End Function



    Public Shared Function GetSAPPartnerProfile(ByVal ERPID As String, ByVal PartnerType As EnumSetting.PartnerTypes, _
        ByRef CompanyName As String, ByRef Address As String, ByRef Attention As String, ByRef Tel As String, ByRef Mobile As String) As Boolean
        CompanyName = "" : Address = "" : Tel = "" : Mobile = "" : Attention = ""
        Dim strSearchedParvw As String = ""
        Select Case PartnerType
            Case EnumSetting.PartnerTypes.BillTo
                strSearchedParvw = "RE"
            Case EnumSetting.PartnerTypes.ShipTo
                strSearchedParvw = "WE"
            Case EnumSetting.PartnerTypes.SoldTo
                strSearchedParvw = "AG"
        End Select
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(" SELECT A.KUNN2 AS company_id,B.NAME1 AS COMPANY_NAME,  " + _
                        " D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1 and E.MANDT=168) AS Address,  " + _
                        " B.Land1 AS  COUNTRY,B.Ort01 AS CITY, B.PSTLZ AS ZIP_CODE, D.region AS STATE,  C.smtp_addr AS CONTACT_EMAIL, B.TELF1 AS TEL_NO, " + _
                        " B.TELF2 AS Mobile, B.TELFX AS FAX_NO, ")
            .AppendLine(" case A.PARVW when 'WE' then 'Ship-To' when 'AG' then 'Sold-To' when 'RE' then 'Bill-To' end as PARTNER_FUNCTION, D.NAME_CO as Attention ")
            .AppendLine(" FROM saprdp.knvp A  ")
            .AppendLine(" Left JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR Left join saprdp.adr6 C on B.adrnr=C.addrnumber ")
            .AppendLine(" Left join saprdp.adrc D on  D.country=B.land1 and D.addrnumber=B.adrnr  ")
            .AppendLine(" where ")
            .AppendFormat("  A.Kunn2 = '{0}' ", ERPID)
            .AppendFormat(" AND A.PARVW in ('AG','RE','WE') and rownum<=30 and A.MANDT=168 and B.MANDT=168 ORDER BY A.Kunn2 ")
        End With
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        If dt.Rows.Count > 0 Then
            Dim rs() As DataRow = dt.Select("PARTNER_FUNCTION='" + strSearchedParvw + "'")
            Dim foundRow As DataRow = Nothing
            If rs.Length > 0 Then
                foundRow = rs(0)
            Else
                foundRow = dt.Rows(0)
            End If
            CompanyName = foundRow.Item("COMPANY_NAME")
            If Not IsDBNull(foundRow.Item("TEL_NO")) Then
                Tel = foundRow.Item("TEL_NO")
            End If
            If Not IsDBNull(foundRow.Item("Attention")) Then
                Attention = foundRow.Item("Attention")
            End If
            If Not IsDBNull(foundRow.Item("Address")) Then
                Address = foundRow.Item("Address")
            End If
            If Not IsDBNull(foundRow.Item("Mobile")) Then
                Mobile = foundRow.Item("Mobile")
            End If
            Return True
        End If
        Return False
    End Function



    Public Shared Function GetDefaultDistChannDivisionSalesGrpOfficeByCompanyId( _
     ByVal CompanyId As String, ByRef dist_chann As String, ByRef division As String, ByRef SalesGroup As String, ByRef SalesOffice As String) As Boolean
        Dim strSql As String = _
            " select b.vtweg as dist_chann, b.SPART as division, b.VKBUR as SalesOffice, b.VKGRP as SalesGroup " + _
            " from saprdp.knvv b " + _
            " where b.mandt='168' and b.kunnr = '" + UCase(CompanyId) + "' and rownum=1 " + _
            " order by b.VKGRP, b.VKBUR "
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                dist_chann = .Item("dist_chann") : division = .Item("division") : SalesGroup = .Item("SalesGroup") : SalesOffice = .Item("SalesOffice")
            End With
            Return True
        Else
            Return False
        End If
    End Function
End Class
