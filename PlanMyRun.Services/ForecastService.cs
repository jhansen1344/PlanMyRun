
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PlanMyRun.Models.ForecastModels;
using PlanMyRun.Models.RunModels;
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

        private readonly bool _likesDark;
        private readonly bool _likesHeat;
        private readonly bool _likesMorning;
        private readonly bool _likesRain;

        public ForecastService(Guid userId, string zipCode)
        {
            _userId = userId;
            _userZip = zipCode;
        }

        public ForecastService(Guid userId, RunnerPreferences runner)
        {
            _userId = userId;
            _userZip = runner.Zipcode;
            _likesDark = runner.LikesDark;
            _likesHeat = runner.LikesHeat;
            _likesMorning = runner.LikesMorning;
            _likesRain = runner.LikesRain;
        }
        readonly string uri = "http://api.weatherunlocked.com/api/forecast/";
        private string appId = "0582982b";
        private string appKey = "1bf4c33953cec0914f0b50a66f13b552";
        private readonly string _userZip;

        public async Task<ForecastResultModel> GetForecastAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                // httpClient.BaseAddress=new Uri($"{uri}{appId}&app_key={appKey}");
                Uri weatherUri = new Uri($"{uri}us.{_userZip}?app_id={appId}&app_key={appKey}");
                HttpResponseMessage response = await httpClient.GetAsync(weatherUri);
                if (response.IsSuccessStatusCode)
                {
                    var model = JsonConvert.DeserializeObject<ForecastResultModel>(await response.Content.ReadAsStringAsync(), new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    return model;
                }
                return null;
            }
        }

        public async Task<List<ForecastEvent>> GetAvailableRunTimes()
        {
            var listForecastEvents = new List<ForecastEvent>();
            TimeSpan morningUnavailable;
            TimeSpan nightUnavailable;
            bool isHot = false;
            int startHeat = 0;
            bool isRaining = false;
            int startRain = 0;
            string description = "";

            var weeklyForecast = await GetForecastAsync();
            foreach (var forecastDay in weeklyForecast.Days)
            {

                if (_likesDark)
                {
                    morningUnavailable = forecastDay.Sunrise_Time.Subtract(TimeSpan.FromHours(1));
                    nightUnavailable = forecastDay.Sunset_Time.Add(TimeSpan.FromHours(1));
                }
                else
                {
                    morningUnavailable = forecastDay.Sunrise_Time.Subtract(TimeSpan.FromHours(.5));
                    nightUnavailable = forecastDay.Sunset_Time.Add(TimeSpan.FromHours(.5));

                }

                if (!_likesMorning)
                {
                    morningUnavailable = new TimeSpan(11, 59, 00);
                }
                var morningEvent = new ForecastEvent()
                {
                    StartTime = forecastDay.Date,
                    EndTime = forecastDay.Date.Add(morningUnavailable),
                    Description = "Too early."
                };
                listForecastEvents.Add(morningEvent);
                var nightEvent = new ForecastEvent()
                {
                    StartTime = forecastDay.Date.Add(nightUnavailable),
                    EndTime = forecastDay.Date.AddDays(1),
                    Description = "Too late."
                };
                listForecastEvents.Add(nightEvent);


                foreach (var item in forecastDay.TimeFrames)
                {
                    if (!_likesHeat)
                    {
                        if (item.FeelsLike_F >= 89 && !isHot)
                        {
                            startHeat = item.Time / 100;
                            isHot = true;
                        }
                        if (item.FeelsLike_F < 89 && isHot)
                        {
                            var endHeat = item.Time / 100;
                            isHot = false;
                            var startTime = forecastDay.Date.AddHours(startHeat);
                            var endTime = forecastDay.Date.AddHours(endHeat);

                            var heatEvent = new ForecastEvent()
                            {
                                StartTime = startTime,
                                EndTime = endTime,
                                Description = "Too hot"
                            };
                            listForecastEvents.Add(heatEvent);
                        }
                    }
                    if (item.Prob_Precip_Pct != "<1")
                    {
                        if (Int32.Parse(item.Prob_Precip_Pct) > 70 && !isRaining)
                        {
                            if (_likesRain && Int32.Parse(item.Prob_Precip_Pct) < 85)
                            {
                                if (item.Wx_Code == 21 || item.Wx_Code == 50 || item.Wx_Code == 60) { continue; }
                                else
                                {
                                    startRain = item.Time / 100;
                                    isRaining = true;
                                    description = item.Wx_Desc;

                                }

                            }
                            else
                            {
                                startRain = item.Time / 100;
                                isRaining = true;
                                description = item.Wx_Desc;
                            }
                        }
                        if (Int32.Parse(item.Prob_Precip_Pct) < 70 && isRaining || forecastDay.TimeFrames.Count() == forecastDay.TimeFrames.ToList().IndexOf(item) + 1 && isRaining)
                        {
                            var endRain = item.Time / 100;
                            isRaining = false;
                            var startTime = forecastDay.Date.AddHours(startRain);
                            var endTime = forecastDay.Date.AddHours(endRain);
                            var rainEvent = new ForecastEvent()
                            {
                                StartTime = startTime,
                                EndTime = endTime,
                                Description = description,
                            };
                            listForecastEvents.Add(rainEvent);
                        }

                    }

                }
            }

            return listForecastEvents;
        }
    }
}
