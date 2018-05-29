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

namespace ClinicaVeterinaria.Clie
{
    /// <summary>
    /// Lógica de interacción para Historiales.xaml
    /// </summary>
    /// 

    public partial class Historiales : Window
    {
        Paciente Paci = new Paciente();

        HistorialClinico HisSelect;
        HistorialClinico NuevoHistorial = new HistorialClinico();
        bool modificar = false;
        public Historiales(Paciente p)
        {
            InitializeComponent();
            Paci = p;
            cbListEmpHis.ItemsSource = MainWindow.uow.RepositorioEmpleado.obtenerVarios(c => c.Tipo == "Sanitario");
            try
            {
                dgHistorial.ItemsSource = MainWindow.uow.RepositorioHistorialClinico.obtenerVarios(c => c.PacienteId == Paci.PacienteId);
            }
            catch { }
            gridNuevoHistorial.Visibility = Visibility.Hidden;
        }
        //metodos
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
        public void CargaNuevoHistorial(HistorialClinico h)
        {
            gridNuevoHistorial.Visibility = Visibility.Visible;

            gridNuevoHistorial.DataContext = h;

        }

        private void DgHistorial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HisSelect = (HistorialClinico)(dgHistorial.SelectedItem);
            }
            catch
            {

            }
        }

        private void DgHistorial_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                HisSelect = (HistorialClinico)(dgHistorial.SelectedItem);
                lgridNuevoHistorial.Content = "Historial";
                btGuardarHis.Content = "Modificar";
                CargaNuevoHistorial(HisSelect);
                modificar = true;

            }
            catch
            {
                MessageBox.Show("seleccione un historial");
            }
        }

        private void BtEditarHis_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                CargaNuevoHistorial(HisSelect);
                lgridNuevoHistorial.Content = "Historial";
                btGuardarHis.Content = "Modificar";
                modificar = true;
            }
            catch
            {
                MessageBox.Show("seleccione un historial");
            }
        }

        private void BtAgregarHis_Click(object sender, RoutedEventArgs e)
        {
            NuevoHistorial.Fecha = DateTime.Today;
            lgridNuevoHistorial.Content = "Nuevo historial";
            btGuardarHis.Content = "Guardar";
            modificar = false;
            CargaNuevoHistorial(NuevoHistorial);
        }

        private void btGuardarHis_Click(object sender, RoutedEventArgs e)
        {
            if (modificar)
            {
                if (Validado(HisSelect))
                {
                    try
                    {

                        Empleado emple = (Empleado)(cbListEmpHis.SelectedItem);
                        HisSelect.Empleado = emple;
                        HisSelect.EmpleadoId = emple.EmpleadoId;
                        HisSelect.Paciente = Paci;
                        HisSelect.PacienteId = Paci.PacienteId;
                        MainWindow.uow.RepositorioHistorialClinico.actualizar(HisSelect);
                        MessageBox.Show("se ha actualizado  correctamente");
                        dgHistorial.ItemsSource = MainWindow.uow.RepositorioHistorialClinico.obtenerVarios(c => c.PacienteId == Paci.PacienteId);
                        NuevoHistorial = new HistorialClinico();
                        gridNuevoHistorial.Visibility = Visibility.Hidden;
                    }

                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }
                }
                else { }
            }
            else
            {
                if (Validado(NuevoHistorial))
                {
                    try
                    {

                        Empleado emple = (Empleado)(cbListEmpHis.SelectedItem);
                        NuevoHistorial.Empleado = emple;
                        NuevoHistorial.EmpleadoId = emple.EmpleadoId;
                        NuevoHistorial.Paciente = Paci;
                        NuevoHistorial.PacienteId = Paci.PacienteId;
                        MainWindow.uow.RepositorioHistorialClinico.crear(NuevoHistorial);
                        MessageBox.Show("se ha guardado correctamente");
                        NuevoHistorial = new HistorialClinico();
                        dgHistorial.ItemsSource = MainWindow.uow.RepositorioHistorialClinico.obtenerVarios(c => c.PacienteId == Paci.PacienteId);
                        gridNuevoHistorial.Visibility = Visibility.Hidden;
                    }

                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }
                }
                else { }
            }
        }

        private void BtEliminarHis_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar este historial?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:


                        MainWindow.uow.RepositorioHistorialClinico.eliminar(HisSelect);
                        dgHistorial.ItemsSource = MainWindow.uow.RepositorioHistorialClinico.obtenerVarios(c => c.PacienteId == Paci.PacienteId);
                        //this.Close();
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
                MessageBox.Show(erro.Message);
            }
        }
    }
}
