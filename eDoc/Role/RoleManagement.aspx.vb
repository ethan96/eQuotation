Public Class RoleManagement1
    Inherits System.Web.UI.Page

    Dim myRoleManagement As New RoleManagement("EQ", "tbRoleManagement")

    Dim IFURL As String = ""
    Dim IFName As String = ""
    Dim IFRoleValue As Decimal = 0
    Dim IFSeq As Integer = 0
    Dim IFCLASS As String = ""
    Function GetValFromForm() As Integer
        IFURL = Me.txtURL.Text.Replace("'", "''")
        IFName = Me.txtURLName.Text.Replace("'", "''")
        IFRoleValue = Me.txtRoleValue.Text.Replace("'", "''")
        IFCLASS = Me.txtClass.Text.Replace("'", "''")
        If IFURL = "" Then
            Util.showMessage(GetGlobalResourceObject("myRs", "URLNullError"))
            Return 0
        End If
        If IFName = "" Then
            Util.showMessage(GetGlobalResourceObject("myRs", "URLNameNullError"))
            Return 0
        End If
        If Not IsNumeric(IFRoleValue) Then
            Util.showMessage(GetGlobalResourceObject("myRs", "RoleValueShouldBeNum"))
            Return 0
        End If
        If Me.txtSeq.Text.Trim.Replace("'", "''") = "" Then
            IFSeq = 0
        Else
            IFSeq = Me.txtSeq.Text.Trim.Replace("'", "''")
        End If
        If IFCLASS = "" Then
            Util.showMessage("Class Can not be null！")
            Return 0
        End If
        Return 1
    End Function
    Function SetValToForm(ByVal IFURL As String, ByVal IFName As String, ByVal IFRoleValue As Decimal, ByVal IFSeq As Integer, ByVal IFCLASS As String) As Integer
        Me.txtURL.Text = IFURL
        Me.txtURLName.Text = IFName
        Me.txtRoleValue.Text = IFRoleValue
        Me.txtSeq.Text = IFSeq
        Me.txtClass.Text = IFCLASS
        Return 1
    End Function

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If GetValFromForm() = 1 Then
            myRoleManagement.Delete(String.Format("URL='{0}'", IFURL))
            myRoleManagement.Add(IFURL, IFName, IFRoleValue, IFSeq, IFCLASS)
            initGV()
        End If
    End Sub

    Protected Sub initGV()
        Dim dt As DataTable = myRoleManagement.GetDT("", "Seq")
        Me.GridView1.DataSource = dt
        Me.GridView1.DataBind()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim o As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As String = Me.GridView1.DataKeys(row.RowIndex).Value
        myRoleManagement.Delete(String.Format("URL='{0}'", key))
        initGV()
    End Sub

    Protected Sub ibtnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim o As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As String = Me.GridView1.DataKeys(row.RowIndex).Value
        Response.Redirect(String.Format("~/Role/RoleManagement.aspx?UID={0}", key))
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not IsNothing(Request("UID")) AndAlso Request("UID") <> "" Then
                initForm(Request("UID"))
            End If
            initGV()
        End If
    End Sub
    Protected Sub initForm(ByVal UID As String)
        Dim dt As DataTable = myRoleManagement.GetDT(String.Format("Url='{0}'", UID), "")
        If dt.Rows.Count > 0 Then
            IFURL = dt.Rows(0).Item("URL")
            IFName = dt.Rows(0).Item("Name")
            IFRoleValue = dt.Rows(0).Item("rolevalue")
            IFSeq = dt.Rows(0).Item("seq")
            IFCLASS = dt.Rows(0).Item("class")
            SetValToForm(IFURL, IFName, IFRoleValue, IFSeq, IFCLASS)
        End If
    End Sub

End Class