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
using System.Web.Script.Serialization;

namespace publicacionCA_DM_CefSharp
{
    public partial class frmPrincipal : Form
    {
        public Demotores dmPub = new Demotores();
        public chileautos caPub = new chileautos();
        private static Dictionary<int, int> _dictionaryCombustible = new Dictionary<int, int>();
        private static Dictionary<int, int> _dictionaryPuertas = new Dictionary<int, int>();
        private static Dictionary<string, int> _dictionaryTransmision = new Dictionary<string, int>();
        private static Dictionary<string, int> _dictionaryDireccion = new Dictionary<string, int>();
        private static Dictionary<string, int> _dictionaryTipoMoneda = new Dictionary<string, int>();
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

            var obj = new BoundObject();
            frmCA.browserCA.RegisterJsObject("bound", obj);
            frmCA.browserCA.FrameLoadEnd += obj.OnFrameLoadEnd;
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


        public class BoundObject
        {
            private string datotipodistancia = "";
            private int datodistancia;

            public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
            {
                if (e.Frame.IsMain)
                {
                    var host = e.Browser.MainFrame.Url;
                    string url = host.ToString();

                    if (url == "http://desarrollofotos.chileautos.cl/actualizadores/paginas/chileautos/opciones.asp")
                    {



                        frmCA.browserCA.ExecuteScriptAsync(@"

                            $('input').mouseleave(function(){
                                if($(this).val() != ''){

                                        if($(this).attr('type') == ""checkbox""){
                                            //alert($(this).attr('type'));
                                            if ($(this).is(':checked')){
                                                bound.onSelected('true', $(this).attr('id'));
                                            }else{
                                                bound.onSelected('false', $(this).attr('id'));
                                            }
                                        }
                                        if($(this).attr('type') != ""checkbox""){

                                            bound.onSelected($(this).val(), $(this).attr('id'));
                                        
                                        }
                                }
                            });


                            $('textarea').mouseleave(function(){
                                if($(this).val() != ''){
                                        bound.onSelected($(this).val(), $(this).attr('id'));
                                }
                            });

                            $('select').change(function(){
                                if($(this).val() != ''){
                                        
                                        if($(this).attr('id') == 'nuevo'){

                                                bound.onSelected($('option:selected',this).val(), $(this).attr('id'));

                                        }else if($(this).attr('id') == 'transmisionwl'){
                                                                
                                                bound.onSelected($('option:selected',this).val(), $(this).attr('id'));
                                        
                                        }else if($(this).attr('id') == 'tipo_direccion'){
                                                                
                                                bound.onSelected($('option:selected',this).val(), $(this).attr('id'));
                                        
                                        }else if($(this).attr('id') == 'tipomoneda'){
                                                                
                                                bound.onSelected($('option:selected',this).val(), $(this).attr('id'));
                                        
                                        }else if($(this).attr('id') == 'tipokm'){
                                                                
                                                bound.onSelected($('option:selected',this).val(), $(this).attr('id'));
                                        
                                        }else{

                                                bound.onSelected($('option:selected',this).text(), $(this).attr('id'));

                                        }

                                }
                            });

                            document.body.onclick = function(e) {
                              var node = e.target;
                              while (node != undefined && node.localName != 'select') {
                                node = node.parentNode;
                              }
                              if (node != undefined) {
                                /*alert('Link!: ' + node.value);*/
                                    if(node.id == 'modelo'){
                                        bound.onSelected(node.value, node.id);
                                    }else if(node.id == 'combustible'){
                                        bound.onSelected(node.value, node.id);
                                    }

                                /* Your link handler here */
                                return false;  // stop handling the click
                              } else {
                                /*alert('This is not a input: ' + e.target.innerHTML)*/
                                return true;  // handle other clicks
                              }
                            }

                    ");
                    }
                }

            }
            public void OnSelected(string dato, string nombrecampo)
            {
                //MessageBox.Show("The user selected some text [" + dato + "]:"+nombrecampo);

                if (nombrecampo == "patente") {

                    CargaPatente(dato);

                }

                if (nombrecampo == "nuevo") {

                    CargaNuevo(dato);

                }

                if (nombrecampo == "cod_marca")
                {
                    CargaMarca(dato);
                }

                if (nombrecampo == "modelo")
                {
                    CargaModelo(dato);
                }

                if (nombrecampo == "version") {

                    CargaVersion(dato);

                }

                if (nombrecampo == "anoIng") {

                    CargaAngo(dato);
                }

                if (nombrecampo == "combustible") {

                    CargaCombustible(dato);

                }

                if (nombrecampo == "puertas") {

                    CargaNumPuertas(dato);

                }

                if (nombrecampo == "transmisionwl") {

                    CargaTransmision(dato);

                }

                if (nombrecampo == "tipo_direccion") {

                    CargaTipoDireccion(dato);

                }

                if (nombrecampo == "tipomoneda") {

                    CargUnidadMonetaria(dato);

                }

                if (nombrecampo == "pesos") {

                    CargaMonto(dato);

                }

                if (nombrecampo == "tipokm") {

                    CargaMedidaDistancia(dato);

                }

                if (nombrecampo == "distancia") {

                    CargaDistancia(dato);

                }

                if (nombrecampo == "carroceria") {

                    CargaCarroceria(dato);

                }

                if (nombrecampo == "cilindrada") {

                    CargaCilindradaMotos(dato);

                }

                if (nombrecampo == "txtOtros") {

                    CargaComentarios(dato);

                }

                if (nombrecampo == "color") {

                    CargaColor(dato);

                }

                if (nombrecampo == "motor") {

                    CargaDatoMotor(dato);

                }

                if (nombrecampo == "fwd") {

                    CargaCuatroWD(dato);

                }

                if (nombrecampo == "airbag") {

                    CargaAirBag(dato);

                }

                if (nombrecampo == "alarma") {

                    CargaAlarma(dato);

                }

                if (nombrecampo == "alzavidrios_electricos") {

                    CargaAlzaVidriosElectricos(dato);

                }

                if (nombrecampo == "aire_acondicionado") {

                    CargaAireAcondicionado(dato);

                }

                if (nombrecampo == "cierre_centralizado") {

                    CargaCierreCentralizado(dato);

                }

                if (nombrecampo == "espejos_electricos") {

                    CargaEspejosElectricos(dato);

                }

                if (nombrecampo == "frenos_Abs") {

                    CargaFrenosAbs(dato);

                }

                if (nombrecampo == "llantas") {

                    CargaLlantas(dato);

                }

                if (nombrecampo == "radio") {

                    CargaRadio(dato);

                }

                if (nombrecampo == "unico_dueno") {

                    CargaUnicoDueno(dato);

                }

            }
            
            public void CargaPatente(string dato) {

                if (dato != "")
                {
                    frmDM.browserDM.ExecuteScriptAsync(@"if (document.getElementById('licensePlate')) { document.getElementById('licensePlate').value ='" + dato + "';}");

                }

            }

            public void CargaNuevo(string dato) {


                if (dato == "S")
                {

                    frmDM.browserDM.ExecuteScriptAsync("document.getElementById('nuevos').checked='true';");

                }
                else {

                    frmDM.browserDM.ExecuteScriptAsync("document.getElementById('ocasion').checked='true';");

                }

            }

            public void CargaMarca(string dato) {


                string strJavascript = "$('#brands option').removeProp('selected'); $('#brands option:contains(\"" + dato + "\")').attr('selected', true);  var idbrand = $('#brands option:contains(\"" + dato + "\")').val(); var modelsBox = $('#models'); var nouEvent = document.createEvent('HTMLEvents');";
                strJavascript = strJavascript + "   nouEvent.initEvent('change', false, true);";
                strJavascript = strJavascript + "   var objecte = document.getElementById('brands');";
                strJavascript = strJavascript + "   objecte.dispatchEvent(nouEvent); giveOptionsBehavior(modelsBox, hasOtherOptionComboBehaviorFunction);";
                strJavascript = strJavascript + "   autoValidate(modelsBox, validateComboUsingOptionSelected);";
                strJavascript = strJavascript + "   autoReloadChilds(modelsBox, ['versions']);";
                strJavascript = strJavascript + "   autoValidateParents(modelsBox, ['brands', 'brands-other-input']);";
                strJavascript = strJavascript + "   associateReloadMySelf(modelsBox, ['brands'], function() { comboClear(modelsBox);";
                strJavascript = strJavascript + "   }, function() {";
                strJavascript = strJavascript + "       reloadCombo(";
                strJavascript = strJavascript + "           modelsBox,";
                strJavascript = strJavascript + "           'http://www.demotores.cl/frontend/posting/json/modelsByBrandIdTypeEnumCountryId.html',";
                strJavascript = strJavascript + "       {";
                strJavascript = strJavascript + "                       'brand': idbrand ,'type': typeId,'country': countryId";
                strJavascript = strJavascript + "       },";
                strJavascript = strJavascript + "   	addOptionToComboWithOther";
                strJavascript = strJavascript + "   );";
                strJavascript = strJavascript + "               });";
                strJavascript = strJavascript + "               modelsBox.bind('change', function(event) {";
                strJavascript = strJavascript + "       var currentOption = modelsBox.find('option:selected');";
                strJavascript = strJavascript + "   $_(currentOption).changeOption();";
                strJavascript = strJavascript + "});";
                strJavascript = strJavascript + "var nouEvent = document.createEvent('HTMLEvents');";
                strJavascript = strJavascript + "nouEvent.initEvent('change', false, true);";
                strJavascript = strJavascript + "var objecte = document.getElementById('brands');";
                strJavascript = strJavascript + "objecte.dispatchEvent(nouEvent);";

                frmDM.browserDM.ExecuteScriptAsync(strJavascript);

            }
            
            public void CargaModelo(string dato)
            {
                string strmodelo = dato;
                strmodelo = strmodelo.ToLower();
                strmodelo = strmodelo.First().ToString().ToUpper() + strmodelo.Substring(1);

                string strJavascript = "$('#models option:contains(\"" + strmodelo + "\")').prop('selected', 'selected'); ";
                // Lanzo el evento onchange de la marca en demotores para que se carguen los modelos
                strJavascript = strJavascript + "var nouEvent = document.createEvent('HTMLEvents');";
                strJavascript = strJavascript + "nouEvent.initEvent('change', false, true);";
                strJavascript = strJavascript + "var objecte = document.getElementById('models');";
                strJavascript = strJavascript + "objecte.dispatchEvent(nouEvent);";

                frmDM.browserDM.ExecuteScriptAsync(strJavascript);

            }

            public void CargaVersion(string dato) {

                if (dato != "")
                {
                    string strversion = dato;
                    strversion = strversion.ToLower();
                    strversion = strversion.First().ToString().ToUpper() + strversion.Substring(1);

                    string strJavascript = "$('#versions option:contains(\"" + strversion + "\")').prop('selected', 'selected');if (document.getElementById('versions')) {";
                    strJavascript += " var versionEncontrado = false; for (var i = 0; i < document.getElementById('versions').length; i++) {";
                    strJavascript += " if (document.getElementById('versions').options[i].text.toUpperCase()=='" + dato + "') {";
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
                    string strJavascript = "if (document.getElementById('versions')) {";
                    strJavascript += "  document.getElementById('msj-versions-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('versions').options[0].selected = true;";
                    strJavascript += " document.getElementById('versions_div').className = 'row row-error';}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }



            }
            
            public void CargaAngo(string dato) {


                int agno = int.Parse(dato);

                if (agno != 0)
                {

                    // Selecciono el año en Demotores
                    string strJavascript = " var agnoEncontrado = false; for (var i = 0; i < document.getElementById('years').length; i++) {";
                    strJavascript += " if (document.getElementById('years').options[i].text.toUpperCase() == '" + agno + "') {";
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
                    string strJavascript = " document.getElementById('msj-years-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('years').options[0].selected = true;";
                    strJavascript += "  document.getElementById('years_div').className = 'row row-error';";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }
                ActivaCargaImagenesDM();
            }

            public void CargaCombustible(string dato) {

                if (dato != "")
                {

                    int strCombustible = _dictionaryCombustible[int.Parse(dato)];

                    //Verifico si id fuels existe en demotores: está en autos/camionetas, camiones y casas rodantes
                    //y luego selecciono el combustible en demotores
                    string strJavascript = "if (document.getElementById('fuels')) {";
                    strJavascript += " var combustibleEncontrado = false; for (var i = 0; i < document.getElementById('fuels').length; i++) {";
                    strJavascript += " if (document.getElementById('fuels').options[i].value == '" + strCombustible + "') {";
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
                    string strJavascript = " document.getElementById('msj-fuels-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('fuels').options[0].selected = true;";
                    strJavascript += "  document.getElementById('fuels_div').className = 'row row-error';";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }

            }

            public void CargaNumPuertas(string dato) {

                if (dato != "")
                {
                    //selecciono las puertas en demotores

                    string strpuertas = dato;
                    if (strpuertas == "5") { strpuertas = "5 o más"; }

                    string strJavascript = " var puertasEncontrado = false; for (var i = 0; i < document.getElementById('doors').length; i++) {";
                    strJavascript += " if (document.getElementById('doors').options[i].text == '" + strpuertas + "') {";
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
                    string strJavascript = " document.getElementById('msj-doors-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('doors').options[0].selected = true;";
                    strJavascript += " document.getElementById('doors_div').className = 'row row-error';";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }

            }

            public void CargaTransmision(string dato) {

                if (dato != "")
                {
                    int entero = _dictionaryTransmision[dato];

                    //verifico si id transmissions existe en demotores
                    //selecciono la transmisión en demotores
                    string strJavascript = "if (document.getElementById('transmissions')) {";
                    strJavascript += " var transmisionEncontrado = false; for (var i = 0; i < document.getElementById('transmissions').length; i++) {";
                    strJavascript += " if (document.getElementById('transmissions').options[i].value == '" + entero + "') {";
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
                    string strJavascript = "if (document.getElementById('transmissions')) {";
                    strJavascript += "  document.getElementById('msj-transmissions-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('transmissions').options[0].selected = true;";
                    strJavascript += " document.getElementById('transmissions_div').className = 'row row-error';}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }

            }

            public void CargaTipoDireccion(string dato) {

                if (dato != "")
                {
                    int entero = _dictionaryDireccion[dato];
                    //selecciono la dirección en demotores
                    string strJavascript = " var direccionEncontrado = false; for (var i = 0; i < document.getElementById('steerings').length; i++) {";
                    strJavascript += " if (document.getElementById('steerings').options[i].value == '" + entero + "') {";
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
                    string strJavascript = "  document.getElementById('msj-steerings-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('steerings').options[0].selected = true;";
                    strJavascript += " document.getElementById('steerings_div').className = 'row row-error';";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }

            }

            public void CargUnidadMonetaria(string dato) {

                if (dato != "")
                {
                    int entero = _dictionaryTipoMoneda[dato];
                    //selecciono el tipo de moneda en demotores
                    string strJavascript = " var tipoMonedaEncontrado = false; for (var i = 0; i < document.getElementById('currencies').length; i++) {";
                    strJavascript += " if (document.getElementById('currencies').options[i].value == '" + entero + "') {";
                    strJavascript += "  tipoMonedaEncontrado = true; document.getElementById('currencies').options[i].selected = true; break;";
                    strJavascript += "}"; //del if
                    strJavascript += "}"; // del for
                    strJavascript += " if (tipoMonedaEncontrado ==  true) { document.getElementById('msj-currencies-error').className = 'correct';}";
                    strJavascript += " else { document.getElementById('msj-currencies-error').className = 'msj-error';}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }

            }

            public void CargaMonto(string dato) {

                if (dato != "")
                {
                    int entero = int.Parse(dato);
                    //Copio el precio a Demotores
                    string strJavascript = " document.getElementById('price').value = '" + entero + "';";
                    strJavascript += "  document.getElementById('msj-price-error').className = 'correct';";
                    strJavascript += "  document.getElementById('price_div').className = 'row';";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }
                else
                {
                    string strJavascript = "  document.getElementById('msj-price-error').className = 'msj-error';";
                    strJavascript += "  document.getElementById('price_div').className = 'row row-error';";
                    strJavascript += " document.getElementById('price').value ='';";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }

            }

            public void CargaMedidaDistancia(string dato) {


                if (dato != "")
                {

                    datotipodistancia = dato;
                    CargaDistancia(datodistancia.ToString());

                }

            }

            public void CargaDistancia(string dato) {

                datodistancia = int.Parse(dato);

                string strJavascript = "";

                if (dato != "")
                {
                    
                    int entero = int.Parse(dato);

                    if (datotipodistancia == "kms")
                    {
                        strJavascript = "if (document.getElementById('km')) {";
                        strJavascript += " document.getElementById('km').value = '" + entero + "';";
                        strJavascript += "  document.getElementById('msj-km-error').className = 'correct';";
                        strJavascript += "  document.getElementById('km_div').className = 'row';";
                        strJavascript += "}";
                    }
                    if (datotipodistancia == "millas")
                    {
                        int enteromillas = (int)(entero / 0.62137);
                        strJavascript = "if (document.getElementById('km')) {";
                        strJavascript += " document.getElementById('km').value = '" + enteromillas + "';";
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
                //datotipodistancia = "";
            }

            public void CargaCarroceria(string dato) {


                if (dato != "Seleccione carroceria" && dato != "Seleccione estilo")
                {
                    //selecciono la carrocería de autos en demotores
                    string strcarroceria = dato;
                    if (DicCarrocerias.ContainsKey(strcarroceria))
                    {
                        strcarroceria = DicCarrocerias[strcarroceria];
                        string strJavascript = "var textToFind = '" + strcarroceria + "';var dd = document.getElementById('segments'); var carroceriaEncontrada = false;";
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
                        string strJavascript = "var textToFind = '" + strcarroceria + "';var dd = document.getElementById('categories');var carroceriaEncontrada = false;";
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
                        string strJavascript = "if (document.getElementById('segments')) {";
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
                    string strJavascript = "if (document.getElementById('segments')) {";
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

            public void CargaCilindradaMotos(string dato) {


                if (dato != "0")
                {
                    //Carga Cilindrada en demotores
                    string strcilindrada = dato;
                    //Verifico si en DOM existe engineSize-input: Está en motos
                    string strJavascript = "if (document.getElementById('engineSize-input')) {";
                    strJavascript += "document.getElementById('engineSize-input').value='" + strcilindrada + "';";
                    strJavascript += " document.getElementById('msj-engineSize-input-error').className = 'correct';";
                    strJavascript += " document.getElementById('engineSize-input_div').className = 'row';} ";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }
                else
                {
                    //Verifico si en DOM existe engineSize-input: Está en motos
                    string strJavascript = "if (document.getElementById('engineSize-input')) {";
                    strJavascript += " document.getElementById('msj-engineSize-input-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('engineSize-input').value  = '';";
                    strJavascript += " document.getElementById('engineSize-input_div').className = 'row row-error';} ";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }

            }

            public void CargaComentarios(string dato) {

                if (dato != null)
                {
                    var strotros = dato;
                    frmDM.browserDM.ExecuteScriptAsync("document.getElementById('moreInfo-area').value='" + strotros + "'");
                }

            }

            public void CargaColor(string dato) {

                if (dato != "")
                {

                    //selecciono el color en demotores
                    //verifico si existe el id colors
                    string strJavascript = "if (document.getElementById('colors')) {";
                    strJavascript += " var colorEncontrado = false; for (var i = 0; i < document.getElementById('colors').length; i++) {";
                    strJavascript += " if (document.getElementById('colors').options[i].text.toUpperCase() == '" + dato.ToUpper() + "') {";
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
                    string strJavascript = "if (document.getElementById('colors')) {";
                    strJavascript += "  document.getElementById('msj-colors-error').className = 'msj-error';";
                    strJavascript += "    document.getElementById('colors').options[0].selected = true;";
                    strJavascript += "   document.getElementById('colors_div').className = 'row row-error';}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }

            }

            public void CargaDatoMotor(string dato) {

                if (dato != "")
                {

                    //Copio el contenido a demotores
                    //Verifico si en DOM existe engine: Está en camiones
                    string strJavascript = " if (document.getElementById('engine')) {";
                    strJavascript += " document.getElementById('engine').value = '" + dato + "';";
                    strJavascript += " document.getElementById('msj-engine-error').className = 'correct';";
                    strJavascript += " document.getElementById('engine_div').className = 'row'}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }
                else
                //Verifico si en DOM existe engine: Está en camiones
                {
                    string strJavascript = " if (document.getElementById('engine')) {";
                    strJavascript += "  document.getElementById('msj-engine-error').className = 'msj-error';";
                    strJavascript += " document.getElementById('engine').value = '';";
                    strJavascript += " document.getElementById('engine_div').className = 'row row-error';}";
                    frmDM.browserDM.ExecuteScriptAsync(strJavascript);
                }

            }

            public void CargaCuatroWD(string dato) {

                if (dato != null)
                {

                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"$('#features19').prop('checked', true);");} else { frmDM.browserDM.ExecuteScriptAsync(@"$('#features19').prop('checked', false);"); }
                    
                }

            }

            public void CargaAirBag(string dato) {

                if (dato != null)
                {

                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features110').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features110').checked = false;"); }
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features109').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features109').checked = false;"); }

                }

            }

            public void CargaAlarma(string dato) {

                if (dato != null)
                {
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features22').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features22').checked = false;"); }

                }

            }

            public void CargaAlzaVidriosElectricos(string dato) {

                if (dato != null)
                {
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features107').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features107').checked = false;"); }
                }

            }

            public void CargaAireAcondicionado(string dato) {

                if (dato != null)
                {
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features7').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features7').checked = false;"); }
                }

            }

            public void CargaCierreCentralizado(string dato) {

                if (dato != null)
                {
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features10').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features10').checked = false;"); }

                }

            }

            public void CargaEspejosElectricos(string dato) {

                if (dato!= null)
                {
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features32').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features32').checked = false;"); }

                }

            }

            public void CargaFrenosAbs(string dato) {

                if (dato != null)
                {
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features20').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features20').checked = false;"); }

                }

            }

            public void CargaLlantas(string dato) {

                if (dato != null)
                {
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features30').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features30').checked = false;"); }

                }

            }


