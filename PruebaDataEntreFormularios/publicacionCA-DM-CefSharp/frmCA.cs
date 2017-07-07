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
    public partial class frmCA : Form
    {
        public static ChromiumWebBrowser browserCA;
        public frmCA()
        {
            InitializeComponent();

            // Crear un componente de explorador web chromium
            browserCA = new ChromiumWebBrowser("http://desarrollofotos.chileautos.cl/actualizadores/login.asp");
            // Agregar componente al formulario y cubrir el formulario con él
            this.Controls.Add(browserCA);
            browserCA.Dock = DockStyle.Fill;



            browserCA.LoadingStateChanged += (sender, args) =>
            {

                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {
                    if (!browserCA.CanExecuteJavascriptInMainFrame) return;
                    if (browserCA.Address == "http://desarrollofotos.chileautos.cl/actualizadores/home.asp")
                    {
                        browserCA.ExecuteScriptAsync(@"$('#contenido-user>table a').click(function(){
                                location.href = $(this).attr('href'); 
                                return false;
                                });");
                    }
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

        private void frmCA_Load(object sender, EventArgs e)
        {

        }
    }
}
