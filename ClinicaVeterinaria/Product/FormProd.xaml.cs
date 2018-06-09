using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Win32;

namespace ClinicaVeterinaria
{
    /// <summary>
    /// Lógica de interacción para FormProd.xaml
    /// </summary>
    public partial class FormProd : Window
    {
       

        Producto pr= new Producto();//producto local
        bool NuevoProd = false;//cambia segun venga de nuevo producto o producto seleccionado
        MainWindow main;//la mainwindows local
        Proveedor prov = new Proveedor();//proveedor del producto que lo cargaremos desde otra ventana
        bool modificado = false;//comprueba que el producto fue modificado correctamente para no reinicializar los valores
        //variables del producto cuando inicia
        string prNombreOrigen = "";
        string prMarcaOrigen="";
        string prImagenOrige = "";
        string prAnimalDirigido = "";
        double prTamañoOrigen;
        double prPesoOrigen;
        double prPrecioOrigen;
        int prStockOrigen;
        string prcategoriaOrigen="";
       

        public FormProd(Producto prod,MainWindow mw)
        {

            InitializeComponent();
            pr = prod;//el producto que paso por parametro lo asigno a una variable local
            GuardarrValoresProdEntrada();
            main = mw;//asigno a una variable local la main window que paso por parametro   
            gridProductoSelect.DataContext = pr;
           
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

        private void BtGuardarProd_Click(object sender, RoutedEventArgs e)
        {
            if (Validado(pr)) { 
                if (NuevoProd) {
                    Producto prodAux = new Producto();
                    prodAux = MainWindow.uow.RepositorioProducto.obtenerUno(c => c.NombreProducto == pr.NombreProducto && c.NombreMarca == pr.NombreMarca);
                    if (prodAux == null)
                    {
                        try
                        {
                            MainWindow.uow.RepositorioProducto.crear(pr);
                            MessageBox.Show("se ha guardado correctamente el producto");
                            modificado = true;
                            main.CargardgProductos(MainWindow.uow.RepositorioProducto.obtenerTodos());
                            main.CargarTPVproductos_todos("Todos");
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                            RecuperarValoresProdEntrada();
                        }
                    }
                    else
                    {
                        MessageBox.Show("existe un producto con el mismo nombre y marca ya registrado");
                        RecuperarValoresProdEntrada();
                    }
                }
                //modificar
                else
                {
                    Producto prodAux = new Producto();
                    prodAux = MainWindow.uow.RepositorioProducto.obtenerUno(c => c.NombreProducto == pr.NombreProducto && c.NombreMarca == pr.NombreMarca && c.ProductoId!=pr.ProductoId);
                    if (prodAux == null)
                    {
                        try
                        {

                            MainWindow.uow.RepositorioProducto.actualizar(pr);
                            MessageBox.Show("se ha modificado correctamente el producto");
                            main.CargarTPVproductos_todos("Todos");
                            modificado = true;
                            main.CargardgProductos(MainWindow.uow.RepositorioProducto.obtenerTodos());

                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                            RecuperarValoresProdEntrada();
                        }
                    }
                    else
                    {
                        MessageBox.Show("existe un producto con el mismo nombre y marca ya registrado");
                        RecuperarValoresProdEntrada();

                    }
                }
            }
            else { }
        }

        private void BtImgProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != "")
                {
                    if (Regex.IsMatch(openFileDialog.FileName, @".(jpg|bmp|png)$"))
                    {
                        string directorioImagen = openFileDialog.SafeFileName;

                        string directorioImagenOrigen = openFileDialog.FileName;

                        string directorioImagenDestino = @"Imagenes\" + directorioImagen;


                        FileInfo f = new FileInfo(directorioImagen);

                        if (!File.Exists(Environment.CurrentDirectory + "\\" + directorioImagenDestino))
                        {
                            File.Copy(directorioImagenOrigen, directorioImagenDestino);
                            pr.Imagen = directorioImagenDestino;
                            imgProd.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + directorioImagenDestino));
                        }
                        else
                        {
                            pr.Imagen = directorioImagenDestino;
                            imgProd.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + directorioImagenDestino));

                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("La imagen no es correcta. Debe estar en formato JPG, BMP, PNG");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("No se ha cargado ninguna imagen");
                }

            }
            catch { }
        }

        private void BtEliminarProd_Click(object sender, RoutedEventArgs e)
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


                        MainWindow.uow.RepositorioProducto.eliminar(pr);
                        main.CargardgProductos(MainWindow.uow.RepositorioProducto.obtenerTodos());
                        this.Close();
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
                MessageBox.Show("Error no se ha podido borrar este producto");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (modificado == false)
            {
                RecuperarValoresProdEntrada();
            }
            main.CargardgProductos(MainWindow.uow.RepositorioProducto.obtenerTodos());
        }


        private void BtBuscarProv_Click(object sender, RoutedEventArgs e)
        {
            BuscadorProv bp = new BuscadorProv(pr);
            bp.ShowDialog();
        }
        public void GuardarrValoresProdEntrada()
        {

            prNombreOrigen = pr.NombreProducto;
            prMarcaOrigen = pr.NombreMarca;
            prImagenOrige = pr.Imagen;
            prAnimalDirigido = pr.AnimalDirigido;
            prTamañoOrigen = pr.Tamaño;
            prPesoOrigen = pr.Peso;
            prPrecioOrigen = pr.Precio;
            prStockOrigen = pr.Stock;
            prcategoriaOrigen = pr.Categoria;
        }
        public void RecuperarValoresProdEntrada()
        {

            pr.NombreProducto= prNombreOrigen;
            pr.NombreMarca = prMarcaOrigen;
            pr.Imagen = prImagenOrige;
            pr.AnimalDirigido = prAnimalDirigido;
            pr.Tamaño = prTamañoOrigen;
            pr.Peso = prTamañoOrigen;
            pr.Precio = prPrecioOrigen;
            pr.Stock = prStockOrigen;
            pr.Categoria = prcategoriaOrigen;
        }




        //para devolver los valores con los que entro el producto a la ventana

    }
}
