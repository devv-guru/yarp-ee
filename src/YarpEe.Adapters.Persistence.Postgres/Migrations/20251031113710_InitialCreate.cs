using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YarpEe.Adapters.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "certificates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    kind = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    location = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    password_secret = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clusters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    load_balancing_policy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clusters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hosts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    base_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    certificate_ref = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hosts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "routes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    host_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cluster_id = table.Column<Guid>(type: "uuid", nullable: false),
                    path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    methods = table.Column<string>(type: "text", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "destinations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cluster_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    health_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_destinations", x => x.id);
                    table.ForeignKey(
                        name: "FK_destinations_clusters_cluster_id",
                        column: x => x.cluster_id,
                        principalTable: "clusters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clusters_name",
                table: "clusters",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_destinations_cluster_id",
                table: "destinations",
                column: "cluster_id");

            migrationBuilder.CreateIndex(
                name: "IX_hosts_name",
                table: "hosts",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_routes_host_id_path",
                table: "routes",
                columns: new[] { "host_id", "path" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "certificates");

            migrationBuilder.DropTable(
                name: "destinations");

            migrationBuilder.DropTable(
                name: "hosts");

            migrationBuilder.DropTable(
                name: "routes");

            migrationBuilder.DropTable(
                name: "clusters");
        }
    }
}
