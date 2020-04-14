using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.RacePlanModels
{
    public class RacePlanListItem
    {
        public int Id { get; set; }
        [Display(Name = "Plan Name")]
        public string RaceName { get; set; }
        [Display(Name = "Race Length")]
        public double? RaceLength { get; set; }
        [Display(Name = "Goal Finish Time")]
        public TimeSpan? GoalTime { get; set; }
        [Display(Name = "Race Date")]
        public DateTime RaceDate { get; set; }
        [Display(Name = "Available As Template?")]
        public bool IsPublic { get; set; }
        [Display(Name = "Number of Runs in Plan")]
        public int NumberOfRuns { get; set; }

    }
}
