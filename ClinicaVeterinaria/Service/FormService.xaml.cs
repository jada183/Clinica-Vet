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
using System.Windows.Shapes;
using ClinicaVeterinaria.DAL;
using ClinicaVeterinaria.MODEL;
namespace ClinicaVeterinaria.Service
{
    /// <summary>
    /// Lógica de interacción para FormService.xaml
    /// </summary>
    public partial class FormService : Window
    {
        UnityOfWork uow;

        Servicio ser = new Servicio();//Servicio local
        bool NuevoServ = false;//cambia segun venga de nuevo producto o producto seleccionado
        bool modificado = false;//comprueba que el servicio ha sido modificado correctamente para que al cerrrar no restaure los valores por defecto
        MainWindow main = new MainWindow();
        //variables para guardar los datos que entran al abrir la ventana
        string serNombreOrigen = "";
        double serCosteOrigen;
        int serDuracionOrigen;
        string serDescripcionOrigen="";


        public FormService(Servicio serv, UnityOfWork uw, MainWindow mw)
        {
            InitializeComponent();
            ser = serv;
            GuardarrValoresServEntrada();
            main = mw;
            uow = uw;
            gridServicioSelect.DataContext = ser;
            //para identificar si es para crear un nuevo producto o para modificar uno existente
            if (ser.ServicioId == 0)
            {
                BtEliminarServ.Visibility = Visibility.Hidden;
                NuevoServ = true;
            }
            else
            {
                BtGuardarServ.Content = "modificar";


            }
        }


        private void BtGuardarServ_Click(object sender, RoutedEventArgs e)
        {
            if (NuevoServ)
            {
                Servicio servAux = new Servicio();
                servAux = uow.RepositorioServicio.obtenerUno(c => c.Nombre==ser.Nombre);
                if (servAux== null)
                {
                    try
                    {

                        uow.RepositorioServicio.crear(ser);
                        MessageBox.Show("se ha guardado correctamente el Servicio");
                        modificado = true;
                        main.CargardgServicio(uow.RepositorioServicio.obtenerTodos());

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                        RecuperarValoresServEntrada();
                    }
                }
                else
                {
                    MessageBox.Show("existe un servicio con el mismo nombre");
                    RecuperarValoresServEntrada();
                }
            }
            //modificar
            else
            {
                Servicio servAux = new Servicio();
                servAux = uow.RepositorioServicio.obtenerUno(c => c.Nombre == ser.Nombre  && c.ServicioId != ser.ServicioId);
                if (servAux == null)
                {
                    try
                    {

                        uow.RepositorioServicio.actualizar(ser);
                        MessageBox.Show("se ha modificado correctamente el producto");
                        modificado = true;
                        main.CargardgProductos(uow.RepositorioProducto.obtenerTodos());

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                        RecuperarValoresServEntrada();
                    }
                }
                else
                {
                    MessageBox.Show("existe un producto con el mismo nombre y marca ya registrado");
                    RecuperarValoresServEntrada();

                }
            }
        }

        public void GuardarrValoresServEntrada()
        {
            serNombreOrigen = ser.Nombre;
            serCosteOrigen = ser.CosteServicio;
            serDuracionOrigen=ser.Tiempo;
            serDescripcionOrigen = ser.Descripcion;

        }
        public void RecuperarValoresServEntrada()
        {

            ser.Nombre = serNombreOrigen;
            ser.CosteServicio = serCosteOrigen;
            ser.Tiempo = serDuracionOrigen;
            ser.Descripcion = serDescripcionOrigen;
        }

        private void BtEliminarServ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar este servicio?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:


                        uow.RepositorioServicio.eliminar(ser);
                        main.CargardgServicio(uow.RepositorioServicio.obtenerTodos());
                        this.Close();
                        break;
                    case MessageBoxResult.No:

                        break;
                    case MessageBoxResult.Cancel:
                        // User pressed Cancel button
                        // ...
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Error no se ha podido borrar este producto");
            }
        }
    }
}
