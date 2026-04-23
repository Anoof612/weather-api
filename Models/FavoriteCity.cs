namespace WeatherAPI.Models;

public class FavoriteCity
{
    public int Id { get; set; }
    public string CityName { get; set; } = string.Empty;
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
}