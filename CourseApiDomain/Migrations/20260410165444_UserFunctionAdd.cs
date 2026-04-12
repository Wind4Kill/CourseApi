using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseApiDomain.Migrations
{
    /// <inheritdoc />
    public partial class UserFunctionAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
    CREATE OR REPLACE FUNCTION public.get_course_rating(p_course_id integer)
    RETURNS double precision
    LANGUAGE plpgsql
    AS $$
    DECLARE
        result double precision;
    BEGIN
        SELECT AVG("ReviewRating")
        INTO result
        FROM "Review"
        WHERE "CourseId" = p_course_id;

        RETURN result;
    END;
    $$;
    """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
