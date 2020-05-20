using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AcornBox.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Length = table.Column<long>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Schema = table.Column<string>(nullable: true),
                    GenerateSchemaJobId = table.Column<string>(nullable: true),
                    GenerateSchemaWorker = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileEntries");
        }
    }
}
