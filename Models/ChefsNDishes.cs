#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Chefs_N_Dishes.Models
{
    public class Chef
    {
        [Key]
        public int ChefId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [CustomValidation(typeof(Chef), "ValidateAge")]
        public DateTime DateOfBirth { get; set; }
        public string Name => $"{FirstName} {LastName}";

        public int Age => DateTime.Today.Year - DateOfBirth.Year;

        public List<Dish> Dishes { get; set; } = new(); // Ensure this is defined as a collection

        public static ValidationResult? ValidateAge(DateTime dateOfBirth, ValidationContext context)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;
            return age >= 18 ? ValidationResult.Success : new ValidationResult("Chef must be at least 18 years old.");
        }
    }

    public class Dish
    {
        [Key]
        public int DishId { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [Range(1, 5)]
        public int Tastiness { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Calories must be greater than 0.")]
        public int Calories { get; set; }

        public int ChefId { get; set; }
        public Chef? Chef { get; set; }
    }
}
