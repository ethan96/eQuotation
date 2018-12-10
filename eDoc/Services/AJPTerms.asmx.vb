Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class AJPTerms
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub AJPTerm1(ByVal QuoteID As String, ByVal HW_COMUSB As String, ByVal HW_Storage As String, ByVal HW_SATA As String, ByVal HW_Disk_Division As String,
                                    ByVal HW_Cable As String, ByVal HW_BIOS As String, ByVal HW_OS_License As String, ByVal HW_OS_Activation As String, ByVal HW_Others As String, ByVal IFSSOP As String)
        Dim result As Boolean = False
        Dim edit As Boolean = False

        Try
            Dim AJP_Entity As Advantech.Myadvantech.DataAccess.AJP_ConfiguratorTerms = Advantech.Myadvantech.DataAccess.eQuotationDAL.Get_AJPTermsRecord(QuoteID)
            If AJP_Entity Is Nothing Then
                AJP_Entity = New Advantech.Myadvantech.DataAccess.AJP_ConfiguratorTerms
            Else
                edit = True
            End If
            AJP_Entity.QuoteID = QuoteID
            AJP_Entity.HW_COMUSB = HW_COMUSB
            AJP_Entity.HW_Storage = HW_Storage
            AJP_Entity.HW_SATA = HW_SATA
            AJP_Entity.HW_Disk_Division = HW_Disk_Division
            AJP_Entity.HW_Cable = HW_Cable
            AJP_Entity.HW_BIOS = HW_BIOS
            AJP_Entity.HW_OS_License = HW_OS_License
            AJP_Entity.HW_OS_Activation = HW_OS_Activation
            AJP_Entity.HW_Others = HW_Others
            AJP_Entity.IFS_SOP = IFSSOP

            If Not edit Then Advantech.Myadvantech.DataAccess.eQuotationContext.Current.AJP_ConfiguratorTerms.Add(AJP_Entity)
            Advantech.Myadvantech.DataAccess.eQuotationContext.Current.SaveChanges()
            result = True
        Catch ex As Exception

        End Try

        System.Web.HttpContext.Current.Response.Clear()
        System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result))
    End Sub

    <WebMethod(EnableSession:=True)> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)> _
    Public Sub AJPTerm2(ByVal QuoteID As String, ByVal SW_OS_Installation As String, ByVal SW_Username As String, ByVal SW_OS_Timezone As String, ByVal SW_OS_Input As String,
                                    ByVal SW_IP_Settings As String, ByVal SW_Settings As String, ByVal SW_Others As String)
        Dim result As Boolean = False
        Dim edit As Boolean = False

        Try
            Dim AJP_Entity As Advantech.Myadvantech.DataAccess.AJP_ConfiguratorTerms = Advantech.Myadvantech.DataAccess.eQuotationDAL.Get_AJPTermsRecord(QuoteID)
            If AJP_Entity Is Nothing Then
                AJP_Entity = New Advantech.Myadvantech.DataAccess.AJP_ConfiguratorTerms
            Else
                edit = True
            End If
            AJP_Entity.QuoteID = QuoteID
            AJP_Entity.SW_OS_Installation = SW_OS_Installation
            AJP_Entity.SW_Username = SW_Username
            AJP_Entity.SW_OS_Timezone = SW_OS_Timezone
            AJP_Entity.SW_OS_Input = SW_OS_Input
            AJP_Entity.SW_IP_Settings = SW_IP_Settings
            AJP_Entity.SW_Settings = SW_Settings
            AJP_Entity.SW_Others = SW_Others

            If Not edit Then Advantech.Myadvantech.DataAccess.eQuotationContext.Current.AJP_ConfiguratorTerms.Add(AJP_Entity)
            Advantech.Myadvantech.DataAccess.eQuotationContext.Current.SaveChanges()
            result = True
        Catch ex As Exception

        End Try

        System.Web.HttpContext.Current.Response.Clear()
        System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result))
    End Sub

    <WebMethod(EnableSession:=True)> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)> _
    Public Sub AJPTerm3(ByVal QuoteID As String, ByVal FINAL_HWSummary As String, ByVal FINAL_SWSummary As String)
        Dim result As Boolean = False
        Dim edit As Boolean = False

        Try
            Dim AJP_Entity As Advantech.Myadvantech.DataAccess.AJP_ConfiguratorTerms = Advantech.Myadvantech.DataAccess.eQuotationDAL.Get_AJPTermsRecord(QuoteID)
            If AJP_Entity Is Nothing Then
                AJP_Entity = New Advantech.Myadvantech.DataAccess.AJP_ConfiguratorTerms
            Else
                edit = True
            End If
            AJP_Entity.QuoteID = QuoteID
            AJP_Entity.FINAL_HWSummary = FINAL_HWSummary
            AJP_Entity.FINAL_SWSummary = FINAL_SWSummary

            If Not edit Then Advantech.Myadvantech.DataAccess.eQuotationContext.Current.AJP_ConfiguratorTerms.Add(AJP_Entity)
            Advantech.Myadvantech.DataAccess.eQuotationContext.Current.SaveChanges()
            result = True
        Catch ex As Exception

        End Try

        System.Web.HttpContext.Current.Response.Clear()
        System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result))
    End Sub

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub GetAJPTermValue(ByVal QuoteID As String)
        Try
            Dim AJP_Entity As Advantech.Myadvantech.DataAccess.AJP_ConfiguratorTerms = Advantech.Myadvantech.DataAccess.eQuotationDAL.Get_AJPTermsRecord(QuoteID)
            If AJP_Entity Is Nothing Then
                Exit Sub
            Else
                System.Web.HttpContext.Current.Response.Clear()
                System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(AJP_Entity))
            End If
        Catch ex As Exception

        End Try
    End Sub

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub SaveAJPTemplateChanges(ByVal _QuoteID As String, ByVal _CustomerName As String, ByVal _CustomerName2 As String,
                                      ByVal _CustomerAddr As String, ByVal _CustomerAddr2 As String, ByVal _CustomerTel As String, ByVal _CustomerFax As String,
                                      ByVal _CustomerContact As String, ByVal _CustomerEM As String, ByVal _CustomerPaymentTerm As String,
                                      ByVal _CustomerShipMethod As String, ByVal _SalesName As String, ByVal _Creator As String, ByVal _Notes As String, ByVal _BTOSParent As String)
        Try
            Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(_QuoteID)
            'Dim _QME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(_QuoteID)
            Dim _QD As List(Of QuoteItem) = MyQuoteX.GetQuoteBTOSParentItems(_QuoteID)

            If Not String.IsNullOrEmpty(_Notes) Then
                _QM.quoteNote = _Notes
            End If

            If Not String.IsNullOrEmpty(_CustomerContact) Then
                _QM.attentionEmail = _CustomerContact
            End If

            If Not String.IsNullOrEmpty(_BTOSParent) Then
                _QD.Where(Function(p) p.line_No = 100).FirstOrDefault.VirtualPartNo = _BTOSParent
            End If
            MyUtil.Current.CurrentDataContext.SubmitChanges()


            tbOPBase.dbExecuteNoQuery("EQ", String.Format(" delete FROM QuotationExtensionAJP WHERE QUOTEID = '{0}'", _QuoteID))
            Dim str As String = String.Format("insert into QuotationExtensionAJP ")
            str += String.Format("values (N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}', N'{6}', N'{7}', N'{8}', N'{9}', N'{10}', N'{11}', N'{12}')" _
                                 , _QuoteID, _CustomerName, _CustomerName2, _CustomerAddr, _CustomerAddr2, _CustomerTel, _CustomerFax _
                                 , _CustomerContact, _CustomerEM, _CustomerPaymentTerm, _CustomerShipMethod, _SalesName, _Creator)
            tbOPBase.dbExecuteNoQuery("EQ", str)

            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(True))
        Catch ex As Exception

        End Try
    End Sub

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub CheckAJPConfigurationFor207Parts(ByVal RootComp As String)
        Try
            Dim root As EDOC.Configurator.ConfiguredComponent = Newtonsoft.Json.JsonConvert.DeserializeObject(Of EDOC.Configurator.ConfiguredComponent)(RootComp)
            Dim components As List(Of String) = New List(Of String)
            RecursiveGetAllComponents(root, components)

            If components.Contains("AGS-CTOS-SYS-A") AndAlso
                (components.Where(Function(p) p.StartsWith("968T")).Any OrElse components.Where(Function(p) p.StartsWith("968Q")).Any) AndAlso
                Not components.Where(Function(p) p.StartsWith("207")).Any Then
                System.Web.HttpContext.Current.Response.Clear()
                System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(True))
            ElseIf components.Contains("AGS-CTOS-SYS-B") AndAlso Not components.Where(Function(p) p.StartsWith("207")).Any Then
                System.Web.HttpContext.Current.Response.Clear()
                System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(True))
            Else
                System.Web.HttpContext.Current.Response.Clear()
                System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(False))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Shared Sub RecursiveGetAllComponents(ByRef Node As EDOC.Configurator.ConfiguredComponent, ByRef Components As List(Of String))
        If Node.CategoryType.Equals("component") Then
            Dim ps() As String = Split(Node.CategoryId.ToString.ToUpper(), "|")
            For Each p As String In ps
                Components.Add(p)
            Next
        End If
        If Not Node.ChildComps Is Nothing AndAlso Node.ChildComps.Count > 0 Then
            For Each Child As EDOC.Configurator.ConfiguredComponent In Node.ChildComps
                RecursiveGetAllComponents(Child, Components)
            Next
        End If
    End Sub


End Class