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
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.CaliburnMicro;
using Dapplo.LogFacade;
using Dapplo.SabNzb.Client.Languages;
using Dapplo.SabNzb.Client.Models;
using Dapplo.HttpExtensions;
using Dapplo.LogFacade.Loggers;
using System.IO;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;

#endregion

namespace Dapplo.SabNzb.Client.ViewModels
{
	/// <summary>
	///     This ViewModel is both the holder of the SabNzbClient and
	///     the dialog to change the settings.
	/// </summary>
	[Export]
	public class ConnectionViewModel : Screen, IPartImportsSatisfiedNotification
	{
		private static readonly LogSource Log = new LogSource();
		private bool _isConnected;

		[Import]
		public IConnectionConfiguration ConnectionConfiguration { get; set; }

		[Import]
		public IConnectionTranslations ConnectionTranslations { get; set; }

		[Import]
		private INetworkConfiguration NetworkConfiguration { get; set; }

		public ConnectionViewModel()
		{
#if DEBUG
			// For the designer
			if (Execute.InDesignMode)
			{
				LogSettings.RegisterDefaultLogger<TraceLogger>(LogLevels.Verbose);
				Log.Info().WriteLine("Running in designer");
				LoadDesignData();
			}
#endif
		}

		/// <summary>
		///     Check if the configuration is correctly filled
		/// </summary>
		public bool IsConfigured
		{
			get
			{
				if (ConnectionConfiguration == null)
				{
					return false;
				}
				if (string.IsNullOrEmpty(ConnectionConfiguration.ApiKey))
				{
					return false;
				}
				if (ConnectionConfiguration.SabNzbUri == null || !ConnectionConfiguration.SabNzbUri.IsAbsoluteUri || !ConnectionConfiguration.SabNzbUri.IsWellFormedOriginalString())
				{
					return false;
				}
				if (ConnectionConfiguration.UseHttpAuthentication)
				{
					if (string.IsNullOrEmpty(ConnectionConfiguration.Username))
					{
						return false;
					}
					if (string.IsNullOrEmpty(ConnectionConfiguration.Password))
					{
						return false;
					}
				}
				return true;
			}
		}

		/// <summary>
		///     Value representing if there is a "connection" (if it is possible to use the api)
		/// </summary>
		public bool IsConnected
		{
			get { return _isConnected; }
			set
			{
				if (_isConnected != value)
				{
					_isConnected = value;
					NotifyOfPropertyChange(nameof(IsConnected));
				}
			}
		}

		public SabNzbClient SabNzbClient { get; private set; }

		public void OnImportsSatisfied()
		{
			// Make sure the settings from the configuration file are used.
			HttpExtensionsGlobals.HttpSettings = NetworkConfiguration;

			// Generate NotifyPropertyChanged when the config changes, by sending IsConfigured
			ConnectionConfiguration.BindNotifyPropertyChanged(".*", OnPropertyChanged, nameof(IsConfigured));
			if (IsConfigured)
			{
				// Make the "connection"
				Task.Run(async () => await Connect());
			}
		}

		/// <summary>
		///     Connect creates a SabNzbClient when the configuration is complete
		/// </summary>
		public async Task Connect()
		{
			if (IsConfigured)
			{
				// Connect
				SabNzbClient = new SabNzbClient(ConnectionConfiguration.SabNzbUri, ConnectionConfiguration.ApiKey);
				if (ConnectionConfiguration.UseHttpAuthentication)
				{
					SabNzbClient.SetBasicAuthentication(ConnectionConfiguration.Username, ConnectionConfiguration.Password);
				}
				try
				{
					await SabNzbClient.GetVersionAsync();
					IsConnected = true;
				}
				catch (Exception ex)
				{
					Log.Error().WriteLine(ex, "Unable to connect to {0}", ConnectionConfiguration.SabNzbUri.AbsoluteUri);
					IsConnected = false;
				}
			}
			TryClose(IsConnected);
		}

		#region Designer
		/// <summary>
		/// Fill values for the designer
		/// </summary>
		private void LoadDesignData([CallerFilePath] string source = null)
		{
			//ConnectionTranslations = InterfaceImpl.InterceptorFactory.New<IConnectionTranslations>();
			Log.Debug().WriteLine("Starting to fill the designer");
			var loader = Config.Language.LanguageLoader.Current;
			if (loader == null)
			{
				loader = new Config.Language.LanguageLoader("SabNzb", specifiedDirectories: new []{ Path.Combine(Path.GetDirectoryName(source), @"..\languages") });
				loader.CorrectMissingTranslations();
			}
			Task.Run(async () =>
			{
				try
				{
					var result = await loader.RegisterAndGetAsync<IConnectionTranslations>();
					await loader.ReloadAsync();
					Log.Debug().WriteLine(string.Join(",", loader.AvailableLanguages.Values));
					ConnectionTranslations = result;
				} catch (Exception ex)
				{
					Log.Error().WriteLine(ex);
				}
			}).Wait();

		}
		#endregion
	}
}