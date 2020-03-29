using PlanMyRun.Data;
using PlanMyRun.Models.LocationModels;
using PlanMyRun.Models.RunModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanMyRun.Services
{
    public class LocationService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _context;
        public LocationService(Guid userId)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
        }

        public async Task<bool> CreateLocationAsync(LocationCreate model)
        {
            var entity =
                new Location()
                {
                  Name=model.Name,
                  MaxDistance=model.MaxDistance,
                  HasLoops=model.HasLoops,
                  Address=model.Address,
                  PathType=model.PathType
                };
            _context.Locations.Add(entity);
            return await _context.SaveChangesAsync() == 1;
        }
        public async Task<List<LocationListItem>> GetLocationssAsync()
        {
            var entityList = await _context.Locations.ToListAsync();
            var runList = entityList
                .Where(e => e.UserId == _userId.ToString())
                .Select(
                    e =>
                        new LocationListItem
                        {
                            Id=e.Id,
                            Name=e.Name,
                            MaxDistance=e.MaxDistance,
                            HasLoops=e.HasLoops
                        }
                );
            return runList.ToList();
        }

        public async Task<LocationDetail> GetLocationByIdAsync(int id)
        {
            var entity = await _context.Locations.FindAsync(id);
            if (entity == null)
                return null;
            var model = new LocationDetail()
            {
                Id = entity.Id,
                Name=entity.Name,
                MaxDistance=entity.MaxDistance,
                HasLoops=entity.HasLoops,
                Address=entity.Address,
                PathType=entity.PathType,
                RunsAtLocation = entity.RunsAtLocation.Select(run => new RunListItem
                {
                    Id = run.Id,
                    RacePlanName = run.RacePlan.RaceName,
                    PlannedDistance = run.PlannedDistance,
                    EstimatedTime = run.EstimatedTime,
                    ScheduledDateTime = run.ScheduledDateTime,
                    LocationId = run.LocationId,
                    ActualDistance = run.ActualDistance,
                    ActualTime = run.ActualTime
                }).ToList()
            };
            return model;
        }

        public async Task<bool> EditLocationAsync(LocationEdit model)
        {
            var entity = await
                _context
                    .Locations
                    .SingleOrDefaultAsync(e => e.Id == model.Id && e.UserId == _userId.ToString());
            entity.Name = model.Name;
            entity.MaxDistance = model.MaxDistance;
            entity.HasLoops = model.HasLoops;
            entity.Address = model.Address;
            entity.PathType = model.PathType;

            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var entity = await
                _context
                    .Locations
                    .SingleOrDefaultAsync(e => e.Id == id && e.UserId == _userId.ToString());
            _context.Locations.Remove(entity);
            return await _context.SaveChangesAsync() == 1;

        }
    }
}
