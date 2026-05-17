# World Cup Assertions Kata - C#

A small .NET 10 / NUnit kata about improving test assertions.

The tests intentionally start with familiar NUnit 3 classic assertions. The learning goal is to refactor them toward the modern NUnit constraint style used and emphasized in NUnit 4.

The production code models a tiny World Cup domain:
- `Team`
- `Player`
- `Game`
- `Location`


The tests pass, but many assertions are intentionally poor. They hide the real expectation inside vague boolean checks, generic counts, weak null checks, or indirect predicates.

Tests are deliberately shaped around common bad assertion smells:

- too vague: `Assert.IsTrue(result.Count > 0)`
- too generic: `Assert.AreEqual(3, items.Length)`
- too indirect: `Assert.IsTrue(names.Any(name => name.StartsWith("A")))`
- almost meaningless: `Assert.NotNull(players.Where(...))`


## Goal

Learn the modern NUnit 4 assertion style by refactoring classic NUnit 3 assertions into constraint-based assertions that make failures explain the business rule clearly.

In this kata, a **business rule** is a rule from the football domain, such as "a team cannot have duplicate shirt numbers" or "a team that did not play should not receive points".

The aim is not to change the production code. The aim is to make the tests communicate better.

## What you will practice

You will take assertions that technically pass, but fail badly when the code is broken.

A weak assertion often says only that something was false:

```csharp
Assert.IsTrue(players.Count > 0);
```

A better assertion says what you expected:

```csharp
Assert.That(players, Is.Not.Empty);
```

Another weak assertion hides the expectation inside a predicate:

```csharp
Assert.IsTrue(germany.Players.Any(player => player.Name == "Manuel Neuer"));
```

A clearer assertion names the collection expectation directly:

```csharp
Assert.That(
    germany.Players.Select(player => player.Name),
    Does.Contain("Manuel Neuer"));
```

Good improvements might include:

- replacing `Assert.IsTrue(...)` with `Assert.That(..., Is...)` or `Assert.That(..., Has...)`
- replacing `Assert.AreEqual(...)` with `Assert.That(actual, Is.EqualTo(expected))`
- asserting directly on collections instead of hiding expectations inside `Any`, `Where`, or `All`
- using better exception assertions with `Assert.That(action, Throws.TypeOf<...>())`
- extracting custom assertion helpers such as `ShouldHaveForwards`, `ShouldContainHostNations`, or `ShouldRejectDuplicateShirtNumbers`
- making test data builders clearer

You are done when the tests still pass normally, but when a production-code mutation is applied, the failing test message clearly explains the broken World Cup rule.


## Run the kata

```bash
dotnet test
```

## Break the production code

The `scripts` folder contains small, reversible production-code mutations. A **mutation** is an intentional bug that changes the production code so you can see how well the tests fail.

The best way to use this kata is to apply one mutation at a time:

```bash
scripts/break-production.sh 1
dotnet test
scripts/reset-production.sh 1
```

Then improve the assertions until the failure messages explain the broken rule clearly.

You can also apply all mutations at once:

```bash
scripts/break-production.sh
dotnet test
```

On Windows, use the `.bat` scripts instead:

```bat
scripts\break-production.bat 1
dotnet test
scripts\reset-production.bat 1
```

Available mutations:

1. Allow duplicate shirt numbers.
2. Add Manuel Neuer to Germany.
3. Remove the United States goalkeeper.
4. Invert winner and loser points.
5. Allow points for a team that did not play.
