using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002654 RID: 9812
	public class PreceptWorker_RitualSeat : PreceptWorker
	{
		// Token: 0x0600D70A RID: 55050 RVA: 0x003FC2DC File Offset: 0x003FA4DC
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			PreceptWorker_RitualSeat.tmpRitualSeats.Clear();
			List<ThingDef> list = null;
			foreach (MemeDef memeDef in ideo.memes)
			{
				if (!memeDef.requireAnyRitualSeat.NullOrEmpty<ThingDef>())
				{
					if (list == null)
					{
						PreceptWorker_RitualSeat.tmpRitualSeats.AddRange(memeDef.requireAnyRitualSeat);
						list = PreceptWorker_RitualSeat.tmpRitualSeats;
					}
					else
					{
						PreceptWorker_RitualSeat.tmpRitualSeatsMerge.Clear();
						foreach (ThingDef item in memeDef.requireAnyRitualSeat)
						{
							if (list.Contains(item))
							{
								PreceptWorker_RitualSeat.tmpRitualSeatsMerge.Add(item);
							}
						}
						list.Clear();
						list.AddRange(PreceptWorker_RitualSeat.tmpRitualSeatsMerge);
						if (list.Count == 0)
						{
							Log.Error("Ideo has 2 memes which have conflicting ritual set requirements!");
						}
					}
				}
			}
			return list == null || list.Contains(def);
		}

		// Token: 0x04009212 RID: 37394
		private static List<ThingDef> tmpRitualSeats = new List<ThingDef>();

		// Token: 0x04009213 RID: 37395
		private static List<ThingDef> tmpRitualSeatsMerge = new List<ThingDef>();
	}
}
