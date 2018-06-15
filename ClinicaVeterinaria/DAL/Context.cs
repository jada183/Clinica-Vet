using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ClinicaVeterinaria.MODEL;
using System.Runtime.Remoting.Contexts;

using System.Data.Entity.Migrations;
//using ClinicaVeterinaria.Migrations;

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
                Database.SetInitializer(new creardb());
            }

        }

        class creardb : CreateDatabaseIfNotExists<Context>
        {
            protected override void Seed(DAL.Context context)
            {
                context.Empleados.AddOrUpdate(

                 new Empleado { Nombre = "Jason", Usuario = "Admin", Contraseña = "admin", Tipo = "Sanitario", Apellidos = "Franco Quintero", Email = "correEjemp@gmail.es", EmpleadoId = 1, Direccion = "calle 2", Titulacion = "veterinario" ,Telefono="699789797",Movil="68154454" , Habilitado = true, Permiso="Administrador" },
                  new Empleado { Nombre = "Beni", Usuario = "empleado1", Contraseña = "empleado1", Tipo = "Sanitario", Apellidos = "Feijoo Otero", Email = "beniFO@gmail.es", EmpleadoId = 2, Direccion = "calle 3", Titulacion = "veterinario", Habilitado = true , Permiso = "Administrador" },
                  new Empleado { Nombre = "Luis", Usuario = "empleado2", Contraseña = "empleado2", Tipo = "Sanitario", Apellidos = "Alvares Outeiriño", Email = "LuisAO@gmail.es", EmpleadoId = 3, Direccion = "calle ddf", Titulacion = "veterinario", Habilitado = true, Permiso = "Usuario" },
                  new Empleado { Nombre = "Gabriel", Usuario = "empleado3", Contraseña = "empleado3", Tipo = "Dependiente", Apellidos = "Guios Barroso", Email = "GGB@gmail.es", EmpleadoId = 4, Titulacion = "Vendedor", Habilitado = true, Permiso = "Usuario" },
                  new Empleado { Nombre = "Yago", Usuario = "empleado4", Contraseña = "empleado4", Tipo = "Dependiente", Apellidos = "Abeiro Alonso", Email = "YagoAA@gmail.com", EmpleadoId = 5, Titulacion = "Vendedor", Habilitado = true, Permiso = "Usuario" }
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
                   new Proveedor { Nombre = "Juan", Apellidos = "Fernadez Perez", Email = "juancho@gmail.com",ProveedorId=1, Direccion="calle 1", Movil="69851653",Telefono="988234135", Habilitado = true },
                   new Proveedor { Nombre = "Patricia", Apellidos = "Fernandez Gonzales", Email = "patri@gmail.com", ProveedorId = 2 , Direccion = "calle 2", Movil = "69850003", Telefono = "98589474805", Habilitado = true },
                   new Proveedor { Nombre = "Ana", Apellidos = "bernabeu Gonzales", Email = "anucha@gmail.com", ProveedorId = 3 , Habilitado = true },
                   new Proveedor { Nombre = "Alex", Apellidos = "Carballo Perez", Email = "carba@gmail.com", ProveedorId = 4 , Habilitado = true },
                   new Proveedor { Nombre = "Jorge", Apellidos = "Casas Paradela", Email = "capra@gmail.com", ProveedorId = 5, Direccion = "calle rd", Movil = "6635850003", Telefono = "98889007480" , Habilitado = true }
                   );

                context.Productos.AddOrUpdate(
                  new Producto { NombreProducto = "Pienso gato", NombreMarca = "purina", AnimalDirigido = "gato", ProveedorId = 1, Tamaño = 0, Peso = 5, Stock = 0 ,Categoria="Alimento",Precio=4.50,ProductoId=1, Habilitado=true},
                  new Producto { NombreProducto = "Antibiotico", NombreMarca = "VetBio", AnimalDirigido = "perro", ProveedorId = 2, Tamaño = 0, Peso = 1, Stock = 20, Categoria = "Medicamento", Precio = 8, ProductoId = 2, Habilitado = true },
                  new Producto { NombreProducto = "Correa", NombreMarca = "collarines", AnimalDirigido = "gato", ProveedorId = 2, Tamaño = 100, Peso = 1, Stock = 0, Categoria = "Accesorio", Precio = 12, ProductoId = 3 , Habilitado = true },
                  new Producto { NombreProducto = "Pienso perro", NombreMarca = "purina", AnimalDirigido = "perro", ProveedorId = 3, Tamaño = 0, Peso = 5, Stock = 20, Categoria = "Alimento", Precio = 6.50, ProductoId = 4, Habilitado = true },
                  new Producto { NombreProducto = "gominolas", NombreMarca = "ultima", AnimalDirigido = "perro", ProveedorId = 3, Tamaño = 0, Peso = 0.5, Stock = 50, Categoria = "Alimento", Precio = 21.50, ProductoId = 5 , Habilitado = true }
                  );
                context.Servicios.AddOrUpdate(
                    new Servicio{ ServicioId=1, Nombre="corte de uñas y limpieza orejas", Descripcion="uso de corta uñas especial", CosteServicio=10,Tiempo=20 ,Habilitado=true},
                    new Servicio { ServicioId = 2, Nombre = "Corte de pelo perro", Descripcion = "con tratamiento para hacer brillante el pelo", CosteServicio = 20.55, Tiempo = 40, Habilitado = true },
                    new Servicio { ServicioId = 3, Nombre = "revision de la vista perros", Descripcion = "visor avanzado de vista", CosteServicio = 25, Tiempo = 20 , Habilitado = true },
                    new Servicio { ServicioId = 4, Nombre = "Castrado de gato", Descripcion = "Operación de corta duración", CosteServicio = 140, Tiempo = 80 , Habilitado = true },
                    new Servicio { ServicioId = 5, Nombre = "Castrado de perrro ", Descripcion = "Operación de corta duración", CosteServicio = 130, Tiempo = 60, Habilitado = true}
                        );
                context.Clientes.AddOrUpdate(
                   new Cliente { ClienteId=1, Nombre="Juan carlos", Apellidos="Sarabando",Telefono="98812247", Movil="6451515251",Direccion="calle progreso nº 4",Email="sarabando@gmail.es", Habilitado = true },
                   new Cliente { ClienteId = 2, Nombre = "Luz ", Apellidos = "Lois", Telefono = "9881224407", Movil = "64515100251", Direccion = "martinez sueiro nº 10", Email = "luz@gmail.es", Habilitado = true },
                   new Cliente { ClienteId = 3, Nombre = "Victor", Apellidos = "Fernandez", Telefono = "9881228472", Movil = "64515222", Direccion = "cifp carballeira nº 7", Email = "victor@gmail.es", Habilitado = true },
                   new Cliente { ClienteId = 4, Nombre = "Luisa", Apellidos = "Gonzales", Telefono = "988155247", Movil = "645150015251", Direccion = "av caldas nº 8", Email = "luisa@gmail.es", Habilitado = true },
                   new Cliente { ClienteId = 5, Nombre = "Jose Luis", Apellidos = "Carnero", Telefono = "9881200247", Movil = "6457775251", Direccion = "av peña trevisca nº25", Email = "josel@gmail.es", Habilitado = true }
                       );
                context.Pacientes.AddOrUpdate(
                   new Paciente { PacienteId = 1, Nombre = "Rockie", Especie = "Perro", Raza = "Malamute", Peso = 35, Altura = 105, FechaNacimiento = Convert.ToDateTime("20/10/2015"), Sexo="Macho",Ingresado=true,ClienteId=1, Habilitado = true },
                   new Paciente { PacienteId = 2, Nombre = "dama", Especie = "gato", Raza = "persa", Peso = 4, Altura = 60, FechaNacimiento = Convert.ToDateTime("10/11/2015"), Sexo = "Macho", Ingresado = false, ClienteId = 2 , Habilitado = true },
                   new Paciente { PacienteId = 3, Nombre = "misifu", Especie = "gato", Raza = "gris", Peso = 3, Altura = 58, FechaNacimiento = Convert.ToDateTime("05/11/2015"), Sexo = "Embra", Ingresado = false, ClienteId = 3 , Habilitado = true },
                   new Paciente { PacienteId = 4, Nombre = "Boby", Especie = "perro", Raza = "pastor aleman", Peso = 45, Altura = 110, FechaNacimiento = Convert.ToDateTime("01/01/2015"), Sexo = "Macho", Ingresado = true, ClienteId = 3, Habilitado = true },
                   new Paciente { PacienteId = 5, Nombre = "Blacky", Especie = "perro", Raza = "pastor belga", Peso = 48, Altura = 112, FechaNacimiento = Convert.ToDateTime("12/08/2015"), Sexo = "Embra", Ingresado = false, ClienteId = 4 , Habilitado = true }
                   );
                context.Vacunas.AddOrUpdate(
                  new Vacuna { VacunaId = 1, Nombre = "Rabia", PacienteId = 1, EmpleadoId = 1,Fecha= Convert.ToDateTime("20/10/2015") },
                  new Vacuna { VacunaId = 2, Nombre = "Leishmaniosis", PacienteId = 2, EmpleadoId = 2, Fecha = Convert.ToDateTime("10/03/2016") },
                  new Vacuna { VacunaId = 3, Nombre = "Hexavalente", PacienteId = 2, EmpleadoId = 2, Fecha = Convert.ToDateTime("10/03/2009") },
                  new Vacuna { VacunaId = 4, Nombre = "Octovalente", PacienteId = 3, EmpleadoId = 3, Fecha = Convert.ToDateTime("12/04/2009") },
                  new Vacuna { VacunaId = 5, Nombre = "Pentavalente", PacienteId = 4, EmpleadoId = 1, Fecha = Convert.ToDateTime("15/06/2013") },
                  new Vacuna { VacunaId = 6, Nombre = "monovalente", PacienteId = 5, EmpleadoId = 2, Fecha = Convert.ToDateTime("15/06/2013") }
                  );
                context.HistorialesClinicos.AddOrUpdate(
                 new HistorialClinico { HistorialClinicoId = 1, Enfermedad = "dirofilaria", PacienteId = 1, EmpleadoId = 1, Fecha = Convert.ToDateTime("20/10/2015"),Detalles="se dio durante el verano" },
                 new HistorialClinico { HistorialClinicoId = 2, Enfermedad = "Parvovirus", PacienteId = 2, EmpleadoId = 1, Fecha = Convert.ToDateTime("27/11/2009"), Detalles = "resultado de picadura de mosquito" },
                 new HistorialClinico { HistorialClinicoId = 3, Enfermedad = "Artrosis", PacienteId = 4, EmpleadoId = 3, Fecha = Convert.ToDateTime("22/07/2010"), Detalles = "tratada a tiempo" },
                 new HistorialClinico { HistorialClinicoId = 4, Enfermedad = "Leishmaniosis", PacienteId = 4, EmpleadoId = 3, Fecha = Convert.ToDateTime("27/11/2009"), Detalles = "ha tenido problemas anteriormente con lo mismo" },
                 new HistorialClinico { HistorialClinicoId = 5, Enfermedad = "Moquillo", PacienteId = 5, EmpleadoId = 1, Fecha = Convert.ToDateTime("07/01/2009"), Detalles = "recuperacion lenta" }
                 );
                context.Citas.AddOrUpdate(
                 new Cita {CitaId=1, Hora="10:00", PacienteId = 1, EmpleadoId = 1, Fecha = Convert.ToDateTime("25/06/2018"),ServicioId=1,Causa="evitar realizarse daño",Atendida=false},
                 new Cita { CitaId = 2, Hora = "11:00", PacienteId = 2, EmpleadoId = 1, Fecha = Convert.ToDateTime("25/06/2018"), ServicioId = 2, Causa = "cuidad de pelaje", Atendida = false },
                 new Cita { CitaId = 3, Hora = "11:30", PacienteId = 3, EmpleadoId = 2, Fecha = Convert.ToDateTime("26/06/2018"), ServicioId = 3, Causa = "problemas de ceguera", Atendida = false },
                 new Cita { CitaId = 4, Hora = "10:30", PacienteId = 4, EmpleadoId = 2, Fecha = Convert.ToDateTime("05/06/2018"), ServicioId = 3, Causa = "", Atendida = true },
                 new Cita { CitaId = 4, Hora = "10:30", PacienteId = 4, EmpleadoId = 1, Fecha = Convert.ToDateTime("04/06/2018"), ServicioId = 4, Causa = "", Atendida = true }
                 );
                context.Ventas.AddOrUpdate(
                    new Venta { VentaId = 1, EmpleadoId = 1, FechaVenta = Convert.ToDateTime("25/06/2018"), ClienteId = 1 },
                    new Venta { VentaId = 2, EmpleadoId = 4, FechaVenta = Convert.ToDateTime("14/06/2018"), ClienteId = 2 },
                    new Venta { VentaId = 3, EmpleadoId = 5, FechaVenta = Convert.ToDateTime("11/06/2018"), ClienteId = 3 },
                    new Venta { VentaId = 4, EmpleadoId = 4, FechaVenta = Convert.ToDateTime("10/06/2018"), ClienteId = 3 }
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
                context.EstadoIngresados.AddOrUpdate(
                    new EstadoIngresado { EstadoIngresadoId = 1, Temperatura = 39.5, FrecuenciaCardiaca=80, FrecuenciaRespiratoria=35,RevisionGeneral="corte en el cuello",PerdidasFisiologicas="perdida de sangre", Medicacion="medicamento para el dolor"
                    , Fecha=Convert.ToDateTime("15/06/2018"),PacienteId=1,EmpleadoId=1},
                     new EstadoIngresado { EstadoIngresadoId = 2, Temperatura = 39.9, FrecuenciaCardiaca=85, FrecuenciaRespiratoria=37,RevisionGeneral="herida interna",PerdidasFisiologicas="perdida de sangre", Medicacion="medicamento para el dolor"
                    , Fecha=Convert.ToDateTime("14/06/2018"),PacienteId=3,EmpleadoId=2}
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