using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PlanMyRun.Models.RunModels
{
    public class RunEdit
    {
        public int Id { get; set; }
        [Display(Name = "Race Id")]
        [Required]
        public int RacePlanId { get; set; }
        public IEnumerable<SelectListItem> RacePlan { get; set; }
        [Required]
        [Display(Name = "Planned Distance for Run")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number")]
        public double PlannedDistance { get; set; }
        [Required]
        [Display(Name = "Date for Run")]
        [DataType(DataType.Date, ErrorMessage = ("Please enter a valid date"))]
        public string ScheduleDateTime { get; set; }

        [Display(Name = "Location Id")]
        public int? LocationId { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        [Display(Name = "Actual Distance Ran")]
        public double? ActualDistance { get; set; }
        [Display(Name = "Actual Time To Complete")]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "Time must be between 00:00:00 to 23:59:00")]
        public TimeSpan? ActualTime { get; set; }

        [Display(Name = "Notes about this Run")]
        [MaxLength(1000, ErrorMessage = "Please enter a shorter note")]
        public string Description { get; set; }
    }

}

