namespace ClinicaVeterinaria.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MODEL;
    internal sealed class Configuration : DbMigrationsConfiguration<ClinicaVeterinaria.DAL.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ClinicaVeterinaria.DAL.Context context)
        {
            //  This method will be called after migrating to the latest version.
            context.Empleados.AddOrUpdate(

                  new Empleado { Nombre = "Admin", Usuario = "Admin", Contraseña = "admin", Tipo = "Sanitario", Apellidos = "garcia garcia", Email = "correEjemp" }
                  );
            context.Proveedores.AddOrUpdate(
               new Proveedor { Nombre = "proveedor prueba", Apellidos = "apellidos", Email = "proveedor@gmail.com", ProveedorId = 1 }
               );

            context.Productos.AddOrUpdate(
              new Producto { NombreProducto = "producto prueba", NombreMarca = "marca1", AnimalDirigido = "gato", ProovedorId = 1, Tamaño = 0, Peso = 0, Stock = 0, Categoria = "Alimento", Precio = 1, ProductoId = 1 }
              );
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
