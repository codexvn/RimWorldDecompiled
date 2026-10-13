using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DE8 RID: 11752
	public class CompRevenantSpine : CompInteractable
	{
		// Token: 0x060108AF RID: 67759 RVA: 0x004C5468 File Offset: 0x004C3668
		protected override void OnInteracted(Pawn caster)
		{
			CompUsable comp = this.parent.GetComp<CompUsable>();
			comp.TryStartUseJob(caster, comp.GetExtraTarget(caster), false);
		}

		// Token: 0x060108B0 RID: 67760 RVA: 0x004C5490 File Offset: 0x004C3690
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}
	}
}
