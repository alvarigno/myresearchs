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

namespace WpfAppProcessWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        VMmain VM;

        public MainWindow()
        {
            InitializeComponent();

            VM = new VMmain();
            this.DataContext = VM; //asignacion del viewmodel

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            VM.PantallaActiva = false;
            VM.VerMensaje = true;
            await haceAlgo();
            VM.VerMensaje = false;
            VM.PantallaActiva = true;

        }

        internal async Task HaceUnProcesoPesado()
        {

            await haceAlgo();

        }

        internal async Task haceAlgo()
        {
            await Task.Run(() => System.Threading.Thread.Sleep(5000));
        }


    }

}
