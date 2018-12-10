<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HTMLEditor.ascx.vb" Inherits="EDOC.HTMLEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>
<HTMLEditor:Editor ID="editorFeature" runat="server" Height="250px" Width="100%"
  AutoFocus="true" ActiveMode="Design"  /> 
(<asp:Label ID="lblCurrentOverageByte" runat="server" ForeColor="Red"></asp:Label>
<asp:Label ID="lblCurrentMaxMinByte" runat="server"></asp:Label>
<asp:HiddenField ID="hfEditorFeature" runat="server" />
<script type="text/javascript">
    $(function () {
        //控件中无法用ID获取到textbox的 Value
        $(".ajax__htmleditor_htmlpanel_default").keyup(function () {
            checkHtmlInputFu2(this.value,this,"<%=lblCurrentOverageByte.ClientID %>",<%=MaxLengthX %>);
        });
        checkHtmlInputFu2($("#<%=hfEditorFeature.ClientID %>").val(), null, "<%=lblCurrentOverageByte.ClientID %>", <%=MaxLengthX %>);
    });
    
</script>