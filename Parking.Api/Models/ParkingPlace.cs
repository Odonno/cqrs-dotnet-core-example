namespace Parking.Api.Models
{
    public class ParkingPlace
    {
        public string ParkingName { get; set; }
        public int Number { get; set; }

        public bool IsFree { get; set; }
        public string UserId { get; set; }

        public Parking Parking { get; set; }
    }
}
