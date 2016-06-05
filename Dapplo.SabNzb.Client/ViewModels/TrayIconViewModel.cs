//  Dapplo - building blocks for desktop applications
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

using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.LogFacade;
using Dapplo.SabNzb.Client.Languages;

#endregion

namespace Dapplo.SabNzb.Client.ViewModels
{
	[Export(typeof(ITrayIconViewModel))]
	public class TrayIconViewModel : Screen, ITrayIconViewModel, IHandle<string>
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		private ConnectionViewModel ConnectionVm { get; set; }

		[Import]
		public IContextMenuTranslations ContextMenuTranslations { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

		[Import]
		public MainScreenViewModel MainScreenVm { get; set; }

		[Import]
		public ITrayIconManager TrayIconManager { get; set; }

		/// <summary>
		///     Used to show a "normal" dialog
		/// </summary>
		[Import]
		private IWindowManager WindowsManager { get; set; }

		public void Handle(string message)
		{
			var trayIcon = TrayIconManager.GetTrayIconFor(this);
			trayIcon.ShowBalloonTip("Event", message);
		}

		public void Configure()
		{
			Log.Debug().WriteLine("Configure");
			WindowsManager.ShowDialog(ConnectionVm);
		}

		public void Exit()
		{
			Log.Debug().WriteLine("Exit");
			Application.Current.Shutdown();
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			var trayIcon = TrayIconManager.GetTrayIconFor(this);
			trayIcon.Show();
			EventAggregator.Subscribe(this);
		}

		public void ShowMain()
		{
			WindowsManager.ShowDialog(MainScreenVm);
		}
	}
}