using System.IO;
using Microsoft.SharePoint.Administration;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.SharePoint
{
    public class SharePointEventSink : SharePointSink
    {
        public SharePointEventSink(
            SPDiagnosticsServiceBase diagnosticsService,
            SPDiagnosticsCategory category,
            ITextFormatter textFormatter)
            : base(diagnosticsService, category, textFormatter) {}

        public override void Emit(LogEvent logEvent)
        {
            using (var renderSpace = new StringWriter())
            {
                TextFormatter.Format(logEvent, renderSpace);
                DiagnosticsService.WriteEvent(
                    (ushort) logEvent.Level,
                    Category,
                    GetEventSeverityByLogEventLevel(logEvent.Level),
                    renderSpace.ToString());
            }
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