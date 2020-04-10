using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string RaceName { get; set; }
        [Required]
        public DateTime RaceDate { get; set;}
        [Required]
        public bool IsPublic { get; set; }
        public double? RaceLength { get; set; }
        public TimeSpan? GoalTime { get; set; }
        public string Description { get; set; }
        public virtual List<Run> ListOfRuns { get; set; }

    }
}
