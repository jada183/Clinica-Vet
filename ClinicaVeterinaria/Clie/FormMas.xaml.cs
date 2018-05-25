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
namespace ClinicaVeterinaria.Clie
{
    /// <summary>
    /// Lógica de interacción para FormMas.xaml
    /// </summary>
    public partial class FormMas : Window
    {
        public FormMas(int cliId, Paciente p)
        {
            InitializeComponent();
        }
    }
}
