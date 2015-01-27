<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="~/WIP.aspx.cs" Inherits="WIP" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="Server">
    <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>--%>
   <%--  <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="HeadDiv" Radius="50" Corners="All">
     </asp:RoundedCornersExtender>--%>

    <div id="HeadDiv" runat="server" class="content_resize"  style="-moz-border-radius: 50px; -webkit-border-radius: 50px;border-radius: 50px;">  
        <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
        <asp:SiteMapPath ID="SiteMap1" runat="server"></asp:SiteMapPath>
        <div class="article" >
            <h2>Pending Work.....</h2>
        </div>
      
    </div>
</asp:Content>

