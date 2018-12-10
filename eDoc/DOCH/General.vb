
Public Class Customer : Implements IBUS.iCustomer

    Private _errCode As COMM.Msg.eErrCode
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property

    Public Function getSalesNotebyCustomer(ByVal companyID As String) As String Implements IBUS.iCustomer.getSalesNotebyCustomer
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

    Public Function is_Valid_Company_Id_All(ByVal company_id As String) As Boolean Implements IBUS.iCustomer.is_Valid_Company_Id_All
        Dim str As String = String.Format("select top 1 COMPANY_ID from SAP_DIMCOMPANY where COMPANY_ID='{0}'", company_id)
        Dim dt As New DataTable
        dt = dbUtil.dbGetDataTable("MY", str)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Public Function isCustomerCompleteDeliv(ByVal CompanyId As String, ByVal org As String) As Boolean Implements IBUS.iCustomer.isCustomerCompleteDeliv
        CompanyId = UCase(CompanyId) : org = UCase(org)
        Dim str As String = String.Format("select KUNNR from sapRDP.KNVV WHERE KUNNR='{0}' AND MANDT='168' and vkorg='{1}' and KZTLF='C'", CompanyId, org)
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", str)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Public Function getByErpIdOrg(ByVal CompanyiD As String, ByVal org As String) As iCustomerLine Implements IBUS.iCustomer.getByErpIdOrg
        Dim o As New SAP_DIMCompany
        Dim DT As DataTable = o.GetDT(String.Format("COMPANY_ID='{0}' and ORG_ID ='{1}'", CompanyiD, org), "")
        If Not IsNothing(DT) AndAlso DT.Rows.Count > 0 Then
            Dim C As New CustomerLine
            If Not IsDBNull(DT.Rows(0).Item("COMPANY_ID")) Then
                C.COMPANY_ID = DT.Rows(0).Item("COMPANY_ID")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ORG_ID")) Then
                C.ORG_ID = DT.Rows(0).Item("ORG_ID")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PARENTCOMPANYID")) Then
                C.PARENTCOMPANYID = DT.Rows(0).Item("PARENTCOMPANYID")
            End If
            If Not IsDBNull(DT.Rows(0).Item("COMPANY_NAME")) Then
                C.COMPANY_NAME = DT.Rows(0).Item("COMPANY_NAME")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ADDRESS")) Then
                C.ADDRESS = DT.Rows(0).Item("ADDRESS")
            End If
            If Not IsDBNull(DT.Rows(0).Item("FAX_NO")) Then
                C.FAX_NO = DT.Rows(0).Item("FAX_NO")
            End If
            If Not IsDBNull(DT.Rows(0).Item("TEL_NO")) Then
                C.TEL_NO = DT.Rows(0).Item("TEL_NO")
            End If
            If Not IsDBNull(DT.Rows(0).Item("COMPANY_TYPE")) Then
                C.COMPANY_TYPE = DT.Rows(0).Item("COMPANY_TYPE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PRICE_CLASS")) Then
                C.PRICE_CLASS = DT.Rows(0).Item("PRICE_CLASS")
            End If
            If Not IsDBNull(DT.Rows(0).Item("CURRENCY")) Then
                C.CURRENCY = DT.Rows(0).Item("CURRENCY")
            End If
            If Not IsDBNull(DT.Rows(0).Item("COUNTRY")) Then
                C.COUNTRY = DT.Rows(0).Item("COUNTRY")
            End If
            If Not IsDBNull(DT.Rows(0).Item("REGION_CODE")) Then
                C.REGION_CODE = DT.Rows(0).Item("REGION_CODE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ZIP_CODE")) Then
                C.ZIP_CODE = DT.Rows(0).Item("ZIP_CODE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("CITY")) Then
                C.CITY = DT.Rows(0).Item("CITY")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ATTENTION")) Then
                C.ATTENTION = DT.Rows(0).Item("ATTENTION")
            End If
            If Not IsDBNull(DT.Rows(0).Item("CREDIT_TERM")) Then
                C.CREDIT_TERM = DT.Rows(0).Item("CREDIT_TERM")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SHIP_VIA")) Then
                C.SHIP_VIA = DT.Rows(0).Item("SHIP_VIA")
            End If
            If Not IsDBNull(DT.Rows(0).Item("URL")) Then
                C.URL = DT.Rows(0).Item("URL")
            End If
            If Not IsDBNull(DT.Rows(0).Item("CREATEDDATE")) Then
                C.CREATEDDATE = DT.Rows(0).Item("CREATEDDATE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("CREATED_BY")) Then
                C.CREATED_BY = DT.Rows(0).Item("CREATED_BY")
            End If
            If Not IsDBNull(DT.Rows(0).Item("COMPANY_PRICE_TYPE")) Then
                C.COMPANY_PRICE_TYPE = DT.Rows(0).Item("COMPANY_PRICE_TYPE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SHIPCONDITION")) Then
                C.SHIPCONDITION = DT.Rows(0).Item("SHIPCONDITION")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ATTRIBUTE4")) Then
                C.ATTRIBUTE4 = DT.Rows(0).Item("ATTRIBUTE4")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SALESOFFICE")) Then
                C.SALESOFFICE = DT.Rows(0).Item("SALESOFFICE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SALESGROUP")) Then
                C.SALESGROUP = DT.Rows(0).Item("SALESGROUP")
            End If
            If Not IsDBNull(DT.Rows(0).Item("AMT_INSURED")) Then
                C.AMT_INSURED = DT.Rows(0).Item("AMT_INSURED")
            End If
            If Not IsDBNull(DT.Rows(0).Item("CREDIT_LIMIT")) Then
                C.CREDIT_LIMIT = DT.Rows(0).Item("CREDIT_LIMIT")
            End If
            If Not IsDBNull(DT.Rows(0).Item("CONTACT_EMAIL")) Then
                C.CONTACT_EMAIL = DT.Rows(0).Item("CONTACT_EMAIL")
            End If
            If Not IsDBNull(DT.Rows(0).Item("DELETION_FLAG")) Then
                C.DELETION_FLAG = DT.Rows(0).Item("DELETION_FLAG")
            End If
            If Not IsDBNull(DT.Rows(0).Item("COUNTRY_NAME")) Then
                C.COUNTRY_NAME = DT.Rows(0).Item("COUNTRY_NAME")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SALESOFFICENAME")) Then
                C.SALESOFFICENAME = DT.Rows(0).Item("SALESOFFICENAME")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SAP_SALESNAME")) Then
                C.SAP_SALESNAME = DT.Rows(0).Item("SAP_SALESNAME")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SAP_SALESCODE")) Then
                C.SAP_SALESCODE = DT.Rows(0).Item("SAP_SALESCODE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SAP_ISNAME")) Then
                C.SAP_ISNAME = DT.Rows(0).Item("SAP_ISNAME")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SAP_OPNAME")) Then
                C.SAP_OPNAME = DT.Rows(0).Item("SAP_OPNAME")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SECTOR")) Then
                C.SECTOR = DT.Rows(0).Item("SECTOR")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PRIMARY_BAA")) Then
                C.PRIMARY_BAA = DT.Rows(0).Item("PRIMARY_BAA")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ACCOUNT_ROW_ID")) Then
                C.ACCOUNT_ROW_ID = DT.Rows(0).Item("ACCOUNT_ROW_ID")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ACCOUNT_NAME")) Then
                C.ACCOUNT_NAME = DT.Rows(0).Item("ACCOUNT_NAME")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ACCOUNT_STATUS")) Then
                C.ACCOUNT_STATUS = DT.Rows(0).Item("ACCOUNT_STATUS")
            End If
            If Not IsDBNull(DT.Rows(0).Item("RBU")) Then
                C.RBU = DT.Rows(0).Item("RBU")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PRIMARY_SALES_EMAIL")) Then
                C.PRIMARY_SALES_EMAIL = DT.Rows(0).Item("PRIMARY_SALES_EMAIL")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PRIMARY_OWNER_DIVISION")) Then
                C.PRIMARY_OWNER_DIVISION = DT.Rows(0).Item("PRIMARY_OWNER_DIVISION")
            End If
            If Not IsDBNull(DT.Rows(0).Item("BUSINESS_GROUP")) Then
                C.BUSINESS_GROUP = DT.Rows(0).Item("BUSINESS_GROUP")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ACCOUNT_TYPE")) Then
                C.ACCOUNT_TYPE = DT.Rows(0).Item("ACCOUNT_TYPE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("FACT2005")) Then
                C.FACT2005 = DT.Rows(0).Item("FACT2005")
            End If
            If Not IsDBNull(DT.Rows(0).Item("FACT2006")) Then
                C.FACT2006 = DT.Rows(0).Item("FACT2006")
            End If
            If Not IsDBNull(DT.Rows(0).Item("FACT2007")) Then
                C.FACT2007 = DT.Rows(0).Item("FACT2007")
            End If
            If Not IsDBNull(DT.Rows(0).Item("FACT2008")) Then
                C.FACT2008 = DT.Rows(0).Item("FACT2008")
            End If
            If Not IsDBNull(DT.Rows(0).Item("FACT2009")) Then
                C.FACT2009 = DT.Rows(0).Item("FACT2009")
            End If
            If Not IsDBNull(DT.Rows(0).Item("FACT2010")) Then
                C.FACT2010 = DT.Rows(0).Item("FACT2010")
            End If
            If Not IsDBNull(DT.Rows(0).Item("LAST_BUY_DATE")) Then
                C.LAST_BUY_DATE = DT.Rows(0).Item("LAST_BUY_DATE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ORDERS_IN_PAST_YEAR")) Then
                C.ORDERS_IN_PAST_YEAR = DT.Rows(0).Item("ORDERS_IN_PAST_YEAR")
            End If
            If Not IsDBNull(DT.Rows(0).Item("AMOUNT_IN_PAST_YEAR")) Then
                C.AMOUNT_IN_PAST_YEAR = DT.Rows(0).Item("AMOUNT_IN_PAST_YEAR")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ORDERS_IN_PAST_HALFYEAR")) Then
                C.ORDERS_IN_PAST_HALFYEAR = DT.Rows(0).Item("ORDERS_IN_PAST_HALFYEAR")
            End If
            If Not IsDBNull(DT.Rows(0).Item("CUST_IND")) Then
                C.CUST_IND = DT.Rows(0).Item("CUST_IND")
            End If
            If Not IsDBNull(DT.Rows(0).Item("VM")) Then
                C.VM = DT.Rows(0).Item("VM")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PRICE_GRP")) Then
                C.PRICE_GRP = DT.Rows(0).Item("PRICE_GRP")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PRICE_LIST")) Then
                C.PRICE_LIST = DT.Rows(0).Item("PRICE_LIST")
            End If
            If Not IsDBNull(DT.Rows(0).Item("INCO1")) Then
                C.INCO1 = DT.Rows(0).Item("INCO1")
            End If
            If Not IsDBNull(DT.Rows(0).Item("INCO2")) Then
                C.INCO2 = DT.Rows(0).Item("INCO2")
            End If

            Return C
        End If
        Return Nothing
    End Function
