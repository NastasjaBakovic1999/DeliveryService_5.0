using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryServiceDomain.Migrations.DeliveryService
{
    public partial class SPGetAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var getAllAdditionalServicesProcedure = 
                @"CREATE PROCEDURE GetAllAdditionalServices
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT AdditionalServiceId, AdditionalServiceName, AdditionalServicePrice
                    FROM AdditionalService;
                END";
            var getAllAdditionalServicesShipmentProcedure =
				@"CREATE PROCEDURE GetAllAdditionalServiceShipments
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT * FROM 
                    AdditionalServiceShipments
                END
                GO";
            var getAllShipmentWeightsProcedure =
				@"CREATE PROCEDURE GetAllShipmentWeights
                AS
                BEGIN
                    SELECT * FROM ShipmentWeight
                END
                ";
            migrationBuilder.Sql(getAllAdditionalServicesProcedure);
            migrationBuilder.Sql(getAllAdditionalServicesShipmentProcedure);
            migrationBuilder.Sql(getAllShipmentWeightsProcedure);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
