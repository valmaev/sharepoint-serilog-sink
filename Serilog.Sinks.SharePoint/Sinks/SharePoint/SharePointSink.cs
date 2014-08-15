using System;
using Microsoft.SharePoint.Administration;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Sinks.SharePoint
{
    public class SharePointSink : ILogEventSink
    {
        private readonly SPDiagnosticsServiceBase _diagnosticsService;
        private readonly SPDiagnosticsCategory _category;
        private readonly IFormatProvider _formatProvider;

        public SharePointSink(
            SPDiagnosticsServiceBase diagnosticsService, 
            SPDiagnosticsCategory category,
            IFormatProvider formatProvider)
        {
            if (diagnosticsService == null)
                throw new ArgumentNullException("diagnosticsService");
            if (category == null)
                throw new ArgumentNullException("category");

            _diagnosticsService = diagnosticsService;
            _category = category;
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            _diagnosticsService.WriteTrace(
                (uint) logEvent.Level,
                _category,
                GetTraceSeverityByLogEventLevel(logEvent.Level),
                logEvent.RenderMessage(_formatProvider));
        }

        private TraceSeverity GetTraceSeverityByLogEventLevel(LogEventLevel level)
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