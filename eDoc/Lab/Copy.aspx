<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="Copy.aspx.vb" Inherits="EDOC.Copy" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UPAUSFreight" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
    <a id="copytext" runat="server" href="#" data-clipboard-text="ssssss">copy</a>
    <script type="text/javascript" src="../js/ZeroClipboard.js"></script>
    <script type="text/javascript">
        var clip = new ZeroClipboard(document.getElementById("<%=copytext.ClientID %>"), {
            moviePath: "../js/ZeroClipboard.swf"
        });

        clip.on('complete', function (client, args) {

            alert("Copied text to clipboard: " + args.text);

        });
    </script>

</ContentTemplate></asp:UpdatePanel>

    </asp:Content>