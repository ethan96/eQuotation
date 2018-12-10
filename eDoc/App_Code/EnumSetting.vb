Imports Microsoft.VisualBasic

Public Class EnumSetting
    Enum eUserRole As Integer
        Normal = 0
        ANAAOnline = 1
        AJP = 2
        Franchiser = 3
    End Enum

    Enum ExpiredDurationDay As Integer
        EUDuration = 30
        USDuration = 10
    End Enum
    Enum UserRoles As Integer
        ANAAOnline = 5
        ITOwner = 3000
        SysOwner = 600
    End Enum

    Enum ItemType As Integer
        Component = 0
        BTORoot = -1
        BTOComponent = 1
    End Enum

    Enum OrderType As Integer
        Component = 0
        BTOS = 1
    End Enum

    Enum PartnerTypes As Integer
        SoldTo = 0
        ShipTo = 1
        BillTo = 2
    End Enum

    Enum USPrintOutFormat As Integer
        MAIN_ITEM_ONLY = 0
        SUB_ITEM_WITH_SUB_ITEM_PRICE = 1
        SUB_ITEM_WITHOUT_SUB_ITEM_PRICE = 2
        SUB_ITEM_WITHPARTNO_WITHOUT_SUB_ITEM_PRICE = 3
        CP_Format = 4
        SUB_ITEM_WITH_BREAKPOINTS = 5
        CP_Format_TOTAL_LIST_PRICE = 6
    End Enum
 
    Enum QuotationForwardType As Integer
        HTML = 0
        PDF = 1
    End Enum

    Enum QuotationNumberType
        NA = -1
        Other = 0
        AUSQ = 1
        AMXQ = 2
        AJPQ = 3
        TWQ = 4
    End Enum

End Class
