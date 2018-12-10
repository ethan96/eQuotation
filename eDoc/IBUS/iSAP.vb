Public Interface iSAP : Inherits iBase
    Function getBTOWorkingDate(ByVal org As String) As COMM.Fixer.eBTOAssemblyDays
    Function getBTOParentDueDate(ByVal reqDate As String, ByVal org As String) As String
    Function getCompNextWorkDate(ByVal reqDate As String, ByVal org As String, Optional ByVal days As Integer = 0) As String
    Function GetNextWeeklyShippingDate(ByVal reqDate As Date, ByRef NextWeeklyShipDate As Date, ByVal CompanyID As String) As Boolean
    Function getBTOChildDueDate(ByVal reqDate As String, ByVal org As String) As String
    Function getNextCustDelDate(ByVal ddate As String, ByVal CompanyId As String) As String
    Function UpdateSAPSOShipToAttentionAddress( _
        ByVal SONO As String, ByVal ShipToId As String, ByVal Name As String, ByVal Attention As String, ByVal Street As String, ByVal Street2 As String, ByVal City As String, ByVal State As String, ByVal Zipcode As String, _
        ByVal Country As String, ByRef ReturnTable As DataTable, Optional ByVal IsSAPProductionServer As Boolean = True) As Boolean

    Function UpdateSOZeroPriceItems(ByVal SO_NO As String, ByVal C As IBUS.iCartList, ByRef ReturnTable As DataTable) As Boolean
    Function UpdateSOSpecId(ByVal SO_NO As String, ByVal SpecialIndicator As COMM.Fixer.eEarlyShipment, ByRef ReturnTable As DataTable) As Boolean
    Function VerifyDistChannelDivisionGroupOffice(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String, _
                                       ByVal strDivision As String, ByVal OrderDocType As String, ByVal SalesGroup As String, ByVal SalesOffice As String, ByRef ReturnTable As DataTable) As Boolean

    Function UpdateSOWarrantyFlagByTable(ByVal dt As DataTable, ByRef S As String, ByVal retCode As Boolean)
    Function OrderSimulate(ByVal Header As IBUS.iDocHeaderLine, _
                                   ByRef Partner As System.Collections.Generic.List(Of IBUS.iPartnerLine), _
                                   ByRef Ret As DataTable, _
                                   Optional ByRef Items As IBUS.iCartList = Nothing, _
                                   Optional ByRef Credit As System.Collections.Generic.List(Of IBUS.iCreditLine) = Nothing, _
                                   Optional ByRef Condition As System.Collections.Generic.List(Of IBUS.iCondLine) = Nothing) As Boolean
    Function OrderCreate(ByVal Header As IBUS.iDocHeaderLine,
                                ByVal Partner As System.Collections.Generic.List(Of IBUS.iPartnerLine), _
                                ByRef Ret As DataTable, ByRef errMsg As String, _
                                Optional ByVal Items As IBUS.iCartList = Nothing, _
                                Optional ByVal HeaderText As System.Collections.Generic.List(Of IBUS.iDocTextLine) = Nothing, _
                                Optional ByVal Condition As System.Collections.Generic.List(Of IBUS.iCondLine) = Nothing, _
                                Optional ByVal Credit As System.Collections.Generic.List(Of IBUS.iCreditLine) = Nothing, _
                                Optional ByVal isProduction As Boolean = False) As Boolean
    Function CreateQuotation(ByVal Header As IBUS.iDocHeaderLine, _
                                     ByVal Partner As System.Collections.Generic.List(Of IBUS.iPartnerLine), _
                                     ByRef Ret As DataTable, ByRef errMsg As String,
                                     Optional ByVal Items As IBUS.iCartList = Nothing, _
                                     Optional ByVal HeaderText As System.Collections.Generic.List(Of IBUS.iDocTextLine) = Nothing, _
                                     Optional ByVal Condition As System.Collections.Generic.List(Of IBUS.iCondLine) = Nothing, _
                                     Optional ByVal isProduction As Boolean = False) As Boolean

    Function getInventoryAndATPTable(ByVal PartNo As String, _
                                                 ByVal Plant As String, _
                                                 ByVal reqQty As Integer, _
                                                 Optional ByRef DueDate As String = "", _
                                                 Optional ByRef Inventory As Integer = 0, _
                                                 Optional ByRef ATPtable As DataTable = Nothing, _
                                                 Optional ByVal reqDate As String = "", _
                                                 Optional ByRef satisFlag As Integer = 1, _
                                                 Optional ByRef qtyCanBeConfirm As Int64 = 0) As Integer
End Interface
