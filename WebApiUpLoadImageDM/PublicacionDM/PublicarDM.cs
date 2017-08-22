using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using AccesoDatos;

namespace PublicacionDM
{
    public class PublicarDM
    {

        public static Dictionary<string, string> DicCombustibles = new Dictionary<string, string>();
        public static Dictionary<string, string> DicTransmision = new Dictionary<string, string>();
        public static Dictionary<string, string> DicDireccion = new Dictionary<string, string>();
        public static Dictionary<string, string> DicNumPuertas = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCarrocerias = new Dictionary<string, string>();
        public static Dictionary<string, string> DicColor = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCarroceriasMotos = new Dictionary<string, string>();

        public static void CargaDiccionarios()
        {

            //Diccionario Combustibles
            DicCombustibles.Add("1", "Bencina");
            DicCombustibles.Add("2", "Diesel");
            DicCombustibles.Add("4", "Eléctrico / Híbrido");
            DicCombustibles.Add("5", "Eléctrico / Híbrido");
            DicCombustibles.Add("3", "Gas / Bencina");
            DicCombustibles.Add("10", "Otros");

            //Diccionario Transmisión
            DicCombustibles.Add("S", "Automática");
            DicCombustibles.Add("N", "Manual");

            //Diccionario Dirección
            DicDireccion.Add("H", "Asistida");
            DicDireccion.Add("M", "Hidráulica");
            DicDireccion.Add("A", "Mecánica");

            //Diccionario Número de puertas
            DicNumPuertas.Add("2","2");
            DicNumPuertas.Add("3", "3");
            DicNumPuertas.Add("4", "4");
            DicNumPuertas.Add("5", "5 o más");

            //Diccionario DicCarrocerias autos
            DicCarrocerias.Add("CS", "Camioneta");
            DicCarrocerias.Add("CA", "Convertible");
            DicCarrocerias.Add("CO", "Coupé");
            DicCarrocerias.Add("ES", "Familiar");
            DicCarrocerias.Add("HA", "Hatchback");
            DicCarrocerias.Add("OD", "Monovolumen");
            DicCarrocerias.Add("PU", "Pick Up");
            DicCarrocerias.Add("CP", "Pick Up");
            DicCarrocerias.Add("SA", "Sedán");
            DicCarrocerias.Add("MM", "Utilitario");

            //Diccionario de Colores
            DicColor.Add("Amarillo", "Amarillo");
            DicColor.Add("Azul","Azul");
            DicColor.Add("Beige","Beige");
            DicColor.Add("Blanco","Blanco");
            DicColor.Add("Bordó","Bordó");
            DicColor.Add("Bronce","Bronce");
            DicColor.Add("eleste","eleste");
            DicColor.Add("hampagne","hampagne");
            DicColor.Add("Gris","Gris");
            DicColor.Add("Marrón","Marrón");
            DicColor.Add("Naranja","Naranja");
            DicColor.Add("Negro","Negro");
            DicColor.Add("Ocre","Ocre");
            DicColor.Add("Plateado","Plateado ");
            DicColor.Add("Rojo","Rojo");
            DicColor.Add("Rosa","Rosa");
            DicColor.Add("Verde","Verde");
            DicColor.Add("Violeta", "Violeta");


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
        public static void CleanAllDiccionary() {

            DicCombustibles.Clear();
            DicTransmision.Clear();
            DicDireccion.Clear();
            DicNumPuertas.Clear();
            DicCarrocerias.Clear();
            DicColor.Clear();
            DicCarroceriasMotos.Clear();

        }

        /// <summary>
        ///´Proceso que toma los datos y los pasa a la api DeMotores para su publicación. de acuerdo al idusuario designado.
        /// </summary>
        /// <returns></returns>
        public string InsertarPublicacion() {

            string result ="";

            //var data = "vehicleType=CAR&brand=Suzuki&model=Aerio&version=GLX&year=2008&fuel=Bencina&transmission=Manual&steering=Asistida&doors=4&segment=Sedán&color=Otro Color&mileage=150000&price=5800000&subtitle=Suzuki Aerio GLX 2008&description=Un+modelo+pionero+que+nunca+perdi%C3%B3+su+liderazgo&provider=GRUPO_CCA&key=3453ceda832a83687d0905c2fcdfbe9c&userId=11053485&currency=CLP&image[0]=http://chileautos.li.csnstatic.com/chileautos/auto/particular/ap5655451225218551194.jpg&providerVehicleId=4050906";
            //string url = "http://www.demotores.cl/frontend/rest/post.service";
            //string responseString;
            //
            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //
            //    var action = Uri.EscapeUriString(url);
            //
            //    var content = new StringContent(data.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            //    
            //    var response = client.PostAsync(action, content).Result;
            //    var responseContent = response.Content;
            //    responseString = responseContent.ReadAsStringAsync().Result;
            //
            //    if (response.IsSuccessStatusCode)
            //    {
            //        result = responseString;
            //    }
            //    else
            //    {
            //        result = responseString;
            //    }
            //
            //}

            ProveedorDatos PrDatos = new ProveedorDatos();

            var datosvehiculo = PrDatos.EntregaDatosVehiculo("2704599");

            CargaDiccionarios();

            string prova = ConsultaString(DicColor, "Rojo Vermellón");

            CleanAllDiccionary();

            return result;
        }


        public static string ConsultaString(Dictionary<string,string> destino, string compara) {
            
            

            string resultado = "Otro Color";

            int cost = 0;

            foreach (KeyValuePair<string, string> a in destino)
            {
                cost = LevenshteinDistance.Compute(a.Key, compara);

                if (cost == 0)
                {

                    resultado = a.Key;

                }

            }

            return resultado;

        }


        /// <summary>
        /// Logra la major aproximación entre dos arreglos.
        /// </summary>
        static class LevenshteinDistance
        {
            /// <summary>
            /// Comprara la distancia entre dos arreglos, es decir, cuantas veces debe un arreglo ser modificado para que sean iguales, mientras menos modificaciones, más ´similar es. Si el resultado es cero, son iguales.
            /// </summary>
            public static int Compute(string s, string t)
            {
                int n = s.Length;
                int m = t.Length;
                int[,] d = new int[n + 1, m + 1];

                // Step 1
                if (n == 0)
                {
                    return m;
                }

                if (m == 0)
                {
                    return n;
                }

                // Step 2
                for (int i = 0; i <= n; d[i, 0] = i++)
                {
                }

                for (int j = 0; j <= m; d[0, j] = j++)
                {
                }

                // Step 3
                for (int i = 1; i <= n; i++)
                {
                    //Step 4
                    for (int j = 1; j <= m; j++)
                    {
                        // Step 5
                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                        // Step 6
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost);
                    }
                }
                // Step 7
                return d[n, m];
            }
        }

    }
}