using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ClinicaVeterinaria.MODEL;
using System.Runtime.Remoting.Contexts;

using System.Data.Entity.Migrations;
using ClinicaVeterinaria.Migrations;

namespace ClinicaVeterinaria.DAL
{


    public class Context : DbContext
    {
        public Context() :
            base("ClinicaVeterinariaEntities")
        {
            if (Database.Exists())
            {



              Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
            }
            else
            {
                Database.SetInitializer(new creardb());
            }

        }

        class creardb : CreateDatabaseIfNotExists<Context>
        {
            protected override void Seed(DAL.Context context)
            {
                context.Empleados.AddOrUpdate(

                 new Empleado { Nombre = "Admin", Usuario = "Admin", Contraseña = "admin", Tipo = "Sanitario", Apellidos = "garcia garcia", Email = "correEjemp@gmail.es", EmpleadoId = 1, Direccion = "calle 2", Titulacion = "veterinario" ,Telefono="699789797",Movil="68154454"},
                  new Empleado { Nombre = "empleado1", Usuario = "empleado1", Contraseña = "empleado1", Tipo = "Sanitario", Apellidos = "perez perez", Email = "Empleado1@gmail.es", EmpleadoId = 2, Direccion = "calle 3", Titulacion = "veterinario" },
                  new Empleado { Nombre = "empleado2", Usuario = "empleado2", Contraseña = "empleado2", Tipo = "Sanitario", Apellidos = "fernandez garcia", Email = "Empleado2@gmail.es", EmpleadoId = 3, Direccion = "calle ddf", Titulacion = "veterinario" },
                  new Empleado { Nombre = "empleado3", Usuario = "empleado3", Contraseña = "empleado3", Tipo = "Dependiente", Apellidos = "Gonzales ferreira", Email = "Empleado3@gmail.es", EmpleadoId = 4, Titulacion = "Vendedor" },
                  new Empleado { Nombre = "empleado4", Usuario = "empleado4", Contraseña = "empleado4", Tipo = "Dependiente", Apellidos = "Gonzales Perez", Email = "Empleado4@gmail.com", EmpleadoId = 5, Titulacion = "Vendedor" }
                  );
                context.Horarios.AddOrUpdate(
                 new Horario { HorarioId = 1, HoraInic = "8:30", HoraFin = "13:00", Dia = "lunes", EmpleadoId = 1 },
                 new Horario { HorarioId = 6, HoraInic = "8:30", HoraFin = "13:00", Dia = "martes", EmpleadoId = 1 },
                 new Horario { HorarioId = 2, HoraInic = "10:30", HoraFin = "13:00", Dia = "martes", EmpleadoId = 2 },
                 new Horario { HorarioId = 3, HoraInic = "11:00", HoraFin = "15:00", Dia = "martes", EmpleadoId = 3 },
                 new Horario { HorarioId = 4, HoraInic = "7:00", HoraFin = "14:00", Dia = "martes", EmpleadoId = 4},
                  new Horario { HorarioId = 5, HoraInic = "9:00", HoraFin = "15:00", Dia = "viernes", EmpleadoId = 5 }
                 );
                context.Proveedores.AddOrUpdate(
                   new Proveedor { Nombre = "proveedor prueba", Apellidos = "apellidos", Email = "proveedor@gmail.com",ProveedorId=1, Direccion="calle 1", Movil="69851653",Telefono="988234135"},
                   new Proveedor { Nombre = "proveedor prueba2", Apellidos = "apellidos2", Email = "proveedor2@gmail.com", ProveedorId = 2 , Direccion = "calle 2", Movil = "69850003", Telefono = "98589474805" },
                   new Proveedor { Nombre = "proveedor prueba3", Apellidos = "apellidos3", Email = "proveedor3@gmail.com", ProveedorId = 3 },
                   new Proveedor { Nombre = "proveedor prueba4", Apellidos = "apellidos4", Email = "proveedor@gmail.com", ProveedorId = 4 },
                   new Proveedor { Nombre = "proveedor prueba5", Apellidos = "apellidos5", Email = "proveedor5@gmail.com", ProveedorId = 5, Direccion = "calle rd", Movil = "6635850003", Telefono = "98889007480" }
                   );

                context.Productos.AddOrUpdate(
                  new Producto { NombreProducto = "producto prueba", NombreMarca = "marca1", AnimalDirigido = "gato", ProveedorId = 1, Tamaño = 0, Peso = 0, Stock = 0 ,Categoria="Alimento",Precio=1,ProductoId=1},
                  new Producto { NombreProducto = "producto prueba2", NombreMarca = "marca1", AnimalDirigido = "perro", ProveedorId = 2, Tamaño = 10, Peso = 1, Stock = 20, Categoria = "Medicamento", Precio = 5, ProductoId = 2 },
                  new Producto { NombreProducto = "producto prueba3", NombreMarca = "marca2", AnimalDirigido = "gato", ProveedorId = 2, Tamaño = 10, Peso = 10, Stock = 0, Categoria = "Accesorio", Precio = 4, ProductoId = 3 },
                  new Producto { NombreProducto = "producto prueba4", NombreMarca = "marca2", AnimalDirigido = "gato", ProveedorId = 3, Tamaño = 0, Peso = 0, Stock = 20, Categoria = "Alimento", Precio = 11.50, ProductoId = 4 },
                  new Producto { NombreProducto = "producto prueba5", NombreMarca = "marca3", AnimalDirigido = "perro", ProveedorId = 3, Tamaño = 2, Peso = 3, Stock = 50, Categoria = "Alimento", Precio = 21.50, ProductoId = 5 }
                  );
                context.Servicios.AddOrUpdate(
                    new Servicio{ ServicioId=1, Nombre="servicio prueba", Descripcion="descripcion de prueba", CosteServicio=10.55,Tiempo=30 },
                    new Servicio { ServicioId = 2, Nombre = "servicio prueba2", Descripcion = "descripcion de prueba2", CosteServicio = 20.55, Tiempo = 40 },
                    new Servicio { ServicioId = 3, Nombre = "servicio prueba3", Descripcion = "descripcion de prueba3", CosteServicio = 30.55, Tiempo = 20 },
                    new Servicio { ServicioId = 4, Nombre = "servicio prueba4", Descripcion = "descripcion de prueba4", CosteServicio = 40.55, Tiempo = 20 },
                    new Servicio { ServicioId = 5, Nombre = "servicio prueba5", Descripcion = "descripcion de prueba5", CosteServicio = 40.00, Tiempo = 60 }
                        );
                context.Clientes.AddOrUpdate(
                   new Cliente { ClienteId=1, Nombre="Juan carlos", Apellidos="Sarabando",Telefono="98812247", Movil="6451515251",Direccion="cifp carballeira nº 4",Email="sarabando@gmail.es"},
                   new Cliente { ClienteId = 2, Nombre = "Luz ", Apellidos = "Lois", Telefono = "9881224407", Movil = "64515100251", Direccion = "cifp carballeira nº 10", Email = "luz@gmail.es" },
                   new Cliente { ClienteId = 3, Nombre = "Victor", Apellidos = "Fernandez", Telefono = "9881228472", Movil = "64515222", Direccion = "cifp carballeira nº 7", Email = "victor@gmail.es" },
                   new Cliente { ClienteId = 4, Nombre = "Luisa", Apellidos = "Gonzales", Telefono = "988155247", Movil = "645150015251", Direccion = "cifp carballeira nº 2", Email = "luisa@gmail.es" },
                   new Cliente { ClienteId = 5, Nombre = "Jose Luis", Apellidos = "Carnero", Telefono = "9881200247", Movil = "6457775251", Direccion = "cifp carballeira nº 8", Email = "josel@gmail.es" }
                       );
                context.Pacientes.AddOrUpdate(
                   new Paciente { PacienteId = 1, Nombre = "mascota1", Especie = "Perro", Raza = "Malamute", Peso = 15, Altura = 90, FechaNacimiento = Convert.ToDateTime("20/10/2015"), Sexo="Macho",Ingresado=true,ClienteId=1 },
                   new Paciente { PacienteId = 2, Nombre = "mascota2", Especie = "gato", Raza = "persa", Peso = 4, Altura = 60, FechaNacimiento = Convert.ToDateTime("10/11/2015"), Sexo = "Macho", Ingresado = false, ClienteId = 2 },
                   new Paciente { PacienteId = 3, Nombre = "mascota3", Especie = "gato", Raza = "gris", Peso = 3, Altura = 58, FechaNacimiento = Convert.ToDateTime("05/11/2015"), Sexo = "Embra", Ingresado = false, ClienteId = 3 },
                   new Paciente { PacienteId = 4, Nombre = "mascota4", Especie = "perro", Raza = "pastor aleman", Peso = 45, Altura = 110, FechaNacimiento = Convert.ToDateTime("01/01/2015"), Sexo = "Macho", Ingresado = true, ClienteId = 3 },
                   new Paciente { PacienteId = 5, Nombre = "mascota5", Especie = "perro", Raza = "pastor belga", Peso = 4, Altura = 112, FechaNacimiento = Convert.ToDateTime("12/08/2015"), Sexo = "Embra", Ingresado = false, ClienteId = 4 }
                   );
                context.Vacunas.AddOrUpdate(
                  new Vacuna { VacunaId = 1, Nombre = "vacuna prueba", PacienteId = 1, EmpleadoId = 1,Fecha= Convert.ToDateTime("20/10/2015") },
                  new Vacuna { VacunaId = 2, Nombre = "vacuna prueba2", PacienteId = 2, EmpleadoId = 2, Fecha = Convert.ToDateTime("10/03/2016") },
                  new Vacuna { VacunaId = 3, Nombre = "vacuna prueba3", PacienteId = 2, EmpleadoId = 2, Fecha = Convert.ToDateTime("10/03/2009") },
                  new Vacuna { VacunaId = 4, Nombre = "vacuna prueba4", PacienteId = 3, EmpleadoId = 3, Fecha = Convert.ToDateTime("12/04/2009") },
                  new Vacuna { VacunaId = 5, Nombre = "vacuna prueba3", PacienteId = 4, EmpleadoId = 1, Fecha = Convert.ToDateTime("15/06/2013") },
                  new Vacuna { VacunaId = 6, Nombre = "vacuna prueba3", PacienteId = 5, EmpleadoId = 2, Fecha = Convert.ToDateTime("15/06/2013") }
                  );
                context.HistorialesClinicos.AddOrUpdate(
                 new HistorialClinico { HistorialClinicoId = 1, Enfermedad = "enfermedad prueba", PacienteId = 1, EmpleadoId = 1, Fecha = Convert.ToDateTime("20/10/2015"),Detalles="detalles por defecto" },
                 new HistorialClinico { HistorialClinicoId = 2, Enfermedad = "enfermedad prueba2", PacienteId = 2, EmpleadoId = 1, Fecha = Convert.ToDateTime("27/11/2009"), Detalles = "detalles por defecto2" },
                 new HistorialClinico { HistorialClinicoId = 3, Enfermedad = "enfermedad prueba3", PacienteId = 4, EmpleadoId = 3, Fecha = Convert.ToDateTime("22/07/2010"), Detalles = "detalles por defecto3" },
                 new HistorialClinico { HistorialClinicoId = 4, Enfermedad = "enfermedad prueba4", PacienteId = 4, EmpleadoId = 3, Fecha = Convert.ToDateTime("27/11/2009"), Detalles = "detalles por defecto4" },
                 new HistorialClinico { HistorialClinicoId = 5, Enfermedad = "enfermedad prueba5", PacienteId = 5, EmpleadoId = 1, Fecha = Convert.ToDateTime("07/01/2009"), Detalles = "detalles por defecto4" }
                 );
                context.Citas.AddOrUpdate(
                 new Cita {CitaId=1, Hora="10:00", PacienteId = 1, EmpleadoId = 1, Fecha = Convert.ToDateTime("25/06/2018"),ServicioId=1,Causa="causa por defecto",Atendida=false},
                 new Cita { CitaId = 2, Hora = "11:00", PacienteId = 2, EmpleadoId = 1, Fecha = Convert.ToDateTime("25/06/2018"), ServicioId = 2, Causa = "causa por defecto2", Atendida = false },
                 new Cita { CitaId = 3, Hora = "11:30", PacienteId = 3, EmpleadoId = 2, Fecha = Convert.ToDateTime("26/06/2018"), ServicioId = 3, Causa = "causa por defect3", Atendida = false },
                 new Cita { CitaId = 4, Hora = "10:30", PacienteId = 4, EmpleadoId = 2, Fecha = Convert.ToDateTime("05/06/2018"), ServicioId = 3, Causa = "causa por defect5", Atendida = true },
                 new Cita { CitaId = 4, Hora = "10:30", PacienteId = 4, EmpleadoId = 1, Fecha = Convert.ToDateTime("04/06/2018"), ServicioId = 4, Causa = "causa por defect4", Atendida = true }
                 );
                context.Ventas.AddOrUpdate(
                    new Venta { VentaId = 1, EmpleadoId = 1, FechaVenta = Convert.ToDateTime("25/06/2018"), ClienteId = 1 },
                    new Venta { VentaId = 2, EmpleadoId = 4, FechaVenta = Convert.ToDateTime("14/06/2018"), ClienteId = 2 },
                    new Venta { VentaId = 3, EmpleadoId = 5, FechaVenta = Convert.ToDateTime("11/06/2018"), ClienteId = 3 },
                    new Venta { VentaId = 4, EmpleadoId = 4, FechaVenta = Convert.ToDateTime("08/06/2018"), ClienteId = 3 }
                    );
                context.LineasVenta.AddOrUpdate(
                   new LineaVenta { LineaVentaId = 1, VentaId = 1, Cantidad = 1, ProductoId = 1, },
                   new LineaVenta { LineaVentaId = 2, VentaId = 1, Cantidad = 2, ProductoId = 4, },
                   new LineaVenta { LineaVentaId = 3, VentaId = 2, Cantidad = 1, ProductoId = 2, },
                   new LineaVenta { LineaVentaId = 4, VentaId = 2, Cantidad = 3, ProductoId = 3, },
                   new LineaVenta { LineaVentaId = 5, VentaId = 2, Cantidad = 1, ProductoId = 1, },
                   new LineaVenta { LineaVentaId = 6, VentaId = 3, Cantidad = 2, ProductoId = 4, },
                   new LineaVenta { LineaVentaId = 7, VentaId = 4, Cantidad = 1, ProductoId = 1, },
                   new LineaVenta { LineaVentaId = 8, VentaId = 4, Cantidad = 1, ProductoId = 2, },
                   new LineaVenta { LineaVentaId = 9, VentaId = 4, Cantidad = 2, ProductoId = 3, }
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
        public DbSet<EstadoIngresado> EstadoIngresados { get; set; }
        

       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);
        }

       
    }
}