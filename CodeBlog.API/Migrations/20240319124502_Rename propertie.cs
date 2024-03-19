using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeBlog.API.Migrations
{
    /// <inheritdoc />
    public partial class Renamepropertie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileExyension",
                table: "BlogImages",
                newName: "FileExtension",
                schema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
           name: "FileExtension",
           table: "BlogImages",
           newName: "FileExyension",
           schema: "dbo");
        }
    }
}
