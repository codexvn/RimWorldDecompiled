using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200108A RID: 4234
	public class PsychicRitualToil_SummonPitGate : PsychicRitualToil
	{
		// Token: 0x06006799 RID: 26521 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_SummonPitGate()
		{
		}

		// Token: 0x0600679A RID: 26522 RVA: 0x0020D1B3 File Offset: 0x0020B3B3
		public PsychicRitualToil_SummonPitGate(PsychicRitualRoleDef invokerRole, FloatRange combatPointMultiplierFromQualityRange)
		{
			this.invokerRole = invokerRole;
			this.combatPointMultiplierFromQualityRange = combatPointMultiplierFromQualityRange;
		}

		// Token: 0x0600679B RID: 26523 RVA: 0x0020D1CC File Offset: 0x0020B3CC
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float combatPointMultiplier = this.combatPointMultiplierFromQualityRange.LerpThroughRange(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, combatPointMultiplier);
			}
		}

		// Token: 0x0600679C RID: 26524 RVA: 0x0020D218 File Offset: 0x0020B418
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float combatPointMultiplier)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = invoker.Map;
			incidentParms.pointMultiplier = combatPointMultiplier;
			incidentParms.forced = true;
			Find.Storyteller.incidentQueue.Add(IncidentDefOf.PitGate, Find.TickManager.TicksGame, incidentParms, 0);
		}

		// Token: 0x0600679D RID: 26525 RVA: 0x0020D268 File Offset: 0x0020B468
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Values.Look<FloatRange>(ref this.combatPointMultiplierFromQualityRange, "combatPointMultiplierFromQualityRange", default(FloatRange), false);
		}

		// Token: 0x040049D2 RID: 18898
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x040049D3 RID: 18899
		private FloatRange combatPointMultiplierFromQualityRange;
	}
}
