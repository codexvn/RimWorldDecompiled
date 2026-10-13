using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BA RID: 6330
	public abstract class ThoughtWorker_PsychicArchotechEmanator : ThoughtWorker
	{
		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x0600908B RID: 37003
		protected abstract ThingDef EmanatorDef { get; }

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x0600908C RID: 37004
		protected abstract int InnerRadius { get; }

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x0600908D RID: 37005
		protected abstract int OuterRadius { get; }

		// Token: 0x0600908E RID: 37006 RVA: 0x002A98A8 File Offset: 0x002A7AA8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsOfDef(this.EmanatorDef);
			for (int i = 0; i < list.Count; i++)
			{
				if (p.Position.InHorDistOf(list[i].Position, (float)this.InnerRadius))
				{
					return ThoughtState.ActiveAtStage(0);
				}
				if (p.Position.InHorDistOf(list[i].Position, (float)this.OuterRadius))
				{
					return ThoughtState.ActiveAtStage(1);
				}
			}
			return false;
		}
	}
}
