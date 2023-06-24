using DeliveryServiceData;
using DeliveryServiceData.UnitOfWork;
using DeliveryServiceDomain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DeliveryServiceAppTests
{
    public class Mocks
    {
        public static Mock<IRepositoryAdditionalService> GetMockAdditionalServiceRepository()
        {
            var additionalServices = new List<AdditionalService>()
            {
                new AdditionalService
                {
                    AdditionalServiceId = 1,
                    AdditionalServiceName = "Signed delivery note",
                    AdditionalServicePrice = 50
                },
                new AdditionalService
                {
                    AdditionalServiceId = 2,
                    AdditionalServiceName = "Return receipt",
                    AdditionalServicePrice = 50
                },
                new AdditionalService
                {
                    AdditionalServiceId = 3,
                    AdditionalServiceName = "Additional packaging",
                    AdditionalServicePrice = 60
                },
                new AdditionalService
                {
                    AdditionalServiceId = 4,
                    AdditionalServiceName = "Personal delivery",
                    AdditionalServicePrice = 60
                },
                new AdditionalService
                {
                    AdditionalServiceId = 5,
                    AdditionalServiceName = "Value insurance",
                    AdditionalServicePrice = 80
                }
            };

            var mockAdditionalServiceRepository = new Mock<IRepositoryAdditionalService>();
            mockAdditionalServiceRepository.Setup(x => x.GetAll()).Returns(additionalServices);
            mockAdditionalServiceRepository.Setup(x => x.FindByID(It.IsAny<int>(), It.IsAny <int[]>())).Returns((int i, int[] j) => additionalServices.SingleOrDefault(x => x.AdditionalServiceId == i));

            return mockAdditionalServiceRepository;
        }

        public static Mock<IRepositoryAdditionalServiceShipment> GetMockAdditionalServiceShipmentRepository()
        {
            var additionalServiceShipments = new List<AdditionalServiceShipment>()
            {
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 1,
                    ShipmentId = 1,
                    Shipment = GetMockShipmentRepository().Object.FindByID(1),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindByID(1)
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 2,
                    ShipmentId = 1,
                    Shipment = GetMockShipmentRepository().Object.FindByID(1),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindByID(2)
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 3,
                    ShipmentId = 1,
                    Shipment = GetMockShipmentRepository().Object.FindByID(1),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindByID(3)
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 2,
                    ShipmentId = 2,
                    Shipment = GetMockShipmentRepository().Object.FindByID(2),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindByID(2)
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 4,
                    ShipmentId = 2,
                    Shipment = GetMockShipmentRepository().Object.FindByID(2),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindByID(4)
                },
            };

            var mockAdditionalServiceShipmentRepository = new Mock<IRepositoryAdditionalServiceShipment>();
            mockAdditionalServiceShipmentRepository.Setup(x => x.GetAll()).Returns(additionalServiceShipments);
            mockAdditionalServiceShipmentRepository.Setup(x => x.FindByID(It.IsAny<int>(), It.IsAny<int[]>())).Returns((int i, int[] j) => additionalServiceShipments.SingleOrDefault(x => x.AdditionalServiceId == i && x.ShipmentId == j[0]));
            mockAdditionalServiceShipmentRepository.Setup(x => x.Add(It.IsAny<AdditionalServiceShipment>())).Callback((AdditionalServiceShipment adss) =>
            {
                additionalServiceShipments.Add(adss);
            }).Verifiable();

            return mockAdditionalServiceShipmentRepository;
        }

        public static Mock<IRepositoryCustomer> GetMockCustomerRepository()
        {
            var customers = new List<Customer>()
            {
                new Customer
                {
                    Id = 1,
                    FirstName = "Pera",
                    LastName = "Peric",
                    UserName = "pera",
                    Email = "pera@gmail.com",
                    Address = "Adresa 1",
                    PostalCode = "11000",
                    PhoneNumber = "0652233445"
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Mika",
                    LastName = "Mikic",
                    UserName = "mika",
                    Email = "mika@gmail.com",
                    Address = "Adresa 2",
                    PostalCode = "34521",
                    PhoneNumber = "0652233445"
                },
                new Customer
                {
                    Id = 3,
                    FirstName = "Sima",
                    LastName = "Simic",
                    UserName = "sima",
                    Email = "sima@gmail.com",
                    Address = "Adresa 3",
                    PostalCode = "11000",
                    PhoneNumber = "0652233445"
                },
                new Customer
                {
                    Id = 4,
                    FirstName = "Jovana",
                    LastName = "Jovanovic",
                    UserName = "jovana",
                    Email = "jovana@gmail.com",
                    Address = "Adresa 4",
                    PostalCode = "87332",
                    PhoneNumber = "0652233445"
                },
                new Customer
                {
                    Id = 5,
                    FirstName = "Ana",
                    LastName = "Anic",
                    UserName = "ana",
                    Email = "ana@gmail.com",
                    Address = "Adresa 5",
                    PostalCode = "31330",
                    PhoneNumber = "0652233445"
                }
            };

            var mockCustomerRepository = new Mock<IRepositoryCustomer>();
            mockCustomerRepository.Setup(x => x.GetAll()).Returns(customers);
            mockCustomerRepository.Setup(x => x.FindByID(It.IsAny<int>(), It.IsAny<int[]>())).Returns((int i, int[] j) =>
            {
                return customers.SingleOrDefault(c => c.Id == i);
            });
            mockCustomerRepository.Setup(x => x.Edit(It.IsAny<Customer>())).Callback((Customer target) =>
            {
                var original = customers.FirstOrDefault(c => c.Id == target.Id);

                original.FirstName = target.FirstName;
                original.LastName = target.LastName;
                original.UserName = target.UserName;
                original.Email = target.Email;
                original.PhoneNumber = target.PhoneNumber;
                original.Address = target.Address;
                original.PostalCode = target.PostalCode;
            }).Verifiable();

            return mockCustomerRepository;
        }

        public static Mock<IRepositoryShipment> GetMockShipmentRepository()
        {
            Random rand = new Random();
            const string chars = "0123456789QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";

            var shipments = new List<Shipment>()
            {
                new Shipment
                {
                    ShipmentId = 1,
                    ShipmentCode = new string(Enumerable.Repeat(chars, 11).Select(s => s[rand.Next(chars.Length)]).ToArray()),
                    ShipmentWeightId = 1,
                    ShipmentContent = "odeca",
				  Sending = new Address
					{
						City = "Beograd",
						Street = "Mije Kovacevica 7b",
						PostalCode = "11060"
					},
					Receiving = new Address
					{
						City = "Priboj",
						Street = "Ive Lole Ribara 5",
						PostalCode = "31330"
					},
					ContactPersonName = "Luka Bakovic",
                    ContactPersonPhone = "0652244105",
                    CustomerId = 1,
                    Price = 330,
                    Note = "stan 8",
                    ShipmentWeight = GetMockShipmentWeightRepository().Object.FindByID(1),
                    Customer = GetMockCustomerRepository().Object.FindByID(1)
                },
                new Shipment
                {
                    ShipmentId = 2,
                    ShipmentCode = new string(Enumerable.Repeat(chars, 11).Select(s => s[rand.Next(chars.Length)]).ToArray()),
                    ShipmentWeightId = 3,
                    ShipmentContent = "racunar",
				   Sending = new Address
					{
						City = "Beograd",
						Street = "Arsenija Carnojevica 17",
						PostalCode = "11060"
					},
					Receiving = new Address
					{
						City = "Kragujevac",
						Street = "Jablanicka 13",
						PostalCode = "76322"
					},
					ContactPersonName = "Marko Markovic",
                    ContactPersonPhone = "0654433221",
                    CustomerId = 3,
                    Price = 330,
                    Note = "stan 8",
                    ShipmentWeight = GetMockShipmentWeightRepository().Object.FindByID(3),
                    Customer = GetMockCustomerRepository().Object.FindByID(4)
                },
                new Shipment
                {
                    ShipmentId = 3,
                    ShipmentCode = new string(Enumerable.Repeat(chars, 11).Select(s => s[rand.Next(chars.Length)]).ToArray()),
                    ShipmentWeightId = 4,
                    ShipmentContent = "obuca",
					  Sending = new Address
					{
						City = "Smederevo",
						Street = "Despota Stefana 99",
						PostalCode = "98222"
					},
					Receiving = new Address
					{
						City = "Obrenovac",
						Street = "Jurija Gagarina 10",
						PostalCode = "90888"
					},
					ContactPersonName = "Ante Antic",
                    CustomerId = 3,
                    Price = 330,
                    Note = "stan 8",
                    ShipmentWeight = GetMockShipmentWeightRepository().Object.FindByID(4),
                    Customer = GetMockCustomerRepository().Object.FindByID(3)
                }
            };

            var mockShipmentRepository = new Mock<IRepositoryShipment>();
            mockShipmentRepository.Setup(x => x.GetAll()).Returns(shipments);
            mockShipmentRepository.Setup(x => x.FindByID(It.IsAny<int>(), It.IsAny<int[]>())).Returns((int i, int[] j) =>
            {
                return shipments.SingleOrDefault(c => c.ShipmentId == i);
            });
            mockShipmentRepository.Setup(x => x.Add(It.IsAny<Shipment>())).Callback((Shipment shipment) =>
            {
                shipments.Add(shipment);
            });
            mockShipmentRepository.Setup(x => x.FindByCode(It.IsAny<string>())).Returns((string code) =>
            {
                return shipments.SingleOrDefault(c => c.ShipmentCode == code);
            });
            mockShipmentRepository.Setup(x => x.GetAllOfSpecifiedUser(It.IsAny<int>())).Returns((int i) =>
            {
                return shipments.Where(c => c.CustomerId == i).ToList();
            });

            return mockShipmentRepository;
        }

        public static Mock<IRepositoryShipmentWeight> GetMockShipmentWeightRepository()
        {
            var shipmentWeights = new List<ShipmentWeight>()
            {
                new ShipmentWeight
                {
                    ShipmentWeightId = 1,
                    ShipmentWeightDescpription = "up to 0,5kg",
                    ShipmentWeightPrice = 80
                },
                new ShipmentWeight
                {
                    ShipmentWeightId = 2,
                    ShipmentWeightDescpription = "from 0,5 to 2kg",
                    ShipmentWeightPrice = 110
                },
                new ShipmentWeight
                {
                    ShipmentWeightId = 3,
                    ShipmentWeightDescpription = "from 2 to 4kg",
                    ShipmentWeightPrice = 150
                },
                new ShipmentWeight
                {
                    ShipmentWeightId = 4,
                    ShipmentWeightDescpription = "from 4kg to 7kg",
                    ShipmentWeightPrice = 190
                },
                new ShipmentWeight
                {
                    ShipmentWeightId = 5,
                    ShipmentWeightDescpription = "from 7 to 10kg",
                    ShipmentWeightPrice = 240
                },
                new ShipmentWeight
                {
                    ShipmentWeightId = 6,
                    ShipmentWeightDescpription = "over 10kg",
                    ShipmentWeightPrice = 300
                },
            };

            var mockShipmentWeightRepository = new Mock<IRepositoryShipmentWeight>();
            mockShipmentWeightRepository.Setup(x => x.GetAll()).Returns(shipmentWeights);
            mockShipmentWeightRepository.Setup(x => x.FindByID(It.IsAny<int>(), It.IsAny<int[]>())).Returns((int i, int[] j) =>
            {
                return shipmentWeights.SingleOrDefault(c => c.ShipmentWeightId == i);
            });

            return mockShipmentWeightRepository;
        }

        public static Mock<IUnitOfWork> GetMockUnitOfWork()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.AdditionalService).Returns(GetMockAdditionalServiceRepository().Object);
            unitOfWork.Setup(x => x.AdditionalServiceShipment).Returns(GetMockAdditionalServiceShipmentRepository().Object);
            unitOfWork.Setup(x => x.Shipment).Returns(GetMockShipmentRepository().Object);
            unitOfWork.Setup(x => x.ShipmentWeight).Returns(GetMockShipmentWeightRepository().Object);
            unitOfWork.Setup(x => x.Commit()).Verifiable();
            return unitOfWork;
        }

        public static Mock<IPersonUnitOfWork> GetMockPersonUnitOfWork()
        {
            var personUnitOfWork = new Mock<IPersonUnitOfWork>();
            personUnitOfWork.Setup(x => x.Customer).Returns(GetMockCustomerRepository().Object);
            personUnitOfWork.Setup(x => x.Commit()).Verifiable();
            return personUnitOfWork;
        }
    }
}
