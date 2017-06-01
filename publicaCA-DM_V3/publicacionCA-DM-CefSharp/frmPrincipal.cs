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
using publicacionCA_DM_CefSharp.Modelos;

namespace publicacionCA_DM_CefSharp
{
    public partial class frmPrincipal : Form
    {
        public Demotores dmPub = new Demotores();
        public chileautos caPub = new chileautos();
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
                Left = (ResolucionPantalla.Width/2)

            };
            frm.Show();
        }

        private void chileautosToolStripMenuItem_Click(object sender, EventArgs e)
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
        }

        private void demotoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size ResolucionPantalla = System.Windows.Forms.SystemInformation.PrimaryMonitorMaximizedWindowSize;
            string mensaje = "alto  " + ResolucionPantalla.Height + " ancho " + ResolucionPantalla.Width;
            int altoForm = ResolucionPantalla.Height - 50;
            int anchoForm = (ResolucionPantalla.Width/2) - 50;

            frmDM frm = new frmDM
            {
                MdiParent = this,
                Height = altoForm,
                Width = anchoForm,
                Top = 1,
                Left = (ResolucionPantalla.Width/2)

            };
            frm.Show();
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strJavascript, script;

            

            if ((frmCA.browserCA.Address.ToString() !=
                 "http://desarrollofotos.chileautos.cl/actualizadores/paginas/chileautos/opciones.asp") ||
                (frmDM.browserDM.Address.ToString() !=
                 "http://www.demotores.cl/frontend/publicacion.html?execution=e1s2"))
                return;

            /*  *************************    Obtener la patente y copiarla          *************************************
             
             */
            script = "(function() {return document.getElementById('patente').value;})();";
            var task = frmCA.browserCA.EvaluateScriptAsync(script);
            task.ContinueWith(tskPatente =>
            {
                var response = tskPatente.Result;
                if (!response.Success || response.Result == null) return;
                caPub.patente = response.Result.ToString();
                frmDM.browserDM.ExecuteScriptAsync("document.getElementById('licensePlate').value='" + caPub.patente + "';");
                dmPub.patente = caPub.patente;
            });
            if (task.IsCompleted)
                task.Dispose();

            /*  *************************    Obtener si auto es nuevo o usado          *************************************
             
             */
            script = "(function() {return document.getElementById('nuevo').value;})();";
            var taskEstado = frmCA.browserCA.EvaluateScriptAsync(script);
            taskEstado.ContinueWith(tskEstado =>
            {
                var response = tskEstado.Result;
                if (!response.Success || response.Result == null) return;
                caPub.nuevo = response.Result.ToString();
                dmPub.nuevo = caPub.nuevo != "N";
                frmDM.browserDM.ExecuteScriptAsync(dmPub.nuevo == true
                    ? "document.getElementById('nuevos').checked=true;"
                    : "document.getElementById('ocasion').checked=true;");
            });
            if (taskEstado.IsCompleted)
                taskEstado.Dispose();


            /* **************************      Para obtener el texto de la marca    *********************************
             */

            script = "(function() {return document.getElementById('cod_marca').options[document.getElementById('cod_marca').selectedIndex].text;})();";
            var taskMarca = frmCA.browserCA.EvaluateScriptAsync(script);
            taskMarca.Wait();
            var response1 = taskMarca.Result;
            if (!response1.Success || response1.Result == null) return;
            if (caPub.marca != response1.Result.ToString())
            {

                caPub.marca = response1.Result.ToString();
                //selecciono la marca en demotores
                strJavascript = "var marcaEncontrada = false; for (var i = 0; i < document.getElementById('brands').length; i++) {";
                strJavascript += " if (document.getElementById('brands').options[i].text == '" + caPub.marca + "') {";
                strJavascript += "document.getElementById('brands').selectedIndex = i;";
                strJavascript += "  marcaEncontrada = true; document.getElementById('brands').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += "if (marcaEncontrada == true) { document.getElementById('msj-brands-error').className = 'correct'; }";
                strJavascript += "else { document.getElementById('msj-brands-error').className = 'msj-error'; }";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);

                // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                strJavascript = "var nouEvent = document.createEvent('HTMLEvents');";
                strJavascript += "nouEvent.initEvent('change', false, true);";
                strJavascript += "var objecte = document.getElementById('brands');";
                strJavascript += "objecte.dispatchEvent(nouEvent);";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                //Hago una pausa de 2 segundos para que se carguen los modelos
                Thread.Sleep(3000);

            }
            //taskMarca.ContinueWith(t =>
            //{
            //    var response = t.Result;
            //    if (!response.Success || response.Result == null) return;
            //    if (caPub.marca != response.Result.ToString()) { 
            //        caPub.marca = response.Result.ToString();
            //        //selecciono la marca en demotores
            //        strJavascript = "var marcaEncontrada = false; for (var i = 0; i < document.getElementById('brands').length; i++) {";
            //        strJavascript += " if (document.getElementById('brands').options[i].text == '" + caPub.marca + "') {";
            //        strJavascript += "document.getElementById('brands').selectedIndex = i;";
            //        strJavascript += "  marcaEncontrada = true; document.getElementById('brands').options[i].selected = true; break;";
            //        strJavascript += "}"; //del if
            //        strJavascript += "}"; // del for
            //        strJavascript += "if (marcaEncontrada == true) { document.getElementById('msj-brands-error').className = 'correct' }";
            //        strJavascript += "else { document.getElementById('msj-brands-error').className = 'msj-error' }";
            //        frmDM.browserDM.ExecuteScriptAsync(strJavascript);

            //        // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
            //        strJavascript = "var nouEvent = document.createEvent('HTMLEvents');";
            //        strJavascript += "nouEvent.initEvent('change', false, true);";
            //        strJavascript += "var objecte = document.getElementById('brands');";
            //        strJavascript += "objecte.dispatchEvent(nouEvent);";
            //        frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            //        //Hago una pausa de 2 segundos para que se carguen los modelos
            //        Thread.Sleep(2000);

            //    }
            //});

            if (taskMarca.IsCompleted)
                taskMarca.Dispose();



            /* **************************************   Obtener el modelo      *************************************************
             */
            script = "(function() {return document.getElementById('modelo').value;})();";
            var taskModelo = frmCA.browserCA.EvaluateScriptAsync(script);
            taskModelo.Wait();
            var responseModelo = taskModelo.Result;
            if (!responseModelo.Success || responseModelo.Result == null) return;
                

            if (caPub.modelo != responseModelo.Result.ToString())
            {
                caPub.modelo = responseModelo.Result.ToString();
                //selecciono el modelo en demotores
                strJavascript = " var modeloEncontrado = false; for (var i = 0; i < document.getElementById('models').length; i++) {";
                strJavascript += " if (document.getElementById('models').options[i].text.toUpperCase() == '" + caPub.modelo + "') {";
                strJavascript += "  modeloEncontrado = true; document.getElementById('models').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += " if (modeloEncontrado ==  true) { document.getElementById('msj-models-error').className = 'correct';}";
                strJavascript += " else { document.getElementById('msj-models-error').className = 'msj-error';}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);

                // Lanzo el evento onchange del modelo en demotores para que se carguen las versiones
                strJavascript = "var nouEvent = document.createEvent('HTMLEvents');";
                strJavascript += "nouEvent.initEvent('change', false, true);";
                strJavascript += "var objecte = document.getElementById('models');";
                strJavascript += "objecte.dispatchEvent(nouEvent);";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }



            if (taskModelo.IsCompleted)
                taskModelo.Dispose();


        }
    }
}
