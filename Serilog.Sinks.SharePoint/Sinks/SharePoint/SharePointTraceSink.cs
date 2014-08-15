using System.IO;
using Microsoft.SharePoint.Administration;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.SharePoint
{
    public class SharePointTraceSink : SharePointSink
    {
        public SharePointTraceSink(
            SPDiagnosticsServiceBase diagnosticsService,
            SPDiagnosticsCategory category,
            ITextFormatter textFormatter)
            : base(diagnosticsService, category, textFormatter) {}

        public override void Emit(LogEvent logEvent)
        {
            using (var renderSpace = new StringWriter())
            {
                TextFormatter.Format(logEvent, renderSpace);
                DiagnosticsService.WriteTrace(
                    (uint)logEvent.Level,
                    Category,
                    GetTraceSeverityByLogEventLevel(logEvent.Level),
                    renderSpace.ToString());
            }
        }
        
        private static TraceSeverity GetTraceSeverityByLogEventLevel(
            LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    return TraceSeverity.Verbose;
                case LogEventLevel.Debug:
                    return TraceSeverity.Verbose;
                case LogEventLevel.Information:
                    return TraceSeverity.High;
                case LogEventLevel.Warning:
                    return TraceSeverity.Monitorable;
                case LogEventLevel.Error:
                    return TraceSeverity.Unexpected;
                case LogEventLevel.Fatal:
                    return TraceSeverity.Unexpected;
                default:
                    SelfLog.WriteLine("Unexpected log event level, " +
                                      "writing to ULS with High trace severity");
                    return TraceSeverity.High;
            }
        }
    }
}