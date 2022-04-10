using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catalog.Persistence.DBLocating.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "RegionHiLo",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RegionCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Version = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                },
                comment: "Регионы справочника ГАР");

            migrationBuilder.CreateTable(
                name: "LocationObject",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationType = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    ProperName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    PlainCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Okato = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Structure = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationObject_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Таблица справочника ГАР");

            migrationBuilder.CreateIndex(
                name: "IX_LocationObject_ParentId",
                table: "LocationObject",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationObject_RegionId",
                table: "LocationObject",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationObject_Uid",
                table: "LocationObject",
                column: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationObject");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropSequence(
                name: "RegionHiLo",
                schema: "dbo");
        }
    }
}
