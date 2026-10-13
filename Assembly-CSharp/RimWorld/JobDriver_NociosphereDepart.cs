using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012C3 RID: 4803
	public class JobDriver_NociosphereDepart : JobDriver
	{
		// Token: 0x060075A8 RID: 30120 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060075A9 RID: 30121 RVA: 0x0023EF13 File Offset: 0x0023D113
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = Toils_General.Wait(JobDriver_NociosphereDepart.LeaveTimeTicks, TargetIndex.None);
			toil.WithEffect(EffecterDefOf.NociosphereDeparting, TargetIndex.A, null);
			toil.PlaySustainerOrSound(SoundDefOf.Pawn_Nociosphere_Departing, 1f);
			Toil toil4 = toil;
			toil4.initAction = (Action)Delegate.Combine(toil4.initAction, new Action(delegate()
			{
				Messages.Message(toil.actor.GetComp<CompNociosphere>().Props.departingMessage, toil.actor, MessageTypeDefOf.NeutralEvent, true);
			}));
			yield return toil;
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			Toil toil3 = toil2;
			toil3.initAction = (Action)Delegate.Combine(toil3.initAction, new Action(delegate()
			{
				CompActivity comp = toil.actor.GetComp<CompActivity>();
				if (comp == null)
				{
					return;
				}
				comp.EnterPassiveState();
			}));
			toil2.AddFinishAction(delegate
			{
				EffecterDefOf.Skip_Entry.SpawnMaintained(toil.actor.PositionHeld, toil.actor.MapHeld, 2f);
				EffecterDefOf.NociosphereDepartComplete.Spawn(toil.actor.PositionHeld, toil.actor.MapHeld, 1f).Cleanup();
			});
			yield return toil2;
			yield break;
		}

		// Token: 0x0400508D RID: 20621
		private static readonly int LeaveTimeTicks = 720;
	}
}
