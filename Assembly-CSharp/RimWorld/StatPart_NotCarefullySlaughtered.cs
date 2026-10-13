using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003711 RID: 14097
	public class StatPart_NotCarefullySlaughtered : StatPart
	{
		// Token: 0x060138BE RID: 80062 RVA: 0x005CC0B0 File Offset: 0x005CA2B0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.HasWounds(req))
			{
				val *= this.factor;
			}
		}

		// Token: 0x060138BF RID: 80063 RVA: 0x005CC0C6 File Offset: 0x005CA2C6
		public override string ExplanationPart(StatRequest req)
		{
			if (this.HasWounds(req))
			{
				return "StatsReport_HasHediffExplanation".Translate() + ": x" + this.factor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x060138C0 RID: 80064 RVA: 0x005CC0FC File Offset: 0x005CA2FC
		private bool HasWounds(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
					for (int i = 0; i < hediffs.Count; i++)
					{
						if (hediffs[i].def != HediffDefOf.ExecutionCut && typeof(Hediff_Injury).IsAssignableFrom(hediffs[i].def.hediffClass) && !hediffs[i].IsPermanent())
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x0400D8D8 RID: 55512
		private float factor;
	}
}
