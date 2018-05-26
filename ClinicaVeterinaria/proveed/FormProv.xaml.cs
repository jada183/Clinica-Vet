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
using ClinicaVeterinaria.MODEL;
using ClinicaVeterinaria.DAL;
namespace ClinicaVeterinaria.Proveed
{
    /// <summary>
    /// Lógica de interacción para FormProv.xaml
    /// </summary>
    public partial class FormProv : Window
    {
        UnityOfWork uow;
        Proveedor pv = new Proveedor();//proveedor local
        bool NuevoProv = false;//cambia segun venga de nuevo proveedor o proveedor seleccionado
        MainWindow main;//la mainwindows local
        bool modificado = false;//comprueba que el proveedor fue modificado correctamente para no reinicializar los valores
        //variables del proveedor al entrar en la ventana
        string nombreOriginal = "";
        string apellidosOriginal = "";
        string telefonoOriginal = "";
        string movilOriginal = "";
        string direccionOriginal = "";
        string emailOriginal = "";

        public FormProv(Proveedor prov,UnityOfWork uw,MainWindow mw)
        {
            InitializeComponent();
            uow = uw;
            main = mw;
            pv = prov;
            GuardarValoresProvEntrada();
            gridProv.DataContext = pv;
            if (prov.ProveedorId == 0)
            {
                BtEliminarProv.Visibility = Visibility.Hidden;
                NuevoProv = true;
            }
            else
            {
                BtGuardarProv.Content = "modificar";


            }
        }
       
        private void BtGuardarProv_Click(object sender, RoutedEventArgs e)
        {
            //nuevo producto
            if (NuevoProv)
            {
                Proveedor provAux = new Proveedor();
                provAux = uow.RepositorioProveedor.obtenerUno(c => c.Email == pv.Email);
                if (provAux == null)
                {
                    try
                    {
                        UnityOfWork uowaux = new UnityOfWork();
                        uowaux.RepositorioProveedor.crear(pv);
                        MessageBox.Show("se ha guardado correctamente el Proveedor");
                        modificado = true;
                        main.CargardgProveedor(uow.RepositorioProveedor.obtenerTodos());

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                        
                    }
                }
                else
                {
                    MessageBox.Show("existe un proveedor con el mismo email");
                    tbEmailProv.Text = "";
                }
            }
            //modificar
            else
            {
                Proveedor provAux = new Proveedor();
                provAux = uow.RepositorioProveedor.obtenerUno(c => c.Email==pv.Email && c.ProveedorId!=pv.ProveedorId);
                if (provAux == null)
                {
                    try
                    {

                        uow.RepositorioProveedor.actualizar(pv);
                        MessageBox.Show("se ha modificado correctamente el proveedor");
                        modificado = true;
                        main.CargardgProveedor(uow.RepositorioProveedor.obtenerTodos());

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                        RecuperarValoresProvEntrada();
                    }
                }
                else
                {
                    MessageBox.Show("existe un producto con el mismo nombre y marca ya registrado");
                    RecuperarValoresProvEntrada();

                }
            }
        }
        public void GuardarValoresProvEntrada()
        {
            nombreOriginal = pv.Nombre;
            apellidosOriginal = pv.Apellidos;
            telefonoOriginal = pv.Telefono;
            movilOriginal = pv.Movil;
            direccionOriginal = pv.Direccion;
            emailOriginal = pv.Email;
        }
        public void RecuperarValoresProvEntrada()
        {

            pv.Nombre = nombreOriginal;
            pv.Apellidos = apellidosOriginal;
            pv.Telefono = telefonoOriginal;
            pv.Movil = movilOriginal;
            pv.Direccion = direccionOriginal;
            pv.Email = emailOriginal;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado == false)
            {
                RecuperarValoresProvEntrada();
            }
            main.CargardgProveedor(uow.RepositorioProveedor.obtenerTodos());
        }

        private void BtEliminarProv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar este proveedor?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:


                        uow.RepositorioProveedor.eliminar(pv);
                        main.CargardgProveedor(uow.RepositorioProveedor.obtenerTodos());
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
                MessageBox.Show("Error no se ha podido borrar este proveedor");
            }
        }
    }
}
