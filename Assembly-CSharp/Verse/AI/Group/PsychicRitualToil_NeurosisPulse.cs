using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001079 RID: 4217
	public class PsychicRitualToil_NeurosisPulse : PsychicRitualToil
	{
		// Token: 0x0600674C RID: 26444 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_NeurosisPulse()
		{
		}

		// Token: 0x0600674D RID: 26445 RVA: 0x0020B201 File Offset: 0x00209401
		public PsychicRitualToil_NeurosisPulse(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x0600674E RID: 26446 RVA: 0x0020B210 File Offset: 0x00209410
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float duration = ((PsychicRitualDef_NeurosisPulse)psychicRitual.def).durationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, duration);
			}
		}

		// Token: 0x0600674F RID: 26447 RVA: 0x0020B268 File Offset: 0x00209468
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float duration)
		{
			foreach (Pawn pawn in invoker.Map.mapPawns.AllHumanlikeSpawned)
			{
				if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > 0f)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PleasurePulse, false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.NeurosisPulse, pawn, null);
					HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
					if (hediffComp_Disappears != null)
					{
						hediffComp_Disappears.ticksToDisappear = Mathf.RoundToInt(duration * 60000f);
					}
					pawn.health.AddHediff(hediff, null, null, null);
				}
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "NeurosisPulseCompleteText".Translate(invoker, Mathf.FloorToInt(duration * 60000f).ToStringTicksToDays("F1"), psychicRitual.def.Named("RITUAL")), LetterDefOf.NeutralEvent, null, 0, true);
		}

		// Token: 0x06006750 RID: 26448 RVA: 0x0020B3AC File Offset: 0x002095AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x040049A5 RID: 18853
		private PsychicRitualRoleDef invokerRole;
	}
}
