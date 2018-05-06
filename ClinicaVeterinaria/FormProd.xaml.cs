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
        Producto pr= new Producto();
        bool NuevoProd = false;//cambia segun venga de nuevo producto o producto seleccionado

        public FormProd(Producto prod, UnityOfWork uw)
        {

            InitializeComponent();
            pr = prod;
            gridProductoSelect.DataContext = pr;
            
            
            uow = uw;
            //para identificar si es para crear un nuevo producto o para modificar uno existente
            if (prod.ProductoId == 0)
            {
                btEliminarProd.Visibility = Visibility.Hidden;
                NuevoProd = true;
            }
            else
            {
                btGuardarProd.Content = "modificar";
               
            }

        }

        private void BtGuardarProd_Click(object sender, RoutedEventArgs e)
        {
            //guardar
            if (NuevoProd) {
                try
                {
                  
                    uow.RepositorioProducto.crear(pr);
                    MessageBox.Show("se ha guardado correctamente el producto");
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("error falt aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                }
            }
            //modificar
            else
            {

            }
        }
    }
}
