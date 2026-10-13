using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EDD RID: 7901
	public class RoomContents_IndustrialStorage : RoomContentsWorker
	{
		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x0600AF02 RID: 44802 RVA: 0x0031EDDC File Offset: 0x0031CFDC
		protected virtual FloatRange ContainersPer100Cells
		{
			get
			{
				return new FloatRange(3f, 5f);
			}
		}

		// Token: 0x0600AF03 RID: 44803 RVA: 0x0031EDF0 File Offset: 0x0031CFF0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			float num = (float)room.Area / 100f;
			int count = Mathf.Max(Mathf.RoundToInt(this.ContainersPer100Cells.RandomInRange * num), 1);
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientLargeContainer, count, room, map, null, null, null, 1, null, false, false, null, null);
			base.FillRoom(map, room, faction, threatPoints);
		}
	}
}
