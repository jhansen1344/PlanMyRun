using PlanMyRun.Models.RunModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.LocationModels
{
    public class LocationDetail
    {

        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Maximum Distance Without Laps")]
        public double MaxDistance { get; set; }
        [Display(Name = "Has Loops")]
        public bool HasLoops { get; set; }
        [Display(Name = "Location ZipCode")]
        public string Address { get; set; }
        [Display(Name = "Path Types at Location")]
        public string PathType { get; set; }
        [Display(Name = "Runs at Location")]
        public List<RunListItem> RunsAtLocation { get; set; }
    }
}
