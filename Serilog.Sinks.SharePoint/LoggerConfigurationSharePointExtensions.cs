using System;
using Microsoft.SharePoint.Administration;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.SharePoint;

namespace Serilog
{
    public static class LoggerConfigurationSharePointExtensions
    {
        private const string DefaultTemplate = "{Message}{NewLine}{Exception}";
        
        public static LoggerConfiguration SharePointTrace(
            this LoggerSinkConfiguration loggerConfiguration,
            SPDiagnosticsServiceBase diagnosticsService = null,
            SPDiagnosticsCategory diagnosticsCategory = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultTemplate,
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
                    new MessageTemplateTextFormatter(outputTemplate, formatProvider)),
                restrictedToMinimumLevel);
        }

        public static LoggerConfiguration SharePointEvent(
            this LoggerSinkConfiguration loggerConfiguration,
            SPDiagnosticsServiceBase diagnosticsService = null,
            SPDiagnosticsCategory diagnosticsCategory = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultTemplate,
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
                    new MessageTemplateTextFormatter(outputTemplate, formatProvider)),
                restrictedToMinimumLevel);
        }
    }
}