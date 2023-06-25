using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryServiceDomain.Migrations.DeliveryService
{
    public partial class EditedSPsagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var getAllShipmentsProcedure =
               @"CREATE PROCEDURE GetAllShipments
               AS
               BEGIN
                    SELECT s.*,
		                    CONCAT(anu.FirstName, ' ', anu.LastName) AS CustomerName
                    FROM Shipments s
					INNER JOIN AspNetUsers anu ON anu.Id = s.CustomerId
               END";

            var editedInsertShipmentProcedure =
                @"ALTER PROCEDURE [dbo].[InsertShipment]
                    @ShipmentCode VARCHAR(50),
                    @ShipmentWeightId INT,
                    @ShipmentContent NVARCHAR(500),
                    @ContactPersonName NVARCHAR(100),
                    @ContactPersonPhone VARCHAR(20),
                    @CustomerId INT,
                    @Price DECIMAL(18,2),
                    @Note NVARCHAR(500),
                    @Sending_City NVARCHAR(100),
                    @Sending_Street NVARCHAR(100),
                    @Sending_PostalCode VARCHAR(20),
                    @Receiving_City NVARCHAR(100),
                    @Receiving_Street NVARCHAR(100),
                    @Receiving_PostalCode VARCHAR(20),
	                @ShipmentId INT,
					@AdditionalServiceIds NVARCHAR(MAX)
                AS
                BEGIN
                    SET NOCOUNT ON;

                    INSERT INTO dbo.Shipments
                    (
                        ShipmentCode,
                        ShipmentWeightId,
                        ShipmentContent,
                        ContactPersonName,
                        ContactPersonPhone,
                        CustomerId,
                        Price,
                        Note,
                        Sending_City,
                        Sending_Street,
                        Sending_PostalCode,
                        Receiving_City,
                        Receiving_Street,
                        Receiving_PostalCode
                    )
                    VALUES
                    (
                        @ShipmentCode,
                        @ShipmentWeightId,
                        @ShipmentContent,
                        @ContactPersonName,
                        @ContactPersonPhone,
                        @CustomerId,
                        @Price,
                        @Note,
                        @Sending_City,
                        @Sending_Street,
                        @Sending_PostalCode,
                        @Receiving_City,
                        @Receiving_Street,
                        @Receiving_PostalCode
                    );

                    SET @ShipmentId = SCOPE_IDENTITY();

					INSERT INTO dbo.AdditionalServiceShipments
					(
						AdditionalServiceId,
						ShipmentId
					)
					SELECT CONVERT(INT, value), @ShipmentId
					FROM OPENJSON(@AdditionalServiceIds);
                END";

            migrationBuilder.Sql(getAllShipmentsProcedure);
            migrationBuilder.Sql(editedInsertShipmentProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
