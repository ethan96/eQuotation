Imports SAP.Connector
Imports System.Configuration
Public Class SAP : Implements iSAP


    Public Function getBTOChildDueDate(ByVal reqDate As String, ByVal org As String) As String Implements IBUS.iSAP.getBTOChildDueDate
        reqDate = CDate(reqDate).ToString("yyyy-MM-dd")
        Dim ws As New EXSY.SAPWS.B2B_AEU_WS
        ws.Timeout = -1

        Dim C As String = getCalendarbyOrg(Left(org, 2))
        ws.Get_Next_WorkingDate_ByCode(reqDate, "-" & getBTOWorkingDate(org), C)
        ws.Dispose()

        Return CDate(reqDate).ToString("yyyy/MM/dd")
    End Function
    Function getCalendarbyOrg(ByVal org As String) As String
        Dim plant As String = org & "H1"
        Dim str As String = String.Format("select LAND1 from saprdp.t001w where WERKS='{0}' and mandt='168' and rownum=1", plant)
        Dim CID As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
        If Not IsNothing(CID) AndAlso CID.ToString <> "" Then
            Return CID.ToString
        End If
        Return "TW"
    End Function
    Function getBTOWorkingDate(ByVal org As String) As COMM.Fixer.eBTOAssemblyDays Implements iSAP.getBTOWorkingDate
        If org = "US01" Then
            Return COMM.Fixer.eBTOAssemblyDays.US
        End If
        Return COMM.Fixer.eBTOAssemblyDays.EU
    End Function
    Public Function getBTOParentDueDate(ByVal reqDate As String, ByVal Org As String) As String Implements IBUS.iSAP.getBTOParentDueDate
        reqDate = CDate(reqDate).ToString("yyyy-MM-dd")
        Dim C As String = getCalendarbyOrg(Left(Org, 2))
        Get_Next_WorkingDate_ByCode(reqDate, getBTOWorkingDate(Org), C)
        Return CDate(reqDate).ToString("yyyy/MM/dd")
    End Function

    Public Function getCompNextWorkDate(ByVal reqDate As String, ByVal org As String, Optional ByVal days As Integer = 0) As String Implements IBUS.iSAP.getCompNextWorkDate
        reqDate = CDate(reqDate).ToString("yyyy-MM-dd")
        Dim ws As New EXSY.SAPWS.B2B_AEU_WS
        ws.Timeout = -1
        Dim C As String = getCalendarbyOrg(Left(org, 2))

        ws.Get_Next_WorkingDate_ByCode(reqDate, days, C)
        ws.Dispose()
        Return CDate(reqDate).ToString("yyyy/MM/dd")
    End Function

    Public Function GetNextWeeklyShippingDate(ByVal reqDate As Date, ByRef NextWeeklyShipDate As Date, ByVal CompanyId As String) As Boolean Implements IBUS.iSAP.GetNextWeeklyShippingDate
        If DateDiff(DateInterval.Day, reqDate, Now) = 0 Then reqDate = DateAdd(DateInterval.Day, 1, Now)
        NextWeeklyShipDate = reqDate
        Dim sql As New System.Text.StringBuilder
        sql.AppendLine(" select rtrim(SOAB1)+rtrim(SOBI1)+rtrim(SOAB2)+rtrim(SOBI2)  as Sunday, ")
        sql.AppendLine(" rtrim(MOAB1)+rtrim(MOBI1)+rtrim(MOAB2)+rtrim(MOBI2)  as Monday, ")
        sql.AppendLine(" rtrim(DIAB1)+rtrim(DIBI1)+rtrim(DIAB2)+rtrim(DIBI2)  as Tuesday, ")
        sql.AppendLine(" rtrim(MIAB1)+rtrim(MIBI1)+rtrim(MIAB2)+rtrim(MIBI2)  as Wednesday, ")
        sql.AppendLine(" rtrim(DOAB1)+rtrim(DOBI1)+rtrim(DOAB2)+rtrim(DOBI2)  as Thursday, ")
        sql.AppendLine(" rtrim(FRAB1)+rtrim(FRBI1)+rtrim(FRAB2)+rtrim(FRBI2)  as Friday, ")
        sql.AppendLine(" rtrim(SAAB1)+rtrim(SABI1)+rtrim(SAAB2)+rtrim(SABI2)  as Saturday ")
        sql.AppendLine(" from SAP_COMPANY_CALENDAR ")
        sql.AppendLine(String.Format(" where KUNNR='{0}'", CompanyId))
        Dim Dt_sap_company_calendar As DataTable = dbUtil.dbGetDataTable("MY", sql.ToString)
        If Dt_sap_company_calendar.Rows.Count > 0 Then
            Dim intOnOffWeekDays() As Integer = {0, 0, 0, 0, 0, 0, 0}, blHasValue As Boolean = False
            For i As Integer = 0 To 6
                If Not Dt_sap_company_calendar.Rows(0).Item(i).ToString().Equals("000000000000000000000000") Then
                    intOnOffWeekDays(i) = 1 : blHasValue = True
                End If
            Next
            If blHasValue = False Then
                Return False
            Else
                Dim intPlusDays As Integer = 0
                While intPlusDays < 7
                    Dim tmpDate As Date = DateAdd(DateInterval.Day, intPlusDays, reqDate)
                    If intOnOffWeekDays(DatePart(DateInterval.Weekday, tmpDate) - 1) = 1 Then
                        NextWeeklyShipDate = tmpDate
                        Return True
                    End If
                    intPlusDays += 1
                End While
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Public Shared Function Get_Next_WorkingDate_ByCode(ByRef iATPDate As String, ByVal Loading_Days As String, ByVal code As String) As Integer
        code = UCase(code)
        Dim proxy1 As New Factory_Date_Conversion.Factory_Date_Conversion
        Dim factory_date_Number As Decimal

        Dim provider1 As New System.Globalization.CultureInfo("fr-FR", True)
        Dim time1 As DateTime = DateTime.ParseExact(iATPDate, "yyyy-mm-dd", provider1)
        iATPDate = time1.ToString("yyyymmdd")
        'iATPDate = Replace(iATPDate, "/", "")

        Try
            proxy1.Connection = New SAPConnection(System.Configuration.ConfigurationManager.AppSettings("SAP_PRD").ToString)
            proxy1.Connection.Open()

            proxy1.Date_Convert_To_Factorydate("+", code, factory_date_Number, "", iATPDate)
            proxy1.Factorydate_Convert_To_Date(code, (factory_date_Number + Loading_Days), iATPDate)

            proxy1.Connection.Close()
            Dim time2 As DateTime = DateTime.ParseExact(iATPDate, "yyyymmdd", provider1)
            iATPDate = time2.ToString("yyyy-mm-dd")

        Catch ex As Exception
            iATPDate = ex.ToString()
            Return -1
            Exit Function

        End Try
        Return 1

    End Function

    Public Function getNextCustDelDate(ByVal ddate As String, ByVal CompanyId As String) As String Implements IBUS.iSAP.getNextCustDelDate
        Dim sql As New System.Text.StringBuilder
        sql.AppendLine("select rtrim(MOAB1)+rtrim(MOBI1)+rtrim(MOAB2)+rtrim(MOBI2)  as Monday,")
        sql.AppendLine("rtrim(DIAB1)+rtrim(DIBI1)+rtrim(DIAB2)+rtrim(DIBI2)  as Tuesday,")
        sql.AppendLine("rtrim(MIAB1)+rtrim(MIBI1)+rtrim(MIAB2)+rtrim(MIBI2)  as Wednesday,")
        sql.AppendLine("rtrim(DOAB1)+rtrim(DOBI1)+rtrim(DOAB2)+rtrim(DOBI2)  as Thursday,")
        sql.AppendLine("rtrim(FRAB1)+rtrim(FRBI1)+rtrim(FRAB2)+rtrim(FRBI2)  as Friday,")
        sql.AppendLine("rtrim(SAAB1)+rtrim(SABI1)+rtrim(SAAB2)+rtrim(SABI2)  as Saturday,")
        sql.AppendLine("rtrim(SOAB1)+rtrim(SOBI1)+rtrim(SOAB2)+rtrim(SOBI2)  as Sunday")
        sql.AppendLine("from SAP_COMPANY_CALENDAR")
        sql.AppendLine(String.Format("where KUNNR='{0}'", CompanyId))
        Dim Dt As New DataTable
        Dt = dbUtil.dbGetDataTable("B2B", sql.ToString)
        If Dt.Rows.Count > 0 Then
            Dim n As Integer = 0
            For i As Integer = 0 To 6
                If CDate(ddate).DayOfWeek = DayOfWeek.Sunday Then
                    n = (7 - 1 + i) Mod 7
                Else
                    n = (CInt(CDate(ddate).DayOfWeek) - 1 + i) Mod 7
                End If

                If Dt.Rows(0).Item(n).ToString.Trim("0").Trim <> "" Then
                    ddate = DateAdd(DateInterval.Day, i, CDate(ddate))
                    Return ddate
                End If
            Next
        End If
        Return ddate
    End Function
    Private _errCode As COMM.Msg.eErrCode = Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property
    Public Shared Function GetSAPPartnerAddressesTableByKunnr(ByVal Kunnr As String, Optional ByVal IsSAPProductionServer As Boolean = True) As Temp.PartnerAddressesDataTable
        Dim retTable As New Temp.PartnerAddressesDataTable
        Dim strSql As String = String.Format( _
          " Select * FROM " + _
            " (select a.kunnr as PARTN_NUMB,a.anred as TITLE, a.NAME1 as NAME, a.NAME2 as NAME_2, a.STRAS as STREET, " + _
            " a.LAND1 as COUNTRY, a.LAND1 as COUNTRY_ISO, a.PSTLZ as POSTL_CODE, '' as POBX_PCD, ''as POBX_CTY, " + _
            " a.TELF2 as TELEPHONE2, a.TELBX as TELEBOX, a.TELFX as FAX_NUMBER,  a.TELF1 as TELEPHONE, " + _
            " a.TELTX as TELEX_NO, a.SPRAS as LANGU, '' as LANGU_ISO, '' as UNLOAD_PT,a.REGIO as REGION, " + _
            " '' as ADDRESS, '' as PRIV_ADDR, 1 as ADDR_TYPE, '' as ADDR_ORIG, '' as ADDR_LINK, '' as REFOBJTYPE,'' as REFOBJKEY, '' as REFLOGSYS, " + _
            " a.ADRNR as ADRNR from saprdp.kna1 a where a.KUNNR='{0}') T " + _
            " Left Join " + _
            " (select " + _
            " b.NAME3 as NAME_3, b.NAME4 as NAME_4, " + _
            " b.CITY1 as CITY, b.CITY2 as DISTRICT, b.CITY_CODE, b.CITYP_CODE as Distrct_No, b.PO_BOX as PO_BOX, " + _
            " b.TEL_EXTENS, " + _
            " b.TRANSPZONE, b.TAXJURCODE,  " + _
            " b.name_co as Attention, b.time_zone, b.deflt_comm, b.addrnumber, b.BUILDING, b.DONT_USE_P, b.DONT_USE_S, " + _
            " b.FAX_EXTENS, b.FLOOR, b.HOUSE_NUM1, b.HOUSE_NUM2, b.HOUSE_NUM3, b.PO_BOX_NUM, b.PO_BOX_CTY, b.PO_BOX_REG, b.HOME_CITY, b.CITYH_CODE, " + _
            " b.POST_CODE1, b.POST_CODE2, b.POST_CODE3, b.REGIOGROUP, b.ROOMNUMBER, b.STR_SUPPL1, b.STR_SUPPL2, b.STR_SUPPL3, b.STREETCODE, b.LOCATION " + _
            " from saprdp.adrc b where b.addrnumber=(select adrnr from saprdp.kna1 a where a.kunnr='{0}' and rownum=1)) M " + _
            " on T.ADRNR=M.addrnumber", Kunnr)
        Dim ConnectionName As String = "SAP_PRD"
        If Not IsSAPProductionServer Then
            ConnectionName = "SAP_Test"
        End If
        Dim dt As DataTable = OraDbUtil.dbGetDataTable(ConnectionName, strSql)
        For Each r As DataRow In dt.Rows
            Dim S_PartnerAddressesRow As Temp.PartnerAddressesRow = retTable.NewPartnerAddressesRow()
            With S_PartnerAddressesRow
                .C_O_Name = r.Item("Attention") : .Addr_No = r.Item("addrnumber") : .Adr_Notes = ""
                .Build_Long = "" : .Building = r.Item("BUILDING") : .Chckstatus = ""
                .City = r.Item("CITY") : .City_No = r.Item("CITY_CODE") : .Comm_Type = r.Item("deflt_comm")
                .Country = r.Item("COUNTRY") : .Countryiso = "" : .Deliv_Dis = ""
                .Distrct_No = r.Item("Distrct_No") : .District = r.Item("DISTRICT") : .Dont_Use_P = r.Item("DONT_USE_P")
                .Dont_Use_S = r.Item("DONT_USE_S") : .E_Mail = "" : .Fax_Extens = r.Item("FAX_EXTENS")
                .Fax_Number = r.Item("FAX_NUMBER") : .Floor = r.Item("FLOOR") : .Formofaddr = ""
                .Home_City = r.Item("Home_City") : .Homecityno = r.Item("cityh_code") : .Homepage = ""
                .House_No = r.Item("HOUSE_NUM1") : .House_No2 = r.Item("HOUSE_NUM2") : .House_No3 = r.Item("HOUSE_NUM3")
                .Langu = r.Item("LANGU") : .Langu_Cr = "" : .Langu_Iso = "" : .Langucriso = ""
                .Location = r.Item("LOCATION") : .Name = r.Item("NAME") : .Name_2 = r.Item("NAME_2")
                .Name_3 = r.Item("NAME_3") : .Name_4 = r.Item("NAME_4") : .Pboxcit_No = r.Item("PO_BOX_NUM")
                .Pcode1_Ext = "" : .Pcode2_Ext = "" : .Pcode3_Ext = "" : .Po_Box = r.Item("PO_BOX")
                .Po_Box_Cit = r.Item("PO_BOX_CTY") : .Po_Box_Reg = r.Item("PO_BOX_REG") : .Po_Ctryiso = ""
                .Po_W_O_No = "" : .Pobox_Ctry = ""
                .Postl_Cod1 = r.Item("POST_CODE1") : .Postl_Cod2 = r.Item("POST_CODE2") : .Postl_Cod3 = r.Item("POST_CODE3")
                .Regiogroup = r.Item("REGIOGROUP") : .Region_str = r.Item("REGION") : .Room_No = r.Item("ROOMNUMBER")
                .Sort1 = "" : .Sort2 = "" : .Str_Abbr = "" : .Str_Suppl1 = r.Item("STR_SUPPL1") : .Str_Suppl2 = r.Item("STR_SUPPL2")
                .Str_Suppl3 = r.Item("STR_SUPPL3") : .Street = r.Item("STREET") : .Street_Lng = ""
                .Street_No = r.Item("STREETCODE") : .Taxjurcode = r.Item("TAXJURCODE") : .Tel1_Ext = r.Item("TEL_EXTENS")
                .Tel1_Numbr = r.Item("TELEPHONE") : .Time_Zone = r.Item("time_zone") : .Title = r.Item("title")
                .Transpzone = r.Item("TRANSPZONE")
            End With
            retTable.AddPartnerAddressesRow(S_PartnerAddressesRow)
        Next
        Return retTable
    End Function
    Public Function UpdateSAPSOShipToAttentionAddress(ByVal SONO As String, ByVal ShipToId As String, ByVal Name As String, ByVal Attention As String, ByVal Street As String, ByVal Street2 As String, ByVal City As String, ByVal State As String, ByVal Zipcode As String, ByVal Country As String, ByRef ReturnTable As System.Data.DataTable, Optional ByVal IsSAPProductionServer As Boolean = True) As Boolean Implements IBUS.iSAP.UpdateSAPSOShipToAttentionAddress
        Dim retbool As Boolean = False
        Dim dtDefaultAddrTable As Temp.PartnerAddressesDataTable = GetSAPPartnerAddressesTableByKunnr(ShipToId, IsSAPProductionServer)
        If dtDefaultAddrTable.Count > 0 Then
            Dim dtDefaultAddrRow = dtDefaultAddrTable(0)
            Dim p1 As New Change_SD_Order.Change_SD_Order()
            If IsSAPProductionServer Then
                p1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            Else
                p1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAPConnTest"))
            End If
            Dim OrderHeader As New Change_SD_Order.BAPISDH1, OrderHeaderX As New Change_SD_Order.BAPISDH1X
            Dim ItemIn As New Change_SD_Order.BAPISDITMTable, PartNr As New Change_SD_Order.BAPIPARNRTable
            Dim Condition As New Change_SD_Order.BAPICONDTable, ScheLine As New Change_SD_Order.BAPISCHDLTable
            Dim ScheLineX As New Change_SD_Order.BAPISCHDLXTable, OrderText As New Change_SD_Order.BAPISDTEXTTable
            Dim sales_note As New Change_SD_Order.BAPISDTEXT, ext_note As New Change_SD_Order.BAPISDTEXT
            Dim op_note As New Change_SD_Order.BAPISDTEXT, retTable As New Change_SD_Order.BAPIRET2Table
            Dim ADDRTable As New Change_SD_Order.BAPIADDR1Table, PartnerChangeTable As New Change_SD_Order.BAPIPARNRCTable
            Dim Doc_Number As String = SONO
            OrderHeaderX.Updateflag = "U"

            Dim ADDRRow1 As New Change_SD_Order.BAPIADDR1, PartnerChangeRow1 As New Change_SD_Order.BAPIPARNRC
            With ADDRRow1
                .Name = dtDefaultAddrRow.Name
                If Not String.IsNullOrEmpty(Name) Then
                    .Name = Name
                End If
                .Addr_No = "1" : .C_O_Name = Attention
                If String.IsNullOrEmpty(City) Then
                    .City = dtDefaultAddrRow.City
                Else
                    .City = City
                End If
                If String.IsNullOrEmpty(Country) Then
                    .Country = dtDefaultAddrRow.Country
                Else
                    .Country = Country
                End If
                If String.IsNullOrEmpty(Zipcode) Then
                    .Postl_Cod1 = dtDefaultAddrRow.Postl_Cod1
                Else
                    .Postl_Cod1 = Zipcode
                End If
                If String.IsNullOrEmpty(Street) Then
                    .Street = dtDefaultAddrRow.Street
                Else
                    .Street = Street
                End If
                If String.IsNullOrEmpty(Street2) Then
                    .Str_Suppl3 = dtDefaultAddrRow.Str_Suppl3
                Else
                    .Str_Suppl3 = Street2
                End If
                If String.IsNullOrEmpty(State) Then
                    .Region = dtDefaultAddrRow.Region_str
                Else
                    .Region = State
                End If
                .Comm_Type = dtDefaultAddrRow.Comm_Type : .Distrct_No = dtDefaultAddrRow.Distrct_No : .District = dtDefaultAddrRow.District
                .Dont_Use_P = dtDefaultAddrRow.Dont_Use_P : .Dont_Use_S = dtDefaultAddrRow.Dont_Use_S : .E_Mail = dtDefaultAddrRow.E_Mail
                .Fax_Extens = dtDefaultAddrRow.Fax_Extens : .Fax_Number = dtDefaultAddrRow.Fax_Number : .Floor = dtDefaultAddrRow.Floor
                .Langu = dtDefaultAddrRow.Langu : .Location = dtDefaultAddrRow.Location : .Name_2 = dtDefaultAddrRow.Name_2
                .Name_3 = dtDefaultAddrRow.Name_3 : .Name_4 = dtDefaultAddrRow.Name_4 : .Pboxcit_No = dtDefaultAddrRow.Pboxcit_No : .Pcode1_Ext = dtDefaultAddrRow.Pcode1_Ext
                .Pcode2_Ext = dtDefaultAddrRow.Pcode2_Ext : .Pcode3_Ext = dtDefaultAddrRow.Pcode3_Ext : .Po_Box = dtDefaultAddrRow.Po_Box
                .Po_Box_Cit = dtDefaultAddrRow.Po_Box_Cit : .Po_Box_Reg = dtDefaultAddrRow.Po_Box_Reg : .Pobox_Ctry = dtDefaultAddrRow.Pobox_Ctry
                .Postl_Cod2 = dtDefaultAddrRow.Postl_Cod2 : .Postl_Cod3 = dtDefaultAddrRow.Postl_Cod3 : .Regiogroup = dtDefaultAddrRow.Regiogroup
                .Taxjurcode = dtDefaultAddrRow.Taxjurcode : .Tel1_Ext = dtDefaultAddrRow.Tel1_Ext : .Tel1_Numbr = dtDefaultAddrRow.Tel1_Numbr
                .Time_Zone = dtDefaultAddrRow.Time_Zone : .Title = dtDefaultAddrRow.Title : .Transpzone = dtDefaultAddrRow.Transpzone
            End With
            With PartnerChangeRow1
                .Document = Doc_Number : .Addr_Link = "1" : .Address = "" : .P_Numb_New = ShipToId : .P_Numb_Old = ShipToId : .Partn_Role = "WE" : .Updateflag = "U"
            End With

            ADDRTable.Add(ADDRRow1) : PartnerChangeTable.Add(PartnerChangeRow1)
            Try
                p1.Connection.Open()
                p1.Bapi_Salesorder_Change("", "", New Change_SD_Order.BAPISDLS, OrderHeader, OrderHeaderX, Doc_Number, "", Condition, _
                    New Change_SD_Order.BAPICONDXTable, New Change_SD_Order.BAPIPAREXTable, New Change_SD_Order.BAPICUBLBTable, _
                    New Change_SD_Order.BAPICUINSTable, New Change_SD_Order.BAPICUPRTTable, New Change_SD_Order.BAPICUCFGTable, _
                    New Change_SD_Order.BAPICUREFTable, New Change_SD_Order.BAPICUVALTable, New Change_SD_Order.BAPICUVKTable, ItemIn, _
                    New Change_SD_Order.BAPISDITMXTable, New Change_SD_Order.BAPISDKEYTable, OrderText, ADDRTable, _
                    PartnerChangeTable, PartNr, retTable, ScheLine, ScheLineX)
                p1.CommitWork() : p1.Connection.Close()
                retbool = True
            Catch ex As Exception
            End Try
            ReturnTable = retTable.ToADODataTable()
            Return retbool
        End If
        Return True
    End Function

    Public Function UpdateSOSpecId(ByVal SO_NO As String, ByVal SpecialIndicator As COMM.Fixer.eEarlyShipment, ByRef ReturnTable As System.Data.DataTable) As Boolean Implements IBUS.iSAP.UpdateSOSpecId
        Dim strSpecialIndicator As String = "000" + CInt(SpecialIndicator).ToString()
        Dim p1 As New Change_SD_Order.Change_SD_Order()
        p1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        Dim OrderHeader As New Change_SD_Order.BAPISDH1, OrderHeaderX As New Change_SD_Order.BAPISDH1X
        Dim ItemIn As New Change_SD_Order.BAPISDITMTable, ItemInX As New Change_SD_Order.BAPISDITMXTable
        Dim PartNr As New Change_SD_Order.BAPIPARNRTable
        Dim Condition As New Change_SD_Order.BAPICONDTable, ScheLine As New Change_SD_Order.BAPISCHDLTable
        Dim ScheLineX As New Change_SD_Order.BAPISCHDLXTable, OrderText As New Change_SD_Order.BAPISDTEXTTable
        Dim sales_note As New Change_SD_Order.BAPISDTEXT, ext_note As New Change_SD_Order.BAPISDTEXT
        Dim op_note As New Change_SD_Order.BAPISDTEXT, retTable As New Change_SD_Order.BAPIRET2Table
        Dim ADDRTable As New Change_SD_Order.BAPIADDR1Table, PartnerChangeTable As New Change_SD_Order.BAPIPARNRCTable
        Dim Doc_Number As String = SO_NO
        OrderHeader.S_Proc_Ind = strSpecialIndicator
        OrderHeaderX.S_Proc_Ind = "X"
        OrderHeaderX.Updateflag = "U"
        p1.Connection.Open()
        p1.Bapi_Salesorder_Change("", "", New Change_SD_Order.BAPISDLS, OrderHeader, OrderHeaderX, Doc_Number, "", Condition, _
            New Change_SD_Order.BAPICONDXTable, New Change_SD_Order.BAPIPAREXTable, New Change_SD_Order.BAPICUBLBTable, _
            New Change_SD_Order.BAPICUINSTable, New Change_SD_Order.BAPICUPRTTable, New Change_SD_Order.BAPICUCFGTable, _
            New Change_SD_Order.BAPICUREFTable, New Change_SD_Order.BAPICUVALTable, New Change_SD_Order.BAPICUVKTable, ItemIn, _
            New Change_SD_Order.BAPISDITMXTable, New Change_SD_Order.BAPISDKEYTable, OrderText, ADDRTable, _
            PartnerChangeTable, PartNr, retTable, ScheLine, ScheLineX)
        p1.CommitWork()
        p1.Connection.Close()
        ReturnTable = retTable.ToADODataTable()
        For Each RetRow As DataRow In ReturnTable.Rows
            If RetRow.Item("Type").ToString().Equals("E") Then Return False
        Next
        Return True
    End Function

    Public Function UpdateSOZeroPriceItems(ByVal SO_NO As String, ByVal C As IBUS.iCartList, ByRef ReturnTable As System.Data.DataTable) As Boolean Implements IBUS.iSAP.UpdateSOZeroPriceItems

        If IsNothing(C) OrElse C.Count = 0 Then
            Return False
        End If
        Dim p1 As New Change_SD_Order.Change_SD_Order()
        p1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        Dim OrderHeader As New Change_SD_Order.BAPISDH1, OrderHeaderX As New Change_SD_Order.BAPISDH1X
        Dim ItemIn As New Change_SD_Order.BAPISDITMTable, ItemInX As New Change_SD_Order.BAPISDITMXTable
        Dim PartNr As New Change_SD_Order.BAPIPARNRTable
        Dim Condition As New Change_SD_Order.BAPICONDTable, ScheLine As New Change_SD_Order.BAPISCHDLTable
        Dim ScheLineX As New Change_SD_Order.BAPISCHDLXTable, OrderText As New Change_SD_Order.BAPISDTEXTTable
        Dim sales_note As New Change_SD_Order.BAPISDTEXT, ext_note As New Change_SD_Order.BAPISDTEXT
        Dim op_note As New Change_SD_Order.BAPISDTEXT, retTable As New Change_SD_Order.BAPIRET2Table
        Dim ADDRTable As New Change_SD_Order.BAPIADDR1Table, PartnerChangeTable As New Change_SD_Order.BAPIPARNRCTable
        Dim Doc_Number As String = SO_NO
        OrderHeaderX.Updateflag = "U"

        For Each OrderDetailRow As iCartLine In C
            If OrderDetailRow.newunitPrice.Value = 0 AndAlso Not OrderDetailRow.partNo.Value.EndsWith("-BTO") AndAlso Not OrderDetailRow.partNo.Value.StartsWith("C-CTOS") Then
                Dim ItemInRow As New Change_SD_Order.BAPISDITM, ItemInRowX As New Change_SD_Order.BAPISDITMX
                With ItemInRow
                    .Itm_Number = OrderDetailRow.lineNo.Value
                    .Material = Util.Format2SAPItem(OrderDetailRow.partNo.Value)
                    .Item_Categ = "ZTN3"
                End With
                With ItemInRowX
                    .Itm_Number = OrderDetailRow.lineNo.Value
                    .Item_Categ = "U"
                End With

                ItemIn.Add(ItemInRow) : ItemInX.Add(ItemInRowX)
            End If
        Next
        If ItemIn.Count > 0 Then
            p1.Connection.Open()
            p1.Bapi_Salesorder_Change("", "", New Change_SD_Order.BAPISDLS, OrderHeader, OrderHeaderX, Doc_Number, "", Condition, _
                New Change_SD_Order.BAPICONDXTable, New Change_SD_Order.BAPIPAREXTable, New Change_SD_Order.BAPICUBLBTable, _
                New Change_SD_Order.BAPICUINSTable, New Change_SD_Order.BAPICUPRTTable, New Change_SD_Order.BAPICUCFGTable, _
                New Change_SD_Order.BAPICUREFTable, New Change_SD_Order.BAPICUVALTable, New Change_SD_Order.BAPICUVKTable, ItemIn, _
                New Change_SD_Order.BAPISDITMXTable, New Change_SD_Order.BAPISDKEYTable, OrderText, ADDRTable, _
                PartnerChangeTable, PartNr, retTable, ScheLine, ScheLineX)
            p1.CommitWork()
            p1.Connection.Close()

            ReturnTable = retTable.ToADODataTable()
            For Each RetRow As DataRow In ReturnTable.Rows
                If RetRow.Item("Type").ToString().Equals("E") Then Return False
            Next
        End If

        Return True
    End Function

    Public Function UpdateSOWarrantyFlagByTable(ByVal dt As System.Data.DataTable, ByRef S As String, ByVal retCode As Boolean) As Object Implements IBUS.iSAP.UpdateSOWarrantyFlagByTable
        Dim ws As New SAPWS.B2B_AEU_WS
        ws.Timeout = -1
        dt.TableName = ("ewUp")
        ws.UpdateSOWarrantyFlagByTable(dt, "", True)
        Return 1
    End Function

    Public Function VerifyDistChannelDivisionGroupOffice(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String, ByVal strDivision As String, ByVal OrderDocType As String, ByVal SalesGroup As String, ByVal SalesOffice As String, ByRef ReturnTable As System.Data.DataTable) As Boolean Implements IBUS.iSAP.VerifyDistChannelDivisionGroupOffice
        If String.IsNullOrEmpty(ShipToId) Then ShipToId = SoldToId
        SoldToId = Trim(UCase(SoldToId)) : ShipToId = Trim(UCase(ShipToId))
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        With OrderHeader
            .Doc_Type = OrderDocType.ToString() : .Sales_Org = Trim(UCase(Org)) : .Distr_Chan = strDistChann
            .Division = strDivision : .Sales_Grp = SalesGroup : .Sales_Off = SalesOffice
        End With
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = SoldToId : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = ShipToId
        Partners.Add(SoldTo) : Partners.Add(ShipTo)

        Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        item.Itm_Number = "1" : item.Material = SAPDAL.SAPDAL.GetAHighLevelItemForPricing(Org) : item.Req_Qty = 1 : ItemsIn.Add(item)
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
    Public Function UpdateFPLA(ByVal OrderNo As String) As Boolean

        Dim pAuthBlock As New ZSD_UPDATE_FPLA.ZSD_UPDATE_FPLA
        Try
            '20120726 TC: Try to sleep two seconds to see if this can tick authorization block successfully

            Dim DT_LOG As New ZSD_UPDATE_FPLA.SWF_LINESTable
            Dim dt As New DataTable
            pAuthBlock.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            pAuthBlock.Connection.Open()
            For i As Integer = 0 To 3
                Threading.Thread.Sleep(2000)
                pAuthBlock.Zsd_Update_Fpla("X", OrderNo, DT_LOG)
                dt = DT_LOG.ToADODataTable
                If dt.Rows.Count > 0 AndAlso dt.Rows(0).Item("Line").ToString.Contains("successful") Then
                    Exit For
                End If
            Next
            If dt.Rows.Count > 0 AndAlso (Not dt.Rows(0).Item("Line").ToString.Contains("successful")) Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(pAuthBlock) AndAlso Not IsNothing(pAuthBlock.Connection) Then
                pAuthBlock.Connection.Close()
            End If
        End Try

    End Function
    Public Function getInventoryAndATPTable(ByVal PartNo As String, _
                                             ByVal Plant As String, _
                                             ByVal reqQty As Integer, _
                                             Optional ByRef DueDate As String = "", _
                                             Optional ByRef Inventory As Integer = 0, _
                                             Optional ByRef ATPtable As DataTable = Nothing, _
                                             Optional ByVal reqDate As String = "", _
                                             Optional ByRef satisFlag As Integer = 1, _
                                             Optional ByRef qtyCanBeConfirm As Int64 = 0) As Integer Implements iSAP.getInventoryAndATPTable

        Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP
        p1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        p1.Connection.Open()
        Dim retDate As Date = DateAdd(DateInterval.Day, -1, Now), retQty As Integer = 0
        PartNo = COMM.Util.Format2SAPItem(Trim(UCase(PartNo)))
        Dim culQty As Integer = 0
        Dim retTb As New GET_MATERIAL_ATP.BAPIWMDVSTable, atpTb As New GET_MATERIAL_ATP.BAPIWMDVETable
        Dim rOfretTb As New GET_MATERIAL_ATP.BAPIWMDVS
        rOfretTb.Req_Qty = reqQty
        If reqDate <> "" AndAlso IsDate(reqDate) Then
            rOfretTb.Req_Date = CDate(reqDate).ToString(COMM.Fixer.eDateFormat.SAPDATE)
        End If
        retTb.Add(rOfretTb)
        p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", PartNo, UCase(Plant), "", "", "", "", "PC", "", Inventory, "", "", _
                                      New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)
        p1.Connection.Close()
        ATPtable = atpTb.ToADODataTable()
        If PartNo.ToUpper.StartsWith(COMM.Fixer.AGSEWPrefix) Then
            DueDate = Now.Date.ToString(COMM.Fixer.eDateFormat.DDMMYYYYDASH)
        Else
            If ATPtable.Rows.Count > 0 Then
                For Each r As DataRow In ATPtable.Rows
                    qtyCanBeConfirm += CType(r.Item("com_qty"), Int64)
                Next
                If qtyCanBeConfirm > 0 Then
                    DueDate = COMM.Util.DateFormat(ATPtable.Rows(ATPtable.Rows.Count - 1).Item("Com_Date").ToString, "YYYYMMDD", "YYYYMMDD", "", "-")
                Else
                    DueDate = COMM.Fixer.StartDate
                End If
            Else
                DueDate = COMM.Fixer.StartDate
            End If
        End If
        If DueDate = COMM.Fixer.StartDate Then
            DueDate = Now.Date.AddDays(getLeadTime(PartNo, Plant))
        End If
        If reqQty > qtyCanBeConfirm Then
            satisFlag = 0
        Else
            satisFlag = 1
        End If
        Return 1
    End Function
    Private Function getLeadTime(ByVal pn As String, ByVal plant As String) As Integer
        Dim N As Integer = 0
        Dim str As String = String.Format("select (PLANNED_DEL_TIME + GP_PROCESSING_TIME) from dbo.SAP_PRODUCT_ABC where PART_NO='{0}' AND PLANT='{1}'", pn, plant)
        Dim dt As New DataTable
        dt = dbUtil.dbGetDataTable("MY", str)
        If dt.Rows.Count > 0 Then
            N = dt.Rows(0).Item(0)
        End If
        Return N
    End Function
    'Nada: for order simulate
    Public Function OrderSimulate(ByVal Header As IBUS.iDocHeaderLine, _
                                  ByRef Partner As System.Collections.Generic.List(Of IBUS.iPartnerLine), _
                                  ByRef Ret As System.Data.DataTable, _
                                  Optional ByRef Items As IBUS.iCartList = Nothing, _
                                  Optional ByRef Credit As System.Collections.Generic.List(Of IBUS.iCreditLine) = Nothing, _
                                  Optional ByRef Condition As System.Collections.Generic.List(Of IBUS.iCondLine) = Nothing) As Boolean Implements IBUS.iSAP.OrderSimulate
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE
        Dim FF As Integer = 1
        Try
            Dim S_OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD
            Dim S_OrderLineDt As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable
            Dim S_PartnerFuncDT As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
            Dim S_ScheLineDT As New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable
            Dim S_CreditCardDT As New BAPI_SALESORDER_SIMULATE.BAPICCARDTable

            Dim O_ConditionDT As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
            Dim O_ScheLineDT As New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable
            Dim O_OrderLineDt As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
            Dim O_CreditCardDT As New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable

            Dim O_SOLDTOP As New BAPI_SALESORDER_SIMULATE.BAPISOLDTO
            Dim O_SHIPTOP As New BAPI_SALESORDER_SIMULATE.BAPISHIPTO
            Dim O_BILLTOP As New BAPI_SALESORDER_SIMULATE.BAPIPAYER

            Dim retTable As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table

            With Header
                S_OrderHeader.Doc_Type = .DocRealType
                S_OrderHeader.Sales_Org = .org
                S_OrderHeader.Distr_Chan = .DIST_CHAN
                S_OrderHeader.Division = .DIVISION
                If Not String.IsNullOrEmpty(.AccGroupCode) AndAlso Not String.IsNullOrEmpty(.AccOfficeCode) Then
                    S_OrderHeader.Sales_Grp = .AccGroupCode
                    S_OrderHeader.Sales_Off = .AccOfficeCode
                End If
                If Not String.IsNullOrEmpty(.OriginalQuoteID.Trim) Then
                    S_OrderHeader.Ref_Doc = .OriginalQuoteID
                End If
                S_OrderHeader.Price_Date = .DocDate.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                S_OrderHeader.Incoterms1 = .Inco1
                S_OrderHeader.Incoterms2 = IIf(String.IsNullOrEmpty(.Inco2), "blank", .Inco2)
                S_OrderHeader.Req_Date_H = .ReqDate.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                If Not String.IsNullOrEmpty(.shipTerm) AndAlso .shipTerm.Length > 2 Then
                    S_OrderHeader.Ship_Cond = Left(.shipTerm, 2)
                End If
                S_OrderHeader.Purch_No_C = IIf(String.IsNullOrEmpty(.PO_NO), .Key, .PO_NO)
                S_OrderHeader.Purch_Date = .DocDate.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                S_OrderHeader.Compl_Dlv = IIf(CInt(.PartialF) = 0, "X", .PartialF)
                If String.IsNullOrEmpty(.paymentTerm) = False Then
                    S_OrderHeader.Pmnttrms = .paymentTerm
                End If
            End With

            For Each r As IBUS.iPartnerLine In Partner
                Dim S_PartnerFuncRow As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
                With r
                    S_PartnerFuncRow.Partn_Role = .TYPE : S_PartnerFuncRow.Partn_Numb = .ERPID
                End With
                S_PartnerFuncDT.Add(S_PartnerFuncRow)
            Next

            If Not IsNothing(Items) Then
                For Each r As IBUS.iCartLine In Items
                    Dim S_OrderLineRow As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN, S_ScheLineRow As New BAPI_SALESORDER_SIMULATE.BAPISCHDL
                    With r
                        S_OrderLineRow.Part_Dlv = ""
                        S_OrderLineRow.Hg_Lv_Item = .parentLineNo.Value
                        S_OrderLineRow.Itm_Number = .lineNo.Value
                        S_OrderLineRow.Dlv_Group = .DELIVERYGROUP.Value
                        S_OrderLineRow.Plant = .divPlant.Value : S_OrderLineRow.Material = COMM.Util.Format2SAPItem(.partNo.Value)
                        S_OrderLineRow.Cust_Mat35 = .CustMaterial.Value
                        S_ScheLineRow.Itm_Number = .lineNo.Value : S_ScheLineRow.Req_Qty = .Qty.Value : S_ScheLineRow.Req_Date = .reqDate.Value.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                        S_OrderLineRow.Short_Text = .partDesc.Value
                        S_OrderLineRow.Ship_Point = .ShipPoint.Value : S_OrderLineRow.Store_Loc = .StorageLoc.Value
                    End With
                    S_OrderLineDt.Add(S_OrderLineRow) : S_ScheLineDT.Add(S_ScheLineRow)
                Next
            End If
           
            If Not IsNothing(Credit) Then
                For Each r As IBUS.iCreditLine In Credit
                    Dim S_CreditCardRow As New BAPI_SALESORDER_SIMULATE.BAPICCARD
                    With r
                        S_CreditCardRow.Cc_Name = .HOLDER : S_CreditCardRow.Cc_Number = .NUMBER : S_CreditCardRow.Cc_Type = .TYPE
                        S_CreditCardRow.Cc_Valid_T = .EXPIRED 'S_CreditCardRow.Cc_Verif_Value = .VERIFICATION_VALUE
                    End With
                    S_CreditCardDT.Add(S_CreditCardRow)
                Next
            End If

            proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))

            proxy1.Connection.Open()
            Dim strRelationType As String = ""
            Dim strPConvert As String = ""
            Dim strpintnumassign As String = "", strPTestRun As String = ""

            proxy1.Bapi_Salesorder_Simulate("", S_OrderHeader, O_BILLTOP, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "", _
                                                 O_SHIPTOP, O_SOLDTOP, _
                                                 New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retTable, _
                                                 S_CreditCardDT, O_CreditCardDT, _
                                                 New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
                                                 New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, _
                                                 New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, O_ConditionDT, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, _
                                                 S_OrderLineDt, O_OrderLineDt, S_PartnerFuncDT, O_ScheLineDT, _
                                                 S_ScheLineDT, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)
            COMM.Util.showDT(O_ConditionDT.ToADODataTable)
            COMM.Util.showDT(O_ScheLineDT.ToADODataTable)
            COMM.Util.showDT(O_OrderLineDt.ToADODataTable)
            COMM.Util.showDT(O_CreditCardDT.ToADODataTable)
            COMM.Util.showDT(retTable.ToADODataTable())

            Ret = retTable.ToADODataTable()
            For Each retMsgRec As DataRow In Ret.Rows
                If retMsgRec.Item("Type") = "E" Then
                    FF = 0
                    Exit For
                End If
            Next
            If FF = 1 Then
                'Return
                If Not IsNothing(Items) Then
                    For Each R As IBUS.iCartLine In Items
                        For Each RSAP As BAPI_SALESORDER_SIMULATE.BAPIITEMEX In O_OrderLineDt
                            If CInt(R.lineNo.Value) = CInt(RSAP.Itm_Number) Then
                                R.listPrice.Value = 0
                                If IsNumeric(RSAP.Subtotal_1) Then
                                    R.listPrice.Value = RSAP.Subtotal_1 / RSAP.Req_Qty
                                End If
                                R.unitPrice.Value = 0
                                If IsNumeric(RSAP.Net_Value) Then
                                    R.unitPrice.Value = RSAP.Net_Value / RSAP.Req_Qty
                                End If
                                If R.unitPrice.Value > R.listPrice.Value Then
                                    R.listPrice.Value = R.unitPrice.Value
                                End If
                                R.newunitPrice.Value = R.unitPrice.Value
                                R.divPlant.Value = RSAP.Plant
                                R.dueDate.Value = COMM.Util.DateFormat(IIf(RSAP.Dlv_Date.Trim("0") = "", CDate(COMM.Fixer.StartDate).ToString("yyyyMMdd"), RSAP.Dlv_Date), "YYYYMMDD", "YYYYMMDD", "", "-")
                                R.CustMaterial.Value = RSAP.Cust_Mat
                                R.satisfyflag.Value = 1
                                If CDate(R.dueDate.Value) = CDate(COMM.Fixer.StartDate) Then
                                    R.satisfyflag.Value = 0
                                End If
                            End If
                        Next
                    Next
                End If
                'Return : will be completed while we need

                'For Each R As IBUS.iPartnerLine In Partner

                'Next
                If Not IsNothing(Credit) Then
                    For Each R As IBUS.iCreditLine In Credit
                        For Each RSAP As BAPI_SALESORDER_SIMULATE.BAPICCARD_EX In O_CreditCardDT
                            If CInt(R.NUMBER) = CInt(RSAP.Cc_Number) Then
                                R.EXPIRED = RSAP.Cc_Valid_T
                            End If
                        Next
                    Next
                End If
      
                '
                '/Return : will be completed while we need
                Return True
            Else
                Return False
            End If
        Catch mex As Exception
            Throw mex
        Finally
            If Not IsNothing(proxy1) AndAlso Not IsNothing(proxy1.Connection) Then
                proxy1.Connection.Close()
            End If
        End Try
    End Function
    'Nada: for order create
    Private Function OrderCreate(ByVal Header As IBUS.iDocHeaderLine,
                                 ByVal Partner As System.Collections.Generic.List(Of IBUS.iPartnerLine), _
                                 ByRef Ret As DataTable, ByRef errMsg As String, _
                                 Optional ByVal Items As IBUS.iCartList = Nothing, _
                                 Optional ByVal HeaderText As System.Collections.Generic.List(Of IBUS.iDocTextLine) = Nothing, _
                                 Optional ByVal Condition As System.Collections.Generic.List(Of IBUS.iCondLine) = Nothing, _
                                 Optional ByVal Credit As System.Collections.Generic.List(Of IBUS.iCreditLine) = Nothing, _
                                 Optional ByVal isProduction As Boolean = False) As Boolean Implements IBUS.iSAP.OrderCreate

        Dim proxy1 As New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SALESORDER_CREATEFROMDAT2
        Dim FF As Integer = 0
        Try
            Dim S_OrderHeader As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1, S_OrderLineDt As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMTable
            Dim S_PartnerFuncDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNRTable, S_ConditionDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDTable
            Dim S_HeaderTextsDt As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXTTable, S_ScheLineDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLTable
            Dim S_CreditCardDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARDTable
            Dim retTable As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIRET2Table
            With Header
                S_OrderHeader.Doc_Type = .DocRealType
                S_OrderHeader.Sales_Org = .org
                S_OrderHeader.Distr_Chan = .DIST_CHAN
                S_OrderHeader.Division = .DIVISION
                If Not String.IsNullOrEmpty(.AccGroupCode) AndAlso Not String.IsNullOrEmpty(.AccOfficeCode) Then
                    S_OrderHeader.Sales_Grp = .AccGroupCode
                    S_OrderHeader.Sales_Off = .AccOfficeCode
                End If
                If Not String.IsNullOrEmpty(.OriginalQuoteID.Trim) Then
                    S_OrderHeader.Ref_Doc = .OriginalQuoteID
                    S_OrderHeader.Refdoc_Cat = "B"
                End If
                S_OrderHeader.Doc_Date = .DocDate.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                S_OrderHeader.Incoterms1 = .Inco1

                S_OrderHeader.Incoterms2 = IIf(String.IsNullOrEmpty(.Inco2), "blank", .Inco2)
                S_OrderHeader.Req_Date_H = .ReqDate.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                If Not String.IsNullOrEmpty(.shipTerm) AndAlso .shipTerm.Length > 2 Then
                    S_OrderHeader.Ship_Cond = Left(.shipTerm, 2)
                End If
                S_OrderHeader.Purch_No_C = IIf(String.IsNullOrEmpty(.PO_NO), .Key, .PO_NO)
                S_OrderHeader.Purch_Date = .DocDate.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                S_OrderHeader.Compl_Dlv = IIf(CInt(.PartialF) = 0, "X", .PartialF)

                S_OrderHeader.Taxdep_Cty = .TAXDEPCITY
                S_OrderHeader.Eutri_Deal = .TRIANGULARINDICATOR
                S_OrderHeader.Alttax_Cls = .TAXCLASS
                S_OrderHeader.Purch_No_S = .SHIPCUSTPONO
                S_OrderHeader.Taxdst_Cty = .TAXDSTCITY
                S_OrderHeader.S_Proc_Ind = "0001"

                If String.IsNullOrEmpty(.paymentTerm) = False Then
                    S_OrderHeader.Pmnttrms = .paymentTerm
                End If
            End With
            If Not IsNothing(Items) Then
                For Each r As IBUS.iCartLine In Items
                    Dim S_OrderLineRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITM, S_ScheLineRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDL, S_ConditionRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND
                    With r
                        S_OrderLineRow.Part_Dlv = ""
                        S_OrderLineRow.Hg_Lv_Item = .parentLineNo.Value
                        S_OrderLineRow.Itm_Number = .lineNo.Value
                        S_OrderLineRow.Dlv_Group = .DELIVERYGROUP.Value
                        S_OrderLineRow.Plant = .divPlant.Value : S_OrderLineRow.Material = COMM.Util.Format2SAPItem(.partNo.Value)
                        S_OrderLineRow.Cust_Mat35 = .CustMaterial.Value : S_OrderLineRow.Usage_Ind = .DMFFlag.Value
                        S_ScheLineRow.Itm_Number = .lineNo.Value : S_ScheLineRow.Req_Qty = .Qty.Value : S_ScheLineRow.Req_Date = .reqDate.Value.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                        S_ConditionRow.Itm_Number = .lineNo.Value : S_ConditionRow.Cond_Type = "ZPN0" : S_ConditionRow.Currency = Header.currency : S_ConditionRow.Cond_Value = .newunitPrice.Value
                        S_OrderLineRow.Short_Text = .partDesc.Value
                        S_OrderLineRow.Ship_Point = .ShipPoint.Value : S_OrderLineRow.Store_Loc = .StorageLoc.Value
                    End With
                    S_OrderLineDt.Add(S_OrderLineRow) : S_ScheLineDT.Add(S_ScheLineRow) : S_ConditionDT.Add(S_ConditionRow)
                Next
            End If
   

            For Each r As IBUS.iPartnerLine In Partner
                Dim S_PartnerFuncRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNR
                With r
                    S_PartnerFuncRow.Partn_Role = .TYPE : S_PartnerFuncRow.Partn_Numb = .ERPID
                End With
                S_PartnerFuncDT.Add(S_PartnerFuncRow)
            Next

            If Not IsNothing(HeaderText) Then
                For Each r As IBUS.iDocTextLine In HeaderText
                    With r
                        Dim StartP As Integer = 1, LongP As Integer = 100, oLine As String = Mid(.Txt, StartP, LongP)
                        While oLine.Trim.Length > 0
                            Dim S_HeaderTextsRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXT
                            S_HeaderTextsRow.Doc_Number = Header.Key : S_HeaderTextsRow.Text_Id = .Type
                            S_HeaderTextsRow.Text_Line = oLine : S_HeaderTextsRow.Langu = "EN" : S_HeaderTextsDt.Add(S_HeaderTextsRow)
                            StartP = StartP + 100 : oLine = Mid(.Txt, StartP, LongP)
                        End While
                    End With
                Next
            End If
     

            If Not IsNothing(Condition) Then
                For Each r As IBUS.iCondLine In Condition
                    Dim S_ConditionRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND
                    With r
                        S_ConditionRow.Itm_Number = "000000" : S_ConditionRow.Cond_Type = .Type : S_ConditionRow.Currency = Header.currency : S_ConditionRow.Cond_Value = .Value
                    End With
                    S_ConditionDT.Add(S_ConditionRow)
                Next
            End If
    


            If Not IsNothing(Credit) Then
                For Each r As IBUS.iCreditLine In Credit
                    Dim S_CreditCardRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARD
                    With r
                        S_CreditCardRow.Cc_Name = .HOLDER : S_CreditCardRow.Cc_Number = .NUMBER : S_CreditCardRow.Cc_Type = .TYPE
                        S_CreditCardRow.Cc_Valid_T = .EXPIRED : S_CreditCardRow.Cc_Verif_Value = .VERIFICATION_VALUE
                    End With
                    S_CreditCardDT.Add(S_CreditCardRow)
                Next
            End If
 


            If isProduction Then
                proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            Else
                proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAPConnTest"))
            End If
            proxy1.Connection.Open()
            Dim strRelationType As String = ""
            Dim strPConvert As String = ""
            Dim strpintnumassign As String = "", strPTestRun As String = ""

            proxy1.Bapi_Salesorder_Createfromdat2( _
            errMsg, strRelationType, strPConvert, _
            strpintnumassign, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDLS, S_OrderHeader, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1X, _
            Header.Key, New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SENDER, strPTestRun, Header.Key, _
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPAREXTable, S_CreditCardDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUBLBTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUINSTable, _
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUPRTTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUCFGTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUREFTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVALTable, _
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVKTable, S_ConditionDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDXTable, _
            S_OrderLineDt, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMXTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDKEYTable, _
            S_PartnerFuncDT, S_ScheLineDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLXTable, S_HeaderTextsDt, New BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1Table, retTable)
            Ret = retTable.ToADODataTable()
            For retRowCount = 0 To Ret.Rows.Count - 1
                If Ret.Rows(retRowCount).Item("Number") = "311" Then
                    FF = 1
                    Exit For
                End If
            Next
            If FF = 1 Then
                proxy1.CommitWork()
                UpdateFPLA(Header.Key)
                Return True
            Else
                Return False
            End If
        Catch mex As Exception
            Throw mex
        Finally
            If Not IsNothing(proxy1) AndAlso Not IsNothing(proxy1.Connection) Then
                proxy1.Connection.Close()
            End If
        End Try
    End Function
    'Nada: for QuotationCreate
    Private Function CreateQuotation(ByVal Header As IBUS.iDocHeaderLine, _
                                     ByVal Partner As System.Collections.Generic.List(Of IBUS.iPartnerLine), _
                                     ByRef Ret As DataTable, ByRef errMsg As String,
                                     Optional ByVal Items As IBUS.iCartList = Nothing, _
                                     Optional ByVal HeaderText As System.Collections.Generic.List(Of IBUS.iDocTextLine) = Nothing, _
                                     Optional ByVal Condition As System.Collections.Generic.List(Of IBUS.iCondLine) = Nothing, _
                                     Optional ByVal isProduction As Boolean = False) As Boolean Implements iSAP.CreateQuotation
        Dim proxy1 As New Quotation_Create_SAP.Quotation_Create_SAP
        Dim FF As Integer = 0
        Try
            Dim S_OrderHeader As New Quotation_Create_SAP.BAPISDHD1
            Dim S_OrderLineDt As New Quotation_Create_SAP.BAPISDITMTable
            Dim S_PartnerFuncDT As New Quotation_Create_SAP.BAPIPARNRTable
            Dim S_ConditionDT As New Quotation_Create_SAP.BAPICONDTable
            Dim S_HeaderTextsDt As New Quotation_Create_SAP.BAPISDTEXTTable
            Dim S_ScheLineDT As New Quotation_Create_SAP.BAPISCHDLTable
            Dim retTable As New Quotation_Create_SAP.BAPIRET2Table
            With Header
                S_OrderHeader.Doc_Type = .DocRealType
                S_OrderHeader.Sales_Org = .org
                S_OrderHeader.Distr_Chan = .DIST_CHAN
                S_OrderHeader.Division = .DIVISION
                If Not String.IsNullOrEmpty(.AccGroupCode) AndAlso Not String.IsNullOrEmpty(.AccOfficeCode) Then
                    S_OrderHeader.Sales_Grp = .AccGroupCode
                    S_OrderHeader.Sales_Off = .AccOfficeCode
                End If
                If Not String.IsNullOrEmpty(.OriginalQuoteID.Trim) Then
                    S_OrderHeader.Ref_Doc = .OriginalQuoteID
                    S_OrderHeader.Refdoc_Cat = "B"
                End If
                S_OrderHeader.Doc_Date = .DocDate.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                S_OrderHeader.Incoterms1 = .Inco1

                S_OrderHeader.Incoterms2 = IIf(String.IsNullOrEmpty(.Inco2), "blank", .Inco2)
                S_OrderHeader.Req_Date_H = .ReqDate.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                If Not String.IsNullOrEmpty(.shipTerm) AndAlso .shipTerm.Length > 2 Then
                    S_OrderHeader.Ship_Cond = Left(.shipTerm, 2)
                End If
                S_OrderHeader.Purch_No_C = IIf(String.IsNullOrEmpty(.PO_NO), .Key, .PO_NO)
                S_OrderHeader.Purch_Date = .DocDate.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                S_OrderHeader.Compl_Dlv = IIf(CInt(.PartialF) = 0, "X", .PartialF)

                S_OrderHeader.Taxdep_Cty = .TAXDEPCITY
                S_OrderHeader.Eutri_Deal = .TRIANGULARINDICATOR
                S_OrderHeader.Alttax_Cls = .TAXCLASS
                S_OrderHeader.Purch_No_S = .SHIPCUSTPONO
                S_OrderHeader.Taxdst_Cty = .TAXDSTCITY
                S_OrderHeader.S_Proc_Ind = "0001"

                If String.IsNullOrEmpty(.paymentTerm) = False Then
                    S_OrderHeader.Pmnttrms = .paymentTerm
                End If
            End With
            If Not IsNothing(Items) Then
                For Each r As IBUS.iCartLine In Items
                    Dim S_OrderLineRow As New Quotation_Create_SAP.BAPISDITM, S_ScheLineRow As New Quotation_Create_SAP.BAPISCHDL, S_ConditionRow As New Quotation_Create_SAP.BAPICOND
                    With r
                        S_OrderLineRow.Part_Dlv = ""
                        S_OrderLineRow.Hg_Lv_Item = .parentLineNo.Value
                        S_OrderLineRow.Itm_Number = .lineNo.Value
                        S_OrderLineRow.Dlv_Group = .DELIVERYGROUP.Value
                        S_OrderLineRow.Plant = .divPlant.Value : S_OrderLineRow.Material = COMM.Util.Format2SAPItem(.partNo.Value)
                        S_OrderLineRow.Cust_Mat35 = .CustMaterial.Value : S_OrderLineRow.Usage_Ind = .DMFFlag.Value
                        S_ScheLineRow.Itm_Number = .lineNo.Value : S_ScheLineRow.Req_Qty = .Qty.Value : S_ScheLineRow.Req_Date = .reqDate.Value.ToString(COMM.Fixer.eDateFormat.SAPDATE)
                        S_ConditionRow.Itm_Number = .lineNo.Value : S_ConditionRow.Cond_Type = "ZPN0" : S_ConditionRow.Currency = Header.currency : S_ConditionRow.Cond_Value = .newunitPrice.Value
                        S_OrderLineRow.Short_Text = .partDesc.Value
                        S_OrderLineRow.Ship_Point = .ShipPoint.Value : S_OrderLineRow.Store_Loc = .StorageLoc.Value
                    End With
                    S_OrderLineDt.Add(S_OrderLineRow) : S_ScheLineDT.Add(S_ScheLineRow) : S_ConditionDT.Add(S_ConditionRow)
                Next
            End If
     

            For Each r As IBUS.iPartnerLine In Partner
                Dim S_PartnerFuncRow As New Quotation_Create_SAP.BAPIPARNR
                With r
                    S_PartnerFuncRow.Partn_Role = .TYPE : S_PartnerFuncRow.Partn_Numb = .ERPID
                End With
                S_PartnerFuncDT.Add(S_PartnerFuncRow)
            Next

            If Not IsNothing(HeaderText) Then
                For Each r As IBUS.iDocTextLine In HeaderText
                    With r
                        Dim StartP As Integer = 1, LongP As Integer = 100, oLine As String = Mid(.Txt, StartP, LongP)
                        While oLine.Trim.Length > 0
                            Dim S_HeaderTextsRow As New Quotation_Create_SAP.BAPISDTEXT
                            S_HeaderTextsRow.Doc_Number = Header.Key : S_HeaderTextsRow.Text_Id = .Type
                            S_HeaderTextsRow.Text_Line = oLine : S_HeaderTextsRow.Langu = "EN" : S_HeaderTextsDt.Add(S_HeaderTextsRow)
                            StartP = StartP + 100 : oLine = Mid(.Txt, StartP, LongP)
                        End While
                    End With
                Next
            End If
  

            If Not IsNothing(Condition) Then
                For Each r As IBUS.iCondLine In Condition
                    Dim S_ConditionRow As New Quotation_Create_SAP.BAPICOND
                    With r
                        S_ConditionRow.Itm_Number = "000000" : S_ConditionRow.Cond_Type = .Type : S_ConditionRow.Currency = Header.currency : S_ConditionRow.Cond_Value = .Value
                    End With
                    S_ConditionDT.Add(S_ConditionRow)
                Next
            End If

            If isProduction Then
                proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            Else
                proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAPConnTest"))
            End If
            proxy1.Connection.Open()
            Dim strRelationType As String = ""
            Dim strPConvert As String = ""
            Dim strpintnumassign As String = "", strPTestRun As String = ""

            proxy1.Bapi_Quotation_Createfromdata2( _
            errMsg, strRelationType, strPConvert, _
            strpintnumassign, New Quotation_Create_SAP.BAPISDLS, S_OrderHeader, New Quotation_Create_SAP.BAPISDHD1X, _
            Header.Key, New Quotation_Create_SAP.BAPI_SENDER, strPTestRun, Header.Key, _
             New Quotation_Create_SAP.BAPIPAREXTable, New Quotation_Create_SAP.BAPIADDR1Table, New Quotation_Create_SAP.BAPICUBLBTable, New Quotation_Create_SAP.BAPICUINSTable, _
             New Quotation_Create_SAP.BAPICUPRTTable, New Quotation_Create_SAP.BAPICUCFGTable, New Quotation_Create_SAP.BAPICUREFTable, New Quotation_Create_SAP.BAPICUVALTable, _
             New Quotation_Create_SAP.BAPICUVKTable, S_ConditionDT, New Quotation_Create_SAP.BAPICONDXTable, _
             S_OrderLineDt, New Quotation_Create_SAP.BAPISDITMXTable, New Quotation_Create_SAP.BAPISDKEYTable, _
              S_PartnerFuncDT, S_ScheLineDT, New Quotation_Create_SAP.BAPISCHDLXTable, S_HeaderTextsDt, retTable)
            Ret = retTable.ToADODataTable()
            For retRowCount = 0 To Ret.Rows.Count - 1
                If Ret.Rows(retRowCount).Item("Number") = "311" Then
                    FF = 1
                    Exit For
                End If
            Next
            If FF = 1 Then
                proxy1.CommitWork()
                Return True
            Else
                Return False
            End If
        Catch mex As Exception
            Throw mex
        Finally
            If Not IsNothing(proxy1) AndAlso Not IsNothing(proxy1.Connection) Then
                proxy1.Connection.Close()
            End If
        End Try

    End Function



End Class
