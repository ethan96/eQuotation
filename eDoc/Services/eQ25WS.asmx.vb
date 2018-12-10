Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class eQ25WS
    Inherits System.Web.Services.WebService

    <WebMethod(EnableSession:=True)>
    Public Function GetAEUTemplateHtml(ByVal QuoteID As String, ByVal UserID As String, ByVal TempID As String) As String
        If Not User.Identity.IsAuthenticated Then
            Dim QM As Quote_Master = MyUtil.Current.CurrentDataContext.Quote_Masters.Where(Function(p) p.quoteId = QuoteID).FirstOrDefault
            If QM IsNot Nothing Then
                Pivot.NewObjProfile.setSession(QM.createdBy, "en-US", "")
            End If
            'If Pivot.NewObjPSSO.isValidSSOMember(COMM.Util.GetClientIP(), TempID, UserID) Then
            'End If
            FormsAuthentication.SetAuthCookie(QM.createdBy, False)
        End If
        Try
            Return Util.GetAllStringForAEU(QuoteID)
        Catch ex As Exception
            Return ex.Message.ToString
        End Try
        Return String.Empty
    End Function

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub GetEOLItems(ByVal Root As String, ByVal ORGID As String)
        Try
            Dim EOLItems As List(Of String) = Advantech.Myadvantech.Business.PartBusinessLogic.GetConfiguratorEOLItems(Root, ORGID)
            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(EOLItems))
        Catch ex As Exception
            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(New List(Of String)))
        End Try
    End Sub

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub IsAKRPartsNeedConfirm(ByVal _InputParts As String)
        Try
            Dim Parts As List(Of String) = _InputParts.Split(";").ToList
            Dim InvaildParts As List(Of String) = New List(Of String)

            For Each Part As String In Parts
                Dim c As Integer = tbOPBase.dbExecuteScalar("MY", String.Format(" select count(part_no) as c from SAP_PRODUCT_STATUS where SALES_ORG = 'KR01' and PRODUCT_STATUS = 'A' and part_no = '{0}'", Part))
                If c = 0 Then
                    InvaildParts.Add(Part)
                End If
            Next

            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(InvaildParts))
        Catch ex As Exception
            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(New List(Of String)))
        End Try
    End Sub

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub IsBTOSParentSBCBTO(ByVal _QuoteID As String, ByVal _ParentLineNo As String)
        Try
            Dim Result As Boolean = False

            Dim BTOSParent As QuoteItem = MyQuoteX.GetQuoteItem(_QuoteID, _ParentLineNo)
            If BTOSParent IsNot Nothing AndAlso BTOSParent.partNo.ToUpper.Equals("SBC-BTO") Then
                Result = True
            End If

            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Result))
        Catch ex As Exception
            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(False))
        End Try
    End Sub

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub UpdateSBCBTOtoPAPOSBTO(ByVal _QuoteID As String, ByVal _ParentLineNo As String)
        Try
            Dim BTOSParent As QuoteItem = MyQuoteX.GetQuoteItem(_QuoteID, _ParentLineNo)
            If BTOSParent IsNot Nothing AndAlso BTOSParent.partNo.ToUpper.Equals("SBC-BTO") Then
                BTOSParent.partNo = "PAP-OS-BTO"
                MyUtil.Current.CurrentDataContext.SubmitChanges()
            End If

            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(True))
        Catch ex As Exception
            System.Web.HttpContext.Current.Response.Clear()
            System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(False))
        End Try
    End Sub
End Class
