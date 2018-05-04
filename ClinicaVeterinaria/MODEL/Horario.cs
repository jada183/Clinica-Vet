using System;
using System.Collections.Generic;
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
        public string Dia { get; set; }
        public string HoraInic { get; set; }

        public string HoraFin { get; set; }

        public virtual Empleado Empleado {get;set;}
        public string   EmpleadoId { get; set; }
    }
}
