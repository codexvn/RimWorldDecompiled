using System;

namespace Verse
{
	// Token: 0x02000666 RID: 1638
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x06002946 RID: 10566 RVA: 0x000D999A File Offset: 0x000D7B9A
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside, null);
		}
	}
}
