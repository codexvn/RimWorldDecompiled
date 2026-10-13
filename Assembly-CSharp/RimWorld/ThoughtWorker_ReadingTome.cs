using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F1 RID: 6385
	public class ThoughtWorker_ReadingTome : ThoughtWorker
	{
		// Token: 0x06009132 RID: 37170 RVA: 0x002AB558 File Offset: 0x002A9758
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (p.CurJobDef != JobDefOf.Reading)
			{
				return false;
			}
			return p.CurJob.targetA.Thing.def == ThingDefOf.Tome;
		}
	}
}
