using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vjb.Sp.BackupLibrary.SpClient;

namespace Vjb.Sp.BackupLibrary
{
    public partial class frmSpLogin : Form
    {
        public SharepointClient SharepointClient { get; set; }
        public frmSpLogin()
        {
            InitializeComponent();
            ActiveControl = tbSiteUrl;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var url = tbSiteUrl.Text;
            var userName = tbUserName.Text;
            var password = tbPassword.Text;

            if (!Utilities.Utilities.IsValidUrl(url))
            {
                Utilities.Utilities.ShowErrorMessage("Invalid URL",
                    "You entered an invalid URL\n\rPlease try again");
                return;
            }

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                Utilities.Utilities.ShowErrorMessage("Invalid username/password",
                    "You entered an invalid username or password.\n\rPlease try again");
                return;
            }

            try
            {
                var sharepointClient = new SharepointClient(url, userName, password);
               SharepointClient = sharepointClient;
               Close();
            }
            catch (Exception exception)
            {
                Utilities.Utilities.ShowErrorMessage("Failed to connect", 
                    "Could not connect to Sharepoint Online\r\nPlease chech URL and credentials, and try again");
            }
        }
    }
}
