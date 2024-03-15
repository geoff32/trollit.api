using Core.Exceptions;
using Serilog.Core;
using Serilog.Events;

namespace TrollIt.Api.Exceptions;

public class ManagedExceptionLogEventFilter : ILogEventFilter
{
    public bool IsEnabled(LogEvent logEvent) => logEvent.Exception is not ManagedException
        && logEvent.Exception?.InnerException is not ManagedException;
}
