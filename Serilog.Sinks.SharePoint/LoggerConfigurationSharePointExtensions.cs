using System;
using Microsoft.SharePoint.Administration;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SharePoint;

namespace Serilog
{
    public static class LoggerConfigurationSharePointExtensions
    {
        public static LoggerConfiguration SharePointTrace(
            this LoggerSinkConfiguration loggerConfiguration,
            SPDiagnosticsServiceBase diagnosticsService = null,
            SPDiagnosticsCategory diagnosticsCategory = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException("loggerConfiguration");

            return loggerConfiguration.Sink(
                new SharePointTraceSink(
                    diagnosticsService ?? SPDiagnosticsService.Local,
                    diagnosticsCategory ?? new SPDiagnosticsCategory(
                                               "Serilog",
                                               TraceSeverity.Medium,
                                               EventSeverity.Information),
                    formatProvider),
                restrictedToMinimumLevel);
        }

        public static LoggerConfiguration SharePointEvent(
            this LoggerSinkConfiguration loggerConfiguration,
            SPDiagnosticsServiceBase diagnosticsService = null,
            SPDiagnosticsCategory diagnosticsCategory = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException("loggerConfiguration");

            return loggerConfiguration.Sink(
                new SharePointEventSink(
                    diagnosticsService ?? SPDiagnosticsService.Local,
                    diagnosticsCategory ?? new SPDiagnosticsCategory(
                                               "Serilog",
                                               TraceSeverity.Medium,
                                               EventSeverity.Information),
                    formatProvider),
                restrictedToMinimumLevel);
        }
    }
}