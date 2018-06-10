namespace ClinicaVeterinaria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        EmpleadoId = c.Int(),
                        ServicioId = c.Int(),
                        Causa = c.String(maxLength: 80),
                        Atendida = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CitaId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .ForeignKey("dbo.Servicio", t => t.ServicioId)
                .Index(t => t.PacienteId)
                .Index(t => t.EmpleadoId)
                .Index(t => t.ServicioId);
            
            CreateTable(
                "dbo.Paciente",
                c => new
                    {
                        PacienteId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 40),
                        Especie = c.String(nullable: false, maxLength: 40),
                        Raza = c.String(),
                        Peso = c.Double(nullable: false),
                        Altura = c.Double(nullable: false),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Imagen = c.String(),
                        Sexo = c.String(nullable: false),
                        Ingresado = c.Boolean(nullable: false),
                        ClienteId = c.Int(),
                    })
                .PrimaryKey(t => t.PacienteId)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.EstadoIngresado",
                c => new
                    {
                        EstadoIngresadoId = c.Int(nullable: false, identity: true),
                        Temperatura = c.Double(nullable: false),
                        FrecuenciaCardiaca = c.Int(nullable: false),
                        FrecuenciaRespiratoria = c.Int(nullable: false),
                        RevisionGeneral = c.String(),
                        PerdidasFisiologicas = c.String(),
                        Medicacion = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        PacienteId = c.Int(),
                        EmpleadoId = c.Int(),
                    })
                .PrimaryKey(t => t.EstadoIngresadoId)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .Index(t => t.PacienteId)
                .Index(t => t.EmpleadoId);
            
            CreateTable(
                "dbo.Empleado",
                c => new
                    {
                        EmpleadoId = c.Int(nullable: false, identity: true),
                        Usuario = c.String(nullable: false, maxLength: 20),
                        Tipo = c.String(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 40),
                        Apellidos = c.String(nullable: false, maxLength: 80),
                        Telefono = c.String(),
                        Movil = c.String(),
                        Titulacion = c.String(),
                        Direccion = c.String(),
                        Email = c.String(nullable: false, maxLength: 40),
                        Contraseña = c.String(nullable: false, maxLength: 12),
                    })
                .PrimaryKey(t => t.EmpleadoId);
            
            CreateTable(
                "dbo.HistorialClinico",
                c => new
                    {
                        HistorialClinicoId = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        PacienteId = c.Int(),
                        Enfermedad = c.String(nullable: false, maxLength: 80),
                        Detalles = c.String(),
                        EmpleadoId = c.Int(),
                    })
                .PrimaryKey(t => t.HistorialClinicoId)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .Index(t => t.PacienteId)
                .Index(t => t.EmpleadoId);
            
            CreateTable(
                "dbo.Horario",
                c => new
                    {
                        HorarioId = c.Int(nullable: false, identity: true),
                        Dia = c.String(nullable: false),
                        HoraInic = c.String(nullable: false),
                        HoraFin = c.String(nullable: false),
                        EmpleadoId = c.Int(),
                    })
                .PrimaryKey(t => t.HorarioId)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .Index(t => t.EmpleadoId);
            
            CreateTable(
                "dbo.Vacuna",
                c => new
                    {
                        VacunaId = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 40),
                        PacienteId = c.Int(),
                        EmpleadoId = c.Int(),
                    })
                .PrimaryKey(t => t.VacunaId)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .Index(t => t.PacienteId)
                .Index(t => t.EmpleadoId);
            
            CreateTable(
                "dbo.Venta",
                c => new
                    {
                        VentaId = c.Int(nullable: false, identity: true),
                        FechaVenta = c.DateTime(nullable: false),
                        EmpleadoId = c.Int(),
                        ClienteId = c.Int(),
                    })
                .PrimaryKey(t => t.VentaId)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoId)
                .Index(t => t.EmpleadoId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        ClienteId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 40),
                        Apellidos = c.String(nullable: false),
                        Telefono = c.String(),
                        Movil = c.String(),
                        Direccion = c.String(maxLength: 120),
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
                        NombreProducto = c.String(nullable: false, maxLength: 40),
                        NombreMarca = c.String(nullable: false, maxLength: 40),
                        AnimalDirigido = c.String(maxLength: 40),
                        Peso = c.Double(nullable: false),
                        Tamaño = c.Double(nullable: false),
                        Imagen = c.String(),
                        Stock = c.Int(nullable: false),
                        Categoria = c.String(),
                        ProveedorId = c.Int(),
                        Habilitado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductoId)
                .ForeignKey("dbo.Proveedor", t => t.ProveedorId)
                .Index(t => t.ProveedorId);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        ProveedorId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        Apellidos = c.String(nullable: false, maxLength: 40),
                        Telefono = c.String(),
                        Movil = c.String(),
                        Direccion = c.String(maxLength: 120),
                        Email = c.String(nullable: false),
                        Habilitado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProveedorId);
            
            CreateTable(
                "dbo.Servicio",
                c => new
                    {
                        ServicioId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 40),
                        CosteServicio = c.Double(nullable: false),
                        Descripcion = c.String(maxLength: 250),
                        Tiempo = c.Int(nullable: false),
                        Habilitado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ServicioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cita", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.EstadoIngresado", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.LineaVenta", "VentaId", "dbo.Venta");
            DropForeignKey("dbo.Producto", "ProveedorId", "dbo.Proveedor");
            DropForeignKey("dbo.LineaVenta", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Venta", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.Paciente", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Venta", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Vacuna", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.Vacuna", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.Horario", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.HistorialClinico", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.HistorialClinico", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.EstadoIngresado", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.Cita", "EmpleadoId", "dbo.Empleado");
            DropForeignKey("dbo.Cita", "PacienteId", "dbo.Paciente");
            DropIndex("dbo.Producto", new[] { "ProveedorId" });
            DropIndex("dbo.LineaVenta", new[] { "VentaId" });
            DropIndex("dbo.LineaVenta", new[] { "ProductoId" });
            DropIndex("dbo.Venta", new[] { "ClienteId" });
            DropIndex("dbo.Venta", new[] { "EmpleadoId" });
            DropIndex("dbo.Vacuna", new[] { "EmpleadoId" });
            DropIndex("dbo.Vacuna", new[] { "PacienteId" });
            DropIndex("dbo.Horario", new[] { "EmpleadoId" });
            DropIndex("dbo.HistorialClinico", new[] { "EmpleadoId" });
            DropIndex("dbo.HistorialClinico", new[] { "PacienteId" });
            DropIndex("dbo.EstadoIngresado", new[] { "EmpleadoId" });
            DropIndex("dbo.EstadoIngresado", new[] { "PacienteId" });
            DropIndex("dbo.Paciente", new[] { "ClienteId" });
            DropIndex("dbo.Cita", new[] { "ServicioId" });
            DropIndex("dbo.Cita", new[] { "EmpleadoId" });
            DropIndex("dbo.Cita", new[] { "PacienteId" });
            DropTable("dbo.Servicio");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Producto");
            DropTable("dbo.LineaVenta");
            DropTable("dbo.Cliente");
            DropTable("dbo.Venta");
            DropTable("dbo.Vacuna");
            DropTable("dbo.Horario");
            DropTable("dbo.HistorialClinico");
            DropTable("dbo.Empleado");
            DropTable("dbo.EstadoIngresado");
            DropTable("dbo.Paciente");
            DropTable("dbo.Cita");
        }
    }
}
