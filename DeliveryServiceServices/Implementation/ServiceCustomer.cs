﻿using DeliveryServiceApp.Services.Interfaces;
using DeliveryServiceData.UnitOfWork;
using DeliveryServiceDomain;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DeliveryServiceApp.Services.Implementation
{
    public class ServiceCustomer : IServiceCustomer
    {

		private readonly IPersonUnitOfWork unitOfWork;

		public ServiceCustomer(IPersonUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public void Edit(Customer customer)
		{
			if (!IsValid(customer))
			{
				throw new ArgumentOutOfRangeException("Invalid entry!");
			}

			unitOfWork.Customer.Edit(customer);
		}

		private bool IsValid(Customer customer)
		{
			bool valid = true;

			if (customer == null) return false;
			if (string.IsNullOrEmpty(customer.Address) || string.IsNullOrEmpty(customer.PostalCode))
			{
				return false;
			}

			return valid;
		}


		public Customer FindByID(int id, params int[] ids)
		{
			return unitOfWork.Customer.FindByID(id, ids);
		}

		public List<Customer> GetAll()
		{
			return unitOfWork.Customer.GetAll();
		}
	}
}
