using FluentAssertions;
using TrollIt.Domain.Scripts;
using TrollIt.Domain.Scripts.Acl.Models;
using TrollIt.Domain.Scripts.Exceptions;
using TrollIt.Domain.Scripts.Abstractions;

namespace TrollIt.Domain.Tests.Scripts;

public class ScriptCategoryTests
{
    [Fact]
    public void ScriptCategory_CreatesCorrectlyFromScriptCategoryDto()
    {
        // Arrange
        var scriptCategoryDto = new ScriptCategoryDto(Name: "testCategory", MaxCall: 5);

        // Act
        var scriptCategory = new ScriptCategory(scriptCategoryDto);

        // Assert
        scriptCategory.Name.Should().Be(scriptCategoryDto.Name);
        scriptCategory.MaxCall.Should().Be(scriptCategoryDto.MaxCall);
    }

    [Fact]
    public void EnsureAccess_ThrowsException_WhenMaxCallExceeded()
    {
        // Arrange
        var scriptCategoryDto = new ScriptCategoryDto(Name: "testCategory", MaxCall: 5);
        var scriptCategory = new ScriptCategory(scriptCategoryDto);
        var scriptCounters = new List<IScriptCounter>
        {
            new ScriptCounter(new Script(ScriptId.Profile, scriptCategory, "/pathProfile", "profile"), 3, 5),
            new ScriptCounter(new Script(ScriptId.Effect, scriptCategory, "/pathEffect", "effect"), 3, 5)
        };

        // Act
        Action act = () => scriptCategory.EnsureAccess(scriptCounters);

        // Assert
        act.Should().Throw<DomainException<ScriptsExceptions>>().WithMessage("*MaxCallScript*");
    }
}