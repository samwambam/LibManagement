using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibManagement.Migrations
{
    /// <inheritdoc />
    public partial class memberandfine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemberID",
                table: "Book",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LateDayRate = table.Column<int>(type: "int", nullable: false),
                    MaxFine = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.MemberID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Member",
                table: "Book",
                column: "MemberID",
                principalTable: "Member",
                principalColumn: "MemberID",
                onDelete: ReferentialAction.SetNull
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fine");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropColumn(
                name: "MemberID",
                table: "Book");
        }
    }
}
