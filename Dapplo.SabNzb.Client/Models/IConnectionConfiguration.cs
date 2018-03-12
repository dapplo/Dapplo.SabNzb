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
using System.ComponentModel;
using Dapplo.Ini.Converters;
using Dapplo.Ini;

#endregion

namespace Dapplo.SabNzb.Client.Models
{
	[IniSection("Connection")]
	public interface IConnectionConfiguration : IIniSection, INotifyPropertyChanged
	{
		[Description("The API key to access the SabNZB server")]
		[TypeConverter(typeof(StringEncryptionTypeConverter))]
		string ApiKey { get; set; }

		[Description("Password for the Http Basic Authentication, for when e.g. an Apache before the SabNzb server needs this.")]
		[TypeConverter(typeof(StringEncryptionTypeConverter))]
		string Password { get; set; }

		[Description("The Uri to the SabNZB server.")]
		Uri SabNzbUri { get; set; }

		[DefaultValue(false), Description("True to activate Http Basic Authentication, for when e.g. an Apache before the SabNzb server needs this.")]
		bool UseHttpAuthentication { get; set; }

		[Description("Username for the Http Basic Authentication, for when e.g. an Apache before the SabNzb server needs this.")]
		[TypeConverter(typeof(StringEncryptionTypeConverter))]
		string Username { get; set; }
	}
}