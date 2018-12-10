Public Class ReConfigureCTOSCheck
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack AndAlso Request("ReConfigId") IsNot Nothing Then
            Dim strReconfigId As String = Trim(Request("ReConfigId"))
            Dim apt As New SqlClient.SqlDataAdapter(
                      " select ROOT_CATEGORY_ID, CONFIG_QTY, CONFIG_TREE_HTML, ORG_ID " +
                      " from CTOS_CONFIG_LOG " +
                      " where ROW_ID=@RID and COMPANY_ID=@ERPID ",
                      ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
            With apt.SelectCommand.Parameters
                .AddWithValue("RID", strReconfigId)
                .AddWithValue("ERPID", MasterRef.AccErpId)
            End With
            Dim reconfigDt As New DataTable
            apt.Fill(reconfigDt) : apt.SelectCommand.Connection.Close()
            If reconfigDt.Rows.Count = 1 Then
                Dim blHasPhasedOutItem As Boolean = False
                Dim hdoc1 As New HtmlAgilityPack.HtmlDocument
                hdoc1.LoadHtml(reconfigDt.Rows(0).Item("CONFIG_TREE_HTML"))
                Dim priceNodes As HtmlAgilityPack.HtmlNodeCollection = hdoc1.DocumentNode.SelectNodes("//div[@class='divPriceValue']")
                For Each priceNode As HtmlAgilityPack.HtmlNode In priceNodes
                    Dim partNoNode As HtmlAgilityPack.HtmlNode = priceNode.ParentNode.ParentNode.SelectSingleNode("input[@class='compOption']")
                    If partNoNode IsNot Nothing Then
                        Dim strCatId As String = partNoNode.ParentNode.ParentNode.ParentNode.ParentNode.Attributes("catname").Value
                        Dim strCompId As String = partNoNode.Attributes("compname").Value
                        If Not Business.IsOrderable(strCompId, reconfigDt.Rows(0).Item("ORG_ID")) Then
                            blHasPhasedOutItem = True
                            lbObsoleteItem.Text = strCompId : lbCatName.Text = strCatId
                            Dim MyCBOMDAL1 As New CBOMWS.MyCBOMDAL
                            Dim dtBom As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYDataTable = MyCBOMDAL1.GetCBOM2(strCatId, MasterRef.siebelRBU, reconfigDt.Rows(0).Item("ORG_ID"), "")
                            Dim dtComps() As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYRow = dtBom.Select("parent_category_id='" + Replace(strCatId, "'", "''") + "'")
                            lBoxAltItems.DataSource = dtComps : lBoxAltItems.DataBind()
                            hdReconfigId.Value = strReconfigId
                            hdCompRadioName.Value = partNoNode.Attributes("name").Value
                        End If
                    End If
                Next
                If Not blHasPhasedOutItem Then
                    Response.Redirect("Configurator.aspx?ReConfigId=" + strReconfigId + "&UID=" + Request("UID"))
                End If
            End If
        End If
    End Sub



    Protected Sub lBoxAltItems_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lbSelAltPrice.Text = COMM.Util.getCurrSign(MasterRef.currency) + Business.GetPrice(lBoxAltItems.SelectedValue, UID).ToString()
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If lBoxAltItems.SelectedIndex < 0 Then
            lbMsg.Text = "Please select one alternative component first"
            Exit Sub
        End If

        Dim apt As New SqlClient.SqlDataAdapter( _
                    " select ROOT_CATEGORY_ID, CONFIG_QTY, CONFIG_TREE_HTML, ORG_ID " + _
                    " from CTOS_CONFIG_LOG " + _
                    " where ROW_ID=@RID and USERID=@UID and COMPANY_ID=@ERPID ", _
                    ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        With apt.SelectCommand.Parameters
            .AddWithValue("RID", hdReconfigId.Value) : .AddWithValue("UID", HttpContext.Current.User.Identity.Name)
            .AddWithValue("ERPID", MasterRef.AccErpId)
        End With
        Dim reconfigDt As New DataTable
        apt.Fill(reconfigDt) : apt.SelectCommand.Connection.Close()
        If reconfigDt.Rows.Count = 1 Then
            Dim blPhasedoutCleared As Boolean = False, blAlternativeSet As Boolean = False
            Dim hdoc1 As New HtmlAgilityPack.HtmlDocument
            hdoc1.LoadHtml(reconfigDt.Rows(0).Item("CONFIG_TREE_HTML"))
            Dim inputNodes As HtmlAgilityPack.HtmlNodeCollection = hdoc1.DocumentNode.SelectNodes("//input[@name='" + hdCompRadioName.Value + "']")
            For Each inputNode As HtmlAgilityPack.HtmlNode In inputNodes
                If String.Equals(inputNode.Attributes("compname").Value, lbObsoleteItem.Text) Then
                    inputNode.ParentNode.SelectSingleNode("div[@class='divPrice']").InnerHtml = "" : inputNode.ParentNode.SelectSingleNode("div[@class='divATP']").InnerHtml = ""
                    blPhasedoutCleared = True
                Else
                    If String.Equals(inputNode.Attributes("compname").Value, lBoxAltItems.SelectedValue) Then
                        inputNode.ParentNode.SelectSingleNode("div[@class='divPrice']").InnerHtml = "<b>Price:</b>" + COMM.Util.getCurrSign(MasterRef.currency) + "<div class='divPriceValue' style='display:inline;'>0</div>"
                        inputNode.ParentNode.SelectSingleNode("div[@class='divATP']").InnerHtml = "<b>Available on:</b><div class='divATPValue' style='display:inline;'>9999/12/31</div>"
                        blAlternativeSet = True
                    End If
                End If
            Next
            If blPhasedoutCleared And blAlternativeSet Then
                Dim cmd As New SqlClient.SqlCommand("update CTOS_CONFIG_LOG set CONFIG_TREE_HTML=@CHTML where ROW_ID=@RID", _
                                                    New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString))
                cmd.Parameters.AddWithValue("CHTML", hdoc1.DocumentNode.InnerHtml) : cmd.Parameters.AddWithValue("RID", hdReconfigId.Value)
                cmd.Connection.Open() : cmd.ExecuteNonQuery() : cmd.Connection.Close()
                lbMsg.Text = "Updated"
                Response.Redirect("ReConfigureCTOSCheck.aspx?ReConfigId=" + hdReconfigId.Value + "&UID=" + Request("UID"))
            End If
        End If
    End Sub

End Class