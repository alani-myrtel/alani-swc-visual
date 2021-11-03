using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using SWC;

namespace SWC_Visualization_App
{
    public partial class VisualizationForm : Form
    {
        private WebBrowser _webBrowser = new();
        private readonly HttpClient _httpClient = new();

        private string _clientId;
        private string _clientSecret;
        private string _redirectUrl;
        private string _characterHandle = "Not initialized";
        private string _accessToken = "Not initialized";
        private string _cookie;

        private bool _isAuthenticated = false;




        public VisualizationForm()
        {
            InitializeComponent();
            GetClientInformation();
        }

        private async void Button_HelloWorld_OnButtonClick(object sender, EventArgs e)
        {
            _isAuthenticated = (await CheckAuthentication() is 200);
        }

        /// <summary>
        /// Gets all the needed information from file. Call this at the start of the application.
        /// </summary>
        private void GetClientInformation()
        {
            string[] output = { _clientId, _clientSecret, _redirectUrl };
            string[] inputFileNames = { ".clientid", ".secret", ".redirurl" };
            string path = @"..\..\Authentication\";

            _clientId = File.ReadAllText(path + inputFileNames[0]);
            _clientSecret = File.ReadAllText(path + inputFileNames[1]);
            _redirectUrl = File.ReadAllText(path + inputFileNames[2]);
        }

        /// <summary>
        /// Checks the current authentication status. 
        /// </summary>
        /// <returns>200 for OK<br>Else for ERROR</br></returns>
        private async Task<int> CheckAuthentication()
        {
            string url = @"https://www.swcombine.com/ws/v2.0/api/helloauth?access_token=" + _accessToken;
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

        
    }
}
