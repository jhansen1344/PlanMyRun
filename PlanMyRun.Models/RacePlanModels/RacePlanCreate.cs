using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.RacePlanModels
{
    public class RacePlanCreate
    {

        [Required]
        [MinLength(2, ErrorMessage = "Please enter at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "Please enter less than 50 characters.")]
        [Display(Name = "Race Name")]
        public string RaceName { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage =("Please enter a valid date"))]
        [Display(Name = "Race Date")]
        public DateTimeOffset RaceDate { get; set; }
        [Required]
        [Display(Name = "Can others use this plan as a template?")]
        public bool IsPublic { get; set; }
        [Display(Name = "Race Length")]
        [Range(0,double.MaxValue,ErrorMessage ="Please enter a valide number.")]
        public double? RaceLength { get; set; }
        [Display(Name = "Goal Finish Time")]
        public TimeSpan? GoalTime { get; set; }
        [Display(Name = "Description of Race")]
        [MaxLength(1000,ErrorMessage ="Please enter a shorter description.")]
        public string Description { get; set; }
    }
}
