using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    InterestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.InterestID);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonID);
                });

            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    WebsiteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.WebsiteID);
                });

            migrationBuilder.CreateTable(
                name: "PersonInterest",
                columns: table => new
                {
                    InterestID = table.Column<int>(type: "int", nullable: false),
                    PersonID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonInterest", x => new { x.InterestID, x.PersonID });
                    table.ForeignKey(
                        name: "FK_PersonInterest_Interests_InterestID",
                        column: x => x.InterestID,
                        principalTable: "Interests",
                        principalColumn: "InterestID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonInterest_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterestWebsite",
                columns: table => new
                {
                    InterestID = table.Column<int>(type: "int", nullable: false),
                    WebsiteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestWebsite", x => new { x.InterestID, x.WebsiteID });
                    table.ForeignKey(
                        name: "FK_InterestWebsite_Interests_InterestID",
                        column: x => x.InterestID,
                        principalTable: "Interests",
                        principalColumn: "InterestID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestWebsite_Websites_WebsiteID",
                        column: x => x.WebsiteID,
                        principalTable: "Websites",
                        principalColumn: "WebsiteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonWebsite",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    WebsiteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonWebsite", x => new { x.PersonID, x.WebsiteID });
                    table.ForeignKey(
                        name: "FK_PersonWebsite_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonWebsite_Websites_WebsiteID",
                        column: x => x.WebsiteID,
                        principalTable: "Websites",
                        principalColumn: "WebsiteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "InterestID", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Our future.", "Tesla" },
                    { 2, "Am big rocket.", "SpaceX" },
                    { 3, "[INSERT-DESCRIPTION-HERE]", "[INSERT-TITLE-HERE]" }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "PersonID", "Address", "Email", "Name", "PhoneNumber", "Surname" },
                values: new object[,]
                {
                    { 1, "Vintergatan 999", "simon.johansson@mail.com", "Simon", "123 456 78 90", "Johansson" },
                    { 2, "Marsgatan 999", "elon.musk@mail.com", "Elon", "123 456 78 91", "Musk" },
                    { 3, "Umeå 999", "rebecca.gerdin@mail.com", "Rebecca", "123 456 78 92", "Gerdin" }
                });

            migrationBuilder.InsertData(
                table: "Websites",
                columns: new[] { "WebsiteID", "Link", "Title" },
                values: new object[,]
                {
                    { 1, "https://www.tesla.com/", "Tesla" },
                    { 2, "https://www.spacex.com/", "SpaceX" },
                    { 3, "https://youtu.be/dQw4w9WgXcQ", "FREE TESLA" }
                });

            migrationBuilder.InsertData(
                table: "InterestWebsite",
                columns: new[] { "InterestID", "WebsiteID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "PersonInterest",
                columns: new[] { "InterestID", "PersonID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "PersonWebsite",
                columns: new[] { "PersonID", "WebsiteID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestWebsite_WebsiteID",
                table: "InterestWebsite",
                column: "WebsiteID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInterest_PersonID",
                table: "PersonInterest",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonWebsite_WebsiteID",
                table: "PersonWebsite",
                column: "WebsiteID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestWebsite");

            migrationBuilder.DropTable(
                name: "PersonInterest");

            migrationBuilder.DropTable(
                name: "PersonWebsite");

            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Websites");
        }
    }
}
