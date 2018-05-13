using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicaVeterinaria.MODEL;


namespace ClinicaVeterinaria.MODEL
{
    [Table("Producto")]
    public class Producto : PropertyValidateModel
    {
        public Producto()
        {
            LineasVentas = new HashSet<LineaVenta>();
        }
        public int ProductoId { get; set; }
        public double Precio { get; set; }

        [Required(ErrorMessage = "No te olvides del nombre de producto")]
        [StringLength(25)]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "No te olvides de la marca")]
        [StringLength(25)]
        public string NombreMarca { get; set; }

        
        public string AnimalDirigido { get; set; }
        public double Peso { get; set; }
        public double Tamaño { get; set; }
        public virtual ICollection<LineaVenta> LineasVentas { get; set;}
        public string Imagen { get; set; }
        public int Stock { get; set; } 
        public string Categoria { get; set; }

        public virtual Proveedor Proveedor{ get; set; }
        public int? ProovedorId { get; set; }
    }
}
