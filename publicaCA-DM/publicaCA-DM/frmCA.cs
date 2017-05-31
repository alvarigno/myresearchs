using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;


namespace publicaCA_DM
{
    public partial class frmCA : Form
    {
        public static ChromiumWebBrowser browserCA;

        public frmCA()
        {
            InitializeComponent();
            InitializeChromium();

        }

        public void frmCA_Load(object sender, EventArgs e)
        {

        }

        public ChromiumWebBrowser chromeBrowser;


        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
            ///////Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("http://desarrollofotos.chileautos.cl/actualizadores/login.asp");
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.FrameLoadEnd += (sender, args) =>
            {
                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain)
                {

                    args.Frame.ExecuteJavaScriptAsync(login());

                }
            };

        }


        public static string login()
        {

            string script = "document.getElementById('user').value = 'cgonzalez';";
            script += "document.getElementById('pass').value = '123';";
            script += "var nouEvent = document.createEvent('MouseEvents');";
            script += "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            script += "var objecte = document.getElementById('btnLogin');";
            script += "var canceled = !objecte.dispatchEvent(nouEvent);";

            return script;


        }


    }
}
