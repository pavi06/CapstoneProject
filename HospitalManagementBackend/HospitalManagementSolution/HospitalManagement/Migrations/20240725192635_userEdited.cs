using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    public partial class userEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Female",
                table: "UserDetails",
                newName: "Gender");

            migrationBuilder.AddColumn<bool>(
                name: "IsAllotted",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "InPatients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActivePatient",
                table: "InPatientDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "PatientType",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllotted",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "InPatients");

            migrationBuilder.DropColumn(
                name: "IsActivePatient",
                table: "InPatientDetails");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "UserDetails",
                newName: "Female");

            migrationBuilder.AlterColumn<int>(
                name: "PatientType",
                table: "Bills",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
