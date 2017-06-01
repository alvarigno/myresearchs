using CefSharp;
using CefSharp.WinForms;
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

            if (urlbr1 == "http://desarrollofotos.chileautos.cl/actualizadores/paginas/chileautos/opciones.asp" && urlbr2 == "http://www.demotores.cl/frontend/publicacion.html?execution=e1s2")
             {

                string strPatente = "";
                string strMarca = "";
                string strModelo = "";
                string script = "(function() {return document.getElementById('patente').value;})();";
                string marca = "(function(){ var e = document.getElementById('cod_marca');return e.options[e.selectedIndex].text;})();";
                string modelo = "(function(){ var e = document.getElementById('modelo');return e.options[e.selectedIndex].text;})();";
                string version = "(function(){ var e = document.getElementById('version');return e.options[e.selectedIndex].text;})();";


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
                    task.Dispose();  
                });

                var taskmarca = frmCA.chromeBrowser.EvaluateScriptAsync(marca);
                taskmarca.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strMarca = response.Result.ToString();
                            string strscriptmarca = "var textToFind = '" + strMarca + "';var dd = document.getElementById('brands');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                            //MessageBox.Show(strscriptmarca);
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('brands').onchange();", TimeSpan.FromSeconds(1));


                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript += "var objecte = document.getElementById('brands');";
                            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                        }
                    }
                    taskmarca.Dispose();
                });

                var taskmodelo = frmCA.chromeBrowser.EvaluateScriptAsync(modelo);
                taskmodelo.ContinueWith(u =>
                {
                    if (!u.IsFaulted)
                    {
                        var response2 = u.Result;
                        if (response2.Success && response2.Result != null)
                        {

                            strModelo = response2.Result.ToString();
                            strModelo = strModelo.ToLower();
                            strModelo = strModelo.First().ToString().ToUpper()+ strModelo.Substring(1);
                            //selecciono el modelo en demotores
                            //MessageBox.Show("cargando Modelos");
                            int milliseconds = 2000;
                            Thread.Sleep(milliseconds);
                            string strJavascript2 = "var x = document.getElementById('models').options.length;function cargamodelo(){ var modeloEncontrado = false; var datomodelo = '" + strModelo + "'; var textToFind2 = datomodelo;var dd = document.getElementById('models'); for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind2){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}}setTimeout('cargamodelo()',1000);";
                            
                                //strJavascript2 +="for (var i = 0; i < document.getElementById('models').length; i++){ ";
                                //strJavascript2 += " if(document.getElementById('models').options[i].text == datomodelo) {";
                                //strJavascript2 += "  modeloEncontrado = true; document.getElementById('models').options[i].selected = true;";
                                //strJavascript2 += "}"; //del if
                                //strJavascript2 += "}"; // del for
                                //strJavascript2 += " if (modeloEncontrado ==  true){ document.getElementById('msj-models-error').className = 'correct';}";
                                //strJavascript2 += " else{ document.getElementById('msj-models-error').className = 'msj-error';}";
                                frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript2);
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('models').onchange();", TimeSpan.FromSeconds(1));

                            // Lanzo el evento onchange de la marca en demotores para que se carguen las versiones
                            //string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            //strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            //strJavascript += "var objecte = document.getElementById('models');";
                            //strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            //frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                            //strModelo = response.Result.ToString();
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("var textToFind = '" + strModelo + "';var dd = document.getElementById('models');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}");
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('models').onchange();", TimeSpan.FromSeconds(1));


                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript3 = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript3 += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript3 += "var objecte = document.getElementById('models');";
                            strJavascript3 += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript3);



                        }
                       // MessageBox.Show(strModelo + "-" + urlbr1 + "-" + urlbr2);
                    }
                    taskmodelo.Dispose();
                });


                var taskversion = frmCA.chromeBrowser.EvaluateScriptAsync(version);
                taskversion.ContinueWith(v =>
                {
                    if (!v.IsFaulted)
                    {
                        var response3 = v.Result;
                        if (response3.Success && response3.Result != null)
                        {

                            string strVersion = response3.Result.ToString();
                            strVersion = strVersion.ToLower();
                            strVersion = strVersion.First().ToString().ToUpper() + strVersion.Substring(1);
                            //selecciono el modelo en demotores
                            //MessageBox.Show("cargando Modelos");
                            int milliseconds = 4000;
                            Thread.Sleep(milliseconds);
                            string strJavascript2 = "var x = document.getElementById('versions').options.length;alert('version'+x);<function cargamodelo(){ var modeloEncontrado = false; var datomodelo = '" + strVersion + "'; var textToFind2 = datomodelo;var dd = document.getElementById('versions'); for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind2){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}}setTimeout('cargamodelo()',1000);";

                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript2);

                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript3 = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript3 += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript3 += "var objecte = document.getElementById('versions');";
                            strJavascript3 += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript3);



                        }
                        // MessageBox.Show(strModelo + "-" + urlbr1 + "-" + urlbr2);
                    }
                    taskmodelo.Dispose();
                });


            }

            
        }

        private object EvaluateScript(object p, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
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
