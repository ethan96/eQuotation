

Public Class Software
    Inherits System.Web.UI.UserControl
    Dim UID As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Role.IsJPAonlineSales Then
            If (Not Request("UID") Is Nothing AndAlso Not String.IsNullOrEmpty(Request("UID").ToString)) Then
                UID = Request("UID").ToString

                'Ryan 20170726 Settings for IFS SOP
                If (Not Request("BTOITEM") Is Nothing AndAlso Not String.IsNullOrEmpty(Request("BTOITEM").ToString)) Then
                    Dim QM As Quote_Master = MyQuoteX.GetQuoteMaster(UID)
                    Dim BTO As String = Request("BTOITEM").ToString
                    Dim ERPID As String = IIf(String.IsNullOrEmpty(QM.quoteToErpId), "", QM.quoteToErpId)

                    Dim ShipTO As Advantech.Myadvantech.DataAccess.EQPARTNER = Advantech.Myadvantech.DataAccess.eQuotationDAL.GetEQPartner(UID).Where(Function(p) p.TYPE.Equals("S")).FirstOrDefault
                    If ShipTO IsNot Nothing AndAlso Not String.IsNullOrEmpty(ShipTO.ERPID) Then
                        ERPID = ShipTO.ERPID
                    End If

                    ddlIFS.Items.Add(New ListItem("DefaultSOP", "DefaultSOP"))

                    'Ryan 20170727 Getting SOP doc from IFS web service 
                    Dim remoteAddress As New System.ServiceModel.EndpointAddress("http://ifs.advantech.co.jp/services/ctos/CTOSDocTunnel.svc")
                    Dim ws As New Advantech.Myadvantech.DataAccess.CTOSDocTunnel.CTOSDocTunnelClient(New System.ServiceModel.BasicHttpBinding(), remoteAddress)
                    Dim items As Advantech.Myadvantech.DataAccess.CTOSDocTunnel.CTOSDocItem() = ws.GetCustomerDocumentList(ERPID, BTO)

                    For Each item As Advantech.Myadvantech.DataAccess.CTOSDocTunnel.CTOSDocItem In items
                        If Not String.IsNullOrEmpty(item.CustomerPartNO) AndAlso Not String.IsNullOrEmpty(item.DocumentID) Then
                            ddlIFS.Items.Add(New ListItem(item.CustomerPartNO, item.DocumentID))
                        End If
                    Next

                End If
            End If

            SetValuetoForm()
        End If
    End Sub

    Public Sub SetValuetoForm()


    End Sub

End Class