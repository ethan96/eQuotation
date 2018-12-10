Imports AjaxControlToolkit.HTMLEditor


Public Class NoToolBarEditor
    Inherits Editor
    Protected Overloads Overrides Sub FillTopToolbar()
        TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold())
        TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.Italic())
        TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.Underline())
        TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertLink())
    End Sub

    Protected Overloads Overrides Sub FillBottomToolbar()
        BottomToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignMode())
        BottomToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.HtmlMode())
        BottomToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.PreviewMode())
        TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertLink())
    End Sub
End Class
Public Class NoToolBarEditor2
    Inherits Editor
    Protected Overloads Overrides Sub FillTopToolbar()
        'TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold())
        'TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.Italic())
        'TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.Underline())
        'TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertLink())
    End Sub

    Protected Overloads Overrides Sub FillBottomToolbar()
        'BottomToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignMode())
        'BottomToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.HtmlMode())
        'BottomToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.PreviewMode())
        'TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.ToolbarButton.InsertLink())
    End Sub
End Class

