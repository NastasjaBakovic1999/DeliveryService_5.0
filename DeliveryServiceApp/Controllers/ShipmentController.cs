using AutoMapper;
using DeliveryServiceApp.Models;
using DeliveryServiceApp.Services.Interfaces;
using DeliveryServiceData.UnitOfWork;
using DeliveryServiceData.UnitOfWork.Implementation;
using DeliveryServiceDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace DeliveryServiceApp.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly UserManager<Person> userManager;
        private readonly IServiceAdditonalService serviceAdditonalService;
        private readonly IServiceShipmentWeight serviceShipmentWeight;
        private readonly IServiceShipment serviceShipment;
        private readonly IServiceAddionalServiceShipment serviceAddionalServiceShipment;

        public ShipmentController(UserManager<Person> userManager, IServiceAdditonalService serviceAdditonalService, IServiceShipmentWeight serviceShipmentWeight,
                                  IServiceShipment serviceShipment, IServiceAddionalServiceShipment serviceAddionalServiceShipment)
        {
            this.userManager = userManager;
            this.serviceAdditonalService = serviceAdditonalService;
            this.serviceShipmentWeight = serviceShipmentWeight;
            this.serviceShipment = serviceShipment;
            this.serviceAddionalServiceShipment = serviceAddionalServiceShipment;
        }

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            try
            {
                List<AdditionalService> additionalServicesList = serviceAdditonalService.GetAll();
                List<SelectListItem> selectAdditionalServicesList = additionalServicesList.Select(s => new SelectListItem { Text = s.AdditionalServiceName + " - " + s.AdditionalServicePrice + " RSD", Value = s.AdditionalServiceId.ToString() }).ToList();

                List<ShipmentWeight> shipmentWeightList = serviceShipmentWeight.GetAll();
                List<SelectListItem> selectShipmentWeightList = shipmentWeightList.Select(s => new SelectListItem { Text = s.ShipmentWeightDescription, Value = s.ShipmentWeightId.ToString() }).ToList();

                CreateShipmentViewModel model = new CreateShipmentViewModel
                {
                    AdditionalServices = selectAdditionalServicesList,
                    ShipmentWeights = selectShipmentWeightList
                };

                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Create(CreateShipmentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    List<AdditionalService> additionalServicesList = serviceAdditonalService.GetAll();
                    List<SelectListItem> selectAdditionalServicesList = additionalServicesList.Select(s => new SelectListItem { Text = s.AdditionalServiceName + " - " + s.AdditionalServicePrice + " RSD", Value = s.AdditionalServiceId.ToString() }).ToList();

                    List<ShipmentWeight> shipmentWeightList = serviceShipmentWeight.GetAll();
                    List<SelectListItem> selectShipmentWeightList = shipmentWeightList.Select(s => new SelectListItem { Text = s.ShipmentWeightDescription, Value = s.ShipmentWeightId.ToString() }).ToList();

                    model.AdditionalServices = selectAdditionalServicesList;
                    model.ShipmentWeights = selectShipmentWeightList;

                    return View(model);
                }

                Shipment shipment = new Shipment
                {
                    ShipmentWeightId = model.ShipmentWeightId,
                    ShipmentContent = model.ShipmentContent,
                    ContactPersonName = model.ContactPersonName,
                    ContactPersonPhone = model.ContactPersonPhone,
                    Note = model.Note,
                    Sending  = new Address
                    {
                        City = model.SendingCity,
                        Street = model.SendingAddress,
                        PostalCode = model.SendingPostalCode
                    },
                    Receiving = new Address
                    {
                        City = model.ReceivingCity,
                        Street = model.ReceivingAddress,
                        PostalCode = model.ReceivingPostalCode
                    }
                };

                shipment.ShipmentCode = GetShipmentCode();


                int userId = -1;
                int.TryParse(userManager.GetUserId(HttpContext.User), out userId);

                if (userId != -1)
                {
                    shipment.CustomerId = userId;
                }

                double weightPrice = serviceShipmentWeight.FindByID(model.ShipmentWeightId).ShipmentWeightPrice;
                double additionalServicesPrice = 0;

                if (model.Services != null && model.Services.Count() > 0)
                {
                    List<AdditionalService> additionalServices = serviceAdditonalService.GetAll();

                    foreach (AdditonalServiceViewModel sa in model.Services)
                    {
                        var additionalService = additionalServices.Find(s => s.AdditionalServiceId == sa.AdditionalServiceId);
                        additionalServicesPrice += additionalService.AdditionalServicePrice;
                        shipment.AdditionalServices.Add(new AdditionalServiceShipment 
                        { 
                                  AdditionalServiceId = additionalService.AdditionalServiceId, 
                                  AdditionalService = additionalService
                        });
                    }
                }

                shipment.Price = weightPrice + additionalServicesPrice;

				serviceShipment.Add(shipment);

				return RedirectToAction("CustomerShipments");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Message = ex.Message });
            }
        }

        private string GetShipmentCode()
        {
            Random rand =  new Random();
            const string chars = "0123456789QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            char[] randomChars = new char[11];

            randomChars[0] = chars[rand.Next(0, 10)];  // Number
            randomChars[1] = chars[rand.Next(10, 36)]; // Uppercase letter
            randomChars[2] = chars[rand.Next(36, 62)]; // Lowercase letter

            for (int i = 3; i < 11; i++)
            {
                randomChars[i] = chars[rand.Next(chars.Length)];
            }

            for (int i = randomChars.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                char temp = randomChars[i];
                randomChars[i] = randomChars[j];
                randomChars[j] = temp;
            }

            string shipmentCode = new string(randomChars);
            return shipmentCode;
        }

        [Authorize(Roles = "User")]
        public IActionResult AddService(int additionalServiceId, int number)
        {
            try
            {
                AdditionalService service = serviceAdditonalService.FindByID(additionalServiceId);

                AdditonalServiceViewModel model = new AdditonalServiceViewModel
                {
                    AdditionalServiceId = service.AdditionalServiceId,
                    AddtionalServiceName = service.AdditionalServiceName,
                    AdditonalServicePrice = service.AdditionalServicePrice,
                    Sn = number
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "User")]
        public IActionResult CustomerShipments()
        {
            try
            {
                var userId = int.Parse(userManager.GetUserId(HttpContext.User));

                List<Shipment> model = serviceShipment.GetAllOfSpecifiedUser(userId);

                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Message = ex.Message });
            }
        }

		public IActionResult RemoveShipment(int id)
		{
            try
            {
				serviceShipment.RemoveShipment(id);

				return RedirectToAction("CustomerShipments");
			}
			catch (Exception ex)
			{
				return RedirectToAction("Error", "Home", new { Message = ex.Message });
			}
		}
	}   
}
