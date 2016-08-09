using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using UpLoadServicesRestWebApiModel;

namespace UpLoadServicesRestWebApi.Controllers
{
    public class AutomotoraController : ApiController
    {
        myConnection myConn = new myConnection();
        string mensajeVerificaSitioCodAutomotora;
        string mensajeVerificaSitio;


        public string create(Automotora automotora)
        {
            bool VerificaCreate = false;
            string mensaje = "";

            if (VerificaSitio(automotora.uid_sitio) && VerificaSitioCodAutomotora(automotora.uid_sitio, automotora.cod_original))
            {

                SqlCommand mycommand = new SqlCommand();

                mycommand.CommandType = System.Data.CommandType.Text;
                mycommand.Connection = myConnection.GetConnection();

                mycommand.CommandText = "INSERT INTO[dbo].[tbl_mp_automotoras] ([uid_sitio] ,[cod_original] ,[automotora] ,[email]) VALUES (" + automotora.uid_sitio + " ," + automotora.cod_original + " ,'" + automotora.automotora + "','" + automotora.email + "')";

                int a = mycommand.ExecuteNonQuery();
                mycommand.Connection.Close();

                //int a = 0;
                if (a <= 0)
                {
                    //Not updated.
                    VerificaCreate = false;
                    mensaje = "¡El número de Sitio o Codigo de automotora son incorrectos!"+ mensajeVerificaSitioCodAutomotora + mensajeVerificaSitio;
                }
                else
                {
                    VerificaCreate = true;
                    mensaje = "¡Registro creado de forma exitosa!";
                }
            }
            else {

                mensaje = "¡Favor de verificar el número de Sitio o Codigo de automotora existe o son incorrectos!" + mensajeVerificaSitioCodAutomotora + mensajeVerificaSitio;

            }

            return mensaje;

        }


        public bool VerificaSitio(int input) {

            bool verifica1 = false;
            int b = 0;
            SqlCommand mycommand2 = new SqlCommand();

            mycommand2.CommandType = System.Data.CommandType.Text;
            mycommand2.Connection = myConnection.GetConnection();

            mycommand2.CommandText = "select count(*) from [dbo].[tbl_mp_sitio] where uid_sitio = "+input;

            b = (Int32) mycommand2.ExecuteScalar();
            mycommand2.Connection.Close();

            //int b = 0;
            if (b <= 0)
            {
                //Not updated.
                verifica1 = false;
                mensajeVerificaSitio = "\n Código Sitio: "+input+", no existe.";
            }
            else
            {
                verifica1 = true;
            }


            return verifica1;

        }

        public bool VerificaCodOriginal(int input)
        {

            bool verifica1 = false;
            int b = 0;
            SqlCommand mycommand2 = new SqlCommand();

            mycommand2.CommandType = System.Data.CommandType.Text;
            mycommand2.Connection = myConnection.GetConnection();

            mycommand2.CommandText = "select count(*) from dbo.tbl_mp_automotoras where cod_original = "+input;

            b = (Int32)mycommand2.ExecuteScalar();
            mycommand2.Connection.Close();

            //int b = 0;
            if (b <= 0)
            {
                //Not updated.
                verifica1 = false;
            }
            else
            {
                verifica1 = true;
            }


            return verifica1;

        }

        public bool VerificaSitioCodAutomotora(int sitio, int codoriginal) {

            bool VerificaSitioCodAutomotora = false;

            int b = 0;
            SqlCommand mycommand2 = new SqlCommand();

            mycommand2.CommandType = System.Data.CommandType.Text;
            mycommand2.Connection = myConnection.GetConnection();

            mycommand2.CommandText = "select count(*) from dbo.tbl_mp_automotoras where cod_original ="+ codoriginal+" and uid_sitio ="+ sitio;

            b = (Int32)mycommand2.ExecuteScalar();
            mycommand2.Connection.Close();

            //int b = 0;
            if (b == 0)
            {
                //Not updated.
                VerificaSitioCodAutomotora = true;
            }
            else
            {
                VerificaSitioCodAutomotora = false;
                mensajeVerificaSitioCodAutomotora = "\n Donde Código automotora: "+codoriginal+" y Código Sitio: "+sitio;
            }
            
            return VerificaSitioCodAutomotora;

        }



    }
}