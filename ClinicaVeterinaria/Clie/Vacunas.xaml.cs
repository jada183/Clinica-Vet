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
    /// Lógica de interacción para Vacunas.xaml
    /// </summary>
    public partial class Vacunas : Window
    {
        Paciente Paci = new Paciente();
        
        Vacuna VacSelect;
        Vacuna NuevaVacuna = new Vacuna();
        bool modificar = false;
      
        public Vacunas(Paciente p)
        {
            InitializeComponent();
            Paci = p;
            
            cbListEmpVac.ItemsSource = MainWindow.uow.RepositorioEmpleado.obtenerVarios(c => c.Tipo == "Sanitario");
            try
            {
                dgVacuna.ItemsSource = MainWindow.uow.RepositorioVacuna.obtenerVarios(c => c.PacienteId == Paci.PacienteId);
            }
            catch { }
            gridNuevaVacuna.Visibility = Visibility.Hidden;
            
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
        public void CargaNuevaVacuna(Vacuna v)
        {
            gridNuevaVacuna.Visibility = Visibility.Visible;

            gridNuevaVacuna.DataContext = v;

        }
        //eventos
        private void DgVacuna_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VacSelect = (Vacuna)(dgVacuna.SelectedItem);
            }
            catch
            {

            }
        }

        private void dgVacuna_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                VacSelect = (Vacuna)(dgVacuna.SelectedItem);
                CargaNuevaVacuna(VacSelect);
                modificar = true;

            }
            catch
            {
                MessageBox.Show("seleccione una vacuna");
            }
        }

        private void BtEditarVacuna_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                CargaNuevaVacuna(VacSelect);
                modificar = true;
            }
            catch
            {
                MessageBox.Show("seleccione una vacuna");
            }
        }

        private void BtAgregarVacuna_Click(object sender, RoutedEventArgs e)
        {
            NuevaVacuna.Fecha = DateTime.Today;
            modificar = false;
            CargaNuevaVacuna(NuevaVacuna);
        }

        private void BtGuardarHor_Click(object sender, RoutedEventArgs e)
        {

            if (modificar)
            {
                if (Validado(VacSelect))
                {
                    try
                    {

                        Empleado emple = (Empleado)(cbListEmpVac.SelectedItem);
                        VacSelect.Empleado = emple;
                        VacSelect.EmpleadoId = emple.EmpleadoId;
                        VacSelect.Paciente = Paci;
                        VacSelect.PacienteId = Paci.PacienteId;
                        MainWindow.uow.RepositorioVacuna.actualizar(VacSelect);
                        MessageBox.Show("se ha actualizado  correctamente");                    
                        dgVacuna.ItemsSource = MainWindow.uow.RepositorioVacuna.obtenerVarios(c => c.PacienteId == Paci.PacienteId);
                        gridNuevaVacuna.Visibility = Visibility.Hidden;
                        
                        cbListEmpVac.SelectedIndex = 0;
                        
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
                if (Validado(NuevaVacuna))
                {
                    try
                    {

                        Empleado emple = (Empleado)(cbListEmpVac.SelectedItem);
                        NuevaVacuna.Empleado = emple;
                        NuevaVacuna.EmpleadoId = emple.EmpleadoId;
                        NuevaVacuna.Paciente = Paci;
                        NuevaVacuna.PacienteId = Paci.PacienteId;
                        MainWindow.uow.RepositorioVacuna.crear(NuevaVacuna);
                        MessageBox.Show("se ha guardado correctamente");
                        NuevaVacuna = new Vacuna();
                        dgVacuna.ItemsSource = MainWindow.uow.RepositorioVacuna.obtenerVarios(c => c.PacienteId == Paci.PacienteId);
                        gridNuevaVacuna.Visibility = Visibility.Hidden;
                       
                        cbListEmpVac.SelectedIndex = 0;
                    }

                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }
                }
                else { }
            }
        }

        private void BtEliminarVacuna_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar esta vacuna?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:


                        MainWindow.uow.RepositorioVacuna.eliminar(VacSelect);
                        dgVacuna.ItemsSource = MainWindow.uow.RepositorioVacuna.obtenerVarios(c => c.PacienteId == Paci.PacienteId);
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
            catch(Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }
    }
}
