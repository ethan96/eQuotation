Imports System.Reflection

Public Class PickSignature
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub


    Public Sub ShowData()
        LoadMySignatureList()
    End Sub

    Private Sub LoadMySignatureList()
        Dim str As New StringBuilder

        str.AppendLine(" SELECT SID,UserID,SignatureData,Active,FileName,LastUpdated FROM Signature ")
        str.AppendLine(" Where UserID='" & Pivot.CurrentProfile.UserId & "' Order by LastUpdated desc ")

        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", str.ToString)

        Me.GV1.DataSource = dt : Me.GV1.DataBind()
    End Sub


    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As Object = Me.GV1.DataKeys(row.RowIndex).Value
        Dim P As Page = Me.Parent.Page
        Dim TP As Type = P.GetType()
        Dim MI As MethodInfo = Nothing
        MI = TP.GetMethod("PickSignatureEnd")

        Dim para(0) As Object
        para(0) = key
        MI.Invoke(P, para)
    End Sub

End Class