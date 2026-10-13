using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02001084 RID: 4228
	public class PsychicRitualToil_SummonAnimals : PsychicRitualToil
	{
		// Token: 0x06006779 RID: 26489 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_SummonAnimals()
		{
		}

		// Token: 0x0600677A RID: 26490 RVA: 0x0020C7DA File Offset: 0x0020A9DA
		public PsychicRitualToil_SummonAnimals(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x0600677B RID: 26491 RVA: 0x0020C7EC File Offset: 0x0020A9EC
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float manhunterChance = ((PsychicRitualDef_SummonAnimals)psychicRitual.def).manhunterSpawnChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, manhunterChance);
			}
		}

		// Token: 0x0600677C RID: 26492 RVA: 0x0020C844 File Offset: 0x0020AA44
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float manhunterChance)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = invoker.Map;
			incidentParms.forced = true;
			LetterDef textLetterDef;
			TaggedString text;
			if (Rand.Chance(manhunterChance))
			{
				textLetterDef = LetterDefOf.ThreatBig;
				text = "SummonAnimalsFailureText".Translate(invoker, psychicRitual.def.Named("RITUAL"));
				incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(invoker.Map);
				Find.Storyteller.incidentQueue.Add(IncidentDefOf.FrenziedAnimals, Find.TickManager.TicksGame + PsychicRitualToil_SummonAnimals.AnimalsDelayTicks.RandomInRange, incidentParms, 0);
			}
			else
			{
				textLetterDef = LetterDefOf.NeutralEvent;
				text = "SummonAnimalsSuccessText".Translate(invoker, psychicRitual.def.Named("RITUAL"));
				Find.Storyteller.incidentQueue.Add(IncidentDefOf.HerdMigration, Find.TickManager.TicksGame + PsychicRitualToil_SummonAnimals.AnimalsDelayTicks.RandomInRange, incidentParms, 0);
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), text, textLetterDef, null, 0, true);
		}

		// Token: 0x0600677D RID: 26493 RVA: 0x0020C95A File Offset: 0x0020AB5A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x040049BB RID: 18875
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x040049BC RID: 18876
		private static readonly IntRange AnimalsDelayTicks = new IntRange(2500, 7500);
	}
}
