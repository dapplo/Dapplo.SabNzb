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
using System.Runtime.Serialization;

#endregion

namespace SabnzbdClient.Client.Entities
{
	[DataContract]
	public class History
	{
		[DataMember(Name = "cache_art", EmitDefaultValue = false)]
		public string CacheArt { get; set; }

		[DataMember(Name = "cache_limit", EmitDefaultValue = false)]
		public string CacheLimit { get; set; }

		[DataMember(Name = "cache_size", EmitDefaultValue = false)]
		public string CacheSize { get; set; }

		[DataMember(Name = "color_scheme", EmitDefaultValue = false)]
		public string ColorScheme { get; set; }

		[DataMember(Name = "darwin", EmitDefaultValue = false)]
		public bool Darwin { get; set; }

		[DataMember(Name = "diskspace1", EmitDefaultValue = false)]
		public string Diskspace1 { get; set; }

		[DataMember(Name = "diskspace2", EmitDefaultValue = false)]
		public string Diskspace2 { get; set; }

		[DataMember(Name = "diskspacetotal1", EmitDefaultValue = false)]
		public string DiskspaceTotal1 { get; set; }

		[DataMember(Name = "diskspacetotal2", EmitDefaultValue = false)]
		public string DiskspaceTotal2 { get; set; }

		[DataMember(Name = "eta", EmitDefaultValue = false)]
		public string ETA { get; set; }

		[DataMember(Name = "finishaction", EmitDefaultValue = false)]
		public object FinishAction { get; set; }

		[DataMember(Name = "have_warnings", EmitDefaultValue = false)]
		public string HaveWarnings { get; set; }

		[DataMember(Name = "helpuri", EmitDefaultValue = false)]
		public string HelpURI { get; set; }

		[DataMember(Name = "kbpersec", EmitDefaultValue = false)]
		public string KBPerSec { get; set; }

		[DataMember(Name = "last_warning", EmitDefaultValue = false)]
		public string LastWarning { get; set; }

		[DataMember(Name = "limit", EmitDefaultValue = false)]
		public int Limit { get; set; }

		[DataMember(Name = "loadavg", EmitDefaultValue = false)]
		public string LoadAVG { get; set; }

		[DataMember(Name = "mb", EmitDefaultValue = false)]
		public string MB { get; set; }

		[DataMember(Name = "mbleft", EmitDefaultValue = false)]
		public string MBLeft { get; set; }

		[DataMember(Name = "month_size", EmitDefaultValue = false)]
		public string MonthSize { get; set; }

		[DataMember(Name = "new_release", EmitDefaultValue = false)]
		public string NewRelease { get; set; }

		[DataMember(Name = "new_rel_url", EmitDefaultValue = false)]
		public string NewRelUrl { get; set; }

		[DataMember(Name = "noofslots", EmitDefaultValue = false)]
		public int NoOfSlots { get; set; }

		[DataMember(Name = "nt", EmitDefaultValue = false)]
		public bool NT { get; set; }

		[DataMember(Name = "nzb_quota", EmitDefaultValue = false)]
		public string NZBQuota { get; set; }

		[DataMember(Name = "paused", EmitDefaultValue = false)]
		public bool Paused { get; set; }

		[DataMember(Name = "pause_int", EmitDefaultValue = false)]
		public string PauseInt { get; set; }

		[DataMember(Name = "restart_req", EmitDefaultValue = false)]
		public bool RestartReq { get; set; }

		[DataMember(Name = "slots", EmitDefaultValue = false)]
		public List<Slot> Slots { get; set; }

		[DataMember(Name = "speedlimit", EmitDefaultValue = false)]
		public string SpeedLimit { get; set; }

		[DataMember(Name = "status", EmitDefaultValue = false)]
		public string Status { get; set; }

		[DataMember(Name = "timeleft", EmitDefaultValue = false)]
		public string Timeleft { get; set; }

		[DataMember(Name = "total_size", EmitDefaultValue = false)]
		public string TotalSize { get; set; }

		[DataMember(Name = "uptime", EmitDefaultValue = false)]
		public string Uptime { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "webdir", EmitDefaultValue = false)]
		public string Webdir { get; set; }

		[DataMember(Name = "week_size", EmitDefaultValue = false)]
		public string WeekSize { get; set; }
	}
}