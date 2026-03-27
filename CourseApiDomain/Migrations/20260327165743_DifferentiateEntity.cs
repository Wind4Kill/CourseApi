using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseApiDomain.Migrations
{
    /// <inheritdoc />
    public partial class DifferentiateEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Category",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Authors",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Authors",
                newName: "AuthorName");
        }
    }
}
