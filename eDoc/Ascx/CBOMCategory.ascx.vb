Imports DBauer.Web.UI.WebControls
Public Class CBOMCategory
    Inherits System.Web.UI.UserControl

    Dim CBOMDALWS As New CBOMWS.MyCBOMDAL
    Public Event SelectedComponentChanged()

    Public ReadOnly Property CompPlaceHolder As DynamicControlsPlaceholder
        Get
            Return Me.ph1
        End Get
    End Property

    Public ReadOnly Property SelectedComponentIdx As Integer
        Get
            Return Me.dlComp.SelectedIndex
        End Get
    End Property

    Public ReadOnly Property ComponentCounts As Integer
        Get
            Return Me.dlComp.Items.Count
        End Get
    End Property

    Public Property IsSYSBOM As Boolean
        Get
            Return ViewState("IsSYSBOM")
        End Get
        Set(ByVal value As Boolean)
            ViewState("IsSYSBOM") = value
        End Set
    End Property

    Public Property CatName As String
        Set(ByVal value As String)
            ViewState("catid") = value
            lbCatName.Text = CatName
        End Set
        Get
            Return lbCatName.Text
        End Get
    End Property

    Public Property Level As Integer
        Get
            Return ViewState("clevel")
        End Get
        Set(ByVal value As Integer)
            ViewState("clevel") = value
        End Set
    End Property

    Public Property IsRequired As Boolean
        Set(ByVal value As Boolean)
            ViewState("IsReq") = value
        End Set
        Get
            Return ViewState("IsReq")
        End Get
    End Property

    Public Property Qty As Integer
        Set(ByVal value As Integer)
            ViewState("Qty") = value
        End Set
        Get
            Return ViewState("Qty")
        End Get
    End Property

    Public Property IsQuote As Boolean
        Set(ByVal value As Boolean)
            ViewState("IsQuote") = value
        End Set
        Get
            Return ViewState("IsQuote")
        End Get
    End Property

    <Serializable()> _
    Public Class BOMComponentProperties
        Public ATPDate As Date, UnitPrice As Decimal, ListPrice As Decimal, ITP As Decimal
        Public CompDesc As String
        Public ProductStatus As String, RoHSFlag As Boolean
        Public Currency As String, CurrencySign As String
        Sub New(ByVal iProductStatus As String, ByVal iCurrency As String, ByVal iCurrencySign As String, ByVal iRoHS As Boolean, ByVal Desc As String)
            Me.ProductStatus = iProductStatus : Me.Currency = iCurrency : Me.RoHSFlag = iRoHS
            UnitPrice = -1 : ListPrice = -1 : ITP = -1 : ATPDate = Date.MinValue : Me.CompDesc = Desc
        End Sub
    End Class

    Public Function GetPrice(ByVal PartNo As String, ByVal oType As COMM.Fixer.eDocType) As Decimal

        Dim DT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), oType)
        Dim _SAPDAL As New SAPDAL.SAPDAL
        If Not IsNothing(DT) Then
            'If PartNo = "No Need" Then Return 0
            If PartNo.ToLower = System.MyExtension.BuildIn.ToLower Then Return 0
            If PartNo.ToUpper.StartsWith("AGS-EW-") Then
                Return Business.getRateByEWItem(PartNo, DT.DocReg) * 100
            End If
            Dim quote_to_company As String = DT.AccErpId

            Dim unit_price As Decimal = 0
            If quote_to_company = "" Then
                Dim dtprice As New DataTable
                SAPTools.getEpricerPrice(PartNo, "", "", DT.AccOfficeName, "", "", DT.currency, DT.org, dtprice)
                If dtprice.Rows.Count > 0 Then
                    unit_price = dtprice.Rows(0).Item("UNIT_PRICE")
                Else
                    unit_price = 0
                End If
            Else
                Dim dtPrice As New DataTable
                Dim org As String = DT.org
                'If DT.Rows(0).Item("quoteToErpId") <> "" Then
                '    org = Business.getOrgByErpId(DT.Rows(0).Item("quoteToErpId"))
                'End If
                'SAPTools.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", dtPrice)
                _SAPDAL.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", "", dtPrice)
                If dtPrice.Rows.Count > 0 Then
                    unit_price = FormatNumber(dtPrice.Rows(0).Item("Netwr"), 2).Replace(",", "")
                End If
            End If
            If unit_price = 0 Then Return 0
            Return unit_price
        End If
        Return 0
    End Function

    Public Function GetATP(ByVal PartNo As String, ByVal ReqQty As Integer) As Date
        'If PartNo = "No Need" Then Return Now.ToString("yyyy/MM/dd")
        If PartNo.ToLower = System.MyExtension.BuildIn.ToLower Then Return Now.ToString("yyyy/MM/dd")
        If PartNo.ToUpper.StartsWith("AGS-EW-") Then
            Return Now.ToString("yyyy/MM/dd")
        End If
        Dim due_date As String = Now.ToString("yyyy/MM/dd")


        Dim DT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
        Dim org As String = DT.org
        'If DT.Rows(0).Item("quoteToErpId") <> "" Then
        '    org = Business.getOrgByErpId(DT.Rows(0).Item("quoteToErpId"))
        'End If
        SAPTools.getInventoryAndATPTable(PartNo, Business.getPlantByOrgID(org), ReqQty, due_date, 0, Nothing, "", 1, 0)
        'Util.GetDueDate(PartNo, ReqQty, Now.ToString("yyyy/MM/dd"), due_date)
        Return CDate(due_date).ToString("MM/dd/yyyy")
    End Function

    Public Property ListItems As List(Of BOMComponentProperties)
        Set(ByVal value As List(Of BOMComponentProperties))
            ViewState("ListItems") = value
        End Set
        Get
            Return ViewState("ListItems")
        End Get
    End Property

    Public ReadOnly Property lbCategoryClientID As String
        Get
            Return lbCatName.ClientID
        End Get
    End Property

    Public Function GetSelectedItems(ByRef NotSelectedCtrl As CBOMCategory) As DataTable
        Dim dt As DataTable = Business.GetConfigOrderCartDt()
        If Not dt.Columns.Contains("Level") Then dt.Columns.Add("Level", GetType(Integer))
        If Not dt.Columns.Contains("ATP_DATE") Then dt.Columns.Add("ATP_DATE", GetType(Date))
        If dlComp.SelectedIndex > 0 Then
            Dim catRow As DataRow = dt.NewRow(), compRow As DataRow = dt.NewRow()
            catRow.Item("category_id") = lbCatName.Text : catRow.Item("CATEGORY_TYPE") = "Category" : catRow.Item("Level") = Me.Level
            catRow.Item("category_name") = lbCatName.Text : catRow.Item("category_qty") = Request("QTY")
            compRow.Item("category_id") = dlComp.SelectedValue : compRow.Item("CATEGORY_TYPE") = "Component"
            compRow.Item("category_name") = dlComp.SelectedValue : compRow.Item("category_qty") = Request("QTY")
            compRow.Item("PARENT_CATEGORY_ID") = lbCatName.Text : compRow.Item("Level") = Me.Level
            If Me.ListItems(dlComp.SelectedIndex - 1).UnitPrice >= 0 Then compRow.Item("category_price") = Me.ListItems(dlComp.SelectedIndex - 1).UnitPrice
            If Me.ListItems(dlComp.SelectedIndex - 1).ATPDate > Date.MinValue Then compRow.Item("ATP_DATE") = Me.ListItems(dlComp.SelectedIndex - 1).ATPDate
            dt.Rows.Add(catRow) : dt.Rows.Add(compRow)
            For Each c As Control In ph1.Controls
                If TypeOf (c) Is CBOMCategory Then
                    Dim subCtrl As CBOMCategory = CType(c, CBOMCategory)
                    Dim subDt As DataTable = subCtrl.GetSelectedItems(NotSelectedCtrl)
                    For Each r As DataRow In subDt.Rows
                        If r.Item("category_type") = "Category" AndAlso r.Item("Level") = Me.Level + 1 Then
                            r.Item("PARENT_CATEGORY_ID") = dlComp.SelectedValue
                        End If
                    Next
                    dt.Merge(subDt)
                End If
            Next
        End If
        If Me.IsRequired AndAlso dlComp.SelectedIndex <= 0 AndAlso dlComp.Items.Count > 1 Then
            NotSelectedCtrl = Me
        End If
        Return dt
    End Function

    Protected Sub dlComp_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ph1.Controls.Clear()
        Dim dl As RadioButtonList = sender
        If dl.SelectedIndex > 0 Then
            Dim selItem As ListItem = dl.SelectedItem
            If Not Me.IsSYSBOM Then


                Dim DTqm As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
                Dim org As String = DTqm.org
                'If DTqm.Rows(0).Item("quoteToErpId") <> "" Then
                '    org = Business.getOrgByErpId(DTqm.Rows(0).Item("quoteToErpId"))
                'End If
                '
                Dim CbomDt As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYDataTable = CBOMDALWS.GetCBOM2(Replace(dl.SelectedValue, "'", "''"),
                                                                                                DTqm.siebelRBU, org, "")
                'Dim dt As DataTable = Business.GetQBOMSql(Replace(dl.SelectedValue, "'", "''"), Request("UID"))
                For Each CbomRow As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYRow In CbomDt.Rows
                    Dim ctrl As New CBOMCategory
                    AddHandler ctrl.SelectedComponentChanged, AddressOf CompSelected
                    ph1.Controls.Add(ctrl)
                    ctrl.CatName = CbomRow.CATEGORY_ID : ctrl.Level = Me.Level + 1
                    If CbomRow.CONFIGURATION_RULE.ToUpper() = "REQUIRED" Then ctrl.IsRequired = True
                    ctrl.SetCompList()
                Next
            End If
            If Me.ListItems(dl.SelectedIndex - 1).UnitPrice < 0 Then
                Dim bi As BOMComponentProperties = Me.ListItems.Item(dl.SelectedIndex - 1)
                bi.UnitPrice = GetPrice(dl.SelectedValue, COMM.Fixer.eDocType.EQ)
            End If
            If Me.ListItems(dl.SelectedIndex - 1).ATPDate <= Date.MinValue Then
                Dim bi As BOMComponentProperties = Me.ListItems.Item(dl.SelectedIndex - 1)
                bi.ATPDate = GetATP(dl.SelectedValue, Request("QTY"))
            End If
            'selItem.Text = selItem.Text + " "
            If Me.ListItems(dl.SelectedIndex - 1).UnitPrice >= 0 Then
                If selItem.Value.ToUpper.StartsWith("AGS-EW-") Then
                    selItem.Text = selItem.Value + " -- " + Me.ListItems(dl.SelectedIndex - 1).CompDesc + "   <b>Price:</b>" + Me.ListItems(dl.SelectedIndex - 1).UnitPrice.ToString() + "% of selling price"
                ElseIf Me.ListItems(dl.SelectedIndex - 1).UnitPrice > 0 Then
                    selItem.Text = selItem.Value + " -- " + Me.ListItems(dl.SelectedIndex - 1).CompDesc + "   <b>Price:</b>" + Me.ListItems(dl.SelectedIndex - 1).UnitPrice.ToString()
                Else
                    selItem.Text = selItem.Value + " -- " + Me.ListItems(dl.SelectedIndex - 1).CompDesc + "   <b>Price:</b>" + "TBD"
                End If
                If Year(Me.ListItems(dl.SelectedIndex - 1).ATPDate) >= Year(Now) Then
                    selItem.Text = selItem.Text + "   <b>Available on:</b>" + Me.ListItems(dl.SelectedIndex - 1).ATPDate.ToString("yyyy/MM/dd")
                Else
                    selItem.Text = selItem.Text + "   <b>Available on:</b>" + "TBD"
                End If
            End If
        End If
        RaiseEvent SelectedComponentChanged()
    End Sub

    Protected Sub CompSelected()
        RaiseEvent SelectedComponentChanged()
    End Sub

    Public Sub SetCompList()
        If ViewState("catid") IsNot Nothing Then
            Dim BOMCompList As New List(Of BOMComponentProperties)
            Dim defaultCompIdx As Integer = 0


            Dim DTM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
            Dim Org As String = DTM.org

            'If DTM.Rows(0).Item("quoteToErpId") <> "" Then
            '    Org = Business.getOrgByErpId(DTM.Rows(0).Item("quoteToErpId"))
            'End If
            Dim CbomDt As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYDataTable = CBOMDALWS.GetCBOM2(ViewState("catid"), DTM.siebelRBU, Org, "")
            'Dim dt As DataTable = Business.GetQBOMSql(Replace(ViewState("catid"), "'", "''"), Request("UID"))
            For i As Integer = 0 To CbomDt.Rows.Count - 1
                Dim CbomRow As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYRow = CbomDt.Rows(i)
                If CbomRow.CONFIGURATION_RULE.ToUpper() = "DEFAULT" And CbomRow.CATEGORY_TYPE.ToUpper() = "COMPONENT" Then
                    defaultCompIdx = i + 1
                End If
                Dim itemText As String = String.Format("{0} -- {1}", "<a target=""_blank"" href=""atpAndPrice.aspx?ID=" & _
                                                       CbomRow.CATEGORY_ID & "&ORG=" & Org & "&COMPANY=" & DTM.AccErpId & """>" & _
                                                       CbomRow.CATEGORY_ID & "</a>", CbomRow.CATEGORY_DESC)
                If CbomRow.RoHS = "y" Then itemText += " <img src='/Images/rohs.jpg' alt='RoHS'>"
                If Business.IsHotSelling(CbomRow.CATEGORY_ID, Org) Then
                    itemText += " <img src='/Images/Hot-Orange.gif' alt='Hot!'/> "
                End If
                If Business.IsFastDelivery(CbomRow.CATEGORY_ID, Org) Then
                    itemText += " <img src='/Images/Fast Delivery.gif' alt='Fast Delivery'/> "
                End If
                dlComp.Items.Add(New ListItem(itemText, CbomRow.CATEGORY_ID))
                Dim BOMComp As New BOMComponentProperties( _
                   CbomRow.STATUS, DTM.currency, Util.getCurrSign(DTM.currency), _
                    IIf(CbomRow.RoHS = "y", True, False), CbomRow.CATEGORY_DESC)
                'BOMComp.CompName = r.Item("category_name")
                BOMCompList.Add(BOMComp)
            Next
            dlComp.SelectedIndex = defaultCompIdx
            lbCatName.Text = ViewState("catid")
            Me.ListItems = BOMCompList
            If IsRequired Then
                lbReq.Visible = True : ExpandAll()
            Else
                CollapseAll()
            End If
            If CbomDt.Rows.Count = 0 Then Me.Visible = False
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack AndAlso ViewState("catid") IsNot Nothing Then
            lbCatName.Text = ViewState("catid")
            SetCompList()
            If dlComp.SelectedIndex > 0 Then TimerHandleDefaultSelect.Enabled = True
        End If
    End Sub

    Public Sub ExpandAll()
        btnShowHide.Text = "-" : tb_CompList.Visible = True
        For Each ctrl As CBOMCategory In ph1.Controls
            ctrl.ExpandAll()
        Next
    End Sub

    Public Sub CollapseAll()
        btnShowHide.Text = "+" : tb_CompList.Visible = False
        For Each ctrl As CBOMCategory In ph1.Controls
            ctrl.CollapseAll()
        Next
    End Sub

    Protected Sub btnShowHide_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If tb_CompList.Visible Then
            btnShowHide.Text = "+" : tb_CompList.Visible = False
        Else
            btnShowHide.Text = "-" : tb_CompList.Visible = True
        End If
    End Sub

    Protected Sub ph1_PostRestore(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each ctrl As Control In ph1.Controls
            If TypeOf (ctrl) Is CBOMCategory Then
                Dim c As CBOMCategory = ctrl
                RemoveHandler c.SelectedComponentChanged, AddressOf CompSelected
                AddHandler c.SelectedComponentChanged, AddressOf CompSelected
            End If
        Next
    End Sub

    Protected Sub TimerHandleDefaultSelect_Tick(ByVal sender As Object, ByVal e As System.EventArgs)
        TimerHandleDefaultSelect.Interval = 99999
        dlComp_SelectedIndexChanged(Me.dlComp, New EventArgs)
        TimerHandleDefaultSelect.Enabled = False
    End Sub

End Class