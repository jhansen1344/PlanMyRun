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
using System.Web.Mvc;

namespace PlanMyRun.Services
{
    public class RunService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _context;
        private readonly string _zipCode;
        private readonly TimeSpan _userPace;

        public RunService(Guid userId, string zipCode, TimeSpan userPace)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
            _zipCode = zipCode;
            _userPace = userPace;
        }

        public async Task<bool> CreateRunAsync(RunCreate model)
        {
            var entity =
                new Run()
                {
                    RacePlanId = model.RacePlanId,
                    PlannedDistance = model.PlannedDistance,
                    EstimatedTime = TimeSpan.FromMinutes(_userPace.TotalMinutes*model.PlannedDistance),
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
                ActualDistance = entity.ActualDistance,
                ActualTime = entity.ActualTime,
                Description = entity.Description
            };
            if(entity.Location!=null)
            {
                model.LocationName = entity.Location.Name;
            }
            else
            {
                model.LocationName = "";
            }
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
                        End = e.ScheduledDateTime.AddHours(e.EstimatedTime.TotalHours),
 
                        ActualDistance = e.ActualDistance,
                        ActualTime = e.ActualTime,
                        Description = e.Description
                    });
            return runList.ToList();
        }

        public async Task<List<RunDetail>> GetUnscheduledRunsAsync()
        {
            var entityList = await _context.Runs.ToListAsync();
            var runList = entityList
                .Where(e => e.RacePlan.UserId == _userId.ToString() && DateTime.Compare(e.ScheduledDateTime.Date,DateTime.Now.AddDays(10))<1)
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
                        End = e.ScheduledDateTime.AddHours(e.EstimatedTime.TotalHours),
                        LocationId = e.LocationId,
                        ActualDistance = e.ActualDistance,
                        ActualTime = e.ActualTime,
                        Description = e.Description
                    });
            return runList.ToList();
        }
        public async Task<List<DailyRunForecast>> GetRunsWithForecastAsync()
        {
            var forecastService = CreateForecastService();
            var weeklyForecast = await forecastService.GetForecastAsync();
            var entityList = await _context.Runs.ToListAsync();
            List<DailyRunForecast> dailyRunForecasts = new List<DailyRunForecast>();
            foreach (var item in weeklyForecast.Days)
            {
                var model = new DailyRunForecast()
                {
                    Date = item.Date,
                    DaysForecast = item
                };
                var run = entityList.SingleOrDefault(e => e.RacePlan.UserId == _userId.ToString() && e.ScheduledDateTime.Date== item.Date.Date);
                if(run!=null)
                {
                    model.DaysRun = new RunDetail()
                    {
                        Id = run.Id,
                        RacePlanId = run.RacePlanId,
                        RacePlanName = run.RacePlan.RaceName,
                        PlannedDistance = run.PlannedDistance,
                        EstimatedTime = run.EstimatedTime,
                        ScheduleDateTime = run.ScheduledDateTime,
                        ActualDistance = run.ActualDistance,
                        ActualTime = run.ActualTime,
                        Description = run.Description
                    };
                    if (run.Location != null)
                    { model.DaysRun.LocationName = run.Location.Name; }
                    else { model.DaysRun.LocationName = ""; }
                }
                    
                dailyRunForecasts.Add(model);
            }

            return dailyRunForecasts;

        }

        public async Task<bool> EditRunAsync(RunEdit model)
        {
            var entity = await
                _context
                    .Runs
                    .SingleOrDefaultAsync(e => e.Id == model.Id && e.RacePlan.UserId == _userId.ToString());
            entity.RacePlanId = model.RacePlanId;
            entity.PlannedDistance = model.PlannedDistance;
            entity.EstimatedTime = TimeSpan.FromMinutes(_userPace.TotalMinutes * model.PlannedDistance);
            entity.ScheduledDateTime = DateTime.Parse(model.ScheduleDateTime);
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

        public async Task<RunCreate> GetCreateSelectListAsync()
        {
            var runCreate = new RunCreate()
            {
                RacePlan = new SelectList(await _context.RacePlans.Where(e => e.UserId == _userId.ToString()).ToListAsync(), "Id", "RaceName"),
                Locations = new SelectList(await _context.Locations.Where(e => e.UserId == _userId.ToString()).ToListAsync(), "Id", "Name")
            };
            return runCreate;
        }

        private ForecastService CreateForecastService()
        {
            var service = new ForecastService(_userId, _zipCode);
            return service;
        }

    }
}
