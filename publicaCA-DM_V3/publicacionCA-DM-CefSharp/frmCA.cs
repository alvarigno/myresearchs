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
using CefSharp.WinForms.Internals;

using System.Runtime.InteropServices;

namespace publicacionCA_DM_CefSharp
{
    public partial class frmCA : Form
    {
        public static ChromiumWebBrowser browserCA;

        public frmCA()
        {
            InitializeComponent();

            
            CefSettings settings = new CefSettings();
            // Inicializar CEF con la configuración proporcionada
            if (!Cef.IsInitialized)
                Cef.Initialize(settings);
            // Crear un componente de explorador web chromium
            browserCA = new ChromiumWebBrowser("http://desarrollofotos.chileautos.cl/actualizadores/login.asp");
            // Agregar componente al formulario y cubrir el formulario con él
            this.Controls.Add(browserCA);
            browserCA.Dock = DockStyle.Fill;

            //if (browserCA.IsBrowserInitialized)
            //    browserCA.ShowDevTools();

            browserCA.LoadingStateChanged += (sender, args) =>
            {

                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {
                    if (!browserCA.CanExecuteJavascriptInMainFrame) return;
                    browserCA.ExecuteScriptAsync("document.getElementById('user').value = 'cgonzalez';");
                    browserCA.ExecuteScriptAsync("document.getElementById('pass').value = '123';");
                    string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                    strJavascript +=
                        "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                    strJavascript += "var objecte = document.getElementById('btnLogin');";
                    strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                    browserCA.ExecuteScriptAsync(strJavascript);
                }
               

            };


        }

        private void frmCA_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Cef.Shutdown();
        }

        private void frmCA_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmCA.browserCA = null;
        }

    }
  }
 