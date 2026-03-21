using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FamilyTree.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Bio", "BirthDate", "CreatedAt", "DeathDate", "FirstName", "Gender", "LastName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111001"), "Основатель семейства", new DateOnly(1970, 1, 1), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Иван", 1, "Иванов", null },
                    { new Guid("11111111-1111-1111-1111-111111111002"), "Сын Ивана", new DateOnly(1995, 5, 15), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Петр", 1, "Иванов", null },
                    { new Guid("11111111-1111-1111-1111-111111111003"), "Дочь Ивана", new DateOnly(1998, 8, 20), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Анна", 2, "Иванова", null }
                });

            migrationBuilder.InsertData(
                table: "Relationships",
                columns: new[] { "Id", "CreatedAt", "PersonId", "RelatedPersonId", "Type" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111001"), new Guid("11111111-1111-1111-1111-111111111002"), 1 },
                    { new Guid("22222222-2222-2222-2222-222222222002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111001"), new Guid("11111111-1111-1111-1111-111111111003"), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Relationships",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222001"));

            migrationBuilder.DeleteData(
                table: "Relationships",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222002"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111001"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111002"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111003"));
        }
    }
}
