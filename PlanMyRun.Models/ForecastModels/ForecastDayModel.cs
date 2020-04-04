using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.ForecastModels
{
    public class ForecastDayModel
    {
        public string Date { get; set; }
        [Display(Name = "Sunrise")]
        public TimeSpan Sunrise_Time { get; set; }
        [Display(Name = "Sunset")]
        public TimeSpan Sunset_Time { get; set; }
        [Display(Name = "High Temp")]
        public decimal Temp_Max_F { get; set; }
        [Display(Name = "Low Temp")]
        public decimal Temp_Min_F { get; set; }
        [Display(Name = "Precip Chances")]
        public decimal Prob_Precip_Pct { get; set; }
        public IEnumerable<ForecastHourlyModel> TimeFrames { get; set; }
    }
}
