using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infinite.Core.Persistence.Migrations
{
    public partial class UpdatedUserPortfolio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileMarkdown",
                table: "UserProfiles",
                newName: "PortfolioMarkdown");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PortfolioMarkdown",
                table: "UserProfiles",
                newName: "ProfileMarkdown");
        }
    }
}
