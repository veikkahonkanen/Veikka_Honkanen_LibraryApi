using Microsoft.EntityFrameworkCore.Migrations;

namespace Veikka_Honkanen_LibraryApi.Migrations
{
    public partial class AddedCustomerAndLiteratureToLoanAndSwappedLiteratureIsLentPropertyIntoAvailableCopies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLent",
                table: "Literatures");

            migrationBuilder.AddColumn<long>(
                name: "AvailableCopies",
                table: "Literatures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Literatures_LiteratureId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LiteratureId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "AvailableCopies",
                table: "Literatures");

            migrationBuilder.AddColumn<bool>(
                name: "IsLent",
                table: "Literatures",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
