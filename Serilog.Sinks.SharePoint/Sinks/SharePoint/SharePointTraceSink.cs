using System;
using Microsoft.SharePoint.Administration;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Sinks.SharePoint
{
    public class SharePointTraceSink : SharePointSink
    {
        public SharePointTraceSink(
            SPDiagnosticsServiceBase diagnosticsService,
            SPDiagnosticsCategory category,
            IFormatProvider formatProvider)
            : base(diagnosticsService, category, formatProvider) {}

        public override void Emit(LogEvent logEvent)
        {
            DiagnosticsService.WriteTrace(
                (uint) logEvent.Level,
                Category,
                GetTraceSeverityByLogEventLevel(logEvent.Level),
                logEvent.RenderMessage(FormatProvider));
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