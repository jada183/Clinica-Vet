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

namespace ClinicaVeterinaria.Clie
{
    /// <summary>
    /// Lógica de interacción para FormMas.xaml
    /// </summary>
    public partial class FormMas : Window
    {
        UnityOfWork uow;
        Paciente pac = new Paciente();
        bool NuevoPac = false;
        FormCli formcli;
       

        public FormMas(Paciente p, FormCli fcli, UnityOfWork uw)
        {
            InitializeComponent();
            pac = p;
            formcli = fcli;
            gridMascota.DataContext = pac;
            uow = uw;
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

            if (Validado(pac)) { 
                if (NuevoPac)
                {
                    Paciente pacAux = new Paciente();
                    pacAux = uow.RepositorioPaciente.obtenerUno(c => c.Nombre == pac.Nombre && c.ClienteId == pac.ClienteId);
                    if (pacAux == null)
                    {
                        try
                        {

                            uow.RepositorioPaciente.crear(pac);
                            MessageBox.Show("se ha guardado correctamente la  mascota");
                            
                            formcli.CargarDgMascotas(uow.RepositorioPaciente.obtenerVarios(c => c.ClienteId == pac.ClienteId));

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
                        MessageBox.Show("existe una mascota con ese nombre registrado");
                        //RecuperarValoresProdEntrada();
                    }
                }
                //modificar
                else
                {
                    Paciente pacAux = new Paciente();
                    pacAux = uow.RepositorioPaciente.obtenerUno(c => c.Nombre == pac.Nombre && c.ClienteId == pac.ClienteId && pac.PacienteId != c.PacienteId);
                    if (pacAux == null)
                    {
                        try
                        {

                            uow.RepositorioPaciente.actualizar(pac);
                            MessageBox.Show("se ha modificado correctamente la mascota");
                            
                            formcli.CargarDgMascotas(uow.RepositorioPaciente.obtenerVarios(c => c.ClienteId == pac.ClienteId));

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
                string messageBoxText = "Estas seguro que deseas eliminar esta mascota?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        uow.RepositorioPaciente.eliminar(pac);
                        formcli.CargarDgMascotas(uow.RepositorioPaciente.obtenerVarios(c => c.ClienteId == pac.ClienteId));
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
    }
}
