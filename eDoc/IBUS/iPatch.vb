Public Interface iPatch
    Function IsEUStockingProgram(ByVal PartNO As String, ByVal QTY As Integer, ByVal CompanyId As String) As Boolean
    Function IsEUStockingCompany(ByVal companyId As String) As Boolean
    Function ReplaceCartBTO(ByVal PartNo As String, ByVal Org As String) As String
    Function isSBCBtoOrder(ByVal CartList As IBUS.iCartList, ByVal ERPID As String) As Boolean
    Function isODMCart(ByVal CartList As IBUS.iCartList) As Boolean
    Function isHasBto(ByVal CartList As IBUS.iCartList) As Boolean
    Function GetCustomerOffice(ByVal connectionName As String, ByVal sql As String) As String
End Interface
