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
            EstadoIngresados = new HashSet<EstadoIngresado>();
        }


        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "No te olvides del usuario de empleado")]
        [StringLength(20, MinimumLength = 4)]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "No te olvides del tipo de empleado")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "No te olvides del nombre")]
        [StringLength(40, MinimumLength = 2)]
        public string Nombre { get; set; }

        [StringLength(80, MinimumLength = 4)]
        [Required(ErrorMessage = "No te olvides de los apellidos")]
        public string  Apellidos { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "El telefono no es valido")]
        [Phone]
        public string Telefono { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "El telefono movil no es valido")]
        [Phone]
        public string Movil { get; set; }

        public string Titulacion { get; set; }


        public string Direccion { get; set; }

        [Required(ErrorMessage = "No te olvides del correo")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El email no es valido")]
        [StringLength(40, MinimumLength = 4)]
        [EmailAddress]
        public string Email { get; set; }



        [Required(ErrorMessage = "No te olvides de la contraseña")]
        [DataType(DataType.Password, ErrorMessage = "la contraseña no es valida")]
        [StringLength(12, MinimumLength = 4)]
        public string Contraseña { get; set; }

        public ICollection<Venta> Ventas { get; set; }

        public ICollection<Vacuna> Vacunas { get; set; }

        public ICollection<Cita> Citas { get; set; }

        public ICollection<HistorialClinico> HistorialesClinicos { get; set; } 

        public ICollection<EstadoIngresado> EstadoIngresados { get; set; }
       

        public ICollection<Horario> Horarios { get; set; }
    } 
}
