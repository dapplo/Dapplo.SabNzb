using Dapplo.CaliburnMicro;
using Dapplo.LogFacade;
using System;
using System.Diagnostics;
using System.Windows;
using Dapplo.LogFacade.Loggers;

namespace Dapplo.SabNzb.Client
{
	public class Startup
	{
		/// <summary>
		///     Start the application
		/// </summary>
		[STAThread, DebuggerNonUserCode]
		public static void Main()
		{
#if DEBUG
			// Initialize a debug logger for Dapplo packages
			LogSettings.Logger = new DebugLogger { Level = LogLevel.Verbose };
#endif
			var application = new Dapplication("Dapplo.CaliburnMicro.Demo", "f32dbad8-9904-473e-86e2-19275c2d06a5")
			{
				ShutdownMode = ShutdownMode.OnLastWindowClose
			};
			application.Add(@".", "Dapplo.CaliburnMicro.dll");
			// Comment this if no TrayIcons should be used
			application.Add(@".", "Dapplo.CaliburnMicro.NotifyIconWpf.dll");
			// Comment this to use the default window manager
			application.Add(@".", "Dapplo.CaliburnMicro.Metro.dll");
			application.Add(typeof(Startup).Assembly);

			application.Run();
		}

	}
}