End Class
Public Class CustomerLine : Implements iCustomerLine
    Private _COMPANY_ID As String = ""
    Public Property COMPANY_ID As String Implements IBUS.iCustomerLine.COMPANY_ID
        Get
            Return _COMPANY_ID
        End Get
        Set(ByVal value As String)
            _COMPANY_ID = value
        End Set
    End Property
    Private _ORG_ID As String = ""
    Public Property ORG_ID As String Implements IBUS.iCustomerLine.ORG_ID
        Get
            Return _ORG_ID
        End Get
        Set(ByVal value As String)
            _ORG_ID = value
        End Set
    End Property
    Private _PARENTCOMPANYID As String = ""
    Public Property PARENTCOMPANYID As String Implements IBUS.iCustomerLine.PARENTCOMPANYID
        Get
            Return _PARENTCOMPANYID
        End Get
        Set(ByVal value As String)
            _PARENTCOMPANYID = value
        End Set
    End Property
    Private _COMPANY_NAME As String = ""
    Public Property COMPANY_NAME As String Implements IBUS.iCustomerLine.COMPANY_NAME
        Get
            Return _COMPANY_NAME
        End Get
        Set(ByVal value As String)
            _COMPANY_NAME = value
        End Set
    End Property
    Private _ADDRESS As String = ""
    Public Property ADDRESS As String Implements IBUS.iCustomerLine.ADDRESS
        Get
            Return _ADDRESS
        End Get
        Set(ByVal value As String)
            _ADDRESS = value
        End Set
    End Property
    Private _FAX_NO As String = ""
    Public Property FAX_NO As String Implements IBUS.iCustomerLine.FAX_NO
        Get
            Return _FAX_NO
        End Get
        Set(ByVal value As String)
            _FAX_NO = value
        End Set
    End Property
    Private _TEL_NO As String = ""
    Public Property TEL_NO As String Implements IBUS.iCustomerLine.TEL_NO
        Get
            Return _TEL_NO
        End Get
        Set(ByVal value As String)
            _TEL_NO = value
        End Set
    End Property
    Private _COMPANY_TYPE As String = ""
    Public Property COMPANY_TYPE As String Implements IBUS.iCustomerLine.COMPANY_TYPE
        Get
            Return _COMPANY_TYPE
        End Get
        Set(ByVal value As String)
            _COMPANY_TYPE = value
        End Set
    End Property
    Private _PRICE_CLASS As String = ""
    Public Property PRICE_CLASS As String Implements IBUS.iCustomerLine.PRICE_CLASS
        Get
            Return _PRICE_CLASS
        End Get
        Set(ByVal value As String)
            _PRICE_CLASS = value
        End Set
    End Property
    Private _CURRENCY As String = ""
    Public Property CURRENCY As String Implements IBUS.iCustomerLine.CURRENCY
        Get
            Return _CURRENCY
        End Get
        Set(ByVal value As String)
            _CURRENCY = value
        End Set
    End Property
    Private _COUNTRY As String = ""
    Public Property COUNTRY As String Implements IBUS.iCustomerLine.COUNTRY
        Get
            Return _COUNTRY
        End Get
        Set(ByVal value As String)
            _COUNTRY = value
        End Set
    End Property
    Private _REGION_CODE As String = ""
    Public Property REGION_CODE As String Implements IBUS.iCustomerLine.REGION_CODE
        Get
            Return _REGION_CODE
        End Get
        Set(ByVal value As String)
            _REGION_CODE = value
        End Set
    End Property
    Private _ZIP_CODE As String = ""
    Public Property ZIP_CODE As String Implements IBUS.iCustomerLine.ZIP_CODE
        Get
            Return _ZIP_CODE
        End Get
        Set(ByVal value As String)
            _ZIP_CODE = value
        End Set
    End Property
    Private _CITY As String = ""
    Public Property CITY As String Implements IBUS.iCustomerLine.CITY
        Get
            Return _CITY
        End Get
        Set(ByVal value As String)
            _CITY = value
        End Set
    End Property
    Private _ATTENTION As String = ""
    Public Property ATTENTION As String Implements IBUS.iCustomerLine.ATTENTION
        Get
            Return _ATTENTION
        End Get
        Set(ByVal value As String)
            _ATTENTION = value
        End Set
    End Property
    Private _CREDIT_TERM As String = ""
    Public Property CREDIT_TERM As String Implements IBUS.iCustomerLine.CREDIT_TERM
        Get
            Return _CREDIT_TERM
        End Get
        Set(ByVal value As String)
            _CREDIT_TERM = value
        End Set
    End Property
    Private _SHIP_VIA As String = ""
    Public Property SHIP_VIA As String Implements IBUS.iCustomerLine.SHIP_VIA
        Get
            Return _SHIP_VIA
        End Get
        Set(ByVal value As String)
            _SHIP_VIA = value
        End Set
    End Property
    Private _URL As String = ""
    Public Property URL As String Implements IBUS.iCustomerLine.URL
        Get
            Return _URL
        End Get
        Set(ByVal value As String)
            _URL = value
        End Set
    End Property
    Private _CREATEDDATE As String = ""
    Public Property CREATEDDATE As String Implements IBUS.iCustomerLine.CREATEDDATE
        Get
            Return _CREATEDDATE
        End Get
        Set(ByVal value As String)
            _CREATEDDATE = value
        End Set
    End Property
    Private _CREATED_BY As String = ""
    Public Property CREATED_BY As String Implements IBUS.iCustomerLine.CREATED_BY
        Get
            Return _CREATED_BY
        End Get
        Set(ByVal value As String)
            _CREATED_BY = value
        End Set
    End Property
    Private _COMPANY_PRICE_TYPE As String = ""
    Public Property COMPANY_PRICE_TYPE As String Implements IBUS.iCustomerLine.COMPANY_PRICE_TYPE
        Get
            Return _COMPANY_PRICE_TYPE
        End Get
        Set(ByVal value As String)
            _COMPANY_PRICE_TYPE = value
        End Set
    End Property
    Private _SHIPCONDITION As String = ""
    Public Property SHIPCONDITION As String Implements IBUS.iCustomerLine.SHIPCONDITION
        Get
            Return _SHIPCONDITION
        End Get
        Set(ByVal value As String)
            _SHIPCONDITION = value
        End Set
    End Property
    Private _ATTRIBUTE4 As String = ""
    Public Property ATTRIBUTE4 As String Implements IBUS.iCustomerLine.ATTRIBUTE4
        Get
            Return _ATTRIBUTE4
        End Get
        Set(ByVal value As String)
            _ATTRIBUTE4 = value
        End Set
    End Property
    Private _SALESOFFICE As String = ""
    Public Property SALESOFFICE As String Implements IBUS.iCustomerLine.SALESOFFICE
        Get
            Return _SALESOFFICE
        End Get
        Set(ByVal value As String)
            _SALESOFFICE = value
        End Set
    End Property
    Private _SALESGROUP As String = ""
    Public Property SALESGROUP As String Implements IBUS.iCustomerLine.SALESGROUP
        Get
            Return _SALESGROUP
        End Get
        Set(ByVal value As String)
            _SALESGROUP = value
        End Set
    End Property
    Private _AMT_INSURED As Decimal = 0
    Public Property AMT_INSURED As Decimal Implements IBUS.iCustomerLine.AMT_INSURED
        Get
            Return _AMT_INSURED
        End Get
        Set(ByVal value As Decimal)
            _AMT_INSURED = value
        End Set
    End Property
    Private _CREDIT_LIMIT As Decimal = 0
    Public Property CREDIT_LIMIT As Decimal Implements IBUS.iCustomerLine.CREDIT_LIMIT
        Get
            Return _CREDIT_LIMIT
        End Get
        Set(ByVal value As Decimal)
            _CREDIT_LIMIT = value
        End Set
    End Property
    Private _CONTACT_EMAIL As String = ""
    Public Property CONTACT_EMAIL As String Implements IBUS.iCustomerLine.CONTACT_EMAIL
        Get
            Return _CONTACT_EMAIL
        End Get
        Set(ByVal value As String)
            _CONTACT_EMAIL = value
        End Set
    End Property
    Private _DELETION_FLAG As String = ""
    Public Property DELETION_FLAG As String Implements IBUS.iCustomerLine.DELETION_FLAG
        Get
            Return _DELETION_FLAG
        End Get
        Set(ByVal value As String)
            _DELETION_FLAG = value
        End Set
    End Property
    Private _COUNTRY_NAME As String = ""
    Public Property COUNTRY_NAME As String Implements IBUS.iCustomerLine.COUNTRY_NAME
        Get
            Return _COUNTRY_NAME
        End Get
        Set(ByVal value As String)
            _COUNTRY_NAME = value
        End Set
    End Property
    Private _SALESOFFICENAME As String = ""
    Public Property SALESOFFICENAME As String Implements IBUS.iCustomerLine.SALESOFFICENAME
        Get
            Return _SALESOFFICENAME
        End Get
        Set(ByVal value As String)
            _SALESOFFICENAME = value
        End Set
    End Property
    Private _SAP_SALESNAME As String = ""
    Public Property SAP_SALESNAME As String Implements IBUS.iCustomerLine.SAP_SALESNAME
        Get
            Return _SAP_SALESNAME
        End Get
        Set(ByVal value As String)
            _SAP_SALESNAME = value
        End Set
    End Property
    Private _SAP_SALESCODE As String = ""
    Public Property SAP_SALESCODE As String Implements IBUS.iCustomerLine.SAP_SALESCODE
        Get
            Return _SAP_SALESCODE
        End Get
        Set(ByVal value As String)
            _SAP_SALESCODE = value
        End Set
    End Property
    Private _SAP_ISNAME As String = ""
    Public Property SAP_ISNAME As String Implements IBUS.iCustomerLine.SAP_ISNAME
        Get
            Return _SAP_ISNAME
        End Get
        Set(ByVal value As String)
            _SAP_ISNAME = value
        End Set
    End Property
    Private _SAP_OPNAME As String = ""
    Public Property SAP_OPNAME As String Implements IBUS.iCustomerLine.SAP_OPNAME
        Get
            Return _SAP_OPNAME
        End Get
        Set(ByVal value As String)
            _SAP_OPNAME = value
        End Set
    End Property
    Private _SECTOR As String = ""
    Public Property SECTOR As String Implements IBUS.iCustomerLine.SECTOR
        Get
            Return _SECTOR
        End Get
        Set(ByVal value As String)
            _SECTOR = value
        End Set
    End Property
    Private _PRIMARY_BAA As String = ""
    Public Property PRIMARY_BAA As String Implements IBUS.iCustomerLine.PRIMARY_BAA
        Get
            Return _PRIMARY_BAA
        End Get
        Set(ByVal value As String)
            _PRIMARY_BAA = value
        End Set
    End Property
    Private _ACCOUNT_ROW_ID As String = ""
    Public Property ACCOUNT_ROW_ID As String Implements IBUS.iCustomerLine.ACCOUNT_ROW_ID
        Get
            Return _ACCOUNT_ROW_ID
        End Get
        Set(ByVal value As String)
            _ACCOUNT_ROW_ID = value
        End Set
    End Property
    Private _ACCOUNT_NAME As String = ""
    Public Property ACCOUNT_NAME As String Implements IBUS.iCustomerLine.ACCOUNT_NAME
        Get
            Return _ACCOUNT_NAME
        End Get
        Set(ByVal value As String)
            _ACCOUNT_NAME = value
        End Set
    End Property
    Private _ACCOUNT_STATUS As String = ""
    Public Property ACCOUNT_STATUS As String Implements IBUS.iCustomerLine.ACCOUNT_STATUS
        Get
            Return _ACCOUNT_STATUS
        End Get
        Set(ByVal value As String)
            _ACCOUNT_STATUS = value
        End Set
    End Property
    Private _RBU As String = ""
    Public Property RBU As String Implements IBUS.iCustomerLine.RBU
        Get
            Return _RBU
        End Get
        Set(ByVal value As String)
            _RBU = value
        End Set
    End Property
    Private _PRIMARY_SALES_EMAIL As String = ""
    Public Property PRIMARY_SALES_EMAIL As String Implements IBUS.iCustomerLine.PRIMARY_SALES_EMAIL
        Get
            Return _PRIMARY_SALES_EMAIL
        End Get
        Set(ByVal value As String)
            _PRIMARY_SALES_EMAIL = value
        End Set
    End Property
    Private _PRIMARY_OWNER_DIVISION As String = ""
    Public Property PRIMARY_OWNER_DIVISION As String Implements IBUS.iCustomerLine.PRIMARY_OWNER_DIVISION
        Get
            Return _PRIMARY_OWNER_DIVISION
        End Get
        Set(ByVal value As String)
            _PRIMARY_OWNER_DIVISION = value
        End Set
    End Property
    Private _BUSINESS_GROUP As String = ""
    Public Property BUSINESS_GROUP As String Implements IBUS.iCustomerLine.BUSINESS_GROUP
        Get
            Return _BUSINESS_GROUP
        End Get
        Set(ByVal value As String)
            _BUSINESS_GROUP = value
        End Set
    End Property
    Private _ACCOUNT_TYPE As String = ""
    Public Property ACCOUNT_TYPE As String Implements IBUS.iCustomerLine.ACCOUNT_TYPE
        Get
            Return _ACCOUNT_TYPE
        End Get
        Set(ByVal value As String)
            _ACCOUNT_TYPE = value
        End Set
    End Property
    Private _FACT2005 As Decimal = 0
    Public Property FACT2005 As Decimal Implements IBUS.iCustomerLine.FACT2005
        Get
            Return _FACT2005
        End Get
        Set(ByVal value As Decimal)
            _FACT2005 = value
        End Set
    End Property
    Private _FACT2006 As Decimal = 0
    Public Property FACT2006 As Decimal Implements IBUS.iCustomerLine.FACT2006
        Get
            Return _FACT2006
        End Get
        Set(ByVal value As Decimal)
            _FACT2006 = value
        End Set
    End Property
    Private _FACT2007 As Decimal = 0
    Public Property FACT2007 As Decimal Implements IBUS.iCustomerLine.FACT2007
        Get
            Return _FACT2007
        End Get
        Set(ByVal value As Decimal)
            _FACT2007 = value
        End Set
    End Property
    Private _FACT2008 As Decimal = 0
    Public Property FACT2008 As Decimal Implements IBUS.iCustomerLine.FACT2008
        Get
            Return _FACT2008
        End Get
        Set(ByVal value As Decimal)
            _FACT2008 = value
        End Set
    End Property
    Private _FACT2009 As Decimal = 0
    Public Property FACT2009 As Decimal Implements IBUS.iCustomerLine.FACT2009
        Get
            Return _FACT2009
        End Get
        Set(ByVal value As Decimal)
            _FACT2009 = value
        End Set
    End Property
    Private _FACT2010 As Decimal = 0
    Public Property FACT2010 As Decimal Implements IBUS.iCustomerLine.FACT2010
        Get
            Return _FACT2010
        End Get
        Set(ByVal value As Decimal)
            _FACT2010 = value
        End Set
    End Property
    Private _LAST_BUY_DATE As DateTime = Now.Date.ToShortDateString
    Public Property LAST_BUY_DATE As DateTime Implements IBUS.iCustomerLine.LAST_BUY_DATE
        Get
            Return _LAST_BUY_DATE
        End Get
        Set(ByVal value As DateTime)
            _LAST_BUY_DATE = value
        End Set
    End Property
    Private _ORDERS_IN_PAST_YEAR As Integer = 0
    Public Property ORDERS_IN_PAST_YEAR As Integer Implements IBUS.iCustomerLine.ORDERS_IN_PAST_YEAR
        Get
            Return _ORDERS_IN_PAST_YEAR
        End Get
        Set(ByVal value As Integer)
            _ORDERS_IN_PAST_YEAR = value
        End Set
    End Property
    Private _AMOUNT_IN_PAST_YEAR As Decimal = 0
    Public Property AMOUNT_IN_PAST_YEAR As Decimal Implements IBUS.iCustomerLine.AMOUNT_IN_PAST_YEAR
        Get
            Return _AMOUNT_IN_PAST_YEAR
        End Get
        Set(ByVal value As Decimal)
            _AMOUNT_IN_PAST_YEAR = value
        End Set
    End Property
    Private _ORDERS_IN_PAST_HALFYEAR As Integer = 0
    Public Property ORDERS_IN_PAST_HALFYEAR As Integer Implements IBUS.iCustomerLine.ORDERS_IN_PAST_HALFYEAR
        Get
            Return _ORDERS_IN_PAST_HALFYEAR
        End Get
        Set(ByVal value As Integer)
            _ORDERS_IN_PAST_HALFYEAR = value
        End Set
    End Property
    Private _CUST_IND As String = ""
    Public Property CUST_IND As String Implements IBUS.iCustomerLine.CUST_IND
        Get
            Return _CUST_IND
        End Get
        Set(ByVal value As String)
            _CUST_IND = value
        End Set
    End Property
    Private _VM As String = ""
    Public Property VM As String Implements IBUS.iCustomerLine.VM
        Get
            Return _VM
        End Get
        Set(ByVal value As String)
            _VM = value
        End Set
    End Property
    Private _PRICE_GRP As String = ""
    Public Property PRICE_GRP As String Implements IBUS.iCustomerLine.PRICE_GRP
        Get
            Return _PRICE_GRP
        End Get
        Set(ByVal value As String)
            _PRICE_GRP = value
        End Set
    End Property
    Private _PRICE_LIST As String = ""
    Public Property PRICE_LIST As String Implements IBUS.iCustomerLine.PRICE_LIST
        Get
            Return _PRICE_LIST
        End Get
        Set(ByVal value As String)
            _PRICE_LIST = value
        End Set
    End Property
    Private _INCO1 As String = ""
    Public Property INCO1 As String Implements IBUS.iCustomerLine.INCO1
        Get
            Return _INCO1
        End Get
        Set(ByVal value As String)
            _INCO1 = value
        End Set
    End Property
    Private _INCO2 As String = ""
    Public Property INCO2 As String Implements IBUS.iCustomerLine.INCO2
        Get
            Return _INCO2
        End Get
        Set(ByVal value As String)
            _INCO2 = value
        End Set
    End Property
    Private _PAYMENT_TERM_CODE As String = ""
    Public Property PAYMENT_TERM_CODE As String Implements IBUS.iCustomerLine.PAYMENT_TERM_CODE
        Get
            Return _PAYMENT_TERM_CODE
        End Get
        Set(ByVal value As String)
            _PAYMENT_TERM_CODE = value
        End Set
    End Property
