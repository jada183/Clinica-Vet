using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
namespace ClinicaVeterinaria.Citass
{
    /// <summary>
    /// Lógica de interacción para FormCita.xaml
    /// </summary>
    public partial class FormCita : Window
    {
        CultureInfo ci = new CultureInfo("Es-Es");
        string horaOrigenCita;
        int? pacienteIdOrigenCita;
        int? EmpleadoIdOrigenCita;
        int? ServicioIdOrigenCita;
        DateTime fechaOrigenCita;
        string causaOrigenCita;
        bool atendidaOrigenCita;
        bool NuevaCita = false;
        Cita cita;
        MainWindow main;
        Servicio servi;
        Empleado empl;
        Paciente pac;
        Boolean modificado=false;
        public FormCita(Cita cit, MainWindow mw)
        {
            InitializeComponent();
            cita = cit;
            main = mw;
            //metodo que guarda los valores por defecto que entran en la ventana
            GuardarValoresEntrada();
            //compruebo que es una nueva cita o es modificar la cita
            if (cita.CitaId == 0)
            {
                btEliminarCita.Visibility = Visibility.Hidden;
                empl = new Empleado();
                NuevaCita =true;
            }
            else
            {
                btGuardarCita.Content = "modificar";


            }
            //si la cita es para modificar
           
            CargarcbSerCit(MainWindow.uow.RepositorioServicio.obtenerTodos());
            CargarcbEmpCit(MainWindow.uow.RepositorioEmpleado.obtenerTodos());
            try
            {
                gridCita.DataContext = cita;
            }
            catch { }
           
            
            if (NuevaCita != true)
            {


                try
                {
                    servi = cita.Servicio;
                    //cita.Servicio = MainWindow.uow.RepositorioServicio.obtenerUno(c => c.ServicioId == cita.ServicioId);
                }
                catch { }
                try
                {
                    empl = cita.Sanitario;
                    //cita.Sanitario = MainWindow.uow.RepositorioEmpleado.obtenerUno(c => c.EmpleadoId == cita.EmpleadoId);
                }
                catch { }
                try
                {
                    //cita.Paciente = MainWindow.uow.RepositorioPaciente.obtenerUno(c => c.PacienteId == cita.PacienteId);
                    pac = cita.Paciente;
                }
                catch { }
                cbHoraCit.IsEnabled = true;
                //CargarHorasCbCita();
                
            }
            else
            {
               
                cita.Fecha = DateTime.Today;
                
            }
            

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
        private void AnularFechas()
        {
            //dpFechaCit.BlackoutDates.Add(new CalendarDateRange(DateTime.Today));
            try
            {
                dpFechaCit.BlackoutDates.Clear();
                List<string> lista = MainWindow.uow.RepositorioHorario.obtenerVarios(c => c.EmpleadoId == cita.Sanitario.EmpleadoId).Select(c => c.Dia).ToList<string>();

                if (lista.Count > 0)
                {
                    for (DateTime i = dpFechaCit.DisplayDateStart.Value; i <= dpFechaCit.DisplayDateEnd.Value; i = i.AddDays(1))
                    {

                        if (!lista.Contains(ci.DateTimeFormat.GetDayName(i.DayOfWeek).ToLower()))
                        {
                           
                            try
                            {
                                dpFechaCit.BlackoutDates.Add(new CalendarDateRange(i));
                            }
                            catch 
                            {
                               // MessageBox.Show(erro.Message);
                            }
                           
                        }
                        
                    }

                }
                else
                {
                    MessageBox.Show("este empleado no tiene horarios establecidos");
                    dpFechaCit.IsEnabled = false;
                }

            }
            catch { }

        }

        //carga el comboBox de los servicios de la cita
        public void CargarcbSerCit(List<Servicio> s)
        {
            cbServCita.ItemsSource = "";
            cbServCita.ItemsSource = s;
        }
        public void CargarcbEmpCit(List<Empleado> e)
        {
            cbEmpCita.ItemsSource = "";
            cbEmpCita.ItemsSource = e;
        }
       
        //el metodo de cargar las horas disponibles segun la fecha marcada
        public void CargarHorasCbCita ()
        {
            try
            {
                bool auxsalir = false;
                DateTime dtaux = Convert.ToDateTime(dpFechaCit.Text);
                cbHoraCit.Items.Clear();
                List<Cita> listAux = MainWindow.uow.RepositorioCita.obtenerVarios(c => c.Fecha.Equals(dtaux) && empl.EmpleadoId == c.EmpleadoId && cita.CitaId!=c.CitaId);
                //si no hay citas ese dia
                if (listAux.Count == 0)
                {
                    string auxDia = ci.DateTimeFormat.GetDayName(dpFechaCit.SelectedDate.Value.DayOfWeek);
                    List<Horario> listauxhor = MainWindow.uow.RepositorioHorario.obtenerVarios(c => c.Dia.Equals(auxDia) && c.EmpleadoId == empl.EmpleadoId);
                    foreach (Horario h in listauxhor)
                    {
                        //saco el valor de la hora y minuto inicial
                        String[] corteshoraini = h.HoraInic.Split(':');
                        String[] corteshoraFin = h.HoraFin.Split(':');
                        int auxint1 = Convert.ToInt32(corteshoraini[0]);
                        int auxint2 = Convert.ToInt32(corteshoraFin[0]);
                        int auxint3 = Convert.ToInt32(corteshoraini[1]);
                        int auxint4 = Convert.ToInt32(corteshoraFin[1]);
                        for (int i = auxint1; i <= auxint2; i++)
                        {

                            for (int j = auxint3; j < 60; j += 30)
                            {
                                if (j == auxint4 && i == auxint2)
                                {
                                    

                                    auxsalir = true;
                                    break;
                                }
                                else
                                {

                                    string constructorHora;
                                    if (j == 0)
                                    {
                                        constructorHora = Convert.ToString(i) + ":" + Convert.ToString(j) + "0";

                                    }
                                    else
                                    {
                                        constructorHora = Convert.ToString(i) + ":" + Convert.ToString(j);
                                    }
                                    cbHoraCit.Items.Add(constructorHora);
                                }

                            }
                            if (auxsalir == true)
                            {
                                break;
                            }
                            auxint3 = 0;
                        }
                    }

                }
                //cuando hay citas ese dia
                else
                {
                    string auxDia = ci.DateTimeFormat.GetDayName(dpFechaCit.SelectedDate.Value.DayOfWeek);
                    List<Horario> listauxhor = MainWindow.uow.RepositorioHorario.obtenerVarios(c => c.Dia.Equals(auxDia) && c.EmpleadoId == empl.EmpleadoId);
                    foreach (Horario h in listauxhor)
                    {
                        //saco el valor de la hora y minuto inicial
                        String[] corteshoraini = h.HoraInic.Split(':');
                        String[] corteshoraFin = h.HoraFin.Split(':');
                        int auxint1 = Convert.ToInt32(corteshoraini[0]);
                        int auxint2 = Convert.ToInt32(corteshoraFin[0]);
                        int auxint3 = Convert.ToInt32(corteshoraini[1]);
                        int auxint4 = Convert.ToInt32(corteshoraFin[1]);
                        for (int i = auxint1; i <= auxint2; i++)
                        {

                            for (int j = 0; j < 60; j += 30)
                            {
                                if (j == auxint4 && i == auxint2)
                                {


                                    auxsalir = true;
                                    break;
                                }
                                else
                                {

                                    string constructorHora;
                                    if (j == 0)
                                    {
                                        constructorHora = Convert.ToString(i) + ":" + Convert.ToString(j) + "0";

                                    }
                                    else
                                    {
                                        constructorHora = Convert.ToString(i) + ":" + Convert.ToString(j);
                                    }

                                    DateTime auxdt = Convert.ToDateTime(dpFechaCit.Text);
                                    //busco una cita que este a la hora del constructorhora y la fecha  del dp
                                    Cita citAux = MainWindow.uow.RepositorioCita.obtenerUno(c => c.Fecha.Equals(auxdt) && c.Hora.Equals(constructorHora));
                                    if (citAux == null)
                                    {
                                        cbHoraCit.Items.Add(constructorHora);

                                    }

                                }

                            }
                            if (auxsalir == true)
                            {
                                break;
                            }
                            auxint4 = 0;
                        }
                    }

                }
                cbHoraCit.IsEnabled = true;
            }
           
            catch(Exception erro) { MessageBox.Show(erro.Message); }
        }

       
        
        private void GuardarValoresEntrada()
        {
            horaOrigenCita=cita.Hora;
            pacienteIdOrigenCita=cita.PacienteId;
            EmpleadoIdOrigenCita = cita.EmpleadoId;
            ServicioIdOrigenCita = cita.ServicioId;
            fechaOrigenCita=cita.Fecha;
            causaOrigenCita=cita.Causa;
            atendidaOrigenCita=cita.Atendida;
        }
        private void RecuperarValoresEntrada()
        {
            cita.Hora = horaOrigenCita;
            cita.PacienteId = pacienteIdOrigenCita;
            cita.EmpleadoId= EmpleadoIdOrigenCita;
            cita.ServicioId= ServicioIdOrigenCita;
            cita.Fecha = fechaOrigenCita;         
            cita.Causa= causaOrigenCita;
            cita.Atendida= atendidaOrigenCita;
        }

        private void CbServCita_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                servi = (Servicio)cbServCita.SelectedItem;             
                cita.ServicioId = servi.ServicioId;
                cita.Servicio = servi;
            }
            catch { }
            

        }

        private void BtGuardarCita_Click(object sender, RoutedEventArgs e)
        {
            if (Validado(cita))
            {

            }
            else { }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado)
            {

            }
            else
            {
                RecuperarValoresEntrada();
                main.CargardgCitas(MainWindow.uow.RepositorioCita.obtenerVarios(c=> c.Atendida==false));
            }
        }

        private void CbEmpCita_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                empl = (Empleado)cbEmpCita.SelectedItem;
                cita.EmpleadoId = empl.EmpleadoId;
                cita.Sanitario = empl;
                dpFechaCit.DisplayDateStart = DateTime.Today;
                dpFechaCit.DisplayDateEnd = DateTime.Today.AddDays(60);
                AnularFechas();
                dpFechaCit.IsEnabled = true;
               
               
            }
            catch { }
           

        }


        private void CbHoraCit_DropDownOpened(object sender, EventArgs e)
        {
            CargarHorasCbCita();
        }

        private void BtBuscarPac_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                BuscadorPac bpac = new BuscadorPac(cita);
                bpac.ShowDialog();
            }
            catch { }
          
        }

        private void DpFechaCit_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarHorasCbCita();
        }
    }
}
