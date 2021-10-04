namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Services.Cars;
    using CarRentingSystem.Services.Cars.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static WebConstants.Cache;

    public class HomeController : Controller
    {
        private readonly IMemoryCache cache;
        private readonly ICarService cars;

        public HomeController(
            IMemoryCache cache,
            ICarService cars)
        {
            this.cache = cache;
            this.cars = cars;
        }

        public IActionResult Index()
        {
            var latestCars = this.cache.Get<List<LatestCarsServiceModel>>(LatestCarsCacheKey);

            if (latestCars==null)
            {
                latestCars = this.cars
                    .Latest()
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                this.cache.Set(LatestCarsCacheKey, latestCars, cacheOptions);
            }

            return View(latestCars);
        }

        public IActionResult Error() => View();
    }
}
