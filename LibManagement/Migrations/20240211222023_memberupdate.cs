using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibManagement.Migrations
{
    /// <inheritdoc />
    public partial class memberupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Member");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Member",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
