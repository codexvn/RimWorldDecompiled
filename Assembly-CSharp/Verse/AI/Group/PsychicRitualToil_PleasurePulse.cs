using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x0200107C RID: 4220
	public class PsychicRitualToil_PleasurePulse : PsychicRitualToil
	{
		// Token: 0x06006758 RID: 26456 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_PleasurePulse()
		{
		}

		// Token: 0x06006759 RID: 26457 RVA: 0x0020B90D File Offset: 0x00209B0D
		public PsychicRitualToil_PleasurePulse(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x0600675A RID: 26458 RVA: 0x0020B91C File Offset: 0x00209B1C
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float duration = ((PsychicRitualDef_PleasurePulse)psychicRitual.def).durationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, duration);
			}
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x0020B974 File Offset: 0x00209B74
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float duration)
		{
			foreach (Pawn pawn in invoker.Map.mapPawns.AllHumanlikeSpawned)
			{
				if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > 0f)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.NeurosisPulse, false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.PleasurePulse, pawn, null);
					HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
					if (hediffComp_Disappears != null)
					{
						hediffComp_Disappears.ticksToDisappear = Mathf.RoundToInt(duration * 60000f);
					}
					pawn.health.AddHediff(hediff, null, null, null);
				}
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "PleasurePulseCompleteText".Translate(invoker, Mathf.FloorToInt(duration * 60000f).ToStringTicksToDays("F1"), psychicRitual.def.Named("RITUAL")), LetterDefOf.NeutralEvent, null, 0, true);
		}

		// Token: 0x0600675C RID: 26460 RVA: 0x0020BAB8 File Offset: 0x00209CB8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x040049AB RID: 18859
		private PsychicRitualRoleDef invokerRole;
	}
}
