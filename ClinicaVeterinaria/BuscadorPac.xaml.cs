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
using ClinicaVeterinaria.MODEL;
namespace ClinicaVeterinaria
{
    /// <summary>
    /// Lógica de interacción para BuscadorPac.xaml
    /// </summary>
    public partial class BuscadorPac : Window
    {
        Paciente pac;
        Cita cita;
        Cliente cl;
        EstadoIngresado ei;
        String tipoEntrada = "";
        public BuscadorPac(Object obj)
        {
            InitializeComponent();
            CargarDgPacientes(MainWindow.uow.RepositorioPaciente.obtenerVarios(c=>c.Habilitado==true));
           
            try
            {
                cita = (Cita)obj;
                tipoEntrada = "cita";
            }
            catch {
            }
            try
            {
                ei = (EstadoIngresado)obj;
                tipoEntrada = "ingresado";
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
        public void CargarDgPacientes(List<Paciente> lpac)
        {
            try
            {
                dgPaciente.ItemsSource = lpac;
            }
            catch { }
            
        }

        private void DgPaciente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                pac = (Paciente)dgPaciente.SelectedItem;
            }
            catch { }
        }

        private void DgPaciente_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tipoEntrada == "cita")
            {
                try
                {
                    pac = (Paciente)dgPaciente.SelectedItem;
                    if (pac.PacienteId > 0)
                    {
                        cita.PacienteId = pac.PacienteId;
                        cita.Paciente = pac;
                        if (Validado(cita))
                        {
                            if (cita.CitaId > 0)
                            {
                                MainWindow.uow.RepositorioCita.actualizar(cita);
                            }

                        }
                        else
                        {

                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Seleccione una mascota");
                    }
                    this.Close();
                }
                catch { }
            }
            else
            {
                try
                {
                    pac = (Paciente)dgPaciente.SelectedItem;
                    if (pac.PacienteId > 0)
                    {
                        ei.PacienteId = pac.PacienteId;
                        ei.Paciente = pac;
                        if (Validado(ei))
                        {
                            if (ei.EstadoIngresadoId > 0)
                            {
                                MainWindow.uow.RepositorioEstadoIngresado.actualizar(ei);
                            }

                        }
                        else
                        {

                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Seleccione una mascota");
                    }
                    this.Close();
                }
                catch { }
            }
         
        }

        private void BtBuscarListPac_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cl = MainWindow.uow.RepositorioCliente.obtenerUno(c => c.Email == tbBuscadorPac.Text);
                if (cl.ClienteId > 0)
                {
                    CargarDgPacientes(MainWindow.uow.RepositorioPaciente.obtenerVarios(c=>c.ClienteId==cl.ClienteId && c.Habilitado==true));
                }
                else
                {
                    MessageBox.Show("no se ha encontrado ninguna mascota de ese email de dueño");
                }
            }
            catch
            {

            }
        }

        private void BtBuscartodiosPac_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CargarDgPacientes(MainWindow.uow.RepositorioPaciente.obtenerVarios(c=>c.Habilitado==true));
            }
            catch
            {

            }
        }

        private void BtSeleccionarPac_Click(object sender, RoutedEventArgs e)
        {
            if (tipoEntrada == "cita")
            {
                try
                {
                    pac = (Paciente)dgPaciente.SelectedItem;
                    if (pac.PacienteId > 0)
                    {
                        cita.PacienteId = pac.PacienteId;
                        cita.Paciente = pac;
                        if (Validado(cita))
                        {
                            if (cita.CitaId > 0)
                            {
                                MainWindow.uow.RepositorioCita.actualizar(cita);
                            }

                        }
                        else
                        {

                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Seleccione una mascota");
                    }
                    this.Close();
                }
                catch { }
            }
            else
            {
                try
                {
                    pac = (Paciente)dgPaciente.SelectedItem;
                    if (pac.PacienteId > 0)
                    {
                        ei.PacienteId = pac.PacienteId;
                        ei.Paciente = pac;
                        if (Validado(ei))
                        {
                            if (ei.EstadoIngresadoId > 0)
                            {
                                MainWindow.uow.RepositorioEstadoIngresado.actualizar(ei);
                            }

                        }
                        else
                        {

                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Seleccione una mascota");
                    }
                    this.Close();
                }
                catch { }
            }
        }
    }
}