            public void CargaRadio(string dato) {

                if (dato != null)
                {
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features37').checked = true;"); } else { frmDM.browserDM.ExecuteScriptAsync(@"document.getElementById('features37').checked = false;"); }

                }


            }


            public void CargaUnicoDueno(string dato) {

                if (dato != null)
                {
                    if (dato == "true") { frmDM.browserDM.ExecuteScriptAsync(@"$('#uniqueOwner').prop('checked', true);"); } else { frmDM.browserDM.ExecuteScriptAsync(@"$('#uniqueOwner').prop('checked', false);"); }
                }

            }

            public void ActivaCargaImagenesDM()
            {

                if (ScriptCargado == false) //Si no se ha cargado
                {

                    frmDM.browserDM.ExecuteScriptAsync(@"var totFiles = 0;
                   $('#noflash').remove();
                   $('#SWFUploaderDiv').remove();
                   $('#divFotos').remove();
                   $('.fotos-txt').remove();
                   $('#fotos').append('<div><input type=""file"" id=""upload"" multiple></div><div id=""wrap-desarrollofotos"" style=""float:left;width:520px;height:500px;""></div>');
                   $('#wrap-desarrollofotos').append('<div id=""wrap-fotos""></div>');
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

            }

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

        private void cargarImágenesToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void copiarToolStripMenuItem1_Click(object sender, EventArgs e)
            {

                string strJavascript, script;

                if (!frmDM.browserDM.IsBrowserInitialized) return;
                if (!frmCA.browserCA.IsBrowserInitialized) return;

                /*
                 *     Inyección de código para carga de desarrollofotos masiva en demotores
                */
                if (ScriptCargado == false) //Si no se ha cargado
                {
                    ////Carga img dentro del formulario DeMotores via JQuery
                    if (imgcargadasenservidordm != null)
                    {

                        string scriptcompeltodm = "";
                        foreach (var fname in imgcargadasenservidordm)
                        {

                            scriptcompeltodm = scriptcompeltodm + "<div class='item-desarrollofotos' style='width:90px;height:85px;float:left;margin:2px 2px;'><img style='width:100%;height:80%;' src='http://images.demotores.cl/post/tmp/siteposting/" + fname + "'><div class='del-item-foto' style='margin-top: -12px;' onclick='eliminar(this)'>eliminar</div></div>";


                        }
                        string adddiv = "<div id='wrap-fotos'></div>";
                        frmDM.browserDM.ExecuteScriptAsync("$('#fotos').append(\"" + adddiv + "\");$('#wrap-fotos').append(\"" + scriptcompeltodm + "\");$('#wrap-fotos').sortable({cursor: 'move',items: 'div',start: function(event, ui) {},stop : function(event, ui) {actualizaFotos();},placeholder : 'placeholder'});function actualizaFotos() { var aImg = [], tot = 0;$.each($('#fotos').find('img'), function(i, e) {if (tot < 24){ aImg.push($(e).attr('src').replace('http://images.demotores.cl/post/tmp/siteposting/', ''));tot++;} });$('#returnedFilesNames').val(aImg.join(','));$('#returnedFiles').val(aImg.join(','));}");

                        frmDM.browserDM.ExecuteScriptAsync(@"var totFiles = 0;
                   $('#noflash').remove();
                   $('#SWFUploaderDiv').remove();
                   $('#divFotos').remove();
                   $('.fotos-txt').remove();
                   $('#fotos').append('<div><input type=""file"" id=""upload"" multiple></div><div id=""wrap-desarrollofotos"" style=""float:left;width:520px;height:500px;""></div>');
                   $('#wrap-desarrollofotos').append('<div id=""wrap-fotos""></div>');
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

                    }

                    //Carga dimg dentro del formulario de Chileautos via JQuery



                    if (imgcargadasenservidorca != null)
                    {

                        string scriptcompletoca = "";
                        string[] cargaimgca = imgcargadasenservidorca.ToArray();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        scriptcompletoca = "var imgArray = " + js.Serialize(cargaimgca) + "; claUpload.init({container: 'uploadBox', page: '//ws.chileautos.cl/api-cla/Upload/auto/particular',redirPage: 'http://' + location.host + '/getupload', images: imgArray});";
                        frmCA.browserCA.ExecuteScriptAsync(scriptcompletoca);
                    }
                    ScriptCargado = true;
                }
            }
        
    }
}
