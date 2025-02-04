using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Reqnroll;
using TrollIt.Api.Tests.Context;
using TrollIt.Application;
using TrollIt.Application.Profiles.Models;
using TrollIt.Domain.Profiles.Acl.Models;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Api.Tests.Steps;

[Binding]
public class ProfilesStepDefinition(
    WebApiContext webApiContext,
    VerifyContext verifyContext,
    ScenarioContext scenarioContext)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Given(@"The profile is shared for reading with me")]
    public void GivenTheProfileIsSharedForReadingWithMe()
    {
        var appUser = scenarioContext.Get<AppUser>();
        var profile = scenarioContext.Get<ProfileDto>();
        var userPolicy = Substitute.For<IUserPolicy>();
        userPolicy.CanRead(FeatureId.Profile, profile.TrollId)
            .Returns(true);
        webApiContext.ShareRepository.GetUserPolicyAsync(appUser.TrollId, Arg.Any<CancellationToken>())
            .Returns(userPolicy);
    }
    [Given(@"The profile is not shared for reading with me")]
    public void GivenTheProfileIsNotSharedForReadingWithMe()
    {
        var appUser = scenarioContext.Get<AppUser>();
        var profile = scenarioContext.Get<ProfileDto>();
        var userPolicy = Substitute.For<IUserPolicy>();
        userPolicy.CanRead(FeatureId.Profile, profile.TrollId)
            .Returns(false);
        webApiContext.ShareRepository.GetUserPolicyAsync(appUser.TrollId, Arg.Any<CancellationToken>())
            .Returns(userPolicy);
    }
    
    [Given(@"The profile is shared for refresh with me")]
    public void GivenTheProfileIsSharedForRefreshWithMe()
    {
        var appUser = scenarioContext.Get<AppUser>();
        var profile = scenarioContext.Get<ProfileDto>();
        var userPolicy = Substitute.For<IUserPolicy>();
        userPolicy.CanRefresh(FeatureId.Profile, profile.TrollId)
            .Returns(true);
        webApiContext.ShareRepository.GetUserPolicyAsync(appUser.TrollId, Arg.Any<CancellationToken>())
            .Returns(userPolicy);
    }
    [Given(@"The profile is not shared for refresh with me")]
    public void GivenTheProfileIsNotSharedForRefreshWithMe()
    {
        var appUser = scenarioContext.Get<AppUser>();
        var profile = scenarioContext.Get<ProfileDto>();
        var userPolicy = Substitute.For<IUserPolicy>();
        userPolicy.CanRefresh(FeatureId.Profile, profile.TrollId)
            .Returns(false);
        webApiContext.ShareRepository.GetUserPolicyAsync(appUser.TrollId, Arg.Any<CancellationToken>())
            .Returns(userPolicy);
    }
    
    [Given(@"An existing profile")]
    public void GivenAnExistingProfile()
    {
        var profile = CreateInitialProfile();
        webApiContext.ProfilesRepository.GetProfileAsync(profile.TrollId, Arg.Any<CancellationToken>())
            .Returns(new Profile(profile));
        
        scenarioContext.Set(profile);
    }
    
    [Given(@"A missing profile")]
    public void GivenAMissingProfile()
    {
        const int trollId = 99;
        scenarioContext.Set((ProfileDto?)null);
        webApiContext.ProfilesRepository.GetProfileAsync(trollId, Arg.Any<CancellationToken>())
            .ReturnsNull();
        scenarioContext.Set<TrollId>(trollId);
    }
    
    [When(@"I get the profile")]
    public async Task WhenIGetTheProfile()
    {
        var profile = scenarioContext.Get<ProfileDto>();
        var trollId = profile?.TrollId ?? scenarioContext.Get<TrollId>();
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/profiles/{trollId}");
        var authenticatedUser = scenarioContext.Get<AppUser>();
        if (authenticatedUser != null)
        {
            request.Headers.Add("Mock-Authenticated-UserId", authenticatedUser.AccountId.ToString());
        }
        
        scenarioContext.Set(await webApiContext.Client.SendAsync(request));
    }
    
    [When(@"I refresh the profile")]
    public async Task WhenIRefreshTheProfile()
    {
        var profile = scenarioContext.Get<ProfileDto>();
        var trollId = profile?.TrollId ?? scenarioContext.Get<TrollId>();
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/profiles/{trollId}");
        var authenticatedUser = scenarioContext.Get<AppUser>();
        if (authenticatedUser != null)
        {
            request.Headers.Add("Mock-Authenticated-UserId", authenticatedUser.AccountId.ToString());
        }
        
        scenarioContext.Set(await webApiContext.Client.SendAsync(request));
    }

    [Then(@"The profile response should be ok")]
    public async Task ThenTheProfileResponseShouldBeOk()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);
        
        await response
            .Should().NotBeNull()
            .And.Be200Ok()
            .And.VerifyContentAsync<ProfileResponse>(verifyContext.Settings);
    }

    private static readonly ProfileDto InitialProfile = new(
        TrollId: 0,
        TurnDuration: new ValueAttributeDto<TimeSpan>(TimeSpan.FromHours(12),
            new BonusMalusDto<TimeSpan>(TimeSpan.Zero, TimeSpan.Zero)),
        Vitality: new ValueAttributeDto(30, new BonusMalusDto(0, 0)),
        View: new ValueAttributeDto(3, new BonusMalusDto(0, 0)),
        Attack: new DiceAttributeDto(3, new BonusMalusDto(0, 0)),
        Dodge: new DiceAttributeDto(3, new BonusMalusDto(0, 0)),
        Damage: new DiceAttributeDto(3, new BonusMalusDto(0, 0)),
        Armor: new DiceAttributeDto(3, new BonusMalusDto(0, 0)),
        Regeneration: new DiceAttributeDto(3, new BonusMalusDto(0, 0)),
        MagicMastery: new ValueAttributeDto(0, new BonusMalusDto(0, 0)),
        MagicResistance: new ValueAttributeDto(0, new BonusMalusDto(0, 0))
    );

    private record TrollId(int Value)
    {
        public static implicit operator int(TrollId trollId) => trollId.Value;
        public static implicit operator TrollId(int value) => new(value);
    }

    private enum Breed
    {
        Kastar,
        Durakuir,
        Skrim,
        Tomawak,
        Darkling,
        Nkrwapu
    }
    
    private static ProfileDto CreateInitialProfile(Breed? breed = null)
    {
        return breed switch
        {
            null or Breed.Kastar => InitialProfile with { TrollId = 1, Damage = InitialProfile.Damage with { Value = 4 } },
            Breed.Durakuir => InitialProfile with { TrollId = 2, Vitality = InitialProfile.Vitality with { Value = 40 } },
            Breed.Skrim => InitialProfile with { TrollId = 3, Attack = InitialProfile.Attack with { Value = 4 } },
            Breed.Tomawak => InitialProfile with { TrollId = 4, View = InitialProfile.View with { Value = 4 } },
            Breed.Darkling => InitialProfile with { TrollId = 5, Regeneration = InitialProfile.Regeneration with { Value = 4 } },
            Breed.Nkrwapu => InitialProfile with { TrollId = 6 },
            _ => throw new ArgumentOutOfRangeException(nameof(breed), breed, null)
        };
    }
}