using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Models.Cars;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarRentingSystem.Controllers
{
    public class CarsController : Controller
    {
        private CarRentingDbContext data;

        public CarsController(CarRentingDbContext data)
        {
            this.data = data;
        }

        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(AddCarFormModel car)
        {
            if (data.Cars.Any(c=>c.Id !=car.Id))
            {
                return View("Error");
            }

            var carData = new Car
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                ImageUrl = car.ImageUrl,
                CategoryId = car.CategoryId,
                Description = car.Description,
            };

            data.Cars.Add(carData);

            data.SaveChanges();

            return Redirect("Home");
        }
    }
}
