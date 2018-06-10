using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("Cliente")]
    public class Cliente: PropertyValidateModel
    {

        public Cliente() {
            Mascotas = new HashSet<Paciente>();
            Compras = new HashSet<Venta>();
        }
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "No te olvides del nombre")]
        [StringLength(40, MinimumLength = 2)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "No te olvides de los apellidos")]
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

        public virtual ICollection<Paciente> Mascotas { get; set; }
        public virtual ICollection<Venta> Compras { get; set; }

        public bool Habilitado { get; set; }
    }
   
}
