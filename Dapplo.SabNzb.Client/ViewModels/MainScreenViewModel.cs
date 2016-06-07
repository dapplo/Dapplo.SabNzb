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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using Dapplo.CaliburnMicro;
using Dapplo.LogFacade;
using Dapplo.SabNzb.Client.Models;
using Dapplo.Utils;
using GongSolutions.Wpf.DragDrop;
using SabnzbdClient.Client.Entities;

#endregion

namespace Dapplo.SabNzb.Client.ViewModels
{
	[Export(typeof(IShell))]
	[Export]
	public class MainScreenViewModel : Conductor<Screen>.Collection.OneActive, IShell, IDropTarget
	{
		private static readonly LogSource Log = new LogSource();
		private readonly AsyncLock _lock = new AsyncLock();
		private bool _canBeShown;
		private DispatcherTimer _timer;

		[Import]
		public IConnectionConfiguration ConnectionConfiguration { get; set; }

		[Import]
		private ConnectionViewModel ConnectionVm { get; set; }

		/// <summary>
		/// Is the view model currently on the screen?
		/// </summary>
		public bool CanBeShown {
			get
			{
				return _canBeShown;
			}
			set
			{
				if (_canBeShown != value)
				{
					_canBeShown = value;
					NotifyOfPropertyChange(nameof(CanBeShown));
				}
			}
		}

		public Queue SabNzbQueue { get; set; }
		public History SabNzbHistory { get; set; }

		public ObservableCollection<Slot> QueuedSlots { get; set; } = new ObservableCollection<Slot>();
		public ObservableCollection<Slot> HistorySlots { get; set; } = new ObservableCollection<Slot>();

		/// <summary>
		///     Used to show a "normal" dialog
		/// </summary>
		[Import]
		private IWindowManager WindowsManager { get; set; }

		public MainScreenViewModel()
		{
#if DEBUG
			// For the designer
			if (Execute.InDesignMode)
			{
				LoadDesignData();
			}
#endif
		}

		/// <summary>
		/// Open the connection configuration
		/// </summary>
		public void Configure()
		{
			// Test if there are settings, if not show the configuration
			var result = WindowsManager.ShowDialog(ConnectionVm);
			if (result == true)
			{
				// ???
			}
		}

		/// <summary>
		/// Pause the queue downloads
		/// </summary>
		public async Task Pause()
		{
			await ConnectionVm.SabNzbClient.PauseQueueAsync();
		}

