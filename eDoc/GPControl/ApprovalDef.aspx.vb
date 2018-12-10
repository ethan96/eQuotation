Public Class ApprovalDef
    Inherits System.Web.UI.Page

    Dim Otype As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("type") <> "" Then
            Otype = Request("type")
        Else
            Otype = "GP"
        End If

        If Otype = "GP" Then
            Me.lbCriterion.Text = "GP% : "
            Me.lbRangeFlag.Visible = False
        Else
            Me.lbCriterion.Text = "Amount Range : "
            Me.lbPercentageSign.Visible = False
        End If

        
        filter(Me.txtStr.Text)
        'Response.Write(Chr(95) & Chr(45))
        If Not IsPostBack Then
            'JJ 因為頁面初始化時會出現找不到dropdownlist的狀況，所以只好指定office_name='ADL'
            Dim Sqlstr As String = "select [Office_Name],[Group_Name],[gp_level],[approver],[active],[id] from GPBLOCK_LOGIC where office_name='ADL' and  type='" & Otype & "' order by GROUP_CODE,GP_LEVEL"
            Me.SqlDataSource1.SelectCommand = Sqlstr
            Me.GridView1.DataBind()
            Me.drpType.SelectedValue = Otype
        Else
            Dim Sqlstr As String = "select [Office_Name],[Group_Name],[gp_level],[approver],[active],[id] from GPBLOCK_LOGIC where office_code='" & Me.drpOfficeSelecter.SelectedValue & "' and type='" & Otype & "' order by GROUP_CODE,GP_LEVEL"
            Me.SqlDataSource1.SelectCommand = Sqlstr
            ' Me.GridView1.DataBind()
        End If
    End Sub


    Sub filter(ByVal key As String)
        key = Replace(key, "-", "–")
        Me.SqlDataSource1.FilterExpression = Me.drpFields.SelectedValue & " like '%" & key & "%'"
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oApprover As String = Me.TxtApprover.Text
        Dim oGP As Decimal = CType(Me.TxtGP.Text.ToString, Decimal) / 100
        Dim oOffice As String = Me.drpOffice.SelectedItem.Text.Split(")")(1)
        Dim oOfficeCode As String = Me.drpOffice.SelectedValue
        Dim oGroup As String = Me.drpGroup.SelectedItem.Text.Split(")")(1)
        Dim oGroupCode As String = Me.drpGroup.SelectedValue
        Dim oActive As Integer = Me.DrpStatus.SelectedValue


        tbOPBase.dbExecuteNoQuery("EQ", "insert into GPBLOCK_LOGIC (Approver,gp_level,office_name,office_code,group_name,group_code,active,TYPE) values " & _
                                "('" & oApprover & "','" & oGP & "','" & oOffice & "','" & oOfficeCode & "','" & oGroup & "','" & oGroupCode & "','" & oActive & "','" & Otype & "')")


        Me.TxtApprover.Text = ""
        Me.TxtGP.Text = ""
        Me.drpOffice.SelectedIndex = 0
        Me.drpGroup.SelectedIndex = 0
        Me.DrpStatus.SelectedIndex = 0
        Me.GridView1.DataBind()
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.GridView1.DataBind()
    End Sub

    Protected Sub drpOfficeSelecter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.GridView1.DataBind()
    End Sub

    Protected Sub drpType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("ApprovalDef.aspx?type=" & Me.drpType.SelectedValue)
    End Sub

    'Protected Sub drpOffice_DataBound(sender As Object, e As EventArgs)
    '    For Each item As ListItem In drpOffice.Items
    '        item.Text = "(" & item.Value & ")" & item.Text
    '    Next
    'End Sub

    'Protected Sub drpGroup_DataBound(sender As Object, e As EventArgs)
    '    For Each item As ListItem In drpGroup.Items
    '        item.Text = "(" & item.Value & ")" & item.Text
    '    Next
    'End Sub

    'Protected Sub drpOfficeSelecter_DataBound(sender As Object, e As EventArgs)
    '    For Each item As ListItem In drpOfficeSelecter.Items
    '        item.Text = "(" & item.Value & ")" & item.Text
    '    Next
    'End Sub

    Protected Sub drpGroup_DataBound(sender As Object, e As EventArgs) Handles drpGroup.DataBound
        For Each item As ListItem In drpGroup.Items
            item.Text = "(" & item.Value & ")" & item.Text
        Next
    End Sub

    Protected Sub drpOfficeSelecter_DataBound(sender As Object, e As EventArgs) Handles drpOfficeSelecter.DataBound
        For Each item As ListItem In drpOfficeSelecter.Items
            item.Text = "(" & item.Value & ")" & item.Text
        Next
    End Sub

    Protected Sub drpOffice_DataBound(sender As Object, e As EventArgs) Handles drpOffice.DataBound
        For Each item As ListItem In drpOffice.Items
            item.Text = "(" & item.Value & ")" & item.Text
        Next
    End Sub
End Class