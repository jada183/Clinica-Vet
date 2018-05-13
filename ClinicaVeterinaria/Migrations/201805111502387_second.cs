namespace ClinicaVeterinaria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Producto", "Proveedor_ProveedorId", "dbo.Proveedor");
            DropIndex("dbo.Producto", new[] { "Proveedor_ProveedorId" });
            DropColumn("dbo.Producto", "ProovedorId");
            RenameColumn(table: "dbo.Producto", name: "Proveedor_ProveedorId", newName: "ProovedorId");
            DropPrimaryKey("dbo.Proveedor");
            AlterColumn("dbo.Producto", "ProovedorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Producto", "ProovedorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Proveedor", "Email", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Proveedor", "Email");
            CreateIndex("dbo.Producto", "ProovedorId");
            AddForeignKey("dbo.Producto", "ProovedorId", "dbo.Proveedor", "Email");
            DropColumn("dbo.Proveedor", "ProveedorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proveedor", "ProveedorId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Producto", "ProovedorId", "dbo.Proveedor");
            DropIndex("dbo.Producto", new[] { "ProovedorId" });
            DropPrimaryKey("dbo.Proveedor");
            AlterColumn("dbo.Proveedor", "Email", c => c.String());
            AlterColumn("dbo.Producto", "ProovedorId", c => c.Int());
            AlterColumn("dbo.Producto", "ProovedorId", c => c.Int());
            AddPrimaryKey("dbo.Proveedor", "ProveedorId");
            RenameColumn(table: "dbo.Producto", name: "ProovedorId", newName: "Proveedor_ProveedorId");
            AddColumn("dbo.Producto", "ProovedorId", c => c.Int());
            CreateIndex("dbo.Producto", "Proveedor_ProveedorId");
            AddForeignKey("dbo.Producto", "Proveedor_ProveedorId", "dbo.Proveedor", "ProveedorId");
        }
    }
}
