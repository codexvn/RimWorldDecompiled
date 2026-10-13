using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CA RID: 6346
	public class ThoughtWorker_HateAura : ThoughtWorker
	{
		// Token: 0x060090B8 RID: 37048 RVA: 0x002A9FEC File Offset: 0x002A81EC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtWorker_HateAura.HateAuraLevel hateAuraLevel = ThoughtWorker_HateAura.HateAuraLevel.None;
			if (p.Map != null)
			{
				List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.Mech_Apocriton);
				list.SortBy((Thing m) => m.Position.DistanceToSquared(m.Position));
				if (list.Count > 0)
				{
					float num = list[0].Position.DistanceTo(p.Position);
					if (num <= 6.9f)
					{
						hateAuraLevel = ThoughtWorker_HateAura.HateAuraLevel.Intense;
					}
					else if (num <= 15.9f)
					{
						hateAuraLevel = ThoughtWorker_HateAura.HateAuraLevel.Strong;
					}
					else
					{
						hateAuraLevel = ThoughtWorker_HateAura.HateAuraLevel.Distant;
					}
				}
			}
			switch (hateAuraLevel)
			{
			case ThoughtWorker_HateAura.HateAuraLevel.None:
				return false;
			case ThoughtWorker_HateAura.HateAuraLevel.Intense:
				return ThoughtState.ActiveAtStage(0);
			case ThoughtWorker_HateAura.HateAuraLevel.Strong:
				return ThoughtState.ActiveAtStage(1);
			case ThoughtWorker_HateAura.HateAuraLevel.Distant:
				return ThoughtState.ActiveAtStage(2);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x04005C51 RID: 23633
		private const float IntenseDistance = 6.9f;

		// Token: 0x04005C52 RID: 23634
		private const float StrongDistance = 15.9f;

		// Token: 0x020018CB RID: 6347
		private enum HateAuraLevel
		{
			// Token: 0x04005C54 RID: 23636
			None,
			// Token: 0x04005C55 RID: 23637
			Intense,
			// Token: 0x04005C56 RID: 23638
			Strong,
			// Token: 0x04005C57 RID: 23639
			Distant
		}
	}
}
