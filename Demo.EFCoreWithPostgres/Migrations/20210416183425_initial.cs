using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Demo.EFCoreWithPostgres.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    UnitPrice = table.Column<double>(type: "double precision", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Name", "UnitPrice", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2021, 4, 16, 0, 0, 0, 0, DateTimeKind.Local), "Product One", 1.5, null, null },
                    { 2, null, new DateTime(2021, 4, 16, 0, 0, 0, 0, DateTimeKind.Local), "Product Two", 2.5, null, null },
                    { 3, null, new DateTime(2020, 11, 17, 0, 0, 0, 0, DateTimeKind.Local), "Old Product", 3.5499999999999998, null, new DateTime(2021, 4, 16, 0, 0, 0, 0, DateTimeKind.Local) },
                    { 4, null, new DateTime(2021, 4, 16, 0, 0, 0, 0, DateTimeKind.Local), "Expensive Product", 150.99000000000001, null, new DateTime(2021, 4, 16, 0, 0, 0, 0, DateTimeKind.Local) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
