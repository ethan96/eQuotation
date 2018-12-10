Imports System.Reflection
Public Class PickQuoteCopy
    Inherits System.Web.UI.UserControl
    Public Sub getData(ByVal desc As String)


        Dim SQLSTR As String = Business.getMySelfQuote(Pivot.CurrentProfile.UserId, desc.Replace("'", "''")) 'SiebelTools.GET_Siebel_Account_List(Name, RBU, erpid, country, location)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", SQLSTR)
        Me.GridView1.DataSource = dt
    End Sub

    Public Sub ShowData(ByVal desc As String)
        getData(desc)
        Me.GridView1.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(Me.txtName.Text.Trim.Replace("'", "''"))
    End Sub


    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As Object = Me.GridView1.DataKeys(row.RowIndex).Values
        Dim P As Page = Me.Parent.Page
        Dim TP As Type = P.GetType()
        Dim MI As MethodInfo = TP.GetMethod("PickQuoteCopyEnd")
        Dim para(0) As Object
        para(0) = key
        MI.Invoke(P, para)
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowData(Me.txtName.Text.Trim.Replace("'", "''"))
    End Sub
    Protected Sub btnQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim QuoteNo As String = Me.GridView1.DataKeys(row.RowIndex).Value
        'Dim newNo As String = Business.GetNoByPrefix(Pivot.CurrentProfile)
        'If copy(QuoteNo, newNo) Then
        '    Response.Redirect(String.Format("~/quote/QuotationMaster.aspx?UID={0}", newNo))
        'End If
        Dim NewQuoteID As String = String.Empty, ErrorStr As String = String.Empty, retbool As Boolean = False
        'Dim newNo As String = Business.GetNoByPrefix(Pivot.CurrentProfile)
        'copy(Util.ReplaceSQLChar(Me.txtRefId.Text.Trim), newNo)
        retbool = Business.CopyQuotation(QuoteNo.Trim, NewQuoteID, ErrorStr, COMM.Fixer.eDocType.EQ)
        If retbool Then
            Response.Redirect(String.Format("~/quote/QuotationMaster.aspx?UID={0}", NewQuoteID))
        Else
            Util.showMessage(ErrorStr)
        End If
    End Sub
    Dim myQD As New QuotationDetail("EQ", "quotationDetail")

    Function copy(ByVal oldNo As String, ByVal newNo As String) As Boolean
        If Not Business.isValidQuote(oldNo, COMM.Fixer.eDocType.EQ) Then
            Util.showMessage("Invalid Ref Id!")
            Return False
        End If
        Pivot.NewObjDocHeader.CopyPasteHeaderLine(oldNo, newNo, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
        myQD.Copy(oldNo, newNo)
        Return True
    End Function

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim data As DataRowView = e.Row.DataItem
            Dim createdby As String = data("createdBy")
            If Not String.IsNullOrEmpty(createdby) AndAlso createdby.IndexOf("@") > 0 Then
                createdby = createdby.Substring(0, createdby.IndexOf("@"))
            End If
            Dim lbCreatedby As Label = e.Row.FindControl("lbCreatedby")
            lbCreatedby.Text = createdby
            If Integer.TryParse(e.Row.Cells(3).Text.Trim, 0) Then
                Dim intstatus As Integer = Integer.Parse(e.Row.Cells(3).Text.Trim)
                e.Row.Cells(3).Text = COMM.EnumHelper.getDescription(CType(intstatus, COMM.Fixer.eDocStatus))
            End If
        End If
    End Sub

End Class