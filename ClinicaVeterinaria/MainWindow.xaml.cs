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
            
            private List<Producto> productosDtGrid = new List<Producto>();
            private Producto prodSelect = new Producto();

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
        //eventos
        private void BtAgregarProd_Click(object sender, RoutedEventArgs e)
        {
            Producto prod = new Producto();
            FormProd fp = new FormProd(prod,uow,this);
            fp.Show();
        }
        private void dgProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                prodSelect = (Producto)(dgProd.SelectedItem);
            }
            catch { }
            
        }
        private void btEditarProd_Click(object sender, RoutedEventArgs e)
        {
            if (prodSelect.NombreProducto != null)
            {
                FormProd fp = new FormProd(prodSelect, uow, this);
                fp.Show();
            }
            else
            {
                MessageBox.Show("seleccione un producto");
            }
        }

        private void dgProd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            prodSelect = (Producto)(dgProd.SelectedItem);
            FormProd fp = new FormProd(prodSelect, uow, this);
            fp.Show();
        }

        private void btBorrarProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar este producto?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:


                        uow.RepositorioProducto.eliminar(prodSelect);
                        CargardgProductos(uow.RepositorioProducto.obtenerTodos());

                        break;
                    case MessageBoxResult.No:

                        break;
                    case MessageBoxResult.Cancel:
                        // User pressed Cancel button
                        // ...
                        break;
                }
            }
            catch
            {
                MessageBox.Show("seleccione un producto");
            }
        }
        #endregion


    }
}
