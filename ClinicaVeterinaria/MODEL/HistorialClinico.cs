﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("HistorialClinico")]
    public class HistorialClinico : PropertyValidateModel
    {

        public int HistorialClinicoId { get; set; }
        
        public DateTime Fecha { get; set; }

        public virtual Paciente Paciente{ get; set; }

        public int? PacienteId{ get; set; }

        [Required(ErrorMessage = "No te olvides de la enfermedad")]
        public string Enfermedad { get; set; }

        public string Detalles { get; set; }

        public virtual Empleado Empleado { get; set; }
        public string EmpleadoId { get; set; }


    }
}
