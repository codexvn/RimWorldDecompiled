using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189D RID: 6301
	public class ThoughtWorker_PsychicEmanatorSoothe : ThoughtWorker
	{
		// Token: 0x0600902C RID: 36908 RVA: 0x002A8690 File Offset: 0x002A6890
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.PsychicEmanator);
			for (int i = 0; i < list.Count; i++)
			{
				CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
				if ((compPowerTrader == null || compPowerTrader.PowerOn) && p.Position.InHorDistOf(list[i].Position, 15f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005C37 RID: 23607
		private const float Radius = 15f;
	}
}
