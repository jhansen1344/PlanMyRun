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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd}")]
        public DateTimeOffset Date { get; set; }
        [Display(Name = "Sunrise")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan Sunrise_Time { get; set; }
        [Display(Name = "Sunset")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan Sunset_Time { get; set; }
        [Display(Name = "High Temp")]
        [DisplayFormat(DataFormatString = "{0:#.}")]
        public decimal Temp_Max_F { get; set; }
 
        [Display(Name = "Low Temp")]
        [DisplayFormat(DataFormatString = "{0:#.}")]
        public decimal Temp_Min_F { get; set; }

        [Display(Name = "Precip.")]
        [DisplayFormat(DataFormatString = "{0:#.}")]
        public decimal Prob_Precip_Pct { get; set; }
        public IEnumerable<ForecastHourlyModel> TimeFrames { get; set; }
    }
}
