﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.RunModels
{
    public class RunDetail
    {
        public int Id { get; set; }
        [Display(Name = "Race Id")]
        public int RacePlanId { get; set; }
        [Display(Name = "Race Name")]
        public string RacePlanName { get; set; }
        [Display(Name = "Planned Distance for Run")]
        public double PlannedDistance { get; set; }
        [Display(Name = "Estimated Time to Complete")]
        public TimeSpan EstimatedTime { get; set; }
        [Display(Name = "Date for Run")]
        public DateTime ScheduleDateTime { get; set; }
        public DateTime End { get; set; }
        public int? LocationId { get; set; }
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        [Display(Name = "Actual Distance")]
        public double? ActualDistance { get; set; }
        [Display(Name = "Actual Time To Complete")]
        public TimeSpan? ActualTime { get; set; }

        [Display(Name = "Notes about this Run")]
        public string Description { get; set; }
    }
}
