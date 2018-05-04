using ClinicaVeterinaria.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.DAL
{
    public class RepositorioHorario : GenericRepository<Horario>
    {
        public RepositorioHorario(Context context) : base(context)
        {
            this.context = context;
        }
    }
}
