using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryServiceDomain.Migrations.DeliveryService
{
    public partial class EditedSPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var editedGetAdditionalServiceShipmentByIDsProcedure =
              @"ALTER PROCEDURE [dbo].[GetAdditionalServiceShipmentByIDs]
                    @ShipmentId INT,
                    @AdditionalServiceId INT
              AS
              BEGIN
                    SELECT ass.*,
		                    ads.AdditionalServiceName,
		                    s.ShipmentCode
                    FROM AdditionalServiceShipments ass
					INNER JOIN AdditionalService ads ON ads.AdditionalServiceId = ass.AdditionalServiceId
					INNER JOIN Shipments s ON s.ShipmentId = ass.ShipmentId
                    WHERE ass.ShipmentId = @ShipmentId AND ass.AdditionalServiceId = @AdditionalServiceId
              END";

            var editedGetShipmentByIdProcedure =
              @"ALTER PROCEDURE [dbo].[GetShipmentById]
                    @ShipmentId INT
              AS
              BEGIN
                    SELECT s.*,
		                    CONCAT(anu.FirstName, ' ', anu.LastName) AS CustomerName
                    FROM Shipments s
					INNER JOIN AspNetUsers anu ON anu.Id = s.CustomerId
                    WHERE s.ShipmentId = @ShipmentId
              END";

            var editedGetShipmentByShipmentCode =
               @"ALTER PROCEDURE [dbo].[GetShipmentByShipmentCode]
                    @ShipmentCode NVARCHAR(50)
               AS
               BEGIN
                    SELECT s.*,
		                    CONCAT(anu.FirstName, ' ', anu.LastName) AS CustomerName
                    FROM Shipments s
					INNER JOIN AspNetUsers anu ON anu.Id = s.CustomerId
                    WHERE s.ShipmentCode = @ShipmentCode
               END";

            var editedGetShipmentByCustomerIdProcedure =
                 @"ALTER PROCEDURE [dbo].[GetShipmentsByCustomerId]
                        @CustomerId INT
                 AS
                 BEGIN
                        SELECT s.*,
		                        CONCAT(anu.FirstName, ' ', anu.LastName) AS CustomerName
                        FROM Shipments s
					    INNER JOIN AspNetUsers anu ON anu.Id = s.CustomerId
                        WHERE s.CustomerId = @CustomerId
                 END";

            var editedGetAllAdditionalServicesShipmentProcedure =
              @"ALTER PROCEDURE GetAllAdditionalServiceShipments
                AS
                BEGIN
                    SET NOCOUNT ON;
                     SELECT ass.*,
		                    ads.AdditionalServiceName,
		                    s.ShipmentCode
                    FROM AdditionalServiceShipments ass
					INNER JOIN AdditionalService ads ON ads.AdditionalServiceId = ass.AdditionalServiceId
					INNER JOIN Shipments s ON s.ShipmentId = ass.ShipmentId
                END
                GO";

            migrationBuilder.Sql(editedGetShipmentByCustomerIdProcedure);
            migrationBuilder.Sql(editedGetShipmentByShipmentCode);
            migrationBuilder.Sql(editedGetShipmentByIdProcedure);
            migrationBuilder.Sql(editedGetAdditionalServiceShipmentByIDsProcedure);
            migrationBuilder.Sql(editedGetAllAdditionalServicesShipmentProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
