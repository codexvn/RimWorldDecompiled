using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020022EB RID: 8939
	public class MedicalRecipesUtility
	{
		// Token: 0x0600C255 RID: 49749 RVA: 0x003859A0 File Offset: 0x00383BA0
		public static bool IsCleanAndDroppable(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !pawn.RaceProps.Animal && (!pawn.IsMutant || pawn.mutant.Def.partsCleanAndDroppable) && part.def.spawnThingOnRemoved != null && MedicalRecipesUtility.IsClean(pawn, part);
		}

		// Token: 0x0600C256 RID: 49750 RVA: 0x003859F8 File Offset: 0x00383BF8
		public static bool IsClean(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !(from x in pawn.health.hediffSet.hediffs
			where x.Part == part
			select x).Any<Hediff>();
		}

		// Token: 0x0600C257 RID: 49751 RVA: 0x00385A45 File Offset: 0x00383C45
		public static void RestorePartAndSpawnAllPreviousParts(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, pos, map);
			MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, pos, map);
			pawn.health.RestorePart(part, null, true);
		}

		// Token: 0x0600C258 RID: 49752 RVA: 0x00385A68 File Offset: 0x00383C68
		public static Thing SpawnNaturalPartIfClean(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
			{
				return GenSpawn.Spawn(part.def.spawnThingOnRemoved, pos, map, WipeMode.Vanish);
			}
			return null;
		}

		// Token: 0x0600C259 RID: 49753 RVA: 0x00385A88 File Offset: 0x00383C88
		public static void SpawnThingsFromHediffs(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			if (!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(part))
			{
				return;
			}
			foreach (Hediff hediff in from x in pawn.health.hediffSet.hediffs
			where x.Part == part
			select x)
			{
				if (hediff.def.spawnThingOnRemoved != null)
				{
					GenSpawn.Spawn(hediff.def.spawnThingOnRemoved, pos, map, WipeMode.Vanish);
				}
			}
			for (int i = 0; i < part.parts.Count; i++)
			{
				MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part.parts[i], pos, map);
			}
		}

		// Token: 0x0600C25A RID: 49754 RVA: 0x00385B70 File Offset: 0x00383D70
		public static IEnumerable<BodyPartRecord> GetFixedPartsToApplyOn(RecipeDef recipe, Pawn pawn, Func<BodyPartRecord, bool> validator = null)
		{
			int num;
			for (int i = 0; i < recipe.appliedOnFixedBodyParts.Count; i = num)
			{
				BodyPartDef part = recipe.appliedOnFixedBodyParts[i];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int j = 0; j < bpList.Count; j = num + 1)
				{
					BodyPartRecord bodyPartRecord = bpList[j];
					if (bodyPartRecord.def == part && (validator == null || validator(bodyPartRecord)))
					{
						yield return bodyPartRecord;
					}
					num = j;
				}
				part = null;
				bpList = null;
				num = i + 1;
			}
			for (int i = 0; i < recipe.appliedOnFixedBodyPartGroups.Count; i = num)
			{
				BodyPartGroupDef group = recipe.appliedOnFixedBodyPartGroups[i];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int j = 0; j < bpList.Count; j = num + 1)
				{
					BodyPartRecord bodyPartRecord2 = bpList[j];
					if (bodyPartRecord2.groups != null && bodyPartRecord2.groups.Contains(group) && (validator == null || validator(bodyPartRecord2)))
					{
						yield return bodyPartRecord2;
					}
					num = j;
				}
				group = null;
				bpList = null;
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x0600C25B RID: 49755 RVA: 0x00385B8E File Offset: 0x00383D8E
		public static IEnumerable<StatDrawEntry> GetMedicalStatsFromRecipeDefs(IEnumerable<RecipeDef> recipes)
		{
			bool multiple = recipes.Count<RecipeDef>() >= 2;
			foreach (RecipeDef def in recipes)
			{
				MedicalRecipesUtility.<>c__DisplayClass6_0 CS$<>8__locals1 = new MedicalRecipesUtility.<>c__DisplayClass6_0();
				string extraLabelPart = multiple ? (" (" + def.addsHediff.label + ")") : "";
				CS$<>8__locals1.diff = def.addsHediff;
				if (CS$<>8__locals1.diff != null)
				{
					if (!def.appliedOnFixedBodyParts.NullOrEmpty<BodyPartDef>())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_InstallSites".Translate(), (from x in def.appliedOnFixedBodyParts
						select x.label).ToCommaList(false, false).CapitalizeFirst(), "Stat_Thing_InstallSites_Desc".Translate(), 3990, null, null, false, false);
						if (CS$<>8__locals1.diff.addedPartProps != null && CS$<>8__locals1.diff.addedPartProps.solid)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_ReplacesParts".Translate(), (from x in MedicalRecipesUtility.<GetMedicalStatsFromRecipeDefs>g__PartAndAllPartsUnder|6_0(def.appliedOnFixedBodyParts)
							select x.def.label).Distinct<string>().ToCommaList(false, false).CapitalizeFirst(), "Stat_Thing_ReplacesParts_Desc".Translate(), 3980, null, null, false, false);
						}
					}
					else if (!def.appliedOnFixedBodyPartGroups.NullOrEmpty<BodyPartGroupDef>())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_InstallSites".Translate(), (from x in def.appliedOnFixedBodyPartGroups
						select x.label).ToCommaList(false, false).CapitalizeFirst(), "Stat_Thing_InstallSites_Desc".Translate(), 3990, null, null, false, false);
					}
					if (CS$<>8__locals1.diff.addedPartProps != null)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BodyPartEfficiency".Translate() + extraLabelPart, CS$<>8__locals1.diff.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute), "Stat_Thing_BodyPartEfficiency_Desc".Translate(), 4000, null, null, false, false);
					}
					foreach (StatDrawEntry statDrawEntry in CS$<>8__locals1.diff.SpecialDisplayStats(StatRequest.ForEmpty()))
					{
						statDrawEntry.category = StatCategoryDefOf.Implant;
						yield return statDrawEntry;
					}
					IEnumerator<StatDrawEntry> enumerator2 = null;
					HediffCompProperties_VerbGiver hediffCompProperties_VerbGiver = CS$<>8__locals1.diff.CompProps<HediffCompProperties_VerbGiver>();
					if (hediffCompProperties_VerbGiver != null)
					{
						if (!hediffCompProperties_VerbGiver.verbs.NullOrEmpty<VerbProperties>())
						{
							VerbProperties verb = hediffCompProperties_VerbGiver.verbs[0];
							if (!verb.IsMeleeAttack)
							{
								if (verb.defaultProjectile != null)
								{
									StringBuilder stringBuilder = new StringBuilder();
									stringBuilder.AppendLine("Stat_Thing_Damage_Desc".Translate());
									stringBuilder.AppendLine();
									int damageAmount = verb.defaultProjectile.projectile.GetDamageAmount(null, stringBuilder);
									yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Damage".Translate() + extraLabelPart, damageAmount.ToString(), stringBuilder.ToString(), 5500, null, null, false, false);
									if (verb.defaultProjectile.projectile.damageDef.armorCategory != null)
									{
										float armorPenetration = verb.defaultProjectile.projectile.GetArmorPenetration(null, null);
										yield return new StatDrawEntry(StatCategoryDefOf.Basics, "ArmorPenetration".Translate() + extraLabelPart, armorPenetration.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false, false);
									}
								}
							}
							else
							{
								int meleeDamageBaseAmount = verb.meleeDamageBaseAmount;
								if (verb.meleeDamageDef.armorCategory != null)
								{
									float num = verb.meleeArmorPenetrationBase;
									if (num < 0f)
									{
										num = (float)meleeDamageBaseAmount * 0.015f;
									}
									yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Melee, "ArmorPenetration".Translate() + extraLabelPart, num.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false, false);
								}
							}
							verb = null;
						}
						else if (!hediffCompProperties_VerbGiver.tools.NullOrEmpty<Tool>())
						{
							Tool tool = hediffCompProperties_VerbGiver.tools[0];
							if (ThingUtility.PrimaryMeleeWeaponDamageType(hediffCompProperties_VerbGiver.tools).armorCategory != null)
							{
								float num2 = tool.armorPenetration;
								if (num2 < 0f)
								{
									num2 = tool.power * 0.015f;
								}
								yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Melee, "ArmorPenetration".Translate() + extraLabelPart, num2.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false, false);
							}
						}
					}
					ThoughtDef thoughtDef = DefDatabase<ThoughtDef>.AllDefs.FirstOrDefault((ThoughtDef x) => x.hediff == CS$<>8__locals1.diff);
					if (thoughtDef != null && thoughtDef.stages != null && thoughtDef.stages.Any<ThoughtStage>())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MoodChange".Translate() + extraLabelPart, thoughtDef.stages.First<ThoughtStage>().baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), "Stat_Thing_MoodChange_Desc".Translate(), 3500, null, null, false, false);
					}
					CS$<>8__locals1 = null;
					extraLabelPart = null;
					def = null;
				}
			}
			IEnumerator<RecipeDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600C25D RID: 49757 RVA: 0x00385B9E File Offset: 0x00383D9E
		[CompilerGenerated]
		internal static IEnumerable<BodyPartRecord> <GetMedicalStatsFromRecipeDefs>g__PartAndAllPartsUnder|6_0(List<BodyPartDef> bpDefs)
		{
			foreach (BodyPartDef def in bpDefs)
			{
				BodyPartRecord bodyPartRecord = BodyDefOf.Human.GetPartsWithDef(def).FirstOrDefault<BodyPartRecord>();
				if (bodyPartRecord == null)
				{
					yield break;
				}
				foreach (BodyPartRecord bodyPartRecord2 in bodyPartRecord.GetPartAndAllChildParts())
				{
					yield return bodyPartRecord2;
				}
				IEnumerator<BodyPartRecord> enumerator2 = null;
			}
			List<BodyPartDef>.Enumerator enumerator = default(List<BodyPartDef>.Enumerator);
			yield break;
			yield break;
		}
	}
}
