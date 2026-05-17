namespace WorldCupAssertionsKata;

public sealed class Game
{
    public Game(
        Team homeTeam,
        Team awayTeam,
        Location location,
        DateTimeOffset kickOff,
        int? homeGoals = null,
        int? awayGoals = null)
    {
        HomeTeam = homeTeam ?? throw new ArgumentNullException(nameof(homeTeam));
        AwayTeam = awayTeam ?? throw new ArgumentNullException(nameof(awayTeam));
        Location = location ?? throw new ArgumentNullException(nameof(location));
        KickOff = kickOff;
        HomeGoals = homeGoals;
        AwayGoals = awayGoals;

        if (homeTeam.Country == awayTeam.Country)
        {
            throw new ArgumentException("A game needs two different teams.", nameof(awayTeam));
        }

        if (homeGoals is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(homeGoals), homeGoals, "Goals cannot be negative.");
        }

        if (awayGoals is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(awayGoals), awayGoals, "Goals cannot be negative.");
        }
    }

    public Team HomeTeam { get; }

    public Team AwayTeam { get; }

    public Location Location { get; }

    public DateTimeOffset KickOff { get; }

    public int? HomeGoals { get; }

    public int? AwayGoals { get; }

    public bool HasBeenPlayed => HomeGoals.HasValue && AwayGoals.HasValue;

    public bool IsDraw => HasBeenPlayed && HomeGoals == AwayGoals;

    public bool IsNeutralSite() => Location.Country != HomeTeam.Country && Location.Country != AwayTeam.Country;

    public string FixtureName() => $"{HomeTeam.Country} vs {AwayTeam.Country} at {Location.Stadium}";

    public string Scoreline()
    {
        EnsureResult();
        return $"{HomeTeam.Country} {HomeGoals} - {AwayGoals} {AwayTeam.Country}";
    }

    public Team? Winner()
    {
        EnsureResult();

        if (HomeGoals == AwayGoals)
        {
            return null;
        }

        return HomeGoals > AwayGoals ? HomeTeam : AwayTeam;
    }

    public int PointsFor(Team team)
    {
        ArgumentNullException.ThrowIfNull(team);
        EnsureResult();

        if (team != HomeTeam && team != AwayTeam)
        {
            throw new ArgumentException("Team did not play in this game.", nameof(team));
        }

        if (IsDraw)
        {
            return 1;
        }

        return Winner() == team ? 3 : 0;
    }

    private void EnsureResult()
    {
        if (!HasBeenPlayed)
        {
            throw new InvalidOperationException("The game has not been played yet.");
        }
    }
}
