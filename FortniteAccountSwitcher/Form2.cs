﻿using System;
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
using System.Net.Sockets;
using System.Text.Json.Nodes;
using System.Security.Policy;

namespace FortniteAccountSwitcher
{
    public partial class Form2 : Form
    {
        public string iOSClientID = "3446cd72694c4a4485d81b77adbb2141";
        public string iOSClientSecret = "9209d4a5e25a457fb9b07489d313b41a";

        public string AuthCode { get; private set; }

        public Form2()
        {
            InitializeComponent();
        }
        private void linkGoogle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.epicgames.com/id/api/redirect?clientId=3446cd72694c4a4485d81b77adbb2141&responseType=code";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // INFO: Gets Input of iOS Exchange Code, then gets the account_id and access_token from the code
            string auth_code = txtAuthCode.Text;
            string result = GrabiOSAuthCode(auth_code);

            // INFO: Deserialises the JSON response and gets the account_id and access_token
            JObject deserialisedResult = JObject.Parse(result);
            string account_id = (string)deserialisedResult["account_id"];
            string access_token = (string)deserialisedResult["access_token"];
            string accountUsername = (string)deserialisedResult["displayName"];

            // INFO: Generates the device_id and secret from the account_id and access_token
            string devIDSecret = GenerateDeviceAuth(account_id, access_token);
            deserialisedResult.Add("device_id", devIDSecret.Split(":")[0]);
            deserialisedResult.Add("secret", devIDSecret.Split(":")[1]);

            // INFO: Writes the JSON response to a file
            result = JsonConvert.SerializeObject(deserialisedResult, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("./" + account_id + ".json", result);

            txtUsername = accountUsername;
            txtAccountID = account_id;
            DialogResult = DialogResult.OK;
        }

        private string GrabiOSAuthCode(string auth_code)
        {
            string clientId = this.iOSClientID;
            string clientSecret = this.iOSClientSecret;
            string authorizationCode = auth_code;
            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            string requestBody = $"grant_type=authorization_code&code={authorizationCode}&client_id={clientId}&client_secret={clientSecret}";

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
                        MessageBox.Show("MakeInitialRequest - Function 1" + errorResponse);
                    }
                    return ex.Message;
                }
                var responseObject = JsonConvert.DeserializeObject(response);
                string jsonString = JsonConvert.SerializeObject(responseObject, Newtonsoft.Json.Formatting.Indented);

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

