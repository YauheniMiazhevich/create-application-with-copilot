using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2069), "Electronic devices and accessories", "Electronics", new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2070) },
                    { 2, new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2072), "Books and publications", "Books", new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2072) },
                    { 3, new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2073), "Apparel and fashion items", "Clothing", new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2074) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "Name", "Price", "Stock", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2150), "High-performance laptop", "Laptop", 999.99m, 15, new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2150) },
                    { 2, 1, new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2152), "Latest smartphone model", "Smartphone", 699.99m, 25, new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2152) },
                    { 3, 2, new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2153), "Learn programming fundamentals", "Programming Book", 49.99m, 50, new DateTime(2026, 2, 16, 19, 0, 49, 666, DateTimeKind.Utc).AddTicks(2153) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
