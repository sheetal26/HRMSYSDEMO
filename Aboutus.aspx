<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHome.master" AutoEventWireup="true" CodeFile="~/Aboutus.aspx.cs" Inherits="Aboutus" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderhead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPage" runat="Server">
    <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>--%>
  <%--  <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="HeadDiv" Radius="50" Corners="All">
    </asp:RoundedCornersExtender>--%>

    <div id="HeadDiv" runat="server" class="content_resize" style="-moz-border-radius: 50px; -webkit-border-radius: 50px;border-radius: 50px;">
        <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
        <asp:SiteMapPath ID="SiteMap1" runat="server"></asp:SiteMapPath>
        <div class="content">
            <div class="mainbar">
                <div class="article_vert" style="float: left">
                    <h2>Integrated I.T. Solutions</h2>
                    <p><big><strong>Customised Software Developer</strong></big></p>
                    <img src="images/IITS.bmp" width="287" height="97" alt="" />
                    <p>I.I.T.S. Is A Proficient Desktop And Web Based Application Development Company.</p>
                    <p><a href="Contactus.aspx" class="red">Read more</a> | <a href="Contactus.aspx">Contact us</a> </p>
                </div>
                <div class="article_vert" style="float: left">
                    <h2>Description</h2>
                    <p>Our Software Group Offers Solution For Diamonds Process, Textile Process And Financial Systems.</p>
                    <p>
                        Our Services Are Based on a Time Proven Techniques That Brings Value to Our Customer.<br />
                        <a href="Contactus.aspx">Learn more...</a>
                    </p>
                </div>
                <div class="sidebar" style="float: left">
                    <div class="gadget">
                        <h2 class="star">Ours Features</h2>
                        <ul class="sb_menu">
                            <li><a href="#">Visual Studio 2012</a></li>
                            <li><a href="#">ASP.Net (C#,VB Script)</a></li>
                            <li><a href="#">.Net Framework 4.5</a></li>
                            <li><a href="#">Visual Basic 6.0</a></li>
                            <li><a href="#">Crystall Report XI Release 2</a></li>
                            <li><a href="#">MS Office 2007</a></li>
                            <li><a href="#">Sql Server 2008</a></li>
                        </ul>
                    </div>
                    <div class="gadget" style="float: left">
                        <h2 class="star">Basic Info</h2>
                        <ul class="ex_menu">
                            <li><a href="#">Started</a><br />
                                March 1, 2014</li>
                            <li><a href="#">Mission</a><br />
                                The Complete Solutions</li>
                        </ul>
                    </div>
                </div>
                <div class="clr"></div>
            </div>
        </div>
    </div>    
</asp:Content>

