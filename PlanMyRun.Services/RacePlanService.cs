using PlanMyRun.Data;
using PlanMyRun.Models.RacePlanModels;
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
        public RacePlanService(Guid userId)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
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
    }
}
