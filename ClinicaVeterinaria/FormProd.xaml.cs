using System;
using System.Collections.Generic;
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
        UnityOfWork uow;
        Producto pr= new Producto();//producto local
        bool NuevoProd = false;//cambia segun venga de nuevo producto o producto seleccionado
        MainWindow main = new MainWindow();//la mainwindows local
        Proveedor prov = new Proveedor();//proveedor del producto que lo cargaremos desde otra ventana
        //variables del producto
        string prNombreOrigen = "";
        string prMarcaOrigen="";

        public FormProd(Producto prod, UnityOfWork uw, MainWindow mw)
        {

            InitializeComponent();
            pr = prod;//el producto que paso por parametro lo asigno a una variable local
            prNombreOrigen = pr.NombreProducto;
            prMarcaOrigen = pr.NombreMarca;
            main = mw;//asigno a una variable local la main window que paso por parametro
            uow = uw;//la unity que deben tener en comun ambas ventanas
         
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

        private void BtGuardarProd_Click(object sender, RoutedEventArgs e)
        {
            //guardar
            if (NuevoProd) {
                Producto prodAux = new Producto();
                prodAux = uow.RepositorioProducto.obtenerUno(c => c.NombreProducto == pr.NombreProducto && c.NombreMarca == pr.NombreMarca);
                if (prodAux == null)
                {
                    try
                    {

                        uow.RepositorioProducto.crear(pr);
                        MessageBox.Show("se ha guardado correctamente el producto");
                        main.CargardgProductos(uow.RepositorioProducto.obtenerTodos());

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                    }
                }
                else
                {
                    MessageBox.Show("existe un producto con el mismo nombre y marca ya registrado");
                }
            }
            //modificar
            else
            {
                Producto prodAux = new Producto();
                prodAux = uow.RepositorioProducto.obtenerUno(c => c.NombreProducto == pr.NombreProducto && c.NombreMarca == pr.NombreMarca && c.ProductoId!=pr.ProductoId);
                if (prodAux == null)
                {
                    try
                    {

                        uow.RepositorioProducto.actualizar(pr);
                        MessageBox.Show("se ha modificado correctamente el producto");
                        main.CargardgProductos(uow.RepositorioProducto.obtenerTodos());

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                      
                    }
                }
                else
                {
                    MessageBox.Show("existe un producto con el mismo nombre y marca ya registrado");
                    pr.NombreProducto = pr.NombreProducto;
                    pr.NombreMarca = pr.NombreMarca;
                }
            }
        }

        private void btImgProd_Click(object sender, RoutedEventArgs e)
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

        private void btEliminarProd_Click(object sender, RoutedEventArgs e)
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


                        uow.RepositorioProducto.eliminar(pr);
                        main.CargardgProductos(uow.RepositorioProducto.obtenerTodos());
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
            main.CargardgProductos(uow.RepositorioProducto.obtenerTodos());
        }

        private void BtBucarProv_Click(object sender, RoutedEventArgs e)
        {
            BuscadorProv bp = new BuscadorProv(pr,uow);
            bp.Show();
        }




        //para devolver los valores con los que entro el producto a la ventana

    }
}
