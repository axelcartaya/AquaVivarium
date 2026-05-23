using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class fixRelacionUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
           name: "IX_Acuarios_UsuarioId",
           table: "Acuarios",
           column: "UsuarioId");

            migrationBuilder.AddForeignKey(
          name: "FK_Acuarios_AspNetUsers_UsuarioId",
          table: "Acuarios",
          column: "UsuarioId",
          principalTable: "AspNetUsers",
          principalColumn: "Id",
          onDelete: ReferentialAction.NoAction); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acuarios_AspNetUsers_UsuarioId",
                table: "Acuarios");

            migrationBuilder.DropIndex(
                name: "IX_Acuarios_UsuarioId",
                table: "Acuarios");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Acuarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Acuarios_ApplicationUserId",
                table: "Acuarios",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Acuarios_AspNetUsers_ApplicationUserId",
                table: "Acuarios",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
