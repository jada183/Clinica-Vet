using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        
        Proveedor prov;
        Producto prod = new Producto();
        
        List<Proveedor> listProv = new List<Proveedor>();
        public BuscadorProv(Producto p)
        {
            InitializeComponent();
            prod= p;
            listProv = MainWindow.uow.RepositorioProveedor.obtenerTodos();
            dgProveedor.ItemsSource = listProv;
           
        }
        //validador
        private Boolean Validado(Object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj, null, null);
            List<System.ComponentModel.DataAnnotations.ValidationResult> errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(obj, validationContext, errors, true);

            if (errors.Count() > 0)
            {

                string mensageErrores = string.Empty;
                foreach (var error in errors)
                {
                    error.MemberNames.First();

                    mensageErrores += error.ErrorMessage + Environment.NewLine;
                }
                System.Windows.MessageBox.Show(mensageErrores); return false;
            }
            else
            {
                return true;
            }
        }
        //carga el proveedor en el datagrid
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prov = MainWindow.uow.RepositorioProveedor.obtenerUno(c => c.Email == tbBuscadorProv.Text);
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
        private void BtMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            listProv = MainWindow.uow.RepositorioProveedor.obtenerTodos();
            dgProveedor.ItemsSource = listProv;
        }
        //cambia el producto seleccionado 
        private void DgProveedor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                prov = (Proveedor)(dgProveedor.SelectedItem);
            }
            catch { }
        }

        private void DgProveedor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                prov = (Proveedor)(dgProveedor.SelectedItem);
                prod.ProveedorId = prov.ProveedorId;
                prod.Proveedor = prov;
                if (Validado(prod))
                {
                    if (prod.ProductoId > 0)
                    {
                        MainWindow.uow.RepositorioProducto.actualizar(prod);
                    }
                    
                   
                }
                else
                {
                    MessageBox.Show("realice los cambios en formulario producto antes de seleccionar un proveedor");
                }
                this.Close();
            }
            catch { }
        }

        private void BtSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (prov != null)
            {
                
                prod.ProveedorId = prov.ProveedorId;
                prod.Proveedor = MainWindow.uow.RepositorioProveedor.obtenerUno(c => c.ProveedorId == prod.ProveedorId);
                if(Validado(prod)){
                    if (prod.ProductoId > 0)
                    {
                        MainWindow.uow.RepositorioProducto.actualizar(prod);
                    }
                    
                   
                }
                else
                {
                    MessageBox.Show("realice los cambios en formulario producto antes de seleccionar un proveedor");
                }
              
                this.Close();
            }
            else
            {
                MessageBox.Show("Seleccione un proveedor");
            }
        }
       
    }
}
