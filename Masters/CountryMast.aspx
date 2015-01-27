<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="CountryMast.aspx.cs" Inherits="Masters_CountryMast" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
        <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>
    <script type="text/javascript">
        function closepopup() {
            $find('ModalPopupExtender1').hide();
        }

        function validate() {
            var str = "";
            if (document.getElementById("<%=TxtCntName.ClientID%>").value == "") {
                str = str + "Country Name Is Blank, Enter Valid Country Value. \n";
            }
            if (str == "") {
                return (true);
            }
            else {
                alert(str);
                return (false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    
    <div id="HeadDiv" runat="server" class="content_resize"  style="-moz-border-radius: 50px; -webkit-border-radius: 50px;border-radius: 50px;">  
        <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
        <asp:SiteMapPath ID="SiteMap1" runat="server"></asp:SiteMapPath>
       
        <div class="article">
            <h2>Country Details</h2>
        </div>
        <div class="content">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td colspan="2" style="text-align: left;">
                                <asp:HiddenField ID="HidFldId" runat="server" />
                            </td>
                            <td>
                                <asp:HiddenField ID="HidFldUID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <th style="text-align: left;" class="auto-style1">Country Name</th>
                            <td style="text-align: left;">
                                <asp:TextBox ID="TxtCntName" runat="server" MaxLength="50" Text="" Width="250px"></asp:TextBox>
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button green" Height="25px" Width="77px" OnClick="BtnSave_Click" OnClientClick="return validate();"  />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="BtnView" runat="server" Text="View All" CssClass="button black" Height="25px" Width="77px" OnClick="BtnView_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:Label ID="LblMsg" runat="server" Text="Label" Font-Bold="True" Font-Size="Small" ForeColor="#0000CC"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"> 
                                <asp:GridView ID="GridCountry" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    PageSize="15" DataKeyNames="Id" 
                                    OnRowCancelingEdit="GridCountry_RowCancelingEdit"
                                    OnRowEditing="GridCountry_RowEditing"
                                    OnRowUpdating="GridCountry_RowUpdating"
                                    OnPageIndexChanging="GridCountry_PageIndexChanging"
                                    CssClass="mGrid">

                                    <Columns>
                                        <asp:TemplateField>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" AlternateText="Update" ImageUrl="~/images/update.jpg" ToolTip="Update" Height="20px" Width="20px" />
                                                <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" AlternateText="Cancel" ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" Height="20px" Width="20px" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" AlternateText="Edit" ImageUrl="~/images/Edit.jpg" ToolTip="Edit" Height="20px" Width="20px" />                                              
                                                <asp:ImageButton ID="imgbtnDelete" runat="server" OnClick="btnDelete_Click" AlternateText="Delete" ImageUrl="~/images/Delete.png" ToolTip="Delete" Height="20px" Width="20px" />
                                            </ItemTemplate>                                           
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Country Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtCountryName" runat="server" Width="350px" BorderStyle="None" ForeColor="Black" Text='<%# Bind("CountryName") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LblCountryName" runat="server" Text='<%# Bind("CountryName") %>' Width="350px" ForeColor="Black"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Id" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="LblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                                    CancelControlID="btnno" BackgroundCssClass="modalBackground">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="130px" Width="400px" Style="display: none;">
                                    <table style="border: Solid 2px #D46900; width: 100%; height: 100%">
                                        <tr style="background-image: url('../images/header-js.gif')">
                                            <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; text-align: left;">Confirm Box
                                            </td>
                                            <td style="color: White; font-weight: bold; padding: 3px; font-size: larger; text-align: right;">
                                                <a href="javascript:void(0)" onclick="closepopup()">
                                                    <img src="../images/Close-js.gif" style="border: 0px; text-align: right;" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; padding: 5px; font-family: Calibri">
                                                <asp:Label ID="lblUser" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style="text-align: right; padding-right: 15px">
                                                <asp:ImageButton ID="btnYes" OnClick="btnYes_Click" runat="server" ImageUrl="~/images/btnyes-js.jpg" />
                                                <asp:ImageButton ID="btnNo" runat="server" ImageUrl="~/images/btnNo-js.jpg" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

