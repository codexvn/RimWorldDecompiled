using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200233B RID: 9019
	public static class AgeInjuryUtility
	{
		// Token: 0x0600C3B4 RID: 50100 RVA: 0x0038D0EB File Offset: 0x0038B2EB
		public static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(Pawn pawn, float age)
		{
			return AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn.def, ModsConfig.BiotechActive ? pawn.GetStatValue(StatDefOf.CancerRate, true, -1) : 1f, age);
		}

		// Token: 0x0600C3B5 RID: 50101 RVA: 0x0038D114 File Offset: 0x0038B314
		private static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(ThingDef raceDef, float cancerFactor, float age)
		{
			List<HediffGiverSetDef> sets = raceDef.race.hediffGiverSets;
			if (sets == null)
			{
				yield break;
			}
			int num2;
			for (int i = 0; i < sets.Count; i = num2 + 1)
			{
				List<HediffGiver> givers = sets[i].hediffGivers;
				if (givers != null)
				{
					for (int j = 0; j < givers.Count; j = num2 + 1)
					{
						HediffGiver_Birthday hediffGiver_Birthday = givers[j] as HediffGiver_Birthday;
						if (hediffGiver_Birthday != null)
						{
							float num = age / raceDef.race.lifeExpectancy;
							if (hediffGiver_Birthday.hediff == HediffDefOf.Carcinoma)
							{
								num *= cancerFactor;
							}
							if (Rand.Value < hediffGiver_Birthday.ageFractionChanceCurve.Evaluate(num))
							{
								yield return hediffGiver_Birthday;
							}
						}
						num2 = j;
					}
					givers = null;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600C3B6 RID: 50102 RVA: 0x0038D134 File Offset: 0x0038B334
		public static void GenerateRandomOldAgeInjuries(Pawn pawn, bool tryNotToKillPawn)
		{
			if (!pawn.kindDef.allowOldAgeInjuries)
			{
				return;
			}
			float num = pawn.RaceProps.IsMechanoid ? 2500f : pawn.RaceProps.lifeExpectancy;
			float num2 = num / 8f;
			float b = num * 1.5f;
			float chance = pawn.RaceProps.Humanlike ? 0.15f : 0.03f;
			int num3 = 0;
			for (float num4 = num2; num4 < Mathf.Min((float)pawn.ageTracker.AgeBiologicalYears, b); num4 += num2)
			{
				if (Rand.Chance(chance))
				{
					num3++;
				}
			}
			bool flag = !pawn.RaceProps.Humanlike || Rand.Chance(AgeInjuryUtility.AmputationChanceFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat));
			Func<BodyPartRecord, bool> <>9__0;
			for (int i = 0; i < num3; i++)
			{
				IEnumerable<BodyPartRecord> notMissingParts = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord x) => x.depth == BodyPartDepth.Outside && (x.def.permanentInjuryChanceFactor != 0f || x.def.pawnGeneratorCanAmputate) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x)));
				}
				IEnumerable<BodyPartRecord> source = notMissingParts.Where(predicate);
				if (source.Any<BodyPartRecord>())
				{
					BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(HealthUtility.RandomPermanentInjuryDamageType(bodyPartRecord.def.frostbiteVulnerability > 0f && pawn.RaceProps.ToolUser), pawn, bodyPartRecord);
					if (bodyPartRecord.def.pawnGeneratorCanAmputate && flag && Rand.Chance(0.3f))
					{
						Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
						hediff_MissingPart.lastInjury = hediffDefFromDamage;
						hediff_MissingPart.Part = bodyPartRecord;
						hediff_MissingPart.IsFresh = false;
						if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_MissingPart))
						{
							pawn.health.AddHediff(hediff_MissingPart, bodyPartRecord, null, null);
							if (pawn.RaceProps.Humanlike && bodyPartRecord.def == BodyPartDefOf.Leg && Rand.Chance(0.5f))
							{
								RecipeDefOf.InstallPegLeg.Worker.ApplyOnPawn(pawn, bodyPartRecord, null, AgeInjuryUtility.emptyIngredientsList, null);
							}
						}
					}
					else if (bodyPartRecord.def.permanentInjuryChanceFactor > 0f && hediffDefFromDamage.HasComp(typeof(HediffComp_GetsPermanent)))
					{
						Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
						hediff_Injury.Severity = (float)Rand.RangeInclusive(2, 6);
						hediff_Injury.TryGetComp<HediffComp_GetsPermanent>().IsPermanent = true;
						hediff_Injury.Part = bodyPartRecord;
						if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_Injury))
						{
							pawn.health.AddHediff(hediff_Injury, bodyPartRecord, null, null);
						}
					}
				}
			}
			for (int j = 1; j < pawn.ageTracker.AgeBiologicalYears; j++)
			{
				foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn, (float)j))
				{
					hediffGiver_Birthday.TryApplyAndSimulateSeverityChange(pawn, (float)j, tryNotToKillPawn);
					if (pawn.Dead)
					{
						break;
					}
				}
				if (pawn.Dead)
				{
					break;
				}
			}
		}

		// Token: 0x0600C3B7 RID: 50103 RVA: 0x0038D4FC File Offset: 0x0038B6FC
		[DebugOutput]
		public static void PermanentInjuryCalculations()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Theoretical injuries=========");
			for (int i = 0; i < 10; i++)
			{
				stringBuilder.AppendLine("#" + i.ToString() + ":");
				List<HediffDef> list = new List<HediffDef>();
				for (int j = 0; j < 100; j++)
				{
					foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(ThingDefOf.Human, 1f, (float)j))
					{
						if (!list.Contains(hediffGiver_Birthday.hediff))
						{
							list.Add(hediffGiver_Birthday.hediff);
							StringBuilder stringBuilder2 = stringBuilder;
							string str = "  age ";
							string str2 = j.ToString();
							string str3 = " - ";
							HediffDef hediff = hediffGiver_Birthday.hediff;
							stringBuilder2.AppendLine(str + str2 + str3 + ((hediff != null) ? hediff.ToString() : null));
						}
					}
				}
			}
			Log.Message(stringBuilder.ToString());
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Actual injuries=========");
			for (int k = 0; k < 200; k++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, null);
				if (pawn.ageTracker.AgeBiologicalYears >= 40)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					Name name = pawn.Name;
					stringBuilder3.AppendLine(((name != null) ? name.ToString() : null) + " age " + pawn.ageTracker.AgeBiologicalYears.ToString());
					foreach (Hediff hediff2 in pawn.health.hediffSet.hediffs)
					{
						StringBuilder stringBuilder4 = stringBuilder;
						string str4 = " - ";
						Hediff hediff3 = hediff2;
						stringBuilder4.AppendLine(str4 + ((hediff3 != null) ? hediff3.ToString() : null));
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04008554 RID: 34132
		private const int MaxPermanentInjuryAge = 100;

		// Token: 0x04008555 RID: 34133
		private static List<Thing> emptyIngredientsList = new List<Thing>();

		// Token: 0x04008556 RID: 34134
		private static readonly SimpleCurve AmputationChanceFromAgeCurve = new SimpleCurve
		{
			{
				new CurvePoint(13f, 0f),
				true
			},
			{
				new CurvePoint(25f, 1f),
				true
			}
		};
	}
}
