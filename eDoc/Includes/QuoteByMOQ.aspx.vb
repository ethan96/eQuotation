Imports System.Web.UI
Public Class QuoteByMOQ
    Inherits System.Web.UI.Page

    Dim UID As String = "07e2640e71fd482"
    Dim GVDT As DataTable = Nothing
    Dim EnableBreakPoints As New List(Of Int16)

    Private Sub QuoteByMOQ_Init(sender As Object, e As EventArgs) Handles Me.Init
        'If Not Page.IsPostBack Then

        Me.UID = Request("UID")

        If Not Page.IsPostBack Then
            Me.GetBreakPointsFromDB()
        End If

        Me.ApplyScales()
        'End If


        Me.SetColumnToGridview()
        'For i As Integer = 0 To _GVDT.Columns.Count - 1
        '    If i = 0 Then
        '        Dim bf As BoundField = New BoundField
        '        bf.HeaderText = _GVDT.Columns(i).Caption
        '        bf.DataField = _GVDT.Columns(i).ColumnName
        '        Me.GridView1.Columns.Add(bf)
        '    Else
        '        Dim tc As TemplateField = New TemplateField
        '        tc.HeaderText = _GVDT.Columns(i).Caption
        '        tc.ItemTemplate = New myItemTemplate(_GVDT.Columns(i).ColumnName, _GVDT.Columns(i).ColumnName & "_" & _GVDT.Columns(i).Caption)
        '        Me.GridView1.Columns.Add(tc)
        '    End If
        'Next

        'Dim tf As New TemplateField


        'End If

        'ApplyScales()

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LBMsg.Text = String.Empty

        If Not Page.IsPostBack Then
            Me.BindDataToGridview()

            For i As Integer = 0 To Me.EnableBreakPoints.Count - 1
                If i = 0 Then Continue For

                Select Case i
                    Case 1
                        Me.TxtScale1.Text = Me.EnableBreakPoints(i)
                    Case 2
                        Me.TxtScale2.Text = Me.EnableBreakPoints(i)
                    Case 3
                        Me.TxtScale3.Text = Me.EnableBreakPoints(i)
                    Case 4
                        Me.TxtScale4.Text = Me.EnableBreakPoints(i)
                End Select
            Next

            If Role.IsUsaAenc Then
                Label1.Text = "Enter Actual Purchase Order Qty, once order received: "
            End If

        End If

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Apply scales
        'Me.ApplyScales()
        'Me.GridView1.Columns.Clear()
        'Me.SetColumnToGridview()

        'Ryan 20161115 validate if textboxes are numbers
        LBMsg.Text = String.Empty
        If Not ValidateTextBoxes() Then
            Exit Sub
        End If

        Me.BindDataToGridview()
        Me.SaveBreakPointsFromDB()
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'save or print pdf
        'Me.SetColumnToGridview()
        'Me.BindDataToGridview()

        'Ryan 20161115 validate if textboxes are numbers
        LBMsg.Text = String.Empty
        If Not ValidateTextBoxes() Then
            Exit Sub
        End If

        Dim _msg As String = "Updated"
        If Not String.IsNullOrEmpty(Me.TxtApplyQuoteQty.Text) Then
            Dim _NewReqQty As Integer = Integer.Parse(Me.TxtApplyQuoteQty.Text)

            If _NewReqQty < 1 Then
                _msg = "Quote's request quantity cannot be less than 1"
            Else

                Dim _QuoteDetail As List(Of QuoteItem) = MyQuoteX.GetQuoteList(UID)

                Dim _dtQuoteDetailQuantityBreakPoint As DataTable = New DataTable
                _dtQuoteDetailQuantityBreakPoint =
                    tbOPBase.dbGetDataTable("EQ", "Select QuoteID,line_No,partNo,QuantityBreakPoint,SAPQuantityUnitPrice,CustomerQuantityUnitPrice From QuotationDetail_BreakPoints where quoteid='" & UID & "'")

                Dim _row() As DataRow = Nothing

                For Each _quoteitem As QuoteItem In _QuoteDetail
                    _row = _dtQuoteDetailQuantityBreakPoint.Select("partno='" & _quoteitem.partNo & "' and line_no='" & _quoteitem.line_No & "' and QuantityBreakPoint<=" & _NewReqQty, "QuantityBreakPoint Desc")

                    If _row.Count > 0 Then
                        _quoteitem.qty = _NewReqQty
                        _quoteitem.newUnitPrice = _row(0).Item("CustomerQuantityUnitPrice")
                    End If

                Next
                MyUtil.Current.CurrentDataContext.SubmitChanges()
            End If

        End If

        Me.LBMsg.Text = _msg

        Dim ccc = 1
        ccc = 1 + 2
    End Sub

    Private Sub SetColumnToGridview()
        'Me.GridView1.Columns.Clear()
        For i As Integer = 0 To GVDT.Columns.Count - 1
            If i < 2 Then
                Dim bf As BoundField = New BoundField
                bf.HeaderText = GVDT.Columns(i).Caption
                bf.DataField = GVDT.Columns(i).ColumnName
                If i = 0 Then bf.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                Me.GridView1.Columns.Add(bf)
            Else
                Dim tc As TemplateField = New TemplateField
                tc.HeaderText = GVDT.Columns(i).Caption
                tc.ItemTemplate = New myItemTemplate(GVDT.Columns(i).ColumnName, GVDT.Columns(i).ColumnName & "_" & GVDT.Columns(i).Caption, UID)
                Me.GridView1.Columns.Add(tc)
            End If
        Next
    End Sub


    Public Function GetDataTable() As DataTable
        Return Nothing
    End Function

    Private Sub SaveBreakPointsFromDB()

        Dim _breakpointstr As String = "1"

        If Not String.IsNullOrEmpty(Me.TxtScale1.Text.Trim) Then
            _breakpointstr &= "," & Me.TxtScale1.Text.Trim
        End If
        If Not String.IsNullOrEmpty(Me.TxtScale2.Text.Trim) Then
            _breakpointstr &= "," & Me.TxtScale2.Text.Trim
        End If
        If Not String.IsNullOrEmpty(Me.TxtScale3.Text.Trim) Then
            _breakpointstr &= "," & Me.TxtScale3.Text.Trim
        End If
        If Not String.IsNullOrEmpty(Me.TxtScale4.Text.Trim) Then
            _breakpointstr &= "," & Me.TxtScale4.Text.Trim
        End If

        Dim _sql As New StringBuilder
        _sql.AppendLine(" update QuotationMaster_BreakPoint ")
        _sql.AppendLine(" set BreakPoints='" & _breakpointstr & "' ")
        _sql.AppendLine(" where quoteid='" & UID & "'")
        _sql.AppendLine("")
        tbOPBase.dbExecuteNoQuery("EQ", _sql.ToString)


    End Sub

    Private Sub GetBreakPointsFromDB()
        Dim _dtQuoteDetailMasterBreakPoint As DataTable = New DataTable
        _dtQuoteDetailMasterBreakPoint = tbOPBase.dbGetDataTable("EQ", "Select QuoteID,BreakPoints From QuotationMaster_BreakPoint where quoteid='" & UID & "'")

        Dim _breakpintstr As String = "1,50,100"

        If _dtQuoteDetailMasterBreakPoint IsNot Nothing AndAlso _dtQuoteDetailMasterBreakPoint.Rows.Count > 0 Then

            _breakpintstr = _dtQuoteDetailMasterBreakPoint.Rows(0).Item("BreakPoints").ToString

        Else

            Dim _sql As New StringBuilder
            _sql.AppendLine(" insert into QuotationMaster_BreakPoint values (")
            _sql.AppendLine(" '" & UID & "' ")
            _sql.AppendLine(" ,'" & _breakpintstr & "'")
            _sql.AppendLine(")")
            tbOPBase.dbExecuteNoQuery("EQ", _sql.ToString)

        End If

        Dim _breakpintstrarr() As String = _breakpintstr.Split(",")
        For i As Integer = 0 To _breakpintstrarr.Length - 1
            EnableBreakPoints.Add(Integer.Parse(_breakpintstrarr(i)))
        Next


    End Sub

    Public Sub ApplyScales()

        Me.GVDT = New DataTable


        Dim _QuoteDetail As List(Of QuoteItem) = MyQuoteX.GetQuoteList(UID)
        Dim _parseint As Integer = 0


        'Dim _dtQuoteDetailMasterBreakPoint As DataTable = New DataTable
        '_dtQuoteDetailMasterBreakPoint = tbOPBase.dbGetDataTable("EQ", "Select QuoteID,BreakPoints From QuotationMaster_BreakPoint where quoteid='" & UID & "'")

        'If _dtQuoteDetailMasterBreakPoint IsNot Nothing AndAlso _dtQuoteDetailMasterBreakPoint.Rows.Count > 0 Then

        '    Dim _breakpintstr As String = _dtQuoteDetailMasterBreakPoint.Rows(0).Item("BreakPoints").ToString
        '    Dim _breakpintstrarr() As String = _breakpintstr.Split(",")
        '    For i As Integer = 0 To _breakpintstrarr.Length - 1
        '        _enablescales.Add(Integer.Parse(_breakpintstrarr(i)))
        '    Next

        'Else

        'EnableBreakPoints.Add(1)

        'If Not String.IsNullOrEmpty(Me.TxtScale1.Text.Trim) Then
        '    _enablescales.Add(Integer.Parse(Me.TxtScale1.Text.Trim))
        'ElseIf Request("TxtScale1") IsNot Nothing AndAlso Integer.TryParse(Request("TxtScale1").ToString, _parseint) Then
        '    _enablescales.Add(_parseint)
        'End If
        If Request("TxtScale1") IsNot Nothing AndAlso Integer.TryParse(Request("TxtScale1").ToString, _parseint) Then
            EnableBreakPoints.Add(_parseint)
        ElseIf Not String.IsNullOrEmpty(Me.TxtScale1.Text.Trim) Then
            EnableBreakPoints.Add(Integer.Parse(Me.TxtScale1.Text.Trim))
        End If

        If Request("TxtScale2") IsNot Nothing AndAlso Integer.TryParse(Request("TxtScale2").ToString, _parseint) Then
            EnableBreakPoints.Add(_parseint)
        ElseIf Not String.IsNullOrEmpty(Me.TxtScale2.Text.Trim) Then
            EnableBreakPoints.Add(Integer.Parse(Me.TxtScale2.Text.Trim))
        End If

        If Request("TxtScale3") IsNot Nothing AndAlso Integer.TryParse(Request("TxtScale3").ToString, _parseint) Then
            EnableBreakPoints.Add(_parseint)
        ElseIf Not String.IsNullOrEmpty(Me.TxtScale3.Text.Trim) Then
            EnableBreakPoints.Add(Integer.Parse(Me.TxtScale3.Text.Trim))
        End If

        If Request("TxtScale4") IsNot Nothing AndAlso Integer.TryParse(Request("TxtScale4").ToString, _parseint) Then
            EnableBreakPoints.Add(_parseint)
        ElseIf Not String.IsNullOrEmpty(Me.TxtScale4.Text.Trim) Then
            EnableBreakPoints.Add(Integer.Parse(Me.TxtScale4.Text.Trim))
        End If

        If EnableBreakPoints.Count > 0 AndAlso EnableBreakPoints(0) <> 1 Then
            EnableBreakPoints.Insert(0, 1)
        End If


        'End If

        'Dim dt As New DataTable
        Dim _col As New DataColumn("LineNo")
        GVDT.Columns.Add(_col)

        _col = New DataColumn("PartNo")
        GVDT.Columns.Add(_col)

        For i As Integer = 1 To EnableBreakPoints.Count
            _col = New DataColumn("Scale" & i)
            _col.Caption = EnableBreakPoints.Item(i - 1)
            GVDT.Columns.Add(_col)
        Next

        Dim _dtQuoteDetailQuantityBreakPoint As DataTable = New DataTable
        _dtQuoteDetailQuantityBreakPoint = tbOPBase.dbGetDataTable("EQ", "Select QuoteID,line_No,partNo,QuantityBreakPoint,SAPQuantityUnitPrice,CustomerQuantityUnitPrice From QuotationDetail_BreakPoints where quoteid='" & UID & "'")



        Dim _row As DataRow = Nothing
        Dim _rows_QDBP As DataRow() = Nothing 'QuoteDetailQuantityBreakPoint
        Dim _GVDTColumncount As Integer = GVDT.Columns.Count

        Dim SAPClient1 As New Z_SD_USPRICELOOKUP.Z_SD_USPRICELOOKUP
        Dim _IsListPriceonly As Boolean = False
        Dim _dt1 As DataTable = Nothing
        Dim _group As String = "10"
        For Each _part As QuoteItem In _QuoteDetail
            _IsListPriceonly = False
            _row = GVDT.NewRow

            _row.Item("LineNo") = _part.line_No
            _row.Item("PartNo") = _part.partNo
            '_row.Item(1) = _part.listPrice

            _rows_QDBP = _dtQuoteDetailQuantityBreakPoint.Select("Line_no=" & _part.line_No)

            If _rows_QDBP.Count > 0 Then

                For Each _RowQDBP As DataRow In _rows_QDBP
                    Dim _indexofScales As Integer = EnableBreakPoints.IndexOf(_RowQDBP.Item("QuantityBreakPoint"))
                    If _indexofScales > -1 Then
                        _row.Item(_indexofScales + 2) = _RowQDBP.Item("CustomerQuantityUnitPrice")
                    End If
                Next

            Else

                'Dim SAPClient1 As New Z_SD_USPRICELOOKUP.Z_SD_USPRICELOOKUP
                _dt1 = Nothing
                _group = "20"
                Try
                    SAPClient1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
                    SAPClient1.Connection.Open()
                    Dim p_error As String = "", p_maktx As String = "", p_head As New Z_SD_USPRICELOOKUP.ZSD_PRICE_HEAD, p_509 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_509Table
                    Dim p_513 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_513Table, p_514 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_514Table, p_517 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_517Table
                    Dim p_521 As New Z_SD_USPRICELOOKUP.ZSD_PRICE_521Table, it_markup As New Z_SD_USPRICELOOKUP.ZSD_PRICE_MARKUPTable
                    SAPClient1.Z_Sd_Uspricelookup("USH1", Now.ToString("yyyyMMdd"), _group, SAPDAL.Global_Inc.Format2SAPItem2(_part.partNo), "US01", "00", p_error, p_maktx, p_head, _
                                                  p_509, p_513, p_514, p_517, p_521, it_markup)

                    SAPClient1.Connection.Close()
                    If _group.Equals("20") Then
                        _dt1 = p_517.ToADODataTable()
                    Else
                        '_dt1 = p_521.ToADODataTable()
                    End If

                Catch ex As Exception
                    _dt1 = Nothing
                    Try
                        SAPClient1.Connection.Close()
                    Catch ex1 As Exception
                    End Try
                End Try


                Dim _listPartLookup As New List(Of PartLookup)

                If _dt1 IsNot Nothing AndAlso _dt1.Rows.Count > 0 Then
                    Dim _rows As DataRow() = Nothing, _fieldname As String = String.Empty
                    'Dim _rows As DataRow() = _dt1.Select("Konda521='L1'")
                    If _group.Equals("20") Then
                        _rows = _dt1.Select("Konda517='P1'")
                        _fieldname = "Sclpr"
                    Else
                        _rows = _dt1.Select("Konda521='L1'")
                        _fieldname = "Sldpr"
                    End If


                    If _rows.Count > 0 Then

                        For Each _row1 As DataRow In _rows
                            Dim _partlookup As New PartLookup
                            _partlookup.PartNo = _part.partNo
                            _partlookup.QuantityBreakPoint = _row1.Item("Kstbm")
                            _partlookup.QuantityBreakPointUnitPrice = _row1.Item(_fieldname)
                            _partlookup.QuantityBreakPointNewUnitPrice = _row1.Item(_fieldname)
                            _partlookup.IsSAPDefinedQuantityBreakPoint = True

                            Dim _indexofScales As Integer = EnableBreakPoints.IndexOf(_partlookup.QuantityBreakPoint)
                            If _indexofScales > -1 Then
                                _row.Item(_indexofScales + 2) = _partlookup.QuantityBreakPointUnitPrice
                            End If

                            _listPartLookup.Add(_partlookup)

                        Next
                        'Else
                        Dim _currentval As Decimal = 0
                        'For i As Integer = 1 To EnableBreakPoints.Count
                        For i As Integer = 2 To _GVDTColumncount - 1

                            If _row.Item(i) Is DBNull.Value Then
                                _row.Item(i) = _currentval

                                Dim _partlookup As New PartLookup
                                _partlookup.PartNo = _part.partNo
                                '_partlookup.QuantityBreakPoint = EnableBreakPoints(i - 1)
                                _partlookup.QuantityBreakPoint = GVDT.Columns(i).Caption
                                _partlookup.QuantityBreakPointUnitPrice = -1
                                _partlookup.QuantityBreakPointNewUnitPrice = _currentval
                                _partlookup.IsSAPDefinedQuantityBreakPoint = False
                                _listPartLookup.Add(_partlookup)
                            Else
                                _currentval = _row.Item(i)
                            End If

                        Next

                    Else
                        '-------------
                        _IsListPriceonly = True
                    End If
                Else
                    _IsListPriceonly = True
                End If

                If _IsListPriceonly Then
                    For i As Integer = 2 To _GVDTColumncount - 1
                        _row.Item(i) = _part.listPrice

                        Dim _partlookup As New PartLookup
                        _partlookup.PartNo = _part.partNo
                        _partlookup.QuantityBreakPoint = GVDT.Columns(i).Caption
                        _partlookup.QuantityBreakPointUnitPrice = -1
                        _partlookup.QuantityBreakPointNewUnitPrice = _part.listPrice
                        _partlookup.IsSAPDefinedQuantityBreakPoint = False
                        _listPartLookup.Add(_partlookup)

                    Next
                End If

                'Write data to table
                For Each _partlookupitem As PartLookup In _listPartLookup
                    'QuoteID,line_No,partNo,QuantityBreakPoint,SAPQuantityUnitPrice,CustomerQuantityUnitPrice
                    Dim _sql As New StringBuilder
                    _sql.AppendLine(" insert into QuotationDetail_BreakPoints values (")
                    _sql.AppendLine(" '" & UID & "' ")
                    _sql.AppendLine(" ," & _part.line_No)
                    _sql.AppendLine(" ,'" & _part.partNo & "' ")
                    _sql.AppendLine(" ," & _partlookupitem.QuantityBreakPoint)
                    _sql.AppendLine(" ," & _partlookupitem.QuantityBreakPointUnitPrice)
                    _sql.AppendLine(" ," & _partlookupitem.QuantityBreakPointNewUnitPrice)
                    _sql.AppendLine(" ,GETDATE() ")
                    _sql.AppendLine(" ,'" & Pivot.CurrentProfile.UserId & "' ")
                    _sql.AppendLine(" ,GETDATE() ")
                    _sql.AppendLine(" ,'" & Pivot.CurrentProfile.UserId & "' ")
                    _sql.AppendLine(")")
                    tbOPBase.dbExecuteNoQuery("EQ", _sql.ToString)
                Next

            End If

            GVDT.Rows.Add(_row)

        Next
        GVDT.AcceptChanges()


    End Sub

    Private Sub BindDataToGridview()
        Me.GridView1.DataSource = GVDT
        Me.GridView1.DataBind()

        Dim _dtcolcount As Integer = GVDT.Columns.Count
        Dim _gvcolcount As Integer = Me.GridView1.Columns.Count

        'If _dtcolcount < _gvcolcount Then
        For i As Integer = 0 To _gvcolcount - 1
            If i < _dtcolcount Then
                Me.GridView1.Columns(i).Visible = True
            Else
                Me.GridView1.Columns(i).Visible = False
            End If

        Next
        'End If

    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound

        'Dim data As DataRowView = e.Row.DataItem

        If e.Row.RowType = DataControlRowType.Header Then
            For i As Integer = 2 To e.Row.Cells.Count - 1
                If i < CType(GridView1.DataSource, DataTable).Columns.Count Then
                    e.Row.Cells(i).Text = CType(GridView1.DataSource, DataTable).Columns(i).Caption
                End If
            Next
        End If

        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    For i As Integer = 1 To e.Row.Cells.Count - 1

        '        Dim txtCountry As New TextBox()
        '        txtCountry.Width = 80
        '        txtCountry.ID = "txtMOQPrice"
        '        txtCountry.Text = TryCast(e.Row.DataItem, DataRowView).Row("Scale" & i).ToString()
        '        AddHandler txtCountry.TextChanged, AddressOf txtMOQPrice_TextChanged

        '        e.Row.Cells(i).Controls.Add(txtCountry)
        '    Next

        'End If

    End Sub

    'Protected Sub txtMOQPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim obj As TextBox = CType(sender, TextBox), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
    '    Dim id As Integer = Me.GridView1.DataKeys(row.RowIndex).Value
    '    Dim D As String = Util.ReplaceSQLChar(obj.Text)
    '    'If id <> 100 Then
    '    '    myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, id), String.Format("category='{0}'", D))
    '    'End If
    'End Sub


    Public Function ValidateTextBoxes() As Boolean
        If Not String.IsNullOrEmpty(Me.TxtScale1.Text) AndAlso Not IsNumeric(Me.TxtScale1.Text) Then
            LBMsg.Text = "Scales can only be numbers."
            Me.TxtScale1.Focus()
            Return False
        ElseIf Not String.IsNullOrEmpty(Me.TxtScale2.Text) AndAlso Not IsNumeric(Me.TxtScale2.Text) Then
            LBMsg.Text = "Scales can only be numbers."
            Me.TxtScale2.Focus()
            Return False
        ElseIf Not String.IsNullOrEmpty(Me.TxtScale3.Text) AndAlso Not IsNumeric(Me.TxtScale3.Text) Then
            LBMsg.Text = "Scales can only be numbers."
            Me.TxtScale3.Focus()
            Return False
        ElseIf Not String.IsNullOrEmpty(Me.TxtScale4.Text) AndAlso Not IsNumeric(Me.TxtScale4.Text) Then
            LBMsg.Text = "Scales can only be numbers."
            Me.TxtScale4.Focus()
            Return False
        Else
            Return True
        End If
    End Function

