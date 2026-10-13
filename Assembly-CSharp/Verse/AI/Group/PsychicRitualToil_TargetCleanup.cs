using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200108C RID: 4236
	public class PsychicRitualToil_TargetCleanup : PsychicRitualToil
	{
		// Token: 0x060067A4 RID: 26532 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_TargetCleanup()
		{
		}

		// Token: 0x060067A5 RID: 26533 RVA: 0x0020D403 File Offset: 0x0020B603
		public PsychicRitualToil_TargetCleanup(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
		}

		// Token: 0x060067A6 RID: 26534 RVA: 0x0020D41C File Offset: 0x0020B61C
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			this.invoker = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			this.target = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
			Pawn pawn = this.invoker;
			if (pawn != null)
			{
				Pawn_JobTracker jobs = pawn.jobs;
				if (jobs != null)
				{
					jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			if (this.target != null)
			{
				if (!this.target.Dead && this.target.IsPrisonerOfColony)
				{
					WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(this.target, this.invoker);
					PawnUtility.ForceWait(this.target, 2500, null, true, false);
				}
			}
			else
			{
				Log.Warning("Tried to cleanup prisoner target for ritual " + psychicRitual.def.defName + ", but no target pawn was found.");
			}
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (this.invoker.IsPlayerControlled && this.target.Faction != null && this.target.HomeFaction != null && this.target.HomeFaction != Faction.OfPlayer && this.target.HomeFaction.def.humanlikeFaction && !this.target.Faction.def.PermanentlyHostileTo(FactionDefOf.PlayerColony))
			{
				this.target.HomeFaction.TryAffectGoodwillWith(Faction.OfPlayer, -25, true, true, HistoryEventDefOf.WasPsychicRitualTarget, null);
			}
			QuestUtility.SendQuestTargetSignals(this.target.questTags, "PsychicRitualTarget", this.target.Named("SUBJECT"));
		}

		// Token: 0x060067A7 RID: 26535 RVA: 0x0020D5A8 File Offset: 0x0020B7A8
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			if (this.target == null || this.target.Dead || !this.target.IsPrisonerOfColony)
			{
				psychicRitual.ReleaseAllPawnsAndBuildings();
				return true;
			}
			if (this.invoker.IsCarryingPawn(this.target))
			{
				psychicRitual.ReleaseAllPawnsAndBuildings();
				return true;
			}
			return false;
		}

		// Token: 0x060067A8 RID: 26536 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ClearJobOnStart(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return false;
		}

		// Token: 0x060067A9 RID: 26537 RVA: 0x0020D5FC File Offset: 0x0020B7FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_References.Look<Pawn>(ref this.invoker, "invoker", false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x040049D7 RID: 18903
		private const int TargetFactionGoodwillChange = -25;

		// Token: 0x040049D8 RID: 18904
		private PsychicRitualRoleDef targetRole;

		// Token: 0x040049D9 RID: 18905
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x040049DA RID: 18906
		private Pawn invoker;

		// Token: 0x040049DB RID: 18907
		private Pawn target;
	}
}
