Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports SAP.Connector
Imports Z_GET_ATP_LIMITQTY
Imports System.Configuration
Imports System.Globalization
Imports System.Web.Caching

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
Public Class SAPDAL
    Inherits System.Web.Services.WebService

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
        p1.Zsd_Customer_Maintain_All(I_Bapiaddr1, I_Bapiaddr2, I_Customer_Is_Consumer, _
                               I_Force_External_Number_Range, I_From_Customermaster, _
                               I_Kna1, I_Knb1, I_Knb1_Reference, I_Knvv, I_Maintain_Address_By_Kna1, _
                               I_No_Bank_Master_Update, I_Raise_No_Bte, _
                               Pi_Add_On_Data, Pi_Cam_Changed, Pi_Postflag, _
                               E_Kunnr, E_Sd_Cust_1321_Done, O_Kna1, T_Upd_Txt, _
                               T_Xkn, T_Xknb5, T_Xknbk, T_Xknex, T_Xknva, T_Xknvd, T_Xknvi, _
                               T_Xknvk, T_Xknvl, T_Xknvp, T_Xknza, T_Ykn, T_Yknb5, T_Yknbk, T_Yknex, T_Yknva, _
                               T_Yknvd, T_Yknvi, T_Yknvk, T_Yknvl, T_Yknvp, T_Yknza)
        p1.CommitWork()
        Return True
    End Function
    Public Function SimulateSO(ByRef refDoc_Number As String, ByRef ErrMsg As String, _
                            ByRef OrderHeaderDt As SalesOrder.OrderHeaderDataTable, ByRef OrderLineDt As SalesOrder.OrderLinesDataTable, _
                            ByRef PartnerFuncDT As SalesOrder.PartnerFuncDataTable, ByRef ConditionDT As SalesOrder.ConditionDataTable, _
                            ByRef HeaderTextsDt As SalesOrder.HeaderTextsDataTable, ByRef CreditCardDT As SalesOrder.CreditCardDataTable, _
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
                S_OrderLineDt.Add(S_OrderLineRow) : S_ScheLineDT.Add(S_ScheLineRow) : S_ConditionDT.Add(S_ConditionRow)

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

            proxy1.Bapi_Salesorder_Simulate("", S_OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, _
                                            "", New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, _
                                            retTable, S_CreditCardDT, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, _
                                            S_ConditionDT, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, S_OrderLineDt, New BAPI_SALESORDER_SIMULATE.BAPIITEMEXTable, _
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
    Function getPriceByProdLine(ByVal PartNo As String, ByVal prodLine As String, Optional ByRef errmsg As String = "") As Decimal
        'If prodLine = "20" Then
        '    companyId = "UAON5005"
        'End If
        If IsNumeric(prodLine) AndAlso CDec(prodLine) = 10 Then
            Dim companyId As String = "UAON00001"
            Dim pQty As Integer = 29
            Dim DTPRICE As DataTable = Nothing
            getSAPPriceByTable(PartNo, "US01", companyId, "USD", DTPRICE, errmsg, pQty)
            If DTPRICE.Rows.Count > 0 Then
                Return FormatNumber(DTPRICE.Rows(0).Item("Netwr"), 2).Replace(",", "")
            End If
        ElseIf IsNumeric(prodLine) AndAlso CDec(prodLine) = 20 Then
            Return getCostForANAPn(PartNo, "USH1") * 1.25
            'Return 4 * getCostForANAPn(PartNo, "USH1") / 3 '(p-c)/p>1/4 ==>p>4c/3
        End If
        Return 0
    End Function

    Shared Function Format2SAPItem(ByVal Part_No As String) As String
        'Try
        If IsNumeric(Part_No) And Not Part_No.Substring(0, 1).Equals("0") Then
            Dim zeroLength As Integer = 18 - Part_No.Length
            For i As Integer = 0 To zeroLength - 1
                Part_No = "0" & Part_No
            Next
            Return Part_No
        Else
            Return Part_No
        End If
        'Catch ex As Exception
        '    Return Part_No
        'End Try
    End Function
    Shared Function testU(ByVal user As String) As Boolean
        Return True
        If user.ToLower.Contains("james.wu") Or
            user.ToLower.Contains("nada.liu") Then
            Return True
        End If
        Return False
    End Function
    Function getSAPPriceByTable(ByVal partNoStr As String, ByVal org As String, ByVal company As String, ByVal Currency As String, ByRef retTable As DataTable, Optional ByRef errMsg As String = "", Optional ByVal qty As Integer = 1) As Integer

        'Dim DTIN As New MySAPDALWS.SAPDALDS.ProductInDataTable, DTOUT As New MySAPDALWS.SAPDALDS.ProductOutDataTable
        Dim DTIN As New SAPDALDS.ProductInDataTable, DTOUT As New SAPDALDS.ProductOutDataTable

        Dim part_noArr() As String = partNoStr.Trim().Trim("|").Split("|")
        For Each p As String In part_noArr
            'Dim R As MySAPDALWS.SAPDALDS.ProductInRow = DTIN.NewRow
            Dim R As SAPDALDS.ProductInRow = DTIN.NewRow
            R.PART_NO = Format2SAPItem(p.Trim).TrimStart("0")
            R.QTY = qty
            DTIN.Rows.Add(R)
        Next
        'Dim ws As New MySAPDALWS.MYSAPDAL
        GetPrice(company, company, org, Currency, DTIN, DTOUT, errMsg)
        'Util.showDT(DTOUT)
        Dim printDT As New DataTable
        printDT.Columns.Add("MATNR") : printDT.Columns.Add("Kzwi1") : printDT.Columns.Add("Netwr") : printDT.Columns.Add("RECYCLE_FEE") : printDT.Columns.Add("TAX")
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
            printDT.Rows.Add(pr)
        Next
        'Util.showDT(printDT)
        retTable = printDT
        Return 1
    End Function
    Public Function CreateQuotation( _
                        ByRef refDoc_Number As String, ByRef ErrMsg As String, _
                        ByRef OrderHeaderDt As SalesOrder.OrderHeaderDataTable, _
                        ByRef OrderLineDt As SalesOrder.OrderLinesDataTable, _
                        ByRef PartnerFuncDT As SalesOrder.PartnerFuncDataTable, _
                        ByRef ConditionDT As SalesOrder.ConditionDataTable, _
                        ByRef HeaderTextsDt As SalesOrder.HeaderTextsDataTable, _
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

            proxy1.Bapi_Quotation_Createfromdata2( _
            ErrMsg, strRelationType, strPConvert, _
            strpintnumassign, New Quotation_Create_SAP.BAPISDLS, S_OrderHeader, New Quotation_Create_SAP.BAPISDHD1X, _
            refDoc_Number, New Quotation_Create_SAP.BAPI_SENDER, strPTestRun, refDoc_Number, _
            New Quotation_Create_SAP.BAPIPAREXTable, New Quotation_Create_SAP.BAPIADDR1Table, _
            New Quotation_Create_SAP.BAPICUBLBTable, New Quotation_Create_SAP.BAPICUINSTable, _
            New Quotation_Create_SAP.BAPICUPRTTable, New Quotation_Create_SAP.BAPICUCFGTable, _
            New Quotation_Create_SAP.BAPICUREFTable, New Quotation_Create_SAP.BAPICUVALTable, _
            New Quotation_Create_SAP.BAPICUVKTable, S_ConditionDT, New Quotation_Create_SAP.BAPICONDXTable, _
            S_OrderLineDt, New Quotation_Create_SAP.BAPISDITMXTable, New Quotation_Create_SAP.BAPISDKEYTable, _
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
        If HttpContext.Current.User.Identity.IsAuthenticated AndAlso HttpContext.Current.User.Identity.Name IsNot Nothing AndAlso _
            HttpContext.Current.User.Identity.Name <> "" Then userid = HttpContext.Current.User.Identity.Name
        Dim iUrl As String = Left(HttpContext.Current.Request.ServerVariables("URL").Replace("'", "''"), 500), iQString As String = ""
        If HttpContext.Current.Request.QueryString.HasKeys Then
            For i As Integer = 0 To HttpContext.Current.Request.QueryString.Count - 1
                iQString += HttpContext.Current.Request.QueryString.Keys(i) & "=" & _
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


        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MyLocal").ConnectionString)
        Dim cmd As New SqlClient.SqlCommand("INSERT INTO MY_ERR_LOG (ROW_ID, USERID, URL, QSTRING, EXMSG, APPID, CLIENT_INFO) VALUES (@UNIQID, @UID, @URL, @REQSTR, @ERRMSG, 'MY', @CLIENTINFO)", conn)

        With cmd.Parameters
            .AddWithValue("UNIQID", Left(System.Guid.NewGuid().ToString().Replace("-", ""), 10)) : .AddWithValue("UID", userid)
            .AddWithValue("URL", iUrl) : .AddWithValue("REQSTR", iQString) : .AddWithValue("ERRMSG", strEx) : .AddWithValue("CLIENTINFO", _HTTP_USER_AGENT)
        End With
        conn.Open() : cmd.ExecuteNonQuery() : conn.Close()
        'Catch ex As Exception

        'End Try
    End Sub
    Public Function CreateSO(ByRef refDoc_Number As String, ByRef ErrMsg As String, _
                            ByRef OrderHeaderDt As SalesOrder.OrderHeaderDataTable, ByRef OrderLineDt As SalesOrder.OrderLinesDataTable, _
                            ByRef PartnerFuncDT As SalesOrder.PartnerFuncDataTable, ByRef ConditionDT As SalesOrder.ConditionDataTable, _
                            ByRef HeaderTextsDt As SalesOrder.HeaderTextsDataTable, ByRef CreditCardDT As SalesOrder.CreditCardDataTable, _
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



            proxy1.Bapi_Salesorder_Createfromdat2( _
            ErrMsg, strRelationType, strPConvert, _
            strpintnumassign, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDLS, S_OrderHeader, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1X, _
            refDoc_Number, New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SENDER, strPTestRun, refDoc_Number, _
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPAREXTable, S_CreditCardDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUBLBTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUINSTable, _
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUPRTTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUCFGTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUREFTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVALTable, _
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVKTable, S_ConditionDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDXTable, _
            S_OrderLineDt, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMXTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDKEYTable, _
            S_PartnerFuncDT, S_ScheLineDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISCHDLXTable, S_HeaderTextsDt, New BAPI_SALESORDER_CREATEFROMDAT2.BAPIADDR1Table, retTable)
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
            Return False
        End Try

        If FF = 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function UpdateSAPSOShipToAttention(ByVal SONO As String, ByVal ShipToId As String, ByVal Attention As String, ByRef ReturnTable As DataTable, _
                                       Optional ByVal IsSAPProductionServer As Boolean = True) As Boolean
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
    End Function
    Public Function CreateSOV2(ByRef refDoc_Number As String, ByRef ErrMsg As String, _
                            ByRef OrderHeaderDt As SalesOrder.OrderHeaderDataTable, ByRef OrderLineDt As SalesOrder.OrderLinesDataTable, _
                            ByRef PartnerFuncDT As SalesOrder.PartnerFuncDataTable, ByVal PartnerAddressDT As SalesOrder.PartnerAddressesDataTable, ByRef ConditionDT As SalesOrder.ConditionDataTable, _
                            ByRef HeaderTextsDt As SalesOrder.HeaderTextsDataTable, ByRef CreditCardDT As SalesOrder.CreditCardDataTable, _
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



            proxy1.Bapi_Salesorder_Createfromdat2( _
            ErrMsg, strRelationType, strPConvert, _
            strpintnumassign, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDLS, S_OrderHeader, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDHD1X, _
            refDoc_Number, New BAPI_SALESORDER_CREATEFROMDAT2.BAPI_SENDER, strPTestRun, refDoc_Number, _
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPIPAREXTable, S_CreditCardDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUBLBTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUINSTable, _
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUPRTTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUCFGTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUREFTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVALTable, _
            New BAPI_SALESORDER_CREATEFROMDAT2.BAPICUVKTable, S_ConditionDT, New BAPI_SALESORDER_CREATEFROMDAT2.BAPICONDXTable, _
            S_OrderLineDt, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDITMXTable, New BAPI_SALESORDER_CREATEFROMDAT2.BAPISDKEYTable, _
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
#End Region

#Region "PRICING"

    <WebMethod()> _
    Public Function GetListPrice(ByVal SAPOrg As String, ByVal SiebelOrg As String, ByVal Currency As String, _
                                 ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOutDataTable, _
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
                cmd = New SqlClient.SqlCommand( _
                     "select top 1 LIST_PRICE from PRODUCT_LIST_PRICE " + _
                    " where ORG='EU10' and PART_NO=@PN and CURRENCY=@CUR and LIST_PRICE>0", eQConn)

                cmd.Parameters.AddWithValue("PN", pinRec.PART_NO) : cmd.Parameters.AddWithValue("CUR", pinRec.PART_NO)
                Dim tmpLP As Object = cmd.ExecuteScalar()
                If tmpLP IsNot Nothing AndAlso Double.TryParse(tmpLP, 0) Then
                    tmpProdOut.AddProductOutRow(pinRec.PART_NO, tmpLP.ToString(), tmpLP.ToString(), "0", "0")
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
            If GetPrice(strERPID, strERPID, strOrgId, "", ProductIn, ProductOut, ErrorMessage) Then
                ProductOut.Merge(tmpProdOut)
                Return True
            Else
                Return False
            End If
        End If
    End Function

    <WebMethod()> _
    Public Function GetPriceV2(ByVal SoldToId As String, ByVal ShipToId As String, ByVal Org As String, ByVal DocOrderType As SAPOrderType, _
                             ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOutDataTable, _
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
            If OriPInRec.PART_NO.Equals("No Need", StringComparison.OrdinalIgnoreCase) Then
                OriPInRec.Delete()
            End If
        Next
        Try
            ErrorMessage = ""
            SoldToId = UCase(Trim(SoldToId)) : Org = Trim(UCase(Org))
            If String.IsNullOrEmpty(ShipToId) Then ShipToId = SoldToId
            Dim strDistChann As String = "10", strDivision As String = "00"
            If Org = "US01" Then
                Dim N As Integer = dbUtil.dbExecuteScalar("MY", String.Format( _
                                                          "select COUNT(COMPANY_ID) from SAP_DIMCOMPANY " + _
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
    Public Function GetPrice(ByVal SoldToId As String, ByVal ShipToId As String, ByVal Org As String, ByVal Currency As String, _
                             ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOutDataTable, _
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
        For Each OriPInRec As SAPDALDS.ProductInRow In ProductIn.Rows
            If OriPInRec.PART_NO.Equals("No Need", StringComparison.OrdinalIgnoreCase) Then
                OriPInRec.Delete()
            End If
        Next
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
                    Dim N As Integer = dbUtil.dbExecuteScalar("MY", String.Format( _
                                                         "select COUNT(COMPANY_ID) from SAP_DIMCOMPANY " + _
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
            Return GetMultiPrice_eStore(Org, SoldToId, ShipToId, Currency, strDistChann, strDivision, ProductIn, ProductOut, ErrorMessage)
            ' Else
            'Dim eup As New Get_Price.Get_Price
            'Dim pin As New Get_Price.ZSSD_01Table, pout As New Get_Price.ZSSD_02Table
            'For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
            '    Dim prec As New Get_Price.ZSSD_01
            '    With prec
            '        .Kunnr = SoldToId : .Mandt = "168" : .Matnr = Global_Inc.Format2SAPItem(PInRow.PART_NO) : .Mglme = 1 : .Vkorg = Org
            '        ' .Prsdt = Now.Date.ToString("yyyyMMdd")
            '    End With
            '    pin.Add(prec)
            'Next
            'eup.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
            'eup.Connection.Open()
            'Try
            '    eup.Z_Ebizaeu_Priceinquiry(strDistChann, strDivision, SoldToId, Org, SoldToId, New Get_Price.BAPIRETURN, pin, pout)
            'Catch ex As Exception
            '    ErrorMessage = "Call Z_Ebizaeu_Priceinquiry error:" + ex.ToString()
            '    eup.Connection.Close() : Return False
            'End Try
            'eup.Connection.Close()
            'For Each x As Get_Price.ZSSD_02 In pout
            '    If x.Kzwi1 < x.Netwr Then
            '        x.Kzwi1 = x.Netwr
            '    End If
            'Next
            'Dim retTable As DataTable = pout.ToADODataTable()
            'ProductOut = New SAPDALDS.ProductOutDataTable
            'For Each retRec As DataRow In retTable.Rows
            '    'pout.Item(0).Matnr : pout.Item(0).Netwr : pout.Item(0).Kzwi1
            '    Dim ProductOutRec As SAPDALDS.ProductOutRow = ProductOut.NewProductOutRow()
            '    ProductOutRec.PART_NO = Global_Inc.RemoveZeroString(retRec.Item("Matnr"))
            '    ProductOutRec.LIST_PRICE = retRec.Item("Kzwi1")
            '    ProductOutRec.UNIT_PRICE = retRec.Item("Netwr")
            '    ProductOut.AddProductOutRow(ProductOutRec)
            'Next
            'Return True
            'End If
        Catch ex As Exception
            ErrorMessage += ".Runtime exception:" + ex.ToString()
            Throw New Exception(ErrorMessage)
        End Try
    End Function

    Private Shared Function GetMultiPrice_eStoreV2(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String, _
                                          ByVal strDivision As String, ByVal OrderDocType As SAPOrderType, ByVal ProductIn As SAPDALDS.ProductInDataTable, _
                                          ByRef ProductOut As SAPDALDS.ProductOutDataTable, _
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
            'If Org = "BR01" Then .Doc_Type = "ZORB"
        End With
        Dim LineNo As Integer = 1
        Dim sqlMA As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        sqlMA.Open()
        For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
            Dim chkSql As String = _
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + _
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + _
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
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = "ADAM-4520-D2E"
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

            proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "", _
                                            New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, _
                                            New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, _
                                            ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable, _
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

    Public Shared Function GetMultiPrice_eStore_PricingDate(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String, _
                                         ByVal strDivision As String, ByVal OrderDocType As SAPOrderType, ByVal PricingDate As Date, ByVal ProductIn As SAPDALDS.ProductInDataTable, _
                                         ByRef ProductOut As SAPDALDS.ProductOutDataTable, _
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
            Dim chkSql As String = _
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + _
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + _
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
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = "ADAM-4520-D2E"
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

            proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "", _
                                            New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, _
                                            New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, _
                                            ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable, _
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
    Public Function OrderSimulate(ByRef ErrMsg As String, ByVal SoldTo As String, ByVal ShipTo As String, ByVal Org As String, _
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


        Dim chkSql As String = _
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + _
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + _
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


        proxy1.Bapi_Salesorder_Simulate("", S_OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "", _
                                                 New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, _
                                                 New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt, _
                                                 S_CreditCardDT, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, _
                                                 New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
                                                 New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, _
                                                 New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, S_ConditionDT, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, _
                                                 S_OrderLineDt, O_OrderLineDt, S_PartnerDT, O_ScheLineDT, _
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
    Public Shared Function GetMultiPrice_eStore(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal Currency As String, ByVal strDistChann As String, _
                                          ByVal strDivision As String, ByVal ProductIn As SAPDALDS.ProductInDataTable, _
                                          ByRef ProductOut As SAPDALDS.ProductOutDataTable, _
                                          ByRef ErrorMessage As String) As Boolean
        'Util.SendEmail("nada.liu@advantech.com.cn", "myadvanteh@advantech.com", "test price", "AAAA", True, "", "")
        SoldToId = SoldToId.ToUpper.Trim
        ShipToId = ShipToId.ToUpper.Trim
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
            .Doc_Type = "ZOR" : .Sales_Org = Trim(UCase(Org)) : .Distr_Chan = strDistChann : .Division = strDivision
            If Org = "BR01" Then .Doc_Type = "ZORB"
            If Not String.IsNullOrEmpty(Currency.Trim) Then .Currency = Currency
        End With
        Dim LineNo As Integer = 1
        Dim sqlMA As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        sqlMA.Open()
        For Each PInRow As SAPDALDS.ProductInRow In ProductIn.Rows
            Dim chkSql As String = _
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + _
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + _
                " where a.part_no='" + PInRow.PART_NO + "' and a.product_status in ('A','N','H','O','S5','V','M1','') and a.sales_org='" + Org + "' "
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
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = "ADAM-4520-D2E"
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

            proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "", _
                                            New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, _
                                            New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, _
                                            ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable, _
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

    <WebMethod()> _
    Public Function GetMultiPrice_ABR_TAX(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, _
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
            Dim chkSql As String = _
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + _
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + _
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
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = "ADAM-4520-D2E"
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

            proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "", _
                                            New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, _
                                            New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, _
                                            ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable, _
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

    <WebMethod()> _
    Public Function GetMultiPrice_ABR_TAX_2(ByVal Org As String, ByVal OrderType As SAPOrderType, ByVal SoldToId As String, ByVal ShipToId As String, _
                                           ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOut_ABRDataTable, _
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
            Dim chkSql As String = _
                " select a.part_no, a.ITEM_CATEGORY_GROUP, IsNull(b.ProfitCenter,'N/A') as ProfitCenter " + _
                " from sap_product_status a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT  " + _
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
            item.Itm_Number = FormatItmNumber(LineNo) : item.Material = "ADAM-4520-D2E"
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

            proxy1.Bapi_Salesorder_Simulate("", OrderHeader, New BAPI_SALESORDER_SIMULATE.BAPIPAYER, New BAPI_SALESORDER_SIMULATE.BAPIRETURN, "", _
                                            New BAPI_SALESORDER_SIMULATE.BAPISHIPTO, New BAPI_SALESORDER_SIMULATE.BAPISOLDTO, _
                                            New BAPI_SALESORDER_SIMULATE.BAPIPAREXTable, retDt, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICCARDTable, New BAPI_SALESORDER_SIMULATE.BAPICCARD_EXTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUBLBTable, New BAPI_SALESORDER_SIMULATE.BAPICUINSTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUPRTTable, New BAPI_SALESORDER_SIMULATE.BAPICUCFGTable, _
                                            New BAPI_SALESORDER_SIMULATE.BAPICUVALTable, Conditions, New BAPI_SALESORDER_SIMULATE.BAPIINCOMPTable, _
                                            ItemsIn, ItemsOut, Partners, New BAPI_SALESORDER_SIMULATE.BAPISDHEDUTable, _
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

    Private Shared Function GetMultiPrice_BR(ByVal SoldToId As String, ByVal ShipToId As String, _
                                      ByVal ProductIn As SAPDALDS.ProductInDataTable, ByRef ProductOut As SAPDALDS.ProductOutDataTable, _
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
            proxy1.Z_Ebizaeu_Priceinquiry_Br("ZORB", distr_chann, Division, SoldToId, Vkorg.Trim().ToUpper(), ShipToId, _
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

    Private Shared Function GetEUPrice(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String, _
                                          ByVal strDivision As String, ByVal ProductIn As SAPDALDS.ProductInDataTable, _
                                          ByRef ProductOut As SAPDALDS.ProductOutDataTable, _
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

            If pOutRec.LIST_PRICE < pOutRec.UNIT_PRICE Then pOutRec.LIST_PRICE = pOutRec.UNIT_PRICE
            pOutRec.TAX = 0
            pOutRec.RECYCLE_FEE = 0
            ProductOut.AddProductOutRow(pOutRec)
        Next
        Return True
        'Return pdt
    End Function
#End Region

#Region "Credit"

    <WebMethod()> _
    Public Function GetCustomerCreditExposure(ByVal CustomerId As String, ByVal Org As String, ByRef CreditLimit As Decimal, _
                                              ByRef CreditExposure As Decimal, ByRef Percentage As String) As Boolean
        CustomerId = Trim(UCase(CustomerId)) : Org = Trim(UCase(Org))
        Select Case Left(Org, 2)
            Case "EU", "AU", "JP", "MY", "BR", "SG", "TL", "TW"
                Org = Left(Org, 2) + "01"
            Case "CN"
                Org = Left(Org, 2) + "C1"
            Case "US"
                Org = Left(Org, 2) + "C1"
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
            p.Zcredit_Exposure(strHorizonDate, Org, CustomerId, cmware, CreditLimit, Delta2Limit, dtKnkk, Knkli, OpenDelivery, OpenDeliverySecure, _
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

#End Region

#Region "Inventory"

    <WebMethod()> _
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
    <WebMethod()> _
    Public Function QueryInventory(ByVal PartNos As SAPDALDS.ProductInDataTable, ByVal plant As String, _
                                   ByRef QueryResult As SAPDALDS.QueryInventory_OutputDataTable, _
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
    <WebMethod()> _
    Public Function QueryInventory_V2(ByVal PartNos As SAPDALDS.ProductInDataTable, ByVal plant As String, _
                                   ByVal Req_Date As Date, ByRef QueryResult As SAPDALDS.QueryInventory_OutputDataTable, _
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
                p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", _FormatedPartNo, UCase(_RowView.Item("PLANT")), "", "", "", "", "PC", _
                              "", Inventory, "", "", New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)

                'Get inventory result
                dt = atpTb.ToADODataTable()

                'write inventory information into datatable for return value
                For Each _InventoryRow As DataRow In dt.Rows

                    If IsDBNull(_InventoryRow.Item("Com_Qty")) Then Continue For
                    If _InventoryRow.Item("Com_Qty") = 0 Then Continue For

                    QueryResult.AddQueryInventory_OutputRow(_PartNo _
                                                            , DateTime.ParseExact(_InventoryRow.Item("Com_Date"), "yyyyMMdd", Nothing) _
                                                            , _InventoryRow.Item("Com_Qty") _
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
    End Enum
    Shared Function getItp(ByVal ORG As String, ByVal PARTNO As String, ByVal Currency As String, ByVal companyId As String, ByVal type As itpType) As Decimal
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
        Dim dtITP As New DataTable, InsertCache As Boolean = True
        If HttpContext.Current.Cache("PartNO_ITP") Is Nothing Then
            dtITP.Columns.Add("PartNO", GetType(String)) : dtITP.Columns.Add("ORG", GetType(String))
            dtITP.Columns.Add("Currency", GetType(String)) : dtITP.Columns.Add("ITP", GetType(Decimal))
            HttpContext.Current.Cache.Insert("PartNO_ITP", dtITP, Nothing, DateTime.Now.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        Else
            dtITP = CType(HttpContext.Current.Cache("PartNO_ITP"), DataTable)
            If dtITP.Rows.Count > 0 Then
                Dim drs As DataRow() = dtITP.Select(String.Format("PartNO= '{0}' and org ='{1}' and Currency='{2}'", PARTNO, ORG, Currency))
                If drs.Length > 0 Then
                    If drs(0).Item("ITP") IsNot Nothing AndAlso Decimal.TryParse(drs(0).Item("ITP"), 0) Then
                        ITP = Decimal.Parse(drs(0).Item("ITP"))
                        InsertCache = False
                    End If
                End If
            End If
        End If
        Dim sapdal As New SAPDAL
        If type = itpType.EU Then
            If ITP = 0 Then
                Dim sapITPDt As New DataTable, TempCurrency As String = String.Empty
                TempCurrency = Currency
                If String.Equals(Currency, "GBP", StringComparison.CurrentCultureIgnoreCase) Then
                    TempCurrency = "EUR"
                End If
                If sapdal.getSAPPriceByTable(PARTNO, "EU10", "UUAAESC", TempCurrency, sapITPDt) = 1 Then
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
                    If sapdal.getSAPPriceByTable(PARTNO, "EU10", "UUAAESC", "EUR", sapITPDt) = 1 Then
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
        ElseIf type = itpType.JP Then
            If ITP = 0 Then
                Dim sapITPDt As New DataTable
                If sapdal.getSAPPriceByTable(PARTNO, "TW01", "AJPADV", Currency, sapITPDt) = 1 Then
                    If sapITPDt.Rows.Count > 0 AndAlso CDbl(sapITPDt.Rows(0).Item("Netwr")) > 0 Then
                        ITP = CDbl(sapITPDt.Rows(0).Item("Netwr").ToString.Replace(",", ""))
                        ITP = FormatNumber(ITP, 2)
                        ITP = ITP * 1.03
                    End If
                End If
            End If
        End If
        If InsertCache AndAlso ITP > 0 Then
            Dim dr As DataRow = dtITP.NewRow
            dr.Item("PartNO") = PARTNO : dr.Item("ORG") = ORG
            dr.Item("Currency") = Currency : dr.Item("ITP") = ITP
            dtITP.Rows.Add(dr) : dtITP.AcceptChanges()
        End If
        Return ITP
    End Function
    Shared Function get_exchangerate(ByVal C_FROM As String, ByVal C_TO As String) As String
        If C_FROM = C_TO Then
            Return 1
        End If
        Dim temp As Object = Nothing
        temp = dbUtil.dbExecuteScalar("b2b", "select top 1 UKURS from SAP_EXCHANGERATE" & _
                                                     " where fCURR='" & C_FROM & "' and TCURR='" & C_TO & "' order by exch_date desc")

        If temp IsNot Nothing AndAlso temp.ToString <> "" Then
            Return temp
        End If
        Return "0.0"
    End Function
    Shared Sub writeItpBack(ByVal ORG As String, ByVal PARTNO As String, ByVal CURR As String, ByVal itp As Decimal)
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        Dim cmd As New SqlClient.SqlCommand( _
            " delete from PRODUCT_ITP where PART_NO=@PN and ORG=@ORG and CURRENCY=@CURR;" + _
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
        Dim str As String = String.Format("select count(Part_No) from sap_product where part_no='{0}' and (PRODUCT_HIERARCHY ='EAPC-INNO-DPX' or PRODUCT_HIERARCHY like 'OTHR-MEMO-%' " & _
        " or PRODUCT_HIERARCHY like 'AGSG-PAPS-%' or PRODUCT_HIERARCHY='AGSG-CTOS-ASS#' or PRODUCT_HIERARCHY ='EAPC-DLOG-DLGR')", partNo)
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
        Dim myConn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        Dim cmd As New SqlClient.SqlCommand("select top 1 ZIP_CODE from SAP_DIMCOMPANY where COMPANY_ID=@CID and country='US' ", myConn)
        cmd.Parameters.AddWithValue("CID", ERPID)
        cmd.Connection.Open()
        Dim objZip As Object = cmd.ExecuteScalar()
        cmd.Connection.Close()
        If objZip IsNot Nothing Then
            ZipCode = objZip.ToString() : Return True
        Else
            Dim strPlSql As String = _
                " select nvl(b.post_code1,'') as post_code1 from saprdp.kna1 a inner join saprdp.adrc b on a.land1=b.country and a.adrnr=b.addrnumber " + _
                " where a.mandt='168' and a.kunnr='" + Replace(UCase(ERPID), "'", "''") + "' and rownum=1 and a.land1='US' "
            Dim oraDt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strPlSql)
            If oraDt.Rows.Count = 1 Then
                ZipCode = oraDt.Rows(0).Item("post_code1") : Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function isTaxExempt(ByVal CompanyId As String) As Boolean
        Dim STR As String = String.Format("select COUNT(A.KUNNR) from SAPRDP.KNVI A INNER JOIN SAPRDP.TSKDT B " & _
                                            " on A.TATYP=B.TATYP AND A.TAXKD=B.TAXKD WHERE  " & _
                                            " A.MANDT='168' AND B.MANDT='168'  " & _
                                            " AND B.SPRAS='E' AND A.ALAND='US' AND A.KUNNR='{0}' AND A.TATYP='UTXJ' And  A.TAXKD=0", CompanyId)
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

        Dim strSql As String = _
            "SELECT a.DOKNR, a.DOKVR, a.LANGU, a.DKTXT, a.DKTXT_UC, b.FILEP, b.DAPPL" + _
            " FROM SAPRDP.DRAT a inner join SAPRDP.DRAW b on a.DOKNR=b.DOKNR and a.DOKVR=b.DOKVR" + _
            " WHERE a.DOKNR LIKE '" + _ERPID + "%'" + _
            " AND a.MANDT='168' AND a.DOKAR='CTO' AND b.MANDT='168' AND b.DOKAR='CTO'" + _
            " AND a.DOKVR<>'00'" + _
            " Order by a.DOKNR,a.DOKVR desc"

        Return OraDbUtil.dbGetDataTable("SAP_PRD", strSql)

    End Function
    Shared Function GetCTOSAssemblyInstructionListByERPIdFromMyadvantech(ByVal _ERPID As String, ByVal DocTxt As String) As DataTable
        Dim strSql As String = _
            "SELECT a.DOKNR, a.DOKVR, a.LANGU, a.DKTXT, a.DKTXT_UC, b.FILEP, b.DAPPL" + _
            " FROM SAP_CTOS_DOC a inner join SAP_CTOS_DOC_URL b on a.DOKNR=b.DOKNR and a.DOKVR=b.DOKVR" + _
            " WHERE 1=1 "
        If Not String.IsNullOrEmpty(_ERPID) Then strSql += " and a.DOKNR LIKE '" + _ERPID + "%'"
        If Not String.IsNullOrEmpty(DocTxt) Then strSql += " and a.DKTXT LIKE N'%" + Replace(Replace(DocTxt, "'", "''"), "*", "%") + "%'"
        'Nada confirmed with Frank and removed criteria 'a.DOKVR<>'00''
        'strSql += " AND a.DOKVR<>'00' Order by a.DOKNR,a.DOKVR desc"
        strSql += " Order by a.DOKNR,a.DOKVR desc"
        Return dbUtil.dbGetDataTable("MY", strSql)

    End Function
    <Obsolete()> _
    Shared Function GetCTOSAssemblyInstructionListByERPIdFromMyadvantech_ob(ByVal _ERPID As String) As String
        Dim strSql As String = _
            " SELECT top 1 a.DOKNR + '****' + b.FILEP" + _
            " FROM SAP_CTOS_DOC a inner join SAP_CTOS_DOC_URL b on a.DOKNR=b.DOKNR and a.DOKVR=b.DOKVR" + _
            " WHERE 1=1 "
        If Not String.IsNullOrEmpty(_ERPID) Then strSql += " and a.DOKNR LIKE '" + _ERPID + "%'"
        'If Not String.IsNullOrEmpty(DocTxt) Then strSql += " and a.DKTXT LIKE N'%" + Replace(Replace(DocTxt, "'", "''"), "*", "%") + "%'"
        strSql += " AND a.DOKVR<>'00' Order by a.DOKNR,a.DOKVR desc"
        Dim O As Object = dbUtil.dbExecuteScalar("MY", strSql)
        If Not IsNothing(O) Then
            Return O.ToString.Trim
        End If
        Return ""
    End Function

    'Nada unified with MYA
    Public Shared Function getSalesNotebyERPid(ByVal companyID As String) As String
        Try
            Dim TXTObj As Object = dbUtil.dbExecuteScalar("MY",
                                                          String.Format("select top 1 TXT from SAP_COMPANY_SALESNOTE " & _
                                                                        "WHERE (COMPANY_ID = '{0}' or COMPANY_ID LIKE '{0} %') and TXT <> '' and TXT IS NOT NULL order by last_upd_date desc", companyID.Trim))
            If TXTObj IsNot Nothing AndAlso TXTObj.ToString <> "" Then
                Return TXTObj.ToString
            End If
        Catch ex As Exception
        End Try
        Return ""
    End Function
    Public Shared Function GetBillToNotSoldTo(ByVal SoldTo As String) As String
        If String.IsNullOrEmpty(SoldTo) Then Return ""
        Dim dt As New DataTable
        Dim sb As New System.Text.StringBuilder
        With sb

            .AppendLine("SELECT company_id FROM  ")
            .AppendLine("(SELECT A.KUNN2 AS company_id FROM saprdp.knvp A  ")
            .AppendFormat("where (A.Kunnr = '{0}')", UCase(SoldTo.Replace("'", "''").Trim))
            .AppendLine("AND A.PARVW ='RE' ")
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

        If PN.StartsWith("EZ-") Then PN = PN.Substring(3, PN.Length - 3)
        Dim vnumber As Object = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PN.ToString, Org.ToUpper.Substring(0, 2)))
        If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
            Return vnumber
        End If
        vnumber = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PN.ToString.Trim + "-BTO", Org.ToUpper.Substring(0, 2)))
        If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
            Return vnumber
        End If
        If PN.Trim.EndsWith("-BTO") Then
            Dim Temp_PN = PN.Substring(0, PN.Length - 4)
            vnumber = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", Temp_PN, Org.ToUpper.Substring(0, 2)))
            If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
                Return vnumber
            End If
        Else
            Return PN
        End If
        Return PN
    End Function

    Shared Function IsBlankPartStatus(ByVal PartNo As String, ByVal SalesOrg As String, ByRef StatusCode As String, ByRef StatusDesc As String) As Boolean

        Dim strSql As String = String.Empty, dtProdStatus As DataTable = Nothing

        'Frank 2012/10/04: After confirming with Jay, if part status is blank then return status with A
        If (PartNo.StartsWith("X", StringComparison.InvariantCultureIgnoreCase) OrElse _
        PartNo.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase)) Then
            strSql = _
            " select a.MMSTA as status_code,'' as status_desc" + _
            " from saprdp.MARC a " + _
            " where a.mandt='168' and a.werks='USH1'" + _
            " and a.matnr='" + PartNo + "' and rownum=1"
        Else
            strSql = _
            " select a.vmsta as status_code, '' as status_desc" + _
            " from saprdp.MVKE a " + _
            " where a.mandt='168' " + _
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

    Shared Function isInvalidPhaseOutV2(ByVal PartNo As String, ByVal SalesOrg As String, ByRef StatusCode As String, ByRef StatusDesc As String, ByRef ATPQty As Decimal, Optional ByVal IsSyncToLocalDB As Boolean = True) As Boolean
        SalesOrg = Trim(UCase(SalesOrg)) : PartNo = Trim(UCase(PartNo))

        'Frank 2012/08/22:Do not check extended warranty partno
        If PartNo.StartsWith("AGS-EW") Then Return False
        PartNo = replaceCartBTO(PartNo, SalesOrg)
        If PartNo.ToUpper.EndsWith("-BTO") Then Return False
        Dim IsNumericPn As Boolean = False
        For i As Integer = 0 To PartNo.Length - 1
            If IsNumeric(PartNo.Substring(i, 1)) Then
                IsNumericPn = True
            Else
                IsNumericPn = False : Exit For
            End If
        Next
        If IsNumericPn Then
            Dim intZeros As Integer = 18 - PartNo.Length
            For i As Integer = 1 To intZeros
                PartNo = "0" + PartNo
            Next
        End If

        Dim strSql As String = String.Empty
        'Dim strSql As String = _
        '    " select a.vmsta as status_code, b.vmstb as status_desc, c.MMSTA as xy_status_code" + _
        '    " from saprdp.MVKE a left join saprdp.TVMST b on a.vmsta=b.vmsta" + _
        '    " left join saprdp.MARC c on a.MATNR=c.MATNR" + _
        '    " where a.mandt='168' " + _
        '    " and b.mandt='168' and a.vkorg='" + SalesOrg + "' and a.matnr='" + PartNo + "' and b.spras='E' and rownum=1"

        'Frank 2012/08/24:If part no start with X or Y and org is US01, then check product status from field "saprdp.MARC.MMSTA"
        If SalesOrg.Equals("US01", StringComparison.InvariantCultureIgnoreCase) AndAlso ( _
            PartNo.StartsWith("X", StringComparison.InvariantCultureIgnoreCase) OrElse _
            PartNo.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase)) Then
            'c.spras='E' means to get English version product status description.
            strSql = _
            " select a.MMSTA as status_code, c.vmstb as status_desc" + _
            " from saprdp.MARC a left join saprdp.TVMST C on a.MMSTA=c.vmsta" + _
            " where a.mandt='168' and a.werks='USH1'" + _
            " and a.matnr='" + PartNo + "' and c.spras='E' and rownum=1"
        Else
            strSql = _
            " select a.vmsta as status_code, b.vmstb as status_desc" + _
            " from saprdp.MVKE a left join saprdp.TVMST b on a.vmsta=b.vmsta" + _
            " where a.mandt='168' " + _
            " and b.mandt='168' and a.vkorg='" + SalesOrg + "' and a.matnr='" + PartNo + "' and b.spras='E' and rownum=1"
        End If

        Dim dtProdStatus As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)




        If dtProdStatus.Rows.Count > 0 Then

            'Frank 2012/08/22:is Sync real time product data from sap to myadvanglobal
            'Put below code here because above codes will add 0 begin with part no if have to do
            If IsSyncToLocalDB Then SAPDAL.SyncSAPProductStatusToMyadvanGlobal(PartNo, SalesOrg)

            StatusCode = dtProdStatus.Rows(0).Item("status_code") : StatusDesc = dtProdStatus.Rows(0).Item("status_desc")

            Select Case StatusCode
                Case "A", "N", "H", "M1"
                    Return False
                Case "O", "S"
                    Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP, intInventory As Integer = -1
                    Dim atpTb As New GET_MATERIAL_ATP.BAPIWMDVETable, retTb As New GET_MATERIAL_ATP.BAPIWMDVSTable, rOfretTb As New GET_MATERIAL_ATP.BAPIWMDVS
                    rOfretTb.Req_Date = Now.ToString("yyyyMMdd") : rOfretTb.Req_Qty = 999 : retTb.Add(rOfretTb)
                    p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
                    p1.Connection.Open()
                    p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", PartNo, Left(SalesOrg, 2) + "H1", "", "", "", "", "PC", _
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

    Public Shared Sub SyncSAPProductStatusToMyadvanGlobal(ByVal pn As String, ByVal ORG As String)

        'Frank 2012/10/15 If Product has been updated to MyA, this sync record will be kept in cache. So the same part number will not be updated to MyA if the cache is alive. 
        Dim _SyncSAPProduct As Dictionary(Of String, String) = CType(HttpContext.Current.Cache("SyncSAPProduct"), Dictionary(Of String, String))
        If _SyncSAPProduct Is Nothing Then
            _SyncSAPProduct = New Dictionary(Of String, String)
            'Cache alive time is 1 hour.
            HttpContext.Current.Cache.Add("SyncSAPProduct", _SyncSAPProduct, Nothing, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If
        If _SyncSAPProduct.ContainsKey(ORG & pn) Then Exit Sub


        Dim DT As DataTable = GetRealTimeProductStatusFromSAP(pn, ORG)

        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then

            For i As Integer = 0 To DT.Rows.Count - 1
                If DT.Rows(i).Item("Part_no").ToString.StartsWith("0") Then
                    For n As Integer = 1 To DT.Rows(i).Item("Part_no").ToString.Length - 1
                        If DT.Rows(i).Item("Part_no").ToString.Substring(n, 1) <> "0" Then
                            DT.Rows(i).Item("Part_no") = DT.Rows(i).Item("Part_no").ToString.Substring(n) : Exit For
                        End If
                    Next
                End If
            Next

            'Clean data of SAP_PRODUCT_STATUS and SAP_PRODUCT_STATUS_ORDERABLE
            Dim Str As String = String.Format("DELETE FROM SAP_PRODUCT_STATUS WHERE PART_NO='{0}' AND sales_org LIKE '{1}%';DELETE FROM SAP_PRODUCT_STATUS_ORDERABLE WHERE PART_NO='{0}' AND sales_org LIKE '{1}%'", pn, ORG)
            dbUtil.dbExecuteNoQuery("MY", Str)

            Dim bk As New System.Data.SqlClient.SqlBulkCopy(ConfigurationManager.ConnectionStrings("B2B").ConnectionString)
            bk.DestinationTableName = "SAP_PRODUCT_STATUS"
            bk.WriteToServer(DT)

            If ORG.StartsWith("EU", StringComparison.InvariantCultureIgnoreCase) AndAlso DT.Rows(0).Item("product_status").ToString.ToUpper = "O" Then
                'Frank 2012/10/17: If part status is O and sales org is EU, then do not write this part into SAP_PRODUCT_STATUS_ORDERABLE
            Else
                bk.DestinationTableName = "SAP_PRODUCT_STATUS_ORDERABLE"
                bk.WriteToServer(DT)
            End If


            'Frank: To save the product sync event into cache after product has been updated to MyA db.
            _SyncSAPProduct.Add(ORG & pn, pn)

        End If


    End Sub


    Public Shared Function GetRealTimeProductStatusFromSAP(ByVal pn As String, ByVal ORG As String) As DataTable

        If String.IsNullOrEmpty(pn) Then Return Nothing
        If String.IsNullOrEmpty(ORG) Then Return Nothing

        Dim ORGCondition As String = ""
        If ORG.ToUpper <> "ALL" Then
            ORGCondition = "and mvke.vkorg like '" & ORG & "%'"
        End If
        Dim str As String = String.Format("select matnr as part_no, vkorg as sales_org, vtweg as dist_channel, vmsta as product_status, " & _
                                           " AUMNG as min_order_qty, LFMNG as min_dlv_qty, EFMNG as min_bto_qty, DWERK as dlv_plant, " & _
                                           " KONDM as material_pricing_grp, vmstd as valid_date, to_char(mvke.mtpos) as item_category_group " & _
                                           " from saprdp.MVKE " & _
                                           " where mandt='168' and matnr='{0}' {1}", pn, ORGCondition)

        Return OraDbUtil.dbGetDataTable("SAP_PRD", str)

    End Function
#End Region


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
        Dim strSql As String = _
            " select a.kunnr as PARTN_NUMB,a.anred as TITLE, a.NAME1 as NAME, a.NAME2 as NAME_2, b.NAME3 as NAME_3, b.NAME4 as NAME_4, " + _
            " a.STRAS as STREET, a.LAND1 as COUNTRY, a.LAND1 as COUNTRY_ISO, a.PSTLZ as POSTL_CODE, '' as POBX_PCD, ''as POBX_CTY,  " + _
            " b.CITY1 as CITY, '' as DISTRICT, a.REGIO as REGION, b.PO_BOX as PO_BOX, a.TELF1 as TELEPHONE, a.TELF2 as TELEPHONE2, a.TELBX as TELEBOX, " + _
            " a.TELFX as FAX_NUMBER, a.TELTX as TELEX_NO, a.SPRAS as LANGU, '' as LANGU_ISO, '' as UNLOAD_PT, b.TRANSPZONE, b.TAXJURCODE, " + _
            " '' as ADDRESS, '' as PRIV_ADDR, 1 as ADDR_TYPE, '' as ADDR_ORIG, '' as ADDR_LINK, '' as REFOBJTYPE, '' as REFOBJKEY, '' as REFLOGSYS " + _
            " ,b.name_co as Attention from saprdp.kna1 a inner join saprdp.adrc b on a.adrnr=b.addrnumber and a.land1=b.country " + _
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

    Public Shared Function GetSAPPartnerAddressesTableByKunnr(ByVal Kunnr As String, Optional IsSAPProductionServer As Boolean = True) As SalesOrder.PartnerAddressesDataTable
        Dim retTable As New SalesOrder.PartnerAddressesDataTable
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
            Dim S_PartnerAddressesRow As SalesOrder.PartnerAddressesRow = retTable.NewPartnerAddressesRow()
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

    Public Shared Function GetSAPPartnerAddressesTableByKunnr_Old(ByVal Kunnr As String) As SalesOrder.PartnerAddressesDataTable
        Dim retTable As New SalesOrder.PartnerAddressesDataTable
        Dim strSql As String = _
            " select a.kunnr as PARTN_NUMB,a.anred as TITLE, a.NAME1 as NAME, a.NAME2 as NAME_2, b.NAME3 as NAME_3, b.NAME4 as NAME_4, " + _
            " a.STRAS as STREET, a.LAND1 as COUNTRY, a.LAND1 as COUNTRY_ISO, a.PSTLZ as POSTL_CODE, '' as POBX_PCD, ''as POBX_CTY,  " + _
            " b.CITY1 as CITY, '' as DISTRICT, a.REGIO as REGION, b.PO_BOX as PO_BOX, a.TELF1 as TELEPHONE, a.TELF2 as TELEPHONE2, a.TELBX as TELEBOX, " + _
            " a.TELFX as FAX_NUMBER, a.TELTX as TELEX_NO, a.SPRAS as LANGU, '' as LANGU_ISO, '' as UNLOAD_PT, b.TRANSPZONE, b.TAXJURCODE, " + _
            " '' as ADDRESS, '' as PRIV_ADDR, 1 as ADDR_TYPE, '' as ADDR_ORIG, '' as ADDR_LINK, '' as REFOBJTYPE, '' as REFOBJKEY, '' as REFLOGSYS " + _
            " ,b.name_co as Attention from saprdp.kna1 a inner join saprdp.adrc b on a.adrnr=b.addrnumber and a.land1=b.country " + _
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
    Public Shared Function GetCustomerDataSet(ByVal companyid As String, ByRef ds As DataSet, Optional ConnectToSAPPRD As Boolean = True) As Boolean
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
                Dim knvpDt2 As DataTable = OraDbUtil.dbGetDataTable(SAPconnection, _
                    " select kunnr from saprdp.knvp  " + _
                    " where kunnr in (select kunnr from saprdp.knvp where kunn2='" + knvpDt.Rows(0).Item("kunnr").ToString() + "' and kunnr<>kunn2 and rownum=1)  " + _
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

    Public Shared Function SearchAllSAPCompanySoldBillShipTo( _
       ByVal ERPID As String, ByVal Org_id As String, ByVal CompanyName As String, ByVal Address As String, ByVal State As String, _
       ByVal Division As String, ByVal SalesGroup As String, ByVal SalesOffice As String) As DataTable
        Dim dt As New DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            ' .AppendLine(" SELECT A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME,  D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| D.country AS Address, ") 'B.STRAS AS ADDRESS,
            .AppendLine(" SELECT A.KUNN2 AS company_id, A.VKORG as ORG_ID, B.NAME1 AS COMPANY_NAME, " + _
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
        Dim strSql As String = _
            " SELECT TOP 1 COMPANY_ID, ORG_ID, PARENTCOMPANYID, COMPANY_NAME, ADDRESS, FAX_NO, TEL_NO,  " + _
            " COMPANY_TYPE, PRICE_CLASS, CURRENCY, COUNTRY, REGION_CODE, ZIP_CODE, CITY, ATTENTION,  " + _
            " CREDIT_TERM, SHIP_VIA, URL, SHIPCONDITION, ATTRIBUTE4, SALESOFFICE, SALESGROUP, AMT_INSURED,  " + _
            " CREDIT_LIMIT, CONTACT_EMAIL, DELETION_FLAG, COUNTRY_NAME, SALESOFFICENAME, SAP_SALESNAME,  " + _
            " SAP_SALESCODE, SAP_ISNAME, SAP_OPNAME " + _
            " FROM SAP_DIMCOMPANY " + _
            " WHERE COMPANY_ID = @CID AND ORG_ID =@OID "
        Dim apt As New SqlClient.SqlDataAdapter(strSql, New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
        apt.SelectCommand.Parameters.AddWithValue("CID", companyid) : apt.SelectCommand.Parameters.AddWithValue("OID", OrgId)
        Dim dt As New DataTable
        apt.Fill(dt)
        apt.SelectCommand.Connection.Close()
        Return dt
    End Function
#End Region
    <WebMethod()> _
    Public Shared Function SearchSAPCompany(ByVal ERPID As String, ByVal Org_id As String) As DataTable
        Dim dt As New DataTable
        If String.IsNullOrEmpty(ERPID) Or String.IsNullOrEmpty(Org_id) Then Return New DataTable("SAPPF")
        ERPID = Replace(Trim(ERPID).ToUpper, "'", "")
        Org_id = Replace(Trim(Org_id).ToUpper, "'", "")
        Dim sb As New System.Text.StringBuilder
        With sb

            .AppendLine(" SELECT A.KUNN2 AS company_id,B.NAME1 AS COMPANY_NAME,  " + _
                        " D.street  ||' '|| D.city1 ||' '|| D.region ||' '|| D.post_code1 ||' '|| (select e.landx from saprdp.t005t e where e.land1=B.land1 and e.spras='E' and rownum=1 and E.MANDT=168) AS Address,  " + _
                        " B.Land1 AS  COUNTRY,B.Ort01 AS CITY, B.PSTLZ AS ZIP_CODE, D.region AS STATE,  C.smtp_addr AS CONTACT_EMAIL,B.TELF1 AS TEL_NO, " + _
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

    <WebMethod()> _
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


    Public Shared Function SearchAllSAPCompanySoldBillShipTo( _
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

    <WebMethod()> _
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
    <WebMethod()> _
    Public Function GetOrderMasterFromSAP(ByVal SoNo As String) As DataTable
        'If String.IsNullOrEmpty(OrgID) Then Return New DataTable("SAPPF")
        'PoNo = Replace(Trim(PoNo.ToUpper), "'", "")
        'SoNo = Replace(Trim(SoNo.ToUpper), "'", "")
        'CompanyID = Replace(Trim(CompanyID.ToUpper), "'", "")
        'OrgID = Replace(Trim(OrgID.ToUpper), "'", "")
        'If DateTime.TryParse(OrderDateFrom, Date.Now()) = False OrElse DateTime.TryParse(OrderDateTo, Date.Now()) = False Then
        '    Return New DataTable("SAPPF")
        'End If
        Dim STR As String = " select VBELN AS ORDNO,WAERK AS CURR,VKORG AS ORG, " & _
                            " (SELECT DISTINCT BEZEI FROM SAPRDP.TVKBT WHERE VKBUR=A.VKBUR AND ROWNUM=1) AS OFFICE, " & _
                            " KUNNR AS COMPANYID, " & _
                            " (SELECT KUNNR FROM SAPRDP.VBPA WHERE SAPRDP.VBPA.VBELN=A.VBELN AND SAPRDP.VBPA.PARVW='WE' AND ROWNUM=1) AS SHIPTOID, " & _
                            " (SELECT KUNNR FROM SAPRDP.VBPA WHERE SAPRDP.VBPA.VBELN=A.VBELN AND SAPRDP.VBPA.PARVW='RE' AND ROWNUM=1) AS BILLTOID, " & _
                            " (SELECT NAME1 FROM SAPRDP.KNA1 WHERE KUNNR=A.KUNNR AND ROWNUM=1) AS COMPANYNAME " & _
                            " from SAPRDP.VBAK A where A.VBELN ='" & SoNo.ToUpper.Trim & "'"

        Dim dt As New DataTable("SAPOrders")
        dt = OraDbUtil.dbGetDataTable("SAP_PRD", STR)
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return Nothing
    End Function
    <WebMethod()> _
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
    <WebMethod()> _
    Public Shared Function Get_Next_WorkingDate_ByCode(ByRef iATPDate As String, ByVal Loading_Days As String, ByVal code As String) As Integer
        code = UCase(code)
        Dim proxy1 As New Factory_Date_Conversion.Factory_Date_Conversion
        Dim factory_date_Number As Decimal

        Dim provider1 As New CultureInfo("fr-FR", True)
        Dim time1 As DateTime = DateTime.ParseExact(iATPDate, "yyyy-mm-dd", provider1)
        iATPDate = time1.ToString("yyyymmdd")
        'iATPDate = Replace(iATPDate, "/", "")

        Try
            proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD").ToString)
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
    <WebMethod()> _
    Public Function HelloKitty() As String
        Return "Hello Kitty!"
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
    Public Function Query( _
    ByVal PlantArray As String, ByVal PartNo As String, ByVal maximumRows As Integer, ByVal startRowIndex As Integer, _
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
            p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", PartNo, plant, "", "", "", "", "PC", "", 9999, "", "", _
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
