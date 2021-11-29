using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SWC_Visualization_App
{
    public partial class VisualizationForm : Form
    {
        private readonly HttpClient _httpClient = new();

        private string _clientId;
        private string _clientSecret;
        private string _redirectUri;
        private string _authenticationToken = "Not initialized";
        private string _accessToken = "Not initialized";

        private readonly string _baseUrl = @"https://www.swcombine.com/ws/v2.0/api/";
        private readonly string _separator = Uri.EscapeDataString(" ");
        private string[] _scopes =
        {
            "character_read",               /* Read basic character information */
            "personal_inv_overview",        /* Read basic information about your inventories */
            //"faction_inv_overview",         /* Read basic information about your faction's inventories */
            //"character_write"
        };
        private Form _webBrowserForm;

        private bool _isAuthenticated = false;




        public VisualizationForm()
        {
            InitializeComponent();
            GetClientInformation();
        }

        private async void Button_HelloWorld_OnButtonClick(object sender, EventArgs e)
        {
            _isAuthenticated = (await CheckAccess() is 200);
        }

        private void Button_Auth_OnButtonClick(object sender, EventArgs e)
        {
            Authorize();
        }

        /// <summary>
        /// Gets all the needed information from file. Call this at the start of the application.
        /// </summary>
        private void GetClientInformation()
        {
            const string path = @"..\..\Authentication\";

            _clientId = File.ReadAllText(path + ".clientid");
            _clientSecret = File.ReadAllText(path + ".secret");
            _redirectUri = File.ReadAllText(path + ".redirurl");
        }

        /// <summary>
        /// Checks the current authentication status. 
        /// </summary>
        /// <returns>200 for OK<br>Else for ERROR</br></returns>
        private async Task<int> CheckAccess()
        {
            string url = @"https://www.swcombine.com/ws/v2.0/api/helloauth/?access_token=" + _accessToken;
            int status = -1;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                status = (int)response.StatusCode;
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                MessageBox.Show(responseBody);

                return status;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message, "HTTP Error - Code: " + (status is -1 ? "" : status), MessageBoxButtons.OK, MessageBoxIcon.Error);

                return status;
            }
        }

        private void Authorize()
        {
            string url = string.Format("{0}?scope={1}&redirect_uri={2}&response_type=code&client_id={3}",
                @"https://www.swcombine.com/ws/oauth2/auth/",
                string.Join(_separator, _scopes),
                _redirectUri,
                _clientId);

            WebBrowser browser = new()
            {
                AllowWebBrowserDrop = false, /* prevents user control */
                Dock = DockStyle.Fill,
                Name = "browser",
                ScrollBarsEnabled = false, /* prevents user control */
                TabIndex = 0,
            };

            browser.Url = new Uri(url);
            browser.DocumentCompleted += (sender, e) => _webBrowserForm_DocumentCompleted(sender, e, url);


            _webBrowserForm = new Form();
            _webBrowserForm.WindowState = FormWindowState.Normal;
            _webBrowserForm.Controls.Add(browser);
            _webBrowserForm.Size = new Size(1000, 800);
            _webBrowserForm.Name = "Authorization Application";
            _webBrowserForm.Text = "Authorize the application";
            _webBrowserForm.FormClosed += _webBrowserForm_FormClosed;

            _webBrowserForm.ShowDialog();
        }

        private void ParseAuthUrl(NameValueCollection queryTokens)
        {
            string reason;

            if (queryTokens["code"] == null)
            {
                // the application was not authorized
                if (queryTokens["Error"] == "access_denied")
                {
                    _isAuthenticated = false;
                    reason = queryTokens["Error"];
                }
                else
                {
                    _isAuthenticated = false;
                    reason = queryTokens["description"];
                }
            }
            else
            {
                _authenticationToken = queryTokens["code"];
                return;
            }



            MessageBox.Show("Denied:\n\n" + reason);
        }

        private async Task<int> ParseAuthenticationToken()
        {
            string url = @"https://www.swcombine.com/ws/oauth2/token/";
            int status = -1;

            Dictionary<string, string> values = new()
            {
                { "code", _authenticationToken },
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "redirect_uri", _redirectUri },
                { "grant_type", "authorization_code" }
            };

            FormUrlEncodedContent data = new(values);
            

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, data);
                status = (int)response.StatusCode;
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                _accessToken = XDocument.Parse(responseBody).Descendants("access_token").Select(e => e.Value).SingleOrDefault();
                _isAuthenticated = true;

                MessageBox.Show(responseBody);

                return status;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message, "HTTP Error - Code: " + (status is -1 ? "" : status), MessageBoxButtons.OK, MessageBoxIcon.Error);

                return status;
            }
        }

        /// <summary>
        /// When the web browser gets closed manually, you won't get authenticated.
        /// </summary>
        private void _webBrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isAuthenticated = false;
        }

        private async void _webBrowserForm_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e, string url)
        {
            if (sender is not WebBrowser) return;

            if (e.Url.AbsolutePath == @"/members/index.php")
            {
                (sender as WebBrowser).Navigate(url);
            }
            else if (e.Url.AbsolutePath == @"/ws/oauth2/auth/code.php" || e.Url.AbsolutePath == @"/ws/oauth2/auth/error.php")
            {
                ParseAuthUrl(HttpUtility.ParseQueryString(e.Url.Query));
                await ParseAuthenticationToken();

                (sender as WebBrowser).FindForm().Close();
                (sender as WebBrowser).FindForm().Dispose();
            }

        }

       
    }
}