End Class

Public Class FreightLine : Implements iCondLine

    Private _DocId As String = ""
    Public Property DocId As String Implements IBUS.iCondLine.DocId
        Get
            Return _DocId
        End Get
        Set(ByVal value As String)
            _DocId = value
        End Set
    End Property
    Private _Type As String = ""
    Public Property Type As String Implements IBUS.iCondLine.Type
        Get
            Return _Type
        End Get
        Set(ByVal value As String)
            _Type = value
        End Set
    End Property
    Private _Value As Decimal = 0
    Public Property Value As Decimal Implements IBUS.iCondLine.Value
        Get
            Return _Value
        End Get
        Set(ByVal value As Decimal)
            _Value = value
        End Set
    End Property
End Class
Public Class FreightF : Implements IBUS.iCond



    Private _errCode As COMM.Msg.eErrCode = Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property



    Public Sub Delelte(ByVal DocId As String) Implements IBUS.iCond.Delete
        Dim o As New Freight
        o.Delete(String.Format("order_id='{0}'", DocId))
    End Sub


    Public Function GetListAll(ByVal DocId As String) As System.Collections.Generic.List(Of IBUS.iCondLine) Implements IBUS.iCond.GetListAll
        Dim f As New Freight
        Dim dt As DataTable = f.GetDT(String.Format("Order_Id = '{0}'", DocId), "")
        If dt.Rows.Count > 0 Then
            Dim o As New List(Of iCondLine)
            For Each r As DataRow In dt.Rows
                Dim nf As New FreightLine
                If Not IsDBNull(r.Item("order_id")) Then
                    nf.DocId = r.Item("order_id")
                End If
                If Not IsDBNull(r.Item("fType")) Then
                    nf.Type = r.Item("fType")
                End If
                If Not IsDBNull(r.Item("fValue")) Then
                    nf.Value = r.Item("fValue")
                End If
                o.Add(nf)
            Next
            Return o
        End If
        Return Nothing
    End Function

    Public Sub Add(ByVal CondLine As IBUS.iCondLine) Implements IBUS.iCond.Add
        Dim o As New Freight
        o.Add(CondLine.DocId, CondLine.Type, CondLine.Value)
    End Sub
