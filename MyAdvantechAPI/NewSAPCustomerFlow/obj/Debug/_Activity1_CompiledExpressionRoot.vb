
#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",21)
Imports System

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",1)
Imports System.Collections

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",22)
Imports System.Collections.Generic

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",26)
Imports System.Activities

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",1)
Imports System.Activities.Expressions

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",1)
Imports System.Activities.Statements

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",23)
Imports System.Data

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",24)
Imports System.Linq

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",25)
Imports System.Text

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",27)
Imports NewSAPCustomerFlow

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\MyAdvantechAPI\NewSAPCustomerFlow\NewSAPAccountFlow.xaml",1)
Imports System.Activities.XamlIntegration

#End ExternalSource


Partial Public Class Activity1
    Implements System.Activities.XamlIntegration.ICompiledExpressionRoot
    
    Private rootActivity As System.Activities.Activity
    
    Private dataContextActivities As Object
    
    Private forImplementation As Boolean = true
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Public Function GetLanguage() As String Implements System.Activities.XamlIntegration.ICompiledExpressionRoot.GetLanguage
        Return "VB"
    End Function
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Public Overloads Function InvokeExpression(ByVal expressionId As Integer, ByVal locations As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal activityContext As System.Activities.ActivityContext) As Object Implements System.Activities.XamlIntegration.ICompiledExpressionRoot.InvokeExpression
        If (Me.rootActivity Is Nothing) Then
            Me.rootActivity = Me
        End If
        If (Me.dataContextActivities Is Nothing) Then
            Me.dataContextActivities = Activity1_TypedDataContext2_ForReadOnly.GetDataContextActivitiesHelper(Me.rootActivity, Me.forImplementation)
        End If
        If (expressionId = 0) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext0 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext0.ValueType___Expr0Get
        End If
        If (expressionId = 1) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext1 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext1.ValueType___Expr1Get
        End If
        If (expressionId = 2) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext2 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext2.ValueType___Expr2Get
        End If
        If (expressionId = 3) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext3 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext3.ValueType___Expr3Get
        End If
        If (expressionId = 4) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext4 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext4.ValueType___Expr4Get
        End If
        If (expressionId = 5) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext5 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext5.ValueType___Expr5Get
        End If
        If (expressionId = 6) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext6 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext6.ValueType___Expr6Get
        End If
        If (expressionId = 7) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext7 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext7.ValueType___Expr7Get
        End If
        If (expressionId = 8) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext8 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext8.ValueType___Expr8Get
        End If
        If (expressionId = 9) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = Activity1_TypedDataContext2_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New Activity1_TypedDataContext2_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext9 As Activity1_TypedDataContext2_ForReadOnly = CType(cachedCompiledDataContext(0),Activity1_TypedDataContext2_ForReadOnly)
            Return valDataContext9.ValueType___Expr9Get
        End If
        Return Nothing
    End Function
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Public Overloads Function InvokeExpression(ByVal expressionId As Integer, ByVal locations As System.Collections.Generic.IList(Of System.Activities.Location)) As Object Implements System.Activities.XamlIntegration.ICompiledExpressionRoot.InvokeExpression
        If (Me.rootActivity Is Nothing) Then
            Me.rootActivity = Me
        End If
        If (expressionId = 0) Then
            Dim valDataContext0 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext0.ValueType___Expr0Get
        End If
        If (expressionId = 1) Then
            Dim valDataContext1 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext1.ValueType___Expr1Get
        End If
        If (expressionId = 2) Then
            Dim valDataContext2 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext2.ValueType___Expr2Get
        End If
        If (expressionId = 3) Then
            Dim valDataContext3 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext3.ValueType___Expr3Get
        End If
        If (expressionId = 4) Then
            Dim valDataContext4 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext4.ValueType___Expr4Get
        End If
        If (expressionId = 5) Then
            Dim valDataContext5 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext5.ValueType___Expr5Get
        End If
        If (expressionId = 6) Then
            Dim valDataContext6 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext6.ValueType___Expr6Get
        End If
        If (expressionId = 7) Then
            Dim valDataContext7 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext7.ValueType___Expr7Get
        End If
        If (expressionId = 8) Then
            Dim valDataContext8 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext8.ValueType___Expr8Get
        End If
        If (expressionId = 9) Then
            Dim valDataContext9 As Activity1_TypedDataContext2_ForReadOnly = New Activity1_TypedDataContext2_ForReadOnly(locations, true)
            Return valDataContext9.ValueType___Expr9Get
        End If
        Return Nothing
    End Function
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Public Function CanExecuteExpression(ByVal expressionText As String, ByVal isReference As Boolean, ByVal locations As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByRef expressionId As Integer) As Boolean Implements System.Activities.XamlIntegration.ICompiledExpressionRoot.CanExecuteExpression
        If ((isReference = false)  _
                    AndAlso ((expressionText = "currApplicationId")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 0
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "AppUtil.IsHaveNextLeader(currApplicationId)")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 1
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "NewSAPCustomerFlow.AppUtil.IsApprovedOrRejected(currApplicationId)")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 2
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "currApplicationId")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 3
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "False")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 4
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "AppUtil.IsCreateBillTo(currApplicationId)")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 5
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "AppUtil.IsCreateBillTo(currApplicationId)")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 6
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "AppUtil.IsCreateSiebelAccount(currApplicationId)")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 7
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "currApplicationId")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 8
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "currApplicationId")  _
                    AndAlso (Activity1_TypedDataContext2_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 9
            Return true
        End If
        expressionId = -1
        Return false
    End Function
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Public Function GetRequiredLocations(ByVal expressionId As Integer) As System.Collections.Generic.IList(Of String) Implements System.Activities.XamlIntegration.ICompiledExpressionRoot.GetRequiredLocations
        Dim returnLocations As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)()
        If (expressionId = 0) Then
            returnLocations.Add("currApplicationId")
        End If
        If (expressionId = 1) Then
            returnLocations.Add("currApplicationId")
        End If
        If (expressionId = 2) Then
            returnLocations.Add("currApplicationId")
        End If
        If (expressionId = 3) Then
            returnLocations.Add("currApplicationId")
        End If
        If (expressionId = 4) Then
        End If
        If (expressionId = 5) Then
            returnLocations.Add("currApplicationId")
        End If
        If (expressionId = 6) Then
            returnLocations.Add("currApplicationId")
        End If
        If (expressionId = 7) Then
            returnLocations.Add("currApplicationId")
        End If
        If (expressionId = 8) Then
            returnLocations.Add("currApplicationId")
        End If
        If (expressionId = 9) Then
            returnLocations.Add("currApplicationId")
        End If
        Return returnLocations
    End Function
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Public Function GetExpressionTreeForExpression(ByVal expressionId As Integer, ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference)) As System.Linq.Expressions.Expression Implements System.Activities.XamlIntegration.ICompiledExpressionRoot.GetExpressionTreeForExpression
        If (expressionId = 0) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr0GetTree
        End If
        If (expressionId = 1) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr1GetTree
        End If
        If (expressionId = 2) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr2GetTree
        End If
        If (expressionId = 3) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr3GetTree
        End If
        If (expressionId = 4) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr4GetTree
        End If
        If (expressionId = 5) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr5GetTree
        End If
        If (expressionId = 6) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr6GetTree
        End If
        If (expressionId = 7) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr7GetTree
        End If
        If (expressionId = 8) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr8GetTree
        End If
        If (expressionId = 9) Then
            Return New Activity1_TypedDataContext2_ForReadOnly(locationReferences).__Expr9GetTree
        End If
        Return Nothing
    End Function
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Private Class Activity1_TypedDataContext0
        Inherits System.Activities.XamlIntegration.CompiledDataContext
        
        Private locationsOffset As Integer
        
        Private Shared expectedLocationsCount As Integer
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal activityContext As System.Activities.ActivityContext, ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, activityContext)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.Location), ByVal computelocationsOffset As Boolean)
            MyBase.New(locations)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference))
            MyBase.New(locationReferences)
        End Sub
        
        Friend Shared Function GetDataContextActivitiesHelper(ByVal compiledRoot As System.Activities.Activity, ByVal forImplementation As Boolean) As Object
            Return System.Activities.XamlIntegration.CompiledDataContext.GetDataContextActivities(compiledRoot, forImplementation)
        End Function
        
        Friend Shared Function GetCompiledDataContextCacheHelper(ByVal dataContextActivities As Object, ByVal activityContext As System.Activities.ActivityContext, ByVal compiledRoot As System.Activities.Activity, ByVal forImplementation As Boolean, ByVal compiledDataContextCount As Integer) As System.Activities.XamlIntegration.CompiledDataContext()
            Return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount)
        End Function
        
        Public Overridable Sub SetLocationsOffset(ByVal locationsOffsetValue As Integer)
            locationsOffset = locationsOffsetValue
        End Sub
        
        Public Shared Function Validate(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal validateLocationCount As Boolean, ByVal offset As Integer) As Boolean
            If ((validateLocationCount = true)  _
                        AndAlso (locationReferences.Count < 0)) Then
                Return false
            End If
            expectedLocationsCount = 0
            Return true
        End Function
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Private Class Activity1_TypedDataContext0_ForReadOnly
        Inherits System.Activities.XamlIntegration.CompiledDataContext
        
        Private locationsOffset As Integer
        
        Private Shared expectedLocationsCount As Integer
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal activityContext As System.Activities.ActivityContext, ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, activityContext)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.Location), ByVal computelocationsOffset As Boolean)
            MyBase.New(locations)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference))
            MyBase.New(locationReferences)
        End Sub
        
        Friend Shared Function GetDataContextActivitiesHelper(ByVal compiledRoot As System.Activities.Activity, ByVal forImplementation As Boolean) As Object
            Return System.Activities.XamlIntegration.CompiledDataContext.GetDataContextActivities(compiledRoot, forImplementation)
        End Function
        
        Friend Shared Function GetCompiledDataContextCacheHelper(ByVal dataContextActivities As Object, ByVal activityContext As System.Activities.ActivityContext, ByVal compiledRoot As System.Activities.Activity, ByVal forImplementation As Boolean, ByVal compiledDataContextCount As Integer) As System.Activities.XamlIntegration.CompiledDataContext()
            Return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount)
        End Function
        
        Public Overridable Sub SetLocationsOffset(ByVal locationsOffsetValue As Integer)
            locationsOffset = locationsOffsetValue
        End Sub
        
        Public Shared Function Validate(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal validateLocationCount As Boolean, ByVal offset As Integer) As Boolean
            If ((validateLocationCount = true)  _
                        AndAlso (locationReferences.Count < 0)) Then
                Return false
            End If
            expectedLocationsCount = 0
            Return true
        End Function
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Private Class Activity1_TypedDataContext1
        Inherits Activity1_TypedDataContext0
        
        Private locationsOffset As Integer
        
        Private Shared expectedLocationsCount As Integer
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal activityContext As System.Activities.ActivityContext, ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, activityContext, false)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.Location), ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, false)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference))
            MyBase.New(locationReferences)
        End Sub
        
        Protected Property currApplicationId() As String
            Get
                Return CType(Me.GetVariableValue((0 + locationsOffset)),String)
            End Get
            Set
                Me.SetVariableValue((0 + locationsOffset), value)
            End Set
        End Property
        
        Protected Property CustomerId() As String
            Get
                Return CType(Me.GetVariableValue((1 + locationsOffset)),String)
            End Get
            Set
                Me.SetVariableValue((1 + locationsOffset), value)
            End Set
        End Property
        
        Protected Property SiebelAccountId() As String
            Get
                Return CType(Me.GetVariableValue((2 + locationsOffset)),String)
            End Get
            Set
                Me.SetVariableValue((2 + locationsOffset), value)
            End Set
        End Property
        
        Protected Property RequestedBy() As String
            Get
                Return CType(Me.GetVariableValue((3 + locationsOffset)),String)
            End Get
            Set
                Me.SetVariableValue((3 + locationsOffset), value)
            End Set
        End Property
        
        Friend Shadows Shared Function GetCompiledDataContextCacheHelper(ByVal dataContextActivities As Object, ByVal activityContext As System.Activities.ActivityContext, ByVal compiledRoot As System.Activities.Activity, ByVal forImplementation As Boolean, ByVal compiledDataContextCount As Integer) As System.Activities.XamlIntegration.CompiledDataContext()
            Return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount)
        End Function
        
        Public Shadows Overridable Sub SetLocationsOffset(ByVal locationsOffsetValue As Integer)
            locationsOffset = locationsOffsetValue
            MyBase.SetLocationsOffset(locationsOffset)
        End Sub
        
        Public Shadows Shared Function Validate(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal validateLocationCount As Boolean, ByVal offset As Integer) As Boolean
            If ((validateLocationCount = true)  _
                        AndAlso (locationReferences.Count < 4)) Then
                Return false
            End If
            If (validateLocationCount = true) Then
                offset = (locationReferences.Count - 4)
            End If
            expectedLocationsCount = 4
            If ((locationReferences((offset + 0)).Name <> "currApplicationId")  _
                        OrElse (locationReferences((offset + 0)).Type <> GetType(String))) Then
                Return false
            End If
            If ((locationReferences((offset + 1)).Name <> "CustomerId")  _
                        OrElse (locationReferences((offset + 1)).Type <> GetType(String))) Then
                Return false
            End If
            If ((locationReferences((offset + 2)).Name <> "SiebelAccountId")  _
                        OrElse (locationReferences((offset + 2)).Type <> GetType(String))) Then
                Return false
            End If
            If ((locationReferences((offset + 3)).Name <> "RequestedBy")  _
                        OrElse (locationReferences((offset + 3)).Type <> GetType(String))) Then
                Return false
            End If
            Return Activity1_TypedDataContext0.Validate(locationReferences, false, offset)
        End Function
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Private Class Activity1_TypedDataContext1_ForReadOnly
        Inherits Activity1_TypedDataContext0_ForReadOnly
        
        Private locationsOffset As Integer
        
        Private Shared expectedLocationsCount As Integer
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal activityContext As System.Activities.ActivityContext, ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, activityContext, false)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.Location), ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, false)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference))
            MyBase.New(locationReferences)
        End Sub
        
        Protected ReadOnly Property currApplicationId() As String
            Get
                Return CType(Me.GetVariableValue((0 + locationsOffset)),String)
            End Get
        End Property
        
        Protected ReadOnly Property CustomerId() As String
            Get
                Return CType(Me.GetVariableValue((1 + locationsOffset)),String)
            End Get
        End Property
        
        Protected ReadOnly Property SiebelAccountId() As String
            Get
                Return CType(Me.GetVariableValue((2 + locationsOffset)),String)
            End Get
        End Property
        
        Protected ReadOnly Property RequestedBy() As String
            Get
                Return CType(Me.GetVariableValue((3 + locationsOffset)),String)
            End Get
        End Property
        
        Friend Shadows Shared Function GetCompiledDataContextCacheHelper(ByVal dataContextActivities As Object, ByVal activityContext As System.Activities.ActivityContext, ByVal compiledRoot As System.Activities.Activity, ByVal forImplementation As Boolean, ByVal compiledDataContextCount As Integer) As System.Activities.XamlIntegration.CompiledDataContext()
            Return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount)
        End Function
        
        Public Shadows Overridable Sub SetLocationsOffset(ByVal locationsOffsetValue As Integer)
            locationsOffset = locationsOffsetValue
            MyBase.SetLocationsOffset(locationsOffset)
        End Sub
        
        Public Shadows Shared Function Validate(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal validateLocationCount As Boolean, ByVal offset As Integer) As Boolean
            If ((validateLocationCount = true)  _
                        AndAlso (locationReferences.Count < 4)) Then
                Return false
            End If
            If (validateLocationCount = true) Then
                offset = (locationReferences.Count - 4)
            End If
            expectedLocationsCount = 4
            If ((locationReferences((offset + 0)).Name <> "currApplicationId")  _
                        OrElse (locationReferences((offset + 0)).Type <> GetType(String))) Then
                Return false
            End If
            If ((locationReferences((offset + 1)).Name <> "CustomerId")  _
                        OrElse (locationReferences((offset + 1)).Type <> GetType(String))) Then
                Return false
            End If
            If ((locationReferences((offset + 2)).Name <> "SiebelAccountId")  _
                        OrElse (locationReferences((offset + 2)).Type <> GetType(String))) Then
                Return false
            End If
            If ((locationReferences((offset + 3)).Name <> "RequestedBy")  _
                        OrElse (locationReferences((offset + 3)).Type <> GetType(String))) Then
                Return false
            End If
            Return Activity1_TypedDataContext0_ForReadOnly.Validate(locationReferences, false, offset)
        End Function
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Private Class Activity1_TypedDataContext2
        Inherits Activity1_TypedDataContext1
        
        Private locationsOffset As Integer
        
        Private Shared expectedLocationsCount As Integer
        
        Protected RejectedTimes As Integer
        
        Protected IsAEUCMApproved As Boolean
        
        Protected IsCreateShodTo As Boolean
        
        Protected IsCreateShipTo As Boolean
        
        Protected IsCreateBillTo As Boolean
        
        Protected IsHaveNextLeader As Boolean
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal activityContext As System.Activities.ActivityContext, ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, activityContext, false)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.Location), ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, false)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference))
            MyBase.New(locationReferences)
        End Sub
        
        Friend Shadows Shared Function GetCompiledDataContextCacheHelper(ByVal dataContextActivities As Object, ByVal activityContext As System.Activities.ActivityContext, ByVal compiledRoot As System.Activities.Activity, ByVal forImplementation As Boolean, ByVal compiledDataContextCount As Integer) As System.Activities.XamlIntegration.CompiledDataContext()
            Return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount)
        End Function
        
        Public Shadows Overridable Sub SetLocationsOffset(ByVal locationsOffsetValue As Integer)
            locationsOffset = locationsOffsetValue
            MyBase.SetLocationsOffset(locationsOffset)
        End Sub
        
        Protected Overrides Sub GetValueTypeValues()
            Me.RejectedTimes = CType(Me.GetVariableValue((4 + locationsOffset)),Integer)
            Me.IsAEUCMApproved = CType(Me.GetVariableValue((5 + locationsOffset)),Boolean)
            Me.IsCreateShodTo = CType(Me.GetVariableValue((6 + locationsOffset)),Boolean)
            Me.IsCreateShipTo = CType(Me.GetVariableValue((7 + locationsOffset)),Boolean)
            Me.IsCreateBillTo = CType(Me.GetVariableValue((8 + locationsOffset)),Boolean)
            Me.IsHaveNextLeader = CType(Me.GetVariableValue((9 + locationsOffset)),Boolean)
            MyBase.GetValueTypeValues
        End Sub
        
        Protected Overrides Sub SetValueTypeValues()
            Me.SetVariableValue((4 + locationsOffset), Me.RejectedTimes)
            Me.SetVariableValue((5 + locationsOffset), Me.IsAEUCMApproved)
            Me.SetVariableValue((6 + locationsOffset), Me.IsCreateShodTo)
            Me.SetVariableValue((7 + locationsOffset), Me.IsCreateShipTo)
            Me.SetVariableValue((8 + locationsOffset), Me.IsCreateBillTo)
            Me.SetVariableValue((9 + locationsOffset), Me.IsHaveNextLeader)
            MyBase.SetValueTypeValues
        End Sub
        
        Public Shadows Shared Function Validate(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal validateLocationCount As Boolean, ByVal offset As Integer) As Boolean
            If ((validateLocationCount = true)  _
                        AndAlso (locationReferences.Count < 10)) Then
                Return false
            End If
            If (validateLocationCount = true) Then
                offset = (locationReferences.Count - 10)
            End If
            expectedLocationsCount = 10
            If ((locationReferences((offset + 4)).Name <> "RejectedTimes")  _
                        OrElse (locationReferences((offset + 4)).Type <> GetType(Integer))) Then
                Return false
            End If
            If ((locationReferences((offset + 5)).Name <> "IsAEUCMApproved")  _
                        OrElse (locationReferences((offset + 5)).Type <> GetType(Boolean))) Then
                Return false
            End If
            If ((locationReferences((offset + 6)).Name <> "IsCreateShodTo")  _
                        OrElse (locationReferences((offset + 6)).Type <> GetType(Boolean))) Then
                Return false
            End If
            If ((locationReferences((offset + 7)).Name <> "IsCreateShipTo")  _
                        OrElse (locationReferences((offset + 7)).Type <> GetType(Boolean))) Then
                Return false
            End If
            If ((locationReferences((offset + 8)).Name <> "IsCreateBillTo")  _
                        OrElse (locationReferences((offset + 8)).Type <> GetType(Boolean))) Then
                Return false
            End If
            If ((locationReferences((offset + 9)).Name <> "IsHaveNextLeader")  _
                        OrElse (locationReferences((offset + 9)).Type <> GetType(Boolean))) Then
                Return false
            End If
            Return Activity1_TypedDataContext1.Validate(locationReferences, false, offset)
        End Function
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Private Class Activity1_TypedDataContext2_ForReadOnly
        Inherits Activity1_TypedDataContext1_ForReadOnly
        
        Private locationsOffset As Integer
        
        Private Shared expectedLocationsCount As Integer
        
        Protected RejectedTimes As Integer
        
        Protected IsAEUCMApproved As Boolean
        
        Protected IsCreateShodTo As Boolean
        
        Protected IsCreateShipTo As Boolean
        
        Protected IsCreateBillTo As Boolean
        
        Protected IsHaveNextLeader As Boolean
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal activityContext As System.Activities.ActivityContext, ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, activityContext, false)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locations As System.Collections.Generic.IList(Of System.Activities.Location), ByVal computelocationsOffset As Boolean)
            MyBase.New(locations, false)
            If (computelocationsOffset = true) Then
                Me.SetLocationsOffset((locations.Count - expectedLocationsCount))
            End If
        End Sub
        
        Public Sub New(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference))
            MyBase.New(locationReferences)
        End Sub
        
        Friend Shadows Shared Function GetCompiledDataContextCacheHelper(ByVal dataContextActivities As Object, ByVal activityContext As System.Activities.ActivityContext, ByVal compiledRoot As System.Activities.Activity, ByVal forImplementation As Boolean, ByVal compiledDataContextCount As Integer) As System.Activities.XamlIntegration.CompiledDataContext()
            Return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount)
        End Function
        
        Public Shadows Overridable Sub SetLocationsOffset(ByVal locationsOffsetValue As Integer)
            locationsOffset = locationsOffsetValue
            MyBase.SetLocationsOffset(locationsOffset)
        End Sub
        
        Friend Function __Expr0GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",41)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of String)) = Function() _
  currApplicationId
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr0Get() As String
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",41)
            Return _
  currApplicationId
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr0Get() As String
            Me.GetValueTypeValues
            Return Me.__Expr0Get
        End Function
        
        Friend Function __Expr1GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",41)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of Boolean)) = Function() _
  AppUtil.IsHaveNextLeader(currApplicationId)
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr1Get() As Boolean
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",41)
            Return _
  AppUtil.IsHaveNextLeader(currApplicationId)
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr1Get() As Boolean
            Me.GetValueTypeValues
            Return Me.__Expr1Get
        End Function
        
        Friend Function __Expr2GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",52)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of Boolean)) = Function() _
                                                                                                                                                                                                                                                 NewSAPCustomerFlow.AppUtil.IsApprovedOrRejected(currApplicationId)
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr2Get() As Boolean
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",52)
            Return _
                                                                                                                                                                                                                                                 NewSAPCustomerFlow.AppUtil.IsApprovedOrRejected(currApplicationId)
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr2Get() As Boolean
            Me.GetValueTypeValues
            Return Me.__Expr2Get
        End Function
        
        Friend Function __Expr3GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",54)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of String)) = Function() _
                                                          currApplicationId
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr3Get() As String
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",54)
            Return _
                                                          currApplicationId
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr3Get() As String
            Me.GetValueTypeValues
            Return Me.__Expr3Get
        End Function
        
        Friend Function __Expr4GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",118)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of Boolean)) = Function() _
                                                                                                                                                                                              False
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr4Get() As Boolean
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",118)
            Return _
                                                                                                                                                                                              False
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr4Get() As Boolean
            Me.GetValueTypeValues
            Return Me.__Expr4Get
        End Function
        
        Friend Function __Expr5GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",67)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of Boolean)) = Function() _
                                                                                                                                                                                            AppUtil.IsCreateBillTo(currApplicationId)
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr5Get() As Boolean
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",67)
            Return _
                                                                                                                                                                                            AppUtil.IsCreateBillTo(currApplicationId)
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr5Get() As Boolean
            Me.GetValueTypeValues
            Return Me.__Expr5Get
        End Function
        
        Friend Function __Expr6GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",69)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of Boolean)) = Function() _
                                                                      AppUtil.IsCreateBillTo(currApplicationId)
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr6Get() As Boolean
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",69)
            Return _
                                                                      AppUtil.IsCreateBillTo(currApplicationId)
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr6Get() As Boolean
            Me.GetValueTypeValues
            Return Me.__Expr6Get
        End Function
        
        Friend Function __Expr7GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",79)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of Boolean)) = Function() _
                                                                                      AppUtil.IsCreateSiebelAccount(currApplicationId)
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr7Get() As Boolean
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",79)
            Return _
                                                                                      AppUtil.IsCreateSiebelAccount(currApplicationId)
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr7Get() As Boolean
            Me.GetValueTypeValues
            Return Me.__Expr7Get
        End Function
        
        Friend Function __Expr8GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",85)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of String)) = Function() _
                                                                                                                                                                                         currApplicationId
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr8Get() As String
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",85)
            Return _
                                                                                                                                                                                         currApplicationId
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr8Get() As String
            Me.GetValueTypeValues
            Return Me.__Expr8Get
        End Function
        
        Friend Function __Expr9GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",57)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of String)) = Function() _
                                                                                                                                                                                                                                                                                    currApplicationId
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr9Get() As String
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\MYADVANTECHAPI\NEWSAPCUSTOMERFLOW\NEWSAPACCOUNTFLOW.XAML",57)
            Return _
                                                                                                                                                                                                                                                                                    currApplicationId
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr9Get() As String
            Me.GetValueTypeValues
            Return Me.__Expr9Get
        End Function
        
        Protected Overrides Sub GetValueTypeValues()
            Me.RejectedTimes = CType(Me.GetVariableValue((4 + locationsOffset)),Integer)
            Me.IsAEUCMApproved = CType(Me.GetVariableValue((5 + locationsOffset)),Boolean)
            Me.IsCreateShodTo = CType(Me.GetVariableValue((6 + locationsOffset)),Boolean)
            Me.IsCreateShipTo = CType(Me.GetVariableValue((7 + locationsOffset)),Boolean)
            Me.IsCreateBillTo = CType(Me.GetVariableValue((8 + locationsOffset)),Boolean)
            Me.IsHaveNextLeader = CType(Me.GetVariableValue((9 + locationsOffset)),Boolean)
            MyBase.GetValueTypeValues
        End Sub
        
        Public Shadows Shared Function Validate(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal validateLocationCount As Boolean, ByVal offset As Integer) As Boolean
            If ((validateLocationCount = true)  _
                        AndAlso (locationReferences.Count < 10)) Then
                Return false
            End If
            If (validateLocationCount = true) Then
                offset = (locationReferences.Count - 10)
            End If
            expectedLocationsCount = 10
            If ((locationReferences((offset + 4)).Name <> "RejectedTimes")  _
                        OrElse (locationReferences((offset + 4)).Type <> GetType(Integer))) Then
                Return false
            End If
            If ((locationReferences((offset + 5)).Name <> "IsAEUCMApproved")  _
                        OrElse (locationReferences((offset + 5)).Type <> GetType(Boolean))) Then
                Return false
            End If
            If ((locationReferences((offset + 6)).Name <> "IsCreateShodTo")  _
                        OrElse (locationReferences((offset + 6)).Type <> GetType(Boolean))) Then
                Return false
            End If
            If ((locationReferences((offset + 7)).Name <> "IsCreateShipTo")  _
                        OrElse (locationReferences((offset + 7)).Type <> GetType(Boolean))) Then
                Return false
            End If
            If ((locationReferences((offset + 8)).Name <> "IsCreateBillTo")  _
                        OrElse (locationReferences((offset + 8)).Type <> GetType(Boolean))) Then
                Return false
            End If
            If ((locationReferences((offset + 9)).Name <> "IsHaveNextLeader")  _
                        OrElse (locationReferences((offset + 9)).Type <> GetType(Boolean))) Then
                Return false
            End If
            Return Activity1_TypedDataContext1_ForReadOnly.Validate(locationReferences, false, offset)
        End Function
    End Class
End Class
