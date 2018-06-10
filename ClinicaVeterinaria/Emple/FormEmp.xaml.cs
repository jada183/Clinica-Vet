using ClinicaVeterinaria.DAL;
using ClinicaVeterinaria.MODEL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
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

namespace ClinicaVeterinaria.Emple
{
    /// <summary>
    /// Lógica de interacción para FormEmp.xaml
    /// </summary>
    public partial class FormEmp : Window
    {

        
        Empleado em;//empleado local
        bool NuevoEmp = false;//cambia segun venga de nuevo empleado o empleado seleccionado
        MainWindow main;//la mainwindows local
        bool modificado = false;//comprueba que el empleado fue modificado correctamente para no reinicializar los valores
        //variables que recuperar cuando  la modificacion no se realiza correctamente
       
        string NombreOriginal = "";
        string ApellidosOrigina = "";
        string TipoOriginal="";
        string TitulacionOriginal = "";
        string DireccionOriginal = "";
        string TelefonoOriginal = "";
        string MovilOriginal = "";
        string EmailOriginal = "";
        string UsuarioOriginal = "";
        string contraseñaOriginal = "";

        //variables para horario
        private Horario NuevoHorario = new Horario();
        private List<Horario> Horarios = new List<Horario>();
        private Horario HorSelect = new Horario();
        private Horario horarioGuardar = new Horario();
        public FormEmp(Empleado emp, MainWindow mw)
        {
            InitializeComponent();
            em = emp;//el producto que paso por parametro lo asigno a una variable local
            GuardarValoresEmpEntrada();
            main = mw;//asigno a una variable local la main window que paso por parametro
           

            gridEmpleadoSelect.DataContext = em;
            if (em.Usuario == null)
            {
                btEliminarEmp.Visibility = Visibility.Hidden;
                tabHorario.Visibility = Visibility.Hidden;
                NuevoEmp = true;
            }
            else
            {
                btGuardarEmp.Content = "modificar";


            }
            //@@@@@@@ una causa por la que falla el borrado
            //para los horarios
            try
            {
                CargardgHorarios(MainWindow.uow.RepositorioHorario.obtenerVarios(c => c.EmpleadoId == em.EmpleadoId));
            }
            catch { }
            

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
        #region perfil

        public void GuardarValoresEmpEntrada()
        {
            NombreOriginal = em.Nombre;
            ApellidosOrigina = em.Apellidos;
            TipoOriginal = em.Tipo;
            TitulacionOriginal = em.Titulacion;
            DireccionOriginal = em.Direccion;
            TelefonoOriginal = em.Telefono;
            MovilOriginal = em.Movil;
            EmailOriginal = em.Email;
            UsuarioOriginal = em.Usuario;
            contraseñaOriginal = em.Contraseña;
        }
        public void RecuperarValoresEmpEntrada()
        {
            em.Nombre = NombreOriginal;
            em.Apellidos = ApellidosOrigina;
            em.Tipo = TipoOriginal;
            em.Titulacion = TitulacionOriginal;
            em.Direccion = DireccionOriginal;
            em.Telefono = TelefonoOriginal;
            em.Movil = MovilOriginal;
            em.Email = EmailOriginal;
            em.Usuario = UsuarioOriginal;
            em.Contraseña = contraseñaOriginal;
        }

       

        private void BtGuardarEmp_Click(object sender, RoutedEventArgs e)
        {
            em.Habilitado = true;
            if (Validado(em)) { 
                if (NuevoEmp)
                {
                    Empleado aux = new Empleado();
                    aux = MainWindow.uow.RepositorioEmpleado.obtenerUno(c => c.Usuario == em.Usuario && c.Habilitado==true);//para comprobar que no existe ningun usuario con ese nombre
                    if(aux==null)
                    {
                        if (tbContraseñaEmp.Text == tbConfirmContraseñaEmp.Text)
                        {
                            try
                            {

                                MainWindow.uow.RepositorioEmpleado.crear(em);
                                MessageBox.Show("se ha guardado correctamente el empleado");
                                GuardarValoresEmpEntrada();
                                modificado = true;
                                main.CargardgEmpleado(MainWindow.uow.RepositorioEmpleado.obtenerVarios(c=>c.Habilitado==true));
                                this.Close();
                            }
                            catch 
                            {

                                MessageBox.Show("no se puede guardar en empleado por algun dato mal introducido");
                           

                            }
                       
                        }
                        //cuando la confirmacion de contraseña no coincide con la primera contraseña
                        else
                        {
                            MessageBox.Show("las contraseñas no coinciden");
                            tbConfirmContraseñaEmp.Text = "";

                        }
                    }
                    //cuando existe un empleado con ese nombre de usuario
                    else
                    {
                        MessageBox.Show("existe un empleado con ese nombre de usuario");
                        RecuperarValoresEmpEntrada();
                        tbUsuarioEmp.Text = "";
                    }
                }
                //cuando se modifica un empleado
                else
                {
                    Empleado aux = new Empleado();
                    aux = MainWindow.uow.RepositorioEmpleado.obtenerUno(c => c.Usuario == em.Usuario && c.EmpleadoId != em.EmpleadoId && c.Habilitado==true);//para comprobar que no existe ningun usuario con ese nombre
                    if (aux == null)
                    {
                        if (tbContraseñaEmp.Text == tbConfirmContraseñaEmp.Text)
                        {
                            try
                            {
                                MainWindow.uow.RepositorioEmpleado.actualizar(em);
                                MessageBox.Show("se ha modificado correctamente el empleado");
                                GuardarValoresEmpEntrada();
                                modificado = true;
                                main.CargardgEmpleado(MainWindow.uow.RepositorioEmpleado.obtenerVarios(c=>c.Habilitado==true));
                                this.Close();
                            }
                            catch
                            {
                                MessageBox.Show("no se ha podido modificar el empleado");
                            }
                       
                        }
                        //cuando la confirmacion de contraseña no coincide con la primera contraseña
                        else
                        {
                            MessageBox.Show("las contraseñas no coinciden");
                            tbConfirmContraseñaEmp.Text = "";
                        }
                    }
                    //cuando existe un empleado con ese nombre de usuario
                    else
                    {
                        MessageBox.Show("existe un empleado con ese nombre de usuario o  algun dato tiene un formato incorrecto");
                        RecuperarValoresEmpEntrada();
                        tbUsuarioEmp.Text = "";
                    }
                }
            }
            else { }
        }

        private void BtEliminarEmp_Click(object sender, RoutedEventArgs e)
        {
            if (em.EmpleadoId != 1)
            {
                List<Cita> lcita = MainWindow.uow.RepositorioCita.obtenerVarios(c => c.EmpleadoId == em.EmpleadoId && c.Atendida == false);
                if (lcita.Count > 0)
                {
                    MessageBox.Show("cuidado vas a eliminar un empleado con una cita sin atender");
                }
                try
                {
                    string messageBoxText = "Estas seguro que deseas eliminar este empleado?";
                    string caption = "Word Processor";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                           
                            dgHorario.ItemsSource = "";
                            em.Habilitado = false;
                            MainWindow.uow.RepositorioEmpleado.actualizar(em);
                            MainWindow.uow.RepositorioHorario.eliminarVarios(c => c.EmpleadoId == em.EmpleadoId);
                            MainWindow.uow.RepositorioCita.eliminarVarios(c => c.EmpleadoId == em.EmpleadoId && c.Atendida == false);
                            main.CargardgEmpleado(MainWindow.uow.RepositorioEmpleado.obtenerVarios(c=>c.Habilitado==true));                          
                            main.CargardgCitas(MainWindow.uow.RepositorioCita.obtenerVarios(c => c.Atendida == false));
                            main.CargardgCitasAtendidas(MainWindow.uow.RepositorioCita.obtenerVarios(c => c.Atendida == true));
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
                    MessageBox.Show("Error no se ha podido borrar este empleado");
                }
            }
            else
            {
                MessageBox.Show("el empleado administrador no se puede borrarr");
            }
        }
        #endregion
        #region horarios
        //metodos
        //hace visible el grid con el formulario para crear un nuevo horario
        public void CargaNuevoHorario()
        {

            gridNuevoHorario.Visibility = Visibility.Visible;
            NuevoHorario = new Horario();
            gridNuevoHorario.DataContext = NuevoHorario;

        }
        public void ActualizarComboBoxHorario()
        {
            //para poner los combo de los horarios nuevos en la posicion inicial
            cbDiaHorNuevo.SelectedIndex = 0;
            cbHoraFNuevoHor.SelectedIndex = 0;
            cbHoraINuevoHor.SelectedIndex = 0;
        }
        public void CargarHorarios()
        {
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 60; j = j + 30)
                {
                    if (j == 0)
                    {
                        cbHoraINuevoHor.Items.Add(Convert.ToString(i) + ":" + Convert.ToString(j) + "0");
                    }
                    else
                    {
                        cbHoraINuevoHor.Items.Add(Convert.ToString(i) + ":" + Convert.ToString(j));
                    }

                }
            }
            cbHoraINuevoHor.SelectedItem = 0;
        }
        public void CargardgHorarios(List<Horario> h)
        {
            try
            {
                Horarios = h;
                dgHorario.ItemsSource = Horarios;
            }
            catch { }
        }
        public void LimpiarGridNuevoHorario()
        {
            gridNuevoHorario.Visibility = Visibility.Hidden;
            ActualizarComboBoxHorario();


        }
        //eventos
        private void BtNuevoHorario_Click(object sender, RoutedEventArgs e)
        {
            CargarHorarios();
            CargaNuevoHorario();
            ActualizarComboBoxHorario();
        }

