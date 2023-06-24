using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryServiceDomain.Migrations
{
    public partial class SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			var editCustomerProcedure =
				@"CREATE PROCEDURE [dbo].[EditCustomer]
                AS
                BEGIN
                    UPDATE Customer
                    SET
                        Address = @Address,
                        PostalCode = @PostalCode
                    WHERE Id = @CustomerId
                END";

			var getCustomerByIdProcedure =
				@"CREATE PROCEDURE [dbo].[GetCustomerById]
                AS
                BEGIN
                    SELECT *
                    FROM dbo.Customer
                    WHERE Id = @CustomerId
                END";

            migrationBuilder.Sql(editCustomerProcedure);
            migrationBuilder.Sql(getCustomerByIdProcedure);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "02465f68-25fc-4e76-86a3-37c8cbb0147c", "AQAAAAEAACcQAAAAEFHurLMIUKJPqfLyhat6CawnCO6eRqf9+bJ1f43FlFntcVr3g0y7WtKaMhB/zGccmw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ca45297f-442e-4310-b83f-77af1730bab8", "AQAAAAEAACcQAAAAECA76mpb67LInis4H6wX9FxYMFUBVDvtQeJm4GO4JroGA5IEctnU4UmiQ1PVSgfdFg==" });
        }
    }
}
