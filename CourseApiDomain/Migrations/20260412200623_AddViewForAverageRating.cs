using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseApiDomain.Migrations
{
    /// <inheritdoc />
    public partial class AddViewForAverageRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            CREATE VIEW "CoursesWithRating" AS
            SELECT c.*, public.get_course_rating(c."CourseId") as "AvgRating"
            FROM "Courses" c;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            """
            DROP VIEW "CoursesWithRating"
            """
            );
        }
    }
}
