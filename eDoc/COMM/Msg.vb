Public Class Msg
    Public Enum eErrCode As ULong
        OK = 0
        <EnumDescription("HigherLevel cannot be found.")> _
        HigherLevelCannotBeFound = 2 ^ 0
        <EnumDescription("HigherLevel line must under startLine")> _
        HigherLevelMustUnderStartLine = 2 ^ 1
        <EnumDescription("Line no can not map to higherLevel.")> _
        LineNoCannotMapToHigherLevel = 2 ^ 2
        <EnumDescription("Update Line No faild,New line no can not map to higherLevel, items will exceed arrange of parent.")> _
        UpdateLineNoNewLineNoCannotMapToHigherLevel = 2 ^ 3
        <EnumDescription("Get new line no failed, New line no can not map to higherLevel.")> _
        NewLineCannotMapToHigherLevel = 2 ^ 4
        <EnumDescription("Get new line no failed, New line no can not map to itemType.")> _
        NewLineCannotMapToItemType = 2 ^ 5
        <EnumDescription("Incorrect Item Type.")> _
        ItemTypeIncorrect = 2 ^ 6
        <EnumDescription("Invalid PartNo.")> _
        InvalidPartNo = 2 ^ 7
        <EnumDescription("Only AGSEW item can be set Item Type AGS.")> _
        AGSEWItemTypeIncorrect = 2 ^ 8
        <EnumDescription("Field name definition duplicated.")> _
        FieldNameDefinitionDuplicated = 2 ^ 9
        <EnumDescription("Login ID or password is incorrect.")> _
        LoginIDorPasswordincorrect = 2 ^ 10
        <EnumDescription("System cannot identify your user role. Please contact us at <a href='mailto:MyAdvantech@advantech.com'>MyAdvantech@advantech.com</a> .")> _
        CannotFindProperRoleMappingToCurrentUser = 2 ^ 11
    End Enum
End Class
<AttributeUsage(AttributeTargets.Field, AllowMultiple:=False)> _
Public NotInheritable Class EnumDescriptionAttribute : Inherits Attribute
    Dim _description As String
    Public ReadOnly Property description As String
        Get
            Return _description
        End Get
    End Property

    Public Sub New(ByVal pdescription As String)
        MyBase.New()
        Me._description = pdescription
    End Sub
End Class
Public Class EnumHelper
    Public Shared Function getDescription(ByVal value As [Enum]) As String
        If IsNothing(value) Then
            Throw New ArgumentNullException("value")
        End If
        Dim description As String = value.ToString
        Dim fieldInfo As System.Reflection.FieldInfo = value.GetType().GetField(description)
        Dim ats() As EnumDescriptionAttribute = CType(fieldInfo.GetCustomAttributes(GetType(EnumDescriptionAttribute), False), EnumDescriptionAttribute())
        If Not IsNothing(ats) AndAlso ats.Length > 0 Then
            description = ats(0).description
        End If
        Return description
    End Function
End Class
Public Class errMsg
    Shared Function getErrMsg(ByVal errCode As Msg.eErrCode) As String
        Dim em As String = ""
        Dim t As Type = GetType(Msg.eErrCode)
        For Each s As String In [Enum].GetNames(t)
            If ([Enum].Format(t, [Enum].Parse(t, s), "d") And errCode) = [Enum].Format(t, [Enum].Parse(t, s), "d") Then
                If [Enum].Format(t, [Enum].Parse(t, s), "d") <> Msg.eErrCode.OK Then
                    em = em & "<br/>" & EnumHelper.getDescription(CType([Enum].Format(t, [Enum].Parse(t, s), "d"), Msg.eErrCode))
                End If
            End If
        Next
        Return em
    End Function
End Class