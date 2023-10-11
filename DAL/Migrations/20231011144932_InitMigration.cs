using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                column: "Id",
                values: new object[]
                {
                    1,
                    2,
                    3,
                    4
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "Email", "Name" },
                values: new object[,]
                {
                    { 1, 23, "user1@test.com", "User 1" },
                    { 2, 87, "user2@test.com", "User 2" },
                    { 3, 56, "user3@test.com", "User 3" },
                    { 4, 59, "user4@test.com", "User 4" },
                    { 5, 50, "user5@test.com", "User 5" },
                    { 6, 43, "user6@test.com", "User 6" },
                    { 7, 93, "user7@test.com", "User 7" },
                    { 8, 87, "user8@test.com", "User 8" },
                    { 9, 53, "user9@test.com", "User 9" },
                    { 10, 58, "user10@test.com", "User 10" },
                    { 11, 54, "user11@test.com", "User 11" },
                    { 12, 97, "user12@test.com", "User 12" },
                    { 13, 99, "user13@test.com", "User 13" },
                    { 14, 99, "user14@test.com", "User 14" },
                    { 15, 75, "user15@test.com", "User 15" },
                    { 16, 66, "user16@test.com", "User 16" },
                    { 17, 55, "user17@test.com", "User 17" },
                    { 18, 22, "user18@test.com", "User 18" },
                    { 19, 93, "user19@test.com", "User 19" },
                    { 20, 92, "user20@test.com", "User 20" }
                });

            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 4 },
                    { 1, 6 },
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 10 },
                    { 1, 11 },
                    { 1, 12 },
                    { 1, 13 },
                    { 1, 14 },
                    { 1, 16 },
                    { 1, 17 },
                    { 2, 1 },
                    { 2, 5 },
                    { 2, 9 },
                    { 2, 10 },
                    { 2, 12 },
                    { 2, 20 },
                    { 3, 2 },
                    { 3, 6 },
                    { 3, 7 },
                    { 3, 15 },
                    { 3, 18 },
                    { 3, 19 },
                    { 4, 3 },
                    { 4, 4 },
                    { 4, 7 },
                    { 4, 8 },
                    { 4, 14 },
                    { 4, 19 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserId",
                table: "RoleUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
