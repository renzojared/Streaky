using System.ComponentModel.DataAnnotations;

namespace Streaky.Movies.DTOs;

public class MovieTheaterNearbyFilterDTO
{
    [Range(-90, 90)]
    public double Latitude { get; set; }
    [Range(-180, 180)]
    public double Length { get; set; }
    private int distanceInKms = 10;
    private int distanceMaxKms = 50;
    public int DistanceinKms { get { return distanceInKms; } set { distanceInKms = (value > distanceMaxKms) ? distanceMaxKms : value; } }
}

