﻿using ClinicaVeterinaria.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.DAL
{
    public class RepositorioEmpleado : GenericRepository<Empleado>
    {
        public RepositorioEmpleado(Context context) : base(context)
        {
            this.context = context;
        }
    }
}
