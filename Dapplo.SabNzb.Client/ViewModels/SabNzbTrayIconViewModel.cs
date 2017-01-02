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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Behaviors;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.CaliburnMicro.NotifyIconWpf.ViewModels;
using Dapplo.SabNzb.Client.Languages;
using MahApps.Metro.IconPacks;

#endregion

namespace Dapplo.SabNzb.Client.ViewModels
{
	[Export(typeof(ITrayIconViewModel))]
	public class SabNzbTrayIconViewModel : TrayIconViewModel, IHandle<string>
	{
		private CompositeDisposable _disposables;

		[ImportMany("contextmenu", typeof(IMenuItem))]
		private IEnumerable<IMenuItem> ContextMenuItems { get; set; }

		[Import]
		private ConnectionViewModel ConnectionVm { get; set; }

		[Import]
		public IContextMenuTranslations ContextMenuTranslations { get; set; }

		[Import]
		public ICoreTranslations CoreTranslations { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

		[Import]
		public MainScreenViewModel MainScreenVm { get; set; }

		/// <summary>
		///     Used to show a "normal" dialog
		/// </summary>
		[Import]
		private IWindowManager WindowsManager { get; set; }

		public void Handle(string message)
		{
			var trayIcon = TrayIconManager.GetTrayIconFor(this);
			trayIcon.ShowBalloonTip("Event", message);
		}

		private void CreateContectMenu()
		{

			// Set the title of the icon (the ToolTipText) to our IContextMenuTranslations.Title
			var coreTranslationsObservable = CoreTranslations.CreateDisplayNameBinding(this, nameof(ICoreTranslations.Title));
			_disposables.Add(coreTranslationsObservable);


			var items = ContextMenuItems.ToList();
			items.Add(new MenuItem
			{
				Id = "V_Main",
				ClickAction = menuItem =>
				{
					if (!MainScreenVm.IsActive)
					{
						WindowsManager.ShowDialog(MainScreenVm);
					}
				}
			});
			var contextMenuDisplayNameBinding = ContextMenuTranslations.CreateDisplayNameBinding(items.Last(), nameof(IContextMenuTranslations.ShowMain));

			_disposables.Add(contextMenuDisplayNameBinding);


			items.Add(new MenuItem
			{
				Id = "X_Configure",
				ClickAction = menuItem =>
				{
					if (!ConnectionVm.IsActive)
					{
						WindowsManager.ShowDialog(ConnectionVm);
					}
				}
			});
			contextMenuDisplayNameBinding.AddDisplayNameBinding(items.Last(),nameof(IContextMenuTranslations.Configure));

			items.Add(new MenuItem
			{
				Style = MenuItemStyles.Separator,
				Id = "Y_Separator"
			});

			items.Add(new MenuItem
			{
				Id = "Z_Exit",
				ClickAction = menuItem => Application.Current.Shutdown()
			});
			contextMenuDisplayNameBinding.AddDisplayNameBinding(items.Last(), nameof(IContextMenuTranslations.Exit));

			ConfigureMenuItems(items);

			// Make sure the margin is set, do this AFTER the icon are set
			items.ApplyIconMargin(new Thickness(2));
		}

		protected override void OnActivate()
		{
			_disposables?.Dispose();
			_disposables = new CompositeDisposable();
			base.OnActivate();

			CreateContectMenu();

			// Use Behavior to set the icon
			var taskbarIcon = TrayIcon as FrameworkElement;
			taskbarIcon?.SetCurrentValue(FrameworkElementIcon.ValueProperty, new PackIconMaterial
			{
				Kind = PackIconMaterialKind.Flash,
				Background = Brushes.White,
				Foreground = Brushes.Black
			});

			Show();
			EventAggregator.Subscribe(this);
		}

		protected override void OnDeactivate(bool close)
		{
			base.OnDeactivate(close);
			_disposables.Dispose();
		}
	}
}