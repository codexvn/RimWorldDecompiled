using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E6F RID: 3695
	public class Verb_CastTargetEffect : Verb_CastBase
	{
		// Token: 0x06005D20 RID: 23840 RVA: 0x001DA9B8 File Offset: 0x001D8BB8
		protected override bool TryCastShot()
		{
			Pawn casterPawn = this.CasterPawn;
			Thing thing = this.currentTarget.Thing;
			if (casterPawn == null || thing == null)
			{
				return false;
			}
			foreach (CompTargetEffect compTargetEffect in base.EquipmentSource.GetComps<CompTargetEffect>())
			{
				compTargetEffect.DoEffectOn(casterPawn, thing);
			}
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}
	}
}
