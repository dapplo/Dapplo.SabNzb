//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
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
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.Log;
using Dapplo.SabNzb.Client.Languages;
using Dapplo.SabNzb.Client.Models;
using Dapplo.HttpExtensions;
using System.IO;
using System.Runtime.CompilerServices;
using Dapplo.Log.Loggers;
using System.Reactive.Disposables;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.Language;

#endregion

namespace Dapplo.SabNzb.Client.ViewModels
{
    /// <summary>
    ///     This ViewModel is both the holder of the SabNzbClient and
    ///     the dialog to change the settings.
    /// </summary>
    public class ConnectionViewModel : Screen
    {
        private static readonly LogSource Log = new LogSource();
        private readonly INetworkConfiguration _networkConfiguration;
        private bool _isConnected;
        private IDisposable _eventRegistrations;

        public IConnectionConfiguration ConnectionConfiguration { get; }

        public IConnectionTranslations ConnectionTranslations { get; private set; }

        public ConnectionViewModel(
            INetworkConfiguration networkConfiguration,
            IConnectionTranslations connectionTranslations,
            IConnectionConfiguration connectionConfiguration)
        {
            _networkConfiguration = networkConfiguration;
            ConnectionTranslations = connectionTranslations;
            ConnectionConfiguration = connectionConfiguration;
#if DEBUG
            // For the designer
            if (!Execute.InDesignMode)
            {
                return;
            }

            LogSettings.RegisterDefaultLogger<TraceLogger>(LogLevels.Verbose);
            Log.Info().WriteLine("Running in designer");
            LoadDesignData();
#endif
            // Make sure the settings from the configuration file are used.
            HttpExtensionsGlobals.HttpSettings = _networkConfiguration;

            if (IsConfigured)
            {
                // Make the "connection"
                Task.Run(async () => await Connect());
            }
            ConnectionConfiguration.OnPropertyChanged().Subscribe(propertyChangedEventArgs =>
            {
                NotifyOfPropertyChange(nameof(IsConnected));
            });
        }

        /// <summary>
        ///     Check if the configuration is correctly filled
        /// </summary>
        public bool IsConfigured
        {
            get
            {
                if (string.IsNullOrEmpty(ConnectionConfiguration?.ApiKey))
                {
                    return false;
                }
                if (ConnectionConfiguration.SabNzbUri == null || !ConnectionConfiguration.SabNzbUri.IsAbsoluteUri || !ConnectionConfiguration.SabNzbUri.IsWellFormedOriginalString())
                {
                    return false;
                }

                if (!ConnectionConfiguration.UseHttpAuthentication)
                {
                    return true;
                }

                if (string.IsNullOrEmpty(ConnectionConfiguration.Username))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(ConnectionConfiguration.Password))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        ///     Value representing if there is a "connection" (if it is possible to use the api)
        /// </summary>
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (_isConnected == value)
                {
                    return;
                }

                _isConnected = value;
                NotifyOfPropertyChange(nameof(IsConnected));
            }
        }

        public SabNzbClient SabNzbClient { get; private set; }

        protected override void OnActivate()
        {
            base.OnActivate();

            // Generate NotifyPropertyChanged when the config changes, by sending IsConfigured
            var languageRegistration = ConnectionTranslations.CreateDisplayNameBinding(this, nameof(IConnectionTranslations.Title));

            _eventRegistrations = Disposable.Create(() =>
            {
                languageRegistration?.Dispose();
            });
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventRegistrations?.Dispose();
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
#if DEBUG
        /// <summary>
        /// Fill values for the designer
        /// </summary>
        private void LoadDesignData([CallerFilePath] string source = null)
        {
            Log.Debug().WriteLine("Starting to fill the designer");
            var loader = LanguageLoader.Current;
            if (loader == null)
            {
                // This creates a LanguageLoader which can find the language directory
                loader = new LanguageLoader("SabNzb", specifiedDirectories: new []{ Path.Combine(Path.GetDirectoryName(source), @"..\languages") });
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
#endif
        #endregion
    }
}