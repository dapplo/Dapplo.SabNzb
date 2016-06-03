using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Dapplo.CaliburnMicro;
using Dapplo.Config.Language;
using Dapplo.SabNzb.Client.Models;
using Dapplo.Utils;
using SabnzbdClient.Client.Entities;

namespace Dapplo.SabNzb.Client.ViewModels
{
	[Export(typeof(IShell))]
	public class MainScreenViewModel : Conductor<Screen>.Collection.OneActive, IShell
	{
		private SabNzbClient _sabNzbClient;

		[Import]
		public IConnectionConfiguration ConnectionConfiguration { get; set; }

		[Import]
		private ConnectionViewModel ConnectionVm { get; set; }

		public Queue SabNzbQueue { get; set; }

		/// <summary>
		/// Used to show a "normal" dialog
		/// </summary>
		[Import]
		private IWindowManager WindowsManager { get; set; }

		protected override void OnActivate()
		{
			base.OnActivate();
			//UiContext.RunOn(async () => await LanguageLoader.Current.ChangeLanguageAsync("en-US")).Wait();

			// Test if there are settings, if not show the configuration
			if (string.IsNullOrEmpty(ConnectionConfiguration.ApiKey) || ConnectionConfiguration.SabNzbUri == null)
			{
				var result = WindowsManager.ShowDialog(ConnectionVm);
				if (result == false)
				{
					Application.Current.Shutdown();
				}
			}
			// Connect
			_sabNzbClient = new SabNzbClient(ConnectionConfiguration.SabNzbUri, ConnectionConfiguration.ApiKey);

			Task.Run(async () =>
			{
				SabNzbQueue = await _sabNzbClient.GetQueueAsync();
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(SabNzbQueue)));
			});
		}
	}
}
