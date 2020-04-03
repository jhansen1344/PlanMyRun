
using Newtonsoft.Json;
using PlanMyRun.Models.ForecastModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Services
{
    public class ForecastService
    {
        private readonly Guid _userId;

        public ForecastService(Guid userId)
        {
            _userId = userId;
        }
        readonly string uri = "http://api.weatherunlocked.com/api/forecast/";
        private string appId = "0582982b";
        private string appKey = "1bf4c33953cec0914f0b50a66f13b552";
        
        private string userZip = "46227";

        public async Task<ForecastResultModel> GetForecastAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                // httpClient.BaseAddress=new Uri($"{uri}{appId}&app_key={appKey}");
                Uri weatherUri = new Uri($"{uri}us.{userZip}?app_id={appId}&app_key={appKey}");
                HttpResponseMessage response = await httpClient.GetAsync(weatherUri);
                if (response.IsSuccessStatusCode)
                {
                    var model = JsonConvert.DeserializeObject<ForecastResultModel>(await response.Content.ReadAsStringAsync());
                    return model;
                }
                return null;
            }
        }
    }
}
