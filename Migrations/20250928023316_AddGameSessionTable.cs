using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BF2Statistics.Migrations
{
    /// <inheritdoc />
    public partial class AddGameSessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "game_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    server_ip = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false, defaultValue: ""),
                    game_port = table.Column<int>(type: "INTEGER", nullable: false),
                    query_port = table.Column<int>(type: "INTEGER", nullable: false),
                    map_name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, defaultValue: ""),
                    map_id = table.Column<int>(type: "INTEGER", nullable: false),
                    map_start = table.Column<double>(type: "REAL", nullable: false),
                    map_end = table.Column<double>(type: "REAL", nullable: false),
                    winner = table.Column<int>(type: "INTEGER", nullable: false),
                    game_mode = table.Column<int>(type: "INTEGER", nullable: false),
                    mod_id = table.Column<int>(type: "INTEGER", nullable: false),
                    version = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false, defaultValue: "bf2"),
                    pc = table.Column<int>(type: "INTEGER", nullable: false),
                    ra1 = table.Column<int>(type: "INTEGER", nullable: false),
                    rs1 = table.Column<int>(type: "INTEGER", nullable: false),
                    ra2 = table.Column<int>(type: "INTEGER", nullable: false),
                    rs2 = table.Column<int>(type: "INTEGER", nullable: false),
                    rst2 = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2025, 9, 28, 2, 33, 15, 904, DateTimeKind.Utc).AddTicks(4673)),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2025, 9, 28, 2, 33, 15, 904, DateTimeKind.Utc).AddTicks(4933))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_sessions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_created_at",
                table: "game_sessions",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_map_name",
                table: "game_sessions",
                column: "map_name");

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_map_start",
                table: "game_sessions",
                column: "map_start");

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_server_ip",
                table: "game_sessions",
                column: "server_ip");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_sessions");
        }
    }
}
