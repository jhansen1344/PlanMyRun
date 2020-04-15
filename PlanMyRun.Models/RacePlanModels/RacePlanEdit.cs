using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.RacePlanModels
{
    public class RacePlanEdit
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Please enter at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "Please enter less than 50 characters.")]
        [Display(Name = "Plan Name")]
        public string RaceName { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = ("Please enter a valid date"))]
        [Display(Name = "Race Date")]
        public DateTime RaceDate { get; set; }
        [Display(Name = "Can others use this plan as a template?")]
        [Required]
        public bool IsPublic { get; set; }
        [Display(Name = "Race Length")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number.")]
        public double? RaceLength { get; set; }
        [Display(Name = "Goal Finish Time")]
        public TimeSpan? GoalTime { get; set; }
        [Display(Name = "Description of Plan")]
        [MaxLength(1000, ErrorMessage = "Please enter a shorter description.")]
        public string Description { get; set; }

    }
}
