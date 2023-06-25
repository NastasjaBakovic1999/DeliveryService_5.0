using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryServiceDomain.Migrations.DeliveryService
{
    public partial class SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusShipments");

            migrationBuilder.DropTable(
                name: "Statuses");

            var deleteShipmentProcedure =
                 @"CREATE PROCEDURE [dbo].[DeleteShipment]
                    @Id INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DELETE FROM dbo.Shipments WHERE ShipmentId = @Id;
                END";
            var getAdditionalServiceByIdProcedure =
                @"CREATE PROCEDURE [dbo].[GetAdditionalServiceById]
                    @AdditionalServiceId INT
                AS
                BEGIN
                    SELECT AdditionalServiceId, AdditionalServiceName, AdditionalServicePrice
                    FROM dbo.AdditionalService
                    WHERE AdditionalServiceId = @AdditionalServiceId
                END";
            var getAdditionalServiceShipmentByIDsProcedure =
                @"CREATE PROCEDURE [dbo].[GetAdditionalServiceShipmentByIDs]
                    @ShipmentId INT,
                    @AdditionalServiceId INT
                AS
                BEGIN
                    SELECT *
                    FROM AdditionalServiceShipments
                    WHERE ShipmentId = @ShipmentId AND AdditionalServiceId = @AdditionalServiceId
                END";
            var getShipmentByIdProcedure =
                @"CREATE PROCEDURE [dbo].[GetShipmentById]
                    @ShipmentId INT
                AS
                BEGIN
                    SELECT *
                    FROM Shipments
                    WHERE ShipmentId = @ShipmentId
                END";
            var getShipmentByShipmentCode =
                @"CREATE PROCEDURE [dbo].[GetShipmentByShipmentCode]
                    @ShipmentCode NVARCHAR(50)
                AS
                BEGIN
                    SELECT *
                    FROM Shipments
                    WHERE ShipmentCode = @ShipmentCode
                END";
            var getShipmentByCustomerIdProcedure =
                @"CREATE PROCEDURE [dbo].[GetShipmentsByCustomerId]
                    @CustomerId INT
                AS
                BEGIN
                    SELECT *
                    FROM Shipments
                    WHERE CustomerId = @CustomerId
                END";
            var getShipmentWeightByIdProcedure =
                @"CREATE PROCEDURE [dbo].[GetShipmentWeightById]
                    @ShipmentWeightId INT
                AS
                BEGIN
                    SELECT *
                    FROM ShipmentWeight
                    WHERE ShipmentWeightId = @ShipmentWeightId
                END";
            var insertShipmentProcedure =
                @"CREATE PROCEDURE [dbo].[InsertShipment]
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
	                @ShipmentId INT
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

                    SELECT SCOPE_IDENTITY() AS 'ShipmentId';
                END";

            migrationBuilder.Sql(insertShipmentProcedure);
            migrationBuilder.Sql(deleteShipmentProcedure);
            migrationBuilder.Sql(getShipmentByIdProcedure);
            migrationBuilder.Sql(getShipmentWeightByIdProcedure);
            migrationBuilder.Sql(getShipmentByCustomerIdProcedure);
            migrationBuilder.Sql(getShipmentByShipmentCode);
            migrationBuilder.Sql(getAdditionalServiceShipmentByIDsProcedure);
            migrationBuilder.Sql(getAdditionalServiceByIdProcedure);
        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "StatusShipments",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    StatusTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusShipments", x => new { x.StatusId, x.ShipmentId });
                    table.ForeignKey(
                        name: "FK_StatusShipments_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "ShipmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusShipments_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Scheduled" },
                    { 2, "On the packaging" },
                    { 3, "Stored for shipping" },
                    { 4, "At the courier" },
                    { 5, "In transport" },
                    { 6, "Delivered" },
                    { 7, "Stored on hold" },
                    { 8, "Rejected" },
                    { 9, "Returned to sender" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusShipments_ShipmentId",
                table: "StatusShipments",
                column: "ShipmentId");
        }
    }
}
