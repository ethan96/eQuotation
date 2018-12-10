Imports System.Activities
Imports Advantech.Myadvantech.DataAccess

Public NotInheritable Class UpdateSiebelAccountERPID
    Inherits CodeActivity

    'Define an activity input argument of type String
    Property Text() As InArgument(Of String)
    Protected Overrides Sub Execute(ByVal context As CodeActivityContext)
        Dim text As String = context.GetValue(Me.Text)
        Dim _currApp As SA_APPLICATION = AppUtil.getAppByInstanceID(context.WorkflowInstanceId.ToString())
        If _currApp IsNot Nothing Then
            SiebelDAL.UpdateAccountErpID(_currApp.SholdToX.AccountRowID, _currApp.SholdToX.CompanyID)
        End If
    End Sub
End Class
