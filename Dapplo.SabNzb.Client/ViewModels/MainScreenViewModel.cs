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

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.CaliburnMicro;
using Dapplo.SabNzb.Client.Models;
using SabnzbdClient.Client.Entities;

#endregion

namespace Dapplo.SabNzb.Client.ViewModels
{
	[Export(typeof(IShell))]
	[Export]
	public class MainScreenViewModel : Conductor<Screen>.Collection.OneActive, IShell
	{
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

		public async Task Configure()
		{
			// Test if there are settings, if not show the configuration
			var result = WindowsManager.ShowDialog(ConnectionVm);
			if (result == true)
			{
				SabNzbQueue = await ConnectionVm.SabNzbClient.GetQueueAsync();
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(SabNzbQueue)));
			}
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			if (string.IsNullOrEmpty(ConnectionConfiguration.ApiKey) || ConnectionConfiguration.SabNzbUri == null)
			{
				// Just call configure
				// ReSharper disable once UnusedVariable
				var ignoreTask = Configure();
			}
		}
	}
}