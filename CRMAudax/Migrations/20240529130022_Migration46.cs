using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration46 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "socioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "socioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "pepTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "pepSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "nomeSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "nomeSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "enderecoSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "enderecoSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "cpfSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "cpfSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "celularSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<string>(
                name: "celularSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "socioTerceiro",
                keyValue: null,
                column: "socioTerceiro",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "socioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "socioSegundo",
                keyValue: null,
                column: "socioSegundo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "socioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "pepTerceiro",
                keyValue: null,
                column: "pepTerceiro",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "pepTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "pepSegundo",
                keyValue: null,
                column: "pepSegundo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "pepSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "nomeSocioTerceiro",
                keyValue: null,
                column: "nomeSocioTerceiro",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "nomeSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "nomeSocioSegundo",
                keyValue: null,
                column: "nomeSocioSegundo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "nomeSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "enderecoSocioTerceiro",
                keyValue: null,
                column: "enderecoSocioTerceiro",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "enderecoSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "enderecoSocioSegundo",
                keyValue: null,
                column: "enderecoSocioSegundo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "enderecoSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "cpfSocioTerceiro",
                keyValue: null,
                column: "cpfSocioTerceiro",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "cpfSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "cpfSocioSegundo",
                keyValue: null,
                column: "cpfSocioSegundo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "cpfSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "celularSocioTerceiro",
                keyValue: null,
                column: "celularSocioTerceiro",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "celularSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "celularSocioSegundo",
                keyValue: null,
                column: "celularSocioSegundo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "celularSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");
        }
    }
}
