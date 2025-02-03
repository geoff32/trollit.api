using Argon;
using Reqnroll;

namespace TrollIt.Api.Tests.Context;

public class VerifyContext(FeatureContext featureContext, ScenarioContext scenarioContext)
{
    public VerifySettings Settings => CreateVerifySettings();

    private VerifySettings CreateVerifySettings()
    {
        var verifySettings = new VerifySettings();
        verifySettings.AddExtraSettings(settings =>
        {
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
        });
        verifySettings.UseDirectory(Path.Join("Features", featureContext.FeatureInfo.Title));
        verifySettings.UseFileName(scenarioContext.ScenarioInfo.Title);
        return verifySettings;
    }
}