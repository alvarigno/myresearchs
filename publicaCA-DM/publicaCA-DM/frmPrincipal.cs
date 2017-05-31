using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace publicaCA_DM
{
    public partial class frmPrincipal : Form
    {

        public static string urlbr1 = "";
        public static string urlbr2 = "";

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Size ResolucionPantalla = System.Windows.Forms.SystemInformation.PrimaryMonitorMaximizedWindowSize;
            string mensaje = "alto  " + ResolucionPantalla.Height + " ancho " + ResolucionPantalla.Width;
            int altoForm = ResolucionPantalla.Height - 50;
            int anchoForm = (ResolucionPantalla.Width/2) - 50;

            frmCA formCA = new frmCA
            {
                MdiParent = this,
                Height = altoForm,
                Width = anchoForm
            };
            formCA.Show();

            //MessageBox.Show(mensaje, "Resolución pantalla");
            frmDM frm = new frmDM
            {
                MdiParent = this,
                Height = altoForm,
                Width = anchoForm,
                Top = 1,
                Left = (ResolucionPantalla.Width / 2) 
                
            };
            frm.Show();

            
            //MessageBox(frmPrincipal.)
        }

        private void chileautosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size ResolucionPantalla = System.Windows.Forms.SystemInformation.PrimaryMonitorMaximizedWindowSize;
            string mensaje = "alto  " + ResolucionPantalla.Height + " ancho " + ResolucionPantalla.Width;
            int altoForm = ResolucionPantalla.Height - 50;
            int anchoForm = (ResolucionPantalla.Width / 2) - 50;

            frmCA formCA = new frmCA
            {
                MdiParent = this,
                Height = altoForm,
                Width = anchoForm
            };
            formCA.Show();
        }

        private void demotoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size ResolucionPantalla = System.Windows.Forms.SystemInformation.PrimaryMonitorMaximizedWindowSize;
            string mensaje = "alto  " + ResolucionPantalla.Height + " ancho " + ResolucionPantalla.Width;
            int altoForm = ResolucionPantalla.Height - 50;
            int anchoForm = (ResolucionPantalla.Width / 2) - 50;

            frmDM frm = new frmDM
            {
                MdiParent = this,
                Height = altoForm,
                Width = anchoForm,
                Top = 1,
                Left = (ResolucionPantalla.Width / 2)

            };
            frm.Show();
        }

        private void patenteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmCA.chromeBrowser.AddressChanged += ChromeBrowser_AddressChanged;
            frmDM.chromeBrowser2.AddressChanged += ChromeBrowser2_AddressChanged;

            urlbr1 = frmCA.chromeBrowser.Address.ToString();
            urlbr2 = frmDM.chromeBrowser2.Address.ToString();

            //string strPatente = "";

            //string script = "(function() {return document.getElementById('patente').value;})();";
            //string returnValue = "";
            //
            //var task = frmCA.chromeBrowser.EvaluateScriptAsync(script);
            //task.ContinueWith(t =>
            //{
            //    if (!t.IsFaulted)
            //    {
            //        var response = t.Result;
            //
            //        if (response.Success && response.Result != null)
            //        {
            //            strPatente = response.Result.ToString();
            //        }
            //    }
            //});


            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('licensePlate').value='" + strPatente + "'");


            if (urlbr1 == "http://desarrollofotos.chileautos.cl/actualizadores/paginas/chileautos/opciones.asp" && urlbr2 == "http://www.demotores.cl/frontend/publicacion.html?execution=e1s2")
             {


                string strPatente = "";
                string script = "(function() {return document.getElementById('patente').value;})();";
                
                var task = frmCA.chromeBrowser.EvaluateScriptAsync(script);
                task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strPatente = response.Result.ToString();
                            frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('licensePlate').value='" + strPatente + "'");
                        }
                    }
                });
                

            }

            //MessageBox.Show(urlbr1+"-"+urlbr2);
        }



        //Obtienen las ULR de cada objeto browser.
        private static void ChromeBrowser2_AddressChanged(object sender, AddressChangedEventArgs b)
        {
            //urlbr2 = frmDM.chromeBrowser2.Address.ToString();
        }

        private static void ChromeBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            urlbr1 = frmCA.chromeBrowser.ToString();

        }

    }
}
