using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ECA RID: 7882
	public class RoomContents_AncientRacks : RoomContentsWorker
	{
		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x0600AEC5 RID: 44741 RVA: 0x0031E0EF File Offset: 0x0031C2EF
		protected virtual FloatRange ShelvesPer100Cells
		{
			get
			{
				return new FloatRange(20f, 40f);
			}
		}

		// Token: 0x0600AEC6 RID: 44742 RVA: 0x0031E100 File Offset: 0x0031C300
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			float num = (float)room.Area / 100f;
			int count = Mathf.Max(Mathf.RoundToInt(this.ShelvesPer100Cells.RandomInRange * num), 1);
			Rot4 value = Rot4.North;
			CellRect boundary = room.Boundary;
			if (boundary.Height > boundary.Width)
			{
				value = Rot4.East;
			}
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientSystemRack, count, room, map, new Rot4?(value), null, null, 1, null, false, false, null, null);
			base.FillRoom(map, room, faction, threatPoints);
		}
	}
}