        // This function can be reused look at me reduce reuse and recycling im so good at this (kill me) (at least this isnt c++ or i would have to deal with pointers and memory management and the urge to kill myself would be even stronger) (nvm no it cant be reused i love life so extremely very much why is this so long help me)
        public string GenerateBearerToken(string accountId, string deviceId, string secret, string authorization)
        {
            string url = "https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token";
            var requestBody = $"grant_type=device_auth&account_id={accountId}&device_id={deviceId}&secret={secret}";

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Basic " + authorization); // fortniteIOSGameClient for 1st call, PC for #2 - this will make sense in the future i hope 
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                string response;
                try { response = client.UploadString(url, requestBody); }
                catch (WebException ex)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        MessageBox.Show("GenerateBearerToken - Function 3" + errorResponse);
                    }
                    return ex.Message;
                }
                return response;
            }
            return null;
        }

        // Function to idfk what this does i think it gets the exchange code from a bearer but idk i dont remember and i dont want to look at this code anymore
        public string GenerateExchangeCode(string bearer, string consumingClientID)
        {
            string url = "https://account-public-service-prod.ol.epicgames.com/account/api/oauth/exchange?consumingClientId=ec684b8c687f479fadea3cb2ad83f5c6";
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Bearer {bearer}");

                string response;
                try { response = client.DownloadString(url); }
                catch (WebException ex)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        MessageBox.Show("GenerateExchangeCode - Function 4" + errorResponse);
                    }
                    return ex.Message;
                }
                return response;
            }
            return "go fuck yourself Visual Studio";
        }

        // FMITA last function kill me now thank you very much
        public string GetBearerUsingAnExchangeCodeBecauseOfCourseItsSomethingDifferentKillMeNowThankYouKindly_IAmSlowlyLosingItWithThisFuckingAPI(string exchange_code, string authorization)
        {
            string url = "https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token";
            var requestBody = $"grant_type=exchange_code&exchange_code={exchange_code}";

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Basic " + authorization); // fortniteIOSGameClient for 1st call, PC for #2 - this will make sense in the future i hope 
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                string response;
                try { response = client.UploadString(url, requestBody); }
                catch (WebException ex)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        MessageBox.Show("GenerateBearerToken - Function 3" + errorResponse);
                    }
                    return ex.Message;
                }
                return response;
            }

            return "with mental help please i really need it this api is going to be the death of me";
        }

        /*
         
        ** Functions that will be used elsewhere in the application for misc things, like checking anticheat, friend codes etc**
         
         */

        // TODO: Check accounts provider for anticheat status
        // TODO: The below function returns "Operation anticheat not valid", please fix with a valid api endpoint

        public string CheckAntiCheat(string account_id, string bearerToken)
        {
            //i just dont know the url, if you do that would be helpful

            /*using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Bearer {bearerToken}");

                string response;
                try { response = client.DownloadString(url); }
                catch (WebException ex)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        MessageBox.Show("CheckAntiCheat - Function 5" + errorResponse);
                    }
                    return ex.Message;
                }
                return response; 
            } */
            return "i am so tired of this code please help me";
        }


        // nvm is needed and is pretty obvious what it does but i dont want to look at this code anymore so i will just leave it here and hope for the best
        public string launch(string account_id, string filePath) 
        {
            JObject fileDump = JObject.Parse(File.ReadAllText(filePath)); 
            string deviceId = fileDump["device_id"].ToString();
            string deviceSecret = fileDump["secret"].ToString();
            // INFO: Gets the bearer token from the device_id and secret
            string bearerTokenResponse = GenerateBearerToken(account_id, deviceId, deviceSecret, "MzQ0NmNkNzI2OTRjNGE0NDg1ZDgxYjc3YWRiYjIxNDE6OTIwOWQ0YTVlMjVhNDU3ZmI5YjA3NDg5ZDMxM2I0MWE=");
            JObject deserialisedBearerResult = JObject.Parse(bearerTokenResponse);
            string bearerToken = (string)deserialisedBearerResult["access_token"];

            // INFO: Gets an Exchange Code from the bearer token for fortniteIOSGameClient
            string lastIOSExchangeCodeResult = GenerateExchangeCode(bearerToken, "ec684b8c687f479fadea3cb2ad83f5c6"); // PC Client ID, CONSUMING so please dont hurt yourself by thinking too hard again noah
            JObject deserialised_LIOSEC_Result = JObject.Parse(lastIOSExchangeCodeResult);
            string lastIOSExchangeCode = (string)deserialised_LIOSEC_Result["code"];

            // INFO: Converts the fortniteIOSGameClient into a bearer token for fortnitePCGameClient despite everyone saying that only launcherAppClient2 can do it but idk this works so if it aint broke don't fix it? Epic API is fragile enough as it is
            string lastBearerTokenResponse = GetBearerUsingAnExchangeCodeBecauseOfCourseItsSomethingDifferentKillMeNowThankYouKindly_IAmSlowlyLosingItWithThisFuckingAPI(lastIOSExchangeCode, "ZWM2ODRiOGM2ODdmNDc5ZmFkZWEzY2IyYWQ4M2Y1YzY6ZTFmMzFjMjExZjI4NDEzMTg2MjYyZDM3YTEzZmM4NGQ=");
            JObject deserialised_LBTR_Result = JObject.Parse(lastBearerTokenResponse);
            string lastBearerToken = (string)deserialised_LBTR_Result["access_token"];

            // INFO: Gets an Exchange Code from the bearer token for fortnitePCGameClient which can log into game - now we just gotta turn this into a login code somehow but idk fuck me in the ass this was hard please help me my brain is dying
            string loginExchangeCodeResponse = GenerateExchangeCode(lastBearerToken, "ec684b8c687f479fadea3cb2ad83f5c6"); // PC Client ID, CONSUMING so please dont hurt yourself by thinking too hard again noah
            JObject deserialised_LECR_Result = JObject.Parse(loginExchangeCodeResponse);
            string loginExchangeCode = (string)deserialised_LECR_Result["code"];

            var p = new Process
            {
                StartInfo =
                {
                    FileName = $"cmd.exe",
                    WorkingDirectory = @"C:\Windows\System32",
                    Arguments = $"/C start /d \"C:\\Program Files\\Epic Games\\Fortnite\\FortniteGame\\Binaries\\Win64\" FortniteLauncher.exe -AUTH_LOGIN=unused -AUTH_PASSWORD={loginExchangeCode} -AUTH_TYPE=exchangecode -epicapp=Fortnite -epicenv=prod-fn -EpicPortal -epicuserid={account_id} "
                }
            }; p.Start(); 
            return "launched ig?"; 
        }
    }

    public class Data
    {
        public string auth_code { get; set; }
        public string username { get; set; }
        public string token { get; set; }
    }
}