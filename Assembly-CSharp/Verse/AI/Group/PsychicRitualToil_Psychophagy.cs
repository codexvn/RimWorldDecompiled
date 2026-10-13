using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x0200107D RID: 4221
	public class PsychicRitualToil_Psychophagy : PsychicRitualToil
	{
		// Token: 0x0600675D RID: 26461 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_Psychophagy()
		{
		}

		// Token: 0x0600675E RID: 26462 RVA: 0x0020BAD0 File Offset: 0x00209CD0
		public PsychicRitualToil_Psychophagy(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole, FloatRange brainDamageRange)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
			this.brainDamageRange = brainDamageRange;
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x0020BAF0 File Offset: 0x00209CF0
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			PsychicRitualDef_Psychophagy psychicRitualDef_Psychophagy = (PsychicRitualDef_Psychophagy)psychicRitual.def;
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
			this.effectDurationTicks = Mathf.RoundToInt(psychicRitualDef_Psychophagy.effectDurationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent) * 60000f);
			if (pawn != null && pawn2 != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, pawn2);
			}
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x0020BB60 File Offset: 0x00209D60
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target)
		{
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, target, null);
			target.health.AddHediff(hediff, null, null, null);
			Hediff hediff2 = HediffMaker.MakeHediff(HediffDefOf.PsychicallyDead, target, null);
			target.health.AddHediff(hediff2, null, null, null);
			Pawn_NeedsTracker needs = target.needs;
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
							memories.TryGainMemory(ThoughtDefOf.PsychicRitualVictim, null, null);
						}
					}
				}
			}
			foreach (Pawn otherPawn in psychicRitual.assignments.AllAssignedPawns.Except(target))
			{
				Pawn_NeedsTracker needs2 = target.needs;
				if (needs2 != null)
				{
					Need_Mood mood2 = needs2.mood;
					if (mood2 != null)
					{
						ThoughtHandler thoughts2 = mood2.thoughts;
						if (thoughts2 != null)
						{
							MemoryThoughtHandler memories2 = thoughts2.memories;
							if (memories2 != null)
							{
								memories2.TryGainMemory(ThoughtDefOf.UsedMeForPsychicRitual, otherPawn, null);
							}
						}
					}
				}
			}
			BodyPartRecord brain = target.health.hediffSet.GetBrain();
			if (brain != null)
			{
				target.TakeDamage(new DamageInfo(DamageDefOf.Psychic, this.brainDamageRange.RandomInRange, 0f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			Hediff hediff3 = invoker.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Psychophage, false);
			if (hediff3 == null)
			{
				hediff3 = HediffMaker.MakeHediff(HediffDefOf.Psychophage, invoker, null);
				HediffComp_Disappears hediffComp_Disappears = hediff3.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears != null)
				{
					hediffComp_Disappears.ticksToDisappear = this.effectDurationTicks;
				}
				invoker.health.AddHediff(hediff3, null, null, null);
			}
			else
			{
				HediffComp_Disappears hediffComp_Disappears2 = hediff3.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears2 != null)
				{
					hediffComp_Disappears2.ticksToDisappear += this.effectDurationTicks;
				}
			}
			if (target.Dead)
			{
				PsychicRitualUtility.RegisterAsExecutionIfPrisoner(target, invoker);
			}
			PsychicRitualUtility.AddPsychicRitualGuiltToPawns(psychicRitual.def, from p in psychicRitual.Map.mapPawns.FreeColonistsSpawned
			where p != target
			select p);
			TaggedString taggedString = "PsychophagyCompleteText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL"), target.Named("TARGET"));
			if (target.Dead)
			{
				taggedString += "\n\n" + "PsychicRitualTargetBrainLiquified".Translate(target.Named("TARGET"));
			}
			else
			{
				taggedString += "\n\n" + "PsychophagyTargetSurvived".Translate(target.Named("TARGET"));
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), taggedString, LetterDefOf.NeutralEvent, new LookTargets(new TargetInfo[]
			{
				invoker,
				target
			}), null, null, null, null, 0, true);
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x0020BEB4 File Offset: 0x0020A0B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_Values.Look<FloatRange>(ref this.brainDamageRange, "brainDamageRange", default(FloatRange), false);
			Scribe_Values.Look<int>(ref this.effectDurationTicks, "effectDurationTicks", 0, false);
		}

		// Token: 0x040049AC RID: 18860
		public PsychicRitualRoleDef targetRole;

		// Token: 0x040049AD RID: 18861
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x040049AE RID: 18862
		public FloatRange brainDamageRange;

		// Token: 0x040049AF RID: 18863
		public int effectDurationTicks;
	}
}
