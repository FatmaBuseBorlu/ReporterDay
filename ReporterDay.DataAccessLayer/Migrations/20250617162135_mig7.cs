using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReporterDay.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropIndex(
                name: "IX_Articles_AppUserId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Articles",
                newName: "CreateDate");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Articles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AppUserId1",
                table: "Articles",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_AppUserId1",
                table: "Articles",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
      

            migrationBuilder.DropIndex(
                name: "IX_Articles_AppUserId1",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Articles",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AppUserId",
                table: "Articles",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_AppUserId",
                table: "Articles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
