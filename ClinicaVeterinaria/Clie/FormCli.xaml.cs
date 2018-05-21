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
namespace ClinicaVeterinaria.Clie
{
    /// <summary>
    /// Lógica de interacción para FormCli.xaml
    /// </summary>
    public partial class FormCli : Window
    {
        UnityOfWork uow;

        Cliente cli = new Cliente();//empleado local
        bool NuevoCli = false;//cambia segun venga de nuevo empleado o empleado seleccionado
        MainWindow main = new MainWindow();//la mainwindows local
        bool modificado = false;//comprueba que el empleado fue modificado correctamente para no reinicializar los valores
        //variables que recuperar cuando  la modificacion no se realiza correctamente
        public FormCli(Cliente cl, UnityOfWork uw, MainWindow mw)
        {
            InitializeComponent();
            cli = cl;
            main = mw;
            uow = uw;
            gridClienteSelect.DataContext = cli;
            if (cli.Nombre == null)
            {
                btEliminarCli.Visibility = Visibility.Hidden;
                NuevoCli = true;
            }
            else
            {
                btGuardarCli.Content = "modificar";


            }
        }
    }
}
