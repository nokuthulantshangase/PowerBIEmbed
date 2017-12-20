using EmbeddingReport.Properties;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace EmbeddingReport
{
    public partial class _Default : Page
    {
        string baseUri = Properties.Settings.Default.PowerBiDataset;

        protected void Page_Load(object sender, EventArgs e)
        {

            //Need an Authorization Code from Azure AD before you can get an access token to be able to call Power BI operations
            //You get the Authorization Code when you click Get Report (see below).
            //After you call AcquireAuthorizationCode(), Azure AD redirects back to this page with an Authorization Code.
            if (Request.Params.Get("code") != null)
            {
                //After you get an AccessToken, you can call Power BI API operations such as Get Report
                Session["AccessToken"] = GetAccessToken(
                    Request.Params.GetValues("code")[0],
                    Settings.Default.ClientID,
                    Settings.Default.ClientSecret,
                    Settings.Default.RedirectUrl);

                //Redirect again to get rid of code=
                Response.Redirect("/Default.aspx");
            }
    
            if (Session["AccessToken"] != null)
            {

                accessToken.Value = Session["AccessToken"].ToString();

                //Get first report. 
                GetReport(0);
            }
        }

        protected void getReportButton_Click(object sender, EventArgs e)
        {
            //You need an Authorization Code from Azure AD so that you can get an Access Token
            //Values are hard-coded for sample purposes.
            GetAuthorizationCode();
        }


        //Get a Report. In this sample, you get the first Report.
        protected void GetReport(int index)
        {
            string responseContent = string.Empty;
            //  Configure Reports request
            System.Net.WebRequest request = System.Net.WebRequest.Create(
                String.Format("{0}/Reports",
                baseUri)) as System.Net.HttpWebRequest;

            request.Method = "GET";
            request.ContentLength = 0;
            request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken.Value));

            //  Get Reports response from request.GetResponse()
            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                //  Get reader from response stream
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                    //  Deserialize JSON string
                    PBIReports Reports = JsonConvert.DeserializeObject<PBIReports>(responseContent);

                    if (Reports != null)
                    {
                        var gridViewDashboards = Reports.value.Select(dashboard => new {
                            Id = dashboard.id,
                            DisplayName = dashboard.name,
                            EmbedUrl = dashboard.embedUrl
                        });

                        this.GridView1.DataSource = gridViewDashboards;
                        this.GridView1.DataBind();
                    }
                }
            }
        }

        public void GetAuthorizationCode()
        {
            //NOTE: Values are hard-coded for sample purposes.
            //Create a query string
            //Create a sign-in NameValueCollection for query string
            var @params = new NameValueCollection
            {
                //Azure AD will return an authorization code. 
                {"response_type", "code"},

                //Client ID is used by the application to identify themselves to the users that they are requesting permissions from. 
                //You get the client id when you register your Azure app.
                {"client_id", Settings.Default.ClientID},

                //Resource uri to the Power BI resource to be authorized
                //The resource uri is hard-coded for sample purposes
                {"resource", Properties.Settings.Default.PowerBiAPI},

                //After app authenticates, Azure AD will redirect back to the web app. In this sample, Azure AD redirects back
                //to Default page (Default.aspx).
                { "redirect_uri", Settings.Default.RedirectUrl}
            };

            //Create sign-in query string
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add(@params);

            //Redirect to Azure AD to get an authorization code
            Response.Redirect(String.Format(Properties.Settings.Default.AADAuthorityUri + "?{0}", queryString));
        }

        public string GetAccessToken(string authorizationCode, string clientID, string clientSecret, string redirectUri)
        {
            //Redirect uri must match the redirect_uri used when requesting Authorization code.
            //Note: If you use a redirect back to Default, as in this sample, you need to add a forward slash
            //such as http://localhost:13526/

            // Get auth token from auth code       
            TokenCache TC = new TokenCache();

            //Values are hard-coded for sample purposes
            string authority = Properties.Settings.Default.AADAuthorityUri;
            AuthenticationContext AC = new AuthenticationContext(authority, TC);
            ClientCredential cc = new ClientCredential(clientID, clientSecret);

            //Set token from authentication result
            return AC.AcquireTokenByAuthorizationCode(
                authorizationCode,
                new Uri(redirectUri), cc).AccessToken;
        }
    }

    //Power BI Reports used to deserialize the Get Reports response.
    public class PBIReports
    {
        public PBIReport[] value { get; set; }
    }
    public class PBIReport
    {
        public string id { get; set; }
        public string name { get; set; }
        public string webUrl { get; set; }
        public string embedUrl { get; set; }
    }
}