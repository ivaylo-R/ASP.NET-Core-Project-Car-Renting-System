using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Infrastucture;
using CarRentingSystem.Models.Cars;
using CarRentingSystem.Services.Cars;
using CarRentingSystem.Services.Cars.Interfaces;
using CarRentingSystem.Services.Dealers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CarRentingSystem.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService carService;
        private readonly IDealerService dealerService;


        public CarsController(
            ICarService carService,
            IDealerService dealerService)
        {
            this.carService = carService;
            this.dealerService = dealerService;
        }

        public IActionResult Details()
            => View();

        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryResult = this.carService.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllCarsQueryModel.CarsPerPage);

            var carBrands = this.carService.AllCarBrands();

            query.Cars = queryResult.Cars;
            query.Brands = carBrands;
            query.TotalCars = queryResult.TotalCars;


            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
            => View(this.carService.ByUser(this.User.GetId()));


        public IActionResult Add()
        {
            if (!dealerService.IsDealer(this.User.GetId()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new CarFormModel
            {
                Categories = this.carService.AllCarCategories()
            });
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(CarFormModel car, IFormFile image)
        {
            var dealerId = dealerService.DealerId(this.User.GetId());

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!this.carService.CategoryExist(car.CategoryId))
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = this.carService.AllCarCategories();

                return View(car);
            }

            int carId = carService.Create(car.Brand,
                car.Model,
                car.Year,
                car.ImageUrl,
                car.CategoryId,
                car.Description,
                dealerId);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();

            if (!this.dealerService.IsDealer(userId))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            var car = this.carService.Details(id);

            if (car.UserId != userId)
            {
                return Unauthorized();
            }

            return View(new CarFormModel
            {
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
                CategoryId = car.CategoryId,
                ImageUrl = car.ImageUrl,
                Categories = this.carService.AllCarCategories(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, CarFormModel car)
        {
            var dealerId = dealerService.DealerId(this.User.GetId());

            if (!this.carService.CategoryExist(car.CategoryId))
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = this.carService.AllCarCategories();

                return View(car);
            }

            if (!carService.IsByDealer(id,dealerId))
            {
                return BadRequest();
            }

            var isSucceed = carService.Edit(id,car.Brand,car.Model,car.Year,car.ImageUrl,car.CategoryId,car.Description,dealerId);

            if (!isSucceed)
            {
                return Unauthorized();
            }

            return RedirectToAction("All", "Cars");
        }

    }
}
