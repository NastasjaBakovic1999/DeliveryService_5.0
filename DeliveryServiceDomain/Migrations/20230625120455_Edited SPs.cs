using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryServiceDomain.Migrations
{
    public partial class EditedSPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var editedGetCustomerByIdProcedure =
                 @"ALTER PROCEDURE [dbo].[GetCustomerById]
                    @CustomerId INT
                 AS
                 BEGIN
                    SELECT anu.FirstName,
		                    anu.LastName,
		                    anu.Email,
		                    anu.UserName,
		                    c.*
                    FROM dbo.Customer c
					INNER JOIN AspNetUsers anu ON anu.Id = c.Id
                    WHERE c.Id = @CustomerId
                 END";

            var editedGetAllCustomersProcedure =
                @"ALTER PROCEDURE GetAllCustomers
                AS
                BEGIN
                    SELECT anu.FirstName,
		                    anu.LastName,
		                    anu.Email,
		                    anu.UserName,
		                    c.*
                    FROM dbo.Customer c
					INNER JOIN AspNetUsers anu ON anu.Id = c.Id
                END";

            migrationBuilder.Sql(editedGetAllCustomersProcedure);
            migrationBuilder.Sql(editedGetCustomerByIdProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
