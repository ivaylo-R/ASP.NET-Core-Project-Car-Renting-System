using CarRentingSystem.Data;
using CarRentingSystem.Models;
using CarRentingSystem.Models.Cars;
using CarRentingSystem.Models.Home;
using CarRentingSystem.Services.Statistics.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace CarRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatisticService service;
        private readonly CarRentingDbContext data;

        public HomeController(IStatisticService service, 
            CarRentingDbContext data)
        {
            this.service = service;
            this.data = data;
        }

        public IActionResult Index()
        {
            var cars = this.data
                .Cars
                .OrderByDescending(c => c.Id)
                .Select(c => new CarIndexViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    ImageUrl = c.ImageUrl,
                    Year = c.Year,
                    Model = c.Model,
                })
                .Take(3)
                .ToList();

            var statistics = this.service.Total();

            return View(new IndexViewModel
            {
                TotalCars=statistics.TotalCars,
                TotalUsers=statistics.TotalUsers,
                Cars=cars,
            });
        }

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });


    }
}
