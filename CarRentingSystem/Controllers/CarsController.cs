using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Models;
using CarRentingSystem.Models.Api.Cars;
using CarRentingSystem.Models.Cars;
using CarRentingSystem.Services.Cars.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CarRentingSystem.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly ICarService carService;

        public CarsController(CarRentingDbContext data, 
            ICarService carService)
        {
            this.data = data;
            this.carService = carService;
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

        public IActionResult Add()
            => View(new AddCarFormModel
            {
                Categories = this.GetCategories()
            });

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddCarFormModel car, IFormFile image)
        {
            if (!this.data.Categories.Any(c => c.Id == car.CategoryId))
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = this.GetCategories();

                return View(car);
            }

            var carData = new Car
            {
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                ImageUrl = car.ImageUrl,
                CategoryId = car.CategoryId,
                Description = car.Description,
            };

            data.Cars.Add(carData);

            data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<CarCategoryViewModel> GetCategories()
            => this.data
                .Categories
                .Select(c => new CarCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
            .ToList();
    }
}
