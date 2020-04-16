using PlanMyRun.Models.ForecastModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.RunModels
{
    public class DailyRunForecast
    {
        public DateTimeOffset Date { get; set; }
        public RunDetail DaysRun { get; set; } = new RunDetail();
        public ForecastDayModel DaysForecast { get; set; }
    }
}
