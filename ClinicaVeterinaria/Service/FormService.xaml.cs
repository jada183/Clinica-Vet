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
namespace ClinicaVeterinaria.Service
{
    /// <summary>
    /// Lógica de interacción para FormService.xaml
    /// </summary>
    public partial class FormService : Window
    {
       

        Servicio ser = new Servicio();//Servicio local
        bool NuevoServ = false;//cambia segun venga de nuevo producto o producto seleccionado
        bool modificado = false;//comprueba que el servicio ha sido modificado correctamente para que al cerrrar no restaure los valores por defecto
        MainWindow main;
        //variables para guardar los datos que entran al abrir la ventana
        string serNombreOrigen = "";
        double serCosteOrigen;
        int serDuracionOrigen;
        string serDescripcionOrigen="";
        //validador
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

        public FormService(Servicio serv,MainWindow mw)
        {
            InitializeComponent();
            ser = serv;
            GuardarrValoresServEntrada();
            main = mw;        
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
            ser.Habilitado = true;
            if (Validado(ser))
            {
                if (NuevoServ)
                {
                    Servicio servAux = new Servicio();
                    servAux = MainWindow.uow.RepositorioServicio.obtenerUno(c => c.Nombre == ser.Nombre && c.Habilitado==true);

                    if (servAux == null)
                    {
                        try
                        {
                            
                            MainWindow.uow.RepositorioServicio.crear(ser);
                            MessageBox.Show("se ha guardado correctamente el Servicio");
                            modificado = true;
                            main.CargardgServicio(MainWindow.uow.RepositorioServicio.obtenerVarios(c=>c.Habilitado==true));

                            this.Close();
                        }
                        catch (Exception erro)
                        {
                            MessageBox.Show(erro.Message);
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
                    servAux = MainWindow.uow.RepositorioServicio.obtenerUno(c => c.Nombre == ser.Nombre && c.ServicioId != ser.ServicioId && c.Habilitado==true);
                    if (servAux == null)
                    {
                        try
                        {

                            MainWindow.uow.RepositorioServicio.actualizar(ser);
                            MessageBox.Show("se ha modificado correctamente el servicio");
                            modificado = true;
                            main.CargardgServicio(MainWindow.uow.RepositorioServicio.obtenerVarios(c=>c.Habilitado==true));

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
                        MessageBox.Show("existe un servicio con el mismo nombre y marca ya registrado");
                        RecuperarValoresServEntrada();

                    }
                }
            }
            else { }
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

                        ser.Habilitado = false;
                        MainWindow.uow.RepositorioServicio.actualizar(ser);
                        main.CargardgServicio(MainWindow.uow.RepositorioServicio.obtenerVarios(c=>c.Habilitado==true));
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

        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado == false)
            {
                RecuperarValoresServEntrada();
            }
            main.CargardgServicio(MainWindow.uow.RepositorioServicio.obtenerVarios(c=>c.Habilitado==true));
           
        }
    }
}
