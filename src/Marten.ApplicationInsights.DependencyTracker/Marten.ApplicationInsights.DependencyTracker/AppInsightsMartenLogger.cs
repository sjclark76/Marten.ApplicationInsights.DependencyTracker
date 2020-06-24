using System;
using System.Diagnostics;
using Marten.Services;
using Microsoft.ApplicationInsights;
using Npgsql;

namespace Marten.ApplicationInsights.DependencyTracker
{
    public class AppInsightsMartenLogger : IMartenLogger, IMartenSessionLogger
    {
        private readonly TelemetryClient _telemetryClient;
        private DateTime _startTime;
        private Stopwatch _timer;

        public AppInsightsMartenLogger(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
            _timer = new Stopwatch();
        }

        public IMartenSessionLogger StartSession(IQuerySession session)
        {
            _startTime = DateTime.UtcNow;
            _timer = Stopwatch.StartNew();

            return this;
        }

        public void SchemaChange(string sql)
        {
            // Do nothing.
        }

        public void LogSuccess(NpgsqlCommand command)
        {
            LogDependency(command, true);
        }

        public void LogFailure(NpgsqlCommand command, Exception ex)
        {
            LogDependency(command, false);
        }

        public void RecordSavedChanges(IDocumentSession session, IChangeSet commit)
        {
            // Do Nothing
        }

        private void LogDependency(NpgsqlCommand command, bool success)
        {
            _timer.Stop();

            _telemetryClient.TrackDependency("PostgreSQL",
                GetDependencyName(command),
                command.CommandText,
                _startTime,
                _timer.Elapsed,
                success);
        }

        private static string GetDependencyName(NpgsqlCommand command)
        {
            var dependencyName = command.Connection?.Host ?? string.Empty;

            return dependencyName;
        }
    }
}