using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeestjeOpJeFeestjeDb.Migrations
{
    /// <inheritdoc />
    public partial class seeddateanimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Animal",
                columns: new[] { "animal_id", "imageUrl", "name", "price", "type" },
                values: new object[,]
                {
                    { 1, "#", "Aap", 150.00m, "Jungle" },
                    { 2, "#", "Olifant", 500.00m, "Jungle" },
                    { 3, "#", "Zebra", 200.00m, "Jungle" },
                    { 4, "#", "Leeuw", 600.00m, "Jungle" },
                    { 5, "#", "Hond", 50.00m, "Boerderij" },
                    { 6, "#", "Ezel", 100.00m, "Boerderij" },
                    { 7, "#", "Koe", 250.00m, "Boerderij" },
                    { 8, "#", "Eend", 30.00m, "Boerderij" },
                    { 9, "#", "Kuiken", 10.00m, "Boerderij" },
                    { 10, "#", "Pinguïn", 100.00m, "Sneeuw" },
                    { 11, "#", "IJsbeer", 350.00m, "Sneeuw" },
                    { 12, "#", "Zeehond", 120.00m, "Sneeuw" },
                    { 13, "#", "Kameel", 400.00m, "Woestijn" },
                    { 14, "#", "Slang", 75.00m, "Woestijn" },
                    { 15, "#", "T-Rex", 1000.00m, "VIP" },
                    { 16, "#", "Unicorn", 1500.00m, "VIP" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ccf1490d-2d05-4cfe-915d-17d4ab925a63", "AQAAAAIAAYagAAAAEG5fY7szSR1BnlaYdk9IPYjuzuZmgFAAqIwRz6V9MMczB6JnGoPJsDzZZx8cUHo5gQ==", "4dc13235-32b5-45cc-b739-9bf06a884130" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a0986696-2886-4bb5-b330-00f410592b9b", "AQAAAAIAAYagAAAAEFyKRbYrU9skiteuqE14c8BYtjXdINW797Msd150os6Uz4F64YouCpUFarxwsnuGAA==", "1dc06cf4-1ded-44e6-8618-af80c9c416be" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Animal",
                keyColumn: "animal_id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d2302a5-c42d-4bd5-bc98-64a38808e5dc", "AQAAAAIAAYagAAAAEMtQGFUk0AMkbulJeBdukwlB7CkKEGt8IE/FuzMGdB4mnU+Kn5pJE8GGqza27oBw8Q==", "1a030ae5-c97d-47e3-ab3a-1646dd5f03b4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a0fd5db6-534b-405d-98b7-445d326f3a5d", "AQAAAAIAAYagAAAAEF9v+GB9ru2xUjMZwI818w4MkEKCMJFWsexmBJ6ZoZbI9VcChbdQcjckQCzU+EceIw==", "776d940d-0e63-4834-963a-d2597f3e3e69" });
        }
    }
}
