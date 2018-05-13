using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ClinicaVeterinaria.MODEL;
using System.Runtime.Remoting.Contexts;

using System.Data.Entity.Migrations;


namespace ClinicaVeterinaria.DAL
{


    public class Context : DbContext
    {
        public Context() :
            base("ClinicaVeterinariaEntities")
        {
            if (Database.Exists())
            {



              // Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
            }
            else
            {
            }

        }

        class creardb : CreateDatabaseIfNotExists<Context>
        {
            protected override void Seed(DAL.Context context)
            {
                context.Empleados.AddOrUpdate(

                  new Empleado { Nombre = "Admin", Usuario = "Admin", Contraseña = "admin", Tipo = "Sanitario", Apellidos = "garcia garcia", Email = "correEjemp@gmail.es" }
                  );
                context.Horarios.AddOrUpdate(
                 new Horario { HorarioId=1, HoraInic="8:30",HoraFin="3:00", Dia="Lunes", EmpleadoId="Admin"}
                 );
                context.Proveedores.AddOrUpdate(
                   new Proveedor { Nombre = "proveedor prueba", Apellidos = "apellidos", Email = "proveedor@gmail.com",ProveedorId=1}
                   );

                context.Productos.AddOrUpdate(
                  new Producto { NombreProducto = "producto prueba", NombreMarca = "marca1", AnimalDirigido = "gato", ProovedorId = 1, Tamaño = 0, Peso = 0, Stock = 0 ,Categoria="Alimento",Precio=1,ProductoId=1}
                  );
                context.Servicios.AddOrUpdate(
                    new Servicio{ ServicioId=1, Nombre="servicio prueba", Descripcion="descripcion de prueba", CosteServicio=10.55,Tiempo=30 }
                    );
                context.Clientes.AddOrUpdate(
                   new Cliente { ClienteId=1, Nombre="Juan carlos", Apellidos="Sarabando",Telefono="98812247", Movil="6451515251",Direccion="cifp carballeira nº 4",Email="sarabando@gmail.es"}
                   );
                context.Pacientes.AddOrUpdate(
                   new Paciente { PacienteId = 1, Nombre = "mascota1", Especie = "Perro", Raza = "Malamute", Peso = 15, Altura = 90, FechaNacimiento = Convert.ToDateTime("20/10/2015"), Sexo="Macho",Ingresado=true,ClienteId=1 }
                   );
                context.Vacunas.AddOrUpdate(
                  new Vacuna { VacunaId = 1, Nombre = "vacuna prueba", PacienteId = 1, EmpleadoId = "Admin",Fecha= Convert.ToDateTime("20/10/2015") }
                  );
                context.HistorialesClinicos.AddOrUpdate(
                 new HistorialClinico { HistorialClinicoId = 1, Enfermedad = "enfermedad prueba", PacienteId = 1, EmpleadoId = "Admin", Fecha = Convert.ToDateTime("20/10/2015"),Detalles="detalles por defecto" }
                 );
                context.Citas.AddOrUpdate(
                 new Cita {CitaId=1, Hora="10:00", PacienteId = 1, EmpleadoId = "Admin", Fecha = Convert.ToDateTime("25/06/2018"),ServicioId=1,Causa="causa por defecto",Atendida=false}
                 );
                context.Ventas.AddOrUpdate(
                    new Venta { VentaId = 1, EmpleadoId = "Admin", FechaVenta = Convert.ToDateTime("25/06/2018"), ClienteId = 1 }
                    );
                context.LineasVenta.AddOrUpdate(
                  new LineaVenta { LineaVentaId = 1, VentaId = 1, Cantidad = 1, ProductoId = 1, }
                  );
            }
        }
    


        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<HistorialClinico> HistorialesClinicos{get;set;}
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Servicio> Servicios { get; set; }   
        public DbSet<Vacuna> Vacunas { get; set; }
        public DbSet<LineaVenta> LineasVenta { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }

        

       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Venta>()
          .Property(b => b.EmpleadoId)
          .IsOptional();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Horario>()
          .Property(b => b.EmpleadoId)
          .IsOptional();
            base.OnModelCreating(modelBuilder);
        }

       
    }
}
