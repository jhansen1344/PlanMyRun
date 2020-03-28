﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.RunModels
{
    public class RunListItem
    {
        [Display(Name = "Race Name")]
        public string RacePlanName { get; set; }
        [Display(Name = "Planned Distance for Run.")]
        public double PlannedDistance { get; set; }
        [Display(Name = "Estimated Time to Complete.")]
        public TimeSpan EstimatedTime { get; set; }
        [Display(Name = "Date for Run")]
        public DateTimeOffset ScheduledDateTime { get; set; }
        [Display(Name = "Location of Run")]
        public string Location { get; set; }
        [Display(Name = "Actual Distance Ran.")]
        public double? ActualDistance { get; set; }
        [Display(Name = "Actual Time To Complete.")]
        public TimeSpan? ActualTime { get; set; }
    }
}
