using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("Vacuna")]
    public class Vacuna : PropertyValidateModel
    {
        public int VacunaId { get; set; }
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "No te olvides del nombre")]
        [StringLength(40, MinimumLength = 2)]
        public string Nombre { get; set; }

        public virtual Paciente Paciente { get; set; }
        public int? PacienteId{ get; set; }
        public virtual Empleado Empleado { get; set; }

        public int? EmpleadoId { get; set; }
    }
}
