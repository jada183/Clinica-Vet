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
using System.ComponentModel.DataAnnotations;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

namespace ClinicaVeterinaria.Clie
{
    /// <summary>
    /// Lógica de interacción para FormMas.xaml
    /// </summary>
    public partial class FormMas : Window
    {
        
        Paciente pac = new Paciente();
        bool NuevoPac = false;
        bool modificado=false;
        FormCli formcli;
        string nombreOriginalMas= "";
        string especieOriginalMas = "";
        string razaOriginalMas = "";
        double pesoOriginalMas;
        double alturaOriginalMas;
        string sexoOriginalMas = "";
        DateTime fechaOriginalMas;
        CultureInfo ci = new CultureInfo("Es-Es");

        public FormMas(Paciente p, FormCli fcli)
        {
            InitializeComponent();           
            pac = p;
            GuardarValoresMasEntrada();
            formcli = fcli;
            gridMascota.DataContext = pac;
           
            if (pac.PacienteId == 0)
            {
                BtEliminarPac.Visibility = Visibility.Hidden;
                NuevoPac = true;
                pac.FechaNacimiento = DateTime.Today;
            }
            else
            {
                BtGuardarPac.Content = "modificar";


            }
        }
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

        private void BtGuardarPac_Click(object sender, RoutedEventArgs e)
        {
            pac.Habilitado = true;
            if (Validado(pac)) { 
                if (NuevoPac)
                {
                    Paciente pacAux = new Paciente();
                    pacAux = MainWindow.uow.RepositorioPaciente.obtenerUno(c => c.Nombre == pac.Nombre && c.ClienteId == pac.ClienteId && c.Habilitado==true);
                    if (pacAux == null)
                    {
                        try
                        {


                            MainWindow.uow.RepositorioPaciente.crear(pac);
                            MessageBox.Show("se ha guardado correctamente la  mascota");
                            modificado = true;
                            formcli.CargarDgMascotas(MainWindow.uow.RepositorioPaciente.obtenerVarios(c => c.ClienteId == pac.ClienteId && c.Habilitado==true));
                            

                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                           
                        }
                    }
                    else
                    {
                        MessageBox.Show("existe una mascota con ese nombre registrado");
                       
                    }
                }
                //modificar
                else
                {
                    Paciente pacAux = new Paciente();
                    pacAux = MainWindow.uow.RepositorioPaciente.obtenerUno(c => c.Nombre == pac.Nombre && c.ClienteId == pac.ClienteId && pac.PacienteId != c.PacienteId  && c.Habilitado == true);
                    if (pacAux == null)
                    {
                        try
                        {

                            MainWindow.uow.RepositorioPaciente.actualizar(pac);
                            MessageBox.Show("se ha modificado correctamente la mascota");
                            modificado = true;
                            formcli.CargarDgMascotas(MainWindow.uow.RepositorioPaciente.obtenerVarios(c => c.ClienteId == pac.ClienteId && c.Habilitado==true));

                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                            //RecuperarValoresProdEntrada();
                        }
                    }
                    else
                    {
                        MessageBox.Show("existe un producto con el mismo nombre y marca ya registrado");
                        //RecuperarValoresProdEntrada();

                    }
                }
            }
        }

        private void BtEliminarPac_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                List<Cita> lcita = MainWindow.uow.RepositorioCita.obtenerVarios(c => c.PacienteId == pac.PacienteId && c.Atendida == false);
                List<EstadoIngresado> lingresado = MainWindow.uow.RepositorioEstadoIngresado.obtenerVarios(c => c.PacienteId == pac.PacienteId);
                if (lcita.Count > 0)
                {
                    MessageBox.Show("Cuidado vas a eliminar una mascota con una cita sin atender");
                }
                if (lingresado.Count > 0)
                {
                    MessageBox.Show("Cuidado vas a eliminar una mascota que se encuentra ingresada");
                }

                string messageBoxText = "Estas seguro que deseas eliminar esta mascota?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        pac.Habilitado = false;
                        MainWindow.uow.RepositorioPaciente.actualizar(pac);

                        MainWindow.uow.RepositorioCita.eliminarVarios(c => c.PacienteId == pac.PacienteId && c.Atendida == false);
                        MainWindow.uow.RepositorioEstadoIngresado.eliminarVarios(c => c.PacienteId == pac.PacienteId);
                        //recargo las tablas
                        //recargo las ventanas necesarias
                        formcli.CargarDgMascotas(MainWindow.uow.RepositorioPaciente.obtenerVarios(c => c.ClienteId == pac.ClienteId && c.Habilitado==true));
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
                MessageBox.Show("seleccione un horario");
            }
        }

        private void btImgProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != "")
                {
                    if (Regex.IsMatch(openFileDialog.FileName, @".(jpg|bmp|png)$"))
                    {
                        string directorioImagen = openFileDialog.SafeFileName;

                        string directorioImagenOrigen = openFileDialog.FileName;

                        string directorioImagenDestino = @"Imagenes\" + directorioImagen;


                        FileInfo f = new FileInfo(directorioImagen);

                        if (!File.Exists(Environment.CurrentDirectory + "\\" + directorioImagenDestino))
                        {
                            File.Copy(directorioImagenOrigen, directorioImagenDestino);
                            pac.Imagen = directorioImagenDestino;
                            imgProd.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + directorioImagenDestino));
                        }
                        else
                        {
                            pac.Imagen = directorioImagenDestino;
                            imgProd.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + directorioImagenDestino));

                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("La imagen no es correcta. Debe estar en formato JPG, BMP, PNG");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("No se ha cargado ninguna imagen");
                }

            }
            catch { }
        }
        private void RecuperarValoresMasEntrada()
        {
            pac.Nombre = nombreOriginalMas;
            pac.Especie= especieOriginalMas;
            pac.Raza= razaOriginalMas;
            pac.Peso = pesoOriginalMas;
            pac.Altura= alturaOriginalMas;
            pac.Sexo= sexoOriginalMas;
            pac.FechaNacimiento= fechaOriginalMas;
        }
        public void GuardarValoresMasEntrada()
        {
            nombreOriginalMas = pac.Nombre;
            especieOriginalMas = pac.Especie;
            razaOriginalMas = pac.Raza;
            pesoOriginalMas = pac.Peso;
            alturaOriginalMas = pac.Altura;
            sexoOriginalMas = pac.Sexo;
            fechaOriginalMas = pac.FechaNacimiento;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado == false)
            {
                RecuperarValoresMasEntrada();
            }
          
        }
    }
}
