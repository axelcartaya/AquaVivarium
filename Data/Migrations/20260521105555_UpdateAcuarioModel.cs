using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAcuarioModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstiloId",
                table: "Acuarios");

            migrationBuilder.AddColumn<int>(
                name: "LargoCm",
                table: "Acuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AltoCm",
                table: "Acuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnchoCm",
                table: "Acuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlujoAgua",
                table: "Acuarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GhActual",
                table: "Acuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NivelIluminacion",
                table: "Acuarios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneCo2",
                table: "Acuarios",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoSustrato",
                table: "Acuarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altura",
                table: "Plantas");

            migrationBuilder.DropColumn(
                name: "DerechosAutor",
                table: "EspecieImagenes");

            migrationBuilder.DropColumn(
                name: "AltoCm",
                table: "Acuarios");

            migrationBuilder.DropColumn(
                name: "AnchoCm",
                table: "Acuarios");

            migrationBuilder.DropColumn(
                name: "FlujoAgua",
                table: "Acuarios");

            migrationBuilder.DropColumn(
                name: "GhActual",
                table: "Acuarios");

            migrationBuilder.DropColumn(
                name: "NivelIluminacion",
                table: "Acuarios");

            migrationBuilder.DropColumn(
                name: "TieneCo2",
                table: "Acuarios");

            migrationBuilder.DropColumn(
                name: "TipoSustrato",
                table: "Acuarios");

            migrationBuilder.RenameColumn(
                name: "Crecimiento",
                table: "Plantas",
                newName: "PlanoAcuario");

            migrationBuilder.RenameColumn(
                name: "LargoCm",
                table: "Acuarios",
                newName: "EstiloId");
        }
    }
}
