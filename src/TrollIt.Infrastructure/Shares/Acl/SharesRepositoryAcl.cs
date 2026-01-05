using System.Diagnostics.CodeAnalysis;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Infrastructure.Shares.Acl.Abstractions;
using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Shares.Acl;

internal class SharesRepositoryAcl(ISharesAcl sharesAcl) : ISharesRepositoryAcl
{
    public Policy ToDataModel(IPolicy policy) =>
        new(policy.Id, policy.Name, policy.Members.Select(ToDataModel).ToArray());

    [return: NotNullIfNotNull(nameof(data))]
    public IPolicy? ToDomain(Policy? data) => 
        data == null
            ? null
            : sharesAcl.ToDomain(ToDto(data));

    public IEnumerable<IPolicy> ToDomain(IEnumerable<Policy> data) => data.Select(d => ToDomain(d));

    private static TrollShare ToDataModel(IMember member)
    {
        return new TrollShare(member.Id, ToDataModel(member.Status), member.Features.Select(ToDataModel).ToArray());
    }

    private static TrollFeature ToDataModel(IFeature feature)
    {
        return new TrollFeature(ToDataModel(feature.Id), ToFeatureStatusDataModel(feature.CanRead, feature.CanRefresh));
    }

    private static Models.FeatureId ToDataModel(Domain.Shares.Abstractions.FeatureId id) => id switch
    {
        Domain.Shares.Abstractions.FeatureId.Profile => Models.FeatureId.Profile,
        Domain.Shares.Abstractions.FeatureId.View => Models.FeatureId.View,
        _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
    };

    private static Models.PolicyStatus ToDataModel(Domain.Shares.Abstractions.PolicyStatus status) => status switch
    {
        Domain.Shares.Abstractions.PolicyStatus.Owner => Models.PolicyStatus.Owner,
        Domain.Shares.Abstractions.PolicyStatus.Admin => Models.PolicyStatus.Admin,
        Domain.Shares.Abstractions.PolicyStatus.User => Models.PolicyStatus.User,
        Domain.Shares.Abstractions.PolicyStatus.Guest => Models.PolicyStatus.Guest,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static FeatureStatus ToFeatureStatusDataModel(bool canRead, bool canRefresh)
    {
        if (canRefresh)
        {
            return FeatureStatus.Readwrite;
        }
        if (canRead)
        {
            return FeatureStatus.Read;
        }

        return FeatureStatus.Inactive;
    }

    private static PolicyDto ToDto(Policy data) => new(data.Id, data.Name, data.Trolls.Select(ToDto));

    private static MemberDto ToDto(TrollShare member) => new
    (
        member.Trollid,
        ToDomain(member.Status),
        [.. member.Features.Select(feature => new FeatureDto(ToDomain(feature.Id), CanRead(feature.Status), CanRefresh(feature.Status)))]
    );

    private static Domain.Shares.Abstractions.FeatureId ToDomain(Models.FeatureId id) => id switch
    {
        Models.FeatureId.Profile => Domain.Shares.Abstractions.FeatureId.Profile,
        Models.FeatureId.View => Domain.Shares.Abstractions.FeatureId.View,
        _ => throw new ArgumentOutOfRangeException(nameof(id), id, null),
    };

    private static Domain.Shares.Abstractions.PolicyStatus ToDomain(Models.PolicyStatus status) => status switch
    {
        Models.PolicyStatus.Owner => Domain.Shares.Abstractions.PolicyStatus.Owner,
        Models.PolicyStatus.Admin => Domain.Shares.Abstractions.PolicyStatus.Admin,
        Models.PolicyStatus.User => Domain.Shares.Abstractions.PolicyStatus.User,
        Models.PolicyStatus.Guest => Domain.Shares.Abstractions.PolicyStatus.Guest,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
    };

    internal static bool CanRead(FeatureStatus status) => status == FeatureStatus.Read || status == FeatureStatus.Readwrite;
    internal static bool CanRefresh(FeatureStatus status) => status == FeatureStatus.Readwrite;
}
