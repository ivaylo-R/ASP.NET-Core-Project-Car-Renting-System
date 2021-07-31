using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Infrastucture;
using CarRentingSystem.Models.Dealers;
using CarRentingSystem.Services.Dealers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarRentingSystem.Controllers
{
    public class DealersController : Controller
    {
        private readonly IDealerService dealerService;

        public DealersController(IDealerService dealerService)
            => this.dealerService = dealerService;


        [Authorize]
        public IActionResult Become() => View();
        
        [Authorize]
        [HttpPost]
        public IActionResult Become(BecomeDealerFormModel dealer)
        {
            var userId = this.User.GetId();


            if (dealerService.IsDealer(userId))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dealer);
            }

            dealerService.RegisterDealer(dealer, userId);

            return RedirectToAction("All","Cars");
        }
    }
}
