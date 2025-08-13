using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserProfileManager.Migrations
{
    /// <inheritdoc />
    public partial class SyncUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    NIK = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    BirthPlace = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    Province = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    Nationality = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    MaritalStatus = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Occupation = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    EducationLevel = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    BloodType = table.Column<string>(type: "TEXT", maxLength: 3, nullable: true),
                    PhotoFileName = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
