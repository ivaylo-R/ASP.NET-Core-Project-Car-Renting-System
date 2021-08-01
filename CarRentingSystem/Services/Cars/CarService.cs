using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Models;
using CarRentingSystem.Models.Cars;
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
            => GetCars(this.data
                .Cars
                .Where(c => c.Dealer.UserId == userId));


        public IEnumerable<CarCategoryServiceModel> AllCarCategories()
            => this.data
                .Categories
                .Select(c => new CarCategoryServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
            .ToList();

        public CarDetailsServiceModel Details(int id)
            => this.data
                .Cars
                .Where(c => c.Id == id)
                .Select(c => new CarDetailsServiceModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    CategoryName = c.Category.Name,
                    CategoryId = c.CategoryId,
                    DealerId = c.DealerId,
                    DealerName = c.Dealer.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    UserId = c.Dealer.UserId

                })
            .FirstOrDefault();

        public int Create(
            string brand,
            string model,
            int year,
            string imageUrl,
            int categoryId,
            string description,
            int dealerId)
        {
            var carData = new Car
            {
                Brand = brand,
                Model = model,
                Year = year,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Description = description,
                DealerId = dealerId,
            };

            data.Cars.Add(carData);

            data.SaveChanges();

            return carData.Id;
        }

        public bool CategoryExist(int categoryId)
            => this.data
                .Categories
                .Any(i => i.Id == categoryId);


        public bool IsByDealer(int carId, int dealerId)
            => this.data.Cars.Any(c => c.Id == carId && c.DealerId == dealerId);

        public bool Edit(int id,
            string brand,
            string model,
            int year,
            string imageUrl,
            int categoryId,
            string description,
            int dealerId)
        {
            var carData= this.data.Cars.Find(id);

            if (carData==null)
            {
                return false;
            }

            carData.Brand = brand;
            carData.Model = model;
            carData.Year = year;
            carData.ImageUrl = imageUrl;
            carData.Description = description;
            carData.CategoryId = categoryId;

            this.data.SaveChanges();

            return true;
        } 

        private static IEnumerable<CarServiceModel> GetCars(IQueryable<Car> carQuery)
           => carQuery
               .Select(c => new CarServiceModel
               {
                   Id = c.Id,
                   Brand = c.Brand,
                   Model = c.Model,
                   Year = c.Year,
                   ImageUrl = c.ImageUrl,
                   CategoryName = c.Category.Name
               })
               .ToList();

    }
}
