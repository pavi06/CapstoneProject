using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    public partial class paymentEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Bills_BillId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_BillId",
                table: "Payments",
                newName: "IX_Payments_BillId");

            migrationBuilder.AddColumn<double>(
                name: "AmountPaid",
                table: "Payments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Bills_BillId",
                table: "Payments",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "BillId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Bills_BillId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_BillId",
                table: "Payment",
                newName: "IX_Payment_BillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Bills_BillId",
                table: "Payment",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "BillId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
