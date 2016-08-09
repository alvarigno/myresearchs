using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using UpLoadServicesRestWebApiModel;

namespace UpLoadServicesRestWebApi.Controllers
{
    public class DocumentosController : ApiController
    {
        myConnection myConn = new myConnection();
        List<Documentos> mio2 = new List<Documentos>();

        public List<Documentos> get(int estado, int sitio)
        {

            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();

                string query = connection.CommandText;
                connection.CommandText = "select * from dbo.documentos where estado =" + estado+" and sitio="+sitio;

                using (var reader = connection.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Documentos mio = new Documentos();
                        mio.fnombre = reader["fnombre"].ToString();
                        mio.estado = int.Parse(reader["estado"].ToString());
                        mio.fecha_insert = DateTime.Parse(reader["fecha_insert"].ToString());
                        mio.idemail = reader["idemail"].ToString();
                        mio.id_num = int.Parse(reader["id_num"].ToString());
                        mio.sitio = int.Parse(reader["sitio"].ToString());

                        //mio.fnombre = "hola";
                        //mio.estado = 1;
                        //mio.fecha_insert = DateTime.Parse(reader["fecha_insert"].ToString());
                        //mio.idemail = null;
                        //mio.id_num = 1;
                        //mio.sitio = 1;

                        mio2.Add(mio);

                    }
                }

                connection.Connection.Close();

            }

            return mio2;

        }


        public List<Documentos> get(string nombrearchivo)
        {

            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();

                string query = connection.CommandText;
                connection.CommandText = "select * from dbo.documentos where fnombre = '" + nombrearchivo + "'";
                
                using (var reader = connection.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Documentos mio = new Documentos();
                        mio.fnombre = reader["fnombre"].ToString();
                        mio.estado = int.Parse(reader["estado"].ToString());
                        mio.fecha_insert = DateTime.Parse(reader["fecha_insert"].ToString());
                        mio.idemail = reader["idemail"].ToString();
                        mio.id_num = int.Parse(reader["id_num"].ToString());
                        mio.sitio = int.Parse(reader["sitio"].ToString());

                        //mio.fnombre = "hola";
                        //mio.estado = 1;
                        //mio.fecha_insert = DateTime.Parse(reader["fecha_insert"].ToString());
                        //mio.idemail = null;
                        //mio.id_num = 1;
                        //mio.sitio = 1;

                        mio2.Add(mio);

                    }
                }

                connection.Connection.Close();

            }

            return mio2;

        }
    }
}