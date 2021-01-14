using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Likvido.ApiAuth.Server
{
    public static class LoggerExtensions
    {
        private static class EventIds
        {
            public static readonly EventId UserInfoHeaderParsingException = new EventId(2000, nameof(UserInfoHeaderParsingException));
            public static readonly EventId ApiKeyAuthorizationFailure = new EventId(2001, nameof(ApiKeyAuthorizationFailure));
        }

        public static void ApiKeyAuthorizationFailureOccurred(this ILogger logger, string details, IDictionary<string, object> state)
        {
            logger.LogWithScope(
                l =>
                l.Log(
                    LogLevel.Error,
                    EventIds.ApiKeyAuthorizationFailure,
                    details),
                state);
        }
        public static void UserInfoHeaderParsingExceptionOccurred(this ILogger logger, Exception ex, IDictionary<string, object> state)
        {
            logger.LogWithScope(
                l =>
                l.Log(
                    LogLevel.Error,
                    EventIds.UserInfoHeaderParsingException,
                    ex,
                    "User info Header parsing error."),
                state);
        }

        private static void LogWithScope(this ILogger logger, Action<ILogger> action, IDictionary<string, object>? state)
        {
            IDisposable? scope = null;
            if (state != null)
            {
                scope = logger.BeginScope(state);
            }
            try
            {
                action(logger);
            }
            finally
            {
                scope?.Dispose();
            }
        }
    }
}
