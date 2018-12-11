
#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",19)
Imports System

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",1)
Imports System.Collections

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",20)
Imports System.Collections.Generic

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",24)
Imports System.Activities

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",1)
Imports System.Activities.Expressions

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",1)
Imports System.Activities.Statements

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",21)
Imports System.Data

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",22)
Imports System.Linq

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",23)
Imports System.Text

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",25)
Imports GPcontrolAPI

#End ExternalSource

#ExternalSource("D:\MyAdvantechProject\MyAdvantechGit\GPcontrolAPI\GPcontrol.xaml",1)
Imports System.Activities.XamlIntegration

#End ExternalSource


Partial Public Class GPcontrol
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
            Me.dataContextActivities = GPcontrol_TypedDataContext1_ForReadOnly.GetDataContextActivitiesHelper(Me.rootActivity, Me.forImplementation)
        End If
        If (expressionId = 0) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = GPcontrol_TypedDataContext1_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New GPcontrol_TypedDataContext1_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext0 As GPcontrol_TypedDataContext1_ForReadOnly = CType(cachedCompiledDataContext(0),GPcontrol_TypedDataContext1_ForReadOnly)
            Return valDataContext0.ValueType___Expr0Get
        End If
        If (expressionId = 1) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = GPcontrol_TypedDataContext1_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New GPcontrol_TypedDataContext1_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext1 As GPcontrol_TypedDataContext1_ForReadOnly = CType(cachedCompiledDataContext(0),GPcontrol_TypedDataContext1_ForReadOnly)
            Return valDataContext1.ValueType___Expr1Get
        End If
        If (expressionId = 2) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = GPcontrol_TypedDataContext1_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New GPcontrol_TypedDataContext1_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext2 As GPcontrol_TypedDataContext1_ForReadOnly = CType(cachedCompiledDataContext(0),GPcontrol_TypedDataContext1_ForReadOnly)
            Return valDataContext2.ValueType___Expr2Get
        End If
        If (expressionId = 3) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = GPcontrol_TypedDataContext1_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New GPcontrol_TypedDataContext1_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext3 As GPcontrol_TypedDataContext1_ForReadOnly = CType(cachedCompiledDataContext(0),GPcontrol_TypedDataContext1_ForReadOnly)
            Return valDataContext3.ValueType___Expr3Get
        End If
        If (expressionId = 4) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = GPcontrol_TypedDataContext1_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New GPcontrol_TypedDataContext1_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext4 As GPcontrol_TypedDataContext1_ForReadOnly = CType(cachedCompiledDataContext(0),GPcontrol_TypedDataContext1_ForReadOnly)
            Return valDataContext4.ValueType___Expr4Get
        End If
        If (expressionId = 5) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = GPcontrol_TypedDataContext1_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New GPcontrol_TypedDataContext1_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext5 As GPcontrol_TypedDataContext1_ForReadOnly = CType(cachedCompiledDataContext(0),GPcontrol_TypedDataContext1_ForReadOnly)
            Return valDataContext5.ValueType___Expr5Get
        End If
        If (expressionId = 6) Then
            Dim cachedCompiledDataContext() As System.Activities.XamlIntegration.CompiledDataContext = GPcontrol_TypedDataContext1_ForReadOnly.GetCompiledDataContextCacheHelper(Me.dataContextActivities, activityContext, Me.rootActivity, Me.forImplementation, 1)
            If (cachedCompiledDataContext(0) Is Nothing) Then
                cachedCompiledDataContext(0) = New GPcontrol_TypedDataContext1_ForReadOnly(locations, activityContext, true)
            End If
            Dim valDataContext6 As GPcontrol_TypedDataContext1_ForReadOnly = CType(cachedCompiledDataContext(0),GPcontrol_TypedDataContext1_ForReadOnly)
            Return valDataContext6.ValueType___Expr6Get
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
            Dim valDataContext0 As GPcontrol_TypedDataContext1_ForReadOnly = New GPcontrol_TypedDataContext1_ForReadOnly(locations, true)
            Return valDataContext0.ValueType___Expr0Get
        End If
        If (expressionId = 1) Then
            Dim valDataContext1 As GPcontrol_TypedDataContext1_ForReadOnly = New GPcontrol_TypedDataContext1_ForReadOnly(locations, true)
            Return valDataContext1.ValueType___Expr1Get
        End If
        If (expressionId = 2) Then
            Dim valDataContext2 As GPcontrol_TypedDataContext1_ForReadOnly = New GPcontrol_TypedDataContext1_ForReadOnly(locations, true)
            Return valDataContext2.ValueType___Expr2Get
        End If
        If (expressionId = 3) Then
            Dim valDataContext3 As GPcontrol_TypedDataContext1_ForReadOnly = New GPcontrol_TypedDataContext1_ForReadOnly(locations, true)
            Return valDataContext3.ValueType___Expr3Get
        End If
        If (expressionId = 4) Then
            Dim valDataContext4 As GPcontrol_TypedDataContext1_ForReadOnly = New GPcontrol_TypedDataContext1_ForReadOnly(locations, true)
            Return valDataContext4.ValueType___Expr4Get
        End If
        If (expressionId = 5) Then
            Dim valDataContext5 As GPcontrol_TypedDataContext1_ForReadOnly = New GPcontrol_TypedDataContext1_ForReadOnly(locations, true)
            Return valDataContext5.ValueType___Expr5Get
        End If
        If (expressionId = 6) Then
            Dim valDataContext6 As GPcontrol_TypedDataContext1_ForReadOnly = New GPcontrol_TypedDataContext1_ForReadOnly(locations, true)
            Return valDataContext6.ValueType___Expr6Get
        End If
        Return Nothing
    End Function
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Public Function CanExecuteExpression(ByVal expressionText As String, ByVal isReference As Boolean, ByVal locations As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByRef expressionId As Integer) As Boolean Implements System.Activities.XamlIntegration.ICompiledExpressionRoot.CanExecuteExpression
        If ((isReference = false)  _
                    AndAlso ((expressionText = "Url")  _
                    AndAlso (GPcontrol_TypedDataContext1_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 0
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "QuoteID")  _
                    AndAlso (GPcontrol_TypedDataContext1_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 1
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "GPcontrolBiz.IsRejected(QuoteID)")  _
                    AndAlso (GPcontrol_TypedDataContext1_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 2
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "GPcontrolBiz.IsApprovedByAll(QuoteID)")  _
                    AndAlso (GPcontrol_TypedDataContext1_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 3
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "QuoteID")  _
                    AndAlso (GPcontrol_TypedDataContext1_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 4
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "QuoteID")  _
                    AndAlso (GPcontrol_TypedDataContext1_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 5
            Return true
        End If
        If ((isReference = false)  _
                    AndAlso ((expressionText = "QuoteID")  _
                    AndAlso (GPcontrol_TypedDataContext1_ForReadOnly.Validate(locations, true, 0) = true))) Then
            expressionId = 6
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
            returnLocations.Add("Url")
        End If
        If (expressionId = 1) Then
            returnLocations.Add("QuoteID")
        End If
        If (expressionId = 2) Then
            returnLocations.Add("QuoteID")
        End If
        If (expressionId = 3) Then
            returnLocations.Add("QuoteID")
        End If
        If (expressionId = 4) Then
            returnLocations.Add("QuoteID")
        End If
        If (expressionId = 5) Then
            returnLocations.Add("QuoteID")
        End If
        If (expressionId = 6) Then
            returnLocations.Add("QuoteID")
        End If
        Return returnLocations
    End Function
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Public Function GetExpressionTreeForExpression(ByVal expressionId As Integer, ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference)) As System.Linq.Expressions.Expression Implements System.Activities.XamlIntegration.ICompiledExpressionRoot.GetExpressionTreeForExpression
        If (expressionId = 0) Then
            Return New GPcontrol_TypedDataContext1_ForReadOnly(locationReferences).__Expr0GetTree
        End If
        If (expressionId = 1) Then
            Return New GPcontrol_TypedDataContext1_ForReadOnly(locationReferences).__Expr1GetTree
        End If
        If (expressionId = 2) Then
            Return New GPcontrol_TypedDataContext1_ForReadOnly(locationReferences).__Expr2GetTree
        End If
        If (expressionId = 3) Then
            Return New GPcontrol_TypedDataContext1_ForReadOnly(locationReferences).__Expr3GetTree
        End If
        If (expressionId = 4) Then
            Return New GPcontrol_TypedDataContext1_ForReadOnly(locationReferences).__Expr4GetTree
        End If
        If (expressionId = 5) Then
            Return New GPcontrol_TypedDataContext1_ForReadOnly(locationReferences).__Expr5GetTree
        End If
        If (expressionId = 6) Then
            Return New GPcontrol_TypedDataContext1_ForReadOnly(locationReferences).__Expr6GetTree
        End If
        Return Nothing
    End Function
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Private Class GPcontrol_TypedDataContext0
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
    Private Class GPcontrol_TypedDataContext0_ForReadOnly
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
    Private Class GPcontrol_TypedDataContext1
        Inherits GPcontrol_TypedDataContext0
        
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
        
        Protected Property Url() As String
            Get
                Return CType(Me.GetVariableValue((0 + locationsOffset)),String)
            End Get
            Set
                Me.SetVariableValue((0 + locationsOffset), value)
            End Set
        End Property
        
        Protected Property QuoteID() As String
            Get
                Return CType(Me.GetVariableValue((1 + locationsOffset)),String)
            End Get
            Set
                Me.SetVariableValue((1 + locationsOffset), value)
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
                        AndAlso (locationReferences.Count < 2)) Then
                Return false
            End If
            If (validateLocationCount = true) Then
                offset = (locationReferences.Count - 2)
            End If
            expectedLocationsCount = 2
            If ((locationReferences((offset + 0)).Name <> "Url")  _
                        OrElse (locationReferences((offset + 0)).Type <> GetType(String))) Then
                Return false
            End If
            If ((locationReferences((offset + 1)).Name <> "QuoteID")  _
                        OrElse (locationReferences((offset + 1)).Type <> GetType(String))) Then
                Return false
            End If
            Return GPcontrol_TypedDataContext0.Validate(locationReferences, false, offset)
        End Function
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Activities", "4.0.0.0"),  _
     System.ComponentModel.BrowsableAttribute(false),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)>  _
    Private Class GPcontrol_TypedDataContext1_ForReadOnly
        Inherits GPcontrol_TypedDataContext0_ForReadOnly
        
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
        
        Protected ReadOnly Property Url() As String
            Get
                Return CType(Me.GetVariableValue((0 + locationsOffset)),String)
            End Get
        End Property
        
        Protected ReadOnly Property QuoteID() As String
            Get
                Return CType(Me.GetVariableValue((1 + locationsOffset)),String)
            End Get
        End Property
        
        Friend Shadows Shared Function GetCompiledDataContextCacheHelper(ByVal dataContextActivities As Object, ByVal activityContext As System.Activities.ActivityContext, ByVal compiledRoot As System.Activities.Activity, ByVal forImplementation As Boolean, ByVal compiledDataContextCount As Integer) As System.Activities.XamlIntegration.CompiledDataContext()
            Return System.Activities.XamlIntegration.CompiledDataContext.GetCompiledDataContextCache(dataContextActivities, activityContext, compiledRoot, forImplementation, compiledDataContextCount)
        End Function
        
        Public Shadows Overridable Sub SetLocationsOffset(ByVal locationsOffsetValue As Integer)
            locationsOffset = locationsOffsetValue
            MyBase.SetLocationsOffset(locationsOffset)
        End Sub
        
        Friend Function __Expr0GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",39)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of String)) = Function() _
  Url
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr0Get() As String
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",39)
            Return _
  Url
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr0Get() As String
            Me.GetValueTypeValues
            Return Me.__Expr0Get
        End Function
        
        Friend Function __Expr1GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",42)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of String)) = Function() _
                                                                                                       QuoteID
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr1Get() As String
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",42)
            Return _
                                                                                                       QuoteID
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr1Get() As String
            Me.GetValueTypeValues
            Return Me.__Expr1Get
        End Function
        
        Friend Function __Expr2GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",42)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of Boolean)) = Function() _
        GPcontrolBiz.IsRejected(QuoteID)
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr2Get() As Boolean
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",42)
            Return _
        GPcontrolBiz.IsRejected(QuoteID)
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr2Get() As Boolean
            Me.GetValueTypeValues
            Return Me.__Expr2Get
        End Function
        
        Friend Function __Expr3GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",51)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of Boolean)) = Function() _
                                                              GPcontrolBiz.IsApprovedByAll(QuoteID)
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr3Get() As Boolean
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",51)
            Return _
                                                              GPcontrolBiz.IsApprovedByAll(QuoteID)
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr3Get() As Boolean
            Me.GetValueTypeValues
            Return Me.__Expr3Get
        End Function
        
        Friend Function __Expr4GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",59)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of String)) = Function() _
                                                                                                                 QuoteID
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr4Get() As String
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",59)
            Return _
                                                                                                                 QuoteID
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr4Get() As String
            Me.GetValueTypeValues
            Return Me.__Expr4Get
        End Function
        
        Friend Function __Expr5GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",54)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of String)) = Function() _
                                                                                                                  QuoteID
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr5Get() As String
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",54)
            Return _
                                                                                                                  QuoteID
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr5Get() As String
            Me.GetValueTypeValues
            Return Me.__Expr5Get
        End Function
        
        Friend Function __Expr6GetTree() As System.Linq.Expressions.Expression
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",47)
            Dim expression As System.Linq.Expressions.Expression(Of System.Func(Of String)) = Function() _
                                                                                                              QuoteID
            
            #End ExternalSource
            Return MyBase.RewriteExpressionTree(expression)
        End Function
        
        <System.Diagnostics.DebuggerHiddenAttribute()>  _
        Public Function __Expr6Get() As String
            
            #ExternalSource("D:\MYADVANTECHPROJECT\MYADVANTECHGIT\GPCONTROLAPI\GPCONTROL.XAML",47)
            Return _
                                                                                                              QuoteID
            
            #End ExternalSource
        End Function
        
        Public Function ValueType___Expr6Get() As String
            Me.GetValueTypeValues
            Return Me.__Expr6Get
        End Function
        
        Public Shadows Shared Function Validate(ByVal locationReferences As System.Collections.Generic.IList(Of System.Activities.LocationReference), ByVal validateLocationCount As Boolean, ByVal offset As Integer) As Boolean
            If ((validateLocationCount = true)  _
                        AndAlso (locationReferences.Count < 2)) Then
                Return false
            End If
            If (validateLocationCount = true) Then
                offset = (locationReferences.Count - 2)
            End If
            expectedLocationsCount = 2
            If ((locationReferences((offset + 0)).Name <> "Url")  _
                        OrElse (locationReferences((offset + 0)).Type <> GetType(String))) Then
                Return false
            End If
            If ((locationReferences((offset + 1)).Name <> "QuoteID")  _
                        OrElse (locationReferences((offset + 1)).Type <> GetType(String))) Then
                Return false
            End If
            Return GPcontrol_TypedDataContext0_ForReadOnly.Validate(locationReferences, false, offset)
        End Function
    End Class
End Class
