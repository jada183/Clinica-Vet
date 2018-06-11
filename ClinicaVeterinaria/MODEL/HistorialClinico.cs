using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("HistorialClinico")]
    public class HistorialClinico : PropertyValidateModel
    {

        public int HistorialClinicoId { get; set; }
        
        public DateTime Fecha { get; set; }

        public virtual Paciente Paciente{ get; set; }

        public int? PacienteId{ get; set; }

        [Required(ErrorMessage = "No te olvides de la enfermedad")]
        [StringLength(80, MinimumLength = 4)]
        public string Enfermedad { get; set; }

        [StringLength(250)]
        public string Detalles { get; set; }

        public virtual Empleado Empleado { get; set; }
        public int? EmpleadoId { get; set; }


    }
}
