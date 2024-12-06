using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration54 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Score",
                table: "Notificacoes",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Notificacoes",
                type: "tinyint(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notificacoes");
        }
    }
}
