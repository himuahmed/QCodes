using Microsoft.EntityFrameworkCore.Migrations;

namespace QCodes.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Village",
                table: "Persons",
                newName: "Union");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Persons",
                newName: "Division");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Union",
                table: "Persons",
                newName: "Village");

            migrationBuilder.RenameColumn(
                name: "Division",
                table: "Persons",
                newName: "Address");
        }
    }
}
