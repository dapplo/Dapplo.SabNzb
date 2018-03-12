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

using System.Runtime.Serialization;

#endregion

namespace Dapplo.SabNzb.Entities
{
	[DataContract]
	public class QueueSlot
	{
		[DataMember(Name = "avg_age")]
		public string AverageAge { get; set; }

		[DataMember(Name = "cat")]
		public string Category { get; set; }

		[DataMember(Name = "eta")]
		public string Eta { get; set; }

		[DataMember(Name = "filename")]
		public string Filename { get; set; }

		[DataMember(Name = "has_rating")]
		public bool HasRating { get; set; }

		[DataMember(Name = "index")]
		public int Index { get; set; }

		[DataMember(Name = "mb")]
		public string Mb { get; set; }

		[DataMember(Name = "mbleft")]
		public string MbLeft { get; set; }

		[DataMember(Name = "missing")]
		public int Missing { get; set; }

		[DataMember(Name = "nzo_id")]
		public string NzoId { get; set; }

		[DataMember(Name = "percentage")]
		public string Percentage { get; set; }

		[DataMember(Name = "priority")]
		public string Priority { get; set; }

		[DataMember(Name = "script")]
		public string Script { get; set; }

		[DataMember(Name = "size")]
		public string Size { get; set; }

		[DataMember(Name = "sizeleft")]
		public string SizeLeft { get; set; }

		[DataMember(Name = "status")]
		public string Status { get; set; }

		[DataMember(Name = "timeleft")]
		public string TimeLeft { get; set; }

		[DataMember(Name = "unpackopts")]
		public string UnpackOpts { get; set; }

		[DataMember(Name = "verbosity")]
		public string Verbosity { get; set; }

		public override int GetHashCode()
		{
			return NzoId?.GetHashCode() ?? base.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			if (!(obj is QueueSlot otherSlot))
			{
				return false;
			}
			return Equals(NzoId, otherSlot.NzoId);
		}
	}
}