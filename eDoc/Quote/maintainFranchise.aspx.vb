Public Class maintainFranchise
    Inherits System.Web.UI.Page

    Public Function getDt() As DataTable
        Dim str As String = String.Format("select Sales_Code as [Sales Code],Company_ID as [Company Id], Email as [eMail] from FRANCHISER")
        Dim DT As New DataTable
        DT = tbOPBase.dbGetDataTableSchema("MY", str)
        Return DT
    End Function
    Sub doAddOrEdit()
        Dim email As String = CType(Me.plForm.FindControl("Email"), TextBox).Text
        Dim company As String = CType(Me.plForm.FindControl("Company Id"), TextBox).Text
        Dim salsecode As String = CType(Me.plForm.FindControl("Sales Code"), TextBox).Text
        If Not Util.isEmail(email) Then
            lbMsg.Text = "Email format Incorrect." : Exit Sub
        Else
            lbMsg.Text = ""
        End If
        If Not Business.is_Valid_Company_Id(company) Then
            lbMsg.Text = "Invalid Company Id." : Exit Sub
        Else
            lbMsg.Text = ""
        End If
        If Business.SSOIsExists(email) = False Then
            Me.lbEmail.Text = email
            Me.UPps.Update()
            Me.MPps.Show()
        Else
            Dim str As String = String.Format("delete from FRANCHISER where email='{2}';insert into FRANCHISER values('{0}','{1}','{2}')", salsecode, _
                                          company, _
                                          email)
            tbOPBase.dbExecuteNoQuery("MY", str)
        End If

        initGV(getDt)
    End Sub
    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim email As String = CType(Me.plForm.FindControl("Email"), TextBox).Text
        Dim company As String = CType(Me.plForm.FindControl("Company Id"), TextBox).Text
        Dim salsecode As String = CType(Me.plForm.FindControl("Sales Code"), TextBox).Text
        If Business.SSORegister(company, email, Me.txtPs.Text, "") Then
            Dim str As String = String.Format("delete from FRANCHISER where email='{2}';insert into FRANCHISER values('{0}','{1}','{2}')", salsecode, _
                                      company, _
                                      email)
            tbOPBase.dbExecuteNoQuery("MY", str)
        End If
        Me.MPps.Hide()
    End Sub
    Sub doEdit(ByVal sender As Object, ByVal e As EventArgs)
        Dim key As String = CType(sender, Button).CommandName
        Dim str As String = String.Format("select * from FRANCHISER WHERE EMAIL='{0}'", key)
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("MY", str)
        If dt.Rows.Count > 0 Then
            CType(Me.plForm.FindControl("Sales Code"), TextBox).Text = dt.Rows(0).Item("sales_code")
            CType(Me.plForm.FindControl("Company Id"), TextBox).Text = dt.Rows(0).Item("company_id")
            CType(Me.plForm.FindControl("Email"), TextBox).Text = dt.Rows(0).Item("email")
        End If
        initGV(getDt)
    End Sub
    Sub doDelete(ByVal sender As Object, ByVal e As EventArgs)
        Dim key As String = CType(sender, Button).CommandName
        Dim str As String = String.Format("delete from FRANCHISER WHERE EMAIL='{0}'", key)
        tbOPBase.dbExecuteNoQuery("MY", str)
        initGV(getDt)

    End Sub
    Function setKey(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) As String
        Dim DBITEM As DataRowView = CType(e.Row.DataItem, DataRowView)
        Return DBITEM.Item("eMail")
    End Function
    Protected Sub ibtnPickAccount_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CType(Me.ascxPickAccount.FindControl("hType"), HiddenField).Value = "ALL"
        Me.UPPickAccount.Update() : Me.MPPickAccount.Show()
    End Sub
    Public Sub PickAccountEnd(ByVal str As Object)
        Dim hQuoteErpId As String = IIf(Business.is_Valid_Company_Id(str(1).ToString), str(1).ToString, "")
        CType(Me.plForm.FindControl("Company Id"), TextBox).Text = hQuoteErpId
        If hQuoteErpId = "" Then
            CType(Me.plForm.FindControl("lbMsg1"), Label).Text = "Invalid Erp Id"
        Else
            CType(Me.plForm.FindControl("lbMsg1"), Label).Text = ""
        End If
        upF.Update()
        Me.MPPickAccount.Hide()
    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As New DataTable
        dt = getDt()
        initForm(dt)
        'If Not IsPostBack Then
        Me.gv.DataSource = dt
        'End If
        Me.gv.DataBind()
    End Sub
    Sub initGV(ByVal dt As DataTable)
        Me.gv.DataSource = dt
        Me.gv.DataBind()
    End Sub
    Private Sub initForm(ByVal dt As DataTable)
        Dim M As Integer = 3
        Dim tb As New Table
        For i As Integer = 0 To Math.Ceiling(dt.Columns.Count / M) - 1
            Dim tr = New TableRow
            For n As Integer = 0 To M - 1
                Dim tc1 As New TableCell
                Dim tc2 As New TableCell
                Dim c As Integer = i * M + n
                If c < dt.Columns.Count Then
                    tc1.Text = dt.Columns(c).ColumnName & ":"
                    Dim cT As New TextBox
                    cT.ID = dt.Columns(c).ColumnName
                    If dt.Columns(c).MaxLength < 0 Then
                        cT.Width = 300
                    ElseIf dt.Columns(c).MaxLength > 200 Then
                        cT.TextMode = TextBoxMode.MultiLine
                        cT.Rows = 4
                        cT.Width = 500
                    Else
                        cT.Width = dt.Columns(c).MaxLength * 2
                    End If

                    tc2.Controls.Add(cT)
                    If dt.Columns(c).ColumnName.ToLower.Contains("company") Then
                        Dim imgb As New ImageButton
                        imgb.ImageUrl = "~/Images/search.gif"
                        AddHandler imgb.Click, AddressOf ibtnPickAccount_Click
                        tc2.Controls.Add(imgb)
                        Dim lb As New Label
                        lb.ID = "lbMsg1"
                        lb.ForeColor = Drawing.Color.Red
                        tc2.Controls.Add(lb)
                    End If
                End If
                tr.Cells.Add(tc1) : tr.Cells.Add(tc2)
            Next
            tb.Rows.Add(tr)
        Next

        Me.plForm.Controls.Add(tb)
    End Sub


    Protected Sub gv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            Dim opCell As New TableCell
            opCell.Text = "Option"
            e.Row.Cells.Add(opCell)
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim opCell As New TableCell
            opCell.Text = "Option"
            Dim editB As New Button : Dim deleteB As New Button
            editB.Text = "Edit" : deleteB.Text = "Delete"
            editB.CommandName = setKey(e)
            deleteB.CommandName = setKey(e)
            AddHandler editB.Click, AddressOf doEdit
            AddHandler deleteB.Click, AddressOf doDelete
            opCell.Controls.Add(editB) : opCell.Controls.Add(deleteB)
            e.Row.Cells.Add(opCell)
        End If
    End Sub



    Protected Sub btnAddEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        doAddOrEdit()
    End Sub

End Class