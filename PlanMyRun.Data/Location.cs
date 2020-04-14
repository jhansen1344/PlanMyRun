using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Data
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double MaxDistance { get; set; }
        [Required]
        public bool HasLoops { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        public string Address { get; set; }
        public string PathType { get; set; }
        public virtual List<Run> RunsAtLocation { get; set; }
    }
}
