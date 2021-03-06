﻿using System;
using System.Collections.Generic;

using System.Windows.Forms;

using Facebook;

namespace BulkFBUploader
{
    public partial class FrmLogin : Form
    {

        public FrmLogin()
        {
            InitializeComponent();
            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(OnWebBrowserNavigated);
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            Dictionary<String, Object> parameters = new Dictionary<String, Object>
            {
                { "client_id", GlobalClass.FACEBOOK_APP_ID },
                { "response_type", "token" }
            };
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            String scope = String.Join(",", GlobalClass.PERMISSIONS);
            parameters["scope"] = scope;

            Uri loginUrl = GlobalClass.FB.GetLoginUrl(parameters);

            webBrowser1.Navigate(loginUrl);
        }

        private void OnWebBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // FacebookOAuthResult result;
            Boolean bSuccess = GlobalClass.FB.TryParseOAuthCallbackUrl(e.Url, out FacebookOAuthResult result);
            if (bSuccess && GlobalClass.FBAccessToken.Equals(""))
            {
                if (result.AccessToken != null)
                {
                    GlobalClass.FBAccessToken = result.AccessToken;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // login unsuccessfull, display and quit
                    GlobalClass.FBAccessToken = "";
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else if (!GlobalClass.FBAccessToken.Equals(""))
            {
                // keep login
                GlobalClass.FBAccessToken = "";
            }
            else if (!bSuccess)
            {
                // unsuccessful login
                GlobalClass.FBAccessToken = "";
            }

        }
    }
}
