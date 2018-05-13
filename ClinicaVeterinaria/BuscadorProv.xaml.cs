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
    /// Lógica de interacción para BuscadorProv.xaml
    /// </summary>
    public partial class BuscadorProv : Window
    {

        UnityOfWork uow= new UnityOfWork();
        Proveedor prov;
        Producto prod = new Producto();
        
        List<Proveedor> listProv = new List<Proveedor>();
        public BuscadorProv(Producto p, UnityOfWork uw)
        {
            InitializeComponent();
            prod= p;
            listProv = uow.RepositorioProveedor.obtenerTodos();
            dgProveedor.ItemsSource = listProv;
            uow=uw;
        }
        //carga el proveedor en el datagrid
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prov = uow.RepositorioProveedor.obtenerUno(c => c.Email == tbBuscadorProv.Text);
                if(prov!= null)
                {
                    listProv = new List<Proveedor>();
                    listProv.Add(prov);
                    dgProveedor.ItemsSource = listProv;
                }
                else
                {
                    MessageBox.Show("no se ha encontrado ningun proveedor con ese correo");
                }
            }
            catch
            {
               
            }
        }
        //muestra todos los proveedores en datagrid de ventana
        private void btMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            listProv = uow.RepositorioProveedor.obtenerTodos();
            dgProveedor.ItemsSource = listProv;
        }
        //cambia el producto seleccionado 
        private void dgProveedor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                prov = (Proveedor)(dgProveedor.SelectedItem);
            }
            catch { }
        }

        private void dgProveedor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                prov = (Proveedor)(dgProveedor.SelectedItem);
                prod.ProveedorId = prov.ProveedorId;
                prod.Proveedor = uow.RepositorioProveedor.obtenerUno(c => c.ProveedorId == prod.ProveedorId);
                this.Close();
            }
            catch { }
        }

        private void btSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (prov != null)
            {
                
                prod.ProveedorId = prov.ProveedorId;
                prod.Proveedor = uow.RepositorioProveedor.obtenerUno(c => c.ProveedorId == prod.ProveedorId);
                this.Close();
            }
            else
            {
                MessageBox.Show("Seleccione un proveedor");
            }
        }
    }
}
