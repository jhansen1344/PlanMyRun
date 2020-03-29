using PlanMyRun.Data;
using PlanMyRun.Models.RunModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Services
{
    public class RunService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _context;
        public RunService(Guid userId)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
        }

        public async Task<bool> CreateRunAsync(RunCreate model)
        {
            var entity =
                new Run()
                {
                    RacePlanId=model.RacePlanId,
                    PlannedDistance=model.PlannedDistance,
                    EstimatedTime=model.EstimatedTime,
                    ScheduledDateTime=model.ScheduledDate,
                    Description=model.Description,
                    LocationId=model.LocationId
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
                    run =>
                        new RunListItem
                        {
                            Id = run.Id,
                            RacePlanName = run.RacePlan.RaceName,
                            PlannedDistance = run.PlannedDistance,
                            EstimatedTime = run.EstimatedTime,
                            ScheduledDateTime = run.ScheduledDateTime,
                            Location = run.Location.Name,
                            ActualDistance = run.ActualDistance,
                            ActualTime = run.ActualTime
                        }
                ).ToList();
            return runList;
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
                LocationName = entity.Location.Name,
                ActualDistance = entity.ActualDistance,
                ActualTime = entity.ActualTime
            };
            return model;
        }

        public async Task<bool> EditRunAsync(RunEdit model)
        {
            var entity = await
                _context
                    .Runs
                    .SingleOrDefaultAsync(e => e.Id==model.Id && e.RacePlan.UserId == _userId.ToString());
            entity.RacePlanId = model.RacePlanId;
            entity.PlannedDistance = model.PlannedDistance;
            entity.EstimatedTime = model.EstimatedTime;
            entity.ScheduledDateTime = model.ScheduleDateTime;
            entity.LocationId = model.LocationId;
            entity.ActualDistance = model.ActualDistance;
            entity.ActualTime = model.ActualTime;

            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<bool> DeletePlanAsync(int id)
        {
            var entity = await
                _context
                    .Runs
                    .SingleOrDefaultAsync(e => e.Id == id && e.RacePlan.UserId == _userId.ToString());
            _context.Runs.Remove(entity);
            return await _context.SaveChangesAsync() == 1;

        }
    }
}
