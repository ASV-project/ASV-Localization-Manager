using System;

using Serilog.Core;
using Serilog.Events;

public class LoggerErrorSink : ILogEventSink
{
	public static event EventHandler<LogEvent> onErrorOccured;
	
	public void Emit(LogEvent logEvent)
	{
		if (logEvent.Level == LogEventLevel.Error)
		{
			onErrorOccured?.Invoke(this, logEvent);
		}
	}
}