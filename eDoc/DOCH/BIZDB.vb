
Public Class dbac
    Shared Function getProductByLine(ByVal PN As String) As DataTable
        Dim str As String = " SELECT PART_NO, MODEL_NO, MATERIAL_GROUP, DIVISION, PRODUCT_HIERARCHY, PRODUCT_GROUP, PRODUCT_DIVISION, PRODUCT_LINE, " & _
        " GENITEMCATGRP, PRODUCT_DESC, ROHS_FLAG, EGROUP, STATUS, EDIVISION, NET_WEIGHT, VOLUME, WEIGHT_UNIT, GROSS_WEIGHT, " & _
        " VOLUME_UNIT, PRODUCT_TYPE, LAST_UPD_DATE, CREATE_DATE, SIZE_DIMENSIONS, GIP_CODE " & _
        " FROM SAP_PRODUCT " & _
        " WHERE (PART_NO = @PN) "
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@PN", PN)}
        Dim DT As New DataTable
        DT = sqlhelper.getDT("MY", CommandType.Text, str, p)
        Return DT
    End Function

End Class
