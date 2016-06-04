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

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Dapplo.CaliburnMicro;
using Dapplo.SabNzb.Client.Models;
using SabnzbdClient.Client.Entities;

#endregion

namespace Dapplo.SabNzb.Client.ViewModels
{
	[Export(typeof(IShell))]
	public class MainScreenViewModel : Conductor<Screen>.Collection.OneActive, IShell
	{
		private SabNzbClient _sabNzbClient;

		[Import]
		public IConnectionConfiguration ConnectionConfiguration { get; set; }

		[Import]
		private ConnectionViewModel ConnectionVm { get; set; }

		public Queue SabNzbQueue { get; set; }

		/// <summary>
		///     Used to show a "normal" dialog
		/// </summary>
		[Import]
		private IWindowManager WindowsManager { get; set; }

		protected override void OnActivate()
		{
			base.OnActivate();
			//UiContext.RunOn(async () => await LanguageLoader.Current.ChangeLanguageAsync("en-US")).Wait();

			// Test if there are settings, if not show the configuration
			if (string.IsNullOrEmpty(ConnectionConfiguration.ApiKey) || ConnectionConfiguration.SabNzbUri == null)
			{
				var result = WindowsManager.ShowDialog(ConnectionVm);
				if (result == false)
				{
					Application.Current.Shutdown();
				}
			}
			// Connect
			_sabNzbClient = new SabNzbClient(ConnectionConfiguration.SabNzbUri, ConnectionConfiguration.ApiKey);

			Task.Run(async () =>
			{
				SabNzbQueue = await _sabNzbClient.GetQueueAsync();
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(SabNzbQueue)));
			});
		}
	}
}