using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInstanciaBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EsporteModalidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EsporteModalidade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusPedido", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoOperacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoOperacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    TipoUsuario = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.CheckConstraint("CK_Usuario_EmailValido", "[Email] LIKE '%@%' AND [Email] LIKE '%.%'");
                    table.CheckConstraint("CK_Usuario_SenhaMinima", "LEN([Senha]) >= 8");
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    Preco = table.Column<double>(type: "FLOAT", nullable: false),
                    EsporteModalidadeId = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_EsporteModalidade_EsporteModalidadeId",
                        column: x => x.EsporteModalidadeId,
                        principalTable: "EsporteModalidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataPedido = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    VendedorId = table.Column<int>(type: "INT", nullable: false),
                    DocumentoCliente = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    EmailCliente = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    StatusPedidoId = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedido_StatusPedido_StatusPedidoId",
                        column: x => x.StatusPedidoId,
                        principalTable: "StatusPedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedido_Usuario_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoEstoque",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutoId = table.Column<int>(type: "INT", nullable: false),
                    NotaFiscal = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Quantidade = table.Column<int>(type: "INT", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoEstoque_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoProduto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "INT", nullable: false),
                    ProdutoId = table.Column<int>(type: "INT", nullable: false),
                    Quantidade = table.Column<int>(type: "INT", nullable: false),
                    ProdutoEstoqueId = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoProduto_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoProduto_ProdutoEstoque_ProdutoEstoqueId",
                        column: x => x.ProdutoEstoqueId,
                        principalTable: "ProdutoEstoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoEstoqueMovimentacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutoEstoqueId = table.Column<int>(type: "INT", nullable: false),
                    TipoOperacaoId = table.Column<int>(type: "INT", nullable: false),
                    Quantidade = table.Column<int>(type: "INT", nullable: false),
                    IdUsuario = table.Column<int>(type: "INT", nullable: false),
                    DataMovimentacao = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoEstoqueMovimentacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoEstoqueMovimentacao_ProdutoEstoque_ProdutoEstoqueId",
                        column: x => x.ProdutoEstoqueId,
                        principalTable: "ProdutoEstoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoEstoqueMovimentacao_TipoOperacao_TipoOperacaoId",
                        column: x => x.TipoOperacaoId,
                        principalTable: "TipoOperacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoEstoqueMovimentacao_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_StatusPedidoId",
                table: "Pedido",
                column: "StatusPedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_VendedorId",
                table: "Pedido",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProduto_PedidoId",
                table: "PedidoProduto",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProduto_ProdutoEstoqueId",
                table: "PedidoProduto",
                column: "ProdutoEstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProduto_ProdutoId",
                table: "PedidoProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_EsporteModalidadeId",
                table: "Produto",
                column: "EsporteModalidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoEstoque_ProdutoId",
                table: "ProdutoEstoque",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoEstoqueMovimentacao_IdUsuario",
                table: "ProdutoEstoqueMovimentacao",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoEstoqueMovimentacao_ProdutoEstoqueId",
                table: "ProdutoEstoqueMovimentacao",
                column: "ProdutoEstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoEstoqueMovimentacao_TipoOperacaoId",
                table: "ProdutoEstoqueMovimentacao",
                column: "TipoOperacaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoProduto");

            migrationBuilder.DropTable(
                name: "ProdutoEstoqueMovimentacao");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "ProdutoEstoque");

            migrationBuilder.DropTable(
                name: "TipoOperacao");

            migrationBuilder.DropTable(
                name: "StatusPedido");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "EsporteModalidade");
        }
    }
}
