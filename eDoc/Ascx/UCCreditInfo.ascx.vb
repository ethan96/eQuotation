Public Class UCCreditInfo
    Inherits System.Web.UI.UserControl

    Public RequestID As String = ""
    Public isBalanceExpired As Boolean = False
    Public ERPID As String = "", ORG As String = "", CE As Decimal = 0, CL As Decimal = 0, P As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub showCreditInfo(ByVal companyid As String)
        If companyid <> "" Then
            Dim org As String = Business.getOrgByErpId(companyid)
            If org <> "" Then
                Dim _temporg As String = org
                Me.lbORG.Text = org
                Me.lbERPID.Text = companyid

                Dim cld As Advantech.Myadvantech.DataAccess.CreditLimitData = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetCustomerCreditExposure(companyid, org)

                Me.lbCreditControlArea.Text = cld.CreditControlAreaOption.ToString
                Me.lbCurrency.Text = cld.Currency.ToString
                Me.lbCreditLimit.Text = Decimal.Round(cld.CreditLimit, 0).ToString("N0")
                Me.lbCreditExposure.Text = Decimal.Round(cld.CreditExposure, 0).ToString("N0")
                'Me.lbPercentage.Text = cld.Percentage
                Me.lbPercentage.Text = Math.Round(cld.CreditLimitUsed * 100, 2, MidpointRounding.AwayFromZero) & " %"
                Me.lbReceivables.Text = Decimal.Round(cld.Receivables, 0).ToString("N0")
                Me.lbSpecialLiability.Text = Decimal.Round(cld.SpecialLiability, 0).ToString("N0")
                Me.lbSalesValue.Text = Decimal.Round(cld.SalesValue, 0).ToString("N0")
                Me.lbRiskCategory.Text = cld.RiskCategory

                If cld.Blocked Then
                    Me.lbBlocked.Text = "Yes"
                Else
                    Me.lbBlocked.Text = ""
                End If

                If cld.CreditLimitUsed > 1 Then
                    Me.isBalanceExpired = True
                    Me.lbPercentage.BackColor = Drawing.Color.Yellow
                End If

                Exit Sub
            End If
        End If
    End Sub

End Class