using PlanMyRun.Data;
using PlanMyRun.Models.ForecastModels;
using PlanMyRun.Models.RunModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Services
{
    public class RunService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _context;
        private readonly string _zipCode;
        private readonly bool _likesDark;
        private readonly bool _likesHeat;
        private readonly bool _likesMorning;
        private readonly bool _likesRain;


        public RunService(Guid userId, RunnerPreferences runner)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
            _zipCode = runner.Zipcode;
            _likesDark = runner.LikesDark;
            _likesHeat = runner.LikesHeat;
            _likesMorning = runner.LikesMorning;
            _likesRain = runner.LikesRain;
        }

        public async Task<List<ForecastEvent>> GetAvailableRunTimes(ForecastDayModel forecastDay)
        {
            var listForecastEvents = new List<ForecastEvent>();
            TimeSpan morningUnavailable;
            TimeSpan nightUnavailable;
            bool isHot = false;
            int startHeat = 0;
            bool isRaining = false;
            int startRain = 0;

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
                        startHeat = item.Time;
                        isHot = true;
                    }
                    if (item.FeelsLike_F < 89 && isHot)
                    {
                        var endHeat = item.Time;
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
                        if(_likesRain || Int32.Parse(item.Prob_Precip_Pct) < 80 ||item.Wx_Code==21|| item.Wx_Code == 50|| item.Wx_Code == 60)
                        startRain = item.Time;
                        isRaining = true;
                    }
                    if(Int32.Parse(item.Prob_Precip_Pct)<70 && isRaining)
                    {
                        var endRain = item.Time;
                        isRaining = false;
                        var startTime = forecastDay.Date.AddHours(startRain);
                        var endTime = forecastDay.Date.AddHours(endRain);
                        var rainEvent = new ForecastEvent()
                        {
                            StartTime = startTime,
                            EndTime = endTime,
                            Description = "Raining"
                        };
                        listForecastEvents.Add(rainEvent);
                    }

                }



            }

            return listForecastEvents;




        }
        public async Task<bool> CreateRunAsync(RunCreate model)
        {
            var entity =
                new Run()
                {
                    RacePlanId = model.RacePlanId,
                    PlannedDistance = model.PlannedDistance,
                    EstimatedTime = model.EstimatedTime,
                    ScheduledDateTime = model.ScheduleDateTime,
                    Description = model.Description,
                    LocationId = model.LocationId
                };
            _context.Runs.Add(entity);
            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<List<RunListItem>> GetRunsAsync()
        {
            var entityList = await _context.Runs.ToListAsync();
            var runList = entityList
                .Where(e => e.RacePlan.UserId == _userId.ToString())
                .Select(
                    e =>
                        new RunListItem
                        {
                            Id = e.Id,
                            RacePlanName = e.RacePlan.RaceName,
                            PlannedDistance = e.PlannedDistance,
                            EstimatedTime = e.EstimatedTime,
                            LocationId = e.LocationId,
                            ScheduledDateTime = e.ScheduledDateTime,
                            ActualDistance = e.ActualDistance,
                            ActualTime = e.ActualTime
                        }
                );
            return runList.ToList();
        }

        public async Task<RunDetail> GetRunByIdAsync(int id)
        {
            var entity = await _context.Runs.FindAsync(id);
            if (entity == null)
                return null;
            var model = new RunDetail()
            {
                Id = entity.Id,
                RacePlanId = entity.RacePlanId,
                RacePlanName = entity.RacePlan.RaceName,
                PlannedDistance = entity.PlannedDistance,
                EstimatedTime = entity.EstimatedTime,
                ScheduleDateTime = entity.ScheduledDateTime,
                LocationId = entity.LocationId,
                ActualDistance = entity.ActualDistance,
                ActualTime = entity.ActualTime,
                Description = entity.Description
            };
            return model;
        }

        public async Task<List<RunDetail>> GetRunsInPlanAsync(int id)
        {
            var entityList = await _context.Runs.ToListAsync();
            var runList = entityList
                .Where(e => e.RacePlan.UserId == _userId.ToString() && e.RacePlanId == id)
                .Select(
                    e =>
                    new RunDetail()
                    {
                        Id = e.Id,
                        RacePlanId = e.RacePlanId,
                        RacePlanName = e.RacePlan.RaceName,
                        PlannedDistance = e.PlannedDistance,
                        EstimatedTime = e.EstimatedTime,
                        ScheduleDateTime = e.ScheduledDateTime,
                        LocationId = e.LocationId,
                        ActualDistance = e.ActualDistance,
                        ActualTime = e.ActualTime,
                        Description = e.Description
                    });
            return runList.ToList();
        }

        public async Task<WeeklyRunForecastList> GetRunsWithForecastAsync()
        {
            var forecastService = CreateForecastService();
            var model = new WeeklyRunForecastList()
            {
                WeeklyForecast = await forecastService.GetForecastAsync(),
            };
            var entityList = await _context.Runs.ToListAsync();
            var runList = entityList
                .Where(e => e.RacePlan.UserId == _userId.ToString() && DateTime.Compare(e.ScheduledDateTime, DateTime.Now.AddDays(7)) <= 0)
                .Select(
                    e =>
                    new RunDetail()
                    {
                        Id = e.Id,
                        RacePlanId = e.RacePlanId,
                        RacePlanName = e.RacePlan.RaceName,
                        PlannedDistance = e.PlannedDistance,
                        EstimatedTime = e.EstimatedTime,
                        ScheduleDateTime = e.ScheduledDateTime,
                        LocationId = e.LocationId,
                        ActualDistance = e.ActualDistance,
                        ActualTime = e.ActualTime,
                        Description = e.Description
                    });
            model.WeeklyRuns = runList.ToList();
            return model;

        }




        public async Task<bool> EditRunAsync(RunEdit model)
        {
            var entity = await
                _context
                    .Runs
                    .SingleOrDefaultAsync(e => e.Id == model.Id && e.RacePlan.UserId == _userId.ToString());
            entity.RacePlanId = model.RacePlanId;
            entity.PlannedDistance = model.PlannedDistance;
            entity.EstimatedTime = model.EstimatedTime;
            entity.ScheduledDateTime = DateTime.Parse(model.ScheduleDateTime);
            //entity.ScheduledDateTime = DateTime.ParseExact(model.ScheduleDateTime, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
            entity.LocationId = model.LocationId;
            entity.ActualDistance = model.ActualDistance;
            entity.ActualTime = model.ActualTime;
            entity.Description = model.Description;
            return await _context.SaveChangesAsync() == 1;

        }

        public async Task<bool> DeleteRunAsync(int id)
        {
            var entity = await
                _context
                    .Runs
                    .SingleOrDefaultAsync(e => e.Id == id && e.RacePlan.UserId == _userId.ToString());
            _context.Runs.Remove(entity);
            return await _context.SaveChangesAsync() == 1;

        }



        private ForecastService CreateForecastService()
        {
            var service = new ForecastService(_userId, _zipCode);
            return service;
        }

    }
}
