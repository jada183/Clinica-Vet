using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicaVeterinaria.MODEL;
namespace ClinicaVeterinaria.DAL
{
    public class RepositorioProveedor : GenericRepository<Proveedor>
    {
        public RepositorioProveedor(Context context) : base(context)
        {
            this.context = context;
        }
    }
}
