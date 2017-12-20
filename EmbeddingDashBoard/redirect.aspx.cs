using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmbeddingDashBoard
{
    public partial class Redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string redirectUri = String.Format("{0}Redirect", Properties.Settings.Default.RedirectUrl);
            string authorityUri = Properties.Settings.Default.AADAuthoritySignInUri;


            string code = Request.Params["code"];

            if (code != null)
            {

                TokenCache TC = new TokenCache();

                AuthenticationContext AC = new AuthenticationContext(authorityUri, TC);
                ClientCredential cc = new ClientCredential
                    (Properties.Settings.Default.ClientID,
                    Properties.Settings.Default.ClientSecret);

                AuthenticationResult AR = AC.AcquireTokenByAuthorizationCode(code, new Uri(redirectUri), cc);


                Session[_Default.authResultString] = AR;
            }
            else
            {

                Session[_Default.authResultString] = null;
            }

            Response.Redirect("/Default.aspx");
        }
    }
}