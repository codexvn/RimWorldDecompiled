using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012F5 RID: 4853
	public class JobDriver_RemoveMechlink : JobDriver
	{
		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x0600770C RID: 30476 RVA: 0x00243608 File Offset: 0x00241808
		protected Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600770D RID: 30477 RVA: 0x0024362E File Offset: 0x0024182E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600770E RID: 30478 RVA: 0x00243651 File Offset: 0x00241851
		public override string GetReport()
		{
			return "ReportRemovingMechlink".Translate(HediffDefOf.MechlinkImplant.label);
		}

		// Token: 0x0600770F RID: 30479 RVA: 0x00243671 File Offset: 0x00241871
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckBiotech("Remove mechlink"))
			{
				yield break;
			}
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOn(() => !this.Corpse.InnerPawn.health.hediffSet.HasHediff(HediffDefOf.MechlinkImplant, false));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			Toil toil = Toils_General.Wait(300, TargetIndex.A).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).WithEffect(() => EffecterDefOf.Surgery, TargetIndex.A, null).PlaySustainerOrSound(SoundDefOf.Recipe_Surgery, 1f).PlaySoundAtEnd(SoundDefOf.Mechlink_Removed);
			toil.handlingFacing = true;
			yield return toil;
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				Pawn innerPawn = this.Corpse.InnerPawn;
				List<Hediff> hediffs = innerPawn.health.hediffSet.hediffs;
				for (int i = hediffs.Count - 1; i >= 0; i--)
				{
					if (hediffs[i].def == HediffDefOf.MechlinkImplant)
					{
						innerPawn.health.RemoveHediff(hediffs[i]);
					}
				}
				Mechlink mechlink = (Mechlink)ThingMaker.MakeThing(ThingDefOf.Mechlink, null);
				mechlink.sentMechsToPlayer = (innerPawn.IsColonist || innerPawn.IsPrisoner || innerPawn.IsSlave || innerPawn.IsQuestLodger());
				GenPlace.TryPlaceThing(mechlink, this.Corpse.Position, base.Map, ThingPlaceMode.Near, null, null, null, 1);
			};
			toil2.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil2;
			yield break;
		}

		// Token: 0x0400513F RID: 20799
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04005140 RID: 20800
		public const int RemoveTicks = 300;
	}
}
