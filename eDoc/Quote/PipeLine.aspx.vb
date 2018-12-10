Public Class PipeLine
    Inherits System.Web.UI.Page

    Dim _QID As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not String.IsNullOrEmpty(Request("QuoteID")) Then
            _QID = Request("QuoteID")
        End If

        If Not Page.IsPostBack Then

            'Both pipeline and un-pipelined quote's required settings.
            Dim QM As Advantech.Myadvantech.DataAccess.QuotationMaster = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMaster(_QID)
            Dim QD As List(Of Advantech.Myadvantech.DataAccess.QuotationDetail) = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationDetail(_QID)
            Me.GridView1.DataSource = QD
            Me.GridView1.DataBind()

            Dim _DT As DataTable = tbOPBase.dbGetDataTable("MY", "SELECT URL FROM SIEBEL_ACCOUNT WHERE ROW_ID='" & QM.quoteToRowId & "'")
            DropDownList_ShipYear.Items.Add(New ListItem(Date.Now.Year, "1"))
            DropDownList_ShipYear.Items.Add(New ListItem(Date.Now.Year + 1, "2"))


            'Ryan 20170228 Items in Industry and SalesGroup DropDownList should be set from Database
            Dim _IndustryDT As DataTable = SiebelTools.GET_Standard_BAA_List()
            If Not _IndustryDT Is Nothing AndAlso _IndustryDT.Rows.Count > 0 Then
                For Each dr As DataRow In _IndustryDT.Rows
                    DropDownList_Industry.Items.Add(New ListItem(dr.Item("Text").ToString.Trim, dr.Item("Value").ToString.Trim))
                Next
            End If
            DropDownList_Industry.Items.Add(New ListItem("Others", "Others"))

            Dim _SalesgroupDT As DataTable = tbOPBase.dbGetDataTable("MY", "SELECT DISTINCT SALES_GROUP FROM eQuotation.dbo.SALES_ROLE_MAPPING ORDER BY SALES_GROUP")
            If Not _SalesgroupDT Is Nothing AndAlso _SalesgroupDT.Rows.Count > 0 Then
                For Each dr As DataRow In _SalesgroupDT.Rows
                    DropDownList_SalesGroup.Items.Add(New ListItem(dr.Item("SALES_GROUP").ToString.Trim, dr.Item("SALES_GROUP").ToString.Trim))
                Next
            End If

            'end

            'Get Quote Amount
            Dim total As Decimal = 0
            For Each d As Advantech.Myadvantech.DataAccess.QuotationDetail In QD
                total = total + (d.qty * d.newUnitPrice)
            Next
            TotalAmount.Text = "Total: $" + total.ToString


            'Check if quote has been pipelined, if so then set data from pipeline db.
            Dim pipeline_dt As DataTable = tbOPBase.dbGetDataTable("Pipeline", "SELECT * FROM pipelineReport where IsActive = '1' and QuoteId = '" + _QID + "'")
            If pipeline_dt.Rows.Count > 0 AndAlso pipeline_dt IsNot Nothing Then
                Me.TextBox_SalesPerson.Text = pipeline_dt.Rows(0).Item("Sales")
                Me.TextBox_District.Text = pipeline_dt.Rows(0).Item("District")
                Me.TextBox_AccountName.Text = pipeline_dt.Rows(0).Item("Company")
                Me.TextBox_AccoutURL.Text = pipeline_dt.Rows(0).Item("Website")
                Me.TextBox_Application.Text = pipeline_dt.Rows(0).Item("App")

                If Not Me.DropDownList_SalesGroup.Items.FindByText(pipeline_dt.Rows(0).Item("Group").ToString.Trim) Is Nothing Then
                    Me.DropDownList_SalesGroup.Items.FindByText(pipeline_dt.Rows(0).Item("Group").ToString.Trim).Selected = True
                End If

                Me.DropDownList_SalesStage.Items.FindByValue(pipeline_dt.Rows(0).Item("Probability")).Selected = True
                Me.DropDownList_CustomerType.Items.FindByText(pipeline_dt.Rows(0).Item("Type")).Selected = True

                If Not Me.DropDownList_Industry.Items.FindByText(pipeline_dt.Rows(0).Item("Industry").ToString.Trim) Is Nothing Then
                    Me.DropDownList_Industry.Items.FindByText(pipeline_dt.Rows(0).Item("Industry").ToString.Trim).Selected = True
                End If

                '------------Ship Year & Month Setting------------
                If Me.DropDownList_ShipMonth.Items.FindByText(pipeline_dt.Rows(0).Item("ShipDate")) IsNot Nothing Then
                    Me.DropDownList_ShipMonth.Items.FindByText(pipeline_dt.Rows(0).Item("ShipDate")).Selected = True
                End If

                If Me.DropDownList_ShipYear.Items.FindByText(pipeline_dt.Rows(0).Item("Year")) IsNot Nothing Then
                    Me.DropDownList_ShipYear.Items.FindByText(pipeline_dt.Rows(0).Item("Year")).Selected = True
                Else
                    Me.DropDownList_ShipYear.Items.FindByValue("-1").Selected = True
                End If
                '------------------End------------------

                Me.TextBox_Notes.Text = pipeline_dt.Rows(0).Item("Notes")

                Dim totalRevenue As Decimal = 0
                For Each gvr As GridViewRow In GridView1.Rows
                    If pipeline_dt.Select("Product = '" + gvr.Cells(2).Text + "'").Length > 0 Then
                        CType(gvr.FindControl("CheckBox1"), CheckBox).Checked = True

                        'Get EAU from this year or next year
                        If Not Integer.Parse(pipeline_dt.Rows(0).Item("EAU")) = 0 AndAlso Integer.Parse(pipeline_dt.Rows(0).Item("EAUNextYear")) = 0 Then
                            CType(gvr.FindControl("txtBox_eau"), TextBox).Text = pipeline_dt.Select("Product = '" + gvr.Cells(2).Text + "'")(0).Item("EAU").ToString
                            totalRevenue = totalRevenue + Convert.ToDecimal(pipeline_dt.Select("Product = '" + gvr.Cells(2).Text + "'")(0).Item("Total").ToString)
                        ElseIf Integer.Parse(pipeline_dt.Rows(0).Item("EAU")) = 0 AndAlso Not Integer.Parse(pipeline_dt.Rows(0).Item("EAUNextYear")) = 0 Then
                            CType(gvr.FindControl("txtBox_eau"), TextBox).Text = pipeline_dt.Select("Product = '" + gvr.Cells(2).Text + "'")(0).Item("EAUNextYear").ToString
                            totalRevenue = totalRevenue + Convert.ToDecimal(pipeline_dt.Select("Product = '" + gvr.Cells(2).Text + "'")(0).Item("TotalNextYear").ToString)
                        End If

                    End If
                Next
                'Me.lb_TotalRevenue.Text = "$ " + totalRevenue.ToString
                Pipeline_Button.Text = "Update Pipeline"
            Else
                Dim _URL As String = String.Empty
                If _DT IsNot Nothing AndAlso _DT.Rows.Count > 0 Then
                    _URL = _DT.Rows(0).Item("URL").ToString
                End If

                Dim _district As String = QM.DISTRICT
                Dim _SalesRepresentativeEmail As String = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetSalesEmailByQuoteID(_QID)

                If Not String.IsNullOrEmpty(_SalesRepresentativeEmail) Then
                    Dim objSalesName As Object = tbOPBase.dbExecuteScalar("Pipeline", String.Format("select top 1 name from SALES_ROLE_MAPPING where sales_email = '{0}'", _SalesRepresentativeEmail))
                    If objSalesName IsNot Nothing AndAlso Not String.IsNullOrEmpty(objSalesName) Then
                        Me.TextBox_SalesPerson.Text = objSalesName.ToString
                    Else
                        If _SalesRepresentativeEmail.Split("@")(0).Contains(".") Then
                            Me.TextBox_SalesPerson.Text = _SalesRepresentativeEmail.Split("@")(0).Split(".")(0) + " " + _SalesRepresentativeEmail.Split("@")(0).Split(".")(1)
                        Else
                            Me.TextBox_SalesPerson.Text = _SalesRepresentativeEmail.Split("@")(0)
                        End If
                    End If
                End If

                '---------------Get sales group old logic, no longer available----------------------
                'Dim _SalesGroupCode As String = String.Empty
                '_SalesGroupCode = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetSalesOfficeCodeBySalesCode(_SalesCode)
                'Dim sql_str As String = "select a.SALES_CODE,a.FULL_NAME ,b.id_sbu as groupname " & _
                '                        " from SAP_EMPLOYEE a left join EAI_IDMAP b " & _
                '                        " on a.SALES_CODE = b.id_sap where a.SALESOFFICE = '" + _SalesGroupCode + "'" & _
                '                        " and a.SALES_CODE = '" + _SalesCode + "'"
                'Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", sql_str)
                'Dim _SalesGroupName As String = IIf(dt IsNot Nothing AndAlso dt.Rows.Count > 0, dt.Rows(0).Item("groupname").ToString, "")
                'Me.TextBox_SalesGroup.Text = _SalesGroupName
                '------------------------------End-------------------------------------

                'Get Sales Group From Nada's database.
                Dim SalesEmail2GroupMapping As DataTable = tbOPBase.dbGetDataTable("MY", "SELECT distinct isnull(SALES_GROUP,'') as SALES_GROUP FROM eQuotation.dbo.SALES_ROLE_MAPPING where SALES_EMAIL = '" + _SalesRepresentativeEmail + "' order by SALES_GROUP")
                If SalesEmail2GroupMapping IsNot Nothing AndAlso SalesEmail2GroupMapping.Rows.Count > 0 _
                    AndAlso Not Me.DropDownList_SalesGroup.Items.FindByText(SalesEmail2GroupMapping.Rows(0).Item("SALES_GROUP").ToString.Trim) Is Nothing Then
                    Me.DropDownList_SalesGroup.Items.FindByText(SalesEmail2GroupMapping.Rows(0).Item("SALES_GROUP").ToString.Trim).Selected = True
                Else
                    Me.DropDownList_SalesGroup.Items.FindByValue("-1").Selected = True
                End If

                Me.TextBox_District.Text = _district
                Me.TextBox_AccountName.Text = QM.quoteToName
                Me.TextBox_AccoutURL.Text = IIf(_URL.Equals("."), "", _URL)
                'Me.lb_TotalRevenue.Text = "$ " + total.ToString
            End If

        End If

    End Sub



    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim _item As Advantech.Myadvantech.DataAccess.QuotationDetail = CType(e.Row.DataItem, Advantech.Myadvantech.DataAccess.QuotationDetail)
            e.Row.Cells(6).Text = Convert.ToDecimal(_item.qty * _item.newUnitPrice)
            Dim _txtbox As TextBox = CType(e.Row.FindControl("txtBox_eau"), TextBox)
            Dim _cbxSelect As CheckBox = CType(e.Row.FindControl("CheckBox1"), CheckBox)
            If _item.ItemTypeX = Advantech.Myadvantech.DataAccess.QuoteItemType.BtosPart Then

                '_txtbox.Enabled = False
                _txtbox.ReadOnly = True
            End If

            _cbxSelect.Checked = getIsSelected(_item.line_No)
            Dim eau As Integer = getEAU(_item.line_No)
            If eau > 0 Then
                _txtbox.Text = eau
            End If
            Dim cbxMultiShip As CheckBox = CType(e.Row.FindControl("cbxMultiShip"), CheckBox)
            Dim imgMultiShipEdit As ImageButton = CType(e.Row.FindControl("ibtnEdit"), ImageButton)
            If _item.line_No > 100 AndAlso _item.line_No Mod 100 <> 0 Then
                'cbxMultiShip.Visible = False
                'imgMultiShipEdit.Visible = False
            End If
            Dim eautotal As Integer = checkMultiship(_item.line_No)
            If eautotal > 0 Then
                _txtbox.Text = eautotal
                cbxMultiShip.Checked = True
            Else
                cbxMultiShip.Checked = False
            End If



        End If
    End Sub

    Protected Sub Pipeline_Button_Click(sender As Object, e As EventArgs)

        Dim pipeline As New Advantech.Myadvantech.DataAccess.Pipeline.general()
        Dim pipeline_report_list As New List(Of Advantech.Myadvantech.DataAccess.Pipeline.pipelineReport)
        Dim QM As Advantech.Myadvantech.DataAccess.QuotationMaster = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMaster(_QID)
        Dim SendBtosRevenue As Boolean = True

        For Each row As GridViewRow In GridView1.Rows
            If (CType(row.FindControl("CheckBox1"), CheckBox)).Checked = True Then

                Dim _txtboxEAU As TextBox = CType(row.FindControl("txtBox_eau"), TextBox)

                Dim pipeline_report As New Advantech.Myadvantech.DataAccess.Pipeline.pipelineReport
                pipeline_report.UID = _QID + "_" + row.Cells(1).Text
                pipeline_report.QuoteId = _QID
                pipeline_report.QuoteNo = QM.quoteNo + " V" + QM.Revision_Number.ToString
                pipeline_report.Group = Me.DropDownList_SalesGroup.SelectedItem.Text

                'Ryan 20170119 pipeline report year will be DropDownList_ShipYear selected value
                pipeline_report.Year = Me.DropDownList_ShipYear.SelectedItem.Text

                pipeline_report.Month = Date.Now.Month
                pipeline_report.Sales = Me.TextBox_SalesPerson.Text
                pipeline_report.District = Me.TextBox_District.Text
                pipeline_report.Company = Me.TextBox_AccountName.Text
                'pipeline_report.KA = ""
                pipeline_report.WebSite = Me.TextBox_AccoutURL.Text
                pipeline_report.Industry = Me.DropDownList_Industry.SelectedItem.Text
                pipeline_report.Type = Me.DropDownList_CustomerType.SelectedItem.Text
                pipeline_report.Probability = Me.DropDownList_SalesStage.SelectedValue

                pipeline_report.Product = row.Cells(2).Text

                pipeline_report.v = IIf(Date.Now.Day <= 15, 1, 2)

                pipeline_report.App = Me.TextBox_Application.Text
                pipeline_report.Notes = Me.TextBox_Notes.Text

                'Validate is Btos or not, if so, get Btos price
                Dim isBtos As Boolean = False
                isBtos = Convert.ToInt32(row.Cells(1).Text) >= 100
                pipeline_report.EAUNextYear = Convert.ToInt32(row.Cells(1).Text)
                pipeline_report.PriceNextYear = Convert.ToInt32(row.Cells(3).Text)
                Dim Btos_Amount As Decimal = 0
                Dim Btos_EAU As Decimal = 0
                If isBtos Then
                    Dim btosparent_list As List(Of QuoteItem) = MyQuoteX.GetQuoteBTOSParentItems(_QID)
                    For Each q As QuoteItem In btosparent_list
                        If Convert.ToInt32(Convert.ToInt32(row.Cells(1).Text) / 100) = (q.line_No / 100) Then
                            Btos_Amount = q.UnitPriceWithWarrantX
                            For Each r As GridViewRow In GridView1.Rows
                                If q.line_No = r.Cells(1).Text Then
                                    Btos_EAU = CType(r.FindControl("txtBox_eau"), TextBox).Text
                                    If String.IsNullOrEmpty(Btos_EAU) Then
                                        Btos_EAU = r.Cells(3).Text
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If


                'Ryan 20170119 Comment below out due to "next year" is no longer available.
                '=========Set data by selected ship year=========
                'If Me.DropDownList_ShipYear.SelectedItem.Text.Equals((Date.Now.Year + 1).ToString()) Then
                '    pipeline_report.EAUNextYear = Integer.Parse(IIf(String.IsNullOrEmpty(_txtboxEAU.Text), row.Cells(3).Text, _txtboxEAU.Text))
                '    pipeline_report.PriceNextYear = row.Cells(5).Text
                '    pipeline_report.sub_TotalNextYear = Convert.ToDecimal(row.Cells(3).Text) * pipeline_report.PriceNextYear
                '    pipeline_report.ShipDateNextYear = Me.DropDownList_ShipMonth.SelectedItem.Text

                '    If Not isBtos Then
                '        pipeline_report.TotalNextYear = Convert.ToDecimal(pipeline_report.EAUNextYear * pipeline_report.PriceNextYear)
                '    Else
                '        pipeline_report.EAUNextYear = Btos_EAU
                '        pipeline_report.TotalNextYear = IIf(SendBtosRevenue, Convert.ToDecimal(Btos_EAU * Btos_Amount), Convert.ToDecimal(-1))
                '    End If

                '    'If selected is next year, then set this year variables to zero
                '    pipeline_report.EAU = 0
                '    pipeline_report.Price = 0
                '    pipeline_report.Sub_Total = 0
                '    pipeline_report.Total = 0
                '    pipeline_report.ShipDate = Me.DropDownList_ShipMonth.Items.FindByText("Jan").Text 'Nada說先暫時填一月份
                'Else
                '    pipeline_report.EAU = Integer.Parse(IIf(String.IsNullOrEmpty(_txtboxEAU.Text), row.Cells(3).Text, _txtboxEAU.Text))
                '    pipeline_report.Price = row.Cells(5).Text
                '    pipeline_report.Sub_Total = Convert.ToDecimal(row.Cells(3).Text) * pipeline_report.Price
                '    pipeline_report.ShipDate = Me.DropDownList_ShipMonth.SelectedItem.Text

                '    If Not isBtos Then
                '        pipeline_report.Total = Convert.ToDecimal(pipeline_report.EAU * pipeline_report.Price)
                '    Else
                '        pipeline_report.EAU = Btos_EAU
                '        pipeline_report.Total = IIf(SendBtosRevenue, Convert.ToDecimal(Btos_EAU * Btos_Amount), Convert.ToDecimal(-1))
                '    End If
                'End If
                '==================End==================
                'End Ryan20170119 Comment out


                'Ryan 20170119 New rule, deprecate "next year"
                pipeline_report.EAU = Integer.Parse(IIf(String.IsNullOrEmpty(_txtboxEAU.Text), row.Cells(3).Text, _txtboxEAU.Text))
                pipeline_report.Price = row.Cells(5).Text
                pipeline_report.Sub_Total = Convert.ToDecimal(row.Cells(3).Text) * pipeline_report.Price
                pipeline_report.ShipDate = Me.DropDownList_ShipMonth.SelectedItem.Text
                If Not isBtos Then
                    pipeline_report.Total = Convert.ToDecimal(pipeline_report.EAU * pipeline_report.Price)
                Else
                    pipeline_report.EAU = Btos_EAU
                    If pipeline_report.EAUNextYear Mod 100 > 0 AndAlso pipeline_report.EAUNextYear > 100 Then
                        pipeline_report.Total = -1
                    Else
                        pipeline_report.Total = Convert.ToDecimal(Btos_EAU * Btos_Amount)
                    End If
                    'pipeline_report.Total = IIf(SendBtosRevenue, Convert.ToDecimal(Btos_EAU * Btos_Amount), Convert.ToDecimal(-1))
                End If

                'If isBtos Then SendBtosRevenue = False

                pipeline_report_list.Add(pipeline_report)
            End If
        Next

        If pipeline_report_list.Count > 0 Then
            SplitMultiShip(pipeline_report_list)
            Dim errmsg As String = String.Empty
            Dim result As Boolean = pipeline.CreatePIPELine(pipeline_report_list.ToArray, errmsg)

            If result Then
                PipelineResult.Text = "Pipeline Successfully Uploaded."

                'Ryan 20170223 All pipeline data is replicated from Nada's db, Pipeline_IsQuoteUploaded does not have to update anymore
                'tbOPBase.dbExecuteNoQuery2("EQ", "delete from Pipeline_IsQuoteUploaded where quoteid = '" + _QID + "'")
                'Dim sb As New StringBuilder
                'sb.AppendFormat("insert into Pipeline_IsQuoteUploaded values (@quoteid, @datetime)")
                'Dim quoteid As New System.Data.SqlClient.SqlParameter("quoteid", SqlDbType.NVarChar) : quoteid.Value = _QID
                'Dim datetime As New System.Data.SqlClient.SqlParameter("datetime", SqlDbType.DateTime) : datetime.Value = Now.ToString
                'Dim para() As System.Data.SqlClient.SqlParameter = {quoteid, datetime}
                'tbOPBase.dbExecuteNoQuery2("EQ", sb.ToString, para)
            Else
                PipelineResult.Text = "Pipeline Failed. Reason: " + errmsg
            End If
        Else
            PipelineResult.Text = "Please select at least one product."
        End If
    End Sub
    Public Function getMonth(ByVal num As Integer) As String

        Dim t() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"}
        Return t(num - 1)
    End Function
    Sub SplitMultiShip(ByVal l As List(Of Advantech.Myadvantech.DataAccess.Pipeline.pipelineReport))
        If Not IsNothing(ViewState("MultiShip")) Then
            Dim tempL As List(Of MultiShip) = CType(ViewState("MultiShip"), List(Of MultiShip))
            Dim p As Advantech.Myadvantech.DataAccess.Pipeline.pipelineReport = Nothing
            If tempL.Count > 0 Then
                For Each x As MultiShip In tempL
                    Dim ilp As IEnumerable(Of Advantech.Myadvantech.DataAccess.Pipeline.pipelineReport) = l.Where(Function(q) Not IsNothing(q.EAUNextYear) AndAlso q.EAUNextYear = x.LineNo)
                    Dim lp As List(Of Advantech.Myadvantech.DataAccess.Pipeline.pipelineReport) = Nothing
                    If Not IsNothing(ilp) Then
                        lp = ilp.ToList()
                    End If
                    If Not IsNothing(lp) AndAlso lp.Count > 0 Then
                        p = lp.FirstOrDefault
                        l.Remove(p)
                    End If

                    Dim pipeline_report As New Advantech.Myadvantech.DataAccess.Pipeline.pipelineReport
                    pipeline_report.UID = System.Guid.NewGuid.ToString
                    pipeline_report.QuoteId = p.QuoteId
                    pipeline_report.QuoteNo = p.QuoteNo
                    pipeline_report.Group = p.Group

                    'Ryan 20170119 pipeline report year will be DropDownList_ShipYear selected value
                    pipeline_report.Year = p.Year

                    pipeline_report.Month = p.Month
                    pipeline_report.Sales = p.Sales
                    pipeline_report.District = p.District
                    pipeline_report.Company = p.Company
                    'pipeline_report.KA = ""
                    pipeline_report.WebSite = p.WebSite
                    pipeline_report.Industry = p.Industry
                    pipeline_report.Type = p.Type
                    pipeline_report.Probability = p.Probability

                    pipeline_report.Product = p.Product

                    pipeline_report.v = p.v

                    pipeline_report.App = p.App
                    pipeline_report.Notes = p.Notes
                    pipeline_report.Sub_Total = p.Sub_Total

                    If p.Total > 0 AndAlso p.Total > 0 Then
                        pipeline_report.Price = p.Total / p.EAU
                    Else
                        pipeline_report.Price = 0
                    End If


                    pipeline_report.ShipDate = getMonth(x.Month)
                    pipeline_report.EAU = x.EAU
                    If p.Total = -1 Then
                        pipeline_report.Total = -1
                    Else
                        pipeline_report.Total = pipeline_report.EAU * pipeline_report.Price

                    End If

                    l.Add(pipeline_report)

                Next

            End If
        End If
    End Sub

    <Serializable()>
    Public Class MultiShip
        Private _LineNo As String = ""
        Public Property LineNo As String
            Get
                Return _LineNo
            End Get
            Set(value As String)
                _LineNo = value
            End Set
        End Property
        Private _Month As String = ""
        Public Property Month As String
            Get
                Return _Month
            End Get
            Set(value As String)
                _Month = value
            End Set
        End Property
        Private _EAU As Integer = 0
        Public Property EAU As String
            Get
                Return _EAU
            End Get
            Set(value As String)
                _EAU = value
            End Set
        End Property
    End Class

    Function checkMultiship(ByVal line As String) As Integer
        If Not IsNothing(ViewState("MultiShip")) Then
            Dim tempL As List(Of MultiShip) = CType(ViewState("MultiShip"), List(Of MultiShip))
            Dim itempL As IEnumerable(Of MultiShip) = tempL.Where(Function(p) p.LineNo = line)
            If Not IsNothing(itempL) Then
                tempL = itempL.ToList
            End If

            If Not IsNothing(tempL) AndAlso tempL.Count > 0 Then
                Return tempL.Sum(Function(p) p.EAU)
            End If

        End If
        Return False
    End Function
    Protected Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click

        Dim l As List(Of MultiShip) = getUI()
        If IsNothing(ViewState("MultiShip")) Then
            ViewState("MultiShip") = l
        Else
            Dim vL As List(Of MultiShip) = CType(ViewState("MultiShip"), List(Of MultiShip))
            vL.RemoveAll(Function(p) p.LineNo = Me.hfCurrentMultishipLine.Value)

            vL.AddRange(l)

            ViewState("MultiShip") = vL
        End If
        If Me.hfCurrentMultishipLine.Value Mod 100 = 0 Then

            SplitChildItem(Me.hfCurrentMultishipLine.Value)
        End If
        Me.MPMultiShip.Hide()
        Dim QM As Advantech.Myadvantech.DataAccess.QuotationMaster = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMaster(_QID)
        Dim QD As List(Of Advantech.Myadvantech.DataAccess.QuotationDetail) = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationDetail(_QID)
        Me.GridView1.DataSource = QD
        Me.GridView1.DataBind()
    End Sub
    Public Sub ibtnEdit_Click(sender As Object, e As EventArgs)
        Dim Obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(Obj.NamingContainer, GridViewRow)
        Dim line As Integer = GridView1.DataKeys(row.RowIndex).Value
        Me.hfCurrentMultishipLine.Value = line
        loadMultiShip(line)
        Me.UPMultiShip.Update()
        Me.MPMultiShip.Show()
    End Sub
    Public Sub SplitChildItem(ByVal Parent_line As Integer)
        If IsNothing(ViewState("ClientData")) Then
            ViewState("ClientData") = getClientData()
        End If
        Dim tempL As List(Of ClientData) = CType(ViewState("ClientData"), List(Of ClientData))
        Dim tempLM As List(Of MultiShip) = Nothing
        If Not IsNothing(ViewState("MultiShip")) Then
            tempLM = CType(ViewState("MultiShip"), List(Of MultiShip))
            If tempLM.Count > 0 Then
                tempLM.RemoveAll(Function(rp) rp.LineNo > Parent_line AndAlso rp.LineNo < Parent_line + 100)
            End If
        End If
        If tempL.Count > 0 Then
            Dim qtyParent As Integer = getQty(Parent_line)
            For Each x As ClientData In tempL
                If x.line > Parent_line And x.line < Parent_line + 100 Then
                    Dim qtyItem As Integer = x.QTY
                    Dim factor As Decimal = qtyItem / qtyParent
                    If Integer.TryParse(factor, Nothing) Then

                        If tempLM.Count > 0 Then

                            Dim ilparentMultiship As IEnumerable(Of MultiShip) = tempLM.Where(Function(p) p.LineNo = Parent_line)
                                If Not IsNothing(ilparentMultiship) Then
                                    Dim lparentMultiship As List(Of MultiShip) = ilparentMultiship.ToList
                                    For Each xm As MultiShip In lparentMultiship
                                        Dim childMultishipline As New MultiShip
                                        childMultishipline.LineNo = x.line
                                        childMultishipline.Month = xm.Month
                                        childMultishipline.EAU = xm.EAU * factor
                                        tempLM.Add(childMultishipline)
                                    Next
                                    ViewState("MultiShip") = tempLM
                                End If

                            End If
                        End If

                    End If

            Next
        End If



    End Sub
    Public Function getUI() As List(Of MultiShip)
        Dim l As New List(Of MultiShip)

        If Integer.TryParse(Me.txtJan.Text.Trim, Nothing) AndAlso CInt(Me.txtJan.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 1
            p.EAU = CInt(Me.txtJan.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtFeb.Text.Trim, Nothing) AndAlso CInt(Me.txtFeb.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 2
            p.EAU = CInt(Me.txtFeb.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtMar.Text.Trim, Nothing) AndAlso CInt(Me.txtMar.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 3
            p.EAU = CInt(Me.txtMar.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtApr.Text.Trim, Nothing) AndAlso CInt(Me.txtApr.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 4
            p.EAU = CInt(Me.txtApr.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtMay.Text.Trim, Nothing) AndAlso CInt(Me.txtMay.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 5
            p.EAU = CInt(Me.txtMay.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtJun.Text.Trim, Nothing) AndAlso CInt(Me.txtJun.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 6
            p.EAU = CInt(Me.txtJun.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtJul.Text.Trim, Nothing) AndAlso CInt(Me.txtJul.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 7
            p.EAU = CInt(Me.txtJul.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtAug.Text.Trim, Nothing) AndAlso CInt(Me.txtAug.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 8
            p.EAU = CInt(Me.txtAug.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtSep.Text.Trim, Nothing) AndAlso CInt(Me.txtSep.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 9
            p.EAU = CInt(Me.txtSep.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtOct.Text.Trim, Nothing) AndAlso CInt(Me.txtOct.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 10
            p.EAU = CInt(Me.txtOct.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtNov.Text.Trim, Nothing) AndAlso CInt(Me.txtNov.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 11
            p.EAU = CInt(Me.txtNov.Text.Trim)
            l.Add(p)
        End If
        If Integer.TryParse(Me.txtDec.Text.Trim, Nothing) AndAlso CInt(Me.txtDec.Text.Trim) > 0 Then
            Dim p As New MultiShip
            p.LineNo = Me.hfCurrentMultishipLine.Value
            p.Month = 12
            p.EAU = CInt(Me.txtDec.Text.Trim)
            l.Add(p)
        End If
        Return l
    End Function

    Public Sub setUI(ByVal l As List(Of MultiShip))
        Me.txtJan.Text = ""
        Me.txtFeb.Text = ""
        Me.txtMar.Text = ""
        Me.txtApr.Text = ""
        Me.txtMay.Text = ""
        Me.txtJun.Text = ""
        Me.txtJul.Text = ""
        Me.txtAug.Text = ""
        Me.txtSep.Text = ""
        Me.txtOct.Text = ""
        Me.txtNov.Text = ""
        Me.txtDec.Text = ""
        For Each t As MultiShip In l
            If t.EAU > 0 Then
                If t.Month = 1 Then
                    Me.txtJan.Text = t.EAU
                End If
                If t.Month = 2 Then
                    Me.txtFeb.Text = t.EAU
                End If
                If t.Month = 3 Then
                    Me.txtMar.Text = t.EAU
                End If
                If t.Month = 4 Then
                    Me.txtApr.Text = t.EAU
                End If
                If t.Month = 5 Then
                    Me.txtMay.Text = t.EAU
                End If
                If t.Month = 6 Then
                    Me.txtJun.Text = t.EAU
                End If
                If t.Month = 7 Then
                    Me.txtJul.Text = t.EAU
                End If
                If t.Month = 8 Then
                    Me.txtAug.Text = t.EAU
                End If
                If t.Month = 9 Then
                    Me.txtSep.Text = t.EAU
                End If
                If t.Month = 10 Then
                    Me.txtOct.Text = t.EAU
                End If
                If t.Month = 11 Then
                    Me.txtNov.Text = t.EAU
                End If
                If t.Month = 12 Then
                    Me.txtDec.Text = t.EAU
                End If
            End If
        Next
    End Sub
    Public Sub loadMultiShip(ByVal line As String)
        If Not IsNothing(ViewState("MultiShip")) Then
            Dim tempL As List(Of MultiShip) = CType(ViewState("MultiShip"), List(Of MultiShip))
            If tempL.Count > 0 Then
                tempL = tempL.Where(Function(x) x.LineNo = line).ToList
                If Not IsNothing(tempL) Then
                    setUI(tempL)
                End If
            End If

        End If
    End Sub
    <Serializable()>
    Public Class ClientData
        Private _line As Integer = 0
        Public Property line As Integer
            Get
                Return _line
            End Get
            Set(value As Integer)
                _line = value
            End Set
        End Property
        Private _EAU As Integer = 0
        Public Property EAU As Integer
            Get
                Return _EAU
            End Get
            Set(value As Integer)
                _EAU = value
            End Set
        End Property
        Private _QTY As Integer = 0
        Public Property QTY As Integer
            Get
                Return _QTY
            End Get
            Set(value As Integer)
                _QTY = value
            End Set
        End Property
        Private _isSelected As Integer = 0
        Public Property isSelected As Integer
            Get
                Return _isSelected
            End Get
            Set(value As Integer)
                _isSelected = value
            End Set
        End Property
    End Class
    Public Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)
        Dim Obj As CheckBox = CType(sender, CheckBox)
        Dim row As GridViewRow = CType(Obj.NamingContainer, GridViewRow)
        Dim line As Integer = GridView1.DataKeys(row.RowIndex).Value

        If IsNothing(ViewState("ClientData")) Then
            ViewState("ClientData") = getClientData()
        End If
        Dim tempL As List(Of ClientData) = CType(ViewState("ClientData"), List(Of ClientData))
        If tempL.Count > 0 Then
            For Each x As ClientData In tempL
                If x.line = line Then
                    If Obj.Checked Then
                        x.isSelected = 1
                    Else
                        x.isSelected = 0
                    End If
                End If
            Next
        End If
    End Sub
    Public Function getQty(ByVal line As Integer) As Integer
        If IsNothing(ViewState("ClientData")) Then
            Return -1
        End If
        Dim tempL As List(Of ClientData) = CType(ViewState("ClientData"), List(Of ClientData))
        If tempL.Count > 0 Then
            For Each x As ClientData In tempL
                If x.line = line Then
                    Return x.QTY
                End If
            Next
        End If
        Return -1
    End Function
    Public Function getEAU(ByVal line As Integer) As Integer
        If IsNothing(ViewState("ClientData")) Then
            Return -1
        End If
        Dim tempL As List(Of ClientData) = CType(ViewState("ClientData"), List(Of ClientData))
        If tempL.Count > 0 Then
            For Each x As ClientData In tempL
                If x.line = line Then
                    Return x.EAU
                End If
            Next
        End If
        Return -1
    End Function

    Public Function getIsSelected(ByVal line As Integer) As Boolean
        If IsNothing(ViewState("ClientData")) Then
            Return False
        End If
        Dim tempL As List(Of ClientData) = CType(ViewState("ClientData"), List(Of ClientData))
        If tempL.Count > 0 Then
            For Each x As ClientData In tempL
                If x.line = line Then
                    If x.isSelected = 1 Then
                        Return True
                    End If

                End If
            Next
        End If
        Return False
    End Function
    Public Sub txtBox_eau_TextChanged(sender As Object, e As EventArgs)
        Dim Obj As TextBox = CType(sender, TextBox)
        Dim row As GridViewRow = CType(Obj.NamingContainer, GridViewRow)
        Dim line As Integer = GridView1.DataKeys(row.RowIndex).Value

        If IsNothing(ViewState("ClientData")) Then
            ViewState("ClientData") = getClientData()
        End If
        Dim tempL As List(Of ClientData) = CType(ViewState("ClientData"), List(Of ClientData))
        If tempL.Count > 0 Then
            For Each x As ClientData In tempL
                If x.line = line Then
                    If Integer.TryParse(Obj.Text.Trim, Nothing) Then
                        x.EAU = Obj.Text.Trim
                    End If
                End If
            Next
        End If
    End Sub
    Public Function getClientData()

        Dim cd As New List(Of ClientData)
        For Each x As GridViewRow In Me.GridView1.Rows
            If x.RowType = DataControlRowType.DataRow Then
                Dim p As New ClientData
                p.line = x.Cells(1).Text.Trim
                p.EAU = x.Cells(3).Text.Trim

                Dim c As CheckBox = CType(x.FindControl("CheckBox1"), CheckBox)
                If c.Checked Then
                    p.isSelected = 1
                Else
                    p.isSelected = 0
                End If
                p.QTY = x.Cells(3).Text.Trim
                cd.Add(p)
            End If
        Next
        Return cd
    End Function


End Class