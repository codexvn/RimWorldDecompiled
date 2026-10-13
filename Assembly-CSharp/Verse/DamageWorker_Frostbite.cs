using System;

namespace Verse
{
	// Token: 0x0200066E RID: 1646
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x0600296B RID: 10603 RVA: 0x000DA5F2 File Offset: 0x000D87F2
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
