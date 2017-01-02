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

using System.Runtime.Serialization;

#endregion

namespace Dapplo.SabNzb.Entities
{
	public enum EndOfQueueActions
	{
		[EnumMember(Value = "hibernate_pc")] Hibernate,
		[EnumMember(Value = "standby_pc")] Standby,
		[EnumMember(Value = "shutdown_program")] Exit,
		[EnumMember(Value = "script_")] Script
	}
}