using System.Collections.Immutable;
using Frognar.Monads.Writers;

namespace Monads.Tests.Unit;

public class WriterTests {
  [Fact]
  public void Wrap_ShouldReturnWriterWithGivenValueAndLogs() {
    const string value = "Test";
    ImmutableList<string> logs = ImmutableList.Create("Log1", "Log2");

    Writer<string, string> result = Writer<string, string>.Wrap(value, logs);

    Assert.Equal(value, result.Value);
    Assert.Equal(logs, result.Logs);
  }

  [Fact]
  public void Map_ShouldApplyFunctionToValueAndKeepLogs() {
    Writer<int, string> initial = Writer<int, string>.Wrap(2, ImmutableList.Create("Log1"));
    string Map(int x) => (x * 2).ToString();

    Writer<string, string> result = initial.Map(Map);

    Assert.Equal("4", result.Value);
    Assert.Equal(initial.Logs, result.Logs);
  }

  [Fact]
  public async Task MapAsync_ShouldApplyAsyncFunctionToValueAndKeepLogs() {
    Writer<int, string> initial = Writer<int, string>.Wrap(2, ImmutableList.Create("Log1"));
    async Task<string> Map(int x) => (await Task.FromResult(x * 2)).ToString();

    Writer<string, string> result = await initial.MapAsync(Map);

    Assert.Equal("4", result.Value);
    Assert.Equal(initial.Logs, result.Logs);
  }

  [Fact]
  public void FlatMap_ShouldApplyFunctionToValueAndCombineLogs() {
    ImmutableList<string> logs = ImmutableList.Create("Log1", "Log2");
    Writer<int, string> initial = Writer<int, string>.Wrap(2, ImmutableList.Create("Log1"));
    Writer<string, string> Map(int x) => Writer<string, string>.Wrap((x * 2).ToString(), ImmutableList.Create("Log2"));

    Writer<string, string> result = initial.FlatMap(Map);

    Assert.Equal("4", result.Value);
    Assert.Equal(logs, result.Logs);
  }

  [Fact]
  public async Task FlatMapAsync_ShouldApplyAsyncFunctionToValueAndCombineLogs() {
    ImmutableList<string> logs = ImmutableList.Create("Log1", "Log2");
    Writer<int, string> initial = Writer<int, string>.Wrap(2, ImmutableList.Create("Log1"));

    async Task<Writer<string, string>> Map(int x) =>
      Writer<string, string>.Wrap((await Task.FromResult(x * 2)).ToString(), ImmutableList.Create("Log2"));

    Writer<string, string> result = await initial.FlatMapAsync(Map);

    Assert.Equal("4", result.Value);
    Assert.Equal(logs, result.Logs);
  }

  [Fact]
  public void ForEachLog_ShouldApplyActionToEachLog() {
    ImmutableList<string> logs = ImmutableList.Create("Log1", "Log2");
    Writer<int, string> initial = Writer<int, string>.Wrap(2, logs);
    List<string> resultLogs = new();
    void Action(string log) => resultLogs.Add(log);

    initial.ForEachLog(Action);

    Assert.Equal(logs, resultLogs);
  }

  [Fact]
  public async Task ForEachLogAsync_ShouldApplyAsyncActionToEachLog() {
    ImmutableList<string> logs = ImmutableList.Create("Log1", "Log2");
    Writer<int, string> initial = Writer<int, string>.Wrap(2, logs);
    List<string> resultLogs = [];
    async Task Action(string log) => resultLogs.Add(await Task.FromResult(log));

    await initial.ForEachLogAsync(Action);

    Assert.Equal(logs, resultLogs);
  }
}