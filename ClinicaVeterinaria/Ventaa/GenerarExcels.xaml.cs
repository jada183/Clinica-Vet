using System;
using System.Collections.Generic;
using System.IO;
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
using ClinicaVeterinaria.MODEL;
using OfficeOpenXml;

namespace ClinicaVeterinaria.Ventaa
{
    /// <summary>
    /// Lógica de interacción para GenerarExcels.xaml
    /// </summary>
    /// 
    public partial class GenerarExcels : Window
    {
        Empleado emp = new Empleado();
        public GenerarExcels(Empleado em)
        {
            InitializeComponent();
            emp = em;
        }

        private void BtCancelarExcelProd_Click(object sender, RoutedEventArgs e)
        {
            gridDatosProductos.Visibility = Visibility.Collapsed;
        }

        private void BtGenExcelVentProd_Click(object sender, RoutedEventArgs e)
        {
            gridDatosProductos.Visibility = Visibility.Visible;
        }

        private void BtGenExcelMisVent_Click(object sender, RoutedEventArgs e)
        {
            int contforeach = 2;
            List<Venta> ventas = MainWindow.uow.RepositorioVenta.obtenerVarios(c => c.EmpleadoId == emp.EmpleadoId);
            if (ventas.Count > 0)
            {

                string messageBoxText = "Estas seguro que deseas generar excel? prodria tener algun excel generarado que se sobreescribiria";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        using (ExcelPackage excel = new ExcelPackage())
                        {
                            excel.Workbook.Worksheets.Add("Ventas");
                            List<string[]> headerRow = new List<string[]>()
                            {
                               new string[] { "Nombre de cliente", "Apellidos de cliente", "Telefono de cliente","Email de cliente","Fecha  de la venta" }
                            };

                            string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                            var worksheet = excel.Workbook.Worksheets["Ventas"];

                            worksheet.Cells[headerRange].Style.Font.Bold = true;
                            worksheet.Cells[headerRange].Style.Font.Size = 14;
                            worksheet.DefaultColWidth = 25;
                            worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Purple);
                            foreach (Venta v in ventas)
                            {
                                for (int i = 1; i < 6; i++)
                                {
                                    switch (i)
                                    {

                                        case 1:
                                            worksheet.Cells[contforeach, i].LoadFromText(Convert.ToString(v.ClienteVenta.Nombre));
                                            break;
                                        case 2:
                                            worksheet.Cells[contforeach, i].LoadFromText(Convert.ToString(v.ClienteVenta.Apellidos));
                                            break;
                                        case 3:
                                            worksheet.Cells[contforeach, i].LoadFromText(Convert.ToString(v.ClienteVenta.Telefono));
                                            break;
                                        case 4:
                                            worksheet.Cells[contforeach, i].LoadFromText(Convert.ToString(v.ClienteVenta.Email));
                                            break;
                                        case 5:
                                            worksheet.Cells[contforeach, i].LoadFromText(Convert.ToString(v.FechaVenta.Date));
                                            break;
                                    }

                                }
                                contforeach++;
                            }
                            worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                            try
                            {
                                FileInfo excelFile = new FileInfo(Environment.CurrentDirectory + "\\excels\\" + emp.Nombre + ".xlsx");
                                excel.SaveAs(excelFile);
                                MessageBox.Show("se ha guardado el excel correctamente");
                            }
                            catch { }

                        }

                        break;
                    case MessageBoxResult.No:

                        break;
                    case MessageBoxResult.Cancel:
                        // User pressed Cancel button
                        // ...
                        break;
                }
            }
            else
            {
                MessageBox.Show(" no se ha encontrado ninguna venta para generar excel ");
            }
           
        }

        private void BtAceptarExcelProd_Click(object sender, RoutedEventArgs e)
        {
            int contforeach = 2;//para indicar la fila en la que se agregan los datos
            Producto p = MainWindow.uow.RepositorioProducto.obtenerUno(c => c.Habilitado == true && tbMarcaProd.Text == c.NombreMarca && c.NombreProducto == tbNombreProd.Text);
            if (p !=null)
            {
                List<LineaVenta> lv = MainWindow.uow.RepositorioLineaVenta.obtenerVarios(c => c.ProductoId == p.ProductoId);
                if (lv.Count > 0)
                {
                    string messageBoxText = "Estas seguro que deseas generar excel? prodria tener algun excel generado para ese producto que se sobreescribiria";
                    string caption = "Word Processor";
                    MessageBoxButton button = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            using (ExcelPackage excel = new ExcelPackage())
                            {
                                excel.Workbook.Worksheets.Add("Ventas");
                                List<string[]> headerRow = new List<string[]>()
                                {
                                   new string[] {"Cantidad", "Fecha  de la venta", "Nombre de cliente", "Apellidos de cliente", "Telefono de cliente","Email de cliente","Usuario del empleado" }
                                };
                                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                                var worksheet = excel.Workbook.Worksheets["Ventas"];
                                worksheet.Cells[headerRange].Style.Font.Bold = true;
                                worksheet.Cells[headerRange].Style.Font.Size = 14;
                                worksheet.DefaultColWidth = 25;
                                worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Purple);
                                foreach(LineaVenta l in lv)
                                {
                                    for(int i=1; i < 8; i++)
                                    {
                                        switch (i)
                                        {

                                            case 1:
                                                worksheet.Cells[contforeach, i].LoadFromText(Convert.ToString(l.Cantidad));
                                                break;
                                            case 2:
                                                worksheet.Cells[contforeach, i].LoadFromText(Convert.ToString((l.Venta.FechaVenta)));
                                                break;
                                            case 3:
                                                worksheet.Cells[contforeach, i].LoadFromText(l.Venta.ClienteVenta.Nombre);
                                                break;
                                            case 4:
                                                worksheet.Cells[contforeach, i].LoadFromText(l.Venta.ClienteVenta.Apellidos);
                                                break;
                                            case 5:
                                                worksheet.Cells[contforeach, i].LoadFromText(l.Venta.ClienteVenta.Telefono);
                                                break;
                                            case 6:
                                                worksheet.Cells[contforeach, i].LoadFromText(l.Venta.ClienteVenta.Email);
                                                break;
                                            case 7:
                                                worksheet.Cells[contforeach, i].LoadFromText(l.Venta.EmpleadoVenta.Usuario);
                                                break;
                                        }
                                    }
                                    contforeach++;
                                }
                                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                                try
                                {
                                    FileInfo excelFile = new FileInfo(Environment.CurrentDirectory + "\\excels\\" + p.NombreProducto+"-"+p.NombreMarca + ".xlsx");
                                    excel.SaveAs(excelFile);
                                    MessageBox.Show("se ha guardado el excel correctamente");
                                    gridDatosProductos.Visibility = Visibility.Collapsed;
                                }
                                catch { }
                            }

                            break;
                        case MessageBoxResult.No:

                            break;
                        case MessageBoxResult.Cancel:
                            // User pressed Cancel button
                            // ...
                            break;
                    }
                }
            }
            else{
                MessageBox.Show("no se ha encontrado ningun producto con esas especificaciones");
            }
        }
    }
}
