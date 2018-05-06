using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
        UnityOfWork uow;
        CultureInfo ci = new CultureInfo("Es-Es");
       
        public FormProd(Producto prod, UnityOfWork uw)
        {

            InitializeComponent();
            gridProductoSelect.DataContext = prod;
            uow = uw;
            //para identificar si es para crear un nuevo producto o para modificar uno existente
            if (prod.ProductoId == 0)
            {
                btEliminarProd.Visibility = Visibility.Hidden;
                
            }
            else
            {
                btGuardarProd.Content = "modificar";
               
            }

        }

        private void BtGuardarProd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
