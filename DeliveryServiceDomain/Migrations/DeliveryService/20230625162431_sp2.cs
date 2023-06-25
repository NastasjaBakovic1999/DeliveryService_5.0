using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryServiceDomain.Migrations.DeliveryService
{
    public partial class sp2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var insertAdditionalServiceShipmentProcedure = @"
                CREATE PROCEDURE [dbo].[InsertAdditionalServiceShipment]
	                @ShipmentId INT,
					@AdditionalServiceId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

					INSERT INTO dbo.AdditionalServiceShipments
					(
						AdditionalServiceId,
						ShipmentId
					)
                    VALUES
                    (
                        @AdditionalServiceId,
					    @ShipmentId 
                    )
                    END";

            migrationBuilder.Sql(insertAdditionalServiceShipmentProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
