using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProcesaDocumentos
{
    public class myConnection
    {
        private SqlConnection _con;


        public SqlConnection Conexion()
        {
            string strCadenaConexion = ConfigurationManager.ConnectionStrings["bdMailParser"].ToString();

            _con = new SqlConnection(strCadenaConexion);
            return _con;
        }


        public void Abrir()
        {
            _con.Open();

        }

        public void Cerrar()
        {
            _con.Close();
        }

    }
}