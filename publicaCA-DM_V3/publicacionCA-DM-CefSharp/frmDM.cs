using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace publicacionCA_DM_CefSharp
{
    public partial class frmDM : Form
    {
        public static ChromiumWebBrowser browserDM;
        public frmDM()
        {
            InitializeComponent();
            CefSettings settings = new CefSettings();
            // Inicializar CEF con la configuración proporcionada
            if (!Cef.IsInitialized)
                Cef.Initialize(settings);
            // Crear un componente de explorador web chromium
            browserDM = new ChromiumWebBrowser("http://autos.demotores.cl");
            // Agregar componente al formulario y cubrir el formulario con él
            this.Controls.Add(browserDM);
            browserDM.Dock = DockStyle.Fill;

            browserDM.LoadingStateChanged += (sender, args) =>
            {
                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {
                    if (!browserDM.CanExecuteJavascriptInMainFrame) return;
                    browserDM.ExecuteScriptAsync("document.getElementById('email').value = 'iduprueba@dm.cl';");
                    browserDM.ExecuteScriptAsync("document.getElementById('Password').value = 'demotores2017';");
                    string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                    strJavascript +=
                        "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                    strJavascript += "var objecte = document.getElementById('enterAjaxLogin');";
                    strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                    browserDM.ExecuteScriptAsync(strJavascript);

                    strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                    strJavascript +=
                        "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                    strJavascript += "var objecte = document.getElementById('sellLink');";
                    strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                    browserDM.ExecuteScriptAsync(strJavascript);

                    strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                    strJavascript +=
                        "nouEvent.initMouseEvent('click', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                    strJavascript += "var objecte = document.getElementById('selectVehicleType');";
                    strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                    browserDM.ExecuteScriptAsync(strJavascript);
                }
            };
        }

        private void frmDM_Load(object sender, EventArgs e)
        {

        }

        private void frmDM_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void frmDM_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmDM.browserDM = null;
        }
    }
}
