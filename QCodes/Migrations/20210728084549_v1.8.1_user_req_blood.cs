using Microsoft.EntityFrameworkCore.Migrations;

namespace QCodes.Migrations
{
    public partial class v181_user_req_blood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodRequests_Persons_PersonId",
                table: "BloodRequests");

            migrationBuilder.DropIndex(
                name: "IX_BloodRequests_PersonId",
                table: "BloodRequests");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "BloodRequests");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "BloodRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "BloodRequests");

            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "BloodRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_PersonId",
                table: "BloodRequests",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodRequests_Persons_PersonId",
                table: "BloodRequests",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
