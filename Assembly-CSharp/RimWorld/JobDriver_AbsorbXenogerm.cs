using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001210 RID: 4624
	public class JobDriver_AbsorbXenogerm : JobDriver
	{
		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x060070F6 RID: 28918 RVA: 0x001E5D9E File Offset: 0x001E3F9E
		public Pawn Target
		{
			get
			{
				return this.job.targetA.Thing as Pawn;
			}
		}

		// Token: 0x060070F7 RID: 28919 RVA: 0x0022FD5B File Offset: 0x0022DF5B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060070F8 RID: 28920 RVA: 0x0022FD7E File Offset: 0x0022DF7E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckBiotech("xenogerm absorbing"))
			{
				yield break;
			}
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.WaitWith(TargetIndex.A, 60, true, false, false, TargetIndex.None, PathEndMode.Touch);
			yield return Toils_General.Do(delegate
			{
				if (this.Target.HomeFaction != null && this.pawn.Faction == Faction.OfPlayer)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(this.Target.HomeFaction, -50, true, !this.Target.HomeFaction.temporary, HistoryEventDefOf.AbsorbedXenogerm, null);
				}
				QuestUtility.SendQuestTargetSignals(this.Target.questTags, "XenogermAbsorbed", this.Target.Named("SUBJECT"));
				GeneUtility.ReimplantXenogerm(this.Target, this.pawn);
			});
			yield break;
		}

		// Token: 0x04004E65 RID: 20069
		public const int TicksToAbsorb = 60;
	}
}
