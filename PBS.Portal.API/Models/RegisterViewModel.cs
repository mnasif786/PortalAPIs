using System;
using System.ComponentModel.DataAnnotations;

namespace PBS.Portal.API.Models
{
    public class RegisterViewModel
    {
        [Required]
        public Guid PendingUserId { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).{7,}.+$", ErrorMessage = "{0} must be at least 8 characters and contain lowercase, uppercase, a number and a special character.")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // considered having a separate PendingUserViewModel to put these in but felt there was no need.
        public string CAN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
