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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClinicaVeterinaria.DAL;
using ClinicaVeterinaria.MODEL;

namespace ClinicaVeterinaria
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UnityOfWork uow = new UnityOfWork();
        //variables usadas en la gestion de producto
            private Producto productoSelect = new Producto();
            private List<Producto> productosDtGrid = new List<Producto>();

        public MainWindow()
        {
            InitializeComponent();
            //carga inicial de producto
                CargardgProductos(uow.RepositorioProducto.obtenerTodos());

        }
        #region Producto
        //metodos
        public void CargardgProductos(List<Producto> lp)
        {
            productosDtGrid = lp;
            dgProd.ItemsSource = productosDtGrid;
        }
        private void BtAgregarProd_Click(object sender, RoutedEventArgs e)
        {
            Producto prod = new Producto();
            FormProd fp = new FormProd(prod,uow,this);
            fp.Show();
        }
       
        #endregion
    }
}
