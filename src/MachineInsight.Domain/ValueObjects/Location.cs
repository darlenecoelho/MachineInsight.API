namespace MachineInsight.Domain.ValueObjects;

public class Location
{
    private Location() { }

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
