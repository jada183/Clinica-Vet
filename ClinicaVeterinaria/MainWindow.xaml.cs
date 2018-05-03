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

namespace ClinicaVeterinaria
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class Producto
        {
            string nombre;
            double precio;
            string marca;

            public string Nombre { get => nombre; set => nombre = value; }
            public double Precio { get => precio; set => precio = value; }
            public string Marca { get => marca; set => marca = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            List<Producto> listp = new List<Producto>();
            for(int i =0; i < 10; i++)
            {
                Producto p = new Producto();
                if(i % 2 ==0)
                {
                    p.Nombre = "Correa extensible";
                    p.Precio = 5.20;
                    p.Marca = "wiskas";
                }
                else
                {
                    p.Nombre = "Pienso";
                    p.Precio = 8.20;
                    p.Marca = "Purina";
                }
                listp.Add(p);
            }
            dgProd.ItemsSource = listp;
        }

    
    }
}
