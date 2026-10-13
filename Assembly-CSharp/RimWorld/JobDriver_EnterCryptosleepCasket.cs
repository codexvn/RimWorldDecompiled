using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001261 RID: 4705
	public class JobDriver_EnterCryptosleepCasket : JobDriver
	{
		// Token: 0x0600731D RID: 29469 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600731E RID: 29470 RVA: 0x00236B1F File Offset: 0x00234D1F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			Toil toil = Toils_General.Wait(500, TargetIndex.None);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return toil;
			Toil enter = ToilMaker.MakeToil("MakeNewToils");
			enter.initAction = delegate()
			{
				Pawn actor = enter.actor;
				Building_CryptosleepCasket pod = (Building_CryptosleepCasket)actor.CurJob.targetA.Thing;
				Action action = delegate()
				{
					bool flag = actor.DeSpawnOrDeselect(DestroyMode.Vanish);
					if (pod.TryAcceptThing(actor, true) && flag)
					{
						Find.Selector.Select(actor, false, false);
					}
				};
				if (pod.def.building.isPlayerEjectable)
				{
					action();
					return;
				}
				if (this.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate(actor.Named("PAWN")).AdjustedFor(actor, "PAWN", true), action, false, null, WindowLayer.Dialog));
					return;
				}
				action();
			};
			enter.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return enter;
			yield break;
		}
	}
}
