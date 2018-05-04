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

namespace ClinicaVeterinaria
{
    /// <summary>
    /// Lógica de interacción para FormProd.xaml
    /// </summary>
    public partial class FormProd : Window
    {
        UnityOfWork uow = new UnityOfWork();
        public FormProd(Producto prod)
        {
            InitializeComponent();
        }

        private void btGuardarProd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
