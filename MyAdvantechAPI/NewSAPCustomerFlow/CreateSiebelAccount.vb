Imports System.Activities
Imports Advantech.Myadvantech.DataAccess

Public NotInheritable Class CreateSiebelAccount
    Inherits CodeActivity
    Property Text() As InArgument(Of String)
    Protected Overrides Sub Execute(ByVal context As CodeActivityContext)
        Dim _currApp As SA_APPLICATION = AppUtil.getAppByInstanceID(context.WorkflowInstanceId.ToString())
        If _currApp IsNot Nothing Then
            SiebelDAL.CreateAccount2(_currApp.ID)
        End If
    End Sub
End Class
