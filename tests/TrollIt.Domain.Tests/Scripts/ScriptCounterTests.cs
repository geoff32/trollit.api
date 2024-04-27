using FluentAssertions;
using FluentAssertions.Specialized;
using TrollIt.Domain.Scripts;
using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;
using TrollIt.Domain.Scripts.Exceptions;

namespace TrollIt.Domain.Tests.Scripts;

public class ScriptCounterTests
{
    [Fact]
    public void ScriptCounter_CreatesCorrectlyFromScriptCounterDto()
    {
        // Arrange
        var scriptDto = new ScriptDto(Id: ScriptId.Profile, Category: new ScriptCategoryDto("testCategory", 5), Path: "testPath", Name: "testName");
        var scriptCounterDto = new ScriptCounterDto(Script: scriptDto, Call: 3, MaxCall: 5);

        // Act
        var scriptCounter = new ScriptCounter(scriptCounterDto);

        // Assert
        scriptCounter.Script.Id.Should().Be(scriptDto.Id);
        scriptCounter.Script.Category.Name.Should().Be(scriptDto.Category.Name);
        scriptCounter.Script.Path.Should().Be(scriptDto.Path);
        scriptCounter.Script.Name.Should().Be(scriptDto.Name);
        scriptCounter.Call.Should().Be(scriptCounterDto.Call);
        scriptCounter.MaxCall.Should().Be(scriptCounterDto.MaxCall);
    }

    [Fact]
    public void EnsureAccess_ThrowsException_WhenMaxCallExceeded()
    {
        // Arrange
        var scriptDto = new ScriptDto(Id: ScriptId.Profile, Category: new ScriptCategoryDto("testCategory", 5), Path: "testPath", Name: "testName");
        var scriptCounterDto = new ScriptCounterDto(Script: scriptDto, Call: 5, MaxCall: 5);
        var scriptCounter = new ScriptCounter(scriptCounterDto);

        // Act
        Action act = () => scriptCounter.EnsureAccess();

        // Assert
        act.Should().ThrowDomainException(ScriptsExceptions.MaxCallScript);
    }
}