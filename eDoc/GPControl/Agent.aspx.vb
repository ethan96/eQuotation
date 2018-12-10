Public Class Agent
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Sqlstr As String = "select [email],[aemail],[seq],[fDate],[tDate],[id] from Agent order by cdate desc"
        Me.SqlDataSource1.SelectCommand = Sqlstr
        filter(Me.txtStr.Text)
        If Not Page.IsPostBack Then
            Me.GridView1.DataBind()
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim pri As String = ""
        Dim agent As String = ""
        Dim seq As String = ""
        Dim fdate As String = ""
        Dim tdate As String = ""
        If Me.txt_Primary.Text.Trim <> "" Then
            pri = Me.txt_Primary.Text.Trim
        Else
            Util.showMessage("Primary Owner cannot be null!")
            Exit Sub
        End If
        If Me.txt_Agent.Text.Trim <> "" Then
            agent = Me.txt_Agent.Text.Trim
        Else
            Util.showMessage("Agent cannot be null!")
            Exit Sub
        End If
        If Me.txt_Seq.Text.Trim <> "" Then
            seq = Me.txt_Seq.Text.Trim
        Else
            Util.showMessage("Sequence cannot be null!")
            Exit Sub
        End If
        If Me.txt_From.Text.Trim <> "" Then
            fdate = txt_From.Text.Trim
        Else
            Util.showMessage("From date cannot be null!")
            Exit Sub
        End If
        If Me.txt_To.Text.Trim <> "" Then
            tdate = Me.txt_To.Text.Trim
        Else
            Util.showMessage("To date cannot be null!")
            Exit Sub
        End If
        insert_Data(pri, agent, seq, fdate, tdate)
        Me.GridView1.DataBind()
    End Sub
    Protected Sub insert_Data(ByVal pri As String, ByVal agent As String, ByVal seq As String, ByVal fdate As String, ByVal tdate As String)
        Dim strSql As String = String.Format("insert into agent values('{0}','{1}','{2}','{3}','{4}',getDate())", pri, agent, seq, fdate, tdate)
        tbOPBase.dbExecuteNoQuery("EQ", strSql)
    End Sub
    Sub filter(ByVal key As String)
        key = Replace(key, "-", "–")
        Me.SqlDataSource1.FilterExpression = Me.drpFields.SelectedValue & " like '%" & key & "%'"
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.GridView1.DataBind()
    End Sub

End Class