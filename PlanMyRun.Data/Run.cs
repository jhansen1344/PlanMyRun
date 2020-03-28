using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Data
{
    public class Run
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(RacePlan))]
        public int RacePlanId { get; set; }
        public virtual RacePlan RacePlan { get; set; }
        [Required]
        public double PlannedDistance { get; set; }
        [Required]
        public TimeSpan EstimatedTime { get; set; }
        [Required]
        public DateTimeOffset ScheduledDateTime { get; set;}
        public string Description { get; set; }
        public TimeSpan? ActualTime { get; set; }
        public double? ActualDistance { get; set; }
        [ForeignKey(nameof(Location))]
        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }

    }
}
