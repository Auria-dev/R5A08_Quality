using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace R508_Revisions.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "t_e_marque",
                schema: "public",
                columns: table => new
                {
                    mrq_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mrq_nom = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_marque", x => x.mrq_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_typeproduit",
                schema: "public",
                columns: table => new
                {
                    tpp_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tpp_nom = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_typeproduit", x => x.tpp_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_produit",
                schema: "public",
                columns: table => new
                {
                    prd_produit = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prd_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prd_description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prd_nomphoto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prd_uriphoto = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    prd_idtype = table.Column<int>(type: "integer", nullable: false),
                    prd_idmarque = table.Column<int>(type: "integer", nullable: false),
                    prd_stockreel = table.Column<int>(type: "integer", nullable: false),
                    prd_stockmin = table.Column<int>(type: "integer", nullable: false),
                    prd_stockmax = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_produit", x => x.prd_produit);
                    table.ForeignKey(
                        name: "fk_produit_marque",
                        column: x => x.prd_idmarque,
                        principalSchema: "public",
                        principalTable: "t_e_marque",
                        principalColumn: "mrq_id");
                    table.ForeignKey(
                        name: "fk_produit_typeproduit",
                        column: x => x.prd_idtype,
                        principalSchema: "public",
                        principalTable: "t_e_typeproduit",
                        principalColumn: "tpp_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_prd_idmarque",
                schema: "public",
                table: "t_e_produit",
                column: "prd_idmarque");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_prd_idtype",
                schema: "public",
                table: "t_e_produit",
                column: "prd_idtype");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_produit",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_marque",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_typeproduit",
                schema: "public");
        }
    }
}
