using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("ServicioOfrecido")]
    public class ServicioOfrecido
    {
        public ServicioOfrecido()
        {
            Empleados = new HashSet<Empleado>();
        }
       
        public int ServicioOfrecidoId { get; set; }
        public virtual Servicio Servicio { get; set; }
        public int IdServicio { get; set; }
        public virtual ICollection<Empleado> Empleados { get; set; }
        public DateTime Fecha { get; set; }
        public virtual Cita Cita { get; set; }

        public int IdCita{ get; set; }

        public virtual Paciente Paciente { get; set; }

        public int IdPaciente { get; set; }
    }


   
}
