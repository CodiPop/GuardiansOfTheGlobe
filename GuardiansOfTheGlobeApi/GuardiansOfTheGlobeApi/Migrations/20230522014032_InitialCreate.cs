using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuardiansOfTheGlobeApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "heroes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    edad = table.Column<int>(type: "int", nullable: true),
                    habilidades = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    debilidades = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    relaciones_personales = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_heroes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "villanos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    edad = table.Column<int>(type: "int", nullable: false),
                    habilidades = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    origen = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    poder = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    debilidades = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_villanos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "agenda",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_heroe = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    evento = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agenda", x => x.id);
                    table.ForeignKey(
                        name: "FK__agenda__id_heroe__3E52440B",
                        column: x => x.id_heroe,
                        principalTable: "heroes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "patrocinadores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_heroe = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    monto = table.Column<double>(type: "float", nullable: false),
                    origen_dinero = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patrocinadores", x => x.id);
                    table.ForeignKey(
                        name: "FK__patrocina__id_he__412EB0B6",
                        column: x => x.id_heroe,
                        principalTable: "heroes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "peleas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_heroe = table.Column<int>(type: "int", nullable: false),
                    id_villano = table.Column<int>(type: "int", nullable: false),
                    resultado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peleas", x => x.id);
                    table.ForeignKey(
                        name: "FK__peleas__id_heroe__440B1D61",
                        column: x => x.id_heroe,
                        principalTable: "heroes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__peleas__id_villa__44FF419A",
                        column: x => x.id_villano,
                        principalTable: "villanos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_agenda_id_heroe",
                table: "agenda",
                column: "id_heroe");

            migrationBuilder.CreateIndex(
                name: "IX_patrocinadores_id_heroe",
                table: "patrocinadores",
                column: "id_heroe");

            migrationBuilder.CreateIndex(
                name: "IX_peleas_id_heroe",
                table: "peleas",
                column: "id_heroe");

            migrationBuilder.CreateIndex(
                name: "IX_peleas_id_villano",
                table: "peleas",
                column: "id_villano");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agenda");

            migrationBuilder.DropTable(
                name: "patrocinadores");

            migrationBuilder.DropTable(
                name: "peleas");

            migrationBuilder.DropTable(
                name: "heroes");

            migrationBuilder.DropTable(
                name: "villanos");
        }
    }
}
