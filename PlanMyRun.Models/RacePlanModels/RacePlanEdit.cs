﻿using System;
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
        [Display(Name = "Race Name")]
        public string RaceName { get; set; }
        [Display(Name = "Race Date")]
        public DateTimeOffset RaceDate { get; set; }
        [Display(Name = "Can others use this plan as a template?")]
        public bool IsPublic { get; set; }
        [Display(Name = "Race Length")]
        public double? RaceLength { get; set; }
        [Display(Name = "Goal Finish Time")]
        public TimeSpan? GoalTime { get; set; }
        [Display(Name = "Description of Race")]
        public string Description { get; set; }

    }
}