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
	public class HistorySlot
	{
		// History
		[DataMember(Name = "action_line")]
		public string ActionLine { get; set; }

		[DataMember(Name = "bytes")]
		public long Bytes { get; set; }

		[DataMember(Name = "category")]
		public string Category { get; set; }

		/// <summary>
		///     Timestamp when the download was completed
		/// </summary>
		[DataMember(Name = "completed")]
		public long Completed { get; set; }

		[DataMember(Name = "completeness")]
		public int Completeness { get; set; }

		[DataMember(Name = "downloaded")]
		public long Downloaded { get; set; }

		[DataMember(Name = "download_time")]
		public int DownloadTime { get; set; }

		[DataMember(Name = "fail_message")]
		public string FailMessage { get; set; }

		[DataMember(Name = "has_rating")]
		public bool HasRating { get; set; }

		[DataMember(Name = "id")]
		public int Id { get; set; }

		[DataMember(Name = "loaded")]
		public bool Loaded { get; set; }

		[DataMember(Name = "md5sum")]
		public string Md5Sum { get; set; }

		[DataMember(Name = "meta")]
		public string Meta { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "nzb_name")]
		public string NzbName { get; set; }

		[DataMember(Name = "nzo_id")]
		public string NzoId { get; set; }

		[DataMember(Name = "path")]
		public string Path { get; set; }

		[DataMember(Name = "postproc_time")]
		public int PostprocTime { get; set; }

		[DataMember(Name = "pp")]
		public string Pp { get; set; }

		[DataMember(Name = "report")]
		public string Report { get; set; }

		[DataMember(Name = "retry")]
		public int Retry { get; set; }

		[DataMember(Name = "script")]
		public string Script { get; set; }

		[DataMember(Name = "script_line")]
		public string ScriptLine { get; set; }

		[DataMember(Name = "script_log")]
		public string ScriptLog { get; set; }

		[DataMember(Name = "series")]
		public string Series{ get; set; }

		[DataMember(Name = "show_details")]
		public string ShowDetails { get; set; }

		[DataMember(Name = "size")]
		public string Size { get; set; }

		[DataMember(Name = "stage_log")]
		public IList<StageLog> StageLogs { get; set; }

		[DataMember(Name = "status")]
		public string Status { get; set; }

		[DataMember(Name = "storage")]
		public string Storage { get; set; }

		[DataMember(Name = "url")]
		public string Url { get; set; }

		[DataMember(Name = "url_info")]
		public string UrlInfo { get; set; }

		public override bool Equals(object other)
		{
			var otherSlot = other as QueueSlot;
			if (otherSlot == null)
			{
				return false;
			}
			return Equals(NzoId, otherSlot.NzoId);
		}

		public override int GetHashCode()
		{
			return NzoId?.GetHashCode() ?? base.GetHashCode();
		}
	}
}