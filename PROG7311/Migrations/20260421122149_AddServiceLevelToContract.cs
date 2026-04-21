using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7311.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceLevelToContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceLevel",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceLevel",
                table: "Contracts");
        }
    }
}
