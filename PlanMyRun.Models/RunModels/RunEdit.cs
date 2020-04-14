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
        public int RacePlanId { get; set; }
        public IEnumerable<SelectListItem> RacePlan { get; set; }
        [Display(Name = "Planned Distance for Run")]
        public double PlannedDistance { get; set; }

        [Display(Name = "Date for Run")]
        public string ScheduleDateTime { get; set; }

        [Display(Name = "Location Id")]
        public int? LocationId { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        [Display(Name = "Location of Run")]
        public double? ActualDistance { get; set; }
        [Display(Name = "Actual Time To Complete")]
        public TimeSpan? ActualTime { get; set; }

        [Display(Name = "Notes about this Run")]
        public string Description { get; set; }
    }

}

