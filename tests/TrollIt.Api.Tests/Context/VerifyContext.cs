using Argon;
using Microsoft.AspNetCore.Mvc;
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
            settings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
        });
        
        verifySettings.UseDirectory(Path.Join("Features", featureContext.FeatureInfo.Title));
        verifySettings.UseFileName(scenarioContext.ScenarioInfo.Title);
        
        verifySettings
            .IgnoreMember<ProblemDetails>(problem => problem.Extensions);
        verifySettings
            .IgnoreMember<ProblemDetails>(problem => problem.Type);
            
        return verifySettings;
    }
}