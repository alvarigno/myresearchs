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
using publicacionCA_DM_CefSharp.Datos;

namespace publicacionCA_DM_CefSharp
{
    public partial class frmDM : Form
    {
        public static ChromiumWebBrowser browserDM;
        AutomotorasDic MapAuto = new AutomotorasDic();
        public static string nombreautomotora = "";

        public void SetAutoMotora(string usr)
        {

            nombreautomotora = usr;
        }


        public frmDM()
        {
            InitializeComponent();
            CefSettings settings = new CefSettings();

            // On Win7 this will create a directory in AppData.
            var cache = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.IO.Path.Combine("publicacionCA-DM-CefSharp", "cache"));
            if (!System.IO.Directory.Exists(cache))
                System.IO.Directory.CreateDirectory(cache);

            // Set the CachePath during initialization.
            settings = new CefSettings() { CachePath = cache };
            //Cef.Initialize(settings);

            // Inicializar CEF con la configuración proporcionada
            if (!Cef.IsInitialized)
                Cef.Initialize(settings);
            // Crear un componente de explorador web chromium
            browserDM = new ChromiumWebBrowser("http://autos.demotores.cl");
            // Agregar componente al formulario y cubrir el formulario con él
            this.Controls.Add(browserDM);
            browserDM.Dock = DockStyle.Fill;

            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            browserDM.BrowserSettings = browserSettings;
            int count = 0;//contador

            browserDM.LoadingStateChanged += (sender, args) =>
            {
                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {
                    /// algoritmo que verifica las automotoras existen en CA y DM////
                    MapAuto.MapAutomotoras();
                    string prova = MapAuto.EncuentraAutomotora(nombreautomotora);
                    string[] DAccess = AccesoDatos.Form1.GetDataAccess(prova);
                    MapAuto.resetdiccionary();
                    string us = DAccess[0].ToString();
                    string pass = DAccess[1].ToString();
                    /// Fin algoritmo que verifica las automotoras existen en CA y DM////

                    if (!browserDM.CanExecuteJavascriptInMainFrame) return;
                    browserDM.ExecuteScriptAsync("document.getElementById('email').value = '"+us+"';");
                    browserDM.ExecuteScriptAsync("document.getElementById('Password').value = '"+pass+"';");
                    string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                    strJavascript += "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                    strJavascript += "var objecte = document.getElementById('enterAjaxLogin');";
                    strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                    browserDM.ExecuteScriptAsync(strJavascript);

                    strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                    strJavascript += "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                    strJavascript += "var objecte = document.getElementById('sellLink');";
                    strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                    browserDM.ExecuteScriptAsync(strJavascript);

                    /// verifica si hay datos de login, sino logout
                    if (prova == "" && count == 0)
                    {

                        browserDM.Load("http://www.demotores.cl/frontend/logout");

                    }
                    count = count + 1;
                    /// fin verifica si hay datos de login, sino logout

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
