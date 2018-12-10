<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WallContent.ascx.vb" Inherits="EDOC.WallContent" %>
<%@ Register Assembly="AOnlineWall" Namespace="AOnlineWall.Business.BaseControls" TagPrefix="AOnline" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>
<%@ Register Src="~/Wall/Module/HTMLEditor.ascx" TagName="HTMLEditor" TagPrefix="Aonline" %>
<script>
    function selectUploadOrWebLink() {
        if ($("#<%=ddlFileType.ClientID %>").val() == "Web Link") {
            $(".trWebLink").show();
            $(".trUploadFile").hide();
        }
        else {
            $(".trWebLink").hide();
            $(".trUploadFile").show();
        }
    }
    $(function () {
        $("#<%=btnUploadFile.ClientID %>").click(function () {
            if ($("#<%=ddlFileType.ClientID %>").val() == "Web Link") {
                var urlLink = $("#<%=txtWebUrlLink.ClientID %>").val();
                urlLink = urlLink.replace(/(^\s*)|(\s*$)/g, "");
                if (urlLink == "" || urlLink == undefined || urlLink == null || urlLink.length == 0) {
                    alert("please enter web link.");
                    return false;
                }
                else {
                    var urlExample = true;
                    if (urlLink.length > 7 && urlLink.substr(0, 8).toUpperCase() == "HTTPS://") {
                        urlExample = false;
                    }
                    else if (urlLink.length > 6 && urlLink.substr(0, 7).toUpperCase() == "HTTP://") {
                        urlExample = false;
                    }
                    if (urlExample) {
                        alert("Example:  \n             http://www.advantech.com\n             https://www.advantech.com");
                        return false;
                    }
                }
            }
            else {
                var fileName = $("#<%=fuFile.ClientID %>").val();
                if (fileName == "" || fileName == undefined || fileName == null || fileName.length == 0) {
                    alert("please upload file.");
                    return false;
                }
            }
            $("#imgLoaderDiv").show();
            $("#<%=btnUploadFile.ClientID %>").hide();
            return true;
        });
    });
