using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Models.Dealers
{

    using static Data.DataConstants.Dealer;
    public class BecomeDealerFormModel
    {
        [Required]
        [StringLength(DealerNameMaxLength,MinimumLength =DealerNameMinLength)]
        public string Name { get; init; }

        [Required]
        [Display(Name="Phone Number")]
        [StringLength(DealerPhoneNumberMaxLength, MinimumLength = DealerPhoneNumberMinLength)]
        public string PhoneNumber { get; init; }

    }
}
