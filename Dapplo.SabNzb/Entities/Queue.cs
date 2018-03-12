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

using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Dapplo.SabNzb.Entities
{
	[DataContract]
	public class Queue
	{
		[DataMember(Name = "cache_art")]
		public string CacheArt { get; set; }

		[DataMember(Name = "cache_limit")]
		public string CacheLimit { get; set; }

		[DataMember(Name = "cache_size")]
		public string CacheSize { get; set; }

		[DataMember(Name = "categories")]
		public IList<string> Categories { get; set; }

		[DataMember(Name = "color_scheme")]
		public string ColorScheme { get; set; }

		[DataMember(Name = "darwin")]
		public bool Darwin { get; set; }

		[DataMember(Name = "diskspace1")]
		public string Diskspace1 { get; set; }

		[DataMember(Name = "diskspace2")]
		public string Diskspace2 { get; set; }

		[DataMember(Name = "diskspacetotal1")]
		public string DiskspaceTotal1 { get; set; }

		[DataMember(Name = "diskspacetotal2")]
		public string DiskspaceTotal2 { get; set; }

		[DataMember(Name = "eta")]
		public string EstimatedTimeOfArrival { get; set; }

		[DataMember(Name = "finish")]
		public int Finish { get; set; }

		[DataMember(Name = "finishaction")]
		public object FinishAction { get; set; }

		[DataMember(Name = "have_warnings")]
		public string HaveWarnings { get; set; }

		[DataMember(Name = "helpuri")]
		public string HelpUri { get; set; }

		[DataMember(Name = "isverbose")]
		public bool IsVerbose { get; set; }

		[DataMember(Name = "kbpersec")]
		public string KbPerSec { get; set; }

		[DataMember(Name = "last_warning")]
		public string LastWarning { get; set; }

		[DataMember(Name = "limit")]
		public int Limit { get; set; }

		[DataMember(Name = "loadavg")]
		public string LoadAverage { get; set; }

		[DataMember(Name = "mb")]
		public string Mb { get; set; }

		[DataMember(Name = "mbleft")]
		public string MbLeft { get; set; }

		[DataMember(Name = "new_release")]
		public string NewRelease { get; set; }

		[DataMember(Name = "new_rel_url")]
		public string NewRelUrl { get; set; }

		[DataMember(Name = "noofslots")]
		public int NoOfSlots { get; set; }

		[DataMember(Name = "nt")]
		public bool Nt { get; set; }

		[DataMember(Name = "nzb_quota")]
		public string NzbQuota { get; set; }

		[DataMember(Name = "paused")]
		public bool Paused { get; set; }

		[DataMember(Name = "pause_int")]
		public string PauseInt { get; set; }

		[DataMember(Name = "queue_details")]
		public string QueueDetails { get; set; }

		[DataMember(Name = "refresh_rate")]
		public string RefreshRate { get; set; }

		[DataMember(Name = "restart_req")]
		public bool RestartReq { get; set; }

		[DataMember(Name = "scripts")]
		public IList<string> Scripts { get; set; }

		[DataMember(Name = "slots")]
		public IList<QueueSlot> Slots { get; set; }

		[DataMember(Name = "speedlimit")]
		public string SpeedLimit { get; set; }

		[DataMember(Name = "start")]
		public int Start { get; set; }

		[DataMember(Name = "status")]
		public string Status { get; set; }

		[DataMember(Name = "timeleft")]
		public string TimeLeft { get; set; }

		[DataMember(Name = "uptime")]
		public string Uptime { get; set; }

		[DataMember(Name = "version")]
		public string Version { get; set; }

		[DataMember(Name = "webdir")]
		public string Webdir { get; set; }
	}
}