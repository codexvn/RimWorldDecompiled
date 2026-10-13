using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017CD RID: 6093
	public class ThinkNode_ConditionalTotalDamage : ThinkNode_Conditional
	{
		// Token: 0x06008DEE RID: 36334 RVA: 0x002A18C8 File Offset: 0x0029FAC8
		protected override bool Satisfied(Pawn pawn)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			float num = 0f;
			for (int i = 0; i < hediffSet.hediffs.Count; i++)
			{
				if (hediffSet.hediffs[i] is Hediff_Injury)
				{
					num += hediffSet.hediffs[i].Severity;
				}
			}
			return num / pawn.health.LethalDamageThreshold > this.thresholdPercent;
		}

		// Token: 0x04005BE0 RID: 23520
		public float thresholdPercent = 0.25f;
	}
}
