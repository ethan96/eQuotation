'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On



<System.Runtime.InteropServices.ComVisible(false)>  _
Partial Public Class GPcontrol
    Inherits System.Activities.Activity
    Implements System.ComponentModel.ISupportInitialize
    
    Private _contentLoaded As Boolean
    
    Private _QuoteID As System.Activities.InArgument(Of String)
    
    Private _Url As System.Activities.InArgument(Of String)
    
Partial Private Sub BeforeInitializeComponent(ByRef isInitialized as Boolean)
End Sub

Partial Private Sub AfterInitializeComponent
End Sub

    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "4.0.0.0")>  _
    Public Sub New()
        MyBase.New
        Me.InitializeComponent
    End Sub
    
    Public Property QuoteID() As System.Activities.InArgument(Of String)
        Get
            Return Me._QuoteID
        End Get
        Set
            Me._QuoteID = value
        End Set
    End Property
    
    Public Property Url() As System.Activities.InArgument(Of String)
        Get
            Return Me._Url
        End Get
        Set
            Me._Url = value
        End Set
    End Property
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "4.0.0.0")>  _
    Public Sub InitializeComponent()
        If (Me._contentLoaded = true) Then
            Return
        End If
        Me._contentLoaded = true
        Dim isInitialized As Boolean = false
        Me.BeforeInitializeComponent(isInitialized)
        If (isInitialized = true) Then
            Me.AfterInitializeComponent
            Return
        End If
        Dim resourceName As String = Me.FindResource
        Dim initializeXaml As System.IO.Stream = GetType(GPcontrol).Assembly.GetManifestResourceStream(resourceName)
        Dim xmlReader As System.Xml.XmlReader = Nothing
        Dim reader As System.Xaml.XamlReader = Nothing
        Dim objectWriter As System.Xaml.XamlObjectWriter = Nothing
        Try 
            Dim schemaContext As System.Xaml.XamlSchemaContext = XamlStaticHelperNamespace._XamlStaticHelper.SchemaContext
            xmlReader = System.Xml.XmlReader.Create(initializeXaml)
            Dim readerSettings As System.Xaml.XamlXmlReaderSettings = New System.Xaml.XamlXmlReaderSettings()
            readerSettings.LocalAssembly = System.Reflection.Assembly.GetExecutingAssembly
            readerSettings.AllowProtectedMembersOnRoot = true
            reader = New System.Xaml.XamlXmlReader(xmlReader, schemaContext, readerSettings)
            Dim writerSettings As System.Xaml.XamlObjectWriterSettings = New System.Xaml.XamlObjectWriterSettings()
            writerSettings.RootObjectInstance = Me
            writerSettings.AccessLevel = System.Xaml.Permissions.XamlAccessLevel.PrivateAccessTo(GetType(GPcontrol))
            objectWriter = New System.Xaml.XamlObjectWriter(schemaContext, writerSettings)
            System.Xaml.XamlServices.Transform(reader, objectWriter)
        Finally
            If (Not (xmlReader) Is Nothing) Then
                CType(xmlReader,System.IDisposable).Dispose
            End If
            If (Not (reader) Is Nothing) Then
                CType(reader,System.IDisposable).Dispose
            End If
            If (Not (objectWriter) Is Nothing) Then
                CType(objectWriter,System.IDisposable).Dispose
            End If
        End Try
        Me.AfterInitializeComponent
    End Sub
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "4.0.0.0")>  _
    Private Function FindResource() As String
        Dim resources() As String = GetType(GPcontrol).Assembly.GetManifestResourceNames
        Dim i As Integer = 0
        Do While (i < resources.Length)
            Dim resource As String = resources(i)
            If (resource.Contains(".GPcontrol.g.xaml") OrElse resource.Equals("GPcontrol.g.xaml")) Then
                Return resource
            End If
            i = (i + 1)
        Loop
        Throw New System.InvalidOperationException("Resource not found.")
    End Function
    
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033"),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "4.0.0.0")>  _
    Sub System_ComponentModel_ISupportInitialize_BeginInit() Implements System.ComponentModel.ISupportInitialize.BeginInit
    End Sub
    
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033"),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "4.0.0.0")>  _
    Sub System_ComponentModel_ISupportInitialize_EndInit() Implements System.ComponentModel.ISupportInitialize.EndInit
        Me.InitializeComponent
    End Sub
End Class