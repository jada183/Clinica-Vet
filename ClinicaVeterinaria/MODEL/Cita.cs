using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{

    [Table("Cita")]
    public class Cita: PropertyValidateModel
    {

        public int CitaId{ get; set; }
        [Required(ErrorMessage = "No te olvides de la fecha")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "No te olvides de la hora")]
        public string Hora { get; set; }

        public virtual Paciente Paciente { get; set; }
        public int? PacienteId { get; set;}
        
        public virtual Empleado Sanitario { get; set; }  
        public int? EmpleadoId { get; set; }

        public virtual Servicio Servicio { get; set; }
        public int? ServicioId { get; set; }

        [StringLength(250)]
        public string Causa { get; set; }
        public bool Atendida { get; set; }
    }

}
