using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F54 RID: 12116
	public class CompUseEffect_AddHediffAndEnableDisappearsComp : CompUseEffect_AddHediff
	{
		// Token: 0x06011150 RID: 69968 RVA: 0x004E7D58 File Offset: 0x004E5F58
		public override void DoEffect(Pawn user)
		{
			user.health.AddHediff(base.Props.hediffDef, null, null, null).TryGetComp<HediffComp_DisappearsDisableable>().disabled = false;
		}
	}
}