End Class

Public Class PartnerLine : Implements iPartnerLine
    Private _ADDRESS As String = ""
    Public Property ADDRESS As String Implements IBUS.iPartnerLine.ADDRESS
        Get
            Return _ADDRESS
        End Get
        Set(ByVal value As String)
            _ADDRESS = value
        End Set
    End Property
    Private _ATTENTION As String = ""
    Public Property ATTENTION As String Implements IBUS.iPartnerLine.ATTENTION
        Get
            Return _ATTENTION
        End Get
        Set(ByVal value As String)
            _ATTENTION = value
        End Set
    End Property
    Private _CITY As String = ""
    Public Property CITY As String Implements IBUS.iPartnerLine.CITY
        Get
            Return _CITY
        End Get
        Set(ByVal value As String)
            _CITY = value
        End Set
    End Property
    Private _COUNTRY As String = ""
    Public Property COUNTRY As String Implements IBUS.iPartnerLine.COUNTRY
        Get
            Return _COUNTRY
        End Get
        Set(ByVal value As String)
            _COUNTRY = value
        End Set
    End Property
    Private _DISTRICT As String = ""
    Public Property DISTRICT As String Implements IBUS.iPartnerLine.DISTRICT
        Get
            Return _DISTRICT
        End Get
        Set(ByVal value As String)
            _DISTRICT = value
        End Set
    End Property
    Private _ERPID As String = ""
    Public Property ERPID As String Implements IBUS.iPartnerLine.ERPID
        Get
            Return _ERPID
        End Get
        Set(ByVal value As String)
            _ERPID = value
        End Set
    End Property
    Private _MOBILE As String = ""
    Public Property MOBILE As String Implements IBUS.iPartnerLine.MOBILE
        Get
            Return _MOBILE
        End Get
        Set(ByVal value As String)
            _MOBILE = value
        End Set
    End Property
    Private _NAME As String = ""
    Public Property NAME As String Implements IBUS.iPartnerLine.NAME
        Get
            Return _NAME
        End Get
        Set(ByVal value As String)
            _NAME = value
        End Set
    End Property
    Private _ORDER_ID As String = ""
    Public Property ORDER_ID As String Implements IBUS.iPartnerLine.ORDER_ID
        Get
            Return _ORDER_ID
        End Get
        Set(ByVal value As String)
            _ORDER_ID = value
        End Set
    End Property
    Private _ROWID As String = ""
    Public Property ROWID As String Implements IBUS.iPartnerLine.ROWID
        Get
            Return _ROWID
        End Get
        Set(ByVal value As String)
            _ROWID = value
        End Set
    End Property
    Private _STATE As String = ""
    Public Property STATE As String Implements IBUS.iPartnerLine.STATE
        Get
            Return _STATE
        End Get
        Set(ByVal value As String)
            _STATE = value
        End Set
    End Property
    Private _STREET As String = ""
    Public Property STREET As String Implements IBUS.iPartnerLine.STREET
        Get
            Return _STREET
        End Get
        Set(ByVal value As String)
            _STREET = value
        End Set
    End Property
    Private _STREET2 As String = ""
    Public Property STREET2 As String Implements IBUS.iPartnerLine.STREET2
        Get
            Return _STREET2
        End Get
        Set(ByVal value As String)
            _STREET2 = value
        End Set
    End Property
    Private _TEL As String = ""
    Public Property TEL As String Implements IBUS.iPartnerLine.TEL
        Get
            Return _TEL
        End Get
        Set(ByVal value As String)
            _TEL = value
        End Set
    End Property
    Private _TYPE As String = ""
    Public Property TYPE As String Implements IBUS.iPartnerLine.TYPE
        Get
            Return _TYPE
        End Get
        Set(ByVal value As String)
            _TYPE = value
        End Set
    End Property
    Private _ZIPCODE As String = ""
    Public Property ZIPCODE As String Implements IBUS.iPartnerLine.ZIPCODE
        Get
            Return _ZIPCODE
        End Get
        Set(ByVal value As String)
            _ZIPCODE = value
        End Set
    End Property
