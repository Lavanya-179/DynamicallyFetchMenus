<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PT03_Content.ascx.cs" Inherits="IGDTheme.PT03_Content" %>

<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="dnn" TagName="META" Src="~/Admin/Skins/Meta.ascx" %>

<dnn:dnncssinclude id="IGDSyle" runat="server" filepath="../../../0/IGD_FrontEnd/dist/igd/styles.css" pathnamealias="SkinPath" name="IGDSyle" version="1.0" />
<dnn:meta id="mobileScale" runat="server" name="viewport" content="width=device-width,initial-scale=1" />
<link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">

<div id="siteWrapper">
    <igd-primary-nav user='<%= IGDTheme.LayoutController.UserString %>'
                     is-logged-in="<%= IGDTheme.LayoutController.LoggedIn %>"
                     secondary-menus='<%# IGDTheme.LayoutController.GetSecondaryNav() %>'
                     retailers='<%# IGDTheme.LayoutController.GetRetailers() %>'
                     markets='<%# IGDTheme.LayoutController.GetMarkets() %>'
                     secondary-nav-header="<%# IGDTheme.LayoutController.SecondaryNavHeader %>"
                     basket-count="2"
                     logo-src="/Portals/0/IGD_FrontEnd/dist/igd/assets/images/igd_colour.svg"
                     footer-logo-src="/Portals/0/IGD_FrontEnd/dist/igd/assets/images/igd_white.svg">
        <main slot="content" class="<%=IGDTheme.LayoutController.LayoutClass()%> v-occupy">
            <header runat="server" id="siteheader" class="">
                <div id="top_nav"></div>
            </header>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 nopadding" id="ContentPane" runat="server"></div>
                </div>
                <div class="row">
                    <div class="col-sm-12 nopadding" id="Pane_2" runat="server">
                    </div>
                </div>
                <div class="row pane-container">
                    <div class="col-sm-3 nopadding pane" id="Pane_3" runat="server"></div>
                    <div class="col-sm-6 nopadding pane">
                        <div class="content-pane" id="Pane_4" runat="server"></div>
                    </div>
                    <div class="col-sm-3 col-5 nopadding pane" id="Pane_5" runat="server"></div>
                </div>
                <div id="related" class="row related-content" visible="false" runat="server">
                    <div class="col-sm-8 nopadding pane ">
                        <h4 style="color:#373a36; justify-self:left">Related Content</h4>
                        <div class="col-sm-12 nopadding pane" id="Pane_6" runat="server"></div>
                    </div>
                </div>

                <div class="row">
                </div>
            </div>
        </main>
        <footer id="sitefooter">
            <div class="container">
                <div id="footer">
                </div>
            </div>
        </footer>
    </igd-primary-nav>
</div>
<style>

    .related-content {
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 30px;
    }

    .pane-container {
        height: fit-content;
        padding: 103px 20px 0 20px;
        padding-top: 36px;
    }

    .content-pane {
        padding-left: 16px;
        padding-right: 16px;
        width: 100% !important;
        -webkit-box-sizing: border-box;
        -moz-box-sizing: border-box;
        -ms-box-sizing: border-box;
        -o-box-sizing: border-box;
        box-sizing: border-box;
    }

    .pane {
        height: fit-content !important;
    }

    .v-occupy {
        flex: 1;
        display: flex;
    }

    .component {
        padding-top: 10px;
        padding-bottom: 5px;
    }
</style>
<script type="text/javascript">
    window.adobeClientId = "<%= IGDTheme.LayoutController.AdobeClientId %>";
    window.primaryMarker = '<%= IGDTheme.LayoutController.PrimaryMarker %>';
</script>
<script src="https://documentservices.adobe.com/view-sdk/viewer.js"></script>
<dnn:dnnjsinclude id="angularRuntime" runat="server" filepath="../../../0/IGD_FrontEnd/dist/igd/runtime.js" pathnamealias="SkinPath" addtag="false" />
<dnn:dnnjsinclude id="polyfills" runat="server" filepath="../../../0/IGD_FrontEnd/dist/igd/polyfills.js" pathnamealias="SkinPath" addtag="false" />
<dnn:dnnjsinclude id="angularComponents" runat="server" filepath="../../../0/IGD_FrontEnd/dist/igd/main.js" pathnamealias="SkinPath" addtag="false" />
<dnn:dnnjsinclude id="primaryNav" runat="server" filepath="js/primary-nav.js" pathnamealias="SkinPath" addtag="false" />
<script type="text/javascript">
    new PrimaryNavController();
</script>
