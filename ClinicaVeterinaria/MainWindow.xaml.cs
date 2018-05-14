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
using ClinicaVeterinaria.Service;
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

        //variables usadas para la gestion de servicios
        private List<Servicio> servicios = new List<Servicio>();
        private Servicio serviSelect = new Servicio();

        public MainWindow()
        {
            InitializeComponent();
            //carga de datos inicial    
            CargardgProductos(uow.RepositorioProducto.obtenerTodos());
            CargardgServicio(uow.RepositorioServicio.obtenerTodos());

        }
        #region Producto
        //metodos
        public void CargardgProductos(List<Producto> lp)
        {
            productosDtGrid = lp;
            dgProd.ItemsSource = productosDtGrid;
        }
        public void CargarVentanaFormProd(Producto p)
        {
            FormProd fp = new FormProd(p, uow, this);
            fp.Show();
        }
        //eventos
        private void BtAgregarProd_Click(object sender, RoutedEventArgs e)
        {
            Producto prod = new Producto();
            CargarVentanaFormProd(prod);
        }
        private void DgProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                prodSelect = (Producto)(dgProd.SelectedItem);
            }
            catch { }
            
        }
        private void BtEditarProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (prodSelect.NombreProducto != null)
                {
                    CargarVentanaFormProd(prodSelect);
                }
                else
                {
                    MessageBox.Show("seleccione un producto");
                }
            }catch
            {
                MessageBox.Show("seleccione un producto primero");
            }
        
        }

        private void DgProd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            prodSelect = (Producto)(dgProd.SelectedItem);
            CargarVentanaFormProd(prodSelect);
        }

        private void BtBorrarProd_Click(object sender, RoutedEventArgs e)
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
        private void BtBucarList_Click(object sender, RoutedEventArgs e)
        {
            if (cbBuscarListProd.Text == "Todos")
            {
                CargardgProductos(uow.RepositorioProducto.obtenerTodos());

            }
            else if (cbBuscarListProd.Text == "Marca")
            {
                try
                {
                    CargardgProductos(uow.RepositorioProducto.obtenerVarios(c => c.NombreMarca == tbBuscadorList.Text));
                }
                catch { }
            }
            else if (cbBuscarListProd.Text == "Animal dirigido")
            {
                try
                {
                    CargardgProductos(uow.RepositorioProducto.obtenerVarios(c => c.AnimalDirigido == tbBuscadorList.Text));
                }
                catch
                {
                }

            }
            else if (cbBuscarListProd.Text == "Sin stock")
            {
                try
                {
                    CargardgProductos(uow.RepositorioProducto.obtenerVarios(c => c.Stock == 0));
                }
                catch { }
            }
            else if (cbBuscarListProd.Text == "Categoria")
            {
                try
                {
                    CargardgProductos(uow.RepositorioProducto.obtenerVarios(c => c.Categoria == tbBuscadorList.Text));
                }
                catch { }
            }
            tbBuscadorList.Text = "";
        }

        private void BtBuscarProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Producto aux = uow.RepositorioProducto.obtenerUno(c => c.NombreProducto == tbBuscadorProdNombre.Text && c.NombreMarca==tbBuscadorProdMarca.Text);
                if (aux.ProductoId != 0)
                {
                    prodSelect = aux;
                    CargarVentanaFormProd(prodSelect);

                }
                else
                {
                    MessageBox.Show("no se ha encontrado ningun producto con ese nombre y esa marca");
                }
            }
            catch{
                MessageBox.Show("no se ha encontrado ningun producto con ese nombre y esa marca");
            }
        }


        #endregion
        #region Servicio
        //metodos
        public void CargardgServicio(List<Servicio> sv)
        {
            servicios = sv;
            dgServ.ItemsSource = servicios;
        }
        public void CargarVentanaFormServ(Servicio s)
        {
            FormService fs = new Service.FormService(s,uow,this);
            fs.Show();
        }
        //eventos
        private void BtAgregarServ_Click(object sender, RoutedEventArgs e)
        {
            Servicio  serv= new Servicio();
            CargarVentanaFormServ(serv);
        }
        private void DgServicios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                serviSelect = (Servicio)(dgServ.SelectedItem);
            }
            catch { }
        }
        private void DgServ_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            serviSelect = (Servicio)(dgServ.SelectedItem);
            CargarVentanaFormServ(serviSelect);
        }

        private void BtBuscarServ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Servicio aux = uow.RepositorioServicio.obtenerUno(c => c.Nombre == tbBuscadorServNombre.Text);
                if (aux.ServicioId != 0)
                {
                    serviSelect = aux;
                    CargarVentanaFormServ(serviSelect);

                }
                else
                {
                    MessageBox.Show("no se ha encontrado ningun servicio con ese nombre");
                }
            }
            catch
            {
                MessageBox.Show("no se ha encontrado ningun servicio con ese nombre");
            }
        }

        private void BtEditarServ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (serviSelect.Nombre != null)
                {
                    CargarVentanaFormServ(serviSelect);
                }
                else
                {
                    MessageBox.Show("seleccione un servicio");
                }
            }
            catch
            {
                MessageBox.Show("seleccione un servicio");
            }
        }

        private void BtBorrarServ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar este servicio?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        
                        uow.RepositorioServicio.eliminar(serviSelect);
                        CargardgServicio(uow.RepositorioServicio.obtenerTodos());
                       
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
                MessageBox.Show("seleccione un Servicio");
              
            }
        }
        #endregion


    }
}
