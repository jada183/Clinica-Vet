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

                  new Empleado { Nombre = "Admin", Usuario = "Admin", Contraseña = "admin", Tipo = "Sanitario", Apellidos = "garcia garcia", Email = "correEjemp" }
                  );
                context.Proveedores.AddOrUpdate(
                   new Proveedor { Nombre = "proveedor prueba", Apellidos = "apellidos", Email = "proveedor@gmail.com",ProveedorId=1}
                   );

                context.Productos.AddOrUpdate(
                  new Producto { NombreProducto = "producto prueba", NombreMarca = "marca1", AnimalDirigido = "gato", ProovedorId = 1, Tamaño = 0, Peso = 0, Stock = 0 ,FechaCaducidad=DateTime.Today,Precio=1,ProductoId=1}
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
