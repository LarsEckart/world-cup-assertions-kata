namespace WorldCupAssertionsKata;

public sealed class Team
{
    public Team(string country, string nickname, IEnumerable<Player> players)
    {
        Country = string.IsNullOrWhiteSpace(country)
            ? throw new ArgumentException("A team needs a country.", nameof(country))
            : country.Trim();

        Nickname = string.IsNullOrWhiteSpace(nickname)
            ? throw new ArgumentException("A team needs a nickname.", nameof(nickname))
            : nickname.Trim();

        Players = players?.ToList().AsReadOnly()
            ?? throw new ArgumentNullException(nameof(players));

        if (Players.Count == 0)
        {
            throw new ArgumentException("A team needs at least one player.", nameof(players));
        }

        var duplicateShirtNumbers = Players
            .GroupBy(player => player.ShirtNumber)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToArray();

        if (duplicateShirtNumbers.Length > 0)
        {
            throw new ArgumentException("A team cannot have duplicate shirt numbers.", nameof(players));
        }
    }

    public string Country { get; }

    public string Nickname { get; }

    public IReadOnlyList<Player> Players { get; }

    public Player Captain() => Players.Single(player => player.ShirtNumber == 10);

    public IReadOnlyList<Player> Goalie() => Players.Where(player => player.Position == Position.Goalkeeper).ToArray();

    public IReadOnlyList<Player> Forwards() => Players.Where(player => player.Position == Position.Forward).ToArray();

    public Player? PlayerNamed(string name) => Players.SingleOrDefault(player => player.Name == name);

    public Player? PlayerWearing(int shirtNumber) => Players.SingleOrDefault(player => player.ShirtNumber == shirtNumber);

    public IReadOnlyList<Player> StartingEleven() => Players.Take(11).ToArray();

    public override string ToString() => $"{Country} ({Nickname})";
}
