using FluentAssertions;
using TrollIt.Domain.Scripts;
using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;
using TrollIt.Domain.Scripts.Exceptions;
using NSubstitute;

namespace TrollIt.Domain.Tests.Scripts;

public class TrollScriptTests
{
    [Fact]
    public void TrollScript_CreatesCorrectlyFromTrollScriptDto()
    {
        // Arrange
        var scriptDto = new ScriptDto(Id: ScriptId.Profile, Category: new ScriptCategoryDto("testCategory", 5), Path: "testPath", Name: "testName");
        var scriptCounterDto = new ScriptCounterDto(Script: scriptDto, Call: 3, MaxCall: 5);
        var trollScriptDto = new TrollScriptDto(TrollId: 1, ScriptCounters: [scriptCounterDto]);

        // Act
        var trollScript = new TrollScript(trollScriptDto);

        // Assert
        trollScript.TrollId.Should().Be(trollScriptDto.TrollId);
        trollScript.ScriptCounters.Should().HaveCount(trollScriptDto.ScriptCounters.Count());
        trollScript.ScriptCounters.First().Script.Id.Should().Be(scriptDto.Id);
        trollScript.ScriptCounters.First().Script.Category.Name.Should().Be(scriptDto.Category.Name);
        trollScript.ScriptCounters.First().Script.Path.Should().Be(scriptDto.Path);
        trollScript.ScriptCounters.First().Script.Name.Should().Be(scriptDto.Name);
        trollScript.ScriptCounters.First().Call.Should().Be(scriptCounterDto.Call);
        trollScript.ScriptCounters.First().MaxCall.Should().Be(scriptCounterDto.MaxCall);
    }

    [Fact]
    public void EnsureAccess_ShouldCallEnsureAccessOnScriptCounterAndCategory()
    {
        // Arrange
        var script = Substitute.For<IScript>();
        var scriptCategory = Substitute.For<IScriptCategory>();
        script.Category.Returns(scriptCategory);
        script.IsSameCategory(script).Returns(true);
        var scriptCounter = Substitute.For<IScriptCounter>();
        scriptCounter.Script.Returns(script);
        var trollScript = new TrollScript(1, [scriptCounter]);

        // Act
        trollScript.EnsureAccess(scriptCounter);

        // Assert
        scriptCounter.Received().EnsureAccess();
        scriptCategory.Received().EnsureAccess(Arg.Is<IEnumerable<IScriptCounter>>(x => x.Single() == scriptCounter));
    }

    [Fact]
    public void GetScriptCounter_ReturnsCorrectScriptCounter_WhenScriptPathExists()
    {
        // Arrange
        var scriptDto = new ScriptDto(Id: ScriptId.Profile, Category: new ScriptCategoryDto("testCategory", 5), Path: "testPath", Name: "testName");
        var scriptCounterDto = new ScriptCounterDto(Script: scriptDto, Call: 3, MaxCall: 5);
        var trollScriptDto = new TrollScriptDto(TrollId: 1, ScriptCounters: [scriptCounterDto]);
        var trollScript = new TrollScript(trollScriptDto);

        // Act
        var scriptCounter = trollScript.GetScriptCounter("testPath");

        // Assert
        scriptCounter.Script.Path.Should().Be("testPath");
    }

    [Fact]
    public void GetScriptCounter_ThrowsException_WhenScriptPathDoesNotExist()
    {
        // Arrange
        var scriptDto = new ScriptDto(Id: ScriptId.Profile, Category: new ScriptCategoryDto("testCategory", 5), Path: "testPath", Name: "testName");
        var scriptCounterDto = new ScriptCounterDto(Script: scriptDto, Call: 3, MaxCall: 5);
        var trollScriptDto = new TrollScriptDto(TrollId: 1, ScriptCounters: [scriptCounterDto]);
        var trollScript = new TrollScript(trollScriptDto);

        // Act
        Action act = () => trollScript.GetScriptCounter("nonexistentPath");

        // Assert
        act.Should().Throw<DomainException<ScriptsExceptions>>().WithMessage("*UnkownScript*");
    }
}