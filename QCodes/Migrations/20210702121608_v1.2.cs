using Microsoft.EntityFrameworkCore.Migrations;

namespace QCodes.Migrations
{
    public partial class v12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PersonId",
                table: "QTags",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_QTags_PersonId",
                table: "QTags",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_QTags_Persons_PersonId",
                table: "QTags",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QTags_Persons_PersonId",
                table: "QTags");

            migrationBuilder.DropIndex(
                name: "IX_QTags_PersonId",
                table: "QTags");

            migrationBuilder.AlterColumn<string>(
                name: "PersonId",
                table: "QTags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