End Class
Public Class Partner : Implements IBUS.iPartner

    Private _errCode As COMM.Msg.eErrCode = Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property



    Public Sub Delelte(ByVal DocId As String, ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iPartner.Delete
        Dim O As New EQPARTNER(oType)
        O.Delete(String.Format("QuoteID='{0}'", DocId))
    End Sub

    Public Function GetListAll(ByVal DocId As String, ByVal oType As COMM.Fixer.eDocType) As System.Collections.Generic.List(Of IBUS.iPartnerLine) Implements IBUS.iPartner.GetListAll
        Dim o As New EQPARTNER(oType)
        Dim dt As DataTable = o.GetDT(String.Format("QuoteId='{0}'", DocId), "")
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            Dim pl As New List(Of IBUS.iPartnerLine)
            For Each r As DataRow In dt.Rows
                Dim p As New PartnerLine
                If Not IsDBNull(r.Item("QuoteId")) Then
                    p.ORDER_ID = r.Item("QuoteId")
                End If
                If Not IsDBNull(r.Item("ROWID")) Then
                    p.ROWID = r.Item("ROWID")
                End If
                If Not IsDBNull(r.Item("ERPID")) Then
                    p.ERPID = r.Item("ERPID")
                End If
                If Not IsDBNull(r.Item("NAME")) Then
                    p.NAME = r.Item("NAME")
                End If
                If Not IsDBNull(r.Item("ADDRESS")) Then
                    p.ADDRESS = r.Item("ADDRESS")
                End If
                If Not IsDBNull(r.Item("TYPE")) Then
                    p.TYPE = r.Item("TYPE")
                End If
                If Not IsDBNull(r.Item("ATTENTION")) Then
                    p.ATTENTION = r.Item("ATTENTION")
                End If
                If Not IsDBNull(r.Item("TEL")) Then
                    p.TEL = r.Item("TEL")
                End If
                If Not IsDBNull(r.Item("MOBILE")) Then
                    p.MOBILE = r.Item("MOBILE")
                End If
                If Not IsDBNull(r.Item("ZIPCODE")) Then
                    p.ZIPCODE = r.Item("ZIPCODE")
                End If
                If Not IsDBNull(r.Item("COUNTRY")) Then
                    p.COUNTRY = r.Item("COUNTRY")
                End If
                If Not IsDBNull(r.Item("CITY")) Then
                    p.CITY = r.Item("CITY")
                End If
                If Not IsDBNull(r.Item("STREET")) Then
                    p.STREET = r.Item("STREET")
                End If
                If Not IsDBNull(r.Item("STATE")) Then
                    p.STATE = r.Item("STATE")
                End If
                If Not IsDBNull(r.Item("DISTRICT")) Then
                    p.DISTRICT = r.Item("DISTRICT")
                End If
                If Not IsDBNull(r.Item("STREET2")) Then
                    p.STREET2 = r.Item("STREET2")
                End If
                pl.Add(p)
            Next
            Return pl
        End If
        Return Nothing
    End Function

    Public Sub Add(ByVal PartnerLine As IBUS.iPartnerLine, ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iPartner.Add
        Dim O As New EQPARTNER(oType)
        O.Add(PartnerLine.ORDER_ID, PartnerLine.ROWID, PartnerLine.ERPID, PartnerLine.NAME, PartnerLine.ADDRESS, PartnerLine.TYPE, _
              PartnerLine.ATTENTION, PartnerLine.TEL, PartnerLine.MOBILE, PartnerLine.ZIPCODE, PartnerLine.COUNTRY, PartnerLine.CITY, PartnerLine.STREET, _
              PartnerLine.STATE, PartnerLine.DISTRICT, PartnerLine.STREET2)
    End Sub
End Class
Public Class NoteLine : Implements iNoteLine

    Private _notetext As String = ""
    Public Property notetext As String Implements IBUS.iNoteLine.notetext
        Get
            Return _notetext
        End Get
        Set(ByVal value As String)
            _notetext = value
        End Set
    End Property
    Private _notetype As String = ""
    Public Property notetype As String Implements IBUS.iNoteLine.notetype
        Get
            Return _notetype
        End Get
        Set(ByVal value As String)
            _notetype = value
        End Set
    End Property
    Private _QUOTEID As String = ""
    Public Property QUOTEID As String Implements IBUS.iNoteLine.QUOTEID
        Get
            Return _QUOTEID
        End Get
        Set(ByVal value As String)
            _QUOTEID = value
        End Set
    End Property
End Class
Public Class Note : Implements IBUS.iNote

    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get

        End Get
    End Property
    Public Function GetListAll(ByVal DocId As String, ByVal oType As COMM.Fixer.eDocType) As System.Collections.Generic.List(Of IBUS.iNoteLine) Implements IBUS.iNote.GetListAll
        Dim O As New QuotationNote(oType)
        Dim dt As DataTable = o.GetDT(String.Format("QuoteId='{0}'", DocId), "")
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            Dim pl As New List(Of IBUS.iNoteLine)
            For Each r As DataRow In dt.Rows
                Dim p As New NoteLine
                If Not IsDBNull(r.Item("QuoteId")) Then
                    p.QUOTEID = r.Item("QuoteId")
                End If
                If Not IsDBNull(r.Item("notetext")) Then
                    p.notetext = r.Item("notetext")
                End If
                If Not IsDBNull(r.Item("notetype")) Then
                    p.notetype = r.Item("notetype")
                End If
                pl.Add(p)
            Next
            Return pl
        End If
        Return Nothing
    End Function
End Class
Public Class DocTxt : Implements iDocText


    Public Sub Delete(ByVal DocId As String) Implements IBUS.iDocText.Delete
        Dim o As New doctext
        o.Delete(String.Format("DocId='{0}'", DocId))
    End Sub

    Public Function GetListAll(ByVal DocId As String) As System.Collections.Generic.List(Of IBUS.iDocTextLine) Implements IBUS.iDocText.GetListAll
        Dim o As New doctext
        Dim dt As DataTable = o.GetDT(String.Format("DocId='{0}'", DocId), "")
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            Dim pl As New List(Of IBUS.iDocTextLine)
            For Each r As DataRow In dt.Rows
                Dim p As New DocTxtLine
                If Not IsDBNull(r.Item("DocId")) Then
                    p.DocId = r.Item("DocId")
                End If
                If Not IsDBNull(r.Item("oType")) Then
                    p.Type = r.Item("oType")
                End If
                If Not IsDBNull(r.Item("TXT")) Then
                    p.Txt = r.Item("TXT")
                End If
                pl.Add(p)
            Next
            Return pl
        End If
        Return Nothing
    End Function

    Public Sub Add(ByVal TextLine As IBUS.iDocTextLine) Implements IBUS.iDocText.Add
        Dim o As New doctext
        o.Add(TextLine.DocId, TextLine.Txt, TextLine.Type)
    End Sub
End Class
Public Class DocTxtLine : Implements iDocTextLine
    Private _DocId As String = ""
    Public Property DocId As String Implements IBUS.iDocTextLine.DocId
        Get
            Return _DocId
        End Get
        Set(ByVal value As String)
            _DocId = value
        End Set
    End Property
    Private _Txt As String = ""
    Public Property Txt As String Implements IBUS.iDocTextLine.Txt
        Get
            Return _Txt
        End Get
        Set(ByVal value As String)
            _Txt = value
        End Set
    End Property
    Private _Type As String = ""
    Public Property Type As String Implements IBUS.iDocTextLine.Type
        Get
            Return _Type
        End Get
        Set(ByVal value As String)
            _Type = value
        End Set
    End Property
End Class
Public Class PODoc
    Public Sub Delete(ByVal QuoteId As String)
        Dim o As New QUOTEPOFILEMAPPING
        o.Delete(String.Format("QuoteId='{0}'", QuoteId))
    End Sub

    Public Function GetListAll(ByVal QuoteId As String) As System.Collections.Generic.List(Of PODocLine)
        Dim o As New QUOTEPOFILEMAPPING
        Dim dt As DataTable = o.GetDT(String.Format("QuoteID='{0}'", QuoteId), "")
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            Dim pl As New List(Of PODocLine)
            For Each r As DataRow In dt.Rows
                Dim p As New PODocLine
                If Not IsDBNull(r.Item("QuoteID")) Then
                    p.QuoteId = r.Item("QuoteID")
                End If
                If Not IsDBNull(r.Item("PODoc")) Then
                    p.DocData = r.Item("PODoc")
                End If
                pl.Add(p)
            Next
            Return pl
        End If
        Return Nothing
    End Function

    Public Sub Add(ByVal PODoc As PODocLine)
        Dim o As New QUOTEPOFILEMAPPING
        o.Add(PODoc.QuoteId, PODoc.DocData)
    End Sub
End Class
Public Class PODocLine
    Private _QuoteId As String = ""
    Public Property QuoteId As String
        Get
            Return _QuoteId
        End Get
        Set(ByVal value As String)
            _QuoteId = value
        End Set
    End Property
    Private _DocData As Byte() = Nothing
    Public Property DocData As Byte()
        Get
            Return _DocData
        End Get
        Set(ByVal value As Byte())
            _DocData = value
        End Set
    End Property
End Class

Public Class Cred : Implements iCredit

    Private _errCode As COMM.Msg.eErrCode = Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property



    Public Sub Delete(ByVal DocId As String) Implements IBUS.iCredit.Delete
        Dim o As New credit
        o.Delete(String.Format("OrderID='{0}'", DocId))
    End Sub

    Public Function GetListAll(ByVal DocId As String) As System.Collections.Generic.List(Of IBUS.iCreditLine) Implements IBUS.iCredit.GetListAll
        Dim o As New credit
        Dim dt As DataTable = o.GetDT(String.Format("OrderID='{0}'", DocId), "")
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            Dim pl As New List(Of IBUS.iCreditLine)
            For Each r As DataRow In dt.Rows
                Dim p As New CredLine
                If Not IsDBNull(r.Item("OrderID")) Then
                    p.DocID = r.Item("OrderID")
                End If
                If Not IsDBNull(r.Item("HOLDER")) Then
                    p.HOLDER = r.Item("HOLDER")
                End If
                If Not IsDBNull(r.Item("EXPIRED")) Then
                    p.EXPIRED = r.Item("EXPIRED")
                End If
                If Not IsDBNull(r.Item("TYPE")) Then
                    p.TYPE = r.Item("TYPE")
                End If
                If Not IsDBNull(r.Item("NUMBER")) Then
                    p.NUMBER = r.Item("NUMBER")
                End If
                If Not IsDBNull(r.Item("VERIFICATION_VALUE")) Then
                    p.VERIFICATION_VALUE = r.Item("VERIFICATION_VALUE")
                End If
                pl.Add(p)
            Next
            Return pl
        End If
        Return Nothing
    End Function

    Public Sub Add(ByVal CreditLine As IBUS.iCreditLine) Implements IBUS.iCredit.Add
        Dim o As New credit
        o.Add(CreditLine.DocID, CreditLine.HOLDER, CreditLine.EXPIRED, CreditLine.TYPE, CreditLine.NUMBER, CreditLine.VERIFICATION_VALUE)
    End Sub
End Class
Public Class CredLine : Implements iCreditLine
    Private _DocId As String = ""
    Public Property DocID As String Implements IBUS.iCreditLine.DocID
        Get
            Return _DocId
        End Get
        Set(ByVal value As String)
            _DocId = value
        End Set
    End Property
    Private _EXPIRED As Date = Now.Date.ToShortDateString
    Public Property EXPIRED As Date Implements IBUS.iCreditLine.EXPIRED
        Get
            Return _EXPIRED
        End Get
        Set(ByVal value As Date)
            _EXPIRED = value
        End Set
    End Property
    Private _HOLDER As String = ""
    Public Property HOLDER As String Implements IBUS.iCreditLine.HOLDER
        Get
            Return _HOLDER
        End Get
        Set(ByVal value As String)
            _HOLDER = value
        End Set
    End Property
    Private _NUMBER As String = ""
    Public Property NUMBER As String Implements IBUS.iCreditLine.NUMBER
        Get
            Return _NUMBER
        End Get
        Set(ByVal value As String)
            _NUMBER = value
        End Set
    End Property
    Private _TYPE As String = ""
    Public Property TYPE As String Implements IBUS.iCreditLine.TYPE
        Get
            Return _TYPE
        End Get
        Set(ByVal value As String)
            _TYPE = value
        End Set
    End Property
    Private _VERIFICATION_VALUE As String = ""
    Public Property VERIFICATION_VALUE As String Implements IBUS.iCreditLine.VERIFICATION_VALUE
        Get
            Return _VERIFICATION_VALUE
        End Get
        Set(ByVal value As String)
            _VERIFICATION_VALUE = value
        End Set
    End Property
End Class


Public Class OrderProcs : Implements IBUS.iOrderProcS




    Public Sub Delete(ByVal DocId As String) Implements IBUS.iOrderProcS.Delete
        Dim o As New ORDER_PROC_STATUS2
        o.Delete(String.Format("ORDER_NO='{0}'", DocId))
    End Sub

    Public Function GetListAll(ByVal DocId As String) As System.Collections.Generic.List(Of IBUS.iOrderProcSLine) Implements IBUS.iOrderProcS.GetListAll
        Dim o As New ORDER_PROC_STATUS2
        Dim dt As DataTable = o.GetDT(String.Format("ORDER_NO='{0}'", DocId), "")
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            Dim pl As New List(Of IBUS.iOrderProcSLine)
            For Each r As DataRow In dt.Rows
                Dim p As New OrderProcsLine
                If Not IsDBNull(r.Item("ORDER_NO")) Then
                    p.ORDER_NO = r.Item("ORDER_NO")
                End If
                If Not IsDBNull(r.Item("LINE_SEQ")) Then
                    p.LINE_SEQ = r.Item("LINE_SEQ")
                End If
                If Not IsDBNull(r.Item("NUMBER")) Then
                    p.NUMBER = r.Item("NUMBER")
                End If
                If Not IsDBNull(r.Item("MESSAGE")) Then
                    p.MESSAGE = r.Item("MESSAGE")
                End If
                If Not IsDBNull(r.Item("CREATED_DATE")) Then
                    p.CREATED_DATE = r.Item("CREATED_DATE")
                End If
                If Not IsDBNull(r.Item("STATUS")) Then
                    p.STATUS = r.Item("STATUS")
                End If
                If Not IsDBNull(r.Item("TYPE")) Then
                    p.TYPE = r.Item("TYPE")
                End If
                pl.Add(p)
            Next
            Return pl
        End If
        Return Nothing
    End Function

    Public Sub Add(ByVal OrderProcsLine As IBUS.iOrderProcSLine) Implements IBUS.iOrderProcS.Add
        Dim o As New ORDER_PROC_STATUS2
        o.Add(OrderProcsLine.ORDER_NO, OrderProcsLine.LINE_SEQ, OrderProcsLine.NUMBER, OrderProcsLine.MESSAGE, OrderProcsLine.CREATED_DATE, OrderProcsLine.STATUS, OrderProcsLine.TYPE)
    End Sub
End Class
Public Class OrderProcsLine : Implements IBUS.iOrderProcSLine
    Private _CREATED_DATE As Date = Now.Date.ToShortDateString
    Public Property CREATED_DATE As Date Implements IBUS.iOrderProcSLine.CREATED_DATE
        Get
            Return _CREATED_DATE
        End Get
        Set(ByVal value As Date)
            _CREATED_DATE = value
        End Set
    End Property
    Private _LINE_SEQ As Integer = 0
    Public Property LINE_SEQ As Integer Implements IBUS.iOrderProcSLine.LINE_SEQ
        Get
            Return _LINE_SEQ
        End Get
        Set(ByVal value As Integer)
            _LINE_SEQ = value
        End Set
    End Property
    Private _MESSAGE As String = ""
    Public Property MESSAGE As String Implements IBUS.iOrderProcSLine.MESSAGE
        Get
            Return _MESSAGE
        End Get
        Set(ByVal value As String)
            _MESSAGE = value
        End Set
    End Property
    Private _NUMBER As Integer = 0
    Public Property NUMBER As Integer Implements IBUS.iOrderProcSLine.NUMBER
        Get
            Return _NUMBER
        End Get
        Set(ByVal value As Integer)
            _NUMBER = value
        End Set
    End Property
    Private _ORDER_NO As String = ""
    Public Property ORDER_NO As String Implements IBUS.iOrderProcSLine.ORDER_NO
        Get
            Return _ORDER_NO
        End Get
        Set(ByVal value As String)
            _ORDER_NO = value
        End Set
    End Property
    Private _STATUS As Integer = 1
    Public Property STATUS As Integer Implements IBUS.iOrderProcSLine.STATUS
        Get
            Return _STATUS
        End Get
        Set(ByVal value As Integer)
            _STATUS = value
        End Set
    End Property
    Private _TYPE As String = ""
    Public Property TYPE As String Implements IBUS.iOrderProcSLine.TYPE
        Get
            Return _TYPE
        End Get
        Set(ByVal value As String)
            _TYPE = value
        End Set
    End Property
End Class