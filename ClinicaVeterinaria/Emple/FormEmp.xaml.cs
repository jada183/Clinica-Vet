using ClinicaVeterinaria.DAL;
using ClinicaVeterinaria.MODEL;
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

namespace ClinicaVeterinaria.Emple
{
    /// <summary>
    /// Lógica de interacción para FormEmp.xaml
    /// </summary>
    public partial class FormEmp : Window
    {
        UnityOfWork uow;

        Empleado em = new Empleado();//empleado local
        bool NuevoEmp = false;//cambia segun venga de nuevo empleado o empleado seleccionado
        MainWindow main = new MainWindow();//la mainwindows local
        bool modificado = false;//comprueba que el empleado fue modificado correctamente para no reinicializar los valores


        public FormEmp(Empleado emp, UnityOfWork uw, MainWindow mw)
        {
            InitializeComponent();
            em = emp;//el producto que paso por parametro lo asigno a una variable local
          
            main = mw;//asigno a una variable local la main window que paso por parametro
            uow = uw;//la unity que deben tener en comun ambas ventanas

            gridEmpleadoSelect.DataContext = em;
            if (em.Usuario == null)
            {
                btEliminarEmp.Visibility = Visibility.Hidden;
                NuevoEmp = true;
            }
            else
            {
                btGuardarEmp.Content = "modificar";


            }
        }
    }
}
