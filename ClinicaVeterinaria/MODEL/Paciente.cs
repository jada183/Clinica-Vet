﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.MODEL
{
    [Table("Paciente")]
    public class Paciente : PropertyValidateModel
    {
        public Paciente()
        {
            Citas = new HashSet<Cita>();
            Historiales = new HashSet<HistorialClinico>();
            Vacunas = new HashSet<Vacuna>();
         
        }

        public int PacienteId { get; set; }
        public string Nombre { get; set; }

        public string Especie { get; set; }

        public string Raza { get; set; }

        public double Peso { get; set; }
       
        public double Altura { get; set; }
       

        public DateTime Edad { get; set; }

        public string Sexo { get; set; }       

        public bool Ingresado { get; set; }

        public virtual Cliente Propietario { get; set; }
        public int ClienteId{ get; set; }

        public ICollection<Cita> Citas { get; set; }

        public ICollection<HistorialClinico> Historiales { get; set; }

        public ICollection<Vacuna> Vacunas { get; set; }
    
        

        public string Imagen { get; set; }


    }
   
}