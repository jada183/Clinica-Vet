using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        Cliente cli;//empleado local
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
        List<Paciente> mascotas;
        Paciente masSelect;
        //public static UnityOfWork uow2 = new UnityOfWork();

        public FormCli(Cliente cl, MainWindow mw)
        {
            InitializeComponent();
            cli = cl;
            main = mw;
            
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
            CargarDgMascotas(MainWindow.uow.RepositorioPaciente.obtenerVarios(c => cli.ClienteId == c.ClienteId && c.Habilitado==true));
            
        }
        private Boolean Validado(Object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj, null, null);
            List<System.ComponentModel.DataAnnotations.ValidationResult> errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(obj, validationContext, errors, true);

            if (errors.Count() > 0)
            {

                string mensageErrores = string.Empty;
                foreach (var error in errors)
                {
                    error.MemberNames.First();

                    mensageErrores += error.ErrorMessage + Environment.NewLine;
                }
                System.Windows.MessageBox.Show(mensageErrores); return false;
            }
            else
            {
                return true;
            }
        }
        #region Cliente

        private void BtGuardarCli_Click(object sender, RoutedEventArgs e)
        {
            cli.Habilitado = true;
            if (Validado(cli)) { 
                if (NuevoCli)
                {
                    Cliente aux;
                    aux = MainWindow.uow.RepositorioCliente.obtenerUno(c => c.Email == cli.Email && c.Habilitado==true);//para comprobar que no existe ningun cliente con ese correo
                    if (aux == null)
                    {
                   
                            try
                            {

                                MainWindow.uow.RepositorioCliente.crear(cli);
                                MessageBox.Show("se ha guardado correctamente el Cliente");                           
                                modificado = true;
                                main.CargardgCliente(MainWindow.uow.RepositorioCliente.obtenerTodos());
                                
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
                    Cliente aux;
                    aux = MainWindow.uow.RepositorioCliente.obtenerUno(c => c.Email == cli.Email && c.ClienteId != cli.ClienteId && c.Habilitado==true);//para comprobar que no existe ningun cliente con ese correo
                    if (aux == null)
                    {
                   
                            try
                            {
                                MainWindow.uow.RepositorioCliente.actualizar(cli);
                                MessageBox.Show("se ha modificado correctamente el cliente");                           
                                modificado = true;
                                main.CargardgCliente(MainWindow.uow.RepositorioCliente.obtenerTodos());
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
            else { }
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
                    string messageBoxText = "Estas seguro que deseas eliminar este cliente con sus mascotas, algunas de estas podrian tener citas pendientes que se eliminaran?";
                    string caption = "Word Processor";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:

                            //eliminado en cascada
                            cli.Habilitado = false;
                            MainWindow.uow.RepositorioCliente.actualizar(cli);
                            List<Paciente> pac12 =MainWindow. uow.RepositorioPaciente.obtenerVarios(c => c.ClienteId == cli.ClienteId);

                            foreach (Paciente p in pac12)
                            {
                                p.Habilitado = false;
                                MainWindow.uow.RepositorioPaciente.actualizar(p);
                                MainWindow.uow.RepositorioCita.eliminarVarios(c => c.PacienteId == p.PacienteId && c.Atendida == false);
                                MainWindow.uow.RepositorioEstadoIngresado.eliminarVarios(c => c.PacienteId == p.PacienteId);
                            }

                            main.CargardgCitas(MainWindow.uow.RepositorioCita.obtenerVarios(c => c.Atendida == false));
                            main.CargardgIngresado(MainWindow.uow.RepositorioEstadoIngresado.obtenerTodos());

                            //recargo las tablas que lo necesiten
                            main.CargardgCliente(MainWindow.uow.RepositorioCliente.obtenerVarios(c => c.Habilitado == true));
                            main.CargarcbClientTPV();

                            break;
                        case MessageBoxResult.No:

                            break;
                        case MessageBoxResult.Cancel:
                            // User pressed Cancel button
                            // ...
                            break;
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.InnerException.Message);
                }
            }
            else
            {
                MessageBox.Show(" no se puede borrar el cliente por defecto");
            }
        }
        #endregion
        #region mascota
        public void CargarDgMascotas(List<Paciente> pac)
        {
            try
            {
                mascotas = pac;
                dgMascota.ItemsSource = pac;
            }
            catch { }
        }
        public void CargarVentanaFormMas(Paciente p)
        {
            FormMas fm = new FormMas(p,this);
            fm.ShowDialog();
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
                p.Propietario = cli;
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
        private void BtEliminarMascota_Click(object sender, RoutedEventArgs e)
        {
            List<Cita> lcita = MainWindow.uow.RepositorioCita.obtenerVarios(c => c.PacienteId == masSelect.PacienteId && c.Atendida == false);
            List<EstadoIngresado> lingresado = MainWindow.uow.RepositorioEstadoIngresado.obtenerVarios(c => c.PacienteId == masSelect.PacienteId);
            if (lcita.Count > 0)
            {
                MessageBox.Show("Cuidado vas a eliminar una mascota con una cita sin atender");
            }
            if (lingresado.Count > 0)
            {
                MessageBox.Show("Cuidado vas a eliminar una mascota que se encuentra ingresada");
            }
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar esta mascota?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        //elimino en cascada
                        masSelect.Habilitado = false;
                        MainWindow.uow.RepositorioPaciente.actualizar(masSelect);
                       
                        MainWindow.uow.RepositorioCita.eliminarVarios(c => c.PacienteId ==masSelect.PacienteId && c.Atendida==false);
                        MainWindow.uow.RepositorioEstadoIngresado.eliminarVarios(c => c.PacienteId == masSelect.PacienteId);
                        //recargo las tablas
                        CargarDgMascotas(MainWindow.uow.RepositorioPaciente.obtenerVarios(c => c.ClienteId == cli.ClienteId && c.Habilitado==true));                      
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

                MessageBox.Show("seleccione una mascota");
            }
        }

        private void BtBuscarMascota_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                masSelect= MainWindow.uow.RepositorioPaciente.obtenerUno(c => c.Nombre == tbBuscadorPac.Text && cli.ClienteId == c.ClienteId && c.Habilitado==true);
                CargarVentanaFormMas(masSelect);
            }
            catch
            {
            }
        }
        private void BtAbrirVacunas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vacunas vac = new Vacunas(masSelect);
                vac.ShowDialog();
            }
            catch
            {

            }
        }
        private void BtAbrirHistoriales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Historiales his = new Historiales(masSelect);
                his.ShowDialog();
            }
            catch
            {

            }
        }
        #endregion
        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado == false)
            {
                RecuperarValoresCliEntrada();
            }
            main.CargardgCliente(MainWindow.uow.RepositorioCliente.obtenerVarios(c=>c.Habilitado==true));
            main.CargardgCitas(MainWindow.uow.RepositorioCita.obtenerVarios(c => c.Atendida == false));
            main.CargardgCitasAtendidas(MainWindow.uow.RepositorioCita.obtenerVarios(c => c.Atendida == true));
            main.CargardgIngresado(MainWindow.uow.RepositorioEstadoIngresado.obtenerTodos());
        }

     
    }
}
