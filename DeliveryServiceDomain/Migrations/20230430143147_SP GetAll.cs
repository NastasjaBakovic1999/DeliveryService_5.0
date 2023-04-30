using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryServiceDomain.Migrations
{
    public partial class SPGetAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var getAllCustomersProcedure =
                @"CREATE PROCEDURE GetAllCustomers
                AS
                BEGIN
                    SELECT * FROM Customer
                END
                ";
            migrationBuilder.Sql(getAllCustomersProcedure);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6e01c2fe-29fe-47fb-bb30-416a6eab2c73", "AQAAAAEAACcQAAAAEHeGa+OWELrJdko6WOhlI1CHFV+DHoo8lZx7KnwCN3fqSGUgPhrO8OEpyZ3p6EJmhQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8e7b9245-5ce6-4f8a-b617-f9893aead4ee", "AQAAAAEAACcQAAAAECjBjMcC/D/7Ht6P0ceuBA/sDwXO2iq4vS2iC599rxZu+FVY54vBow2xXLtLsX/Hkw==" });
        }
    }
}
