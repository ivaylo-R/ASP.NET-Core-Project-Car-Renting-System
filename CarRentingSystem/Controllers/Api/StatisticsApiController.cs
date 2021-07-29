using CarRentingSystem.Services.Statistics;
using CarRentingSystem.Services.Statistics.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingSystem.Controllers.Api
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticService statistics;

        public StatisticsApiController(IStatisticService service)
            =>this.statistics = service;
        
        public StatisticsServiceViewModel GetStatistics()
            => this.statistics.Total();
    }
}
