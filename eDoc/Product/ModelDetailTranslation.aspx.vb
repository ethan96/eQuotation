Public Class ModelDetailTranslation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim _PartNo As String = Request("part_no"), _model_no As String = String.Empty
        'Finding out the model by model_product
        Dim _dt As DataTable = Business.GetModelByPartNo(_PartNo)
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            'Model name instead of part no
            _model_no = _dt.Rows(0).Item("DISPLAY_NAME")
        End If
        'Finding out the model by PRODUCT_LOGISTICS_NEW_V2
        If String.IsNullOrEmpty(_model_no) Then
            _dt = Business.GetModelByPartNoFromLOGISTICS(_PartNo)
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                _model_no = _dt.Rows(0).Item("model_name")
            End If
        End If

        If String.IsNullOrEmpty(_model_no) Then _model_no = _PartNo

        Response.Redirect(String.Format("http://my.advantech.com/product/model_detail.aspx?model_no={0}", HttpUtility.UrlEncode(_model_no)))
    End Sub

End Class