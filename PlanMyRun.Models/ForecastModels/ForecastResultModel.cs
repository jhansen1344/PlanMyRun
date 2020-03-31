using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Models.ForecastModels
{
    public class ForecastResultModel
    {
        public IEnumerable<ForecastDayModel> Days { get; set; }
    }
}
