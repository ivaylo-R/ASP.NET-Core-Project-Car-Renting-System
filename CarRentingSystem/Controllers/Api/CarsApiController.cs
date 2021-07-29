using CarRentingSystem.Models.Api.Cars;
using CarRentingSystem.Services.Cars;
using CarRentingSystem.Services.Cars.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingSystem.Controllers.Api
{
    [ApiController]
    [Route("api/cars")]
    public class CarsApiController : ControllerBase
    {
        private readonly ICarService cars;

        public CarsApiController(ICarService carService)
            =>this.cars = carService;
        

        [HttpGet]
        public CarQueryServiceModel All([FromQuery] AllCarsApiRequestModel query)
            =>cars.All(query.Brand, 
                query.SearchTerm, 
                query.Sorting, 
                query.CurrentPage,
                query.CarsPerPage);

    }
}
