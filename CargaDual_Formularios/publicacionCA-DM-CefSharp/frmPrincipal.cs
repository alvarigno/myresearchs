using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CefSharp;
using publicacionCA_DM_CefSharp.Modelos;
using System.IO;
using UpLoadFile_WF;

namespace publicacionCA_DM_CefSharp
{
    public partial class frmPrincipal : Form
    {
        public Demotores dmPub = new Demotores();
        public chileautos caPub = new chileautos();
        private readonly Dictionary<int, int> _dictionaryCombustible = new Dictionary<int, int>();
        private readonly Dictionary<int, int> _dictionaryPuertas = new Dictionary<int, int>();
        private readonly Dictionary<string, int> _dictionaryTransmision = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _dictionaryDireccion = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _dictionaryTipoMoneda = new Dictionary<string, int>();
        public static Dictionary<string, string> DicCarrocerias = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCarroceriasMotos = new Dictionary<string, string>();
        public static bool ScriptCargado = false;
        public static String[] myImagenesArray;
        public static List<string> imgcargadasenservidordm = new List<string>();
        public static List<string> imgcargadasenservidorca = new List<string>();


        public frmPrincipal()
        {
            InitializeComponent();
            /* Relaciono los códigos de CA con los de DM */
            _dictionaryCombustible.Add(1, 69);
            _dictionaryCombustible.Add(2, 67);
            _dictionaryCombustible.Add(3, 291);
            _dictionaryCombustible.Add(4, 68);
            _dictionaryCombustible.Add(5, 68);
            _dictionaryCombustible.Add(10, 70);

            /* Código de puertas */
            _dictionaryPuertas.Add(0, 0);
            _dictionaryPuertas.Add(2, 64);
            _dictionaryPuertas.Add(3, 65);
            _dictionaryPuertas.Add(4, 66);
            _dictionaryPuertas.Add(5, 80);

            /* Transmisión */
            _dictionaryTransmision.Add("M", 60);
            _dictionaryTransmision.Add("A", 58);

            /* Tipo de dirección */
            _dictionaryDireccion.Add("H", 56);
            _dictionaryDireccion.Add("M", 57);
            _dictionaryDireccion.Add("A", 55);
            _dictionaryDireccion.Add("N", 1); // Sin correspondencia en demotores

            /* Tipo de moneda */
            _dictionaryTipoMoneda.Add("cl", 3);
            _dictionaryTipoMoneda.Add("us", 2);

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

            CefSettings settings = new CefSettings();
            // Inicializar CEF con la configuración proporcionada
            if (!Cef.IsInitialized)
                Cef.Initialize(settings);

        }

        private void frmPrincipal_Load(object sender, EventArgs e)
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

        // Ingresar otro aviso   **************************************************
        private void demotoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Si esta es la url cerrar página y formulario: http://www.demotores.cl/frontend/publicacion.html?execution=e1s3

            frmDM.NomAutomotora = "";

            //se localiza el formulario buscandolo entre los forms abiertos 
            Form formulario = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is frmDM);

            //si la instancia existe la cierro
            formulario?.Close();

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
            ScriptCargado = false;

            // Obtener nombre de automotora

            string script = "(function() {return document.getElementById('automotora').innerHTML;})();";
            var taskNomAutom = frmCA.browserCA.EvaluateScriptAsync(script);
            taskNomAutom.Wait();
            var responseNomAutom = taskNomAutom.Result;
            if (responseNomAutom.Success && responseNomAutom.Result.ToString() != "")
            {
                frmDM.NomAutomotora = responseNomAutom.Result.ToString();
            }

        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strJavascript, script;

            if (!frmDM.browserDM.IsBrowserInitialized) return;
            if (!frmCA.browserCA.IsBrowserInitialized) return;

            if ((frmCA.browserCA.Address.ToString() !=
                 "http://desarrollofotos.chileautos.cl/actualizadores/paginas/chileautos/opciones.asp") ||
                (frmDM.browserDM.Address.ToString() !=
                 "http://www.demotores.cl/frontend/publicacion.html?execution=e1s2"))
                return;


