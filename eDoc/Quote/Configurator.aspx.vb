Public Class Configurator
    Inherits PageBase
    'Datatype def area
    Public Class ReconfigTreeObject
        Private _BtoItem As String, _reConfigTreeHtml As String, _reConfigQty As Integer
        Public Property BTOItem As String
            Get
                Return _BtoItem
            End Get
            Set(ByVal value As String)
                _BtoItem = value
            End Set
        End Property
        Public Property ReConfigTreeHtml As String
            Get
                Return _reConfigTreeHtml
            End Get
            Set(ByVal value As String)
                _reConfigTreeHtml = value
            End Set
        End Property
        Public Property ReConfigQty As Integer
            Get
                Return _reConfigQty
            End Get
            Set(ByVal value As Integer)
                _reConfigQty = value
            End Set
        End Property
    End Class
    Public Class ConfiguredComponent
        Private _catid As String, _childComps As List(Of ConfiguredComponent), _catType As String
        Public Property CategoryId As String
            Get
                Return _catid
            End Get
            Set(ByVal value As String)
                _catid = value
            End Set
        End Property
        Public Property CategoryType As String
            Get
                Return _catType
            End Get
            Set(ByVal value As String)
                _catType = value
            End Set
        End Property
        Public Property ChildComps As List(Of ConfiguredComponent)
            Get
                Return _childComps
            End Get
            Set(ByVal value As List(Of ConfiguredComponent))
                _childComps = value
            End Set
        End Property
    End Class
    Public Class SaveToCartResult
        Private _procStatus As Boolean, _procMsg As String
        Public Property ProcessStatus As Boolean
            Get
                Return _procStatus
            End Get
            Set(ByVal value As Boolean)
                _procStatus = value
            End Set
        End Property
        Public Property ProcessMessage As String
            Get
                Return _procMsg
            End Get
            Set(ByVal value As String)
                _procMsg = value
            End Set
        End Property

    End Class
    Public Class CBom
        Private _CategoryType As String, _CategoryId As String, _IsCatRequired As Boolean, _IsCompDefault As Boolean, _strDescription As String
        Private _IsCompRoHS As Boolean, _Expand As Boolean, _ChildCategories As List(Of CBom), _ClientId As String, _strExtDescription As String 'ICC 2015/4/10 Add extended description prop
        Private _IsHot As Boolean 'Ryan 20160719 Add isHot tag for EU 
        Public Property CategoryType As String
            Get
                Return _CategoryType
            End Get
            Set(ByVal value As String)
                _CategoryType = value
            End Set
        End Property
        Public Property CategoryId As String
            Get
                Return _CategoryId
            End Get
            Set(ByVal value As String)
                _CategoryId = value
            End Set
        End Property
        Public Property IsCatRequired As Boolean
            Get
                Return _IsCatRequired
            End Get
            Set(ByVal value As Boolean)
                _IsCatRequired = value
            End Set
        End Property
        Public Property IsCompDefault As Boolean
            Get
                Return _IsCompDefault
            End Get
            Set(ByVal value As Boolean)
                _IsCompDefault = value
            End Set
        End Property
        Public Property Description As String
            Get
                Return _strDescription
            End Get
            Set(ByVal value As String)
                _strDescription = value
            End Set
        End Property

        'ICC 2015/4/10 Add extended decription prop
        Public Property ExtDescription As String
            Get
                Return _strExtDescription
            End Get
            Set(ByVal value As String)
                _strExtDescription = value
            End Set
        End Property
        ' ICC 2015/4/10 Add extended description flag to check
        Public ReadOnly Property IsExtDesc As Boolean
            Get
                If Not String.IsNullOrEmpty(Me.ExtDescription) Then Return True
                Return False
            End Get
        End Property

        Public Property IsCompRoHS As Boolean
            Get
                Return _IsCompRoHS
            End Get
            Set(ByVal value As Boolean)
                _IsCompRoHS = value
            End Set
        End Property
        Public Property IsHot As Boolean
            Get
                Return _IsHot
            End Get
            Set(value As Boolean)
                _IsHot = value
            End Set
        End Property
        Public Property Expand As Boolean
            Get
                Return _Expand
            End Get
            Set(ByVal value As Boolean)
                _Expand = value
            End Set
        End Property
        Public Property ChildCategories As List(Of CBom)
            Get
                Return _ChildCategories
            End Get
            Set(ByVal value As List(Of CBom))
                _ChildCategories = value
            End Set
        End Property

        Public Property ClientId As String
            Get
                Return _ClientId
            End Get
            Set(ByVal value As String)
                _ClientId = value
            End Set
        End Property

        Sub CalcClientId(ByVal strInput As String)
            Dim hashAlgorithm As New System.Security.Cryptography.SHA1CryptoServiceProvider
            Dim byteValue() As Byte = Encoding.UTF8.GetBytes(strInput)
            Dim hashValue() As Byte = hashAlgorithm.ComputeHash(byteValue)
            _ClientId = Left(Convert.ToBase64String(hashValue).Replace("_", "").Replace("+", "").Replace("=", "").Replace("/", ""), 10)
        End Sub

    End Class
    Public Class PriceATP
        Private _Price As Decimal, _ATPDate As String, _ATPQty As Integer, _currencySign As String, _IsEw As Boolean
        Private _ListPrice As Decimal
        Public Property Price As Decimal
            Get
                Return _Price
            End Get
            Set(ByVal value As Decimal)
                _Price = value
            End Set
        End Property
        Public Property ListPrice As Decimal
            Get
                Return _ListPrice
            End Get
            Set(ByVal value As Decimal)
                _ListPrice = value
            End Set
        End Property
        Public Property ATPDate As String
            Get
                Return _ATPDate
            End Get
            Set(ByVal value As String)
                _ATPDate = value
            End Set
        End Property
        Public Property ATPQty As Integer
            Get
                Return _ATPQty
            End Get
            Set(ByVal value As Integer)
                _ATPQty = value
            End Set
        End Property
        Public Property CurrencySign As String
            Get
                Return _currencySign
            End Get
            Set(ByVal value As String)
                _currencySign = value
            End Set
        End Property
        Public Property IsEw As Boolean
            Get
                Return _IsEw
            End Get
            Set(ByVal value As Boolean)
                _IsEw = value
            End Set
        End Property
    End Class
    '/Datatype def area

    'Page logic
    Public Function isOnly1Level(ByVal RootID As String, ByVal Org As String) As Boolean
        Dim F As Boolean = False
        If IsEstoreBom(RootID, Org) Then
            F = True
        End If
        'If (RootID.StartsWith("C-CTOS") Or RootID.StartsWith("SYS-")) And (Not RootID.StartsWith("C-CTOS-UUAAESC")) Then
        If RootID.StartsWith("C-CTOS") Or RootID.StartsWith("SYS-") Then
            F = True
        End If
        Return F
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            'Nada Added to take the place of session
            Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initCurr",
            "$('#hdCurrency').val('" + MasterRef.currency + "');", True)
            Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initCompanyId",
            "$('#hdCompanyId').val('" + MasterRef.AccErpId + "');", True)
            Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initOrg",
            "$('#hdOrg').val('" + MasterRef.org + "');", True)
            Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initRBU",
            "$('#hdRBU').val('" + MasterRef.siebelRBU + "');", True)
            Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initUID",
            "$('#hdUID').val('" + MasterRef.Key + "');", True)
            If Request("ReConfigId") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request("ReConfigId")) Then
                Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initReConfigId",
         "$('#ReConfigId').val('" + Request("ReConfigId").ToString.Trim + "');", True)
            End If
            If Request("BTOItem") IsNot Nothing Then
                Dim strBTOItem As String = Trim(Request("BTOItem")), intConfigQty As Integer = 1, blIsEstoreOrOneLevelBTO As Boolean = False
                Dim _IseStoreCBom As Boolean = IsEstoreBom(strBTOItem, Pivot.CurrentProfile.getCurrOrg)
                Dim retCount As Integer = 0, sql As String = String.Empty

                If _IseStoreCBom Then
                    'Frank 20131002 We should use below sql to check if eStoreCBom exists in table "cbom_catalog_category"
                    sql = "select count(category_id) as c from cbom_catalog_category where PARENT_CATEGORY_ID=@RootId and org=@ORG and CATEGORY_ID<>@RootId"
                    sql &= " and (CATEGORY_TYPE='Category' or CATEGORY_TYPE='Component' or (CATEGORY_TYPE='Component' and (CATEGORY_ID='No Need' or CATEGORY_ID like '%|%')) or CATEGORY_TYPE like '%extended%') "
                    Dim cmdCheckBTOItemValid As New SqlClient.SqlCommand(sql, New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
                    With cmdCheckBTOItemValid.Parameters
                        .AddWithValue("RootId", strBTOItem) : .AddWithValue("ORG", Left(Pivot.CurrentProfile.getCurrOrg, 2))
                    End With
                    cmdCheckBTOItemValid.Connection.Open()
                    retCount = CInt(cmdCheckBTOItemValid.ExecuteScalar())
                    cmdCheckBTOItemValid.Connection.Close()

                Else
                    '20130619 Check if BTOItem is valid, if not, diable continue button
                    Dim cmdCheckBTOItemValid As New SqlClient.SqlCommand(
                        "select count(category_id) as c from cbom_catalog_category where category_id=@RootId and org=@ORG and parent_category_id='Root' ",
                        New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
                    'Ryan 20170510 Add ORG2ViewORG function for AJP
                    With cmdCheckBTOItemValid.Parameters
                        .AddWithValue("RootId", strBTOItem) : .AddWithValue("ORG", ORG2ViewORG(Left(Pivot.CurrentProfile.getCurrOrg, 2)))
                    End With
                    cmdCheckBTOItemValid.Connection.Open()
                    retCount = CInt(cmdCheckBTOItemValid.ExecuteScalar())
                    cmdCheckBTOItemValid.Connection.Close()

                End If

                If retCount = 0 Then
                    Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initConfigQty", "$('.continueBtn').prop('disabled', true);", True) : Exit Sub
                End If

                If Request("Qty") IsNot Nothing AndAlso Integer.TryParse(Request("Qty"), 1) AndAlso CInt(Request("Qty")) > 0 Then
                    Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initConfigQty",
                   "$('#hdConfigQty').val('" + CInt(Request("Qty")).ToString() + "');", True)
                    intConfigQty = CInt(Request("Qty"))
                End If

                'If IsEstoreBom(strBTOItem, Pivot.CurrentProfile.getCurrOrg) OrElse isOnly1Level(strBTOItem, Pivot.CurrentProfile.getCurrOrg) Then
                If _IseStoreCBom OrElse isOnly1Level(strBTOItem, Pivot.CurrentProfile.getCurrOrg) Then
                    Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "setOnlyOneLevel", "$('#hdIsOneLevel').val('1');", True)
                    blIsEstoreOrOneLevelBTO = True
                Else
                    If MasterRef.org.ToString().StartsWith("EU", StringComparison.OrdinalIgnoreCase) Then
                        Dim _sql As New StringBuilder
                        _sql.AppendLine(" Select a.Catalog_Org,a.CATALOG_TYPE,b.LOCAL_NAME,a.CATALOG_ID,a.CATALOG_NAME,a.CATALOG_DESC, a.CREATED ")
                        _sql.AppendLine(" From CBOM_CATALOG a inner join CBOM_CATALOG_LOCALNAME b on a.CATALOG_TYPE=b.CATALOG_TYPE ")
                        _sql.AppendLine(" Where a.Catalog_Org='EU' and a.CATALOG_TYPE like '%Pre-Configuration' and a.CATALOG_NAME ='" & Replace(strBTOItem, "'", "''") & "' ")
                        Dim _dt As DataTable = tbOPBase.dbGetDataTable("MY", _sql.ToString)
                        If Not IsNothing(_dt) AndAlso _dt.Rows.Count > 0 AndAlso
                            _dt.Rows(0).Item("LOCAL_NAME").ToString.Equals("Pre-Configuration for AEU eStore (buy.advantech.eu) Configuration",
                            StringComparison.InvariantCultureIgnoreCase) Then
                            '_IsAEUeStore = True
                            Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "setOnlyOneLevel", "$('#hdIsOneLevel').val('1');", True)
                            blIsEstoreOrOneLevelBTO = True
                        End If
                    End If
                End If

                Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initBTOValue",
                    "$('#hdBTOId').val('" + strBTOItem + "'); InitLoadBOM();", True)

                Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initCurrencySign", "$($('.totalPriceCurrSign')[0]).html('" + COMM.Util.getCurrSign(MasterRef.currency) + "');", True)
                SetSourcePath(strBTOItem, intConfigQty, MasterRef.org)
                If Not blIsEstoreOrOneLevelBTO Then
                    Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "DefaultExpandAll", "collapseExpandAll();", True)
                End If
            Else
                If Request("ReConfigId") IsNot Nothing Then
                    Dim strReconfigId As String = Trim(Request("ReConfigId"))
                    Dim apt As New SqlClient.SqlDataAdapter(
                        " select ROOT_CATEGORY_ID, CONFIG_QTY, CONFIG_TREE_HTML, ORG_ID,ISONELEVEL " +
                        " from CTOS_CONFIG_LOG " +
                        " where ROW_ID=@RID and COMPANY_ID=@ERPID ",
                        ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
                    With apt.SelectCommand.Parameters
                        .AddWithValue("RID", strReconfigId) : .AddWithValue("ERPID", MasterRef.AccErpId.ToString())
                    End With
                    Dim reconfigDt As New DataTable
                    apt.Fill(reconfigDt) : apt.SelectCommand.Connection.Close()
                    If reconfigDt.Rows.Count = 1 Then

                        Dim hdoc1 As New HtmlAgilityPack.HtmlDocument
                        hdoc1.LoadHtml(reconfigDt.Rows(0).Item("CONFIG_TREE_HTML"))
                        Dim priceNodes As HtmlAgilityPack.HtmlNodeCollection = hdoc1.DocumentNode.SelectNodes("//div[@class='divPriceValue']")

                        For Each priceNode As HtmlAgilityPack.HtmlNode In priceNodes
                            Dim partNoNode As HtmlAgilityPack.HtmlNode = priceNode.ParentNode.ParentNode.SelectSingleNode("input[@class='compOption']")
                            If partNoNode IsNot Nothing Then
                                Dim strCatId As String = partNoNode.ParentNode.ParentNode.ParentNode.ParentNode.Attributes("catname").Value
                                Dim strCompId As String = partNoNode.Attributes("compname").Value
                                If Not Business.IsOrderable(strCompId, reconfigDt.Rows(0).Item("ORG_ID")) Then
                                    Response.Redirect("ReConfigureCTOSCheck.aspx?UID=" + UID + "&ReConfigId=" + strReconfigId)
                                    Exit Sub
                                End If
                            End If
                        Next

                        Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initReconfigTree",
                        "InitReconfigData('" + Trim(Request("ReConfigId")) + "');", True)
                        If Not IsDBNull(reconfigDt.Rows(0).Item("ISONELEVEL")) AndAlso reconfigDt.Rows(0).Item("ISONELEVEL").ToString.Trim = "1" Then
                            Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "setOnlyOneLevel", "$('#hdIsOneLevel').val('1');", True)
                        End If
                    End If
                Else
                    Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "initConfigQty", "$('.continueBtn').prop('disabled', true);", True) : Exit Sub
                End If
            End If

            'Ryan 20170120 Add for AJP (will execute below code either in normal config or reconfig)
            If Role.IsJPAonlineSales Then
                divJP.Visible = True
                If MyQuoteX.IsHaveBtos(UID) Then
                    Msg.Text = "Attention! Configure a new system will delete all existing items and systems in quote."
                End If
            End If


        End If
    End Sub


    <Services.WebMethod(EnableSession:=True)>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function GetReconfigTree(ByVal ReConfigId As String, ByVal CompanyId As String, ByVal UID As String) As String
        Dim ReconfigTreeObject1 As New ReconfigTreeObject
        Dim apt As New SqlClient.SqlDataAdapter(
                       " select ROOT_CATEGORY_ID, CONFIG_QTY, CONFIG_TREE_HTML, ORG_ID " +
                       " from CTOS_CONFIG_LOG " +
                       " where ROW_ID=@RID and COMPANY_ID=@ERPID ",
                       ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        With apt.SelectCommand.Parameters
            .AddWithValue("RID", ReConfigId) : .AddWithValue("ERPID", CompanyId.ToString())
        End With
        Dim reconfigDt As New DataTable
        apt.Fill(reconfigDt) : apt.SelectCommand.Connection.Close()
        If reconfigDt.Rows.Count = 1 Then

            Dim hdoc1 As New HtmlAgilityPack.HtmlDocument
            hdoc1.LoadHtml(reconfigDt.Rows(0).Item("CONFIG_TREE_HTML"))
            Dim priceNodes As HtmlAgilityPack.HtmlNodeCollection = hdoc1.DocumentNode.SelectNodes("//div[@class='divPriceValue']")
            Dim atpNodes As HtmlAgilityPack.HtmlNodeCollection = hdoc1.DocumentNode.SelectNodes("//div[@class='divATPValue']")

            For Each pNode As HtmlAgilityPack.HtmlNode In priceNodes
                Dim partNoNode As HtmlAgilityPack.HtmlNode = pNode.ParentNode.ParentNode.SelectSingleNode("input[@class='compOption']")
                pNode.InnerHtml = Business.GetPrice(partNoNode.Attributes("compname").Value, UID)
            Next
            For Each atpNode As HtmlAgilityPack.HtmlNode In atpNodes
                Dim partNoNode As HtmlAgilityPack.HtmlNode = atpNode.ParentNode.ParentNode.SelectSingleNode("input[@class='compOption']")
                atpNode.InnerHtml = GetATP(partNoNode.Attributes("compname").Value, reconfigDt.Rows(0).Item("CONFIG_QTY")).ToString("yyyy/MM/dd")
            Next
            With ReconfigTreeObject1
                .BTOItem = reconfigDt.Rows(0).Item("ROOT_CATEGORY_ID") : .ReConfigQty = reconfigDt.Rows(0).Item("CONFIG_QTY")
                .ReConfigTreeHtml = hdoc1.DocumentNode.InnerHtml
            End With
        End If
        Dim serializer = New Script.Serialization.JavaScriptSerializer()
        Return serializer.Serialize(ReconfigTreeObject1)
    End Function

    Public Shared Function GetConfigOrderCartDt() As DataTable
        Dim dt As New DataTable("ConfigCart")
        With dt.Columns
            .Add("CATEGORY_ID", GetType(String)) : .Add("CATEGORY_NAME", GetType(String))
            .Add("CATEGORY_TYPE")
            .Add("PARENT_CATEGORY_ID", GetType(String)) : .Add("CATALOG_ID", GetType(String))
            .Add("CATALOGCFG_SEQ", GetType(Integer)) : .Add("CATEGORY_DESC", GetType(String))
            .Add("DISPLAY_NAME", GetType(String)) : .Add("IMAGE_ID", GetType(String))
            .Add("EXTENDED_DESC", GetType(String)) : .Add("CREATED", GetType(DateTime))
            .Add("CREATED_BY", GetType(String)) : .Add("LAST_UPDATED", GetType(DateTime))
            .Add("LAST_UPDATED_BY", GetType(String)) : .Add("SEQ_NO", GetType(Integer))
            .Add("PUBLISH_STATUS", GetType(String)) : .Add("CATEGORY_PRICE", GetType(Double)) : .Add("CATEGORY_LIST_PRICE", GetType(Double)) : .Add("CATEGORY_RECYCLE_FEE", GetType(Double))
            .Add("ITP", GetType(Double)) : .Add("NewITP", GetType(Double))
            .Add("CATEGORY_QTY", GetType(Integer)) : .Add("ParentSeqNo", GetType(Integer)) : .Add("ParentRoot", GetType(String))
        End With
        Return dt
    End Function
    <Services.WebMethod(EnableSession:=True)>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function SaveConfigResult(ByVal RootComp As ConfiguredComponent, ByVal ConfigQty As Integer, ByVal ConfigTreeHtml As String, ByVal CompanyId As String, ByVal Org As String, ByVal UID As String, ByVal ReConfigId As String, ByVal IsOneLevel As Integer) As String
        'Save config tree html to eQuotation.dbo.CTOS_CONFIG_LOG for re-configuration purpose
        Dim _NewReConfigId As String = System.Guid.NewGuid.ToString().Replace("-", "").Substring(0, 15) + DateTime.Now.ToString("_yyyyMMddHHmmssfff")
        'If String.IsNullOrEmpty(RECFIGID) Then
        '    RECFIGID = System.Guid.NewGuid.ToString().Replace("-", "").Substring(0, 15) + DateTime.Now.ToString("_yyyyMMddHHmmssfff")
        'End If
        Dim eqCmd As New SqlClient.SqlCommand(
            " insert into CTOS_CONFIG_LOG (ROW_ID, ROOT_CATEGORY_ID, CONFIG_QTY, USERID, COMPANY_ID, ORG_ID, CONFIG_TREE_HTML, CART_ID,ISONELEVEL) " +
            " values(@RID, @BTOITEM, @QTY, @UID, @ERPID, @ORGID, @CONFIGHTML, @CARTID,@ISONELEVEL)",
            New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString))
        With eqCmd.Parameters
            .AddWithValue("RID", _NewReConfigId) : .AddWithValue("BTOITEM", RootComp.CategoryId) : .AddWithValue("QTY", ConfigQty)
            .AddWithValue("UID", HttpContext.Current.User.Identity.Name) : .AddWithValue("ERPID", CompanyId.ToString()) : .AddWithValue("ORGID", Org)
            .AddWithValue("CONFIGHTML", ConfigTreeHtml) : .AddWithValue("CARTID", UID) : .AddWithValue("ISONELEVEL", IsOneLevel)
        End With
        eqCmd.Connection.Open() : eqCmd.ExecuteNonQuery() : eqCmd.Connection.Close()

        Dim procObj As New SaveToCartResult
        Try
            Dim dt As DataTable = GetConfigOrderCartDt()
            If Not dt.Columns.Contains("Level") Then dt.Columns.Add("Level", GetType(Integer))
            If Not dt.Columns.Contains("ATP_DATE") Then dt.Columns.Add("ATP_DATE", GetType(Date))
            'RecursiveGetConfigResult(RootComp, "Root", dt, ConfigQty, 0, UID)
            RecursiveGetConfigResultV2(RootComp, "Root", Org, dt, ConfigQty, 0, UID)

            '20130619 TC: comment below logic for calculating price, inventory and ex-warranty, because it has been handled in save2cart function
            'Find if AGS-EW is selected
            'Dim blHasEW As Boolean = False, dEwRate As Double = 0
            'For Each rComp As DataRow In dt.Rows
            '    If rComp.Item("category_type") = "Component" AndAlso _
            '        rComp.Item("category_id").ToString.StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase) Then
            '        dEwRate = Glob.getRateByEWItem(rComp.Item("category_id"), Left(HttpContext.Current.Session("org_id"), 2) + "H1")
            '        blHasEW = True
            '        Exit For
            '    End If
            'Next

            ''If AGS-EW is selected, calculate EW fee
            'If blHasEW Then
            '    Dim dTotalAmt As Double = 0
            '    For Each rComp As DataRow In dt.Rows
            '        If rComp.Item("category_type") = "Component" AndAlso _
            '            Not rComp.Item("category_id").ToString.StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase) Then
            '            dTotalAmt += rComp.Item("category_price")
            '        End If
            '    Next
            '    For Each rComp As DataRow In dt.Rows
            '        If rComp.Item("category_type") = "Component" AndAlso _
            '            rComp.Item("category_id").ToString.StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase) Then
            '            rComp.Item("category_price") = dTotalAmt * dEwRate
            '            Exit For
            '        End If
            '    Next
            '    dt.AcceptChanges()
            'End If


            'Ryan 20180514 Check if ATW standard parts ZMIP are missing.
            If Org.StartsWith("TW") AndAlso CompanyId.StartsWith("T") Then
                Dim configitems As List(Of String) = New List(Of String)
                For Each dr As DataRow In dt.Rows
                    If String.Equals(dr.Item("CATEGORY_TYPE").ToString.Trim, "Component", StringComparison.CurrentCultureIgnoreCase) Then
                        configitems.Add(dr("CATEGORY_ID").ToString)
                    End If
                Next

                Dim invalidparts As List(Of String) = New List(Of String)
                If Advantech.Myadvantech.Business.PartBusinessLogic.isATWPartsWithoutZMIP(configitems, Org, CompanyId, invalidparts) Then
                    procObj.ProcessStatus = False : procObj.ProcessMessage = "Condition ZMIP for below parts are not maintained, please check again: \n"
                    For Each s As String In invalidparts
                        procObj.ProcessMessage += s + " "
                    Next
                    Dim JSSerializer = New Script.Serialization.JavaScriptSerializer()
                    Return JSSerializer.Serialize(procObj)
                End If
            End If


            If SaveConfig2Cart(dt, RootComp.CategoryId, ConfigQty, CompanyId, Org, UID, _NewReConfigId, ReConfigId) Then
                procObj.ProcessStatus = True : procObj.ProcessMessage = "ok"
            Else
                procObj.ProcessStatus = False : procObj.ProcessMessage = "not ok"
            End If
        Catch ex As Exception
            procObj.ProcessStatus = False
            If ex.Message.StartsWith("Invalid PartNo.") Then
                procObj.ProcessMessage = ex.Message & ". Product does not exist in SAP or it is not a orderable product."
            Else
                procObj.ProcessMessage = ex.ToString()
            End If
        End Try
        Dim serializer = New Script.Serialization.JavaScriptSerializer()
        Return serializer.Serialize(procObj)
    End Function
    Shared Function SaveConfig2Cart(ByRef DTCOM As DataTable, ByVal strBTOItem As String, ByVal intConfigQty As Integer, ByVal CompanyId As String, ByVal org As String, ByVal UID As String, ByVal RECFIGID As String, ByVal OldReConfigId As String) As Boolean
        Dim M As IBUS.iDocHeader = Pivot.NewObjDocHeader

        Dim dtQM As IBUS.iDocHeaderLine = M.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(dtQM) Then
            Dim QMRow As IBUS.iDocHeaderLine = dtQM, company As String = QMRow.AccErpId
            Dim plant As String = Business.getPlantByOrgID(QMRow.org)
            Dim RBU As String = QMRow.siebelRBU, ewFLAG As Integer = 0, Currency As String = QMRow.currency
            Dim strBTORootItem As String = strBTOItem, intReqQty As Integer = 1
            If Integer.TryParse(intConfigQty, 1) Then intReqQty = CInt(intConfigQty)
            Dim ddMaxDueDate As Date = Now.ToShortDateString
            'myQD.Delete(String.Format("quoteId='{0}'", UID))
            'Ming 20140619 欧洲客户config BTOS时要清空QuoteDatail的单品
            If String.Equals(QMRow.org, "EU10", StringComparison.CurrentCultureIgnoreCase) Then
                Dim Qlist As List(Of QuoteItem) = MyQuoteX.GetQuoteList(UID)
                If Qlist IsNot Nothing Then
                    Dim Dlist As List(Of QuoteItem) = Qlist.Where(Function(p) p.line_No < 100).ToList
                    If Dlist IsNot Nothing AndAlso Dlist.Count > 0 Then
                        MyUtil.Current.CurrentDataContext.QuoteItems.DeleteAllOnSubmit(Dlist)
                        MyUtil.Current.CurrentDataContext.SubmitChanges()
                    End If
                End If
            End If
            'END
            Dim intParentLineNo As Integer = 0
            Dim EWROW() As DataRow = DTCOM.Select("CATEGORY_ID LIKE 'AGS-EW%'")
            If EWROW.Length > 0 Then
                ewFLAG = Business.getMonthByEWItem(EWROW(0).Item("CATEGORY_ID").ToString.ToUpper())
            Else
                ewFLAG = 0
            End If

            'Frank 2014/04/15 Get system's parent line number
            'intParentLineNo = MyQuoteX.getMaxParentLineNo(UID) + 100
            'Ming 20140923    Get system's parent line number
            intParentLineNo = MyQuoteX.getBtosParentLineNo(UID)
            ' ming add for recofing bug 2013-10-25
            If Not String.IsNullOrEmpty(OldReConfigId) Then
                Dim _dt As DataTable = tbOPBase.dbGetDataTable("EQ", String.Format("SELECT line_No   FROM QuotationDetail WHERE  quoteId='{1}' and RECFIGID='{0}'", OldReConfigId, UID))
                If _dt.Rows.Count > 0 Then
                    Dim _HigherLevel As String = _dt.Rows(0).Item("line_No")
                    tbOPBase.dbExecuteNoQuery("EQ", String.Format("Delete From QuotationDetail WHERE  quoteId='{1}' and ( line_No = {0} or HigherLevel ={0})", _HigherLevel, UID))
                    intParentLineNo = Integer.Parse(_HigherLevel)
                End If
            End If
            ' end
            'Business.ADD2QUOTE_V2(UID, strBTORootItem, intReqQty, ewFLAG, COMM.Fixer.eItemType.Parent, "", 0, 0, Now, 0, intParentLineNo, "")

            'Ryan 20161116 If is JP01 quotes and already contains BTOS parent, remove all btos items due to there can only be one BTOS at a time for JP
            If String.Equals(QMRow.org, "JP01", StringComparison.CurrentCultureIgnoreCase) Then
                Dim _HigherLevel As String = "100"
                tbOPBase.dbExecuteNoQuery("EQ", String.Format("Delete From QuotationDetail WHERE quoteId='{0}'", UID))
                intParentLineNo = Integer.Parse(_HigherLevel)
            End If

            'Frank 20140414:Use Bulk Copy to insert data into quotationdetail for a assembly system.
            'tbOPBase.dbExecuteNoQuery("EQ", "Delete from QuotationDetail Where QuoteID='" & UID & "'")
            Dim _QuoteDetailDT As DataTable = tbOPBase.dbGetDataTable("EQ", "Select * from QuotationDetail Where QuoteID='-1'")

            Dim _row As DataRow = Nothing, _LineNo As Integer = 0, _ParentLineNumber = 0, _ItemType = 0, _ProDesc As String = String.Empty
            Dim dt1 As DataTable = Nothing
            Dim _IsATWUser As Boolean = Role.IsTWAonlineSales
            Dim _IsACNUser As Boolean = Role.IsCNAonlineSales
            Dim _IsAUSUser As Boolean = Role.IsUsaUser
            Dim _IsKRAonlineUser As Boolean = Role.IsKRAonlineSales 'ICC 2016/1/14 Add KR Aonline role.
            Dim _IsAJPUser As Boolean = Role.IsJPAonlineSales
            Dim _HigherLevelQty As Int64 = 1
            For Each R As DataRow In DTCOM.Rows
                'If String.Equals(R.Item("CATEGORY_ID").ToString.Trim, "No Need", StringComparison.CurrentCultureIgnoreCase) Then
                If String.Equals(R.Item("CATEGORY_ID").ToString.Trim, MyExtension.BuildIn, StringComparison.CurrentCultureIgnoreCase) Then
                    Continue For
                End If

                If Not String.Equals(R.Item("CATEGORY_TYPE").ToString.Trim, "Component", StringComparison.CurrentCultureIgnoreCase) _
                    AndAlso Not String.Equals(R.Item("PARENT_CATEGORY_ID").ToString.Trim, "Root", StringComparison.CurrentCultureIgnoreCase) Then
                    Continue For
                End If

                _row = _QuoteDetailDT.NewRow()

                If String.Equals(R.Item("PARENT_CATEGORY_ID").ToString.Trim, "Root", StringComparison.CurrentCultureIgnoreCase) Then
                    _LineNo = intParentLineNo
                    _row.Item("PartNo") = Business.replaceCartBTO(R.Item("CATEGORY_ID").ToString, QMRow.org)
                    _row.Item("ListPrice") = 0
                    _row.Item("UnitPrice") = 0
                    _row.Item("newUnitPrice") = 0
                    _row.Item("HigherLevel") = 0
                    _row.Item("RECFIGID") = RECFIGID
                    _row.Item("ItemType") = 1
                    _row.Item("RecyclingFee") = 0
                Else
                    _row.Item("PartNo") = R.Item("CATEGORY_ID").ToString
                    _row.Item("ListPrice") = R.Item("CATEGORY_List_Price")
                    _row.Item("UnitPrice") = R.Item("CATEGORY_Price")
                    '20150612 Ming get Recycling Fee
                    _row.Item("newUnitPrice") = R.Item("CATEGORY_Price")
                    _row.Item("RecyclingFee") = 0
                    If _IsAUSUser Then
                        Dim _RecycleFee As Decimal = 0
                        If Not IsDBNull(R.Item("CATEGORY_RECYCLE_FEE")) AndAlso Decimal.TryParse(R.Item("CATEGORY_RECYCLE_FEE").ToString(), _RecycleFee) Then
                            Dim _newUnitPrice As Decimal = 0, _ListPrice = 0
                            If _RecycleFee > 0 AndAlso Not IsDBNull(R.Item("CATEGORY_Price")) AndAlso Decimal.TryParse(R.Item("CATEGORY_Price").ToString(), _newUnitPrice) Then
                                _row.Item("RecyclingFee") = _RecycleFee
                                _row.Item("UnitPrice") = _newUnitPrice - _RecycleFee
                                _row.Item("newUnitPrice") = _newUnitPrice - _RecycleFee
                                If Not IsDBNull(R.Item("CATEGORY_List_Price")) AndAlso Decimal.TryParse(R.Item("CATEGORY_List_Price").ToString(), _ListPrice) Then
                                    _row.Item("ListPrice") = _ListPrice - _RecycleFee
                                End If
                            End If
                        End If
                    End If

                    'ICC 2016/1/14 When KRAonline add AGS-CTOS-SYS-A/B, set list price & unit price. Request by Jessica.Lee.
                    If _IsKRAonlineUser Then
                        If R.Item("CATEGORY_ID").ToString.Equals("AGS-CTOS-SYS-A", StringComparison.OrdinalIgnoreCase) Then
                            _row.Item("ListPrice") = 40000
                            _row.Item("UnitPrice") = 40000
                            _row.Item("newUnitPrice") = 40000
                        ElseIf R.Item("CATEGORY_ID").ToString.Equals("AGS-CTOS-SYS-B", StringComparison.OrdinalIgnoreCase) Then
                            _row.Item("ListPrice") = 60000
                            _row.Item("UnitPrice") = 60000
                            _row.Item("newUnitPrice") = 60000
                        End If
                    End If

                    'Ryan 20171005 Add for AJP special PTrade items logic
                    If _IsAJPUser AndAlso Business.IsPTD(R.Item("CATEGORY_ID").ToString) Then
                        Dim AJPPtradePrice As Decimal = Business.GetAJPPTradePrice(_row.Item("UnitPrice"), R.Item("ITP"))
                        If _row.Item("UnitPrice") < AJPPtradePrice Then
                            _row.Item("ListPrice") = AJPPtradePrice
                            _row.Item("UnitPrice") = AJPPtradePrice
                            _row.Item("newUnitPrice") = AJPPtradePrice
                        End If
                    End If

                    _row.Item("HigherLevel") = intParentLineNo
                    _row.Item("RECFIGID") = ""
                    _row.Item("ItemType") = 0
                End If

                'Frank 20150213
                'ATW的報價單，Virtual Part No.預設帶出Part No.
                If _IsATWUser Or _IsACNUser Or _IsAJPUser Or _IsKRAonlineUser Then
                    _row.Item("VirtualPartNo") = _row.Item("PartNo")
                    _row.Item("DisplayLineNo") = _LineNo
                    'ICC 2015/3/9 ATW wants to show displayQty
                    If _row.Item("HigherLevel") = 0 Then
                        _row.Item("DisplayQty") = R.Item("CATEGORY_QTY")
                        _HigherLevelQty = R.Item("CATEGORY_QTY")
                    Else
                        _row.Item("DisplayQty") = R.Item("CATEGORY_QTY") / _HigherLevelQty
                    End If
                End If

                _row.Item("quoteId") = UID
                _row.Item("Line_No") = _LineNo

                dt1 = tbOPBase.dbGetDataTable("MY", "Select PART_NO,EXTENDED_DESC From SAP_PRODUCT_EXT_DESC Where PART_NO='" & _row.Item("PartNo") & "'")
                If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then
                    _row.Item("Description") = dt1.Rows(0).Item("EXTENDED_DESC").ToString
                Else
                    _row.Item("Description") = ""
                End If
                'Ryan 20170727 Replace BTO Item's desc for AJP IFS settings.
                If _IsAJPUser AndAlso String.Equals(R.Item("PARENT_CATEGORY_ID").ToString.Trim, "Root", StringComparison.CurrentCultureIgnoreCase) Then
                    Dim IFSDOC As Advantech.Myadvantech.DataAccess.AJP_ConfiguratorTerms = Advantech.Myadvantech.DataAccess.eQuotationDAL.Get_AJPTermsRecord(UID)
                    If Not IFSDOC Is Nothing AndAlso Not String.IsNullOrEmpty(IFSDOC.IFS_SOP) Then
                        _row.Item("Description") = IFSDOC.IFS_SOP
                    End If
                End If

                '_row.Item("Qty") = intReqQty
                _row.Item("QTY") = R.Item("CATEGORY_QTY")

                If _IsAUSUser AndAlso _row.Item("ItemType") = 0 Then
                    _row.Item("Itp") = Business.GetAUSCost(_row.Item("PartNo"), "USH1", UID)
                Else
                    _row.Item("Itp") = R.Item("ITP")
                End If


                _row.Item("newItp") = R.Item("NewITP")
                _row.Item("DeliveryPlant") = plant
                _row.Item("Category") = R.Item("PARENT_CATEGORY_ID").ToString.Replace("'", "''").ToUpper
                _row.Item("rohs") = 0
                _row.Item("satisfyFlag") = 0
                _row.Item("canBeConfirmed") = 1
                _row.Item("custMaterial") = ""
                _row.Item("inventory") = 0
                _row.Item("modelNo") = ""


                If R.Item("CATEGORY_ID").ToString.StartsWith("AGS-") Then
                    _row.Item("ewFlag") = 0
                Else
                    _row.Item("ewFlag") = ewFLAG
                End If

                _row.Item("reqDate") = Now
                _row.Item("dueDate") = Now

                _LineNo += 1

                _QuoteDetailDT.Rows.Add(_row)
            Next

            Dim bk As New System.Data.SqlClient.SqlBulkCopy(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
            bk.DestinationTableName = "QuotationDetail"
            bk.WriteToServer(_QuoteDetailDT)





            'Dim o As Boolean = Business.ADD2QUOTE_V2_1(UID, strBTORootItem, intReqQty, ewFLAG, COMM.Fixer.eItemType.Parent, "", 0, 0, Now, 0, plant, intParentLineNo, "", Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
            'If o Then


            '    If DTCOM.Rows.Count > 0 Then



            '        For Each R As DataRow In DTCOM.Select("CATEGORY_TYPE='Component'")
            '            If Not String.Equals(R.Item("CATEGORY_ID").ToString.Trim, "No Need", StringComparison.CurrentCultureIgnoreCase) Then
            '                If R.Item("CATEGORY_ID").ToString.Contains("|") Then
            '                    Dim ps() As String = Split(R.Item("CATEGORY_ID").ToString.ToUpper(), "|")

            '                    'Frank 2013/10/08 To group by part no by LINQ
            '                    Dim itemlist As New List(Of String)
            '                    For Each p As String In ps
            '                        itemlist.Add(p)
            '                    Next
            '                    Dim groups = itemlist.GroupBy(Function(value) value)
            '                    For Each grpitem In groups
            '                        If groups.Count < 1 Then Continue For
            '                        Dim cate As String = R.Item("PARENT_CATEGORY_ID").ToString.Replace("'", "''").ToUpper
            '                        Business.ADD2QUOTE_V2_1(UID, grpitem(0), (intReqQty * grpitem.Count), ewFLAG, COMM.Fixer.eItemType.Others, cate, 1, 1, Now, intParentLineNo, plant, 0, "", Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
            '                    Next
            '                Else
            '                    If R.Item("CATEGORY_ID").ToString.ToUpper().StartsWith("AGS-EW") Then
            '                        ewFLAG = Business.getMonthByEWItem(R.Item("CATEGORY_ID").ToString.ToUpper())
            '                    Else
            '                        Dim p As String = R.Item("CATEGORY_ID").ToString.ToUpper()
            '                        Dim cate As String = R.Item("PARENT_CATEGORY_ID").ToString.Replace("'", "''").ToUpper
            '                        Business.ADD2QUOTE_V2_1(UID, p.ToUpper, intReqQty, ewFLAG, COMM.Fixer.eItemType.Others, cate, 1, 1, Now, intParentLineNo, plant, 0, "", Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
            '                    End If
            '                End If
            '            End If
            '        Next
            '    End If

            'add Other Item
            'myQD.Update(String.Format("quoteId='{0}'", UID), String.Format("ewFlag='{0}'", ewFLAG))
            If IsDate(ddMaxDueDate) Then
                Dim myQD As New quotationDetail("EQ", "quotationDetail")
                ddMaxDueDate = DateAdd(DateInterval.Day, CInt(Pivot.NewObjDoc.getBTOWorkingDate(org)), ddMaxDueDate)
                myQD.Update(String.Format("quoteId='{0}' and itemType='" & COMM.Fixer.eItemType.Parent & "'", UID), String.Format("duedate='{0}'", CDate(ddMaxDueDate).ToShortDateString))
            End If
            Dim cF As IBUS.iCartF = Pivot.FactCart
            Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, org)
            If ewFLAG > 0 Then
                Dim Sline As New SortedSet(Of Integer)
                Sline.Add(intParentLineNo)
                cart.updateEW(Sline, Pivot.CurrentProfile.CurrDocReg, COMM.Fixer.eDocType.EQ, Currency)
            End If

            If Role.IsCNAonlineSales Then
                Dim _errmsg As String = String.Empty
                'Advantech.Myadvantech.Business.QuoteBusinessLogic.WriteACNQuoteLinesITP(UID, _errmsg)
            End If


            If Role.IsHQDCSales Then
                Dim _errmsg As String = String.Empty
                Advantech.Myadvantech.Business.QuoteBusinessLogic.WriteInterconQuoteLinesITP(UID, _errmsg)
            End If

            If Role.IsABRSales Then
                Dim ABRDiscount As Decimal = 0
                Dim abrdiscountdt As DataTable = tbOPBase.dbGetDataTable("EQ", "select * from QuotationExtensionABR where quoteId = '" + UID + "'")
                If abrdiscountdt Is Nothing OrElse abrdiscountdt.Rows.Count = 0 Then
                    ABRDiscount = 0
                Else
                    ABRDiscount = Convert.ToDecimal(abrdiscountdt.Rows(0).Item("discount").ToString)
                End If
                Advantech.Myadvantech.Business.QuoteBusinessLogic.UpdateSystemPriceForABRQuote(UID, ABRDiscount)
            End If

            'cart.UpdateByLine(String.Format("{0}='{1}'", cart.Item1.RECFIGID.Name, RECFIGID), intParentLineNo, COMM.Fixer.eDocType.EQ)
            Return True
        End If
        'End If
        Return False
    End Function
    Private Shared Sub RecursiveGetConfigResult(ByRef Comp As ConfiguredComponent, ByVal ParentCatId As String _
                                                , ByRef dt As DataTable, ByRef ConfigQty As Integer, ByVal intLevel As Integer _
                                                , ByVal UID As String)

        Dim compRow As DataRow = dt.NewRow()
        With compRow
            .Item("category_id") = Comp.CategoryId : .Item("Level") = intLevel : .Item("category_name") = Comp.CategoryId
            .Item("category_qty") = ConfigQty : .Item("PARENT_CATEGORY_ID") = ParentCatId
        End With
        Select Case Comp.CategoryType
            Case "category"
                compRow.Item("CATEGORY_TYPE") = "Category"
            Case "component"
                Dim _ListPrice As Decimal, _UnitPrice As Decimal
                compRow.Item("CATEGORY_TYPE") = "Component" : compRow.Item("category_price") = Business.GetPrice(Comp.CategoryId, UID)
                compRow.Item("CATEGORY_TYPE") = "Component" : compRow.Item("category_list_price") = _ListPrice : compRow.Item("category_price") = _UnitPrice
                compRow.Item("ATP_DATE") = GetATP(Comp.CategoryId, ConfigQty)
        End Select
        dt.Rows.Add(compRow)

        For Each childComp As ConfiguredComponent In Comp.ChildComps
            RecursiveGetConfigResult(childComp, Comp.CategoryId, dt, ConfigQty, intLevel + 1, UID)
        Next

    End Sub


    Private Shared Sub RecursiveGetConfigResultV2(ByRef Comp As ConfiguredComponent, ByVal ParentCatId As String, ByVal Org As String _
                                            , ByRef dt As DataTable, ByRef ConfigQty As Integer, ByVal intLevel As Integer _
                                            , ByVal UID As String)

        'Dim compRow As DataRow = dt.NewRow()
        Dim compRow As DataRow = Nothing
        Select Case Comp.CategoryType
            Case "category"

                compRow = dt.NewRow()
                With compRow
                    .Item("category_id") = Comp.CategoryId : .Item("Level") = intLevel : .Item("category_name") = Comp.CategoryId
                    .Item("category_qty") = ConfigQty : .Item("PARENT_CATEGORY_ID") = ParentCatId
                End With
                compRow.Item("ITP") = 0
                compRow.Item("NewITP") = 0
                compRow.Item("CATEGORY_TYPE") = "Category"
                dt.Rows.Add(compRow)

            Case "component"
                Dim _ListPrice As Decimal, _UnitPrice As Decimal, _RecyclingFee As Decimal = 0
                'compRow.Item("CATEGORY_TYPE") = "Component" : compRow.Item("category_price") = Business.GetPrice(Comp.CategoryId, UID)


                'If Comp.CategoryId.ToString.Contains("|") Then
                'Else
                'End If

                Dim ps() As String = Split(Comp.CategoryId.ToString.ToUpper(), "|")

                'Frank 2013/10/08 To group by part no by LINQ
                Dim itemlist As New List(Of String)
                For Each p As String In ps
                    itemlist.Add(p)
                Next
                Dim groups = itemlist.GroupBy(Function(value) value)
                For Each grpitem In groups
                    If groups.Count < 1 Then Continue For
                    'Dim cate As String = R.Item("PARENT_CATEGORY_ID").ToString.Replace("'", "''").ToUpper
                    'Business.ADD2QUOTE_V2_1(UID, grpitem(0), (intReqQty * grpitem.Count), ewFLAG, COMM.Fixer.eItemType.Others, cate, 1, 1, Now, intParentLineNo, plant, 0, "", Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
                    _ListPrice = 0 : _UnitPrice = 0
                    compRow = dt.NewRow()
                    With compRow
                        .Item("category_id") = grpitem(0) : .Item("Level") = intLevel : .Item("category_name") = Comp.CategoryId
                        .Item("category_qty") = ConfigQty * grpitem.Count : .Item("PARENT_CATEGORY_ID") = ParentCatId
                    End With

                    Business.GetPriceV2(grpitem(0), UID, Org, _ListPrice, _UnitPrice, _RecyclingFee)



                    Dim ITP As Decimal = 0
                    Business.GetITP(UID, grpitem(0), ITP, _UnitPrice)
                    compRow.Item("ITP") = ITP
                    compRow.Item("NewITP") = ITP
                    compRow.Item("CATEGORY_TYPE") = "Component" : compRow.Item("category_list_price") = _ListPrice : compRow.Item("category_price") = _UnitPrice : compRow.Item("CATEGORY_RECYCLE_FEE") = _RecyclingFee
                    compRow.Item("ATP_DATE") = GetATP(Comp.CategoryId, ConfigQty * grpitem.Count)
                    dt.Rows.Add(compRow)

                Next




                'Business.GetPriceV2(Comp.CategoryId, UID, _ListPrice, _UnitPrice)
                'compRow.Item("CATEGORY_TYPE") = "Component" : compRow.Item("category_list_price") = _ListPrice : compRow.Item("category_price") = _UnitPrice
                'compRow.Item("ATP_DATE") = GetATP(Comp.CategoryId, ConfigQty)
        End Select
        'dt.Rows.Add(compRow)

        For Each childComp As ConfiguredComponent In Comp.ChildComps
            RecursiveGetConfigResultV2(childComp, Comp.CategoryId, Org, dt, ConfigQty, intLevel + 1, UID)
        Next

    End Sub


    <Services.WebMethod()>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function GetCompPriceATP(ByVal ComponentCategoryId As String, ByVal ConfigQty As Integer, ByVal Currency As String, ByVal UID As String, ByVal ORG As String) As String
        Dim objPriceATP As New PriceATP

        With objPriceATP
            Dim WS As New CBOMWS.MyCBOMDAL
            Dim _UnitPrice As Decimal = 0, _ListPrice As Decimal = 0
            '.Price = Business.GetPrice(ComponentCategoryId, UID) : .ATPDate = GetATP(ComponentCategoryId, ConfigQty).ToString("yyyy/MM/dd") : .ATPQty = ConfigQty
            Business.GetPriceV2(ComponentCategoryId, UID, ORG, _ListPrice, _UnitPrice)
            .Price = _UnitPrice : .ListPrice = _ListPrice
            .ATPDate = GetATP(ComponentCategoryId, ConfigQty).ToString("yyyy/MM/dd") : .ATPQty = ConfigQty
            If ComponentCategoryId.StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase) Then
                .CurrencySign = "" : .IsEw = True
            Else
                .CurrencySign = COMM.Util.getCurrSign(Currency) : .IsEw = False
            End If

        End With
        Dim serializer = New Script.Serialization.JavaScriptSerializer()
        Return serializer.Serialize(objPriceATP)
    End Function

    <Services.WebMethod()>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function GetCBOM(ByVal ParentCategoryId As String, ByVal ConfigQty As Integer, ByVal RBU As String, ByVal ORG As String, ByVal RootId As String) As String
        'Return ParentCategoryId
        Try
            Dim cBomDal As New CBOMWS.MyCBOMDAL

            'Frank 20150930 測試臨時把url切換到MyA測試機，測完還是要把以下三行註解掉
            If COMM.Util.IsTesting Then
                cBomDal.Url = "http://my.advantech.com:4002/Services/MyCBOMDAL.asmx?wsdl"
                'cBomDal.Url = "http://localhost:4002/Services/MyCBOMDAL.asmx?wsdl"
            Else
                cBomDal.Url = "http://my.advantech.com/Services/MyCBOMDAL.asmx?wsdl"
            End If

            Dim dtBom As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYDataTable = cBomDal.GetCBOM2(ParentCategoryId, RBU, ORG, RootId)
            Dim lsCBom As New List(Of CBom)
            For Each rBom As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYRow In dtBom.Rows
                Dim bom1 As New CBom
                bom1.ChildCategories = New List(Of CBom)
                With bom1
                    .CategoryType = rBom.CATEGORY_TYPE : .CategoryId = rBom.CATEGORY_ID : .Description = rBom.CATEGORY_NAME
                    .IsCatRequired = IIf(String.Equals(rBom.CONFIGURATION_RULE, "REQUIRED", StringComparison.CurrentCultureIgnoreCase), True, False)
                    '.ClientId = AEUIT_Rijndael.EncryptDefault(rBom.CATEGORY_ID)
                    .CalcClientId(rBom.PARENT_CATEGORY_ID + rBom.CATEGORY_ID + rBom.SEQ_NO.ToString())
                    .IsCompRoHS = False
                    .ExtDescription = rBom.EXT_DESCRIPTION 'ICC 2015/4/10 Add extended description from MyA ws.
                End With
                lsCBom.Add(bom1)
                If String.Equals(bom1.CategoryType, "Category", StringComparison.CurrentCultureIgnoreCase) Or String.Equals(bom1.CategoryType, "extendedcategory", StringComparison.CurrentCultureIgnoreCase) Then
                    Dim dtComps As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYDataTable = cBomDal.GetCBOM2(bom1.CategoryId, RBU, ORG, RootId)
                    For Each rComp As CBOMWS.CBOMDS.CBOM_CATALOG_CATEGORYRow In dtComps.Rows
                        Dim compBom As New CBom
                        With compBom
                            .CategoryId = rComp.CATEGORY_ID : .CategoryType = rComp.CATEGORY_TYPE : .IsCatRequired = False : .Description = rComp.CATEGORY_DESC
                            '.ClientId = AEUIT_Rijndael.EncryptDefault(rComp.CATEGORY_ID)
                            .CalcClientId(rComp.PARENT_CATEGORY_ID + rComp.CATEGORY_ID + rComp.SEQ_NO.ToString())
                            .IsCompDefault = IIf(String.Equals(rComp.CONFIGURATION_RULE, "DEFAULT", StringComparison.CurrentCultureIgnoreCase), True, False)
                            .IsCompRoHS = IIf(String.Equals(rComp.RoHS, "y", StringComparison.CurrentCultureIgnoreCase), True, False)

                            'Ryan 20160719 Add for EU hot icon validation for Ptrade products
                            If Pivot.CurrentProfile.getCurrOrg.Equals("EU10", StringComparison.OrdinalIgnoreCase) Then
                                If Business.IsPTD(rComp.CATEGORY_ID) Then
                                    Dim abc_indicator As String = Advantech.Myadvantech.Business.PartBusinessLogic.GetABCIndicator(rComp.CATEGORY_ID, "EUH1")
                                    .IsHot = IIf((abc_indicator.StartsWith("A", StringComparison.OrdinalIgnoreCase) OrElse abc_indicator.StartsWith("B", StringComparison.OrdinalIgnoreCase)), True, False)
                                Else
                                    .IsHot = False
                                End If
                            Else
                                .IsHot = False
                            End If

                        End With
                        bom1.ChildCategories.Add(compBom)
                    Next
                End If
            Next
            Dim serializer = New Script.Serialization.JavaScriptSerializer()
            Return serializer.Serialize(lsCBom)
        Catch ex As Exception
            Return ex.ToString()
        End Try

    End Function
    Public Function IsEstoreBom(ByVal BTORootID As String, ByVal Org As String) As Boolean
        If BTORootID.StartsWith("EZ-", StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If

        'Frank 2013/02/19:If Catalog local name is Pre-Configuration for AEU eStore (buy.advantech.eu) Configuration,
        'then it's a AEU eStore bom
        If Org.ToString().StartsWith("EU", StringComparison.OrdinalIgnoreCase) Then
            Dim _sql As New StringBuilder
            _sql.AppendLine(" Select a.Catalog_Org,a.CATALOG_TYPE,b.LOCAL_NAME,a.CATALOG_ID,a.CATALOG_NAME,a.CATALOG_DESC, a.CREATED ")
            _sql.AppendLine(" From CBOM_CATALOG a inner join CBOM_CATALOG_LOCALNAME b on a.CATALOG_TYPE=b.CATALOG_TYPE ")
            _sql.AppendLine(" Where a.Catalog_Org='EU' and a.CATALOG_TYPE like '%Pre-Configuration' and a.CATALOG_NAME ='" & Request("BTOITEM") & "' ")
            Dim _dt As DataTable = tbOPBase.dbGetDataTable("MY", _sql.ToString)
            If Not IsNothing(_dt) AndAlso _dt.Rows.Count > 0 AndAlso _
                _dt.Rows(0).Item("LOCAL_NAME").ToString.Equals("Pre-Configuration for AEU eStore (buy.advantech.eu) Configuration", _
                StringComparison.InvariantCultureIgnoreCase) Then
                Return True
            End If
        End If
        Dim ObjectEZ_FLAG As Object = tbOPBase.dbExecuteScalar("B2B",
                                                               String.Format("SELECT ISNULL(COUNT(BTONo),0) as Bcount  FROM  ESTORE_BTOS_CATEGORY WHERE  DisplayPartno ='{1}' and StoreID like '%{0}'", _
                                                                     Left(Org.ToUpper, 2), BTORootID.Trim))
        If ObjectEZ_FLAG IsNot Nothing AndAlso Integer.TryParse(ObjectEZ_FLAG, 0) AndAlso Integer.Parse(ObjectEZ_FLAG) > 0 Then
            Return True
        End If
        Return False
    End Function
    Private Sub SetSourcePath(ByVal strBTOItem As String, ByVal intConfigQty As Integer, ByVal Org As String)
        Dim strhtml As String = "", UID As String = String.Empty
        If Request("UID") IsNot Nothing Then UID = Request("UID")

        strhtml = "<font color='Navy'>■</font>&nbsp;&nbsp;<a href='./Catalog.aspx?UID=" + UID + "' target='_self' style='color:Navy;font-weight:bold; text-decoration:none;'>System Configuration/Ordering Portal</a><strong>&nbsp;&nbsp;>&nbsp;&nbsp;</strong>"
        If IsEstoreBom(strBTOItem, Pivot.CurrentProfile.getCurrOrg) Then
            strhtml += "<a href='./CBOM_eStoreBTO_List1.aspx?UID=" + UID + "' target='_self' style='color:Navy;font-weight:bold;text-decoration:none;'>" + "eStore BTOS" + "</a><strong>&nbsp;&nbsp;>&nbsp;&nbsp;</strong>"
        Else
            strhtml += "<a href='./CBOM_List.aspx?Catalog_Type=" + get_catalog_type(Trim(Request("BTOITEM")), Org) + "&UID=" + UID + "' target='_self' style='color:Navy;font-weight:bold;text-decoration:none;'>" + get_catalog_type(Trim(Request("BTOITEM")), Org, 1) + "</a><strong>&nbsp;&nbsp;>&nbsp;&nbsp;</strong>"
        End If
        strhtml += "<a href='./Configurator.aspx?BTOITEM=" + strBTOItem + "&QTY=" + intConfigQty.ToString() + "&UID=" + UID + "' target='_self' style='color:Navy;font-weight:bold; text-decoration:none;'>" + strBTOItem + "</a>"

        page_path.InnerHtml = strhtml
    End Sub

    Private Shared Function get_catalog_type(ByVal name As String, ByVal ORG As String, Optional ByVal Flag As Integer = 0) As String
        Dim catalog_name As String = ""
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", "select catalog_type from CBOM_CATALOG where Catalog_org='" & Left(ORG.ToString.ToUpper, 2) & "' and CATALOG_NAME = '" + name + "'")
        If dt.Rows.Count > 0 Then
            If Not Convert.IsDBNull(dt.Rows(0).Item("catalog_type")) Then
                catalog_name = dt.Rows(0).Item("catalog_type").ToString.Trim
            End If
        End If
        If Flag = 1 Then
            Dim CBOMWS As New CBOMWS.MyCBOMDAL
            Return CBOMWS.getCatalogLocalName(catalog_name, Left(Pivot.CurrentProfile.getCurrOrg, 2))
        Else
            Return catalog_name
        End If
    End Function

    Public Shared Function GetATP(ByVal PartNo As String, ByVal ReqQty As Integer) As Date
        'If PartNo = "No Need" OrElse PartNo = "None" Then Return Now.ToString("yyyy/MM/dd")
        If PartNo.ToLower = MyExtension.BuildIn.ToLower OrElse PartNo = "None" Then Return Now.ToString("yyyy/MM/dd")
        If PartNo.ToUpper.StartsWith("AGS-EW") Then
            Return Now.ToString("yyyy/MM/dd")
        End If
        Dim due_date As String = Now.ToString("yyyy/MM/dd")
        Dim org As String = Pivot.CurrentProfile.getCurrOrg
        SAPTools.getInventoryAndATPTable(PartNo, Business.getPlantByOrgID(org), ReqQty, due_date, 0, Nothing, "", 1, 0)
        Return CDate(due_date).ToString("MM/dd/yyyy")
    End Function

    Private Shared Function ORG2ViewORG(ByVal ORGID As String) As String
        ORGID = ORGID.ToUpper

        If ORGID.Equals("JP") Then
            Return "TW"
        Else
            Return ORGID
        End If
    End Function

    '/Page logic
End Class