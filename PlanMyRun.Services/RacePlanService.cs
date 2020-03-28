using PlanMyRun.Data;
using PlanMyRun.Models.RacePlanModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Services
{
    public class RacePlanService
    {
        private readonly Guid _userId;
        public RacePlanService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateRacePlan(RacePlanCreate model)
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

            using (var ctx = new ApplicationDbContext())
            {
                ctx.RacePlans.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<RacePlanListItem> GetRacePlans()
        {
            using(var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .RacePlans
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
                        );
                return query.ToList();
            }
        }
    }
}
