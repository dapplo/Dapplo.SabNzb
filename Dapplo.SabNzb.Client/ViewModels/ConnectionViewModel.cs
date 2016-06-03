using System.ComponentModel.Composition;
using Caliburn.Micro;
using Dapplo.SabNzb.Client.Languages;
using Dapplo.SabNzb.Client.Models;

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
