Public Class soldtoshipto
    Inherits System.Web.UI.UserControl

     Private _QM As IBUS.iDocHeaderLine = Nothing
    Public Property QM As IBUS.iDocHeaderLine
        Get
            Return _QM
        End Get
        Set(ByVal value As IBUS.iDocHeaderLine)
            _QM = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not IsNothing(QM) Then

                Dim P As List(Of IBUS.iPartnerLine) = Pivot.NewObjPartner.GetListAll(QM.Key, COMM.Fixer.eDocType.ORDER)
                For Each r As IBUS.iPartnerLine In P
                    If r.TYPE.Equals("SOLDTO", StringComparison.OrdinalIgnoreCase) Then
                        litsoldto.Text = r.ERPID
                        litsoldtocompany.Text = r.NAME
                        litsoldtoaddress.Text = r.STREET + " " + r.CITY + " " + r.STATE + " " + r.COUNTRY + " " + r.ZIPCODE
                        litsoldtotel.Text = r.TEL
                        litsoldtoattention.Text = r.ATTENTION
                    End If
                    If r.TYPE.Equals("S", StringComparison.OrdinalIgnoreCase) Then
                        litshipto.Text = r.ERPID
                        litshiptocompany.Text = r.NAME
                        litshiptoaddress.Text = r.STREET + " " + r.CITY + " " + r.STATE + " " + r.COUNTRY + " " + r.ZIPCODE
                        litshiptoaddress2.Text = r.STREET2
                        litshiptotel.Text = r.TEL
                        litshiptoattention.Text = r.ATTENTION
                    End If
                Next
            End If
        End If
    End Sub

End Class