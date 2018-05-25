using ClinicaVeterinaria.DAL;
using ClinicaVeterinaria.MODEL;
using System;
using System.Collections.Generic;
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
        UnityOfWork uow;
        UnityOfWork uow2 = new UnityOfWork();//usada de forma local para los horarios

        Empleado em = new Empleado();//empleado local
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
        public FormEmp(Empleado emp, UnityOfWork uw, MainWindow mw)
        {
            InitializeComponent();
            em = emp;//el producto que paso por parametro lo asigno a una variable local
            GuardarValoresEmpEntrada();
            main = mw;//asigno a una variable local la main window que paso por parametro
            uow = uw;//la unity que deben tener en comun ambas ventanas

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
                CargardgHorarios(uow2.RepositorioHorario.obtenerVarios(c => c.EmpleadoId == em.EmpleadoId));
            }
            catch { }
            CargarHorarios();

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
            if (NuevoEmp)
            {
                Empleado aux = new Empleado();
                aux = uow.RepositorioEmpleado.obtenerUno(c => c.Usuario == em.Usuario);//para comprobar que no existe ningun usuario con ese nombre
                if(aux==null)
                {
                    if (tbContraseñaEmp.Text == tbConfirmContraseñaEmp.Text)
                    {
                        try
                        {
                            UnityOfWork uowaux = new UnityOfWork();
                            uowaux.RepositorioEmpleado.crear(em);
                            MessageBox.Show("se ha guardado correctamente el empleado");
                            GuardarValoresEmpEntrada();
                            modificado = true;
                            main.CargardgEmpleado(uow.RepositorioEmpleado.obtenerTodos());
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
                aux = uow.RepositorioEmpleado.obtenerUno(c => c.Usuario == em.Usuario && c.EmpleadoId != em.EmpleadoId);//para comprobar que no existe ningun usuario con ese nombre
                if (aux == null)
                {
                    if (tbContraseñaEmp.Text == tbConfirmContraseñaEmp.Text)
                    {
                        try
                        {
                            uow.RepositorioEmpleado.actualizar(em);
                            MessageBox.Show("se ha modificado correctamente el empleado");
                            GuardarValoresEmpEntrada();
                            modificado = true;
                            main.CargardgEmpleado(uow.RepositorioEmpleado.obtenerTodos());
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

        private void BtEliminarEmp_Click(object sender, RoutedEventArgs e)
        {
            if (em.EmpleadoId != 1)
            {
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
                            uow.RepositorioEmpleado.eliminar(em);
                            main.CargardgEmpleado(uow.RepositorioEmpleado.obtenerTodos());
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
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 60; j = j + 30)
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
            }
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
            CargaNuevoHorario();
            ActualizarComboBoxHorario();
        }

        private void BtGuardarHor_Click(object sender, RoutedEventArgs e)
        {

            //@@@ hace que falle el borrado

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
                uow2.RepositorioHorario.crear(horarioGuardar);
                MessageBox.Show("se ha guardado correctamente el horario");
                NuevoHorario = new Horario();
                horarioGuardar = new Horario();
                CargardgHorarios(uow2.RepositorioHorario.obtenerVarios(c => c.EmpleadoId == em.EmpleadoId));
                LimpiarGridNuevoHorario();
            }
            catch
            {

                MessageBox.Show("no se ha podido guardar un nuevo horario por falta de algun campo algun dato mal introducido");
            }

        }
        private void BtCancelarNuevoHor_Click(object sender, RoutedEventArgs e)
        {
            LimpiarGridNuevoHorario();
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
                        uow2.RepositorioHorario.eliminar(HorSelect);
                        CargardgHorarios(uow2.RepositorioHorario.obtenerVarios(c => c.EmpleadoId == em.EmpleadoId));
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
                CargardgHorarios(uow2.RepositorioHorario.obtenerVarios(c => c.EmpleadoId == em.EmpleadoId));

            }
            else if (cbBuscarListHorarios.Text == "Lunes" || (cbBuscarListHorarios.Text == "Martes") || (cbBuscarListHorarios.Text == "Miercoles") || (cbBuscarListHorarios.Text == "Jueves") || (cbBuscarListHorarios.Text == "Viernes")
                 || (cbBuscarListHorarios.Text == "Sabado"))
            {
                try
                {
                    CargardgHorarios(uow2.RepositorioHorario.obtenerVarios(c => c.Dia == cbBuscarListHorarios.Text && c.EmpleadoId == em.EmpleadoId));
                }
                catch { }
            }
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado == false)
            {
                RecuperarValoresEmpEntrada();
            }
            main.CargardgEmpleado(uow.RepositorioEmpleado.obtenerTodos());
            this.Close();
        }
    }
}