            /*
             *     Inyección de código para carga de desarrollofotos masiva en demotores
            */
            if (ScriptCargado == false) //Si no se ha cargado
            {
                ////Carga img dentro del formulario DeMotores via JQuery
                if (imgcargadasenservidordm != null) {

                    string scriptcompeltodm = "";
                    foreach (var fname in imgcargadasenservidordm)
                    {

                        scriptcompeltodm = scriptcompeltodm + "<div class='item-desarrollofotos' style='width:90px;height:85px;float:left;margin:2px 2px;'><img style='width:100%;height:80%;' src='http://images.demotores.cl/post/tmp/siteposting/"+fname+"'><div class='del-item-foto' style='margin-top: -12px;' onclick='eliminar(this)'>eliminar</div></div>";
                    

                    }
                    string adddiv = "<div id='wrap-fotos'></div>";
                    frmDM.browserDM.ExecuteScriptAsync("$('#fotos').append(\""+adddiv+"\");$('#wrap-fotos').append(\"" + scriptcompeltodm+ "\");$('#wrap-fotos').sortable({cursor: 'move',items: 'div',start: function(event, ui) {},stop : function(event, ui) {actualizaFotos();},placeholder : 'placeholder'});function actualizaFotos() { var aImg = [], tot = 0;$.each($('#fotos').find('img'), function(i, e) {if (tot < 24){ aImg.push($(e).attr('src').replace('http://images.demotores.cl/post/tmp/siteposting/', ''));tot++;} });$('#returnedFilesNames').val(aImg.join(','));$('#returnedFiles').val(aImg.join(','));}");
                }

                //Carga dimg dentro del formulario de Chileautos via JQuery

                

                if (imgcargadasenservidorca != null) {

                    string scriptcompletoca = "";
                    foreach (var fname in imgcargadasenservidorca) {

                        scriptcompletoca = scriptcompletoca + "<div class='claUpload-imgPreview'><input type='hidden' class='hiddenNameImg' value='"+fname+"'><div class='claUpload-btnCloseImg' title='Borrar foto'></div><img src='"+fname+"' style='height:100%; width: 100%;'></div>";

                    }
                    frmCA.browserCA.ExecuteScriptAsync("$('#claUpload-photos').append(\""+scriptcompletoca+"\")");
                }


                
                frmDM.browserDM.ExecuteScriptAsync(@"var totFiles = 0;
                   $('#noflash').remove();
                   $('#SWFUploaderDiv').remove();
                   $('#divFotos').remove();
                   $('.fotos-txt').remove();
                   $('#fotos').append('<div id=""wrap-fotos""></div>');
                   $('#fotos').append('<div><input type=""file"" id=""upload"" multiple></div><div id=""wrap-desarrollofotos"" style=""float:left;width:520px;height:500px;""></div>');
                   $('#upload').change(function(e){

                            

                       for (var i = 0, len = e.target.files.length; i < len; i++) {
                           (function(file, eliminar) {
                               var form = new FormData();

                               form.name = 'file';
                               form.append('file', file);
                                    
                               $.ajax({
                                   url: 'http://www.demotores.cl/frontend/upload',
                                   type: 'POST',
                                   data: form,
                                   contentType: false,
                                   processData: false
                               }).success(function(r){
                                   console.log(r);
                                   totFiles++;
                                   $('#wrap-fotos').append('<div class=""item-desarrollofotos"" style=""width:90px;height:85px;float:left;margin:2px 2px;""><img style=""width:100%;height:80%;"" src=""http://images.demotores.cl/post/tmp/siteposting/'+r+'""><div class=""del-item-foto"" style=""margin-top: -12px;"" onclick=""eliminar(this)"">eliminar</div></div>');

                                   if ($('#returnedFilesNames').val() !== '' || totFiles > 0){
                                       $('#returnedFilesNames').val($('#returnedFilesNames').val() + ',' + r);
                                       $('#returnedFiles').val($('#returnedFiles').val() + ',' + r);
                                   } else {
                                       $('#returnedFilesNames').val(r);
                                       $('#returnedFiles').val(r);
                                   }
                   
                                   $('#wrap-fotos').sortable({  
                                       cursor: 'move',
                                       items: 'div',
                                       start : function(event, ui) {
                                       },
                                       stop : function(event, ui) {
                                           actualizaFotos();
                                       },
                                       placeholder : 'placeholder'
                                   });
                               })
                           })(e.target.files[i], eliminar);
                       }
                   });
                   function eliminar(e) {
                       $(e).parent().remove();
                       totFiles--;

                       actualizaFotos();
                   }

                    function getSortedFilesForSubmit() {
                        var aImg = [], tot = 0;

                        $.each($('#fotos').find('img'), function(i, e) {
                            if (tot < 24) {
                                aImg.push($(e).attr('src').replace('http://images.demotores.cl/post/tmp/siteposting/', ''));
                                tot++;
                            }
                        });

                        return aImg.join(',');
                    } 

                   function actualizaFotos() {
                       var aImg = [], tot = 0;

                       $.each($('#fotos').find('img'), function(i, e) {
                           if (tot < 24) {
                               aImg.push($(e).attr('src').replace('http://images.demotores.cl/post/tmp/siteposting/', ''));
                                tot++;
                            }
                       });

                       $('#returnedFilesNames').val(aImg.join(','));
                       $('#returnedFiles').val(aImg.join(','));
                   }");

                ScriptCargado = true;
            }


            /*  *************************    Obtener la patente y copiarla          *************************************

             */
            script = "(function() {return document.getElementById('patente').value;})();";
            var task = frmCA.browserCA.EvaluateScriptAsync(script);
            task.ContinueWith(tskPatente =>
            {
                var response = tskPatente.Result;
                if (response.Success && response.Result.ToString() != "")
                {
                    caPub.patente = response.Result.ToString();
                    //Verifico si id licensePlate existe
                    frmDM.browserDM.ExecuteScriptAsync(@"if (document.getElementById('licensePlate')) { document.getElementById('licensePlate').value ='" + caPub.patente + "';}");
                    dmPub.patente = caPub.patente;
                }
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
            if (response1.Success && response1.Result != null)
            {
                if (caPub.marca != response1.Result.ToString())
                {

                    caPub.marca = response1.Result.ToString();
                    //selecciono la marca en demotores
                    strJavascript =
                        "var marcaEncontrada = false; for (var i = 0; i < document.getElementById('brands').length; i++) {";
                    strJavascript += " if (document.getElementById('brands').options[i].text == '" + caPub.marca +
                                     "') {";
                    strJavascript += "document.getElementById('brands').selectedIndex = i;";
                    strJavascript +=
                        "  marcaEncontrada = true; document.getElementById('brands').options[i].selected = true; break;";
                    strJavascript += "}"; //del if
                    strJavascript += "}"; // del for
                    strJavascript +=
                        "if (marcaEncontrada == true) { document.getElementById('msj-brands-error').className = 'correct';";
                    strJavascript += " document.getElementById('brands_div').className = 'row'; }";
                    strJavascript += "else { document.getElementById('msj-brands-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('brands').options[0].selected = true;";
                    strJavascript += " document.getElementById('brands_div').className = 'row row-error'; }";
                    strJavascript += " document.getElementById('_eventId_continue').removeAttribute('disabled');";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);

                    // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                    strJavascript = "var nouEvent = document.createEvent('HTMLEvents');";
                    strJavascript += "nouEvent.initEvent('change', false, true);";
                    strJavascript += "var objecte = document.getElementById('brands');";
                    strJavascript += "objecte.dispatchEvent(nouEvent);";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }
            }
            else
            {
                strJavascript = "document.getElementById('msj-brands-error').className = 'msj-error';";
                strJavascript += " document.getElementById('brands').options[0].selected = true;";
                strJavascript += " document.getElementById('brands_div').className = 'row row-error';";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskMarca.IsCompleted)
                taskMarca.Dispose();

            /* *************************   Obtener el año   ************************************************* 
             */
            script = "(function() {return document.getElementById('anoIng').value;})();";
            var taskAgno = frmCA.browserCA.EvaluateScriptAsync(script);
            taskAgno.Wait();
            var responseAgno = taskAgno.Result;
            if (responseAgno.Success && responseAgno.Result.ToString() != "")
            {
                caPub.agno = int.Parse(responseAgno.Result.ToString());
                // Selecciono el año en Demotores
                strJavascript =
                    " var agnoEncontrado = false; for (var i = 0; i < document.getElementById('years').length; i++) {";
                strJavascript += " if (document.getElementById('years').options[i].text.toUpperCase() == '" + caPub.agno + "') {";
                strJavascript += "  agnoEncontrado = true; document.getElementById('years').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += " if (agnoEncontrado ==  true) { document.getElementById('msj-years-error').className = 'correct';";
                strJavascript += "  document.getElementById('years_div').className = 'row';}";
                strJavascript += " else { document.getElementById('msj-years-error').className = 'msj-error';";
                strJavascript += " document.getElementById('years').options[0].selected = true;";
                strJavascript += "  document.getElementById('years_div').className = 'row row-error';}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            else
            {
                strJavascript = " document.getElementById('msj-years-error').className = 'msj-error';";
                strJavascript += " document.getElementById('years').options[0].selected = true;";
                strJavascript += "  document.getElementById('years_div').className = 'row row-error';";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskAgno.IsCompleted)
                taskAgno.Dispose();

            /* *********************************    Obtener fuente de energía (combustible)     *******************************
             */
            script = "(function() {return document.getElementById('combustible').value;})();";
            var taskFEnergia = frmCA.browserCA.EvaluateScriptAsync(script);
            taskFEnergia.Wait();
            var responseFEnergia = taskFEnergia.Result;
            if (responseFEnergia.Success && responseFEnergia.Result.ToString() != "")
            {
                caPub.fEnergia = int.Parse(responseFEnergia.Result.ToString());

                //Verifico si id fuels existe en demotores: está en autos/camionetas, camiones y casas rodantes
                //y luego selecciono el combustible en demotores
                strJavascript = "if (document.getElementById('fuels')) {";
                strJavascript += " var combustibleEncontrado = false; for (var i = 0; i < document.getElementById('fuels').length; i++) {";
                strJavascript += " if (document.getElementById('fuels').options[i].value == '" + _dictionaryCombustible[caPub.fEnergia] + "') {";
                strJavascript += "  combustibleEncontrado = true; document.getElementById('fuels').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += " if (combustibleEncontrado ==  true) { document.getElementById('msj-fuels-error').className = 'correct';";
                strJavascript += "  document.getElementById('fuels_div').className = 'row';}";
                strJavascript += " else { document.getElementById('msj-fuels-error').className = 'msj-error';";
                strJavascript += " document.getElementById('fuels').options[0].selected = true;";
                strJavascript += "  document.getElementById('fuels_div').className = 'row row-error';}";
                strJavascript += "}"; //fin del if inicial
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            else
            {
                strJavascript = " document.getElementById('msj-fuels-error').className = 'msj-error';";
                strJavascript += " document.getElementById('fuels').options[0].selected = true;";
                strJavascript += "  document.getElementById('fuels_div').className = 'row row-error';";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskFEnergia.IsCompleted)
                taskFEnergia.Dispose();

            /*    *************************************   Nro. de puertas          ********************************************
             */
            script = "(function() {return document.getElementById('puertas').value;})();";
            var taskPuertas = frmCA.browserCA.EvaluateScriptAsync(script);
            taskPuertas.Wait();
            var responsePuertas = taskPuertas.Result;
            if (responsePuertas.Success && responsePuertas.Result.ToString() != "")
            {
                caPub.puertas = int.Parse(responsePuertas.Result.ToString());
                //selecciono las puertas en demotores
                strJavascript = " var puertasEncontrado = false; for (var i = 0; i < document.getElementById('doors').length; i++) {";
                strJavascript += " if (document.getElementById('doors').options[i].value == '" + _dictionaryPuertas[caPub.puertas] + "') {";
                strJavascript += "  puertasEncontrado = true; document.getElementById('doors').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += " if (puertasEncontrado ==  true) { document.getElementById('msj-doors-error').className = 'correct';";
                strJavascript += " document.getElementById('doors_div').className = 'row';}";
                strJavascript += " else { document.getElementById('msj-doors-error').className = 'msj-error';";
                strJavascript += " document.getElementById('doors').options[0].selected = true;";
                strJavascript += " document.getElementById('doors_div').className = 'row row-error';}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            else
            {
                strJavascript = " document.getElementById('msj-doors-error').className = 'msj-error';";
                strJavascript += " document.getElementById('doors').options[0].selected = true;";
                strJavascript += " document.getElementById('doors_div').className = 'row row-error';";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskPuertas.IsCompleted)
                taskPuertas.Dispose();

            /*  ************************************ Obtener tipo de transmisión **********************************************
             */
            script = "(function() {return document.getElementById('transmisionwl').value;})();";
            var taskTransmision = frmCA.browserCA.EvaluateScriptAsync(script);
            taskTransmision.Wait();
            var responseTransmision = taskTransmision.Result;
            if (responseTransmision.Success && responseTransmision.Result.ToString() != "")
            {
                caPub.transmision = responseTransmision.Result.ToString();
                //verifico si id transmissions existe en demotores
                //selecciono la transmisión en demotores
                strJavascript = "if (document.getElementById('transmissions')) {";
                strJavascript += " var transmisionEncontrado = false; for (var i = 0; i < document.getElementById('transmissions').length; i++) {";
                strJavascript += " if (document.getElementById('transmissions').options[i].value == '" + _dictionaryTransmision[caPub.transmision] + "') {";
                strJavascript += "  transmisionEncontrado = true; document.getElementById('transmissions').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += " if (transmisionEncontrado ==  true) { document.getElementById('msj-transmissions-error').className = 'correct';";
                strJavascript += " document.getElementById('transmissions_div').className = 'row';}";
                strJavascript += " else { document.getElementById('msj-transmissions-error').className = 'msj-error';";
                strJavascript += " document.getElementById('transmissions').options[0].selected = true;";
                strJavascript += " document.getElementById('transmissions_div').className = 'row row-error';}";
                strJavascript += "}"; //fin del if inicial
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            else
            {
                strJavascript = "if (document.getElementById('transmissions')) {";
                strJavascript += "  document.getElementById('msj-transmissions-error').className = 'msj-error';";
                strJavascript += " document.getElementById('transmissions').options[0].selected = true;";
                strJavascript += " document.getElementById('transmissions_div').className = 'row row-error';}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskTransmision.IsCompleted)
                taskTransmision.Dispose();

            /*  ************************************ Obtener tipo de dirección **********************************************
             */
            script = "(function() {return document.getElementById('tipo_direccion').value;})();";
            var taskDireccion = frmCA.browserCA.EvaluateScriptAsync(script);
            taskDireccion.Wait();
            var responseDireccion = taskDireccion.Result;
            if (responseDireccion.Success && responseDireccion.Result.ToString() != "")
            {
                caPub.direccion = responseDireccion.Result.ToString();
                //selecciono la dirección en demotores
                strJavascript = " var direccionEncontrado = false; for (var i = 0; i < document.getElementById('steerings').length; i++) {";
                strJavascript += " if (document.getElementById('steerings').options[i].value == '" + _dictionaryDireccion[caPub.direccion] + "') {";
                strJavascript += "  direccionEncontrado = true; document.getElementById('steerings').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += " if (direccionEncontrado ==  true) { document.getElementById('msj-steerings-error').className = 'correct';";
                strJavascript += " document.getElementById('steerings_div').className = 'row';}";
                strJavascript += " else { document.getElementById('msj-steerings-error').className = 'msj-error';";
                strJavascript += " document.getElementById('steerings').options[0].selected = true;";
                strJavascript += " document.getElementById('steerings_div').className = 'row row-error';}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            else
            {
                strJavascript = "  document.getElementById('msj-steerings-error').className = 'msj-error';";
                strJavascript += " document.getElementById('steerings').options[0].selected = true;";
                strJavascript += " document.getElementById('steerings_div').className = 'row row-error';";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskDireccion.IsCompleted)
                taskDireccion.Dispose();

            /* **********************************   obtener unidad monetaria y precio     *************************************
             *  
             */
            script = "(function() {return document.getElementById('tipomoneda').value;})();";
            var taskTipoMoneda = frmCA.browserCA.EvaluateScriptAsync(script);
            taskTipoMoneda.Wait();
            var responseTipoMoneda = taskTipoMoneda.Result;
            if (responseTipoMoneda.Success && responseTipoMoneda.Result.ToString() != "")
            {
                caPub.tipoMoneda = responseTipoMoneda.Result.ToString();
                //selecciono el tipo de moneda en demotores
                strJavascript = " var tipoMonedaEncontrado = false; for (var i = 0; i < document.getElementById('currencies').length; i++) {";
                strJavascript += " if (document.getElementById('currencies').options[i].value == '" + _dictionaryTipoMoneda[caPub.tipoMoneda] + "') {";
                strJavascript += "  tipoMonedaEncontrado = true; document.getElementById('currencies').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += " if (tipoMonedaEncontrado ==  true) { document.getElementById('msj-currencies-error').className = 'correct';}";
                strJavascript += " else { document.getElementById('msj-currencies-error').className = 'msj-error';}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskTipoMoneda.IsCompleted)
                taskTipoMoneda.Dispose();

            script = "(function() {return document.getElementById('pesos').value;})();";
            var taskPrecio = frmCA.browserCA.EvaluateScriptAsync(script);
            taskPrecio.Wait();
            var responsePrecio = taskPrecio.Result;
            if (responsePrecio.Success && responsePrecio.Result.ToString() != "")
            {
                caPub.precio = int.Parse(responsePrecio.Result.ToString());
                //Copio el precio a Demotores
                strJavascript = " document.getElementById('price').value = '" + caPub.precio + "';";
                strJavascript += "  document.getElementById('msj-price-error').className = 'correct';";
                strJavascript += "  document.getElementById('price_div').className = 'row';";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            else
            {
                strJavascript = "  document.getElementById('msj-price-error').className = 'msj-error';";
                strJavascript += "  document.getElementById('price_div').className = 'row row-error';";
                strJavascript += " document.getElementById('price').value ='';";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskPrecio.IsCompleted)
                taskPrecio.Dispose();

            /* *************************************  Unidad de medida de distancia y distancia (kilometraje)   ****************
            */
            script = "(function() {return document.getElementById('tipokm').value;})();";
            var taskTipoDistancia = frmCA.browserCA.EvaluateScriptAsync(script);
            taskTipoDistancia.Wait();
            var responseTipoDistancia = taskTipoDistancia.Result;
            if (responseTipoDistancia.Success && responseTipoDistancia.Result.ToString() != "")
            {
                caPub.tipoDistancia = responseTipoDistancia.Result.ToString();
            }
            if (taskTipoDistancia.IsCompleted)
                taskTipoDistancia.Dispose();

            const string scriptDistancia = "(function() {return document.getElementById('distancia').value;})();";
            var taskDistancia = frmCA.browserCA.EvaluateScriptAsync(scriptDistancia);
            taskDistancia.Wait();
            var responseDistancia = taskDistancia.Result;
            if (responseDistancia.Success && responseDistancia.Result.ToString() != "")
            {
                caPub.distancia = int.Parse(responseDistancia.Result.ToString());

                if (caPub.tipoDistancia == "kms")
                {
                    strJavascript = "if (document.getElementById('km')) {";
                    strJavascript += " document.getElementById('km').value = '" + caPub.distancia + "';";
                    strJavascript += "  document.getElementById('msj-km-error').className = 'correct';";
                    strJavascript += "  document.getElementById('km_div').className = 'row';";
                    strJavascript += "}";
                }
                else
                {
                    dmPub.distancia = (int)(caPub.distancia / 0.62137);
                    strJavascript = "if (document.getElementById('km')) {";
                    strJavascript += " document.getElementById('km').value = '" + dmPub.distancia + "';";
                    strJavascript += "  document.getElementById('msj-km-error').className = 'correct';";
                    strJavascript += "  document.getElementById('km_div').className = 'row';";
                    strJavascript += "}";
                }
            }
            else
            {
                strJavascript = "if (document.getElementById('km')) {";
                strJavascript += "  document.getElementById('msj-km-error').className = 'msj-error';";
                strJavascript += "  document.getElementById('km_div').className = 'row row-error';";
                strJavascript += " document.getElementById('km').value = '';";
                strJavascript += "}";
            }
            frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            if (taskDistancia.IsCompleted)
                taskDistancia.Dispose();


            /*  ************************************ Obtener Carroceria **********************************************
             */
            script = "(function(){ var e = document.getElementById('carroceria');return e.options[e.selectedIndex].text;})();";
            var taskcarroceria = frmCA.browserCA.EvaluateScriptAsync(script);
            taskcarroceria.Wait();
            var responseCarroceria = taskcarroceria.Result;
            if (responseCarroceria.Success && responseCarroceria.Result.ToString() != "Seleccione carroceria" && responseCarroceria.Result.ToString() != "Seleccione estilo")
            {
                //selecciono la carrocería de autos en demotores
                string strcarroceria = responseCarroceria.Result.ToString();
                if (DicCarrocerias.ContainsKey(strcarroceria))
                {
                    strcarroceria = DicCarrocerias[strcarroceria];
                    strJavascript = "var textToFind = '" + strcarroceria + "';var dd = document.getElementById('segments'); var carroceriaEncontrada = false;";
                    strJavascript += "for (var i = 0; i < dd.options.length; i++) { ";
                    strJavascript += "if (dd.options[i].text === textToFind){ ";
                    strJavascript += "   dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');";
                    strJavascript += " carroceriaEncontrada=true;";
                    strJavascript += "break;}";
                    strJavascript += "}"; //fin del for
                    strJavascript += "if (carroceriaEncontrada == true) {";
                    strJavascript += " document.getElementById('msj-segments-error').className = 'correct';";
                    strJavascript += " document.getElementById('segments_div').className = 'row';}";
                    strJavascript += " else {";
                    strJavascript += " document.getElementById('msj-segments-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('segments').options[0].selected = true;";
                    strJavascript += " document.getElementById('segments_div').className = 'row row-error';}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);

                }
                else if (DicCarroceriasMotos.ContainsKey(strcarroceria))
                {
                    //Carrocerías motos
                    strcarroceria = DicCarroceriasMotos[strcarroceria];
                    strJavascript = "var textToFind = '" + strcarroceria + "';var dd = document.getElementById('categories');var carroceriaEncontrada = false;";
                    strJavascript += "for (var i = 0; i < dd.options.length; i++) { ";
                    strJavascript += "if (dd.options[i].text === textToFind) { ";
                    strJavascript += "dd.selectedIndex = i;dd.options[i].setAttribute('selected', 'selected');";
                    strJavascript += " carroceriaEncontrada=true;";
                    strJavascript += "break;}";
                    strJavascript += "}"; //fin del for
                    strJavascript += "if (carroceriaEncontrada == true) {";
                    strJavascript += " document.getElementById('msj-categories-error').className = 'correct';";
                    strJavascript += " document.getElementById('category_div').className = 'row';}";
                    strJavascript += " else {";
                    strJavascript += " document.getElementById('msj-categories-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('categories').options[0].selected = true;";
                    strJavascript += " document.getElementById('category_div').className = 'row row-error';}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }
                else
                {
                    //En las motos el segmento tiene el id de categories, y en autos segments
                    strJavascript = "if (document.getElementById('segments')) {";
                    strJavascript += " document.getElementById('msj-segments-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('segments').options[0].selected = true;";
                    strJavascript += " document.getElementById('segments_div').className = 'row row-error';";
                    strJavascript += "}";
                    strJavascript += "if (document.getElementById('categories')) {";
                    strJavascript += " document.getElementById('msj-categories-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('categories').options[0].selected = true;";
                    strJavascript += " document.getElementById('category_div').className = 'row row-error';";
                    strJavascript += "}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }
            }
            else
            {
                //En las motos el segmento tiene el id de categories, y en autos segments
                strJavascript = "if (document.getElementById('segments')) {";
                strJavascript += " document.getElementById('msj-segments-error').className = 'msj-error';";
                strJavascript += " document.getElementById('segments').options[0].selected = true;";
                strJavascript += " document.getElementById('segments_div').className = 'row row-error';";
                strJavascript += "}";
                strJavascript += "if (document.getElementById('categories')) {";
                strJavascript += " document.getElementById('msj-categories-error').className = 'msj-error';";
                strJavascript += " document.getElementById('categories').options[0].selected = true;";
                strJavascript += " document.getElementById('category_div').className = 'row row-error';";
                strJavascript += "}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskcarroceria.IsCompleted)
                taskcarroceria.Dispose();


            /* **************************      Para obtener la cilindrada para motos    *********************************
             */

            script = "(function(){ return document.getElementById('cilindrada').value;})(); ";
            var taskCilindrada = frmCA.browserCA.EvaluateScriptAsync(script);
            taskCilindrada.Wait();
            var responseCilindrada = taskCilindrada.Result;
            if (responseCilindrada.Success && responseCilindrada.Result.ToString() != "0")
            {
                //Carga Cilindrada en demotores
                string strcilindrada = responseCilindrada.Result.ToString();
                //Verifico si en DOM existe engineSize-input: Está en motos
                strJavascript = "if (document.getElementById('engineSize-input')) {";
                strJavascript += "document.getElementById('engineSize-input').value='" + strcilindrada + "';";
                strJavascript += " document.getElementById('msj-engineSize-input-error').className = 'correct';";
                strJavascript += " document.getElementById('engineSize-input_div').className = 'row';} ";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            else
            {
                //Verifico si en DOM existe engineSize-input: Está en motos
                strJavascript = "if (document.getElementById('engineSize-input')) {";
                strJavascript += " document.getElementById('msj-engineSize-input-error').className = 'msj-error';";
                strJavascript += " document.getElementById('engineSize-input').value  = '';";
                strJavascript += " document.getElementById('engineSize-input_div').className = 'row row-error';} ";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }

            if (taskCilindrada.IsCompleted)
                taskCilindrada.Dispose();


            /*  *************************    Obtener la otros y copiarla a comentarios          *************************************
             
             */
            script = "(function(){ return document.getElementById('txtOtros').value;})();";
            var taskotros = frmCA.browserCA.EvaluateScriptAsync(script);
            taskotros.Wait();
            var responseOtros = taskotros.Result;
            if (responseOtros.Success && responseOtros.Result != null)
            {
                var strotros = responseOtros.Result;
                frmDM.browserDM.ExecuteScriptAsync("document.getElementById('moreInfo-area').value='" + strotros + "'");
            }
            if (taskotros.IsCompleted)
                taskotros.Dispose();

            /*     *************************************  obtener los checkboxes seleccionados    *****************************
             *     
            */
            //            4WD  -> 4 Wheel Drive
            script = "(function() {return document.getElementById('fwd').checked;})();";
            var taskFwd = frmCA.browserCA.EvaluateScriptAsync(script);
            taskFwd.Wait();
            var responseFwd = taskFwd.Result;
            if (responseFwd.Success && responseFwd.Result != null)
            {
                caPub.fwd = responseFwd.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.fwd
                    ? @"document.getElementById('features19').checked = true;"
                    : @"document.getElementById('features19').checked = false;");
            }

            //            Airbag
            script = "(function() {return document.getElementById('airbag').checked;})();";
            var taskAirbag = frmCA.browserCA.EvaluateScriptAsync(script);
            taskAirbag.Wait();
            var responseAirbag = taskAirbag.Result;
            if (responseAirbag.Success && responseAirbag.Result != null)
            {
                caPub.airbag = responseAirbag.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.airbag
                    ? @"document.getElementById('features110').checked = true;"
                    : @"document.getElementById('features110').checked = false;");

                frmDM.browserDM.ExecuteScriptAsync(caPub.airbag
                    ? @"document.getElementById('features109').checked = true;"
                    : @"document.getElementById('features109').checked = false;");
            }

            //            Alarma
            script = "(function() {return document.getElementById('alarma').checked;})();";
            var taskAlarma = frmCA.browserCA.EvaluateScriptAsync(script);
            taskAlarma.Wait();
            var responseAlarma = taskAlarma.Result;
            if (responseAlarma.Success && responseAlarma.Result != null)
            {
                caPub.alarma = responseAlarma.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.alarma
                    ? @"document.getElementById('features22').checked = true;"
                    : @"document.getElementById('features22').checked = false;");
            }

            //            Alzavidrios eléctricos
            script = "(function() {return document.getElementById('alzavidrios_electricos').checked;})();";
            var taskAlzavidriosElectricos = frmCA.browserCA.EvaluateScriptAsync(script);
            taskAlzavidriosElectricos.Wait();
            var responseAlzavidriosElectricos = taskAlzavidriosElectricos.Result;
            if (responseAlzavidriosElectricos.Success && responseAlzavidriosElectricos.Result != null)
            {
                caPub.alzavidrios_electricos = responseAlzavidriosElectricos.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.alzavidrios_electricos
                    ? @"document.getElementById('features107').checked = true;"
                    : @"document.getElementById('features107').checked = false;");
            }

            //            Aire acondicionado
            script = "(function() {return document.getElementById('aire_acondicionado').checked;})();";
            var taskAireAcondicionado = frmCA.browserCA.EvaluateScriptAsync(script);
            taskAireAcondicionado.Wait();
            var responseAireAcondicionado = taskAireAcondicionado.Result;
            if (responseAireAcondicionado.Success && responseAireAcondicionado.Result != null)
            {
                caPub.aire_acondicionado = responseAireAcondicionado.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.aire_acondicionado
                    ? @"document.getElementById('features7').checked = true;"
                    : @"document.getElementById('features7').checked = false;");
            }

            //            Cierre centralizado
            script = "(function() {return document.getElementById('cierre_centralizado').checked;})();";
            var taskCierreCentralizado = frmCA.browserCA.EvaluateScriptAsync(script);
            taskCierreCentralizado.Wait();
            var responseCierreCentralizado = taskCierreCentralizado.Result;
            if (responseCierreCentralizado.Success && responseCierreCentralizado.Result != null)
            {
                caPub.cierre_centralizado = responseCierreCentralizado.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.cierre_centralizado
                    ? @"document.getElementById('features10').checked = true;"
                    : @"document.getElementById('features10').checked = false;");
            }

            //            Espejos eléctricos
            script = "(function() {return document.getElementById('espejos_electricos').checked;})();";
            var taskEspejosElectricos = frmCA.browserCA.EvaluateScriptAsync(script);
            taskEspejosElectricos.Wait();
            var responseEspejosElectricos = taskEspejosElectricos.Result;
            if (responseEspejosElectricos.Success && responseEspejosElectricos.Result != null)
            {
                caPub.espejos_electricos = responseEspejosElectricos.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.espejos_electricos
                    ? @"document.getElementById('features32').checked = true;"
                    : @"document.getElementById('features32').checked = false;");
            }

            //            Frenos ABS
            script = "(function() {return document.getElementById('frenos_Abs').checked;})();";
            var taskFrenosAbs = frmCA.browserCA.EvaluateScriptAsync(script);
            taskFrenosAbs.Wait();
            var responseFrenosAbs = taskFrenosAbs.Result;
            if (responseFrenosAbs.Success && responseFrenosAbs.Result != null)
            {
                caPub.frenos_Abs = responseFrenosAbs.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.frenos_Abs
                    ? @"document.getElementById('features20').checked = true;"
                    : @"document.getElementById('features20').checked = false;");
            }

            //            Llantas
            script = "(function() {return document.getElementById('llantas').checked;})();";
            var taskLlantas = frmCA.browserCA.EvaluateScriptAsync(script);
            taskLlantas.Wait();
            var responseLlantas = taskLlantas.Result;
            if (responseLlantas.Success && responseLlantas.Result != null)
            {
                caPub.llantas = responseLlantas.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.llantas
                    ? @"document.getElementById('features30').checked = true;"
                    : @"document.getElementById('features30').checked = false;");
            }


            //            Radio
            script = "(function() {return document.getElementById('radio').checked;})();";
            var taskRadio = frmCA.browserCA.EvaluateScriptAsync(script);
            taskRadio.Wait();
            var responseRadio = taskRadio.Result;
            if (responseRadio.Success && responseRadio.Result != null)
            {
                caPub.radio = responseRadio.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.radio
                    ? @"document.getElementById('features37').checked = true;"
                    : @"document.getElementById('features37').checked = false;");
            }

            //            Único dueño - primer dueño
            script = "(function() {return document.getElementById('unico_dueno').checked;})();";
            var taskUnicoDuegno = frmCA.browserCA.EvaluateScriptAsync(script);
            taskUnicoDuegno.Wait();
            var responseUnicoDuegno = taskUnicoDuegno.Result;
            if (responseUnicoDuegno.Success && responseUnicoDuegno.Result != null)
            {
                caPub.unico_duegno = responseUnicoDuegno.Result.ToString() == "True" ? true : false;
                frmDM.browserDM.ExecuteScriptAsync(caPub.unico_duegno
                    ? @"document.getElementById('uniqueOwner').checked = true;"
                    : @"document.getElementById('uniqueOwner').checked = false;");
            }

            /* **************************************   Obtener el modelo      *************************************************
                       * Observación: Si el modelo no se encuentra el select se transforma en un input
                    */
            script = "(function() {return document.getElementById('modelo').value;})();";
            Thread.Sleep(2500);
            var taskModelo = frmCA.browserCA.EvaluateScriptAsync(script);
            taskModelo.Wait();
            var responseModelo = taskModelo.Result;
            if (responseModelo.Success && responseModelo.Result != null)
            {
                if (caPub.modelo != responseModelo.Result.ToString())
                {
                    caPub.modelo = responseModelo.Result.ToString();
                    /*  
                     *   verifico si se cargaron los modelos
                     */
                    strJavascript = "if (document.getElementById('models').length < 2) {";

                    // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                    strJavascript += "var nouEvent = document.createEvent('HTMLEvents');";
                    strJavascript += "nouEvent.initEvent('change', false, true);";
                    strJavascript += "var objecte = document.getElementById('brands');";
                    strJavascript += "objecte.dispatchEvent(nouEvent);}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);

                    //selecciono el modelo en demotores
                    strJavascript = "if (document.getElementById('models').length > 1) {";
                    strJavascript += " var modeloEncontrado = false; for (var i = 0; i < document.getElementById('models').length; i++) {";
                    strJavascript += " if (document.getElementById('models').options[i].text.toUpperCase()=='" + caPub.modelo + "') {";
                    strJavascript += "  modeloEncontrado = true; document.getElementById('models').options[i].selected = true; break;";
                    strJavascript += "}"; //del if
                    strJavascript += "}"; // del for
                    strJavascript += " if (modeloEncontrado ==  true) { document.getElementById('msj-models-error').className = 'correct';";
                    strJavascript += "   document.getElementById('models_div').className = 'row';}";
                    strJavascript += " else { document.getElementById('msj-models-error').className = 'msj-error';";
                    strJavascript += "    document.getElementById('models').options[0].selected = true;";
                    strJavascript += "   document.getElementById('models_div').className = 'row row-error';}";
                    strJavascript += "}"; //fin verificación de modelos cargados
                    strJavascript += "else {alert('No se cargaron los modelos');}";

                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);

                    // Lanzo el evento onchange del modelo en demotores para que se carguen las versiones
                    strJavascript = "var nouEvent = document.createEvent('HTMLEvents');";
                    strJavascript += "nouEvent.initEvent('change', false, true);";
                    strJavascript += "var objecte = document.getElementById('models');";
                    strJavascript += "objecte.dispatchEvent(nouEvent);";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }
            }
            if (taskModelo.IsCompleted)
                taskModelo.Dispose();

            /* *************************************      Obtengo el color     ************************************************
           */
            script = "(function() {return document.getElementById('color').value;})();";
            var taskColor = frmCA.browserCA.EvaluateScriptAsync(script);
            taskColor.Wait();
            var responseColor = taskColor.Result;
            if (responseColor.Success && responseColor.Result.ToString() != "")
            {
                caPub.color = responseColor.Result.ToString();

                //selecciono el color en demotores
                //verifico si existe el id colors
                strJavascript = "if (document.getElementById('colors')) {";
                strJavascript += " var colorEncontrado = false; for (var i = 0; i < document.getElementById('colors').length; i++) {";
                strJavascript += " if (document.getElementById('colors').options[i].text.toUpperCase() == '" + caPub.color.ToUpper() + "') {";
                strJavascript += "  colorEncontrado = true; document.getElementById('colors').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += " if (colorEncontrado ==  true) { document.getElementById('msj-colors-error').className = 'correct';";
                strJavascript += "   document.getElementById('colors_div').className = 'row';}";
                strJavascript += " else { document.getElementById('msj-colors-error').className = 'msj-error';";
                strJavascript += "    document.getElementById('colors').options[0].selected = true;";
                strJavascript += "   document.getElementById('colors_div').className = 'row row-error';}";
                strJavascript += "}"; //cierre del if inicial
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);

            }
            else
            {
                strJavascript = "if (document.getElementById('colors')) {";
                strJavascript += "  document.getElementById('msj-colors-error').className = 'msj-error';";
                strJavascript += "    document.getElementById('colors').options[0].selected = true;";
                strJavascript += "   document.getElementById('colors_div').className = 'row row-error';}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskColor.IsCompleted)
                taskColor.Dispose();


            /*    *************************************   Obtener descripción motor       ***********************************************
            *    
            */
            script = "(function() {return document.getElementById('motor').value;})();";
            var taskMotor = frmCA.browserCA.EvaluateScriptAsync(script);
            taskMotor.Wait();
            var responseMotor = taskMotor.Result;
            if (responseMotor.Success && responseMotor.Result.ToString() != "")
            {
                caPub.motor = responseMotor.Result.ToString();
                //Copio el contenido a demotores
                //Verifico si en DOM existe engine: Está en camiones
                strJavascript = " if (document.getElementById('engine')) {";
                strJavascript += " document.getElementById('engine').value = '" + caPub.motor + "';";
                strJavascript += " document.getElementById('msj-engine-error').className = 'correct';";
                strJavascript += " document.getElementById('engine_div').className = 'row'}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            else
            //Verifico si en DOM existe engine: Está en camiones
            {
                strJavascript = " if (document.getElementById('engine')) {";
                strJavascript += "  document.getElementById('msj-engine-error').className = 'msj-error';";
                strJavascript += " document.getElementById('engine').value = '';";
                strJavascript += " document.getElementById('engine_div').className = 'row row-error';}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskMotor.IsCompleted)
                taskMotor.Dispose();

            /*    *************************************   Obtener la versión       ***********************************************
                *    * Observación: Si la versión no se encuentra el select se transforma en un input
            */
            script = "(function() {return document.getElementById('version').value;})();";
            var taskVersion = frmCA.browserCA.EvaluateScriptAsync(script);
            taskVersion.Wait();
            var responseVersion = taskVersion.Result;
            if (responseVersion.Success && responseVersion.Result.ToString() != "")
            {
                caPub.version = responseVersion.Result.ToString();
                //Verifico si id versions existe en demotores
                //selecciono la versión en demotores
                strJavascript = "if (document.getElementById('versions')) {";
                strJavascript += " var versionEncontrado = false; for (var i = 0; i < document.getElementById('versions').length; i++) {";
                strJavascript += " if (document.getElementById('versions').options[i].text.toUpperCase()=='" + caPub.version.ToUpper() + "') {";
                strJavascript += "  versionEncontrado = true; document.getElementById('versions').options[i].selected = true; break;";
                strJavascript += "}"; //del if
                strJavascript += "}"; // del for
                strJavascript += " if (versionEncontrado ==  true) { document.getElementById('msj-versions-error').className = 'correct';";
                strJavascript += "   document.getElementById('versions_div').className = 'row';}";
                strJavascript += " else { document.getElementById('msj-versions-error').className = 'msj-error';";
                strJavascript += "    document.getElementById('versions').options[0].selected = true;";
                strJavascript += "   document.getElementById('versions_div').className = 'row row-error';}";
                strJavascript += "}"; //fin if inicial
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            else
            {
                strJavascript = "if (document.getElementById('versions')) {";
                strJavascript += "  document.getElementById('msj-versions-error').className = 'msj-error';";
                strJavascript += " document.getElementById('versions').options[0].selected = true;";
                strJavascript += " document.getElementById('versions_div').className = 'row row-error';}";
                frmDM.browserDM.ExecuteScriptAsync(strJavascript);
            }
            if (taskVersion.IsCompleted)
                taskVersion.Dispose();

        }

        private void publicarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Confirme publicación simultánea", @"Confirmación", MessageBoxButtons.YesNo) !=
                DialogResult.Yes) return;

            //var strJavascript = "nouEvent.initMouseEvent('click', true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);";
            //strJavascript += "var objecte = document.getElementById('btnsub');";
            //strJavascript += "var canceled = !objecte.dispatchEvent(nouEvent);";
            frmCA.browserCA.ExecuteScriptAsync(@"$('#btnsub').click();");
            frmDM.browserDM.ExecuteScriptAsync(@"$('#_eventId_continue').click();");
            ScriptCargado = false;

        }

        private void subirImágenesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new UpLoadFile_WF.Form1() { Owner = this })
            {
                f.ShowDialog();
                myImagenesArray = UpLoadFile_WF.Form1.listadoimagenes;
            }

            string dataimg = "";
            if (myImagenesArray != null)
            {

                foreach (String txt in myImagenesArray)
                {

                    dataimg = dataimg + txt + ";";
                }

                if (dataimg != "") { dataimg = dataimg.Remove(dataimg.Length - 1); }
                EnviaImgToDMImgServer();
                EnviaImgToCAImgServer();
                MessageBox.Show("Listado de imágenes: " + dataimg);

            }

        }

        public byte[] imgToByteArray(string inImg)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(inImg);
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        /// <summary>
        /// Sube listado de array into DeMotores ImageServer
        /// </summary>
        public void EnviaImgToDMImgServer() {

            DM_ImgUploadServer uploadserver = new DM_ImgUploadServer();

            //carga Var Global con listado de nombres de imagenes en servidor de DM//
            imgcargadasenservidordm = uploadserver.Uploadimage(myImagenesArray);

        }

        /// <summary>
        /// Sube listado de array into Chileautos ImageServer
        /// </summary>
        public void EnviaImgToCAImgServer()
        {

            CA_ImgUploadServer uploadserver = new CA_ImgUploadServer();

            //carga Var Global con listado de nombres de imagenes en servidor de DM//
            imgcargadasenservidorca = uploadserver.Uploadimage(myImagenesArray);

        }

        

    }
}
