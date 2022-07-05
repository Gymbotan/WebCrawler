using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCrawler.Migrations
{
    public partial class _initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    FullText = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeoAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Alpha2 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    INN = table.Column<string>(type: "text", nullable: true),
                    Geo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    Gender = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleGeoAttribute",
                columns: table => new
                {
                    GeoAttributesId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleGeoAttribute", x => new { x.GeoAttributesId, x.OwnersId });
                    table.ForeignKey(
                        name: "FK_ArticleGeoAttribute_Articles_OwnersId",
                        column: x => x.OwnersId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleGeoAttribute_GeoAttributes_GeoAttributesId",
                        column: x => x.GeoAttributesId,
                        principalTable: "GeoAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleOrganizationAttribute",
                columns: table => new
                {
                    OrganizationAttributesId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleOrganizationAttribute", x => new { x.OrganizationAttributesId, x.OwnersId });
                    table.ForeignKey(
                        name: "FK_ArticleOrganizationAttribute_Articles_OwnersId",
                        column: x => x.OwnersId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleOrganizationAttribute_OrganizationAttributes_Organiz~",
                        column: x => x.OrganizationAttributesId,
                        principalTable: "OrganizationAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticlePersonAttribute",
                columns: table => new
                {
                    OwnersId = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonAttributesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlePersonAttribute", x => new { x.OwnersId, x.PersonAttributesId });
                    table.ForeignKey(
                        name: "FK_ArticlePersonAttribute_Articles_OwnersId",
                        column: x => x.OwnersId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticlePersonAttribute_PersonAttributes_PersonAttributesId",
                        column: x => x.PersonAttributesId,
                        principalTable: "PersonAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleGeoAttribute_OwnersId",
                table: "ArticleGeoAttribute",
                column: "OwnersId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleOrganizationAttribute_OwnersId",
                table: "ArticleOrganizationAttribute",
                column: "OwnersId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticlePersonAttribute_PersonAttributesId",
                table: "ArticlePersonAttribute",
                column: "PersonAttributesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleGeoAttribute");

            migrationBuilder.DropTable(
                name: "ArticleOrganizationAttribute");

            migrationBuilder.DropTable(
                name: "ArticlePersonAttribute");

            migrationBuilder.DropTable(
                name: "GeoAttributes");

            migrationBuilder.DropTable(
                name: "OrganizationAttributes");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "PersonAttributes");
        }
    }
}
