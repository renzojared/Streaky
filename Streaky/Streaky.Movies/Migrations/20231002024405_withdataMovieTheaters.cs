using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Streaky.Movies.Migrations
{
    /// <inheritdoc />
    public partial class withdataMovieTheaters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MovieTheaters",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { 2, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-11.994766 -11.994766)"), "Mall de Comas" },
                    { 3, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-77.06287 -11.994766)"), "Mega Plaza" },
                    { 4, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-105.619548 32.675991)"), "Espania Mall" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MovieTheaters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MovieTheaters",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MovieTheaters",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
