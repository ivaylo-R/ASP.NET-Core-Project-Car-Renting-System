using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Data.Models
{
    using static Data.DataConstants.Dealer;
    public class Dealer
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [StringLength(DealerNameMaxLength,MinimumLength =DealerNameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(DealerPhoneNumberMaxLength,MinimumLength =DealerPhoneNumberMinLength)]
        public string PhoneNumber { get; init; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Car> Cars { get; init; } = new List<Car>();
    }
}
