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

        [Required(ErrorMessage = "No te olvides del tipo de empleado")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "No te olvides del nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "No te olvides de los apellidos")]
        public string  Apellidos { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "El telefono no es valido")]
        public string Telefono { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "El telefono movil no es valido")]
        public string Movil { get; set; }

        public string Titulacion { get; set; }


        public string Direccion { get; set; }

        [Required(ErrorMessage = "No te olvides del correo")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El email no es valido")]
        public string Email { get; set; }



        [Required(ErrorMessage = "No te olvides del tipo de empleado")]
        [DataType(DataType.Password, ErrorMessage = "la contraseña no es valida no es valido")]
        public string Contraseña { get; set; }

        public ICollection<Venta> Ventas { get; set; }

        public ICollection<Vacuna> Vacunas { get; set; }

        public ICollection<Cita> Citas { get; set; }

        public ICollection<HistorialClinico> HistorialesClinicos { get; set; } 

       

        public ICollection<Horario> Horarios { get; set; }
    } 
}
