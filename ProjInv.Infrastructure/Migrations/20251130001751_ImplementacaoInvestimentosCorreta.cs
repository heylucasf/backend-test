using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjInv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImplementacaoInvestimentosCorreta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Investidores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investidores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Investimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvestidorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investimentos_Investidores_InvestidorId",
                        column: x => x.InvestidorId,
                        principalTable: "Investidores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Retiradas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvestimentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataRetirada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impostos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorLiquido = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retiradas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Retiradas_Investimentos_InvestimentoId",
                        column: x => x.InvestimentoId,
                        principalTable: "Investimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investimentos_InvestidorId",
                table: "Investimentos",
                column: "InvestidorId");

            migrationBuilder.CreateIndex(
                name: "IX_Retiradas_InvestimentoId",
                table: "Retiradas",
                column: "InvestimentoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Retiradas");

            migrationBuilder.DropTable(
                name: "Investimentos");

            migrationBuilder.DropTable(
                name: "Investidores");
        }
    }
}
