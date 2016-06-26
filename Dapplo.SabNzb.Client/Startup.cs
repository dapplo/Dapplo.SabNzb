﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.SabNzb
// 
//  Dapplo.SabNzb is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.SabNzb is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.SabNzb. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System;
using System.Diagnostics;
using System.Windows;
using Dapplo.CaliburnMicro;
using Dapplo.Config.Converters;
using Dapplo.Log.Facade;
using Dapplo.Log.Facade.Loggers;

#endregion

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
			LogSettings.RegisterDefaultLogger<DebugLogger>(LogLevels.Verbose);
#endif
			var application = new Dapplication("Dapplo.SabNZB", "68cb5937-90cd-4a17-9d30-d68fa9906cd6")
			{
				ShutdownMode = ShutdownMode.OnExplicitShutdown
			};
			application.Add(@".", "Dapplo.CaliburnMicro.dll");
			// Comment this if no TrayIcons should be used
			application.Add(@".", "Dapplo.CaliburnMicro.NotifyIconWpf.dll");
			// Comment this to use the default window manager
			application.Add(@".", "Dapplo.CaliburnMicro.Metro.dll");
			application.Add(typeof(Startup).Assembly);

			StringEncryptionTypeConverter.RgbIv = "0@94hFj3&E4r!k231E!";
			StringEncryptionTypeConverter.RgbKey = "lkfwWF/63)=ßDeefkez4§Rf33g39hh§F";

			application.Run();
		}
	}
}