		/// <summary>
		/// Update by retrieving the information, call on UI!!
		/// </summary>
		private async Task UpdateAsync(CancellationToken token = default(CancellationToken))
		{
			using (await _lock.LockAsync())
			{
				if (ConnectionVm.IsConfigured)
				{
					if (!ConnectionVm.IsConnected)
					{
						await ConnectionVm.Connect();
					}

					var client = ConnectionVm.SabNzbClient;

					// TODO: Extract the queue information into a VM.
					SabNzbQueue = await client.GetQueueAsync(token);
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(SabNzbQueue)));
					foreach (var slot in SabNzbQueue.Slots)
					{
						var queueSlotIndex = QueuedSlots.IndexOf(slot);
						if (queueSlotIndex < 0)
						{
							QueuedSlots.Add(slot);
						}
						else
						{
							QueuedSlots.RemoveAt(queueSlotIndex);
							if (QueuedSlots.Count == queueSlotIndex)
							{
								QueuedSlots.Add(slot);
							}
							else
							{
								QueuedSlots.Insert(queueSlotIndex, slot);
							}
						}
					}
					// Find the slots that are no longer in the queue
					var finishedSlots = QueuedSlots.Where(x => !SabNzbQueue.Slots.Contains(x)).ToList();
					// TODO: Notify!?
					foreach (var finishedSlot in finishedSlots)
					{
						QueuedSlots.Remove(finishedSlot);
					}


					// TODO: Extract the history information into a VM.
					SabNzbHistory = await client.GetHistoryAsync(token);
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(SabNzbHistory)));
					foreach (var slot in SabNzbHistory.Slots)
					{
						var historySlotIndex = HistorySlots.IndexOf(slot);
						if (historySlotIndex < 0)
						{
							HistorySlots.Add(slot);
						}
						else
						{
							HistorySlots.RemoveAt(historySlotIndex);
							if (HistorySlots.Count == historySlotIndex)
							{
								HistorySlots.Add(slot);
							}
							else
							{
								HistorySlots.Insert(historySlotIndex, slot);
							}
						}
					}
					// Find the slots that are no longer in the history
					var finishedHistorySlots = HistorySlots.Where(x => !SabNzbHistory.Slots.Contains(x)).ToList();
					// TODO: Notify!?
					foreach (var finishedHistorySlot in finishedHistorySlots)
					{
						HistorySlots.Remove(finishedHistorySlot);
					}
				}
			}
		}

		/// <summary>
		/// This is called when the ViewModel is deactivated
		/// </summary>
		/// <param name="close"></param>
		protected override void OnDeactivate(bool close)
		{
			_timer.Stop();
			CanBeShown = true;
			base.OnDeactivate(close);
		}

		/// <summary>
		/// This is called when the ViewModel is activated
		/// </summary>
		protected override void OnActivate()
		{
			_timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(5),
			};
			_timer.Tick += async (sender, eventArgs) => await UpdateAsync();
			_timer.Start();
			base.OnActivate();
			CanBeShown = false;
			if (!ConnectionVm.IsConfigured)
			{
				// Just call configure
				Configure();
			}
		}

		#region Drag n Drop
		private DragDropEffects GetEffect(IEnumerable<string> dragFileList)
		{
			return dragFileList.Any(item =>
			{
				var extension = Path.GetExtension(item);
				return ConnectionVm.IsConnected && extension != null && extension.Equals(".nzb");
			}) ? DragDropEffects.Copy : DragDropEffects.None;
		}
		void IDropTarget.DragOver(IDropInfo dropInfo)
		{
			var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
			dropInfo.Effects = GetEffect(dragFileList);
		}

		void IDropTarget.Drop(IDropInfo dropInfo)
		{
			var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>().ToList();
			dropInfo.Effects = GetEffect(dragFileList);

			Task.Run(async () =>
			{
				foreach (var nzbFile in dragFileList.Where(x => x != null && Path.GetExtension(x).Equals(".nzb")))
				{
					using (var filestream = new FileStream(nzbFile, FileMode.Open, FileAccess.Read))
					{
						var nzoId = await ConnectionVm.SabNzbClient.AddAsync(Path.GetFileName(nzbFile), filestream);
						Log.Info().WriteLine("Added {0}", nzoId);
					}
				}
			}).Wait();
		}
		#endregion

		#region Designer
#if DEBUG
		/// <summary>
		/// This is only available when configuration is debug, and loads the data for the designer
		/// </summary>
		private void LoadDesignData()
		{
			SabNzbQueue = new Queue();
			var random = new Random();
			SabNzbQueue.DiskspaceTotal1 = $"{random.Next(0, 2400)}Mb";
			for (int i = 0; i < 5; i++)
			{
				var slot = new Slot
				{
					NzoId = $"DesignId{i}",
					Name = $"This is the nice NZB name for {i}",
					NzbName = $"blub{i}.nzb",
					Percentage = $"{random.Next(0, 100)}",
					Filename = $"blub {i}.nzb"
				};
				QueuedSlots.Add(slot);
			}

			for (int i = 100; i < 105; i++)
			{
				var slot = new Slot
				{
					NzoId = $"DesignId{i}",
					Name = $"This is the nice NZB name for {i}",
					NzbName = $"blub{i}.nzb",
					Size = $"{random.Next(0, 14000)} KB",
					Status = "Completed",
					Category = "Books",
					Filename = $"blub {i}.nzb"
				};
				HistorySlots.Add(slot);
			}
		}
#endif
		#endregion
	}
}