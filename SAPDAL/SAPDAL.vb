Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports SAP.Connector
Imports Z_GET_ATP_LIMITQTY
Imports System.Configuration
Imports System.Globalization
Imports System.Web.Caching
Imports System.Linq
Imports SAP.Middleware.Connector
Imports System.Xml

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
Public Class SAPDAL
    Inherits System.Web.Services.WebService

    Public Shared strSAPDbConn As String = "user id=ebiz;password=ebiz;data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=172.20.1.166)(PORT=1527))(CONNECT_DATA=(SERVICE_NAME=RDP)))"


#Region "Enum Definitions"
    Public Enum EnumCompanyType
        Enum_Z001 ' Customer
        Enum_Z002 ' ShipTo
    End Enum

    Public Enum EnumIndustryCode
        Enum_1000 ' Taiwan
        Enum_2000 ' America
        Enum_3000 ' Europe
        Enum_4000 ' China
        Enum_5000 ' Asia - Others
        Enum_BRCT ' Brazil
        Enum_BRNC ' Non-Contribu.
    End Enum

    Public Enum EnumRegionWestEast
        Enum_0000000001 ' East
        Enum_0000000002 ' West
    End Enum

    Public Enum EnumCustomerClass
        Enum_01 'AXSC
        Enum_02 'RBU
        Enum_03 'External
        Enum_04 'Joint Venture
    End Enum

    Public Enum EnumCreditTerm
        Enum_NONE
        Enum_07D4
        Enum_10D1
        Enum_10D2
        Enum_10D5
        Enum_15D1
        Enum_15D2
        Enum_15D5
        Enum_30D3
        Enum_CN01
        Enum_CN02
        Enum_CN04
        Enum_CN05
        Enum_CN07
        Enum_CN10
        Enum_CN15
        Enum_COD
        Enum_CODC
        Enum_CODM
        Enum_EC30
        Enum_ECBD
        Enum_ECBO
        Enum_ECOB
        Enum_ECOO
        Enum_I001
        Enum_I007
        Enum_I010
        Enum_I014
        Enum_I015
        Enum_I021
        Enum_I028
        Enum_I030
        Enum_I035
        Enum_I045
        Enum_I060
        Enum_I070
        Enum_I075
        Enum_I090
        Enum_I120
        Enum_LC00
        Enum_M014
        Enum_M015
        Enum_M025
        Enum_M030
        Enum_M045
        Enum_M060
        Enum_M075
        Enum_M090
        Enum_M120
        Enum_M150
        Enum_M20
        Enum_M25
        Enum_M30
        Enum_MA15
        Enum_MA30
        Enum_MB60
        Enum_MC30
        Enum_MC60
        Enum_NM25
        Enum_P007
        Enum_P015
        Enum_P030
        Enum_P045
        Enum_P060
        Enum_PPD
        Enum_PPDW
        Enum_T030
        Enum_T045
        Enum_T060
        Enum_T075
        Enum_T090
        Enum_T120
        Enum_TN01
    End Enum

    Public Enum EnumIncoTerm
        Enum_AIR
        Enum_CFR
        Enum_CIF
        Enum_CIP
        Enum_CPT
        Enum_DDP
        Enum_DDU
        Enum_DHL
        Enum_EW1
        Enum_EW3
        Enum_EWS
        Enum_EXW
        Enum_FB1
        Enum_FB2
        Enum_FB4
        Enum_FB5
        Enum_FCA
        Enum_FEX
        Enum_FOB
        Enum_LEX
        Enum_MOE
        Enum_OTR
        Enum_TBD
        Enum_UPS
    End Enum

    Public Enum EnumReconciliationAccount
        Enum_0000113997
        Enum_0000121001
        Enum_0000121002
        Enum_0000121003
        Enum_0000121005
        Enum_0000121006
        Enum_0000121007
        Enum_0000121008
        Enum_0000121009
        Enum_0000123100
        Enum_0000142000
        Enum_0000148009
        Enum_0000245000
        Enum_0000248000
    End Enum

    Public Enum EnumVerticalMarket
        Enum_NONE
        Enum_080
        Enum_081
        Enum_082
        Enum_083
        Enum_084
        Enum_100
        Enum_101
        Enum_103
        Enum_104
        Enum_105
        Enum_106
        Enum_107
        Enum_108
        Enum_109
        Enum_130
        Enum_131
        Enum_132
        Enum_133
        Enum_140
        Enum_141
        Enum_142
        Enum_143
        Enum_144
        Enum_145
        Enum_146
        Enum_150
        Enum_151
        Enum_152
        Enum_153
        Enum_154
        Enum_155
        Enum_156
        Enum_157
        Enum_158
        Enum_170
        Enum_200
        Enum_201
        Enum_202
        Enum_203
        Enum_204
        Enum_221
        Enum_222
        Enum_224
        Enum_227
        Enum_260
        Enum_261
        Enum_262
        Enum_263
        Enum_265
        Enum_266
        Enum_270
        Enum_400
        Enum_401
        Enum_590
        Enum_591
        Enum_592
        Enum_593
        Enum_594
        Enum_610
        Enum_611
        Enum_612
        Enum_614
        Enum_615
        Enum_700
        Enum_710
        Enum_720
        Enum_730
        Enum_740
        Enum_750
        Enum_760
        Enum_770
        Enum_780
        Enum_790
        Enum_800
        Enum_810
        Enum_999
    End Enum

    Public Enum EnumShippingCondition
        Enum_01
        Enum_02
        Enum_03
        Enum_04
        Enum_05
        Enum_06
        Enum_07
        Enum_08
        Enum_09
        Enum_10
        Enum_11
        Enum_12
        Enum_13
        Enum_14
        Enum_15
        Enum_16
        Enum_17
        Enum_18
        Enum_19
        Enum_20
        Enum_22
        Enum_23
    End Enum

    Public Enum EnumPlanningGroup
        Enum_A1
        Enum_A2
        Enum_E1
        Enum_E2
        Enum_E3
        Enum_E4
        Enum_P1
        Enum_P3
        Enum_R1
        Enum_R2
        Enum_R3
    End Enum

    Public Enum EnumAccountingClerk
        Enum_01
        Enum_02
        Enum_03
        Enum_04
        Enum_05
        Enum_06
        Enum_07
        Enum_08
        Enum_09
        Enum_10
        Enum_11
        Enum_12
        Enum_13
        Enum_14
        Enum_15
        Enum_16
        Enum_17
        Enum_18
        Enum_19
        Enum_20
        Enum_21
        Enum_22
        Enum_23
        Enum_24
        Enum_25
        Enum_26
        Enum_27
        Enum_28
        Enum_29
        Enum_30
        Enum_31
        Enum_32
        Enum_33
        Enum_34
        Enum_35
        Enum_36
        Enum_37
        Enum_38
        Enum_39
        Enum_40
        Enum_41
        Enum_42
        Enum_43
        Enum_44
        Enum_45
        Enum_46
        Enum_47
        Enum_48
        Enum_49
        Enum_50
        Enum_51
        Enum_52
        Enum_53
        Enum_54
        Enum_55
        Enum_56
        Enum_57
        Enum_58
        Enum_59
        Enum_60
        Enum_61
        Enum_62
        Enum_63
        Enum_64
        Enum_65
        Enum_66
        Enum_67
        Enum_68
        Enum_69
        Enum_70
        Enum_71
        Enum_72
        Enum_73
        Enum_74
        Enum_75
        Enum_76
        Enum_77
        Enum_78
        Enum_79
        Enum_81
        Enum_82
        Enum_83
        Enum_84
        Enum_85
        Enum_86
        Enum_87
        Enum_88
        Enum_89
        Enum_90
        Enum_91
        Enum_93
        Enum_94
        Enum_95
        Enum_96
        Enum_97
        Enum_98
        Enum_AC
        Enum_AI
        Enum_CT
        Enum_EI
        Enum_OP
        Enum_TI
        Enum_Z1
    End Enum

    Public Enum EnumSalesDistrict
        Enum_010
        Enum_020
        Enum_030
        Enum_040
        Enum_050
        Enum_060
        Enum_070
        Enum_080
        Enum_090
        Enum_100
        Enum_110
        Enum_120
        Enum_130
        Enum_140
        Enum_150
        Enum_160
        Enum_170
        Enum_180
        Enum_190
        Enum_200
        Enum_210
        Enum_220
        Enum_230
        Enum_240
        Enum_250
        Enum_260
        Enum_270
        Enum_280
        Enum_290
        Enum_330
        Enum_D10
        Enum_D15
        Enum_D20
        Enum_D21
        Enum_D25
        Enum_D30
        Enum_D35
        Enum_D36
        Enum_D39
        Enum_D40
        Enum_D41
        Enum_D45
        Enum_D46
        Enum_D50
        Enum_D51
        Enum_D52
        Enum_D55
        Enum_D56
        Enum_D60
        Enum_D61
        Enum_D70
        Enum_D75
        Enum_D80
        Enum_D85
        Enum_D90
        Enum_D91
        Enum_D94
        Enum_D95
        Enum_D97
        Enum_D98
        Enum_DLG
        Enum_DMS
        Enum_E01
        Enum_E02
        Enum_E03
        Enum_E04
        Enum_E05
        Enum_E06
        Enum_E07
        Enum_E08
        Enum_E09
        Enum_E10
        Enum_I20
        Enum_I90
        Enum_L10
        Enum_L20
        Enum_L30
        Enum_L40
        Enum_L50
        Enum_L60
        Enum_M10
        Enum_M15
        Enum_M20
        Enum_M25
        Enum_M30
        Enum_M35
        Enum_M40
        Enum_M45
        Enum_M50
        Enum_M55
        Enum_M65
        Enum_M70
        Enum_M75
        Enum_M80
        Enum_PC0
    End Enum

    Public Enum EnumCustomerGroup
        Enum_01
        Enum_02
        Enum_03
        Enum_04
        Enum_05
        Enum_06
        Enum_07
        Enum_08
        Enum_09
        Enum_10
        Enum_11
        Enum_12
        Enum_13
        Enum_15
        Enum_20
        Enum_30
        Enum_B1
        Enum_D1
        Enum_K1
    End Enum

    Public Enum EnumCurrency
        Enum_AUD
        Enum_BRL
        Enum_CNY
        Enum_EUR
        Enum_GBP
        Enum_JPY
        Enum_KRW
        Enum_MYR
        Enum_SGD
        Enum_THB
        Enum_TWD
        Enum_USD
    End Enum

    Public Enum EnumOrgId
        Enum_AU01
        Enum_BR01
        Enum_CN01
        Enum_CN02
        Enum_CN10
        Enum_CN11
        Enum_CN12
        Enum_CN13
        Enum_CN20
        Enum_CN30
        Enum_CN40
        Enum_EU10
        Enum_EU33
        Enum_EU34
        Enum_EU50
        Enum_HK05
        Enum_JP01
        Enum_KR01
        Enum_MY01
        Enum_SG01
        Enum_TL01
        Enum_TW01
        Enum_TW02
        Enum_TW03
        Enum_TW04
        Enum_TW05
        Enum_TWCP
        Enum_US01
    End Enum

    Public Enum EnumCountryCode
        Enum_AE
        Enum_AL
        Enum_AM
        Enum_AN
        Enum_AO
        Enum_AR
        Enum_AT
        Enum_AU
        Enum_AZ
        Enum_BA
        Enum_BD
        Enum_BE
        Enum_BF
        Enum_BG
        Enum_BH
        Enum_BM
        Enum_BN
        Enum_BO
        Enum_BR
        Enum_BS
        Enum_BW
        Enum_BY
        Enum_BZ
        Enum_CA
        Enum_CH
        Enum_CL
        Enum_CN
        Enum_CO
        Enum_CR
        Enum_CY
        Enum_CZ
        Enum_DE
        Enum_DK
        Enum_DM
        Enum_DO
        Enum_DZ
        Enum_EC
        Enum_EE
        Enum_EG
        Enum_ES
        Enum_FI
        Enum_FJ
        Enum_FK
        Enum_FR
        Enum_GB
        Enum_GD
        Enum_GE
        Enum_GL
        Enum_GR
        Enum_GT
        Enum_HK
        Enum_HN
        Enum_HR
        Enum_HU
        Enum_ID
        Enum_IE
        Enum_IL
        Enum_IN
        Enum_IQ
        Enum_IR
        Enum_IS
        Enum_IT
        Enum_JM
        Enum_JO
        Enum_JP
        Enum_KE
        Enum_KG
        Enum_KH
        Enum_KR
        Enum_KW
        Enum_KY
        Enum_KZ
        Enum_LA
        Enum_LB
        Enum_LI
        Enum_LK
        Enum_LT
        Enum_LU
        Enum_LV
        Enum_LY
        Enum_MA
        Enum_MC
        Enum_MD
        Enum_MF
        Enum_MG
        Enum_MK
        Enum_MM
        Enum_MN
        Enum_MO
        Enum_MR
        Enum_MT
        Enum_MU
        Enum_MV
        Enum_MW
        Enum_MX
        Enum_MY
        Enum_NA
        Enum_NC
        Enum_NE
        Enum_NG
        Enum_NI
        Enum_NL
        Enum_NO
        Enum_NP
        Enum_NZ
        Enum_OM
        Enum_PA
        Enum_PE
        Enum_PH
        Enum_PK
        Enum_PL
        Enum_PR
        Enum_PT
        Enum_PY
        Enum_QA
        Enum_RO
        Enum_RS
        Enum_RU
        Enum_SA
        Enum_SB
        Enum_SE
        Enum_SG
        Enum_SI
        Enum_SK
        Enum_SL
        Enum_SV
        Enum_SY
        Enum_SZ
        Enum_TF
        Enum_TH
        Enum_TJ
        Enum_TN
        Enum_TR
        Enum_TT
        Enum_TW
        Enum_UA
        Enum_UG
        Enum_US
        Enum_UY
        Enum_UZ
        Enum_VA
        Enum_VE
        Enum_VG
        Enum_VI
        Enum_VN
        Enum_YU
        Enum_ZA
        Enum_ZM
        Enum_ZW
    End Enum
#End Region

#Region "Create SAP Data"



    Public Shared Function CreateNewCust_ANA_eQ(ByVal KTOKD As String, ByVal KUNNR As String, ByVal VKORG As String,
            ByVal VTWEG As String, ByVal SPART As String, ByVal NAME1 As String, ByVal NAME2 As String,
            ByVal SORT1 As String, ByVal SORT2 As String, ByVal NAME_CO As String, ByVal STREET As String,
            ByVal STR_SUPPL3 As String, ByVal LOCATION As String, ByVal CITY1 As String, ByVal REGION As String,
            ByVal POST_CODE1 As String, ByVal COUNTRY As String, ByVal TXJCD As String, ByVal TRANSPZONE As String,
            ByVal FAX_NUMBER As String, ByVal TEL_NUMBER As String, ByVal MOB_NUMBER As String, ByVal SMTP_ADDR As String,
            ByVal REMARK As String, ByVal KUKLA As String, ByVal BRSCH As String, ByVal STCD1 As String, ByVal STCEG As String,
            ByVal AKONT As String, ByVal ZUAWA As String, ByVal FDGRV As String, ByVal BUSAB As String, ByVal BZIRK As String, ByVal VKBUR As String,
            ByVal VKGRP As String, ByVal KDGRP As String, ByVal WAERS As String, ByVal KLIMK As Decimal,
            ByVal KONDA As String, ByVal VSBED As String, ByVal VWERK As String, ByVal INCO1 As String,
            ByVal INCO2 As String, ByVal ZTERM As String, ByVal KTGRD As String, ByVal TAXKD As String,
            ByVal KVGR1 As String, ByRef retResult As String) As Integer

        'Dim p As New SAPTools.SAP
        Dim r As String = ""
        'p.CreateNewCust(KTOKD, KUNNR, VKORG, VTWEG, SPART, NAME1, "", "", NAME1, NAME1, STREET,
        '     "", "", CITY1, REGION, POST_CODE1, COUNTRY, TXJCD, "0000000001",
        '     "", TEL_NUMBER, "", SMTP_ADDR, "", "03", "2000", "", STCEG, "121001",
        '     "001", "E2", "AC", BZIRK, VKBUR, VKGRP, "20", WAERS, 0, "L1", VSBED, "USH1", INCO1,
        '     INCO2, ZTERM, "01", TAXKD, "GEN", retResult)

        CreateNewCust(KTOKD, KUNNR, VKORG, VTWEG, SPART, NAME1, "", "", NAME1, NAME1, STREET,
                 "", "", CITY1, REGION, POST_CODE1, COUNTRY, TXJCD, "0000000001",
                 "", TEL_NUMBER, "", SMTP_ADDR, "", "03", "2000", "", STCEG, "121001",
                 "001", "E2", "AC", BZIRK, VKBUR, VKGRP, "20", WAERS, 0, "L1", VSBED, "USH1", INCO1,
                 INCO2, ZTERM, "01", TAXKD, "GEN", retResult)

    End Function

    Public Shared Function CreateNewCust(ByVal KTOKD As String, ByVal KUNNR As String, ByVal VKORG As String,
            ByVal VTWEG As String, ByVal SPART As String, ByVal NAME1 As String, ByVal NAME2 As String,
            ByVal SORT1 As String, ByVal SORT2 As String, ByVal NAME_CO As String, ByVal STREET As String,
            ByVal STR_SUPPL3 As String, ByVal LOCATION As String, ByVal CITY1 As String, ByVal REGION As String,
            ByVal POST_CODE1 As String, ByVal COUNTRY As String, ByVal TXJCD As String, ByVal TRANSPZONE As String,
            ByVal FAX_NUMBER As String, ByVal TEL_NUMBER As String, ByVal MOB_NUMBER As String, ByVal SMTP_ADDR As String,
            ByVal REMARK As String, ByVal KUKLA As String, ByVal BRSCH As String, ByVal STCD1 As String, ByVal STCEG As String,
            ByVal AKONT As String, ByVal ZUAWA As String, ByVal FDGRV As String, ByVal BUSAB As String, ByVal BZIRK As String, ByVal VKBUR As String,
            ByVal VKGRP As String, ByVal KDGRP As String, ByVal WAERS As String, ByVal KLIMK As Decimal,
            ByVal KONDA As String, ByVal VSBED As String, ByVal VWERK As String, ByVal INCO1 As String,
            ByVal INCO2 As String, ByVal ZTERM As String, ByVal KTGRD As String, ByVal TAXKD As String,
            ByVal KVGR1 As String, ByRef retResult As String) As Integer
        Try

            Dim _SAPUS_TEST As String = "SAPUS_TEST"
            Dim _SAPUS_PRD As String = "SAPUS_PRD"

            'Dim _SAPUS_TEST As String = "SAPConnTest"
            'Dim _SAPUS_PRD As String = "SAPConnTest"


            Dim SAPRfcDestination As RfcDestination = RfcDestinationManager.GetDestination(_SAPUS_TEST)
            Dim SAPRfcRepository As RfcRepository = SAPRfcDestination.Repository
            Dim F As IRfcFunction = SAPRfcRepository.CreateFunction("Z_SD_B2C_CUSTOMER")
            Dim tLine As IRfcTable = F.GetTable("T_CUSTOMER")

            tLine.Append()
            tLine.CurrentRow.SetValue("KTOKD", KTOKD.ToUpper)
            tLine.CurrentRow.SetValue("KUNNR", KUNNR.ToUpper)
            tLine.CurrentRow.SetValue("VKORG", VKORG.ToUpper)
            tLine.CurrentRow.SetValue("VTWEG", VTWEG.ToUpper)
            tLine.CurrentRow.SetValue("SPART", SPART.ToUpper)
            tLine.CurrentRow.SetValue("NAME1", NAME1.ToUpper)
            tLine.CurrentRow.SetValue("NAME2", NAME2.ToUpper)
            tLine.CurrentRow.SetValue("SORT1", SORT1.ToUpper)
            tLine.CurrentRow.SetValue("SORT2", SORT2.ToUpper)
            tLine.CurrentRow.SetValue("NAME_CO", NAME_CO.ToUpper)
            tLine.CurrentRow.SetValue("STREET", STREET.ToUpper)
            tLine.CurrentRow.SetValue("STR_SUPPL3", STR_SUPPL3.ToUpper)
            tLine.CurrentRow.SetValue("LOCATION", LOCATION.ToUpper)
            tLine.CurrentRow.SetValue("CITY1", CITY1.ToUpper)
            tLine.CurrentRow.SetValue("REGION", REGION.ToUpper)
            tLine.CurrentRow.SetValue("POST_CODE1", POST_CODE1.ToUpper)
            tLine.CurrentRow.SetValue("COUNTRY", COUNTRY.ToUpper)
            tLine.CurrentRow.SetValue("TXJCD", TXJCD.ToUpper)
            tLine.CurrentRow.SetValue("TRANSPZONE", TRANSPZONE.ToUpper)
            tLine.CurrentRow.SetValue("FAX_NUMBER", FAX_NUMBER.ToUpper)
            tLine.CurrentRow.SetValue("TEL_NUMBER", TEL_NUMBER.ToUpper)
            tLine.CurrentRow.SetValue("MOB_NUMBER", MOB_NUMBER.ToUpper)
            tLine.CurrentRow.SetValue("SMTP_ADDR", SMTP_ADDR.ToUpper)
            tLine.CurrentRow.SetValue("REMARK", REMARK.ToUpper)
            tLine.CurrentRow.SetValue("KUKLA", KUKLA.ToUpper)
            tLine.CurrentRow.SetValue("BRSCH", BRSCH.ToUpper)
            tLine.CurrentRow.SetValue("STCD1", STCD1.ToUpper)
            tLine.CurrentRow.SetValue("STCEG", STCEG.ToUpper)
            tLine.CurrentRow.SetValue("AKONT", AKONT.ToUpper)
            tLine.CurrentRow.SetValue("ZUAWA", ZUAWA.ToUpper)
            tLine.CurrentRow.SetValue("FDGRV", FDGRV.ToUpper)
            tLine.CurrentRow.SetValue("BUSAB", BUSAB.ToUpper)
            tLine.CurrentRow.SetValue("BZIRK", BZIRK.ToUpper)
            tLine.CurrentRow.SetValue("VKBUR", VKBUR.ToUpper)
            tLine.CurrentRow.SetValue("VKGRP", VKGRP.ToUpper)
            tLine.CurrentRow.SetValue("KDGRP", KDGRP.ToUpper)
            tLine.CurrentRow.SetValue("WAERS", WAERS.ToUpper)
            tLine.CurrentRow.SetValue("KLIMK", KLIMK)
            tLine.CurrentRow.SetValue("KONDA", KONDA.ToUpper)
            tLine.CurrentRow.SetValue("VSBED", VSBED.ToUpper)
            tLine.CurrentRow.SetValue("VWERK", VWERK.ToUpper)
            tLine.CurrentRow.SetValue("INCO1", INCO1.ToUpper)
            tLine.CurrentRow.SetValue("INCO2", INCO2.ToUpper)
            tLine.CurrentRow.SetValue("ZTERM", ZTERM.ToUpper)
            tLine.CurrentRow.SetValue("KTGRD", KTGRD.ToUpper)
            tLine.CurrentRow.SetValue("TAXKD", TAXKD.ToUpper)
            tLine.CurrentRow.SetValue("KVGR1", KVGR1.ToUpper)

            F.Invoke(SAPRfcDestination)
            Dim TbOut As IRfcTable = F.GetTable("T_LOG")
            Dim dtRet As DataTable = RFCTable2Datatable(TbOut)



            If Not IsNothing(dtRet) AndAlso dtRet.Rows.Count > 0 Then
                If Not IsDBNull(dtRet.Rows(0).Item("Kunnr")) AndAlso Not IsDBNull(dtRet.Rows(0).Item("Log")) AndAlso Not IsDBNull(dtRet.Rows(0).Item("Msgtyp")) Then
                    retResult = dtRet.Rows(0).Item("Kunnr") + " " + dtRet.Rows(0).Item("Log").Replace("", " ") + " ErrType:" + dtRet.Rows(0).Item("Msgtyp")
                End If

            End If
            Return 1
        Catch ex As Exception
            retResult = System.Web.HttpUtility.HtmlEncode("[Server]Connect Error: " & ex.Message.ToString())
            Return -1
        End Try
        Return 0
    End Function

    Public Shared Function RFCTable2Datatable(ByVal RFCTable As IRfcTable) As System.Data.DataTable
        If Not IsNothing(RFCTable) AndAlso RFCTable.RowCount > 0 Then
            Dim loTable As New System.Data.DataTable()
            Dim liElement As Integer = 0
            For liElement = 0 To RFCTable.ElementCount - 1
                Dim metadata As RfcElementMetadata = RFCTable.GetElementMetadata(liElement)
                loTable.Columns.Add(metadata.Name)
            Next
            For Each Row As IRfcStructure In RFCTable
                Dim ldr As System.Data.DataRow = loTable.NewRow()
                For liElement = 0 To RFCTable.ElementCount - 1
                    Dim metadata As RfcElementMetadata = RFCTable.GetElementMetadata(liElement)
                    ldr(metadata.Name) = Row.GetString(metadata.Name)
                Next
                loTable.Rows.Add(ldr)
            Next
            Return loTable
        End If
        Return Nothing
    End Function


    Public Shared Function CreateSAPCustomer() As Boolean
        Dim p1 As New ZSD_CUSTOMER_MAINTAIN_ALL.ZSD_CUSTOMER_MAINTAIN_ALL
        p1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAPConnTest"))
        Dim I_Bapiaddr1 As New ZSD_CUSTOMER_MAINTAIN_ALL.BAPIADDR1
        Dim I_Bapiaddr2 As New ZSD_CUSTOMER_MAINTAIN_ALL.BAPIADDR2
        Dim I_Customer_Is_Consumer As String = ""
        Dim I_Force_External_Number_Range As String = ""
        Dim I_From_Customermaster As String = ""
        Dim I_Kna1 As New ZSD_CUSTOMER_MAINTAIN_ALL.KNA1
        Dim I_Knb1 As New ZSD_CUSTOMER_MAINTAIN_ALL.KNB1
        Dim I_Knb1_Reference As String = ""
        Dim I_Knvv As New ZSD_CUSTOMER_MAINTAIN_ALL.KNVV
        Dim I_Maintain_Address_By_Kna1 As String = ""
        Dim I_No_Bank_Master_Update As String = ""
        Dim I_Raise_No_Bte As String = ""
        Dim Pi_Add_On_Data As New ZSD_CUSTOMER_MAINTAIN_ALL.CUST_ADD_ON_DATA
        Dim Pi_Cam_Changed As String = ""
        Dim Pi_Postflag As String = ""
        ''Return Arguments
        Dim E_Kunnr As String = ""
        Dim E_Sd_Cust_1321_Done As String = ""
        Dim O_Kna1 As New ZSD_CUSTOMER_MAINTAIN_ALL.KNA1
        Dim T_Upd_Txt As New ZSD_CUSTOMER_MAINTAIN_ALL.FKUNTXTTable
        Dim T_Xkn As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNASTable
        Dim T_Xknb5 As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNB5Table
        Dim T_Xknbk As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNBKTable
        Dim T_Xknex As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNEXTable
        Dim T_Xknva As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVATable
        Dim T_Xknvd As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVDTable
        Dim T_Xknvi As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVITable
        Dim T_Xknvk As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVKTable
        Dim T_Xknvl As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVLTable
        Dim T_Xknvp As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVPTable
        Dim T_Xknza As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNZATable
        Dim T_Ykn As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNASTable
        Dim T_Yknb5 As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNB5Table
        Dim T_Yknbk As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNBKTable
        Dim T_Yknex As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNEXTable
        Dim T_Yknva As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVATable
        Dim T_Yknvd As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVDTable
        Dim T_Yknvi As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVITable
        Dim T_Yknvk As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVKTable
        Dim T_Yknvl As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVLTable
        Dim T_Yknvp As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNVPTable
        Dim T_Yknza As New ZSD_CUSTOMER_MAINTAIN_ALL.FKNZATable
        'Assignment 
        With I_Bapiaddr1
            .Addr_No = ""
            .City = ""
        End With
        With I_Bapiaddr2
            .Addr_No = ""
        End With
        I_Customer_Is_Consumer = ""
        I_Force_External_Number_Range = "1"
        I_From_Customermaster = "1"
        With I_Kna1
            .Mandt = "168"
            .Kunnr = "EFFRFA05"
            .Land1 = "FR"
            .Name1 = "FACTORY SYSTEMES SAS"
            .Name2 = " "
            .Ort01 = "MARNE LA VALLEE CEDE"
            .Pstlz = "77437"
            .Regio = " "
            .Sortl = "FR89423785"
            .Stras = "22 RUE VLADIMIR JANKELEVITCH"
            .Telf1 = "0033 164616868"
            .Telfx = "0033 164616734"
            .Xcpdk = " "
            .Adrnr = "0000090780"
            .Mcod1 = "FACTORY SYSTEMES SAS"
            .Mcod2 = " "
            .Mcod3 = "MARNE LA VALLEE CEDE"
            .Anred = "Company"
            .Aufsd = " "
            .Bahne = " "
            .Bahns = " "
            .Bbbnr = "0000000"
            .Bbsnr = "00000"
            .Begru = " "
            .Brsch = "3000"
            .Bubkz = "0"
            .Datlt = " "
            .Erdat = "20051206"
            .Ernam = "AMBER.TSENG"
            .Exabl = " "
            .Faksd = " "
            .Fiskn = " "
            .Knazk = " "
            .Knrza = " "
            .Konzs = " "
            .Ktokd = "Z001"
            .Kukla = "03"
            .Lifnr = " "
            .Lifsd = " "
            .Locco = " "
            .Loevm = " "
            .Name3 = " "
            .Name4 = " "
            .Niels = " "
            .Ort02 = " "
            .Pfach = " "
            .Pstl2 = " "
            .Counc = " "
            .Cityc = " "
            .Rpmkr = " "
            .Sperr = " "
            .Spras = "E"
            .Stcd1 = " "
            .Stcd2 = " "
            .Stkza = " "
            .Stkzu = " "
            .Telbx = " "
            .Telf2 = " "
            .Teltx = " "
            .Telx1 = " "
            .Lzone = "0000000001"
            .Xzemp = " "
            .Vbund = " "
            .Stceg = "FR89423785492"
            .Dear1 = " "
            .Dear2 = " "
            .Dear3 = " "
            .Dear4 = " "
            .Dear5 = " "
            .Gform = " "
            .Bran1 = " "
            .Bran2 = " "
            .Bran3 = " "
            .Bran4 = " "
            .Bran5 = " "
            .Ekont = " "
            .Umsat = "0"
            .Umjah = "0000"
            .Uwaer = " "
            .Jmzah = "000000"
            .Jmjah = "0000"
            .Katr1 = " "
            .Katr2 = " "
            .Katr3 = "04"
            .Katr4 = " "
            .Katr5 = " "
            .Katr6 = " "
            .Katr7 = "620"
            .Katr8 = " "
            .Katr9 = " "
            .Katr10 = " "
            .Stkzn = " "
            .Umsa1 = "0"
            .Txjcd = " "
            .Periv = " "
            .Abrvw = " "
            .Inspbydebi = " "
            .Inspatdebi = " "
            .Ktocd = " "
            .Pfort = " "
            .Werks = " "
            .Dtams = " "
            .Dtaws = " "
            .Duefl = "X"
            .Hzuor = "00"
            .Sperz = " "
            .Etikg = " "
            .Civve = "X"
            .Milve = " "
            .Kdkg1 = "A5"
            .Kdkg2 = "E5"
            .Kdkg3 = "A6"
            .Kdkg4 = "A6"
            .Kdkg5 = "R2"
            .Xknza = " "
            .Fityp = " "
            .Stcdt = " "
            .Stcd3 = " "
            .Stcd4 = " "
            .Xicms = " "
            .Xxipi = " "
            .Xsubt = " "
            .Cfopc = " "
            .Txlw1 = " "
            .Txlw2 = " "
            .Ccc01 = " "
            .Ccc02 = " "
            .Ccc03 = " "
            .Ccc04 = " "
            .Cassd = " "
            .Knurl = " "
            .J_1kfrepre = " "
            .J_1kftbus = " "
            .J_1kftind = " "
            .Confs = " "
            .Updat = "00000000"
            .Uptim = "000000"
            .Nodel = " "
            .Dear6 = " "
            .Alc = " "
            .Pmt_Office = " "
            .Psofg = " "
            .Psois = " "
            .Pson1 = " "
            .Pson2 = " "
            .Pson3 = " "
            .Psovn = " "
            .Psotl = " "
            .Psohs = " "
            .Psost = " "
            .Psoo1 = " "
            .Psoo2 = " "
            .Psoo3 = " "
            .Psoo4 = " "
            .Psoo5 = " "

        End With
        With I_Knb1
            .Mandt = "168"
            .Kunnr = "EFFRFA05"
            .Bukrs = "EU10"
            .Pernr = "00000000"
            .Erdat = "20051207"
            .Ernam = "TED.TSAO"
            .Sperr = " "
            .Loevm = " "
            .Zuawa = "001"
            .Busab = "EI"
            .Akont = "0000121007"
            .Begru = " "
            .Knrze = " "
            .Knrzb = " "
            .Zamim = " "
            .Zamiv = " "
            .Zamir = " "
            .Zamib = " "
            .Zamio = " "
            .Zwels = " "
            .Xverr = " "
            .Zahls = " "
            .Zterm = "M030"
            .Wakon = " "
            .Vzskz = " "
            .Zindt = "00000000"
            .Zinrt = "00"
            .Eikto = " "
            .Zsabe = " "
            .Kverm = " "
            .Fdgrv = "R2"
            .Vrbkz = " "
            .Vlibb = "400000"
            .Vrszl = "0"
            .Vrspr = "0"
            .Vrsnr = "7818005"
            .Verdt = "00000000"
            .Perkz = " "
            .Xdezv = " "
            .Xausz = " "
            .Webtr = "0"
            .Remit = " "
            .Datlz = "00000000"
            .Xzver = "X"
            .Togru = " "
            .Kultg = "0"
            .Hbkid = " "
            .Xpore = " "
            .Blnkz = " "
            .Altkn = " "
            .Zgrup = " "
            .Urlid = " "
            .Mgrup = "01"
            .Lockb = " "
            .Uzawe = " "
            .Ekvbd = " "
            .Sregl = " "
            .Xedip = " "
            .Frgrp = " "
            .Vrsdg = " "
            .Tlfxs = " "
            .Intad = " "
            .Xknzb = " "
            .Guzte = " "
            .Gricd = " "
            .Gridt = " "
            .Wbrsl = " "
            .Confs = " "
            .Updat = "00000000"
            .Uptim = "000000"
            .Nodel = " "
            .Tlfns = " "
            .Cession_Kz = " "
            .Gmvkzd = " "
        End With
        I_Knb1_Reference = ""
        With I_Knvv
            .Mandt = "168"
            .Kunnr = "EFFRFA05"
            .Vkorg = "EU10"
            .Vtweg = "00"
            .Spart = "00"
            .Ernam = "AMBER.TSENG"
            .Erdat = "20051206"
            .Begru = " "
            .Loevm = " "
            .Versg = " "
            .Aufsd = " "
            .Kalks = "1"
            .Kdgrp = "02"
            .Bzirk = "E04"
            .Konda = "00"
            .Pltyp = "00"
            .Awahr = "100"
            .Inco1 = "EWS"
            .Inco2 = " "
            .Lifsd = " "
            .Autlf = " "
            .Antlf = "9"
            .Kztlf = " "
            .Kzazu = "X"
            .Chspl = " "
            .Lprio = "02"
            .Eikto = " "
            .Vsbed = "15"
            .Faksd = " "
            .Mrnkz = " "
            .Perfk = " "
            .Perrl = " "
            .Kvakz = " "
            .Kvawt = "0"
            .Waers = "EUR"
            .Klabc = " "
            .Ktgrd = "02"
            .Zterm = "M030"
            .Vwerk = "EUH1"
            .Vkgrp = "321"
            .Vkbur = "3200"
            .Vsort = " "
            .Kvgr1 = " "
            .Kvgr2 = " "
            .Kvgr3 = "D4"
            .Kvgr4 = " "
            .Kvgr5 = " "
            .Bokre = " "
            .Boidt = "00000000"
            .Kurst = " "
            .Prfre = " "
            .Prat1 = " "
            .Prat2 = " "
            .Prat3 = " "
            .Prat4 = " "
            .Prat5 = " "
            .Prat6 = " "
            .Prat7 = " "
            .Prat8 = " "
            .Prat9 = " "
            .Prata = " "
            .Kabss = " "
            .Kkber = " "
            .Cassd = " "
            .Rdoff = " "
            .Agrel = " "
            .Megru = " "
            .Uebto = "0"
            .Untto = "0"
            .Uebtk = " "
            .Pvksm = " "
            .Podkz = " "
            .Podtg = "0"
            .Blind = " "
            .Bev1_Emlgforts = " "
            .Bev1_Emlgpfand = " "
        End With
        I_Maintain_Address_By_Kna1 = ""
        I_No_Bank_Master_Update = ""
        I_Raise_No_Bte = ""
        With Pi_Add_On_Data
            '  .Kunnr = "EFFRFA05"
        End With
        Pi_Cam_Changed = ""
        Pi_Postflag = ""
        '
        p1.Zsd_Customer_Maintain_All(I_Bapiaddr1, I_Bapiaddr2, I_Customer_Is_Consumer,
                               I_Force_External_Number_Range, I_From_Customermaster,
                               I_Kna1, I_Knb1, I_Knb1_Reference, I_Knvv, I_Maintain_Address_By_Kna1,
                               I_No_Bank_Master_Update, I_Raise_No_Bte,
                               Pi_Add_On_Data, Pi_Cam_Changed, Pi_Postflag,
                               E_Kunnr, E_Sd_Cust_1321_Done, O_Kna1, T_Upd_Txt,
                               T_Xkn, T_Xknb5, T_Xknbk, T_Xknex, T_Xknva, T_Xknvd, T_Xknvi,
                               T_Xknvk, T_Xknvl, T_Xknvp, T_Xknza, T_Ykn, T_Yknb5, T_Yknbk, T_Yknex, T_Yknva,
                               T_Yknvd, T_Yknvi, T_Yknvk, T_Yknvl, T_Yknvp, T_Yknza)
        p1.CommitWork()
        Return True
    End Function
    Public Function SimulateSO(ByRef refDoc_Number As String, ByRef ErrMsg As String,
                            ByRef OrderHeaderDt As SalesOrder.OrderHeaderDataTable, ByRef OrderLineDt As SalesOrder.OrderLinesDataTable,
                            ByRef PartnerFuncDT As SalesOrder.PartnerFuncDataTable, ByRef ConditionDT As SalesOrder.ConditionDataTable,
                            ByRef HeaderTextsDt As SalesOrder.HeaderTextsDataTable, ByRef CreditCardDT As SalesOrder.CreditCardDataTable,
                            ByRef retDataTableDT As DataTable, ByVal LocalTime As DateTime) As Boolean

        If refDoc_Number = "" Then
            ErrMsg = "NO ORDER NO!" : Return False
        End If

        If OrderHeaderDt.Rows.Count <= 0 Then
            ErrMsg = "NO HEADER!" : Return False
        End If

        If OrderLineDt.Rows.Count <= 0 Then
            ErrMsg = "NO DETAIL!" : Return False
        End If

        If PartnerFuncDT.Rows.Count <= 0 Then
            ErrMsg = "NO PARTNER FUNC!" : Return False
        End If
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE
        Dim FF As Integer = 0
        Try

            Dim S_OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, S_OrderLineDt As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable
            Dim S_PartnerFuncDT As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable, S_ConditionDT As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
            Dim S_ScheLineDT As New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable
            Dim S_CreditCardDT As New BAPI_SALESORDER_SIMULATE.BAPICCARDTable


            Dim OrderHeaderRow As SalesOrder.OrderHeaderRow = OrderHeaderDt.Rows(0)
            With OrderHeaderRow
                S_OrderHeader.Doc_Type = .ORDER_TYPE : S_OrderHeader.Sales_Org = .SALES_ORG : S_OrderHeader.Distr_Chan = .DIST_CHAN : S_OrderHeader.Division = .DIVISION
                If Not String.IsNullOrEmpty(.SalesGroup) AndAlso Not String.IsNullOrEmpty(.SalesOffice) Then
                    S_OrderHeader.Sales_Grp = .SalesGroup : S_OrderHeader.Sales_Off = .SalesOffice
                End If

                S_OrderHeader.Req_Date_H = LocalTime.ToString("yyyy/MM/dd")
                'S_OrderHeader.Price_Date = LocalTime.ToString("yyyy/MM/dd")
                S_OrderHeader.Doc_Number = refDoc_Number
                S_OrderHeader.Incoterms1 = .INCO1 : S_OrderHeader.Incoterms2 = .INCO2
                'S_OrderHeader.Taxdep_Cty = .SHIPTO_COUNTRY : S_OrderHeader.Alttax_Cls = .TAX_CLASS : S_OrderHeader.Eutri_Deal = .TRIANGULAR_INDICATOR
                S_OrderHeader.Req_Date_H = .REQUIRE_DATE : S_OrderHeader.Ship_Cond = .SHIP_CONDITION : S_OrderHeader.Purch_No_C = .CUST_PO_NO
                S_OrderHeader.Purch_No_S = .SHIP_CUST_PO_NO : S_OrderHeader.Purch_Date = .PO_DATE : S_OrderHeader.Compl_Dlv = .PARTIAL_SHIPMENT
                'S_OrderHeader.S_Proc_Ind = .EARLY_SHIP : S_OrderHeader.Taxdep_Cty = .TAXDEL_CTY : S_OrderHeader.Taxdst_Cty = .TAXDES_CTY
                If String.IsNullOrEmpty(.PAYTERM) = False Then
                    S_OrderHeader.Pmnttrms = .PAYTERM
                    If S_CreditCardDT.Count > 0 Then

                    End If
                End If
            End With
            For Each r As SalesOrder.OrderLinesRow In OrderLineDt.Rows
                Dim S_OrderLineRow As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN, S_ScheLineRow As New BAPI_SALESORDER_SIMULATE.BAPISCHDL, S_ConditionRow As New BAPI_SALESORDER_SIMULATE.BAPICOND
                With r
                    S_OrderLineRow.Part_Dlv = "" : S_OrderLineRow.Hg_Lv_Item = .HIGHER_LEVEL : S_OrderLineRow.Itm_Number = .LINE_NO
                    S_OrderLineRow.Dlv_Group = .DELIVERY_GROUP : S_OrderLineRow.Plant = .PLANT : S_OrderLineRow.Material = .MATERIAL
                    S_OrderLineRow.Cust_Mat35 = .CUST_MATERIAL 'S_OrderLineRow.Usage_Ind = .DMF_FLAG
                    S_ScheLineRow.Itm_Number = .LINE_NO : S_ScheLineRow.Req_Qty = .QTY : S_ScheLineRow.Req_Date = .REQ_DATE
                    S_ConditionRow.Itm_Number = .LINE_NO : S_ConditionRow.Cond_Type = "ZPN0" : S_ConditionRow.Currency = .CURRENCY : S_ConditionRow.Cond_Value = .PRICE
                    S_OrderLineRow.Short_Text = .Description
                    S_OrderLineRow.Ship_Point = .ShipPoint : S_OrderLineRow.Store_Loc = .StorageLoc
                End With
                S_OrderLineDt.Add(S_OrderLineRow) : S_ScheLineDT.Add(S_ScheLineRow)
                'ICC 2014/10/24 Check ItemCategoryGroup. If it is ZTN3 then do not add to condition
                If S_OrderLineRow.Item_Categ IsNot Nothing AndAlso S_OrderLineRow.Item_Categ = "ZTN3" Then
                Else
                    S_ConditionDT.Add(S_ConditionRow)
                End If
            Next

            For Each r As SalesOrder.PartnerFuncRow In PartnerFuncDT.Rows
                Dim S_PartnerFuncRow As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
                With r
                    S_PartnerFuncRow.Partn_Role = .ROLE : S_PartnerFuncRow.Partn_Numb = .NUMBER
                End With
                S_PartnerFuncDT.Add(S_PartnerFuncRow)

            Next

            'If Not IsNothing(HeaderTextsDt) AndAlso HeaderTextsDt.Rows.Count > 0 Then
            '    For Each r As SalesOrder.HeaderTextsRow In HeaderTextsDt.Rows
            '        With r
            '            Dim StartP As Integer = 1, LongP As Integer = 100, oLine As String = Mid(.TEXT_LINE, StartP, LongP)
            '            While oLine.Trim.Length > 0
            '                Dim S_HeaderTextsRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXT
            '                S_HeaderTextsRow.Doc_Number = refDoc_Number : S_HeaderTextsRow.Text_Id = .TEXT_ID
            '                S_HeaderTextsRow.Text_Line = oLine : S_HeaderTextsRow.Langu = .LANG_ID : S_HeaderTextsDt.Add(S_HeaderTextsRow)
            '                StartP = StartP + 100 : oLine = Mid(.TEXT_LINE, StartP, LongP)
            '            End While
            '        End With

            '    Next
            'End If
            If Not IsNothing(ConditionDT) AndAlso ConditionDT.Rows.Count > 0 Then
                For Each r As SalesOrder.ConditionRow In ConditionDT.Rows
                    Dim S_ConditionRow As New BAPI_SALESORDER_SIMULATE.BAPICOND
                    With r
                        S_ConditionRow.Itm_Number = "000000" : S_ConditionRow.Cond_Type = .TYPE : S_ConditionRow.Currency = .CURRENCY : S_ConditionRow.Cond_Value = .VALUE
                    End With
                    S_ConditionDT.Add(S_ConditionRow)
                Next
            End If

            If Not IsNothing(CreditCardDT) AndAlso CreditCardDT.Rows.Count > 0 Then
                For Each r As SalesOrder.CreditCardRow In CreditCardDT.Rows
                    Dim S_CreditCardRow As New BAPI_SALESORDER_SIMULATE.BAPICCARD
                    With r
                        S_CreditCardRow.Cc_Name = .HOLDER : S_CreditCardRow.Cc_Number = .NUMBER : S_CreditCardRow.Cc_Type = .TYPE
                        S_CreditCardRow.Cc_Valid_T = .EXPIRED 'S_CreditCardRow.Cc_Verif_Value = .VERIFICATION_VALUE
                    End With
                    S_CreditCardDT.Add(S_CreditCardRow)

                Next
            End If
            'For IT testing purpose
            'OrderHeaderRow.DEST_TYPE = 1

            ' proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            If OrderHeaderRow.DEST_TYPE = 0 Then
                proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            Else
                proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAPConnTest"))
            End If
            proxy1.Connection.Open()
            Dim strRelationType As String = ""
            Dim strPConvert As String = ""
            Dim strpintnumassign As String = "", strPTestRun As String = "", retTable As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table

            'Dim dtcon As New DataTable
            'dtcon = S_CreditCardDT.ToADODataTable()
            'Dim str As String = Global_Inc.getDTHtml(dtcon)
            'Global_Inc.Utility_EMailPage("nada.liu@advantech.com.cn", "nada.liu@advantech.com.cn", "", "", _
            '   "so_create return 0", "", Str)

            proxy1.Bapi_Salesorder_Simulate("", S_OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN,
                                            "", New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable,
                                            retTable, S_CreditCardDT, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable,
                                            New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, New BAPI_SALESORDER_SIMULATE.BAPICUVALTable,
                                            S_ConditionDT, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, S_OrderLineDt, New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable,
                                            S_PartnerFuncDT, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable, S_ScheLineDT, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)

            'proxy1.Bapi_Salesorder_Createfromdat2( _
            'ErrMsg, strRelationType, strPConvert, _
            'strpintnumassign, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDLS, S_OrderHeader, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1X, _
            'refDoc_Number, New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SENDER, strPTestRun, refDoc_Number, _
            'New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPAREXTable, S_CreditCardDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUBLBTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUINSTable, _
            'New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUPRTTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUCFGTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUREFTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVALTable, _
            'New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVKTable, S_ConditionDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDXTable, _
            'S_OrderLineDt, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMXTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDKEYTable, _
            'S_PartnerFuncDT, S_ScheLineDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLXTable, S_HeaderTextsDt, New BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1Table, retTable)
            retDataTableDT = retTable.ToADODataTable()

            For retRowCount = 0 To retDataTableDT.Rows.Count - 1
                If retDataTableDT.Rows(retRowCount).Item("Number") = "311" Then
                    FF = 1
                    Exit For
                End If
            Next
            If FF = 1 Then
                proxy1.CommitWork()
            End If
            proxy1.Connection.Close()
            'If S_CreditCardDT.Count > 0 AndAlso FF = 1 Then
            '    '20120726 TC: Try to sleep two seconds to see if this can tick authorization block successfully
            '    Threading.Thread.Sleep(2000)
            '    Dim pAuthBlock As New ZSD_UPDATE_FPLA.ZSD_UPDATE_FPLA
            '    pAuthBlock.Connection = New SAP.Connector.SAPConnection(proxy1.ConnectionString)
            '    pAuthBlock.Connection.Open()
            '    pAuthBlock.Zsd_Update_Fpla("X", refDoc_Number, Nothing)
            '    pAuthBlock.Connection.Close()
            'End If

        Catch mex As Exception
            If Not IsNothing(proxy1) AndAlso Not IsNothing(proxy1.Connection) Then
                proxy1.Connection.Close()
            End If
            ErrMsg = mex.ToString()
            Return False
        End Try

        If FF = 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    Function getProdPricingGrp(ByVal partNo As String) As String
        Dim prodLine As String = ""
        Dim dtp As New DataTable
        dtp = dbUtil.dbGetDataTable("MY", String.Format("select PRODUCT_LINE from SAP_PRODUCT where PART_NO='{0}'", partNo))
        If dtp.Rows.Count > 0 Then
            prodLine = dtp.Rows(0).Item("PRODUCT_LINE")
        End If
        If prodLine <> "" Then
            Dim dt As New DataTable
            dt = dbUtil.dbGetDataTable("EQ", String.Format("select PricingGrp from ProdPricingGrp where ProdLine='{0}'", prodLine))

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0).Item("PricingGrp")
            End If
        End If
        Return ""
    End Function
    Shared Function isNeedANAGPControl(ByVal PN As String) As Boolean
        Dim PGroup As String = ""
        Dim DT As DataTable = CommonLogic.getSAPProductLine(PN)
        If Not IsNothing(DT) AndAlso DT.Rows.Count > 0 Then
            PGroup = DT.Rows(0).Item("PRODUCT_DIVISION")
        End If
        If PGroup = "MEDC" Then Return False
        Return True
    End Function
    Shared Function getCostForANAPn(ByVal PN As String, ByVal Plant As String) As Decimal
        Dim strSql As String = String.Format("select isnull(standard_price,0) AS P from PRODUCT_COST where PART_NO=@PN and PLANT =@PLANT", PN, Plant)
        Dim cmd As New SqlClient.SqlCommand(strSql, New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString))
        cmd.Parameters.AddWithValue("PN", PN) : cmd.Parameters.AddWithValue("PLANT", Plant)
        cmd.Connection.Open()
        Dim objCost As Object = cmd.ExecuteScalar()
        cmd.Connection.Close()
        If objCost IsNot Nothing Then
            Return CType(objCost, Decimal)
        Else
            Return 0
        End If
    End Function
    ''' <summary>
    ''' ICC 2015/9/22 Check product belows GP.
    ''' If product line = 10, then use eA rule. If product line = 20 or other circumstances, then use eP rule.
    ''' </summary>
    ''' <param name="PartNo"></param>
    ''' <param name="prodLine"></param>
    ''' <param name="errmsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPriceByProdLine(ByVal PartNo As String, ByVal prodLine As String, Optional ByRef errmsg As String = "") As Decimal
        'If prodLine = "20" Then
        '    companyId = "UAON5005"
        'End If

        'ICC 2015/9/22 Product line = 10 then use eA rule.
        If IsNumeric(prodLine) AndAlso CDec(prodLine) = 10 Then
            Dim companyId As String = "UAON00001"
            Dim pQty As Integer = 29
            Dim DTPRICE As DataTable = Nothing
            getSAPPriceByTable(PartNo, "US01", companyId, DTPRICE, errmsg, pQty)
            If DTPRICE.Rows.Count > 0 Then
                Return FormatNumber(DTPRICE.Rows(0).Item("Netwr"), 2).Replace(",", "")
            End If
        Else
            'ICC 2015/9/22 Product line = 20 or other situations then use eP rule.
            Return getCostForANAPn(PartNo, "USH1") * 1.25
            'Return 4 * getCostForANAPn(PartNo, "USH1") / 3 '(p-c)/p>1/4 ==>p>4c/3
        End If
        Return 0
    End Function

    'Shared Function Format2SAPItem(ByVal Part_No As String) As String
    '    'Try
    '    If IsNumeric(Part_No) And Not Part_No.Substring(0, 1).Equals("0") Then
    '        Dim zeroLength As Integer = 18 - Part_No.Length
    '        For i As Integer = 0 To zeroLength - 1
    '            Part_No = "0" & Part_No
    '        Next
    '        Return Part_No
    '    Else
    '        Return Part_No
    '    End If
    '    'Catch ex As Exception
    '    '    Return Part_No
    '    'End Try
    'End Function
    Shared Function testU(ByVal user As String) As Boolean
        Return True
        If user.ToLower.Contains("james.wu") Or
            user.ToLower.Contains("nada.liu") Then
            Return True
        End If
        Return False
    End Function


    Shared Sub UpdateSOWarrantyFlagByTable(ByVal DT As DataTable, ByRef status As String, ByRef retCode As Boolean)

        'Utilities.Utility_EMailPage("nada.liu@advantech.com.cn", "nada.liu@advantech.com.cn", "", "", _
        '   "so_create return 0", "", inxml)

        Dim p1 As New Update_Warranty.Update_Warranty
        Dim retTb As New Update_Warranty.BAPIRET2Table
        Dim inTb As New Update_Warranty.ZSO_CHANGE_B2BTable

        'Dim dt As DataTable = Utilities.ADOXml2DataTable(inxml)
        If DT Is Nothing Then
            status = "Xml parse error"
            'Utilities.Utility_EMailPage( _
            '"tc.chen@advantech.com.tw", "tc.chen@advantech.com.tw;jackie.wu@advantech.com.cn", "", "", _
            '"ExFlag Fail, dt is nothing", "", "")
            retCode = False
            Exit Sub
        End If


        'Utilities.Utility_EMailPage("nada.liu@advantech.com.cn", "nada.liu@advantech.com.cn", "", "", _
        '   "UP EW return 0", "", Utilities.getDTHtml(DT))
        For i As Integer = 0 To DT.Rows.Count - 1

            'Dim r As New Update_Warranty.ZSO_CHANGE_B2B
            'r.Vbeln = dt.Rows(i).Item("so_no").ToString() : r.Posnr = dt.Rows(i).Item("line_no").ToString()
            'If dt.Rows(i).Item("exwarranty_flag").ToString().Equals("0") Then
            '    r.Zz_Guara = ""
            'Else
            '    r.Zz_Guara = dt.Rows(i).Item("exwarranty_flag").ToString()
            'End If

            'inTb.Add(r)
            Dim r As New Update_Warranty.ZSO_CHANGE_B2B
            r.Vbeln = DT.Rows(i).Item("so_no").ToString() : r.Posnr = DT.Rows(i).Item("line_no").ToString()
            If r.Posnr.Length < 6 Then
                Dim appendZeroQty As Integer = 6 - r.Posnr.Length
                For j As Integer = 0 To appendZeroQty - 1
                    r.Posnr = "0" + r.Posnr
                Next
            End If
            If DT.Rows(i).Item("exwarranty_flag").ToString().Equals("0") Or
            DT.Rows(i).Item("exwarranty_flag").ToString().Equals("00") Then
                r.Zz_Guara = "00"
            Else
                r.Zz_Guara = DT.Rows(i).Item("exwarranty_flag").ToString()
            End If
            inTb.Add(r)

        Next
        'Dim DTD As DataTable = inTb.ToADODataTable()
        'Utilities.Utility_EMailPage("nada.liu@advantech.com.cn", "nada.liu@advantech.com.cn", "", "", _
        '  "UP EW return 0", "", Utilities.getDTHtml(DTD))
        'Dim destination1 As Destination = Nothing
        'destination1 = New Destination
        'destination1.AppServerHost = "172.20.1.176"
        ''Me.destination1.AppServerHost = "172.20.1.1"
        'destination1.Client = "168"
        'destination1.Language = ""
        'destination1.Password = "ebizaeu"
        'destination1.SystemNumber = "5"
        ''Me.destination1.SystemNumber = "1"
        'destination1.Username = "b2baeu"
        'p1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD").ToString)
        p1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD").ToString)
        p1.Connection.Open()
        p1.Timeout = 5000
        Try
            p1.Z_Change_So_B2b(retTb, inTb)
        Catch ex As Exception
            'Utilities.Utility_EMailPage( _
            '"tc.chen@advantech.com.tw", "tc.chen@advantech.com.tw;jackie.wu@advantech.com.cn", "", "", "ExFlag excption", _
            '"", ex.ToString())
            p1.Connection.Close()
            status = ex.ToString() : retCode = False : Exit Sub
        End Try

        p1.Connection.Close()
        'Utilities.Utility_EMailPage( _
        '    "tc.chen@advantech.com.tw", "tc.chen@advantech.com.tw;jackie.wu@advantech.cn", "", "", "ExFlag success!", _
        '    "", _
        '    "<![CDATA" + Utilities.SAPTableToADOXML(inTb) + Utilities.SAPTableToADOXML(retTb) + "]]>")
        'status = Utilities.SAPTableToADOXML(retTb)
        retCode = True

    End Sub

    '    Public Shared Function SAPTableToADOXML(ByVal table As SAPTable) As String
    '        Dim table1 As DataTable = table.ToADODataTable

    '        Dim class1 As New DOMDocumentClass
    '        Dim class2 As New RecordsetClass
    '        Dim num1 As Integer
    '        For num1 = 0 To table1.Columns.Count - 1
    '            Select Case table1.Columns.Item(num1).DataType.ToString
    '                Case "System.Int16"
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adSmallInt, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case "System.SByte"
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adTinyInt, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case "System.Int32"
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case "System.Int64"
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adBigInt, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case "System.Single"
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adSingle, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case "System.Double"
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case "System.Decimal"
    '                    'class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adDecimal, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adCurrency, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case "System.DateTime"
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adDate, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case "System.Object"
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adVariant, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case "System.String"
    '                    class2.Fields.Append(table1.Columns.Item(num1).ColumnName, DataTypeEnum.adVarChar, 255, FieldAttributeEnum.adFldUnspecified, Missing.Value)
    '                    GoTo Label_02EC
    '                Case Else
    '                    GoTo Label_02EC
    '            End Select

    'Label_02EC:

    '        Next num1

    '        class2.CursorLocation = CursorLocationEnum.adUseClient
    '        class2.Open(Missing.Value, Missing.Value, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1)

    '        Dim num2 As Integer
    '        For num2 = 0 To table1.Rows.Count - 1
    '            class2.AddNew(Missing.Value, Missing.Value)
    '            Dim num3 As Integer
    '            For num3 = 0 To table1.Columns.Count - 1
    '                If (table1.Rows.Item(num2).Item(table1.Columns.Item(num3).ColumnName) Is DBNull.Value) Then
    '                    Try
    '                        class2.Fields.Item(table1.Columns.Item(num3).ColumnName).Value = table1.Columns.Item(num3).DefaultValue
    '                    Catch ex As Exception

    '                    End Try
    '                Else
    '                    Try
    '                        class2.Fields.Item(table1.Columns.Item(num3).ColumnName).Value = table1.Rows.Item(num2).Item(table1.Columns.Item(num3).ColumnName)
    '                    Catch ex As Exception
    '                        'class2.Fields.Item(table1.Columns.Item(num3).ColumnName).Value = ""
    '                    End Try

    '                End If
    '            Next num3
    '        Next num2

    '        class2.Save(class1, PersistFormatEnum.adPersistXML)
    '        Return class1.xml

    '    End Function


    Function getSAPPriceByTable(ByVal partNoStr As String, ByVal org As String, ByVal SoldTo As String, ByVal ShipTo As String, ByVal Currency As String, ByVal OrderType As String, ByRef retTable As DataTable, Optional ByRef errMsg As String = "", Optional ByVal qty As Integer = 1) As Integer

        'Dim DTIN As New MySAPDALWS.SAPDALDS.ProductInDataTable, DTOUT As New MySAPDALWS.SAPDALDS.ProductOutDataTable
        Dim DTIN As New SAPDALDS.ProductInDataTable, DTOUT As New SAPDALDS.ProductOutDataTable

        Dim part_noArr() As String = partNoStr.Trim().Trim("|").Split("|")
        For Each p As String In part_noArr
            'Dim R As MySAPDALWS.SAPDALDS.ProductInRow = DTIN.NewRow
            If Not p.Equals("Build In", StringComparison.OrdinalIgnoreCase) OrElse Not p.Equals("No Need", StringComparison.OrdinalIgnoreCase) Then
                'If Not p.Equals(, StringComparison.OrdinalIgnoreCase) Then
                Dim R As SAPDALDS.ProductInRow = DTIN.NewRow
                R.PART_NO = Global_Inc.Format2SAPItem(p.Trim) ' Format2SAPItem(p.Trim).TrimStart("0") 
                R.QTY = qty
                DTIN.Rows.Add(R)
            End If
        Next
        'Dim ws As New MySAPDALWS.MYSAPDAL
        GetPrice(SoldTo, ShipTo, org, Currency, OrderType, DTIN, DTOUT, errMsg)
        'Util.showDT(DTOUT)
        Dim printDT As New DataTable
        printDT.Columns.Add("MATNR") : printDT.Columns.Add("Kzwi1") : printDT.Columns.Add("Netwr") : printDT.Columns.Add("RECYCLE_FEE") : printDT.Columns.Add("TAX") : printDT.Columns.Add("ZMIP") : printDT.Columns.Add("VPRS")
        'For Each x As MySAPDALWS.SAPDALDS.ProductOutRow In DTOUT
        For Each x As SAPDALDS.ProductOutRow In DTOUT
            'If CDbl(x.LIST_PRICE) < CDbl(x.UNIT_PRICE) Then
            '    x.LIST_PRICE = x.UNIT_PRICE
            'End If
            Dim pr As DataRow = printDT.NewRow

            pr.Item("MATNR") = x.PART_NO
            If IsNumeric(x.LIST_PRICE) Then
                pr.Item("Kzwi1") = x.LIST_PRICE
            Else
                pr.Item("Kzwi1") = 0
            End If
            If IsNumeric(x.UNIT_PRICE) Then
                pr.Item("Netwr") = x.UNIT_PRICE
            Else
                pr.Item("Netwr") = 0
            End If
            If IsNumeric(x.RECYCLE_FEE) Then
                pr.Item("RECYCLE_FEE") = x.RECYCLE_FEE
            Else
                pr.Item("RECYCLE_FEE") = 0
            End If
            If IsNumeric(x.TAX) Then
                pr.Item("TAX") = x.TAX
            Else
                pr.Item("TAX") = 0
            End If
            If Not x.Condition_ZMIP Is Nothing AndAlso IsNumeric(x.Condition_ZMIP) Then
                pr.Item("ZMIP") = x.Condition_ZMIP
            Else
                pr.Item("ZMIP") = 0
            End If
            If Not x.Condition_VPRS Is Nothing AndAlso IsNumeric(x.Condition_VPRS) Then
                pr.Item("VPRS") = x.Condition_VPRS
            Else
                pr.Item("VPRS") = 0
            End If
            printDT.Rows.Add(pr)
        Next
        'Util.showDT(printDT)
        retTable = printDT
        Return 1

    End Function


    Function getSAPPriceByTable(ByVal partNoStr As String, ByVal org As String, ByVal company As String, ByRef retTable As DataTable, Optional ByRef errMsg As String = "", Optional ByVal qty As Integer = 1) As Integer

        Return getSAPPriceByTable(partNoStr, org, company, company, "", "ZOR", retTable, errMsg, qty)

        ''Dim DTIN As New MySAPDALWS.SAPDALDS.ProductInDataTable, DTOUT As New MySAPDALWS.SAPDALDS.ProductOutDataTable
        'Dim DTIN As New SAPDALDS.ProductInDataTable, DTOUT As New SAPDALDS.ProductOutDataTable

        'Dim part_noArr() As String = partNoStr.Trim().Trim("|").Split("|")
        'For Each p As String In part_noArr
        '    'Dim R As MySAPDALWS.SAPDALDS.ProductInRow = DTIN.NewRow
        '    Dim R As SAPDALDS.ProductInRow = DTIN.NewRow
        '    R.PART_NO = Format2SAPItem(p.Trim).TrimStart("0")
        '    R.QTY = qty
        '    DTIN.Rows.Add(R)
        'Next
        ''Dim ws As New MySAPDALWS.MYSAPDAL
        'GetPrice(company, company, org, DTIN, DTOUT, errMsg)
        ''Util.showDT(DTOUT)
        'Dim printDT As New DataTable
        'printDT.Columns.Add("MATNR") : printDT.Columns.Add("Kzwi1") : printDT.Columns.Add("Netwr") : printDT.Columns.Add("RECYCLE_FEE") : printDT.Columns.Add("TAX")
        ''For Each x As MySAPDALWS.SAPDALDS.ProductOutRow In DTOUT
        'For Each x As SAPDALDS.ProductOutRow In DTOUT
        '    'If CDbl(x.LIST_PRICE) < CDbl(x.UNIT_PRICE) Then
        '    '    x.LIST_PRICE = x.UNIT_PRICE
        '    'End If
        '    Dim pr As DataRow = printDT.NewRow

        '    pr.Item("MATNR") = x.PART_NO
        '    If IsNumeric(x.LIST_PRICE) Then
        '        pr.Item("Kzwi1") = x.LIST_PRICE
        '    Else
        '        pr.Item("Kzwi1") = 0
        '    End If
        '    If IsNumeric(x.UNIT_PRICE) Then
        '        pr.Item("Netwr") = x.UNIT_PRICE
        '    Else
        '        pr.Item("Netwr") = 0
        '    End If
        '    If IsNumeric(x.RECYCLE_FEE) Then
        '        pr.Item("RECYCLE_FEE") = x.RECYCLE_FEE
        '    Else
        '        pr.Item("RECYCLE_FEE") = 0
        '    End If
        '    If IsNumeric(x.TAX) Then
        '        pr.Item("TAX") = x.TAX
        '    Else
        '        pr.Item("TAX") = 0
        '    End If
        '    printDT.Rows.Add(pr)
        'Next
        ''Util.showDT(printDT)
        'retTable = printDT
        'Return 1
    End Function
    Public Function CreateQuotation(
                        ByRef refDoc_Number As String, ByRef ErrMsg As String,
                        ByRef OrderHeaderDt As SalesOrder.OrderHeaderDataTable,
                        ByRef OrderLineDt As SalesOrder.OrderLinesDataTable,
                        ByRef PartnerFuncDT As SalesOrder.PartnerFuncDataTable,
                        ByRef ConditionDT As SalesOrder.ConditionDataTable,
                        ByRef HeaderTextsDt As SalesOrder.HeaderTextsDataTable,
                        ByRef retDataTableDT As DataTable) As Boolean

        If refDoc_Number = "" Then
            ErrMsg = "NO ORDER NO!"
            Return False
        End If

        If OrderHeaderDt.Rows.Count <= 0 Then
            ErrMsg = "NO HEADER!"
            Return False
        End If

        If OrderLineDt.Rows.Count <= 0 Then
            ErrMsg = "NO DETAIL!"
            Return False
        End If

        If PartnerFuncDT.Rows.Count <= 0 Then
            ErrMsg = "NO PARTNER FUNC!"
            Return False
        End If
        Dim proxy1 As New Quotation_Create_SAP.Quotation_Create_SAP
        Dim FF As Integer = 0
        Try

            Dim S_OrderHeader As New Quotation_Create_SAP.BAPISDHD1
            Dim S_OrderLineDt As New Quotation_Create_SAP.BAPISDITMTable
            Dim S_PartnerFuncDT As New Quotation_Create_SAP.BAPIPARNRTable
            Dim S_ConditionDT As New Quotation_Create_SAP.BAPICONDTable
            Dim S_HeaderTextsDt As New Quotation_Create_SAP.BAPISDTEXTTable
            Dim S_ScheLineDT As New Quotation_Create_SAP.BAPISCHDLTable


            Dim OrderHeaderRow As SalesOrder.OrderHeaderRow = OrderHeaderDt.Rows(0)
            With OrderHeaderRow
                S_OrderHeader.Doc_Type = .ORDER_TYPE
                S_OrderHeader.Sales_Org = .SALES_ORG
                S_OrderHeader.Distr_Chan = .DIST_CHAN
                S_OrderHeader.Division = .DIVISION
                S_OrderHeader.Incoterms1 = .INCO1
                S_OrderHeader.Incoterms2 = .INCO2
                S_OrderHeader.Taxdep_Cty = .SHIPTO_COUNTRY
                S_OrderHeader.Alttax_Cls = .TAX_CLASS
                S_OrderHeader.Eutri_Deal = .TRIANGULAR_INDICATOR
                S_OrderHeader.Req_Date_H = .REQUIRE_DATE
                S_OrderHeader.Ship_Cond = .SHIP_CONDITION
                S_OrderHeader.Purch_No_C = .CUST_PO_NO
                S_OrderHeader.Purch_No_S = .SHIP_CUST_PO_NO
                S_OrderHeader.Purch_Date = .PO_DATE
                S_OrderHeader.Compl_Dlv = .PARTIAL_SHIPMENT
                S_OrderHeader.S_Proc_Ind = .EARLY_SHIP

                If .VERSION IsNot Nothing AndAlso Not String.IsNullOrEmpty(.VERSION) Then
                    S_OrderHeader.Version = .VERSION
                End If

                'Ryan 20170705 For ACN create SAP quotation
                If .SALES_ORG.StartsWith("CN") Then
                    If Not String.IsNullOrEmpty(.SalesGroup) AndAlso Not String.IsNullOrEmpty(.SalesOffice) Then
                        S_OrderHeader.Sales_Grp = .SalesGroup : S_OrderHeader.Sales_Off = .SalesOffice
                    End If
                    S_OrderHeader.Qt_Valid_F = DateTime.Now.ToString("yyyy/MM/dd")
                    S_OrderHeader.Qt_Valid_T = DateTime.Now.AddDays(30).ToString("yyyy/MM/dd")
                End If

            End With
            For Each r As SalesOrder.OrderLinesRow In OrderLineDt.Rows
                Dim S_OrderLineRow As New Quotation_Create_SAP.BAPISDITM
                Dim S_ScheLineRow As New Quotation_Create_SAP.BAPISCHDL
                Dim S_ConditionRow As New Quotation_Create_SAP.BAPICOND
                With r
                    S_OrderLineRow.Part_Dlv = ""
                    S_OrderLineRow.Hg_Lv_Item = .HIGHER_LEVEL
                    S_OrderLineRow.Itm_Number = .LINE_NO
                    S_OrderLineRow.Dlv_Group = .DELIVERY_GROUP
                    S_OrderLineRow.Plant = .PLANT
                    S_OrderLineRow.Material = .MATERIAL
                    S_OrderLineRow.Cust_Mat35 = .CUST_MATERIAL
                    S_OrderLineRow.Usage_Ind = .DMF_FLAG

                    S_ScheLineRow.Itm_Number = .LINE_NO
                    S_ScheLineRow.Req_Qty = .QTY
                    S_ScheLineRow.Req_Date = .REQ_DATE

                    S_ConditionRow.Itm_Number = .LINE_NO
                    S_ConditionRow.Cond_Type = "ZPN0"
                    S_ConditionRow.Currency = .CURRENCY
                    S_ConditionRow.Cond_Value = .PRICE

                    'Ryan 20170720 ACN quotations tax settings
                    If OrderHeaderRow.SALES_ORG.StartsWith("CN", StringComparison.InvariantCultureIgnoreCase) Then
                        S_ConditionRow.Cond_Value = Decimal.Parse(.PRICE) / (1 + ConfigurationManager.AppSettings("ACNTaxRate"))
                    End If

                End With
                S_OrderLineDt.Add(S_OrderLineRow)
                S_ScheLineDT.Add(S_ScheLineRow)
                S_ConditionDT.Add(S_ConditionRow)

            Next

            For Each r As SalesOrder.PartnerFuncRow In PartnerFuncDT.Rows
                Dim S_PartnerFuncRow As New Quotation_Create_SAP.BAPIPARNR
                With r
                    S_PartnerFuncRow.Partn_Role = .ROLE
                    S_PartnerFuncRow.Partn_Numb = .NUMBER
                End With
                S_PartnerFuncDT.Add(S_PartnerFuncRow)

            Next

            If Not IsNothing(HeaderTextsDt) AndAlso HeaderTextsDt.Rows.Count > 0 Then
                For Each r As SalesOrder.HeaderTextsRow In HeaderTextsDt.Rows
                    Dim S_HeaderTextsRow As New Quotation_Create_SAP.BAPISDTEXT
                    With r
                        S_HeaderTextsRow.Doc_Number = refDoc_Number
                        S_HeaderTextsRow.Text_Id = .TEXT_ID
                        S_HeaderTextsRow.Text_Line = .TEXT_LINE
                        S_HeaderTextsRow.Langu = .LANG_ID
                    End With
                    S_HeaderTextsDt.Add(S_HeaderTextsRow)

                Next
            End If
            If Not IsNothing(ConditionDT) AndAlso ConditionDT.Rows.Count > 0 Then
                For Each r As SalesOrder.ConditionRow In ConditionDT.Rows
                    Dim S_ConditionRow As New Quotation_Create_SAP.BAPICOND
                    With r
                        S_ConditionRow.Itm_Number = "000000"
                        S_ConditionRow.Cond_Type = .TYPE
                        S_ConditionRow.Currency = .CURRENCY
                        S_ConditionRow.Cond_Value = .VALUE
                    End With
                    S_ConditionDT.Add(S_ConditionRow)
                Next
            End If

            ' proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            If OrderHeaderRow.DEST_TYPE = 0 Then
                proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            Else
                proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAPConnTest"))
            End If

            proxy1.Connection.Open()

            Dim strRelationType As String = ""
            Dim strPConvert As String = ""
            Dim strpintnumassign As String = ""
            Dim strPTestRun As String = ""
            Dim retTable As New Quotation_Create_SAP.BAPIRET2Table

            proxy1.Bapi_Quotation_Createfromdata2(
            ErrMsg, strRelationType, strPConvert,
            strpintnumassign, New Quotation_Create_SAP.BAPISDLS, S_OrderHeader, New Quotation_Create_SAP.BAPISDHD1X,
            refDoc_Number, New Quotation_Create_SAP.BAPI_SENDER, strPTestRun, refDoc_Number,
            New Quotation_Create_SAP.BAPIPAREXTable, New Quotation_Create_SAP.BAPIADDR1Table,
            New Quotation_Create_SAP.BAPICUBLBTable, New Quotation_Create_SAP.BAPICUINSTable,
            New Quotation_Create_SAP.BAPICUPRTTable, New Quotation_Create_SAP.BAPICUCFGTable,
            New Quotation_Create_SAP.BAPICUREFTable, New Quotation_Create_SAP.BAPICUVALTable,
            New Quotation_Create_SAP.BAPICUVKTable, S_ConditionDT, New Quotation_Create_SAP.BAPICONDXTable,
            S_OrderLineDt, New Quotation_Create_SAP.BAPISDITMXTable, New Quotation_Create_SAP.BAPISDKEYTable,
            S_PartnerFuncDT, S_ScheLineDT, New Quotation_Create_SAP.BAPISCHDLXTable, S_HeaderTextsDt, retTable)

            retDataTableDT = retTable.ToADODataTable()
            For retRowCount = 0 To retDataTableDT.Rows.Count - 1
                If retDataTableDT.Rows(retRowCount).Item("Number") = "311" Then
                    FF = 1
                    Exit For
                End If
            Next
            If FF = 1 Then
                proxy1.CommitWork()
            End If
            proxy1.Connection.Close()
        Catch mex As Exception
            If Not IsNothing(proxy1) AndAlso Not IsNothing(proxy1.Connection) Then
                proxy1.Connection.Close()
            End If
            ErrMsg = mex.ToString()
            Return False
        End Try

        If FF = 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub UpF_test(ByVal OrderId As String)
        'Dim proxy1 As New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SALESORDER_CREATEFROMDAT2
        'proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        'Dim pAuthBlock As New ZSD_UPDATE_FPLA.ZSD_UPDATE_FPLA
        'Dim DT_LOG As New ZSD_UPDATE_FPLA.SWF_LINESTable
        'Dim dt As New DataTable
        'pAuthBlock.Connection = New SAP.Connector.SAPConnection(proxy1.ConnectionString)
        'pAuthBlock.Connection.Open()
        'For i As Integer = 0 To 3
        '    Threading.Thread.Sleep(2000)
        '    pAuthBlock.Zsd_Update_Fpla("X", OrderId, DT_LOG)
        '    dt = DT_LOG.ToADODataTable
        '    If dt.Rows.Count > 0 AndAlso dt.Rows(0).Item("Line").ToString.Contains("successful") Then
        '        Exit For
        '    End If
        'Next
        'If dt.Rows.Count > 0 AndAlso (Not dt.Rows(0).Item("Line").ToString.Contains("successful")) Then
        '    logUpdateAuthBlockFlag(OrderId)
        'End If
        'pAuthBlock.Connection.Close()
    End Sub
    Public Function logUpdateAuthBlockFlag(ByVal orderId As String)
        dbUtil.dbExecuteNoQuery("MY", "insert into UPDATE_AUTHORIZATION_BLOCK_LOG VALUES('" & orderId & "')")
    End Function
    Public Shared Sub InsertMyErrLog(ByRef strEx As String)
        'Try
        Dim userid As String = ""
        If HttpContext.Current.User.Identity.IsAuthenticated AndAlso HttpContext.Current.User.Identity.Name IsNot Nothing AndAlso
            HttpContext.Current.User.Identity.Name <> "" Then userid = HttpContext.Current.User.Identity.Name
        Dim iUrl As String = Left(HttpContext.Current.Request.ServerVariables("URL").Replace("'", "''"), 500), iQString As String = ""
        If HttpContext.Current.Request.QueryString.HasKeys Then
            For i As Integer = 0 To HttpContext.Current.Request.QueryString.Count - 1
                iQString += HttpContext.Current.Request.QueryString.Keys(i) & "=" &
                         HttpContext.Current.Request.QueryString.Item(i) & "&"
            Next
            iQString.Replace("'", "&aps").Replace("'", "''")
        End If

        'Frank 2012/05/15
        'log user client information
        Dim _HTTP_USER_AGENT As String = "HTTP_USER_AGENT value is "
        If HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT") Is Nothing Then
            _HTTP_USER_AGENT &= "nothing"
        Else
            _HTTP_USER_AGENT &= HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT").Replace("'", "''")
        End If


        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        Dim cmd As New SqlClient.SqlCommand("INSERT INTO MY_ERR_LOG (ROW_ID, USERID, URL, QSTRING, EXMSG, APPID, CLIENT_INFO) VALUES (@UNIQID, @UID, @URL, @REQSTR, @ERRMSG, 'MY', @CLIENTINFO)", conn)

        With cmd.Parameters
            .AddWithValue("UNIQID", Left(System.Guid.NewGuid().ToString().Replace("-", ""), 10)) : .AddWithValue("UID", userid)
            .AddWithValue("URL", iUrl) : .AddWithValue("REQSTR", iQString) : .AddWithValue("ERRMSG", strEx) : .AddWithValue("CLIENTINFO", _HTTP_USER_AGENT)
        End With
        conn.Open() : cmd.ExecuteNonQuery() : conn.Close()
        'Catch ex As Exception

        'End Try
    End Sub
    Public Function CreateSO(ByRef refDoc_Number As String, ByRef ErrMsg As String,
                            ByRef OrderHeaderDt As SalesOrder.OrderHeaderDataTable, ByRef OrderLineDt As SalesOrder.OrderLinesDataTable,
                            ByRef PartnerFuncDT As SalesOrder.PartnerFuncDataTable, ByRef ConditionDT As SalesOrder.ConditionDataTable,
                            ByRef HeaderTextsDt As SalesOrder.HeaderTextsDataTable, ByRef CreditCardDT As SalesOrder.CreditCardDataTable,
                            ByRef retDataTableDT As DataTable, ByVal LocalTime As DateTime) As Boolean

        'If refDoc_Number = "" Then
        '    ErrMsg = "NO ORDER NO!" : Return False
        'End If

        If OrderHeaderDt.Rows.Count <= 0 Then
            ErrMsg = "NO HEADER!" : Return False
        End If

        If OrderLineDt.Rows.Count <= 0 Then
            ErrMsg = "NO DETAIL!" : Return False
        End If

        If PartnerFuncDT.Rows.Count <= 0 Then
            ErrMsg = "NO PARTNER FUNC!" : Return False
        End If
        Dim proxy1 As New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SALESORDER_CREATEFROMDAT2
        Dim FF As Integer = 0
        Try

            Dim S_OrderHeader As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1, S_OrderLineDt As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMTable
            Dim S_PartnerFuncDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNRTable, S_ConditionDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDTable
            Dim S_HeaderTextsDt As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXTTable, S_ScheLineDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLTable
            Dim S_CreditCardDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARDTable
            Dim S_Addr1DT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1Table

            Dim OrderHeaderRow As SalesOrder.OrderHeaderRow = OrderHeaderDt.Rows(0)
            With OrderHeaderRow
                S_OrderHeader.Doc_Type = .ORDER_TYPE : S_OrderHeader.Sales_Org = .SALES_ORG : S_OrderHeader.Distr_Chan = .DIST_CHAN : S_OrderHeader.Division = .DIVISION
                If Not String.IsNullOrEmpty(.SalesGroup) AndAlso Not String.IsNullOrEmpty(.SalesOffice) Then
                    S_OrderHeader.Sales_Grp = .SalesGroup : S_OrderHeader.Sales_Off = .SalesOffice
                End If
                If Not String.IsNullOrEmpty(.Ref_Doc.Trim) Then
                    'S_OrderHeader.REF_DOC_L = .Ref_Doc
                    'S_OrderHeader.Sd_Doc_Cat = "C"
                    S_OrderHeader.Ref_Doc = .Ref_Doc
                    S_OrderHeader.Refdoc_Cat = "B"
                End If
                If .Currency IsNot Nothing AndAlso Not String.IsNullOrEmpty(.Currency) Then
                    S_OrderHeader.Currency = .Currency
                End If
                S_OrderHeader.Doc_Date = LocalTime.ToString("yyyy/MM/dd")
                'S_OrderHeader.Price_Date = LocalTime.ToString("yyyy/MM/dd")
                S_OrderHeader.Incoterms1 = .INCO1 : S_OrderHeader.Incoterms2 = .INCO2
                S_OrderHeader.Taxdep_Cty = .SHIPTO_COUNTRY : S_OrderHeader.Alttax_Cls = .TAX_CLASS : S_OrderHeader.Eutri_Deal = .TRIANGULAR_INDICATOR
                S_OrderHeader.Req_Date_H = .REQUIRE_DATE : S_OrderHeader.Ship_Cond = .SHIP_CONDITION : S_OrderHeader.Purch_No_C = .CUST_PO_NO
                S_OrderHeader.Purch_No_S = .SHIP_CUST_PO_NO : S_OrderHeader.Purch_Date = .PO_DATE : S_OrderHeader.Compl_Dlv = .PARTIAL_SHIPMENT
                S_OrderHeader.S_Proc_Ind = .EARLY_SHIP : S_OrderHeader.Taxdep_Cty = .TAXDEL_CTY : S_OrderHeader.Taxdst_Cty = .TAXDES_CTY
                If .VERSION IsNot Nothing AndAlso Not String.IsNullOrEmpty(.VERSION) Then
                    S_OrderHeader.Version = .VERSION
                End If
                If Not IsDBNull(.DISTRICT) AndAlso .DISTRICT.ToString.Trim <> "" Then
                    S_OrderHeader.Sales_Dist = .DISTRICT
                End If
                If String.IsNullOrEmpty(.PAYTERM) = False Then
                    S_OrderHeader.Pmnttrms = .PAYTERM
                    If S_CreditCardDT.Count > 0 Then

                    End If
                End If
                '20150326 TC: For AJP CTOS order, if component is manually added instead of via eConfigurator, then set delivery block 20 (Verify BTO Config.)
                S_OrderHeader.Dlv_Block = .DLV_BLOCK
            End With
            For Each r As SalesOrder.OrderLinesRow In OrderLineDt.Rows
                Dim S_OrderLineRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITM, S_ScheLineRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDL, S_ConditionRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND
                With r
                    S_OrderLineRow.Part_Dlv = "" : S_OrderLineRow.Hg_Lv_Item = .HIGHER_LEVEL : S_OrderLineRow.Itm_Number = .LINE_NO
                    S_OrderLineRow.Dlv_Group = .DELIVERY_GROUP : S_OrderLineRow.Plant = .PLANT : S_OrderLineRow.Material = .MATERIAL
                    S_OrderLineRow.Cust_Mat35 = .CUST_MATERIAL : S_OrderLineRow.Usage_Ind = .DMF_FLAG
                    S_ScheLineRow.Itm_Number = .LINE_NO : S_ScheLineRow.Req_Qty = .QTY : S_ScheLineRow.Req_Date = .REQ_DATE
                    S_ConditionRow.Itm_Number = .LINE_NO : S_ConditionRow.Cond_Type = "ZPN0" : S_ConditionRow.Currency = .CURRENCY
                    If Decimal.TryParse(.PRICE, 0) Then
                        S_ConditionRow.Cond_Value = Decimal.Parse(.PRICE)
                        'Frank 20170323
                        If OrderHeaderRow.SALES_ORG.StartsWith("CN", StringComparison.InvariantCultureIgnoreCase) Then
                            S_ConditionRow.Cond_Value = Decimal.Parse(.PRICE) / (1 + ConfigurationManager.AppSettings("ACNTaxRate"))
                        End If

                    End If
                    S_OrderLineRow.Short_Text = .Description
                    S_OrderLineRow.Ship_Point = .ShipPoint : S_OrderLineRow.Store_Loc = .StorageLoc

                    'Ryan 20170329 AJP特例，在DueDateReset AJP實際上儲存的是cust_po_no到每個Line Items, 
                    'AJP不使用CUST_MATERIAL欄位，故AJP的該欄位清空，並且要寫入cust_po_no欄位
                    If S_OrderHeader.Sales_Org.StartsWith("JP") Then
                        S_OrderLineRow.Cust_Mat35 = ""
                        S_OrderLineRow.Purch_No_C = .CUST_MATERIAL
                    End If

                    'ICC 20170720 ACN 中科專案，只要Material是CM-開頭，就將cust material & cust po no 都填上原始料號 Ex.CM-10A3-T4A719901
                    If S_OrderHeader.Sales_Org.StartsWith("CN") AndAlso .CUST_MATERIAL.ToUpper.StartsWith("CM-", StringComparison.InvariantCultureIgnoreCase) = True Then
                        S_OrderLineRow.Cust_Mat35 = .CUST_MATERIAL
                        S_OrderLineRow.Purch_No_C = .CUST_MATERIAL
                    End If

                    'JJ 2014/2/26：當TW01的單子有968T開頭的料號時，將ZTB1塞入ItCa欄位
                    If r.ItCa IsNot Nothing AndAlso Not String.IsNullOrEmpty(r.ItCa) Then
                        S_OrderLineRow.Item_Categ = r.ItCa
                    End If

                End With
                S_OrderLineDt.Add(S_OrderLineRow) : S_ScheLineDT.Add(S_ScheLineRow)
                'ICC 2014/10/24 Check ItemCategoryGroup. If it is ZTN3 then do not add to condition
                If S_OrderLineRow.Item_Categ IsNot Nothing AndAlso S_OrderLineRow.Item_Categ = "ZTN3" Then
                Else
                    S_ConditionDT.Add(S_ConditionRow)
                End If
                'ICC 2017/01/16 For SRP part no. Get language pack in SRP_ORDER_LANGUAGE table
                If r.MATERIAL.StartsWith("SRP-") Then
                    Dim LangPack As Object = dbUtil.dbExecuteScalar("MY", String.Format("SELECT TOP 1 Language_Pack FROM SRP_ORDER_LANGUAGE s INNER JOIN Cart2OrderMaping cm ON s.Cart_ID = cm.CartID WHERE cm.OrderNo ='{0}' AND s.Part_No = '{1}' AND s.Line_No = {2} ", refDoc_Number, r.MATERIAL, r.LINE_NO))
                    If Not LangPack Is Nothing AndAlso Not String.IsNullOrEmpty(LangPack.ToString) Then
                        Dim SrpHeader As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXT()
                        SrpHeader.Doc_Number = refDoc_Number
                        SrpHeader.Text_Id = "0001"
                        SrpHeader.Text_Line = LangPack.ToString
                        SrpHeader.Langu = "EN"
                        SrpHeader.Itm_Number = r.LINE_NO
                        S_HeaderTextsDt.Add(SrpHeader)
                    End If
                End If
            Next

            For Each r As SalesOrder.PartnerFuncRow In PartnerFuncDT.Rows
                Dim S_PartnerFuncRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNR
                With r
                    S_PartnerFuncRow.Partn_Role = .ROLE : S_PartnerFuncRow.Partn_Numb = .NUMBER

                    'Ryan 20180410 Also allow ADLoG to send ship-to data.
                    'Ryan 20171024 Currently only allow AJP/BBUS to overwrite ship-to data.
                    'Ryan 20171023 Type ZP/ZQ included.
                    'Ryan 20170512 AJP users are able to input ship-to data manually. Data are from OrderInfo page.
                    If (S_OrderHeader.Sales_Org.Equals("JP01") OrElse S_OrderHeader.Sales_Org.Equals("US10") OrElse S_OrderHeader.Sales_Org.Equals("EU80")) AndAlso
                        (.ROLE = "WE" OrElse .ROLE = "ZP" OrElse .ROLE = "ZQ") Then

                        Dim OPType As String = String.Empty, AddrLinkNo As String = String.Empty
                        If .ROLE = "WE" Then
                            OPType = "S"
                            AddrLinkNo = "000001"
                        ElseIf .ROLE = "ZP" Then
                            OPType = "ZP"
                            AddrLinkNo = "000002"
                        ElseIf .ROLE = "ZQ" Then
                            OPType = "ZQ"
                            AddrLinkNo = "000003"
                        End If

                        ' Only overwrite address data to SAP if necessary
                        If IsOrderPartnerAddressDataChanged(refDoc_Number, .NUMBER, OPType) Then

                            Dim ShiptoDT As DataTable = dbUtil.dbGetDataTable("MY", String.Format("SELECT * FROM ORDER_PARTNERS WHERE ORDER_ID = '{0}' AND TYPE = '{1}'", refDoc_Number, OPType))
                            If Not ShiptoDT Is Nothing AndAlso ShiptoDT.Rows.Count > 0 Then
                                'S_PartnerFuncRow.Addr_Link is a self-define value which maps to S_Addr1Row.Addr_No
                                S_PartnerFuncRow.Addr_Link = AddrLinkNo

                                Dim S_Addr1Row As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1
                                S_Addr1Row.Addr_No = AddrLinkNo

                                S_Addr1Row.Title = "Company"
                                S_Addr1Row.Name = ShiptoDT.Rows(0)("NAME").ToString
                                S_Addr1Row.Country = ShiptoDT.Rows(0)("COUNTRY").ToString
                                S_Addr1Row.Street = ShiptoDT.Rows(0)("STREET").ToString
                                S_Addr1Row.Str_Suppl3 = ShiptoDT.Rows(0)("STREET2").ToString
                                S_Addr1Row.City = ShiptoDT.Rows(0)("CITY").ToString
                                S_Addr1Row.C_O_Name = ShiptoDT.Rows(0)("ATTENTION").ToString
                                S_Addr1Row.Region = ShiptoDT.Rows(0)("STATE").ToString
                                S_Addr1Row.Tel1_Numbr = ShiptoDT.Rows(0)("TEL").ToString
                                S_Addr1Row.Postl_Cod1 = ShiptoDT.Rows(0)("ZIPCODE").ToString
                                S_Addr1Row.E_Mail = ShiptoDT.Rows(0)("ADDRESS").ToString
                                S_Addr1Row.Taxjurcode = ShiptoDT.Rows(0)("TAXJURI").ToString
                                S_Addr1Row.Langu = "English"

                                'Get Data From SAP.ADRC Table to fill Communication Method and Time Zone fields
                                Dim ADRCDT As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", "select NVL(b.DEFLT_COMM,'') as CM, NVL(b.TIME_ZONE,'') as TZ, NVL(b.TRANSPZONE,'') as TPZ from saprdp.kna1 a inner join saprdp.ADRC b on a.adrnr = b.ADDRNUMBER where a.kunnr = '" + .NUMBER + "'")
                                If ADRCDT IsNot Nothing AndAlso ADRCDT.Rows.Count > 0 Then
                                    If Not String.IsNullOrEmpty(ADRCDT.Rows(0)("CM").ToString) Then
                                        S_Addr1Row.Comm_Type = ADRCDT.Rows(0)("CM").ToString
                                    End If
                                    If Not String.IsNullOrEmpty(ADRCDT.Rows(0)("TZ").ToString) Then
                                        S_Addr1Row.Time_Zone = ADRCDT.Rows(0)("TZ").ToString
                                    End If
                                    If Not String.IsNullOrEmpty(ADRCDT.Rows(0)("TPZ").ToString) Then
                                        S_Addr1Row.Transpzone = ADRCDT.Rows(0)("TPZ").ToString
                                    End If
                                End If

                                S_Addr1DT.Add(S_Addr1Row)
                            End If
                        End If
                    ElseIf S_OrderHeader.Sales_Org.Equals("US10") AndAlso .ROLE = "AP" Then
                        ' Create AP as new address for BBUS further multiple emails insertion
                        S_PartnerFuncRow.Addr_Link = "000009"

                        Dim APDT As DataTable = dbUtil.dbGetDataTable("MY", String.Format("SELECT * FROM ORDER_PARTNERS WHERE ORDER_ID = '{0}' AND TYPE = 'AP'", refDoc_Number))
                        Dim S_AddrAPRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1
                        S_AddrAPRow.Addr_No = "000009"

                        S_AddrAPRow.Title = "Mr. and Mrs."
                        S_AddrAPRow.Name = APDT.Rows(0)("NAME").ToString
                        S_AddrAPRow.E_Mail = APDT.Rows(0)("ADDRESS").ToString
                        S_AddrAPRow.Langu = "English"
                        S_Addr1DT.Add(S_AddrAPRow)
                    End If
                End With
                S_PartnerFuncDT.Add(S_PartnerFuncRow)
            Next

            If Not IsNothing(HeaderTextsDt) AndAlso HeaderTextsDt.Rows.Count > 0 Then
                For Each r As SalesOrder.HeaderTextsRow In HeaderTextsDt.Rows
                    With r
                        Dim StartP As Integer = 1, LongP As Integer = 100, oLine As String = Mid(.TEXT_LINE, StartP, LongP)
                        While oLine.Trim.Length > 0
                            Dim S_HeaderTextsRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXT
                            S_HeaderTextsRow.Doc_Number = refDoc_Number : S_HeaderTextsRow.Text_Id = .TEXT_ID
                            S_HeaderTextsRow.Text_Line = oLine : S_HeaderTextsRow.Langu = .LANG_ID

                            'Ryan 20180810 If line id is defined, text goes to item level not order header.
                            If .LINE_ID IsNot Nothing AndAlso Not String.IsNullOrEmpty(.LINE_ID) Then
                                S_HeaderTextsRow.Itm_Number = .LINE_ID
                            End If
                            S_HeaderTextsDt.Add(S_HeaderTextsRow)
                            StartP = StartP + 100 : oLine = Mid(.TEXT_LINE, StartP, LongP)
                        End While
                    End With
                Next
            End If

            If Not IsNothing(ConditionDT) AndAlso ConditionDT.Rows.Count > 0 Then

                'Ryan 20170918 Special case for BBUS
                If S_OrderHeader.Sales_Org.ToUpper.Equals("US10") Then
                    Dim r As SalesOrder.ConditionRow = ConditionDT.Rows(0)
                    Dim FirstLineNo As Integer = S_OrderLineDt(0).Itm_Number
                    Dim ConditionRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND
                    ConditionRow.Itm_Number = FirstLineNo
                    ConditionRow.Cond_Type = r.TYPE
                    ConditionRow.Currency = r.CURRENCY
                    ConditionRow.Cond_Value = r.VALUE
                    S_ConditionDT.Add(ConditionRow)
                Else
                    For Each r As SalesOrder.ConditionRow In ConditionDT.Rows
                        Dim S_ConditionRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND
                        With r
                            S_ConditionRow.Itm_Number = "000000" : S_ConditionRow.Cond_Type = .TYPE : S_ConditionRow.Currency = .CURRENCY : S_ConditionRow.Cond_Value = .VALUE
                        End With
                        S_ConditionDT.Add(S_ConditionRow)
                    Next
                End If
            End If

            If Not IsNothing(CreditCardDT) AndAlso CreditCardDT.Rows.Count > 0 Then
                For Each r As SalesOrder.CreditCardRow In CreditCardDT.Rows
                    Dim S_CreditCardRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARD
                    With r
                        S_CreditCardRow.Cc_Name = .HOLDER : S_CreditCardRow.Cc_Number = .NUMBER : S_CreditCardRow.Cc_Type = .TYPE
                        S_CreditCardRow.Cc_Valid_T = .EXPIRED : S_CreditCardRow.Cc_Verif_Value = .VERIFICATION_VALUE

                        'Ryan 20171130 Add for Authorize.net intergration
                        If Not String.IsNullOrEmpty(.AUTH_REFNO) AndAlso Not String.IsNullOrEmpty(.AUTH_CC_NO) AndAlso Decimal.TryParse(.AUTH_AMOUNT, 0) Then
                            S_CreditCardRow.Auth_Flag = "X"
                            S_CreditCardRow.Auth_Refno = .AUTH_REFNO : S_CreditCardRow.Auth_Cc_No = .AUTH_CC_NO
                            S_CreditCardRow.Authamount = .AUTH_AMOUNT : S_CreditCardRow.Currency = S_OrderHeader.Currency
                        End If
                    End With
                    S_CreditCardDT.Add(S_CreditCardRow)

                Next
            End If
            'For IT testing purpose
            'OrderHeaderRow.DEST_TYPE = 1
            If OrderHeaderRow.DEST_TYPE = 0 Then
                proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            Else
                proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAPConnTest"))
            End If
            proxy1.Connection.Open()

            'Frank 20170117
            Dim _TimeOutSecond As Integer = 1
            Integer.TryParse(ConfigurationManager.AppSettings("BAPITimeout"), _TimeOutSecond)
            proxy1.Timeout = _TimeOutSecond * 60 * 1000

            ''Test code
            'proxy1.Timeout = 2000
            'System.Threading.Thread.Sleep(3000)
            'Throw New Exception("test by frank")

            Dim strRelationType As String = ""
            Dim strPConvert As String = ""
            Dim strpintnumassign As String = "", strPTestRun As String = "", retTable As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIRET2Table

            'Dim dtcon As New DataTable
            'dtcon = S_CreditCardDT.ToADODataTable()
            'Dim str As String = Global_Inc.getDTHtml(dtcon)
            'Global_Inc.Utility_EMailPage("nada.liu@advantech.com.cn", "nada.liu@advantech.com.cn", "", "", _
            '   "so_create return 0", "", Str)




            proxy1.Bapi_Salesorder_Createfromdat2(
            ErrMsg, strRelationType, strPConvert,
            strpintnumassign, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDLS, S_OrderHeader, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1X,
            refDoc_Number, New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SENDER, strPTestRun, refDoc_Number,
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPAREXTable, S_CreditCardDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUBLBTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUINSTable,
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUPRTTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUCFGTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUREFTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVALTable,
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVKTable, S_ConditionDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDXTable,
            S_OrderLineDt, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMXTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDKEYTable,
            S_PartnerFuncDT, S_ScheLineDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLXTable, S_HeaderTextsDt, S_Addr1DT, retTable)
            retDataTableDT = retTable.ToADODataTable()
            For retRowCount = 0 To retDataTableDT.Rows.Count - 1
                If retDataTableDT.Rows(retRowCount).Item("Number") = "311" Then
                    FF = 1
                    Exit For
                End If
            Next
            If FF = 1 Then
                If Not proxy1.CommitWork() Then
                    Throw New Exception("Commit sales order to SAP failed")
                End If
            End If
            proxy1.Connection.Close()

            If S_CreditCardDT.Count > 0 AndAlso FF = 1 Then
                '20120726 TC: Try to sleep two seconds to see if this can tick authorization block successfully
                Dim pAuthBlock As New ZSD_UPDATE_FPLA.ZSD_UPDATE_FPLA
                Dim DT_LOG As New ZSD_UPDATE_FPLA.SWF_LINESTable
                Dim dt As New DataTable
                pAuthBlock.Connection = New SAP.Connector.SAPConnection(proxy1.ConnectionString)
                pAuthBlock.Connection.Open()
                For i As Integer = 0 To 3
                    Threading.Thread.Sleep(2000)
                    pAuthBlock.Zsd_Update_Fpla("X", refDoc_Number, DT_LOG)
                    dt = DT_LOG.ToADODataTable
                    If dt.Rows.Count > 0 AndAlso dt.Rows(0).Item("Line").ToString.Contains("successful") Then
                        Exit For
                    End If
                Next
                If dt.Rows.Count > 0 AndAlso (Not dt.Rows(0).Item("Line").ToString.Contains("successful")) Then
                    'logUpdateAuthBlockFlag(refDoc_Number)
                    InsertMyErrLog("Failed to tick Authorization Block for SO " & refDoc_Number)
                End If
                pAuthBlock.Connection.Close()
            End If
        Catch mex As Exception
            If Not IsNothing(proxy1) AndAlso Not IsNothing(proxy1.Connection) Then
                proxy1.Connection.Close()
            End If
            ErrMsg = mex.ToString()
            'Frank 20170117 log error message
            InsertMyErrLog(ErrMsg)
            Return False
        End Try

        If FF = 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function UpdateSAPSOShipToAttention(ByVal SONO As String, ByVal ShipToId As String, ByVal Attention As String, ByRef ReturnTable As DataTable,
                                       Optional IsSAPProductionServer As Boolean = True) As Boolean
        Dim retbool As Boolean = False
        Dim dtDefaultAddrTable As SalesOrder.PartnerAddressesDataTable = GetSAPPartnerAddressesTableByKunnr(ShipToId, IsSAPProductionServer)
        If dtDefaultAddrTable.Count > 0 Then
            Dim dtDefaultAddrRow = dtDefaultAddrTable(0)
            Dim p1 As New Change_SD_Order.Change_SD_Order()
            If IsSAPProductionServer Then
                p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            Else
                p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAPConnTest"))
            End If
            Dim OrderHeader As New Change_SD_Order.BAPISDH1, OrderHeaderX As New Change_SD_Order.BAPISDH1X
            Dim ItemIn As New Change_SD_Order.BAPISDITMTable, PartNr As New Change_SD_Order.BAPIPARNRTable
            Dim Condition As New Change_SD_Order.BAPICONDTable, ScheLine As New Change_SD_Order.BAPISCHDLTable
            Dim ScheLineX As New Change_SD_Order.BAPISCHDLXTable, OrderText As New Change_SD_Order.BAPISDTEXTTable
            Dim sales_note As New Change_SD_Order.BAPISDTEXT, ext_note As New Change_SD_Order.BAPISDTEXT
            Dim op_note As New Change_SD_Order.BAPISDTEXT, retTable As New Change_SD_Order.BAPIRET2Table
            Dim ADDRTable As New Change_SD_Order.BAPIADDR1Table
            Dim PartnerChangeTable As New Change_SD_Order.BAPIPARNRCTable
            Dim Doc_Number As String = SONO
            OrderHeaderX.Updateflag = "U"


            Dim ADDRRow1 As New Change_SD_Order.BAPIADDR1, PartnerChangeRow1 As New Change_SD_Order.BAPIPARNRC
            With ADDRRow1
                .Addr_No = "1" : .C_O_Name = Attention
                .City = dtDefaultAddrRow.City : .Comm_Type = dtDefaultAddrRow.Comm_Type : .Country = dtDefaultAddrRow.Country
                .Region = dtDefaultAddrRow.Region_str
                .Distrct_No = dtDefaultAddrRow.Distrct_No : .District = dtDefaultAddrRow.District
                .Dont_Use_P = dtDefaultAddrRow.Dont_Use_P : .Dont_Use_S = dtDefaultAddrRow.Dont_Use_S
                .E_Mail = dtDefaultAddrRow.E_Mail : .Fax_Extens = dtDefaultAddrRow.Fax_Extens : .Fax_Number = dtDefaultAddrRow.Fax_Number
                .Floor = dtDefaultAddrRow.Floor : .Langu = dtDefaultAddrRow.Langu : .Location = dtDefaultAddrRow.Location
                .Name = dtDefaultAddrRow.Name : .Name_2 = dtDefaultAddrRow.Name_2 : .Name_3 = dtDefaultAddrRow.Name_3
                .Name_4 = dtDefaultAddrRow.Name_4 : .Pboxcit_No = dtDefaultAddrRow.Pboxcit_No
                .Pcode1_Ext = dtDefaultAddrRow.Pcode1_Ext : .Pcode2_Ext = dtDefaultAddrRow.Pcode2_Ext
                .Pcode3_Ext = dtDefaultAddrRow.Pcode3_Ext : .Po_Box = dtDefaultAddrRow.Po_Box
                .Po_Box_Cit = dtDefaultAddrRow.Po_Box_Cit : .Po_Box_Reg = dtDefaultAddrRow.Po_Box_Reg
                .Pobox_Ctry = dtDefaultAddrRow.Pobox_Ctry : .Postl_Cod1 = dtDefaultAddrRow.Postl_Cod1
                .Postl_Cod2 = dtDefaultAddrRow.Postl_Cod2 : .Postl_Cod3 = dtDefaultAddrRow.Postl_Cod3
                .Regiogroup = dtDefaultAddrRow.Regiogroup : .Street = dtDefaultAddrRow.Street
                .Taxjurcode = dtDefaultAddrRow.Taxjurcode : .Tel1_Ext = dtDefaultAddrRow.Tel1_Ext
                .Tel1_Numbr = dtDefaultAddrRow.Tel1_Numbr : .Time_Zone = dtDefaultAddrRow.Time_Zone
                .Title = dtDefaultAddrRow.Title : .Transpzone = dtDefaultAddrRow.Transpzone : .Region = dtDefaultAddrRow.Region_str
            End With
            With PartnerChangeRow1
                .Document = Doc_Number
                .Addr_Link = "1" : .Address = "" : .P_Numb_New = ShipToId : .P_Numb_Old = ShipToId : .Partn_Role = "WE" : .Updateflag = "U"
            End With

            ADDRTable.Add(ADDRRow1) : PartnerChangeTable.Add(PartnerChangeRow1)
            Try
                p1.Connection.Open()
                p1.Bapi_Salesorder_Change("", "", New Change_SD_Order.BAPISDLS, OrderHeader, OrderHeaderX, Doc_Number, "", Condition,
                    New Change_SD_Order.BAPICONDXTable, New Change_SD_Order.BAPIPAREXTable, New Change_SD_Order.BAPICUBLBTable,
                    New Change_SD_Order.BAPICUINSTable, New Change_SD_Order.BAPICUPRTTable, New Change_SD_Order.BAPICUCFGTable,
                    New Change_SD_Order.BAPICUREFTable, New Change_SD_Order.BAPICUVALTable, New Change_SD_Order.BAPICUVKTable, ItemIn,
                    New Change_SD_Order.BAPISDITMXTable, New Change_SD_Order.BAPISDKEYTable, OrderText, ADDRTable,
                    PartnerChangeTable, PartNr, retTable, ScheLine, ScheLineX)
                p1.CommitWork() : p1.Connection.Close()
                retbool = True
            Catch ex As Exception
            End Try
            ReturnTable = retTable.ToADODataTable()
            Return retbool
        End If
    End Function
    Public Function CreateSOV2(ByRef refDoc_Number As String, ByRef ErrMsg As String,
                            ByRef OrderHeaderDt As SalesOrder.OrderHeaderDataTable, ByRef OrderLineDt As SalesOrder.OrderLinesDataTable,
                            ByRef PartnerFuncDT As SalesOrder.PartnerFuncDataTable, ByVal PartnerAddressDT As SalesOrder.PartnerAddressesDataTable, ByRef ConditionDT As SalesOrder.ConditionDataTable,
                            ByRef HeaderTextsDt As SalesOrder.HeaderTextsDataTable, ByRef CreditCardDT As SalesOrder.CreditCardDataTable,
                            ByRef retDataTableDT As DataTable, ByVal LocalTime As DateTime) As Boolean

        If refDoc_Number = "" Then
            ErrMsg = "NO ORDER NO!" : Return False
        End If

        If OrderHeaderDt.Rows.Count <= 0 Then
            ErrMsg = "NO HEADER!" : Return False
        End If

        If OrderLineDt.Rows.Count <= 0 Then
            ErrMsg = "NO DETAIL!" : Return False
        End If

        If PartnerFuncDT.Rows.Count <= 0 Then
            ErrMsg = "NO PARTNER FUNC!" : Return False
        End If
        Dim proxy1 As New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SALESORDER_CREATEFROMDAT2
        Dim FF As Integer = 0
        Try

            Dim S_OrderHeader As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1, S_OrderLineDt As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMTable
            Dim S_PartnerFuncDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNRTable, S_ConditionDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDTable
            Dim S_HeaderTextsDt As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXTTable, S_ScheLineDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLTable
            Dim S_CreditCardDT As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARDTable
            Dim S_PartnerAddresses As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1Table
            Dim OrderHeaderRow As SalesOrder.OrderHeaderRow = OrderHeaderDt.Rows(0)
            With OrderHeaderRow
                S_OrderHeader.Doc_Type = .ORDER_TYPE : S_OrderHeader.Sales_Org = .SALES_ORG : S_OrderHeader.Distr_Chan = .DIST_CHAN : S_OrderHeader.Division = .DIVISION
                If Not String.IsNullOrEmpty(.SalesGroup) AndAlso Not String.IsNullOrEmpty(.SalesOffice) Then
                    S_OrderHeader.Sales_Grp = .SalesGroup : S_OrderHeader.Sales_Off = .SalesOffice
                End If
                If Not String.IsNullOrEmpty(.DISTRICT) Then
                    S_OrderHeader.Sales_Dist = .DISTRICT
                End If
                S_OrderHeader.Doc_Date = LocalTime.ToString("yyyy/MM/dd")
                'S_OrderHeader.Price_Date = LocalTime.ToString("yyyy/MM/dd")
                S_OrderHeader.Incoterms1 = .INCO1 : S_OrderHeader.Incoterms2 = .INCO2
                S_OrderHeader.Taxdep_Cty = .SHIPTO_COUNTRY : S_OrderHeader.Alttax_Cls = .TAX_CLASS : S_OrderHeader.Eutri_Deal = .TRIANGULAR_INDICATOR
                S_OrderHeader.Req_Date_H = .REQUIRE_DATE : S_OrderHeader.Ship_Cond = .SHIP_CONDITION : S_OrderHeader.Purch_No_C = .CUST_PO_NO
                S_OrderHeader.Purch_No_S = .SHIP_CUST_PO_NO : S_OrderHeader.Purch_Date = .PO_DATE : S_OrderHeader.Compl_Dlv = .PARTIAL_SHIPMENT
                S_OrderHeader.S_Proc_Ind = .EARLY_SHIP : S_OrderHeader.Taxdep_Cty = .TAXDEL_CTY : S_OrderHeader.Taxdst_Cty = .TAXDES_CTY
                If String.IsNullOrEmpty(.PAYTERM) = False Then
                    S_OrderHeader.Pmnttrms = .PAYTERM
                    If S_CreditCardDT.Count > 0 Then

                    End If
                End If
            End With
            For Each r As SalesOrder.OrderLinesRow In OrderLineDt.Rows
                Dim S_OrderLineRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITM, S_ScheLineRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDL, S_ConditionRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND
                With r
                    S_OrderLineRow.Part_Dlv = "" : S_OrderLineRow.Hg_Lv_Item = .HIGHER_LEVEL : S_OrderLineRow.Itm_Number = .LINE_NO
                    S_OrderLineRow.Dlv_Group = .DELIVERY_GROUP : S_OrderLineRow.Plant = .PLANT : S_OrderLineRow.Material = .MATERIAL
                    S_OrderLineRow.Cust_Mat35 = .CUST_MATERIAL : S_OrderLineRow.Usage_Ind = .DMF_FLAG
                    S_ScheLineRow.Itm_Number = .LINE_NO : S_ScheLineRow.Req_Qty = .QTY : S_ScheLineRow.Req_Date = .REQ_DATE
                    S_ConditionRow.Itm_Number = .LINE_NO : S_ConditionRow.Cond_Type = "ZPN0" : S_ConditionRow.Currency = .CURRENCY : S_ConditionRow.Cond_Value = .PRICE
                    S_OrderLineRow.Short_Text = .Description
                    S_OrderLineRow.Ship_Point = .ShipPoint : S_OrderLineRow.Store_Loc = .StorageLoc
                End With
                S_OrderLineDt.Add(S_OrderLineRow) : S_ScheLineDT.Add(S_ScheLineRow) : S_ConditionDT.Add(S_ConditionRow)

            Next

            For Each r As SalesOrder.PartnerFuncRow In PartnerFuncDT.Rows
                Dim S_PartnerFuncRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPARNR
                With r
                    S_PartnerFuncRow.Partn_Role = .ROLE : S_PartnerFuncRow.Partn_Numb = .NUMBER
                    If S_PartnerFuncRow.Partn_Role.Equals("WE", StringComparison.OrdinalIgnoreCase) Then
                        If PartnerAddressDT.Rows.Count > 0 Then
                            Dim PtnrAdressRow As SalesOrder.PartnerAddressesRow = PartnerAddressDT.Rows(0)
                            S_PartnerFuncRow.Addr_Link = PtnrAdressRow.Addr_No
                        End If
                    End If
                End With
                S_PartnerFuncDT.Add(S_PartnerFuncRow)
            Next
            For Each r As SalesOrder.PartnerAddressesRow In PartnerAddressDT.Rows
                With r
                    Dim S_PartnerAddressesRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1
                    S_PartnerAddressesRow.C_O_Name = .C_O_Name
                    S_PartnerAddressesRow.Addr_No = .Addr_No
                    S_PartnerAddressesRow.Adr_Notes = .Adr_Notes
                    S_PartnerAddressesRow.Build_Long = .Build_Long
                    S_PartnerAddressesRow.Building = .Building
                    S_PartnerAddressesRow.Chckstatus = .Chckstatus
                    S_PartnerAddressesRow.City = .City
                    S_PartnerAddressesRow.City_No = .City_No
                    S_PartnerAddressesRow.Comm_Type = .Comm_Type
                    S_PartnerAddressesRow.Country = .Country
                    S_PartnerAddressesRow.Countryiso = .Countryiso
                    S_PartnerAddressesRow.Deliv_Dis = .Deliv_Dis
                    S_PartnerAddressesRow.Distrct_No = .Distrct_No
                    S_PartnerAddressesRow.District = .District
                    S_PartnerAddressesRow.Dont_Use_P = .Dont_Use_P
                    S_PartnerAddressesRow.Dont_Use_S = .Dont_Use_S
                    S_PartnerAddressesRow.E_Mail = .E_Mail
                    S_PartnerAddressesRow.Fax_Extens = .Fax_Extens
                    S_PartnerAddressesRow.Fax_Number = .Fax_Number
                    S_PartnerAddressesRow.Floor = .Floor
                    S_PartnerAddressesRow.Formofaddr = .Formofaddr
                    S_PartnerAddressesRow.Home_City = .Home_City
                    S_PartnerAddressesRow.Homecityno = .Homecityno
                    S_PartnerAddressesRow.Homepage = .Homepage
                    S_PartnerAddressesRow.House_No = .House_No
                    S_PartnerAddressesRow.House_No2 = .House_No2
                    S_PartnerAddressesRow.House_No3 = .House_No3
                    S_PartnerAddressesRow.Langu = .Langu
                    S_PartnerAddressesRow.Langu_Cr = .Langu_Cr
                    S_PartnerAddressesRow.Langu_Iso = .Langu_Iso
                    S_PartnerAddressesRow.Langucriso = .Langucriso
                    S_PartnerAddressesRow.Location = .Location
                    S_PartnerAddressesRow.Name = .Name
                    S_PartnerAddressesRow.Name_2 = .Name_2
                    S_PartnerAddressesRow.Name_3 = .Name_3
                    S_PartnerAddressesRow.Name_4 = .Name_4
                    S_PartnerAddressesRow.Pboxcit_No = .Pboxcit_No
                    S_PartnerAddressesRow.Pcode1_Ext = .Pcode1_Ext
                    S_PartnerAddressesRow.Pcode2_Ext = .Pcode2_Ext
                    S_PartnerAddressesRow.Pcode3_Ext = .Pcode3_Ext
                    S_PartnerAddressesRow.Po_Box = .Po_Box
                    S_PartnerAddressesRow.Po_Box_Cit = .Po_Box_Cit
                    S_PartnerAddressesRow.Po_Box_Reg = .Po_Box_Reg
                    S_PartnerAddressesRow.Po_Ctryiso = .Po_Ctryiso
                    S_PartnerAddressesRow.Po_W_O_No = .Po_W_O_No
                    S_PartnerAddressesRow.Pobox_Ctry = .Pobox_Ctry
                    S_PartnerAddressesRow.Postl_Cod1 = .Postl_Cod1
                    S_PartnerAddressesRow.Postl_Cod2 = .Postl_Cod2
                    S_PartnerAddressesRow.Postl_Cod3 = .Postl_Cod3
                    S_PartnerAddressesRow.Regiogroup = .Regiogroup
                    S_PartnerAddressesRow.Region = .Region_str
                    S_PartnerAddressesRow.Room_No = .Room_No
                    S_PartnerAddressesRow.Sort1 = .Sort1
                    S_PartnerAddressesRow.Sort2 = .Sort2
                    S_PartnerAddressesRow.Str_Abbr = .Str_Abbr
                    S_PartnerAddressesRow.Str_Suppl1 = .Str_Suppl1
                    S_PartnerAddressesRow.Str_Suppl2 = .Str_Suppl2
                    S_PartnerAddressesRow.Str_Suppl3 = .Str_Suppl3
                    S_PartnerAddressesRow.Street = .Street
                    S_PartnerAddressesRow.Street_Lng = .Street_Lng
                    S_PartnerAddressesRow.Street_No = .Street_No
                    S_PartnerAddressesRow.Taxjurcode = .Taxjurcode
                    S_PartnerAddressesRow.Tel1_Ext = .Tel1_Ext
                    S_PartnerAddressesRow.Tel1_Numbr = .Tel1_Numbr
                    S_PartnerAddressesRow.Time_Zone = .Time_Zone
                    S_PartnerAddressesRow.Title = .Title
                    S_PartnerAddressesRow.Transpzone = .Transpzone
                    S_PartnerAddresses.Add(S_PartnerAddressesRow)
                End With
            Next
            If Not IsNothing(HeaderTextsDt) AndAlso HeaderTextsDt.Rows.Count > 0 Then
                For Each r As SalesOrder.HeaderTextsRow In HeaderTextsDt.Rows
                    With r
                        Dim StartP As Integer = 1, LongP As Integer = 100, oLine As String = Mid(.TEXT_LINE, StartP, LongP)
                        While oLine.Trim.Length > 0
                            Dim S_HeaderTextsRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDTEXT
                            S_HeaderTextsRow.Doc_Number = refDoc_Number : S_HeaderTextsRow.Text_Id = .TEXT_ID
                            S_HeaderTextsRow.Text_Line = oLine : S_HeaderTextsRow.Langu = .LANG_ID : S_HeaderTextsDt.Add(S_HeaderTextsRow)
                            StartP = StartP + 100 : oLine = Mid(.TEXT_LINE, StartP, LongP)
                        End While
                    End With

                Next
            End If
            If Not IsNothing(ConditionDT) AndAlso ConditionDT.Rows.Count > 0 Then
                For Each r As SalesOrder.ConditionRow In ConditionDT.Rows
                    Dim S_ConditionRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICOND
                    With r
                        S_ConditionRow.Itm_Number = "000000" : S_ConditionRow.Cond_Type = .TYPE : S_ConditionRow.Currency = .CURRENCY : S_ConditionRow.Cond_Value = .VALUE
                    End With
                    S_ConditionDT.Add(S_ConditionRow)
                Next
            End If

            If Not IsNothing(CreditCardDT) AndAlso CreditCardDT.Rows.Count > 0 Then
                For Each r As SalesOrder.CreditCardRow In CreditCardDT.Rows
                    Dim S_CreditCardRow As New BAPI_SALESORDER_CREATEFROMDAT2.BAPICCARD
                    With r
                        S_CreditCardRow.Cc_Name = .HOLDER : S_CreditCardRow.Cc_Number = .NUMBER : S_CreditCardRow.Cc_Type = .TYPE
                        S_CreditCardRow.Cc_Valid_T = .EXPIRED : S_CreditCardRow.Cc_Verif_Value = .VERIFICATION_VALUE
                    End With
                    S_CreditCardDT.Add(S_CreditCardRow)

                Next
            End If
            'For IT testing purpose
            'OrderHeaderRow.DEST_TYPE = 1
            If OrderHeaderRow.DEST_TYPE = 0 Then
                proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            Else
                proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAPConnTest"))
            End If
            proxy1.Connection.Open()
            Dim strRelationType As String = ""
            Dim strPConvert As String = ""
            Dim strpintnumassign As String = "", strPTestRun As String = "", retTable As New BAPI_SALESORDER_CREATEFROMDAT2.BAPIRET2Table

            'Dim dtcon As New DataTable
            'dtcon = S_CreditCardDT.ToADODataTable()
            'Dim str As String = Global_Inc.getDTHtml(dtcon)
            'Global_Inc.Utility_EMailPage("nada.liu@advantech.com.cn", "nada.liu@advantech.com.cn", "", "", _
            '   "so_create return 0", "", Str)



            proxy1.Bapi_Salesorder_Createfromdat2(
            ErrMsg, strRelationType, strPConvert,
            strpintnumassign, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDLS, S_OrderHeader, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1X,
            refDoc_Number, New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SENDER, strPTestRun, refDoc_Number,
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPAREXTable, S_CreditCardDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUBLBTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUINSTable,
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUPRTTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUCFGTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUREFTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVALTable,
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVKTable, S_ConditionDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDXTable,
            S_OrderLineDt, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMXTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDKEYTable,
            S_PartnerFuncDT, S_ScheLineDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLXTable, S_HeaderTextsDt, S_PartnerAddresses, retTable)
            retDataTableDT = retTable.ToADODataTable()
            For retRowCount = 0 To retDataTableDT.Rows.Count - 1
                If retDataTableDT.Rows(retRowCount).Item("Number") = "311" Then
                    FF = 1
                    Exit For
                End If
            Next
            If FF = 1 Then
                proxy1.CommitWork()
            End If
            proxy1.Connection.Close()
            If S_CreditCardDT.Count > 0 AndAlso FF = 1 Then
                '20120726 TC: Try to sleep two seconds to see if this can tick authorization block successfully
                Threading.Thread.Sleep(2000)
                Dim pAuthBlock As New ZSD_UPDATE_FPLA.ZSD_UPDATE_FPLA
                pAuthBlock.Connection = New SAP.Connector.SAPConnection(proxy1.ConnectionString)
                pAuthBlock.Connection.Open()
                pAuthBlock.Zsd_Update_Fpla("X", refDoc_Number, Nothing)
                pAuthBlock.Connection.Close()
            End If

        Catch mex As Exception
            If Not IsNothing(proxy1) AndAlso Not IsNothing(proxy1.Connection) Then
                proxy1.Connection.Close()
            End If
            ErrMsg = mex.ToString()
            Return False
        End Try

        If FF = 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Sub CreatePo(ByVal POHead As PO_Head, ByVal POitems As List(Of PO_Item), ByRef result As String, ByRef retCode As Boolean)

        'Dim obj_FSO As System.IO.FileInfo = New System.IO.FileInfo("C:\B2B_SAP_WS\Lab\potest.xml")
        'Dim objFStrm As System.IO.StreamReader
        'objFStrm = obj_FSO.OpenText
        'inXml = objFStrm.ReadToEnd()
        'objFStrm.Close()

        Dim destination1 As Destination

        Dim SAPIP As String = "172.20.1.88"
        '    If POHead.IsTesting = 1 Then SAPIP = "172.20.1.1"
        Const SAPSN As String = "0"
        Const SAPID As String = "b2bacl"
        'Private Const SAPPWD As String = "acl"
        Const SAPPWD As String = "aclacl"
        Dim dest1 As New SAP.Connector.Destination
        dest1.AppServerHost = SAPIP : dest1.Client = "168" : dest1.Language = "" : dest1.SystemNumber = SAPSN
        '   If POHead.IsTesting = 1 Then dest1.SystemNumber = 1
        dest1.Username = SAPID : dest1.Password = SAPPWD

        Dim p1 As New Create_Po.Create_Po
        Dim PoHeader As New Create_Po.BAPIMEPOHEADER, PoHeaderX As New Create_Po.BAPIMEPOHEADERX
        Dim POItem As New Create_Po.BAPIMEPOITEMTable, PoItemX As New Create_Po.BAPIMEPOITEMXTable
        Dim POAccount As New Create_Po.BAPIMEPOACCOUNTTable, POAccountX As New Create_Po.BAPIMEPOACCOUNTXTable
        Dim PoSchedule As New Create_Po.BAPIMEPOSCHEDULETable, PoScheduleX As New Create_Po.BAPIMEPOSCHEDULXTable
        Dim returnTable As New Create_Po.BAPIRET2Table

        'Dim xmlDoc As New System.Xml.XmlDocument
        'xmlDoc.LoadXml(inXml)

        PoHeader.Po_Number = POHead.Po_Number ' xmlDoc.GetElementsByTagName("Po_Number").Item(0).InnerText
        PoHeader.Comp_Code = POHead.Comp_Code ' xmlDoc.GetElementsByTagName("Comp_Code").Item(0).InnerText
        PoHeader.Doc_Type = POHead.Doc_Type ' xmlDoc.GetElementsByTagName("Doc_Type").Item(0).InnerText
        PoHeader.Purch_Org = POHead.Purch_Org ' xmlDoc.GetElementsByTagName("Purch_Org").Item(0).InnerText
        PoHeader.Pur_Group = POHead.Purch_Group ' xmlDoc.GetElementsByTagName("Purch_Group").Item(0).InnerText
        PoHeader.Vendor = POHead.Vendor_ID ' xmlDoc.GetElementsByTagName("Vendor_ID").Item(0).InnerText
        'If PoHeader.Comp_Code = "JP01" Then
        '    PoHeader.Currency = "JPY"
        'Else
        '    PoHeader.Currency = "USD"
        'End If
        PoHeader.Currency = POHead.Currency
        PoHeaderX.Po_Number = "X" : PoHeaderX.Comp_Code = "X" : PoHeaderX.Doc_Type = "X"
        PoHeaderX.Purch_Org = "X" : PoHeaderX.Pur_Group = "X"
        PoHeaderX.Vendor = "X" : PoHeaderX.Currency = "X"
        ' Dim po_item As XmlNodeList = xmlDoc.GetElementsByTagName("Order_Line")
        For Each inode As PO_Item In POitems
            Dim item1 As New Create_Po.BAPIMEPOITEM
            ' If inode.SelectNodes("Matl_Group").Count = 0 OrElse inode.SelectNodes("Matl_Group").Item(0).InnerText.Trim() = "" Then
            If inode.Matl_Group = "" Then
                'component item
                item1.Tax_Code = inode.Tax_Code
                item1.Po_Item = inode.Po_Item
                '  item1.Preq_Name = POHead.Po_Number + "-" + inode.Po_Item
                item1.Material = inode.Material
                item1.Ematerial = inode.EMaterial
                item1.Plant = inode.Plant
                item1.Stge_Loc = inode.Storage_Location
                item1.Quantity = inode.Item_Qty
                item1.Net_Price = inode.Net_Price
                'item1.Preq_Name = POHead.Po_Number + "-" + inode.Po_Item ' inode.Preq_Name
                item1.Preq_Name = POHead.Po_Number + inode.Po_Item
                'If inode.SelectNodes("Free_Item").Count > 0 AndAlso inode.SelectNodes("Free_Item").Item(0).InnerText.Trim() <> "" Then
                '    item1.Free_Item = inode.SelectNodes("Free_Item").Item(0).InnerText.Trim()
                'End If
                If inode.Free_Item IsNot Nothing AndAlso Not String.IsNullOrEmpty(inode.Free_Item) Then
                    item1.Free_Item = inode.Free_Item
                End If
                POItem.Add(item1)
                Dim item1X As New Create_Po.BAPIMEPOITEMX
                item1X.Po_Item = inode.Po_Item : item1X.Po_Itemx = "X" : item1X.Material = "X"
                item1X.Plant = "X" : item1X.Stge_Loc = "X" : item1X.Quantity = "X" : item1X.Preq_Name = "X"
                item1X.Net_Price = "X"
                'If inode.SelectNodes("Free_Item").Count > 0 Then
                If inode.Free_Item IsNot Nothing AndAlso Not String.IsNullOrEmpty(inode.Free_Item) Then
                    item1X.Free_Item = "X"
                End If
                item1X.Tax_Code = "X"
                'End If
                PoItemX.Add(item1X)
                Dim sch1 As New Create_Po.BAPIMEPOSCHEDULE
                sch1.Po_Item = inode.Po_Item : sch1.Delivery_Date = inode.Delivery_Date
                sch1.Quantity = inode.Item_Qty
                PoSchedule.Add(sch1)
                Dim sch1X As New Create_Po.BAPIMEPOSCHEDULX
                sch1X.Po_Item = inode.Po_Item : sch1X.Del_Datcat_Ext = "X" : sch1X.Delivery_Date = "X"
                sch1X.Quantity = "X"
                PoScheduleX.Add(sch1X)
            Else
                'service item 
                item1.Tax_Code = inode.Tax_Code
                'item1.Preq_Name = POHead.Po_Number + "-" + inode.Po_Item
                item1.Preq_Name = POHead.Po_Number + inode.Po_Item
                item1.Po_Item = inode.Po_Item
                item1.Net_Price = inode.Net_Price
                item1.Quantity = inode.Item_Qty
                item1.Matl_Group = inode.Matl_Group
                item1.Acctasscat = inode.Acctasscat
                'item1.Acctasscat = "K"
                item1.Plant = inode.Plant
                item1.Stge_Loc = inode.Storage_Location
                'item1.Material = ""
                item1.Short_Text = inode.Short_Text
                item1.Orderpr_Un = "ST"
                item1.Price_Unit = 1
                item1.Po_Unit = "ST" : item1.Po_Unit_Iso = "ST" : item1.Orderpr_Un = "ST" : item1.Orderpr_Un_Iso = "ST"
                '  item1.Preq_Name = inode.Preq_Name
                POItem.Add(item1)

                Dim item1X As New Create_Po.BAPIMEPOITEMX
                item1X.Po_Item = inode.Po_Item : item1X.Po_Itemx = "X"
                item1X.Net_Price = "X" : item1X.Quantity = "X"
                item1X.Matl_Group = "X" : item1X.Acctasscat = "X" 'item1X.Material = "X"
                item1X.Short_Text = "X" : item1X.Plant = "X" : item1X.Stge_Loc = "X"
                item1X.Orderpr_Un = "X" : item1X.Price_Unit = "X"
                item1X.Po_Unit = "X" : item1X.Po_Unit_Iso = "X" : item1X.Orderpr_Un = "X" : item1X.Orderpr_Un_Iso = "X"
                item1X.Preq_Name = "X"
                item1X.Tax_Code = "X"
                PoItemX.Add(item1X)

                Dim account As New Create_Po.BAPIMEPOACCOUNT
                account.Po_Item = inode.Po_Item
                account.Gl_Account = inode.Gl_Account
                account.Costcenter = inode.Costcenter
                'account.Co_Area = inode.Item("Co_Area").InnerText
                account.Serial_No = inode.Serial_No
                account.Quantity = CDbl(inode.Item_Qty)
                POAccount.Add(account)

                Dim accountX As New Create_Po.BAPIMEPOACCOUNTX
                accountX.Po_Item = inode.Po_Item
                accountX.Po_Itemx = "X"
                accountX.Gl_Account = "X" : accountX.Costcenter = "X" 'accountX.Co_Area = "X"
                accountX.Serial_No = inode.Serial_No : accountX.Quantity = "X"
                POAccountX.Add(accountX)
                Dim sch1 As New Create_Po.BAPIMEPOSCHEDULE
                sch1.Po_Item = inode.Po_Item : sch1.Delivery_Date = inode.Delivery_Date
                sch1.Quantity = inode.Item_Qty
                PoSchedule.Add(sch1)
                Dim sch1X As New Create_Po.BAPIMEPOSCHEDULX
                sch1X.Po_Item = inode.Po_Item : sch1X.Del_Datcat_Ext = "X" : sch1X.Delivery_Date = "X"
                sch1X.Quantity = "X"
                PoScheduleX.Add(sch1X)
            End If


        Next

        p1.Connection = New SAPConnection(dest1.ConnectionString)
        Try
            p1.Connection.Open()
            p1.Bapi_Po_Create1("", "", "", "", "", "X", New Create_Po.BAPIMEPOADDRVENDOR, New Create_Po.BAPIEIKP,
            New Create_Po.BAPIEIKPX, PoHeader, PoHeaderX, "", New Create_Po.BAPIMEDCM, New Create_Po.BAPIMEPOHEADER,
            New Create_Po.BAPIEIKP, "", New Create_Po.BAPIMEDCM_ALLVERSIONSTable, New Create_Po.BAPIPAREXTable,
            New Create_Po.BAPIPAREXTable, POAccount, New Create_Po.BAPIMEPOACCOUNTPROFITSEGMENTTable,
            POAccountX, New Create_Po.BAPIMEPOADDRDELIVERYTable, New Create_Po.BAPIMEPOCONDTable,
            New Create_Po.BAPIMEPOCONDHEADERTable, New Create_Po.BAPIMEPOCONDHEADERXTable, New Create_Po.BAPIMEPOCONDXTable,
            New Create_Po.BAPIESUCCTable, New Create_Po.BAPIEIPOTable, New Create_Po.BAPIEIPOXTable, POItem, PoItemX,
            New Create_Po.BAPIESUHCTable, New Create_Po.BAPIEKKOPTable, PoSchedule, PoScheduleX, New Create_Po.BAPIESLLCTable,
            New Create_Po.BAPIESLLTXTable, New Create_Po.BAPIESKLCTable, New Create_Po.BAPIMEPOTEXTHEADERTable,
            New Create_Po.BAPIMEPOTEXTTable, returnTable)
            If POHead.IsTesting <> 1 Then
                p1.CommitWork()
            End If
            p1.Connection.Close()
        Catch ex As Exception
            result = ex.ToString()
            retCode = False
            Exit Sub
        End Try
        retCode = True
        Dim ds As New DataSet("Xml") : ds.Tables.Add(returnTable.ToADODataTable())
        result = ds.GetXml()
        '  result = Utilities.SAPTableToADOXML(returnTable)
    End Sub

#End Region
#Region "SAP Order"
    Public Shared Function SO_GetNumber(ByVal preFix As String) As String
        Dim NUM As New Object
        Dim str As String = String.Format("begin tran SELECT num+1 as num FROM ORDERNO with (tablockx) where prefix='{0}' update orderno set num=num+1 where prefix='{0}' commit tran", preFix)
        NUM = dbUtil.dbExecuteScalar("MY", str)
        If IsNumeric(NUM) Then
            'Ryan 20170324 Convert order number to Hex value
            'If Not HttpContext.Current.Session Is Nothing AndAlso HttpContext.Current.Session("ORG_ID").ToString.StartsWith("CN") Then
            '    Return preFix & CInt(NUM).ToString("X6")
            'Else
            '    Return preFix & CDbl(NUM).ToString("000000")
            'End If
            Return preFix & CDbl(NUM).ToString("000000")
        End If
        Return ""
    End Function
#End Region

#Region "Update SAP SO"

    Function UnblockSOGP(SONO As String, isTesting As Boolean) As Boolean
        Dim sap_connstr As String = IIf(isTesting, "SAP_Test", "SAP_PRD")
        Dim sap_appstr As String = IIf(isTesting, "SAPConnTest", "SAP_PRD")

        Dim sqlSOGPBlockLines As String =
            " select POSNR, LSSTA from saprdp.vbup where LSSTA='C' and vbeln='" + FormatToSAPSODNLineNo(SONO) + "' "
        Dim SAPGPApt As New Oracle.DataAccess.Client.OracleDataAdapter(sqlSOGPBlockLines, ConfigurationManager.ConnectionStrings(sap_connstr).ConnectionString)
        Dim dtSOGPLines As New DataTable
        SAPGPApt.Fill(dtSOGPLines)
        SAPGPApt.SelectCommand.Connection.Close()
        If dtSOGPLines.Rows.Count > 0 Then
            Dim pro1 As New Z_RELEASE_GP_ITEM.Z_RELEASE_GP_ITEM(ConfigurationManager.AppSettings(sap_appstr))
            pro1.Connection.Open()
            For Each GPLineRow As DataRow In dtSOGPLines.Rows
                Dim _returnval As Integer = 0
                Try
                    'pro1.Z_Release_Gp_Item(GPLineRow.Item("POSNR"), SONO, "", 0)
                    pro1.Z_Release_Gp_Item(GPLineRow.Item("POSNR"), SONO, "", _returnval)
                Catch ex As Exception
                    Dim aaa = ex.Message
                End Try

            Next
            pro1.Connection.Close()
        End If
        Return True
    End Function

    Function UnblockSOHeaderGP(SONO As String, isTesting As Boolean) As Boolean
        Dim sap_connstr As String = IIf(isTesting, "SAP_Test", "SAP_PRD")
        Dim sap_appstr As String = IIf(isTesting, "SAPConnTest", "SAP_PRD")

        Dim sqlSOGPBlock As String =
            " select * from saprdp.vbak where lifsk ='10' and vbeln ='" + SONO + "' "
        Dim SAPGPApt As New Oracle.DataAccess.Client.OracleDataAdapter(sqlSOGPBlock, ConfigurationManager.ConnectionStrings(sap_connstr).ConnectionString)
        Dim dtSOGP As New DataTable
        SAPGPApt.Fill(dtSOGP)
        SAPGPApt.SelectCommand.Connection.Close()
        If dtSOGP.Rows.Count > 0 Then
            Dim conn As String = ConfigurationManager.AppSettings("SAP_PRD")
            If isTesting = True Then conn = ConfigurationManager.AppSettings("SAPConnTest")
            Dim p1 As New BAPI_SALESORDER_CHANGE.BAPI_SALESORDER_CHANGE()
            p1.Connection = New SAP.Connector.SAPConnection(conn)
            Dim OrderHeader As New BAPI_SALESORDER_CHANGE.BAPISDH1, OrderHeaderX As New BAPI_SALESORDER_CHANGE.BAPISDH1X, ItemIn As New BAPI_SALESORDER_CHANGE.BAPISDITMTable,
            ItemInX As New BAPI_SALESORDER_CHANGE.BAPISDITMXTable, PartNr As New BAPI_SALESORDER_CHANGE.BAPIPARNRTable, Condition As New BAPI_SALESORDER_CHANGE.BAPICONDTable,
            ConditionX As New BAPI_SALESORDER_CHANGE.BAPICONDXTable, ScheLine As New BAPI_SALESORDER_CHANGE.BAPISCHDLTable, ScheLineX As New BAPI_SALESORDER_CHANGE.BAPISCHDLXTable,
            OrderText As New BAPI_SALESORDER_CHANGE.BAPISDTEXTTable, sales_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT, ext_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT,
            op_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT, retTable As New BAPI_SALESORDER_CHANGE.BAPIRET2Table, ADDRTable As New BAPI_SALESORDER_CHANGE.BAPIADDR1Table,
            PartnerChangeTable As New BAPI_SALESORDER_CHANGE.BAPIPARNRCTable
            Dim Doc_Number As String = FormatToSAPSODNNo(RemovePrecedingZeros(Trim(SONO).ToUpper()))
            OrderHeaderX.Updateflag = "U"
            OrderHeaderX.Dlv_Block = "X"
            OrderHeader.Dlv_Block = "11" 'change hearder delivery block status to GP approval

            p1.Connection.Open()


            p1.Bapi_Salesorder_Change("", "", New BAPI_SALESORDER_CHANGE.BAPISDLS, "", OrderHeader, OrderHeaderX, Doc_Number, "", Condition,
                                      ConditionX, New BAPI_SALESORDER_CHANGE.BAPIPAREXTable, New BAPI_SALESORDER_CHANGE.BAPICUBLBTable,
                                      New BAPI_SALESORDER_CHANGE.BAPICUINSTable, New BAPI_SALESORDER_CHANGE.BAPICUPRTTable, New BAPI_SALESORDER_CHANGE.BAPICUCFGTable,
                                      New BAPI_SALESORDER_CHANGE.BAPICUREFTable, New BAPI_SALESORDER_CHANGE.BAPICUVALTable, New BAPI_SALESORDER_CHANGE.BAPICUVKTable, ItemIn,
                                      ItemInX, New BAPI_SALESORDER_CHANGE.BAPISDKEYTable, OrderText, ADDRTable,
                                      PartnerChangeTable, PartNr, retTable, ScheLine, ScheLineX)
            p1.CommitWork()

            p1.Connection.Close()


        End If
        Return True
    End Function

    Function UpdateSPRNo(SONO As String, QuoteID As String, isTesting As Boolean) As Boolean
        Dim sap_connstr As String = IIf(isTesting, "SAP_Test", "SAP_PRD")
        Dim sap_appstr As String = IIf(isTesting, "SAPConnTest", "SAP_PRD")

        Dim sqlSOGPBlockLines As String = " select MANDT,VBELN,POSNR,MATNR from saprdp.vbap where mandt = '168' and vbeln='" + FormatToSAPSODNLineNo(SONO) + "' "
        Dim SAPApt As New Oracle.DataAccess.Client.OracleDataAdapter(sqlSOGPBlockLines, ConfigurationManager.ConnectionStrings(sap_connstr).ConnectionString)
        Dim dtSOdt As New DataTable
        SAPApt.Fill(dtSOdt)
        SAPApt.SelectCommand.Connection.Close()

        If dtSOdt.Rows.Count > 0 Then
            Dim pro1 As New Z_RELEASE_GP_ITEM.Z_RELEASE_GP_ITEM(ConfigurationManager.AppSettings(sap_appstr))
            pro1.Connection.Open()
            For Each SOLineRow As DataRow In dtSOdt.Rows
                Dim _returnval As Integer = 0
                Dim _objsprno As Object = dbUtil.dbExecuteScalar("EQ", String.Format(" select top 1 ISNULL(sprNo,'') as SPRNO from QuotationDetail where quoteId = '{0}' and partNo = '{1}'", QuoteID, RemovePrecedingZeros(SOLineRow.Item("MATNR"))))
                Dim _sprno As String = _objsprno.ToString

                Try
                    If (Not String.IsNullOrEmpty(_sprno)) Then
                        pro1.Z_Release_Gp_Item(SOLineRow.Item("POSNR"), SONO, _sprno, _returnval)
                    End If
                Catch ex As Exception
                    Dim aaa = ex.Message
                End Try
            Next
            pro1.Connection.Close()
        End If
        Return True
    End Function

    Public Function DeleteSOLine(ByVal SONo As String, ByRef OrderLinesObject As List(Of SOC_SOLine), ByRef ErrorMessage As String) As Boolean
        Dim p1 As New BAPI_SALESORDER_CHANGE.BAPI_SALESORDER_CHANGE()
        p1.Connection = New SAP.Connector.SAPConnection("")
        Dim OrderHeader As New BAPI_SALESORDER_CHANGE.BAPISDH1, OrderHeaderX As New BAPI_SALESORDER_CHANGE.BAPISDH1X, ItemIn As New BAPI_SALESORDER_CHANGE.BAPISDITMTable,
           ItemInX As New BAPI_SALESORDER_CHANGE.BAPISDITMXTable, PartNr As New BAPI_SALESORDER_CHANGE.BAPIPARNRTable, Condition As New BAPI_SALESORDER_CHANGE.BAPICONDTable,
           ConditionX As New BAPI_SALESORDER_CHANGE.BAPICONDXTable, ScheLine As New BAPI_SALESORDER_CHANGE.BAPISCHDLTable, ScheLineX As New BAPI_SALESORDER_CHANGE.BAPISCHDLXTable,
           OrderText As New BAPI_SALESORDER_CHANGE.BAPISDTEXTTable, sales_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT, ext_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT,
           op_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT, retTable As New BAPI_SALESORDER_CHANGE.BAPIRET2Table, ADDRTable As New BAPI_SALESORDER_CHANGE.BAPIADDR1Table,
           PartnerChangeTable As New BAPI_SALESORDER_CHANGE.BAPIPARNRCTable
        Dim Doc_Number As String = FormatToSAPSODNNo(RemovePrecedingZeros(Trim(SONo).ToUpper()))
        OrderHeaderX.Updateflag = "U"
        If OrderLinesObject IsNot Nothing AndAlso OrderLinesObject.Count > 0 Then
            For Each soLine In OrderLinesObject
                Dim ItemInRow As New BAPI_SALESORDER_CHANGE.BAPISDITM, ItemInRowX As New BAPI_SALESORDER_CHANGE.BAPISDITMX
                With ItemInRow
                    .Itm_Number = soLine.LineNo
                End With
                With ItemInRowX
                    .Itm_Number = soLine.LineNo : .Updateflag = "D"
                End With
                ItemIn.Add(ItemInRow) : ItemInX.Add(ItemInRowX)
            Next
        End If
        p1.Connection.Open()

        'p1.Bapi_Salesorder_Change("","",New BAPI_SALESORDER_CHANGE.BAPISDLS, "",
        p1.Bapi_Salesorder_Change("", "", New BAPI_SALESORDER_CHANGE.BAPISDLS, "", OrderHeader, OrderHeaderX, Doc_Number, "", Condition,
                                  ConditionX, New BAPI_SALESORDER_CHANGE.BAPIPAREXTable, New BAPI_SALESORDER_CHANGE.BAPICUBLBTable,
                                  New BAPI_SALESORDER_CHANGE.BAPICUINSTable, New BAPI_SALESORDER_CHANGE.BAPICUPRTTable, New BAPI_SALESORDER_CHANGE.BAPICUCFGTable,
                                  New BAPI_SALESORDER_CHANGE.BAPICUREFTable, New BAPI_SALESORDER_CHANGE.BAPICUVALTable, New BAPI_SALESORDER_CHANGE.BAPICUVKTable, ItemIn,
                                  ItemInX, New BAPI_SALESORDER_CHANGE.BAPISDKEYTable, OrderText, ADDRTable,
                                  PartnerChangeTable, PartNr, retTable, ScheLine, ScheLineX)
        p1.CommitWork()

        p1.Connection.Close()
        'DataGridView1.DataSource = retTable.ToADODataTable()
    End Function

    Public Function UpdateSOLine(ByVal SONo As String, ByRef OrderLinesObject As List(Of SOC_SOLine), ByRef ErrorMessage As String, Optional isTesting As Boolean = True) As Boolean
        Dim p1 As New BAPI_SALESORDER_CHANGE.BAPI_SALESORDER_CHANGE()
        Dim conn As String = ConfigurationManager.AppSettings("SAP_PRD")
        If isTesting = True Then conn = ConfigurationManager.AppSettings("SAPConnTest")
        p1.Connection = New SAP.Connector.SAPConnection(conn)
        Dim OrderHeader As New BAPI_SALESORDER_CHANGE.BAPISDH1, OrderHeaderX As New BAPI_SALESORDER_CHANGE.BAPISDH1X, ItemIn As New BAPI_SALESORDER_CHANGE.BAPISDITMTable,
           ItemInX As New BAPI_SALESORDER_CHANGE.BAPISDITMXTable, PartNr As New BAPI_SALESORDER_CHANGE.BAPIPARNRTable, Condition As New BAPI_SALESORDER_CHANGE.BAPICONDTable,
           ConditionX As New BAPI_SALESORDER_CHANGE.BAPICONDXTable, ScheLine As New BAPI_SALESORDER_CHANGE.BAPISCHDLTable, ScheLineX As New BAPI_SALESORDER_CHANGE.BAPISCHDLXTable,
           OrderText As New BAPI_SALESORDER_CHANGE.BAPISDTEXTTable, sales_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT, ext_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT,
           op_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT, retTable As New BAPI_SALESORDER_CHANGE.BAPIRET2Table, ADDRTable As New BAPI_SALESORDER_CHANGE.BAPIADDR1Table,
           PartnerChangeTable As New BAPI_SALESORDER_CHANGE.BAPIPARNRCTable
        Dim Doc_Number As String = FormatToSAPSODNNo(RemovePrecedingZeros(Trim(SONo).ToUpper()))
        OrderHeaderX.Updateflag = "U"
        If OrderLinesObject IsNot Nothing AndAlso OrderLinesObject.Count > 0 Then
            For Each soLine In OrderLinesObject
                Dim ItemInRow As New BAPI_SALESORDER_CHANGE.BAPISDITM, ItemInRowX As New BAPI_SALESORDER_CHANGE.BAPISDITMX, ScheLine_Record As New BAPI_SALESORDER_CHANGE.BAPISCHDL
                Dim ScheLine_RecordX As New BAPI_SALESORDER_CHANGE.BAPISCHDLX, ConditionRecord As New BAPI_SALESORDER_CHANGE.BAPICOND, ConditionRecordX As New BAPI_SALESORDER_CHANGE.BAPICONDX

                If (soLine.UpdatePN OrElse soLine.UpdateQty) Then
                    With ItemInRow
                        .Itm_Number = soLine.LineNo
                        If soLine.UpdatePN Then .Material = FormatToSAPPartNo(soLine.PartNo)
                        If soLine.UpdateQty Then .Target_Qty = soLine.NewQty
                    End With
                    With ItemInRowX
                        .Itm_Number = soLine.LineNo : .Updateflag = "U"
                        If soLine.UpdatePN Then .Material = "X"
                        If soLine.UpdateQty Then .Target_Qty = "X"
                    End With
                    ItemIn.Add(ItemInRow) : ItemInX.Add(ItemInRowX)
                End If

                If soLine.UpdatePrice AndAlso Not String.IsNullOrEmpty(soLine.LineNo) AndAlso Decimal.TryParse(soLine.NewPrice, 0) AndAlso CDbl(soLine.NewPrice) >= 0 Then
                    With ConditionRecord
                        .Itm_Number = soLine.LineNo : .Cond_Type = "ZPN0" : .Cond_Value = soLine.NewPrice / 10 'ICC 2017/8/24 因為送進SAP的價格不知道為什麼都會被 * 10，所以TC建議先 /10
                    End With
                    With ConditionRecordX
                        .Itm_Number = soLine.LineNo : .Updateflag = "U" : .Cond_Type = "X" : .Cond_Value = "X"
                    End With
                    Condition.Add(ConditionRecord) : ConditionX.Add(ConditionRecordX)
                End If

                If (soLine.UpdateQty OrElse soLine.UpdateReqDate) AndAlso Not String.IsNullOrEmpty(soLine.LineNo) Then
                    With ScheLine_Record
                        .Itm_Number = soLine.LineNo : .Sched_Line = "1"
                        If soLine.UpdateReqDate AndAlso DateDiff(DateInterval.Day, Now, soLine.NewReqDate) >= 0 Then .Req_Date = soLine.NewReqDate.ToString("yyyyMMdd")
                        If soLine.UpdateQty AndAlso Decimal.TryParse(soLine.NewQty, 0) AndAlso CDbl(soLine.NewQty) > 0 Then .Req_Qty = soLine.NewQty
                    End With
                    With ScheLine_RecordX
                        .Itm_Number = soLine.LineNo : .Updateflag = "U"
                        .Sched_Line = "1"
                        If soLine.UpdateReqDate AndAlso DateDiff(DateInterval.Day, Now, soLine.NewReqDate) >= 0 Then .Req_Date = "X"
                        If soLine.UpdateQty AndAlso Decimal.TryParse(soLine.NewQty, 0) AndAlso CDbl(soLine.NewQty) > 0 Then .Req_Qty = "X"
                    End With
                    'ScheLine.Add(ScheLine_Record) : ScheLineX.Add(ScheLine_RecordX)
                End If

            Next
        End If


        p1.Connection.Open()

        'p1.Bapi_Salesorder_Change("","",New BAPI_SALESORDER_CHANGE.BAPISDLS, "",
        p1.Bapi_Salesorder_Change("", "", New BAPI_SALESORDER_CHANGE.BAPISDLS, "", OrderHeader, OrderHeaderX, Doc_Number, "", Condition,
                                  ConditionX, New BAPI_SALESORDER_CHANGE.BAPIPAREXTable, New BAPI_SALESORDER_CHANGE.BAPICUBLBTable,
                                  New BAPI_SALESORDER_CHANGE.BAPICUINSTable, New BAPI_SALESORDER_CHANGE.BAPICUPRTTable, New BAPI_SALESORDER_CHANGE.BAPICUCFGTable,
                                  New BAPI_SALESORDER_CHANGE.BAPICUREFTable, New BAPI_SALESORDER_CHANGE.BAPICUVALTable, New BAPI_SALESORDER_CHANGE.BAPICUVKTable, ItemIn,
                                  ItemInX, New BAPI_SALESORDER_CHANGE.BAPISDKEYTable, OrderText, ADDRTable,
                                  PartnerChangeTable, PartNr, retTable, ScheLine, ScheLineX)
        p1.CommitWork()

        p1.Connection.Close()
        'DataGridView1.DataSource = retTable.ToADODataTable()
    End Function

    Public Function AddSOLine(ByVal SONo As String, ByRef OrderLinesObject As List(Of SOC_SOLine), ByRef ErrorMessage As String) As Boolean
        Dim p1 As New BAPI_SALESORDER_CHANGE.BAPI_SALESORDER_CHANGE()
        p1.Connection = New SAP.Connector.SAPConnection("")
        Dim SapDbConn As New Oracle.DataAccess.Client.OracleConnection(strSAPDbConn)
        SapDbConn.Open()
        Dim OrderHeader As New BAPI_SALESORDER_CHANGE.BAPISDH1, OrderHeaderX As New BAPI_SALESORDER_CHANGE.BAPISDH1X, ItemIn As New BAPI_SALESORDER_CHANGE.BAPISDITMTable,
            ItemInX As New BAPI_SALESORDER_CHANGE.BAPISDITMXTable, PartNr As New BAPI_SALESORDER_CHANGE.BAPIPARNRTable, Condition As New BAPI_SALESORDER_CHANGE.BAPICONDTable,
            ConditionX As New BAPI_SALESORDER_CHANGE.BAPICONDXTable, ScheLine As New BAPI_SALESORDER_CHANGE.BAPISCHDLTable, ScheLineX As New BAPI_SALESORDER_CHANGE.BAPISCHDLXTable,
            OrderText As New BAPI_SALESORDER_CHANGE.BAPISDTEXTTable, sales_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT, ext_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT,
            op_note As New BAPI_SALESORDER_CHANGE.BAPISDTEXT, retTable As New BAPI_SALESORDER_CHANGE.BAPIRET2Table, ADDRTable As New BAPI_SALESORDER_CHANGE.BAPIADDR1Table,
            PartnerChangeTable As New BAPI_SALESORDER_CHANGE.BAPIPARNRCTable
        Dim Doc_Number As String = FormatToSAPSODNNo(RemovePrecedingZeros(Trim(SONo).ToUpper()))

        OrderHeaderX.Updateflag = "U"

        If OrderLinesObject IsNot Nothing AndAlso OrderLinesObject.Count > 0 Then
            For Each soLine In OrderLinesObject
                Dim ItemInRow As New BAPI_SALESORDER_CHANGE.BAPISDITM, ItemInRowX As New BAPI_SALESORDER_CHANGE.BAPISDITMX, ScheLine_Record As New BAPI_SALESORDER_CHANGE.BAPISCHDL
                Dim ScheLine_RecordX As New BAPI_SALESORDER_CHANGE.BAPISCHDLX, ConditionRecord As New BAPI_SALESORDER_CHANGE.BAPICOND, ConditionRecordX As New BAPI_SALESORDER_CHANGE.BAPICONDX

                Dim cmd As New Oracle.DataAccess.Client.OracleCommand("select max(a.posnr)+1 as new_line_no from saprdp.vbap a where a.mandt='168' and vbeln='" + Doc_Number + "' ", SapDbConn)
                Dim LineNo As String = cmd.ExecuteScalar()
                With ItemInRow
                    .Itm_Number = LineNo : .Material = FormatToSAPPartNo(soLine.PartNo)
                End With

                With ItemInRowX
                    .Itm_Number = LineNo : .Updateflag = "I" : .Material = "X"
                End With

                With ConditionRecord
                    .Itm_Number = LineNo : .Cond_Type = "ZPN0" : .Cond_Value = soLine.NewPrice
                End With
                With ConditionRecordX
                    .Itm_Number = LineNo : .Updateflag = "I" : .Cond_Type = "X" : .Cond_Value = "X"
                End With

                With ScheLine_Record
                    .Itm_Number = LineNo : .Sched_Line = "1" : .Req_Date = soLine.NewReqDate.ToString("yyyyMMdd") : .Req_Qty = soLine.NewQty
                End With

                With ScheLine_RecordX
                    .Itm_Number = LineNo : .Updateflag = "I" : .Req_Date = "X" : .Req_Qty = "X"
                End With

                ItemIn.Add(ItemInRow) : ItemInX.Add(ItemInRowX)
                Condition.Add(ConditionRecord) : ConditionX.Add(ConditionRecordX)
                ScheLine.Add(ScheLine_Record) : ScheLineX.Add(ScheLine_RecordX)
            Next
        End If


        p1.Connection.Open()

        'p1.Bapi_Salesorder_Change("","",New BAPI_SALESORDER_CHANGE.BAPISDLS, "",
        p1.Bapi_Salesorder_Change("", "", New BAPI_SALESORDER_CHANGE.BAPISDLS, "", OrderHeader, OrderHeaderX, Doc_Number, "", Condition,
                                  ConditionX, New BAPI_SALESORDER_CHANGE.BAPIPAREXTable, New BAPI_SALESORDER_CHANGE.BAPICUBLBTable,
                                  New BAPI_SALESORDER_CHANGE.BAPICUINSTable, New BAPI_SALESORDER_CHANGE.BAPICUPRTTable, New BAPI_SALESORDER_CHANGE.BAPICUCFGTable,
                                  New BAPI_SALESORDER_CHANGE.BAPICUREFTable, New BAPI_SALESORDER_CHANGE.BAPICUVALTable, New BAPI_SALESORDER_CHANGE.BAPICUVKTable, ItemIn,
                                  ItemInX, New BAPI_SALESORDER_CHANGE.BAPISDKEYTable, OrderText, ADDRTable,
                                  PartnerChangeTable, PartNr, retTable, ScheLine, ScheLineX)
        p1.CommitWork()

        p1.Connection.Close()
        SapDbConn.Close()

        'DataGridView1.DataSource = retTable.ToADODataTable()
    End Function

    Public Class SOC_SOHeader
        Public Property HeaderNote As String
        Public Sub New()
            HeaderNote = ""
        End Sub
        Public Sub New(HeaderNote As String)
            Me.HeaderNote = HeaderNote
        End Sub
    End Class

    Public Class SOC_SOLine
        Public Property LineNo As String : Public Property PartNo As String
        Public Property NewPrice As Double : Public Property NewReqDate As Date : Public Property NewQty As Integer
        Public Property UpdatePN As Boolean : Public Property UpdateReqDate As Boolean : Public Property UpdateQty As Boolean : Public Property UpdatePrice As Boolean
        Public Sub New()
            UpdatePN = False : UpdateReqDate = False : UpdateQty = False : UpdatePrice = False
        End Sub
    End Class

    Public Shared Function FormatToSAPSODNNo(ByVal str As String) As String
        If String.IsNullOrEmpty(str) Then Return ""
        str = UCase(str)
        If Not Decimal.TryParse(str.Substring(0, 1), 0) Then Return str
        While str.Length < 10
            str = "0" + str
        End While
        Return str
    End Function

    Public Shared Function FormatToSAPSODNLineNo(ByVal str As String) As String
        If String.IsNullOrEmpty(str) Then Return ""
        str = UCase(str)
        If Not Decimal.TryParse(str.Substring(0, 1), 0) Then Return str
        While str.Length < 6
            str = "0" + str
        End While
        Return str
    End Function

    Public Shared Function RemovePrecedingZeros(ByVal str As String) As String
        If Not str.StartsWith("0") Then Return str
        If str.Length > 1 Then
            Return RemovePrecedingZeros(str.Substring(1))
        Else
            Return str
        End If
    End Function

    Public Shared Function FormatToSAPPartNo(ByVal str As String) As String
        If String.IsNullOrEmpty(Trim(str)) Then Return ""
        str = RemovePrecedingZeros(str)
        Dim IsNumericPart As Nullable(Of Boolean)
        For i As Integer = 0 To str.Length - 1
            If Not Decimal.TryParse(str.Substring(i, 1), 0) Then
                IsNumericPart = False : Exit For
            Else
                IsNumericPart = True
            End If
        Next
        If IsNumericPart = True Then
            While str.Length < 18
                str = "0" + str
            End While
        End If
        Return str
    End Function
#End Region

#Region "PRICING"


    Public Class DocumentLine
        Public Property PartNo As String : Public Property LineNo As String : Public Property Qty As Integer : Public Property ListPrice As Decimal
        Public Property UnitPrice As Decimal : Public Property Discount As Double : Public Property Cost As Decimal : Public Property GPPercent As Double
        Public Property ThresholdPercent As Double : Public Property GPBlock As Boolean : Public Property MaterialType As String

    End Class

    Public Class GPInfo
        Public Property LineItems As List(Of DocumentLine)
        Public ReadOnly Property TotalUnitPrice As Decimal
            Get
                Return Aggregate q In LineItems Into Sum(q.Qty * q.UnitPrice)
            End Get
        End Property
        Public ReadOnly Property TotalListPrice As Decimal
            Get
                Return Aggregate q In LineItems Into Sum(q.Qty * q.ListPrice)
            End Get
        End Property
        Public ReadOnly Property TotalDiscount As Double
            Get
                If TotalListPrice = 0 Then Return 0
                Return Math.Round(1.0 - TotalUnitPrice / TotalListPrice, 2)
            End Get
        End Property
        Public ReadOnly Property TotalCost As Decimal
            Get
                Return Aggregate q In LineItems Into Sum(q.Qty * q.Cost)
            End Get
        End Property
        Public ReadOnly Property TotalGPPercent As Double
            Get
                If TotalUnitPrice = 0 Then Return 0
                Return Math.Round(1.0 - TotalCost / TotalUnitPrice, 2)
            End Get
        End Property

        Public ReadOnly Property TotalMinPrice As Double
            Get
                Return Aggregate q In LineItems Into Sum(q.Qty * q.Cost * (1.0 + q.ThresholdPercent))
            End Get
        End Property

        Public ReadOnly Property TotalThresholdPercent As Double
            Get
                If TotalCost = 0 Then Return 0
                Return Math.Round(1.0 - TotalCost / TotalMinPrice, 2)
            End Get
        End Property
        Public ReadOnly Property IsTotalGPBlock As Boolean
            Get
                Return TotalUnitPrice < TotalMinPrice
            End Get
        End Property

        Public Sub New()
            LineItems = New List(Of DocumentLine)
        End Sub
    End Class

    Public Shared Function CalcANAAOnlineDocGP(LineItems As List(Of DocumentLine)) As GPInfo
        Dim GPInfo1 As New GPInfo
        For Each qline As DocumentLine In LineItems
            Select Case UCase(qline.MaterialType)
                Case "ZCTO", "ZCON", "ZSRV"
                Case Else
                    If CDbl(qline.ListPrice) > 0 Then
                        qline.Discount = Math.Round(1.0 - (CDbl(qline.UnitPrice) / CDbl(qline.ListPrice)), 2)
                    End If

                    'Dim condlist = GetUSPN_Cost_GPPercent(qline.PartNo)
                    'Dim costRec = From q In condlist Where q.COND_TYPE = "VPRS"
                    'If costRec.Count > 0 Then
                    '    qline.Cost = Math.Round(costRec.First.COND_VAL, 2)
                    'End If

                    If CDbl(qline.UnitPrice) > 0 Then
                        qline.GPPercent = Math.Round(1.0 - CDbl(qline.Cost) / CDbl(qline.UnitPrice), 2)
                    End If

                    'Dim gppercent = From q In condlist Where q.COND_TYPE = "ZMGP"
                    'If gppercent.Count > 0 Then
                    '    qline.ThresholdPercent = gppercent.First.COND_VAL * 0.01
                    'End If
                    'Esther : Our threshold for GP is 14%
                    'qline.ThresholdPercent = 0.14

                    If CDbl(qline.UnitPrice) > 0 Then
                        If qline.UnitPrice < qline.Cost Then
                            qline.GPBlock = True
                        Else
                            qline.GPBlock = False
                        End If
                    End If
            End Select
        Next
        GPInfo1.LineItems.AddRange(LineItems)
        Return GPInfo1
    End Function

    Public Shared Function CalcACNDocGP(LineItems As List(Of DocumentLine)) As GPInfo
        Dim GPInfo1 As New GPInfo
        For Each qline As DocumentLine In LineItems
            Select Case UCase(qline.MaterialType)
                Case "ZCTO", "ZCON", "ZSRV"
                Case Else
                    If CDbl(qline.ListPrice) > 0 Then
                        qline.Discount = Math.Round(1.0 - (CDbl(qline.UnitPrice) / CDbl(qline.ListPrice)), 2)
                    End If

                    Dim condlist = GetCNPN_Cost_GPPercent(qline.PartNo)
                    Dim costRec = From q In condlist Where q.COND_TYPE = "VPRS"
                    If costRec.Count > 0 Then
                        If costRec.First.COND_P_UNIT <> 0 Then
                            qline.Cost = Math.Round(costRec.First.COND_VAL / costRec.First.COND_P_UNIT, 2)
                        Else
                            qline.Cost = Math.Round(costRec.First.COND_VAL, 2)
                        End If
                    End If

                    If CDbl(qline.UnitPrice) > 0 Then
                        qline.GPPercent = Math.Round(1.0 - CDbl(qline.Cost) / CDbl(qline.UnitPrice), 2)
                    End If

                    'Dim gppercent = From q In condlist Where q.COND_TYPE = "ZMGP"
                    'If gppercent.Count > 0 Then
                    '    qline.ThresholdPercent = gppercent.First.COND_VAL * 0.01
                    'End If
                    'Esther : Our threshold for GP is 14%
                    qline.ThresholdPercent = 0.14

                    If CDbl(qline.UnitPrice) > 0 Then
                        If qline.GPPercent < qline.ThresholdPercent Then
                            qline.GPBlock = True
                        Else
                            qline.GPBlock = False
                        End If
                    End If
            End Select
        Next
        GPInfo1.LineItems.AddRange(LineItems)
        Return GPInfo1
    End Function

    Public Shared Function CalcAENCDocGP(LineItems As List(Of DocumentLine)) As GPInfo
        Dim GPInfo1 As New GPInfo
        For Each qline As DocumentLine In LineItems
            Select Case UCase(qline.MaterialType)
                Case "ZCTO", "ZCON", "ZSRV"
                Case Else
                    If CDbl(qline.ListPrice) > 0 Then
                        qline.Discount = Math.Round(1.0 - (CDbl(qline.UnitPrice) / CDbl(qline.ListPrice)), 2)
                    End If

                    Dim condlist = GetUSPN_Cost_GPPercent(qline.PartNo)
                    Dim costRec = From q In condlist Where q.COND_TYPE = "VPRS"
                    If costRec.Count > 0 Then
                        qline.Cost = Math.Round(costRec.First.COND_VAL, 2)
                    End If

                    If CDbl(qline.UnitPrice) > 0 Then
                        qline.GPPercent = Math.Round(1.0 - CDbl(qline.Cost) / CDbl(qline.UnitPrice), 2)
                    End If

                    'Dim gppercent = From q In condlist Where q.COND_TYPE = "ZMGP"
                    'If gppercent.Count > 0 Then
                    '    qline.ThresholdPercent = gppercent.First.COND_VAL * 0.01
                    'End If
                    'Esther : Our threshold for GP is 14%
                    qline.ThresholdPercent = 0.14

                    If CDbl(qline.UnitPrice) > 0 Then
                        If qline.GPPercent < qline.ThresholdPercent Then
                            qline.GPBlock = True
                        Else
                            qline.GPBlock = False
                        End If
                    End If
            End Select
        Next
        GPInfo1.LineItems.AddRange(LineItems)
        Return GPInfo1
    End Function
    Public Shared Function CalcANADocGP(LineItems As List(Of DocumentLine)) As GPInfo
        Dim GPInfo1 As New GPInfo
        For Each qline As DocumentLine In LineItems
            Select Case UCase(qline.MaterialType)
                Case "ZCTO", "ZCON", "ZSRV"
                Case Else
                    If CDbl(qline.ListPrice) > 0 Then
                        qline.Discount = Math.Round(1.0 - (CDbl(qline.UnitPrice) / CDbl(qline.ListPrice)), 2)
                    End If

                    Dim condlist = GetUSPN_Cost_GPPercent(qline.PartNo)
                    Dim costRec = From q In condlist Where q.COND_TYPE = "VPRS"
                    If costRec.Count > 0 Then
                        qline.Cost = Math.Round(costRec.First.COND_VAL, 2)
                    End If

                    If CDbl(qline.UnitPrice) > 0 Then
                        qline.GPPercent = Math.Round(1.0 - CDbl(qline.Cost) / CDbl(qline.UnitPrice), 2)
                    End If

                    Dim gppercent = From q In condlist Where q.COND_TYPE = "ZMGP"
                    If gppercent.Count > 0 Then
                        qline.ThresholdPercent = gppercent.First.COND_VAL * 0.01
                    End If

                    If CDbl(qline.UnitPrice) > 0 Then
                        If qline.GPPercent < qline.ThresholdPercent Then
                            qline.GPBlock = True
                        Else
                            qline.GPBlock = False
                        End If
                    End If
            End Select
        Next
        GPInfo1.LineItems.AddRange(LineItems)
        Return GPInfo1
    End Function

    Public Shared Function CalcANASOGP(sono As String) As GPInfo
        If String.IsNullOrEmpty(sono) Then Return Nothing
        Dim strSql As String =
            " select a.matnr as partNo, a.posnr as line_No, a.kwmeng as qty,  " +
            " a.kzwi6/a.kwmeng as listPrice, a.netpr as unitPrice, b.mtart as Material_Type " +
            " from saprdp.vbap a inner join saprdp.mara b on a.matnr=b.matnr " +
            " where a.mandt='168' and b.mandt='168' and a.vbeln='" + FormatToSAPSODNNo(Trim(sono)) + "' " +
            " order by a.posnr "
        Dim dtQuoteDetail As New DataTable
        Dim eqApt As New Oracle.DataAccess.Client.OracleDataAdapter(strSql, ConfigurationManager.ConnectionStrings("SAP_PRD").ConnectionString)
        eqApt.Fill(dtQuoteDetail)
        eqApt.SelectCommand.Connection.Close()

        Dim LineItems As New List(Of DocumentLine)
        For Each qline As DataRow In dtQuoteDetail.Rows
            Dim LineItem As New DocumentLine
            With LineItem
                .PartNo = qline("partNo") : .MaterialType = qline.Item("Material_Type") : .LineNo = qline.Item("line_No") : .ListPrice = qline("listPrice")
                .UnitPrice = qline.Item("unitPrice") : .Qty = qline.Item("qty")
            End With
            LineItems.Add(LineItem)
        Next
        Return CalcANADocGP(LineItems)
    End Function

    Public Shared Function CalcANAAOlineQuoteGP(quoteId As String) As GPInfo
        If String.IsNullOrEmpty(quoteId) Then Return Nothing
        Dim strSql As String =
            " select b.partNo, b.line_No, b.qty, b.listPrice, b.NewUnitPrice, b.newitp,    " +
            " 0.0 as Discount, 0.0 as Cost, 0.0 as GPPercent, 0.0 as ThresholdPercent, cast(0 as bit) as GPBlock, IsNull(c.PRODUCT_TYPE,'') as Material_Type  " +
            " from QuotationMaster a   " +
            " inner join QuotationDetail b on a.quoteId=b.quoteId " +
            " inner join MyAdvantechGlobal.dbo.SAP_PRODUCT c on b.partNo=c.PART_NO  " +
            " where a.quoteId=@QuoteId  " +
            " order by b.line_No  "
        Dim dtQuoteDetail As New DataTable
        Dim eqApt As New SqlClient.SqlDataAdapter(strSql, ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        eqApt.SelectCommand.Parameters.AddWithValue("QuoteId", quoteId)
        eqApt.Fill(dtQuoteDetail)
        eqApt.SelectCommand.Connection.Close()

        Dim LineItems As New List(Of DocumentLine)
        For Each qline As DataRow In dtQuoteDetail.Rows

            If qline("partNo").ToString.StartsWith("AGS-") Then
                Continue For
            End If

            Dim LineItem As New DocumentLine
            With LineItem
                .PartNo = qline("partNo") : .MaterialType = qline.Item("Material_Type") : .LineNo = qline.Item("line_No") : .ListPrice = qline("listPrice")
                .UnitPrice = qline.Item("NewUnitPrice") : .Qty = qline.Item("qty")
                .Cost = qline.Item("newitp")
            End With
            LineItems.Add(LineItem)
        Next
        Return CalcANAAOnlineDocGP(LineItems)
    End Function


    Public Shared Function CalcAENCQuoteGP(quoteId As String) As GPInfo
        If String.IsNullOrEmpty(quoteId) Then Return Nothing
        Dim strSql As String =
            " select b.partNo, b.line_No, b.qty, b.listPrice, b.NewUnitPrice,    " +
            " 0.0 as Discount, 0.0 as Cost, 0.0 as GPPercent, 0.0 as ThresholdPercent, cast(0 as bit) as GPBlock, IsNull(c.PRODUCT_TYPE,'') as Material_Type  " +
            " from QuotationMaster a   " +
            " inner join QuotationDetail b on a.quoteId=b.quoteId " +
            " inner join MyAdvantechGlobal.dbo.SAP_PRODUCT c on b.partNo=c.PART_NO  " +
            " where a.quoteId=@QuoteId  " +
            " order by b.line_No  "
        Dim dtQuoteDetail As New DataTable
        Dim eqApt As New SqlClient.SqlDataAdapter(strSql, ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        eqApt.SelectCommand.Parameters.AddWithValue("QuoteId", quoteId)
        eqApt.Fill(dtQuoteDetail)
        eqApt.SelectCommand.Connection.Close()

        Dim LineItems As New List(Of DocumentLine)
        For Each qline As DataRow In dtQuoteDetail.Rows

            If qline("partNo").ToString.StartsWith("AGS-") Then
                Continue For
            End If

            Dim LineItem As New DocumentLine
            With LineItem
                .PartNo = qline("partNo") : .MaterialType = qline.Item("Material_Type") : .LineNo = qline.Item("line_No") : .ListPrice = qline("listPrice")
                .UnitPrice = qline.Item("NewUnitPrice") : .Qty = qline.Item("qty")
            End With
            LineItems.Add(LineItem)
        Next
        Return CalcAENCDocGP(LineItems)
    End Function

    Public Shared Function CalcACNQuoteGP(quoteId As String) As GPInfo
        If String.IsNullOrEmpty(quoteId) Then Return Nothing
        Dim strSql As String =
            " select b.partNo, b.line_No, b.qty, b.listPrice, b.NewUnitPrice,    " +
            " 0.0 as Discount, 0.0 as Cost, 0.0 as GPPercent, 0.0 as ThresholdPercent, cast(0 as bit) as GPBlock, IsNull(c.PRODUCT_TYPE,'') as Material_Type  " +
            " from QuotationMaster a   " +
            " inner join QuotationDetail b on a.quoteId=b.quoteId " +
            " inner join MyAdvantechGlobal.dbo.SAP_PRODUCT c on b.partNo=c.PART_NO  " +
            " where a.quoteId=@QuoteId  " +
            " order by b.line_No  "
        Dim dtQuoteDetail As New DataTable
        Dim eqApt As New SqlClient.SqlDataAdapter(strSql, ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        eqApt.SelectCommand.Parameters.AddWithValue("QuoteId", quoteId)
        eqApt.Fill(dtQuoteDetail)
        eqApt.SelectCommand.Connection.Close()

        Dim LineItems As New List(Of DocumentLine)
        For Each qline As DataRow In dtQuoteDetail.Rows

            If qline("partNo").ToString.StartsWith("AGS-") Then
                Continue For
            End If

            Dim LineItem As New DocumentLine
            With LineItem
                .PartNo = qline("partNo") : .MaterialType = qline.Item("Material_Type") : .LineNo = qline.Item("line_No") : .ListPrice = qline("listPrice")
                .UnitPrice = qline.Item("NewUnitPrice") : .Qty = qline.Item("qty")
            End With
            LineItems.Add(LineItem)
        Next
        Return CalcACNDocGP(LineItems)
    End Function


    Public Shared Function CalcANAQuoteGP(quoteId As String) As GPInfo
        If String.IsNullOrEmpty(quoteId) Then Return Nothing
        Dim strSql As String =
            " select b.partNo, b.line_No, b.qty, b.listPrice, b.NewUnitPrice,    " +
            " 0.0 as Discount, 0.0 as Cost, 0.0 as GPPercent, 0.0 as ThresholdPercent, cast(0 as bit) as GPBlock, IsNull(c.PRODUCT_TYPE,'') as Material_Type  " +
            " from QuotationMaster a   " +
            " inner join QuotationDetail b on a.quoteId=b.quoteId " +
            " inner join MyAdvantechGlobal.dbo.SAP_PRODUCT c on b.partNo=c.PART_NO  " +
            " where a.quoteId=@QuoteId  " +
            " order by b.line_No  "
        Dim dtQuoteDetail As New DataTable
        Dim eqApt As New SqlClient.SqlDataAdapter(strSql, ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        eqApt.SelectCommand.Parameters.AddWithValue("QuoteId", quoteId)
        eqApt.Fill(dtQuoteDetail)
        eqApt.SelectCommand.Connection.Close()

        Dim LineItems As New List(Of DocumentLine)
        For Each qline As DataRow In dtQuoteDetail.Rows
            Dim LineItem As New DocumentLine
            With LineItem
                .PartNo = qline("partNo") : .MaterialType = qline.Item("Material_Type") : .LineNo = qline.Item("line_No") : .ListPrice = qline("listPrice")
                .UnitPrice = qline.Item("NewUnitPrice") : .Qty = qline.Item("qty")
            End With
            LineItems.Add(LineItem)
        Next
        Return CalcANADocGP(LineItems)
    End Function

    <Serializable()>
    Public Class US_Cost_GP_Cache
        Public Property RootBTOItem As String : Public Property PriceConditionList As List(Of PN_COND)
        Public Sub New()
            RootBTOItem = "" : PriceConditionList = New List(Of PN_COND)
            Dim cmd As New SqlClient.SqlCommand(
                " select top 1 a.PART_NO from MyAdvantechGlobal.dbo.SAP_PRODUCT a (nolock) inner join MyAdvantechGlobal.dbo.SAP_PRODUCT_STATUS b (nolock) on a.PART_NO=b.PART_NO " +
                " where a.MATERIAL_GROUP='BTOS' and b.PRODUCT_STATUS='A' and b.SALES_ORG=@ORG and a.PART_NO like 'IPC-%-BTO' ",
                New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
            cmd.Parameters.AddWithValue("ORG", "US01")
            cmd.Connection.Open() : RootBTOItem = cmd.ExecuteScalar().ToString() : cmd.Connection.Close()
        End Sub
    End Class

    <Serializable()>
    Public Class PN_COND
        Public Property COND_TYPE As String : Public Property PART_NO As String : Public Property COND_VAL As Decimal
        Public Property COND_UNIT As String : Public Property COND_P_UNIT As String : Public Property CURCY As String
    End Class

    Public Shared Function GetUSPN_Cost_GPPercent(PartNo As String) As List(Of PN_COND)
        Dim US_Cost_GP_Cache1 As US_Cost_GP_Cache = Nothing
        If HttpContext.Current IsNot Nothing Then
            If HttpContext.Current.Cache("US Cost GP") IsNot Nothing Then
                Try
                    US_Cost_GP_Cache1 = HttpContext.Current.Cache("US Cost GP")
                Catch ex As InvalidCastException
                    US_Cost_GP_Cache1 = Nothing : HttpContext.Current.Cache.Remove("US Cost GP")
                End Try
            End If
            If US_Cost_GP_Cache1 Is Nothing Then
                US_Cost_GP_Cache1 = New US_Cost_GP_Cache()
                HttpContext.Current.Cache.Add("US Cost GP", US_Cost_GP_Cache1, Nothing, Now.AddHours(9), Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
            End If
        Else
            US_Cost_GP_Cache1 = New US_Cost_GP_Cache()
        End If
        'US_Cost_GP_Cache1.PriceConditionList.Clear()
        SyncLock US_Cost_GP_Cache1.PriceConditionList
            Dim cacheData = From q In US_Cost_GP_Cache1.PriceConditionList Where q.PART_NO = PartNo

            If cacheData.Count > 0 Then
                Dim PNCondList As New List(Of PN_COND) : PNCondList.AddRange(cacheData) : Return PNCondList
            End If
        End SyncLock



        Dim ReturnConditions As New List(Of PN_COND)
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        With OrderHeader
            .Doc_Type = "ZOR" : .Sales_Org = "US01" : .Distr_Chan = "10"
            .Division = "10" : .Sales_Grp = "" : .Sales_Off = ""
        End With
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = "UAAC00100" : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = "UAAC00100"
        Partners.Add(SoldTo) : Partners.Add(ShipTo)

        Dim rootItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        rootItem.Itm_Number = "100" : rootItem.Material = US_Cost_GP_Cache1.RootBTOItem : rootItem.Req_Qty = 1 : ItemsIn.Add(rootItem)
        Dim LineNo As Integer = 101, NewItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        With NewItem
            .Itm_Number = LineNo.ToString() : .Material = FormatToSAPPartNo(Trim(PartNo).ToUpper()) : .Req_Qty = 1 : .Hg_Lv_Item = "100"
        End With
        ItemsIn.Add(NewItem) : LineNo += 1
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE
        proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        proxy1.Connection.Open()
        Try
            proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "",
                                           New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO,
                                           New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt,
                                           New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable,
                                           ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)
        Catch ex As Exception
            proxy1.Connection.Close()
            'Util.InsertMyErrLog("Call SAP RFC Bapi_Salesorder_Simulate error:" + ex.ToString())
            Return Nothing
        End Try

        proxy1.Connection.Close()

        'gv1.DataSource = Conditions.ToADODataTable() : gv1.DataBind()

        Dim lqCond As New List(Of BAPI_SALESORDER_SIMULATE.BAPICOND)
        For Each condRec As BAPI_SALESORDER_SIMULATE.BAPICOND In Conditions
            If condRec.Itm_Number <> "000100" Then lqCond.Add(condRec)
        Next

        For Each item As BAPI_SALESORDER_SIMULATE.BAPIITEMIN In ItemsIn
            Dim r = From q In lqCond Where q.Itm_Number = item.Itm_Number And (q.Cond_Type = "VPRS" Or q.Cond_Type = "ZMGP") And item.Itm_Number <> "100"

            For Each c In r
                Dim pncond1 As New PN_COND
                With pncond1
                    .COND_TYPE = c.Cond_Type : .PART_NO = RemovePrecedingZeros(item.Material) : .COND_VAL = c.Cond_Value : .COND_UNIT = c.Cond_Unit
                    .COND_P_UNIT = c.Cond_P_Unt : .CURCY = c.Currency
                End With

                ReturnConditions.Add(pncond1)
            Next
        Next

        SyncLock US_Cost_GP_Cache1.PriceConditionList
            US_Cost_GP_Cache1.PriceConditionList.AddRange(ReturnConditions)
        End SyncLock

        Return ReturnConditions
    End Function

    Public Shared Function GetCNPN_Cost_GPPercent(PartNo As String) As List(Of PN_COND)
        Dim US_Cost_GP_Cache1 As US_Cost_GP_Cache = Nothing
        If HttpContext.Current IsNot Nothing Then
            If HttpContext.Current.Cache("CN Cost GP") IsNot Nothing Then
                Try
                    US_Cost_GP_Cache1 = HttpContext.Current.Cache("CN Cost GP")
                Catch ex As InvalidCastException
                    US_Cost_GP_Cache1 = Nothing : HttpContext.Current.Cache.Remove("CN Cost GP")
                End Try
            End If
            If US_Cost_GP_Cache1 Is Nothing Then
                US_Cost_GP_Cache1 = New US_Cost_GP_Cache()
                HttpContext.Current.Cache.Add("CN Cost GP", US_Cost_GP_Cache1, Nothing, Now.AddHours(9), Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
            End If
        Else
            US_Cost_GP_Cache1 = New US_Cost_GP_Cache()
        End If
        'US_Cost_GP_Cache1.PriceConditionList.Clear()
        SyncLock US_Cost_GP_Cache1.PriceConditionList
            Dim cacheData = From q In US_Cost_GP_Cache1.PriceConditionList Where q.PART_NO = PartNo

            If cacheData.Count > 0 Then
                Dim PNCondList As New List(Of PN_COND) : PNCondList.AddRange(cacheData) : Return PNCondList
            End If
        End SyncLock



        Dim ReturnConditions As New List(Of PN_COND)
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        With OrderHeader
            .Doc_Type = "ZOR" : .Sales_Org = "CN10" : .Distr_Chan = "30"
            .Division = "00" : .Sales_Grp = "" : .Sales_Off = ""
        End With
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = "C200020" : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = "C200020"
        Partners.Add(SoldTo) : Partners.Add(ShipTo)

        Dim rootItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        rootItem.Itm_Number = "100" : rootItem.Material = US_Cost_GP_Cache1.RootBTOItem : rootItem.Req_Qty = 1 : ItemsIn.Add(rootItem)
        Dim LineNo As Integer = 101, NewItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        With NewItem
            .Itm_Number = LineNo.ToString() : .Material = FormatToSAPPartNo(Trim(PartNo).ToUpper()) : .Req_Qty = 1 : .Hg_Lv_Item = "100"
        End With
        ItemsIn.Add(NewItem) : LineNo += 1
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE
        proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        proxy1.Connection.Open()
        Try
            proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "",
                                           New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO,
                                           New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt,
                                           New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable,
                                           ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)
        Catch ex As Exception
            proxy1.Connection.Close()
            'Util.InsertMyErrLog("Call SAP RFC Bapi_Salesorder_Simulate error:" + ex.ToString())
            Return Nothing
        End Try

        proxy1.Connection.Close()

        'gv1.DataSource = Conditions.ToADODataTable() : gv1.DataBind()

        Dim lqCond As New List(Of BAPI_SALESORDER_SIMULATE.BAPICOND)
        For Each condRec As BAPI_SALESORDER_SIMULATE.BAPICOND In Conditions
            If condRec.Itm_Number <> "000100" Then lqCond.Add(condRec)
        Next

        For Each item As BAPI_SALESORDER_SIMULATE.BAPIITEMIN In ItemsIn
            Dim r = From q In lqCond Where q.Itm_Number = item.Itm_Number And (q.Cond_Type = "VPRS" Or q.Cond_Type = "ZMGP") And item.Itm_Number <> "100"

            For Each c In r
                Dim pncond1 As New PN_COND
                With pncond1
                    .COND_TYPE = c.Cond_Type : .PART_NO = RemovePrecedingZeros(item.Material) : .COND_VAL = c.Cond_Value : .COND_UNIT = c.Cond_Unit
                    .COND_P_UNIT = c.Cond_P_Unt : .CURCY = c.Currency
                End With

                ReturnConditions.Add(pncond1)
            Next
        Next

        SyncLock US_Cost_GP_Cache1.PriceConditionList
            US_Cost_GP_Cache1.PriceConditionList.AddRange(ReturnConditions)
        End SyncLock

        Return ReturnConditions
    End Function


    Public Shared Function GetAKRPN_Cost_GPPercent(QuoteID As String, ByRef errorMsg As String) As List(Of PN_COND)
        Dim QMdt As DataTable = dbUtil.dbGetDataTable("EQ", String.Format("select TOP 1 ORG from QuotationMaster  where quoteid='{0}'", QuoteID))
        Dim QITEMSdt As DataTable = dbUtil.dbGetDataTable("EQ", String.Format("select partNo, line_No,HigherLevel from  QuotationDetail  where quoteid='{0}' ORDER BY line_No", QuoteID))
        Dim QPARTNERdt As DataTable = dbUtil.dbGetDataTable("EQ", String.Format("select  TYPE,ERPID  from  EQPARTNER  where quoteid='{0}'", QuoteID))
        Dim ReturnConditions As New List(Of PN_COND)
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        Dim Sales_Org As String = "BR01" : If Not IsDBNull(QMdt.Rows(0).Item("ORG")) Then Sales_Org = QMdt.Rows(0).Item("ORG")
        Dim Distr_Chan As String = "10" ' : If Not IsDBNull(QMdt.Rows(0).Item("DIST_CHAN")) Then Distr_Chan = QMdt.Rows(0).Item("DIST_CHAN")
        Dim Division As String = "00" ' : If Not IsDBNull(QMdt.Rows(0).Item("DIVISION")) Then Division = QMdt.Rows(0).Item("DIVISION")
        Dim DefaultSoldTo As String = "AKRCE0416 " : Dim DefaultShipTo As String = "AKRCE0416 "
        Dim Erps As String() = New String() {"SOLDTO", "S"}
        Dim query As IEnumerable(Of DataRow) = QPARTNERdt.AsEnumerable()
        For Each erp In Erps
            Dim dr As DataRow = query.SingleOrDefault(Function(P) P.Item("TYPE") = erp)
            If dr IsNot Nothing AndAlso Not IsDBNull(dr.Item("ERPID")) AndAlso Not String.IsNullOrEmpty(dr.Item("ERPID")) Then
                If String.Equals(erp, "SOLDTO") Then DefaultSoldTo = dr.Item("ERPID").ToString.Trim
                If String.Equals(erp, "S") Then DefaultShipTo = dr.Item("ERPID").ToString.Trim
            End If
        Next
        With OrderHeader
            .Doc_Type = "ZOR" : .Sales_Org = Sales_Org : .Distr_Chan = Distr_Chan
            .Division = Division : .Sales_Grp = "" : .Sales_Off = ""
        End With
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = DefaultSoldTo : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = DefaultShipTo
        Partners.Add(SoldTo) : Partners.Add(ShipTo)

        'Dim rootItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        'rootItem.Itm_Number = "100" : rootItem.Material = US_Cost_GP_Cache1.RootBTOItem : rootItem.Req_Qty = 1 : ItemsIn.Add(rootItem)
        ' Dim LineNo As Integer = 101, NewItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        Dim RootBTOItem As String = ""
        Dim items As IEnumerable(Of DataRow) = QITEMSdt.AsEnumerable()
        If items.Where(Function(p) p.Item("line_No") >= 100).Count() > 0 Then
            Dim cmd As New SqlClient.SqlCommand(
              " select top 1 a.PART_NO from MyAdvantechGlobal.dbo.SAP_PRODUCT a (nolock) inner join MyAdvantechGlobal.dbo.SAP_PRODUCT_STATUS b (nolock) on a.PART_NO=b.PART_NO " +
              " where a.MATERIAL_GROUP='BTOS' and b.PRODUCT_STATUS='A' and b.SALES_ORG=@ORG and a.PART_NO like 'IPC-%-BTO' ",
              New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
            cmd.Parameters.AddWithValue("ORG", "KR01")
            cmd.Connection.Open() : RootBTOItem = cmd.ExecuteScalar().ToString() : cmd.Connection.Close()
        End If
        For Each dr In items
            Dim NewItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
            With NewItem
                Dim lineNo As Integer = Integer.Parse(dr.Item("line_No"))
                .Itm_Number = dr.Item("line_No")
                If lineNo Mod 100 = 0 Then
                    .Material = RootBTOItem
                Else
                    .Material = FormatToSAPPartNo(Trim(dr.Item("partNo")).ToUpper())
                End If

                .Req_Qty = 1 : .Hg_Lv_Item = dr.Item("HigherLevel")
            End With
            ItemsIn.Add(NewItem)
        Next



        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE
        proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        proxy1.Connection.Open()
        Try
            proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "",
                                           New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO,
                                           New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt,
                                           New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable,
                                           ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)
        Catch ex As Exception
            proxy1.Connection.Close()
            'Util.InsertMyErrLog("Call SAP RFC Bapi_Salesorder_Simulate error:" + ex.ToString())
            Return Nothing
        End Try

        proxy1.Connection.Close()

        Dim drs As IEnumerable(Of DataRow) = retDt.ToADODataTable().AsEnumerable()
        For Each dr In drs
            If String.Equals(dr.Item("Type"), "E") Then
                errorMsg = dr.Item("Message")
                Return ReturnConditions
                Exit For
            End If
        Next
        'gv1.DataSource = Conditions.ToADODataTable() : gv1.DataBind()

        Dim lqCond As New List(Of BAPI_SALESORDER_SIMULATE.BAPICOND)
        For Each condRec As BAPI_SALESORDER_SIMULATE.BAPICOND In Conditions
            If condRec.Itm_Number <> "000100" Then lqCond.Add(condRec)
        Next

        For Each item As BAPI_SALESORDER_SIMULATE.BAPIITEMIN In ItemsIn
            Dim r = From q In lqCond Where q.Itm_Number = item.Itm_Number And (q.Cond_Type = "VPRS") ' Or q.Cond_Type = "ZMGP") And item.Itm_Number <> "100"
            For Each c In r
                Dim pncond1 As New PN_COND
                With pncond1
                    .COND_TYPE = c.Cond_Type : .PART_NO = RemovePrecedingZeros(item.Material) : .COND_VAL = c.Cond_Value : .COND_UNIT = c.Cond_Unit
                    .COND_P_UNIT = c.Cond_P_Unt : .CURCY = c.Currency
                End With
                ReturnConditions.Add(pncond1)
            Next
        Next
        'SyncLock US_Cost_GP_Cache1.PriceConditionList
        '    US_Cost_GP_Cache1.PriceConditionList.AddRange(ReturnConditions)
        'End SyncLock

        Return ReturnConditions
    End Function
    Public Shared Function GetInterconPN_Cost_GPPercent(QuoteID As String, ByRef errorMsg As String) As List(Of PN_COND)
        Dim QMdt As DataTable = dbUtil.dbGetDataTable("EQ", String.Format("select TOP 1 ORG,currency from QuotationMaster  where quoteid='{0}'", QuoteID))
        Dim Sales_Org As String = "TW01" : If Not IsDBNull(QMdt.Rows(0).Item("ORG")) Then Sales_Org = QMdt.Rows(0).Item("ORG")

        'Dim QITEMSdt As DataTable = dbUtil.dbGetDataTable("EQ", String.Format("select partNo, line_No,HigherLevel,QTY,reqDate from  QuotationDetail  where quoteid='{0}' ORDER BY line_No", QuoteID))
        'Dim QITEMSdt As DataTable = dbUtil.dbGetDataTable("EQ", String.Format("select a.partNo, a.line_No,a.HigherLevel,a.QTY,a.reqDate from QuotationDetail a left join MyAdvantechGlobal.dbo.SAP_PRODUCT_STATUS b on a.partNo = b.PART_NO where a.quoteId = '{0}' and b.PRODUCT_STATUS <> 'S' and b.SALES_ORG = '{1}' order by a.line_No", QuoteID, Sales_Org))
        Dim QITEMSdt As DataTable = dbUtil.dbGetDataTable("EQ", String.Format("select a.partNo, a.line_No,a.HigherLevel,a.QTY,a.reqDate,isnull(a.deliveryPlant,'') as DeliveryPlant  from QuotationDetail a left join MyAdvantechGlobal.dbo.SAP_PRODUCT_STATUS b on a.partNo = b.PART_NO where a.quoteId = '{0}'  and b.PRODUCT_STATUS <> 'S' and b.SALES_ORG = '{1}' order by a.line_No", QuoteID, Sales_Org))

        Dim QPARTNERdt As DataTable = dbUtil.dbGetDataTable("EQ", String.Format("select  TYPE,ERPID  from  EQPARTNER  where quoteid='{0}'", QuoteID))
        Dim ReturnConditions As New List(Of PN_COND)
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        Dim S_ScheLineDT As New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable
        Dim Distr_Chan As String = "10" ' : If Not IsDBNull(QMdt.Rows(0).Item("DIST_CHAN")) Then Distr_Chan = QMdt.Rows(0).Item("DIST_CHAN")
        Dim Division As String = "00" ' : If Not IsDBNull(QMdt.Rows(0).Item("DIVISION")) Then Division = QMdt.Rows(0).Item("DIVISION")
        Dim DefaultSoldTo As String = "INTERCON" : Dim DefaultShipTo As String = "INTERCON"

        'Dim DefaultCurrency As String = "USD" : If Not IsDBNull(QMdt.Rows(0).Item("currency")) Then DefaultCurrency = QMdt.Rows(0).Item("currency")
        Dim DefaultCurrency As String = "USD"
        Dim AccountCurrency As String = "USD" : If Not IsDBNull(QMdt.Rows(0).Item("currency")) Then AccountCurrency = QMdt.Rows(0).Item("currency")

        Dim Erps As String() = New String() {"SOLDTO", "S"}
        'Dim query As IEnumerable(Of DataRow) = QPARTNERdt.AsEnumerable()
        'For Each erp In Erps
        '    Dim dr As DataRow = query.SingleOrDefault(Function(P) P.Item("TYPE") = erp)
        '    If dr IsNot Nothing AndAlso Not IsDBNull(dr.Item("ERPID")) AndAlso Not String.IsNullOrEmpty(dr.Item("ERPID")) Then
        '        If String.Equals(erp, "SOLDTO") Then DefaultSoldTo = dr.Item("ERPID").ToString.Trim
        '        If String.Equals(erp, "S") Then DefaultShipTo = dr.Item("ERPID").ToString.Trim
        '    End If
        'Next
        With OrderHeader
            .Doc_Type = "AG"
            .Sales_Org = Sales_Org : .Distr_Chan = Distr_Chan
            .Division = Division : .Sales_Grp = "" : .Sales_Off = "" : .Currency = DefaultCurrency
        End With
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = DefaultSoldTo : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = DefaultShipTo
        Partners.Add(SoldTo) : Partners.Add(ShipTo)

        'Dim rootItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        'rootItem.Itm_Number = "100" : rootItem.Material = US_Cost_GP_Cache1.RootBTOItem : rootItem.Req_Qty = 1 : ItemsIn.Add(rootItem)
        ' Dim LineNo As Integer = 101, NewItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
        Dim RootBTOItem As String = ""
        Dim items As IEnumerable(Of DataRow) = QITEMSdt.AsEnumerable()
        If items.Where(Function(p) p.Item("line_No") >= 100).Count() > 0 Then
            Dim cmd As New SqlClient.SqlCommand(
              " select top 1 a.PART_NO from MyAdvantechGlobal.dbo.SAP_PRODUCT a (nolock) inner join MyAdvantechGlobal.dbo.SAP_PRODUCT_STATUS b (nolock) on a.PART_NO=b.PART_NO " +
              " where a.MATERIAL_GROUP='BTOS' and b.PRODUCT_STATUS='A' and b.SALES_ORG=@ORG and a.PART_NO like 'IPC-%-BTO' ",
              New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
            cmd.Parameters.AddWithValue("ORG", "TW01")
            cmd.Connection.Open() : RootBTOItem = cmd.ExecuteScalar().ToString() : cmd.Connection.Close()
        End If
        For Each dr In items
            If Not dr.Item("partNo").StartsWith("AGS-", StringComparison.CurrentCultureIgnoreCase) Then
                Dim NewItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                Dim S_ScheLineRow As New BAPI_SALESORDER_SIMULATE.BAPISDHEDU
                With NewItem
                    Dim lineNo As Integer = Integer.Parse(dr.Item("line_No"))
                    .Itm_Number = dr.Item("line_No")
                    If lineNo Mod 100 = 0 Then
                        .Material = RootBTOItem
                    Else
                        .Material = FormatToSAPPartNo(Trim(dr.Item("partNo")).ToUpper())
                    End If

                    'Frank added 20170628,怡萱建議查價固定加plant
                    If dr.Item("DeliveryPlant").ToString() = "" Then
                        .Plant = "TWH1"
                    Else
                        .Plant = dr.Item("DeliveryPlant").ToString()
                    End If

                    .Req_Qty = 1000
                    .Hg_Lv_Item = dr.Item("HigherLevel")

                    S_ScheLineRow.Itm_Number = dr.Item("line_No") : S_ScheLineRow.Req_Qty = 1 : S_ScheLineRow.Req_Date = dr.Item("reqDate")
                End With
                ItemsIn.Add(NewItem)
                S_ScheLineDT.Add(S_ScheLineRow)
            End If
        Next
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE
        proxy1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        proxy1.Connection.Open()
        Try
            proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "",
                                           New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO,
                                           New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt,
                                           New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable,
                                           New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable,
                                           ItemsIn, ItemsOut, Partners, S_ScheLineDT,
                                           New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)
        Catch ex As Exception
            proxy1.Connection.Close()
            'Util.InsertMyErrLog("Call SAP RFC Bapi_Salesorder_Simulate error:" + ex.ToString())
            Return Nothing
        End Try

        proxy1.Connection.Close()

        Dim drs As IEnumerable(Of DataRow) = retDt.ToADODataTable().AsEnumerable()
        For Each dr In drs
            If String.Equals(dr.Item("Type"), "E") Then
                errorMsg = dr.Item("Message")
                Return ReturnConditions
                Exit For
            End If
        Next
        'gv1.DataSource = Conditions.ToADODataTable() : gv1.DataBind()
        Dim PInDt As DataTable = ItemsIn.ToADODataTable()
        Dim POutDt As DataTable = ItemsOut.ToADODataTable()
        Dim lqCond As New List(Of BAPI_SALESORDER_SIMULATE.BAPICOND)
        For Each condRec As BAPI_SALESORDER_SIMULATE.BAPICOND In Conditions
            If condRec.Itm_Number <> "000100" Then lqCond.Add(condRec)
        Next

        'For Each item As BAPI_SALESORDER_SIMULATE.BAPIITEMIN In ItemsIn
        '         Dim r = From q In lqCond Where q.Itm_Number = item.Itm_Number And (q.Cond_Type = "ZPN0" OrElse q.Cond_Type = "ZPR0") '' And item.Itm_Number <> "100"
        '         Dim ZMIP As BAPI_SALESORDER_SIMULATE.BAPICOND = r.FirstOrDefault(Function(p) p.Cond_Type = "ZPN0")
        '         'If ZMIP Is Nothing Then
        '         '    ZMIP = r.FirstOrDefault(Function(p) p.Cond_Type = "ZPR0")
        '         'End If
        '         If ZMIP IsNot Nothing Then
        '             Dim pncond1 As New PN_COND
        '             With pncond1
        '                 .COND_TYPE = ZMIP.Cond_Type : .PART_NO = RemovePrecedingZeros(item.Material)
        '                 .COND_VAL = ZMIP.Cond_Value
        '                 If Not String.Equals(DefaultCurrency, ZMIP.Currency_2) Then
        '                     .COND_VAL = ZMIP.Cond_Value * CType(get_exchangerate(ZMIP.Currency_2, DefaultCurrency).ToString, Decimal)
        '                 End If
        '                 .COND_UNIT = ZMIP.Cond_Unit : .COND_P_UNIT = ZMIP.Cond_P_Unt : .CURCY = ZMIP.Currency
        '             End With
        '             ReturnConditions.Add(pncond1)
        '         End If
        '     Next
        'SyncLock US_Cost_GP_Cache1.PriceConditionList
        '    US_Cost_GP_Cache1.PriceConditionList.AddRange(ReturnConditions)
        'End SyncLock
        For Each PIn As DataRow In PInDt.Rows
            Dim POutRs() As DataRow = POutDt.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
            If POutRs.Length > 0 Then
                Dim UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
                Dim pncond1 As New PN_COND
                With pncond1
                    .COND_TYPE = "AG" : .PART_NO = RemovePrecedingZeros(PIn.Item("Material"))
                    .COND_VAL = UNIT_PRICE
                    'Frank 20180629
                    'If Not String.Equals(DefaultCurrency, POutRs(0).Item("Currency")) Then
                    '    .COND_VAL = UNIT_PRICE * CType(get_exchangerate(POutRs(0).Item("Currency"), DefaultCurrency).ToString, Decimal)
                    'End If
                    '.COND_UNIT = "" : .COND_P_UNIT = "" : .CURCY = DefaultCurrency

                    If Not String.Equals(AccountCurrency, POutRs(0).Item("Currency")) Then
                        If AccountCurrency.Equals("EUR", StringComparison.InvariantCultureIgnoreCase) Then
                            .COND_VAL = UNIT_PRICE / 1.17
                        Else
                            .COND_VAL = UNIT_PRICE * CType(get_exchangerate(POutRs(0).Item("Currency"), AccountCurrency).ToString, Decimal)
                        End If
                    End If
                    .COND_UNIT = "" : .COND_P_UNIT = "" : .CURCY = DefaultCurrency

                End With
                ReturnConditions.Add(pncond1)
            End If
        Next
        Return ReturnConditions
    End Function
    <WebMethod()>
    Public Function GetListPrice(ByVal SAPOrg As String, ByVal SiebelOrg As String, ByVal Currency As String,
                                 ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOutDataTable,
                                 ByRef ErrorMessage As String) As Boolean
        Dim strERPID As String = "", strOrgId As String = Trim(UCase(SAPOrg))
        Currency = Trim(UCase(Currency))
        Select Case Left(strOrgId, 2)
            Case "AU"
                strERPID = "AAU105" : strOrgId = "AU01"
                'Case "BR"
                '    strERPID = ""
            Case "CN"
                strERPID = "C100001" : strOrgId = "CN10"
            Case "EU"
                strERPID = "EDATEV01" : strOrgId = "EU10"
                'Case "HK"
                '    strERPID = ""
            Case "JP"
                strERPID = "JJCBOM" : strOrgId = "JP01"
            Case "KR"
                strERPID = "AKRC00485" : strOrgId = "KR01"
                'Case "MY"
                '    strERPID = ""
            Case "SG"
                strERPID = "SSAONLINE" : strOrgId = "SG01"
                'Case "TL"
                '    strERPID = ""
            Case "TW"
                strERPID = "2NC00001" : strOrgId = "TW01"
            Case "US"
                strERPID = "UEPP5001" : strOrgId = "US01"
            Case Else
                ErrorMessage = "Org " + SAPOrg + " is not yet defined in WS GetListPrice" : Return False
        End Select
        ProductOut = New SAPDALDS.ProductOutDataTable
        Dim tmpProdOut As New SAPDALDS.ProductOutDataTable
        If strOrgId = "EU10" And (Currency = "USD" Or Currency = "EUR" Or Currency = "GBP") Then
            Dim eQConn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString), cmd As SqlClient.SqlCommand = Nothing
            eQConn.Open()
            For Each pinRec As SAPDALDS.ProductInRow In ProductIn.Rows
                'cmd = New SqlClient.SqlCommand( _
                '     "select top 1 LIST_PRICE from eQuotation.dbo.PRODUCT_LIST_PRICE " + _
                '    " where ORG='EU10' and PART_NO=@PN and CURRENCY=@CUR and LIST_PRICE>0", eQConn)
                cmd = New SqlClient.SqlCommand(
                     "select top 1 LIST_PRICE from PRODUCT_LIST_PRICE " +
                    " where ORG='EU10' and PART_NO=@PN and CURRENCY=@CUR and LIST_PRICE>0", eQConn)

                cmd.Parameters.AddWithValue("PN", pinRec.PART_NO) : cmd.Parameters.AddWithValue("CUR", pinRec.PART_NO)
                Dim tmpLP As Object = cmd.ExecuteScalar()
                If tmpLP IsNot Nothing AndAlso Double.TryParse(tmpLP, 0) Then
                    tmpProdOut.AddProductOutRow(pinRec.PART_NO, tmpLP.ToString(), tmpLP.ToString(), "0", "0", "0", "0")
                End If
            Next
            eQConn.Close()
        End If
        For Each poutRec As SAPDALDS.ProductOutRow In tmpProdOut.Rows
            Dim InRs() = ProductIn.Select("PART_NO='" + poutRec.PART_NO + "'")
            For Each inR As SAPDALDS.ProductInRow In InRs
                inR.Delete()
            Next
        Next
        If ProductIn.Rows.Count > 0 Then
            If GetPrice(strERPID, strERPID, strOrgId, Currency, "", ProductIn, ProductOut, ErrorMessage) Then
                ProductOut.Merge(tmpProdOut)
                Return True
            Else
                Return False
            End If
        End If
    End Function

    <WebMethod()>
    Public Function GetPriceV2(ByVal SoldToId As String, ByVal ShipToId As String, ByVal Org As String, ByVal DocOrderType As SAPOrderType,
                             ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOutDataTable,
                             ByRef ErrorMessage As String) As Boolean
        Dim PipeLinePIn As New SAPDALDS.ProductInDataTable
        For Each OriPInRec As SAPDALDS.ProductInRow In ProductIn.Rows
            If OriPInRec.PART_NO.Contains("|") Then
                Dim strProds() As String = Split(OriPInRec.PART_NO, "|")
                If strProds.Length > 1 Then
                    OriPInRec.PART_NO = strProds(0)
                    For i As Integer = 1 To strProds.Length - 1
                        PipeLinePIn.AddProductInRow(strProds(i), OriPInRec.QTY, "")
                    Next
                End If
            End If
        Next
        For Each pipePInRec As SAPDALDS.ProductInRow In PipeLinePIn.Rows
            ProductIn.AddProductInRow(pipePInRec.PART_NO, pipePInRec.QTY, "")
        Next
        For Each OriPInRec As SAPDALDS.ProductInRow In ProductIn.Rows
            If OriPInRec.PART_NO.Equals("Build In", StringComparison.OrdinalIgnoreCase) OrElse OriPInRec.PART_NO.Equals("No Need", StringComparison.OrdinalIgnoreCase) Then
                OriPInRec.Delete()
            End If
        Next
        Try
            ErrorMessage = ""
            SoldToId = UCase(Trim(SoldToId)) : Org = Trim(UCase(Org))
            If String.IsNullOrEmpty(ShipToId) Then ShipToId = SoldToId
            Dim strDistChann As String = "10", strDivision As String = "00"
            If Org = "US01" Then
                Dim N As Integer = dbUtil.dbExecuteScalar("MY", String.Format(
                                                          "select COUNT(COMPANY_ID) from SAP_DIMCOMPANY " +
                                                          " where SALESOFFICE in ('2300','2700') and COMPANY_ID='{0}' and ORG_ID='US01'", SoldToId))
                If N > 0 Then
                    strDistChann = "10" : strDivision = "20"
                Else
                    strDistChann = "30" : strDivision = "10"
                End If
            End If
            For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
                PInRow.PART_NO = PInRow.PART_NO.ToUpper()
            Next
            If True Then
                Return GetMultiPrice_eStoreV2(Org, SoldToId, ShipToId, strDistChann, strDivision, DocOrderType, ProductIn, ProductOut, ErrorMessage)
            Else
                Dim eup As New Get_Price.Get_Price
                Dim pin As New Get_Price.ZSSD_01Table, pout As New Get_Price.ZSSD_02Table
                For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
                    Dim prec As New Get_Price.ZSSD_01
                    With prec
                        .Kunnr = SoldToId : .Mandt = "168" : .Matnr = Global_Inc.Format2SAPItem(PInRow.PART_NO) : .Mglme = 1 : .Vkorg = Org
                        ' .Prsdt = Now.Date.ToString("yyyyMMdd")
                    End With
                    pin.Add(prec)
                Next
                eup.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
                eup.Connection.Open()
                Try
                    eup.Z_Ebizaeu_Priceinquiry(strDistChann, strDivision, SoldToId, Org, SoldToId, New Get_Price.BAPIRETURN, pin, pout)
                Catch ex As Exception
                    ErrorMessage = "Call Z_Ebizaeu_Priceinquiry error:" + ex.ToString()
                    eup.Connection.Close() : Return False
                End Try
                eup.Connection.Close()
                For Each x As Get_Price.ZSSD_02 In pout
                    If x.Kzwi1 < x.Netwr Then
                        x.Kzwi1 = x.Netwr
                    End If
                Next
                Dim retTable As DataTable = pout.ToADODataTable()
                ProductOut = New SAPDALDS.ProductOutDataTable
                For Each retRec As DataRow In retTable.Rows
                    'pout.Item(0).Matnr : pout.Item(0).Netwr : pout.Item(0).Kzwi1
                    Dim ProductOutRec As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
                    ProductOutRec.PART_NO = Global_Inc.RemoveZeroString(retRec.Item("Matnr"))
                    ProductOutRec.LIST_PRICE = retRec.Item("Kzwi1")
                    ProductOutRec.UNIT_PRICE = retRec.Item("Netwr")
                    ProductOut.AddProductOutRow(ProductOutRec)
                Next
                Return True
            End If
        Catch ex As Exception
            ErrorMessage += ".Runtime exception:" + ex.ToString()
            Throw New Exception(ErrorMessage)
        End Try
    End Function
    Function getDivision(ByVal companyId As String) As String
        Dim dt As New DataTable
        Dim sql As String = String.Format("SELECT SPART FROM SAPRDP.KNVV WHERE KUNNR='{0}' AND ROWNUM=1", companyId.ToUpper)
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sql)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("SPART")
        End If
        Return ""
    End Function
    Public Shared Function isMEDC(ByVal Part_No As String) As Boolean
        Dim str As String = String.Format(" select COUNT(part_no) from SAP_PRODUCT where PART_NO='{0}' and PRODUCT_LINE in " &
                                            "('DLGR'," &
                                            "'POCI'," &
                                            "'ITPA'," &
                                            "'POCA'," &
                                            "'IPCV'," &
                                            "'POCN'," &
                                            "'POCS'," &
                                            "'HITW')", Part_No)
        Dim O As Object = dbUtil.dbExecuteScalar("MY", str)
        If Integer.TryParse(O, 0) AndAlso CInt(O) > 0 Then
            Return True
        End If
        Return False
    End Function
    Public Function GetPrice(ByVal SoldToId As String, ByVal ShipToId As String, ByVal Org As String, ByVal Currency As String,
                              ByVal OrderType As String, ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOutDataTable,
                             ByRef ErrorMessage As String, Optional ByVal division As String = "") As Boolean
        Dim PipeLinePIn As New SAPDALDS.ProductInDataTable
        For Each OriPInRec As SAPDALDS.ProductInRow In ProductIn.Rows
            If OriPInRec.PART_NO.Contains("|") Then
                Dim strProds() As String = Split(OriPInRec.PART_NO, "|")
                If strProds.Length > 1 Then
                    OriPInRec.PART_NO = strProds(0)
                    For i As Integer = 1 To strProds.Length - 1
                        PipeLinePIn.AddProductInRow(strProds(i), OriPInRec.QTY, "")
                    Next
                End If
            End If
        Next
        For Each pipePInRec As SAPDALDS.ProductInRow In PipeLinePIn.Rows
            ProductIn.AddProductInRow(pipePInRec.PART_NO, pipePInRec.QTY, "")
        Next
        'For Each OriPInRec As SAPDALDS.ProductInRow In ProductIn.Rows
        '    If OriPInRec.PART_NO.Equals("No Need", StringComparison.OrdinalIgnoreCase) Then
        '        OriPInRec.Delete()
        '    End If
        'Next
        Try
            ErrorMessage = ""
            SoldToId = UCase(Trim(SoldToId)) : Org = Trim(UCase(Org))
            If String.IsNullOrEmpty(ShipToId) Then ShipToId = SoldToId
            Dim strDistChann As String = "10", strDivision As String = "00"
            If Org = "US01" Then
                'Dim N As Integer = dbUtil.dbExecuteScalar("MY", String.Format( _
                '                                          "select COUNT(COMPANY_ID) from SAP_DIMCOMPANY " + _
                '                                          " where SALESOFFICE in ('2300','2700') and COMPANY_ID='{0}' and ORG_ID='US01'", SoldToId))
                If SoldToId.Equals("UAON00001", System.StringComparison.OrdinalIgnoreCase) Then

                    strDistChann = "30" : strDivision = "10"
                Else
                    Dim N As Integer = dbUtil.dbExecuteScalar("MY", String.Format(
                                                         "select COUNT(COMPANY_ID) from SAP_DIMCOMPANY " +
                                                             " where SALESOFFICE in ('2300','2700') and COMPANY_ID='{0}' and ORG_ID='US01'", SoldToId))
                    If N > 0 Then
                        strDistChann = "10" : strDivision = "20"
                    Else
                        strDistChann = "30" : strDivision = "10"
                    End If
                End If
            End If
            For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
                PInRow.PART_NO = PInRow.PART_NO.ToUpper()
            Next
            'If True Then
            Return GetMultiPrice_eStore(Org, SoldToId, ShipToId, Currency, strDistChann, strDivision, OrderType, ProductIn, ProductOut, ErrorMessage)
            'Else
            '    Dim eup As New Get_Price.Get_Price
            '    Dim pin As New Get_Price.ZSSD_01Table, pout As New Get_Price.ZSSD_02Table
            '    For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
            '        Dim prec As New Get_Price.ZSSD_01
            '        With prec
            '            .Kunnr = SoldToId : .Mandt = "168" : .Matnr = Global_Inc.Format2SAPItem(PInRow.PART_NO) : .Mglme = 1 : .Vkorg = Org
            '            ' .Prsdt = Now.Date.ToString("yyyyMMdd")
            '        End With
            '        pin.Add(prec)
            '    Next
            '    eup.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            '    eup.Connection.Open()
            '    Try
            '        eup.Z_Ebizaeu_Priceinquiry(strDistChann, strDivision, SoldToId, Org, SoldToId, New Get_Price.BAPIRETURN, pin, pout)
            '    Catch ex As Exception
            '        ErrorMessage = "Call Z_Ebizaeu_Priceinquiry error:" + ex.ToString()
            '        eup.Connection.Close() : Return False
            '    End Try
            '    eup.Connection.Close()
            '    For Each x As Get_Price.ZSSD_02 In pout
            '        If x.Kzwi1 < x.Netwr Then
            '            x.Kzwi1 = x.Netwr
            '        End If
            '    Next
            '    Dim retTable As DataTable = pout.ToADODataTable()
            '    ProductOut = New SAPDALDS.ProductOutDataTable
            '    For Each retRec As DataRow In retTable.Rows
            '        'pout.Item(0).Matnr : pout.Item(0).Netwr : pout.Item(0).Kzwi1
            '        Dim ProductOutRec As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
            '        ProductOutRec.PART_NO = Global_Inc.RemoveZeroString(retRec.Item("Matnr"))
            '        ProductOutRec.LIST_PRICE = retRec.Item("Kzwi1")
            '        ProductOutRec.UNIT_PRICE = retRec.Item("Netwr")
            '        ProductOut.AddProductOutRow(ProductOutRec)
            '    Next
            '    Return True
            'End If
        Catch ex As Exception
            ErrorMessage += ".Runtime exception:" + ex.ToString()
            Throw New Exception(ErrorMessage)
        End Try
    End Function

    Public Shared Function GetMultiPrice_eStoreV2(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String,
                                          ByVal strDivision As String, ByVal OrderDocType As SAPOrderType, ByVal ProductIn As SAPDALDS.ProductInDataTable,
                                          ByRef ProductOut As SAPDALDS.ProductOutDataTable,
                                          ByRef ErrorMessage As String) As Boolean
        'Util.SendEmail("nada.liu@advantech.com.cn", "myadvanteh@advantech.com", "test price", "AAAA", True, "", "")
        If SoldToId = "CKM4" Then Return False
        ErrorMessage = ""
        Dim HasPhaseOutItem As Boolean = False
        Dim phaseOutItems As New ArrayList, ZSWLItemSet As New DataTable
        With ZSWLItemSet.Columns
            .Add("PartNo") : .Add("Qty", GetType(Integer))
        End With
        Dim RemoveAddedItem As Boolean = False : Dim AddedItemLineNo As String = ""
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))
        If SoldToId = "SAID" Then proxy1.ConnectionString = ConfigurationManager.AppSettings("SAPConnTest")
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        With OrderHeader
            .Doc_Type = OrderDocType.ToString() : .Sales_Org = Trim(UCase(Org)) : .Distr_Chan = strDistChann : .Division = strDivision
            If Org = "BR01" Then .Doc_Type = "ZORB"
        End With
        Dim LineNo As Integer = 1
        Dim sqlMA As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        sqlMA.Open()
        For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
            Dim chkSql As String =
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " +
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " +
                " where a.part_no='" + PInRow.PART_NO + "' and a.product_status in ('A','N','H','O','M1') and a.sales_org='" + Org + "' "
            Dim chkDt As New DataTable, sqlAptr As New SqlClient.SqlDataAdapter(chkSql, sqlMA)
            Try
                sqlAptr.Fill(chkDt)
            Catch ex As SqlClient.SqlException
                sqlMA.Close() : ErrorMessage = ex.ToString() : Return Nothing
            End Try
            If chkDt.Rows.Count > 0 AndAlso (Org <> "TW01" Or (Org = "TW01") And chkDt.Rows(0).Item("ProfitCenter") <> "N/A") Then
                If chkDt.Rows(0).Item("ITEM_CATEGORY_GROUP") <> "ZSWL" Then
                    Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                    item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(PInRow.PART_NO.ToUpper())
                    item.Req_Qty = PInRow.QTY.ToString()
                    item.Req_Qty = CInt(item.Req_Qty) * 1000
                    ItemsIn.Add(item)
                    LineNo += 1
                Else
                    Dim zr As DataRow = ZSWLItemSet.NewRow()
                    zr.Item("PartNo") = PInRow.PART_NO.ToUpper() : zr.Item("Qty") = PInRow.QTY : ZSWLItemSet.Rows.Add(zr)
                End If
            Else
                phaseOutItems.Add(PInRow.PART_NO.ToUpper())
            End If
        Next
        sqlMA.Close()

        If ItemsIn.Count = 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = GetAHighLevelItemForPricing(Org)
            item.Req_Qty = 1
            item.Req_Qty = CInt(item.Req_Qty) * 1000
            ItemsIn.Add(item)
            RemoveAddedItem = True : AddedItemLineNo = LineNo.ToString()
            LineNo += 1
        End If
        If ItemsIn.Count > 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            For Each r As DataRow In ZSWLItemSet.Rows
                Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(r.Item("PartNo").Trim().ToUpper())
                item.Req_Qty = r.Item("Qty").ToString()
                item.Req_Qty = CInt(item.Req_Qty) * 1000
                item.Hg_Lv_Item = "1"
                ItemsIn.Add(item)
                LineNo += 1
            Next
        End If
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
        Dim retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = SoldToId : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = ShipToId
        Partners.Add(SoldTo) : Partners.Add(ShipTo)
        proxy1.Connection.Open()
        Dim isZPN0MT As Boolean = False
        Try

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
            Dim retAdoDt As DataTable = retDt.ToADODataTable()



            For Each retMsgRec As DataRow In retAdoDt.Rows
                If retMsgRec.Item("Type") = "E" Then
                    HasPhaseOutItem = True
                    ErrorMessage += String.Format("Type:{0};Msg:{1}", retMsgRec.Item("Type"), retMsgRec.Item("Message_V1")) + vbCrLf
                End If
            Next

            Dim ConditionOut As DataTable = Conditions.ToADODataTable()
            Dim PInDt As DataTable = ItemsIn.ToADODataTable()
            Dim POutDt As DataTable = ItemsOut.ToADODataTable()

            'gv2.DataSource = retAdoDt : gv2.DataBind()

            DTRET = retDt.ToADODataTable()

            ProductOut = New SAPDALDS.ProductOutDataTable
            For Each PIn As DataRow In PInDt.Rows
                'Dim pout As New ProductOut(RemoveZeroString(PIn.Item("Material")))
                Dim poutRec As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
                poutRec.PART_NO = Global_Inc.RemoveZeroString(PIn.Item("Material"))
                poutRec.LIST_PRICE = 0 : poutRec.RECYCLE_FEE = 0
                Dim rs2() As DataRow = ConditionOut.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
                For Each r As DataRow In rs2
                    Select Case r.Item("Cond_Type").ToString().ToUpper()
                        Case "ZPN0", "ZPR0"
                            If Double.TryParse(poutRec.LIST_PRICE, 0) AndAlso Double.TryParse(r.Item("Cond_Value"), 0) Then
                                If r.Item("Cond_Value") > poutRec.LIST_PRICE Then
                                    poutRec.LIST_PRICE = FormatNumber(r.Item("Cond_Value"), 2)
                                    isZPN0MT = True
                                End If
                            End If
                        Case "ZHB0"
                            poutRec.RECYCLE_FEE = FormatNumber(r.Item("Cond_Value"), 2)
                    End Select
                Next
                Dim POutRs() As DataRow = POutDt.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
                If Global_Inc.IsNumericItem(PIn.Item("Material")) Then
                    If poutRec.LIST_PRICE <= 0 AndAlso POutRs.Length > 0 Then
                        poutRec.LIST_PRICE = FormatNumber(POutRs(0).Item("net_value1") / POutRs(0).Item("req_qty"), 2)
                    End If
                End If
                If POutRs.Length > 0 Then
                    poutRec.TAX = FormatNumber(POutRs(0).Item("Tx_Doc_Cur") / POutRs(0).Item("req_qty"), 2)
                    poutRec.UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
                    If Org = "BR01" Then
                        Dim cond_rs() As DataRow = ConditionOut.Select("Cond_Type='ZPR0' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                        If cond_rs.Length > 0 Then
                            poutRec.LIST_PRICE = FormatNumber(cond_rs(0).Item("Cond_Value"), 2)
                        End If
                        'poutRec.UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
                    End If
                End If
                If Not RemoveAddedItem Or (RemoveAddedItem And Global_Inc.RemoveZeroString(PIn.Item("Itm_Number")) <> AddedItemLineNo) Then
                    ProductOut.Rows.Add(poutRec)
                End If

            Next
            For Each itm As String In phaseOutItems
                Dim pout As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
                pout.PART_NO = itm
                pout.LIST_PRICE = 0 : pout.RECYCLE_FEE = 0 : pout.UNIT_PRICE = 0
                ProductOut.AddProductOutRow(pout)
            Next
        Catch ex As Exception
            ErrorMessage += vbCrLf + "Exception Message of calling Bapi_Salesorder_Simulate:" + ex.ToString() : proxy1.Connection.Close() : Return False
        End Try
        proxy1.Connection.Close()
        If HasPhaseOutItem Then
            Return GetEUPrice(Org, SoldToId, ShipToId, strDistChann, strDivision, ProductIn, ProductOut, ErrorMessage)
        End If
        For Each pOutRow As SAPDALDS.ProductOutRow In ProductOut.Rows
            If IsNumeric(pOutRow.LIST_PRICE) AndAlso IsNumeric(pOutRow.UNIT_PRICE) AndAlso CDbl(pOutRow.LIST_PRICE) < CDbl(pOutRow.UNIT_PRICE) Then
                pOutRow.LIST_PRICE = pOutRow.UNIT_PRICE
            End If
            'for CN org if no List Price maintained in SAP , set both prices = 0
            If Org.ToUpper.StartsWith("CN") AndAlso isZPN0MT = False Then
                pOutRow.UNIT_PRICE = 0
                pOutRow.LIST_PRICE = pOutRow.UNIT_PRICE
            End If
        Next
        ProductOut.AcceptChanges()
        If String.IsNullOrEmpty(ErrorMessage) = False Then Return False
        Return True
    End Function

    Public Shared Function GetMultiPrice_eStore_PricingDate(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String,
                                         ByVal strDivision As String, ByVal OrderDocType As SAPOrderType, ByVal PricingDate As Date, ByVal ProductIn As SAPDALDS.ProductInDataTable,
                                         ByRef ProductOut As SAPDALDS.ProductOutDataTable,
                                         ByRef ErrorMessage As String) As Boolean
        'Util.SendEmail("nada.liu@advantech.com.cn", "myadvanteh@advantech.com", "test price", "AAAA", True, "", "")
        ErrorMessage = ""
        Dim HasPhaseOutItem As Boolean = False
        Dim phaseOutItems As New ArrayList, ZSWLItemSet As New DataTable
        With ZSWLItemSet.Columns
            .Add("PartNo") : .Add("Qty", GetType(Integer))
        End With
        Dim RemoveAddedItem As Boolean = False : Dim AddedItemLineNo As String = ""
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))
        If SoldToId = "SAID" Then proxy1.ConnectionString = ConfigurationManager.AppSettings("SAPConnTest")
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        With OrderHeader
            .Doc_Type = OrderDocType.ToString() : .Sales_Org = Trim(UCase(Org)) : .Distr_Chan = strDistChann : .Division = strDivision
            .Price_Date = PricingDate.ToString("yyyyMMdd")
            'If Org = "BR01" Then .Doc_Type = "ZORB"
        End With
        Dim LineNo As Integer = 1
        Dim sqlMA As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        sqlMA.Open()
        For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
            Dim chkSql As String =
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " +
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " +
                " where a.part_no='" + PInRow.PART_NO + "' and a.product_status in ('A','N','H','O','M1') and a.sales_org='" + Org + "' "
            Dim chkDt As New DataTable, sqlAptr As New SqlClient.SqlDataAdapter(chkSql, sqlMA)
            Try
                sqlAptr.Fill(chkDt)
            Catch ex As SqlClient.SqlException
                sqlMA.Close() : ErrorMessage = ex.ToString() : Return Nothing
            End Try
            If chkDt.Rows.Count > 0 AndAlso (Org <> "TW01" Or (Org = "TW01") And chkDt.Rows(0).Item("ProfitCenter") <> "N/A") Then
                If chkDt.Rows(0).Item("ITEM_CATEGORY_GROUP") <> "ZSWL" Then
                    Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                    item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(PInRow.PART_NO.ToUpper())
                    item.Req_Qty = PInRow.QTY.ToString()
                    item.Req_Qty = CInt(item.Req_Qty) * 1000
                    ItemsIn.Add(item)
                    LineNo += 1
                Else
                    Dim zr As DataRow = ZSWLItemSet.NewRow()
                    zr.Item("PartNo") = PInRow.PART_NO.ToUpper() : zr.Item("Qty") = PInRow.QTY : ZSWLItemSet.Rows.Add(zr)
                End If
            Else
                phaseOutItems.Add(PInRow.PART_NO.ToUpper())
            End If
        Next
        sqlMA.Close()

        If ItemsIn.Count = 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = GetAHighLevelItemForPricing(Org)
            item.Req_Qty = 1
            item.Req_Qty = CInt(item.Req_Qty) * 1000
            ItemsIn.Add(item)
            RemoveAddedItem = True : AddedItemLineNo = LineNo.ToString()
            LineNo += 1
        End If
        If ItemsIn.Count > 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            For Each r As DataRow In ZSWLItemSet.Rows
                Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(r.Item("PartNo").Trim().ToUpper())
                item.Req_Qty = r.Item("Qty").ToString()
                item.Req_Qty = CInt(item.Req_Qty) * 1000
                item.Hg_Lv_Item = "1"
                ItemsIn.Add(item)
                LineNo += 1
            Next
        End If
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
        Dim retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = SoldToId : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = ShipToId
        Partners.Add(SoldTo) : Partners.Add(ShipTo)
        proxy1.Connection.Open()
        Try

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
            Dim retAdoDt As DataTable = retDt.ToADODataTable()



            For Each retMsgRec As DataRow In retAdoDt.Rows
                If retMsgRec.Item("Type") = "E" Then
                    HasPhaseOutItem = True
                    ErrorMessage += String.Format("Type:{0};Msg:{1}", retMsgRec.Item("Type"), retMsgRec.Item("Message_V1")) + vbCrLf
                End If
            Next

            Dim ConditionOut As DataTable = Conditions.ToADODataTable()
            Dim PInDt As DataTable = ItemsIn.ToADODataTable()
            Dim POutDt As DataTable = ItemsOut.ToADODataTable()

            'gv2.DataSource = retAdoDt : gv2.DataBind()

            DTRET = retDt.ToADODataTable()

            ProductOut = New SAPDALDS.ProductOutDataTable
            For Each PIn As DataRow In PInDt.Rows
                'Dim pout As New ProductOut(RemoveZeroString(PIn.Item("Material")))
                Dim poutRec As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
                poutRec.PART_NO = Global_Inc.RemoveZeroString(PIn.Item("Material"))
                poutRec.LIST_PRICE = 0 : poutRec.RECYCLE_FEE = 0
                Dim rs2() As DataRow = ConditionOut.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
                For Each r As DataRow In rs2
                    Select Case r.Item("Cond_Type").ToString().ToUpper()
                        Case "ZPN0", "ZPR0"
                            poutRec.LIST_PRICE = FormatNumber(r.Item("Cond_Value"), 2)
                        Case "ZHB0"
                            poutRec.RECYCLE_FEE = FormatNumber(r.Item("Cond_Value"), 2)
                    End Select
                Next
                Dim POutRs() As DataRow = POutDt.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
                If Global_Inc.IsNumericItem(PIn.Item("Material")) Then
                    If poutRec.LIST_PRICE <= 0 AndAlso POutRs.Length > 0 Then
                        poutRec.LIST_PRICE = FormatNumber(POutRs(0).Item("net_value1") / POutRs(0).Item("req_qty"), 2)
                    End If
                End If
                If POutRs.Length > 0 Then
                    poutRec.TAX = FormatNumber(POutRs(0).Item("Tx_Doc_Cur") / POutRs(0).Item("req_qty"), 2)
                    poutRec.UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
                    If Org = "BR01" Then
                        Dim cond_rs() As DataRow = ConditionOut.Select("Cond_Type='ZPR0' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                        If cond_rs.Length > 0 Then
                            poutRec.LIST_PRICE = FormatNumber(cond_rs(0).Item("Cond_Value"), 2)
                        End If
                        'poutRec.UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
                    End If
                End If
                If Not RemoveAddedItem Or (RemoveAddedItem And Global_Inc.RemoveZeroString(PIn.Item("Itm_Number")) <> AddedItemLineNo) Then
                    ProductOut.Rows.Add(poutRec)
                End If

            Next
            For Each itm As String In phaseOutItems
                Dim pout As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
                pout.PART_NO = itm
                pout.LIST_PRICE = 0 : pout.RECYCLE_FEE = 0 : pout.UNIT_PRICE = 0
                ProductOut.AddProductOutRow(pout)
            Next
        Catch ex As Exception
            ErrorMessage += vbCrLf + "Exception Message of calling Bapi_Salesorder_Simulate:" + ex.ToString() : proxy1.Connection.Close() : Return False
        End Try
        proxy1.Connection.Close()
        If HasPhaseOutItem Then
            Return GetEUPrice(Org, SoldToId, ShipToId, strDistChann, strDivision, ProductIn, ProductOut, ErrorMessage)
        End If
        For Each pOutRow As SAPDALDS.ProductOutRow In ProductOut.Rows
            If IsNumeric(pOutRow.LIST_PRICE) AndAlso IsNumeric(pOutRow.UNIT_PRICE) AndAlso CDbl(pOutRow.LIST_PRICE) < CDbl(pOutRow.UNIT_PRICE) Then
                pOutRow.LIST_PRICE = pOutRow.UNIT_PRICE
            End If
        Next
        ProductOut.AcceptChanges()
        If String.IsNullOrEmpty(ErrorMessage) = False Then Return False
        Return True
    End Function

    Private Shared Function GetMinPriceListFromCache() As List(Of ProductMinPrice)
        Dim ListMinPriceCache As List(Of ProductMinPrice) = HttpContext.Current.Cache("ListMinPriceCache")
        If ListMinPriceCache Is Nothing Then
            ListMinPriceCache = New List(Of ProductMinPrice)
            HttpContext.Current.Cache.Add("ListMinPriceCache", ListMinPriceCache, Nothing, Now.AddHours(1),
                                          System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If

        Return ListMinPriceCache
    End Function

    ''' <summary>
    ''' Adding/Updating part's minimum price to/from cache container
    ''' </summary>
    ''' <param name="SalesOrg"></param>
    ''' <param name="PartNo"></param>
    ''' <param name="Currency"></param>
    ''' <param name="MinimumPrice"></param>
    ''' <remarks></remarks>
    Public Shared Sub SetMinPriceToCache(ByVal SalesOrg As String, ByVal PartNo As String, ByVal Currency As String, ByVal MinimumPrice As Double, ByVal _SalesTeam As MinimumPrice_SalesTeam)
        Dim ListMinPriceCache As List(Of ProductMinPrice) = GetMinPriceListFromCache()

        'Frank 20151002：Do not process minimum price if the price can not be extracted from SAP.
        Dim MinPrice1 As New ProductMinPrice(SalesOrg, PartNo, "", -1, False, _SalesTeam)
        If ListMinPriceCache.Contains(MinPrice1) Then
            Dim SalesOrgPartNoMinPriceResult As ProductMinPrice = ListMinPriceCache.Find(Function(obj) obj.SalesOrg = SalesOrg And obj.PartNo = PartNo)
            If MinimumPrice = -1 Then
                SalesOrgPartNoMinPriceResult.HasPriceFlag = False
            Else
                SalesOrgPartNoMinPriceResult.HasPriceFlag = True
            End If
            SalesOrgPartNoMinPriceResult.MinPrice = MinimumPrice
        Else
            If MinimumPrice = -1 Then
                MinPrice1.HasPriceFlag = False
            Else
                MinPrice1.HasPriceFlag = True
            End If
            MinPrice1.MinPrice = MinimumPrice
            MinPrice1.Currency = Currency
            ListMinPriceCache.Add(MinPrice1)
        End If
    End Sub

    Enum MinimumPrice_SalesTeam
        ATW_AOnline
        Intercon_iA
        Intercon_EC
        Intercon_iS
    End Enum
    Public Shared Function GetMinPrice(ByVal SalesOrg As String, ByVal PartNo As String, ByVal AccountCurrency As String, ByVal SalesTeam As MinimumPrice_SalesTeam, ByRef ErrorMessage As String, ByRef Currency As String) As Double
        Currency = "" : ErrorMessage = "" : PartNo = Trim(UCase(PartNo))
        'Dim ListMinPriceCache As List(Of ProductMinPrice) = HttpContext.Current.Cache("ListMinPriceCache")
        'If ListMinPriceCache Is Nothing Then
        '    ListMinPriceCache = New List(Of ProductMinPrice)
        '    HttpContext.Current.Cache.Add("ListMinPriceCache", ListMinPriceCache, Nothing, Now.AddHours(1), _
        '                                  System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        'End If
        Dim ListMinPriceCache As List(Of ProductMinPrice) = GetMinPriceListFromCache()
        If String.IsNullOrEmpty(AccountCurrency) Then
            AccountCurrency = "TWD"
        End If

        Dim MinPrice1 As New ProductMinPrice(SalesOrg, PartNo, AccountCurrency, -1, False, SalesTeam)
        If Not ListMinPriceCache.Contains(MinPrice1) Then

            Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))

            Try
                Dim ERPId As String = ""

                Select Case Left(SalesOrg, 2).ToUpper()
                    '20140116 TC: Should select ERPID from eQuotation.dbo.ESTORE_PRICING_ERPID instead
                    Case "TW"
                        Select Case AccountCurrency
                            Case "TWD"
                                ERPId = "2NC00001" : SalesOrg = "TW01"
                            Case "USD"
                                Select Case SalesTeam
                                    Case MinimumPrice_SalesTeam.ATW_AOnline
                                        ERPId = "EDER002" : SalesOrg = "TW01"
                                    Case MinimumPrice_SalesTeam.Intercon_iA
                                        ERPId = "AADE007" : SalesOrg = "TW01"
                                    Case MinimumPrice_SalesTeam.Intercon_EC
                                        ERPId = "AILA003" : SalesOrg = "TW01"
                                    Case MinimumPrice_SalesTeam.Intercon_iS
                                        ERPId = "AILA003" : SalesOrg = "TW01"
                                    Case Else
                                        ERPId = "EDER002" : SalesOrg = "TW01"
                                End Select

                        End Select

                End Select

                Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
                Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
                Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable

                With OrderHeader
                    .Doc_Type = "ZOR" : .Sales_Org = SalesOrg : .Distr_Chan = "10" : .Division = "00"
                End With

                'Frank 20150811為了避掉SAP會檢查有些料號必需在組裝系統下的情形，所以統一加了一個higher level料號來抓價格
                '上述的例子是如果是微軟作業系統料號，必需搭配機器來賣，所以SAP會檢查這類料號有沒有在某個系統之下
                Dim FakeItem As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                FakeItem.Itm_Number = "000001" : FakeItem.Material = GetAHighLevelItemForPricing(SalesOrg) : FakeItem.Req_Qty = 1 : ItemsIn.Add(FakeItem)

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
                Dim rs() As DataRow = condDt.Select("Cond_Type='ZMIP' and Itm_Number='000002'")
                If rs.Length > 0 Then
                    Currency = rs(0).Item("Currency")
                    MinPrice1.MinPrice = rs(0).Item("Cond_Value") : MinPrice1.Currency = Currency : MinPrice1.HasPriceFlag = True
                Else
                    ErrorMessage += "; Cannot find condition ZMIP"
                    MinPrice1.MinPrice = -1 : MinPrice1.Currency = "" : MinPrice1.HasPriceFlag = False
                End If
                ListMinPriceCache.Add(MinPrice1)
            Catch ex As Exception
                If Not IsNothing(proxy1) AndAlso Not IsNothing(proxy1.Connection) Then
                    proxy1.Connection.Close()
                End If
            End Try

        End If

        If ListMinPriceCache.Contains(MinPrice1) Then
            Dim SalesOrgPartNoMinPriceResult As ProductMinPrice = ListMinPriceCache.Find(Function(obj) obj.SalesOrg = SalesOrg And obj.PartNo = PartNo And obj.Currency = AccountCurrency And obj.SalesTeam = SalesTeam)
            If SalesOrgPartNoMinPriceResult IsNot Nothing Then
                Currency = SalesOrgPartNoMinPriceResult.Currency
                If SalesOrgPartNoMinPriceResult.HasPriceFlag Then
                    Return SalesOrgPartNoMinPriceResult.MinPrice
                End If
            End If
        End If
        Return -1
    End Function


    Public Class ProductMinPrice
        Implements IEquatable(Of ProductMinPrice)

        Public Property SalesOrg As String : Public Property PartNo As String : Public Property Currency As String
        Public Property MinPrice As Double : Public Property HasPriceFlag As Boolean : Public Property SalesTeam As MinimumPrice_SalesTeam
        Public Sub New(SalesOrg As String, PartNo As String, Currency As String, MinPrice As String, HasPriceFlag As Boolean, _SalesTeam As MinimumPrice_SalesTeam)
            Me.SalesOrg = SalesOrg : Me.PartNo = PartNo : Me.Currency = Currency : Me.MinPrice = MinPrice : Me.HasPriceFlag = HasPriceFlag
            Me.SalesTeam = _SalesTeam
        End Sub

        Public Function Equals1(other As ProductMinPrice) As Boolean Implements System.IEquatable(Of ProductMinPrice).Equals
            If Me.SalesOrg = other.SalesOrg And Me.PartNo = other.PartNo And Me.Currency = other.Currency And Me.SalesTeam = other.SalesTeam Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class

    Public Function OrderSimulate(ByRef ErrMsg As String, ByVal SoldTo As String, ByVal ShipTo As String, ByVal Org As String,
            ByVal DISChannel As String, ByVal Division As String, ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef outDT As SAPDALDS.SimulateOutDataTable) As Boolean
        If ProductIn.Rows.Count = 0 Then
            Return False
        End If

        Dim PartNoStr As String = ""
        For Each r As SAPDALDS.ProductInRow In ProductIn
            PartNoStr = PartNoStr + r.PART_NO + "|"
        Next

        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))
        If SoldTo = "SAID" Then proxy1.ConnectionString = ConfigurationManager.AppSettings("SAPConnTest")

        Dim S_OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD
        Dim S_OrderLineDt As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable
        Dim S_ConditionDT As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        Dim S_PartnerDT As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim S_CreditCardDT As New BAPI_SALESORDER_SIMULATE.BAPICCARDTable
        Dim S_ScheLineDT As New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable

        Dim O_ScheLineDT As New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable
        Dim O_OrderLineDt As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable

        S_OrderHeader.Doc_Type = "ZOR2" : S_OrderHeader.Sales_Org = Org : S_OrderHeader.Distr_Chan = DISChannel
        S_OrderHeader.Division = Division
        S_OrderHeader.Req_Date_H = Now.ToString("yyyy/MM/dd")


        Dim chkSql As String =
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " +
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " +
                " where a.part_no in ('" + PartNoStr.Trim.Trim("|").Replace("|", "','") + "') and a.product_status in ('A','N','H','O') and a.sales_org='" + Org + "' "
        Dim dt As New DataTable
        Dim sqlMA As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        Dim chkDt As New DataTable, sqlAptr As New SqlClient.SqlDataAdapter(chkSql, sqlMA)
        Try
            sqlAptr.Fill(chkDt)
        Catch ex As SqlClient.SqlException
            sqlMA.Close() : ErrMsg = ex.ToString() : Return Nothing
        End Try

        'Dim part_noArr() As String = PartNoStr.Trim().Trim("|").Split("|")

        Dim lineNo As Integer = 0
        For Each p As SAPDALDS.ProductInRow In ProductIn
            lineNo = lineNo + 1
            Dim S_OrderLineRow As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
            Dim S_ScheLineRow As New BAPI_SALESORDER_SIMULATE.BAPISCHDL
            Dim S_ConditionRow As New BAPI_SALESORDER_SIMULATE.BAPICOND

            Dim F As Integer = 0
            For Each R As DataRow In chkDt.Rows
                If p.PART_NO.ToUpper = R.Item("part_no") And Org <> "TW01" Or (Org = "TW01" And R.Item("ProfitCenter") <> "N/A") Then
                    If R.Item("ITEM_CATEGORY_GROUP") <> "ZSWL" Then
                        F = 0
                    Else
                        F = 1
                    End If
                    Exit For
                Else
                    F = 2
                End If
            Next
            If F = 1 Then
                S_OrderLineRow.Hg_Lv_Item = "1"
            End If
            If F <> 2 Then
                S_OrderLineRow.Itm_Number = FormatItmNumber(lineNo)
                S_OrderLineRow.Material = Global_Inc.Format2SAPItem(p.PART_NO.ToUpper())
                S_OrderLineRow.Req_Qty = 9999
                'S_OrderLineRow.Plant = "EUH1"
                S_ScheLineRow.Itm_Number = FormatItmNumber(lineNo)
                S_ScheLineRow.Req_Qty = 9999
                'S_ScheLineRow.Req_Date = Now.ToString("yyyy/MM/dd")
                S_ConditionRow.Itm_Number = FormatItmNumber(lineNo)
                S_ConditionRow.Cond_Type = "ZPN0"
                S_ConditionRow.Cond_Value = 0

                S_OrderLineDt.Add(S_OrderLineRow)
                S_ScheLineDT.Add(S_ScheLineRow)
                S_ConditionDT.Add(S_ConditionRow)
            End If

            Dim poutRec As SAPDALDS.SimulateOutRow = outDT.NewSimulateOutRow()
            poutRec.PartNo = p.PART_NO
            poutRec.ListPrice = 0
            poutRec.UnitPrice = 0
            poutRec.TAX = 0
            poutRec.RECYCLE_FEE = 0
            poutRec.ATPNOW = 0
            poutRec.DueDate = "1900-01-01"
            poutRec.ATPQtyTotal = 0
            outDT.Rows.Add(poutRec)
        Next



        Dim S_PartnerFuncRow As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
        Dim S_PartnerFuncRow1 As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
        S_PartnerFuncRow.Partn_Role = "AG" : S_PartnerFuncRow.Partn_Numb = SoldTo
        S_PartnerFuncRow1.Partn_Role = "WE" : S_PartnerFuncRow1.Partn_Numb = SoldTo
        S_PartnerDT.Add(S_PartnerFuncRow) : S_PartnerDT.Add(S_PartnerFuncRow1)


        Dim retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table


        proxy1.Bapi_Salesorder_Simulate("", S_OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "",
                                                 New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO,
                                                 New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt,
                                                 S_CreditCardDT, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable,
                                                 New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable,
                                                 New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable,
                                                 New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, S_ConditionDT, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable,
                                                 S_OrderLineDt, O_OrderLineDt, S_PartnerDT, O_ScheLineDT,
                                                 S_ScheLineDT, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)

        proxy1.Connection.Close()
        Dim retAdoDt As DataTable = retDt.ToADODataTable()
        Dim condDT As New DataTable
        Dim scheDT As New DataTable
        Dim LineDT As New DataTable
        scheDT = O_ScheLineDT.ToADODataTable
        LineDT = O_OrderLineDt.ToADODataTable
        condDT = S_ConditionDT.ToADODataTable

        Dim isSuccess As Integer = 0
        For Each retMsgRec As DataRow In retAdoDt.Rows
            If retMsgRec.Item("Type") = "E" Then
                ErrMsg += String.Format("Type:{0};Msg:{1}", retMsgRec.Item("Type"), retMsgRec.Item("Message_V1")) + vbCrLf
                isSuccess = 1
            End If
        Next

        If isSuccess = 1 Then
            Return False
        Else
            For Each pIn As DataRow In S_OrderLineDt.ToADODataTable.Rows

                Dim ListP As String = "0"
                Dim UnitP As String = "0"
                Dim FEE As String = "0"
                Dim TAX As String = "0"
                Dim due As String = "1900-01-01"
                Dim rs2() As DataRow = condDT.Select("Itm_Number='" + pIn.Item("Itm_Number") + "'")
                For Each r As DataRow In rs2

                    Select Case r.Item("Cond_Type").ToString().ToUpper()
                        Case "ZPN0", "ZPR0"
                            ListP = FormatNumber(r.Item("Cond_Value"), 2)
                        Case "ZHB0"
                            FEE = FormatNumber(r.Item("Cond_Value"), 2)
                    End Select
                Next

                Dim POutRs() As DataRow = LineDT.Select("Itm_Number='" + pIn.Item("Itm_Number") + "'")
                If Global_Inc.IsNumericItem(pIn.Item("Material")) Then
                    For Each rout As SAPDALDS.SimulateOutRow In outDT.Select("PartNo='" + pIn.Item("Material") + "'")
                        ListP = FormatNumber(POutRs(0).Item("net_value1") / POutRs(0).Item("req_qty"), 2)
                    Next
                End If
                If POutRs.Length > 0 Then
                    TAX = FormatNumber(POutRs(0).Item("Tx_Doc_Cur") / POutRs(0).Item("req_qty"), 2)
                    UnitP = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
                    If Org = "BR01" Then
                        Dim cond_rs() As DataRow = condDT.Select("Cond_Type='ZPR0' AND Itm_Number='" + pIn.Item("Itm_Number") + "'")
                        If cond_rs.Length > 0 Then
                            ListP = FormatNumber(cond_rs(0).Item("Cond_Value"), 2)
                        End If
                    End If
                End If

                Dim ATPNOW As Integer = 0
                Dim ATPTOTAL As Integer = 0
                Dim RSATP() As DataRow = scheDT.Select("Itm_Number='" + pIn.Item("Itm_Number") + "'")
                Dim executControllor As Integer = 0

                Dim DueDateQty As Integer = 0
                For Each rin As SAPDALDS.ProductInRow In ProductIn
                    If Global_Inc.Format2SAPItem(rin.PART_NO.ToUpper) = pIn.Item("Itm_Number").ToString.ToUpper Then
                        DueDateQty = DueDateQty + rin.QTY
                    End If
                Next
                For Each r As DataRow In RSATP
                    If r.Item("Req_Date").ToString.Trim = Now.Date.ToString("yyyyMMdd") Then
                        ATPNOW = ATPNOW + r.Item("Confir_Qty").ToString.Trim
                    End If
                    ATPTOTAL = ATPTOTAL + r.Item("Confir_Qty").ToString.Trim
                    If ATPTOTAL >= DueDateQty And executControllor = 0 Then
                        due = Global_Inc.DateFormat(r.Item("Req_Date").ToString, "YYYYMMDD", "YYYYMMDD", "", "-")
                        executControllor = 1
                    End If
                Next
                If due = "1900-01-01" Then
                    due = Now.Date.AddDays(getLeadTime(pIn.Item("Material"), pIn.Item("Plant")))
                End If
                For Each rout As SAPDALDS.SimulateOutRow In outDT.Select("PartNo='" + pIn.Item("Material") + "'")
                    rout.ListPrice = ListP
                    rout.UnitPrice = UnitP
                    rout.RECYCLE_FEE = FEE
                    rout.TAX = TAX
                    rout.ATPNOW = ATPNOW
                    rout.ATPQtyTotal = ATPTOTAL
                    rout.DueDate = due
                Next
            Next
            outDT.AcceptChanges()
            Return True
        End If
    End Function
    Public Shared Function getLeadTime(ByVal pn As String, ByVal plant As String) As Integer
        Dim N As Integer = 0
        Dim str As String = String.Format("select (PLANNED_DEL_TIME + GP_PROCESSING_TIME) from dbo.SAP_PRODUCT_ABC where PART_NO='{0}' AND PLANT='{1}'", pn, plant)
        Dim dt As New DataTable
        dt = dbUtil.dbGetDataTable("MY", str)
        If dt.Rows.Count > 0 Then
            N = dt.Rows(0).Item(0)
        End If
        Return N
    End Function
    'Private Shared Function GetMultiPrice_eStore(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal Currency As String, _
    '                                      ByVal strDistChann As String, ByVal strDivision As String, ByVal ProductIn As SAPDALDS.ProductInDataTable, _
    '                                      ByRef ProductOut As SAPDALDS.ProductOutDataTable, ByRef ErrorMessage As String) As Boolean
    '    'Util.SendEmail("nada.liu@advantech.com.cn", "myadvanteh@advantech.com", "test price", "AAAA", True, "", "")
    '    SoldToId = SoldToId.ToUpper.Trim : ShipToId = ShipToId.ToUpper.Trim

    '    ErrorMessage = ""
    '    Dim HasPhaseOutItem As Boolean = False
    '    Dim phaseOutItems As New ArrayList, ZSWLItemSet As New DataTable
    '    With ZSWLItemSet.Columns
    '        .Add("PartNo") : .Add("Qty", GetType(Integer))
    '    End With
    '    Dim RemoveAddedItem As Boolean = False : Dim AddedItemLineNo As String = ""
    '    Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))
    '    If SoldToId = "SAID" Then proxy1.ConnectionString = ConfigurationManager.AppSettings("SAPConnTest")
    '    Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
    '    Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
    '    Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
    '    With OrderHeader
    '        .Doc_Type = "ZOR" : .Sales_Org = Trim(UCase(Org)) : .Distr_Chan = strDistChann : .Division = strDivision
    '        If Org = "BR01" Then .Doc_Type = "ZORB"
    '        If Not String.IsNullOrEmpty(Currency.Trim) Then .Currency = Currency
    '    End With
    '    Dim LineNo As Integer = 1
    '    Dim sqlMA As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
    '    sqlMA.Open()
    '    'Ming 20141110 优化减少捞数据次数
    '    Dim dtPNPC As DataTable = HttpContext.Current.Cache("dtPNPC")
    '    If dtPNPC Is Nothing Then
    '        dtPNPC = New DataTable()
    '        'HttpContext.Current.Cache.Add("dtPNPC", dtPNPC, Nothing, Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
    '        Dim sql As New Text.StringBuilder()
    '        sql.AppendFormat("  select a.part_no AS PartNo,a.sales_org as ORG, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter ")
    '        sql.AppendFormat(" from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT ")
    '        sql.AppendFormat(" where a.product_status in ('A','N','H','O','S5','V','M1') ")
    '        sql.AppendFormat(" group by a.part_no,a.sales_org ,a.ITEM_CATEGORY_GROUP , b.ProfitCenter  ")
    '        Dim Aptr As New SqlClient.SqlDataAdapter(sql.ToString(), sqlMA)
    '        Try
    '            Aptr.Fill(dtPNPC)
    '            dtPNPC.PrimaryKey = New DataColumn() {dtPNPC.Columns("PartNo"), dtPNPC.Columns("ORG")}

    '            HttpContext.Current.Cache.Add("dtPNPC", dtPNPC, Nothing, Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
    '        Catch ex As SqlClient.SqlException
    '            sqlMA.Close() : ErrorMessage = ex.ToString() : Return Nothing
    '        End Try
    '    End If
    '    'End
    '    For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
    '        'Dim chkSql As String = _
    '        '    " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + _
    '        '    " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + _
    '        '    " where a.part_no='" + Global_Inc.RemovePrecedingZeros(PInRow.PART_NO) + "' and a.product_status in ('A','N','H','O','S5','V','M1','') and a.sales_org='" + Org + "' "
    '        'Dim chkDt As New DataTable, sqlAptr As New SqlClient.SqlDataAdapter(chkSql, sqlMA)
    '        'Try
    '        '    sqlAptr.Fill(chkDt)
    '        'Catch ex As SqlClient.SqlException
    '        '    sqlMA.Close() : ErrorMessage = ex.ToString() : Return Nothing
    '        'End Try
    '        'Dim dr As DataRow() = dtPNPC.Select(String.Format("ORG='{0}' and PartNo='{1}'", Org, PInRow.PART_NO))
    '        Dim dr As DataRow = dtPNPC.Rows.Find(New Object() {PInRow.PART_NO, Org})
    '        'If dr.Length > 0 AndAlso (Org <> "TW01" Or (Org = "TW01") And dr(0).Item("ProfitCenter") <> "N/A") Then
    '        If dr IsNot Nothing AndAlso (Org <> "TW01" Or (Org = "TW01") And dr.Item("ProfitCenter") <> "N/A") Then
    '            'If dr(0).Item("ITEM_CATEGORY_GROUP") <> "ZSWL" Then
    '            If dr.Item("ITEM_CATEGORY_GROUP") <> "ZSWL" Then
    '                Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
    '                item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(PInRow.PART_NO.ToUpper())
    '                item.Req_Qty = PInRow.QTY.ToString()
    '                item.Req_Qty = CInt(item.Req_Qty) * 1000
    '                ItemsIn.Add(item)
    '                LineNo += 1
    '            Else
    '                Dim zr As DataRow = ZSWLItemSet.NewRow()
    '                zr.Item("PartNo") = PInRow.PART_NO.ToUpper() : zr.Item("Qty") = PInRow.QTY : ZSWLItemSet.Rows.Add(zr)
    '            End If
    '        Else
    '            phaseOutItems.Add(PInRow.PART_NO.ToUpper())
    '        End If
    '    Next
    '    sqlMA.Close()

    '    If ItemsIn.Count = 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
    '        Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
    '        item.Itm_Number = FormatItmNumber(LineNo) : item.Material = GetAHighLevelItemForPricing(Org)
    '        item.Req_Qty = 1
    '        item.Req_Qty = CInt(item.Req_Qty) * 1000
    '        ItemsIn.Add(item)
    '        RemoveAddedItem = True : AddedItemLineNo = LineNo.ToString()
    '        LineNo += 1
    '    End If
    '    If ItemsIn.Count > 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
    '        For Each r As DataRow In ZSWLItemSet.Rows
    '            Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
    '            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(r.Item("PartNo").Trim().ToUpper())
    '            item.Req_Qty = r.Item("Qty").ToString()
    '            item.Req_Qty = CInt(item.Req_Qty) * 1000
    '            item.Hg_Lv_Item = "1"
    '            ItemsIn.Add(item)
    '            LineNo += 1
    '        Next
    '    End If
    '    Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
    '    Dim retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
    '    SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = SoldToId : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = ShipToId
    '    Partners.Add(SoldTo) : Partners.Add(ShipTo)
    '    proxy1.Connection.Open()
    '    Try

    '        Dim dtItem As New DataTable, dtPartNr As New DataTable, dtcon As New DataTable, DTRET As New DataTable

    '        dtItem = ItemsIn.ToADODataTable() : dtPartNr = Partners.ToADODataTable() : dtcon = Conditions.ToADODataTable()

    '        proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "", _
    '                                        New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, _
    '                                        New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt, _
    '                                        New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, _
    '                                        New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
    '                                        New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, _
    '                                        New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, _
    '                                        ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable, _
    '                                        New BAPI_SALESORDER_SIMULATE.BAPISCHDLTable, New BAPI_SALESORDER_SIMULATE.BAPIADDR1Table)
    '        Dim retAdoDt As DataTable = retDt.ToADODataTable()



    '        For Each retMsgRec As DataRow In retAdoDt.Rows
    '            If retMsgRec.Item("Type") = "E" Then
    '                HasPhaseOutItem = True
    '                ErrorMessage += String.Format("{0}", retMsgRec.Item("Message")) + vbCrLf
    '            End If
    '        Next

    '        Dim ConditionOut As DataTable = Conditions.ToADODataTable()
    '        Dim PInDt As DataTable = ItemsIn.ToADODataTable()
    '        Dim POutDt As DataTable = ItemsOut.ToADODataTable()

    '        'gv2.DataSource = retAdoDt : gv2.DataBind()

    '        DTRET = retDt.ToADODataTable()

    '        ProductOut = New SAPDALDS.ProductOutDataTable
    '        For Each PIn As DataRow In PInDt.Rows
    '            'Dim pout As New ProductOut(RemoveZeroString(PIn.Item("Material")))
    '            Dim poutRec As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
    '            poutRec.PART_NO = Global_Inc.RemoveZeroString(PIn.Item("Material"))
    '            poutRec.LIST_PRICE = 0 : poutRec.RECYCLE_FEE = 0
    '            Dim rs2() As DataRow = ConditionOut.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
    '            For Each r As DataRow In rs2
    '                Select Case r.Item("Cond_Type").ToString().ToUpper()
    '                    'Case "ZPN0", "ZPR0"
    '                    '    poutRec.LIST_PRICE = FormatNumber(r.Item("Cond_Value"), 2)
    '                    Case "ZPN0", "ZPR0"
    '                        If Double.TryParse(poutRec.LIST_PRICE, 0) AndAlso Double.TryParse(r.Item("Cond_Value"), 0) Then
    '                            If r.Item("Cond_Value") > poutRec.LIST_PRICE Then
    '                                poutRec.LIST_PRICE = FormatNumber(r.Item("Cond_Value"), 2)
    '                            End If
    '                        End If
    '                    Case "ZHB0"
    '                        poutRec.RECYCLE_FEE = FormatNumber(r.Item("Cond_Value"), 2)
    '                End Select
    '            Next
    '            Dim POutRs() As DataRow = POutDt.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
    '            If Global_Inc.IsNumericItem(PIn.Item("Material")) Then
    '                If poutRec.LIST_PRICE <= 0 AndAlso POutRs.Length > 0 Then
    '                    poutRec.LIST_PRICE = FormatNumber(POutRs(0).Item("net_value1") / POutRs(0).Item("req_qty"), 2)
    '                End If
    '            End If
    '            If POutRs.Length > 0 Then
    '                poutRec.TAX = FormatNumber(POutRs(0).Item("Tx_Doc_Cur") / POutRs(0).Item("req_qty"), 2)
    '                poutRec.UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
    '                If Org = "BR01" Then
    '                    Dim cond_rs() As DataRow = ConditionOut.Select("Cond_Type='ZPR0' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
    '                    If cond_rs.Length > 0 Then
    '                        poutRec.LIST_PRICE = FormatNumber(cond_rs(0).Item("Cond_Value"), 2)
    '                    End If
    '                    'poutRec.UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
    '                End If
    '            End If
    '            If Not RemoveAddedItem Or (RemoveAddedItem And Global_Inc.RemoveZeroString(PIn.Item("Itm_Number")) <> AddedItemLineNo) Then
    '                ProductOut.Rows.Add(poutRec)
    '            End If

    '        Next
    '        For Each itm As String In phaseOutItems
    '            Dim pout As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
    '            pout.PART_NO = itm
    '            pout.LIST_PRICE = 0 : pout.RECYCLE_FEE = 0 : pout.UNIT_PRICE = 0
    '            ProductOut.AddProductOutRow(pout)
    '        Next
    '    Catch ex As Exception
    '        ErrorMessage += vbCrLf + "Exception Message of calling Bapi_Salesorder_Simulate:" + ex.ToString() : proxy1.Connection.Close() : Return False
    '    End Try
    '    proxy1.Connection.Close()
    '    If HasPhaseOutItem Then
    '        Return GetEUPrice(Org, SoldToId, ShipToId, strDistChann, strDivision, ProductIn, ProductOut, ErrorMessage)
    '    End If
    '    For Each pOutRow As SAPDALDS.ProductOutRow In ProductOut.Rows
    '        If IsNumeric(pOutRow.LIST_PRICE) AndAlso IsNumeric(pOutRow.UNIT_PRICE) AndAlso CDbl(pOutRow.LIST_PRICE) < CDbl(pOutRow.UNIT_PRICE) Then
    '            pOutRow.LIST_PRICE = pOutRow.UNIT_PRICE
    '        End If
    '    Next
    '    ProductOut.AcceptChanges()
    '    If String.IsNullOrEmpty(ErrorMessage) = False Then Return False
    '    Return True
    'End Function
    Private Shared Function GetMultiPrice_eStore(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal Currency As String,
                                           ByVal strDistChann As String, ByVal strDivision As String, ByVal strOrderType As String, ByVal ProductIn As SAPDALDS.ProductInDataTable,
                                          ByRef ProductOut As SAPDALDS.ProductOutDataTable, ByRef ErrorMessage As String) As Boolean
        'Util.SendEmail("nada.liu@advantech.com.cn", "myadvanteh@advantech.com", "test price", "AAAA", True, "", "")
        SoldToId = SoldToId.ToUpper.Trim : ShipToId = ShipToId.ToUpper.Trim

        ErrorMessage = ""
        Dim HasPhaseOutItem As Boolean = False
        Dim phaseOutItems As New ArrayList, ZSWLItemSet As New DataTable
        With ZSWLItemSet.Columns
            .Add("PartNo") : .Add("Qty", GetType(Integer))
        End With
        Dim RemoveAddedItem As Boolean = False : Dim AddedItemLineNo As String = ""
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))
        If SoldToId = "SAID" Then proxy1.ConnectionString = ConfigurationManager.AppSettings("SAPConnTest")
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable

        Dim _epricer_exch_rate_EURtoUSD As Decimal = 1
        If SoldToId.Equals("AAEA010", StringComparison.InvariantCultureIgnoreCase) Then
            Currency = "USD"
            Dim _Year As Integer = Date.Now.Year
            Dim _Quarter As Integer = Math.Ceiling(DateTime.Today.Month / 3)

            Dim _exch_rate_sql As String = String.Empty
            _exch_rate_sql &= " SELECT [Year],[Quarter],[Currency_Origin],[Currency_Target],[ExchangeRate],[Modified_User],[Modified_Time] "
            _exch_rate_sql &= " FROM ExchangeRate_Pricing "
            _exch_rate_sql &= " WHERE Year = " & _Year & " AND Quarter = " & _Quarter & " AND Currency_Origin = 'EUR' AND Currency_Target = 'USD' "
            _exch_rate_sql &= " Order by Year Desc,Quarter Desc "
            Dim _exch_rate_dt As DataTable = Nothing
            Try
                _exch_rate_dt = dbUtil.dbGetDataTable("ACLSQL7", _exch_rate_sql)
            Catch ex As Exception
            End Try
            If _exch_rate_dt IsNot Nothing AndAlso _exch_rate_dt.Rows.Count > 0 Then
                _epricer_exch_rate_EURtoUSD = _exch_rate_dt.Rows(0).Item("ExchangeRate")
            End If
        End If

        With OrderHeader
            .Doc_Type = "ZOR" : .Sales_Org = Trim(UCase(Org)) : .Distr_Chan = strDistChann : .Division = strDivision
            If Org = "BR01" Then
                If String.IsNullOrEmpty(strOrderType) Then
                    .Doc_Type = "ZORB"
                Else
                    .Doc_Type = strOrderType
                End If

            End If

            If Not String.IsNullOrEmpty(Currency.Trim) Then .Currency = Currency
        End With
        Dim LineNo As Integer = 1
        Dim sqlMA As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        sqlMA.Open()
        For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
            Dim chkSql As String =
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " +
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " +
                " where a.part_no='" + Global_Inc.RemovePrecedingZeros(PInRow.PART_NO) + "' and a.product_status in ('A','N','H','O','M1','C','P','S2','S5','T','V','') and a.sales_org='" + Org + "' "
            Dim chkDt As New DataTable, sqlAptr As New SqlClient.SqlDataAdapter(chkSql, sqlMA)
            Try
                sqlAptr.Fill(chkDt)
            Catch ex As SqlClient.SqlException
                sqlMA.Close() : ErrorMessage = ex.ToString() : Return Nothing
            End Try
            If chkDt.Rows.Count > 0 AndAlso (Org <> "TW01" Or (Org = "TW01") And chkDt.Rows(0).Item("ProfitCenter") <> "N/A") Then
                If chkDt.Rows(0).Item("ITEM_CATEGORY_GROUP") <> "ZSWL" Then
                    Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                    item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(PInRow.PART_NO.ToUpper())
                    item.Req_Qty = PInRow.QTY.ToString()
                    item.Req_Qty = CInt(item.Req_Qty) * 1000

                    'Ryan 20180718 Temporary add for TW20 per Tina's suggestion
                    If Org = "TW20" Then
                        item.Plant = "ASH1"
                    End If

                    ItemsIn.Add(item)
                    LineNo += 1
                Else
                    Dim zr As DataRow = ZSWLItemSet.NewRow()
                    zr.Item("PartNo") = PInRow.PART_NO.ToUpper() : zr.Item("Qty") = PInRow.QTY : ZSWLItemSet.Rows.Add(zr)
                End If
            Else
                phaseOutItems.Add(PInRow.PART_NO.ToUpper())
            End If
        Next
        sqlMA.Close()

        If ItemsIn.Count = 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = GetAHighLevelItemForPricing(Org)
            item.Req_Qty = 1
            item.Req_Qty = CInt(item.Req_Qty) * 1000
            ItemsIn.Add(item)
            RemoveAddedItem = True : AddedItemLineNo = LineNo.ToString()
            LineNo += 1
        End If
        If ItemsIn.Count > 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            For Each r As DataRow In ZSWLItemSet.Rows
                Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(r.Item("PartNo").Trim().ToUpper())
                item.Req_Qty = r.Item("Qty").ToString()
                item.Req_Qty = CInt(item.Req_Qty) * 1000
                item.Hg_Lv_Item = "1"
                ItemsIn.Add(item)
                LineNo += 1
            Next
        End If
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
        Dim retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = SoldToId : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = ShipToId
        Partners.Add(SoldTo) : Partners.Add(ShipTo)
        proxy1.Connection.Open()
        Try

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
            Dim retAdoDt As DataTable = retDt.ToADODataTable()



            For Each retMsgRec As DataRow In retAdoDt.Rows
                If retMsgRec.Item("Type") = "E" Then
                    HasPhaseOutItem = True
                    ErrorMessage += String.Format("{0}", retMsgRec.Item("Message")) + vbCrLf
                End If
            Next

            Dim ConditionOut As DataTable = Conditions.ToADODataTable()
            Dim PInDt As DataTable = ItemsIn.ToADODataTable()
            Dim POutDt As DataTable = ItemsOut.ToADODataTable()

            'gv2.DataSource = retAdoDt : gv2.DataBind()

            'ICMS Amount
            Dim _BX13 As Decimal = 0
            'IPI Amount
            Dim _BX23 As Decimal = 0
            'ICMS ST(Sub Trib Amount)
            Dim _BX41 As Decimal = 0

            DTRET = retDt.ToADODataTable()

            ProductOut = New SAPDALDS.ProductOutDataTable
            For Each PIn As DataRow In PInDt.Rows
                'Dim pout As New ProductOut(RemoveZeroString(PIn.Item("Material")))
                Dim poutRec As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
                poutRec.PART_NO = Global_Inc.RemoveZeroString(PIn.Item("Material"))
                poutRec.LIST_PRICE = 0 : poutRec.RECYCLE_FEE = 0
                poutRec.Condition_ZMIP = 0 : poutRec.Condition_VPRS = 0
                Dim rs2() As DataRow = ConditionOut.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
                For Each r As DataRow In rs2
                    Select Case r.Item("Cond_Type").ToString().ToUpper()
                        'Case "ZPN0", "ZPR0"
                        '    poutRec.LIST_PRICE = FormatNumber(r.Item("Cond_Value"), 2)
                        Case "ZPN0", "ZPR0"
                            If Double.TryParse(poutRec.LIST_PRICE, 0) AndAlso Double.TryParse(r.Item("Cond_Value"), 0) Then
                                If r.Item("Cond_Value") > poutRec.LIST_PRICE Then
                                    poutRec.LIST_PRICE = FormatNumber(r.Item("Cond_Value"), 2)
                                End If
                            End If
                        Case "BX13"
                            Decimal.TryParse(r.Item("CondValue").ToString(), _BX13)
                        Case "BX23"
                            Decimal.TryParse(r.Item("CondValue").ToString(), _BX23)
                        Case "BX41"
                            Decimal.TryParse(r.Item("CondValue").ToString(), _BX41)
                        Case "ZHB0"
                            poutRec.RECYCLE_FEE = FormatNumber(r.Item("Cond_Value"), 2)
                        Case "ZMIP"
                            poutRec.Condition_ZMIP = FormatNumber(r.Item("Cond_Value"), 2)
                        Case "VPRS"
                            'Ryan 20170619 If request currency is not equal to ConditionTable currency, then do exchange
                            If Not r.Item("Currency").Equals(Currency, StringComparison.OrdinalIgnoreCase) Then
                                poutRec.Condition_VPRS = FormatNumber(r.Item("Cond_Value") * CType(get_exchangerate(r.Item("Currency"), Currency).ToString, Decimal), 2)
                            Else
                                poutRec.Condition_VPRS = FormatNumber(r.Item("Cond_Value"), 2)
                            End If

                            'Ryan 20170908 VPRS should be divided by condition per unit
                            Dim ConditionPerUnit As Integer = 1
                            If Integer.TryParse(r.Item("COND_P_UNT").ToString(), ConditionPerUnit) AndAlso ConditionPerUnit <> 0 Then
                                poutRec.Condition_VPRS = poutRec.Condition_VPRS / ConditionPerUnit
                            End If

                    End Select
                Next
                Dim POutRs() As DataRow = POutDt.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
                If Global_Inc.IsNumericItem(PIn.Item("Material")) Then
                    If poutRec.LIST_PRICE <= 0 AndAlso POutRs.Length > 0 Then
                        poutRec.LIST_PRICE = FormatNumber(POutRs(0).Item("net_value1") / POutRs(0).Item("req_qty"), 2)
                    End If
                End If
                If POutRs.Length > 0 Then
                    poutRec.TAX = FormatNumber(POutRs(0).Item("Tx_Doc_Cur") / POutRs(0).Item("req_qty"), 2)
                    poutRec.UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)

                    'Frank 20170323
                    'CN's sales and customer only check the price which includes tax
                    If Org.StartsWith("CN", StringComparison.InvariantCultureIgnoreCase) Then
                        poutRec.UNIT_PRICE = poutRec.UNIT_PRICE * (1 + ConfigurationManager.AppSettings("ACNTaxRate"))
                        poutRec.LIST_PRICE = poutRec.LIST_PRICE * (1 + ConfigurationManager.AppSettings("ACNTaxRate"))
                    End If

                    'Frank 20170328
                    'Account AAEA010 need to make transation with Advantech in EUR currency,
                    'but it's a Intercon's account and the currency only can be setup as USD in SAP.
                    'So we need to transfer the price from USD to EUR
                    If SoldToId.Equals("AAEA010", StringComparison.InvariantCultureIgnoreCase) Then
                        poutRec.UNIT_PRICE = poutRec.UNIT_PRICE / _epricer_exch_rate_EURtoUSD
                        poutRec.LIST_PRICE = poutRec.LIST_PRICE / _epricer_exch_rate_EURtoUSD
                    End If

                    If Org = "BR01" Then
                        'Dim cond_rs() As DataRow = ConditionOut.Select("Cond_Type='ZPR0' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                        'If cond_rs.Length > 0 Then
                        '    poutRec.LIST_PRICE = FormatNumber(cond_rs(0).Item("Cond_Value"), 2)
                        'End If

                        poutRec.LIST_PRICE = poutRec.UNIT_PRICE
                        Select Case strOrderType
                            Case "ZQTI"
                                poutRec.LIST_PRICE = poutRec.LIST_PRICE + _BX13 + _BX23
                            Case "ZQTC"
                                poutRec.LIST_PRICE = poutRec.LIST_PRICE + _BX13 + _BX23 + _BX41
                            Case "ZQTR"
                                poutRec.LIST_PRICE = poutRec.LIST_PRICE + _BX13 + _BX23 + _BX41
                        End Select
                        poutRec.UNIT_PRICE = poutRec.LIST_PRICE

                        'poutRec.UNIT_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
                    End If
                End If
                If Not RemoveAddedItem Or (RemoveAddedItem And Global_Inc.RemoveZeroString(PIn.Item("Itm_Number")) <> AddedItemLineNo) Then
                    ProductOut.Rows.Add(poutRec)
                End If

            Next
            For Each itm As String In phaseOutItems
                Dim pout As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
                pout.PART_NO = itm
                pout.LIST_PRICE = 0 : pout.RECYCLE_FEE = 0 : pout.UNIT_PRICE = 0
                ProductOut.AddProductOutRow(pout)
            Next
        Catch ex As Exception
            ErrorMessage += vbCrLf + "Exception Message of calling Bapi_Salesorder_Simulate:" + ex.ToString() : proxy1.Connection.Close() : Return False
        End Try
        proxy1.Connection.Close()
        If HasPhaseOutItem Then
            Return GetEUPrice(Org, SoldToId, ShipToId, strDistChann, strDivision, ProductIn, ProductOut, ErrorMessage)
        End If
        For Each pOutRow As SAPDALDS.ProductOutRow In ProductOut.Rows
            If IsNumeric(pOutRow.LIST_PRICE) AndAlso IsNumeric(pOutRow.UNIT_PRICE) AndAlso CDbl(pOutRow.LIST_PRICE) < CDbl(pOutRow.UNIT_PRICE) Then
                pOutRow.LIST_PRICE = pOutRow.UNIT_PRICE
            End If
        Next
        ProductOut.AcceptChanges()
        If String.IsNullOrEmpty(ErrorMessage) = False Then Return False
        Return True
    End Function
    <WebMethod()>
    Public Function GetMultiPrice_ABR_TAX(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String,
                                           ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOut_ABRDataTable, ByRef ErrorMessage As String) As Boolean
        ErrorMessage = ""
        Dim strDistChann As String = "10", strDivision As String = "00"
        Dim HasPhaseOutItem As Boolean = False, phaseOutItems As New ArrayList, ZSWLItemSet As New DataTable
        With ZSWLItemSet.Columns
            .Add("PartNo") : .Add("Qty", GetType(Integer))
        End With
        Dim RemoveAddedItem As Boolean = False : Dim AddedItemLineNo As String = ""
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))
        If SoldToId = "SAID" Then proxy1.ConnectionString = ConfigurationManager.AppSettings("SAP_PRD")
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        With OrderHeader
            .Doc_Type = "ZORB" : .Sales_Org = Trim(UCase(Org)) : .Distr_Chan = strDistChann : .Division = strDivision
        End With
        Dim LineNo As Integer = 1
        Dim sqlMA As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        sqlMA.Open()
        For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
            Dim chkSql As String =
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " +
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " +
                " where a.part_no='" + PInRow.PART_NO + "' and a.product_status in ('A','N','H','O') and a.sales_org='" + Org + "' "
            Dim chkDt As New DataTable, sqlAptr As New SqlClient.SqlDataAdapter(chkSql, sqlMA)
            Try
                sqlAptr.Fill(chkDt)
            Catch ex As SqlClient.SqlException
                sqlMA.Close() : ErrorMessage = ex.ToString() : Return Nothing
            End Try
            If chkDt.Rows.Count > 0 AndAlso (Org <> "TW01" Or (Org = "TW01") And chkDt.Rows(0).Item("ProfitCenter") <> "N/A") Then
                If chkDt.Rows(0).Item("ITEM_CATEGORY_GROUP") <> "ZSWL" Then
                    Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                    item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(PInRow.PART_NO.ToUpper())
                    item.Req_Qty = PInRow.QTY.ToString()
                    item.Req_Qty = CInt(item.Req_Qty) * 1000
                    ItemsIn.Add(item)
                    LineNo += 1
                Else
                    Dim zr As DataRow = ZSWLItemSet.NewRow()
                    zr.Item("PartNo") = PInRow.PART_NO.ToUpper() : zr.Item("Qty") = PInRow.QTY : ZSWLItemSet.Rows.Add(zr)
                End If
            Else
                phaseOutItems.Add(PInRow.PART_NO.ToUpper())
            End If
        Next
        sqlMA.Close()

        If ItemsIn.Count = 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = GetAHighLevelItemForPricing(Org)
            item.Req_Qty = 1
            item.Req_Qty = CInt(item.Req_Qty) * 1000
            ItemsIn.Add(item)
            RemoveAddedItem = True : AddedItemLineNo = LineNo.ToString()
            LineNo += 1
        End If
        If ItemsIn.Count > 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            For Each r As DataRow In ZSWLItemSet.Rows
                Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(r.Item("PartNo").Trim().ToUpper())
                item.Req_Qty = r.Item("Qty").ToString()
                item.Req_Qty = CInt(item.Req_Qty) * 1000
                item.Hg_Lv_Item = "1"
                ItemsIn.Add(item)
                LineNo += 1
            Next
        End If
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
        Dim retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = SoldToId : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = ShipToId
        Partners.Add(SoldTo) : Partners.Add(ShipTo)
        proxy1.Connection.Open()
        Try

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
            Dim retAdoDt As DataTable = retDt.ToADODataTable()

            For Each retMsgRec As DataRow In retAdoDt.Rows
                If retMsgRec.Item("Type") = "E" Then
                    HasPhaseOutItem = True
                    ErrorMessage += String.Format("Type:{0};Msg:{1}", retMsgRec.Item("Type"), retMsgRec.Item("Message_V1")) + vbCrLf
                End If
            Next

            Dim ConditionOut As DataTable = Conditions.ToADODataTable()
            Dim PInDt As DataTable = ItemsIn.ToADODataTable()
            Dim POutDt As DataTable = ItemsOut.ToADODataTable()

            'gv2.DataSource = POutDt : gv2.DataBind()

            DTRET = retDt.ToADODataTable()
            Dim ctrlCodeConn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
            Dim ctrlCodeCmd As New SqlClient.SqlCommand()
            ctrlCodeCmd.Connection = ctrlCodeConn
            ProductOut = New SAPDALDS.ProductOut_ABRDataTable
            For Each PIn As DataRow In PInDt.Rows
                Dim poutRec As SAPDALDS.ProductOut_ABRRow = ProductOut.NewProductOut_ABRRow()
                poutRec.PART_NO = Global_Inc.RemoveZeroString(PIn.Item("Material")) : poutRec.LIST_PRICE = 0
                Dim POutRs() As DataRow = POutDt.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
                If POutRs.Length > 0 Then
                    poutRec.NET_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
                    Dim cond_rs() As DataRow = ConditionOut.Select("Cond_Type='ICMI' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.PR_UNIT = FormatNumber(cond_rs(0).Item("Cond_Value"), 2)
                        poutRec.PR_TOTAL = FormatNumber(cond_rs(0).Item("Condvalue"), 2)
                    Else
                        poutRec.PR_UNIT = -1 : poutRec.PR_TOTAL = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='ZPR0' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.LIST_PRICE = FormatNumber(cond_rs(0).Item("Cond_Value"), 2)
                    Else
                        poutRec.LIST_PRICE = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='BX13' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.BX13 = FormatNumber(cond_rs(0).Item("Condvalue") / POutRs(0).Item("req_qty"), 2).ToString()
                    Else
                        poutRec.BX13 = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='BX23' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.BX23 = FormatNumber(cond_rs(0).Item("Condvalue") / POutRs(0).Item("req_qty"), 2).ToString()
                    Else
                        poutRec.BX23 = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='BX72' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.BX72 = FormatNumber(cond_rs(0).Item("Condvalue") / POutRs(0).Item("req_qty"), 2).ToString()
                    Else
                        poutRec.BX72 = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='BX82' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.BX82 = FormatNumber(cond_rs(0).Item("Condvalue") / POutRs(0).Item("req_qty"), 2).ToString()
                    Else
                        poutRec.BX82 = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='IPVA' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.IPI = FormatNumber(cond_rs(0).Item("Cond_Value"), 2).ToString()
                    Else
                        poutRec.IPI = -1
                    End If

                    If ctrlCodeCmd.Connection.State <> ConnectionState.Open Then ctrlCodeCmd.Connection.Open()
                    ctrlCodeCmd.CommandText = "select top 1 Ctrl_Code from SAP_PRODUCT_ABC where PART_NO='" + poutRec.PART_NO + "' and PLANT='BRH1' and Ctrl_Code<>'' and Ctrl_Code is not null"
                    Dim obj As Object = ctrlCodeCmd.ExecuteScalar()
                    If obj IsNot Nothing Then poutRec.NCM = obj.ToString()
                End If

                If Not RemoveAddedItem Or (RemoveAddedItem And Global_Inc.RemoveZeroString(PIn.Item("Itm_Number")) <> AddedItemLineNo) Then
                    ProductOut.Rows.Add(poutRec)
                End If

            Next
            If ctrlCodeCmd.Connection.State <> ConnectionState.Closed Then ctrlCodeCmd.Connection.Close()
            For Each itm As String In phaseOutItems
                Dim pout As SAPDALDS.ProductOut_ABRRow = ProductOut.NewProductOut_ABRRow()
                pout.PART_NO = itm
                pout.LIST_PRICE = 0 : pout.NET_PRICE = 0
                ProductOut.AddProductOut_ABRRow(pout)
            Next
        Catch ex As Exception
            ErrorMessage += vbCrLf + "Exception Message of calling Bapi_Salesorder_Simulate:" + ex.ToString() : proxy1.Connection.Close() : Return False
        End Try
        proxy1.Connection.Close()
        If String.IsNullOrEmpty(ErrorMessage) = False Then Return False
        Return True
    End Function

    Public Enum SAPOrderType
        ZOR
        ZOR2
        ZORB
        ZORC
        ZORR
        ZORI
    End Enum

    <WebMethod()>
    Public Function GetMultiPrice_ABR_TAX_2(ByVal Org As String, ByVal OrderType As SAPOrderType, ByVal SoldToId As String, ByVal ShipToId As String,
                                           ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOut_ABRDataTable,
                                           ByRef ErrorMessage As String) As Boolean
        ErrorMessage = ""
        Dim strDistChann As String = "10", strDivision As String = "00"
        Dim HasPhaseOutItem As Boolean = False, phaseOutItems As New ArrayList, ZSWLItemSet As New DataTable
        With ZSWLItemSet.Columns
            .Add("PartNo") : .Add("Qty", GetType(Integer))
        End With
        Dim RemoveAddedItem As Boolean = False : Dim AddedItemLineNo As String = ""
        Dim proxy1 As New BAPI_SALESORDER_SIMULATE.BAPI_SALESORDER_SIMULATE(ConfigurationManager.AppSettings("SAP_PRD"))
        If SoldToId = "SAID" Then proxy1.ConnectionString = ConfigurationManager.AppSettings("SAP_PRD")
        Dim OrderHeader As New BAPI_SALESORDER_SIMULATE.BAPISDHEAD, Partners As New BAPI_SALESORDER_SIMULATE.BAPIPARTNRTable
        Dim ItemsIn As New BAPI_SALESORDER_SIMULATE.BAPIITEMINTable, ItemsOut As New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable
        Dim Conditions As New BAPI_SALESORDER_SIMULATE.BAPICONDTable
        With OrderHeader
            .Doc_Type = OrderType.ToString() : .Sales_Org = Trim(UCase(Org)) : .Distr_Chan = strDistChann : .Division = strDivision
        End With
        Dim LineNo As Integer = 1
        Dim sqlMA As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        sqlMA.Open()
        For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
            Dim chkSql As String =
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " +
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " +
                " where a.part_no='" + PInRow.PART_NO + "' and a.product_status in ('A','N','H','O') and a.sales_org='" + Org + "' "
            Dim chkDt As New DataTable, sqlAptr As New SqlClient.SqlDataAdapter(chkSql, sqlMA)
            Try
                sqlAptr.Fill(chkDt)
            Catch ex As SqlClient.SqlException
                sqlMA.Close() : ErrorMessage = ex.ToString() : Return Nothing
            End Try
            If chkDt.Rows.Count > 0 AndAlso (Org <> "TW01" Or (Org = "TW01") And chkDt.Rows(0).Item("ProfitCenter") <> "N/A") Then
                If chkDt.Rows(0).Item("ITEM_CATEGORY_GROUP") <> "ZSWL" Then
                    Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                    item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(PInRow.PART_NO.ToUpper())
                    item.Req_Qty = PInRow.QTY.ToString()
                    item.Req_Qty = CInt(item.Req_Qty) * 1000
                    ItemsIn.Add(item)
                    LineNo += 1
                Else
                    Dim zr As DataRow = ZSWLItemSet.NewRow()
                    zr.Item("PartNo") = PInRow.PART_NO.ToUpper() : zr.Item("Qty") = PInRow.QTY : ZSWLItemSet.Rows.Add(zr)
                End If
            Else
                phaseOutItems.Add(PInRow.PART_NO.ToUpper())
            End If
        Next
        sqlMA.Close()

        If ItemsIn.Count = 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = GetAHighLevelItemForPricing(Org)
            item.Req_Qty = 1
            item.Req_Qty = CInt(item.Req_Qty) * 1000
            ItemsIn.Add(item)
            RemoveAddedItem = True : AddedItemLineNo = LineNo.ToString()
            LineNo += 1
        End If
        If ItemsIn.Count > 0 AndAlso ZSWLItemSet.Rows.Count > 0 Then
            For Each r As DataRow In ZSWLItemSet.Rows
                Dim item As New BAPI_SALESORDER_SIMULATE.BAPIITEMIN
                item.Itm_Number = FormatItmNumber(LineNo) : item.Material = Global_Inc.Format2SAPItem(r.Item("PartNo").Trim().ToUpper())
                item.Req_Qty = r.Item("Qty").ToString()
                item.Req_Qty = CInt(item.Req_Qty) * 1000
                item.Hg_Lv_Item = "1"
                ItemsIn.Add(item)
                LineNo += 1
            Next
        End If
        Dim SoldTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR, ShipTo As New BAPI_SALESORDER_SIMULATE.BAPIPARTNR
        Dim retDt As New BAPI_SALESORDER_SIMULATE.BAPIRET2Table
        SoldTo.Partn_Role = "AG" : SoldTo.Partn_Numb = SoldToId : ShipTo.Partn_Role = "WE" : ShipTo.Partn_Numb = ShipToId
        Partners.Add(SoldTo) : Partners.Add(ShipTo)
        proxy1.Connection.Open()
        Try

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
            Dim retAdoDt As DataTable = retDt.ToADODataTable()

            For Each retMsgRec As DataRow In retAdoDt.Rows
                If retMsgRec.Item("Type") = "E" Then
                    HasPhaseOutItem = True
                    ErrorMessage += String.Format("Type:{0};Msg:{1}", retMsgRec.Item("Type"), retMsgRec.Item("Message_V1")) + vbCrLf
                End If
            Next

            Dim ConditionOut As DataTable = Conditions.ToADODataTable()
            Dim PInDt As DataTable = ItemsIn.ToADODataTable()
            Dim POutDt As DataTable = ItemsOut.ToADODataTable()

            'gv2.DataSource = POutDt : gv2.DataBind()

            DTRET = retDt.ToADODataTable()
            Dim ctrlCodeConn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
            Dim ctrlCodeCmd As New SqlClient.SqlCommand()
            ctrlCodeCmd.Connection = ctrlCodeConn
            ProductOut = New SAPDALDS.ProductOut_ABRDataTable
            For Each PIn As DataRow In PInDt.Rows
                Dim poutRec As SAPDALDS.ProductOut_ABRRow = ProductOut.NewProductOut_ABRRow()
                poutRec.PART_NO = Global_Inc.RemoveZeroString(PIn.Item("Material")) : poutRec.LIST_PRICE = 0
                Dim POutRs() As DataRow = POutDt.Select("Itm_Number='" + PIn.Item("Itm_Number") + "'")
                If POutRs.Length > 0 Then
                    poutRec.NET_PRICE = FormatNumber(POutRs(0).Item("Net_Value1") / POutRs(0).Item("req_qty"), 2)
                    Dim cond_rs() As DataRow = ConditionOut.Select("Cond_Type='ICMI' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.PR_UNIT = FormatNumber(cond_rs(0).Item("Cond_Value"), 2)
                        poutRec.PR_TOTAL = FormatNumber(cond_rs(0).Item("Condvalue"), 2)
                    Else
                        poutRec.PR_UNIT = -1 : poutRec.PR_TOTAL = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='ZPR0' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.LIST_PRICE = FormatNumber(cond_rs(0).Item("Cond_Value"), 2)
                    Else
                        poutRec.LIST_PRICE = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='BX13' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.BX13 = FormatNumber(cond_rs(0).Item("Condvalue") / POutRs(0).Item("req_qty"), 2).ToString()
                    Else
                        poutRec.BX13 = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='BX23' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.BX23 = FormatNumber(cond_rs(0).Item("Condvalue") / POutRs(0).Item("req_qty"), 2).ToString()
                    Else
                        poutRec.BX23 = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='BX72' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.BX72 = FormatNumber(cond_rs(0).Item("Condvalue") / POutRs(0).Item("req_qty"), 2).ToString()
                    Else
                        poutRec.BX72 = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='BX82' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.BX82 = FormatNumber(cond_rs(0).Item("Condvalue") / POutRs(0).Item("req_qty"), 2).ToString()
                    Else
                        poutRec.BX82 = -1
                    End If
                    cond_rs = ConditionOut.Select("Cond_Type='IPVA' AND Itm_Number='" + PIn.Item("Itm_Number") + "'")
                    If cond_rs.Length > 0 Then
                        poutRec.IPI = FormatNumber(cond_rs(0).Item("Cond_Value"), 2).ToString()
                    Else
                        poutRec.IPI = -1
                    End If

                    If ctrlCodeCmd.Connection.State <> ConnectionState.Open Then ctrlCodeCmd.Connection.Open()
                    ctrlCodeCmd.CommandText = "select top 1 Ctrl_Code from SAP_PRODUCT_ABC where PART_NO='" + poutRec.PART_NO + "' and PLANT='BRH1' and Ctrl_Code<>'' and Ctrl_Code is not null"
                    Dim obj As Object = ctrlCodeCmd.ExecuteScalar()
                    If obj IsNot Nothing Then poutRec.NCM = obj.ToString()
                End If

                If Not RemoveAddedItem Or (RemoveAddedItem And Global_Inc.RemoveZeroString(PIn.Item("Itm_Number")) <> AddedItemLineNo) Then
                    ProductOut.Rows.Add(poutRec)
                End If

            Next
            If ctrlCodeCmd.Connection.State <> ConnectionState.Closed Then ctrlCodeCmd.Connection.Close()
            For Each itm As String In phaseOutItems
                Dim pout As SAPDALDS.ProductOut_ABRRow = ProductOut.NewProductOut_ABRRow()
                pout.PART_NO = itm
                pout.LIST_PRICE = 0 : pout.NET_PRICE = 0
                ProductOut.AddProductOut_ABRRow(pout)
            Next
        Catch ex As Exception
            ErrorMessage += vbCrLf + "Exception Message of calling Bapi_Salesorder_Simulate:" + ex.ToString() : proxy1.Connection.Close() : Return False
        End Try
        proxy1.Connection.Close()
        If String.IsNullOrEmpty(ErrorMessage) = False Then Return False
        Return True
    End Function

    Private Shared Function GetMultiPrice_BR(ByVal SoldToId As String, ByVal ShipToId As String,
                                      ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOutDataTable,
                                      ByRef strErrMsg As String) As Boolean
        Dim Vkorg As String = "BR01"
        SoldToId = Trim(UCase(SoldToId))
        Dim proxy1 As New Z_EBIZAEU_PRICEINQUIRY_BR.Z_EBIZAEU_PRICEINQUIRY_BR(ConfigurationManager.AppSettings("SAP_PRD"))
        Dim zssD_01Table1 As New Z_EBIZAEU_PRICEINQUIRY_BR.ZSSD_01Table, zssD_02Table1 As New Z_EBIZAEU_PRICEINQUIRY_BR.ZSSD_02Table
        Dim distr_chann As String = "10", Division As String = "00"
        For Each prodRec As SAPDALDS.ProductInRow In ProductIn.Rows
            Dim zssd_1 As New Z_EBIZAEU_PRICEINQUIRY_BR.ZSSD_01
            With zssd_1
                .Kunnr = SoldToId : .Mandt = "168" : .Matnr = Global_Inc.Format2SAPItem(prodRec.PART_NO) : .Mglme = prodRec.QTY : .Vkorg = Vkorg
            End With
            zssD_01Table1.Add(zssd_1)
        Next
        Try
            proxy1.Connection.Open()
            proxy1.Z_Ebizaeu_Priceinquiry_Br("ZORB", distr_chann, Division, SoldToId, Vkorg.Trim().ToUpper(), ShipToId,
                                              New Z_EBIZAEU_PRICEINQUIRY_BR.BAPIRETURN, zssD_01Table1, zssD_02Table1)

        Catch ex As Exception
            strErrMsg = "Call Z_Ebizaeu_Priceinquiry_Br error:" + ex.ToString()
            proxy1.Connection.Close() : Return False
        End Try

        proxy1.Connection.Close()
        ProductOut = New SAPDALDS.ProductOutDataTable
        Dim sapPOutDt As DataTable = zssD_02Table1.ToADODataTable()
        For Each sapPOutRec As DataRow In sapPOutDt.Rows
            Dim pOutRec As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
            pOutRec.PART_NO = Global_Inc.RemoveZeroString(sapPOutRec.Item("Matnr"))
            pOutRec.LIST_PRICE = sapPOutRec.Item("Kzwi1")
            pOutRec.UNIT_PRICE = sapPOutRec.Item("Netwr")
            If (pOutRec.LIST_PRICE = 0) Then pOutRec.LIST_PRICE = -1
            If (pOutRec.UNIT_PRICE = 0) Then pOutRec.UNIT_PRICE = -1
            If pOutRec.LIST_PRICE < pOutRec.UNIT_PRICE Then pOutRec.LIST_PRICE = pOutRec.UNIT_PRICE
            ProductOut.AddProductOutRow(pOutRec)
        Next

        Return True

    End Function

    Private Shared Function GetEUPrice(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String,
                                          ByVal strDivision As String, ByVal ProductIn As SAPDALDS.ProductInDataTable,
                                          ByRef ProductOut As SAPDALDS.ProductOutDataTable,
                                          ByRef ErrorMessage As String) As Boolean
        ProductOut = New SAPDALDS.ProductOutDataTable
        Dim pDate As String = Now.ToString("yyyyMMdd")
        Dim eup As New Z_SD_EUPRICEINQUERY.Z_SD_EUPRICEINQUERY
        Dim pin As New Z_SD_EUPRICEINQUERY.ZSSD_01_EUTable, pout As New Z_SD_EUPRICEINQUERY.ZSSD_02_EUTable
        For Each pinRec As SAPDALDS.ProductInRow In ProductIn.Rows
            Dim prec As New Z_SD_EUPRICEINQUERY.ZSSD_01_EU
            With prec
                .Kunnr = SoldToId : .Mandt = "168" : .Matnr = Global_Inc.Format2SAPItem(pinRec.PART_NO)
                .Mglme = pinRec.QTY : .Prsdt = pDate : .Vkorg = Org
            End With
            pin.Add(prec)
        Next

        'Next
        eup.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        eup.Connection.Open()
        Try
            eup.Z_Sd_Eupriceinquery("1", pin, pout)
        Catch ex As Exception
            eup.Connection.Close() : ErrorMessage += vbCrLf + ex.ToString() : Return False
        End Try
        eup.Connection.Close()
        'OrderUtilities.showDT(pout.ToADODataTable)
        For i As Integer = 0 To pout.Count - 1
            Dim pOutRec As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
            Dim retRec As Z_SD_EUPRICEINQUERY.ZSSD_02_EU = pout.Item(i)
            pOutRec.PART_NO = Global_Inc.RemoveZeroString(retRec.Matnr)
            If retRec.Mglme > 0 Then
                pOutRec.LIST_PRICE = retRec.Kzwi1 / retRec.Mglme
                pOutRec.UNIT_PRICE = retRec.Netwr / retRec.Mglme
            Else
                pOutRec.LIST_PRICE = retRec.Kzwi1 : pOutRec.UNIT_PRICE = retRec.Netwr
            End If

            If Decimal.TryParse(pOutRec.LIST_PRICE, 0) AndAlso Decimal.TryParse(pOutRec.UNIT_PRICE, 0) Then
                If Decimal.Parse(pOutRec.LIST_PRICE) < Decimal.Parse(pOutRec.UNIT_PRICE) Then pOutRec.LIST_PRICE = pOutRec.UNIT_PRICE
            End If

            pOutRec.TAX = 0
            pOutRec.RECYCLE_FEE = 0
            pOutRec.Condition_ZMIP = 0 : pOutRec.Condition_VPRS = 0
            ProductOut.AddProductOutRow(pOutRec)
        Next
        Return True
        'Return pdt
    End Function


    Public Shared Function GetAHighLevelItemForPricing(org As String) As String
        Dim dicPNOrg As Dictionary(Of String, String) = HttpContext.Current.Cache("High Level Pricing PN Org Pair")
        If dicPNOrg Is Nothing Then
            dicPNOrg = New Dictionary(Of String, String)
            HttpContext.Current.Cache.Add("High Level Pricing PN Org Pair", dicPNOrg, Nothing, Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If
        If Not dicPNOrg.ContainsKey(org) Then
            'Dim cmd As New SqlClient.SqlCommand( _
            '    " select top 1 a.PART_NO from SAP_PRODUCT a inner join SAP_PRODUCT_STATUS b on a.PART_NO=b.PART_NO " + _
            '    " where a.MATERIAL_GROUP='PRODUCT' and b.PRODUCT_STATUS='A' and b.SALES_ORG=@ORG and a.PRODUCT_LINE like 'ADM%' and a.PART_NO like 'ADAM-%' ", _
            '    New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))

            Dim cmd As New SqlClient.SqlCommand(
                " select top 1 a.PART_NO from SAP_PRODUCT a inner join SAP_PRODUCT_STATUS b on a.PART_NO=b.PART_NO " +
                " where a.MATERIAL_GROUP='BTOS' and b.PRODUCT_STATUS='A' and b.SALES_ORG=@ORG and a.PRODUCT_LINE like 'IPC%' and a.PART_NO like 'IPC-%-BTO' ",
                New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))


            cmd.Parameters.AddWithValue("ORG", org)
            cmd.Connection.Open()
            Dim pnObject As Object = cmd.ExecuteScalar()
            cmd.Connection.Close()
            If pnObject IsNot Nothing Then
                dicPNOrg.Add(org, pnObject.ToString())
            End If
        End If
        If dicPNOrg.ContainsKey(org) Then
            Return dicPNOrg.Item(org)
        End If
        Return "ADAM-4520-EE"
    End Function

#End Region

#Region "Credit"

    <WebMethod()>
    Public Function GetCustomerCreditExposure(ByVal CustomerId As String, ByVal Org As String, ByRef CreditLimit As Decimal,
                                              ByRef CreditExposure As Decimal, ByRef Percentage As String) As Boolean
        CustomerId = Trim(UCase(CustomerId)) : Org = Trim(UCase(Org))
        Select Case Left(Org, 2)
            Case "EU", "AU", "JP", "MY", "BR", "SG", "TL", "TW"
                Org = Left(Org, 2) + "01"
            Case "CN"
                Org = Left(Org, 2) + "C1"
            Case "US"
                'Frank this is for AENC's special rule
                If Org.Equals("USAENC", StringComparison.InvariantCultureIgnoreCase) Then
                    Org = Left(Org, 2) + "C2"
                Else
                    Org = Left(Org, 2) + "C1"
                End If
            Case "HK"
                Org = Left(Org, 2) + "05"
        End Select
        Dim strHorizonDate As String = DateAdd(DateInterval.Month, 1, Now).ToString("yyyyMMdd")
        Dim p As New GetCreditExposure.GetCreditExposure(ConfigurationManager.AppSettings("SAP_PRD"))
        Dim cmware As String = "", Delta2Limit As Decimal, dtKnkk As GetCreditExposure.KNKK = Nothing, Knkli As String = ""
        Dim OpenDelivery As Decimal, OpenDeliverySecure As Decimal, OpenInvoice As Decimal, OpenInvoiceSecure As Decimal
        Dim OpenItems As Decimal, OpenOrders As Decimal, OpenOrderSecure As Decimal, OpenSepcial As Decimal, SumOpen As Decimal
        p.Connection.Open()
        Try
            p.Zcredit_Exposure(strHorizonDate, Org, CustomerId, cmware, CreditLimit, Delta2Limit, dtKnkk, Knkli, OpenDelivery, OpenDeliverySecure,
                       OpenInvoice, OpenInvoiceSecure, OpenItems, OpenOrders, OpenOrderSecure, OpenSepcial, Percentage, SumOpen)
        Catch ex As Exception
            p.Connection.Close() : Return False
        End Try
        p.Connection.Close()
        CreditExposure = CreditLimit + Delta2Limit
        Return True
    End Function
#End Region

#Region "Employee"
    Public Shared Function IsJPPowerUser(ByVal Userid As String) As Boolean
        'Mary.Huang:因為Doi san是AJP Back office的Head，請協助用單獨加入的方式
        If Userid.Equals("manami.doi@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If Userid.Equals("tanya.lin@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If Userid.Equals("Mary.Huang@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then Return True
        If Userid.Equals("eileen.soh@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then Return True
        Return False
    End Function
    Public Shared Sub GetInternalNamebyADAndSiebel(ByVal email As String, ByRef last_name As String, ByRef first_name As String)
        'Dim sql As String = String.Format("select isnull(b.firstname,'') as firstname, isnull(b.lastname,'') as lastname from ADVANTECH_ADDRESSBOOK b inner join ADVANTECH_ADDRESSBOOK_ALIAS a on a.ID=b.ID where a.Email ='{0}' or b.PrimarySmtpAddress ='{0}' ", email)
        Dim sql As String = String.Format("select isnull(b.firstname,'') as firstname, isnull(b.lastname,'') as lastname from AD_MEMBER b inner join AD_MEMBER_ALIAS a on b.PrimarySmtpAddress=a.EMAIL where a.ALIAS_EMAIL ='{0}' or b.PrimarySmtpAddress ='{0}' ", email)
        Dim dt As DataTable = dbUtil.dbGetDataTable("MY", sql)
        If dt.Rows.Count > 0 Then
            first_name = dt.Rows(0).Item("firstname").ToString : last_name = dt.Rows(0).Item("lastname").ToString
        End If
        If String.IsNullOrEmpty(first_name) AndAlso String.IsNullOrEmpty(last_name) Then
            sql = String.Format("select isnull(firstname,'') as firstname, isnull(lastname,'') as lastname from SIEBEL_CONTACT where EMAIL_ADDRESS='{0}' ", email)
            dt = dbUtil.dbGetDataTable("MY", sql)
            If dt.Rows.Count > 0 Then
                first_name = dt.Rows(0).Item("firstname").ToString : last_name = dt.Rows(0).Item("lastname").ToString
            End If
        End If
    End Sub


    Public Shared Function GetSalesRepresentativeEmailByEmployeeID(ByVal employeeID As String, ByVal orderCreatorEmail As String) As String

        Dim _SalesPersonEmail As String = String.Empty, _dt As DataTable = Nothing

        If Not String.IsNullOrEmpty(employeeID) Then

            _dt = dbUtil.dbGetDataTable("MY", String.Format("Select EMAIL from SAP_EMPLOYEE where SALES_CODE='{0}'", employeeID))

            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                _SalesPersonEmail = _dt.Rows(0).Item("EMAIL").ToString
                If Not String.IsNullOrEmpty(_SalesPersonEmail) Then Return _SalesPersonEmail
            End If

        End If

        Return orderCreatorEmail

    End Function



    Public Shared Function GetSalesRepresentativeByEmployeeID(ByVal employeeID As String, ByVal orderCreatorEmail As String) As String

        Dim _SalesPerson As String = String.Empty, _dt As DataTable = Nothing

        If Not String.IsNullOrEmpty(employeeID) Then

            _dt = dbUtil.dbGetDataTable("MY", String.Format("Select FULL_NAME,FIRST_NAME,LAST_NAME from SAP_EMPLOYEE where SALES_CODE='{0}'", employeeID))

            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                _SalesPerson = _dt.Rows(0).Item("FULL_NAME").ToString
                If String.IsNullOrEmpty(_SalesPerson) Then _SalesPerson = _dt.Rows(0).Item("FIRST_NAME") + " " + _dt.Rows(0).Item("LAST_NAME")
            End If

        End If

        If String.IsNullOrEmpty(_SalesPerson) Then
            'Get name from Siebel:This logic copy from eQuotation Util.GetSalesRepresentative

            Dim firstname As String = "", lastname As String = ""
            GetInternalNamebyADAndSiebel(orderCreatorEmail, lastname, firstname)
            If lastname = "" AndAlso firstname = "" Then
                Dim email_name As String = orderCreatorEmail.ToString.Split("@")(0)
                If email_name.Contains(".") Then
                    For Each name As String In email_name.Split(".")
                        _SalesPerson += name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1).ToLower + " "
                    Next
                Else
                    _SalesPerson += email_name.Substring(0, 1).ToUpper() + email_name.Substring(1, email_name.Length - 1).ToLower
                End If
            Else
                _SalesPerson += firstname + " " + lastname
            End If


        End If


        Return _SalesPerson

    End Function


    Public Shared Function GetSalesRepresentativeForUSAonline(ByVal employeeID As String, ByVal orderCreatorEmail As String, Optional SalesTel As String = "") As String

        Dim _SalesPerson As String = String.Empty, _dt As DataTable = Nothing
        Dim _SalesEmail As String = String.Empty
        If Not String.IsNullOrEmpty(employeeID) Then

            _dt = dbUtil.dbGetDataTable("MY", String.Format("Select FULL_NAME,FIRST_NAME,LAST_NAME,EMAIL from SAP_EMPLOYEE where SALES_CODE='{0}'", employeeID))

            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then

                '_SalesPerson = _dt.Rows(0).Item("FULL_NAME").ToString
                'If String.IsNullOrEmpty(_SalesPerson) Then
                '    _SalesPerson = _dt.Rows(0).Item("FIRST_NAME") + " " + _dt.Rows(0).Item("LAST_NAME")
                'End If
                _SalesPerson = _dt.Rows(0).Item("FIRST_NAME") + " " + _dt.Rows(0).Item("LAST_NAME")

                If Not String.IsNullOrEmpty(_SalesPerson) Then
                    _SalesEmail = _dt.Rows(0).Item("EMAIL").ToString.ToLower
                    _SalesPerson += " Email: <A Href='mailto:" & _SalesEmail & "'>" & _SalesEmail & "</A>"
                End If

            End If

        End If

        If String.IsNullOrEmpty(_SalesPerson) Then
            'Get name from Siebel:This logic copy from eQuotation Util.GetSalesRepresentative

            Dim firstname As String = "", lastname As String = ""
            GetInternalNamebyADAndSiebel(orderCreatorEmail, lastname, firstname)
            If lastname = "" AndAlso firstname = "" Then
                Dim email_name As String = orderCreatorEmail.ToString.Split("@")(0)
                If email_name.Contains(".") Then
                    For Each name As String In email_name.Split(".")
                        _SalesPerson += name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1).ToLower + " "
                    Next
                Else
                    _SalesPerson += email_name.Substring(0, 1).ToUpper() + email_name.Substring(1, email_name.Length - 1).ToLower
                End If
            Else
                _SalesPerson += firstname + " " + lastname
            End If

            _SalesPerson += " Email: <A Href='mailto:" & orderCreatorEmail & "'>" & orderCreatorEmail & "</A>"
            _SalesEmail = orderCreatorEmail
        End If

        'Sales tel
        If Not String.IsNullOrEmpty(_SalesPerson) AndAlso Not String.IsNullOrEmpty(_SalesEmail) Then

            Dim _tel As String = String.Empty

            If Not String.IsNullOrEmpty(SalesTel) Then
                _tel = SalesTel
            Else
                'Dim strsql As String = String.Empty
                'strsql &= " Select TEL From ANA_AOnlineSales_Tel "
                'strsql &= " Where EMail='" & _SalesEmail & "'"

                '_dt = dbUtil.dbGetDataTable("EQ", strsql)
                'If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                '    _tel = _dt.Rows(0).Item("TEL")
                'End If
                _tel = GetSalesWorkPhone(_SalesEmail)
            End If

            If Not String.IsNullOrEmpty(_SalesPerson) AndAlso Not String.IsNullOrEmpty(_tel) AndAlso Not _tel.Equals("Ignore", StringComparison.InvariantCultureIgnoreCase) Then
                _SalesPerson += " / " & _tel
            End If

        End If

        Return _SalesPerson

    End Function

    Public Shared Function GetSalesWorkPhone(ByVal _SalesEmail As String) As String
        Dim strsql As String = String.Empty
        strsql &= " Select TEL From ANA_AOnlineSales_Tel "
        strsql &= " Where EMail='" & _SalesEmail & "'"

        Dim _dt = dbUtil.dbGetDataTable("EQ", strsql), _tel As String = String.Empty
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            _tel = _dt.Rows(0).Item("TEL")
        End If

        If _tel Is DBNull.Value Then _tel = String.Empty

        Return _tel
    End Function

    Public Shared Function GetSalesInfo(ByVal _SalesEmail As String) As DataTable
        Dim strsql As String = String.Empty
        strsql &= " Select EMail,TEL,TeamName,PositionName From ANA_AOnlineSales_Tel "
        strsql &= " Where EMail='" & _SalesEmail & "'"

        Return dbUtil.dbGetDataTable("EQ", strsql)
    End Function

    Public Shared Function GetAKRSalesInfo(ByVal _SalesEmail As String) As DataTable
        Dim strsql As String = String.Empty
        strsql &= " Select Team_Name,Name,Position,Sales_email,Report_to,Office_Tel,Mobile_Phone From AKR_Sales_List "
        strsql &= " Where Sales_email='" & _SalesEmail & "'"

        Return dbUtil.dbGetDataTable("EQ", strsql)
    End Function


#End Region

#Region "Inventory"

    <WebMethod()>
    Public Sub QueryLimitQuantity(ByVal VK_ORG As String, ByVal PART_NO() As String, ByRef Result As DataSet, ByRef strErrMsg As String)

        Try

            Dim proxy1 As New Z_GET_ATP_LIMITQTY.Z_GET_ATP_LIMITQTY(ConfigurationManager.AppSettings("SAP_PRD"))
            'Dim proxy1 As New Z_GET_ATP_LIMITQTY.Z_GET_ATP_LIMITQTY(ConfigurationManager.AppSettings("SAPConnTest"))
            Dim dtZTBLATP As New ZTBLATPTable
            Dim dtZTBLMATNR As New ZTBLMATNRTable
            Dim i As Integer
            For i = 0 To PART_NO.Length - 1
                Dim drZTBLMATNR As New ZTBLMATNR
                drZTBLMATNR.Matnr = PART_NO(i)
                dtZTBLMATNR.Add(drZTBLMATNR)
            Next

            proxy1.Connection.Open()
            proxy1.Zget_Atp_Limitqty("X", VK_ORG, dtZTBLMATNR, dtZTBLATP)
            proxy1.Connection.Close()
            Result.Tables.Add(dtZTBLATP.ToADODataTable())
            strErrMsg = ""
        Catch ex As Exception
            strErrMsg = ex.ToString()
        End Try

    End Sub



    ''' <summary>
    ''' Query Product Inventory
    ''' </summary>
    ''' <param name="PartNos"></param>
    ''' <param name="plant"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Function QueryInventory(ByVal PartNos As SAPDALDS.ProductInDataTable, ByVal plant As String,
                                   ByRef QueryResult As SAPDALDS.QueryInventory_OutputDataTable,
                                   ByRef ErrorMsg As String) As Boolean

        Return Me.QueryInventory_V2(PartNos, plant, Now, QueryResult, ErrorMsg)

    End Function

    ''' <summary>
    ''' Query Product Inventory V2
    ''' </summary>
    ''' <param name="PartNos"></param>
    ''' <param name="plant"></param>
    ''' <param name="Req_Date">format is yyyyMMdd(Ex 20120101)</param>
    ''' <param name="QueryResult"></param>
    ''' <param name="ErrorMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Function QueryInventory_V2(ByVal PartNos As SAPDALDS.ProductInDataTable, ByVal plant As String,
                                   ByVal Req_Date As Date, ByRef QueryResult As SAPDALDS.QueryInventory_OutputDataTable,
                                   ByRef ErrorMsg As String) As Boolean
        Return QueryInventory_V3(PartNos, plant, Req_Date, QueryResult, "", ErrorMsg)

    End Function

    <WebMethod()>
    Public Function QueryInventory_V3(ByVal PartNos As SAPDALDS.ProductInDataTable, ByVal plant As String,
                                     ByVal Req_Date As Date, ByRef QueryResult As SAPDALDS.QueryInventory_OutputDataTable,
                                     ByVal Stoc As String,
                                     ByRef ErrorMsg As String) As Boolean
        Try

            If PartNos Is Nothing Then
                ErrorMsg = "Input value PartNos Is null" : Return False
            End If
            If PartNos.Rows.Count = 0 Then
                ErrorMsg = "Input value PartNos has no record" : Return False
            End If

            Dim _req_date As String = Format(Req_Date, "yyyyMMdd")

            'QueryResult(called by reference) is return value that keeps the Inventory Information
            If QueryResult Is Nothing Then QueryResult = New SAPDALDS.QueryInventory_OutputDataTable()

            Dim dt As DataTable = Nothing

            'Create GET_MATERIAL_ATP object of SAP API 
            Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP
            'Set connect string
            p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            'Open connection
            p1.Connection.Open()

            Dim retDate As Date = DateAdd(DateInterval.Day, -1, Now), retQty As Integer = 0

            'Create object as parameter for calling GET_MATERIAL_ATP.Bapi_Material_Availability
            Dim retTb As GET_MATERIAL_ATP.BAPIWMDVSTable = Nothing, atpTb As GET_MATERIAL_ATP.BAPIWMDVETable = Nothing, rOfretTb As GET_MATERIAL_ATP.BAPIWMDVS = Nothing

            Dim Inventory As Int16 = 0, _PartNo As String = String.Empty, _FormatedPartNo As String = String.Empty
            Dim _Qty As String = String.Empty, _QTYSumValue As Integer = 0

            'Frank 2012/09/26 If part number is not specified delivery plant then use function call parameter "plant" instead of.
            For Each _row As SAPDALDS.ProductInRow In PartNos.Rows
                If _row.IsPLANTNull OrElse String.IsNullOrEmpty(_row.PLANT) Then _row.PLANT = UCase(plant)
            Next

            'sort by part number
            Dim InputPartView As DataView = PartNos.DefaultView, _ExecutingPartItem As String = String.Empty, _ExecutedPartItem As String = String.Empty
            InputPartView.Sort = "PART_NO"

            For Each _RowView As DataRowView In InputPartView

                _PartNo = Trim(UCase(_RowView.Item("PART_NO")))

                'if part no is IsNullOrEmpty or already got the inventory information then skip this one
                If String.IsNullOrEmpty(_PartNo) Then Continue For

                _ExecutingPartItem = "Part=" & _PartNo & ":Plant=" & UCase(_RowView.Item("PLANT"))

                If _ExecutedPartItem.Equals(_ExecutingPartItem, StringComparison.InvariantCultureIgnoreCase) Then Continue For
                _ExecutedPartItem = _ExecutingPartItem

                'Counting the total qty for input part number
                _QTYSumValue = 0 : _QTYSumValue = PartNos.Compute("Sum(QTY)", "PART_NO='" & _RowView.Item("PART_NO") & "' And PLANT='" & _RowView.Item("PLANT") & "' ")
                _Qty = Trim(_QTYSumValue & "")

                'Format partno string
                _FormatedPartNo = Global_Inc.Format2SAPItem(_PartNo)
                retTb = New GET_MATERIAL_ATP.BAPIWMDVSTable : atpTb = New GET_MATERIAL_ATP.BAPIWMDVETable : rOfretTb = New GET_MATERIAL_ATP.BAPIWMDVS
                Try
                    rOfretTb.Req_Qty = Decimal.Parse(_Qty)
                    If rOfretTb.Req_Qty < 0 Then rOfretTb.Req_Qty = 9999
                Catch ex As Exception
                    'If QTY is not an integer then set QTY to 9999
                    rOfretTb.Req_Qty = 9999
                End Try

                'Default Req_Date is nothing. Means from now
                rOfretTb.Req_Date = _req_date

                retTb.Add(rOfretTb)


                'call Bapi_Material_Availability to get inventory information
                p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", _FormatedPartNo, UCase(_RowView.Item("PLANT")), "", "", Stoc, "", "PC",
                              "", Inventory, "", "", New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)

                'Get inventory result
                dt = atpTb.ToADODataTable()

                'write inventory information into datatable for return value
                For Each _InventoryRow As DataRow In dt.Rows

                    If IsDBNull(_InventoryRow.Item("Com_Qty")) Then Continue For
                    If _InventoryRow.Item("Com_Qty") = 0 Then Continue For

                    'ICC 2017/10/25 Set 9999 as maximum number for atp
                    Dim atp As Decimal = 0
                    Decimal.TryParse(_InventoryRow.Item("Com_Qty").ToString, atp)
                    If atp > 9999 Then atp = 9999
                    QueryResult.AddQueryInventory_OutputRow(_PartNo _
                                                            , DateTime.ParseExact(_InventoryRow.Item("Com_Date"), "yyyyMMdd", Nothing) _
                                                            , atp _
                                                            , UCase(_RowView.Item("PLANT")))
                Next

            Next

            InputPartView = Nothing

            'close connection
            p1.Connection.Close()

        Catch ex As Exception
            ErrorMsg = ex.Message : Return False
        End Try
        Return True

    End Function
#End Region

#Region "Product"
    Enum itpType
        EU = 0
        JP = 1
        KR = 2
        CN = 3
    End Enum
    Shared Function getItpV0(ByVal ORG As String, ByVal PARTNO As String, ByVal CURR As String, ByVal companyId As String, Optional ByVal type As itpType = itpType.EU) As Decimal
        Dim ITP As Decimal = 0
        'Dim ITPfirst As New ITP_first("EQ", "ITP_first")

        Dim DT As New DataTable
        'DT = ITPfirst.GetDT(String.Format("org='{0}' and part_no='{1}' and currency='{2}' and companyId='{3}'", ORG, PARTNO, CURR, companyId), "")
        'If DT.Rows.Count <= 0 Then
        Dim c As String = ""
        If type = itpType.EU Then
            c = "EUR"
        ElseIf type = itpType.JP Then
            c = "JPY"
        End If
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@org", ORG),
                                            New SqlClient.SqlParameter("@PN", PARTNO),
                                             New SqlClient.SqlParameter("@CUR", c)}
        DT = sqlhelper.getDT("EQ", CommandType.Text, String.Format("select * from Product_ITP where org=@org and part_no=@PN and currency=@CUR"), p)

        Dim sapdal As New SAPDAL
        'End If
        If DT.Rows.Count > 0 Then
            ITP = DT.Rows(0).Item("itp")
        End If
        If type = itpType.EU Then
            If ITP = 0 AndAlso (Not isEUITPFromUUAAESC(PARTNO)) Then
                Dim sapITPDt As New DataTable
                If sapdal.getSAPPriceByTable(PARTNO, "TW01", "EHLA002", sapITPDt) = 1 Then
                    If sapITPDt.Rows.Count > 0 AndAlso CDbl(sapITPDt.Rows(0).Item("Netwr")) > 0 Then
                        ITP = CDbl(sapITPDt.Rows(0).Item("Netwr").ToString.Replace(",", ""))
                        ITP = ITP * 1.045
                        ITP = FormatNumber(ITP, 2)
                        writeItpBack(ORG, PARTNO, c, ITP)
                        'Catch ex As Exception
                        'End Try
                    End If
                End If
            End If
            If ITP = 0 Then
                Dim sapITPDt As New DataTable
                If sapdal.getSAPPriceByTable(PARTNO, "EU10", "UUAAESC", sapITPDt) = 1 Then
                    If sapITPDt.Rows.Count > 0 AndAlso CDbl(sapITPDt.Rows(0).Item("Netwr")) > 0 Then
                        ITP = CDbl(sapITPDt.Rows(0).Item("Netwr").ToString.Replace(",", ""))
                        ITP = FormatNumber(ITP, 2)
                        writeItpBack(ORG, PARTNO, c, ITP)
                        'Catch ex As Exception
                        'End Try
                    End If
                End If
            End If
        ElseIf type = itpType.JP Then
            If ITP = 0 Then
                Dim sapITPDt As New DataTable
                If sapdal.getSAPPriceByTable(PARTNO, "TW01", "AJPADV", sapITPDt) = 1 Then
                    If sapITPDt.Rows.Count > 0 AndAlso CDbl(sapITPDt.Rows(0).Item("Netwr")) > 0 Then
                        ITP = CDbl(sapITPDt.Rows(0).Item("Netwr").ToString.Replace(",", ""))
                        ITP = FormatNumber(ITP, 2)
                        ITP = ITP * 1.03
                        writeItpBack(ORG, PARTNO, c, ITP)
                        'Catch ex As Exception
                        'End Try
                    End If
                End If
            End If
        End If

        Return ITP
    End Function

    Class ITPDataEntity
        Property PartNO As String = String.Empty
        Property ORG As String = String.Empty
        Property Currency As String = String.Empty
        Property ITP As Decimal = 0
    End Class


    Shared Function getItp(ByVal ORG As String, ByVal PARTNO As String, ByVal Currency As String, ByVal companyId As String, ByVal type As itpType) As Decimal
        Dim ITP As Decimal = 0
        'Dim ITPfirst As New ITP_first("EQ", "ITP_first")

        Dim DT As New DataTable
        'DT = ITPfirst.GetDT(String.Format("org='{0}' and part_no='{1}' and currency='{2}' and companyId='{3}'", ORG, PARTNO, CURR, companyId), "")
        'If DT.Rows.Count <= 0 Then

        Dim dtITP As New DataTable, InsertCache As Boolean = True
        Dim listITP As List(Of ITPDataEntity)  '  (Of ITPDataEntity)
        If HttpContext.Current.Cache("PartNO_ITP") Is Nothing Then
            'dtITP.Columns.Add("PartNO", GetType(String)) : dtITP.Columns.Add("ORG", GetType(String))
            'dtITP.Columns.Add("Currency", GetType(String)) : dtITP.Columns.Add("ITP", GetType(Decimal))
            'HttpContext.Current.Cache.Insert("PartNO_ITP", dtITP, Nothing, DateTime.Now.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
            listITP = New List(Of ITPDataEntity)
            HttpContext.Current.Cache.Insert("PartNO_ITP", listITP, Nothing, DateTime.Now.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        Else
            'dtITP = CType(HttpContext.Current.Cache("PartNO_ITP"), DataTable)
            'If dtITP.Rows.Count > 0 Then
            '    Dim drs As DataRow() = dtITP.Select(String.Format("PartNO= '{0}' and org ='{1}' and Currency='{2}'", PARTNO, ORG, Currency))
            '    If drs.Length > 0 Then
            '        If drs(0).Item("ITP") IsNot Nothing AndAlso Decimal.TryParse(drs(0).Item("ITP"), 0) Then
            '            ITP = Decimal.Parse(drs(0).Item("ITP"))
            '            InsertCache = False
            '        End If
            '    End If
            'End If
            listITP = CType(HttpContext.Current.Cache("PartNO_ITP"), List(Of ITPDataEntity))
            If listITP.Count > 0 Then
                Dim _itps = From ITPs In listITP
                            Where ITPs.PartNO = PARTNO And ITPs.ORG = ORG And ITPs.Currency = Currency
                            Select ITPs

                If _itps.Count > 0 AndAlso _itps(0) IsNot Nothing Then
                    ITP = _itps(0).ITP
                    InsertCache = False
                End If
            End If
        End If
        Dim sapdal As New SAPDAL
        If type = itpType.EU Then
            ' Reserve current eQuotation (GP checking function) back to use UUAAESC in EU10. 
            ' Don 't use and simulate EHLA002*1.045 as EU RBU ITP. 
            'If ITP = 0 AndAlso (Not isEUITPFromUUAAESC(PARTNO)) Then
            '    Dim sapITPDt As New DataTable
            '    If sapdal.getSAPPriceByTable(PARTNO, "TW01", "EHLA002", sapITPDt) = 1 Then
            '        If sapITPDt.Rows.Count > 0 AndAlso CDbl(sapITPDt.Rows(0).Item("Netwr")) > 0 Then
            '            ITP = CDbl(sapITPDt.Rows(0).Item("Netwr").ToString.Replace(",", ""))
            '            ITP = ITP * 1.045
            '            ITP = FormatNumber(ITP, 2)
            '            'writeItpBack(ORG, PARTNO, c, ITP)
            '            'Catch ex As Exception
            '            'End Try
            '        End If
            '    End If
            'End If
            If ITP = 0 Then
                Dim sapITPDt As New DataTable, TempCurrency As String = String.Empty
                TempCurrency = Currency
                If String.Equals(Currency, "GBP", StringComparison.CurrentCultureIgnoreCase) Then
                    TempCurrency = "EUR"
                End If
                If sapdal.getSAPPriceByTable(PARTNO, "EU10", "UUAAESC", "UUAAESC", TempCurrency, "", sapITPDt) = 1 Then
                    If sapITPDt.Rows.Count > 0 Then
                        If CDbl(sapITPDt.Rows(0).Item("Netwr")) > 0 Then
                            ITP = CDbl(sapITPDt.Rows(0).Item("Netwr").ToString.Replace(",", ""))
                            ITP = FormatNumber(ITP, 2)
                        End If
                    End If
                End If
                If CDec(ITP) > 0 AndAlso String.Equals(Currency, "GBP", StringComparison.CurrentCultureIgnoreCase) Then
                    ITP = FormatNumber(ITP * CType(get_exchangerate("EUR", Currency).ToString, Decimal), 2)
                End If
                If ITP = 0 AndAlso Currency = "USD" Then
                    sapITPDt = Nothing
                    If sapdal.getSAPPriceByTable(PARTNO, "EU10", "UUAAESC", "UUAAESC", "EUR", "", sapITPDt) = 1 Then
                        If sapITPDt.Rows.Count > 0 Then
                            If CDbl(sapITPDt.Rows(0).Item("Netwr")) > 0 Then
                                ITP = CDbl(sapITPDt.Rows(0).Item("Netwr").ToString.Replace(",", ""))
                                ITP = FormatNumber(ITP, 2)
                            End If
                        End If
                    End If
                    ITP = FormatNumber(ITP * CType(get_exchangerate("EUR", Currency).ToString, Decimal), 2)
                End If
            End If
        ElseIf type = itpType.CN Then

            ' First get ACN ITP with ADVACN/TW01
            Dim sapITPDt As New DataTable
            If sapdal.getSAPPriceByTable(PARTNO, "TW01", "ADVACN", "ADVACN", Currency, "", sapITPDt) = 1 Then
                If sapITPDt.Rows.Count > 0 Then
                    If CDbl(sapITPDt.Rows(0).Item("Netwr")) > 0 Then
                        ITP = CDbl(sapITPDt.Rows(0).Item("Netwr").ToString.Replace(",", ""))
                        ITP = FormatNumber(ITP, 2)
                    End If
                End If
            End If

            ' Second get ACN ITP with its ERPID's VPRS if ITP is zero
            If ITP = 0 Then
                sapITPDt = New DataTable
                If sapdal.getSAPPriceByTable(PARTNO, ORG, companyId, companyId, Currency, "", sapITPDt) = 1 Then
                    If CDbl(sapITPDt.Rows(0).Item("VPRS")) > 0 Then
                        ITP = CDbl(sapITPDt.Rows(0).Item("VPRS").ToString.Replace(",", ""))
                        ITP = FormatNumber(ITP, 2)
                    End If
                End If
            End If
        ElseIf type = itpType.KR Then
            Dim sapITPDt As New DataTable
            'Ryan 20170727 For temporary use - if has no ERPID, take AKRCE0416 first
            If String.IsNullOrEmpty(companyId) Then
                companyId = "AKRC00485"
            End If

            If sapdal.getSAPPriceByTable(PARTNO, ORG, companyId, companyId, Currency, "", sapITPDt) = 1 Then
                If CDbl(sapITPDt.Rows(0).Item("VPRS")) > 0 Then
                    ITP = CDbl(sapITPDt.Rows(0).Item("VPRS").ToString.Replace(",", ""))
                    ITP = FormatNumber(ITP, 2)
                End If
            End If
        ElseIf type = itpType.JP Then
            If ITP = 0 Then
                Dim sapITPDt As New DataTable

                If PARTNO.ToUpper.StartsWith("XAJP") OrElse PARTNO.ToUpper.StartsWith("968T") Then
                    Dim decimalvalue As Decimal = 0
                    'TC has confirmed with YC that below code only can get lable's price
                    Dim str As String = "select Round(a.netpr/a.peinh*100,2) as itp from saprdp.ekpo a inner join saprdp.ekbe b on a.ebeln=b.ebeln and a.ebelp=b.ebelp " +
                                        " where a.mandt='168' and b.mandt='168' And b.matnr='" + PARTNO + "' and b.werks='JPH1' and b.waers='JPY' And rownum=1 order by b.BUDAT desc"
                    Dim obj As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
                    If Not obj Is Nothing AndAlso Decimal.TryParse(obj.ToString, decimalvalue) Then
                        ITP = decimalvalue

                        If Currency = "USD" Then
                            ITP = FormatNumber(ITP * CType(get_exchangerate("JPY", "USD").ToString, Decimal), 2)
                        End If
                    End If

                    'Also needs to add Royalty fee for 968T material

                    'Dear Frank
                    '以下Rate AJP財務毎週都会Input一次,因此由MS進貨的OS ITP計算方式，請幇我使用以下計算方法
                    '例）USD 120*最新Rate= 111.033=13,323.96 + ZRMM32 貼紙ITP  ￥130円＝13,453.96*最後再乗上1.03 =13,857.57
                    'Rgs, YC
                    If PARTNO.ToUpper.StartsWith("968T") Then
                        Dim _str1 As New System.Text.StringBuilder
                        _str1.AppendLine(" select a.kschl as condition_type, a.EKORG as org_id, a.knumh,  a.lifnr as vendor_id ")
                        _str1.AppendLine(" ,a.matnr as part_no, b.datab as valid_from_date, b.datbi as valid_to_date ")
                        _str1.AppendLine(" ,c.kbetr as condition_amount_percent, c.KPEIN as condition_price_unit, c.KMEIN as condition_unit, c.konwa ")
                        _str1.AppendLine(" from saprdp.a904 a inner join saprdp.konh b on a.knumh=b.knumh and a.kschl=b.kschl ")
                        _str1.AppendLine(" inner join saprdp.konp c on b.knumh=c.knumh and a.kschl=c.kschl ")
                        _str1.AppendLine(" where a.mandt='168' and b.mandt='168' and c.mandt='168' and a.kschl='ZFR2' and a.EKORG='JP01' ")
                        _str1.AppendLine(" and a.matnr ='" + PARTNO + "' and c.konwa='USD' ")
                        _str1.AppendLine(" order by b.datab desc ")

                        Dim _dt1 As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", _str1.ToString)
                        If Not _dt1 Is Nothing AndAlso _dt1.Rows.Count > 0 Then
                            Dim _Royalty As Decimal = 0
                            If _dt1.Rows(0).Item("condition_price_unit") <> 0 Then
                                _Royalty = _dt1.Rows(0).Item("condition_amount_percent") / _dt1.Rows(0).Item("condition_price_unit")
                            Else
                                _Royalty = _dt1.Rows(0).Item("condition_amount_percent")
                            End If

                            ITP = ITP + FormatNumber(_Royalty * CType(get_exchangerate("USD", "JPY").ToString, Decimal), 2)

                            If Currency = "USD" Then
                                ITP = FormatNumber(ITP * CType(get_exchangerate("JPY", "USD").ToString, Decimal), 2)
                            End If
                        End If
                    End If
                    ITP = ITP * 1.03
                Else
                    If sapdal.getSAPPriceByTable(PARTNO, "TW01", "ADVAJP", "ADVAJP", Currency, "", sapITPDt) = 1 Then
                        If sapITPDt.Rows.Count > 0 Then
                            If CDbl(sapITPDt.Rows(0).Item("Netwr")) > 0 Then
                                ITP = CDbl(sapITPDt.Rows(0).Item("Netwr").ToString.Replace(",", ""))
                                ITP = FormatNumber(ITP, 2)
                                ITP = ITP * 1.03
                            ElseIf CDbl(sapITPDt.Rows(0).Item("ZMIP")) > 0 Then
                                ITP = CDbl(sapITPDt.Rows(0).Item("ZMIP").ToString.Replace(",", ""))
                                ITP = FormatNumber(ITP, 2)
                                ITP = ITP * 1.03
                            End If
                        End If
                    End If
                End If

                If ITP = 0 Then
                    sapITPDt = New DataTable
                    If sapdal.getSAPPriceByTable(PARTNO, ORG, companyId, companyId, Currency, "", sapITPDt) = 1 Then
                        If CDbl(sapITPDt.Rows(0).Item("VPRS")) > 0 Then
                            ITP = CDbl(sapITPDt.Rows(0).Item("VPRS").ToString.Replace(",", ""))
                            ITP = FormatNumber(ITP, 2)
                        End If
                    End If
                End If
            End If
        End If
        If InsertCache AndAlso ITP > 0 Then

            'Dim dr As DataRow = dtITP.NewRow
            'dr.Item("PartNO") = PARTNO : dr.Item("ORG") = ORG
            'dr.Item("Currency") = Currency : dr.Item("ITP") = ITP
            'dtITP.Rows.Add(dr) : dtITP.AcceptChanges()
            Dim _itpEntity As New ITPDataEntity
            _itpEntity.PartNO = PARTNO
            _itpEntity.ORG = ORG
            _itpEntity.Currency = Currency
            _itpEntity.ITP = ITP

            SyncLock listITP
                Dim _CheckInsertitps = From ITPs In listITP
                                       Where ITPs.PartNO = PARTNO And ITPs.ORG = ORG And ITPs.Currency = Currency
                                       Select ITPs

                If _CheckInsertitps Is Nothing OrElse _CheckInsertitps.Count = 0 Then
                    listITP.Add(_itpEntity)
                End If

            End SyncLock

        End If

        Return ITP

    End Function
    Shared Function get_exchangerate(ByVal C_FROM As String, ByVal C_TO As String) As String
        If C_FROM = C_TO Then
            Return 1
        End If
        Dim temp As Object = Nothing
        temp = dbUtil.dbExecuteScalar("MY", "select top 1 UKURS from SAP_EXCHANGERATE" &
                                                     " where fCURR='" & C_FROM & "' and TCURR='" & C_TO & "' and EXCH_DATE<=GETDATE() order by exch_date desc")

        If temp IsNot Nothing AndAlso temp.ToString <> "" Then
            Return temp
        End If
        Return "0.0"
    End Function

    Shared Sub writeItpBack(ByVal ORG As String, ByVal PARTNO As String, ByVal CURR As String, ByVal itp As Decimal)
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        Dim cmd As New SqlClient.SqlCommand(
            " delete from PRODUCT_ITP where PART_NO=@PN and ORG=@ORG and CURRENCY=@CURR;" +
            " insert into PRODUCT_ITP (ORG,PART_NO,ITP,CURRENCY) values(@ORG,@PN,@ITP,@CURR)", conn)
        With cmd.Parameters
            .AddWithValue("PN", PARTNO) : .AddWithValue("ORG", ORG) : .AddWithValue("ITP", itp) : .AddWithValue("CURR", CURR)
        End With
        ' Try
        conn.Open() : cmd.ExecuteNonQuery() : conn.Close()
    End Sub
    Public Shared Function getProdHrck(ByVal partNo As String) As String
        Dim str As String = String.Format("select top 1 PRODUCT_HIERARCHY from sap_product where part_no='{0}'", partNo)
        Dim o As New Object
        o = dbUtil.dbExecuteScalar("MY", str)
        If Not IsNothing(o) Then
            Return o
        End If
        Return ""
    End Function
    Public Shared Function isEUITPFromUUAAESC(ByVal partNo As String) As Boolean
        Dim str As String = String.Format("select count(Part_No) from sap_product where part_no='{0}' and (PRODUCT_HIERARCHY  " &
        " like 'OTHR-%' or PRODUCT_HIERARCHY ='EAPC-INNO-DPX' or PRODUCT_HIERARCHY ='EAPC-DLOG-DLGR' or PRODUCT_HIERARCHY like 'AGSG-PAPS-%' or PRODUCT_HIERARCHY='AGSG-CTOS-ASS#')", partNo)

        'Frank 2013/12/11: Below is the sql condition wrote on EDOC. If above original sql has problem, please refer to this statement.
        ' Dim str As String = String.Format("select count(Part_No) from sap_product where part_no='{0}' and (PRODUCT_HIERARCHY ='EAPC-INNO-DPX' or PRODUCT_HIERARCHY like 'OTHR-MEMO-%' " & _
        '" or PRODUCT_HIERARCHY like 'AGSG-PAPS-%' or PRODUCT_HIERARCHY='AGSG-CTOS-ASS#' or PRODUCT_HIERARCHY ='EAPC-DLOG-DLGR')", partNo)

        Dim o As New Object
        o = dbUtil.dbExecuteScalar("MY", str)
        If Not IsNothing(o) AndAlso CInt(o) > 0 Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function getUSZipcodeByShipToID(ByVal strShipToId As String) As String
        Dim txtTempZipCode As String = String.Empty
        If Not String.IsNullOrEmpty(strShipToId) Then getUSZipcodeByERPID(strShipToId, txtTempZipCode)
        Return txtTempZipCode
    End Function

    Public Shared Function getUSZipcodeByERPID(ByVal ERPID As String, ByRef ZipCode As String) As Boolean
        Dim objZip As Object = Nothing
        'Dim myConn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        'Dim cmd As New SqlClient.SqlCommand("select top 1 ZIP_CODE from SAP_DIMCOMPANY where COMPANY_ID=@CID and country='US' ", myConn)
        'cmd.Parameters.AddWithValue("CID", ERPID)
        'cmd.Connection.Open()
        'objZip = cmd.ExecuteScalar()
        'cmd.Connection.Close()
        If objZip IsNot Nothing Then
            ZipCode = objZip.ToString() : Return True
        Else
            Dim strPlSql As String =
                " select nvl(b.post_code1,'') as post_code1 from saprdp.kna1 a inner join saprdp.adrc b on a.land1=b.country and a.adrnr=b.addrnumber " +
                " where a.mandt='168' and a.kunnr='" + Replace(UCase(ERPID), "'", "''") + "' and rownum=1 and a.land1='US' "
            Dim oraDt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strPlSql)
            If oraDt.Rows.Count = 1 Then
                ZipCode = oraDt.Rows(0).Item("post_code1") : Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function isTaxExempt(ByVal CompanyId As String) As Boolean
        Dim STR As String = String.Format("select COUNT(A.KUNNR) from SAPRDP.KNVI A INNER JOIN SAPRDP.TSKDT B " &
                                            " on A.TATYP=B.TATYP AND A.TAXKD=B.TAXKD WHERE  " &
                                            " A.MANDT='168' AND B.MANDT='168'  " &
                                            " AND B.SPRAS='E' AND A.ALAND='US' AND A.KUNNR='{0}' AND A.TATYP='UTXJ' And  A.TAXKD = '0'", CompanyId)
        Dim O As New Object
        O = OraDbUtil.dbExecuteScalar("SAP_PRD", STR)
        If IsNumeric(O) AndAlso CInt(O) > 0 Then
            Return True
        End If
        Return False
    End Function
    Shared Function getSalesTaxByZIP(ByVal txtTempZipCode As String) As Decimal

        Dim decTaxPercentage As Decimal = 0

        Dim _SalesTax As Dictionary(Of String, String) = CType(HttpContext.Current.Cache("SalesTax"), Dictionary(Of String, String))
        If _SalesTax Is Nothing Then
            _SalesTax = New Dictionary(Of String, String)
            HttpContext.Current.Cache.Add("SalesTax", _SalesTax, Nothing, DateTime.Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If

        If Not _SalesTax.ContainsKey(txtTempZipCode) Then

            Dim ws As New agsWS.USTaxService
            ws.getSalesTaxByZIP(Left(txtTempZipCode, 5), decTaxPercentage)
            _SalesTax.Add(txtTempZipCode, decTaxPercentage)
            Return decTaxPercentage
        End If

        Double.TryParse(_SalesTax.Item(txtTempZipCode), decTaxPercentage)
        Return decTaxPercentage

    End Function

    Public Shared Function isTaxable(ByVal PartNo As String, ByVal ShipToId As String) As Boolean

        Dim ConCode As String = getCountryCodebyShipTo(ShipToId)
        Dim str As String = String.Format("select count(Part_No) FROM SAP_PRODUCT_TAXEXEMPT where Part_NO='{0}' AND Country_Code='{1}'", PartNo.ToUpper, ConCode.ToUpper)
        Dim O As New Object
        O = dbUtil.dbExecuteScalar("MY", str)
        If IsNumeric(O) AndAlso CInt(O) > 0 Then
            Return False
        End If
        Return True
    End Function
    Shared Function GetCTOSAssemblyInstructionListByERPIdFromSAP(ByVal _ERPID As String) As DataTable

        Dim strSql As String =
            "SELECT a.DOKNR, a.DOKVR, a.LANGU, a.DKTXT, a.DKTXT_UC, b.FILEP, b.DAPPL" +
            " FROM SAPRDP.DRAT a inner join SAPRDP.DRAW b on a.DOKNR=b.DOKNR and a.DOKVR=b.DOKVR" +
            " WHERE a.DOKNR LIKE '" + _ERPID + "%'" +
            " AND a.MANDT='168' AND a.DOKAR='CTO' AND b.MANDT='168' AND b.DOKAR='CTO'" +
            " AND a.DOKVR<>'00'" +
            " Order by a.DOKNR,a.DOKVR desc"

        Return OraDbUtil.dbGetDataTable("SAP_PRD", strSql)

    End Function

    Shared Function GetCTOSAssemblyInstructionListByERPIdFromMyadvantech(ByVal _ERPID As String, ByVal DocTxt As String) As DataTable
        Dim strSql As String =
            "SELECT a.DOKNR, a.DOKVR, a.LANGU, a.DKTXT, a.DKTXT_UC, b.FILEP, b.DAPPL" +
            " FROM SAP_CTOS_DOC a inner join SAP_CTOS_DOC_URL b on a.DOKNR=b.DOKNR and a.DOKVR=b.DOKVR" +
            " WHERE 1=1 "
        'If Not String.IsNullOrEmpty(_ERPID) Then strSql += " and a.DOKNR LIKE '" + _ERPID + "%'"
        If Not String.IsNullOrEmpty(_ERPID) Then strSql += " and a.COMPANY_ID = '" + _ERPID + "'"
        If Not String.IsNullOrEmpty(DocTxt) Then strSql += " and a.DKTXT LIKE N'%" + Replace(Replace(DocTxt, "'", "''"), "*", "%") + "%'"
        'Nada confirmed with Frank and removed criteria 'a.DOKVR<>'00''
        'strSql += " AND a.DOKVR<>'00' Order by a.DOKNR,a.DOKVR desc"
        strSql += " Order by a.DOKNR,a.DOKVR desc"
        Return dbUtil.dbGetDataTable("MY", strSql)

    End Function

    'Public Shared Function GetCTOSAssemblyInstructionListByERPIdFromMyadvantech1(ByVal _ERPID As String) As String
    '    Dim strSql As String = _
    '        " SELECT top 1  a.DOKNR + '****' + b.FILEP" + _
    '        " FROM SAP_CTOS_DOC a inner join SAP_CTOS_DOC_URL b on a.DOKNR=b.DOKNR and a.DOKVR=b.DOKVR" + _
    '        " WHERE 1=1 "
    '    If Not String.IsNullOrEmpty(_ERPID) Then strSql += " and a.DOKNR LIKE '" + _ERPID + "%'"
    '    'If Not String.IsNullOrEmpty(DocTxt) Then strSql += " and a.DKTXT LIKE N'%" + Replace(Replace(DocTxt, "'", "''"), "*", "%") + "%'"
    '    strSql += " AND a.DOKVR<>'00' Order by a.DOKNR,a.DOKVR desc"
    '    Dim O As Object = dbUtil.dbExecuteScalar("MY", strSql)
    '    If Not IsNothing(O) Then
    '        Return O.ToString.Trim
    '    End If
    '    Return ""
    'End Function

    Public Shared Function getSalesNotebyERPid(ByVal _ERPID As String) As String
        Dim TXTObj As Object = dbUtil.dbExecuteScalar("MY", "select TXT from SAP_COMPANY_SALESNOTE WHERE COMPANY_ID='" + _ERPID + "'")
        If TXTObj IsNot Nothing AndAlso TXTObj.ToString <> "" Then
            Return TXTObj.ToString
        End If
        Return ""
    End Function
    Public Shared Function GetBillToNotSoldTo(ByVal SoldTo As String, ByVal ORGID As String) As String
        If String.IsNullOrEmpty(SoldTo) Then Return ""
        Dim dt As New DataTable
        Dim sb As New System.Text.StringBuilder
        With sb

            .AppendLine("SELECT company_id FROM  ")
            .AppendLine("(SELECT A.KUNN2 AS company_id FROM saprdp.knvp A  ")
            .AppendFormat("where (A.Kunnr = '{0}')", UCase(SoldTo.Replace("'", "''").Trim))
            .AppendLine("AND A.PARVW ='RE' ")
            .AppendFormat(" AND A.VKORG = '{0}' ", ORGID)
            .AppendLine("AND (select B.ktokd from saprdp.kna1 B where B.KUNNR = A.KUNN2) in ('Z001','Z003') ORDER BY A.Kunn2) WHERE ROWNUM=1 ")

            'Nada  marked for performance issue and noticed the logic of this function
            'is very strange, will always return slodto as billto

            '.AppendLine(" SELECT A.KUNN2 AS company_id ")
            '.AppendLine(" FROM saprdp.knvp A  ")
            '.AppendLine(" INNER JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR ")
            '.AppendLine(" where rownum=1 ")
            '.AppendFormat(" AND (A.Kunnr = '{0}') ", UCase(SoldTo.Replace("'", "''").Trim))
            ''.AppendFormat(" and (A.KUNN2 <> '{0}')  ", UCase(SoldTo.Replace("'", "''").Trim))
            '.Append(" AND A.PARVW ='RE' ")
            '.AppendFormat(" AND B.ktokd in ('Z001','Z003')")
            '.AppendFormat(" ORDER BY A.Kunn2 ")
        End With
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("company_id").ToString.Trim
        End If
        Return ""
    End Function
    Public Shared Function IsInShiptoList(ByVal Shipto As String, ByVal SoldTo As String) As Boolean
        If String.IsNullOrEmpty(SoldTo) OrElse String.IsNullOrEmpty(Shipto) Then Return False
        Dim dt As New DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(" SELECT A.KUNN2 AS company_id ")
            .AppendLine(" FROM saprdp.knvp A  ")
            .AppendLine(" INNER JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR ")
            .AppendFormat(" where  A.KUNN2='{0}' ", Shipto)
            .AppendFormat(" AND (A.Kunnr = '{0}') ", UCase(SoldTo.Replace("'", "''").Trim))
            '.AppendFormat(" and (A.KUNN2 <> '{0}')  ", UCase(SoldTo.Replace("'", "''").Trim))
            .Append(" AND A.PARVW ='WE' ")
            .AppendFormat(" AND B.ktokd in ('Z001','Z002')")
            .AppendFormat(" ORDER BY A.Kunn2 ")
        End With
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                If String.Equals(dr.Item("company_id").ToString.Trim, Shipto) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function


    Shared Function replaceCartBTO(ByVal PN As String, ByVal Org As String) As String

        'Ryan 20170621 Bypass JP01 for BTO replacing
        If Not String.IsNullOrEmpty(Org) AndAlso Org.ToUpper.Equals("JP01") Then
            Return PN
        End If

        If Not String.IsNullOrEmpty(PN) AndAlso PN.Length > 3 Then
            If String.Equals(Org, "US01") AndAlso PN Like "ARK-*-BTO" Then Return "ARK-BTO"
            If PN.StartsWith("EZ-") Then PN = PN.Substring(3, PN.Length - 3)
            Dim vnumber As Object = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PN.ToString, Org.ToUpper.Substring(0, 2)))
            If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
                Dim MaterialGroup As Object = dbUtil.dbExecuteScalar("MY", String.Format("select TOP 1 MATERIAL_GROUP from SAP_PRODUCT where PART_NO = '{0}' ", vnumber.ToString))
                If Not IsNothing(MaterialGroup) AndAlso MaterialGroup.ToString.Equals("BTOS", StringComparison.OrdinalIgnoreCase) Then
                    Return vnumber
                End If
            End If
            vnumber = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PN.ToString.Trim + "-BTO", Org.ToUpper.Substring(0, 2)))
            If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
                Dim MaterialGroup As Object = dbUtil.dbExecuteScalar("MY", String.Format("select TOP 1 MATERIAL_GROUP from SAP_PRODUCT where PART_NO = '{0}' ", vnumber.ToString))
                If Not IsNothing(MaterialGroup) AndAlso MaterialGroup.ToString.Equals("BTOS", StringComparison.OrdinalIgnoreCase) Then
                    Return vnumber
                End If
            End If
            If PN.Trim.EndsWith("-BTO") Then
                Dim Temp_PN = PN.Substring(0, PN.Length - 4)
                vnumber = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", Temp_PN, Org.ToUpper.Substring(0, 2)))
                If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
                    Dim MaterialGroup As Object = dbUtil.dbExecuteScalar("MY", String.Format("select TOP 1 MATERIAL_GROUP from SAP_PRODUCT where PART_NO = '{0}' ", vnumber.ToString))
                    If Not IsNothing(MaterialGroup) AndAlso MaterialGroup.ToString.Equals("BTOS", StringComparison.OrdinalIgnoreCase) Then
                        Return vnumber
                    End If
                End If
            Else
                Return PN
            End If

        End If
        Return PN
    End Function

    Shared Function IsBlankPartStatus(ByVal PartNo As String, ByVal SalesOrg As String, ByRef StatusCode As String, ByRef StatusDesc As String) As Boolean

        Dim strSql As String = String.Empty, dtProdStatus As DataTable = Nothing

        'Frank 2012/10/04: After confirming with Jay, if part status is blank then return status with A
        If (PartNo.StartsWith("X", StringComparison.InvariantCultureIgnoreCase) OrElse
        PartNo.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase)) Then
            strSql =
            " select a.MMSTA as status_code,'' as status_desc" +
            " from saprdp.MARC a " +
            " where a.mandt='168' and a.werks='USH1'" +
            " and a.matnr='" + PartNo + "' and rownum=1"
        Else
            strSql =
            " select a.vmsta as status_code, '' as status_desc" +
            " from saprdp.MVKE a " +
            " where a.mandt='168' " +
            " and a.vkorg='" + SalesOrg + "' and a.matnr='" + PartNo + "' and rownum=1"
        End If
        dtProdStatus = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
        If dtProdStatus.Rows.Count > 0 AndAlso Trim(dtProdStatus.Rows(0).Item("status_code").ToString) = "" Then
            StatusCode = "A"
            StatusDesc = "M/P(B2B)"
            Return True
        End If
        StatusCode = ""
        StatusDesc = ""
        Return False

    End Function

    Shared Function isInvalidPhaseOutV2(ByVal PartNo As String, ByVal SalesOrg As String, ByRef StatusCode As String, ByRef StatusDesc As String, ByRef ATPQty As Decimal, Optional ByVal IsSyncToLocalDB As Boolean = True, Optional ByVal ItemType As Integer = 0) As Boolean
        SalesOrg = Trim(UCase(SalesOrg)) : PartNo = Trim(UCase(PartNo))
        If PartNo.StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase) OrElse PartNo.ToUpper.EndsWith("-BTO", StringComparison.CurrentCultureIgnoreCase) Then Return False
        If CInt(ItemType) = -1 Then
            PartNo = replaceCartBTO(PartNo, SalesOrg)
        End If

        If IsSyncToLocalDB Then SyncSAPProductStatusToMyadvanGlobal(PartNo, SalesOrg)

        Dim PNstatus As Object = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 ISNULL(PRODUCT_STATUS,'') AS ProductStatus from dbo.SAP_PRODUCT_STATUS_ORDERABLE where PART_NO='{0}' and SALES_ORG='{1}'", PartNo, SalesOrg))
        If PNstatus IsNot Nothing AndAlso Not String.IsNullOrEmpty(PNstatus) Then
            StatusCode = PNstatus
            Return False
        End If
        'Frank 2012/08/22:Do not check extended warranty partno
        PartNo = Format2SAPItem(PartNo)
        Dim strSql As String = String.Empty
        'Frank 2012/08/24:If part no start with X or Y and org is US01, then check product status from field "saprdp.MARC.MMSTA"
        If SalesOrg.Equals("US01", StringComparison.InvariantCultureIgnoreCase) AndAlso (
            PartNo.StartsWith("X", StringComparison.InvariantCultureIgnoreCase) OrElse
            PartNo.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase)) Then
            'c.spras='E' means to get English version product status description.
            strSql =
            " select a.MMSTA as status_code, c.vmstb as status_desc" +
            " from saprdp.MARC a left join saprdp.TVMST C on a.MMSTA=c.vmsta" +
            " where a.mandt='168' and a.werks='USH1'" +
            " and a.matnr='" + PartNo + "' and c.spras='E' and rownum=1"
        Else
            strSql =
            " select a.vmsta as status_code, b.vmstb as status_desc" +
            " from saprdp.MVKE a left join saprdp.TVMST b on a.vmsta=b.vmsta" +
            " where a.mandt='168' " +
            " and b.mandt='168' and a.vkorg='" + SalesOrg + "' and a.matnr='" + PartNo + "' and b.spras='E' and rownum=1"
        End If

        Dim dtProdStatus As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
        If dtProdStatus.Rows.Count > 0 Then
            'Frank 2012/08/22:is Sync real time product data from sap to myadvanglobal
            'Put below code here because above codes will add 0 begin with part no if have to do
            'If IsSyncToLocalDB Then SAPDAL.SyncSAPProductStatusToMyadvanGlobal(PartNo, SalesOrg)
            StatusCode = dtProdStatus.Rows(0).Item("status_code") : StatusDesc = dtProdStatus.Rows(0).Item("status_desc")
            Select Case StatusCode
                'Case "A", "N", "H", "M1", "T"
                Case "A", "C", "N", "H", "M1", "P", "S2", "S5", "V", "T"
                    Return False
                Case "O", "S"
                    Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP, intInventory As Integer = -1
                    Dim atpTb As New GET_MATERIAL_ATP.BAPIWMDVETable, retTb As New GET_MATERIAL_ATP.BAPIWMDVSTable, rOfretTb As New GET_MATERIAL_ATP.BAPIWMDVS
                    rOfretTb.Req_Date = Now.ToString("yyyyMMdd") : rOfretTb.Req_Qty = 999 : retTb.Add(rOfretTb)
                    p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
                    p1.Connection.Open()
                    p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", PartNo, Left(SalesOrg, 2) + "H1", "", "", "", "", "PC",
                                           "", intInventory, "", "", New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)
                    p1.Connection.Close()
                    For i As Integer = 0 To atpTb.Count - 1
                        If atpTb(i).Com_Qty > 0 Then
                            ATPQty = atpTb(i).Com_Qty : Return False
                        End If
                    Next
                    ATPQty = 0 : Return True
                Case "I"
                    Return True
                Case Else
                    Return True
            End Select
        Else
            StatusCode = "" : StatusDesc = "" : Return True
        End If
    End Function

    Class Prodstatus
        Private _pn As String = ""
        Private _LastSyncTimeStamp As DateTime = Now
        Public Property pn As String
            Get
                Return _pn
            End Get
            Set(ByVal value As String)
                _pn = value
            End Set
        End Property
        Public Property LastSyncTimeStamp As DateTime
            Get
                Return _LastSyncTimeStamp
            End Get
            Set(ByVal value As DateTime)
                _LastSyncTimeStamp = value
            End Set
        End Property
    End Class
    Public Shared Function IsNumericItem(ByVal part_no As String) As Boolean

        Dim pChar() As Char = part_no.ToCharArray()

        For i As Integer = 0 To pChar.Length - 1
            If Not IsNumeric(pChar(i)) Then
                Return False
                Exit Function
            End If
        Next

        Return True
    End Function

    <Obsolete("Please use Global_Inc.Format2SAPItem to transfer digit part number format")>
    Public Shared Function Format2SAPItem(ByVal Part_No As String) As String
        'Frank 2014/05/14
        'To uniform the function call, "Global_Inc.Format2SAPItem" is the only way to transfer part number as SAP format
        Return Global_Inc.Format2SAPItem(Part_No)

        'Dim _result As Long = 0
        'If Long.TryParse(Part_No, _result) Then
        '    Part_No = Part_No.PadLeft(18, "0")
        'End If

        'Return Part_No

        'Try
        '    If IsNumericItem(Part_No) AndAlso Not Part_No.Substring(0, 1).Equals("0") Then
        '        Dim zeroLength As Integer = 18 - Part_No.Length
        '        For i As Integer = 0 To zeroLength - 1
        '            Part_No = "0" & Part_No
        '        Next
        '        Return Part_No
        '    Else
        '        Return Part_No
        '    End If
        'Catch ex As Exception
        '    Return Part_No
        'End Try

    End Function
    Public Shared Function isProductChangedOnSAP(ByVal PN As String, ByVal LastSyncTimeStamp As DateTime) As Boolean
        PN = Format2SAPItem(PN)
        Dim odate As String = ""
        odate = LastSyncTimeStamp.ToString("yyyyMMdd")
        Dim otime As String = ""
        otime = LastSyncTimeStamp.ToString("HHmmss")
        Dim str As String = String.Format(" SELECT count(a.mandant) " &
                                           " FROM saprdp.CDHDR a  " &
                                           " WHERE a.mandant='168' and a.OBJECTCLAS = 'MATERIAL' and  " &
                                           " (a.UDATE>'" & odate & "' or (a.UDATE='" & odate & "' and a.UTIME>'" & otime & "')) " &
                                           " and (a.TCODE = 'MM01' or a.TCODE = 'MM02' or a.TCODE = 'MM01(BAPI)' or a.TCODE = 'MM02(BAPI)') and a.OBJECTID='" & PN & "'")
        Dim o As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
        If Integer.TryParse(o, 0) AndAlso CInt(o) > 0 Then
            Return True
        End If
        Return False
    End Function
    Public Shared Sub SyncSAPProductStatusToMyadvanGlobal(ByVal pn As String, ByVal ORG As String)
        Dim orgPrefix As String = Left(ORG, 2)
        Dim prodCacheList As List(Of Prodstatus) = CType(HttpContext.Current.Cache("SyncSAPProduct"), List(Of Prodstatus))
        If IsNothing(prodCacheList) Then
            prodCacheList = New List(Of Prodstatus)
            HttpContext.Current.Cache.Add("SyncSAPProduct", prodCacheList, Nothing, DateTime.Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If
        Dim temp As Prodstatus = prodCacheList.Where(Function(x) String.Equals(x.pn, pn, StringComparison.OrdinalIgnoreCase)).FirstOrDefault
        If Not IsNothing(temp) Then
            If Not DateDiff(DateInterval.Minute, temp.LastSyncTimeStamp, Now) < 60 Then
                If isProductChangedOnSAP(pn, temp.LastSyncTimeStamp) Then
                    'Dim ps As New syncSingleProduct
                    Dim em As String = ""
                    Dim PNA As New ArrayList : PNA.Add(pn)
                    syncSingleProduct.syncSAPProduct(PNA, "ALL", False, em)
                    If em = "" Then
                        Dim t As New Prodstatus
                        t.pn = pn
                        t.LastSyncTimeStamp = Now
                        prodCacheList.RemoveAll(Function(x) String.Equals(x.pn, pn, StringComparison.OrdinalIgnoreCase))
                        prodCacheList.Add(t)
                    End If
                End If
            End If
        Else
            'Dim ps As New syncSingleProduct
            Dim em As String = ""
            Dim PNA As New ArrayList : PNA.Add(pn)
            syncSingleProduct.syncSAPProduct(PNA, "ALL", False, em)
            If em = "" Then
                Dim t As New Prodstatus
                t.pn = pn
                t.LastSyncTimeStamp = Now
                prodCacheList.RemoveAll(Function(x) String.Equals(x.pn, pn, StringComparison.OrdinalIgnoreCase))
                prodCacheList.Add(t)
            End If

        End If

        ''Frank 2012/10/15 If Product has been updated to MyA, this sync record will be kept in cache. So the same part number will not be updated to MyA if the cache is alive. 
        'Dim _SyncSAPProduct As Dictionary(Of String, String) = CType(HttpContext.Current.Cache("SyncSAPProduct"), Dictionary(Of String, String))
        'If _SyncSAPProduct Is Nothing Then
        '    _SyncSAPProduct = New Dictionary(Of String, String)
        '    'Cache alive time is 1 hour.
        '    HttpContext.Current.Cache.Add("SyncSAPProduct", _SyncSAPProduct, Nothing, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        'End If
        'If _SyncSAPProduct.ContainsKey(ORG & pn) Then Exit Sub


        'Dim DT As DataTable = GetRealTimeProductStatusFromSAP(pn, ORG)

        'If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then

        '    For i As Integer = 0 To DT.Rows.Count - 1
        '        If DT.Rows(i).Item("Part_no").ToString.StartsWith("0") Then
        '            For n As Integer = 1 To DT.Rows(i).Item("Part_no").ToString.Length - 1
        '                If DT.Rows(i).Item("Part_no").ToString.Substring(n, 1) <> "0" Then
        '                    DT.Rows(i).Item("Part_no") = DT.Rows(i).Item("Part_no").ToString.Substring(n) : Exit For
        '                End If
        '            Next
        '        End If
        '    Next

        '    'Clean data of SAP_PRODUCT_STATUS and SAP_PRODUCT_STATUS_ORDERABLE
        '    Dim Str As String = String.Format("DELETE FROM SAP_PRODUCT_STATUS WHERE PART_NO='{0}' AND sales_org LIKE '{1}%';DELETE FROM SAP_PRODUCT_STATUS_ORDERABLE WHERE PART_NO='{0}' AND sales_org LIKE '{1}%'", pn, ORG)
        '    dbUtil.dbExecuteNoQuery("MY", Str)

        '    Dim bk As New System.Data.SqlClient.SqlBulkCopy(ConfigurationManager.ConnectionStrings("B2B").ConnectionString)
        '    bk.DestinationTableName = "SAP_PRODUCT_STATUS"
        '    bk.WriteToServer(DT)

        '    If ORG.StartsWith("EU", StringComparison.InvariantCultureIgnoreCase) AndAlso DT.Rows(0).Item("product_status").ToString.ToUpper = "O" Then
        '        'Frank 2012/10/17: If part status is O and sales org is EU, then do not write this part into SAP_PRODUCT_STATUS_ORDERABLE
        '    Else
        '        bk.DestinationTableName = "SAP_PRODUCT_STATUS_ORDERABLE"
        '        bk.WriteToServer(DT)
        '    End If


        '    'Frank: To save the product sync event into cache after product has been updated to MyA db.
        '    _SyncSAPProduct.Add(ORG & pn, pn)

        'End If
    End Sub


    Public Shared Function GetRealTimeProductStatusFromSAP(ByVal pn As String, ByVal ORG As String) As DataTable

        If String.IsNullOrEmpty(pn) Then Return Nothing
        If String.IsNullOrEmpty(ORG) Then Return Nothing

        Dim ORGCondition As String = ""
        If ORG.ToUpper <> "ALL" Then
            ORGCondition = "and mvke.vkorg like '" & ORG & "%'"
        End If
        Dim str As String = String.Format("select matnr as part_no, vkorg as sales_org, vtweg as dist_channel, vmsta as product_status, " &
                                           " AUMNG as min_order_qty, LFMNG as min_dlv_qty, EFMNG as min_bto_qty, DWERK as dlv_plant, " &
                                           " KONDM as material_pricing_grp, vmstd as valid_date, to_char(mvke.mtpos) as item_category_group " &
                                           " from saprdp.MVKE " &
                                           " where mandt='168' and matnr='{0}' {1}", pn, ORGCondition)

        Return OraDbUtil.dbGetDataTable("SAP_PRD", str)

    End Function
#End Region

    Public Class isPNChangedInSAP
        Private _PN As String = ""
        Public Property PN As String
            Get
                Return _PN
            End Get
            Set(ByVal value As String)
                _PN = value
            End Set
        End Property
        Private _isChanged As Boolean = False
        Public Property isChanged As Boolean
            Get
                Return _isChanged
            End Get
            Set(ByVal value As Boolean)
                _isChanged = value
            End Set
        End Property
    End Class

    Public Shared Function CheckIsPNChangedInSAP(ByRef ProdList As List(Of isPNChangedInSAP), Optional ByVal PeriodByHours As Integer = 48) As Boolean
        Dim F As Boolean = False
        Dim ar As New ArrayList
        If Not IsNothing(ProdList) AndAlso ProdList.Count > 0 Then
            For Each pr As isPNChangedInSAP In ProdList
                ar.Add(Format2SAPItem(pr.PN).ToUpper)
            Next
        End If
        Dim partCond As String = ""
        If Not IsNothing(ar) AndAlso ar.Count > 0 Then
            partCond = " and a.OBJECTID in ('" & String.Join("','", ar.ToArray) & "')"
        End If

        Dim lastSyncTimeStamp As DateTime = Now.AddHours(CDbl("-" & PeriodByHours))
        Dim odate As String = ""
        odate = lastSyncTimeStamp.ToString("yyyyMMdd")
        Dim otime As String = ""
        otime = lastSyncTimeStamp.ToString("HHmmss")
        Dim str As String = String.Format(" SELECT distinct a.OBJECTID as PN " &
                                           " FROM saprdp.CDHDR a  " &
                                           " WHERE a.mandant='168' and a.OBJECTCLAS = 'MATERIAL' and  " &
                                           " (a.UDATE>'" & odate & "' or (a.UDATE='" & odate & "' and a.UTIME>'" & otime & "')) " &
                                           " and (a.TCODE = 'MM01' or a.TCODE = 'MM02' or a.TCODE = 'MM01(BAPI)' or a.TCODE = 'MM02(BAPI)') " & partCond)
        Dim o As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", str)
        If Not IsNothing(o) AndAlso o.Rows.Count > 0 Then
            For Each Pr As isPNChangedInSAP In ProdList
                If o.Select("PN='" & Pr.PN & "'").Count > 0 Then
                    Pr.isChanged = True
                    F = True
                End If
            Next
        End If
        Return F
    End Function

    Public Shared Function IsOrderPartnerAddressDataChanged(ByVal _OrderID, ByVal _ShiptoID, ByVal _PartnerType) As Boolean
        Dim dtShipto As DataTable = dbUtil.dbGetDataTable("MY", String.Format("SELECT * FROM ORDER_PARTNERS WHERE ORDER_ID = '{0}' AND TYPE = '{1}'", _OrderID, _PartnerType))
        Dim dtSAPAddress As SalesOrder.PartnerAddressesDataTable = GetSAPPartnerAddressesTableByKunnr(_ShiptoID)

        If Not dtShipto Is Nothing AndAlso dtShipto.Rows.Count > 0 AndAlso Not dtSAPAddress Is Nothing AndAlso dtSAPAddress.Rows.Count > 0 Then
            Dim rowShipto As DataRow = dtShipto.Rows(0)

            For Each rowSAPAddress As SalesOrder.PartnerAddressesRow In dtSAPAddress.Rows
                Dim isCurrentRowChanged As Boolean = False

                If rowShipto("NAME") IsNot Nothing AndAlso Not rowSAPAddress.Name.Trim().Equals(rowShipto("NAME").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("ATTENTION") IsNot Nothing AndAlso Not rowSAPAddress.C_O_Name.Trim().Equals(rowShipto("ATTENTION").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("TEL") IsNot Nothing AndAlso Not rowSAPAddress.Tel1_Numbr.Trim().Equals(rowShipto("TEL").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("STREET") IsNot Nothing AndAlso Not rowSAPAddress.Street.Trim().Equals(rowShipto("STREET").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("STREET2") IsNot Nothing AndAlso Not rowSAPAddress.Str_Suppl3.Trim().Equals(rowShipto("STREET2").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("CITY") IsNot Nothing AndAlso Not rowSAPAddress.City.Trim().Equals(rowShipto("CITY").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("STATE") IsNot Nothing AndAlso Not rowSAPAddress.Region_str.Trim().Equals(rowShipto("STATE").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("ZIPCODE") IsNot Nothing AndAlso Not rowSAPAddress.Postl_Cod1.Trim().Equals(rowShipto("ZIPCODE").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("COUNTRY") IsNot Nothing AndAlso Not rowSAPAddress.Country.Trim().Equals(rowShipto("COUNTRY").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("TAXJURI") IsNot Nothing AndAlso Not rowSAPAddress.Taxjurcode.Trim().Equals(rowShipto("TAXJURI").ToString.Trim) Then
                    isCurrentRowChanged = True
                ElseIf rowShipto("ADDRESS") IsNot Nothing AndAlso Not rowSAPAddress.E_Mail.Trim().Equals(rowShipto("ADDRESS").ToString.Trim) Then
                    isCurrentRowChanged = True
                End If

                If isCurrentRowChanged = False Then
                    Return False
                End If
            Next
            Return True
        End If
        Return False
    End Function

    Public Shared Function SelectSAPATPByChunk_Beta(PNList As List(Of PartNo)) As List(Of PartNo_ATPQty)
        Dim ATPResult As New List(Of PartNo_ATPQty)
        For Each pn As PartNo In PNList
            Dim result = From ar In ATPResult Where ar.PartNo = pn.PN
            If result.Count = 0 Then
                ATPResult.Add(New PartNo_ATPQty(pn.PN, 0))
            End If
        Next

        Dim PNListToBeQueried As New List(Of String)
        Dim SAPApt As New Oracle.DataAccess.Client.OracleDataAdapter("", strSAPDbConn)

        For i As Integer = 0 To ATPResult.Count - 1
            PNListToBeQueried.Add("'" + Global_Inc.FormatToSAPPartNo(ATPResult(i).PartNo.ToUpper()) + "'")
                If PNListToBeQueried.Count = 10 Or i = ATPResult.Count - 1 Then
                Dim strPNList As String = String.Join(",", PNListToBeQueried.ToArray())
                Dim dtATP As New DataTable
                If SAPApt.SelectCommand.Connection.State <> ConnectionState.Open Then SAPApt.SelectCommand.Connection.Open()
                SAPApt.SelectCommand.CommandText =
                    "  select matnr, sum(a.labst) as qty  " +
                    "  from saprdp.mard a  " +
                    "  where a.mandt='168' and matnr in (" + strPNList + ") and a.diskz=' '  " +
                    "  group by matnr "
                SAPApt.Fill(dtATP)
                PNListToBeQueried.Clear()

                For Each ar As PartNo_ATPQty In ATPResult
                    Dim rs() As DataRow = dtATP.Select("matnr='" + Global_Inc.FormatToSAPPartNo(ar.PartNo) + "'")
                    If rs.Length > 0 Then
                        ar.ATPQty = rs(0).Item("qty")
                    End If
                Next

            End If
        Next
        SAPApt.SelectCommand.Connection.Close()
        Return ATPResult
    End Function

    Class PartNo
        Public Property PN As String
        Public Sub New()
            PN = ""
        End Sub
        Public Sub New(PN As String)
            Me.PN = PN
        End Sub
    End Class

    Class PartNo_ATPQty
        Public Property PartNo As String : Public Property ATPQty As Decimal
        Public Sub New()
            PartNo = "" : ATPQty = 0
        End Sub
        Public Sub New(pn As String, q As Decimal)
            PartNo = pn : ATPQty = q
        End Sub
    End Class

    Public Shared Function IsFranchiser(ByVal email As String, ByRef strCompanyId As String) As Boolean
        'If email.EndsWith("@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then
        Dim str As String = String.Format("SELECT SALES_CODE,COMPANY_ID,EMAIL FROM FRANCHISER WHERE (EMAIL = '{0}')", email)
        Dim _dt As New DataTable
        _dt = dbUtil.dbGetDataTable("MY", str)
        If _dt.Rows.Count > 0 Then
            strCompanyId = _dt.Rows(0).Item("COMPANY_ID") : Return True
        End If
        'End If
        Return False
    End Function
    Shared Function getCountryCodebyShipTo(ByVal ShipToId As String) As String
        Dim str As String = String.Format("SELECT LAND1 FROM SAPRDP.KNA1 WHERE KUNNR='{0}' and rownum=1", ShipToId.ToUpper)
        Dim CID As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
        If Not IsNothing(CID) AndAlso CID.ToString <> "" Then
            Return CID.ToString
        End If
        Return "TW"
    End Function
    Public Shared Function GetSAPPartnerTableByKunnr(ByVal Kunnr As String) As SalesOrder.SAP_BAPIPARNRDataTable
        Dim retTable As New SalesOrder.SAP_BAPIPARNRDataTable
        Dim strSql As String =
            " select a.kunnr as PARTN_NUMB,a.anred as TITLE, a.NAME1 as NAME, a.NAME2 as NAME_2, b.NAME3 as NAME_3, b.NAME4 as NAME_4, " +
            " a.STRAS as STREET, a.LAND1 as COUNTRY, a.LAND1 as COUNTRY_ISO, a.PSTLZ as POSTL_CODE, '' as POBX_PCD, ''as POBX_CTY,  " +
            " b.CITY1 as CITY, '' as DISTRICT, a.REGIO as REGION, b.PO_BOX as PO_BOX, a.TELF1 as TELEPHONE, a.TELF2 as TELEPHONE2, a.TELBX as TELEBOX, " +
            " a.TELFX as FAX_NUMBER, a.TELTX as TELEX_NO, a.SPRAS as LANGU, '' as LANGU_ISO, '' as UNLOAD_PT, b.TRANSPZONE, b.TAXJURCODE, " +
            " '' as ADDRESS, '' as PRIV_ADDR, 1 as ADDR_TYPE, '' as ADDR_ORIG, '' as ADDR_LINK, '' as REFOBJTYPE, '' as REFOBJKEY, '' as REFLOGSYS " +
            " ,b.name_co as Attention from saprdp.kna1 a inner join saprdp.adrc b on a.adrnr=b.addrnumber and a.land1=b.country " +
            " where a.mandt='168' and a.kunnr='" + UCase(Trim(Kunnr)) + "' "
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
        For Each r As DataRow In dt.Rows
            Dim PtnrRow As SalesOrder.SAP_BAPIPARNRRow = retTable.NewSAP_BAPIPARNRRow()
            PtnrRow.ADDR_LINK = ""
            PtnrRow.ADDR_ORIG = ""
            PtnrRow.ADDR_TYPE = r.Item("ADDR_TYPE")
            PtnrRow.ADDRESS = ""
            PtnrRow.CITY = r.Item("CITY")
            PtnrRow.COUNTRY_ISO = r.Item("COUNTRY_ISO")
            PtnrRow.COUNTRY = r.Item("COUNTRY")
            PtnrRow.DISTRICT = ""
            PtnrRow.FAX_NUMBER = r.Item("FAX_NUMBER")
            PtnrRow.ITM_NUMBER = ""
            PtnrRow.LANGU = r.Item("LANGU")
            PtnrRow.LANGU_ISO = ""
            PtnrRow.NAME = r.Item("NAME")
            PtnrRow.NAME_2 = r.Item("NAME_2")
            PtnrRow.NAME_3 = r.Item("NAME_3")
            PtnrRow.NAME_4 = r.Item("NAME_4")
            PtnrRow.PARTN_NUMB = Kunnr
            PtnrRow.PARTN_ROLE = ""
            PtnrRow.PO_BOX = r.Item("PO_BOX")
            PtnrRow.POBX_CTY = ""
            PtnrRow.POBX_PCD = ""
            PtnrRow.POSTL_CODE = r.Item("POSTL_CODE")
            PtnrRow.PRIV_ADDR = ""
            PtnrRow.REFLOGSYS = ""
            PtnrRow.REFOBJKEY = ""
            PtnrRow.REFOBJTYPE = ""
            PtnrRow.STREET = r.Item("STREET")
            PtnrRow.TAXJURCODE = r.Item("TAXJURCODE")
            PtnrRow.TELEBOX = r.Item("TELEBOX")
            PtnrRow.TELEPHONE = r.Item("TELEPHONE")
            PtnrRow.TELEPHONE2 = r.Item("TELEPHONE2")
            PtnrRow.TELETEX_NO = ""
            PtnrRow.TELEX_NO = r.Item("TELEX_NO")
            PtnrRow.TITLE = r.Item("TITLE")
            PtnrRow.TRANSPZONE = r.Item("TRANSPZONE")
            PtnrRow.UNLOAD_PT = ""
            PtnrRow._REGION = r.Item("REGION")
            retTable.AddSAP_BAPIPARNRRow(PtnrRow)
        Next
        Return retTable
    End Function

    Public Shared Function GetSAPPartnerAddressesTableByKunnr(ByVal Kunnr As String, Optional ByVal IsSAPProductionServer As Boolean = True) As SalesOrder.PartnerAddressesDataTable
        Dim retTable As New SalesOrder.PartnerAddressesDataTable
        Dim strSql As String = String.Format(
          " Select * FROM " +
            " (select a.kunnr as PARTN_NUMB,a.anred as TITLE, a.NAME1 as NAME, a.NAME2 as NAME_2, a.STRAS as STREET, " +
            " a.LAND1 as COUNTRY, a.LAND1 as COUNTRY_ISO, a.PSTLZ as POSTL_CODE, '' as POBX_PCD, ''as POBX_CTY, " +
            " a.TELF2 as TELEPHONE2, a.TELBX as TELEBOX, a.TELFX as FAX_NUMBER,  a.TELF1 as TELEPHONE, " +
            " a.TELTX as TELEX_NO, a.SPRAS as LANGU, '' as LANGU_ISO, '' as UNLOAD_PT,a.REGIO as REGION, " +
            " '' as ADDRESS, '' as PRIV_ADDR, 1 as ADDR_TYPE, '' as ADDR_ORIG, '' as ADDR_LINK, '' as REFOBJTYPE,'' as REFOBJKEY, '' as REFLOGSYS, " +
            " a.ADRNR as ADRNR from saprdp.kna1 a where a.KUNNR='{0}') T " +
            " Left Join " +
            " (select " +
            " b.NAME3 as NAME_3, b.NAME4 as NAME_4, " +
            " b.CITY1 as CITY, b.CITY2 as DISTRICT, b.CITY_CODE, b.CITYP_CODE as Distrct_No, b.PO_BOX as PO_BOX, " +
            " b.TEL_EXTENS, " +
            " b.TRANSPZONE, b.TAXJURCODE,  " +
            " b.name_co as Attention, b.time_zone, b.deflt_comm, b.addrnumber, b.BUILDING, b.DONT_USE_P, b.DONT_USE_S, " +
            " b.FAX_EXTENS, b.FLOOR, b.HOUSE_NUM1, b.HOUSE_NUM2, b.HOUSE_NUM3, b.PO_BOX_NUM, b.PO_BOX_CTY, b.PO_BOX_REG, b.HOME_CITY, b.CITYH_CODE, " +
            " b.POST_CODE1, b.POST_CODE2, b.POST_CODE3, b.REGIOGROUP, b.ROOMNUMBER, b.STR_SUPPL1, b.STR_SUPPL2, b.STR_SUPPL3, b.STREETCODE, b.LOCATION " +
            " from saprdp.adrc b where b.addrnumber=(select adrnr from saprdp.kna1 a where a.kunnr='{0}' and rownum=1)) M " +
            " on T.ADRNR=M.addrnumber" +
            " Left Join" +
            " (select c.addrnumber, c.SMTP_ADDR as Contact_Email from SAPRDP.ADR6 c where c.addrnumber = (select adrnr from saprdp.kna1 a where a.kunnr='{0}' and rownum=1) and rownum=1 ) E " +
            " On T.ADRNR = E.addrnumber", Kunnr)
        Dim ConnectionName As String = "SAP_PRD"
        If Not IsSAPProductionServer Then
            ConnectionName = "SAP_Test"
        End If
        Dim dt As DataTable = OraDbUtil.dbGetDataTable(ConnectionName, strSql)
        For Each r As DataRow In dt.Rows
            Dim S_PartnerAddressesRow As SalesOrder.PartnerAddressesRow = retTable.NewPartnerAddressesRow()
            With S_PartnerAddressesRow
                .C_O_Name = r.Item("Attention") : .Addr_No = r.Item("addrnumber") : .Adr_Notes = ""
                .Build_Long = "" : .Building = r.Item("BUILDING") : .Chckstatus = ""
                .City = r.Item("CITY") : .City_No = r.Item("CITY_CODE") : .Comm_Type = r.Item("deflt_comm")
                .Country = r.Item("COUNTRY") : .Countryiso = "" : .Deliv_Dis = ""
                .Distrct_No = r.Item("Distrct_No") : .District = r.Item("DISTRICT") : .Dont_Use_P = r.Item("DONT_USE_P")
                .Dont_Use_S = r.Item("DONT_USE_S") : .Fax_Extens = r.Item("FAX_EXTENS")
                .E_Mail = IIf(IsDBNull(r.Item("Contact_Email")), "", r.Item("Contact_Email"))
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

    Public Shared Function GetSAPPartnerAddressesTableByKunnr_Old(ByVal Kunnr As String) As SalesOrder.PartnerAddressesDataTable
        Dim retTable As New SalesOrder.PartnerAddressesDataTable
        Dim strSql As String =
            " select a.kunnr as PARTN_NUMB,a.anred as TITLE, a.NAME1 as NAME, a.NAME2 as NAME_2, b.NAME3 as NAME_3, b.NAME4 as NAME_4, " +
            " a.STRAS as STREET, a.LAND1 as COUNTRY, a.LAND1 as COUNTRY_ISO, a.PSTLZ as POSTL_CODE, '' as POBX_PCD, ''as POBX_CTY,  " +
            " b.CITY1 as CITY, '' as DISTRICT, a.REGIO as REGION, b.PO_BOX as PO_BOX, a.TELF1 as TELEPHONE, a.TELF2 as TELEPHONE2, a.TELBX as TELEBOX, " +
            " a.TELFX as FAX_NUMBER, a.TELTX as TELEX_NO, a.SPRAS as LANGU, '' as LANGU_ISO, '' as UNLOAD_PT, b.TRANSPZONE, b.TAXJURCODE, " +
            " '' as ADDRESS, '' as PRIV_ADDR, 1 as ADDR_TYPE, '' as ADDR_ORIG, '' as ADDR_LINK, '' as REFOBJTYPE, '' as REFOBJKEY, '' as REFLOGSYS " +
            " ,b.name_co as Attention from saprdp.kna1 a inner join saprdp.adrc b on a.adrnr=b.addrnumber and a.land1=b.country " +
            " where a.mandt='168' and a.kunnr='" + UCase(Trim(Kunnr)) + "' "
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
        For Each r As DataRow In dt.Rows
            Dim S_PartnerAddressesRow As SalesOrder.PartnerAddressesRow = retTable.NewPartnerAddressesRow()
            S_PartnerAddressesRow.C_O_Name = r.Item("NAME_CO")
            S_PartnerAddressesRow.Addr_No = r.Item("addrnumber")
            S_PartnerAddressesRow.Adr_Notes = r.Item("5555")
            S_PartnerAddressesRow.Build_Long = r.Item("5555")
            S_PartnerAddressesRow.Building = r.Item("BUILDING")
            S_PartnerAddressesRow.Chckstatus = r.Item("CHCKSTATUS")
            S_PartnerAddressesRow.City = r.Item("CITY1")
            S_PartnerAddressesRow.City_No = r.Item("CITY_CODE")
            S_PartnerAddressesRow.Comm_Type = r.Item("5555")
            S_PartnerAddressesRow.Country = r.Item("COUNTRY")
            S_PartnerAddressesRow.Countryiso = r.Item("5555")
            S_PartnerAddressesRow.Deliv_Dis = r.Item("5555")
            S_PartnerAddressesRow.Distrct_No = r.Item("5555")
            S_PartnerAddressesRow.District = r.Item("5555")
            S_PartnerAddressesRow.Dont_Use_P = r.Item("5555")
            S_PartnerAddressesRow.Dont_Use_S = r.Item("DONT_USE_S")
            S_PartnerAddressesRow.E_Mail = r.Item("5555")
            S_PartnerAddressesRow.Fax_Extens = r.Item("FAX_EXTENS")
            S_PartnerAddressesRow.Fax_Number = r.Item("FAX_NUMBER")
            S_PartnerAddressesRow.Floor = r.Item("FLOOR")
            S_PartnerAddressesRow.Formofaddr = r.Item("5555")
            S_PartnerAddressesRow.Home_City = r.Item("Home_City")
            S_PartnerAddressesRow.Homecityno = r.Item("cityh_code")
            S_PartnerAddressesRow.Homepage = r.Item("5555")
            S_PartnerAddressesRow.House_No = r.Item("HOUSE_NUM1")
            S_PartnerAddressesRow.House_No2 = r.Item("HOUSE_NUM2")
            S_PartnerAddressesRow.House_No3 = r.Item("HOUSE_NUM3")
            S_PartnerAddressesRow.Langu = r.Item("LANGU")
            S_PartnerAddressesRow.Langu_Cr = r.Item("5555")
            S_PartnerAddressesRow.Langu_Iso = r.Item("5555")
            S_PartnerAddressesRow.Langucriso = r.Item("5555")
            S_PartnerAddressesRow.Location = r.Item("LOCATION")
            S_PartnerAddressesRow.Name = r.Item("NAME1")
            S_PartnerAddressesRow.Name_2 = r.Item("NAME2")
            S_PartnerAddressesRow.Name_3 = r.Item("NAME3")
            S_PartnerAddressesRow.Name_4 = r.Item("NAME4")
            S_PartnerAddressesRow.Pboxcit_No = r.Item("PO_BOX_NUM")
            S_PartnerAddressesRow.Pcode1_Ext = r.Item("5555")
            S_PartnerAddressesRow.Pcode2_Ext = r.Item("5555")
            S_PartnerAddressesRow.Pcode3_Ext = r.Item("5555")
            S_PartnerAddressesRow.Po_Box = r.Item("PO_BOX")
            S_PartnerAddressesRow.Po_Box_Cit = r.Item("PO_BOX_CTY")
            S_PartnerAddressesRow.Po_Box_Reg = r.Item("PO_BOX_REG")
            S_PartnerAddressesRow.Po_Ctryiso = r.Item("5555")
            S_PartnerAddressesRow.Po_W_O_No = r.Item("5555")
            S_PartnerAddressesRow.Pobox_Ctry = r.Item("5555")
            S_PartnerAddressesRow.Postl_Cod1 = r.Item("POST_CODE1")
            S_PartnerAddressesRow.Postl_Cod2 = r.Item("POST_CODE2")
            S_PartnerAddressesRow.Postl_Cod3 = r.Item("POST_CODE3")
            S_PartnerAddressesRow.Regiogroup = r.Item("REGIOGROUP")
            S_PartnerAddressesRow.Region_str = r.Item("REGION")
            S_PartnerAddressesRow.Room_No = r.Item("ROOMNUMBER")
            S_PartnerAddressesRow.Sort1 = r.Item("SORT1")
            S_PartnerAddressesRow.Sort2 = r.Item("SORT2")
            S_PartnerAddressesRow.Str_Abbr = r.Item("5555")
            S_PartnerAddressesRow.Str_Suppl1 = r.Item("STR_SUPPL1")
            S_PartnerAddressesRow.Str_Suppl2 = r.Item("STR_SUPPL2")
            S_PartnerAddressesRow.Str_Suppl3 = r.Item("STR_SUPPL3")
            S_PartnerAddressesRow.Street = r.Item("STREET")
            S_PartnerAddressesRow.Street_Lng = r.Item("5555")
            S_PartnerAddressesRow.Street_No = r.Item("STREETCODE")
            S_PartnerAddressesRow.Taxjurcode = r.Item("5555")
            S_PartnerAddressesRow.Tel1_Ext = r.Item("TEL_EXTENS")
            S_PartnerAddressesRow.Tel1_Numbr = r.Item("TEL_NUMBER")
            S_PartnerAddressesRow.Time_Zone = r.Item("5555")
            S_PartnerAddressesRow.Title = r.Item("title")
            S_PartnerAddressesRow.Transpzone = r.Item("TRANSPZONE")
            retTable.AddPartnerAddressesRow(S_PartnerAddressesRow)
        Next
        Return retTable
    End Function
    Public Shared Function GetCustomerDataSet(ByVal companyid As String, ByRef ds As DataSet, Optional ByVal ConnectToSAPPRD As Boolean = True) As Boolean
        companyid = Replace(Trim(UCase(companyid)), "'", "")
        ds = New DataSet("SAPCustomer")
        Dim SAPconnection As String = "SAP_PRD"
        If ConnectToSAPPRD = False Then
            SAPconnection = "SAP_Test"
        End If
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" select kna1.kunnr as Company_Id, "))
            .AppendLine(String.Format(" 	   knvv.vkorg as org_id, "))
            .AppendLine(String.Format("     (select MIN(knvp.kunnr) from saprdp.knvp where knvp.kunn2 = kna1.kunnr and knvp.vkorg=knvv.vkorg AND knvp.parvw='WE') as ParentCompanyId, "))
            .AppendLine(String.Format(" 		kna1.name1 || kna1.name2 as Company_Name, "))
            .AppendLine(String.Format(" 		adrc.street || adrc.str_suppl3 || adrc.location as Address, "))
            .AppendLine(String.Format(" 		kna1.telfx as fax_no, "))
            .AppendLine(String.Format(" 		kna1.telf1 as tel_no, "))
            .AppendLine(String.Format(" 		kna1.ktokd as company_type, "))
            .AppendLine(String.Format(" 		kna1.kdkg1 || kna1.kdkg2 || kna1.kdkg3 || kna1.kdkg4 as price_class,  "))
            .AppendLine(String.Format("     '' as ptrade_price_class, "))
            .AppendLine(String.Format(" 		knvv.waers as Currency, "))
            .AppendLine(String.Format(" 		adrc.country as Country,  "))
            .AppendLine(String.Format("     '' as region, "))
            .AppendLine(String.Format(" 		adrc.post_code1 as Zip_Code, "))
            .AppendLine(String.Format(" 		adrc.city1 as City, "))
            .AppendLine(String.Format(" 		adrc.name_co as Attention, "))
            .AppendLine(String.Format(" 		'0' as Credit_Limit, "))
            .AppendLine(String.Format(" 		knvv.zterm as Credit_Term, "))
            .AppendLine(String.Format(" 		knvv.inco1 || '  ' || knvv.inco2 as Ship_Via, "))
            .AppendLine(String.Format(" 		kna1.knurl as Url,  "))
            .AppendLine(String.Format("     '' as LAST_UPDATED,  "))
            .AppendLine(String.Format("     '' as UPDATED_BY,  "))
            .AppendLine(String.Format(" 		kna1.erdat as CREATED_DATE, "))
            .AppendLine(String.Format(" 		kna1.ernam as Created_By, "))
            .AppendLine(String.Format(" 		knvv.kdgrp as Company_Price_Type,	 "))
            .AppendLine(String.Format("     '' as SALES_USERID,	 "))
            .AppendLine(String.Format(" 		knvv.vsbed as SHIP_CONDITION, "))
            .AppendLine(String.Format(" 		kna1.KATR4 as attribute4, "))
            .AppendLine(String.Format(" 		KNVV.VKBUR as SalesOffice, "))
            .AppendLine(String.Format("     KNVV.VKGRP as SalesGroup, "))
            .AppendLine(String.Format(" (select KNVI.TAXKD from saprdp.KNVI where KNVI.kunnr=kna1.kunnr and KNVI.ALAND = 'NL' and KNVI.TATYP = 'MWST' and KNVI.mandt='168' and rownum=1) as TAX_CLASS "))
            .AppendLine(String.Format(" from saprdp.knvv inner join saprdp.kna1 on knvv.kunnr=kna1.kunnr  "))
            .AppendLine(String.Format(" 	inner join saprdp.adrc on kna1.adrnr=adrc.addrnumber and kna1.land1=adrc.country   "))
            .AppendLine(String.Format(" where rownum=1 and knvv.mandt='168'  and kna1.loevm = ' ' and knvv.kunnr='{0}' ", companyid))
        End With
        Dim masterDt As DataTable = OraDbUtil.dbGetDataTable(SAPconnection, sb.ToString())
        masterDt.TableName = "Master" : ds.Tables.Add(masterDt)

        sb = New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" SELECT A.KUNNR AS COMPANY_ID, B.SMTP_ADDR AS CONTACT_EMAIL, B.FLGDEFAULT, B.CONSNUMBER,  "))
            .AppendLine(String.Format(" B.HOME_FLAG, B.SMTP_SRCH, B.PERSNUMBER, C.TEL_NUMBER, C.TELNR_LONG, C.TELNR_CALL "))
            .AppendLine(String.Format(" FROM SAPRDP.KNA1 A INNER JOIN SAPRDP.ADR6 B ON A.ADRNR=B.ADDRNUMBER  "))
            .AppendLine(String.Format(" LEFT JOIN SAPRDP.ADR2 C ON B.ADDRNUMBER=C.ADDRNUMBER AND B.PERSNUMBER=C.PERSNUMBER "))
            .AppendLine(String.Format(" where A.KUNNR ='" + companyid + "' "))
        End With

        Dim contactDt As DataTable = OraDbUtil.dbGetDataTable(SAPconnection, sb.ToString())
        contactDt.TableName = "Contact" : ds.Tables.Add(contactDt)

        Dim CompanyPartnerId As String = companyid
        Dim knvpDt As DataTable = OraDbUtil.dbGetDataTable(SAPconnection, "select kunnr, parvw from saprdp.knvp where kunnr='" + CompanyPartnerId + "' and rownum=1")
        If knvpDt.Rows.Count > 0 Then
            Dim HasAGFlag As Boolean = False
            For i As Integer = 0 To knvpDt.Rows.Count - 1
                If knvpDt.Rows(0).Item("parvw").ToString() = "AG" Then
                    HasAGFlag = True : CompanyPartnerId = knvpDt.Rows(0).Item("kunnr").ToString() : Exit For
                End If
            Next
            If Not HasAGFlag Then
                Dim knvpDt2 As DataTable = OraDbUtil.dbGetDataTable(SAPconnection,
                    " select kunnr from saprdp.knvp  " +
                    " where kunnr in (select kunnr from saprdp.knvp where kunn2='" + knvpDt.Rows(0).Item("kunnr").ToString() + "' and kunnr<>kunn2 and rownum=1)  " +
                    " and parvw='AG' and rownum=1 ")
                If knvpDt2.Rows.Count = 1 Then
                    HasAGFlag = True : CompanyPartnerId = knvpDt2.Rows(0).Item("kunnr").ToString()
                End If
            End If

            If HasAGFlag Then
                sb = New System.Text.StringBuilder
                With sb
                    sb.AppendLine(" select b.kunnr as COMPANY_ID, b.vkorg as ORG_ID, b.vtweg as DIST_CHANN,  ")
                    sb.AppendLine(" b.spart as DIVISION, b.parvw as PARTNER_FUNCTION, b.kunn2 as PARENT_COMPANY_ID,  ")
                    sb.AppendLine(" b.lifnr as VENDOR_CREDITOR, b.pernr as SALES_CODE, b.parnr as PARTNER_NUMBER, b.KNREF, b.DEFPA ")
                    sb.AppendLine(" from saprdp.kna1 a inner join saprdp.knvp b on a.kunnr=b.kunnr ")
                    sb.AppendLine(" where a.mandt='168' and b.mandt='168' ")
                    sb.AppendLine(" and b.vkorg not in " + ConfigurationManager.AppSettings("InvalidOrg") + "  ")
                    sb.AppendFormat(" and b.kunnr ='{0}' ", CompanyPartnerId)
                End With
                Dim PartnerDt As DataTable = OraDbUtil.dbGetDataTable(SAPconnection, sb.ToString())
                PartnerDt.TableName = "Partner" : ds.Tables.Add(PartnerDt)
                Return True
            End If

        End If

        Return False
    End Function

    Public Shared Function SearchAllSAPCompanySoldBillShipTo(
       ByVal ERPID As String, ByVal Org_id As String, ByVal CompanyName As String, ByVal Address As String, ByVal State As String,
       ByVal Division As String, ByVal SalesGroup As String, ByVal SalesOffice As String) As DataTable
        Dim dt As New DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            ' .AppendLine(" SELECT A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME,  D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| D.country AS Address, ") 'B.STRAS AS ADDRESS,
            .AppendLine(" SELECT A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME, " +
                        " D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1) AS Address, ") 'B.STRAS AS ADDRESS,
            .AppendLine(" B.Land1 AS  COUNTRY,B.Ort01 AS CITY,")
            .AppendLine(" B.PSTLZ AS ZIP_CODE, D.region AS STATE,  C.smtp_addr AS CONTACT_EMAIL,B.TELF1 AS TEL_NO,B.TELFX AS FAX_NO, D.NAME_CO as Attention, ")
            .AppendLine(" case A.PARVW when 'WE' then 'Ship-To' when 'AG' then 'Sold-To' when 'RE' then 'Bill-To' end as PARTNER_FUNCTION, ")
            .AppendLine(" E.VKBUR as SalesOffice, E.VKGRP as SalesGroup, E.SPART as division  ")
            .AppendLine(" FROM saprdp.knvp A  ")
            .AppendLine(" INNER JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR left join saprdp.adr6 C on B.adrnr=C.addrnumber ")
            .AppendLine(" inner join saprdp.adrc D on  D.country=B.land1 and D.addrnumber=B.adrnr inner join saprdp.knvv E on B.KUNNR=E.KUNNR  ")
            .AppendLine(" where rownum<=30 ")
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

            .AppendFormat(" AND A.PARVW in ('WE','AG','RE') ORDER BY A.Kunn2 ", Org_id)
        End With
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        dt.TableName = "SAPPF"
        Return dt
    End Function

    Shared Function FormatItmNumber(ByVal ItemNumber As Integer) As String
        Dim Zeros As Integer = 6 - ItemNumber.ToString.Length
        If Zeros = 0 Then Return ItemNumber.ToString()
        Dim strItemNumber As String = ItemNumber.ToString()
        For i As Integer = 0 To Zeros - 1
            strItemNumber = "0" + strItemNumber
        Next
        Return strItemNumber
    End Function

#Region "Get Data From Local"
    Public Shared Function GetCompanyDataFromLocal(ByVal companyid As String, ByVal OrgId As String) As DataTable
        Dim strSql As String =
            " SELECT TOP 1 COMPANY_ID, ORG_ID, PARENTCOMPANYID, COMPANY_NAME, ADDRESS, FAX_NO, TEL_NO,  " +
            " COMPANY_TYPE, PRICE_CLASS, CURRENCY, COUNTRY, REGION_CODE, ZIP_CODE, CITY, ATTENTION,  " +
            " CREDIT_TERM, SHIP_VIA, URL, SHIPCONDITION, ATTRIBUTE4, SALESOFFICE, SALESGROUP, AMT_INSURED,  " +
            " CREDIT_LIMIT, CONTACT_EMAIL, DELETION_FLAG, COUNTRY_NAME, SALESOFFICENAME, SAP_SALESNAME,  " +
            " SAP_SALESCODE, SAP_ISNAME, SAP_OPNAME " +
            " FROM SAP_DIMCOMPANY " +
            " WHERE COMPANY_ID = @CID AND ORG_ID =@OID "
        Dim apt As New SqlClient.SqlDataAdapter(strSql, New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
        apt.SelectCommand.Parameters.AddWithValue("CID", companyid) : apt.SelectCommand.Parameters.AddWithValue("OID", OrgId)
        Dim dt As New DataTable
        apt.Fill(dt)
        apt.SelectCommand.Connection.Close()
        Return dt
    End Function
#End Region
    <WebMethod()>
    Public Shared Function SearchSAPCompany(ByVal ERPID As String, ByVal Org_id As String) As DataTable
        Dim dt As New DataTable
        If String.IsNullOrEmpty(ERPID) Or String.IsNullOrEmpty(Org_id) Then Return New DataTable("SAPPF")
        ERPID = Replace(Trim(ERPID).ToUpper, "'", "")
        Org_id = Replace(Trim(Org_id).ToUpper, "'", "")
        Dim sb As New System.Text.StringBuilder
        With sb

            .AppendLine(" SELECT A.KUNN2 AS company_id,B.NAME1 AS COMPANY_NAME,  " +
                        " D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1 and E.MANDT=168) AS Address,  " +
                        " B.Land1 AS  COUNTRY,B.Ort01 AS CITY, B.PSTLZ AS ZIP_CODE, D.region AS STATE,  C.smtp_addr AS CONTACT_EMAIL,B.TELF1 AS TEL_NO, " +
                        " B.TELF2 AS Mobile,B.TELFX AS FAX_NO, ")
            .AppendLine(" case A.PARVW when 'WE' then 'Ship-To' when 'AG' then 'Sold-To' when 'RE' then 'Bill-To' end as PARTNER_FUNCTION ")
            .AppendLine(" FROM saprdp.knvp A  ")
            .AppendLine(" Left JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR Left join saprdp.adr6 C on B.adrnr=C.addrnumber ")
            .AppendLine(" Left join saprdp.adrc D on  D.country=B.land1 and D.addrnumber=B.adrnr  ")
            .AppendLine(" where ")
            .AppendFormat("  A.Kunnr = '{0}' ", ERPID)
            .AppendFormat(" AND A.PARVW = 'AG' AND A.VKORG='{0}' and rownum=1 and A.MANDT=168 and B.MANDT=168 ORDER BY A.Kunn2 ", Org_id)
        End With
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        dt.TableName = "SAPPF"
        'If dt.Rows.Count > 0 Then
        '    Return dt
        'End If
        Return dt
    End Function

    <WebMethod()>
    Public Function GetSAPCustomerPartnerFunctionByParameters(ByVal ERPID As String, ByVal Org_id As String, ByVal CompanyName As String, ByVal Address As String, ByVal State As String) As DataTable
        Dim dt As New DataTable
        If String.IsNullOrEmpty(ERPID) Then Return New DataTable("SAPPF")
        ERPID = Replace(Trim(ERPID).ToUpper, "'", "")
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(" SELECT A.KUNN2 AS company_id,B.NAME1 AS COMPANY_NAME, B.STRAS AS ADDRESS,  B.Land1 AS  COUNTRY,B.Ort01 AS CITY, B.PSTLZ AS ZIP_CODE, D.region AS STATE,  C.smtp_addr AS CONTACT_EMAIL,B.TELF1 AS TEL_NO,B.TELFX AS FAX_NO, ")
            .AppendLine(" case A.PARVW when 'WE' then 'Ship-To' when 'AG' then 'Sold-To' when 'RE' then 'Bill-To' end as PARTNER_FUNCTION ")
            .AppendLine(" FROM saprdp.knvp A  ")
            .AppendLine(" INNER JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR inner join saprdp.adr6 C on B.adrnr=C.addrnumber ")
            .AppendLine(" inner join saprdp.adrc D on  D.country=B.land1 and D.addrnumber=B.adrnr  ")
            .AppendLine(" where ")
            .AppendFormat(" D.region LIKE '%{0}%' AND B.STRAS LIKE '%{1}%' AND B.NAME1 LIKE '%{2}%' AND A.Kunnr LIKE '%{3}%' ", State.Replace("'", "''").Trim, Address.Replace("'", "''").Trim, CompanyName.Replace("'", "''").Trim, ERPID)
            .AppendFormat(" AND A.PARVW in ('WE','AG','RE') AND A.VKORG='{0}' ORDER BY A.Kunn2 ", Org_id)
        End With
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        dt.TableName = "SAPPF"
        Return dt
    End Function


    Public Shared Function SearchAllSAPCompanySoldBillShipTo(
        ByVal ERPID As String, ByVal Org_id As String, ByVal CompanyName As String, ByVal Address As String, ByVal State As String, ByVal Office As String, ByVal Group As String) As DataTable
        Dim dt As New DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            ' .AppendLine(" SELECT A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME,  D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| D.country AS Address, ") 'B.STRAS AS ADDRESS,
            .AppendLine(" SELECT A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME,  D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1) AS Address, ") 'B.STRAS AS ADDRESS,
            .AppendLine(" B.Land1 AS  COUNTRY,B.Ort01 AS CITY,")
            .AppendLine(" B.PSTLZ AS ZIP_CODE, D.region AS STATE,  C.smtp_addr AS CONTACT_EMAIL,B.TELF1 AS TEL_NO,B.TELFX AS FAX_NO, ")
            .AppendLine(" case A.PARVW when 'WE' then 'Ship-To' when 'AG' then 'Sold-To' when 'RE' then 'Bill-To' end as PARTNER_FUNCTION ")
            .AppendLine(" FROM saprdp.knvp A  ")
            .AppendLine(" INNER JOIN saprdp.kna1 B on A.KUNN2 = B.KUNNR inner join saprdp.adr6 C on B.adrnr=C.addrnumber ")
            .AppendLine(" inner join saprdp.adrc D on  D.country=B.land1 and D.addrnumber=B.adrnr inner join saprdp.knvv E on B.KUNNR=E.KUNNR  ")
            .AppendLine(" where rownum<=30 ")
            If Not String.IsNullOrEmpty(State) Then .AppendFormat(" and Upper(D.region) LIKE '%{0}%' ", UCase(State.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(Address) Then .AppendFormat(" and Upper(B.STRAS) LIKE '%{0}%' ", UCase(Address.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(CompanyName) Then .AppendFormat(" and (Upper(B.NAME1) LIKE '%{0}%' or B.NAME2 like '%{0}%') ", UCase(CompanyName.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(ERPID) Then .AppendFormat(" and (Upper(A.Kunnr) LIKE '%{0}%' or upper(A.Kunnr) like '%{0}%') ", UCase(ERPID.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(Org_id) Then .AppendFormat(" and A.VKORG like '%{0}%' ", UCase(Org_id.Replace("'", "''").Trim))
            If Not String.IsNullOrEmpty(Office) And Not String.IsNullOrEmpty(Group) Then .AppendFormat(" and E.VKBUR = '{0}' and E.VKGRP='{1}' ", UCase(Office.Replace("'", "''").Trim), UCase(Group.Replace("'", "''").Trim))
            .AppendFormat(" AND A.PARVW in ('WE','AG','RE') ORDER BY A.Kunn2 ", Org_id)
        End With
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        dt.TableName = "SAPPF"
        Return dt
    End Function

    <WebMethod()>
    Public Function GetOrderListFromSAP(ByVal PoNo As String, ByVal SoNo As String, ByVal CompanyID As String, ByVal OrgID As String, ByVal OrderDateFrom As DateTime, ByVal OrderDateTo As DateTime) As DataTable
        If String.IsNullOrEmpty(OrgID) Then Return New DataTable("SAPPF")
        PoNo = Replace(Trim(PoNo.ToUpper), "'", "")
        SoNo = Replace(Trim(SoNo.ToUpper), "'", "")
        CompanyID = Replace(Trim(CompanyID.ToUpper), "'", "")
        OrgID = Replace(Trim(OrgID.ToUpper), "'", "")
        If DateTime.TryParse(OrderDateFrom, Date.Now()) = False OrElse DateTime.TryParse(OrderDateTo, Date.Now()) = False Then
            Return New DataTable("SAPPF")
        End If
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(" select VBAK.VBELN AS SoNo, VBAK.BSTNK AS PoNo, VBAK.KUNNR as SOLDTOID, ")
            .AppendLine(" (select kunnr from saprdp.vbpa where vbpa.vbeln=vbak.vbeln and vbpa.parvw='RE' and rownum=1) AS BILLTOID, ")
            .AppendLine(" (select kunnr from saprdp.vbpa where vbpa.vbeln=vbak.vbeln and vbpa.parvw='WE' and rownum=1) AS SHIPTOID,    VBAK.BUKRS_VF AS ORG_ID,")
            .AppendFormat(" VBAK.AUDAT AS ORDERDATE  from SAPRDP.VBAK where VBAK.AUDAT between '{0}' and '{1}' and ", OrderDateFrom.ToString("yyyyMMdd"), OrderDateTo.ToString("yyyyMMdd"))
            .AppendFormat(" VBAK.KUNNR like '%{0}%' and ", CompanyID)
            .AppendFormat("  VBAK.BSTNK like '%{0}%' and VBAK.BSTNK like '%{1}%' and VBAK.BUKRS_VF='{2}' AND ", PoNo, SoNo, OrgID)
            .AppendFormat(" rownum<=30 ")
            .AppendLine("  order by  VBAK.AUDAT desc")
        End With
        Dim dt As New DataTable("SAPOrders")
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return Nothing
    End Function
    <WebMethod()>
    Public Function GetOrderMasterFromSAP(ByVal SoNo As String) As DataTable
        'If String.IsNullOrEmpty(OrgID) Then Return New DataTable("SAPPF")
        'PoNo = Replace(Trim(PoNo.ToUpper), "'", "")
        'SoNo = Replace(Trim(SoNo.ToUpper), "'", "")
        'CompanyID = Replace(Trim(CompanyID.ToUpper), "'", "")
        'OrgID = Replace(Trim(OrgID.ToUpper), "'", "")
        'If DateTime.TryParse(OrderDateFrom, Date.Now()) = False OrElse DateTime.TryParse(OrderDateTo, Date.Now()) = False Then
        '    Return New DataTable("SAPPF")
        'End If
        Dim STR As String = " select VBELN AS ORDNO,WAERK AS CURR,VKORG AS ORG, " &
                            " (SELECT DISTINCT BEZEI FROM SAPRDP.TVKBT WHERE VKBUR=A.VKBUR AND ROWNUM=1) AS OFFICE, " &
                            " KUNNR AS COMPANYID, " &
                            " (SELECT KUNNR FROM SAPRDP.VBPA WHERE SAPRDP.VBPA.VBELN=A.VBELN AND SAPRDP.VBPA.PARVW='WE' AND ROWNUM=1) AS SHIPTOID, " &
                            " (SELECT KUNNR FROM SAPRDP.VBPA WHERE SAPRDP.VBPA.VBELN=A.VBELN AND SAPRDP.VBPA.PARVW='RE' AND ROWNUM=1) AS BILLTOID, " &
                            " (SELECT NAME1 FROM SAPRDP.KNA1 WHERE KUNNR=A.KUNNR AND ROWNUM=1) AS COMPANYNAME " &
                            " from SAPRDP.VBAK A where A.VBELN ='" & SoNo.ToUpper.Trim & "'"

        Dim dt As New DataTable("SAPOrders")
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", STR)
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return Nothing
    End Function
    <WebMethod()>
    Public Function GetOrderDetailFromSAPByPoNo(ByVal PoNo As String) As DataTable
        If String.IsNullOrEmpty(PoNo) Then Return New DataTable("SAPDT")
        PoNo = Replace(Trim(PoNo.ToUpper), "'", "")
        If Global_Inc.IsNumericItem(PoNo) Then
            PoNo = Global_Inc.Format2SAPItem2(PoNo)
        End If
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine("  select cast(VBAP.POSNR as integer) AS Lineno, VBAP.MATWA AS  Partno,  ")
            .AppendLine("  VBAP.LSMENG AS  Qty, VBAP.ZZ_EDATU AS ReqDate, VBAP.NETPR AS UnitPrice,VBAP.NETWR AS  Amount ")
            .AppendFormat(" from   saprdp.VBAP where VBAP.VBELN ='{0}'  ", PoNo)
            .AppendLine(" order by Lineno ")
        End With
        Dim dt As New DataTable("SAPOrders")
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return New DataTable("SAPDT")
    End Function

    Public Shared Function GetCalendarIDbyOrg(ByVal org As String) As String
        Dim plant As String = org & "H1"
        Dim str As String = String.Format("select FABKL from saprdp.t001w where WERKS='{0}' and mandt='168' and rownum=1", plant)
        Dim CID As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
        If Not IsNothing(CID) AndAlso CID.ToString <> "" Then
            Return CID.ToString
        End If
        Return "TW"
    End Function

    Public Shared Function GetCalendarIDbyPlant(ByVal PlantID As String) As String
        Dim str As String = String.Format("select LAND1 from saprdp.t001w where WERKS='{0}' and mandt='168' and rownum=1", PlantID)
        Dim CID As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
        If Not IsNothing(CID) AndAlso CID.ToString <> "" Then
            Return CID.ToString
        End If
        Return "TW"
    End Function

    Public Shared Function IsBBDropshipmentCustomer(ByVal CustomerId As String) As Boolean
        Dim str As String = String.Format("select KATR1 from saprdp.KNA1 a where  a.mandt='168' and a.kunnr = '{0}'", CustomerId)
        Dim CusAttr As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
        If Not IsNothing(CusAttr) AndAlso CusAttr.ToString = "93" Then
            Return True
        End If
        Return False
    End Function



    <WebMethod()>
    Public Shared Function Get_Next_WorkingDate_ByCode(ByRef iATPDate As DateTime, ByVal Loading_Days As String, ByVal code As String) As Integer
        code = UCase(code)
        Dim proxy1 As New Factory_Date_Conversion.Factory_Date_Conversion
        Dim factory_date_Number As Decimal

        Dim provider1 As New CultureInfo("fr-FR", True)
        Dim time1 As DateTime = iATPDate ' DateTime.ParseExact(iATPDate, "yyyy-mm-dd", provider1)
        Dim iATPDateStr As String = time1.ToString("yyyyMMdd")
        'iATPDate = Replace(iATPDate, "/", "")

        Try
            proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD").ToString)
            proxy1.Connection.Open()

            proxy1.Date_Convert_To_Factorydate("+", code, factory_date_Number, "", iATPDateStr)
            proxy1.Factorydate_Convert_To_Date(code, (factory_date_Number + Loading_Days), iATPDateStr)

            proxy1.Connection.Close()
            Dim time2 As DateTime = DateTime.ParseExact(iATPDateStr, "yyyyMMdd", provider1)
            iATPDate = time2 '.ToString("yyyy-mm-dd")

        Catch ex As Exception
            ' iATPDate = ex.ToString()
            Return -1
            Exit Function

        End Try
        Return 1

    End Function
    <WebMethod()>
    Public Function HelloKitty() As String
        Return "Hello Kitty!"
    End Function
#Region "get SAP product more detailed information, and return whether or not there is(are) invalid product(s)  "
    '\ updated by Ming 2013-08-27, if there is a phaseout product, return true. Instead, return false.
    Public Shared Function GetSAPProductInfo(ByVal SalesOrg As String, ByRef quotationDetail As DataTable) As Boolean
        Dim _anyphaseout As Boolean = False
        If quotationDetail IsNot Nothing And quotationDetail.Rows.Count > 0 Then
            SalesOrg = Trim(UCase(SalesOrg))
            Dim pns As List(Of String) = New List(Of String)
            Dim IsNumericPn As Boolean = False, PartNo As String = String.Empty
            'Dim regex As New Text.RegularExpressions.Regex("^[0-9]+$")
            For Each row In quotationDetail.Rows
                PartNo = Global_Inc.Format2SAPItem(row("PartNo").ToString.Trim)
                pns.Add(PartNo)
            Next
            'empty part no list
            If Not pns.Any() Then
                Return False
            End If
            Dim strSql As String =
                " select a.matnr,a.vmsta as status_code, b.vmstb as status_desc from saprdp.MVKE a left join saprdp.TVMST b on a.vmsta=b.vmsta where a.mandt='168' " +
                " and b.mandt='168' and a.vkorg='" + SalesOrg + "' and a.matnr in('" + String.Join("','", pns.ToArray()) + "') and b.spras='E' "
            Dim dtProdStatus As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
            quotationDetail.Columns.Add("isPhaseout", GetType(Boolean))
            quotationDetail.Columns.Add("StatusCode", GetType(String))
            quotationDetail.Columns.Add("StatusDesc", GetType(String))
            quotationDetail.Columns.Add("ATPQty", GetType(Integer))
            For Each row In quotationDetail.Rows
                'Frank here also need to do the P-trade part number format transformation
                'Dim statusrow = dtProdStatus.Select(String.Format("matnr='{0}'", row("PartNo"))).FirstOrDefault()
                Dim statusrow = dtProdStatus.Select(String.Format("matnr='{0}'", Global_Inc.Format2SAPItem(row("PartNo").ToString.Trim))).FirstOrDefault()
                Dim _isphaseout As Boolean = False
                If statusrow IsNot Nothing Then
                    row("StatusCode") = statusrow.Item("status_code")
                    row("StatusDesc") = statusrow.Item("status_desc")
                    Dim ATPQty As Integer = 0
                    Select Case statusrow.Item("status_code")
                        'Case "A", "N", "H", "M1"
                        Case "A", "C", "N", "H", "M1", "P", "S2", "S5", "V", "T"
                            _isphaseout = False
                        Case "O", "S"
                            Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP, intInventory As Integer = -1
                            Dim atpTb As New GET_MATERIAL_ATP.BAPIWMDVETable, retTb As New GET_MATERIAL_ATP.BAPIWMDVSTable, rOfretTb As New GET_MATERIAL_ATP.BAPIWMDVS
                            rOfretTb.Req_Date = Now.ToString("yyyyMMdd") : rOfretTb.Req_Qty = 999 : retTb.Add(rOfretTb)
                            p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
                            p1.Connection.Open()
                            p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", row("PartNo"), Left(SalesOrg, 2) + "H1", "", "", "", "", "PC",
                                                   "", intInventory, "", "", New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)
                            p1.Connection.Close()
                            For i As Integer = 0 To atpTb.Count - 1
                                If atpTb(i).Com_Qty > 0 Then
                                    ATPQty = atpTb(i).Com_Qty : _isphaseout = False
                                End If
                            Next
                            If ATPQty = 0 Then _isphaseout = True
                        Case "I"
                            _isphaseout = True
                        Case Else
                            _isphaseout = True
                    End Select
                    '\ ming test for SQF-P10S2-4G-CTE
                    'If row("PartNo").ToString.Equals("SQF-P10S2-4G-CTE") Then
                    '    _isphaseout = True
                    'End If
                    ' / end 
                    row("isPhaseout") = _isphaseout
                    row("ATPQty") = ATPQty
                Else
                    row("isPhaseout") = False
                    row("StatusCode") = ""
                    row("StatusDesc") = ""
                    row("ATPQty") = 0
                End If
                If _isphaseout AndAlso Not _anyphaseout Then
                    _anyphaseout = _isphaseout
                End If
            Next
        End If
        Return _anyphaseout
    End Function
#End Region
    Public Shared Function GetPriceRFC_ABR(ByVal Mandt As String, ByVal Vkorg As String, ByVal Kunnr As String, ByVal Matnr As String, ByVal Mglme As Integer, ByRef p_fltList_Price As Decimal, ByRef p_fltUnit_Price As Decimal) As Integer

        Dim proxy1 As New Z_EBIZAEU_PRICEINQUIRY_BR.Z_EBIZAEU_PRICEINQUIRY_BR
        Dim zssD_01Table1 As New Z_EBIZAEU_PRICEINQUIRY_BR.ZSSD_01Table
        Dim zssD_02Table1 As New Z_EBIZAEU_PRICEINQUIRY_BR.ZSSD_02Table
        Dim distr_chann As String = "10"
        Dim Division As String = "00"

        Try

            'Dim dest As New Destination
            'dest.AppServerHost = "172.20.1.1"
            'dest.Client = "168"
            'dest.Language = ""
            'dest.Password = "aclacl"
            'dest.SystemNumber = "01"
            'dest.Username = "b2bacl"
            'dest.AppServerHost = SAPIP
            'dest.Client = "168"
            'dest.Language = ""
            'dest.Password = SAPPWD
            'dest.SystemNumber = SAPSN
            'dest.Username = SAPID
            proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD")) 'dest.ConnectionString
            proxy1.Connection.Open()
            'set RFC parameter
            Dim zssd_1 As New Z_EBIZAEU_PRICEINQUIRY_BR.ZSSD_01
            zssd_1.Kunnr = Kunnr
            zssd_1.Mandt = Mandt
            zssd_1.Matnr = Format2SAPItem(Matnr)
            zssd_1.Mglme = CType(Mglme, Decimal)
            zssd_1.Vkorg = Vkorg

            zssD_01Table1 = New Z_EBIZAEU_PRICEINQUIRY_BR.ZSSD_01Table
            zssD_01Table1.Add(zssd_1)

            proxy1.Z_Ebizaeu_Priceinquiry_Br("ZORB", distr_chann, Division, Kunnr.Trim().ToUpper(), Vkorg.Trim().ToUpper(), Kunnr.Trim().ToUpper(), New Z_EBIZAEU_PRICEINQUIRY_BR.BAPIRETURN, zssD_01Table1, zssD_02Table1)
            proxy1.Connection.Close()


            p_fltList_Price = zssD_02Table1.Item(0).Kzwi1
            p_fltUnit_Price = zssD_02Table1.Item(0).Netwr

            If (p_fltList_Price = 0) Then
                p_fltList_Price = -1
            End If

            If (p_fltUnit_Price = 0) Then
                p_fltUnit_Price = -1
            End If

            If p_fltList_Price < p_fltUnit_Price Then
                p_fltList_Price = p_fltUnit_Price
            End If

        Catch exception1 As Exception
            proxy1.Connection.Close()
            p_fltList_Price = -1
            p_fltUnit_Price = -1
            Return -1
        End Try

        Return 0

    End Function
    Public Shared Function Update_Quotation_BR(ByVal sales_org As String, ByVal strQuotation As String, ByVal strQuot As String, ByRef strMsg As String) As Integer
        Try
            Dim strConditionType = ""
            Dim strConditionRate = ""

            Dim quot_change As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPI_CUSTOMERQUOTATION_CHANGE
            Dim header As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDH1
            Dim partner As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPIPARNR
            Dim tblpartner As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPIPARNRTable
            'Dim item_in As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDITM
            Dim tblitem_in As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDITMTable
            Dim tblcond As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICONDTable

            Dim sdh1x As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDH1X

            Dim tblcondx As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICONDXTable
            Dim tblparex As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPIPAREXTable
            Dim tbladdr1 As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPIADDR1Table
            Dim tblparnrc As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPIPARNRCTable
            Dim tblcublb As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICUBLBTable
            Dim tblcuins As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICUINSTable
            Dim tblcuprt As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICUPRTTable
            Dim tblcucfg As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICUCFGTable
            Dim tblcuref As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICUREFTable
            Dim tblcuval As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICUVALTable
            Dim tblcuvk As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICUVKTable
            Dim tblsditmx As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDITMXTable
            Dim tblsdkey As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDKEYTable
            Dim tblsdtext As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDTEXTTable
            Dim tblret2 As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPIRET2Table
            Dim tblschdl As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISCHDLTable
            Dim tblschdlx As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISCHDLXTable
            Dim sdls As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDLS

            Dim xmlDoc As New System.Xml.XmlDocument
            xmlDoc.LoadXml(strQuotation)

            Dim OrderNode As System.Xml.XmlNode = Nothing
            Dim subOrderNode As System.Xml.XmlNode = Nothing
            Dim i As Integer = 0
            OrderNode = xmlDoc.DocumentElement


            For Each subOrderNode In OrderNode.ChildNodes
                Select Case (UCase(subOrderNode.Name))

                    Case ("UNICODE_ID")
                        'nothing
                    Case ("COMPANY_ID")
                        partner.Partn_Role = "SP"
                        partner.Partn_Numb = UCase(subOrderNode.InnerText)
                        partner.Country = "SP"
                        tblpartner.Add(partner)
                    Case ("COMPANY_NAME")

                        'nothing
                    Case ("PO")
                        header.Purch_No_C = UCase(subOrderNode.InnerText)
                    Case ("PAYMENT_TERM")
                        header.Pmnttrms = UCase(subOrderNode.InnerText)
                    Case ("INCOTERM")
                        header.Incoterms1 = UCase(subOrderNode.InnerText)
                    Case ("INCOTERM2")
                        header.Incoterms2 = UCase(subOrderNode.InnerText)
                    Case ("PO_DATE")
                        If UCase(subOrderNode.InnerText).Length > 0 Then
                            header.Purch_Date = Convert.ToDateTime(UCase(subOrderNode.InnerText)).ToString("yyyy/MM/dd")
                        End If
                    Case ("TAX_TYPE")

                        'header.Doc_Type = UCase(subOrderNode.InnerText)
                        'Case ("CONDITION_TYPE")
                        'strConditionType = UCase(subOrderNode.InnerText)
                        'Case ("CONDITION_RATE")
                        'strConditionRate = UCase(subOrderNode.InnerText)
                    Case ("VALIDFROM")
                        header.Qt_Valid_F = Convert.ToDateTime(UCase(subOrderNode.InnerText)).ToString("yyyy/MM/dd")
                    Case ("VALIDTO")
                        header.Qt_Valid_T = Convert.ToDateTime(UCase(subOrderNode.InnerText)).ToString("yyyy/MM/dd")
                    Case ("PRICING_DATE")
                        'header.Price_Date = Convert.ToDateTime(UCase(subOrderNode.InnerText)).ToString("yyyy/MM/dd")

                    Case ("DETAIL")
                        'Dim itemIn As New Bapi_Quotation_Create.BAPIITEMIN
                        Dim itemIn As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDITM
                        Dim itmeInx As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPISDITMX
                        Dim condIn As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICOND
                        Dim condInx As New BAPI_CUSTOMERQUOTATION_CHANGE.BAPICONDX

                        Dim subOrderLineNode As System.Xml.XmlNode
                        i = i + 1
                        itemIn.Itm_Number = i.ToString()



                        'itemIn.Cond_P_Unt = "10"
                        'itemIn.J_1bcfop = "5102AA"

                        'itemIn.J_1btxsdc = "Z3"
                        For Each subOrderLineNode In subOrderNode
                            Select Case (UCase(subOrderLineNode.Name))
                                Case ("CONDITION_TYPE")
                                    strConditionType = UCase(subOrderLineNode.InnerText)
                                Case ("CONDITION_RATE")
                                    If UCase(subOrderLineNode.InnerText) = "" Then
                                        strConditionRate = "0"
                                    Else
                                        strConditionRate = UCase(subOrderLineNode.InnerText)
                                    End If
                                Case ("ITEM_NO")
                                    condIn.Itm_Number = UCase(subOrderLineNode.InnerText)
                                    itmeInx.Itm_Number = UCase(subOrderLineNode.InnerText)
                                    condIn.Cond_P_Unt = "10"
                                    itemIn.Itm_Number = UCase(subOrderLineNode.InnerText)
                                    condInx.Itm_Number = UCase(subOrderLineNode.InnerText)
                                Case ("HLV_NO")
                                    itemIn.Hg_Lv_Item = UCase(subOrderLineNode.InnerText)
                                Case ("MATERIAL_NO")
                                    itemIn.Material = UCase(subOrderLineNode.InnerText)
                                    If strConditionRate <> "0" Then
                                        If strConditionType = "ZK06" Then
                                            condIn.Cond_Type = "ZK06"
                                            condIn.Cond_Value = Convert.ToDecimal(strConditionRate)
                                            'itemIn.Cd_Type2 = "ZK06"
                                            'itemIn.Cd_Value2 = Convert.ToDecimal(strConditionRate)
                                        Else
                                            condIn.Cond_Type = "ZKB6"
                                            condIn.Cond_Value = Convert.ToDecimal(strConditionRate)
                                            'itemIn.Cd_Type3 = "ZKB6"
                                            'itemIn.Cd_Value3 = Convert.ToDecimal(strConditionRate)
                                        End If
                                    End If
                                    condInx.Cond_Value = "X"
                                Case ("QTY")
                                    itemIn.Target_Qty = Convert.ToInt16(subOrderLineNode.InnerText) * 1000

                                    itmeInx.Target_Qty = "X"
                                    'itemIn.Req_Qty = Convert.ToInt16(subOrderLineNode.InnerText) * 1000
                                Case ("PRICE")

                                    'If itemIn.Cond_Type = "ZPN0" Then
                                    'itemIn.Cond_Value = Convert.ToDecimal(subOrderLineNode.InnerText)
                                    'End If
                                Case ("PRICE_TYPE")
                                    'itemIn.Cond_Type = UCase(subOrderLineNode.InnerText)
                            End Select
                        Next
                        tblitem_in.Add(itemIn)
                        tblsditmx.Add(itmeInx)
                        tblcondx.Add(condInx)
                        tblcond.Add(condIn)
                End Select
            Next
            sdh1x.Updateflag = "U"

            'Set Connection information
            'Me.destination1 = New Destination
            'Me.destination1.AppServerHost = SAPIP
            'Me.destination1.Client = "168"
            'Me.destination1.Language = ""
            'Me.destination1.Password = SAPPWD
            'Me.destination1.SystemNumber = SAPSN
            'Me.destination1.Username = SAPID

            quot_change.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD")) 'Me.destination1.ConnectionString
            quot_change.Connection.Open()

            quot_change.Timeout = 99999999
            quot_change.Bapi_Customerquotation_Change("", "", sdls, header, sdh1x, strQuot.PadLeft(10, "0"), "", tblcond, tblcondx, tblparex, tbladdr1, tblparnrc, tblpartner, tblcublb, tblcuins, tblcuprt, tblcucfg, tblcuref, tblcuval, tblcuvk, tblitem_in, tblsditmx, tblsdkey, tblsdtext, tblret2, tblschdl, tblschdlx)
            quot_change.Connection.Close()
            tblitem_in.ToADODataTable().WriteXml("C:\XMLLOG\ITEM_IN.XML")
            tblcond.ToADODataTable().WriteXml("C:\XMLLOG\COND_IN.XML")
            tblcondx.ToADODataTable().WriteXml("C:\XMLLOG\COND_INX.XML")
            tblsditmx.ToADODataTable().WriteXml("C:\XMLLOG\ITEM_INX.XML")
            tblret2.ToADODataTable().WriteXml("C:\XMLLOG\Message.XML")
            'strMsg = ret1.Message.ToString()


        Catch ex As Exception
            strMsg = ex.ToString()
        End Try
    End Function
    Public Shared Function Create_Quotation_BR(ByVal sales_org As String, ByVal strQuotation As String, ByRef strQuot As String, ByRef strMsg As String) As Integer
        Dim strDebug As String = ""
        Try
            Dim without_commit As String = ""





            Dim quot_create As New Bapi_Quotation_Create.Bapi_Quotation_Create
            Dim header As New Bapi_Quotation_Create.BAPISDHEAD
            Dim payer As New Bapi_Quotation_Create.BAPIPAYER
            Dim ret1 As New Bapi_Quotation_Create.BAPIRETURN1
            Dim shipTo As New Bapi_Quotation_Create.BAPISHIPTO
            Dim soldTo As New Bapi_Quotation_Create.BAPISOLDTO
            Dim tblCUINS As New Bapi_Quotation_Create.BAPICUINSTable
            Dim tblCUPR As New Bapi_Quotation_Create.BAPICUPRTTable
            Dim tblCUCFG As New Bapi_Quotation_Create.BAPICUCFGTable
            Dim tblCUVAL As New Bapi_Quotation_Create.BAPICUVALTable
            Dim tblItemIn As New Bapi_Quotation_Create.BAPIITEMINTable
            Dim tblitemEx As New Bapi_Quotation_Create.BAPIITEMEXTable
            Dim tblPartner As New Bapi_Quotation_Create.BAPIPARTNRTable
            Dim partner As New Bapi_Quotation_Create.BAPIPARTNR


            Dim strConditionType = ""
            Dim strConditionRate = ""

            'header.Doc_Type = "ZQT"
            header.Sales_Org = sales_org
            header.Distr_Chan = "10"
            header.Division = "00"
            header.Sales_Grp = "991"
            header.Sales_Off = "9900"
            header.Req_Date_H = System.DateTime.Now.ToString("yyyy/MM/dd")

            Dim xmlDoc As New System.Xml.XmlDocument
            xmlDoc.LoadXml(strQuotation)

            Dim OrderNode As System.Xml.XmlNode = Nothing
            Dim subOrderNode As System.Xml.XmlNode = Nothing
            Dim i As Integer = 0
            OrderNode = xmlDoc.DocumentElement

            For Each subOrderNode In OrderNode.ChildNodes
                Select Case (UCase(subOrderNode.Name))
                    Case ("UNICODE_ID")
                        'nothing
                    Case ("COMPANY_ID")
                        partner.Partn_Role = "SP"
                        partner.Partn_Numb = UCase(subOrderNode.InnerText)
                        partner.Country = "SP"
                        tblPartner.Add(partner)
                    Case ("COMPANY_NAME")

                        'nothing
                    Case ("CURRENCY")
                        header.Currency = UCase(subOrderNode.InnerText).Trim
                    Case ("PO")
                        header.Purch_No = UCase(subOrderNode.InnerText)
                    Case ("PAYMENT_TERM")
                        header.Pmnttrms = UCase(subOrderNode.InnerText)
                    Case ("INCOTERM")
                        header.Incoterms1 = UCase(subOrderNode.InnerText)
                    Case ("INCOTERM2")
                        header.Incoterms2 = UCase(subOrderNode.InnerText)
                    Case ("PO_DATE")
                        If UCase(subOrderNode.InnerText).Length > 0 Then
                            header.Purch_Date = Convert.ToDateTime(UCase(subOrderNode.InnerText)).ToString("yyyy/MM/dd")
                        End If
                    Case ("TAX_TYPE")
                        header.Doc_Type = UCase(subOrderNode.InnerText)
                        'Case ("CONDITION_TYPE")
                        'strConditionType = UCase(subOrderNode.InnerText)
                        'Case ("CONDITION_RATE")
                        'strConditionRate = UCase(subOrderNode.InnerText)
                    Case ("VALIDFROM")
                        header.Qt_Valid_F = Convert.ToDateTime(UCase(subOrderNode.InnerText)).ToString("yyyy/MM/dd")
                    Case ("VALIDTO")
                        header.Qt_Valid_T = Convert.ToDateTime(UCase(subOrderNode.InnerText)).ToString("yyyy/MM/dd")
                    Case ("PRICING_DATE")
                        'header.Price_Date = Convert.ToDateTime(UCase(subOrderNode.InnerText)).ToString("yyyy/MM/dd")

                    Case ("DETAIL")
                        Dim itemIn As New Bapi_Quotation_Create.BAPIITEMIN
                        Dim subOrderLineNode As System.Xml.XmlNode
                        i = i + 1
                        itemIn.Itm_Number = i.ToString()


                        itemIn.Cond_P_Unt = "10"
                        itemIn.J_1bcfop = "5102AA"
                        itemIn.J_1btxsdc = "Z3"
                        itemIn.Currency = header.Currency
                        For Each subOrderLineNode In subOrderNode
                            Select Case (UCase(subOrderLineNode.Name))
                                Case ("CONDITION_TYPE")
                                    strConditionType = UCase(subOrderLineNode.InnerText)
                                Case ("CONDITION_RATE")
                                    If UCase(subOrderLineNode.InnerText) = "" Then
                                        strConditionRate = "0"
                                    Else
                                        strConditionRate = UCase(subOrderLineNode.InnerText)
                                    End If
                                Case ("ITEM_NO")
                                    itemIn.Itm_Number = UCase(subOrderLineNode.InnerText)
                                Case ("HLV_NO")
                                    itemIn.Hg_Lv_Item = UCase(subOrderLineNode.InnerText)
                                Case ("MATERIAL_NO")

                                    itemIn.Material = UCase(subOrderLineNode.InnerText)
                                    If strConditionRate <> "0" Then
                                        If strConditionType = "ZK06" Then
                                            itemIn.Cd_Type2 = "ZK06"
                                            itemIn.Cd_Value2 = Convert.ToDecimal(strConditionRate)
                                        Else
                                            itemIn.Cd_Type3 = "ZKB6"
                                            itemIn.Cd_Value3 = Convert.ToDecimal(strConditionRate)
                                        End If
                                    End If
                                Case ("QTY")
                                    itemIn.Req_Qty = Convert.ToInt16(subOrderLineNode.InnerText) * 1000
                                Case ("PRICE")
                                    If itemIn.Cond_Type = "ZPN0" Then
                                        itemIn.Cond_Value = Convert.ToDecimal(subOrderLineNode.InnerText)
                                    End If
                                Case ("PRICE_TYPE")
                                    itemIn.Cond_Type = UCase(subOrderLineNode.InnerText)
                            End Select
                        Next
                        tblItemIn.Add(itemIn)
                End Select
            Next




            ''Set Connection information
            'Me.destination1 = New Destination
            'Me.destination1.AppServerHost = SAPIP
            ''Me.destination1.AppServerHost = "172.20.1.1"
            'Me.destination1.Client = "168"
            'Me.destination1.Language = ""
            'Me.destination1.Password = SAPPWD
            'Me.destination1.SystemNumber = SAPSN
            ''Me.destination1.SystemNumber = "01"
            'Me.destination1.Username = SAPID

            quot_create.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            quot_create.Connection.Open()

            quot_create.Timeout = 99999999
            quot_create.Bapi_Quotation_Createfromdata("X", header, without_commit, payer, ret1, strQuot, shipTo, soldTo, tblCUINS, tblCUPR, tblCUCFG, tblCUVAL, tblItemIn, tblitemEx, tblPartner)
            quot_create.Connection.Close()
            strMsg = ret1.Message.ToString()


        Catch ex As Exception
            strMsg = ex.ToString() & " strDebug= " & strDebug
        End Try



    End Function
    Public Shared Function Create_Quotation_PDF(ByVal quotation_id As String, ByRef strMsg As String) As Integer

        Try

            Dim quopdf As New ZSD_QUOTATION_ABR.ZSD_QUOTATION_ABR
            Dim retTbl As New ZSD_QUOTATION_ABR.BAPIRETURN1Table


            'Set Connection information
            'Me.destination1 = New Destination
            'Me.destination1.AppServerHost = SAPIP
            'Me.destination1.Client = "168"
            'Me.destination1.Language = ""
            'Me.destination1.Password = SAPPWD
            'Me.destination1.SystemNumber = SAPSN
            'Me.destination1.Username = SAPID

            quopdf.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            quopdf.Connection.Open()
            quopdf.Timeout = 99999999
            quopdf.Zsd_Quotation_Abr("2", quotation_id, retTbl)
            quopdf.Connection.Close()

        Catch ex As Exception
            strMsg = ex.ToString

        End Try



    End Function
End Class

Public Class GlobalATP
    Dim gdt As DataTable, _pn As String, _plants As String
    Public rdt As DataTable
    Public Sub New(ByVal PN As String, ByVal Plants As String)
        _pn = Trim(UCase(PN)) : _plants = Plants
    End Sub
    Public Sub Query()
        Try
            rdt = Query(_plants, _pn)
        Catch ex As Exception

        End Try
    End Sub
    Public Function Query(
    ByVal PlantArray As String, ByVal PartNo As String, ByVal maximumRows As Integer, ByVal startRowIndex As Integer,
    ByVal SortExpression As String, ByVal Direction As UI.WebControls.SortDirection) As DataTable
        If gdt Is Nothing Then
            gdt = New DataTable
            gdt.Columns.Add("plant") : gdt.Columns.Add("atp_date") : gdt.Columns.Add("atp_qty", Type.GetType("System.Double"))
        Else
            gdt.Clear()
        End If
        PartNo = Trim(PartNo).ToUpper()
        If PartNo = "" Then Return Nothing
        Dim plants() As String = Split(PlantArray, ",")
        Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP
        p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        p1.Connection.Open()
        For Each plant As String In plants
            plant = Trim(plant).ToUpper()
            Dim retTb As New GET_MATERIAL_ATP.BAPIWMDVSTable, atpTb As New GET_MATERIAL_ATP.BAPIWMDVETable
            p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", PartNo, plant, "", "", "", "", "PC", "", 9999, "", "",
                                          New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)
            Dim adt As DataTable = atpTb.ToADODataTable()
            For Each r As DataRow In adt.Rows
                If r.Item(4) > 0 And r.Item(4) < 99999999 Then
                    Dim r2 As DataRow = gdt.NewRow
                    r2.Item("plant") = plant
                    r2.Item("atp_date") = Date.ParseExact(r.Item(3).ToString(), "yyyyMMdd", New Globalization.CultureInfo("fr-FR")).ToString("yyyy/MM/dd")
                    r2.Item("atp_qty") = CDbl(r.Item(4))
                    gdt.Rows.Add(r2)
                End If
            Next
        Next
        p1.Connection.Close()
        If gdt IsNot Nothing AndAlso gdt.Rows.Count > 0 Then
            Dim ndt As DataTable = gdt.Copy()
            If SortExpression <> "" Then
                If Direction = UI.WebControls.SortDirection.Ascending Then
                    ndt.DefaultView.Sort = SortExpression + " asc"
                Else
                    ndt.DefaultView.Sort = SortExpression + " desc"
                End If
                ndt = gdt.DefaultView.ToTable()
            End If
            Return ndt
        Else
            Return Nothing
        End If
    End Function

    Public Function Query(ByVal PlantArray As String, ByVal PartNo As String) As DataTable
        Return Query(PlantArray, PartNo, 0, 0, "", UI.WebControls.SortDirection.Descending)
    End Function




End Class
