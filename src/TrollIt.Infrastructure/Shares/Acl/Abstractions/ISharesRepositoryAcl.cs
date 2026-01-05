using System.Diagnostics.CodeAnalysis;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Shares.Acl.Abstractions;

internal interface ISharesRepositoryAcl
{
    Policy ToDataModel(IPolicy policy);
    [return: NotNullIfNotNull(nameof(data))]
    IPolicy? ToDomain(Policy? data);
    IEnumerable<IPolicy> ToDomain(IEnumerable<Policy> data);
}
