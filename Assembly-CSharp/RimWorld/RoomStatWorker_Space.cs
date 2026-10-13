using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002178 RID: 8568
	public class RoomStatWorker_Space : RoomStatWorker
	{
		// Token: 0x0600BAF5 RID: 47861 RVA: 0x0035F7F0 File Offset: 0x0035D9F0
		public override float GetScore(Room room)
		{
			if (room.PsychologicallyOutdoors)
			{
				return 350f;
			}
			float num = 0f;
			foreach (IntVec3 c in room.Cells)
			{
				if (c.Standable(room.Map))
				{
					num += 1.4f;
				}
				else if (c.WalkableByNormal(room.Map))
				{
					num += 0.5f;
				}
			}
			return Mathf.Min(num, 350f);
		}
	}
}
