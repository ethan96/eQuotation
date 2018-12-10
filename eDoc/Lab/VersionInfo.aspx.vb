Imports System.Data.SqlClient
Imports System.IO
Imports Aspose.Pdf
Imports Winnovative

Public Class VersionInfo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Pivot.CurrentProfile.UserId.ToUpper.Contains(("Cathee.Cao").ToUpper) OrElse _
               Pivot.CurrentProfile.UserId.ToUpper.Contains(("Jay.Lee").ToUpper) OrElse _
                Role.IsAdmin Then
            Me.lbport.Text = HttpContext.Current.Request.ServerVariables("SERVER_PORT")
            Me.lbIsTesting.Text = COMM.Util.IsTesting
            Dim _con As New SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
            Me.lbeQDB.Text = _con.Database
            _con = Nothing
            Dim appPath As String = HttpContext.Current.Request.ApplicationPath
            Dim physicalPath As String = HttpContext.Current.Request.MapPath(appPath)
            Me.lbPubPath.Text = physicalPath
        End If
        Me.RoleIsUSAonlineSales.Text = Role.IsUsaUser()
        Me.RoleIsUSAACSales.Text = Role.IsUSAACSales()
        Me.GetRuntimeSiteUrl.Text = Util.GetRuntimeSiteUrl()
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '' Instantiate an object PDF class
        'Dim pdf As Aspose.Pdf.Pdf = New Aspose.Pdf.Pdf()
        '' add the section to PDF document sections collection
        'Dim section As Aspose.Pdf.Section = pdf.Sections.Add()

        '' Read the contents of HTML file into StreamReader object
        Dim appPath As String = HttpContext.Current.Request.ApplicationPath
        'Dim r As StreamReader = File.OpenText(HttpContext.Current.Request.MapPath(appPath) & "lab\versioninfo.aspx")
        ''Create text paragraphs containing HTML text
        'Dim text2 As Aspose.Pdf.Text = New Aspose.Pdf.Text(section, r.ReadToEnd())
        '' enable the property to display HTML contents within their own formatting
        'text2.IsHtmlTagSupported = True
        '' Add the text object containing HTML contents to PD Sections
        'section.Paragraphs.Add(text2)
        '' Specify the URL which serves as images database
        'pdf.HtmlInfo.ImgUrl = "d:\pdftest\MemoryStream\"

        ''Save the pdf document
        'pdf.Save("D:\pdftest\MemoryStream\HTML2pdf.pdf")


        'Dim basePath As [String] = HttpContext.Current.Request.MapPath(appPath) & "lab\"
        'Dim htmloptions As New LoadOptions(basePath)
        '' Load HTML file
        'Dim doc As New Document("input.html", htmloptions)
        '' Save HTML file
        'doc.Save("output.pdf")


    End Sub
End Class
