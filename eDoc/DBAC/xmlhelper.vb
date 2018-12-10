Imports System.Xml
Imports System.IO
Public Class xmlhelper
    Public Shared Function selectSingleNodeNyPrefixAndName(ByVal XFile As String, ByVal prefix As String, ByVal name As String, ByVal msg As String) As XmlNode
        Dim xdoc As New XmlDocument
        Dim fname As String = XFile
        If Not File.Exists(fname) Then
            msg = "config file '" & fname & "' not be found." : Return Nothing
        End If
        xdoc.Load(XFile)
        Dim nd As XmlNode = xdoc.SelectSingleNode("//root/" & prefix & "[@name='" & name & "']")
        If IsNothing(nd) Then
            msg = "config item '//root/" & prefix & "@name=" & name & "' not be found." : Return Nothing
        End If
        If IsNothing(nd.Attributes("value")) Then
            msg = "config attribute 'value' not be found in item '//root/" & prefix & "@name=" & name & "'" : Return Nothing
        End If
        Return nd
    End Function
End Class
