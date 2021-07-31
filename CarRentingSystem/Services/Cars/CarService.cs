using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Models;
using CarRentingSystem.Services.Cars.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CarRentingSystem.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly CarRentingDbContext data;

        public CarService(CarRentingDbContext data)
            => this.data = data;

        public CarQueryServiceModel All([FromQuery]
            string brand,
            string searchTerm,
            CarSorting sorting,
            int currentPage,
            int carsPerPage
            )
        {
            var carsQuery = this.data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                carsQuery = carsQuery.Where(c =>
                      c.Brand == brand);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                carsQuery = carsQuery.Where(c =>
                      (c.Brand + " " + c.Model).ToLower().Contains(searchTerm.ToLower()) ||
                      c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            carsQuery = sorting switch
            {
                CarSorting.Year => carsQuery.OrderByDescending(c => c.Year),
                CarSorting.Brand => carsQuery.OrderBy(c => c.Brand),
                CarSorting.Model => carsQuery.OrderBy(c => c.Model),
                CarSorting.DateCreated or _ => carsQuery.OrderByDescending(c => c.Id)
            };

            var totalCars = carsQuery.Count();

            var cars =
                GetCars(carsQuery)
                .Skip((currentPage - 1) * carsPerPage)
                .Take(carsPerPage)
                .ToList();

            return new CarQueryServiceModel
            {
                TotalCars = totalCars,
                Cars = cars,
                CurrentPage = currentPage,
                CarsPerPage = carsPerPage,
            };
        }

        public IEnumerable<string> AllCarBrands()
            => this.data
                .Cars
                .Select(c => c.Brand)
                .Distinct()
                .ToList();

        public IEnumerable<CarServiceModel> ByUser(string userId)
            => this.GetCars(
                this.data
                .Cars
                .Where(u => u.Dealer.UserId == userId));
                



        private IEnumerable<CarServiceModel> GetCars(IQueryable<Car> carsQuery)
            => carsQuery
                 .Select(c => new CarServiceModel
                 {
                     Id = c.Id,
                     Brand = c.Brand,
                     Category = c.Category.Name,
                     ImageUrl = c.ImageUrl,
                     Year = c.Year,
                     Model = c.Model,
                 });


    }
}
