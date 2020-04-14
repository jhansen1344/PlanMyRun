using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.LocationModels
{
    public class LocationCreate
    {
        [Required]
        [MinLength(2, ErrorMessage = "Please enter at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "Please enter less than 50 characters.")]
        [Display(Name="Location Name")]
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number.")]
        [Display(Name = "Maximum Distance Without Laps")]
        public double MaxDistance { get; set; }
        [Required]
        [Display(Name = "Has Loops")]
        public bool HasLoops { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Location ZipCode")]
        public string Address { get; set; }
        [Display(Name = "Path Types at Location")]
        [MinLength(2, ErrorMessage = "Please enter at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "Please enter less than 50 characters.")]
        public string PathType { get; set; }
    }
}
