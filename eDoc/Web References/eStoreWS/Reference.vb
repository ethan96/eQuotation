﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.18063
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
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18063.
'
Namespace eStoreWS
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="shippingrateSoap", [Namespace]:="http://tempuri.org/")>  _
    Partial Public Class shippingrate
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private getShippingRateOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.EDOC.My.MySettings.Default.EDOC_eStoreWS_shippingrate
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
        Public Event getShippingRateCompleted As getShippingRateCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getShippingRate", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function getShippingRate(ByVal order As Order) As Response
            Dim results() As Object = Me.Invoke("getShippingRate", New Object() {order})
            Return CType(results(0),Response)
        End Function
        
        '''<remarks/>
        Public Overloads Sub getShippingRateAsync(ByVal order As Order)
            Me.getShippingRateAsync(order, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub getShippingRateAsync(ByVal order As Order, ByVal userState As Object)
            If (Me.getShippingRateOperationCompleted Is Nothing) Then
                Me.getShippingRateOperationCompleted = AddressOf Me.OngetShippingRateOperationCompleted
            End If
            Me.InvokeAsync("getShippingRate", New Object() {order}, Me.getShippingRateOperationCompleted, userState)
        End Sub
        
        Private Sub OngetShippingRateOperationCompleted(ByVal arg As Object)
            If (Not (Me.getShippingRateCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getShippingRateCompleted(Me, New getShippingRateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
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
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public Class Order
        
        Private storeIdField As String
        
        Private shiptoField As Address
        
        Private billtoField As Address
        
        Private itemsField() As Item
        
        Private systemsField() As ConfigSystem
        
        '''<remarks/>
        Public Property StoreId() As String
            Get
                Return Me.storeIdField
            End Get
            Set
                Me.storeIdField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Shipto() As Address
            Get
                Return Me.shiptoField
            End Get
            Set
                Me.shiptoField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Billto() As Address
            Get
                Return Me.billtoField
            End Get
            Set
                Me.billtoField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Items() As Item()
            Get
                Return Me.itemsField
            End Get
            Set
                Me.itemsField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Systems() As ConfigSystem()
            Get
                Return Me.systemsField
            End Get
            Set
                Me.systemsField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public Class Address
        
        Private countrycodeField As String
        
        Private stateCodeField As String
        
        Private zipcodeField As String
        
        '''<remarks/>
        Public Property Countrycode() As String
            Get
                Return Me.countrycodeField
            End Get
            Set
                Me.countrycodeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property StateCode() As String
            Get
                Return Me.stateCodeField
            End Get
            Set
                Me.stateCodeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Zipcode() As String
            Get
                Return Me.zipcodeField
            End Get
            Set
                Me.zipcodeField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public Class Box
        
        Private widthField As Decimal
        
        Private lengthField As Decimal
        
        Private heightField As Decimal
        
        Private weightField As Decimal
        
        Private detailsField() As Item
        
        '''<remarks/>
        Public Property Width() As Decimal
            Get
                Return Me.widthField
            End Get
            Set
                Me.widthField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Length() As Decimal
            Get
                Return Me.lengthField
            End Get
            Set
                Me.lengthField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Height() As Decimal
            Get
                Return Me.heightField
            End Get
            Set
                Me.heightField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Weight() As Decimal
            Get
                Return Me.weightField
            End Get
            Set
                Me.weightField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Details() As Item()
            Get
                Return Me.detailsField
            End Get
            Set
                Me.detailsField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(ConfigSystem)),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public Class Item
        
        Private productIDField As String
        
        Private qtyField As Integer
        
        '''<remarks/>
        Public Property ProductID() As String
            Get
                Return Me.productIDField
            End Get
            Set
                Me.productIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Qty() As Integer
            Get
                Return Me.qtyField
            End Get
            Set
                Me.qtyField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public Class ConfigSystem
        Inherits Item
        
        Private detailsField() As Item
        
        '''<remarks/>
        Public Property Details() As Item()
            Get
                Return Me.detailsField
            End Get
            Set
                Me.detailsField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")> _
    Partial Public Class ShippingRate

        Private nmaeField As String

        Private rateField As Single

        '''<remarks/>
        Public Property Nmae() As String
            Get
                Return Me.nmaeField
            End Get
            Set(value As String)
                Me.nmaeField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Rate() As Single
            Get
                Return Me.rateField
            End Get
            Set(value As Single)
                Me.rateField = Value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public Class Response
        
        Private statusField As String
        
        Private messageField As String
        
        Private shippingRatesField() As ShippingRate
        
        Private boxexField() As Box
        
        '''<remarks/>
        Public Property Status() As String
            Get
                Return Me.statusField
            End Get
            Set
                Me.statusField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property message() As String
            Get
                Return Me.messageField
            End Get
            Set
                Me.messageField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property ShippingRates() As ShippingRate()
            Get
                Return Me.shippingRatesField
            End Get
            Set
                Me.shippingRatesField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Boxex() As Box()
            Get
                Return Me.boxexField
            End Get
            Set
                Me.boxexField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")>  _
    Public Delegate Sub getShippingRateCompletedEventHandler(ByVal sender As Object, ByVal e As getShippingRateCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class getShippingRateCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As Response
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),Response)
            End Get
        End Property
    End Class
End Namespace