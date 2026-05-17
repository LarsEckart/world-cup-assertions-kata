namespace WorldCupAssertionsKata;

public sealed record Location
{
    public Location(string stadium, string city, string country, int capacity)
    {
        if (string.IsNullOrWhiteSpace(stadium))
        {
            throw new ArgumentException("A location needs a stadium.", nameof(stadium));
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("A location needs a city.", nameof(city));
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("A location needs a country.", nameof(country));
        }

        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity must be positive.");
        }

        Stadium = stadium.Trim();
        City = city.Trim();
        Country = country.Trim();
        Capacity = capacity;
    }

    public string Stadium { get; }

    public string City { get; }

    public string Country { get; }

    public int Capacity { get; }

    public string DisplayName() => $"{Stadium}, {City}, {Country}";
}
