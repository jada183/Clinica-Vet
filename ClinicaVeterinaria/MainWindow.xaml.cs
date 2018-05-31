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
using ClinicaVeterinaria.Proveed;
using ClinicaVeterinaria.Emple;
using ClinicaVeterinaria.Clie;
namespace ClinicaVeterinaria
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static UnityOfWork uow = new UnityOfWork();

        //variables usadas en la gestion de producto     
        private List<Producto> productosDtGrid = new List<Producto>();
        private Producto prodSelect = new Producto();

        //variables usadas para la gestion de servicios
        private List<Servicio> servicios = new List<Servicio>();
        private Servicio serviSelect = new Servicio();

        //variables usadas para la gestion de proveedor
        private List<Proveedor> proveedores = new List<Proveedor>();
        private Proveedor provSelect= new Proveedor();

        //variable usadas para la gestion de empleado
        private List<Empleado> empleados = new List<Empleado>();
        public static Empleado empSelect = new Empleado();

        //variables usadas en clientes
        private List<Cliente> Clientes = new List<Cliente>();
        private Cliente cliSelect = new Cliente();
        public MainWindow()
        {
            InitializeComponent();
            //carga de datos inicial    
            CargardgProductos(uow.RepositorioProducto.obtenerTodos());
            CargardgServicio(uow.RepositorioServicio.obtenerTodos());
            CargardgProveedor(uow.RepositorioProveedor.obtenerTodos());
            CargardgEmpleado(uow.RepositorioEmpleado.obtenerTodos());
            CargardgCliente(uow.RepositorioCliente.obtenerTodos());
           
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
            FormProd fp = new FormProd(p,this);
            fp.ShowDialog();
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
            try
            {
                prodSelect = (Producto)(dgProd.SelectedItem);
                CargarVentanaFormProd(prodSelect);
            }
            catch { }
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
            FormService fs = new Service.FormService(s,this);
            fs.ShowDialog();
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
            try
            {
                serviSelect = (Servicio)(dgServ.SelectedItem);
                CargarVentanaFormServ(serviSelect);
            }
            catch { }
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
        #region Proveedor
        //metodos
        public void CargarVentanaFormProv(Proveedor p)
        {
            FormProv fpv = new FormProv(p,this);
            fpv.ShowDialog();
        }
        //eventos
        private void BtAgregarProv_Click(object sender, RoutedEventArgs e)
        {
            Proveedor proveedor = new Proveedor();
            CargarVentanaFormProv(proveedor);
        }
        public void CargardgProveedor(List<Proveedor> lpv)
        {
           proveedores = lpv;
           dgProv.ItemsSource = proveedores;
        }
        private void DgProv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                provSelect = (Proveedor)(dgProv.SelectedItem);
            }
            catch { }
        }

        private void DgProv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                prodSelect = (Producto)(dgProd.SelectedItem);
                CargarVentanaFormProv(provSelect);
            }
            catch { }
        }
        private void BtEditarProv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (provSelect.Nombre != null)
                {
                    CargarVentanaFormProv(provSelect);
                }
                else
                {
                    MessageBox.Show("seleccione un proveedor");
                }
            }
            catch
            {
                MessageBox.Show("seleccione un proveedor primero");
            }
        }
        private void BtBorrarProv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar este proveedor?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:


                        uow.RepositorioProveedor.eliminar(provSelect);
                        CargardgProveedor(uow.RepositorioProveedor.obtenerTodos());

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
                MessageBox.Show("seleccione un proveedor");
            }
        }
        private void BtBuscarProv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Proveedor aux = uow.RepositorioProveedor.obtenerUno(c => c.Email == tbBuscadorProvEmail.Text);
                if (aux.ProveedorId != 0)
                {
                    provSelect = aux;
                    CargarVentanaFormProv(provSelect);

                }
                else
                {
                    MessageBox.Show("no se ha encontrado ningun proveedor con ese email");
                }
            }
            catch
            {
                MessageBox.Show("no se ha encontrado ningun proveedor con ese email");
            }
        }
        #endregion
        #region Empleado
        //metodos
        public void CargardgEmpleado(List<Empleado> lemp)
        {
            empleados = lemp;
            dgEmpleado.ItemsSource = empleados;
        }
        public void CargarVentanaFormEmp(Empleado em)
        {

            FormEmp femp = new FormEmp(em,this);
            femp.ShowDialog();
        }
        //eventos
        private void BtAgregarEmp_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = new Empleado();
            CargarVentanaFormEmp(emp);
            
        }
        private void DgEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                empSelect = (Empleado)(dgEmpleado.SelectedItem);
                
            }
            catch { }
        }
        private void DgEmpleado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                empSelect = (Empleado)(dgEmpleado.SelectedItem);
                CargarVentanaFormEmp(empSelect);
            }
            catch { }
        }

        private void BtEditarEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (empSelect.Nombre != null)
                {
                    empSelect = (Empleado)(dgEmpleado.SelectedItem);
                    CargarVentanaFormEmp(empSelect);
                }
                else
                {
                    MessageBox.Show("seleccione un empleado");
                }
            }
            catch
            {
                MessageBox.Show("seleccione un empleado");
            }
        }

        private void BtBorrarEmp_Click(object sender, RoutedEventArgs e)
        {
            if (empSelect.EmpleadoId != 1) { 
                try
                {
                    string messageBoxText = "Estas seguro que deseas eliminar este empleado?";
                    string caption = "Word Processor";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:


                            uow.RepositorioEmpleado.eliminar(empSelect);
                            CargardgEmpleado(uow.RepositorioEmpleado.obtenerTodos());

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
                    MessageBox.Show("seleccione un empleado");
                }
            }
            else
            {
                MessageBox.Show(" no se puede borrar el empleado admin");
            }
        }
        private void BtBucarListEmp_Click(object sender, RoutedEventArgs e)
        {
            if (cbBuscarListEmp.Text == "Todos")
            {
                CargardgEmpleado(uow.RepositorioEmpleado.obtenerTodos());

            }
            else if (cbBuscarListEmp.Text == "Tipo")
            {
                try
                {
                    CargardgEmpleado(uow.RepositorioEmpleado.obtenerVarios(c => c.Tipo == tbBuscadorListEmp.Text));
                }
                catch { }
            }
            else if (cbBuscarListEmp.Text == "Titulacion")
            {
                try
                {
                    CargardgEmpleado(uow.RepositorioEmpleado.obtenerVarios(c => c.Titulacion == tbBuscadorListEmp.Text));
                }
                catch
                {
                }

            }

            tbBuscadorListEmp.Text = "";
        }
        private void BtBuscarEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Empleado aux = uow.RepositorioEmpleado.obtenerUno(c => c.Usuario == tbBuscadorEmp.Text);
                if (aux.Usuario != null)
                {
                    CargarVentanaFormEmp(aux);
                    tbBuscadorEmp.Text = "";                                                      
                }

            }
            catch
            {
                MessageBox.Show("no se ha encontrado ningun Empleado con ese nombre");
            }
        }
        #endregion
        #region Cliente
        //metodos
        public void CargardgCliente(List<Cliente> c)
        {
            Clientes = c;
            dgCliente.ItemsSource = Clientes;
            dgCliente.SelectedIndex = 0;
        }
        public void CargarVentanaFormCli(Cliente cl)
        {
            FormCli fcli = new FormCli(cl,this);
            fcli.ShowDialog();
        }
        //eventos
        private void BtAgregarCli_Click(object sender, RoutedEventArgs e)
        {
            Cliente cli = new Cliente();
            CargarVentanaFormCli(cli);
        }

        private void DgCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cliSelect=(Cliente)(dgCliente.SelectedItem);
                                        
            }
            catch
            {
               
            }
        }
        private void DgCliente_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                cliSelect = (Cliente)(dgCliente.SelectedItem);
                CargarVentanaFormCli(cliSelect);
            }
            catch { }
        }
        private void BtEditarCli_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cliSelect.Nombre != null)
                {
                    CargarVentanaFormCli(cliSelect);
                }
                else
                {
                    MessageBox.Show("Seleccione un cliente");
                }
            } catch
            {
                MessageBox.Show("seleccione un cliente");
            }
        }
        private void BtBorrarCli_Click(object sender, RoutedEventArgs e)
        {
            if (cliSelect.ClienteId != 1)
            {
                try
                {
                    string messageBoxText = "Estas seguro que deseas eliminar este cliente?";
                    string caption = "Word Processor";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:


                            uow.RepositorioCliente.eliminar(cliSelect);
                            CargardgCliente(uow.RepositorioCliente.obtenerTodos());

                            break;
                        case MessageBoxResult.No:

                            break;
                        case MessageBoxResult.Cancel:
                            // User pressed Cancel button
                            // ...
                            break;
                    }
                }
                catch(Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
            else
            {
                MessageBox.Show(" no se puede borrar el cliente por defecto");
            }
        }

        private void BtBuscarCli_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente aux = uow.RepositorioCliente.obtenerUno(c => c.Email == tbBuscadorCli.Text);
                if (aux.Email != null)
                {
                    CargarVentanaFormCli(cliSelect);
                    tbBuscadorCli.Text = "";
                }

            }
            catch
            {
                MessageBox.Show("no se ha encontrado ningun Cliente con ese email");
            }
        }

        #endregion
       

    }
}
