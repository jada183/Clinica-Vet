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
using ClinicaVeterinaria.MODEL;
using ClinicaVeterinaria.DAL;
namespace ClinicaVeterinaria.Clie
{
    /// <summary>
    /// Lógica de interacción para FormMas.xaml
    /// </summary>
    public partial class FormMas : Window
    {
        UnityOfWork uow;
        Paciente pac = new Paciente();
        bool NuevoPac = false;
        FormCli formcli;
        bool modificado = false;
       
        public FormMas(Paciente p,FormCli fcli,UnityOfWork uw)
        {
            InitializeComponent();
            pac = p;
            formcli = fcli;
            gridMascota.DataContext = pac;
            uow = uw;
            if (pac.PacienteId == 0)
            {
                BtEliminarPac.Visibility = Visibility.Hidden;
                NuevoPac = true;
                pac.FechaNacimiento = DateTime.Today;
            }
            else
            {
                BtGuardarPac.Content = "modificar";


            }
        }

        private void BtGuardarPac_Click(object sender, RoutedEventArgs e)
        {
            if (NuevoPac)
            {
                Paciente pacAux = new Paciente();
                pacAux = uow.RepositorioPaciente.obtenerUno(c => c.Nombre == pac.Nombre && c.ClienteId==pac.ClienteId);
                if (pacAux == null)
                {
                    try
                    {

                        uow.RepositorioPaciente.crear(pac);
                        MessageBox.Show("se ha guardado correctamente la  mascota");
                        modificado = true;
                        formcli.CargarDgMascotas(uow.RepositorioPaciente.obtenerVarios(c=> c.ClienteId==pac.ClienteId));

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                        //RecuperarValoresProdEntrada();
                    }
                }
                else
                {
                    MessageBox.Show("existe una mascota con ese nombre registrado");
                    //RecuperarValoresProdEntrada();
                }
            }
            //modificar
            else
            {
                Paciente pacAux = new Paciente();
                pacAux = uow.RepositorioPaciente.obtenerUno(c => c.Nombre == pac.Nombre && c.ClienteId == pac.ClienteId && pac.PacienteId != c.PacienteId);
                if (pacAux == null)
                {
                    try
                    {

                        uow.RepositorioPaciente.actualizar(pac);
                        MessageBox.Show("se ha modificado correctamente la mascota");
                        modificado = true;
                        formcli.CargarDgMascotas(uow.RepositorioPaciente.obtenerVarios(c=>c.ClienteId==pac.ClienteId));

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("error falta aun campo obligatorio por cubrir o algun tipo de dato con valores no validos");
                        //RecuperarValoresProdEntrada();
                    }
                }
                else
                {
                    MessageBox.Show("existe un producto con el mismo nombre y marca ya registrado");
                    //RecuperarValoresProdEntrada();

                }
            }
        }
    }
}
