using ClinicaVeterinaria.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.DAL
{
    public class RepositorioVenta : GenericRepository<Venta>
    {
        public RepositorioVenta(Context context) : base(context)
        {
            this.context = context;
        }
    }
}
