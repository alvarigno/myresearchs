using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using mshtml;
using System.Net;
using System.Security.Permissions;
using HtmlAgilityPack;
using System.Xml;

namespace WpfAppBrowser
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            myWebBrowser.Address = "http://www.macal.cl/Loteo.aspx";
           
        }

        public void LoadHTML() {


            var doc = new HtmlDocument();
            var url = myWebBrowser.Address;
            var web = new HtmlWeb();
            //doc = web.Load(myWebBrowser.Address);


            string HTML = myWebBrowser.GetSourceAsync().Result;
            doc.LoadHtml(HTML);
            ObtieneImagenes(doc);

        }

        private void btncarga_Click(object sender, RoutedEventArgs e)
        {
            LoadHTML();
        }

        public List<string> ObtieneImagenes(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> listado = new List<string>();
            var query = localdoc.DocumentNode.Descendants("a")
            .Where(c => (c.Attributes["rel"] != null))
            .Select(c => c.ChildAttributes("href").FirstOrDefault().Value);

            listado = query.ToList();
            listado.RemoveAt(0);

            return listado;
        }

    }
}
