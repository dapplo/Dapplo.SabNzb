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
using System.IO;
using System.Threading.Tasks;
using Dapplo.Log.XUnit;
using Dapplo.LogFacade;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Dapplo.SabNzb.Tests
{
	public class SabNzbTests
	{
		private readonly SabNzbClient _sabNzbClient;

		public SabNzbTests(ITestOutputHelper testOutputHelper)
		{
			XUnitLogger.RegisterLogger(testOutputHelper, LogLevels.Verbose);
			var sabNzbUri = Environment.GetEnvironmentVariable("sabnzb_test_uri");
			if (string.IsNullOrEmpty(sabNzbUri))
			{
				throw new ArgumentNullException("sabnzb_test_uri");
			}
			var apiKey = Environment.GetEnvironmentVariable("sabnzb_test_apikey");
			if (string.IsNullOrEmpty(apiKey))
			{
				throw new ArgumentNullException("sabnzb_test_apikey");
			}
			_sabNzbClient = new SabNzbClient(new Uri(sabNzbUri), apiKey);

			var username = Environment.GetEnvironmentVariable("sabnzb_test_username");
			var password = Environment.GetEnvironmentVariable("sabnzb_test_password");
			if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
			{
				_sabNzbClient.SetBasicAuthentication(username, password);
			}
		}

		//[Fact]
		public async Task TestAddFile()
		{
			var filename = @"path\filename.nzb";
			using (var filestream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				var nzoId = await _sabNzbClient.AddAsync(Path.GetFileName(filename), filestream, "Nice name");
				Assert.NotNull(nzoId);
			}
		}

		[Fact]
		public async Task TestGetHistory()
		{
			var history = await _sabNzbClient.GetHistoryAsync();
			Assert.NotNull(history);
		}

		[Fact]
		public async Task TestGetQueue()
		{
			var queue = await _sabNzbClient.GetQueueAsync();
			Assert.NotNull(queue);
		}
	}
}