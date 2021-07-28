using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Models.Cars;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        public IActionResult Details()
            => View();

        public IActionResult All([FromQuery]AllCarsQueryModel query)
        {
            var carsQuery = this.data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                carsQuery = carsQuery.Where(c =>
                      c.Brand == query.Brand);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                carsQuery = carsQuery.Where(c =>
                      (c.Brand + " " + c.Model).ToLower().Contains(query.SearchTerm.ToLower()) ||
                      c.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            carsQuery = query.Sorting switch
            {
                CarSorting.Year=>carsQuery.OrderByDescending(c=>c.Year),
                CarSorting.Brand=>carsQuery.OrderBy(c=>c.Brand),
                CarSorting.Model=>carsQuery.OrderBy(c=>c.Model),
                CarSorting.DateCreated or _=> carsQuery.OrderByDescending(c => c.Id)
            };

            var totalCars = carsQuery.Count();

            var cars =
                carsQuery
                .Skip((query.CurrentPage-1)* AllCarsQueryModel.CarsPerPage)
                .Take(AllCarsQueryModel.CarsPerPage)
                .Select(c => new CarListingViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Category = c.Category.Name,
                    ImageUrl = c.ImageUrl,
                    Year = c.Year,
                    Model = c.Model,
                })
                .ToList();

            var carBrands = this.data
                .Cars
                .Select(c => c.Brand)
                .Distinct()
                .ToList();

            query.Cars = cars;
            query.Brands = carBrands;
            query.TotalCars = totalCars;


            return View(query);
        }

        public IActionResult Add()
            => View(new AddCarFormModel
            {
                Categories = this.GetCategories()
            });

        [HttpPost]
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
