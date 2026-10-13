using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200118D RID: 4493
	public class JobDriver_SelfDetonate : JobDriver
	{
		// Token: 0x06006DE0 RID: 28128 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006DE1 RID: 28129 RVA: 0x0022574F File Offset: 0x0022394F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			OverlayHandle? overlayBurningWick = null;
			Toil warmupToil = Toils_General.Wait(120, TargetIndex.None);
			warmupToil.initAction = delegate()
			{
				Pawn actor = warmupToil.actor;
				actor.Map.overlayDrawer.Disable(actor, ref overlayBurningWick);
				overlayBurningWick = new OverlayHandle?(actor.Map.overlayDrawer.Enable(actor, OverlayTypes.BurningWick));
			};
			warmupToil.AddFinishAction(delegate
			{
				Pawn actor = warmupToil.actor;
				actor.Map.overlayDrawer.Disable(actor, ref overlayBurningWick);
			});
			warmupToil.PlaySustainerOrSound(SoundDefOf.HissSmall, 1f);
			yield return warmupToil;
			Toil toil = Toils_General.Do(delegate
			{
				warmupToil.actor.Kill(null, null);
			});
			yield return toil;
			yield break;
		}
	}
}
