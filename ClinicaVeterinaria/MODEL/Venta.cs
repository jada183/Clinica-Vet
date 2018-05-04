using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("Venta")]
    public class Venta : PropertyValidateModel
    {
        public Venta()
        {
            LineasVenta = new HashSet<LineaVenta>();  
        }
        public int VentaId { get; set; }
        public DateTime FechaVenta { get; set; }
        public virtual ICollection<LineaVenta> LineasVenta{ get; set; }
        public virtual  Empleado EmpleadoVenta { get; set; }
        public string  EmpleadoId { get; set; }
        
        public virtual Cliente ClienteVenta { get; set; }
        public int? ClienteId { get; set; }
    }

}
