﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.18444
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18444.
'
Namespace AgsWS
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="USTaxServiceSoap", [Namespace]:="http://tempuri.org/")>  _
    Partial Public Class USTaxService
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private getSalesTaxByZIPOperationCompleted As System.Threading.SendOrPostCallback
        
        Private getStateTaxInfoOperationCompleted As System.Threading.SendOrPostCallback
        
        Private getZIPInfoOperationCompleted As System.Threading.SendOrPostCallback
        
        Private getPFPOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.EDOC.My.MySettings.Default.EDOC_AgsWS_USTaxService
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event getSalesTaxByZIPCompleted As getSalesTaxByZIPCompletedEventHandler
        
        '''<remarks/>
        Public Event getStateTaxInfoCompleted As getStateTaxInfoCompletedEventHandler
        
        '''<remarks/>
        Public Event getZIPInfoCompleted As getZIPInfoCompletedEventHandler
        
        '''<remarks/>
        Public Event getPFPCompleted As getPFPCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getSalesTaxByZIP", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function getSalesTaxByZIP(ByVal pStrZIP5Digit As String, ByRef pDecSalesTax As Decimal) As Boolean
            Dim results() As Object = Me.Invoke("getSalesTaxByZIP", New Object() {pStrZIP5Digit, pDecSalesTax})
            pDecSalesTax = CType(results(1),Decimal)
            Return CType(results(0),Boolean)
        End Function
        
        '''<remarks/>
        Public Overloads Sub getSalesTaxByZIPAsync(ByVal pStrZIP5Digit As String, ByVal pDecSalesTax As Decimal)
            Me.getSalesTaxByZIPAsync(pStrZIP5Digit, pDecSalesTax, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub getSalesTaxByZIPAsync(ByVal pStrZIP5Digit As String, ByVal pDecSalesTax As Decimal, ByVal userState As Object)
            If (Me.getSalesTaxByZIPOperationCompleted Is Nothing) Then
                Me.getSalesTaxByZIPOperationCompleted = AddressOf Me.OngetSalesTaxByZIPOperationCompleted
            End If
            Me.InvokeAsync("getSalesTaxByZIP", New Object() {pStrZIP5Digit, pDecSalesTax}, Me.getSalesTaxByZIPOperationCompleted, userState)
        End Sub
        
        Private Sub OngetSalesTaxByZIPOperationCompleted(ByVal arg As Object)
            If (Not (Me.getSalesTaxByZIPCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getSalesTaxByZIPCompleted(Me, New getSalesTaxByZIPCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getStateTaxInfo", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function getStateTaxInfo(ByVal pStrStateAbb2Digit As String, ByRef TAX_SHIPPING_ALONE As Boolean, ByRef Advan_Taxable As Boolean) As Boolean
            Dim results() As Object = Me.Invoke("getStateTaxInfo", New Object() {pStrStateAbb2Digit, TAX_SHIPPING_ALONE, Advan_Taxable})
            TAX_SHIPPING_ALONE = CType(results(1),Boolean)
            Advan_Taxable = CType(results(2),Boolean)
            Return CType(results(0),Boolean)
        End Function
        
        '''<remarks/>
        Public Overloads Sub getStateTaxInfoAsync(ByVal pStrStateAbb2Digit As String, ByVal TAX_SHIPPING_ALONE As Boolean, ByVal Advan_Taxable As Boolean)
            Me.getStateTaxInfoAsync(pStrStateAbb2Digit, TAX_SHIPPING_ALONE, Advan_Taxable, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub getStateTaxInfoAsync(ByVal pStrStateAbb2Digit As String, ByVal TAX_SHIPPING_ALONE As Boolean, ByVal Advan_Taxable As Boolean, ByVal userState As Object)
            If (Me.getStateTaxInfoOperationCompleted Is Nothing) Then
                Me.getStateTaxInfoOperationCompleted = AddressOf Me.OngetStateTaxInfoOperationCompleted
            End If
            Me.InvokeAsync("getStateTaxInfo", New Object() {pStrStateAbb2Digit, TAX_SHIPPING_ALONE, Advan_Taxable}, Me.getStateTaxInfoOperationCompleted, userState)
        End Sub
        
        Private Sub OngetStateTaxInfoOperationCompleted(ByVal arg As Object)
            If (Not (Me.getStateTaxInfoCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getStateTaxInfoCompleted(Me, New getStateTaxInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getZIPInfo", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function getZIPInfo(ByVal pStrZIP5Digit As String, ByRef pStrStateAbb2Digit As String, ByRef pStrCountyName As String, ByRef pStrCityName As String, ByRef TAX_SHIPPING_ALONE As Boolean, ByRef Advan_Taxable As Boolean) As Boolean
            Dim results() As Object = Me.Invoke("getZIPInfo", New Object() {pStrZIP5Digit, pStrStateAbb2Digit, pStrCountyName, pStrCityName, TAX_SHIPPING_ALONE, Advan_Taxable})
            pStrStateAbb2Digit = CType(results(1),String)
            pStrCountyName = CType(results(2),String)
            pStrCityName = CType(results(3),String)
            TAX_SHIPPING_ALONE = CType(results(4),Boolean)
            Advan_Taxable = CType(results(5),Boolean)
            Return CType(results(0),Boolean)
        End Function
        
        '''<remarks/>
        Public Overloads Sub getZIPInfoAsync(ByVal pStrZIP5Digit As String, ByVal pStrStateAbb2Digit As String, ByVal pStrCountyName As String, ByVal pStrCityName As String, ByVal TAX_SHIPPING_ALONE As Boolean, ByVal Advan_Taxable As Boolean)
            Me.getZIPInfoAsync(pStrZIP5Digit, pStrStateAbb2Digit, pStrCountyName, pStrCityName, TAX_SHIPPING_ALONE, Advan_Taxable, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub getZIPInfoAsync(ByVal pStrZIP5Digit As String, ByVal pStrStateAbb2Digit As String, ByVal pStrCountyName As String, ByVal pStrCityName As String, ByVal TAX_SHIPPING_ALONE As Boolean, ByVal Advan_Taxable As Boolean, ByVal userState As Object)
            If (Me.getZIPInfoOperationCompleted Is Nothing) Then
                Me.getZIPInfoOperationCompleted = AddressOf Me.OngetZIPInfoOperationCompleted
            End If
            Me.InvokeAsync("getZIPInfo", New Object() {pStrZIP5Digit, pStrStateAbb2Digit, pStrCountyName, pStrCityName, TAX_SHIPPING_ALONE, Advan_Taxable}, Me.getZIPInfoOperationCompleted, userState)
        End Sub
        
        Private Sub OngetZIPInfoOperationCompleted(ByVal arg As Object)
            If (Not (Me.getZIPInfoCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getZIPInfoCompleted(Me, New getZIPInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getPFP", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function getPFP(ByVal pParmList As String, ByVal pBolPoduction As Boolean, ByRef pResponse As String) As Boolean
            Dim results() As Object = Me.Invoke("getPFP", New Object() {pParmList, pBolPoduction, pResponse})
            pResponse = CType(results(1),String)
            Return CType(results(0),Boolean)
        End Function
        
        '''<remarks/>
        Public Overloads Sub getPFPAsync(ByVal pParmList As String, ByVal pBolPoduction As Boolean, ByVal pResponse As String)
            Me.getPFPAsync(pParmList, pBolPoduction, pResponse, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub getPFPAsync(ByVal pParmList As String, ByVal pBolPoduction As Boolean, ByVal pResponse As String, ByVal userState As Object)
            If (Me.getPFPOperationCompleted Is Nothing) Then
                Me.getPFPOperationCompleted = AddressOf Me.OngetPFPOperationCompleted
            End If
            Me.InvokeAsync("getPFP", New Object() {pParmList, pBolPoduction, pResponse}, Me.getPFPOperationCompleted, userState)
        End Sub
        
        Private Sub OngetPFPOperationCompleted(ByVal arg As Object)
            If (Not (Me.getPFPCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getPFPCompleted(Me, New getPFPCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")>  _
    Public Delegate Sub getSalesTaxByZIPCompletedEventHandler(ByVal sender As Object, ByVal e As getSalesTaxByZIPCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class getSalesTaxByZIPCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As Boolean
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),Boolean)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property pDecSalesTax() As Decimal
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(1),Decimal)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")>  _
    Public Delegate Sub getStateTaxInfoCompletedEventHandler(ByVal sender As Object, ByVal e As getStateTaxInfoCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class getStateTaxInfoCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As Boolean
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),Boolean)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property TAX_SHIPPING_ALONE() As Boolean
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(1),Boolean)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property Advan_Taxable() As Boolean
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(2),Boolean)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")>  _
    Public Delegate Sub getZIPInfoCompletedEventHandler(ByVal sender As Object, ByVal e As getZIPInfoCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class getZIPInfoCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As Boolean
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),Boolean)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property pStrStateAbb2Digit() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(1),String)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property pStrCountyName() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(2),String)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property pStrCityName() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(3),String)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property TAX_SHIPPING_ALONE() As Boolean
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(4),Boolean)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property Advan_Taxable() As Boolean
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(5),Boolean)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")>  _
    Public Delegate Sub getPFPCompletedEventHandler(ByVal sender As Object, ByVal e As getPFPCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class getPFPCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As Boolean
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),Boolean)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property pResponse() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(1),String)
            End Get
        End Property
    End Class
End Namespace
