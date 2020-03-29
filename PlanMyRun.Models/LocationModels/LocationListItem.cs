using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.LocationModels
{
    public class LocationListItem
    {
        public int Id { get; set; }
        [Display(Name = "Location Name")]
        public string Name { get; set; }
        [Display(Name = "Maximum Distance without laps")]
        public double MaxDistance { get; set; }
        [Display(Name = "Has Loops")]
        public bool HasLoops { get; set; }
    }
}
