Public Class ProdInvalidList
    Inherits System.Web.UI.UserControl

    Public Function iSHaveInvalidItem(ByVal i As IBUS.iCartList) As IBUS.iCartList
        Dim valList as IBUS.iCartList = i.ItemValidate(COMM.Fixer.eCartItemValidateType.IsOrderable, Pivot.CurrentProfile)
        If Not IsNothing(valList) AndAlso valList.Count > 0 Then
            Return valList
        End If
        Return Nothing
    End Function
    Public Sub Bind(ByVal i As IBUS.iCartList)
        gvUnAvailablePartNo.DataSource = i
        gvUnAvailablePartNo.DataBind()
    End Sub
End Class