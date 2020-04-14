using PlanMyRun.Data;
using PlanMyRun.Models.RacePlanModels;
using PlanMyRun.Models.RunModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Services
{
    public class RacePlanService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _context;
        private readonly string _zipCode;
        private readonly TimeSpan _userPace;
        public RacePlanService(Guid userId, string zipCode, TimeSpan userPace)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
            _zipCode = zipCode;
            _userPace = userPace;
        }

        public async Task<bool> CreateRacePlanAsync(RacePlanCreate model)
        {
            var entity =
                new RacePlan()
                {
                    UserId = _userId.ToString(),
                    RaceName = model.RaceName,
                    RaceDate = model.RaceDate,
                    IsPublic = model.IsPublic,
                    RaceLength = model.RaceLength,
                    GoalTime = model.GoalTime,
                    Description = model.Description
                };

            _context.RacePlans.Add(entity);
            return await _context.SaveChangesAsync() == 1;

        }

        public async Task<List<RacePlanListItem>> GetRacePlansAsync()
        {
            var entityList = await _context.RacePlans.ToListAsync();
            var racePlanList = entityList
                        .Where(e => e.UserId == _userId.ToString())
                        .Select(
                            e =>
                                new RacePlanListItem
                                {
                                    Id = e.Id,
                                    RaceName = e.RaceName,
                                    RaceLength = e.RaceLength,
                                    GoalTime = e.GoalTime,
                                    RaceDate = e.RaceDate,
                                    IsPublic = e.IsPublic,
                                    NumberOfRuns = e.ListOfRuns.Count
                                }
                        ).ToList();
            return racePlanList;

        }
        public async Task<List<RacePlanListItem>> GetTemplates()
        {
            var entityList = await _context.RacePlans.ToListAsync();
            var racePlanList = entityList
                        .Where(e => e.IsPublic ==true)
                        .Select(
                            e =>
                                new RacePlanListItem
                                {
                                    Id = e.Id,
                                    RaceName = e.RaceName,
                                    RaceLength = e.RaceLength,
                                    GoalTime = e.GoalTime,
                                    RaceDate = e.RaceDate,
                                    IsPublic = e.IsPublic,
                                    NumberOfRuns = e.ListOfRuns.Count
                                }
                        ).ToList();
            return racePlanList;
        }

        public async Task<bool> CreateFromTemplateAsync(RacePlanTemplate model)
        {
            var existingRace = await _context.RacePlans.FindAsync(model.ExistingPlanId);
            var entity =
                new RacePlan()
                {
                    UserId = _userId.ToString(),
                    RaceName = model.RaceName,
                    RaceDate = model.RaceDate,
                    IsPublic = model.IsPublic,
                    RaceLength = existingRace.RaceLength,
                    GoalTime = model.GoalTime,
                    Description = model.Description
                };

            _context.RacePlans.Add(entity);
           var savedChanges = await _context.SaveChangesAsync()==1;
            var newId = entity.Id;
            var runService = CreateRunService();
            List<RunCreate> templateRuns = new List<RunCreate>();
            foreach (var item in existingRace.ListOfRuns)
            {
                var runCreate = new RunCreate()
                {
                    RacePlanId = entity.Id,
                    PlannedDistance = item.PlannedDistance,
                    ScheduleDateTime = entity.RaceDate-(existingRace.RaceDate-item.ScheduledDateTime),
                    Description = item.Description,
                    LocationId = item.LocationId
                };
                var savedRun = await runService.CreateRunAsync(runCreate);
                if (savedRun)
                    continue;
                else
                {
                    return false;
                }
            }

            return savedChanges;
        }

        public async Task<RacePlanDetail> GetPlanByIdAsync(int id)
        {
            var entity = await _context.RacePlans.FindAsync(id);
            if (entity == null)
                return null;

            var model = new RacePlanDetail()
            {
                Id = entity.Id,
                RaceName = entity.RaceName,
                RaceDate = entity.RaceDate,
                IsPublic = entity.IsPublic,
                RaceLength = entity.RaceLength,
                GoalTime = entity.GoalTime,
                Description = entity.Description,
                ListOfRuns = entity.ListOfRuns.Select(run => new RunListItem
                {
                    Id=run.Id,
                    RacePlanName = entity.RaceName,
                    PlannedDistance=run.PlannedDistance,
                    EstimatedTime=run.EstimatedTime,
                    ScheduledDateTime=run.ScheduledDateTime,
                    ActualDistance=run.ActualDistance,
                    ActualTime=run.ActualTime
                }).ToList()
            };
            return model;
        }

        public async Task<bool> EditRacePlanAsync(RacePlanEdit model)
        {
            var entity = await
                _context
                    .RacePlans
                    .SingleOrDefaultAsync(e => e.Id == model.Id && e.UserId == _userId.ToString());
            entity.RaceName = model.RaceName;
            entity.RaceDate = model.RaceDate;
            entity.IsPublic = model.IsPublic;
            entity.RaceLength = model.RaceLength;
            entity.GoalTime = model.GoalTime;
            entity.Description = model.Description;

            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<bool> DeletePlanAsync(int id)
        {
            var entity =
                _context
                    .RacePlans
                    .Single(e => e.Id == id && e.UserId == _userId.ToString());
            _context.RacePlans.Remove(entity);
            return await _context.SaveChangesAsync() == 1;
        }

        private RunService CreateRunService()
        {
            var service = new RunService(_userId, _zipCode,_userPace);
            return service;
        }
    }
}
