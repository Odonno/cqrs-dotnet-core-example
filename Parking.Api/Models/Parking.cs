using System.Collections.Generic;

namespace Parking.Api.Models
{
    public class Parking
    {
        public string Name { get; set; }
        public bool IsOpened { get; set; }

        public List<ParkingPlace> Places { get; set; }
    }
}
