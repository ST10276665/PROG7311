using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7311.Migrations
{
    /// <inheritdoc />
    public partial class AddSignedAgreementToContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignedAgreement",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignedAgreement",
                table: "Contracts");
        }
    }
}
