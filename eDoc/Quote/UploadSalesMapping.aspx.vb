Public Class UploadSalesMapping
    Inherits System.Web.UI.Page

    Dim MS As New ManagerSalesMap("eq", "ManagerSalesMap")
    Sub initDT(ByRef DT As DataTable)
        DT.Columns.Add("M")
        DT.Columns.Add("S")
        'DT.Columns.Add("Extended Warranty")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim dt As New DataTable
            initDT(dt)
            Me.gv1.DataSource = dt
            Me.gv1.DataBind()
        End If

    End Sub

    Protected Sub btnImPort_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        import()

    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim filename As System.IO.Stream = upload()
        If Not IsNothing(filename) Then
            preview(filename)
        End If
    End Sub
    Function upload() As System.IO.Stream
        'Dim fileName As String = Server.MapPath("/") & "\Files\ITP.xls"
        If Me.FileUpload1.PostedFile.ContentLength > 0 Then
            Dim MSM As New System.IO.MemoryStream(Me.FileUpload1.FileBytes)
            'Me.FileUpload1.SaveAs(fileName)
            Return MSM
        End If
        Return Nothing
    End Function
    Sub import()
        'Try
        If Not IsNothing(ViewState("Cart")) Then
            Dim dttemp As DataTable = CType(ViewState("Cart"), DataTable)
            If dttemp.Rows.Count <= 0 Then
                Util.showMessage("No data be uploaded!")
                Exit Sub
            End If
            Dim dt As New DataTable
            initDT(dt)

            For Each r As DataRow In dttemp.Rows
                Dim rr As DataRow = dt.NewRow
                rr.Item("M") = r.Item(0)
                rr.Item("S") = r.Item(1)

                dt.Rows.Add(rr)
            Next

            If dt.Rows.Count > 0 Then
                MS.Delete(String.Format(""))

                For Each r As DataRow In dt.Rows
                    Dim M As String = r.Item("M")
                    Dim S As String = r.Item("S")
                    MS.Add(M, S, Pivot.CurrentProfile.UserId)
                Next
            End If
            Util.showMessage("Upload Success!")
        End If
        'Catch ex As Exception
        ' Util.showMessage(ex.Message)
        'End Try
    End Sub

    Sub preview(ByVal fileName As System.IO.Stream)

        Dim tempdt As DataTable = Util.ExcelFile2DataTable(fileName, 1, 0)
        If tempdt.Rows.Count <= 0 Then
            Util.showMessage("No data be uploaded!")
            Exit Sub
        End If
        If tempdt.Columns.Count <> 2 Then
            Util.showMessage("Columns Count Error!")
            Exit Sub
        End If

        Dim dt As New DataTable
        initDT(dt)
        For Each r As DataRow In tempdt.Rows
            Dim rr As DataRow = dt.NewRow
            rr.Item("M") = r.Item(0)
            rr.Item("S") = r.Item(1)
            dt.Rows.Add(rr)
        Next
        Me.gv1.DataSource = dt
        ViewState("Cart") = dt
        Me.gv1.DataBind()

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not IsNothing(ViewState("Cart")) Then
            'Try
            Dim dt As DataTable = CType(ViewState("Cart"), DataTable)
            If dt.Rows.Count > 0 Then
                Me.btnImPort.Visible = True
            End If
            'Catch ex As Exception

            'End Try
        End If
    End Sub

End Class