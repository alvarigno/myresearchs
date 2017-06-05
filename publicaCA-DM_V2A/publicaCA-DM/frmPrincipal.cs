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
        public static Dictionary<string, string> DicCarrocerias = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCarroceriasMotos = new Dictionary<string, string>();

        public frmPrincipal()
        {
            InitializeComponent();

            //Diccionario DicCarrocerias autos
            DicCarrocerias.Add("Camioneta", "Camioneta");
            DicCarrocerias.Add("Convertible", "Convertible");
            DicCarrocerias.Add("Coupé", "Coupé");
            DicCarrocerias.Add("Familiar", "StationWagon");
            DicCarrocerias.Add("Hatchback", "Hatchback");
            DicCarrocerias.Add("Monovolumen", "Monovolumen");
            DicCarrocerias.Add("Pick Up", "Pick");
            DicCarrocerias.Add("Sedan", "Sedán");
            DicCarrocerias.Add("Utilitario", "Utilitario");
            DicCarrocerias.Add("Micro Car", "Hatchback");

            //Diccionario DicCarrocerias Motos
            DicCarroceriasMotos.Add("Chopper", "Custom y Choppers");
            DicCarroceriasMotos.Add("Cuadrimoto", "Cuatriciclos y Triciclos");
            DicCarroceriasMotos.Add("Custom", "Custom y Choppers");
            DicCarroceriasMotos.Add("Deportivas", "Deportivas");
            DicCarroceriasMotos.Add("Enduro - cross", "Cross y Enduro");
            DicCarroceriasMotos.Add("Moto de nieve", "Motos de Nieve");
            DicCarroceriasMotos.Add("Racing", "Touring y Trails");
            DicCarroceriasMotos.Add("Retro", "Custom y Choppers");
            DicCarroceriasMotos.Add("Scooter", "Scooters y Ciclomotores");
            DicCarroceriasMotos.Add("Sport calle - urbanas", "");
            DicCarroceriasMotos.Add("Todo terrenos", "Touring y Trails");
            DicCarroceriasMotos.Add("Trabajo - calle", "Calle y Naked");

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
                string strAno = "";
                string strCombustible = "";
                string strTransmision = "";
                string strtipo_direccion = "";
                string strpuertas = "";
                string strdistancia = "";
                string strpesos = "";
                string strcarroceria = "";
                string strotros = "";
                string striconoprecio = "";
                string strcilindrada = "";
                string strcolor = "";
                string script = "(function() {return document.getElementById('patente').value;})();";
                string marca = "(function(){ var e = document.getElementById('cod_marca');return e.options[e.selectedIndex].text;})();";
                string modelo = "(function(){ var e = document.getElementById('modelo');return e.options[e.selectedIndex].text;})();";
                string version = "(function(){ var e = document.getElementById('version');return e.options[e.selectedIndex].text;})();";
                string ano = "(function(){ var e = document.getElementById('anoIng');return e.options[e.selectedIndex].text;})();";
                string combustible = "(function(){ var e = document.getElementById('combustible');return e.options[e.selectedIndex].text;})();";
                string transmision = "(function(){ var e = document.getElementById('transmisionwl');return e.options[e.selectedIndex].text;})();";
                string tipo_direccion = "(function(){ var e = document.getElementById('tipo_direccion');return e.options[e.selectedIndex].text;})();";
                string puertas = "(function(){ var e = document.getElementById('puertas');return e.options[e.selectedIndex].text;})();";
                string distancia = "(function(){ return document.getElementById('distancia').value;})();";
                string pesos = "(function(){ return document.getElementById('pesos').value;})();";
                string carroceria = "(function(){ var e = document.getElementById('carroceria');return e.options[e.selectedIndex].text;})();";
                string otros = "(function(){ return document.getElementById('txtOtros').value;})();";
                string iconoprecio = "(function(){ var e = document.getElementById('tipomoneda');return e.options[e.selectedIndex].text;})();";
                string cilindrada = "(function(){ var e = document.getElementById('cilindrada');return e.options[e.selectedIndex].text;})();";
                string color = "(function(){ return document.getElementById('color').value;})();";

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

                //carga de años
                var taskano = frmCA.chromeBrowser.EvaluateScriptAsync(ano);
                taskano.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strAno = response.Result.ToString();
                            string strscriptmarca = "var textToFind = '" + strAno + "';var dd = document.getElementById('years');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                            //MessageBox.Show(strscriptmarca);
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('brands').onchange();", TimeSpan.FromSeconds(1));


                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript += "var objecte = document.getElementById('versions');";
                            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                        }
                    }
                    taskano.Dispose();
                });

                ////fuels
                //carga de combustible
                var taskcombustible = frmCA.chromeBrowser.EvaluateScriptAsync(combustible);
                taskcombustible.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strCombustible = response.Result.ToString();
                            if (strCombustible == "Diesel (Petroleo)") { strCombustible = "Diesel"; }
                            if (strCombustible == "Gas") { strCombustible = "Gas / Bencina"; }
                            if (strCombustible == "Híbrido") { strCombustible = "Eléctrico / Híbrido"; }
                            if (strCombustible == "Eléctrico") { strCombustible = "Eléctrico / Híbrido"; }
                            if (strCombustible == "Otro") { strCombustible = "Otros"; }
                            string strscriptmarca = "var textToFind = '" + strCombustible + "';var dd = document.getElementById('fuels');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                            //MessageBox.Show(strscriptmarca);
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('brands').onchange();", TimeSpan.FromSeconds(1));


                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript += "var objecte = document.getElementById('versions');";
                            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                        }
                    }
                    taskcombustible.Dispose();
                });

                //carga de transmissions
                var tasktransmision = frmCA.chromeBrowser.EvaluateScriptAsync(transmision);
                tasktransmision.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strTransmision = response.Result.ToString();
                            if (strTransmision == "Automatica") { strTransmision = "Automática";  }
                            string strscriptmarca = "var textToFind = '" + strTransmision + "';var dd = document.getElementById('transmissions');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                            //MessageBox.Show(strscriptmarca);
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('brands').onchange();", TimeSpan.FromSeconds(1));


                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript += "var objecte = document.getElementById('versions');";
                            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                        }
                    }
                    tasktransmision.Dispose();
                });

                //carga de tipo dirección
                var tasktipo_direccion = frmCA.chromeBrowser.EvaluateScriptAsync(tipo_direccion);
                tasktipo_direccion.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strtipo_direccion = response.Result.ToString();
                            if (strtipo_direccion == "Hidraulica") { strtipo_direccion = "Hidráulica"; }
                            if (strtipo_direccion == "Mecanica") { strtipo_direccion = "Mecánica"; }
                            string strscriptmarca = "var textToFind = '" + strtipo_direccion + "';var dd = document.getElementById('steerings');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                            //MessageBox.Show(strscriptmarca);
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('brands').onchange();", TimeSpan.FromSeconds(1));


                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript += "var objecte = document.getElementById('versions');";
                            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                        }
                    }
                    tasktipo_direccion.Dispose();
                });

                // carga doors
                var taskpuertas = frmCA.chromeBrowser.EvaluateScriptAsync(puertas);
                taskpuertas.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strpuertas = response.Result.ToString();
                            if (strpuertas == "5") { strpuertas = "5 o más"; }
                            string strscriptmarca = "var textToFind = '" + strpuertas + "';var dd = document.getElementById('doors');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                            //MessageBox.Show(strscriptmarca);
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('brands').onchange();", TimeSpan.FromSeconds(1));


                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript += "var objecte = document.getElementById('versions');";
                            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                        }
                    }
                    taskpuertas.Dispose();
                });

                //distancia
                var taskdistancia = frmCA.chromeBrowser.EvaluateScriptAsync(distancia);
                taskdistancia.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strdistancia = response.Result.ToString();
                            string strscriptmarca = "document.getElementById('km').value='" + strdistancia + "'";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);

                        }
                    }
                    taskdistancia.Dispose();
                });

                //Cilindrada - engineSize
                var taskcilindrada = frmCA.chromeBrowser.EvaluateScriptAsync(cilindrada);
                taskcilindrada.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strcilindrada = response.Result.ToString();
                            string strscriptmarca = "document.getElementById('engineSize-input').value='" + strcilindrada + "'";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);

                        }
                    }
                    taskcilindrada.Dispose();
                });

                //Tipo moneda - currencies
                var tasktipomoneda = frmCA.chromeBrowser.EvaluateScriptAsync(iconoprecio);
                tasktipomoneda.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            striconoprecio = response.Result.ToString();

                                string strscriptmarca = "var textToFind = '" + striconoprecio + "';var dd = document.getElementById('currencies');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                                //MessageBox.Show(strscriptmarca);
                                frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);

                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript += "var objecte = document.getElementById('versions');";
                            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                        }
                    }
                    tasktipomoneda.Dispose();
                });

                //pesos
                var taskpesos = frmCA.chromeBrowser.EvaluateScriptAsync(pesos);
                taskpesos.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strpesos = response.Result.ToString();
                            string strscriptmarca = "document.getElementById('price').value='" + strpesos + "'";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);

                        }
                    }
                    taskpesos.Dispose();
                });

                // carga carrocerias
                var taskcarroceria = frmCA.chromeBrowser.EvaluateScriptAsync(carroceria);
                taskcarroceria.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strcarroceria = response.Result.ToString();
                            if (DicCarrocerias.ContainsKey(strcarroceria))
                            {

                                strcarroceria = DicCarrocerias[strcarroceria];

                                string strscriptmarca = "var textToFind = '" + strcarroceria + "';var dd = document.getElementById('segments');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                                //MessageBox.Show(strscriptmarca);
                                frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);

                            }
                            else if (DicCarroceriasMotos.ContainsKey(strcarroceria)){

                                strcarroceria = DicCarroceriasMotos[strcarroceria];
                                string strscriptmarca = "var textToFind = '" + strcarroceria + "';var dd = document.getElementById('categories');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                                frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);
                            }

                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript += "var objecte = document.getElementById('versions');";
                            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                        }
                    }
                    taskcarroceria.Dispose();
                });

                // carga color
                var taskcolor = frmCA.chromeBrowser.EvaluateScriptAsync(color);
                taskcolor.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {

                            strcolor = response.Result.ToString();
                            if (strcolor == "") { strcolor = "Otro Color"; }
                            strcolor = strcolor.ToLower();
                            strcolor = strcolor.First().ToString().ToUpper() + strcolor.Substring(1);
                            string strscriptmarca = "var textToFind = '" + strcolor + "';var dd = document.getElementById('colors');for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}";
                            //MessageBox.Show(strscriptmarca);
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);
                            //frmDM.chromeBrowser2.ExecuteScriptAsync("document.getElementById('brands').onchange();", TimeSpan.FromSeconds(1));


                            // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                            string strJavascript = "var nouEvent = document.createEvent('MouseEvents');";
                            strJavascript += "nouEvent.initMouseEvent('change', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);";
                            strJavascript += "var objecte = document.getElementById('versions');";
                            strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strJavascript);

                        }
                    }
                    taskcolor.Dispose();
                });

                //Otros
                var taskotros = frmCA.chromeBrowser.EvaluateScriptAsync(otros);
                taskotros.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            strotros = response.Result.ToString();
                            string strscriptmarca = "document.getElementById('moreInfo-area').value='" + strotros + "'";
                            frmDM.chromeBrowser2.ExecuteScriptAsync(strscriptmarca);

                        }
                    }
                    taskotros.Dispose();
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
                            //strVersion = strVersion.ToLower();
                            //strVersion = strVersion.First().ToString().ToUpper() + strVersion.Substring(1);
                            //selecciono el modelo en demotores
                            //MessageBox.Show("cargando Modelos");
                            int milliseconds = 10000;
                            Thread.Sleep(milliseconds);
                            //var x = document.getElementById('versions').options.length;alert('version'+x);function cargaversion(){ var versionEncontrado = false; var datoversion = 'Hola'; var textToFind2 = datoversion;var dd = document.getElementById('versions'); for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind2){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;} if (dd.options[i].text === 'Otra Versión'){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}}setTimeout('cargaversion()',1000);
                            string strJavascript2 = "var x = document.getElementById('versions').options.length;function cargaversion(){ alert('largo'+x);var versionEncontrado = false; var datoversion = '" + strVersion +"'; var textToFind2 = datoversion;var dd = document.getElementById('versions'); for (var i = 0; i < dd.options.length; i++){if (dd.options[i].text === textToFind2){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;} if (dd.options[i].text === 'Otra Versión'){dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');break;}}}setTimeout('cargaversion()',1000);";

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
