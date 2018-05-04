using System;
using System.Collections.Generic;
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

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public string Movil { get; set; }

        public string Direccion { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Paciente> Mascotas { get; set; }
        public virtual ICollection<Venta> Compras { get; set; }
    }
   
}
