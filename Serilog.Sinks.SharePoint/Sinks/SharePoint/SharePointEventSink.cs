using System;
using Microsoft.SharePoint.Administration;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Sinks.SharePoint
{
    public class SharePointEventSink : SharePointSink
    {
        public SharePointEventSink(
            SPDiagnosticsServiceBase diagnosticsService,
            SPDiagnosticsCategory category,
            IFormatProvider formatProvider)
            : base(diagnosticsService, category, formatProvider) {}

        public override void Emit(LogEvent logEvent)
        {
            DiagnosticsService.WriteEvent(
                (ushort) logEvent.Level,
                Category,
                GetEventSeverityByLogEventLevel(logEvent.Level),
                logEvent.RenderMessage(FormatProvider));
        }

        private static EventSeverity GetEventSeverityByLogEventLevel(
            LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    return EventSeverity.Verbose;
                case LogEventLevel.Debug:
                    return EventSeverity.Verbose;
                case LogEventLevel.Information:
                    return EventSeverity.Information;
                case LogEventLevel.Warning:
                    return EventSeverity.Warning;
                case LogEventLevel.Error:
                    return EventSeverity.Error;
                case LogEventLevel.Fatal:
                    return EventSeverity.ErrorCritical;
                default:
                    SelfLog.WriteLine("Unexpected log event level, " +
                                      "writing to EventLog as Information");
                    return EventSeverity.Information;
            }
        }
    }
}