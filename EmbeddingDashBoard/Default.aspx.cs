using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace EmbeddingDashBoard
{
    public partial class _Default : Page
    {
        public const string authResultString = "authResult";
        public AuthenticationResult authResult { get; set; }
        string baseUri = Properties.Settings.Default.PowerBiDataset;

        protected void Page_Load(object sender, EventArgs e)
        {

            //Test for AuthenticationResult
            if (Session[authResultString] != null)
            {
                //Get the authentication result from the session
                authResult = (AuthenticationResult)Session[authResultString];

                //Show Power BI Panel
                signInStatus.Visible = true;
                signInButton.Visible = false;
                signOffButton.Visible = true;

                //Set user and token from authentication result
                userLabel.Text = authResult.UserInfo.DisplayableId;
                accessTokenTextbox.Text = authResult.AccessToken;
            }
            else
            {
                signOffButton.Visible = false;
            }
        }

        protected void signInButton_Click(object sender, EventArgs e)
        {
            var @params = new NameValueCollection
            {
                {"response_type", "code"},
                {"client_id", Properties.Settings.Default.ClientID},
                {"resource", Properties.Settings.Default.PowerBiAPI},
                {"redirect_uri", "http://localhost:54337/Redirect"}
            };
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add(@params);
            string authorityUri = Properties.Settings.Default.AADAuthoritySignInUri;
            var authUri = String.Format("{0}?{1}", authorityUri, queryString);
            Response.Redirect(authUri);
        }

        protected void signOffButton_Click(object sender, EventArgs e)
        {
            var @params = new NameValueCollection
            {
                {"client_id", Properties.Settings.Default.ClientID},
                {"resource", Properties.Settings.Default.PowerBiAPI},
                {"redirect_uri", "http://localhost:54337/"},
                {"post_logout_redirect_uri", "http://localhost:54337/Redirect"}
            }; 
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add(@params);
            string authorityUri = Properties.Settings.Default.AADAuthoritySignOffUri;
            var authUri = String.Format("{0}?{1}", authorityUri, queryString);
            Response.Redirect(authUri);
        }

        protected void getDashboardsButton_Click(object sender, EventArgs e)
        {
            string responseContent = string.Empty;
            System.Net.WebRequest request = System.Net.WebRequest.Create(
                String.Format("{0}dashboards",
                baseUri)) as System.Net.HttpWebRequest;
            request.Method = "GET";
            request.ContentLength = 0;
            request.Headers.Add("Authorization", String.Format("Bearer {0}", authResult.AccessToken));
            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                //Get reader from response stream
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();

                    //Deserialize JSON string
                    PBIDashboards PBIDashboards = JsonConvert.DeserializeObject<PBIDashboards>(responseContent);

                    if (PBIDashboards != null)
                    {
                        var gridViewDashboards = PBIDashboards.value.Select(dashboard => new {
                            Id = dashboard.id,
                            DisplayName = dashboard.displayName,
                            EmbedUrl = dashboard.embedUrl
                        });

                        this.GridView1.DataSource = gridViewDashboards;
                        this.GridView1.DataBind();
                    }
                }
            }
        }


    }
    public class PBIDashboards
    {
        public PBIDashboard[] value { get; set; }
    }

    public class PBIDashboard
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string embedUrl { get; set; }
        public bool isReadOnly { get; set; }
    }
}
    
