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
using ClinicaVeterinaria.DAL;
using ClinicaVeterinaria.MODEL;
namespace ClinicaVeterinaria.Service
{
    /// <summary>
    /// Lógica de interacción para FormService.xaml
    /// </summary>
    public partial class FormService : Window
    {
        public FormService(Servicio serv, UnityOfWork uw, MainWindow mw)
        {
            InitializeComponent();
        }
    }
}
