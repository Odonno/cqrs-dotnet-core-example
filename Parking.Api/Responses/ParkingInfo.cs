namespace Parking.Api.Responses
{
    public class ParkingInfo
    {
        public string Name { get; set; }
        public bool IsOpened { get; set; }
        public int MaximumPlaces { get; set; }
        public int AvailablePlaces { get; set; }
    }
}
