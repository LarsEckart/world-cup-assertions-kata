namespace WorldCupAssertionsKata;

public sealed record Player
{
    public Player(string name, int shirtNumber, Position position)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A player needs a name.", nameof(name));
        }

        if (shirtNumber is < 1 or > 99)
        {
            throw new ArgumentOutOfRangeException(nameof(shirtNumber), shirtNumber, "Shirt numbers must be between 1 and 99.");
        }

        Name = name.Trim();
        ShirtNumber = shirtNumber;
        Position = position;
    }

    public string Name { get; }

    public int ShirtNumber { get; }

    public Position Position { get; }

    public string MatchdayCard() => $"#{ShirtNumber} {Name} ({Position})";
}
