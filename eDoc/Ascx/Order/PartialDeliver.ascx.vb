Public Class PartialDeliver
    Inherits System.Web.UI.UserControl
    'Private _isFromExcel As Boolean = False
    'Public Property isFromExcel As Boolean
    '    Get
    '        Return _isFromExcel
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _isFromExcel = value
    '    End Set
    'End Property
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


                If QM.org.ToString.Trim.Equals("EU10", StringComparison.OrdinalIgnoreCase) Then
                    If Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, QM.Key, QM.org).isPartialAble() Then
                        pdtb.Visible = False
                    Else
                        If Pivot.NewObjCustomer.isCustomerCompleteDeliv(QM.AccErpId, QM.org) Then
                            Me.rbtnIsPartial.SelectedValue = "0"
                        End If
                        'If _isFromExcel Then
                        '    rbtnIsPartial.SelectedValue = "1"
                        'End If
                    End If
                Else
                    rbtnIsPartial.SelectedValue = "0"
                    'If mycart.isOnlyComponentOrder(CartId) Then
                    'ElseIf mycart.isOnlyBtoOrder(CartId) Then
                    '    rbtnIsPartial.Enabled = False
                    'Else
                    'End If
                    'If mycart.isOnlyBtoOrder(CartId) Then
                    '      rbtnIsPartial.Enabled = False
                    '  End If
                End If
            End If
        End If

    End Sub
End Class