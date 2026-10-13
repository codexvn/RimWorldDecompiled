using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200108B RID: 4235
	public class PsychicRitualToil_SummonShamblers : PsychicRitualToil
	{
		// Token: 0x0600679E RID: 26526 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_SummonShamblers()
		{
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x0020D2A5 File Offset: 0x0020B4A5
		public PsychicRitualToil_SummonShamblers(PsychicRitualRoleDef invokerRole, FloatRange combatPointsFromQualityRange)
		{
			this.invokerRole = invokerRole;
			this.combatPointsFromQualityRange = combatPointsFromQualityRange;
		}

		// Token: 0x060067A0 RID: 26528 RVA: 0x0020D2BC File Offset: 0x0020B4BC
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float combatPoints = this.combatPointsFromQualityRange.LerpThroughRange(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, combatPoints);
			}
		}

		// Token: 0x060067A1 RID: 26529 RVA: 0x0020D308 File Offset: 0x0020B508
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float combatPoints)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = invoker.Map;
			incidentParms.points = combatPoints;
			incidentParms.forced = true;
			Find.Storyteller.incidentQueue.Add(IncidentDefOf.ShamblerSwarm, Find.TickManager.TicksGame + PsychicRitualToil_SummonShamblers.ShamblersDelayTicks.RandomInRange, incidentParms, 0);
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "SummonShamblersCompleteText".Translate(invoker, psychicRitual.def.Named("RITUAL")), LetterDefOf.ThreatBig, null, 0, true);
		}

		// Token: 0x060067A2 RID: 26530 RVA: 0x0020D3B0 File Offset: 0x0020B5B0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Values.Look<FloatRange>(ref this.combatPointsFromQualityRange, "combatPointsFromQualityRange", default(FloatRange), false);
		}

		// Token: 0x040049D4 RID: 18900
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x040049D5 RID: 18901
		private FloatRange combatPointsFromQualityRange;

		// Token: 0x040049D6 RID: 18902
		private static readonly IntRange ShamblersDelayTicks = new IntRange(2500, 7500);
	}
}
