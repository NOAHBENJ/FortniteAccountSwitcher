using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Json;
using System.Text;
using System.Xml;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace FortniteAccountSwitcher
{
    public partial class Form2 : Form
    {
        public string AuthCode { get; private set; }

        public Form2()
        {
            InitializeComponent();
        }
        private void linkGoogle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.google.com");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            AuthCode = txtAuthCode.Text;
            string auth_code = txtAuthCode.Text;
            string result = MakeInitialRequest(auth_code);
            
            JObject deserialisedResult = JObject.Parse(result);
            string account_id = (string)deserialisedResult["account_id"];
            
            string devIDSecret = GenerateDeviceAuth(account_id, (string)deserialisedResult["access_token"]);
            deserialisedResult.Add("device_id", devIDSecret.Split(":")[0]);
            deserialisedResult.Add("secret", devIDSecret.Split(":")[1]);
            result = JsonConvert.SerializeObject(deserialisedResult, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(@"C:\Users\Noah\Desktop\" + account_id + ".json", result);
            MessageBox.Show("Account added successfully! Proof: " + GetAuthTokenFromDeviceSecret(account_id, devIDSecret.Split(":")[0], devIDSecret.Split(":")[1]));
        }

        private string MakeInitialRequest(string auth_code)
        {
            // Your client ID and secret
            string clientId = "3446cd72694c4a4485d81b77adbb2141";
            string clientSecret = "9209d4a5e25a457fb9b07489d313b41a";

            // The authorization code you received
            string authorizationCode = auth_code;

            // Encode the client ID and secret
            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            // Prepare the request body
            string requestBody = $"grant_type=authorization_code&code={authorizationCode}&client_id={clientId}&client_secret={clientSecret}";

            // Create a WebClient instance
            using (WebClient client = new WebClient())
            {
                // Set the headers
                client.Headers.Add("Authorization", $"Basic {credentials}");
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                // Make the POST request and get the response
                string response;
                try { response = client.UploadString("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token", requestBody); }
                catch (WebException ex) {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        MessageBox.Show("MakeInitialRequest - Function 1" + errorResponse);
                    }
                    return ex.Message;
                }
                //string response = client.UploadString("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token", requestBody);

                // Deserialize the JSON response
                var responseObject = JsonConvert.DeserializeObject(response);
                string jsonString = JsonConvert.SerializeObject(responseObject, Newtonsoft.Json.Formatting.Indented);

                // Return the JSON string
                Console.WriteLine(jsonString);
                return jsonString;
            }
            return null;
        }
        public string GenerateDeviceAuth(string accountId, string accessToken)
        {
            string url = $"https://account-public-service-prod03.ol.epicgames.com/account/api/public/account/{accountId}/deviceAuth";
            string userAgent = "Fortnite/++Fortnite+Release-21.00-CL-20463113 Windows/10";

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Bearer {accessToken}");
                client.Headers.Add("User-Agent", userAgent);
                try
                {
                    string response = client.UploadString(url, "");
                    JObject responseObject = JObject.Parse(response);

                    string deviceId = (string)responseObject["deviceId"];
                    string secret = (string)responseObject["secret"];
                    return deviceId + ":" + secret;
                }
                catch (WebException ex)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        MessageBox.Show("GenerateDeviceAuth - Function 2" + errorResponse);
                    }
                    return ex.Message;
                }
                return null;
            }
            return null;
        }
        public string GetAuthTokenFromDeviceSecret(string accountId, string device_id, string secret)
        {
            //string clientId = "3f69e56c7649492c8cc29f1af08a8a12";
            //string clientSecret = "b51ee9cb12234f50a69efa67ef53812e";
            string clientId = "34a02cf8f4414e29b15921876da36f9a";
            string clientSecret = "daafbccc737745039dffe53d94fc76cf";
            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            string requestBody = $"grant_type=device_auth&account_id={accountId}&device_id={device_id}&secret={secret}";
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Basic {credentials}");
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string response;
                try { response = client.UploadString("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token", requestBody); }
                catch (WebException ex) {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        MessageBox.Show("GetAuthTokenFromDeviceSecret - Function 3" + errorResponse);
                    }
                    return ex.Message; 
                }
                var responseObject = JsonConvert.DeserializeObject(response);
                string jsonString = JsonConvert.SerializeObject(responseObject, Newtonsoft.Json.Formatting.Indented);
                return jsonString;
            }
            return null;
        }
    }

    public class Data
    {
        public string auth_code { get; set; }
        public string username { get; set; }
        public string token { get; set; }
    }
}
