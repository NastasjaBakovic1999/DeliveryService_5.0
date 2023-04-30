﻿using DeliveryServiceApp.Services.Implementation;
using DeliveryServiceData.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace DeliveryServiceDomain.Tests
{
    public class AdditionalServiceShipmentTests
    {
        Mock<IUnitOfWork> unitOfWork = Mocks.GetMockUnitOfWork();

        [Fact]
        public void TestServiceAdditionalServiceShipmentFindById()
        {
            var service = new ServiceAdditionalServiceShipment(unitOfWork.Object);
            var result = service.FindByID(1, new int[] {1});
            var resultAdditionalServiceShipment = Assert.IsType<AdditionalServiceShipment>(result);
            var expected = unitOfWork.Object.AdditionalServiceShipment.FindByID(1, new int[] { 1 });
            Assert.Equal(expected.AdditionalServiceId, resultAdditionalServiceShipment.AdditionalServiceId);
            Assert.Equal(expected.ShipmentId, resultAdditionalServiceShipment.ShipmentId);
        }

        [Fact]
        public void TestServiceAdditionalServiceShipmentFindByIdInvalid()
        {
            var service = new ServiceAdditionalServiceShipment(unitOfWork.Object);
            var result = service.FindByID(-4);

            Assert.Null(result);
        }

        [Fact]
        public void TestServiceAdditionalServiceShipmentGetAll()
        {
            var service = new ServiceAdditionalServiceShipment(unitOfWork.Object);
            var result = service.GetAll();
            var resultList = Assert.IsAssignableFrom<List<AdditionalServiceShipment>>(result);
            var expected = unitOfWork.Object.AdditionalServiceShipment.GetAll();
            Assert.Equal<int>(expected.Count, resultList.Count);
        }

        [Fact]
        public void TestServiceAdditionalServiceShipmentAdd()
        {
            var service = new ServiceAdditionalServiceShipment(unitOfWork.Object);
            var newAdditionalServiceShipment = new AdditionalServiceShipment
            {
                AdditionalServiceId = 1,
                AdditionalService = unitOfWork.Object.AdditionalService.FindByID(1),
                ShipmentId = 2,
                Shipment = unitOfWork.Object.Shipment.FindByID(2)
            };
            service.Add(newAdditionalServiceShipment);
            var additionalServiceShipment = service.FindByID(1, new int[] { 2 });
            Assert.Equal(newAdditionalServiceShipment.AdditionalServiceId, additionalServiceShipment.AdditionalServiceId);
            Assert.Equal(newAdditionalServiceShipment.ShipmentId, additionalServiceShipment.ShipmentId);
            unitOfWork.Verify(x => x.AdditionalServiceShipment.Add(It.Is<AdditionalServiceShipment>(p => p.AdditionalServiceId == 1 && p.ShipmentId == 2)), Times.Once);
            unitOfWork.Verify(x => x.Commit(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(AdditionalServiceShipmentData))]
        public void TestServiceAdditionalServiceShipmentAddInvalidId(AdditionalServiceShipment newAdditionalServiceShipment)
        {
            var service = new ServiceAdditionalServiceShipment(unitOfWork.Object);

            Assert.Throws<ArgumentOutOfRangeException>(() => service.Add(newAdditionalServiceShipment));
            unitOfWork.Verify(x => x.AdditionalServiceShipment.Add(It.IsAny<AdditionalServiceShipment>()), Times.Never);
            unitOfWork.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public void TestServiceAdditionalServiceShipmentAddAlreadyExist()
        {
            var service = new ServiceAdditionalServiceShipment(unitOfWork.Object);
            var newAdditionalServiceShipment = new AdditionalServiceShipment
            {
                AdditionalServiceId = 1,
                AdditionalService = unitOfWork.Object.AdditionalService.FindByID(1),
                ShipmentId = 1,
                Shipment = unitOfWork.Object.Shipment.FindByID(1)
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => service.Add(newAdditionalServiceShipment));
            unitOfWork.Verify(x => x.AdditionalServiceShipment.Add(It.IsAny<AdditionalServiceShipment>()), Times.Never);
            unitOfWork.Verify(x => x.Commit(), Times.Never);
        }

        public static IEnumerable<object[]> AdditionalServiceShipmentData()
        {
            yield return new object[] {  new AdditionalServiceShipment
            {
                AdditionalServiceId = -1,
                ShipmentId = 3
            }};
            yield return new object[] {  new AdditionalServiceShipment
            {
                AdditionalServiceId = 1,
                ShipmentId = -3
            }};
            yield return new object[] {  new AdditionalServiceShipment
            {
                AdditionalServiceId = -2,
                ShipmentId = -4
            }};
            yield return new object[] {  new AdditionalServiceShipment
            {
                AdditionalServiceId = 0,
                ShipmentId = 4
            }};
            yield return new object[] {  new AdditionalServiceShipment
            {
                AdditionalServiceId = 2,
                ShipmentId = 0
            }};
            yield return new object[] {  new AdditionalServiceShipment
            {
                AdditionalServiceId = -2,
                ShipmentId = 0
            }};
            yield return new object[] {  new AdditionalServiceShipment
            {
                AdditionalServiceId = 0,
                ShipmentId = -4
            }};
            yield return new object[] {  new AdditionalServiceShipment
            {
                AdditionalServiceId = 0,
                ShipmentId = 0
            }};
            yield return new object[] { null };
        }
    }
}
