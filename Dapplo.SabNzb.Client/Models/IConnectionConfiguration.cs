using System;
using System.ComponentModel;
using Dapplo.Config.Ini;

namespace Dapplo.SabNzb.Client.Models
{
	[IniSection("Connection")]
	public interface IConnectionConfiguration : IIniSection, INotifyPropertyChanged
	{
		Uri SabNzbUri { get; set; }

		string ApiKey { get; set; }
	}
}
