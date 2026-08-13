using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoIntegrador.Backend.Migrations
{
    /// <inheritdoc />
    public partial class RenomeiaTabelaReceitasFavoritas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Executa a remoção usando a sintaxe nativa do MySQL
            migrationBuilder.Sql("ALTER TABLE `ReceitaFavoritas` DROP FOREIGN KEY `FK_ReceitaFavoritas_Receitas_ReceitaId`;");
            migrationBuilder.Sql("ALTER TABLE `ReceitaFavoritas` DROP FOREIGN KEY `FK_ReceitaFavoritas_Usuarios_UsuarioId`;");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceitaFavoritas",
                table: "ReceitaFavoritas");

            migrationBuilder.RenameTable(
                name: "ReceitaFavoritas",
                newName: "ReceitasFavoritas");

            migrationBuilder.RenameIndex(
                name: "IX_ReceitaFavoritas_ReceitaId",
                table: "ReceitasFavoritas",
                newName: "IX_ReceitasFavoritas_ReceitaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceitasFavoritas",
                table: "ReceitasFavoritas",
                columns: new[] { "UsuarioId", "ReceitaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReceitasFavoritas_Receitas_ReceitaId",
                table: "ReceitasFavoritas",
                column: "ReceitaId",
                principalTable: "Receitas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceitasFavoritas_Usuarios_UsuarioId",
                table: "ReceitasFavoritas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE `ReceitasFavoritas` DROP FOREIGN KEY `FK_ReceitasFavoritas_Receitas_ReceitaId`;");
            migrationBuilder.Sql("ALTER TABLE `ReceitasFavoritas` DROP FOREIGN KEY `FK_ReceitasFavoritas_Usuarios_UsuarioId`;");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceitasFavoritas",
                table: "ReceitasFavoritas");

            migrationBuilder.RenameTable(
                name: "ReceitasFavoritas",
                newName: "ReceitaFavoritas");

            migrationBuilder.RenameIndex(
                name: "IX_ReceitasFavoritas_ReceitaId",
                table: "ReceitaFavoritas",
                newName: "IX_ReceitaFavoritas_ReceitaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceitaFavoritas",
                table: "ReceitaFavoritas",
                columns: new[] { "UsuarioId", "ReceitaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReceitaFavoritas_Receitas_ReceitaId",
                table: "ReceitaFavoritas",
                column: "ReceitaId",
                principalTable: "Receitas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceitaFavoritas_Usuarios_UsuarioId",
                table: "ReceitaFavoritas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
