<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EmbeddingReport._Default" %>
<asp:Content ID="BodyContent" style="background-color:dodgerblue" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="accessToken" runat="server" />
<div class="jumbotron" style="align-content:center;border-style:solid; text-align:center;background-color:darkgray;">

    <header >   
        <h2>
            <u>Power BI Embed Report</u>
        </h2>
        <h3 style="text-align:center">
           <b> Basic Sample: </b><br />
        </h3>
            <p>First make sure you <a href="https://dev.powerbi.com/apps">register your app</a>. <br>After registration, copy <u>Client ID</u> and <u>Client Secret</u> to web.config file.
        </p>
    </header>
   <div class="row" style="align-content:center;border-style:solid; text-align:center;">
     <div > 
        <asp:Panel ID="PanelDashboards" runat="server" Visible="true" >
           <div style="border-style:solid" >
        <p>
           <b> Select <u><i>"Get Report"</i></u> to get and embed first report from your Power BI account.</b>
        </p>
               <br/>
        <asp:Button ID="Button1" runat="server" OnClick="getReportButton_Click" Text="Get Report" />  
    </div>
            <div class="gridWrapper">
                <asp:GridView ID="GridView1" runat="server" CssClass="grid">
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
        </div>
     <div class="row"style="align-content:center;border-style:solid; text-align:center;">
         
        <asp:Panel ID="PanelEmbed" runat="server" Visible="true">
            <div style="align-content:center;border-style:solid; text-align:center;">
                <div><b class="step">Step 3</b>: Embed a Report</div>

                <div>Enter an embed url for a report from Step 2 (starts with https://):</div>
                <input type="text" id="tb_EmbedURL" />
                <br />
                <input type="button" id="bEmbedreportAction" value="Embed Report" />
            </div>
                 <div><b class="step">Step 4</b>:Embedded Report</div>               
                <br />
                <div  id="reportContainer" style="height: 608px;" />
        </asp:Panel>
                 <div class="row">
        Log View
        <br />
         <div ID="logView" style="align-content:center;border-style:solid;"></div>
    </div>
    </div>  
      </div> 
  
</asp:Content>