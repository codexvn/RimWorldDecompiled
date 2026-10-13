using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002172 RID: 8562
	public class RoomRoleWorker_WorshipRoom : RoomRoleWorker
	{
		// Token: 0x0600BAE4 RID: 47844 RVA: 0x0035F1D0 File Offset: 0x0035D3D0
		public override string PostProcessedLabel(string baseLabel, Room room)
		{
			Ideo ideo = this.DominatingIdeo(room);
			if (ideo == null || ideo.WorshipRoomLabel.NullOrEmpty())
			{
				return base.PostProcessedLabel(baseLabel, room);
			}
			return ideo.WorshipRoomLabel;
		}

		// Token: 0x0600BAE5 RID: 47845 RVA: 0x0035F204 File Offset: 0x0035D404
		public override float GetScore(Room room)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return -1f;
			}
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			int num = 0;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i].def.isAltar)
				{
					ThingWithComps thingWithComps = containedAndAdjacentThings[i] as ThingWithComps;
					CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
					bool flag;
					if (compStyleable == null)
					{
						flag = (null != null);
					}
					else
					{
						Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
						if (sourcePrecept == null)
						{
							flag = (null != null);
						}
						else
						{
							Ideo ideo = sourcePrecept.ideo;
							flag = (((ideo != null) ? ideo.StructureMeme : null) != null);
						}
					}
					if (flag)
					{
						num++;
					}
				}
			}
			return (float)((num == 0) ? 0 : Mathf.Max(2000, num * 75));
		}

		// Token: 0x0600BAE6 RID: 47846 RVA: 0x0035F2A4 File Offset: 0x0035D4A4
		private Ideo DominatingIdeo(Room room)
		{
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i].def.isAltar)
				{
					ThingWithComps thingWithComps = containedAndAdjacentThings[i] as ThingWithComps;
					CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
					bool flag;
					if (compStyleable == null)
					{
						flag = (null != null);
					}
					else
					{
						Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
						if (sourcePrecept == null)
						{
							flag = (null != null);
						}
						else
						{
							Ideo ideo = sourcePrecept.ideo;
							flag = (((ideo != null) ? ideo.StructureMeme : null) != null);
						}
					}
					if (flag)
					{
						return compStyleable.SourcePrecept.ideo;
					}
				}
			}
			return null;
		}

		// Token: 0x040080B2 RID: 32946
		private const int MinScore = 2000;
	}
}
