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

namespace ClinicaVeterinaria.Ventaa
{
    /// <summary>
    /// Lógica de interacción para GenerarExcels.xaml
    /// </summary>
    public partial class GenerarExcels : Window
    {
        public GenerarExcels()
        {
            InitializeComponent();
        }

        private void BtCancelarExcelProd_Click(object sender, RoutedEventArgs e)
        {
            gridDatosProductos.Visibility = Visibility.Collapsed;
        }

        private void BtGenExcelVentProd_Click(object sender, RoutedEventArgs e)
        {
            gridDatosProductos.Visibility = Visibility.Visible;
        }
    }
}
