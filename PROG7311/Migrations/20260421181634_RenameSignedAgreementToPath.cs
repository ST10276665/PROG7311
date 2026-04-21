using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7311.Migrations
{
    public partial class RenameSignedAgreementToPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignedAgreement",
                table: "Contracts",
                newName: "SignedAgreementPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignedAgreementPath",
                table: "Contracts",
                newName: "SignedAgreement");
        }
    }
}