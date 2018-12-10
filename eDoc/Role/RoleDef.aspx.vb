Imports System.Data
Public Class RoleDef
    Inherits System.Web.UI.Page

    'Dim myRole As New Role("EQ", "tbRole")

    'Dim IFRoleName As String = ""
    'Dim IFRoleValue As Decimal = 0
    'Function GetValFromForm() As Integer
    '    IFRoleName = Me.txtRoleName.Text.Replace("'", "''")
    '    If IFRoleName = "" Then
    '        Util.showMessage(GetGlobalResourceObject("myRs", "RoleNameNullError"))
    '        Return 0
    '    End If
    '    If Not IsNumeric(Me.txtRoleValue.Text.Replace("'", "''")) Then
    '        Util.showMessage(GetGlobalResourceObject("myRs", "RoleValueShouldBeNum"))
    '        Return 0
    '    End If
    '    IFRoleValue = Me.txtRoleValue.Text.Replace("'", "''")
    '    Return 1
    'End Function
    'Function SetValToForm(ByVal IFRoleName As String, ByVal IFRoleValue As Decimal) As Integer
    '    Me.txtRoleName.Text = IFRoleName
    '    Me.txtRoleValue.Text = IFRoleValue
    '    Return 1
    'End Function

    'Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If GetValFromForm() = 1 Then
    '        Dim RoleId As String = System.Guid.NewGuid().ToString
    '        Pivot.Add(RoleId, IFRoleName, IFRoleValue)
    '        initGV()
    '    End If
    'End Sub


    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Not IsPostBack Then
    '        If Not IsNothing(Request("UID")) AndAlso Request("UID") <> "" Then
    '            initForm(Request("UID"))
    '        End If
    '        initGV()
    '    End If
    'End Sub
    'Protected Sub initForm(ByVal UID As String)
    '    Dim dt As DataTable = Pivot.GetDT(String.Format("UId='{0}'", UID), "")
    '    If dt.Rows.Count > 0 Then
    '        IFRoleName = dt.Rows(0).Item("Name")
    '        IFRoleValue = CDec(dt.Rows(0).Item("Value"))
    '        SetValToForm(IFRoleName, IFRoleValue)
    '        Me.btnEdit.Visible = True
    '    End If
    'End Sub
    'Protected Sub initGV()
    '    Dim dt As DataTable = Pivot.GetDT("", "Value")
    '    Me.GridView1.DataSource = dt
    '    Me.GridView1.DataBind()
    'End Sub

    'Protected Sub ibtnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim o As ImageButton = CType(sender, ImageButton)
    '    Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
    '    Dim key As String = Me.GridView1.DataKeys(row.RowIndex).Value
    '    Response.Redirect(String.Format("~/User/RoleDef.aspx?UID={0}", key))
    'End Sub

    'Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim o As ImageButton = CType(sender, ImageButton)
    '    Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
    '    Dim key As String = Me.GridView1.DataKeys(row.RowIndex).Value
    '    Pivot.Delete(String.Format("UId='{0}'", key))
    '    initGV()
    'End Sub

    'Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If GetValFromForm() = 1 Then
    '        Pivot.Update(String.Format("UId='{0}'", Request("UID")), String.Format("Name=N'{0}',Value=N'{1}'", IFRoleName, IFRoleValue))
    '        Response.Redirect("~/User/RoleDef.aspx")
    '    End If
    'End Sub

End Class