using CarRentingSystem.Data.Models;
using CarRentingSystem.Models;
using CarRentingSystem.Models.Cars;
using System.Collections.Generic;

namespace CarRentingSystem.Services.Cars.Interfaces
{
    public interface ICarService
    {
        CarQueryServiceModel All(string brand,
            string searchTerm,
            CarSorting sorting,
            int currentPage,
            int carsPerPage);

        IEnumerable<CarServiceModel> ByUser(string userId);

        IEnumerable<string> AllCarBrands();

        IEnumerable<CarCategoryServiceModel> AllCarCategories();

        CarDetailsServiceModel Details(int id);

        int Create(
            string brand,
            string model,
            int year,
            string imageUrl,
            int categoryId,
            string description,
            int dealerId);

        bool Edit(int id,
            string brand,
            string model,
            int year,
            string imageUrl,
            int categoryId,
            string description,
            int dealerId);

        bool IsByDealer(int carId, int dealerId);

        bool CategoryExist(int categoryId);

    }
}
