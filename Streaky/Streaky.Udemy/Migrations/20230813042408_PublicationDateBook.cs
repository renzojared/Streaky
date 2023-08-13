using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streaky.Udemy.Migrations
{
    public partial class PublicationDateBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Book",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Book");
        }
    }
}
