using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team.Repo.Migrations
{
    /// <inheritdoc />
    public partial class Initialcommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "Email", "Coachid", "ContactNumber", "Dob", "FirstName", "FlagRole", "LastName" },
                values: new object[] { "nilaydoshi@gmail.com", 1, "9033062657", new DateOnly(1999, 1, 2), "Nilay", 5, "Doshi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coaches",
                keyColumn: "Email",
                keyValue: "nilaydoshi@gmail.com");
        }
    }
}
