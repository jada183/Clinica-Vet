using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("EstadoIngresado")]
    public class EstadoIngresado : PropertyValidateModel
    {
        public EstadoIngresado()
        {

        }
        public int EstadoIngresadoId { get; set; }

    

        public double Temperatura { get; set; }

        public int FrecuenciaCardiaca { get; set; }

        public int FrecuenciaRespiratoria { get; set; }

        public string RevisionGeneral { get; set; }

        public string PerdidasFisiologicas { get; set; }

        public string Medicacion { get; set; }

        public DateTime  Fecha { get; set; }

        public virtual Paciente Paciente { get; set; }

        public int? PacienteId { get; set; }

        public virtual Empleado Empleado { get; set; }

        public int? EmpleadoId { get; set; }
    }
}
