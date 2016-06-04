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
using Caliburn.Micro;
using Dapplo.SabNzb.Client.Languages;
using Dapplo.SabNzb.Client.Models;

#endregion

namespace Dapplo.SabNzb.Client.ViewModels
{
	[Export]
	public class ConnectionViewModel : Screen
	{
		[Import]
		public IConnectionConfiguration ConnectionConfiguration { get; set; }

		[Import]
		public IConnectionTranslations ConnectionTranslations { get; set; }

		public void Connect()
		{
			TryClose(true);
		}
	}
}