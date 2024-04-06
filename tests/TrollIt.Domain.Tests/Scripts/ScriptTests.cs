using FluentAssertions;
using TrollIt.Domain.Scripts;
using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;

namespace TrollIt.Domain.Tests.Scripts;

public class ScriptTests
{
    [Fact]
    public void Script_CreatesCorrectlyFromScriptDto()
    {
        // Arrange
        var scriptDto = new ScriptDto(Id: ScriptId.Profile, Category: new ScriptCategoryDto("testCategory", 3), Path: "testPath", Name: "testName");

        // Act
        var script = new Script(scriptDto);

        // Assert
        script.Id.Should().Be(scriptDto.Id);
        script.Category.Name.Should().Be(scriptDto.Category.Name);
        script.Path.Should().Be(scriptDto.Path);
        script.Name.Should().Be(scriptDto.Name);
    }

    [Fact]
    public void IsSameCategory_ReturnsCorrectly()
    {
        // Arrange
        var scriptDto = new ScriptDto(Id: ScriptId.Profile, Category: new ScriptCategoryDto("testCategory", 5), Path: "testPath", Name: "testName");
        var script = new Script(scriptDto);
        var otherScript = new Script(new ScriptDto(Id: ScriptId.Effect, Category: new ScriptCategoryDto("testCategory", 5), Path: "otherPath", Name: "otherName"));

        // Act
        var isSameCategory = script.IsSameCategory(otherScript);

        // Assert
        isSameCategory.Should().BeTrue();
    }
}