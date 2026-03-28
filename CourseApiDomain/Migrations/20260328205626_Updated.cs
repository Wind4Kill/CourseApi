using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseApiDomain.Migrations
{
    /// <inheritdoc />
    public partial class Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorCourse_Courses_BooksCourseId",
                table: "AuthorCourse");

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ReviewId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 5);

            migrationBuilder.RenameColumn(
                name: "BooksCourseId",
                table: "AuthorCourse",
                newName: "CoursesCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorCourse_BooksCourseId",
                table: "AuthorCourse",
                newName: "IX_AuthorCourse_CoursesCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorCourse_Courses_CoursesCourseId",
                table: "AuthorCourse",
                column: "CoursesCourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorCourse_Courses_CoursesCourseId",
                table: "AuthorCourse");

            migrationBuilder.RenameColumn(
                name: "CoursesCourseId",
                table: "AuthorCourse",
                newName: "BooksCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorCourse_CoursesCourseId",
                table: "AuthorCourse",
                newName: "IX_AuthorCourse_BooksCourseId");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Name" },
                values: new object[] { 4, "John Doe" });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 5, "Backend" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "CourseName", "CourseRating", "IsDeleted", "CourseDetails_CourseDescription", "CourseDetails_CoursePrice" },
                values: new object[] { 5, "AspNet Core", null, false, "AspNetCore essentials", 1000.0m });

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "ReviewId", "CourseId", "ReviewRating", "ReviewText" },
                values: new object[] { 1, 5, 10.0, "Great course!" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorCourse_Courses_BooksCourseId",
                table: "AuthorCourse",
                column: "BooksCourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
