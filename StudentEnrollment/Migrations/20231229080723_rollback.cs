using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentEnrollment.Migrations
{
    /// <inheritdoc />
    public partial class rollback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentModels_AspNetUsers_StudentId",
                table: "EnrollmentModels");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentModels_CourseModels_CourseId",
                table: "EnrollmentModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnrollmentModels",
                table: "EnrollmentModels");

            migrationBuilder.DropIndex(
                name: "IX_EnrollmentModels_StudentId",
                table: "EnrollmentModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseModels",
                table: "CourseModels");

            migrationBuilder.RenameTable(
                name: "EnrollmentModels",
                newName: "EnrollmentModel");

            migrationBuilder.RenameTable(
                name: "CourseModels",
                newName: "CourseModel");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EnrollmentModel",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "EnrollmentModel",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CourseModelId",
                table: "EnrollmentModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserModelId",
                table: "EnrollmentModel",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnrollmentModel",
                table: "EnrollmentModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseModel",
                table: "CourseModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentModel_CourseModelId",
                table: "EnrollmentModel",
                column: "CourseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentModel_UserModelId",
                table: "EnrollmentModel",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentModel_AspNetUsers_UserModelId",
                table: "EnrollmentModel",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentModel_CourseModel_CourseModelId",
                table: "EnrollmentModel",
                column: "CourseModelId",
                principalTable: "CourseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentModel_AspNetUsers_UserModelId",
                table: "EnrollmentModel");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentModel_CourseModel_CourseModelId",
                table: "EnrollmentModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnrollmentModel",
                table: "EnrollmentModel");

            migrationBuilder.DropIndex(
                name: "IX_EnrollmentModel_CourseModelId",
                table: "EnrollmentModel");

            migrationBuilder.DropIndex(
                name: "IX_EnrollmentModel_UserModelId",
                table: "EnrollmentModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseModel",
                table: "CourseModel");

            migrationBuilder.DropColumn(
                name: "CourseModelId",
                table: "EnrollmentModel");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "EnrollmentModel");

            migrationBuilder.RenameTable(
                name: "EnrollmentModel",
                newName: "EnrollmentModels");

            migrationBuilder.RenameTable(
                name: "CourseModel",
                newName: "CourseModels");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "EnrollmentModels",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EnrollmentModels",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnrollmentModels",
                table: "EnrollmentModels",
                columns: new[] { "CourseId", "StudentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseModels",
                table: "CourseModels",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentModels_StudentId",
                table: "EnrollmentModels",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentModels_AspNetUsers_StudentId",
                table: "EnrollmentModels",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentModels_CourseModels_CourseId",
                table: "EnrollmentModels",
                column: "CourseId",
                principalTable: "CourseModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
