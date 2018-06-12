using ClinicaVeterinaria.MODEL;
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

namespace ClinicaVeterinaria.Ingresadoo
{
    /// <summary>
    /// Lógica de interacción para FormIngresado.xaml
    /// </summary>
    public partial class FormIngresado : Window
    {
        EstadoIngresado estadoIng;
        MainWindow main;
        public FormIngresado(EstadoIngresado ei,MainWindow mw)
        {
            InitializeComponent();
            estadoIng = ei;
            main = mw;
            gridIngresado.DataContext = estadoIng;
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
        private void BtBuscarPac_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BuscadorPac bpac = new BuscadorPac(estadoIng);
                bpac.ShowDialog();
            }
            catch { }
        }

        private void BtGuardarIngresado_Click(object sender, RoutedEventArgs e)
        {
            if (estadoIng.PacienteId> 0)
            {
                if (Validado(estadoIng))
                {
                    MainWindow.uow.RepositorioEstadoIngresado.crear(estadoIng);
                    MessageBox.Show("Se ha guardado correctamente");
                    main.CargardgIngresado(MainWindow.uow.RepositorioEstadoIngresado.obtenerTodos());
                    this.Close();
                }
                else
                {

                }
            }
        }
    }
}
