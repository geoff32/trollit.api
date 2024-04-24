using System.Diagnostics.CodeAnalysis;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Infrastructure.Shares.Acl.Abstractions;
using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Shares.Acl;

internal class SharesRepositoryAcl(ISharesAcl sharesAcl) : ISharesRepositoryAcl
{
    public SharePolicy ToDataModel(ISharePolicy sharePolicy) =>
        new(sharePolicy.Id, sharePolicy.Name, sharePolicy.Members.Select(ToDataModel).ToArray());

    [return: NotNullIfNotNull(nameof(data))]
    public ISharePolicy? ToDomain(SharePolicy? data) => 
        data == null
            ? null
            : sharesAcl.ToDomain(ToDto(data));

    public IEnumerable<ISharePolicy> ToDomain(IEnumerable<SharePolicy> data) => data.Select(d => ToDomain(d));

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

    private static Models.ShareStatus ToDataModel(Domain.Shares.Abstractions.ShareStatus status) => status switch
    {
        Domain.Shares.Abstractions.ShareStatus.Owner => Models.ShareStatus.Owner,
        Domain.Shares.Abstractions.ShareStatus.Admin => Models.ShareStatus.Admin,
        Domain.Shares.Abstractions.ShareStatus.User => Models.ShareStatus.User,
        Domain.Shares.Abstractions.ShareStatus.Guest => Models.ShareStatus.Guest,
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

    private static SharePolicyDto ToDto(SharePolicy data) => new(data.Id, data.Name, data.Trolls.Select(ToDto));

    private static MemberDto ToDto(TrollShare member) => new
    (
        member.Trollid,
        ToDomain(member.Status),
        member.Features.Select(feature => new FeatureDto(ToDomain(feature.Id), CanRead(feature.Status), CanRefresh(feature.Status)))
    );

    private static Domain.Shares.Abstractions.FeatureId ToDomain(Models.FeatureId id) => id switch
    {
        Models.FeatureId.Profile => Domain.Shares.Abstractions.FeatureId.Profile,
        Models.FeatureId.View => Domain.Shares.Abstractions.FeatureId.View,
        _ => throw new ArgumentOutOfRangeException(nameof(id), id, null),
    };

    private static Domain.Shares.Abstractions.ShareStatus ToDomain(Models.ShareStatus status) => status switch
    {
        Models.ShareStatus.Owner => Domain.Shares.Abstractions.ShareStatus.Owner,
        Models.ShareStatus.Admin => Domain.Shares.Abstractions.ShareStatus.Admin,
        Models.ShareStatus.User => Domain.Shares.Abstractions.ShareStatus.User,
        Models.ShareStatus.Guest => Domain.Shares.Abstractions.ShareStatus.Guest,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
    };

    private static bool CanRead(FeatureStatus status) => status == FeatureStatus.Read || status == FeatureStatus.Readwrite;
    private static bool CanRefresh(FeatureStatus status) => status == FeatureStatus.Readwrite;
}
