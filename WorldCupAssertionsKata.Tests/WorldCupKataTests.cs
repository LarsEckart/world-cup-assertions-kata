using WorldCupAssertionsKata;

namespace WorldCupAssertionsKata.Tests;

public sealed class WorldCupKataTests
{
    [Test]
    public void Duplicate_shirt_numbers_Team_creation_is_rejected()
    {
        var players = new[]
        {
            new Player("Miroslav Klose", 11, Position.Forward),
            new Player("Thomas Muller", 11, Position.Forward)
        };

        var exception = Assert.Catch(() => new Team("Germany", "Die Mannschaft", players));

        Assert.IsNotNull(exception);
    }

    [Test]
    public void Germany_squad_Does_not_include_Manuel_Neuer()
    {
        var germany = Germany();

        Assert.IsTrue(germany.PlayerNamed("Manuel Neuer") == null);
    }

    [Test]
    public void United_States_squad_Has_goalkeeper()
    {
        var goalies = UnitedStates().Goalie();

        Assert.IsTrue(goalies.Count > 0);
    }

    [Test]
    public void Winning_team_Gets_three_points()
    {
        var game = WorldCup2026Game(homeGoals: 2, awayGoals: 0);

        Assert.IsTrue(game.PointsFor(game.HomeTeam) == 3);
    }

    [Test]
    public void Team_that_did_not_play_Cannot_receive_points()
    {
        var game = WorldCup2026Game(homeGoals: 1, awayGoals: 0);
        var canada = Canada();

        var exception = Assert.Catch(() => game.PointsFor(canada));

        Assert.IsTrue(exception is ArgumentException);
    }

    private static Game WorldCup2026Game(int? homeGoals = null, int? awayGoals = null) =>
        new(
            UnitedStates(),
            Germany(),
            HostLocation(),
            new DateTimeOffset(2026, 6, 11, 20, 00, 00, TimeSpan.Zero),
            homeGoals,
            awayGoals);

    private static Location HostLocation() =>
        new("BMO Field", "Toronto", "Canada", capacity: 30000);

    private static Team UnitedStates() =>
        WorldCup.Create2026().Teams().Single(team => team.Country == "United States");

    private static Team Germany() =>
        WorldCup.Create2026().Teams().Single(team => team.Country == "Germany");

    private static Team Canada() =>
        WorldCup.Create2026().Teams().Single(team => team.Country == "Canada");
}
