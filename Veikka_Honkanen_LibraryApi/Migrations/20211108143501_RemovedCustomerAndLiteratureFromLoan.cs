using Microsoft.EntityFrameworkCore.Migrations;

namespace Veikka_Honkanen_LibraryApi.Migrations
{
    public partial class RemovedCustomerAndLiteratureFromLoan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Literatures_LiteratureId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LiteratureId",
                table: "Loans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Loans_LiteratureId",
                table: "Loans",
                column: "LiteratureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Literatures_LiteratureId",
                table: "Loans",
                column: "LiteratureId",
                principalTable: "Literatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
