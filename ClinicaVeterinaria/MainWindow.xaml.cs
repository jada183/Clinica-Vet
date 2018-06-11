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
using ClinicaVeterinaria.Citass;
using System.ComponentModel.DataAnnotations;

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

        //variables usadas en citas
        private List<Cita> Citas = new List<Cita>();
        private List<Cita> CitasAt = new List<Cita>();
        private Cita citaSelect = new Cita();
        private Cita citaSelectAten = new Cita();

        //variables para la gestion de las ventas
        private List<Venta> ventas = new List<Venta>();
        private List<LineaVenta> lineasVenta = new List<LineaVenta>();
        private Venta ventaSelect = new Venta();

        //variables para tpv
        Venta venta = new Venta();
        Cliente cliTPV= new Cliente();
        public Producto productoTPV { get; set; }
        private double total = 0;
        //empleado accceso
        Empleado EmpActual = new Empleado();
        public MainWindow()
        {
            InitializeComponent();
            //carga de datos inicial    
            CargardgProductos(uow.RepositorioProducto.obtenerVarios(c=> c.Habilitado==true));
            CargardgServicio(uow.RepositorioServicio.obtenerVarios(c=>c.Habilitado==true));
            CargardgProveedor(uow.RepositorioProveedor.obtenerVarios(c=>c.Habilitado==true));
            CargardgEmpleado(uow.RepositorioEmpleado.obtenerVarios(c=>c.Habilitado==true));
            CargardgCliente(uow.RepositorioCliente.obtenerVarios(c=>c.Habilitado==true));
            CargardgCitas(uow.RepositorioCita.obtenerVarios(c=>c.Atendida==false));
            CargardgCitasAtendidas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == true));
            CargarDgVenta(uow.RepositorioVenta.obtenerTodos());
            //temporal 
            EmpActual = uow.RepositorioEmpleado.obtenerUno(c => c.EmpleadoId == 2);
            //hago binding del empleado logueado
            gridEmpleadoActual.DataContext = EmpActual;
            //carga tpv

            CargarTPVproductos_todos("Todos");
            CargarcbClientTPV();
        }
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

                        prodSelect.Habilitado = false;
                        uow.RepositorioProducto.actualizar(prodSelect);                     
                        CargardgProductos(uow.RepositorioProducto.obtenerVarios(c=> c.Habilitado==true));
                        dgProd.SelectedIndex = 0;
                        CargarTPVproductos_todos("Todos");
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
        private void BtBucarList_Click(object sender, RoutedEventArgs e)
        {
            if (cbBuscarListProd.Text == "Todos")
            {
                CargardgProductos(uow.RepositorioProducto.obtenerVarios(c=>c.Habilitado==true));

            }
            else if (cbBuscarListProd.Text == "Marca")
            {
                try
                {
                    CargardgProductos(uow.RepositorioProducto.obtenerVarios(c => c.NombreMarca == tbBuscadorList.Text && c.Habilitado==true ));
                }
                catch { }
            }
            else if (cbBuscarListProd.Text == "Animal dirigido")
            {
                try
                {
                    CargardgProductos(uow.RepositorioProducto.obtenerVarios(c => c.AnimalDirigido == tbBuscadorList.Text && c.Habilitado==true));
                }
                catch
                {
                }

            }
            else if (cbBuscarListProd.Text == "Sin stock")
            {
                try
                {
                    CargardgProductos(uow.RepositorioProducto.obtenerVarios(c => c.Stock == 0 && c.Habilitado==true));
                }
                catch { }
            }
            else if (cbBuscarListProd.Text == "Categoria")
            {
                try
                {
                    CargardgProductos(uow.RepositorioProducto.obtenerVarios(c => c.Categoria == tbBuscadorList.Text && c.Habilitado==true));
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
                        serviSelect.Habilitado = false;
                        uow.RepositorioServicio.actualizar(serviSelect);
                        CargardgServicio(uow.RepositorioServicio.obtenerVarios(c => c.Habilitado == true));
                        dgServ.SelectedIndex = 0;

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

                        provSelect.Habilitado = false;
                        List<Producto> lprod = MainWindow.uow.RepositorioProducto.obtenerVarios(c => c.ProveedorId == provSelect.ProveedorId);
                        foreach (Producto p in lprod)
                        {
                            p.ProveedorId = null;
                            p.Proveedor = null;
                            uow.RepositorioProducto.actualizar(p);
                        }
                        uow.RepositorioProveedor.actualizar(provSelect);
                        CargardgProveedor(uow.RepositorioProveedor.obtenerVarios(c=>c.Habilitado==true));
                        dgProv.SelectedIndex = 0;

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

            FormEmp femp = new FormEmp(em,this,EmpActual.EmpleadoId);
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
            if (empSelect.EmpleadoId != 1 && empSelect.EmpleadoId!=EmpActual.EmpleadoId) {
                List<Cita> lcita = uow.RepositorioCita.obtenerVarios(c => c.EmpleadoId == empSelect.EmpleadoId && c.Atendida == false);
                if (lcita.Count > 0)
                {
                    MessageBox.Show("cuidado vas a eliminar un empleado con una cita sin atender");
                }
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

                            empSelect.Habilitado = false;
                            uow.RepositorioEmpleado.actualizar(empSelect);
                            uow.RepositorioHorario.eliminarVarios(c => c.EmpleadoId == empSelect.EmpleadoId);
                           
                            //elimino las citas del empleado que no han sido atendidas
                            uow.RepositorioCita.eliminarVarios(c => c.EmpleadoId == empSelect.EmpleadoId && c.Atendida==false);

                            CargardgEmpleado(uow.RepositorioEmpleado.obtenerVarios(c=>c.Habilitado==true));
                            CargardgCitas(uow.RepositorioCita.obtenerVarios(c => c.Atendida==false));
                            CargardgCitasAtendidas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == true));
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
                MessageBox.Show(" no se puede borrar el empleado admin ni el empleado logueado");
            }
        }
        private void BtBucarListEmp_Click(object sender, RoutedEventArgs e)
        {
            if (cbBuscarListEmp.Text == "Todos")
            {
                CargardgEmpleado(uow.RepositorioEmpleado.obtenerVarios(c=>c.Habilitado==true));

            }
            else if (cbBuscarListEmp.Text == "Tipo")
            {
                try
                {
                    CargardgEmpleado(uow.RepositorioEmpleado.obtenerVarios(c => c.Tipo == tbBuscadorListEmp.Text && c.Habilitado==true));
                }
                catch { }
            }
            else if (cbBuscarListEmp.Text == "Titulacion")
            {
                try
                {
                    CargardgEmpleado(uow.RepositorioEmpleado.obtenerVarios(c => c.Titulacion == tbBuscadorListEmp.Text && c.Habilitado==true));
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
                    string messageBoxText = "Estas seguro que deseas eliminar este cliente con sus mascotas, algunas de estas podrian tener citas pendientes que se eliminaran?";
                    string caption = "Word Processor";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:

                            //eliminado en cascada
                            cliSelect.Habilitado = false;
                            uow.RepositorioCliente.actualizar(cliSelect);
                            List<Paciente> pac12 = uow.RepositorioPaciente.obtenerVarios(c => c.ClienteId == cliSelect.ClienteId);

                            foreach(Paciente p in pac12)
                            {
                                p.Habilitado = false;
                                uow.RepositorioPaciente.actualizar(p);
                                uow.RepositorioCita.eliminarVarios(c => c.PacienteId == p.PacienteId && c.Atendida==false);
                            }

                            CargardgCitas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == false));


                            //recargo las tablas que lo necesiten
                            CargardgCliente(uow.RepositorioCliente.obtenerVarios(c=>c.Habilitado==true));
                            CargarcbClientTPV();

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
                    MessageBox.Show(erro.InnerException.Message);
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
        #region Cita
        //metodos
        public void CargardgCitas(List<Cita> lcit)
        {
            Citas = lcit;
            dgCita.ItemsSource = Citas;
        }
        public void CargardgCitasAtendidas(List<Cita> lcit)
        {
            CitasAt = lcit;
            dgCitasAtendidas.ItemsSource = CitasAt;
        }
        public void CargarVentanaFormCita(Cita cita)
        {
            FormCita fcit = new FormCita(cita,this);
            fcit.ShowDialog();
        }
        private void BtAgregarCita_Click(object sender, RoutedEventArgs e)
        {
            Cita cit = new Cita();
            CargarVentanaFormCita(cit);
        }
        private void DgCita_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                citaSelect = (Cita)(dgCita.SelectedItem);

            }
            catch
            {

            }
        }
        private void DgCita_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                citaSelect = (Cita)(dgCita.SelectedItem);
                CargarVentanaFormCita(citaSelect);
            }
            catch
            {

            }
        }

        private void BtEliminarCita_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar esta cita?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:


                        uow.RepositorioCita.eliminar(citaSelect);
                        CargardgCitas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == false));

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
                MessageBox.Show("seleccione una cita");
            }
        }
        private void BtCheckear_Click(object sender, RoutedEventArgs e)
        {
            if(citaSelect != null)
            {
                try
                {
                    string messageBoxText = "Estas seguro que deseas marcar como atendida esta cita?";
                    string caption = "Word Processor";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:


                            citaSelect.Atendida = true;
                            MainWindow.uow.RepositorioCita.actualizar(citaSelect);
                            CargardgCitas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == false));
                            CargardgCitasAtendidas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == true));

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
                    MessageBox.Show("seleccione una cita");
                }
               
            }
        }
        private void BtEditarCit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                CargarVentanaFormCita(citaSelect);
            }
            catch
            {
                MessageBox.Show("Seleccione una cita");
            }
        }
        // buscador de citas
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (cbBuscarListCit.Text == "Usuario empleado")
                {
                    Empleado empaux = uow.RepositorioEmpleado.obtenerUno(c => c.Usuario == tbBuscarListCit.Text);
                    if (empaux.Usuario != null)
                    {
                        if (dpFechaBusCita.SelectedDate != null)
                        {
                            CargardgCitas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == false && c.Fecha == dpFechaBusCita.SelectedDate && c.EmpleadoId == empaux.EmpleadoId));
                        }
                        else
                        {
                            CargardgCitas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == false && c.EmpleadoId == empaux.EmpleadoId));

                        }
                    }
                }
                else
                {
                    if (dpFechaBusCita.SelectedDate != null)
                    {
                        CargardgCitas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == false && c.Fecha == dpFechaBusCita.SelectedDate));
                    }
                    else
                    {
                        CargardgCitas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == false));

                    }
                }


            }
            catch
            {
                MessageBox.Show("no se ha encontrado ninguna cita");
            }
        }

        private void BtBuscadorCitaAten_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbBuscarListCitAten.Text == "Usuario empleado")
                {
                    Empleado empaux = uow.RepositorioEmpleado.obtenerUno(c => c.Usuario == tbBuscarListCitAten.Text);
                    if (empaux.Usuario != null)
                    {
                        if (dpFechaBusCitaAten.SelectedDate != null)
                        {
                            CargardgCitasAtendidas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == true && c.Fecha == dpFechaBusCitaAten.SelectedDate && c.EmpleadoId == empaux.EmpleadoId));
                        }
                        else
                        {
                            CargardgCitasAtendidas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == true && c.EmpleadoId == empaux.EmpleadoId));

                        }
                    }
                }
                else
                {
                    if (dpFechaBusCitaAten.SelectedDate != null)
                    {
                        CargardgCitasAtendidas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == true && c.Fecha == dpFechaBusCitaAten.SelectedDate));
                    }
                    else
                    {
                        CargardgCitasAtendidas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == true));

                    }
                }


            }
            catch
            {
                MessageBox.Show("no se ha encontrado ninguna cita");
            }
        }
        private void BtEliminarCitaAtendida_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar esta cita?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:


                        uow.RepositorioCita.eliminar(citaSelectAten);
                        CargardgCitasAtendidas(uow.RepositorioCita.obtenerVarios(c => c.Atendida == true));

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
                MessageBox.Show("seleccione una cita");
            }
        }

        private void DgCitasAtendidas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                citaSelectAten = (Cita)(dgCitasAtendidas.SelectedItem);
            }
            catch
            {

            }
        }

        #endregion
        #region Venta
        //metodos
        public void CargarDgVenta(List<Venta> lv)
        {
            ventas = lv;
            DgVentas.ItemsSource = ventas;

        }
        public void CargarDgLineasVenta(List<LineaVenta> lnv)
        {
            lineasVenta = lnv;
            DgLineasVentaV.ItemsSource = lineasVenta;
        }
        //eventos
        private void DgVentas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ventaSelect = (Venta)(DgVentas.SelectedItem);
                expanderLineasVentaV.IsExpanded = true;
                CargarDgLineasVenta(uow.RepositorioLineaVenta.obtenerVarios(c => c.VentaId == ventaSelect.VentaId));
            }
            catch
            {

            }

        }
        private void BtBucarListVenta_Click(object sender, RoutedEventArgs e)
        {
            if (cbBuscarListVen.Text == "Todos")
            {
                try
                {
                    if (DpFechaBuscVent.SelectedDate == null)
                    {
                        CargarDgVenta(uow.RepositorioVenta.obtenerTodos());
                        DgVentas.SelectedIndex = 0;
                    }
                    else
                    {
                        CargarDgVenta(uow.RepositorioVenta.obtenerVarios(c=> c.FechaVenta==DpFechaBuscVent.SelectedDate));
                        DgVentas.SelectedIndex = 0;
                    }
                
                }
                catch
                {

                }

            }
            else if(cbBuscarListVen.Text == "Usuario empleado")
            {
                try
                {
                    Empleado empaux = uow.RepositorioEmpleado.obtenerUno(c => c.Usuario == tbBuscadorListVen.Text);
                    if (empaux.Usuario != null)
                    {
                        if (DpFechaBuscVent.SelectedDate == null)
                        {
                            CargarDgVenta(uow.RepositorioVenta.obtenerVarios(c => c.EmpleadoId == empaux.EmpleadoId));
                            DgVentas.SelectedIndex = 0;
                        }
                        else
                        {
                            CargarDgVenta(uow.RepositorioVenta.obtenerVarios(c => c.FechaVenta == DpFechaBuscVent.SelectedDate && c.EmpleadoId == empaux.EmpleadoId));
                            DgVentas.SelectedIndex = 0;
                        }  
                    }
                }
                catch
                {

                }
            }
            else if(cbBuscarListVen.Text== "Email cliente")
            {
                try
                {
                    Cliente cliaux = uow.RepositorioCliente.obtenerUno(c => c.Email == tbBuscadorListVen.Text);
                    if (cliaux.Email != null)
                    {
                        if (DpFechaBuscVent.SelectedDate == null)
                        {
                            CargarDgVenta(uow.RepositorioVenta.obtenerVarios(c => c.ClienteId == cliaux.ClienteId));
                            DgVentas.SelectedIndex = 0;
                        }
                        else
                        {
                            CargarDgVenta(uow.RepositorioVenta.obtenerVarios(c => c.FechaVenta == DpFechaBuscVent.SelectedDate && c.ClienteId==cliaux.ClienteId));
                            DgVentas.SelectedIndex = 0;
                        }   
                    }
                }
                catch
                {

                }
            }

        }
        private void BtElimarVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = "Estas seguro que deseas eliminar esta venta?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:


                        uow.RepositorioVenta.eliminar(ventaSelect);
                        CargarDgVenta(uow.RepositorioVenta.obtenerTodos());
                        DgVentas.SelectedIndex = 0;
                        CargarDgLineasVenta(uow.RepositorioLineaVenta.obtenerVarios(c => c.VentaId == ventaSelect.VentaId));
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
                MessageBox.Show("seleccione una cita");
            }
        }
        #endregion
        #region TPV
        public void CargarcbClientTPV()
        {
            cbClientTPV.ItemsSource = uow.RepositorioCliente.obtenerVarios(c=>c.Habilitado==true);
        }

        public void CargarTPVproductos_todos(String categoria)
        {
          
            List<Producto> list = new List<Producto>();
            if (categoria != "Todos")
            {
                 list = uow.RepositorioProducto.obtenerVarios(c => c.Categoria == categoria && c.Habilitado==true);
                lListProd.Content = categoria+"s";
            }
            else
            {
                list = uow.RepositorioProducto.obtenerVarios(c=>c.Habilitado==true);
                lListProd.Content = "Todos los productos";
            }
            //cliTPV = (Cliente)cbClientTPV.SelectedItem;



            tpv_dock_productos.Children.Clear();

            foreach (Producto prod in list)
            {



                StackPanel stackp = new StackPanel();

                stackp.Orientation = Orientation.Vertical;
                stackp.VerticalAlignment = VerticalAlignment.Center;
                stackp.HorizontalAlignment = HorizontalAlignment.Stretch;
                stackp.Width = 155;
                stackp.Height = 135;

                Label l = new Label();
                l.Content = (prod.NombreProducto+"-"+prod.NombreMarca);
                l.FontSize = 12;

                Label lprice = new Label();
                lprice.Content = "Precio:"+Convert.ToString(prod.Precio)+"€";
                lprice.FontSize = 12;
                lprice.Background = Brushes.SkyBlue;
                Image img = new Image();

                try
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(prod.Imagen);
                    logo.EndInit();
                    img.Source = logo;
                    img.Stretch = Stretch.Fill;
                    

                    stackp.Children.Add(img);
                }
                catch { }
             
                stackp.Children.Add(l);
                stackp.Children.Add(lprice);

                Button bt = new Button();


                String btNombreSinEspacios= prod.NombreProducto.Replace(" ", "_");
                String btname= btNombreSinEspacios +"_"+Convert.ToString(prod.ProductoId);
                bt.Name = btname;
                if (prod.Stock > 0)
                {
                    bt.Background = Brushes.Lavender;
                }
                else
                {
                    bt.Background = Brushes.LightSalmon;
                }

                bt.Content = stackp;
                bt.Width = 165;
                bt.Height = 140;

                bt.Margin = new Thickness(10, 10, 10, 10);
                bt.Click += Producto_click;

                tpv_dock_productos.Children.Add(bt);

            }
        }

        private void Producto_click(object sender, RoutedEventArgs e)
        {
         
            LineaVenta lv = new LineaVenta();


            var aux = e.OriginalSource;
            if (aux.GetType() == typeof(Button))
            {
                Button b = (Button)aux;
                String btname = b.Name;

                string[] divbtName = b.Name.Split('_');
                
                int auxint = Convert.ToInt32(divbtName[(divbtName.Count()) - 1]);
                productoTPV = uow.RepositorioProducto.obtenerUno(c => auxint == c.ProductoId);

                if (productoTPV.Stock < 1)
                {
                    MessageBox.Show("El producto no tiene stock", "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                else
                {
                    venta.EmpleadoVenta = EmpActual;
                    venta.EmpleadoId = EmpActual.EmpleadoId;
                   
                    venta.FechaVenta = DateTime.Today;


                    if (venta.LineasVenta.Where(c => c.Producto.ProductoId.Equals(productoTPV.ProductoId)).FirstOrDefault() == null && venta.VentaId == 0)
                    {

                        lv = new LineaVenta();
                        lv.Producto = productoTPV;
                        lv.ProductoId = productoTPV.ProductoId;
                        lv.Cantidad = 1;
                        lv.Venta = venta;

                        venta.LineasVenta.Add(lv);

                        total += lv.Producto.Precio;
                        setTotal();

                        productoTPV.Stock--;//@@@@@@@@@@@@
                        uow.RepositorioProducto.actualizar(productoTPV);
                    }
                    else
                    {

                        LineaVenta linea = venta.LineasVenta.Where(c => c.Producto.ProductoId.Equals(productoTPV.ProductoId)).FirstOrDefault();
                        linea.Cantidad++;
                        total += linea.Producto.Precio;
                        setTotal();

                        productoTPV.Stock--;
                        uow.RepositorioProducto.actualizar(productoTPV);
                    }


                    dgridTPV.ItemsSource = "";
                    dgridTPV.ItemsSource = venta.LineasVenta.ToList();




                }
            }
        }
        public void setTotal()
        {

            TPVTotal.Content = Convert.ToString(total);
        }
        private void BTSelectCatMedicamento_Click(object sender, RoutedEventArgs e)
        {
            CargarTPVproductos_todos("Medicamento");
          
        }
        private void BTSelectCatTodos_Click(object sender, RoutedEventArgs e)
        {
            CargarTPVproductos_todos("Todos");
            
        }
        private void BTSelectCatAccesorio_Click(object sender, RoutedEventArgs e)
        {
            CargarTPVproductos_todos("Accesorio");
            
        }
        private void BTSelectCatAlimento_Click(object sender, RoutedEventArgs e)
        {
            CargarTPVproductos_todos("Alimento");
           
        }
        private void CbClientTPV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cliente cl= (Cliente)cbClientTPV.SelectedItem;
            venta.ClienteVenta = cl;
            venta.ClienteId = cl.ClienteId;
        }

        private void BtCancelarVentaTPV_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                foreach (LineaVenta l in venta.LineasVenta)
                {
                    Producto proaux = uow.RepositorioProducto.obtenerUno(c => c.ProductoId == l.ProductoId);
                    proaux.Stock += l.Cantidad;
                    uow.RepositorioProducto.actualizar(proaux);
                }
                venta = new Venta();
                total = 0;

                dgridTPV.ItemsSource = "";
                dgridTPV.ItemsSource = venta.LineasVenta.ToList();

                total = 0;
                setTotal();
                venta = new Venta();

                cbClientTPV.SelectedIndex = 0;

            }
            catch { }



        }

        private void BtConfirmarVentaTPV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (venta.LineasVenta.Count > 0)
                {
                    uow.RepositorioVenta.crear(venta);

                    MessageBox.Show("se ha realizado la venta satisfactoriamente");
                    CargarDgVenta(uow.RepositorioVenta.obtenerTodos());
                    //cbClientTPV.Text = "";

                    venta = new Venta();
                    Cliente cl=(Cliente)cbClientTPV.SelectedItem;
                    venta.ClienteVenta = cl;
                    venta.ClienteId = cl.ClienteId;
                    dgridTPV.ItemsSource = "";
                    dgridTPV.ItemsSource = venta.LineasVenta.ToList();
                    total = 0;
                    setTotal();
                    cbClientTPV.SelectedIndex = 0;
                             

                }
                else
                {
                    MessageBox.Show("no ha seleccionado ningun producto");
                }

            }
            catch
            {
                MessageBox.Show("no ha seleccionado ningun producto");
            }
        }

        #endregion
        #region Logueado
        
        //para el empleado logueado
        private void BtGuardarEmp_Click(object sender, RoutedEventArgs e)
        {
            if (Validado(EmpActual))
            {
                if (tbContraseñaEmp.Text == tbConfirmContraseñaEmp.Text)
                {
                    uow.RepositorioEmpleado.actualizar(EmpActual);
                    MessageBox.Show("se ha actualizado correctamente");
                }
                else
                {
                    MessageBox.Show("las contraseñas no coinciden");
                }
            }
            else
            {

            }
        }
        #endregion
        private void Window_Closed(object sender, EventArgs e)
        {
            //para el tpv que no guarde los cambios en caso de no ejecutar la venta
            try
            {
                foreach (LineaVenta l in venta.LineasVenta)
                {
                    Producto proaux = uow.RepositorioProducto.obtenerUno(c => c.ProductoId == l.ProductoId);
                    proaux.Stock += l.Cantidad;
                    uow.RepositorioProducto.actualizar(proaux);
                }
                venta = new Venta();
                total = 0;

                dgridTPV.ItemsSource = "";
                dgridTPV.ItemsSource = venta.LineasVenta.ToList();


                venta = new Venta();

            }
            catch { }
        }
       
    }
}
