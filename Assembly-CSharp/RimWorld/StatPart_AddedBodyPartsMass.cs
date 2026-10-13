using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020036DB RID: 14043
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		// Token: 0x060137C0 RID: 79808 RVA: 0x005C926C File Offset: 0x005C746C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x060137C1 RID: 79809 RVA: 0x005C928C File Offset: 0x005C748C
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetValue(req, out num) && num != 0f)
			{
				return "StatsReport_AddedBodyPartsMass".Translate() + ": " + num.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x060137C2 RID: 79810 RVA: 0x005C92D4 File Offset: 0x005C74D4
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => StatPart_AddedBodyPartsMass.GetAddedBodyPartsMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x060137C3 RID: 79811 RVA: 0x005C9328 File Offset: 0x005C7528
		private static float GetAddedBodyPartsMass(Pawn p)
		{
			float num = 0f;
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_AddedPart hediff_AddedPart = hediffs[i] as Hediff_AddedPart;
				if (hediff_AddedPart != null && hediff_AddedPart.def.spawnThingOnRemoved != null)
				{
					num += hediff_AddedPart.def.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.Mass, null) * 0.9f;
				}
			}
			return num;
		}

		// Token: 0x0400D87D RID: 55421
		private const float AddedBodyPartMassFactor = 0.9f;
	}
}
