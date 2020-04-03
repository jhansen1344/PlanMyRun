using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.RunModels
{
    public class RunCreate
    {
        [Required]
        [Display(Name ="Add Run to Race Plan:")]
        public int RacePlanId { get; set; }
        [Required]
        [Display(Name = "Planned Distance for Run.")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number.")]
        public double PlannedDistance { get; set; }
        [Required]
        [Display(Name = "Estimated Time to Complete.")]
        public TimeSpan EstimatedTime { get; set; }
        [Required]
        [Display(Name = "Date for Run")]
        [DataType(DataType.Date, ErrorMessage = ("Please enter a valid date"))]
        public DateTime ScheduledDate { get; set; }
        [Display(Name = "Notes about this Run:")]
        [MaxLength(1000, ErrorMessage = "Please enter a shorter note")]
        public string Description { get; set; }
        [Display(Name = "Location of Run")]
        public int? LocationId { get; set; }
    }
}
