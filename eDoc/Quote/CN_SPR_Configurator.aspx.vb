Public Class CN_SPR_Configurator
    Inherits System.Web.UI.Page

    Public CBOMWS As New CBOMWS.MyCBOMDAL
    Public strOrg As String = "CN10", strSiebelRBU As String = "ABJ", strErpId As String = "C100001", strCurrency As String = "CNY"
    Protected Sub btnNext1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim NotSelCtrl As HierarchyConfigUI = Nothing
        Dim ConfigDt As DataTable = GetConfigDt(NotSelCtrl)
        If NotSelCtrl IsNot Nothing Then
            FlyOutTargetClientId.Value = NotSelCtrl.lbCategoryClientID
        Else
            If save2Cart(ConfigDt) = True Then
                gvConfiguredItems.DataSource = ConfigDt : gvConfiguredItems.DataBind()
                hyBackToSPR.NavigateUrl = "http://employeezone.advantech.com.tw/ToSPR=" + Request("SPRNUM")
                Dim cmd As New SqlClient.SqlCommand("update QuotationMaster set CustomId=@SPRNUM where quoteid=@QID", _
                                                    New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString))
                cmd.Parameters.AddWithValue("SPRNUM", Request("SPRNUM"))
                cmd.Parameters.AddWithValue("QID", Request("UID"))
                cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
                PanelDone.Visible = True : ph1.Visible = False : panel2.Visible = False
                '    initInterFace(Request("UID")) : MultiView1.ActiveViewIndex = 0
            End If
        End If
    End Sub

    Function save2Cart(ByVal DTCOM As DataTable) As Boolean
        Dim aptQD As New EQDSTableAdapters.QuotationDetailTableAdapter
        Dim dtQM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.ORDER)
        If Not IsNothing(dtQM) Then
            Dim UID As String = dtQM.Key, company As String = dtQM.AccRowId
            Dim plant As String = Business.getPlantByOrgID(dtQM.org), ORG As String = dtQM.org
            Dim RBU As String = dtQM.AccOfficeName, ewFLAG As Integer = 0, Currency As String = dtQM.currency
            Dim strBTORootItem As String = Request("BTOITEM").ToUpper, intReqQty As Integer = 1
            If Integer.TryParse(Request("QTY"), 1) Then intReqQty = CInt(Request("QTY"))
            aptQD.DeleteQuoteDetailById(Request("UID"))
            tbOPBase.adddblog("delete from quotationDetail where quoteid='" & Request("UID") & "'")
            Dim intParentLineNo As Integer = 0
            Dim EWROW() As DataRow = DTCOM.Select("CATEGORY_ID LIKE 'AGS-EW%'")
            If EWROW.Length > 0 Then
                ewFLAG = Business.getMonthByEWItem(EWROW(0).Item("CATEGORY_ID").ToString.ToUpper())
            Else
                ewFLAG = 0
            End If
            'Business.ADD2QUOTE_V2(UID, strBTORootItem, intReqQty, ewFLAG, COMM.Fixer.eItemType.Parent, "", 0, 0, Now, 0, intParentLineNo, "")
            Business.ADD2QUOTE_V2_1(UID, strBTORootItem, intReqQty, ewFLAG, COMM.Fixer.eItemType.Parent, "", 0, 0, Now, 0, "", intParentLineNo, "", Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)

            If DTCOM.Rows.Count > 0 Then
                For Each R As DataRow In DTCOM.Select("CATEGORY_TYPE='Component'")
                    If R.Item("CATEGORY_ID").ToString.Contains("|") Then
                        Dim ps() As String = Split(R.Item("CATEGORY_ID").ToString.ToUpper(), "|")
                        For Each p As String In ps
                            Dim cate As String = R.Item("PARENT_CATEGORY_ID").ToString.Replace("'", "''").ToUpper
                            Business.ADD2QUOTE_V2_1(UID, p.ToUpper, Request("Qty"), ewFLAG, COMM.Fixer.eItemType.Others, cate, 1, 1, Now, intParentLineNo, "", 0, "", Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
                        Next
                    Else
                        If R.Item("CATEGORY_ID").ToString.ToUpper().StartsWith("AGS-EW") Then
                            ewFLAG = Business.getMonthByEWItem(R.Item("CATEGORY_ID").ToString.ToUpper())
                        Else
                            Dim p As String = R.Item("CATEGORY_ID").ToString.ToUpper()
                            Dim cate As String = R.Item("PARENT_CATEGORY_ID").ToString.Replace("'", "''").ToUpper
                            Business.ADD2QUOTE_V2_1(UID, p.ToUpper, Request("Qty"), ewFLAG, COMM.Fixer.eItemType.Others, cate, 1, 1, Now, intParentLineNo, "", 0, "", Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
                        End If
                    End If

                Next
            End If

            'add Other Item





            Return True
        End If
        Return False
    End Function


    Protected Sub btnHide_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.panel1.Visible = False Then
            Me.panel1.Visible = True
        Else
            Me.panel1.Visible = False
        End If
    End Sub

    Protected Sub TimerExpandAll_Tick(ByVal sender As Object, ByVal e As System.EventArgs)
        TimerExpandAll.Interval = 99999
        btnExpandAll_Click(Me.btnExpandAll(), New EventArgs)
        TimerExpandAll.Enabled = False
    End Sub

    Protected Sub ph1_PostRestore(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each ctrl As Control In ph1.Controls
            If TypeOf (ctrl) Is HierarchyConfigUI Then
                Dim c As HierarchyConfigUI = ctrl
                RemoveHandler c.SelectedComponentChanged, AddressOf CompSelected
                AddHandler c.SelectedComponentChanged, AddressOf CompSelected
            End If
        Next
    End Sub

    Protected Sub btnExpandAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each ctrl As HierarchyConfigUI In ph1.Controls
            If btnExpandAll.Text = "Expand All" Then
                ctrl.ExpandAll()
            Else
                ctrl.CollapseAll()
            End If
        Next
        If btnExpandAll.Text = "Expand All" Then
            btnExpandAll.Text = "Collapse All"
        Else
            btnExpandAll.Text = "Expand All"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request("BTOITEM") Is Nothing OrElse Request("SPRNUM") Is Nothing OrElse Request("UserId") Is Nothing OrElse Request("TempId") Is Nothing Then Response.End()
        Pivot.CurrentProfile.setSession("tienchi.tai@advantech.com.tw", "en-Us", "")
        If Request("Qty") Is Nothing Then
            Response.Redirect(Request.ServerVariables("SCRIPT_NAME") + "?" + Request.QueryString.ToString() + "&Qty=1")
        End If

        If Request("UID") Is Nothing OrElse Request("UID").ToString.StartsWith("ACNQ", StringComparison.CurrentCultureIgnoreCase) = False Then

            Dim H As IBUS.iDocHeaderLine = Pivot.NewLineHeader
            H.CustomId = Request("SPRNUM")
            H.AccRowId = "preQuote"
            H.AccErpId = strErpId
            H.AccName = "preQuote"
            H.AccOfficeName = strSiebelRBU
            H.currency = strCurrency
            H.org = strOrg
            H.siebelRBU = strSiebelRBU
            Dim strQuoteId As String = Pivot.NewObjDocHeader.Add(H, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ).Key
            Response.Redirect(Request.ServerVariables("SCRIPT_NAME") + "?" + Request.QueryString.ToString() + "&UID=" + strQuoteId)
        End If

        If Not Page.IsPostBack AndAlso Request("BTOITEM") IsNot Nothing Then
            If Request("Qty") IsNot Nothing AndAlso Integer.TryParse(Request("QTY"), 0) Then
                hd_Qty.Value = Math.Abs(CInt(Request("Qty")))
            Else
                hd_Qty.Value = 1
            End If
            hd_BTOItem.Value = Trim(Request("BTOITEM")).ToUpper()

            If True Then

                Dim CBOMDt As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYDataTable = CBOMWS.GetCBOM2(hd_BTOItem.Value, strSiebelRBU, strOrg, "")
                'gv1.DataSource = CBOMDt : gv1.DataBind()
                For Each CBOMRow As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYRow In CBOMDt
                    'Nada modified for dynamically load user control
                    Dim ctrl As HierarchyConfigUI = LoadControl("~/ascx/HierarchyConfigUI.ascx")
                    AddHandler ctrl.SelectedComponentChanged, AddressOf CompSelected
                    ph1.Controls.Add(ctrl) : ctrl.CatName = CBOMRow.CATEGORY_ID
                    If Not IsDBNull(CBOMRow.CONFIGURATION_RULE) AndAlso CBOMRow.CONFIGURATION_RULE.Trim.ToUpper = "REQUIRED" Then ctrl.IsRequired = True
                    ctrl.Level = 1 : ctrl.IsSYSBOM = isOnly1Level(hd_BTOItem.Value, strOrg)
                Next
                tv1.Nodes.Add(New TreeNode(hd_BTOItem.Value, hd_BTOItem.Value))
                If True Then
                    Me.TimerExpandAll.Enabled = True
                End If
                If Not String.IsNullOrEmpty(strErpId) Then
                    'Write price cache to db
                    Dim arrDefaultItems As ArrayList = Business.GetBTOSLevel1DefaultComponents(hd_BTOItem.Value, Left(strOrg, 2))
                    If arrDefaultItems.Count > 0 Then
                        Dim strPNs As String = String.Join("|", arrDefaultItems.ToArray())
                        Dim dtPrice As DataTable = Nothing
                        Dim _SAPDAL As New SAPDAL.SAPDAL
                        'SAPTools.getSAPPriceByTable(strPNs, strOrg, strErpId, strErpId, "", dtPrice)
                        _SAPDAL.getSAPPriceByTable(strPNs, strOrg, strErpId, strErpId, "", "", dtPrice)
                        If dtPrice IsNot Nothing AndAlso dtPrice.Rows.Count > 0 Then
                            Dim dtPriceCache As New DataTable
                            With dtPriceCache.Columns
                                .Add("COMPANY_ID") : .Add("ORG") : .Add("PART_NO") : .Add("LIST_PRICE", GetType(Decimal)) : .Add("UNIT_PRICE", GetType(Decimal)) : .Add("CURRENCY") : .Add("CACHE_DATE", GetType(DateTime))
                            End With
                            For Each rPrice As DataRow In dtPrice.Rows
                                Dim rCache As DataRow = dtPriceCache.NewRow()
                                With rCache
                                    .Item("COMPANY_ID") = strErpId : .Item("ORG") = strOrg : .Item("PART_NO") = rPrice.Item("MATNR") : .Item("LIST_PRICE") = rPrice.Item("Kzwi1")
                                    .Item("UNIT_PRICE") = rPrice.Item("Netwr") : .Item("CURRENCY") = strCurrency : .Item("CACHE_DATE") = Now
                                End With
                                dtPriceCache.Rows.Add(rCache)
                            Next
                            Dim EQConn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
                            Dim bkCache As New SqlClient.SqlBulkCopy(EQConn)
                            bkCache.DestinationTableName = "SAP_PRICE_CACHE"
                            EQConn.Open()
                            bkCache.WriteToServer(dtPriceCache)
                            Dim cmdClearOldCache As New SqlClient.SqlCommand("delete from SAP_PRICE_CACHE where CACHE_DATE<=GETDATE()-1", EQConn)
                            If EQConn.State <> ConnectionState.Open Then EQConn.Open()
                            cmdClearOldCache.ExecuteNonQuery()
                            EQConn.Close()
                        End If
                    End If
                    'btnExpandAll_Click(Nothing, Nothing)
                End If
            End If
        End If
    End Sub

    Public Function isOnly1Level(ByVal RootID As String, ByVal Org As String) As Boolean
        Dim F As Boolean = False
        If IsEstoreBom(RootID, Org) Then
            F = True
        End If
        If (RootID.StartsWith("C-CTOS") Or RootID.StartsWith("SYS-")) And (Not RootID.StartsWith("C-CTOS-UUAAESC")) Then
            F = True
        End If
        Return F
    End Function
    Public Function IsEstoreBom(ByVal BTORootID As String, ByVal Org As String) As Boolean
        If BTORootID.StartsWith("EZ-", StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If
        Dim ObjectEZ_FLAG As Object = tbOPBase.dbExecuteScalar("B2B",
                                                               String.Format("SELECT ISNULL(COUNT(BTONo),0) as Bcount  FROM  ESTORE_BTOS_CATEGORY WHERE  DisplayPartno ='{1}' and StoreID like '%{0}'", _
                                                                     Left(Org.ToUpper, 2), BTORootID.Trim))
        If ObjectEZ_FLAG IsNot Nothing AndAlso Integer.TryParse(ObjectEZ_FLAG, 0) AndAlso Integer.Parse(ObjectEZ_FLAG) > 0 Then
            Return True
        End If
        Return False
    End Function

    Function GetConfigDt(ByRef NotSelCat As HierarchyConfigUI) As DataTable
        Dim dt As DataTable = Business.GetConfigOrderCartDt()
        For Each c As Control In ph1.Controls
            If TypeOf (c) Is HierarchyConfigUI Then
                Dim subCtrl As HierarchyConfigUI = CType(c, HierarchyConfigUI)
                Dim subDt As DataTable = subCtrl.GetSelectedItems(NotSelCat)
                For Each r As DataRow In subDt.Rows
                    If r.Item("category_type") = "Category" AndAlso r.Item("Level") = 1 Then
                        r.Item("PARENT_CATEGORY_ID") = hd_BTOItem.Value
                    End If
                Next
                dt.Merge(subDt)
            End If
        Next
        'Dim withoutCategory() As DataRow = dt.Select("CATEGORY_TYPE='Component'")
        'dt = withoutCategory.CopyToDataTable()
        Return dt
    End Function

    Sub ShowConfigDt(ByRef dt As DataTable, Optional ByRef tn As TreeNode = Nothing)
        Dim pid As String = ""
        If tn Is Nothing Then
            pid = hd_BTOItem.Value : tv1.Nodes.Clear()
            tv1.Nodes.Add(New TreeNode(hd_BTOItem.Value, hd_BTOItem.Value))
        Else
            pid = tn.Value
        End If
        Dim rs() As DataRow = dt.Select("parent_category_id='" + Replace(pid, "'", "''") + "'")
        For Each r As DataRow In rs
            Dim n As New TreeNode(r.Item("category_id"), r.Item("category_id"))
            If tn Is Nothing Then
                tv1.Nodes(0).ChildNodes.Add(n)
            Else
                tn.ChildNodes.Add(n)
            End If
            ShowConfigDt(dt, n)
        Next
        'gv1.DataSource = dt : gv1.DataBind()
    End Sub

    Protected Sub CompSelected()
        CatFlyOut.AttachTo = ""
        Dim dt As DataTable = GetConfigDt(Nothing)
        'gv1.DataSource = dt : gv1.DataBind()
        ShowConfigDt(dt) : tv1.ExpandAll()
        Dim tp As Decimal = 0, mdate As Date = Date.MinValue
        For Each r As DataRow In dt.Rows
            If r.Item("category_price") IsNot DBNull.Value Then
                tp += r.Item("category_price")
            End If
            If r.Item("ATP_DATE") IsNot DBNull.Value Then
                If DateDiff(DateInterval.Day, mdate, r.Item("ATP_DATE")) > 0 Then
                    mdate = r.Item("ATP_DATE")
                End If
            End If
        Next
        'Dim DTM As DataTable = myQM.GetDT(String.Format("quoteId='{0}'", Request("UID")), "")
        'If DTM.Rows.Count > 0 Then
        '    lbTotalPrice.Text = Util.getCurrSign(DTM.Rows(0).Item("currency")) + tp.ToString()
        '    If IsDate(mdate) Then
        '        If _IsAUSQ Then
        '            lbMaxDueDate.Text = mdate
        '        Else
        '            lbMaxDueDate.Text = Business.getBTOParentDueDate(CDate(mdate).ToString("yyyy/MM/dd"))
        '        End If

        '    Else
        '        If _IsAUSQ Then
        '            lbMaxDueDate.Text = Now.Date
        '        Else
        '            lbMaxDueDate.Text = Business.getBTOParentDueDate(CDate(DateAdd(DateInterval.Day, 3, Now.Day)).ToString("yyyy/MM/dd"))
        '        End If
        '        'Business.getBTOParentDueDate(CDate(DateAdd(DateInterval.Day, 3, Now.Day)).ToString("yyyy/MM/dd"))
        '    End If

        'End If
    End Sub
    Protected Sub gvConfiguredItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim datakey As DataKey = gvConfiguredItems.DataKeys(e.Row.RowIndex)
            If String.Equals(datakey("CATEGORY_TYPE").ToString.Trim, "category", StringComparison.CurrentCultureIgnoreCase) Then
                e.Row.Visible = False
            End If
        End If
    End Sub

End Class