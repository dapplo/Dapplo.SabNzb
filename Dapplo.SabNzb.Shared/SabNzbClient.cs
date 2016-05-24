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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.HttpExtensions;
using SabnzbdClient.Client.Entities;

#endregion

namespace Dapplo.SabNzb
{
	public class SabNzbClient
	{
		/// <summary>
		///     Store the specific HttpBehaviour, which contains a IHttpSettings and also some additional logic for making a
		///     HttpClient which works with Jira
		/// </summary>
		private readonly HttpBehaviour _behaviour;

		private string _password;

		private string _user;

		/// <summary>
		///     Create the SabNzbClient object, here the HttpClient is configured
		/// </summary>
		/// <param name="baseUri">Base URL, e.g. https://yoursabnzbserver</param>
		/// <param name="apiKey">API token</param>
		/// <param name="httpSettings">IHttpSettings or null for default</param>
		public SabNzbClient(Uri baseUri, string apiKey, IHttpSettings httpSettings = null)
		{
			if (baseUri == null)
			{
				throw new ArgumentNullException(nameof(baseUri));
			}
			SabNzbApiUri = baseUri.AppendSegments("api").ExtendQuery(new Dictionary<string, string>
			{
				{"output", "json"},
				{"apikey", apiKey}
			});

			_behaviour = new HttpBehaviour
			{
				HttpSettings = httpSettings ?? HttpExtensionsGlobals.HttpSettings,
				OnHttpRequestMessageCreated = httpMessage =>
				{
					if (!string.IsNullOrEmpty(_user) && _password != null)
					{
						httpMessage?.SetBasicAuthorization(_user, _password);
					}
					return httpMessage;
				}
			};
		}

		/// <summary>
		///     The base URI for your JIRA server
		/// </summary>
		public Uri SabNzbApiUri { get; }

		/// <summary>
		///     Retrieve the History
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>SabnzbDRoot</returns>
		public async Task<History> GetHistoryAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queueUri = SabNzbApiUri.ExtendQuery("mode", "history");
			_behaviour.MakeCurrent();
			var root = await queueUri.GetAsAsync<SabnzbDRoot>(cancellationToken);
			return root.HistoryDetails;
		}

		/// <summary>
		///     Retrieve the Queue
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>SabnzbDRoot</returns>
		public async Task<Queue> GetQueueAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queueUri = SabNzbApiUri.ExtendQuery("mode", "queue");
			_behaviour.MakeCurrent();
			var root = await queueUri.GetAsAsync<SabnzbDRoot>(cancellationToken);
			return root.QueueDetails;
		}

		/// <summary>
		///     Set Basic Authentication for the current client
		/// </summary>
		/// <param name="user">username</param>
		/// <param name="password">password</param>
		public void SetBasicAuthentication(string user, string password)
		{
			_user = user;
			_password = password;
		}
	}
}