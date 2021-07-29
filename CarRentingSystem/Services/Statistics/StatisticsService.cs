using CarRentingSystem.Data;
using CarRentingSystem.Services.Statistics.Interfaces;
using System.Linq;

namespace CarRentingSystem.Services.Statistics
{
    public class StatisticsService : IStatisticService
    {
        private readonly CarRentingDbContext data;

        public StatisticsService(CarRentingDbContext data)
            =>  this.data = data;


        public StatisticsServiceViewModel Total()
        {
            var totalCars = this.data.Cars.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceViewModel
            {
                TotalCars = totalCars,
                TotalUsers = totalUsers,
            };
        }
    }
}
