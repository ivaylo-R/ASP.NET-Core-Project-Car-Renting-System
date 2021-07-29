﻿using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Infrastucture;
using CarRentingSystem.Models.Dealers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarRentingSystem.Controllers
{
    public class DealersController : Controller
    {
        private readonly CarRentingDbContext data;

        public DealersController(CarRentingDbContext data)
            =>this.data = data;


        [Authorize]
        public IActionResult Become() => View();
        
        [Authorize]
        [HttpPost]
        public IActionResult Become(BecomeDealerFormModel dealer)
        {
            var userId = this.User.GetId();

            var userIsAlreadyDealer = this.data
                .Dealers
                .Any(d => d.UserId == userId);

            if (userIsAlreadyDealer)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dealer);
            }

            var dealerData = new Dealer
            {
                PhoneNumber = dealer.PhoneNumber,
                Name = dealer.Name,
                UserId = userId,
            };

            this.data.Dealers.Add(dealerData);

            this.data.SaveChanges();

            return RedirectToAction("All","Cars");
        }
    }
}
