Imports Oracle.DataAccess.Client
Public Class SiebelQuote
   
    Inherits System.Web.UI.Page
    Public listR As Integer = 0
    Public chkR As Integer = 0
    Public Shared _BlockSearchSiebelQuoteByCreatedDate As String = "2015-09-01"
    Public Shared _BlockSiebelQuoteCreatedDate As String = "2015-09-01"
    Public _BlockQuote2OrderFunctionDate As String = "2015-10-01"
    'Public _BlockQuote2eQFunctionDate As String = "2015-10-01"
    Public _BlockQuote2eQFunctionDate As String = "9999-12-31"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Frank 20150902 Release converting Siebel quote to order for ATW KA sales
        If Request("QUOTEID") IsNot Nothing Then
            Dim sqlstr As New StringBuilder
            sqlstr.AppendLine(" select a.ROW_ID, a.CREATED_BY, b.EMAIL_ADDR, c.NAME as PRIMARY_POSITION_NAME ")
            sqlstr.AppendLine(" from S_DOC_QUOTE a inner join S_CONTACT b on a.CREATED_BY=b.ROW_ID inner join S_POSTN c on b.ROW_ID=c.PR_EMP_ID ")
            sqlstr.AppendLine(" where (c.NAME like '%ATW/%KA%/%' or c.NAME like '%ATW%KA%-%' or c.NAME like '%ATW/CCR/eP%') ")
            sqlstr.AppendLine(" and a.ROW_ID='" & Request("QUOTEID") & "' ")

            Dim dt As DataTable = tbOPBase.dbGetDataTable("CRM", sqlstr.ToString)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                _BlockSiebelQuoteCreatedDate = "9999-12-31"
                _BlockQuote2OrderFunctionDate = "9999-12-31"
            Else
                _BlockSiebelQuoteCreatedDate = "2015-09-01"
                _BlockQuote2OrderFunctionDate = "2015-10-01" 'Frank：這個日期不用改了，因為我在改的現在已超過2015-10-01了，所以沒差
            End If

        End If

        'Frank 20151002:因為ATW業務認為把SIEBEL QUOTE轉進eQ很重要所以TC有以下調整指示
        'Dear Julia,
        '理解, 我們會修改程式如下
        '1.Siebel上所有的quote皆可轉成eQuotation的quote
        '2.Siebel上9/1前建立的quote可透過MyAdvantech轉單, 9/1後的無法轉單，但仍可轉成eQuotation的quote
        '再請Frank修改好後告知Julia了, 謝謝


        'TC已發公告通知以下
        ' Dear all,
        '7/1 eQuotation正式上線以來至今，相信各位已經將報價作業平臺完全移至eQuotation了
        '因此9/1起，我們會將Siebel quote轉入MyAdvantech Order的這段程式停用，讓所有quote都一律由eQuotation建立
        '9/1前在Siebel上建立的quote則在10/1前仍可透過MyAdvantech轉單，敬請知悉，謝謝
        Dim BlockQuote2OrderDate As DateTime = DateTime.ParseExact(_BlockQuote2OrderFunctionDate, "yyyy-MM-dd", Nothing)
        Dim BlockQuote2eQDate As DateTime = DateTime.ParseExact(_BlockQuote2eQFunctionDate, "yyyy-MM-dd", Nothing)
        Dim dtimeToday As DateTime = DateTime.Now
        If BlockQuote2OrderDate < dtimeToday Then btnQuote2Order.Enabled = False
        If BlockQuote2eQDate < dtimeToday Then btnQuote2eQ.Enabled = False


        If Not Page.IsPostBack AndAlso Request("QUOTEID") IsNot Nothing Then

            gvItems.EmptyDataText = "No line items"
            Dim qid As String = Trim(Request("QUOTEID")).Replace("'", "")


            hd_Qid.Value = qid
            Session.Contents.Remove("Product_Err")
            Session.Contents.Remove("GV_checkbox")
            listR = 0
            chkR = 0
            Dim sb As New System.Text.StringBuilder
            Dim qmDt As DataTable = GetQuoteHeader(qid)
            If qmDt IsNot Nothing AndAlso qmDt.Rows.Count = 1 Then
                With qmDt.Rows(0)
                    lbAccount.Text = .Item("ACCOUNT_NAME") : lbCurr.Text = .Item("Currency")
                    'If IsDBNull(.Item("DUE_DT")) OrElse IsDBNull(.Item("Effective_Date")) Then                      
                    'Else
                    lbDue.Text = CDate(.Item("DUE_DT")).ToString("yyyy/MM/dd") : lbEffDate.Text = CDate(.Item("Effective_Date")).ToString("yyyy/MM/dd")
                    'End If
                    lbOptyid.Text = qmDt.Rows(0).Item("OPTY_ID").ToString.Trim
                    lbFstName.Text = .Item("First_Name") : lbLstName.Text = .Item("Last_Name")
                    lbOptyName.Text = .Item("OPTY_NAME") : lbQuoteName.Text = .Item("NAME")
                    lbPickedQuoteName.Text = .Item("NAME")
                    lbQuoteNum.Text = .Item("QUOTE_NUM") : lbQuoteStatus.Text = .Item("QUOTE_STATUS")
                    lbSalesRep.Text = .Item("Sales_Rep")
                    lbTotal.Text = .Item("QUOTE_SUM")
                    lbERPID.Text = .Item("ERPID")
                    lbSalesPersonLstName.Text = .Item("SalesName")
                    Labaccountrowid.Text = .Item("ACCOUNT_ROW_ID")
                    Me.txtReqDate.Text = CDate(.Item("DUE_DT")).ToString("yyyy/MM/dd")
                    If Not String.IsNullOrEmpty(lbOptyid.Text.Trim) Then
                        If .Item("Sales_Stage_ID").ToString.Equals("1-VXVAID") Then
                            Me.lbOptyid.Text &= "<br /><font color='red'>The opportunity's sales stage is '100% Won-PO', please specify a new opportunity</font>"
                            Me.txtOptyName.Text = ""
                            Me.txtOptyRowID.Text = ""
                        Else
                            Me.txtOptyName.Text = .Item("OPTY_NAME")
                            Me.txtOptyRowID.Text = lbOptyid.Text
                        End If
                        DDLOptyStage.Visible = False
                        LabelOptyStage.Visible = False
                    End If
                End With

                Bind()
                'gvItems.DataSource = qiDt : gvItems.DataBind()
                '  src1.SelectCommand = GetQuoteLinesSql(hd_Qid.Value)


                ' Dim GVDataSource = Me.gvItems.DataSource


                'OrderUtilities.showDT(qmDt)
                'OrderUtilities.showDT(dbUtil.dbGetDataTable("CRMAPPDB", GetQuoteLinesSql(hd_Qid.Value)))
                Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "SELECT  top 1 (City+' '+Address+' '+ZIPCODE ) as AddressStr,isnull(PHONE_NUM,'')  as Tel   FROM  SIEBEL_ACCOUNT WHERE ROW_ID ='" + Labaccountrowid.Text.Trim + "'")
                Dim _tel As String = String.Empty
                If dt.Rows.Count > 0 Then
                    lbAdr.Text = dt.Rows(0).Item("AddressStr")
                    'lbtel.Text = dt.Rows(0).Item("Tel")
                    _tel = dt.Rows(0).Item("Tel").ToString
                    If Not String.IsNullOrEmpty(_tel) Then
                        lbtel.Text = _tel.Split(Chr(10))(0)
                    End If
                End If
            Else
                lbMsg.Text = "Requested quotation cannot be found"
            End If
        End If
    End Sub
    Public Sub Bind()
        Dim ItemsDt As DataTable = GetQuoteLines(Request("QUOTEID"))
        Dim ItemsDt2 As DataTable = ItemsDt.Clone()
        Dim ChineseItems As String() = New String() {"一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三"}
        ItemsDt.Columns.Add("LineNo", GetType(Integer))
        'ItemsDt2.Columns.Add("LineNo", GetType(Integer))
        ' Dim j As Integer = ItemsDt.Rows.Count - 1
        Dim k As Integer = 1
        For Each dr As DataRow In ItemsDt.Rows
            dr.Item("LineNo") = k
            k += 1
        Next
        ItemsDt.AcceptChanges()
        Dim TotalCount As Integer = ItemsDt.Rows.Count + 2
        Dim ishaveBtos As Boolean = False, pn As String = String.Empty
        For i As Integer = 0 To ItemsDt.Rows.Count - 1
            If (ItemsDt.Rows(i).Item("ItemMark").ToString.StartsWithV2(ChineseItems)) Then
                ItemsDt.Rows(i).Item("LineNo") = -i
                pn = ItemsDt.Rows(i).Item("PART_NO").ToString.Trim
                'If String.Equals(ItemsDt.Rows(i).Item("PART_NO").ToString.Trim, "T") OrElse Business.IsPartInBTOS_CTOSMaterialGroup(pn) Then
                If String.Equals(pn, "T") OrElse Business.IsPartInBTOS_CTOSMaterialGroup(pn) Then
                    If String.Equals(pn, "T") Then ItemsDt.Rows(i).Item("PART_NO") = "PTRADE-BTO"
                    If ishaveBtos = False Then
                        ItemsDt.Rows(i).Item("LineNo") = 0
                        ishaveBtos = True
                    Else
                        ItemsDt.Rows(i).Item("LineNo") = i
                    End If
                End If
            End If
            If ishaveBtos AndAlso ItemsDt.Rows(i).Item("PART_NO").ToString.StartsWith("AGS-EW-", StringComparison.InvariantCultureIgnoreCase) Then
                ItemsDt.Rows(i).Item("LineNo") = TotalCount
                TotalCount += 1
            End If
            ItemsDt.AcceptChanges()
        Next
        Dim dv As DataView = ItemsDt.DefaultView
        dv.Sort = "LineNo asc"
        'Do While ItemsDt.Rows.Count > 0
        '    If 1 = 1 Then
        '        Exit Do
        '    End If
        'Loop
        'GridView1.DataSource = dv.ToTable()
        'GridView1.DataBind()

        gvItems.DataSource = dv.ToTable()

        gvItems.DataBind()

    End Sub
    Public Shared Function ShowProdStatus(ByVal pn As String) As String
        Dim obj As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 status from sap_product_org where org_id='TW01' and part_no='{0}'", pn.Replace("'", "")))
        If obj IsNot Nothing Then
            Return obj.ToString()
        Else
            Return "N/A"
        End If
    End Function
    Protected Sub cbx_NewOpty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If CType(sender, CheckBox).Checked Then
            Me.txtOptyRowID.Text = "new ID"
        Else
            Me.txtOptyRowID.Text = ""
        End If
        Me.txtOptyName.Text = "" : Me.UP_Opty.Update()
    End Sub
    Private Sub pupupMPOPTY(ByVal _tabid As Integer)
        Dim _accrowid As String = Me.Labaccountrowid.Text
        CType(Me.ascxPickopty1.FindControl("h_rowid"), HiddenField).Value = _accrowid
        Me.ascxPickopty1.ShowData(_accrowid, txtOptyName.Text.Replace("'", "''").Trim) : Me.ascxPickopty1.SetTabSelectedIndex(_tabid)
        CType(ascxPickopty1.FindControl("TabContainer1").FindControl("tbPickOpportunity").FindControl("txtSearchOptyName"), TextBox).Text = txtOptyName.Text
        Me.UPPickOpty.Update() : Me.MPPickOpty.Show()
    End Sub

    Protected Sub ibtn_PickOpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_PickOpty.Click
        pupupMPOPTY(0)
    End Sub

    Protected Sub ButtonNewOpty_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        pupupMPOPTY(1)
    End Sub



    Public Sub PickOptyEnd(ByVal optyid As Object, ByVal optyName As Object, ByVal optyStage As Object)
        Dim Id As String = optyid, Name As String = optyName, Stage As String = optyStage

        Me.txtOptyRowID.Text = Id : Me.txtOptyName.Text = Name : Me.UP_Opty.Update() : Me.MPPickOpty.Hide()
        If Not String.IsNullOrEmpty(optyStage) Then
            Me.DDLOptyStage.SelectedValue = optyStage
        Else
            DDLOptyStage.Visible = False
            LabelOptyStage.Visible = False
        End If
        'Me.LabelOptyRowID.Text = Id : Me.LabelOptyName.Text = Name : Me.LabelOptyStage.Text = Stage
        'Me.UP_Opty.Update() : Me.MP1.Hide()
    End Sub
    Protected Sub btnQuote2Order_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        btnQuote2Ordernew_Click(Nothing, Nothing)
    End Sub
    Public Function IsBtoOrder(ByVal partno As String) As Boolean
        'For Each r As GridViewRow In gvItems.Rows
        '    If r.RowType = Web.UI.WebControls.DataControlRowType.DataRow Then
        '        Dim chk As CheckBox = r.FindControl("item")
        '        If chk.Checked Then
        '            Dim rPn As String = CType(r.FindControl("txtRowPN"), TextBox).Text.Trim()                   
        '            If rPn.Trim.ToUpper.EndsWith("BTO") AndAlso r.RowIndex = 0 Then
        '                Return True
        '            Else
        '                Return False
        '            End If 
        '        End If
        '    End If
        'Next
        If partno.Trim.EndsWith("BTO", StringComparison.CurrentCultureIgnoreCase) OrElse partno.Trim.EndsWith("CTOS", StringComparison.CurrentCultureIgnoreCase) Then
            Return True
        End If
        Return False
    End Function
    Protected Sub gvItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        src1.SelectCommand = GetQuoteLinesSql(hd_Qid.Value)
    End Sub

    Protected Sub gvItems_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)
        src1.SelectCommand = GetQuoteLinesSql(hd_Qid.Value)
    End Sub

    Protected Sub gvItems_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        src1.SelectCommand = GetQuoteLinesSql(hd_Qid.Value)
    End Sub
    Dim TotalRevenue As Decimal = 0
    Protected Sub gvItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(11).Text = FormatNumber(CDbl(CType(e.Row.Cells(8).FindControl("txtPrice"), TextBox).Text) * CDbl(CType(e.Row.Cells(10).FindControl("txtQty"), TextBox).Text), 2)
            If Decimal.TryParse(e.Row.Cells(11).Text, 0) Then
                TotalRevenue += Decimal.Parse(e.Row.Cells(11).Text)
            End If
            lbTotal.Text = TotalRevenue.ToString("f2")
            '為了防止perant item有修改過，重新整理後qty數量會復原
            If hf_qty.Value <> "" Then
                Dim pQty As Integer = CInt(hf_qty.Value)
                CType(e.Row.Cells(7).FindControl("txtQty"), TextBox).Text = CStr(CType(CType(e.Row.Cells(7).FindControl("txtQty"), TextBox).Text, Integer) * pQty)
            End If

            Dim rPn As String = CType(e.Row.Cells(2).FindControl("txtRowPN"), TextBox).Text.Trim()
            'Below code is commented out by Frank
            'If rPn.EndsWith("BTO") Then
            '    Dim chk As CheckBox = e.Row.Cells(1).FindControl("item")
            '    chk.Checked = True
            '    chk.Enabled = False
            'End If

            'JJ 2014/3/18：Session("Product_Err") Not Is Nothing表示,ProductList內的料號其中有一個有問題
            If Not Session("Product_Err") Is Nothing Then

                Dim Literal_Err As Literal = e.Row.Cells(2).FindControl("Literal_err")
                Dim _ProductList As New List(Of SAPDAL.ProductX)
                Dim _GVcheckbox As New List(Of Boolean)
                Dim chk As CheckBox = e.Row.Cells(1).FindControl("item")
                _ProductList = Session("Product_Err")

                '因為檢查後如果有問題，重跑RowDataBound後CheckBox會被還原了，所以必須記下來重新載入
                _GVcheckbox = Session("GV_checkbox")
                chk.Checked = _GVcheckbox(chkR)

                If chk.Checked Then
                    CType(e.Row.Cells(2).FindControl("txtRowPN"), TextBox).Text = _ProductList(listR).PartNo

                    'StatusCode=""表示查無此料號
                    If _ProductList(listR).StatusCode = "" Then
                        Literal_Err.Text += "<p style='color: #CC0000'>Invalid part number<p>"
                    Else
                        'Is Phase Out就顯示料號的狀態
                        If _ProductList(listR).IsPhaseOut Then
                            Literal_Err.Text += "<p style='color: #CC0000'>Invalid part number<p>"
                            Literal_Err.Text += "<p style='color: #CC0000'>Status:(" + _ProductList(listR).StatusCode + ")" + _ProductList(listR).StatusDesc + "<p>"
                        Else
                            Literal_Err.Text = ""
                        End If
                    End If
                    listR += 1 'Product List用來記順序的

                Else
                    Literal_Err.Text = ""
                End If
                chkR += 1 '用來記錄GridView內checkbox的順序的
            End If
        End If
    End Sub

    Protected Sub gvItems_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)

    End Sub


    Protected Sub btnConfirmOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.End()
        'For k As Integer = 0 To gvItems.Rows.Count - 1
        '    Response.Write(k & "|")
        '    For m As Integer = 0 To gvItems.Columns.Count - 1
        '        Response.Write("<font color=""#ff0000"">" & m & ":</font>") : Response.Write(gvItems.Rows(k).Cells(m).Text)
        '    Next
        '    Response.Write("<br>")
        'Next
        'Response.End()
        lbMsg.Text = "" : lbOrderMsg.Text = ""
        src1.SelectCommand = GetQuoteLinesSql(hd_Qid.Value)
        If Request("QUOTEID") IsNot Nothing Then
            Dim qid As String = Trim(Request("QUOTEID")).Replace("'", "")
            Dim sb As New System.Text.StringBuilder
            Dim qmDt As DataTable = GetQuoteHeader(qid)
            If qmDt IsNot Nothing AndAlso qmDt.Rows.Count = 1 Then
                Dim QuoteToERPId As String = qmDt.Rows(0).Item("ERPID").ToString().ToUpper().Trim().Replace("'", "")
                Dim OptyId As String = qmDt.Rows(0).Item("OPTY_ID").ToString()
                Dim ReqDate As String = qmDt.Rows(0).Item("DUE_DT").ToString()
                If QuoteToERPId <> "" Then
                    Dim ERPIDInfDt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", _
                    String.Format("select kunnr, name1 from saprdp.kna1 where mandt='168' and kunnr='{0}'", QuoteToERPId))
                    'Dim qiDt As DataTable = GetQuoteLines(qid)
                    If ERPIDInfDt.Rows.Count = 1 Then
                        'Me.chgCompany.TargetCompanyId = QuoteToERPId
                        'Me.chgCompany.ChangeToCompanyId()

                        'master
                        Dim UID As String = System.Guid.NewGuid().ToString.Replace("-", "")
                        Dim OrderNo As String = ""
                        ' OrderNo = OrderUtilities.getOrderNumberOracle(UID)
                        Dim PoNo As String = OrderNo
                        Dim PartialFlag As String = "Y"
                        Dim RequiredDate As String = Me.txtReqDate.Text
                        Dim Currency As String = Session("Company_Currency")
                        Dim Incoterm As String = ""
                        Dim IncotermText As String = ""
                        Dim ShipCondition As String = "" 'OrderUtilities.getShipConditionByERPID(QuoteToERPId)
                        Dim SalesNote As String = Me.txtSalesNote.Text
                        Dim OrderNote As String = Me.txtOrderNote.Text

                        'OrderUtilities.clearOrder(OrderNo)

                        ' OrderUtilities.OrderMaster_Insert(UID, OrderNo, "SO", PoNo, Now(), QuoteToERPId, QuoteToERPId, QuoteToERPId, "", Now(), "", "", PartialFlag, "", "", 0, 0, "", "", "", "", Now(), RequiredDate, "", Currency, OrderNote, "", 0, 0, Session("User_id"), "Z", Incoterm, IncotermText, SalesNote, "", ShipCondition)

                        'Dim str As String = "select IsNull(attention,'') as attention, IsNull(ship_via,'') as ship_via from sap_dimcompany where company_id = '" & Session("COMPANY_ID") & "' and org_id = '" & Session("COMPANY_ORG_ID") & "' and company_type='Partner'"

                        'detail
                        Dim i As Integer = 0
                        Dim count As Integer = 0
                        Dim g_adoConn As New OracleConnection(ConfigurationManager.ConnectionStrings("SAP_PRD").ConnectionString)
                        g_adoConn.Open()
                        Dim dbCmd As OracleCommand = g_adoConn.CreateCommand()
                        dbCmd.CommandType = CommandType.Text

                        For Each r As GridViewRow In gvItems.Rows
                            If count + 1 = 100 Or i > 200 Then
                                lbMsg.Text = "too much items."
                                Exit Sub
                            End If


                            If r.RowType = Web.UI.WebControls.DataControlRowType.DataRow Then
                                Dim chk As CheckBox = r.FindControl("item")
                                If chk.Checked Then
                                    Dim rPn As String = CType(r.FindControl("txtRowPN"), TextBox).Text.Trim()
                                    Dim rLp As Decimal = CDbl(r.Cells(7).Text)
                                    Dim rUp As Decimal = CType(r.FindControl("txtPrice"), TextBox).Text.Trim()
                                    Dim rQty As Integer = CType(r.FindControl("txtQty"), TextBox).Text.Trim()
                                    Dim rReqDate As String = CType(r.FindControl("txtDueDate"), TextBox).Text.Trim()
                                    Dim strSqlCmd As String = String.Format( _
                                    "select COUNT(A.MATNR) from saprdp.mara a left join saprdp.mvke b on a.matnr=b.matnr where a.matnr='{1}'" & _
                                    " and (a.MATKL IN ('BTOS','CTOS','ZSRV') or (b.VMSTA IN ('A','N','H','S5','M1') and b.VKORG='{0}')) " & _
                                    " and a.mandt='168' AND B.MANDT='168'", Session("org_id").ToString.ToUpper, SAPDAL.Global_Inc.Format2SAPItem(rPn.Replace("'", "''").ToUpper))
                                    dbCmd.CommandText = strSqlCmd
                                    Dim retObj As Object = Nothing
                                    Try
                                        retObj = dbCmd.ExecuteScalar()
                                    Catch ex As Exception

                                    End Try

                                    If Not IsNothing(retObj) AndAlso CInt(retObj) > 0 Then
                                        If rPn.ToString.ToUpper Like "*-BTO" Then
                                            Dim SBUPDATELINENO As String = String.Format("update MyAdvantechGlobal.dbo.order_detail set order_detail.LINE_NO=order_detail.LINE_NO+100 where order_detail.order_id='{0}'", UID)
                                            tbOPBase.dbExecuteNoQuery("B2B", SBUPDATELINENO.ToString())
                                            'OrderUtilities.OrderDetail_Insert(UID, 100, "", rPn, "", rQty, rLp, rUp, Now(), "", "", rReqDate, "Z", 0, Now(), 0)
                                            i += 100
                                        Else
                                            ' OrderUtilities.OrderDetail_Insert(UID, i + 1, "", rPn, "", rQty, rLp, rUp, Now(), "", "", rReqDate, "Z", 0, Now(), 0)
                                            i += 1
                                        End If
                                        Dim sb1 As New System.Text.StringBuilder
                                        With sb1
                                            .AppendLine(String.Format(" update MyAdvantechGlobal.dbo.order_detail set order_detail.DeliveryPlant=p.DeliveryPlant "))
                                            .AppendLine(String.Format(" from sap_product_org p  "))
                                            .AppendLine(String.Format(" where order_detail.part_no=p.part_no " + _
                                                                      " and order_id='{0}' and p.org_id='{1}' ", UID, Session("org_id")))
                                        End With
                                        ' dbUtil.dbExecuteNoQuery("B2B", sb1.ToString())
                                        count += 1
                                    Else
                                        Dim sql As String = String.Format("delete from  MyAdvantechGlobal.dbo.order_master where order_id='{0}'", UID)
                                        '  dbUtil.dbExecuteNoQuery("B2B", sql.ToString())
                                        sql = String.Format("delete from  MyAdvantechGlobal.dbo.order_detail where order_id='{0}'", UID)
                                        'dbUtil.dbExecuteNoQuery("B2B", sql.ToString())
                                        lbMsg.Text = rPn & " is an invalid part no."
                                        lbOrderMsg.Text = "Part Number:" + rPn + " is an invalid part no."
                                        Exit Sub
                                    End If
                                End If
                            End If
                        Next
                        g_adoConn.Close() : g_adoConn = Nothing
                        'Response.End()
                        If OptyId <> "" Then
                            Dim sb2 As New System.Text.StringBuilder
                            With sb2
                                .AppendLine(String.Format(" update MyAdvantechGlobal.dbo.order_detail set order_detail.optyid='{0}' where order_id='{1}'", OptyId, UID))
                            End With
                            '        dbUtil.dbExecuteNoQuery("B2B", sb2.ToString())
                            Session("Optyid") = OptyId
                        End If
                        Dim exeFunc As Integer = 0
                        '     exeFunc = OrderUtilities.OrderXML_Create("SO", UID, Session("ORG_ID"))
                        '      exeFunc = OrderUtilities.ERPOrder_Integrate("SO", OrderNo)
                        '       exeFunc = OrderUtilities.SendPI(OrderNo, "PI", "")
                        If exeFunc = 1 Then
                            '   dbUtil.dbExecuteNoQuery("b2b", String.Format("update oracleOrderNum set isSuccess=1 where order_id='{0}'", UID))
                        End If
                        If OptyId <> "" And exeFunc = 1 Then
                            Dim OPTYrevenue As Decimal = 0.0
                            '     OPTYrevenue = dbUtil.dbExecuteScalar("B2B", "select SUM(QTY * UNIT_PRICE) from order_DETAIL where order_id = '" & Trim(UID) & "'")
                            '       OptyId = dbUtil.dbExecuteScalar("B2B", "select TOP 1 OPTYID from order_DETAIL where order_id = '" & Trim(UID) & "'")
                            '         Dim ws As New aeu_eai2000.Siebel_WS
                            '       ws.Timeout = -1
                            '      ws.UseDefaultCredentials = True
                            Dim b As Boolean = False
                            Try
                                '        b = ws.UpdateOpportunityStatusRevenue(OptyId, "Won", OPTYrevenue, False)
                            Catch ex As Exception
                                '   Util.SendEmail("ebusiness.aeu@advantech.eu", "ebiz.aeu@advantech.eu", _
                                '                    String.Format("Update Opty to Won for SO:{0} OptyID:{1}", OrderNo, OptyId), ex.ToString(), True, "", "")
                            End Try
                            Session("Optyid") = ""
                            Session("Optyid") = Nothing
                            'If Session("user_id").ToString.ToLower.Contains("nada.liu") Then
                            '    Response.Write(OPTYrevenue & OPTYID) : Response.End()
                            'End If
                        End If
                        Dim flgOrderExist As String = "No"
                        'Response.Redirect("~/order/Order_Confirm_V6.aspx?flag=" & flgOrderExist & "&order_no=" & OrderNo & "&order_id=" & UID)
                        '  Util.AjaxRedirect(upOrderMsg, "../order/Order_Confirm_V6.aspx?flag=" & flgOrderExist & "&order_no=" & OrderNo & "&order_id=" & UID)

                    Else
                        lbMsg.Text = "ERPID is not correctly maintained for this quote-to account in SIEBEL"
                    End If
                Else
                    lbMsg.Text = "ERPID is not maintained for this quote-to account in SIEBEL"
                End If
            End If
        End If
    End Sub

    Protected Sub btnQuote2Ordernew_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lbPOPMsg.Text = ""
        ' src1.SelectCommand = GetQuoteLinesSql(hd_Qid.Value)
        If Request("QUOTEID") IsNot Nothing Then

            Dim _dt As DataTable = GetQuoteHeader(Request("QUOTEID"))
            If _dt Is Nothing OrElse _dt.Rows.Count = 0 Then
                lbPOPMsg.Text = String.Format("Request quotation cannot be found")
                Exit Sub
            End If

            If String.IsNullOrEmpty(txtOptyRowID.Text.Trim) Then
                lbPOPMsg.Text = String.Format("Please specify an opportunity for this quote.")
                Exit Sub
            End If
            Dim qid As String = Trim(Request("QUOTEID")).Replace("'", "")
            Dim sb As New System.Text.StringBuilder
            Dim qmDt As DataTable = GetQuoteHeader(qid)
            If qmDt IsNot Nothing AndAlso qmDt.Rows.Count = 1 Then
                Dim _contact_email As String = qmDt.Rows(0).Item("Contact_Email").ToString()
                Dim _sales_person As String = qmDt.Rows(0).Item("SalesName").ToString()
                Dim _Sales_Rep As String = qmDt.Rows(0).Item("Sales_Rep").ToString.Trim
                Dim QuoteToERPId As String = qmDt.Rows(0).Item("ERPID").ToString().ToUpper().Trim().Replace("'", "")
                If QuoteToERPId <> "" Then
                    If Business.is_Valid_Company_Id(QuoteToERPId) = False Then
                        'lbPOPMsg.Text = String.Format("Please click <a href=""../Admin/SyncCustomer.aspx?companyid={0}"" target=""_blank"" style=""text-decoration: underline;""><strong style=""color:#FF0000"">here</strong></a> to synchronize company id({0}) From SAP to MyAdvantech", QuoteToERPId.ToString.Trim)
                        lbPOPMsg.Text = String.Format("ErpId " + QuoteToERPId + " is invalid, Please click <a href=""http://my.advantech.com/Admin/SyncCustomer.aspx?companyid={0}"" target=""_blank"" style=""text-decoration: underline;""><strong style=""color:#FF0000"">here</strong></a> to synchronize company id({0}) From SAP to MyAdvantech", QuoteToERPId.ToString.Trim)
                        Exit Sub
                    End If
                    ' Dim ERPIDInfDt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD",    String.Format("select kunnr, name1 from saprdp.kna1 where mandt='168' and kunnr='{0}'", QuoteToERPId))
                    'Dim qiDt As DataTable = GetQuoteLines(qid)                                     
                    'If ERPIDInfDt.Rows.Count = 1 Then
                    If 1 = 1 Then
                        'Me.chgCompany.TargetCompanyId = QuoteToERPId
                        'Me.chgCompany.ChangeToCompanyId()
                        'Dim Istrue As Boolean = Me.chgCompany.ChangeToCompanyId()
                        'If Istrue = False Then
                        '    lbMsg.Text = ""
                        '    Exit Sub
                        'End If

                        'JJ 2014/3/18：檢查Product是否存在及不為Phase Out
                        Dim _orgid As String = tbOPBase.dbExecuteScalar("MY", "select top 1 ORG_ID   from  SAP_DIMCOMPANY  where COMPANY_ID='" + QuoteToERPId.Trim + "'")
                        Dim IsHavePhaseout As Boolean = False
                        Dim _ProductList As New List(Of SAPDAL.ProductX)
                        Dim _GVcheckbox As New List(Of Boolean)
                        Dim _ProductX As New SAPDAL.ProductX()
                        For Each r As GridViewRow In gvItems.Rows
                            If r.RowType = Web.UI.WebControls.DataControlRowType.DataRow Then
                                Dim chk As CheckBox = r.FindControl("item")
                                _GVcheckbox.Add(chk.Checked)
                                If chk.Checked Then
                                    Dim rPn As String = CType(r.FindControl("txtRowPN"), TextBox).Text.Trim()
                                    _ProductList.Add(New SAPDAL.ProductX(rPn, _orgid, ""))
                                End If
                            End If
                        Next
                        _ProductList = _ProductX.GetProductInfo(_ProductList, _orgid, IsHavePhaseout)

                        'JJ 2014/3/18：IsHavePhaseout=true表示Product List中有料號有問題
                        '必須存在Session中是因為顯示Error是在GridView中的Row中，所以是要在RowDataBound中做
                        '所以用Session帶過去，如果沒問題就清空Session 
                        If IsHavePhaseout Then
                            Session("Product_Err") = _ProductList
                            Session("GV_checkbox") = _GVcheckbox
                            listR = 0
                            chkR = 0
                            ' gvItems.DataBind()
                            Bind()
                            Exit Sub
                        Else
                            Session.Contents.Remove("Product_Err")
                            Session.Contents.Remove("GV_checkbox")
                            listR = 0
                            chkR = 0
                        End If

                        Dim strcartId As String = "", i As Integer = 0 'Session("CART_ID")
                        '      Dim mycart As New CartList("b2b", "cart_detail_V2")
                        Dim objcontext As New SiebelQuoteDBDataContext()
                        Dim _QuoteMaster As New SiebelQuoteMaster
                        _QuoteMaster.QuoteRowid = lbQuoteNum.Text.Trim
                        _QuoteMaster.AccountRowid = Labaccountrowid.Text.Trim
                        _QuoteMaster.AccountErpid = lbERPID.Text.Trim
                        _QuoteMaster.OptyID = txtOptyRowID.Text.Trim
                        _QuoteMaster.OptyName = txtOptyName.Text.Trim
                        _QuoteMaster.OptyStage = DDLOptyStage.SelectedValue.Trim
                        _QuoteMaster.OptyAmount = 0
                        If Decimal.TryParse(lbTotal.Text.Trim, 0) Then
                            _QuoteMaster.OptyAmount = Decimal.Parse(lbTotal.Text.Trim)
                        End If
                        _QuoteMaster.CreateBy = Pivot.CurrentProfile.UserId
                        _QuoteMaster.CreateTime = Now
                        objcontext.SiebelQuoteMasters.InsertOnSubmit(_QuoteMaster)
                        objcontext.SubmitChanges()
                        ' MyCartX.DeleteCartAllItem(strcartId)
                        Dim _SiebelQuoteDetail As New List(Of SiebelQuoteDetail)
                        Dim Type_int As Integer = 0
                        Dim _higherLevel As Integer = 0, _isUpdatePrice As Integer = 1, _ParentItemQty As Integer = 1, _Line_NO = 1
                        Dim _IsBto As Boolean = False, _ParentItemLineNumber As Integer = 0
                        For Each r As GridViewRow In gvItems.Rows

                            If r.RowType = Web.UI.WebControls.DataControlRowType.DataRow Then
                                _IsBto = False
                                Dim chk As CheckBox = r.FindControl("item")
                                If chk.Checked Then
                                    Dim rPn As String = CType(r.FindControl("txtRowPN"), TextBox).Text.Trim()
                                    Dim rLp As Decimal = 0
                                    If Decimal.TryParse(r.Cells(8).Text, 0) Then
                                        rLp = CDbl(r.Cells(8).Text)
                                    End If
                                    Dim rUp As Decimal = 0
                                    If CType(r.FindControl("txtPrice"), TextBox).Text.Trim() = "" Then
                                        rUp = 0
                                    Else
                                        rUp = CDec(CType(r.FindControl("txtPrice"), TextBox).Text.Trim())
                                    End If

                                    Dim rQty As Integer = 1
                                    Dim tbQty As String = CType(r.FindControl("txtQty"), TextBox).Text.Trim()
                                    If Integer.TryParse(tbQty, 0) AndAlso Integer.Parse(tbQty) > 0 Then
                                        rQty = Integer.Parse(tbQty)
                                    End If
                                    Dim rDueDate As DateTime = CDate(CType(r.FindControl("txtDueDate"), TextBox).Text.Trim())
                                    Dim _Description As String = r.Cells(4).Text.Trim
                                    'lbMsg.Text = r.Cells(13).Text.Trim.ToString
                                    'Exit Sub
                                    Dim rDataKey As Integer = CInt(r.Cells(13).Text.Trim.ToString) 'gvItems.DataKeys(r.RowIndex).Values(0).ToString                                 

                                    _IsBto = IsBtoOrder(rPn)

                                    'If IsBtoOrder(rPn) Then
                                    If _IsBto Then
                                        Type_int = -1
                                        _ParentItemLineNumber = _ParentItemLineNumber + 100
                                        _Line_NO = _ParentItemLineNumber
                                        '_Line_NO = 100
                                        '_higherLevel = MyCartX.getBtosParentLineNo(strcartId)
                                    End If
                                    'If Type_int = -1 AndAlso Not IsBtoOrder(rPn) Then
                                    If Type_int = -1 AndAlso Not _IsBto Then
                                        Type_int = 1
                                    End If
                                    If Type_int = -1 Then
                                        _higherLevel = 0 : _ParentItemQty = rQty
                                    End If
                                    If Type_int = 1 Then
                                        '_higherLevel = 100 : rQty = rQty * _ParentItemQty
                                        _higherLevel = _ParentItemLineNumber : rQty = rQty * _ParentItemQty
                                    End If
                                    'If Type_int = CartItemType.BtosPart Then
                                    '    _isUpdatePrice = 0
                                    'End If
                                    'rPn.StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase) OrElse
                                    If String.Equals(rPn.Trim, "T", StringComparison.CurrentCultureIgnoreCase) Then
                                        Continue For
                                    End If
                                    Dim _item As SiebelQuoteDetail = New SiebelQuoteDetail()
                                    _item.ItemType = Type_int
                                    _item.PartNO = rPn
                                    _item.QTY = rQty
                                    _item.UnitPrice = rUp
                                    _item.Description = _Description
                                    _item.Line_NO = _Line_NO
                                    _item.HigherLevel = _higherLevel
                                    _item.MasterID = _QuoteMaster.id
                                    _SiebelQuoteDetail.Add(_item)
                                    'Dim line_no As Integer = mycart.ADD2CART_V2(strcartId, rPn, rQty, 0, Type_int, "", _isUpdatePrice, 0, rDueDate, _Description, "", _higherLevel, False)
                                    '  Dim line_no As Integer = MyCartOrderBizDAL.Add2Cart_BIZ(strcartId, rPn, rQty, 0, Type_int, "", _isUpdatePrice, 0, rDueDate, _Description, "", _higherLevel, False)

                                    ' mycart.Update(String.Format("cart_id='{0}' and line_no='{1}'", strcartId, line_no), String.Format("list_price='{0}',unit_price='{1}'", rLp, rUp))
                                    'If Decimal.TryParse(rUp, 0) AndAlso Decimal.Parse(rUp) > 0 AndAlso Type_int = CartItemType.Part Then
                                    '    mycart.Update(String.Format("cart_id='{0}' and line_no='{1}'", strcartId, line_no), String.Format("unit_price='{0}'", rUp))
                                    'End If
                                    _Line_NO = _Line_NO + 1
                                End If
                            End If
                        Next
                        objcontext.SiebelQuoteDetails.InsertAllOnSubmit(_SiebelQuoteDetail)
                        objcontext.SubmitChanges()

                        'ICC 2016/1/14 This method of creating Siebel opportunity is out of date, so we change to web job function to replace it.
                        Business.transQuote2Siebel(qid)
                        'If String.Equals(txtOptyRowID.Text.Trim, "NEW ID", StringComparison.CurrentCultureIgnoreCase) Then
                        '    Dim eCovWs As New eCovWS.WSSiebel, emp As New eCovWS.EMPLOYEE, opty As New eCovWS.OPPTY
                        '    emp.USER_ID = ConfigurationManager.AppSettings("CRMHQId") : emp.PASSWORD = ConfigurationManager.AppSettings("CRMHQPwd")
                        '    With opty
                        '        .ACC_ROW_ID = _QuoteMaster.AccountRowid : .CLOSE_DATE = DateAdd(DateInterval.Month, 1, Now)
                        '        .CURRENCY_CODE = lbCurr.Text.Trim : .DESP = _QuoteMaster.OptyName : .ORG = "ATW" ': .OWNER_EMAIL = _QuoteMaster.CreateBy
                        '        .PROJ_NAME = _QuoteMaster.OptyName : .SALES_METHOD = "Funnel Sales Methodology" : .SALES_STAGE = _QuoteMaster.OptyStage
                        '        ' .REVENUE = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, quoteId, Pivot.CurrentProfile.getCurrOrg).GetListAll(COMM.Fixer.eDocType.EQ).getTotalAmount
                        '        .REVENUE = 0
                        '        If Decimal.TryParse(lbTotal.Text.Trim, 0) Then
                        '            .REVENUE = Decimal.Parse(lbTotal.Text.Trim) \ 1
                        '        End If
                        '        '.CON_ROW_ID = SiebelTools.GET_ContactRowID_by_Email(_QuoteMaster.CreateBy)
                        '        .CON_ROW_ID = SiebelTools.GET_CustomerContactRowID_by_Email(_contact_email)
                        '        .OWNER_EMAIL = SiebelTools.GeteMailByLocalName(_sales_person)
                        '        If String.IsNullOrEmpty(.OWNER_EMAIL) AndAlso Not String.IsNullOrEmpty(_Sales_Rep) Then
                        '            .OWNER_EMAIL = _Sales_Rep
                        '        End If
                        '        .SRC_ID = ""
                        '    End With
                        '    Dim res As eCovWS.RESULT = Nothing
                        '    Try
                        '        'If Not COMM.Util.IsTesting() Then
                        '        res = eCovWs.AddOppty(emp, opty)
                        '        ' End If
                        '        Dim MessageStr As String = ""
                        '        Dim MailSubject As String = "Creating New Opportunity by SiebelQuote"
                        '        If Not IsNothing(res) Then
                        '            If String.IsNullOrEmpty(res.ERR_MSG) Then
                        '                MailSubject &= "(Success)"
                        '            Else
                        '                MailSubject &= "(Failed!)"
                        '            End If
                        '            MessageStr &= "Return OptyRowId:" & res.ROW_ID & "<br>"
                        '            MessageStr &= "Return Error Message:<font color='red'>" & res.ERR_MSG & "</font><br>"
                        '            MessageStr &= "================================" & "<br>"
                        '            MessageStr &= "Call Method:eCovWs.AddOppty" & "<br>"
                        '            MessageStr &= "OPPTY.PROJ_NAME =" & opty.PROJ_NAME & "<br>"
                        '            MessageStr &= "OPPTY.SALES_STAGE =" & opty.SALES_STAGE & "<br>"
                        '            MessageStr &= "OPPTY.ACC_ROW_ID =" & opty.ACC_ROW_ID & "<br>"
                        '            MessageStr &= "OPPTY.CLOSE_DATE =" & opty.CLOSE_DATE & "<br>"
                        '            MessageStr &= "OPPTY.CURRENCY_CODE =" & opty.CURRENCY_CODE & "<br>"
                        '            MessageStr &= "OPPTY.DESP =" & opty.DESP & "<br>"
                        '            MessageStr &= "OPPTY.ORG =" & opty.ORG & "<br>"
                        '            MessageStr &= "OPPTY.OWNER_EMAIL =" & opty.OWNER_EMAIL & "<br>"
                        '            MessageStr &= "OPPTY.SALES_METHOD =" & opty.SALES_METHOD & "<br>"
                        '            MessageStr &= "OPPTY.CON_ROW_ID =" & opty.CON_ROW_ID & "<br>"
                        '            MessageStr &= "OPPTY.SRC_ID =" & opty.SRC_ID & "<br>"
                        '            Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", MailSubject, "", MessageStr, "")
                        '        End If
                        '    Catch ex As Exception
                        '        Dim MailSubject As String = "Creating New Opportunity by SiebelQuote Faild: " + lbQuoteNum.Text.Trim
                        '        Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", MailSubject, "", ex.ToString, "")
                        '    End Try
                        '    If res IsNot Nothing AndAlso Not String.Equals(res.ROW_ID.Trim, "NEW ID", StringComparison.CurrentCultureIgnoreCase) AndAlso Not String.IsNullOrEmpty(res.ROW_ID) Then
                        '        _QuoteMaster.OptyID = res.ROW_ID.Trim
                        '        objcontext.SubmitChanges()
                        '    End If

                        'End If

                        'Dim _CartMaster As New CartMaster
                        '_CartMaster.CartID = strcartId
                        '_CartMaster.ErpID = QuoteToERPId
                        '_CartMaster.CreatedDate = Now
                        '_CartMaster.QuoteID = qid
                        '_CartMaster.Currency = Session("COMPANY_CURRENCY")
                        '_CartMaster.CreatedBy = Session("user_id")
                        '_CartMaster.LastUpdatedDate = Now
                        '_CartMaster.LastUpdatedBy = Session("user_id")
                        'If qmDt.Rows(0).Item("OPTY_ID").ToString <> "" Then
                        '    Session("OptyId") = qmDt.Rows(0).Item("OPTY_ID").ToString
                        '    _CartMaster.OpportunityID = qmDt.Rows(0).Item("OPTY_ID").ToString
                        'End If
                        'MyCartX.LogCartMaster(_CartMaster)
                        'Response.Redirect("/Order/Cart_List.aspx")
                        'Dim dtsalecode As DataTable = SAPDOC.GetKeyInPersonV2(Session("user_id").ToString.Trim)
                        'If dtsalecode.Rows.Count > 0 Then
                        '    For Each r As DataRow In dtsalecode.Rows
                        '        If r.Item("SALES_CODE") IsNot Nothing AndAlso Not String.IsNullOrEmpty(r.Item("SALES_CODE")) Then
                        '            txtERE.Text = r.Item("SALES_CODE").ToString.Trim
                        '            Exit For
                        '        End If
                        '    Next
                        'End If
                        'Response.Redirect(String.Format("~/Order/OrderInfoV2.aspx?PAR1={0}", txtERE.Text.Trim))
                        'Response.Redirect("~/Order/Cart_ListV2.aspx")
                        Dim RURL As String = HttpContext.Current.Server.UrlEncode(String.Format("/Order/SiebelQuote2Cart.aspx?QuoteID={0}", _QuoteMaster.id)) ' Request.Url.AbsoluteUri 
                        Dim strMyAdvantechUrl As String = "http://my.advantech.com"
                        If Util.GetRuntimeSiteUrl.Contains("localhost") Then strMyAdvantechUrl = "http://localhost"
                        If COMM.Util.IsTesting() Then strMyAdvantechUrl += ":4002"
                        Response.Redirect(String.Format(strMyAdvantechUrl + "/ORDER/SSOENTER.ASPX?ID={0}&USER={1}&COMPANY={2}&RURL={3}&ORG={4}", Pivot.CurrentProfile.SSOID, Pivot.CurrentProfile.UserId, HttpUtility.UrlEncode(_QuoteMaster.AccountErpid), RURL, _orgid))
                    Else
                        lbPOPMsg.Text = "ERPID is not correctly maintained for this quote-to account in SIEBEL"
                    End If
                Else
                    lbPOPMsg.Text = "ERPID is not maintained for this quote-to account in SIEBEL"
                End If
            End If
        End If
    End Sub
    Public Function Add2CartCheck(ByVal part_no As String, ByVal QuoteToERPId As String) As Boolean
        Dim fdt As DataTable = tbOPBase.dbGetDataTable("MY", String.Format("select a.PART_NO, a.PRODUCT_STATUS  from SAP_PRODUCT_STATUS a inner join SAP_DIMCOMPANY b on a.SALES_ORG=b.ORG_ID  where a.PART_NO='{1}' and b.COMPANY_ID='{0}' and a.PRODUCT_STATUS in ('A','N','H'.'M1')", QuoteToERPId, part_no))
        If fdt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    'Protected Sub pickERE_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Me.ascxPickERE.ShowData("")
    '    Me.UPPickERE.Update()
    '    Me.MPPickERE.Show()
    'End Sub

    Public Sub PickEREEnd(ByVal str As Object)
        Dim KEY As String = str.ToString
        Me.txtERE.Text = KEY
        Me.UPPickFm.Update()
        'Me.MPPickERE.Hide()
    End Sub

    Protected Sub txtQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim t As TextBox = CType(sender, TextBox)

        Dim drv As GridViewRow = CType(t.NamingContainer, GridViewRow)
        Dim rowIndex As Integer = drv.RowIndex

        Dim partNo As String = CType(gvItems.Rows(rowIndex).FindControl("txtRowPN"), TextBox).Text

        'JJ 2014/3/19：判斷是不是parent part no
        Dim checkPN As DataTable = tbOPBase.dbGetDataTable("B2B", String.Format("select count(material_group) from sap_product where part_no='{0}' and material_group in ('BTOS','CTOS')", partNo))

        'JJ 2014/3/19：大於0就是parent part no，但數量必須不為空白
        If checkPN.Rows(0)(0) > 0 AndAlso t.Text <> "" Then
            If t.Text <> "" Then
                hf_qty.Value = t.Text
            End If

            Dim dt As DataTable = GetQuoteLines(hd_Qid.Value)

            Dim rint As Integer = 0
            For Each r As GridViewRow In gvItems.Rows

                If r.RowType = Web.UI.WebControls.DataControlRowType.DataRow Then
                    If Not CType(r.Cells(2).FindControl("txtRowPN"), TextBox).Text = CType(drv.Cells(2).FindControl("txtRowPN"), TextBox).Text Then
                        'CType(r.Cells(7).FindControl("txtQty"), TextBox).Text = CStr(CType(CType(r.Cells(7).FindControl("txtQty"), TextBox).Text, Integer) * t1)
                        CType(r.Cells(7).FindControl("txtQty"), TextBox).Text = CStr(CType(dt.Rows(rint)(7), Integer) * CInt(hf_qty.Value))
                    End If
                    rint += 1
                End If
            Next

        End If
    End Sub

    <Services.WebMethod()> _
    <Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetSIEBELQuotes(ByVal QuoteName As String, ByVal AccountName As String) As String
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" select top 100 a.ROW_ID, a.QUOTE_NUM, a.TARGET_OU_ID as ACCOUNT_ROW_ID, b.NAME as ACCOUNT_NAME, IsNull(c.ATTRIB_05,'') as ERPID, "))
            .AppendLine(String.Format(" IsNull((select cast(sum(z.QTY_REQ*z.NET_PRI) as numeric(18,2)) from S_QUOTE_ITEM z where z.SD_ID=a.ROW_ID),0) as QUOTE_SUM, "))
            .AppendLine(String.Format(" a.NAME, IsNull(a.STATUS_DT,'') as QUOTE_STATUS,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.FST_NAME from S_CONTACT z where z.ROW_ID=a.CON_PER_ID),'') as First_Name,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.LAST_NAME from S_CONTACT z where z.ROW_ID=a.CON_PER_ID),'') as Last_Name,   "))
            .AppendLine(String.Format(" IsNull((select top 1 z.EMAIL_ADDR from S_CONTACT z where z.ROW_ID=a.CON_PER_ID),'') as Contact_Email,  "))
            .AppendLine(String.Format(" a.CURCY_CD as Currency,  "))
            .AppendLine(String.Format(" a.EFF_START_DT as Effective_Date,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.NAME from S_OPTY z where z.ROW_ID=a.OPTY_ID),'') as OPTY_NAME, a.REV_NUM, a.OPTY_ID,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.EMAIL_ADDR from S_CONTACT z where z.ROW_ID in (select z2.PR_EMP_ID from S_POSTN z2 where z2.ROW_ID=a.SALES_REP_POSTN_ID)),'') as Sales_Rep,  "))
            .AppendLine(String.Format(" a.CREATED, a.DESC_TEXT as QUOTE_DESC, a.DUE_DT, a.EFF_END_DT, a.ACTIVE_FLG, "))
            .AppendLine(String.Format(" a.CREATED_BY, a.SALES_REP_POSTN_ID as OWNER_ID, a.LAST_UPD "))
            .AppendLine(String.Format(" from S_DOC_QUOTE a inner join S_ORG_EXT b on a.TARGET_OU_ID=b.ROW_ID  inner join S_ORG_EXT_X c on b.ROW_ID=c.ROW_ID	"))
            '.AppendLine(" where ( Upper(a.CREATED_BY) IN (select ROW_ID  from  S_USER  WHERE (Upper(LOGIN) = 'MTL' OR Upper(LOGIN) ='MYADVANTECH'))	OR 	 a.CREATED <'2015-07-01') ")
            .AppendLine(" where a.CREATED <'" & _BlockSearchSiebelQuoteByCreatedDate & "' ")
            If QuoteName.Trim <> "" Then
                .AppendLine(String.Format(" and Upper(a.NAME) like N'%{0}%' ", QuoteName.ToUpper.Trim().Replace("'", "''").Replace("*", "%")))
            End If
            If AccountName.Trim <> "" Then
                .AppendLine(String.Format(" and Upper(b.NAME) like N'%{0}%' ", AccountName.ToUpper.Trim().Replace("'", "''").Replace("*", "%")))
            End If
            .AppendLine(" order by a.CREATED desc")
        End With
        Dim dt As DataTable = tbOPBase.dbGetDataTable("CRM", sb.ToString())
        If dt.Rows.Count = 0 Then
            Return "No matched Quotation"
        End If
        sb = New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format("<table width='100%'>"))
            .AppendLine(String.Format("<tr><th style='text-align:left' width='20%'>Quotation Name</th>"))
            .AppendLine(String.Format("<th style='text-align:center' width='10%'>Rev No.</th>"))
            .AppendLine(String.Format("<th style='text-align:left' width='30%'>Account Name</th>"))
            .AppendLine(String.Format("<th style='text-align:left' width='20%'>Created date</th>"))
            .AppendLine(String.Format("<th style='text-align:left' width='20%'>Last Updated date</th></tr>"))
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim r As DataRow = dt.Rows(i)
                Dim bcolor As String = "FEFEFE"
                If i Mod 2 = 1 Then bcolor = "DCDBDB"
                .AppendLine(String.Format("<tr style='background-color:#" + bcolor + ";'>" + _
                                          " <td>" + _
                                          "     <a href='SiebelQuote.aspx?QUOTEID={0}'>{1}</a>" + _
                                          " </td>" + _
                                          " <td style='text-align:center'>{2}</td>" + _
                                          " <td>{3}</td>" + _
                                          " <td>{4}</td>" + _
                                          " <td>{5}</td>" + _
                                          "</tr>", r.Item("ROW_ID"), r.Item("NAME"), CInt(r.Item("REV_NUM")), r.Item("ACCOUNT_NAME"), r.Item("CREATED"), r.Item("LAST_UPD")))
            Next
            .AppendLine(String.Format("</table>"))
        End With
        Return sb.ToString()
    End Function
    Function GetQuoteLinesSql(ByVal QuoteId As String) As String
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" SELECT a.LN_NUM, b.NAME AS PART_NO, b.DESC_TEXT, IsNull(cast(IsNull(a.BASE_UNIT_PRI,a.UNIT_PRI) as numeric(18,2)),0) as START_PRICE,  "))
            'Nada20140102 for Show's request changed Qty to Req Qty ,Price to Net Price
            '.AppendLine(String.Format(" IsNull(cast(IsNull(a.NET_PRI,a.BASE_UNIT_PRI) as numeric(18,2)),0) as DISCOUNT_PRICE, "))
            .AppendLine(String.Format(" (case when ISNUMERIC(c.ATTRIB_15) = 1 then CAST(c.ATTRIB_15 as numeric(18,2)) else 0 end) as DISCOUNT_PRICE, "))
            '/Nada20140102
            .AppendLine(String.Format(" case  CONVERT(int, IsNull(a.BASE_UNIT_PRI ,0)) when 0 then CONVERT(varchar(10),0.0)+'%' else "))
            .AppendLine(String.Format(" cast(IsNull(cast((a.BASE_UNIT_PRI-IsNull(a.UNIT_PRI,a.BASE_UNIT_PRI))/a.BASE_UNIT_PRI*100 as numeric(18,2)),0.0) as varchar(10))+'%' end as DISC, "))
            'Nada20140102 for Show's request changed Qty to Req Qty ,Price to Net Price
            '.AppendLine(String.Format(" cast(a.QTY_REQ as int) as QTY_REQ, cast(a.EXTD_QTY as int) as EXTD_QTY, "))
            .AppendLine(String.Format(" (case when ISNUMERIC(c.ATTRIB_04)=1 THEN CAST(c.ATTRIB_04 as int) ELSE 1 END) as QTY_REQ, cast(a.EXTD_QTY as int) as EXTD_QTY, "))
            '/Nada20140102
            .AppendLine(String.Format(" IsNull(c.ATTRIB_03,'') as Product_Rpt,IsNull(c.ATTRIB_05,'') as ItemMark, IsNull(c.ATTRIB_47,'') as Description_Rpt,convert(varchar(14),a.QUOTE_ITM_EXCH_DT,111) as duedate "))
            .AppendLine(String.Format(" FROM S_QUOTE_ITEM AS a INNER JOIN S_PROD_INT AS b ON a.PROD_ID = b.ROW_ID inner join S_QUOTE_ITEM_X c on a.ROW_ID=c.ROW_ID "))
            .AppendLine(String.Format(" WHERE a.SD_ID = '{0}' ORDER BY a.LN_NUM", QuoteId))
        End With
        Return sb.ToString()
    End Function
    Function GetQuoteLines(ByVal QuoteId As String) As DataTable
        Dim qiDt As DataTable = Nothing
        For i As Integer = 1 To 3
            Try
                qiDt = tbOPBase.dbGetDataTable("CRM", GetQuoteLinesSql(QuoteId))
                Exit For
            Catch ex As System.Data.SqlClient.SqlException
                Threading.Thread.Sleep(500)
            End Try
        Next
        Return qiDt
    End Function

    Function GetQuoteHeader(ByVal QuoteId As String) As System.Data.DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" select a.QUOTE_NUM, a.TARGET_OU_ID as ACCOUNT_ROW_ID, b.NAME as ACCOUNT_NAME, IsNull(c.ATTRIB_05,'') as ERPID, "))
            .AppendLine(String.Format(" IsNull((select cast(sum(z.QTY_REQ*z.NET_PRI) as numeric(18,2)) from S_QUOTE_ITEM z where z.SD_ID=a.ROW_ID),0) as QUOTE_SUM, "))
            .AppendLine(String.Format(" a.NAME, IsNull(a.STATUS_DT,'') as QUOTE_STATUS,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.FST_NAME from S_CONTACT z where z.ROW_ID=a.CON_PER_ID),'') as First_Name,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.LAST_NAME from S_CONTACT z where z.ROW_ID=a.CON_PER_ID),'') as Last_Name,   "))
            .AppendLine(String.Format(" IsNull((select top 1 z.EMAIL_ADDR from S_CONTACT z where z.ROW_ID=a.CON_PER_ID),'') as Contact_Email,  "))
            .AppendLine("  ISNULL((SELECT top 1 T.ATTRIB_04 FROM S_DOC_QUOTE_X T where T.ROW_ID=a.ROW_ID),'') as SalesName,  ")
            .AppendLine(String.Format(" a.CURCY_CD as Currency,  "))
            .AppendLine(String.Format(" IsNull(a.EFF_START_DT,GetDate()) as Effective_Date,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.NAME from S_OPTY z where z.ROW_ID=a.OPTY_ID),'') as OPTY_NAME, a.OPTY_ID,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.CURR_STG_ID from S_OPTY z where z.ROW_ID=a.OPTY_ID),'') as Sales_Stage_ID,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.EMAIL_ADDR from S_CONTACT z where z.ROW_ID in (select z2.PR_EMP_ID from S_POSTN z2 where z2.ROW_ID=a.SALES_REP_POSTN_ID)),'') as Sales_Rep,  "))
            .AppendLine(String.Format(" a.CREATED, a.DESC_TEXT as QUOTE_DESC,IsNull(a.DUE_DT,GetDate()) as DUE_DT,a.EFF_END_DT, a.ACTIVE_FLG, "))
            .AppendLine(String.Format(" a.CREATED_BY, a.SALES_REP_POSTN_ID as OWNER_ID "))
            .AppendLine(String.Format(" from S_DOC_QUOTE a inner join S_ORG_EXT b on a.TARGET_OU_ID=b.ROW_ID  inner join S_ORG_EXT_X c on b.ROW_ID=c.ROW_ID	"))
            .AppendLine(String.Format(" where a.ROW_ID='{0}' and a.CREATED <'" & _BlockSiebelQuoteCreatedDate & "' ", QuoteId))
        End With
        Dim qmDt As DataTable = Nothing
        For i As Integer = 1 To 3
            Try
                qmDt = tbOPBase.dbGetDataTable("CRM", sb.ToString())
                Exit For
            Catch ex As System.Data.SqlClient.SqlException
                Threading.Thread.Sleep(500)
            End Try
        Next
        Return qmDt
    End Function

    Private Sub btnQuote2eQ_Click(sender As Object, e As EventArgs) Handles btnQuote2eQ.Click
        If Request("QUOTEID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request("QUOTEID")) Then
            Dim _dt As DataTable = GetQuoteHeader(Request("QUOTEID"))
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                Dim Quoteid As String = ""
                SiebelTools.CopySiebelQuote2eQuotation(Request("QUOTEID"), Quoteid)
                Response.Redirect("~/Quote/QuotationMaster.aspx?UID=" + Quoteid)
            Else
                lbPOPMsg.Text = String.Format("Request quotation cannot be found")
                Exit Sub
            End If
        End If
    End Sub
End Class