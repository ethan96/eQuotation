Imports System.Activities
Imports Advantech.Myadvantech.Business
Public NotInheritable Class InitApprovers
    Inherits CodeActivity
    
    'Define an activity input argument of type String
    Property QuoteID() As InArgument(Of String)
    Property url() As InArgument(Of String)
    ' If your activity returns a value, derive from CodeActivity(Of TResult)
    ' and return the value from the Execute method.
    Protected Overrides Sub Execute(ByVal context As CodeActivityContext)
        'Obtain the runtime value of the Text input argument
        Dim _QuoteId As String = context.GetValue(Me.QuoteID)
        Dim _url As String = context.GetValue(Me.url)
        Dim InstanceId As String = context.WorkflowInstanceId.ToString()
        QuoteBusinessLogic.InitQuotApprove(_QuoteId, InstanceId, _url)

    End Sub
End Class
