using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("Proveedor")]
    public class Proveedor: PropertyValidateModel
    {
        public Proveedor() {
            Productos = new HashSet<Producto>();
        }
        public int ProveedorId { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public string Movil { get; set; }

        public string Direccion { get; set; }

        public string Email { get; set; }

        public ICollection<Producto> Productos { get; set; }

}
}
