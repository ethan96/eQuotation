Public Class PLMDB_Test
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dt As New DataTable
        Dim sql As String = "Select * from addontbl.V_ZCHANGE_EPO_INITIAL"
        dt = OraDbUtil.dbGetDataTable("PLM_PRD", sql)

        Me.GridView1.DataSource = dt
        Me.GridView1.DataBind()

    End Sub

End Class