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
namespace ClinicaVeterinaria.Clie
{
    /// <summary>
    /// Lógica de interacción para FormCli.xaml
    /// </summary>
    public partial class FormCli : Window
    {
        UnityOfWork uow;
        Cliente cli = new Cliente();//empleado local
        bool NuevoCli = false;//cambia segun venga de nuevo empleado o empleado seleccionado
        MainWindow main;//la mainwindows local
        bool modificado = false;//comprueba que el empleado fue modificado correctamente para no reinicializar los valores
        //variables que recuperar cuando  la modificacion no se realiza correctamente
        string nombreOriginalCli = "";
        string apellidosOriginalCli = "";
        string direccionOriginalCli = "";
        string telefonoOriginalCli = "";
        string movilOriginalCli = "";
        string emailOriginalCli = "";

        //Para las mascotas
        List<Paciente> mascotas = new List<Paciente>();
        Paciente masSelect = new Paciente();
        UnityOfWork uow2 = new UnityOfWork();

        public FormCli(Cliente cl, UnityOfWork uw, MainWindow mw)
        {
            InitializeComponent();
            cli = cl;
            main = mw;
            uow = uw;
            GuardarValoresCliEntrada();
            gridClienteSelect.DataContext = cli;
            if (cli.Nombre == null)
            {
                btEliminarCli.Visibility = Visibility.Hidden;
                NuevoCli = true;
                tabMascotas.Visibility = Visibility.Hidden;
            }
            else
            {
                btGuardarCli.Content = "modificar";


            }
            //para mascotas
            CargarDgMascotas(uow2.RepositorioPaciente.obtenerVarios(c => cli.ClienteId == c.ClienteId));
        }
        #region Cliente
       
        private void BtGuardarCli_Click(object sender, RoutedEventArgs e)
        {
            if (NuevoCli)
            {
                Cliente aux = new Cliente();
                aux = uow.RepositorioCliente.obtenerUno(c => c.Email == cli.Email);//para comprobar que no existe ningun cliente con ese correo
                if (aux == null)
                {
                   
                        try
                        {
                            UnityOfWork uowaux = new UnityOfWork();
                            uowaux.RepositorioCliente.crear(cli);
                            MessageBox.Show("se ha guardado correctamente el Cliente");                           
                            modificado = true;
                            main.CargardgCliente(uow.RepositorioCliente.obtenerTodos());
                            GuardarValoresCliEntrada();
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Error falta algun campo obligatorio por cubrir o  algun dato tiene un formato incorrecto");
                            RecuperarValoresCliEntrada();
                        }

                    
                 
                    
                }
                //cuando existe un  cliente con ese correo
                else
                {
                    MessageBox.Show("existe un cliente con ese nombre de email");
                    RecuperarValoresCliEntrada();
                    tbEmailCli.Text = "";
                }
            }
            //cuando se modifica un empleado
            else
            {
                Cliente aux = new Cliente();
                aux = uow.RepositorioCliente.obtenerUno(c => c.Email == cli.Email && c.ClienteId != cli.ClienteId);//para comprobar que no existe ningun cliente con ese correo
                if (aux == null)
                {
                   
                        try
                        {
                            uow.RepositorioCliente.actualizar(cli);
                            MessageBox.Show("se ha modificado correctamente el cliente");                           
                            modificado = true;
                            main.CargardgCliente(uow.RepositorioCliente.obtenerTodos());
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Error falta algun campo obligatorio por cubrir");
                            RecuperarValoresCliEntrada();
                        }

                   
                    
                }
                //cuando existe un empleado con ese nombre de usuario
                else
                {
                    MessageBox.Show("existe un cliente con ese email");
                    RecuperarValoresCliEntrada();
                    tbEmailCli.Text = "";
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado == false)
            {
                RecuperarValoresCliEntrada();
            }
            main.CargardgCliente(uow.RepositorioCliente.obtenerTodos());
        }

        private void RecuperarValoresCliEntrada()
        {
            cli.Nombre = nombreOriginalCli;
            cli.Apellidos = apellidosOriginalCli;
            cli.Direccion = direccionOriginalCli;
            cli.Telefono = telefonoOriginalCli;
            cli.Movil = movilOriginalCli;
            cli.Email = emailOriginalCli;
        }

        public void GuardarValoresCliEntrada()
        {
            nombreOriginalCli = cli.Nombre;
            apellidosOriginalCli = cli.Apellidos;
            direccionOriginalCli = cli.Direccion;
            telefonoOriginalCli = cli.Telefono;
            movilOriginalCli = cli.Movil;
            emailOriginalCli = cli.Email;
        }

        private void BtEliminarCli_Click(object sender, RoutedEventArgs e)
        {
            if (cli.ClienteId != 1)
            {
                try
                {
                    string messageBoxText = "Estas seguro que deseas eliminar este Cliente?";
                    string caption = "Word Processor";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:


                            uow.RepositorioCliente.eliminar(cli);
                            main.CargardgCliente(uow.RepositorioCliente.obtenerTodos());
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
                    MessageBox.Show("Error no se ha podido borrar este cliente");
                }
            }
            else
            {
                MessageBox.Show("el cliente por defecto no se puede borrarr");
            }
        }
        #endregion
        #region mascota
        public void CargarDgMascotas(List<Paciente> pac) {
            try
            {
                mascotas = pac;
                dgMascota.ItemsSource = pac;
            }
            catch { }
        }
        public void CargarVentanaFormMas(Paciente p)
        {
            FormMas fm = new FormMas(p,this,uow2);
            fm.Show();
        }
        //eventos
        private void dgMascota_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                masSelect = (Paciente)(dgMascota.SelectedItem);
            }
            catch
            {

            }
        }

        

        private void dgMascota_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                masSelect = (Paciente)(dgMascota.SelectedItem);
                CargarVentanaFormMas(masSelect);
            }
            catch
            {

            }
        }

        private void BtNuevaMascota_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Paciente p = new Paciente();
                p.ClienteId = cli.ClienteId;          
                CargarVentanaFormMas(p);

            }
            catch
            {

            }
        }

        private void BtEditarMas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                CargarVentanaFormMas(masSelect);

            }
            catch
            {

            }
        }
        #endregion
    }
}
