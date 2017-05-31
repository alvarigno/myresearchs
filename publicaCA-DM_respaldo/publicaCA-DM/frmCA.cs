using DotNetBrowser;
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
using DotNetBrowser.Events;
using DotNetBrowser.WinForms;

namespace publicaCA_DM
{
    public partial class frmCA : Form
    {
        public static Browser browserCA;
        public frmCA()
        {
            InitializeComponent();

            browserCA = BrowserFactory.Create();
            BrowserView browserView = new WinFormsBrowserView(browserCA);
            Controls.Add((Control)browserView);

           
            ManualResetEvent waitEvent = new ManualResetEvent(false);
            browserCA.FinishLoadingFrameEvent += delegate (object sender, FinishLoadingEventArgs e)
            {
                // Wait until main document of the web page is loaded completely.
                if (e.IsMainFrame)
                {
                    waitEvent.Set();
                }
            };

            browserCA.LoadURL("http://desarrollofotos.chileautos.cl/actualizadores/login.asp");
            waitEvent.WaitOne();

                
            browserCA.ExecuteJavaScript("document.getElementById('user').value = 'cgonzalez';");
            browserCA.ExecuteJavaScript("document.getElementById('pass').value = '123';");

            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            strJavascript += "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            strJavascript += "var objecte = document.getElementById('btnLogin');";
            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";


            browserCA.ExecuteJavaScript(strJavascript);

        }

        
    }
}
