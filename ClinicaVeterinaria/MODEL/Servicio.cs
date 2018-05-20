using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("Servicio")]
    public class Servicio : PropertyValidateModel
    {
        public Servicio(){
            Citas = new HashSet<Cita>();
            
        }

        public int ServicioId { get; set; }

        [Required(ErrorMessage = "No te olvides del nombre del servicio")]
        [StringLength(40, MinimumLength = 2)]
        public string Nombre { get; set; }

        public double CosteServicio { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        public int Tiempo { get; set; }
        
        public virtual ICollection<Cita> Citas { get; set; }

    }
}
