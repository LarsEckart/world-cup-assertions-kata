namespace WorldCupAssertionsKata;

public sealed class WorldCup
{
    private readonly IReadOnlyList<Team> hosts;
    private readonly IReadOnlyList<Team> teams;

    public WorldCup(int year, IEnumerable<Team> hosts, IEnumerable<Team> teams)
    {
        Year = year;
        this.hosts = hosts?.ToList().AsReadOnly()
            ?? throw new ArgumentNullException(nameof(hosts));
        this.teams = teams?.ToList().AsReadOnly()
            ?? throw new ArgumentNullException(nameof(teams));

        if (this.hosts.Count == 0)
        {
            throw new ArgumentException("A World Cup needs at least one host.", nameof(hosts));
        }

        if (this.teams.Count == 0)
        {
            throw new ArgumentException("A World Cup needs at least one team.", nameof(teams));
        }
    }

    public int Year { get; }

    public static WorldCup Create2026() =>
        new(
            year: 2026,
            hosts: new[]
            {
                UnitedStates(),
                Mexico(),
                Canada()
            },
            teams: new[]
            {
                UnitedStates(),
                Mexico(),
                Canada(),
                Germany()
            });

    public IReadOnlyList<Team> Hosts() => hosts;

    public IReadOnlyList<Team> Teams() => teams;

    private static Team UnitedStates() =>
        new(
            "United States",
            "USMNT",
            new[]
            {
                new Player("Matt Turner", 1, Position.Goalkeeper),
                new Player("Antonee Robinson", 5, Position.Defender),
                new Player("Tim Ream", 13, Position.Defender),
                new Player("Chris Richards", 3, Position.Defender),
                new Player("Joe Scally", 22, Position.Defender),
                new Player("Tyler Adams", 4, Position.Midfielder),
                new Player("Weston McKennie", 8, Position.Midfielder),
                new Player("Giovanni Reyna", 7, Position.Midfielder),
                new Player("Timothy Weah", 21, Position.Forward),
                new Player("Folarin Balogun", 20, Position.Forward),
                new Player("Christian Pulisic", 10, Position.Forward)
            });

    private static Team Germany() =>
        new(
            "Germany",
            "Die Mannschaft",
            new[]
            {
                new Player("Oliver Baumann", 1, Position.Goalkeeper),
                new Player("Jonathan Tah", 4, Position.Defender),
                new Player("Joshua Kimmich", 6, Position.Midfielder),
                new Player("Florian Wirtz", 17, Position.Midfielder),
                new Player("Leroy Sane", 19, Position.Forward),
                new Player("Nick Woltemade", 11, Position.Forward)
            });

    private static Team Mexico() =>
        new(
            "Mexico",
            "El Tri",
            new[]
            {
                new Player("Guillermo Ochoa", 1, Position.Goalkeeper),
                new Player("Edson Alvarez", 4, Position.Midfielder),
                new Player("Alexis Vega", 10, Position.Forward)
            });

    private static Team Canada() =>
        new(
            "Canada",
            "The Reds",
            new[]
            {
                new Player("Maxime Crepeau", 16, Position.Goalkeeper),
                new Player("Derek Cornelius", 13, Position.Defender),
                new Player("Ismael Kone", 8, Position.Midfielder),
                new Player("Jonathan David", 10, Position.Forward)
            });
}
