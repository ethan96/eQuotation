Imports System.Configuration
<Serializable()> _
Public Class ProductX
    Private _partno As String
    Private _org As String
    Private _plant As String
    Private _isphaseout As Boolean
    Private _StatusCode As String
    Private _StatusDesc As String
    Private _ATPQty As Integer
    Public Sub New(ByVal partno As String, ByVal org As String, ByVal plant As String)
        _partno = partno
        _org = org
        _plant = plant
        _isphaseout = False
        _ATPQty = 0
    End Sub
    Public Sub New()
    End Sub
    Property PartNo As String
        Get
            Return _partno
        End Get
        Set(ByVal value As String)
            _partno = value
        End Set
    End Property
    Property ORG As String
        Get
            Return _org
        End Get
        Set(ByVal value As String)
            _org = value
        End Set
    End Property
    Property Plant As String
        Get
            If String.IsNullOrEmpty(_plant) Then
                Dim sql As String = String.Format("select top 1 DELIVERYPLANT from SAP_PRODUCT_ORG where ORG_ID='{1}' and PART_NO = '{0}'", Me.PartNo, Me.ORG)
                Dim obj As Object = dbUtil.dbExecuteScalar("MY", sql)
                If obj IsNot Nothing Then
                    Return obj.ToString.Trim
                End If
            End If
            Return _plant
        End Get
        Set(ByVal value As String)
            _plant = value
        End Set
    End Property
    Property IsPhaseOut As Boolean
        Get
            Return _isphaseout
        End Get
        Set(ByVal value As Boolean)
            _isphaseout = value
        End Set
    End Property
    Property StatusCode As String
        Get
            Return _StatusCode
        End Get
        Set(ByVal value As String)
            _StatusCode = value
        End Set
    End Property
    Property StatusDesc As String
        Get
            Return _StatusDesc
        End Get
        Set(ByVal value As String)
            _StatusDesc = value
        End Set
    End Property
    Property ATPQty As Integer
        Get
            Return _ATPQty
        End Get
        Set(ByVal value As Integer)
            _ATPQty = value
        End Set
    End Property
    ''' <summary>
    ''' 'Get SAP status and ATP on runtime
    ''' </summary>
    ''' <param name="Products">料集合</param>
    ''' <param name="SalesOrg">SalesOrg</param>
    ''' <param name="IsHavePhaseout">IsHavePhaseout</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProductInfo(ByVal Products As List(Of ProductX), ByVal SalesOrg As String, ByRef IsHavePhaseout As Boolean) As List(Of ProductX)
        Dim pns As List(Of String) = New List(Of String)
        For Each i As ProductX In Products
            If i.PartNo.EndsWith("-BTO", StringComparison.InvariantCultureIgnoreCase) Then
                pns.Add("'" + SAPDAL.replaceCartBTO(i.PartNo, SalesOrg) + "'")
            Else
                pns.Add("'" + Global_Inc.Format2SAPItem(i.PartNo) + "'")
            End If
        Next
        Dim strSql As String = _
            " select a.matnr,a.vmsta as status_code, b.vmstb as status_desc from saprdp.MVKE a left join saprdp.TVMST b on a.vmsta=b.vmsta where a.mandt='168' " + _
            " and b.mandt='168' and a.vkorg='" + SalesOrg + "' and a.matnr in(" + String.Join(",", pns.ToArray()) + ") and b.spras='E' "
        Dim dtProdStatus As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
        Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP
        p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
        p1.Connection.Open()
        Dim PartNo As String = ""
        For Each P As ProductX In Products
            PartNo = P.PartNo.ToString().Trim()
            If P.PartNo.EndsWith("-BTO", StringComparison.InvariantCultureIgnoreCase) Then
                PartNo = SAPDAL.replaceCartBTO(P.PartNo, SalesOrg)
            End If
            Dim statusrow As DataRow() = dtProdStatus.Select(String.Format("matnr='{0}'", Global_Inc.Format2SAPItem(PartNo)))
            Dim _isphaseout As Boolean = False
            If statusrow.Length >= 1 Then
                Dim currRow As DataRow = statusrow(0)
                P.StatusCode = currRow.Item("status_code")
                P.StatusDesc = currRow.Item("status_desc")
                Dim ATPQty As Integer = 0
                Select Case currRow.Item("status_code")
                    Case "A", "C", "N", "H", "M1", "P", "S2", "S5", "V", "T"
                        _isphaseout = False
                    Case "O", "S"
                        Dim intInventory As Integer = -1
                        Dim atpTb As New GET_MATERIAL_ATP.BAPIWMDVETable, retTb As New GET_MATERIAL_ATP.BAPIWMDVSTable, rOfretTb As New GET_MATERIAL_ATP.BAPIWMDVS
                        rOfretTb.Req_Date = Now.ToString("yyyyMMdd") : rOfretTb.Req_Qty = 999 : retTb.Add(rOfretTb)

                        Dim DlvPlant As String = Left(SalesOrg, 2) + "H1"

                        p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", Global_Inc.Format2SAPItem(P.PartNo), DlvPlant, "", "", "", "", "PC",
                                               "", intInventory, "", "", New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)

                        For i As Integer = 0 To atpTb.Count - 1
                            If atpTb(i).Com_Qty > 0 Then
                                ATPQty = atpTb(i).Com_Qty
                                _isphaseout = False
                                Exit For
                            End If
                        Next
                        If ATPQty = 0 Then _isphaseout = True
                    Case "I"
                        _isphaseout = True
                    Case Else
                        _isphaseout = True
                End Select
                P.IsPhaseOut = _isphaseout
                P.ATPQty = ATPQty
            Else
                'JJ 2014/3/18：查無此料號時，IsPhaseOut=true、StatusCode=""、StatusDesc = "Invalid part number"
                P.IsPhaseOut = True
                P.StatusCode = ""
                P.StatusDesc = "Invalid part number"
                P.ATPQty = 0
                _isphaseout = True
            End If
            If _isphaseout Then
                IsHavePhaseout = _isphaseout
            End If
        Next
        p1.Connection.Close()
        Return Products
    End Function
End Class
