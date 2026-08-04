using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProjetoIntegrador.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTabelaReceitas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Receitas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ImagemUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    TagRestricao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    TempoPreparoMinutos = table.Column<int>(type: "int", nullable: false),
                    Dificuldade = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    ProteinaPorcentagem = table.Column<int>(type: "int", nullable: false),
                    CarboidratosPorcentagem = table.Column<int>(type: "int", nullable: false),
                    GordurasPorcentagem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receitas", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receitas");
        }
    }
}
