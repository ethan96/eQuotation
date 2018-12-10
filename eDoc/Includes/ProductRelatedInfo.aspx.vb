Public Class ProductRelatedInfo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    '20130530 TC: Add Page Method GetACLATP to let client side using jQuery Ajax to get ACL inventory and show it right beside product related info block
    <Services.WebMethod()> _
        <Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetACLATP(ByVal strPartNo As String, ByVal strPlant As String _
                                     , ByVal shipvia As Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia) As String
        Dim ATP As New StringBuilder
        Dim prod_input As New SAPDAL.SAPDALDS.ProductInDataTable, _sapdal As New SAPDAL.SAPDAL
        Dim MainDeliveryPlant As String = "TWH1", _errormsg As String = String.Empty
        If Not String.IsNullOrEmpty(strPlant) Then MainDeliveryPlant = strPlant
        Dim inventory_out As New SAPDAL.SAPDALDS.QueryInventory_OutputDataTable
        prod_input.AddProductInRow(strPartNo, 0, MainDeliveryPlant)
        If _sapdal.QueryInventory_V2(prod_input, MainDeliveryPlant, Now, inventory_out, _errormsg) Then
            'Dim atpInfoObj As New ATPTotalInfo
            'atpInfoObj.PartNo = strPartNo
            'atpInfoObj.ATPRecords = New List(Of ATPRecord)
            ATP.Append("<table valign=""top"" style=""width: 120px;float: left;margin-left:2px""><tr ><th colspan='2' style='color:Black'>Plant: " + strPlant + "</th></tr>")
            'If shipvia <> Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.NA Then
            '    ATP.Append("<th colspan='2' style='color:Black'>Ship via: " + shipvia.ToString + "</th></tr>")
            'End If
            ATP.Append("<tr class='HeaderStyle'><th>Available Date</th><th>Qty</th></tr>")

            'Dim _NewDateByShipVia As Date
            For Each invRow As SAPDAL.SAPDALDS.QueryInventory_OutputRow In inventory_out
                ' Dim atpRec As New ATPRecord
                'atpRec.Qty = invRow.STOCK : atpRec.AvailableDate = invRow.STOCK_DATE.ToString(Pivot.CurrentProfile.DatePresentationFormat)
                'atpInfoObj.ATPRecords.Add(atpRec)

                ATP.Append("<tr><td align='center'>" + invRow.STOCK_DATE.ToString(Pivot.CurrentProfile.DatePresentationFormat) + "</td><td align='right'>" + invRow.STOCK.ToString() + "</td></tr>")
                '20150729 Jay said that do not need to add postpone days to available date
                '_NewDateByShipVia = invRow.STOCK_DATE
                'Select Case shipvia
                '    Case Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.Air
                '        _NewDateByShipVia = DateAdd(DateInterval.Day, 14, invRow.STOCK_DATE)
                '    Case Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.Sea
                '        _NewDateByShipVia = DateAdd(DateInterval.Day, 42, invRow.STOCK_DATE)
                '    Case Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.NA
                '        _NewDateByShipVia = invRow.STOCK_DATE
                'End Select
                'ATP.Append("<tr><td align='center'>" + _NewDateByShipVia.ToString(Pivot.CurrentProfile.DatePresentationFormat) + "</td><td align='right'>" + invRow.STOCK.ToString() + "</td></tr>")
            Next

            If inventory_out.Rows.Count = 0 Then
                ATP.Append("<tr><td colspan=""2"">No  inventory</td></tr>")
            End If
            'Dim serializer = New Script.Serialization.JavaScriptSerializer()
            'Dim json As String = serializer.Serialize(atpInfoObj)
            'Return json
            ATP.Append(" </table>")
        End If
        Return ATP.ToString()
    End Function


    <Services.WebMethod()> _
    <Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetProductInfo(ByVal strPartNo As String, ByVal strPlant As String) As String

        Dim _sql As New StringBuilder
        _sql.AppendLine(" Select a.PART_NO,a.PRODUCT_STATUS,isnull(a.MIN_ORDER_QTY,0) as  MIN_ORDER_QTY,isnull(a.DLV_PLANT,'') as DLV_PLANT,isnull(b.PLANT,'') as PLANT,isnull(b.ABC_INDICATOR,'') as  ABC_INDICATOR,isnull(c.txt,'') as PLMNotice ")
        _sql.AppendLine(" From SAP_PRODUCT_STATUS a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT ")
        _sql.AppendLine(" left join SAP_PRODUCT_ORDERNOTE c on b.PART_NO=c.PART_NO and a.SALES_ORG=c.ORG ")
        _sql.AppendLine(String.Format(" Where a.DLV_PLANT='{1}' and a.PART_NO='{0}' ", strPartNo, strPlant))

        Dim DT As DataTable = tbOPBase.dbGetDataTable("b2b", _sql.ToString)
        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
            Dim PartInfo As New PartInfo

            With DT.Rows(0)
                PartInfo.PartNo = .Item("PART_NO") : PartInfo.ProductStatus = .Item("PRODUCT_STATUS") : PartInfo.MOQ = .Item("MIN_ORDER_QTY")
                PartInfo.Indicator = .Item("ABC_INDICATOR") : PartInfo.PLMNotice = .Item("PLMNotice")
            End With

            Dim dtplm As New DataTable
            Dim sql As String = "Select * from addontbl.V_ZCHANGE_EPO_INITIAL where ITEM_NUMBER='" & strPartNo & "'"
            'Ming 2014-07-28 因PLM数据查询效能问题暂时移除
            'dtplm = OraDbUtil.dbGetDataTable("PLM_PRD", sql)
            If dtplm IsNot Nothing AndAlso dtplm.Rows.Count > 0 Then
                PartInfo.IsApplyingForPhaseOutStatus = True
            Else
                PartInfo.IsApplyingForPhaseOutStatus = False
            End If

            Dim serializer = New Script.Serialization.JavaScriptSerializer()
            Dim json As String = serializer.Serialize(PartInfo)
            Return json

        End If

        Return ""
    End Function
    <Services.WebMethod()> _
    <Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetProductATP(ByVal strPartNo As String, ByVal strOrgid As String, ByVal AccountCurrency As String) As String

        Dim procObj As New ProcessResult
        Dim _IsIntercon As Boolean = Role.IsHQDCSales

        Try

            Dim plant1 As String = String.Empty, plant2 As String = String.Empty
            Dim plant3 As String = String.Empty, plant4 As String = String.Empty
            Dim plant5 As String = String.Empty
            Dim RetStr As New StringBuilder()
            Dim _ShipVia As Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia = Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.NA
            Select Case strOrgid.Trim
                Case "US01"
                    plant1 = "USH1"
                    'plant2 = "TWH1"
                    'Jay 20150720:Determine ACL inventory's plant by MRP2-Special Procurement
                    plant2 = Advantech.Myadvantech.Business.PartBusinessLogic.GetReferencePlantBySpecialProcurement(strPartNo, plant1)
                    'Ivy Yes, please always use USH1 material master for the default ship mode.  The TWH1 or CKH2 ship mode only determines how parts are being ship to TW & CN
                    _ShipVia = Advantech.Myadvantech.Business.PartBusinessLogic.GetShippingVia(strPartNo, plant1)

                    If Advantech.Myadvantech.Business.PartBusinessLogic.IsBBProduct(strPartNo) Then
                        plant3 = "UBH1"
                    End If
                Case "EU10"
                    plant1 = "EUH1"
                    plant2 = "TWH1"
                Case "TW01"
                    plant1 = "TWH1"
                    plant2 = "CKH2"
                Case "KR01"
                    plant1 = "KRH1"
                    plant2 = "TWH1"
                    plant3 = "CKH2"
                Case "BR01"
                    plant1 = "BRH1"
                    plant2 = "TWH1"
                Case "JP01"
                    plant1 = "TWH1"
                    plant2 = "JPH1"
                    plant3 = "TWM3"
                    plant4 = "TWM4"
                    plant5 = "CKH1"
                Case Else
                    plant1 = "TWH1"
                    plant2 = "CKH2"
            End Select


            Dim strStatusCode As String = "", strStatusDesc As String = ""
            Dim decATP As Decimal = 0, IsApplyingForPhaseOutStatus As Boolean = False
            'Frank 20140717: Execute isInvalidPhaseOutV2 first because this function will check if product information need to be sycned from SAP to MyA
            Dim _IsPhaseOut As Boolean = SAPDAL.SAPDAL.isInvalidPhaseOutV2(strPartNo, strOrgid, strStatusCode, strStatusDesc, decATP)

            Dim _sql As New StringBuilder
            _sql.AppendLine(" Select top 1 a.PART_NO,a.PRODUCT_STATUS,isnull(a.MIN_ORDER_QTY,0) as  MIN_ORDER_QTY,isnull(a.DLV_PLANT,'') as DLV_PLANT,isnull(b.PLANT,'') as PLANT,isnull(b.ABC_INDICATOR,'') as  ABC_INDICATOR,isnull(c.txt,'') as PLMNotice ")
            If _IsIntercon Then
                _sql.AppendLine(" ,d.PRODUCT_HIERARCHY ")
            End If

            _sql.AppendLine(" From SAP_PRODUCT_STATUS a left join SAP_PRODUCT_ABC b on a.PART_NO=b.PART_NO and a.DLV_PLANT=b.PLANT ")
            _sql.AppendLine(" left join SAP_PRODUCT_ORDERNOTE c on a.PART_NO=c.PART_NO and a.SALES_ORG=c.ORG ")
            If _IsIntercon Then
                _sql.AppendLine(" left join SAP_PRODUCT d on a.PART_NO=d.PART_NO ")
            End If

            _sql.AppendLine(String.Format(" Where a.SALES_ORG='{1}' and a.PART_NO='{0}' ", strPartNo, strOrgid))
            Dim DT As DataTable = tbOPBase.dbGetDataTable("b2b", _sql.ToString)
            If Not _IsPhaseOut Then
                Dim dtplm As New DataTable
                Dim sql As String = "Select ""Item_Number"" from addontbl.V_ZCHANGE_EPO_INITIAL where ""Item_Number""='" & strPartNo & "'"
                'Ming 2014-07-28 因PLM数据查询效能问题暂时移除
                'dtplm = OraDbUtil.dbGetDataTable("PLM_PRD", sql)
                If dtplm IsNot Nothing AndAlso dtplm.Rows.Count > 0 Then
                    IsApplyingForPhaseOutStatus = True
                End If
            End If

            If DT.Rows.Count > 0 Then

                Dim strMinPrice As String = ""
                If Left(strOrgid, 2).ToUpper() = "TW" Then
                    Dim tmpCurrency As String = "", tmpErrMsg As String = ""

                    Dim _MinimumPrice_SalesTeam As SAPDAL.SAPDAL.MinimumPrice_SalesTeam
                    If Role.IsTWAonlineSales Then
                        _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.ATW_AOnline
                    End If
                    If Role.IsInterconIA Then
                        _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.Intercon_iA
                    ElseIf Role.IsInterconEC OrElse Role.IsInterconIS Then
                        _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.Intercon_EC
                    End If



                    Dim tmpMinPrice As Double = SAPDAL.SAPDAL.GetMinPrice("TW01", strPartNo, AccountCurrency, _MinimumPrice_SalesTeam, tmpErrMsg, tmpCurrency)
                    If String.IsNullOrEmpty(tmpErrMsg) AndAlso tmpMinPrice >= 0 Then
                        strMinPrice = Util.FormatMoney(tmpMinPrice, tmpCurrency)
                    End If
                End If



                With DT.Rows(0)
                    RetStr.Append(" <table id=""divProduct"" style=""width: 250px;float: left;"" valign=""top"">")
                    RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' width='100px'> Part No : </td>")
                    RetStr.Append(" <td  align='left' nowrap='nowrap' width='150px'> " + .Item("PART_NO") + "</td>")
                    RetStr.Append("</tr>")
                    RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' >  Product Status :</td>")
                    RetStr.Append(" <td align='left'> " + .Item("PRODUCT_STATUS") + " </td> </tr>")
                    RetStr.Append("<tr> <td align='right' class='black'  nowrap='nowrap' >   ABCD Indicator :</td>")
                    RetStr.Append(" <td align='left'> " + .Item("ABC_INDICATOR") + " </td> </tr>")
                    RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' >  MOQ :</td>")
                    RetStr.Append(" <td align='left'>" + Decimal.Parse(.Item("MIN_ORDER_QTY")).ToString("f0") + " </td> </tr>")
                    If _ShipVia <> Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.NA Then
                        Dim _PostPone As String = String.Empty
                        Select Case _ShipVia
                            Case Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.Air
                                _PostPone = "(plus 2 weeks)"
                            Case Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.Sea
                                _PostPone = "(plus 6 weeks)"
                        End Select
                        RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' >  Loading Group :</td>")
                        RetStr.Append(" <td align='left'>" + _ShipVia.ToString + _PostPone + " </td> </tr>")
                    End If
                    RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' >   PLM Notice :</td>")
                    RetStr.Append(" <td align='left'> " + .Item("PLMNotice"))
                    If IsApplyingForPhaseOutStatus Then
                        'RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' >   EOL :</td>")
                        If Not String.IsNullOrEmpty(.Item("PLMNotice")) Then RetStr.Append("<br/>")
                        RetStr.Append("<b>This product is in PLM EOL Initial stage</b>")
                    End If
                    RetStr.Append(" </td> </tr>")




                    If Not String.IsNullOrEmpty(strMinPrice) Then
                        RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' >   Min. Price :</td>")
                        RetStr.Append(" <td align='left'> " + strMinPrice + " </td> </tr>")
                    End If
                    If _IsIntercon Then
                        RetStr.Append(" <tr> <td align='right' class='black'  nowrap='nowrap' >  Product Hierarchy :</td>")
                        RetStr.Append(" <td align='left'>" + .Item("PRODUCT_HIERARCHY") + " </td> </tr>")
                    End If

                    RetStr.Append(" </table>")

                End With
            End If
            RetStr.Append(GetACLATP(strPartNo, plant1, Advantech.Myadvantech.Business.PartBusinessLogic.ShippingVia.NA))
            RetStr.Append(GetACLATP(strPartNo, plant2, _ShipVia))

            'Frank 20180124
            If strOrgid.Equals("US01", StringComparison.InvariantCultureIgnoreCase) AndAlso Not String.IsNullOrEmpty(plant3) Then
                RetStr.Append(GetACLATP(strPartNo, plant3, _ShipVia))
            End If

            'Frank 20160330
            If strOrgid.Equals("KR01", StringComparison.InvariantCultureIgnoreCase) Then
                RetStr.Append(GetACLATP(strPartNo, plant3, _ShipVia))
            End If

            'Frank 20160330
            If strOrgid.Equals("JP01", StringComparison.InvariantCultureIgnoreCase) Then
                RetStr.Append(GetACLATP(strPartNo, plant3, _ShipVia))
                RetStr.Append(GetACLATP(strPartNo, plant4, _ShipVia))
                RetStr.Append(GetACLATP(strPartNo, plant5, _ShipVia))
            End If

            RetStr.Append("<div class=""clear""></div><style>.clear{ clear:both;}</style>")

            procObj.ProcessStatus = True : procObj.ProcessMessage = RetStr.ToString()

            'Return RetStr.ToString()

        Catch ex As Exception
            procObj.ProcessStatus = False : procObj.ProcessMessage = ex.ToString
            Dim userid As String = "Anonymous"
            Try
                userid = Pivot.CurrentProfile.UserId.ToString()
            Catch ex1 As Exception
            End Try
            Util.InsertMyErrLog(ex.ToString, userid)
        End Try

        Dim serializer = New Script.Serialization.JavaScriptSerializer()
        Return serializer.Serialize(procObj)

    End Function


    Public Class ProcessResult
        Private _procStatus As Boolean, _procMsg As String
        Public Property ProcessStatus As Boolean
            Get
                Return _procStatus
            End Get
            Set(value As Boolean)
                _procStatus = value
            End Set
        End Property
        Public Property ProcessMessage As String
            Get
                Return _procMsg
            End Get
            Set(value As String)
                _procMsg = value
            End Set
        End Property

    End Class


    Class PartInfo

        Private _strPN As String
        Public Property PartNo As String
            Get
                Return _strPN
            End Get
            Set(ByVal value As String)
                _strPN = value
            End Set
        End Property

        Private _PartStatus As String
        Public Property ProductStatus() As String
            Get
                Return _PartStatus
            End Get
            Set(ByVal value As String)
                _PartStatus = value
            End Set
        End Property

        Private _ABCDIndicator As String
        Public Property Indicator() As String
            Get
                Return _ABCDIndicator
            End Get
            Set(ByVal value As String)
                _ABCDIndicator = value
            End Set
        End Property

        Private _MOQ As Integer
        Public Property MOQ() As Integer
            Get
                Return _MOQ
            End Get
            Set(ByVal value As Integer)
                _MOQ = value
            End Set
        End Property

        Private _PLMNotice As String
        Public Property PLMNotice() As String
            Get
                Return _PLMNotice
            End Get
            Set(ByVal value As String)
                _PLMNotice = value
            End Set
        End Property

        Public Property MinPrice As String


        Private _IsApplyingForPhaseOutStatus As Boolean
        ''' <summary>
        ''' It means the product is applied for phase out status
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsApplyingForPhaseOutStatus() As Boolean
            Get
                Return _IsApplyingForPhaseOutStatus
            End Get
            Set(ByVal value As Boolean)
                _IsApplyingForPhaseOutStatus = value
            End Set
        End Property


    End Class

    Class ATPTotalInfo
        Private _strPN As String, _ATPRecords As List(Of ATPRecord)
        Public Property PartNo As String
            Get
                Return _strPN
            End Get
            Set(ByVal value As String)
                _strPN = value
            End Set
        End Property

        Public Property ATPRecords As List(Of ATPRecord)
            Get
                Return _ATPRecords
            End Get
            Set(ByVal value As List(Of ATPRecord))
                _ATPRecords = value
            End Set
        End Property

    End Class

    Class ATPRecord
        Private _intQty As Integer, _dtDate As String
        Public Property Qty As Integer
            Get
                Return _intQty
            End Get
            Set(ByVal value As Integer)
                _intQty = value
            End Set
        End Property
        Public Property AvailableDate As String
            Get
                Return _dtDate
            End Get
            Set(ByVal value As String)
                _dtDate = value
            End Set
        End Property
    End Class

End Class