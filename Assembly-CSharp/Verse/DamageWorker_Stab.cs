using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000671 RID: 1649
	public class DamageWorker_Stab : DamageWorker_AddInjury
	{
		// Token: 0x06002972 RID: 10610 RVA: 0x000DA828 File Offset: 0x000D8A28
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			BodyPartRecord randomNotMissingPart = pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, dinfo.Depth, null);
			if (randomNotMissingPart.depth != BodyPartDepth.Inside && Rand.Chance(this.def.stabChanceOfForcedInternal))
			{
				BodyPartRecord randomNotMissingPart2 = pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, BodyPartHeight.Undefined, BodyPartDepth.Inside, randomNotMissingPart);
				if (randomNotMissingPart2 != null)
				{
					return randomNotMissingPart2;
				}
			}
			return randomNotMissingPart;
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x000DA898 File Offset: 0x000D8A98
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage = base.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn);
			List<BodyPartRecord> list = new List<BodyPartRecord>();
			for (BodyPartRecord bodyPartRecord = dinfo.HitPart; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				list.Add(bodyPartRecord);
				if (bodyPartRecord.depth == BodyPartDepth.Outside)
				{
					break;
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				BodyPartRecord bodyPartRecord2 = list[i];
				float totalDamage2;
				if (list.Count == 1)
				{
					totalDamage2 = totalDamage;
				}
				else
				{
					totalDamage2 = ((bodyPartRecord2.depth == BodyPartDepth.Outside) ? (totalDamage * 0.75f) : (totalDamage * 0.4f));
				}
				DamageInfo dinfo2 = dinfo;
				dinfo2.SetHitPart(bodyPartRecord2);
				base.FinalizeAndAddInjury(pawn, totalDamage2, dinfo2, result);
			}
		}

		// Token: 0x040021BF RID: 8639
		private const float DamageFractionOnOuterParts = 0.75f;

		// Token: 0x040021C0 RID: 8640
		private const float DamageFractionOnInnerParts = 0.4f;
	}
}
