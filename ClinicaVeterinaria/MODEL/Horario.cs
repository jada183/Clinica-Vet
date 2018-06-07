using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{

    [Table("Horario")]
    public class Horario : PropertyValidateModel
    {
        public int HorarioId { get; set; }

        [Required(ErrorMessage = "No te olvides del dia")]
        public string Dia { get; set; }

        [Required(ErrorMessage = "No te olvides de la hora inicial")]
        public string HoraInic { get; set; }

        [Required(ErrorMessage = "No te olvides de la hora final")]
        public string HoraFin { get; set; }

        public virtual Empleado Empleado {get;set;}
        public int?   EmpleadoId { get; set; }
    }
}
