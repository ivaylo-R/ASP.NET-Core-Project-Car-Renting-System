﻿using CarRentingSystem.Models;
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
    }
}
