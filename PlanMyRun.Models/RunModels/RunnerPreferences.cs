using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.RunModels
{
    public class RunnerPreferences
    {
        public bool LikesHeat { get; set; }
        public bool LikesDark { get; set; }
        public bool LikesMorning { get; set; }
        public bool LikesRain { get; set; }

        public string Zipcode { get; set; }
    }
}
