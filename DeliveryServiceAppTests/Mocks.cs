using AutoMapper;
using DataTransferObjects;
using DeliveryServiceData;
using DeliveryServiceData.UnitOfWork;
using DeliveryServiceDomain;
using DeliveryServiceServices.Profiles;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            mockAdditionalServiceRepository.Setup(x => x.FindOneByExpression(It.IsAny<Expression<Func<AdditionalService, bool>>>())).Returns((Expression<Func<AdditionalService, bool>> expression) => additionalServices.SingleOrDefault(expression.Compile()));

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
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x=>x.ShipmentId == 1),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindOneByExpression(x => x.AdditionalServiceId == 1)
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 2,
                    ShipmentId = 1,
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x=>x.ShipmentId == 1),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindOneByExpression(x => x.AdditionalServiceId == 2)
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 3,
                    ShipmentId = 1,
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x=>x.ShipmentId == 1),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindOneByExpression(x => x.AdditionalServiceId == 3)
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 2,
                    ShipmentId = 2,
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x=>x.ShipmentId == 2),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindOneByExpression(x => x.AdditionalServiceId == 2)
                },
                new AdditionalServiceShipment
                {
                    AdditionalServiceId = 4,
                    ShipmentId = 2,
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x=>x.ShipmentId == 2),
                    AdditionalService = GetMockAdditionalServiceRepository().Object.FindOneByExpression(x => x.AdditionalServiceId == 4)
                },
            };

            var mockAdditionalServiceShipmentRepository = new Mock<IRepositoryAdditionalServiceShipment>();
            mockAdditionalServiceShipmentRepository.Setup(x => x.GetAll()).Returns(additionalServiceShipments);
            mockAdditionalServiceShipmentRepository.Setup(x => x.FindOneByExpression(It.IsAny<Expression<Func<AdditionalServiceShipment, bool>>>())).Returns((Expression<Func<AdditionalServiceShipment, bool>> expression) => additionalServiceShipments.SingleOrDefault(expression.Compile()));
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
            mockCustomerRepository.Setup(x => x.FindOneByExpression(It.IsAny<Expression<Func<Customer, bool>>>())).Returns((Expression<Func<Customer, bool>> expression) => { return customers.SingleOrDefault(expression.Compile()); });
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

        public static Mock<IRepositoryPerson> GetMockPersonRepository()
        {
            var people = new List<Person>()
            {
                new Person
                {
                    Id = 1,
                    FirstName = "Pera",
                    LastName = "Peric",
                    UserName = "pera",
                    Email = "pera@gmail.com",
                    PhoneNumber = "0652233445"
                },
                new Person
                {
                    Id = 2,
                    FirstName = "Mika",
                    LastName = "Mikic",
                    UserName = "mika",
                    Email = "mika@gmail.com",
                    PhoneNumber = "0652233445"
                },
                new Person
                {
                    Id = 3,
                    FirstName = "Sima",
                    LastName = "Simic",
                    UserName = "sima",
                    Email = "sima@gmail.com",
                    PhoneNumber = "0652233445"
                },
                new Person
                {
                    Id = 4,
                    FirstName = "Jovana",
                    LastName = "Jovanovic",
                    UserName = "jovana",
                    Email = "jovana@gmail.com",
                    PhoneNumber = "0652233445"
                },
                new Person
                {
                    Id = 5,
                    FirstName = "Ana",
                    LastName = "Anic",
                    UserName = "ana",
                    Email = "ana@gmail.com",
                    PhoneNumber = "0652233445"
                }
            };

            var mockPersonRepository = new Mock<IRepositoryPerson>();
            mockPersonRepository.Setup(x => x.GetAll()).Returns(people);
            mockPersonRepository.Setup(x => x.FindOneByExpression(It.IsAny<Expression<Func<Person, bool>>>())).Returns((Expression<Func<Person, bool>> expression) =>
            {
                return people.SingleOrDefault(expression.Compile());
            });

            return mockPersonRepository;
        }

        public static Mock<IRepositoryShipment> GetMockShipmentRepository()
        {
            Random rand = new();
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
                    ShipmentWeight = GetMockShipmentWeightRepository().Object.FindOneByExpression(x => x.ShipmentWeightId == 1),
                    Customer = GetMockCustomerRepository().Object.FindOneByExpression(x => x.Id == 1)
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
                    ShipmentWeight = GetMockShipmentWeightRepository().Object.FindOneByExpression(x => x.ShipmentWeightId == 3),
                    Customer = GetMockCustomerRepository().Object.FindOneByExpression(x => x.Id == 4)
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
                    ShipmentWeight = GetMockShipmentWeightRepository().Object.FindOneByExpression(x => x.ShipmentWeightId == 4),
                    Customer = GetMockCustomerRepository().Object.FindOneByExpression(x => x.Id == 3)
                }
            };

            var mockShipmentRepository = new Mock<IRepositoryShipment>();
            mockShipmentRepository.Setup(x => x.GetAll()).Returns(shipments);
            mockShipmentRepository.Setup(x => x.FindOneByExpression(It.IsAny<Expression<Func<Shipment, bool>>>())).Returns((Expression<Func<Shipment, bool>> expression) =>
            {
                return shipments.SingleOrDefault(expression.Compile());
            });
            mockShipmentRepository.Setup(x => x.Add(It.IsAny<Shipment>())).Callback((Shipment shipment) =>
            {
                shipments.Add(shipment);
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
            mockShipmentWeightRepository.Setup(x => x.FindOneByExpression(It.IsAny<Expression<Func<ShipmentWeight, bool>>>())).Returns((Expression<Func<ShipmentWeight, bool>> expression) =>
            {
                return shipmentWeights.SingleOrDefault(expression.Compile());
            });

            return mockShipmentWeightRepository;
        }

        public static Mock<IRepositoryStatus> GetMockStatusReposiotry()
        {
            var statuses = new List<Status>()
            {
                new Status
                {
                    StatusId = 1,
                    StatusName = "Scheduled"
                },
                new Status
                {
                    StatusId = 2,
                    StatusName = "On the packaging"
                },
                new Status
                {
                    StatusId = 3,
                    StatusName = "Stored for shipping"
                },
                new Status
                {
                    StatusId = 4,
                    StatusName = "At the courier"
                },
                new Status
                {
                    StatusId = 5,
                    StatusName = "In transport"
                },
                new Status
                {
                    StatusId = 6,
                    StatusName = "Delivered"
                }
            };

            var mockStatusRepository = new Mock<IRepositoryStatus>();
            mockStatusRepository.Setup(x => x.GetAll()).Returns(statuses);
            mockStatusRepository.Setup(x => x.FindOneByExpression(It.IsAny<Expression<Func<Status, bool>>>())).Returns((Expression<Func<Status, bool>> expression) =>
            {
                return statuses.SingleOrDefault(expression.Compile());
            });

            return mockStatusRepository;
        }

        public static Mock<IRepositoryStatusShipment> GetMockStatusShipmentRepository()
        {
            var statusShipments = new List<StatusShipment>()
            {
                new StatusShipment
                {
                    StatusId = 1,
                    Status = GetMockStatusReposiotry().Object.FindOneByExpression(x => x.StatusId == 1),
                    ShipmentId = 1,
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x => x.ShipmentId == 1),
                },
                new StatusShipment
                {
                    StatusId = 2,
                    Status = GetMockStatusReposiotry().Object.FindOneByExpression(x => x.StatusId == 2),
                    ShipmentId = 1,
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x => x.ShipmentId == 1),
                },
                new StatusShipment
                {
                    StatusId = 1,
                    Status = GetMockStatusReposiotry().Object.FindOneByExpression(x => x.StatusId == 1),
                    ShipmentId = 2,
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x => x.ShipmentId == 2),
                },
                new StatusShipment
                {
                    StatusId = 3,
                    Status = GetMockStatusReposiotry().Object.FindOneByExpression(x => x.StatusId == 3),
                    ShipmentId = 2,
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x => x.ShipmentId == 2),
                },
                new StatusShipment
                {
                    StatusId = 4,
                    Status = GetMockStatusReposiotry().Object.FindOneByExpression(x => x.StatusId == 4),
                    ShipmentId = 2,
                    Shipment = GetMockShipmentRepository().Object.FindOneByExpression(x => x.ShipmentId == 2)
                },

            };

            var mockStatusShipmentRepository = new Mock<IRepositoryStatusShipment>();
            mockStatusShipmentRepository.Setup(x => x.GetAll()).Returns(statusShipments);
            mockStatusShipmentRepository.Setup(x => x.FindOneByExpression(It.IsAny<Expression<Func<StatusShipment, bool>>>())).Returns((Expression<Func<StatusShipment, bool>> expression) =>
            {
                return statusShipments.SingleOrDefault(expression.Compile());
            });
            mockStatusShipmentRepository.Setup(x => x.GetAllByShipmentId(It.IsAny<int>())).Returns((int i) =>
            {
                return statusShipments.Where(s => s.ShipmentId == i).ToList();
            });
            mockStatusShipmentRepository.Setup(x => x.Add(It.IsAny<StatusShipment>())).Callback((StatusShipment ss) =>
            {
                statusShipments.Add(ss);
            }).Verifiable();

            return mockStatusShipmentRepository;
        }

        public static Mock<IUnitOfWork> GetMockUnitOfWork()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.AdditionalService).Returns(GetMockAdditionalServiceRepository().Object);
            unitOfWork.Setup(x => x.AdditionalServiceShipment).Returns(GetMockAdditionalServiceShipmentRepository().Object);
            unitOfWork.Setup(x => x.Shipment).Returns(GetMockShipmentRepository().Object);
            unitOfWork.Setup(x => x.ShipmentWeight).Returns(GetMockShipmentWeightRepository().Object);
            unitOfWork.Setup(x => x.Status).Returns(GetMockStatusReposiotry().Object);
            unitOfWork.Setup(x => x.StatusShipment).Returns(GetMockStatusShipmentRepository().Object);
            unitOfWork.Setup(x => x.Commit()).Verifiable();
            return unitOfWork;
        }

        public static Mock<IPersonUnitOfWork> GetMockPersonUnitOfWork()
        {
            var personUnitOfWork = new Mock<IPersonUnitOfWork>();
            personUnitOfWork.Setup(x => x.Customer).Returns(GetMockCustomerRepository().Object);
            personUnitOfWork.Setup(x => x.Person).Returns(GetMockPersonRepository().Object);
            personUnitOfWork.Setup(x => x.Commit()).Verifiable();
            return personUnitOfWork;
        }

        public static IMapper GetMockAutoMapper()
        {
            var profiles = new List<Profile>() { 
                new AdditionalServiceProfile(), 
                new AdditionalServiceShipmentProfile(),
                new AddressProfile(),
                new CustomerProfile(),
                new PersonProfile(),
                new ShipmentProfile(),
                new ShipmentWeightProfile(),
                new StatusProfile(),
                new StatusShipmentProfile()
            };
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            IMapper mapper = new Mapper(configuration); 
            return mapper;
        }

    }
}
