using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codexam.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig1123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPages_Exams_ExamId",
                table: "TeacherPages");

            migrationBuilder.DropIndex(
                name: "IX_TeacherPages_ExamId",
                table: "TeacherPages");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "TeacherPages");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "TeacherPages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPages_ExamId",
                table: "TeacherPages",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPages_Exams_ExamId",
                table: "TeacherPages",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
