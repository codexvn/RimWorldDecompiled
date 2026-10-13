using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F6F RID: 12143
	public class CompUseEffect_StartWick : CompUseEffect
	{
		// Token: 0x060111AC RID: 70060 RVA: 0x004E916B File Offset: 0x004E736B
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
