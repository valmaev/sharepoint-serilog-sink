using System;
using Microsoft.SharePoint.Administration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.SharePoint
{
    public abstract class SharePointSink : ILogEventSink
    {
        protected readonly SPDiagnosticsServiceBase DiagnosticsService;
        protected readonly SPDiagnosticsCategory Category;
        protected readonly ITextFormatter TextFormatter;

        protected SharePointSink(
            SPDiagnosticsServiceBase diagnosticsService, 
            SPDiagnosticsCategory category,
            ITextFormatter textFormatter)
        {
            if (diagnosticsService == null)
                throw new ArgumentNullException("diagnosticsService");
            if (category == null)
                throw new ArgumentNullException("category");

            DiagnosticsService = diagnosticsService;
            Category = category;
            TextFormatter = textFormatter;
        }

        public abstract void Emit(LogEvent logEvent);
    }
}