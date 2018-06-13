using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "No te olvides del nombre")]
        [StringLength(20, MinimumLength = 2)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "No te olvides de los apellidos")]
        [StringLength(40, MinimumLength = 2)]
        public string Apellidos { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "El telefono no es valido")]
        [Phone]
        public string Telefono { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "El telefono movil no es valido")]
        [Phone]
        public string Movil { get; set; }

        [StringLength(120)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "No te olvides del correo")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El email no es valido")]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Producto> Productos { get; set; }
        
        public bool Habilitado { get; set; }

}
}
