using System;
using Microsoft.SharePoint.Administration;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.SharePoint
{
    public abstract class SharePointSink : ILogEventSink
    {
        protected readonly SPDiagnosticsServiceBase DiagnosticsService;
        protected readonly SPDiagnosticsCategory Category;
        protected readonly IFormatProvider FormatProvider;

        protected SharePointSink(
            SPDiagnosticsServiceBase diagnosticsService, 
            SPDiagnosticsCategory category,
            IFormatProvider formatProvider)
        {
            if (diagnosticsService == null)
                throw new ArgumentNullException("diagnosticsService");
            if (category == null)
                throw new ArgumentNullException("category");

            DiagnosticsService = diagnosticsService;
            Category = category;
            FormatProvider = formatProvider;
        }

        public abstract void Emit(LogEvent logEvent);
    }
}