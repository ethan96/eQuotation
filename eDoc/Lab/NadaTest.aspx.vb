Public Class NadaTest_
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim sap As IBUS.iSAP = Pivot.NewObjSAPTools
        'Dim Header As IBUS.iDocHeaderLine = Pivot.NewLineHeader
        'Header.DocRealType = "ZOR2"
        'Header.Key = "TESTNADA1"
        'Header.DIST_CHAN = "30"
        'Header.DIVISION = "20"
        'Header.org = "US01"

        'Dim Detail As IBUS.iCartList = Pivot.NewObjCartList

        'Dim Line1 As IBUS.iCartLine = Pivot.NewLineCart("", 1, 0, "ADAM-4012-DE", "", 0, 0, 0, 999, 0, "USH1", Now.ToShortDateString, COMM.Fixer.eItemType.Others, "")
        'Dim Line2 As IBUS.iCartLine = Pivot.NewLineCart("", 2, 1, "ADAM-4520-D2E", "", 0, 0, 0, 1000, 0, "USH1", Now.ToShortDateString, COMM.Fixer.eItemType.Others, "")

        'Dim Line As IBUS.iCartLine = Pivot.NewLineCart("", 3, 1, "96HD2000G-ST-SG7K2", "", 0, 0, 0, 100, 0, "USH1", Now.ToShortDateString, COMM.Fixer.eItemType.Others, "")

        'Detail.Add(Line1)
        'Detail.Add(Line2)
        'Detail.Add(Line)
        'Dim P As New List(Of IBUS.iPartnerLine)
        'Dim PL As IBUS.iPartnerLine = Pivot.NewLinePartner()
        'PL.TYPE = "AG"
        'PL.ERPID = "UJEL5010"
        'Dim PL1 As IBUS.iPartnerLine = Pivot.NewLinePartner()
        'PL1.TYPE = "WE"
        'PL1.ERPID = "UJEL5010"

        'P.Add(PL) : P.Add(PL1)
        'Dim RET As New DataTable
        'sap.OrderSimulate(Header, P, RET, Detail, New List(Of IBUS.iCreditLine), New List(Of IBUS.iCondLine))
        'sap.OrderCreate(Header, P, RET, "", Detail, New List(Of IBUS.iDocTextLine), New List(Of IBUS.iCondLine), New List(Of IBUS.iCreditLine), True)
    End Sub

   
End Class