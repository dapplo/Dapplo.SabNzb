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
	public class Slot
	{
		// History
		[DataMember(Name = "action_line", EmitDefaultValue = false)]
		public string ActionLine { get; set; }

		[DataMember(Name = "avg_age", EmitDefaultValue = false)]
		public string AverageAge { get; set; }

		[DataMember(Name = "bytes", EmitDefaultValue = false)]
		public long Bytes { get; set; }

		[DataMember(Name = "cat", EmitDefaultValue = false)]
		public string Category { get; set; }

		[DataMember(Name = "completed", EmitDefaultValue = false)]
		public int Completed { get; set; }

		[DataMember(Name = "completeness", EmitDefaultValue = false)]
		public int Completeness { get; set; }

		[DataMember(Name = "downloaded", EmitDefaultValue = false)]
		public long Downloaded { get; set; }

		[DataMember(Name = "download_time", EmitDefaultValue = false)]
		public int DownloadTime { get; set; }

		[DataMember(Name = "eta", EmitDefaultValue = false)]
		public string Eta { get; set; }

		[DataMember(Name = "fail_message", EmitDefaultValue = false)]
		public string FailMessage { get; set; }

		[DataMember(Name = "filename", EmitDefaultValue = false)]
		public string Filename { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(Name = "index", EmitDefaultValue = false)]
		public int Index { get; set; }

		[DataMember(Name = "loaded", EmitDefaultValue = false)]
		public bool Loaded { get; set; }

		[DataMember(Name = "mb", EmitDefaultValue = false)]
		public string Mb { get; set; }

		[DataMember(Name = "mbleft", EmitDefaultValue = false)]
		public string MbLeft { get; set; }

		[DataMember(Name = "msgid", EmitDefaultValue = false)]
		public string MsgId { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "nzb_name", EmitDefaultValue = false)]
		public string NzbName { get; set; }

		[DataMember(Name = "nzo_id", EmitDefaultValue = false)]
		public string NzoId { get; set; }

		[DataMember(Name = "path", EmitDefaultValue = false)]
		public string Path { get; set; }

		[DataMember(Name = "percentage", EmitDefaultValue = false)]
		public string Percentage { get; set; }

		[DataMember(Name = "postproc_time", EmitDefaultValue = false)]
		public int PostprocTime { get; set; }

		[DataMember(Name = "pp", EmitDefaultValue = false)]
		public string Pp { get; set; }

		[DataMember(Name = "priority", EmitDefaultValue = false)]
		public string Priority { get; set; }

		[DataMember(Name = "report", EmitDefaultValue = false)]
		public string Report { get; set; }

		[DataMember(Name = "script", EmitDefaultValue = false)]
		public string Script { get; set; }

		[DataMember(Name = "script_line", EmitDefaultValue = false)]
		public string ScriptLine { get; set; }

		[DataMember(Name = "script_log", EmitDefaultValue = false)]
		public string ScriptLog { get; set; }

		[DataMember(Name = "show_details", EmitDefaultValue = false)]
		public string ShowDetails { get; set; }

		[DataMember(Name = "size", EmitDefaultValue = false)]
		public string Size { get; set; }

		[DataMember(Name = "stage_log", EmitDefaultValue = false)]
		public List<StageLog> StageLogs { get; set; }

		[DataMember(Name = "status", EmitDefaultValue = false)]
		public string Status { get; set; }

		[DataMember(Name = "storage", EmitDefaultValue = false)]
		public string Storage { get; set; }

		[DataMember(Name = "timeleft", EmitDefaultValue = false)]
		public string TimeLeft { get; set; }

		[DataMember(Name = "unpackopts", EmitDefaultValue = false)]
		public string UnpackOpts { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "url_info", EmitDefaultValue = false)]
		public string UrlInfo { get; set; }

		[DataMember(Name = "verbosity", EmitDefaultValue = false)]
		public string Verbosity { get; set; }

		public override int GetHashCode()
		{
			return NzoId == null ? base.GetHashCode() : NzoId.GetHashCode();
		}
		public override bool Equals(object other)
		{
			var otherSlot = other as Slot;
			if (otherSlot == null)
			{
				return false;
			}
			return object.Equals(NzoId, otherSlot.NzoId);
		}
	}
}