using ClinicaVeterinaria.MODEL;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ClinicaVeterinaria.Ventaa
{
    /// <summary>
    /// Lógica de interacción para GraficosVentas.xaml
    /// </summary>
    public partial class GraficosVentas : Window
    {
        public PlotModel GraficoMayoresVentas { get; set; }
        public PlotModel GraficoMenoresVentas { get; set; }
        public PlotModel GraficoVentasEmpleado { get; set; }
        public PlotModel GraficoComprascliente { get; set; }
        public GraficosVentas()
        {
            InitializeComponent();
            CargarGraficoMayoresVent();
            CargarGraficoMenoresVent();
            CargarGraficoEmpVentas();
            CargarGraficoCliCompras();
        }
       public void CargarGraficoMayoresVent()
        {
            List<Producto> productos = MainWindow.uow.RepositorioProducto.obtenerVarios(c => c.Habilitado == true);
            List<Producto> productoOrdenados = productos.OrderByDescending(c => c.LineasVentas.Count).ToList();
            List<Producto> MasVendidos = new List<Producto>();
            for(int i = 0; i < 5; i++)
            {
                MasVendidos.Add(productoOrdenados[i]);
            }
            // Create the plot model
            try
            {
               
                var tmp = new PlotModel { Title = "Productos que aparecen en mas ventas", LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };

                // Add the axes, note that MinimumPadding and AbsoluteMinimum should be set on the value axis.
                tmp.Axes.Add(new CategoryAxis {  ItemsSource = MasVendidos, LabelField = "NombreProducto"});
                tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, AbsoluteMinimum = 0 });

                // Add the series, note that the BarSeries are using the same ItemsSource as the CategoryAxis.
                tmp.Series.Add(new ColumnSeries { Title = "Ventas", ItemsSource = MasVendidos, ValueField = "LineasVentas.Count" });

                this.GraficoMayoresVentas = tmp;

                gridGrafico1.DataContext = this;
            }
            catch { }
        }

        public void CargarGraficoMenoresVent()
        {
            List<Producto> productos = MainWindow.uow.RepositorioProducto.obtenerVarios(c => c.Habilitado == true);
            List<Producto> productoOrdenados = productos.OrderBy(c => c.LineasVentas.Count).ToList();
            List<Producto> MenosVendidos = new List<Producto>();
            for (int i = 0; i < 5; i++)
            {
                MenosVendidos.Add(productoOrdenados[i]);
            }
            try
            {
                

                var tmp = new PlotModel { Title = "productos que aparecen en menos ventas", LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };
                
                // Add the axes, note that MinimumPadding and AbsoluteMinimum should be set on the value axis.
                tmp.Axes.Add(new CategoryAxis { ItemsSource = MenosVendidos, LabelField = "NombreProducto" });
                tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, AbsoluteMinimum = 0 });

                // Add the series, note that the BarSeries are using the same ItemsSource as the CategoryAxis.
                tmp.Series.Add(new ColumnSeries { Title = "Ventas", ItemsSource = MenosVendidos, ValueField = "LineasVentas.Count" });

                this.GraficoMenoresVentas = tmp;

                GraficoMenoresVentas.DefaultColors = new List<OxyColor>
                {
                    OxyColors.Red
                    
                };
                gridGrafico2.DataContext = this;
            }
            catch { }
        }

        public void CargarGraficoEmpVentas()
        {
            List<Empleado> empleados = MainWindow.uow.RepositorioEmpleado.obtenerVarios(c => c.Habilitado == true);
            
            try
            {
                var tmp = new PlotModel { Title = "Ventas de cada empleado", LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };

                // Add the axes, note that MinimumPadding and AbsoluteMinimum should be set on the value axis.
                tmp.Axes.Add(new CategoryAxis { Position = AxisPosition.Left, ItemsSource = empleados, LabelField = "Usuario" });
                tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, AbsoluteMinimum = 0 });

                // Add the series, note that the BarSeries are using the same ItemsSource as the CategoryAxis.
                tmp.Series.Add(new BarSeries { Title = "Ventas", ItemsSource = empleados, ValueField = "Ventas.Count" });

                this.GraficoVentasEmpleado = tmp;

                GraficoVentasEmpleado.DefaultColors = new List<OxyColor>
                {
                    OxyColor.FromRgb(67,31,137)

                };
                gridGrafico3.DataContext = this;
            }
            catch { }

        }
        public void CargarGraficoCliCompras()
        {
            int cont=0;
            List<Cliente> clientes = MainWindow.uow.RepositorioCliente.obtenerVarios(c => c.Habilitado == true);
            try { 
                clientes.OrderByDescending(c => c.Compras.Count).ToList();
                var plotModel = new PlotModel { Title = "Clientes que más compran" };
                var pieSeries = new PieSeries();
                foreach(Cliente cl in clientes)
                {
                    if (cl.Compras.Count > 0)
                    {
                        pieSeries.Slices.Add(new PieSlice(cl.Nombre + "-" + cl.Email, cl.Compras.Count) { IsExploded = true });
                    }
                    
                    cont++;
                    if (cont == 5)
                    {
                        break;
                    }
                }
                pieSeries.InnerDiameter = 0.2;
                pieSeries.ExplodedDistance = 0;
                pieSeries.Stroke = OxyColors.Black;
                pieSeries.StrokeThickness = 1.0;
                pieSeries.AngleSpan = 360;
                pieSeries.StartAngle = 0;
                plotModel.Series.Add(pieSeries);
                this.GraficoComprascliente = plotModel;
                gridGrafico4.DataContext = this;
            }
            catch { }
        }
    }
}
