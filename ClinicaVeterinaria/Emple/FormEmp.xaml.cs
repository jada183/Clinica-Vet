using ClinicaVeterinaria.DAL;
using ClinicaVeterinaria.MODEL;
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

namespace ClinicaVeterinaria.Emple
{
    /// <summary>
    /// Lógica de interacción para FormEmp.xaml
    /// </summary>
    public partial class FormEmp : Window
    {
        UnityOfWork uow;

        Empleado em = new Empleado();//empleado local
        bool NuevoEmp = false;//cambia segun venga de nuevo empleado o empleado seleccionado
        MainWindow main = new MainWindow();//la mainwindows local
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
                NuevoEmp = true;
            }
            else
            {
                btGuardarEmp.Content = "modificar";


            }
        }
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

        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado == false)
            {
                RecuperarValoresEmpEntrada();
            }
            main.CargardgEmpleado(uow.RepositorioEmpleado.obtenerTodos());
        }

        private void btGuardarEmp_Click(object sender, RoutedEventArgs e)
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
                            uow.RepositorioEmpleado.crear(em);
                            MessageBox.Show("se ha guardado correctamente el empleado");
                            GuardarValoresEmpEntrada();
                            modificado = true;
                            main.CargardgEmpleado(uow.RepositorioEmpleado.obtenerTodos());
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Error falta algun campo obligatorio por cubrir o  algun dato tiene un formato incorrecto");
                        }
                       
                    }
                    //cuando la confirmacion de contraseña no coincide con la primera contraseña
                    else
                    {
                        MessageBox.Show("las contraseñas no coinciden");
                        RecuperarValoresEmpEntrada();

                    }
                }
                //cuando existe un empleado con ese nombre de usuario
                else
                {
                    MessageBox.Show("existe un empleado con ese nombre de usuario");
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
                            MessageBox.Show("Error falta algun campo obligatorio por cubrir");
                            RecuperarValoresEmpEntrada();
                        }
                       
                    }
                    //cuando la confirmacion de contraseña no coincide con la primera contraseña
                    else
                    {
                        MessageBox.Show("las contraseñas no coinciden");
                        RecuperarValoresEmpEntrada();
                    }
                }
                //cuando existe un empleado con ese nombre de usuario
                else
                {
                    MessageBox.Show("existe un empleado con ese nombre de usuario o  algun dato tiene un formato incorrecto");
                    tbUsuarioEmp.Text = "";
                }
            }
        }
    }
}
