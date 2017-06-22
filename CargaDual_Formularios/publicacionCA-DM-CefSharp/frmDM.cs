using System.Linq;
using System.Windows.Forms;
using AccesoDatos.Datos;
using CefSharp;
using CefSharp.WinForms;


namespace publicacionCA_DM_CefSharp
{
    public partial class frmDM : Form
    {
        public static ChromiumWebBrowser browserDM;
        public static string NomAutomotora;
        public frmDM()
        {
            InitializeComponent();


            // Crear un componente de explorador web chromium bajo o en contexto
            browserDM = new ChromiumWebBrowser("http://autos.demotores.cl") { RequestContext = new RequestContext() };
            // Agregar componente al formulario y cubrir el formulario con él
            this.Controls.Add(browserDM);

            browserDM.Dock = DockStyle.Fill;

            browserDM.LoadingStateChanged += (sender, args) =>
            {
                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {

                    if (frmCA.browserCA.Address != null && frmCA.browserCA.Address == "http://desarrollofotos.chileautos.cl/actualizadores/paginas/chileautos/opciones.asp"
                            && browserDM.Address == "http://autos.demotores.cl/")
                    {
                        baseprod2Entities baseprod = new baseprod2Entities();
                        SP_Datos_Automotora_CADM_Result dm = new SP_Datos_Automotora_CADM_Result();


                        // Obtener nombre de automotora

                        dm = baseprod.SP_Datos_Automotora_CADM(NomAutomotora).FirstOrDefault();
                        if (dm == null) return;
                        if (!browserDM.CanExecuteJavascriptInMainFrame) return;
                        browserDM.ExecuteScriptAsync("document.getElementById('email').value = '" + dm.usr_DM + "';");
                        browserDM.ExecuteScriptAsync("document.getElementById('Password').value = '" + dm.pass_DM + "';");

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
                    }
                }
            };


            if (browserDM.Address.ToString() == "http://www.demotores.cl/frontend/publicacion.html?execution=e1s2") { 

                //Wait for the page to finish loading (all resources will have been loaded, rendering is likely still happening)
                browserDM.LoadingStateChanged += (sender, args) =>
                {
                    //Wait for the Page to finish loading
                    if (args.IsLoading == false)
                    {
                        browserDM.ExecuteScriptAsync(@"alert('All Resources Have Loaded');
                                                    document.getElementById('_eventId_continue').removeAttribute('disabled');");
                    }
                };

            }
        }

        private void frmDM_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Controls.Clear();
            browserDM.RequestContext.Dispose();
            browserDM.Dispose();
            browserDM = null;
        }

        private void frmDM_Load(object sender, System.EventArgs e)
        {

        }
    }
}
