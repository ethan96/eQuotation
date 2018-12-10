Imports System.Web
Imports System.Web.Caching
Imports System.Configuration
Imports System.Linq

Public Class ePricerDiscount
    Public Shared Function GetProductDiscountByEPricerPriceGrade(PriceGrade As String, PricingRBU As String, PricingCurrency As String,
                                                                    SalesOrg As String, Products As List(Of ProductPrice)) As Boolean

        Dim epPriceCache As List(Of ePricerPriceCache) = Nothing
        Try
            epPriceCache = HttpContext.Current.Cache("ePricer Price Cache")
        Catch ex As InvalidCastException
            HttpContext.Current.Cache.Remove("ePricer Price Cache") : epPriceCache = Nothing
        End Try
        If epPriceCache Is Nothing Then
            epPriceCache = New List(Of ePricerPriceCache)
            HttpContext.Current.Cache.Add("ePricer Price Cache", epPriceCache, Nothing, Now.AddHours(12),
                                     System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If

        '20150209TC: Get the current pricing year & quarter from ePricer's definition table
        'Ryan 20161212 Change connection string to ACLSTNR12 due to MyLocal is unstable
        Dim MYConn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        Dim CurPriceYear As Integer = -1, CurPriceQuarter As Integer = -1

        '***************Ryan 20161020 Mark below code out due to ePricer_Price_Control table is no longer available.*********
        'Dim YQApt As New SqlClient.SqlDataAdapter("select pricec_curr_year, pricec_curr_quarter from ePricer_Price_Control with (nolock) where org=@RBU and getdate() between pricec_start_date and pricec_end_date ", ePriceConn)
        'YQApt.SelectCommand.Parameters.AddWithValue("RBU", PricingRBU)
        'Dim YQDt As New DataTable, CurPriceYear As Integer = -1, CurPriceQuarter As Integer = -1
        'YQApt.Fill(YQDt)
        'If YQDt.Rows.Count > 0 Then
        '    CurPriceYear = YQDt.Rows(0).Item("pricec_curr_year") : CurPriceQuarter = YQDt.Rows(0).Item("pricec_curr_quarter")
        'Else
        '    Return False
        'End If
        '***********************************End Ryan 20161020 Comment out.****************************************************

        For Each p In Products

            Dim tmpCache = From q In epPriceCache Where q.PartNo = p.PartNo And q.PriceGrade = PriceGrade _
                           And q.PricingCurrency = PricingCurrency And q.PricingRBU = PricingRBU And q.SalesOrg = SalesOrg
            '20150209TC: If item + pricegrade already exists in cache then get it from cache
            If tmpCache.Count > 0 Then
                p.ListPrice = tmpCache(0).ListPrice : p.DiscountPrice = tmpCache(0).DiscountPrice : p.Cost = tmpCache(0).Cost
                p.IsPricingOK = True
            Else
                If MYConn.State <> ConnectionState.Open Then MYConn.Open()
                Dim tmpMaterialPricingGrp As String = "", tmpProductLine As String = ""

                If IsPTD(p.PartNo, SalesOrg, tmpProductLine, tmpMaterialPricingGrp) Then

                    ' Ryan 20161219 New Ptrade discount logic
                    ' 1. get ZVP2 cost from SAP
                    ' 2. get item 's material pricing group and product_line from SAP product tables (already been done in IsPTD function)
                    ' 3. get KNUMH by Pirce Grade user inputed from SAP A523 table
                    ' 4. get discount rate from SAP KONP table with KUNMH value 

                    p.IsPTrade = True : p.MaterialPricingGroup = tmpMaterialPricingGrp : p.ProductLine = tmpProductLine

                    'Verify Price Grade input format
                    If PriceGrade.Length = 8 Then
                        For i As Integer = 0 To PriceGrade.Length - 1
                            If i Mod 2 <> 0 AndAlso Not (Char.IsNumber(PriceGrade(i))) Then
                                Return False
                            ElseIf i Mod 2 = 0 AndAlso Not (Char.IsLetter(PriceGrade(i))) Then
                                Return False
                            End If
                        Next
                    Else
                        Return False
                    End If

                    'Split price grade into four substring for further using
                    Dim G1 As String = PriceGrade.Substring(0, 2), G2 As String = PriceGrade.Substring(2, 2), G3 As String = PriceGrade.Substring(4, 2), G4 As String = PriceGrade.Substring(6, 2)

                    Dim err As String = "", cur As String = ""
                    Dim cost As Double = GetZVP2Cost(SalesOrg, p.PartNo, err, cur)
                    p.Cost = cost

                    If String.IsNullOrEmpty(err) And cost > 0 Then
                        Dim SAP_str1 As String = "SELECT KNUMH from saprdp.A523 WHERE MANDT='168' AND KAPPL='V' AND KSCHL='ZMAR' " +
                          " AND DATBI>='" + Date.Today.ToString("yyyyMMdd") + "'  AND DATAB<='" + Date.Today.ToString("yyyyMMdd") + "'" +
                          " AND VKORG='" + SalesOrg + "' AND KDGRP='03'" +
                          " AND KDKG1='" + G1 + "' AND KDKG2='" + G2 + "' AND KDKG3='" + G3 + "' AND KDKG4='" + G4 + "' " +
                          " AND PRODH3='" + p.ProductLine + "' AND KONDM='" + p.MaterialPricingGroup + "'"

                        Dim SAP_KNUMH As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", SAP_str1)
                        If Not SAP_KNUMH Is Nothing Then
                            Dim SAP_Discount As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", "select KBETR from saprdp.KONP WHERE KNUMH = '" + SAP_KNUMH.ToString + "'")
                            If Not SAP_Discount Is Nothing Then
                                p.DiscountPrice = cost * (1 + Convert.ToDouble(SAP_Discount) / 1000) : p.IsPricingOK = True : p.IsPTrade = True
                                Dim ePricerPriceCache1 As New ePricerPriceCache
                                With ePricerPriceCache1
                                    .PartNo = p.PartNo : .PriceGrade = PriceGrade : .PricingCurrency = PricingCurrency : .PricingRBU = PricingRBU : .SalesOrg = SalesOrg
                                    .ListPrice = -1 : .DiscountPrice = p.DiscountPrice : .Cost = p.Cost
                                End With
                                epPriceCache.Add(ePricerPriceCache1)
                            Else
                                p.DiscountPrice = -1
                            End If
                        Else
                            p.DiscountPrice = -1
                        End If
                    Else
                        p.Cost = -1 : p.DiscountPrice = -1
                    End If


                    '20150209TC: If item is a p-trade, get the markup rate from PriceGrade_Detail, and get ZVP2 cost from SAP, and then use cost to markup
                    ' To select markup from PriceGrade_Detail we need to get item's material pricing group and product_line from SAP product tables
                    'p.IsPTrade = True : p.MaterialPricingGroup = tmpMaterialPricingGrp : p.ProductLine = tmpProductLine
                    'Dim sql As String = _
                    '    " select distinct top 1 a.PROD_LN, a.ITEM_GROUP, cast((a.AMOUNT *0.01)+1 as numeric(18,2)) as MarkupRate, a.YEAR, a.[QUARTER]  " + _
                    '    " from ePricer_PriceGrade_Detail a with (nolock)  " + _
                    '    " where a.ORG=@RBU and a.PROD_LN=@PLINE and a.GRADE_NAME=@PGRADE  " + _
                    '    " and a.[YEAR]=" + CurPriceYear.ToString() + " and a.[QUARTER]=" + CurPriceQuarter.ToString() + " and a.ITEM_GROUP=@MPGRP and a.PRI_TYPE='% Markup' " + _
                    '    " order by a.[YEAR] desc, a.[QUARTER] desc "

                    'Dim apt As New SqlClient.SqlDataAdapter(sql, ePriceConn)
                    'With apt.SelectCommand.Parameters
                    '    .AddWithValue("RBU", PricingRBU) : .AddWithValue("PLINE", tmpProductLine) : .AddWithValue("PGRADE", PriceGrade) : .AddWithValue("MPGRP", tmpMaterialPricingGrp)
                    'End With
                    'Dim dt As New DataTable
                    'apt.Fill(dt)

                    'Dim err As String = "", cur As String = ""
                    'Dim cost As Double = GetZVP2Cost(SalesOrg, p.PartNo, err, cur)
                    'p.Cost = cost
                    'End 20150209TC Code

                    'If dt.Rows.Count > 0 Then
                    '    If String.IsNullOrEmpty(Err) And cost > 0 Then
                    '        p.DiscountPrice = cost * dt.Rows(0).Item("MarkupRate") : p.IsPricingOK = True : p.IsPTrade = True
                    '        Dim ePricerPriceCache1 As New ePricerPriceCache
                    '        With ePricerPriceCache1
                    '            .PartNo = p.PartNo : .PriceGrade = PriceGrade : .PricingCurrency = PricingCurrency : .PricingRBU = PricingRBU : .SalesOrg = SalesOrg
                    '            .ListPrice = -1 : .DiscountPrice = p.DiscountPrice : .Cost = p.Cost
                    '        End With
                    '        epPriceCache.Add(ePricerPriceCache1)
                    '    Else
                    '        p.Cost = -1 : p.DiscountPrice = -1
                    '    End If
                    'End If
                Else
                    '20150209 TC: if it's a standard item, get discount price from table "Price"
                    p.IsPTrade = False

                    'Ryan 20161020 No longer use year and quater to select price data, use part_no to get latest data is enough.
                    Dim sql As String =
                       " select distinct top 1 a.PROD_NAME, a.LIST_PRICE, a.DISCOUNT1, a.AMT1, a.YEAR, a.[QUARTER]  " +
                       " from ePricer_Price a with (nolock) " +
                       " where a.ORG=@RBU and a.GRADE_NAME=@PGRADE and a.CURCY_CD='TWD' " +
                       " and a.PROD_NAME=@PN " +
                       " order by a.YEAR desc, a.[QUARTER] desc "

                    Dim apt As New SqlClient.SqlDataAdapter(sql, MYConn)
                    With apt.SelectCommand.Parameters
                        .AddWithValue("RBU", PricingRBU) : .AddWithValue("PN", p.PartNo) : .AddWithValue("PGRADE", PriceGrade)
                    End With
                    Dim dt As New DataTable
                    apt.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        p.DiscountPrice = dt.Rows(0).Item("AMT1") : p.ListPrice = dt.Rows(0).Item("LIST_PRICE") : p.IsPricingOK = True
                        Dim ePricerPriceCache1 As New ePricerPriceCache
                        With ePricerPriceCache1
                            .PartNo = p.PartNo : .PriceGrade = PriceGrade : .PricingCurrency = PricingCurrency : .PricingRBU = PricingRBU : .SalesOrg = SalesOrg
                            .ListPrice = p.ListPrice : .DiscountPrice = p.DiscountPrice : .Cost = -1
                        End With
                        epPriceCache.Add(ePricerPriceCache1)

                    Else
                        p.DiscountPrice = -1
                    End If

                End If
            End If


        Next
        MYConn.Close()

        Return True
    End Function

    <Serializable()>
    Public Class ePricerPriceCache
        Public Property PartNo As String : Public Property PriceGrade As String : Public Property PricingRBU As String
        Public Property PricingCurrency As String : Public Property SalesOrg As String
        Public Property ListPrice As Double : Public Property DiscountPrice As Double : Public Property Cost As Double
    End Class

    <Serializable()>
    Public Class ProductPrice
        Public Property PartNo As String : Property Qty As Integer : Public Property DiscountPrice As Double
        Public Property ListPrice As Double : Public Property Cost As Double : Public Property IsPricingOK As Boolean
        Public Property IsPTrade As Boolean : Public MaterialPricingGroup As String : Public Property ProductLine As String
        Public Sub New()
            IsPricingOK = False : IsPTrade = False
        End Sub
        Public Sub New(PartNo As String)
            Me.PartNo = PartNo : Me.Qty = 1
        End Sub
    End Class

    <Serializable()>
    Public Class IsPtradeRec
        Public Property PartNo As String : Public Property ProductLine As String : Public Property IsPTrade As Boolean
        Public Property MaterialPricingGrp As String : Public Property SalesOrg As String
        Public Sub New(PartNo As String, ProductLine As String, IsPTrade As Boolean, MaterialPricingGrp As String, SalesOrg As String)
            Me.PartNo = PartNo : Me.ProductLine = ProductLine : Me.IsPTrade = IsPTrade : Me.MaterialPricingGrp = MaterialPricingGrp : Me.SalesOrg = SalesOrg
        End Sub
    End Class

    ''' <summary>
    ''' Pass part no. and SAP sales org (ex: TW01), and returns true if it's a p-trade
    ''' And when it's p-trade, its product line and sales org's corresponding material pricing group are also returned
    ''' If it's not p-trade, function will only return false.    
    ''' </summary>
    ''' <param name="PartNo"></param>
    ''' <param name="SalesOrg"></param>
    ''' <param name="ProductLine"></param>
    ''' <param name="MaterialPricingGrp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsPTD(PartNo As String, SalesOrg As String, ByRef ProductLine As String, ByRef MaterialPricingGrp As String) As Boolean
        PartNo = Trim(PartNo).ToLower()
        Dim ListPTD As List(Of IsPtradeRec) = Nothing
        Try
            ListPTD = HttpContext.Current.Cache("Is PTrade List")
        Catch ex As InvalidCastException
            HttpContext.Current.Cache.Remove("Is PTrade List") : ListPTD = Nothing
        End Try

        If ListPTD Is Nothing Then
            ListPTD = New List(Of IsPtradeRec)
            HttpContext.Current.Cache.Add("Is PTrade List", ListPTD, Nothing, Now.AddHours(12),
                                        System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If

        Dim r = From q In ListPTD Where String.Equals(q.PartNo, PartNo, StringComparison.CurrentCultureIgnoreCase) And
                String.Equals(q.SalesOrg, SalesOrg, StringComparison.CurrentCultureIgnoreCase)

        If r.Count = 0 Then
            Dim f As Boolean = False
            Dim STR As String =
                " select distinct a.PRODUCT_TYPE, a.PRODUCT_LINE, IsNull(b.MATERIAL_PRICING_GRP,'') as MATERIAL_PRICING_GRP " +
                " from SAP_PRODUCT a with (nolock) inner join SAP_PRODUCT_STATUS b with (nolock) on a.PART_NO=b.PART_NO  " +
                " where b.SALES_ORG=@ORG and " +
                " ( " +
                " 	(PRODUCT_TYPE = 'ZPER') OR  " +
                " 	( " +
                " 		(PRODUCT_TYPE = 'ZFIN' OR PRODUCT_TYPE = 'ZOEM') AND  " +
                " 		( " +
                " 			a.PART_NO LIKE 'BT%' OR a.PART_NO LIKE 'DSD%' OR a.PART_NO LIKE 'ES%' OR a.PART_NO LIKE 'EWM%' OR a.PART_NO LIKE 'GPS%'  " +
                " 			OR a.PART_NO LIKE 'SQF%' OR a.PART_NO LIKE 'WIFI%' OR a.PART_NO LIKE 'PMM%' OR a.PART_NO LIKE 'Y%' " +
                " 		) " +
                " 	) OR  " +
                " 	( " +
                " 		(PRODUCT_TYPE = 'ZRAW') AND (a.PART_NO LIKE '206Q%') " +
                " 	) OR  " +
                " 	( " +
                " 		(PRODUCT_TYPE = 'ZSEM') AND (a.PART_NO LIKE '968Q%') " +
                " 	) " +
                " )  " +
                " AND a.PART_NO = @PN "
            Dim apt As New SqlClient.SqlDataAdapter(STR, ConfigurationManager.ConnectionStrings("MY").ConnectionString)
            apt.SelectCommand.Parameters.AddWithValue("ORG", SalesOrg) : apt.SelectCommand.Parameters.AddWithValue("PN", PartNo)
            Dim dt As New DataTable
            apt.Fill(dt)
            apt.SelectCommand.Connection.Close()
            Dim IsPtradeRec1 As IsPtradeRec = Nothing
            If dt.Rows.Count > 0 Then
                IsPtradeRec1 = New IsPtradeRec(PartNo, dt.Rows(0).Item("PRODUCT_LINE"), True, dt.Rows(0).Item("MATERIAL_PRICING_GRP"), SalesOrg)
            Else
                IsPtradeRec1 = New IsPtradeRec(PartNo, "", False, "", SalesOrg)
            End If
            ListPTD.Add(IsPtradeRec1)
        End If

        r = From q In ListPTD Where String.Equals(q.PartNo, PartNo, StringComparison.CurrentCultureIgnoreCase) And
            String.Equals(q.SalesOrg, SalesOrg, StringComparison.CurrentCultureIgnoreCase)

        If r.Count > 0 Then
            MaterialPricingGrp = r(0).MaterialPricingGrp : ProductLine = r(0).ProductLine
            Return r(0).IsPTrade
        End If
        Return False
    End Function

    <Serializable()>
    Public Class PTradeZVP2Cost
        Implements IEquatable(Of PTradeZVP2Cost)

        Public Property SalesOrg As String : Public Property PartNo As String : Public Property Currency As String
        Public Property ZVP2Cost As Double : Public Property HasPriceFlag As Boolean
        Public Sub New(SalesOrg As String, PartNo As String, Currency As String, ZVP2Cost As String, HasPriceFlag As Boolean)
            Me.SalesOrg = SalesOrg : Me.PartNo = PartNo : Me.Currency = Currency : Me.ZVP2Cost = ZVP2Cost : Me.HasPriceFlag = HasPriceFlag
        End Sub

        Public Function Equals1(other As PTradeZVP2Cost) As Boolean Implements System.IEquatable(Of PTradeZVP2Cost).Equals
            If Me.SalesOrg = other.SalesOrg And Me.PartNo = other.PartNo Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class

    ''' <summary>
    ''' Get item's price condition ZVP2. This value will be multiplied by ePricer's markup rate to get the final price based on ATW AOnline sales input's price grade.
    ''' If ZVP2 is not available, this function will return -1.    
    ''' </summary>
    ''' <param name="SalesOrg"></param>
    ''' <param name="PartNo"></param>
    ''' <param name="ErrorMessage"></param>
    ''' <param name="Currency"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetZVP2Cost(ByVal SalesOrg As String, ByVal PartNo As String, ByRef ErrorMessage As String, ByRef Currency As String) As Double
        Currency = "" : ErrorMessage = "" : PartNo = Trim(UCase(PartNo))
        Dim ListPTradeCost As List(Of PTradeZVP2Cost) = Nothing
        Try
            ListPTradeCost = HttpContext.Current.Cache("List PTradeZVP2Cost")
        Catch ex As InvalidCastException
            HttpContext.Current.Cache.Remove("List PTradeZVP2Cost") : ListPTradeCost = Nothing
        End Try

        If ListPTradeCost Is Nothing Then
            ListPTradeCost = New List(Of PTradeZVP2Cost)
            HttpContext.Current.Cache.Add("List PTradeZVP2Cost", ListPTradeCost, Nothing, Now.AddHours(12),
                                          System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If

        Dim PTradeZVP2Cost1 As New PTradeZVP2Cost(SalesOrg, PartNo, "", -1, False)
        If Not ListPTradeCost.Contains(PTradeZVP2Cost1) Then

            Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))

            Try
                Dim ERPId As String = ""

                Select Case Left(SalesOrg, 2).ToUpper()
                    '20140116 TC: Should select ERPID from eQuotation.dbo.ESTORE_PRICING_ERPID instead
                    Case "TW"
                        ERPId = "2NC00001" : SalesOrg = "TW01"
                End Select

                Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
                Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
                Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable

                With OrderHeader
                    .Doc_Type = "ZOR" : .Sales_Org = SalesOrg : .Distr_Chan = "10" : .Division = "00"
                End With

                Dim FakeItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                FakeItem.Itm_Number = "000001" : FakeItem.Material = SAPDAL.GetAHighLevelItemForPricing(SalesOrg) : FakeItem.Req_Qty = 1 : ItemsIn.Add(FakeItem)

                Dim MainItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                MainItem.Itm_Number = "000002" : MainItem.Material = Global_Inc.Format2SAPItem(PartNo.Trim().ToUpper()) : MainItem.Req_Qty = 1 : MainItem.Hg_Lv_Item = "000001"
                ItemsIn.Add(MainItem)


                Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
                Dim retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
                SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = ERPId : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = ERPId
                Partners.Add(SoldTo) : Partners.Add(ShipTo)
                proxy1.Connection.Open()
                Dim dtItem As New DataTable, dtPartNr As New DataTable, dtcon As New DataTable, DTRET As New DataTable

                dtItem = ItemsIn.ToADODataTable() : dtPartNr = Partners.ToADODataTable() : dtcon = Conditions.ToADODataTable()

                proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "",
                                                New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO,
                                                New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt,
                                                New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable,
                                                New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable,
                                                New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable,
                                                New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable,
                                                ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable,
                                                New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)
                proxy1.Connection.Close()

                For Each retMsgRec As BAPI_SALESORDER_SIMULATE.BAPIRET2 In retDt
                    If retMsgRec.Type = "E" Then
                        ErrorMessage += String.Format("Type:{0};Msg:{1}", retMsgRec.Type, retMsgRec.Message + ";" + retMsgRec.Message_V1 + ";" + retMsgRec.Message_V2) + vbCrLf
                    End If
                Next
                Dim condDt As DataTable = Conditions.ToADODataTable()
                Dim rs() As DataRow = condDt.Select("Cond_Type='ZVP2' and Itm_Number='000002'")
                If rs.Length > 0 Then
                    Currency = rs(0).Item("Currency")
                    PTradeZVP2Cost1.ZVP2Cost = rs(0).Item("Cond_Value") : PTradeZVP2Cost1.Currency = Currency : PTradeZVP2Cost1.HasPriceFlag = True
                Else
                    ErrorMessage += "; Cannot find condition ZVP2"
                    PTradeZVP2Cost1.ZVP2Cost = -1 : PTradeZVP2Cost1.Currency = "" : PTradeZVP2Cost1.HasPriceFlag = False
                End If
                ListPTradeCost.Add(PTradeZVP2Cost1)
            Catch ex As Exception
                If Not IsNothing(proxy1) AndAlso Not IsNothing(proxy1.Connection) Then
                    proxy1.Connection.Close()
                End If
            End Try

            'Return SalesOrgPartNoMinPrice1.MinPrice
        End If

        If ListPTradeCost.Contains(PTradeZVP2Cost1) Then
            Dim CostResult As PTradeZVP2Cost = ListPTradeCost.Find(Function(obj) obj.SalesOrg = SalesOrg And obj.PartNo = PartNo)
            Currency = CostResult.Currency
            Return CostResult.ZVP2Cost
        Else
            Return -1
        End If
    End Function


    <Services.WebMethod(enablesession:=True)>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function GetPriceGrades(ByVal prefixText As String, ByVal count As Integer) As String()
        prefixText = UCase(Replace(Trim(prefixText), "'", "''"))
        prefixText = UCase(Replace(Trim(prefixText), "*", "%"))
        Dim PGradeList As List(Of ePricerPriceGrade) = Nothing
        Try
            PGradeList = HttpContext.Current.Cache("ATW Price Grade List")
        Catch ex As InvalidCastException
            HttpContext.Current.Cache.Remove("ATW Price Grade List") : PGradeList = Nothing
        End Try

        If PGradeList Is Nothing Then
            PGradeList = New List(Of ePricerPriceGrade)
            'Dim dt As DataTable = dbUtil.dbGetDataTable("EPRICER", "select a.GRADE_NAME from Price a where a.ORG='ATW' and a.GRADE_NAME is not null group by a.GRADE_NAME order by a.GRADE_NAME")
            Dim dt As DataTable = dbUtil.dbGetDataTable("EPRICER", "select a.PriceGrade as GRADE_NAME from Price a where a.ORG='ATW' and a.PriceGrade is not null group by a.PriceGrade order by a.PriceGrade")
            For Each r As DataRow In dt.Rows
                PGradeList.Add(New ePricerPriceGrade(r.Item("GRADE_NAME")))
            Next
            HttpContext.Current.Cache.Add("ATW Price Grade List", PGradeList, Nothing, Now.AddHours(12),
                                    System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If

        Dim plist = From q In PGradeList Where q.PriceGrade.StartsWith(prefixText) Or prefixText.StartsWith(q.PriceGrade) Take 10

        If plist.Count > 0 Then
            Dim str(plist.Count - 1) As String
            For i As Integer = 0 To plist.Count - 1
                str(i) = plist(i).PriceGrade
            Next
            Return str
        End If
        Return Nothing
    End Function

    Public Class ePricerPriceGrade
        Public Property PriceGrade As String
        Public Sub New(pg As String)
            Me.PriceGrade = UCase(Trim(pg))
        End Sub
    End Class
End Class
