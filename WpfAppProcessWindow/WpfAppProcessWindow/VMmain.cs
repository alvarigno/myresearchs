using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices; 

namespace WpfAppProcessWindow
{
    class VMmain : INotifyPropertyChanged
    {
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public VMmain()
        {

            PantallaActiva = true;
        }

        private bool _PantallaActiva;

        public bool PantallaActiva
        {
            get { return _PantallaActiva; }
            set { _PantallaActiva = value; RaisePropertyChanged(); }
        }


        private bool _VerMensaje;
        private string _Vermensajetext;

        public bool VerMensaje
        {
            get { return _VerMensaje; }
            set { _VerMensaje = value; RaisePropertyChanged(); }

        }

    }
}
