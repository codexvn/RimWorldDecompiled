using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Sound;

namespace Verse.AI.Group
{
	// Token: 0x0200107F RID: 4223
	public class PsychicRitualToil_SkipAbduction : PsychicRitualToil
	{
		// Token: 0x06006764 RID: 26468 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_SkipAbduction()
		{
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x0020BF21 File Offset: 0x0020A121
		public PsychicRitualToil_SkipAbduction(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x0020BF30 File Offset: 0x0020A130
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn);
			}
		}

		// Token: 0x06006767 RID: 26471 RVA: 0x0020BF64 File Offset: 0x0020A164
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker)
		{
			List<Pawn> freeColonistsSpawned = psychicRitual.Map.mapPawns.FreeColonistsSpawned;
			freeColonistsSpawned.AddRange(psychicRitual.Map.mapPawns.SlavesOfColonySpawned);
			freeColonistsSpawned.RemoveAll((Pawn p) => p.health.hediffSet.HasHediff(HediffDefOf.DarkPsychicShock, false));
			Pawn pawn;
			if (!freeColonistsSpawned.TryRandomElement(out pawn))
			{
				return;
			}
			psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(pawn, pawn.Map, 1f), pawn.PositionHeld, 60);
			SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(pawn.PositionHeld, pawn.Map, false));
			IntVec3 cell = psychicRitual.assignments.Target.Cell;
			if (!cell.Standable(psychicRitual.Map))
			{
				CellFinder.TryFindRandomSpawnCellForPawnNear(invoker.PositionHeld, psychicRitual.Map, out cell, 4, null);
			}
			SkipUtility.SkipTo(pawn, cell, psychicRitual.Map);
			psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(cell, psychicRitual.Map, 1f), cell, 60);
			SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(cell, psychicRitual.Map, false));
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, pawn, null);
			hediff.TryGetComp<HediffComp_Disappears>().disappearsAfterTicks = 30000;
			pawn.health.AddHediff(hediff, null, null, null);
			foreach (Pawn otherPawn in psychicRitual.assignments.AllAssignedPawns)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						ThoughtHandler thoughts = mood.thoughts;
						if (thoughts != null)
						{
							MemoryThoughtHandler memories = thoughts.memories;
							if (memories != null)
							{
								memories.TryGainMemory(ThoughtDefOf.UsedMeForPsychicRitual, otherPawn, null);
							}
						}
					}
				}
			}
			if (!psychicRitual.def.letterAICompleteLabel.NullOrEmpty() && !psychicRitual.def.letterAICompleteText.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(psychicRitual.def.letterAICompleteLabel, psychicRitual.def.letterAICompleteText.Formatted(pawn.Named("PAWN")), LetterDefOf.ThreatBig, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06006768 RID: 26472 RVA: 0x0020C1C0 File Offset: 0x0020A3C0
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (Pawn pawn in psychicRitual.assignments.AllAssignedPawns)
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
			}
		}

		// Token: 0x06006769 RID: 26473 RVA: 0x0020C244 File Offset: 0x0020A444
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x040049B1 RID: 18865
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x040049B2 RID: 18866
		private const int ComaDurationTicks = 30000;
	}
}
