using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Loan.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BorrowerId = table.Column<int>(nullable: false),
                    ProviderId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Balance = table.Column<int>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    FailureReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RulesForLoan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<Guid>(nullable: false),
                    RuleId = table.Column<int>(nullable: false),
                    Isvalid = table.Column<bool>(nullable: false),
                    ManagerMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulesForLoan", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "RulesForLoan");
        }
    }
}
