using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseApiDomain.Migrations
{
    /// <inheritdoc />
    public partial class CategoriesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Courses_CourseId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_CourseId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Category");

            migrationBuilder.CreateTable(
                name: "CategoryCourse",
                columns: table => new
                {
                    CategoriesCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CoursesCourseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCourse", x => new { x.CategoriesCategoryId, x.CoursesCourseId });
                    table.ForeignKey(
                        name: "FK_CategoryCourse_Category_CategoriesCategoryId",
                        column: x => x.CategoriesCategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCourse_Courses_CoursesCourseId",
                        column: x => x.CoursesCourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 4,
                column: "Name",
                value: "John Doe");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCourse_CoursesCourseId",
                table: "CategoryCourse",
                column: "CoursesCourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCourse");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Category",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 4,
                column: "Name",
                value: "JohnDoe");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CourseId",
                value: 5);

            migrationBuilder.CreateIndex(
                name: "IX_Category_CourseId",
                table: "Category",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Courses_CourseId",
                table: "Category",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId");
        }
    }
}
