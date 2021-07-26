using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Data.Models
{

    using static DataConstants;

    public class Car
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(CarBrandMaxLength)]
        public string Brand { get; set; }

        [Required]
        [StringLength(CarModelMaxLength,MinimumLength=CarModelMinLength)]
        public string Model { get; set; }

        [Required]
        [MaxLength(CarDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Range(CarYearMinValue, CarYearMaxValue)]
        public int Year { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