</script>
<asp:HiddenField ID="hfCategoryId" runat="server" Value="0" />
<table width="100%">
    <tr>
        <td>
            <table width="100%">
                <tr style="background-color:#0072c6;">
                    <td  valign="top">
                        <table class="tdIcon">
                            <tr>
                                <td class="clearPaddingTopBottom">
                                        <img src="/Images/back.png" alt="Back" class="iconM" 
                                            onmouseover="this.src='/Images/backdis.png';" onmouseout="this.src='/Images/back.png';"/>
                                </td>
                                <td class="clearPaddingTopBottom">
                                    <img src="/Images/godis.png" alt="Forward" class="iconM"  
                                            onmouseover="this.src='/Images/go.png';" onmouseout="this.src='/Images/godis.png';"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table>
                            <tr>
                                <td align="right" class="clearPaddingTopBottom">
                                    <span class="bold colorWhite">Search:&nbsp;</span>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="260px"></asp:TextBox>
                                </td>
                                <td align="right" class="clearPaddingTopBottom" style="width:22px;">
                                    <img src="/Images/search.png" alt="Search"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>

    <tr>
        <td>
            Category: <asp:Label ID="lblWallCategoryName" runat="server" Font-Bold="true" CssClass="colorRed" Text="Root"></asp:Label>
            <p></p>
        </td>
    </tr>

    <tr>
        <td class="clearPadding">
            <asp:Button runat="server" ID="btnNewCategory" Text="New Category" onclick="btnNewCategory_Click" class="right marginTopFirst" style="height:22px;" />
            <asp:TextBox ID="txtCategoryName" runat="server" class="right" Width="200"></asp:TextBox>
            <asp:CheckBox ID="ckbShowCategory" runat="server" Text="Category Information" onclick="showContainer(this,'categoryContainer')"/>
            
            
            <div id="categoryContainer" runat="server" clientidmode="Static">
                <asp:Label ID="lblCategoryMessage" runat="server" Visible="false" Text="Empty" ForeColor="Red" Font-Size="18px" ></asp:Label>
                <asp:GridView ID="gvWallCategory" runat="server" Width="100%" GridLines="None" rules="none" AutoGenerateColumns="false" 
                    CssClass="walltable" onrowdeleting="gvWallCategory_RowDeleting" onrowcancelingedit="gvWallCategory_RowCancelingEdit" 
                    onrowediting="gvWallCategory_RowEditing" onrowupdating="gvWallCategory_RowUpdating" >
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px">
                            <ItemTemplate>
                                <img alt="" src="/Images/folder.png" class="iconGV" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%# Eval("CategoryName")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCategoryName" runat="server" Width="200" Text='<%# Eval("CategoryName")%>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" ItemStyle-Width="150">
                            <ItemTemplate>
                                <%# Eval("UpdateDate")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Owner" ItemStyle-Width="250" ItemStyle-Wrap="true">
                            <ItemTemplate>
                                <%# Eval("Owner")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active" ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/Images/edit-2.png" CommandName="Edit" CommandArgument='<%# Eval("Id") %>' CssClass="iconGV" />
                                <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/Images/emptytrash.png" Width="20" Height="17" Visible="false" 
                                                  CommandName="Delete" CommandArgument='<%# Eval("Id") %>'   OnClientClick="javascript:return confirm('Are you Sure to Delte?');" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="imgUpdate" runat="server" ImageUrl="~/Images/filesave.png" CommandName="Update" CommandArgument='<%# Eval("Id") %>' CssClass="iconGV" />
                                <asp:ImageButton ID="imgCancel" runat="server" ImageUrl="~/Images/edit-cancel.png" CssClass="iconGV" CommandName="Cancel" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHeader" />
                    <RowStyle HorizontalAlign="Center" />
                    <PagerStyle HorizontalAlign="Left" CssClass="GridViewPagerBorder" />
                </asp:GridView>
            </div>
        </td>
    </tr>

    <tr>
        <td class="clearPadding">
            <table class="containerFirst" width="100%">
                <tr>
                    <td class="dottedBlue">
                        <asp:Button runat="server" ID="btnDeleteSelected" Text="Delete Selected" onclick="btnDeleteSelected_Click" Visible="false" OnClientClick="return checkDeleteFile()" />
                        <asp:Button ID="btnUploadDialog" runat="server" Text="Add Document" Enabled="false" 
                                    OnClientClick="showDialogById('containerUpload','Add Doc',615,'auto');return false;" />
                        <div class="hide"></div>
                    </td>
                </tr>
            </table>
            
            <div class="dottedBlue containerFirst center"> 
                <asp:Label ID="lblWallMessage" runat="server" Visible="false" Text="Empty" ForeColor="Red" Font-Size="18px" ></asp:Label>
                <asp:GridView ID="gvWall" runat="server" GridLines="None" rules="none" 
                    AutoGenerateColumns="false" CssClass="walltable" AllowPaging="true" PageSize="18" onrowdatabound="gvWall_RowDataBound"
                    OnPageIndexChanging="gvWall_PageIndexChanging" onrowcancelingedit="gvWall_RowCancelingEdit"  onrowupdating="gvWall_RowUpdating"  >
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30">
                            <HeaderTemplate>
                                <input type="checkbox" id="ckbAllWall" onclick="selectAllBox(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="ckbDelete" test="dd" class="checkAllBox"/>
                                <asp:HiddenField ID="hfFileId" runat="server" Value='<%#Eval("Id") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:Image ID="imgType" runat="server" ImageUrl="~/Images/File.jpg" CssClass="iconGV" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subject" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbName" runat="server" Font-Overline="false" ForeColor="Black" 
                                                            Visible="false" OnClick="lbDown_Click" ><%# Eval("Name")%></asp:LinkButton>
                                <asp:HyperLink ID="hlName" runat="server" Font-Overline="false" ForeColor="Black" Visible="false" ><%# Eval("Name")%></asp:HyperLink>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" runat="server" Width="200" Text='<%# Eval("Name")%>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FileSize" ItemStyle-Width="130">
                            <ItemTemplate>
                                <asp:Label ID="lblFileSize" runat="server">0KB</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" ItemStyle-Width="150">
                            <ItemTemplate>
                                <%# Eval("UpdateDate")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Owner" ItemStyle-Width="250" ItemStyle-Wrap="true">
                            <ItemTemplate>
                                <%# Eval("Owner")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active" ItemStyle-Width="100">
                            <ItemTemplate>
                                <%--<asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/Images/edit-2.png" CommandName="Edit" CommandArgument='<%# Eval("Id") %>' CssClass="iconGV" />--%>
                                <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/Images/edit-2.png" CommandArgument='<%# Eval("Id") %>' CssClass="iconGV" onclick="imgEdit_Click"/>
                                <asp:ImageButton ID="imgDown" runat="server" ImageUrl="~/Images/download.png" CssClass="iconGV" 
                                                AlternateText='<%# Eval("Name") %>' CommandArgument='<%# Eval("FileName") %>' onclick="imgDown_Click" />
                                <asp:HyperLink ID="hlDown" runat="server" BorderWidth="0" BorderStyle="None" CssClass="downButton" Visible="false" ImageUrl="~/Images/download.png" ></asp:HyperLink>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="imgUpdate" runat="server" ImageUrl="~/Images/filesave.png" CommandName="Update" CommandArgument='<%# Eval("Id") %>' CssClass="iconGV" />
                                <asp:ImageButton ID="imgCancel" runat="server" ImageUrl="~/Images/edit-cancel.png" CssClass="iconGV" CommandName="Cancel" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHeader" />
                    <RowStyle HorizontalAlign="Center" />
                    <PagerStyle HorizontalAlign="Left" CssClass="GridViewPagerBorder" />
                </asp:GridView>
            </div>

            <div id="containerUpload" class="hide">
                <table width="100%">
                    <tr>
                        <td>Subject: </td>
                        <td>
                            <HTMLEditor:Editor ID="editorSubject" runat="server" Height="170px" Width="100%" AutoFocus="true" ActiveMode="Design"  /> 
                        </td>
                    </tr>
                    <tr>
                        <td style=" width:100px; ">File Type: </td>
                        <td>
                            <asp:DropDownList ID="ddlFileType" runat="server" onchange="selectUploadOrWebLink()" Width="100">
                                <asp:ListItem Text="File" Value="File"></asp:ListItem>
                                <asp:ListItem Text="Web Link" Value="Web Link"></asp:ListItem> 
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="trWebLink hide">
                        <td>Web Link: </td>
                        <td><asp:TextBox ID="txtWebUrlLink" runat="server" Width="465"></asp:TextBox></td>
                    </tr>
                    <tr class="trWebLink hide">
                        <td></td>
                        <td>Example: <span class="colorRed">http://buy.advantech.com</span></td>
                    </tr>
                    <tr class="trUploadFile">
                        <td>Upload File: </td>
                        <td><asp:FileUpload ID="fuFile" runat="server" /></td>
                    </tr>
                    <tr class="trUploadFile">
                        <td><span class="colorRed">Max Size 20MB!</span></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnUploadFile" runat="server" Text="Submit" onclick="btnUploadFile_Click" />
                            <div id="imgLoaderDiv" class="hide"><img src="/Images/loader.gif" />please wait</div>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="editSubjectContainer" class="hide">
                <HTMLEditor:Editor ID="editorHtmlSubject" runat="server" Height="200px" Width="100%" ActiveMode="Design"  /> 
                <asp:Button ID="btnEditSubjectName" runat="server" Text="Update" onclick="btnEditSubjectName_Click" />
            </div>
        </td>
    </tr>
</table>