namespace ClinicaVeterinaria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cita",
                c => new
                    {
                        CitaId = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Hora = c.String(nullable: false),
                        PacienteId = c.Int(),
                        EmpleadoId = c.String(),
                        ServicioId = c.Int(),
                        Causa = c.String(),
                        Atendida = c.Boolean(nullable: false),
                        Sanitario_Usuario = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CitaId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .ForeignKey("dbo.Empleado", t => t.Sanitario_Usuario)
                .ForeignKey("dbo.Servicio", t => t.ServicioId)
                .Index(t => t.PacienteId)
                .Index(t => t.ServicioId)
                .Index(t => t.Sanitario_Usuario);
            
            CreateTable(
                "dbo.Paciente",
                c => new
                    {
                        PacienteId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Especie = c.String(nullable: false),
                        Raza = c.String(),
                        Peso = c.Double(nullable: false),
                        Altura = c.Double(nullable: false),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Imagen = c.String(),
                        Sexo = c.String(nullable: false),
                        Ingresado = c.Boolean(nullable: false),
                        ClienteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PacienteId)
                .ForeignKey("dbo.Cliente", t => t.ClienteId, cascadeDelete: true)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.HistorialClinico",
                c => new
                    {
                        HistorialClinicoId = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        PacienteId = c.Int(),
                        Enfermedad = c.String(nullable: false),
                        Detalles = c.String(),
                        EmpleadoId = c.String(),
                        Empleado_Usuario = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.HistorialClinicoId)
                .ForeignKey("dbo.Empleado", t => t.Empleado_Usuario)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .Index(t => t.PacienteId)
                .Index(t => t.Empleado_Usuario);
            
            CreateTable(
                "dbo.Empleado",
                c => new
                    {
                        Usuario = c.String(nullable: false, maxLength: 128),
                        Tipo = c.String(nullable: false),
                        Nombre = c.String(nullable: false),
                        Apellidos = c.String(nullable: false),
                        Telefono = c.String(),
                        Movil = c.String(),
                        Titulacion = c.String(),
                        Direccion = c.String(),
                        Email = c.String(nullable: false),
                        Contraseña = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Usuario);
            
            CreateTable(
                "dbo.Horario",
                c => new
                    {
                        HorarioId = c.Int(nullable: false, identity: true),
                        Dia = c.String(nullable: false),
                        HoraInic = c.String(nullable: false),
                        HoraFin = c.String(nullable: false),
                        EmpleadoId = c.String(),
                        Empleado_Usuario = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.HorarioId)
                .ForeignKey("dbo.Empleado", t => t.Empleado_Usuario)
                .Index(t => t.Empleado_Usuario);
            
            CreateTable(
                "dbo.Vacuna",
                c => new
                    {
                        VacunaId = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Nombre = c.String(nullable: false),
                        PacienteId = c.Int(),
                        EmpleadoId = c.String(),
                        Empleado_Usuario = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VacunaId)
                .ForeignKey("dbo.Empleado", t => t.Empleado_Usuario)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .Index(t => t.PacienteId)
                .Index(t => t.Empleado_Usuario);
            
            CreateTable(
                "dbo.Venta",
                c => new
                    {
                        VentaId = c.Int(nullable: false, identity: true),
                        FechaVenta = c.DateTime(nullable: false),
                        EmpleadoId = c.String(),
                        ClienteId = c.Int(),
                        EmpleadoVenta_Usuario = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VentaId)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoVenta_Usuario)
                .Index(t => t.ClienteId)
                .Index(t => t.EmpleadoVenta_Usuario);
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        ClienteId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Apellidos = c.String(nullable: false),
                        Telefono = c.String(),
                        Movil = c.String(),
                        Direccion = c.String(),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteId);
            
            CreateTable(
                "dbo.LineaVenta",
                c => new
                    {
                        LineaVentaId = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        ProductoId = c.Int(),
                        VentaId = c.Int(),
                    })
                .PrimaryKey(t => t.LineaVentaId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .ForeignKey("dbo.Venta", t => t.VentaId)
                .Index(t => t.ProductoId)
                .Index(t => t.VentaId);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        ProductoId = c.Int(nullable: false, identity: true),
                        Precio = c.Double(nullable: false),
                        NombreProducto = c.String(nullable: false, maxLength: 25),
                        NombreMarca = c.String(nullable: false, maxLength: 25),
                        AnimalDirigido = c.String(),
                        Peso = c.Double(nullable: false),
                        Tamaño = c.Double(nullable: false),
                        Imagen = c.String(),
                        Stock = c.Int(nullable: false),
                        Categoria = c.String(),
                        ProveedorId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductoId)
                .ForeignKey("dbo.Proveedor", t => t.ProveedorId)
                .Index(t => t.ProveedorId);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        ProveedorId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Apellidos = c.String(nullable: false),
                        Telefono = c.String(),
                        Movil = c.String(),
                        Direccion = c.String(),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ProveedorId);
            
            CreateTable(
                "dbo.Servicio",
                c => new
                    {
                        ServicioId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        CosteServicio = c.Double(nullable: false),
                        Descripcion = c.String(),
                        Tiempo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServicioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cita", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.HistorialClinico", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.LineaVenta", "VentaId", "dbo.Venta");
            DropForeignKey("dbo.Producto", "ProveedorId", "dbo.Proveedor");
            DropForeignKey("dbo.LineaVenta", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Venta", "EmpleadoVenta_Usuario", "dbo.Empleado");
            DropForeignKey("dbo.Paciente", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Venta", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Vacuna", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.Vacuna", "Empleado_Usuario", "dbo.Empleado");
            DropForeignKey("dbo.Horario", "Empleado_Usuario", "dbo.Empleado");
            DropForeignKey("dbo.HistorialClinico", "Empleado_Usuario", "dbo.Empleado");
            DropForeignKey("dbo.Cita", "Sanitario_Usuario", "dbo.Empleado");
            DropForeignKey("dbo.Cita", "PacienteId", "dbo.Paciente");
            DropIndex("dbo.Producto", new[] { "ProveedorId" });
            DropIndex("dbo.LineaVenta", new[] { "VentaId" });
            DropIndex("dbo.LineaVenta", new[] { "ProductoId" });
            DropIndex("dbo.Venta", new[] { "EmpleadoVenta_Usuario" });
            DropIndex("dbo.Venta", new[] { "ClienteId" });
            DropIndex("dbo.Vacuna", new[] { "Empleado_Usuario" });
            DropIndex("dbo.Vacuna", new[] { "PacienteId" });
            DropIndex("dbo.Horario", new[] { "Empleado_Usuario" });
            DropIndex("dbo.HistorialClinico", new[] { "Empleado_Usuario" });
            DropIndex("dbo.HistorialClinico", new[] { "PacienteId" });
            DropIndex("dbo.Paciente", new[] { "ClienteId" });
            DropIndex("dbo.Cita", new[] { "Sanitario_Usuario" });
            DropIndex("dbo.Cita", new[] { "ServicioId" });
            DropIndex("dbo.Cita", new[] { "PacienteId" });
            DropTable("dbo.Servicio");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Producto");
            DropTable("dbo.LineaVenta");
            DropTable("dbo.Cliente");
            DropTable("dbo.Venta");
            DropTable("dbo.Vacuna");
            DropTable("dbo.Horario");
            DropTable("dbo.Empleado");
            DropTable("dbo.HistorialClinico");
            DropTable("dbo.Paciente");
            DropTable("dbo.Cita");
        }
    }
}
