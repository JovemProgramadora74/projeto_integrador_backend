using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoIntegrador.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaRelacionamentoEntreTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Contatos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Alertas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_IdUsuario",
                table: "Contatos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_UsuarioId",
                table: "Alertas",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alertas_Usuarios_UsuarioId",
                table: "Alertas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_Usuarios_IdUsuario",
                table: "Contatos",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alertas_Usuarios_UsuarioId",
                table: "Alertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_Usuarios_IdUsuario",
                table: "Contatos");

            migrationBuilder.DropIndex(
                name: "IX_Contatos_IdUsuario",
                table: "Contatos");

            migrationBuilder.DropIndex(
                name: "IX_Alertas_UsuarioId",
                table: "Alertas");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Alertas");
        }
    }
}
