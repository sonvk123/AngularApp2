using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularApp2.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListMessageTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListMessageTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sender_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sender_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message_content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    media_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    media_path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTables", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListMessageTables");

            migrationBuilder.DropTable(
                name: "MessageTables");
        }
    }
}
