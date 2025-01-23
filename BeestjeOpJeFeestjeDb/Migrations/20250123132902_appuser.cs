using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeestjeOpJeFeestjeDb.Migrations
{
    /// <inheritdoc />
    public partial class appuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_booking_id",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AppUser");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AppUser",
                newName: "appuser_id");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "Animal",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "card",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "appuser_id");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "appuser_id",
                keyValue: "1",
                columns: new[] { "card", "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { null, "e4b0fbde-61eb-430e-8c09-ac2268445238", "JURIAN", "AQAAAAIAAYagAAAAEOAtd6nUonCu2qiOVUdrmjyjlbPfHjmUiuyrjx1Mjz7szNRDqnM9JOSCY657ibUcqQ==", "13e1d16d-a1c7-4fc9-90f0-ca248b3b15dd", "jurian" });

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "appuser_id",
                keyValue: "2",
                columns: new[] { "card", "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { null, "2dde62aa-31be-4913-ace4-d6ea97b6ff95", "ETHAN", "AQAAAAIAAYagAAAAEIe9zp7WRL0jJUYnUBIhza4Mfbd7tFd6FqgZWEqsC8kBJBvWA+Chfm+hF4RtJhKwvg==", "b2854e6f-64a2-49ab-8c35-16ef496fbe46", "ethan" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AppUser_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "appuser_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AppUser_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "appuser_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AppUser_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "appuser_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AppUser_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "appuser_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AppUser_booking_id",
                table: "Booking",
                column: "booking_id",
                principalTable: "AppUser",
                principalColumn: "appuser_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AppUser_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AppUser_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AppUser_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AppUser_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AppUser_booking_id",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "card",
                table: "AppUser");

            migrationBuilder.RenameTable(
                name: "AppUser",
                newName: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "appuser_id",
                table: "AspNetUsers",
                newName: "Id");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "Animal",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "8bfe2862-eb64-42f8-93d5-a47bc6227749", "KORAY YILMAZ", "AQAAAAIAAYagAAAAEH3KLjfVnUyqXuDkERmivfLU6v631jyFSRFdlushRuj+JESXT3sIGVOh0HjiFGEBag==", "e873fffa-39fd-4312-b63b-57583b8c9e3a", "Koray Yilmaz" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "bb9548bd-699d-4e11-a65a-14f0ae4cb356", "KORAY YILMAZS", "AQAAAAIAAYagAAAAEGZHK+/LCbmA6vKNxfqg4aGIvJKgBzSYApxzWoQkFckXppQxn2+5gh2062GHnXx/Ng==", "7ad5d723-97f4-4061-bd60-d9e156d7a062", "Koray Yilmazs" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_booking_id",
                table: "Booking",
                column: "booking_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
