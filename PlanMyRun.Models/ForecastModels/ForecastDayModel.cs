using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.ForecastModels
{
    public class ForecastDayModel
    {
        public string Date { get; set; }
        public TimeSpan Sunrise_Time { get; set; }
        public TimeSpan Sunset_Time { get; set; }
        public decimal Temp_Max_F { get; set; }
        public decimal Temp_Min_F { get; set; }
        public int Prop_Precip_Pct { get; set; }
        public IEnumerable<ForecastHourlyModel> TimeFrames { get; set; }
    }
}
