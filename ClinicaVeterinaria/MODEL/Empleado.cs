using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("Empleado")]
    public class Empleado : PropertyValidateModel
    {
        public Empleado()
        {
            Ventas = new HashSet<Venta>();
            Vacunas = new HashSet<Vacuna>();
            Citas = new HashSet<Cita>();
            HistorialesClinicos = new HashSet<HistorialClinico>();        
            Horarios = new HashSet<Horario>();
        }


      
        [Key]
        public string Usuario { get; set; }
        public string Tipo { get; set; }

        public string Nombre { get; set; }

        public string  Apellidos { get; set; }

        public string Telefono { get; set; }

        public string Movil { get; set; }

        public string Titulacion { get; set; }

        public string Direccion { get; set; }

        public string Email { get; set; }

       

        public string Contraseña { get; set; }

        public ICollection<Venta> Ventas { get; set; }

        public ICollection<Vacuna> Vacunas { get; set; }

        public ICollection<Cita> Citas { get; set; }

        public ICollection<HistorialClinico> HistorialesClinicos { get; set; } 

       

        public ICollection<Horario> Horarios { get; set; }
    } 
}
