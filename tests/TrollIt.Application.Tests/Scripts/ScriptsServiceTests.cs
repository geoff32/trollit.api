using TrollIt.Application.Scripts;
using TrollIt.Application.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;
using TrollIt.Domain.Scripts.Infrastructure;
using NSubstitute;
using TrollIt.Domain.Scripts.Abstractions;

namespace TrollIt.Application.Tests.Scripts;

public class ScriptsServiceTests
{
    private readonly IScriptRepository _scriptRepository;
    private readonly ScriptsService _scriptsService;

    public ScriptsServiceTests()
    {
        _scriptRepository = Substitute.For<IScriptRepository>();
        _scriptsService = new ScriptsService(_scriptRepository);
    }

    [Fact]
    public async Task CleanAsync_CallsCleanHistoryAsync_WithCorrectDate()
    {
        // Arrange
        var expectedDate = DateTimeOffset.UtcNow.AddDays(-2);

        // Act
        await _scriptsService.CleanAsync(CancellationToken.None);

        // Assert
        await _scriptRepository.Received().CleanHistoryAsync(Arg.Is<DateTimeOffset>(date => date.Date == expectedDate.Date));
    }

    [Fact]
    public async Task TraceAsync_TracesScript_WhenScriptExists()
    {
        // Arrange
        var trollId = 1;
        var scriptId = ScriptId.Profile;
        var scriptPath = "scriptPath";
        var trollScript = Substitute.For<ITrollScript>();
        var scriptCounter = Substitute.For<IScriptCounter>();
        trollScript.TrollId.Returns(trollId);
        trollScript.GetScriptCounter(scriptPath).Returns(scriptCounter);
        _scriptRepository.GetTrollScriptAsync(trollId, Arg.Any<CancellationToken>()).Returns(trollScript);

        // Act
        await _scriptsService.TraceAsync(trollId, scriptPath, CancellationToken.None);

        // Assert
        await _scriptRepository.Received().TraceAsync(trollScript, scriptId, Arg.Any<DateTimeOffset>(), Arg.Any<CancellationToken>());
    }
}