﻿using PlanMyRun.Data;
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

        public RunService(Guid userId, string zipCode)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
            _zipCode = zipCode;
        }

        public async Task<bool> CreateRunAsync(RunCreate model)
        {
            var entity =
                new Run()
                {
                    RacePlanId = model.RacePlanId,
                    PlannedDistance = model.PlannedDistance,
                    EstimatedTime = model.EstimatedTime,
                    ScheduledDateTime = model.ScheduledDate,
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
            entity.ScheduledDateTime = DateTime.ParseExact(model.ScheduleDateTime, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
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
