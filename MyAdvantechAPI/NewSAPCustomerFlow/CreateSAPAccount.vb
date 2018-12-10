Imports System.Activities
Imports Advantech.Myadvantech.DataAccess

Public NotInheritable Class CreateSAPAccount
    Inherits CodeActivity

    Property Text() As InArgument(Of String)
    Property IsCreateSholdTo() As InArgument(Of Boolean)
    Property IsCreateShipTo() As InArgument(Of Boolean)
    Property IsCreateBillTo() As InArgument(Of Boolean)
    Protected Overrides Sub Execute(ByVal context As CodeActivityContext)
        Dim IsSholdTo As Boolean = context.GetValue(Me.IsCreateSholdTo)
        Dim IsShipTo As Boolean = context.GetValue(Me.IsCreateShipTo)
        Dim IsBillTo As Boolean = context.GetValue(Me.IsCreateBillTo)
        Dim retMsg As String = String.Empty
        Dim _currApp As SA_APPLICATION = AppUtil.getAppByInstanceID(context.WorkflowInstanceId.ToString())
        If _currApp IsNot Nothing Then
            If IsSholdTo Then
                SAPDAL.CreateSAPAccount(_currApp.ID.ToString(), companyType.SholdTo, AppUtil.IsTesting(), retMsg)
            End If
            If IsShipTo Then
                SAPDAL.CreateSAPAccount(_currApp.ID.ToString(), companyType.ShipTo, AppUtil.IsTesting(), retMsg)
            End If
            If IsBillTo Then
                SAPDAL.CreateSAPAccount(_currApp.ID.ToString(), companyType.BillTo, AppUtil.IsTesting(), retMsg)
            End If
            Dim currInStanceID As String = context.WorkflowInstanceId.ToString()
            AppUtil.UpdateApplicationStatus(_currApp.ID, AccountWorkFlowStatus.Approved, currInStanceID)
        End If
    End Sub
End Class
