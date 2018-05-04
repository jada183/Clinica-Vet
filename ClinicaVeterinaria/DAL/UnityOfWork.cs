using ClinicaVeterinaria.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;


namespace ClinicaVeterinaria.DAL
{
    public class UnityOfWork
    {
        private Context context = new Context();
        private RepositorioCliente repositorioCliente;
        private RepositorioCita repositorioCita;
        private RepositorioEmpleado repositorioEmpleado;
        private RepositorioPaciente repositorioPaciente;
        private RepositorioProducto repositorioProducto;
        private RepositorioServicio repositorioServicio;
       
        private RepositorioVacuna repositorioVacuna;
        private RepositorioVenta repositorioVenta;
        private RepositorioLineaVenta repositorioLineaVenta;

        private RepositorioHorario repositorioHorario;
        private RepositorioHistorialClinico repositorioHistorialClinico;


        public RepositorioCliente RepositorioCliente
        {
            get
            {
                if (this.repositorioCliente == null)
                {
                    this.repositorioCliente = new RepositorioCliente(context);
                }
                return repositorioCliente;
            }
        }
        public RepositorioCita RepositorioCita
        {
            get
            {
                if (this.repositorioCita == null)
                {
                    this.repositorioCita = new RepositorioCita(context);
                }
                return repositorioCita;
            }
        }

        public RepositorioEmpleado RepositorioEmpleado
        {
            get
            {
                if (this.repositorioEmpleado == null)
                {
                    this.repositorioEmpleado = new RepositorioEmpleado(context);
                }
                return repositorioEmpleado;
            }
        }
        public RepositorioPaciente RepositorioPaciente
        {
            get
            {
                if (this.repositorioPaciente == null)
                {
                    this.repositorioPaciente = new RepositorioPaciente(context);
                }
                return repositorioPaciente;
            }
        }
        public RepositorioProducto RepositorioProducto
        {
            get
            {
                if (this.repositorioProducto == null)
                {
                    this.repositorioProducto = new RepositorioProducto(context);
                }
                return repositorioProducto;
            }
        }
        public RepositorioServicio RepositorioServicio
        {
            get
            {
                if (this.repositorioServicio == null)
                {
                    this.repositorioServicio = new RepositorioServicio(context);
                }
                return repositorioServicio;
            }
        }
        
        public RepositorioVacuna RepositorioVacuna

        {
            get
            {
                if (this.repositorioVacuna == null)
                {
                    this.repositorioVacuna = new RepositorioVacuna(context);
                }
                return repositorioVacuna;
            }
        }
        public RepositorioVenta RepositorioVenta

        {
            get
            {
                if (this.repositorioVenta == null)
                {
                    this.repositorioVenta = new RepositorioVenta(context);
                }
                return repositorioVenta;
            }
        }
        public RepositorioLineaVenta RepositorioLineaVenta

        {
            get
            {
                if (this.repositorioLineaVenta == null)
                {
                    this.repositorioLineaVenta = new RepositorioLineaVenta(context);
                }
                return repositorioLineaVenta;
            }
        }
        public RepositorioHorario RepositorioHorario

        {
            get
            {
                if (this.repositorioHorario == null)
                {
                    this.repositorioHorario = new RepositorioHorario(context);
                }
                return repositorioHorario;
            }
        }
        public RepositorioHistorialClinico RepositorioHistorialClinico

        {
            get
            {
                if (this.repositorioHistorialClinico == null)
                {
                    this.repositorioHistorialClinico = new RepositorioHistorialClinico(context);
                }
                return repositorioHistorialClinico;
            }
        }
    }   
}
