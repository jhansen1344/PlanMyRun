using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Data
{
    public class RacePlan
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string RaceName { get; set; }
        [Required]
        public DateTimeOffset RaceDate { get; set;}
        [Required]
        public bool IsPublic { get; set; }
        public double? RaceLength { get; set; }
        public TimeSpan? GoalTime { get; set; }
        public string Description { get; set; }
        public virtual List<Run> ListOfRuns { get; set; }

    }
}
