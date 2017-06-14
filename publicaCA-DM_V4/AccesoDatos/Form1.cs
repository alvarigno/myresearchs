using AccesoDatos.Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccesoDatos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static string[] GetDataAccess(string nomauto)
        {

            baseprod2Entities baseprod = new baseprod2Entities();

            ObjectParameter nom = new ObjectParameter("usr", typeof(string));
            ObjectParameter pass = new ObjectParameter("pass", typeof(string));
            string[] logrado = new string[2];

            var datosresultados = baseprod.SP_Datos_Usuario_CADM(nomauto, nom, pass);
            logrado[0] = nom.Value.ToString();
            logrado[1] = pass.Value.ToString();


            return logrado;
        }

    }
}
