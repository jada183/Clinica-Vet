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

        public GraficosVentas()
        {
            InitializeComponent();
            CargarGraficoMayoresVent();
        }
       public void CargarGraficoMayoresVent()
        {
            List<Producto> productos = MainWindow.uow.RepositorioProducto.obtenerVarios(c => c.Habilitado == true);

            // Create the plot model
            var tmp = new PlotModel { Title = "Bar series", LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };

            // Add the axes, note that MinimumPadding and AbsoluteMinimum should be set on the value axis.
            tmp.Axes.Add(new CategoryAxis { Position = AxisPosition.Left, ItemsSource = productos, LabelField = "NombreProducto" });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, AbsoluteMinimum = 0 });

            // Add the series, note that the BarSeries are using the same ItemsSource as the CategoryAxis.
            tmp.Series.Add(new BarSeries { Title = "Ventas", ItemsSource = productos, ValueField = "LineasVentas.Count" });

            this.GraficoMayoresVentas = tmp;

            tabGrafico1.DataContext = this;
        }
        
    }
}
