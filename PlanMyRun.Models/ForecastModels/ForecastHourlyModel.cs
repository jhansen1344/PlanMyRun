using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.ForecastModels
{
    public class ForecastHourlyModel
    {
        public int Time { get; set; }
        public string Wx_Desc { get; set; }
        public int Wx_Code { get; set; }
        public string Wx_Desc_Short { get; set; }
        public string Wx_Icon { get; set; }
        public decimal Temp_F { get; set; }
        public decimal FeelsLike_F { get; set; }
        public string Prob_Precip_Pct { get; set; }
    }
}
