using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Linq;
using System.Windows.Forms;
using Facebook;
using System.Web.Script.Serialization;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private readonly Uri _loginUrl;
        protected FacebookClient client;

        public FacebookOAuthResult FacebookOAuthResult { get; private set; }
        public Form1()
        {
            InitializeComponent();
            client = new FacebookClient();
            dynamic parameters = new ExpandoObject();
            parameters.client_id = "558074537545521";
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrWhiteSpace("user_about_me,publish_stream"))
                parameters.scope = "publish_stream, manage_pages";
            _loginUrl = client.GetLoginUrl(parameters);
            //var loginUrl = client.GetLoginUrl(new { client_id = "seshu.miriyala.1", redirect_uri = "https://www.facebook.com/pages/My-Items/150336288455058" });

            //WebBrowser w = new WebBrowser();
            //w.Navigated += new WebBrowserNavigatedEventHandler(webBrowser_Navigated);
            //w.Navigate(loginUrl);
            //w.Show();
            //dynamic me = client.Get("150336288455058");
            //FacebookOAuthResult oauthResult;
            //Uri uri = new Uri("https://www.facebook.com/pages/My-Items/150336288455058");
            //if (client.TryParseOAuthCallbackUrl(uri, out oauthResult))
            //{
            //    // The url is the result of OAuth 2.0 authentication
            //    if (oauthResult.IsSuccess)
            //    {
            //        var accesstoken = oauthResult.AccessToken;
            //    }
            //    else
            //    {
            //        var errorDescription = oauthResult.ErrorDescription;
            //        var errorReason = oauthResult.ErrorReason;
            //    }
            //}
            //else
            //{
            //    var dicParams = new Dictionary<string, object>();
            //    dicParams["message"] = "Hi";
            //    dicParams["caption"] = string.Empty;
            //    dicParams["description"] = string.Empty;
            //    dicParams["name"] = "Hi1";
            //    dicParams["req_perms"] = "publish_stream";
            //    dicParams["scope"] = "publish_stream";

            //     //Get the access token of the posting user if we need to
            //    //if (destinationID != this.FacebookAccount.UserAccountId)
            //    //{
            //    dicParams["access_token"] = "manage_pages";
            //    //}
            //    //dynamic publishResponse = client.Post(me["link"] + "/messages", dicParams);
            //}
            //var dicParams = new Dictionary<string, object>();
            //dicParams["message"] = stSmContentTitle;
            //dicParams["caption"] = string.Empty;
            //dicParams["description"] = string.Empty;
            //dicParams["name"] = smContent.CmeUrl;
            //dicParams["req_perms"] = "publish_stream";
            //dicParams["scope"] = "publish_stream";

            // Get the access token of the posting user if we need to
            //if (destinationID != this.FacebookAccount.UserAccountId)
            //{
            //    dicParams["access_token"] = this.getPostingUserAuthToken(destinationID);
            //}
            //publishResponse = this.FacebookConnection.Post("/" + destinationID + "/feed", dicParams);
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // whenever the browser navigates to a new url, try parsing the url.
            // the url may be the result of OAuth 2.0 authentication.

            var fb = new FacebookClient();
            FacebookOAuthResult oauthResult;
            if (fb.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication
                if (oauthResult.IsSuccess)
                {
                    var accesstoken = oauthResult.AccessToken;
                }
                else
                {
                    var errorDescription = oauthResult.ErrorDescription;
                    var errorReason = oauthResult.ErrorReason;
                }
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
            }
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            client.UseFacebookBeta = true;
            client.IsSecureConnection = true;
            FacebookOAuthResult oauthResult;
            if (client.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication
                FacebookOAuthResult = oauthResult;
                client.AccessToken = FacebookOAuthResult.AccessToken;
                var fb = new FacebookClient(FacebookOAuthResult.AccessToken);

                // Note: the result can either me IDictionary<string,object> or IList<object>
                // json objects with properties can be casted to IDictionary<string,object> or IDictionary<string,dynamic>
                // json arrays can be casted to IList<object> or IList<dynamic>

                // for this particular request we can guarantee that the result is
                // always IDictionary<string,object>.
                var result = (IDictionary<string, object>)client.Get("me");

                // make sure to cast the object to appropriate type
                var id = (string)result["id"];

                // FacebookClient's Get/Post/Delete methods only supports JSON response results.
                // For non json results, you will need to use different mechanism,

                // here is an example for pictures.
                // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
                // for more info visit http://developers.facebook.com/docs/reference/api
                string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}", id, "square");
                pictureBox1.LoadAsync(profilePictureUrl);
                pictureBox1.Visible = true;
                var dicParams = new Dictionary<string, object>();
                dicParams["message"] = "Can You see this?";
                dicParams["caption"] = "Test2";
                dicParams["description"] = "first sample1";
                dicParams["name"] = "This is Test2";
                dicParams["req_perms"] = "manage_pages";
                dicParams["scope"] = "manage_pages";

                //Get the access token of the posting user if we need to
                //if (destinationID != this.FacebookAccount.UserAccountId)
                //{

                //}
                //var webClient = new WebClient();
                //string oauthUrl = string.Format("https://graph.facebook.com/me/accounts?access_token={0}", FacebookOAuthResult.AccessToken);

                //string data = webClient.DownloadString(oauthUrl);
                //var json = new JavaScriptSerializer();
                //var pages = (Dictionary<string, object>)json.DeserializeObject(data);
                //object[] pages2 = (object[])pages.FirstOrDefault(p => p.Key == "data").Value;
                //string accessToken = null;
                //foreach (object page in pages2)
                //{
                    //Dictionary<string, object> page2 = (Dictionary<string, object>)page;
                //    if (page2["name"].ToString() == "MQ163")
                //    {
                //        accessToken = page2["access_token"].ToString();
                //        break;
                //    }
                //}
                //var json = System.Runtime.Serialization.s
                //https://developers.facebook.com/docs/howtos/login/login-as-page/
                //https://graph.facebook.com/seshu.miriyala.1/accounts?access_token=AAACEdEose0cBAG7kZCHS59oeoB4fV6jjRsm4je68vpMQUqXM6G5YsasRCFs4wPi3ld0bdGSAZAZBLZBEbyPrMRHvSk78uJZAdApaZC7D6gx9NyE08vLf0W
                //http://developers.facebook.com/docs/getting-started/graphapi/
                //http://stackoverflow.com/questions/3010707/facebook-graph-api-facebook-pages
                dicParams["access_token"] = FacebookOAuthResult.AccessToken;
                //var fbc = new FacebookClient(accessToken);
                //string pagePosts = webClient.DownloadString(string.Format("https://graph.facebook.com/wikipedia/posts?access_token={0} ", accessToken));
                //String[] permissions = { "offline_access", "publish_stream", "user_photos", "publish_checkins", "photo_upload" };
                //FacebookClient.a.authorize(MainActivity.this, permissions,
                //            new LoginDialogListener());
                dynamic publishResponse = client.Post("https://graph.facebook.com/MQ163/feed", dicParams);
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
                FacebookOAuthResult = null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(_loginUrl.AbsoluteUri);
        }
    }

    public class Entity
    {
        public EntityData Data { get; set; }
    }
}
