﻿using AutoMapper;
using DataTransferObjects;
using DeliveryServiceApp.Models;
using DeliveryServiceApp.Services.Interfaces;
using DeliveryServiceData.UnitOfWork;
using DeliveryServiceData.UnitOfWork.Implementation;
using DeliveryServiceDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IServiceStatus serviceStatus;
        private readonly IServiceStatusShipment serviceStatusShipment;
        private readonly IMapper mapper;

        public ShipmentController(UserManager<Person> userManager, IServiceAdditonalService serviceAdditonalService, IServiceShipmentWeight serviceShipmentWeight,
                                  IServiceShipment serviceShipment, IServiceAddionalServiceShipment serviceAddionalServiceShipment, IServiceStatus serviceStatus,
                                  IServiceStatusShipment serviceStatusShipment, IMapper mapper)
        {
            this.userManager = userManager;
            this.serviceAdditonalService = serviceAdditonalService;
            this.serviceShipmentWeight = serviceShipmentWeight;
            this.serviceShipment = serviceShipment;
            this.serviceAddionalServiceShipment = serviceAddionalServiceShipment;
            this.serviceStatus = serviceStatus;
            this.serviceStatusShipment = serviceStatusShipment;
            this.mapper = mapper;
        }

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            try
            {
                List<AdditionalServiceDto> additionalServicesList = serviceAdditonalService.GetAll();
                List<SelectListItem> selectAdditionalServicesList = additionalServicesList.Select(s => new SelectListItem { Text = s.AdditionalServiceName + " - " + s.AdditionalServicePrice + " RSD", Value = s.AdditionalServiceId.ToString() }).ToList();

                List<ShipmentWeightDto> shipmentWeightList = serviceShipmentWeight.GetAll();
                List<SelectListItem> selectShipmentWeightList = shipmentWeightList.Select(s => new SelectListItem { Text = s.ShipmentWeightDescpription, Value = s.ShipmentWeightId.ToString() }).ToList();

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
                    List<AdditionalServiceDto> additionalServicesList = serviceAdditonalService.GetAll();
                    List<SelectListItem> selectAdditionalServicesList = additionalServicesList.Select(s => new SelectListItem { Text = s.AdditionalServiceName + " - " + s.AdditionalServicePrice + " RSD", Value = s.AdditionalServiceId.ToString() }).ToList();

                    List<ShipmentWeightDto> shipmentWeightList = serviceShipmentWeight.GetAll();
                    List<SelectListItem> selectShipmentWeightList = shipmentWeightList.Select(s => new SelectListItem { Text = s.ShipmentWeightDescpription, Value = s.ShipmentWeightId.ToString() }).ToList();

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

                Random rand = new Random();
                const string chars = "0123456789QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
                shipment.ShipmentCode = new string(Enumerable.Repeat(chars, 11)
                                                              .Select(s => s[rand.Next(chars.Length)])
                                                              .ToArray());


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
                    List<AdditionalServiceDto> additionalServices = serviceAdditonalService.GetAll();

                    foreach (AdditonalServiceViewModel sa in model.Services)
                    {
                        additionalServicesPrice += additionalServices.Find(s => s.AdditionalServiceId == sa.AdditionalServiceId).AdditionalServicePrice;
                    }
                }

                shipment.Price = weightPrice + additionalServicesPrice;

                serviceShipment.Add(mapper.Map<ShipmentDto>(shipment));

                var newShipment = serviceShipment.FindByCode(shipment.ShipmentCode);

                foreach (AdditonalServiceViewModel sa in model.Services)
                {
                    AdditionalServiceShipment ass = new AdditionalServiceShipment();
                    ass.AdditionalServiceId = sa.AdditionalServiceId;
                    ass.ShipmentId = newShipment.ShipmentId;
                    serviceAddionalServiceShipment.Add(mapper.Map<AdditionalServiceShipmentDto>(ass));
                }

                StatusShipment ss = new StatusShipment
                {
                    ShipmentId = newShipment.ShipmentId,
                    StatusId = serviceStatus.GetByName("Scheduled").StatusId,
                    StatusTime = DateTime.Now
                };

                serviceStatusShipment.Add(mapper.Map<StatusShipmentDto>(ss));

                return RedirectToAction("CustomerShipments");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "User")]
        public IActionResult AddService(int additionalServiceId, int number)
        {
            try
            {
                AdditionalServiceDto service = serviceAdditonalService.FindByID(additionalServiceId);

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

                List<ShipmentDto> model = serviceShipment.GetAllOfSpecifiedUser(userId);

                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Message = ex.Message });
            }
        }
    }   
}
