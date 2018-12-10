Imports Quartz

Public Class SiebelJob1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Msg.Text = String.Empty
    End Sub

    Protected Sub StartJob_Click(sender As Object, e As EventArgs) Handles StartJob.Click
        Dim myJob As New ScheduledJob()

        Try
            myJob.StopSiebelJob()
            myJob.StartSiebelJob()
            Msg.Text = "Starting Job Success"
        Catch ex As Exception
            Msg.Text = ex.ToString()
        End Try

    End Sub

    Protected Sub StopJob_Click(sender As Object, e As EventArgs) Handles StopJob.Click
        Dim myJob As New ScheduledJob()

        Try
            myJob.StopSiebelJob()
            Msg.Text = "Stop Job Success"
        Catch ex As Exception
            Msg.Text = ex.ToString()
        End Try
    End Sub

End Class