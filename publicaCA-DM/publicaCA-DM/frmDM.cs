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
    public partial class frmDM : Form
    {
        public static ChromiumWebBrowser browserDM;
        public frmDM()
        {
            InitializeComponent();
            InitializeChromium();

            //browserDM = BrowserFactory.Create();
            //BrowserView browserView = new WinFormsBrowserView(browserDM);
            //Controls.Add((Control)browserView);
            //
            //ManualResetEvent waitEvent = new ManualResetEvent(false);
            //browserDM.FinishLoadingFrameEvent += delegate (object sender, FinishLoadingEventArgs e)
            //{
            //    // Wait until main document of the web page is loaded completely.
            //    if (e.IsMainFrame)
            //    {
            //        waitEvent.Set();
            //    }
            //};
            //
            //browserDM.LoadURL("http://autos.demotores.cl/");
            //waitEvent.WaitOne();
            //
            //browserDM.ExecuteJavaScript("showLoginModal();");
            //
            //browserDM.ExecuteJavaScript("document.getElementById('email').value = 'iduprueba@dm.cl'");
            //browserDM.ExecuteJavaScript("document.getElementById('Password').value = 'demotores2017'");
            //
            //var strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            //strJavascript +="nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            //strJavascript += "var objecte = document.getElementById('enterAjaxLogin');";
            //strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            //browserDM.ExecuteJavaScript(strJavascript);
            ////waitEvent.WaitOne();
            //
            //strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            //strJavascript +=
            //    "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            //strJavascript += "var objecte = document.getElementById('sellLink');";
            //strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            //browserDM.ExecuteJavaScript(strJavascript);
            ////waitEvent.WaitOne();
            //
            //strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            //strJavascript +=
            //    "nouEvent.initMouseEvent('click', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            //strJavascript += "var objecte = document.getElementById('selectVehicleType');";
            //strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            //browserDM.ExecuteJavaScript(strJavascript);
            //
            //
            //
            //
            //
            //
            //
            //
            ////browser.ExecuteJavaScript("document.getElementById('moto').checked = true;");
            ////

        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        public ChromiumWebBrowser chromeBrowser2;


        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
            ///////Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser2 = new ChromiumWebBrowser("http://autos.demotores.cl/");
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser2);
            chromeBrowser2.Dock = DockStyle.Fill;
            chromeBrowser2.FrameLoadEnd += (sender, args) =>
            {

                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain) 
                {

                   // args.Frame.ExecuteJavaScriptAsync(login());

                    var response = EvaluateScript(login(), TimeSpan.FromSeconds(1));

                    args.Frame.ExecuteJavaScriptAsync(segundo());
                    args.Frame.ExecuteJavaScriptAsync(tercero());

                }


            };

        }


        public object EvaluateScript(string script, TimeSpan timeout)
        {
            var browser = (ChromiumWebBrowser)chromeBrowser2;
            object result = null;

            if (browser.IsBrowserInitialized && !browser.IsDisposed && !browser.Disposing)
            {
                var task = browser.EvaluateScriptAsync(script, timeout);
                var complete = task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        //var response = t.Result;
                        //result = response.Success ? (response.Result ?? "null") : response.Message;
                        result = true;
                    }
                }, TaskScheduler.Default);
                complete.Wait();

            }
            return result;
        }

        //browserDM.ExecuteJavaScript("showLoginModal();");
        //
        //browserDM.ExecuteJavaScript("document.getElementById('email').value = 'iduprueba@dm.cl'");
        //browserDM.ExecuteJavaScript("document.getElementById('Password').value = 'demotores2017'");
        //
        //var strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
        //strJavascript +="nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
        //strJavascript += "var objecte = document.getElementById('enterAjaxLogin');";
        //strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
        //browserDM.ExecuteJavaScript(strJavascript);
        ////waitEvent.WaitOne();
        //

        //


        public static string login()
        {

            string script = "showLoginModal();";
           script += "document.getElementById('email').value = 'iduprueba@dm.cl';";
           script += "document.getElementById('Password').value = 'demotores2017';";
            script += "var nouEvent = document.createEvent('MouseEvents');";
            script += "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
           script += "var objecte = document.getElementById('enterAjaxLogin');";
           script += "var canceled = !objecte.dispatchEvent(nouEvent);";

            return script;


        }
       //
       //
        public static string segundo(){

            //strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            //strJavascript +=
            //    "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            //strJavascript += "var objecte = document.getElementById('sellLink');";
            //strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            //browserDM.ExecuteJavaScript(strJavascript);
            ////waitEvent.WaitOne();

            string script = "var nouEvent = document.createEvent('MouseEvents');";
             script += "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
             script += "var objecte = document.getElementById('sellLink');";
             script += "var canceled = !objecte.dispatchEvent(nouEvent);";

            //string script = "document.getElementById('email').value = 'iduprueba@dm.cl'";
            //script += "document.getElementById('Password').value = 'demotores2017'";

            return script;
        }
       
        public static string tercero() {

            //strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            //strJavascript +=
            //    "nouEvent.initMouseEvent('click', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            //strJavascript += "var objecte = document.getElementById('selectVehicleType');";
            //strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            //browserDM.ExecuteJavaScript(strJavascript);

            string script = "var nouEvent = document.createEvent('MouseEvents');";
            script += "nouEvent.initMouseEvent('click', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            script += "var objecte = document.getElementById('selectVehicleType');";
            script += "var canceled = !objecte.dispatchEvent(nouEvent);";

            return script;
        }

    }
}
