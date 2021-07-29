﻿using CarRentingSystem.Services.Cars;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Models.Cars
{
    public class AllCarsQueryModel
    {
        public const int CarsPerPage = 3;

        public string Brand { get; init; }


        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public int CurrentPage { get; init; } = 1;

        public CarSorting Sorting { get; init; }

        public IEnumerable<CarServiceModel> Cars { get; set; }

        public IEnumerable<string> Brands { get; set; }

        public int TotalCars { get; set; }

    }
}
