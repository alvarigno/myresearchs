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
using publicaCA_DM.Modelos;

namespace publicaCA_DM
{
    public partial class frmPrincipal : Form
    {
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

        

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((frmCA.browserCA.URL.ToString() !=
                 "http://desarrollofotos.chileautos.cl/actualizadores/paginas/chileautos/opciones.asp") ||
                (frmDM.browserDM.URL.ToString() != "http://www.demotores.cl/frontend/publicacion.html?execution=e1s2"))
                return;
            string strJavascript;

            Demotores dmPub = new Demotores();
            chileautos caPub = new chileautos();
            //ManualResetEvent waitEvent = new ManualResetEvent(false);


            caPub.patente = frmCA.browserCA.ExecuteJavaScriptAndReturnValue("document.getElementById('patente').value").ToString();
            caPub.nuevo = frmCA.browserCA.ExecuteJavaScriptAndReturnValue("document.getElementById('nuevo').value").ToString();
        
            dmPub.patente = caPub.patente;
                
            frmDM.browserDM.ExecuteJavaScript("document.getElementById('licensePlate').value='" + dmPub.patente + "'");
            dmPub.nuevo = caPub.nuevo != "N";
            frmDM.browserDM.ExecuteJavaScript(dmPub.nuevo == true
                ? "document.getElementById('nuevos').checked=true"
                : "document.getElementById('ocasion').checked=true");

            /* Para obtener el texto de la marca */
            caPub.marca = frmCA.browserCA.ExecuteJavaScriptAndReturnValue("document.getElementById('cod_marca').options[document.getElementById('cod_marca').selectedIndex].text").ToString();

            //selecciono la marca en demotores
            strJavascript = "var marcaEncontrada = false; for (var i = 0; i < document.getElementById('brands').length; i++) {";
            strJavascript += " if (document.getElementById('brands').options[i].text == '" + caPub.marca + "') {";
            //strJavascript += "document.getElementById('brands').selectedIndex = i";
            strJavascript += "  marcaEncontrada = true; document.getElementById('brands').options[i].selected = true; exit for;";
            strJavascript += "}"; //del if
            strJavascript += "}"; // del for
            strJavascript += "if (marcaEncontrada == true) { document.getElementById('msj-brands-error').className = 'correct' }";
            strJavascript += "else { document.getElementById('msj-brands-error').className = 'msj-error' }";
            frmDM.browserDM.ExecuteJavaScript(strJavascript);

            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
            strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            strJavascript += "var objecte = document.getElementById('brands');";
            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            frmDM.browserDM.ExecuteJavaScript(strJavascript);

            //Hago una pausa de 2 segundos para que se carguen los modelos
            Thread.Sleep(2000);
            //waitEvent.WaitOne();


            /* Obtengo el modelo */
            caPub.modelo = frmCA.browserCA.ExecuteJavaScriptAndReturnValue("document.getElementById('modelo').value").ToString();
            //MessageBox.Show("modelo :" + caPub.modelo, "modelo");

            //selecciono el modelo en demotores
            strJavascript = " var modeloEncontrado = false for (var i = 0; i < document.getElementById('models').length; i++) {";
            strJavascript += " if (document.getElementById('models').options[i].text.toUpperCase() == '" + caPub.modelo + "') {";
            strJavascript += "  modeloEncontrado = true; document.getElementById('models').options[i].selected = true;";
            strJavascript += "}"; //del if
            strJavascript += "}"; // del for
            strJavascript += " if (modeloEncontrado ==  true) document.getElementById('msj-models-error').className = 'correct'";
            strJavascript += " else document.getElementById('msj-models-error').className = 'msj-error'";
            frmDM.browserDM.ExecuteJavaScript(strJavascript);

            // Lanzo el evento onchange del modelo en demotores para que se carguen las versiones
            strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            strJavascript += "var objecte = document.getElementById('models');";
            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            frmDM.browserDM.ExecuteJavaScript(strJavascript);
        }
    }
}
