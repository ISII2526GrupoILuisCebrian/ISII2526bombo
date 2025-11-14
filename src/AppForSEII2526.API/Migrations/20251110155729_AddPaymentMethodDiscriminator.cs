using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppForSEII2526.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethodDiscriminator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SurnameCustomer",
                table: "PurchaseOrders",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreditCardNumber",
                table: "PaymentMethods",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SurnameCustomer",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<string>(
                name: "CreditCardNumber",
                table: "PaymentMethods",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
