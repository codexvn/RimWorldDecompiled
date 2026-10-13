using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001069 RID: 4201
	public class PsychicRitualToil_Chronophagy : PsychicRitualToil
	{
		// Token: 0x060066B7 RID: 26295 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_Chronophagy()
		{
		}

		// Token: 0x060066B8 RID: 26296 RVA: 0x00207A3A File Offset: 0x00205C3A
		public PsychicRitualToil_Chronophagy(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole, FloatRange yearsTransferredFromQualityRange)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
			this.yearsTransferredFromQualityRange = yearsTransferredFromQualityRange;
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x00207A58 File Offset: 0x00205C58
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
			float years = this.yearsTransferredFromQualityRange.LerpThroughRange(psychicRitual.PowerPercent);
			if (pawn != null && pawn2 != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, pawn2, years);
			}
		}

		// Token: 0x060066BA RID: 26298 RVA: 0x00207AAC File Offset: 0x00205CAC
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target, float years)
		{
			List<Hediff> curedHediffs;
			float invokerAgeDelta;
			PsychicRitualToil_Chronophagy.ReverseAgePawn(invoker, years, out curedHediffs, out invokerAgeDelta);
			List<Hediff> gainedHediffs;
			bool diedOfOldAge;
			bool diedOfBrainDamage;
			PsychicRitualToil_Chronophagy.AgePawn(psychicRitual, target, years, out gainedHediffs, out diedOfOldAge, out diedOfBrainDamage);
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, target, null);
			target.health.AddHediff(hediff, null, null, null);
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
			if (target.Dead)
			{
				PsychicRitualUtility.RegisterAsExecutionIfPrisoner(target, invoker);
			}
			PsychicRitualUtility.AddPsychicRitualGuiltToPawns(psychicRitual.def, from p in psychicRitual.Map.mapPawns.FreeColonistsSpawned
			where p != target
			select p);
			PsychicRitualToil_Chronophagy.GenerateLetter(psychicRitual, invoker, target, years, invokerAgeDelta, diedOfBrainDamage, diedOfOldAge, gainedHediffs, curedHediffs);
		}

		// Token: 0x060066BB RID: 26299 RVA: 0x00207C40 File Offset: 0x00205E40
		private static void ReverseAgePawn(Pawn pawn, float years, out List<Hediff> removedHediffs, out float pawnAgeDelta)
		{
			removedHediffs = new List<Hediff>();
			float num = Mathf.Max(pawn.ageTracker.AgeBiologicalYearsFloat - years, 13f);
			pawnAgeDelta = pawn.ageTracker.AgeBiologicalYearsFloat - num;
			pawn.ageTracker.AgeBiologicalTicks = (long)Mathf.RoundToInt(num * 3600000f);
			pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.ViaTreatment, false);
			List<HediffGiverSetDef> hediffGiverSets = pawn.RaceProps.hediffGiverSets;
			if (hediffGiverSets == null)
			{
				return;
			}
			List<Hediff> list = new List<Hediff>();
			foreach (HediffGiverSetDef hediffGiverSetDef in hediffGiverSets)
			{
				List<HediffGiver> hediffGivers = hediffGiverSetDef.hediffGivers;
				if (hediffGivers != null)
				{
					foreach (HediffGiver hediffGiver in hediffGivers)
					{
						HediffGiver_Birthday agb = hediffGiver as HediffGiver_Birthday;
						if (agb != null)
						{
							float num2 = num / pawn.RaceProps.lifeExpectancy;
							float x = agb.ageFractionChanceCurve.Points[0].x;
							if (num2 < x)
							{
								pawn.health.hediffSet.GetHediffs<Hediff>(ref list, (Hediff hd) => hd.def == agb.hediff);
								foreach (Hediff hediff in list)
								{
									pawn.health.RemoveHediff(hediff);
									removedHediffs.Add(hediff);
								}
							}
						}
					}
				}
			}
			int num3 = Rand.RangeInclusive(1, 2);
			for (int i = pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff2 = pawn.health.hediffSet.hediffs[i];
				if (hediff2 is Hediff_Injury && hediff2.IsPermanent())
				{
					pawn.health.RemoveHediff(hediff2);
					removedHediffs.Add(hediff2);
					num3--;
					if (num3 <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x00207EA0 File Offset: 0x002060A0
		private static void AgePawn(PsychicRitual psychicRitual, Pawn pawn, float years, out List<Hediff> gainedHediffs, out bool diedOfOldAge, out bool diedOfBrainDamage)
		{
			List<HediffGiver_Birthday> list = new List<HediffGiver_Birthday>();
			gainedHediffs = new List<Hediff>();
			diedOfBrainDamage = false;
			BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
			if (brain != null)
			{
				PsychicRitualDef_Chronophagy psychicRitualDef_Chronophagy = (PsychicRitualDef_Chronophagy)psychicRitual.def;
				pawn.TakeDamage(new DamageInfo(DamageDefOf.Psychic, psychicRitualDef_Chronophagy.brainDamageFromAgeCurve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears), 0f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				diedOfBrainDamage = pawn.Dead;
			}
			int num = 1;
			while ((float)num <= years)
			{
				list.AddRange(AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn, (float)(pawn.ageTracker.AgeBiologicalYears + num)));
				num++;
			}
			foreach (HediffGiver_Birthday hediffGiver_Birthday in list)
			{
				hediffGiver_Birthday.TryApply(pawn, gainedHediffs);
			}
			pawn.ageTracker.AgeBiologicalTicks += (long)Mathf.RoundToInt(years * 3600000f);
			diedOfOldAge = (!diedOfBrainDamage && pawn.Dead);
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x00207FC0 File Offset: 0x002061C0
		private static void GenerateLetter(PsychicRitual psychicRitual, Pawn invoker, Pawn target, float years, float invokerAgeDelta, bool diedOfBrainDamage, bool diedOfOldAge, List<Hediff> gainedHediffs, List<Hediff> curedHediffs)
		{
			TaggedString taggedString = "ChronophagyCompleteText".Translate(invoker, psychicRitual.def.Named("RITUAL"));
			List<string> list = new List<string>();
			list.Add("ChronophagyInvokerRejuvenated".Translate(invoker, Mathf.FloorToInt(invokerAgeDelta)) + " " + "ChronophagyCurrentAge".Translate(invoker.ageTracker.AgeBiologicalYearsFloat));
			if (diedOfBrainDamage)
			{
				list.Add("PsychicRitualTargetBrainLiquified".Translate(target.Named("TARGET")));
			}
			else
			{
				list.Add("ChronophagyTargetAged".Translate(target, Mathf.FloorToInt(years)) + " " + "ChronophagyCurrentAge".Translate(target.ageTracker.AgeBiologicalYearsFloat));
			}
			taggedString += "\n\n" + list.ToLineList("  - ");
			if (!diedOfBrainDamage)
			{
				if (diedOfOldAge)
				{
					taggedString += "\n\n" + "ChronophagyTargetDiedOfOldAge".Translate(target);
				}
				else if (!gainedHediffs.NullOrEmpty<Hediff>())
				{
					taggedString += "\n\n" + "ChronophagyTargetOldAgeDiseases".Translate(target) + ":";
					taggedString += "\n\n" + (from h in gainedHediffs
					select h.LabelCap).ToLineList("  - ", false);
				}
			}
			if (!curedHediffs.NullOrEmpty<Hediff>())
			{
				taggedString += "\n\n" + "ChronophagyInvokerCuredDiseases".Translate(invoker);
				taggedString += "\n\n" + (from h in curedHediffs
				select h.LabelCap).ToLineList("  - ", false);
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), taggedString, LetterDefOf.NeutralEvent, new LookTargets(new TargetInfo[]
			{
				invoker,
				target
			}), null, null, null, null, 0, true);
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x0020823C File Offset: 0x0020643C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_Values.Look<FloatRange>(ref this.yearsTransferredFromQualityRange, "yearsTransferredFromQualityRange", default(FloatRange), false);
		}

		// Token: 0x0400494D RID: 18765
		public PsychicRitualRoleDef targetRole;

		// Token: 0x0400494E RID: 18766
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x0400494F RID: 18767
		public FloatRange yearsTransferredFromQualityRange;
	}
}