End Class


Class PartLookup

    Public PartNo As String = String.Empty
    Public IsSAPDefinedQuantityBreakPoint As Boolean = True
    Public QuantityBreakPoint As Integer = 0
    Public QuantityBreakPointUnitPrice As Decimal = 0.0
    Public QuantityBreakPointNewUnitPrice As Decimal = 0.0


End Class

Public Class myItemTemplate
    Implements ITemplate


    Private ColName As String = String.Empty
    Private TextBoxID As String = String.Empty
    Private QuiteID As String = String.Empty

    Public Sub New(ByVal _ColName As String, ByVal _TextBoxID As String, ByVal _QuiteID As String)
        ColName = _ColName
        TextBoxID = _TextBoxID
        QuiteID = _QuiteID
    End Sub

    Public Sub InstantiateIn(ByVal container As Control) _
          Implements ITemplate.InstantiateIn
        Dim txtPSN As TextBox = New TextBox()
        'txtPSN.ID = "txtPSN"
        txtPSN.ID = Me.TextBoxID
        txtPSN.AutoPostBack = False
        txtPSN.Width = 80

        AddHandler txtPSN.DataBinding, AddressOf txtPSN_DataBinding

        AddHandler txtPSN.TextChanged, AddressOf txtPSN_TextChanged
        container.Controls.Add(txtPSN)
    End Sub

    Private Sub txtPSN_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData(sender, ColName)
    End Sub

    Private Sub txtPSN_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        'CType(sender, TextBox).Page.Response.Write("-")


        Dim txtid As String = CType(sender, TextBox).ID ' e.g. Scale2_50
        Dim gvrow As GridViewRow = DirectCast(CType(sender, TextBox).NamingContainer, System.Web.UI.WebControls.GridViewRow)

        Dim lineno = gvrow.Cells(0).Text '	1 or 2 ... or 100 or 101...etc.

        Dim partno = gvrow.Cells(1).Text '	96PSA-A60W12R1-1
        Dim BreakPoint As String = txtid.Split("_")(1)
        Dim _newprice As String = CType(sender, TextBox).Text

        'Dim _decval As Decimal = 0
        'If Not Decimal.TryParse(_newprice, _decval) Then
        '    Exit Sub
        'End If

        Dim rowidx As Integer = DirectCast(CType(sender, TextBox).NamingContainer, System.Web.UI.WebControls.GridViewRow).RowIndex


        Dim _sql As New StringBuilder

        _sql.AppendLine(" select quoteid,QuantityBreakPoint,line_No,CustomerQuantityUnitPrice from QuotationDetail_BreakPoints ")
        _sql.AppendLine(" where quoteid='" & QuiteID & "'")
        _sql.AppendLine(" and QuantityBreakPoint=" & BreakPoint)
        _sql.AppendLine(" and line_No=" & lineno)

        'Dim rowcount = tbOPBase.dbExecuteScalar("EQ", _sql.ToString)
        Dim _dtQBP As DataTable = tbOPBase.dbGetDataTable("EQ", _sql.ToString)
        _sql.Clear()

        Dim _decval As Decimal = 0, _IsNumber As Boolean = True
        _IsNumber = Decimal.TryParse(_newprice, _decval)

        'If rowcount IsNot Nothing AndAlso rowcount > 0 Then
        If _dtQBP IsNot Nothing AndAlso _dtQBP.Rows.Count > 0 Then


            If Not _IsNumber Then
                CType(sender, TextBox).Text = _dtQBP.Rows(0).Item("CustomerQuantityUnitPrice")
                Exit Sub
            Else
                _sql.AppendLine(" update QuotationDetail_BreakPoints ")
                _sql.AppendLine(" set CustomerQuantityUnitPrice=" & _newprice)
                _sql.AppendLine(" where quoteid='" & QuiteID & "'")
                _sql.AppendLine(" and QuantityBreakPoint=" & BreakPoint)
                _sql.AppendLine(" and line_No=" & lineno)
                tbOPBase.dbExecuteNoQuery("EQ", _sql.ToString)
            End If

        Else
            If _IsNumber Then
                _sql.AppendLine(" insert into QuotationDetail_BreakPoints values (")
                _sql.AppendLine(" '" & QuiteID & "' ")
                _sql.AppendLine(" ," & lineno)
                _sql.AppendLine(" ,'" & partno & "' ")
                _sql.AppendLine(" ," & BreakPoint)
                _sql.AppendLine(" ,-1") 'set to -1 means SAP does not have this breakpoint of the part
                _sql.AppendLine(" ," & _newprice)
                _sql.AppendLine(" ,GETDATE() ")
                _sql.AppendLine(" ,'" & Pivot.CurrentProfile.UserId & "' ")
                _sql.AppendLine(" ,GETDATE() ")
                _sql.AppendLine(" ,'" & Pivot.CurrentProfile.UserId & "' ")
                _sql.AppendLine(")")
                tbOPBase.dbExecuteNoQuery("EQ", _sql.ToString)
            End If

        End If
        'tbOPBase.dbExecuteNoQuery("EQ", _sql.ToString)
    End Sub

    'Private Sub txtPSN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim ccc = 1
    'End Sub

    Public Sub bindData(ByVal sender As Object, ByVal colName As String)
        Dim ctrl As TextBox = CType(sender, TextBox)
        Dim container = CType(ctrl.NamingContainer, GridViewRow)
        Dim value As Object = DataBinder.Eval(container.DataItem, colName)
        If value Is DBNull.Value Then
            ctrl.Text = "N/A"
        Else
            ctrl.Text = value.ToString
        End If
    End Sub


End Class

