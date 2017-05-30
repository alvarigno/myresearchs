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
using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using DotNetBrowser.WinForms;

namespace publicaCA_DM
{
    public partial class frmDM : Form
    {
        public static Browser browserDM;
       public frmDM()
        {
            InitializeComponent();

            browserDM = BrowserFactory.Create();
            BrowserView browserView = new WinFormsBrowserView(browserDM);
            Controls.Add((Control)browserView);

            ManualResetEvent waitEvent = new ManualResetEvent(false);
            browserDM.FinishLoadingFrameEvent += delegate (object sender, FinishLoadingEventArgs e)
            {
                // Wait until main document of the web page is loaded completely.
                if (e.IsMainFrame)
                {
                    waitEvent.Set();
                }
            };

            browserDM.LoadURL("http://autos.demotores.cl/");
            waitEvent.WaitOne();

            browserDM.ExecuteJavaScript("showLoginModal();");

            browserDM.ExecuteJavaScript("document.getElementById('email').value = 'iduprueba@dm.cl'");
            browserDM.ExecuteJavaScript("document.getElementById('Password').value = 'demotores2017'");

            var strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            strJavascript +=
                "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            strJavascript += "var objecte = document.getElementById('enterAjaxLogin');";
            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            browserDM.ExecuteJavaScript(strJavascript);
            //waitEvent.WaitOne();

            strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            strJavascript +=
                "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            strJavascript += "var objecte = document.getElementById('sellLink');";
            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            browserDM.ExecuteJavaScript(strJavascript);
            //waitEvent.WaitOne();

            strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            strJavascript +=
                "nouEvent.initMouseEvent('click', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            strJavascript += "var objecte = document.getElementById('selectVehicleType');";
            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            browserDM.ExecuteJavaScript(strJavascript);
            







            //browser.ExecuteJavaScript("document.getElementById('moto').checked = true;");
            //

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }
    }
}
