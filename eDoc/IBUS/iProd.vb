Public Interface iProd : Inherits iBase
    Property type As COMM.Fixer.eProdType
    Property partNo As String
    Property org As String
    Property divPlant As String
    Property partDesc As String
    Property partModel As String
    Property productLine As String
    Property ROHS As Integer
    Property ABC As String
    Function isTaxable(ByVal PartNo As String, ByVal ShipToId As String) As Boolean
    Function isWarrantable(ByVal PN As String, ByVal docReg As COMM.Fixer.eDocReg) As Boolean
    Function isSoftware(ByVal PN As String) As Boolean
    'Property type As COMM.Fixer.eProdType
    'Property PART_NO As String
    'Property ORG_ID As String
    ''Property DIST_CHANNEL As String
    ''Property STATUS As String
    'Property B2BONLINE As String
    'Property DELIVERYPLANT As String
    'Property PRICINGGROUP As String
    ''Property LAST_UPD_DATE As String
    'Property MIN_ORD_QTY As Integer
    ''Property MIN_DLV_QTY As Integer
    'Property MODEL_NO As String
    'Property MATERIAL_GROUP As String
    'Property DIVISION As String
    'Property PRODUCT_HIERARCHY As String
    'Property PRODUCT_GROUP As String
    'Property PRODUCT_DIVISION As String
    'Property PRODUCT_LINE As String
    'Property GENITEMCATGRP As String
    'Property PRODUCT_DESC As String
    'Property ROHS_FLAG As String
    'Property STATUS As String
    'Property EGROUP As String
    'Property EDIVISION As String
    'Property NET_WEIGHT As Decimal
    'Property GROSS_WEIGHT As Decimal
    'Property WEIGHT_UNIT As String
    'Property VOLUME As Decimal
    'Property VOLUME_UNIT As String
    'Property CREATE_DATE As String
    'Property LAST_UPD_DATE As String
    'Property PRODUCT_TYPE As String
    'Property GIP_CODE As String
    'Property SIZE_DIMENSIONS As String
    'Property SALES_ORG As String
    'Property DIST_CHANNEL As String
    'Property PRODUCT_STATUS As String
    'Property MIN_ORDER_QTY As Decimal
    'Property MIN_DLV_QTY As Decimal
    'Property MIN_BTO_QTY As Decimal
    'Property DLV_PLANT As String
    'Property MATERIAL_PRICING_GRP As String
    'Property VALID_DATE As String
    'Property ITEM_CATEGORY_GROUP As String
    'Property PLANT As String
    'Property ABC_INDICATOR As String
    'Property PLANNED_DEL_TIME As Integer
    'Property GP_PROCESSING_TIME As Integer
    'Property IN_HOUSE_PRODUCTION As Integer
    'Property ProfitCenter As String
    'Property Ctrl_Code As String
    'Property safety_stock As Decimal
    'Property min_safety_stock As Decimal
    'Property service_level As Decimal
    'Property MIN_LOT_SIZE As Decimal
End Interface
Public Interface iProdF : Inherits iBase
    Function getProdByPartNo(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "") As iProd
End Interface

Public Interface iEWTypeLine
    Property NameItem As String
    Property Month As Integer
    Property Rate As Decimal
    Property Type As Integer
End Interface

Public Interface iEWUtil
    Function getRealMonthByMonth(ByVal Month As Integer, ByVal DocReg As COMM.Fixer.eDocReg) As Integer
    Function getMonthByEWItem(ByVal itemNo As String, ByVal DocReg As COMM.Fixer.eDocReg) As Integer
    Function getRateByEWItem(ByVal itemNo As String, ByVal DocReg As COMM.Fixer.eDocReg) As Decimal
    Function getEWItemByMonth(ByVal month As Integer, ByVal DocReg As COMM.Fixer.eDocReg) As String
    Function getListByReg(ByVal DocReg As COMM.Fixer.eDocReg, ByVal Type As Integer) As List(Of iEWTypeLine)
End Interface