        private void BtGuardarHor_Click(object sender, RoutedEventArgs e)
        {

           

            try
            {
                string horaInGuardar = NuevoHorario.HoraInic;
                string horaFnGuardar = NuevoHorario.HoraFin;
                string diaGuardar = NuevoHorario.Dia;
                horarioGuardar = new Horario();
                //para evitar problemas con el binding del horarionuevo con el formulario
                horarioGuardar.Dia = diaGuardar;
                horarioGuardar.HoraFin = horaFnGuardar;
                horarioGuardar.HoraInic = horaInGuardar;

                horarioGuardar.EmpleadoId = em.EmpleadoId;
                //horarioGuardar.Empleado = em;
                if (Validado(horarioGuardar))
                {
                    MainWindow.uow.RepositorioHorario.crear(horarioGuardar);
                    MessageBox.Show("se ha guardado correctamente el horario");
                    NuevoHorario = new Horario();
                    horarioGuardar = new Horario();
                    CargardgHorarios(MainWindow.uow.RepositorioHorario.obtenerVarios(c => c.EmpleadoId == em.EmpleadoId));
                    LimpiarGridNuevoHorario();
                    LimpiarComboBox();

                }
                else { }
              
            }
            catch
            {

                MessageBox.Show("no se ha podido guardar un nuevo horario por falta de algun campo algun dato mal introducido");
            }

        }
        private void BtCancelarNuevoHor_Click(object sender, RoutedEventArgs e)
        {
            LimpiarGridNuevoHorario();
            LimpiarComboBox();
        }
        private void DgHorario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HorSelect = (Horario)(dgHorario.SelectedItem);
            }
            catch
            {

            }
        }

        private void BtEliminarHorario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar este horario?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MainWindow.uow.RepositorioHorario.eliminar(HorSelect);
                        CargardgHorarios(MainWindow.uow.RepositorioHorario.obtenerVarios(c => c.EmpleadoId == em.EmpleadoId));
                        cbBuscarListHorarios.SelectedIndex = 0;
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
                MessageBox.Show("seleccione un horario");
            }
        }
        private void BtBuscarListHorarios_Click(object sender, RoutedEventArgs e)
        {
            if (cbBuscarListHorarios.Text == "Todos")
            {
                CargardgHorarios(MainWindow.uow.RepositorioHorario.obtenerVarios(c => c.EmpleadoId == em.EmpleadoId));

            }
            else if (cbBuscarListHorarios.Text == "lunes" || (cbBuscarListHorarios.Text == "martes") || (cbBuscarListHorarios.Text == "miércoles") || (cbBuscarListHorarios.Text == "jueves") || (cbBuscarListHorarios.Text == "viernes")
                 || (cbBuscarListHorarios.Text == "sábado"))
            {
                try
                {
                    CargardgHorarios(MainWindow.uow.RepositorioHorario.obtenerVarios(c => c.Dia == cbBuscarListHorarios.Text && c.EmpleadoId == em.EmpleadoId));
                }
                catch { }
            }
        }
        //carga de primer comboBox
        private void cbHoraINuevoHor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbHoraINuevoHor.Text != "" && cbHoraINuevoHor.Text != null)
            {


                cbHoraFNuevoHor.IsEnabled = true;
               
               
            }
        }
        //carga de segundo comboBox
        private void CbHoraFNuevoHor_DropDownOpened(object sender, EventArgs e)
        {
            cbHoraFNuevoHor.Items.Clear();
            cbHoraFNuevoHor.SelectedIndex = 0;
            String[] horayminHorInicial = new String[2];
            horayminHorInicial = cbHoraINuevoHor.Text.Split(':');
            cbHoraFNuevoHor.Items.Clear();
            for (int i = Convert.ToInt32(horayminHorInicial[0]); i < 24; i++)
            {
                for (int j = Convert.ToInt32(horayminHorInicial[1]); j < 60; j = j + 30)
                {
                    if (j == 0)
                    {
                        cbHoraFNuevoHor.Items.Add(Convert.ToString(i) + ":" + Convert.ToString(j) + "0");
                    }
                    else
                    {
                        cbHoraFNuevoHor.Items.Add(Convert.ToString(i) + ":" + Convert.ToString(j));
                    }

                }
                horayminHorInicial[1] = "0";
            }
            cbHoraFNuevoHor.SelectedItem = 0;
        }
        public void LimpiarComboBox()
        {
            cbHoraFNuevoHor.Items.Clear();
            cbHoraINuevoHor.Items.Clear();
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado == false)
            {
                RecuperarValoresEmpEntrada();
            }
            main.CargardgEmpleado(MainWindow.uow.RepositorioEmpleado.obtenerVarios(c=>c.Habilitado==true));
            
            this.Close();
        }

       

       

        
    }
}
