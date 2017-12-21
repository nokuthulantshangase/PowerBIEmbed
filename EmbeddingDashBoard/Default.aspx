<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EmbeddingDashBoard._Default" %>

<asp:Content ID="BodyContent" style="background-color:dodgerblue" ContentPlaceHolderID="MainContent" runat="server">
   
<div class="jumbotron" style="align-content:center;border-style:solid;padding-top:25px; text-align:center;background-color:darkgray;">
    <h2>
    Power BI Embedded DashBoard</h2>
     <div class="row" style="align-content:center;border-style:solid; text-align:center;">
        <asp:Panel ID="SignInPanel" runat="server" Visible="true">     
            <p ><b class="step">Step 1</b>: Sign in to your Power BI account to link your account to this web application.</p>
            <p style="align-content:center; text-align:center">
                <asp:Button ID="signInButton" runat="server" OnClick="signInButton_Click" Text="Sign in to Power BI" />
                <asp:Button ID="signOffButton" runat="server" OnClick="signOffButton_Click" Text="Sign off from Power BI" />
            </p>   

            <asp:Panel ID="signInStatus" runat="server" Visible="true" BorderStyle="Solid" style="align-content:center; text-align:center">           
                     <div><b>Signed in as:</b>
                        <asp:Label ID="userLabel" runat="server"></asp:Label>
                  </div>
                <br/>
                    <div>
                        <b>Access Token:</b>              
                        <asp:TextBox ID="accessTokenTextbox"  runat="server"></asp:TextBox>                    
 </div>
            </asp:Panel>
        </asp:Panel>   
    </div>
    <hr class="stepHr" />
    <!-- Get Dashboards -->
    <div class="row"style="align-content:center;border-style:solid; text-align:center;">
        <asp:Panel ID="PanelDashboards" runat="server" Visible="true" BorderStyle="Solid" style="align-content:center; text-align:center">
            <div style="align-content:center; text-align:center">
                <div style="align-content:center; text-align:center">
                    <b class="step">Step 2</b>: Get dashboards from your account.
                </div>
                <asp:Button ID="Button1" runat="server" OnClick="getDashboardsButton_Click" Text="Get Dashboards" />
            </div>
            <div class="gridWrapper">
                <asp:GridView ID="GridView1" runat="server" CssClass="grid">
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
    <hr class="stepHr" />

    <!-- Embed Dashboard-->
    <div class="row"style="align-content:center;border-style:solid; text-align:center;">
        <asp:Panel ID="PanelEmbed" runat="server" Visible="true">
 <div style="align-content:center;border-style:solid; text-align:center;">
                <div>
                    <b class="step">Step 3</b>: Embed a dashboard
                </div>
                <div>Enter an embed url for a dashboard from Step 2 (starts with https://):</div>
                <input type="text" id="tb_EmbedURL" />
                <br />              
                <input  type="button" id="bEmbedDashboardAction" value="Embed Dashboard" />                
            </div>
          <div><b class="step">Step 4</b>:Embedded DashBoard</div>
  <br/>         
 <div  id="dashboardContainer" style="height: 608px;" />
        </asp:Panel>
        <div class="row">
        Log View
        <br />
         <div ID="logView" style="align-content:center;border-style:solid;"></div>
    </div> 
    </div>
    </div> 
</asp:Content>
