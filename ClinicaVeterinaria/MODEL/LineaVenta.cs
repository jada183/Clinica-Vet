using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("LineaVenta")]
    public class LineaVenta: PropertyValidateModel
    {
        public int LineaVentaId { get; set; }
        public int Cantidad { get; set; }

        public virtual Producto Producto { get; set; }

        public int? ProductoId { get; set; }

        public virtual Venta Venta { get; set; }
        public int? VentaId { get; set; }
    }
}
