using PlanMyRun.Models.ForecastModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.RunModels
{
    public class WeeklyRunForecastList
    {
        public List<RunDetail> WeeklyRuns { get; set; }
        public ForecastResultModel WeeklyForecast { get; set; }
    }
}
