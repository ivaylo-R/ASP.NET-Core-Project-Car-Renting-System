
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Data.Models
{
    using static Data.DataConstants.Category;

    public class Category
    {
        public int Id { get; init; }

        [Required]
        [StringLength(CategoryNameMaxLength,MinimumLength =CategoryNameMinLength)]
        public string Name { get; init; }

        public IEnumerable<Car> Cars { get; init; } = new List<Car>();
    }
}
