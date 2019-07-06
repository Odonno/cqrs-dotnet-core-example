using Microsoft.EntityFrameworkCore;
using Parking.Api.Models;
using Parking.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parking.Api.Queries.Handlers
{
    public class ParkingQueryHandler
    {
        private readonly DbContext _dbContext;

        public ParkingQueryHandler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ParkingInfo> Handle(GetAllParkingInfoQuery _)
        {
            var parkings = _dbContext.Set<Models.Parking>()
                .Include(p => p.Places)
                .ToList();

            return parkings.Select(p =>
            {
                return new ParkingInfo
                {
                    Name = p.Name,
                    IsOpened = p.IsOpened,
                    MaximumPlaces = p.Places.Count,
                    AvailablePlaces =
                        p.IsOpened
                            ? p.Places.Where(pp => pp.IsFree).Count()
                            : 0
                };
            });
        }

        public ParkingInfo Handle(GetParkingInfoQuery query)
        {
            var parking = _dbContext.Set<Models.Parking>()
                .Include(p => p.Places)
                .FirstOrDefault(p => p.Name == query.ParkingName);

            if (parking == null)
            {
                throw new Exception($"Cannot find parking '{query.ParkingName}'.");
            }

            return new ParkingInfo
            {
                Name = parking.Name,
                IsOpened = parking.IsOpened,
                MaximumPlaces = parking.Places.Count,
                AvailablePlaces = 
                    parking.IsOpened 
                        ? parking.Places.Where(pp => pp.IsFree).Count()
                        : 0
            };
        }

        public ParkingPlaceInfo Handle(GetRandomAvailablePlace _)
        {
            var random = new Random();

            var parkingPlace = _dbContext.Set<ParkingPlace>()
                .Include(p => p.Parking)
                .Where(p => p.Parking.IsOpened && p.IsFree)
                .OrderBy(p => random.Next())
                .FirstOrDefault();

            return new ParkingPlaceInfo
            {
                ParkingName = parkingPlace.ParkingName,
                Number = parkingPlace.Number
            };
        }

        public int Handle(GetTotalAvailablePlacesQuery _)
        {
            return _dbContext.Set<ParkingPlace>()
                .Where(p => p.Parking.IsOpened && p.IsFree)
                .Count();
        }
    }
}
