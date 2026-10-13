using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200067C RID: 1660
	public static class HealthUtility
	{
		// Token: 0x06002989 RID: 10633 RVA: 0x000DB0B0 File Offset: 0x000D92B0
		public static string GetGeneralConditionLabel(Pawn pawn, bool shortVersion = false)
		{
			if (pawn.health.Dead)
			{
				return "Dead".Translate();
			}
			if (pawn.Deathresting)
			{
				return "Deathresting".Translate().CapitalizeFirst();
			}
			if (!pawn.health.capacities.CanBeAwake)
			{
				return "Unconscious".Translate();
			}
			if (pawn.health.InPainShock)
			{
				return (shortVersion && "PainShockShort".CanTranslate()) ? "PainShockShort".Translate() : "PainShock".Translate();
			}
			if (pawn.Downed && !LifeStageUtility.AlwaysDowned(pawn))
			{
				return "Incapacitated".Translate();
			}
			bool flag = false;
			for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = pawn.health.hediffSet.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					flag = true;
				}
			}
			if (flag)
			{
				return "Injured".Translate();
			}
			if (pawn.health.hediffSet.PainTotal > 0.3f)
			{
				return "InPain".Translate();
			}
			return "Healthy".Translate();
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000DB208 File Offset: 0x000D9408
		public static Pair<string, Color> GetPartConditionLabel(Pawn pawn, BodyPartRecord part)
		{
			float partHealth = pawn.health.hediffSet.GetPartHealth(part);
			float maxHealth = part.def.GetMaxHealth(pawn);
			float num = partHealth / maxHealth;
			Color second = Color.white;
			string first;
			if (partHealth <= 0f)
			{
				Hediff_MissingPart hediff_MissingPart = null;
				List<Hediff_MissingPart> missingPartsCommonAncestors = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
				{
					if (missingPartsCommonAncestors[i].Part == part)
					{
						hediff_MissingPart = missingPartsCommonAncestors[i];
						break;
					}
				}
				if (hediff_MissingPart == null)
				{
					bool fresh = false;
					if (hediff_MissingPart != null && hediff_MissingPart.IsFreshNonSolidExtremity)
					{
						fresh = true;
					}
					bool solid = part.def.IsSolid(part, pawn.health.hediffSet.hediffs);
					first = HealthUtility.GetGeneralDestroyedPartLabel(part, fresh, solid);
					second = Color.gray;
				}
				else
				{
					first = hediff_MissingPart.LabelCap;
					second = hediff_MissingPart.LabelColor;
				}
			}
			else if (num < 0.4f)
			{
				first = "SeriouslyImpaired".Translate();
				second = HealthUtility.RedColor;
			}
			else if (num < 0.7f)
			{
				first = "Impaired".Translate();
				second = HealthUtility.ImpairedColor;
			}
			else if (num < 0.999f)
			{
				first = "SlightlyImpaired".Translate();
				second = HealthUtility.SlightlyImpairedColor;
			}
			else
			{
				first = "GoodCondition".Translate();
				second = HealthUtility.GoodConditionColor;
			}
			return new Pair<string, Color>(first, second);
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x000DB370 File Offset: 0x000D9570
		public static string GetGeneralDestroyedPartLabel(BodyPartRecord part, bool fresh, bool solid)
		{
			if (part.parent == null)
			{
				return "SeriouslyImpaired".Translate();
			}
			if (part.depth != BodyPartDepth.Inside && !fresh)
			{
				return "MissingBodyPart".Translate();
			}
			if (solid)
			{
				return "ShatteredBodyPart".Translate();
			}
			return "DestroyedBodyPart".Translate();
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000DB3D4 File Offset: 0x000D95D4
		private static IEnumerable<BodyPartRecord> HittablePartsViolence(HediffSet bodyModel)
		{
			return from x in bodyModel.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside || (x.depth == BodyPartDepth.Inside && x.def.IsSolid(x, bodyModel.hediffs))
			select x;
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000DB410 File Offset: 0x000D9610
		public static void HealNonPermanentInjuriesAndRestoreLegs(Pawn p)
		{
			if (p.Dead)
			{
				return;
			}
			HealthUtility.tmpHediffs.Clear();
			HealthUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			for (int i = 0; i < HealthUtility.tmpHediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = HealthUtility.tmpHediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					p.health.RemoveHediff(hediff_Injury);
				}
				else
				{
					Hediff_MissingPart hediff_MissingPart = HealthUtility.tmpHediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) && (hediff_MissingPart.Part.parent == null || p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff_MissingPart.Part.parent)))
					{
						p.health.RestorePart(hediff_MissingPart.Part, null, true);
					}
				}
			}
			HealthUtility.tmpHediffs.Clear();
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000DB510 File Offset: 0x000D9710
		public static void GiveRandomSurgeryInjuries(Pawn p, int totalDamage, BodyPartRecord operatedPart)
		{
			IEnumerable<BodyPartRecord> source;
			if (operatedPart == null)
			{
				source = from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where !x.def.conceptual
				select x;
			}
			else
			{
				source = from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where !x.def.conceptual
				select x into pa
				where pa == operatedPart || pa.parent == operatedPart || (operatedPart != null && operatedPart.parent == pa)
				select pa;
			}
			source = from x in source
			where HealthUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(x, p) >= 2f
			select x;
			BodyPartRecord brain = p.health.hediffSet.GetBrain();
			if (brain != null)
			{
				float maxBrainHealth = brain.def.GetMaxHealth(p);
				source = from x in source
				where x != brain || p.health.hediffSet.GetPartHealth(x) >= maxBrainHealth * 0.5f + 1f
				select x;
			}
			while (totalDamage > 0 && source.Any<BodyPartRecord>())
			{
				BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int num = Mathf.Max(3, GenMath.RoundRandom(partHealth * Rand.Range(0.5f, 1f)));
				float minHealthOfPartsWeWantToAvoidDestroying = HealthUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(bodyPartRecord, p);
				if (minHealthOfPartsWeWantToAvoidDestroying - (float)num < 1f)
				{
					num = Mathf.RoundToInt(minHealthOfPartsWeWantToAvoidDestroying - 1f);
				}
				if (bodyPartRecord == brain && partHealth - (float)num < brain.def.GetMaxHealth(p) * 0.5f)
				{
					num = Mathf.Max(Mathf.RoundToInt(partHealth - brain.def.GetMaxHealth(p) * 0.5f), 1);
				}
				if (num <= 0)
				{
					break;
				}
				DamageDef def = Rand.Element<DamageDef>(DamageDefOf.Cut, DamageDefOf.Scratch, DamageDefOf.Stab, DamageDefOf.Crush);
				DamageInfo dinfo = new DamageInfo(def, (float)num, 0f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetIgnoreArmor(true);
				dinfo.SetIgnoreInstantKillProtection(true);
				p.TakeDamage(dinfo);
				totalDamage -= num;
			}
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000DB7A0 File Offset: 0x000D99A0
		private static float GetMinHealthOfPartsWeWantToAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			float num = 999999f;
			while (part != null)
			{
				if (HealthUtility.ShouldRandomSurgeryInjuriesAvoidDestroying(part, pawn))
				{
					num = Mathf.Min(num, pawn.health.hediffSet.GetPartHealth(part));
				}
				part = part.parent;
			}
			return num;
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000DB7E4 File Offset: 0x000D99E4
		private static bool ShouldRandomSurgeryInjuriesAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			if (part == pawn.RaceProps.body.corePart)
			{
				return true;
			}
			if (part.def.tags.Any((BodyPartTagDef x) => x.vital))
			{
				return true;
			}
			for (int i = 0; i < part.parts.Count; i++)
			{
				if (HealthUtility.ShouldRandomSurgeryInjuriesAvoidDestroying(part.parts[i], pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000DB868 File Offset: 0x000D9A68
		public static void DamageUntilDowned(Pawn p, bool allowBleedingWounds = true, DamageDef damage = null, ThingDef sourceDef = null, BodyPartGroupDef bodyGroupDef = null)
		{
			if (p.Downed)
			{
				return;
			}
			HediffSet hediffSet = p.health.hediffSet;
			p.health.forceDowned = true;
			IEnumerable<BodyPartRecord> source = from x in HealthUtility.HittablePartsViolence(hediffSet)
			where !p.health.hediffSet.hediffs.Any((Hediff y) => y.Part == x && y.CurStage != null && y.CurStage.partEfficiencyOffset < 0f)
			select x;
			int num = 0;
			while (num < 300 && !p.Downed && source.Any<BodyPartRecord>())
			{
				num++;
				BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				int num2 = Mathf.RoundToInt(hediffSet.GetPartHealth(bodyPartRecord));
				float statValue = p.GetStatValue(StatDefOf.IncomingDamageFactor, true, -1);
				if (statValue > 0f)
				{
					num2 = (int)((float)num2 / statValue);
				}
				num2 -= 3;
				if (num2 > 0 && (num2 >= 8 || num >= 250))
				{
					if (num > 275)
					{
						num2 = Rand.Range(1, 8);
					}
					DamageDef damageDef;
					if (damage != null)
					{
						damageDef = damage;
					}
					else if (bodyPartRecord.depth == BodyPartDepth.Outside)
					{
						if (!allowBleedingWounds && bodyPartRecord.def.bleedRate > 0f)
						{
							damageDef = DamageDefOf.Blunt;
						}
						else
						{
							damageDef = HealthUtility.RandomViolenceDamageType();
						}
					}
					else
					{
						damageDef = DamageDefOf.Blunt;
					}
					int num3 = Rand.RangeInclusive(Mathf.RoundToInt((float)num2 * 0.65f), num2);
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
					if (!p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num3 * p.GetStatValue(StatDefOf.IncomingDamageFactor, true, -1)))
					{
						DamageInfo dinfo = new DamageInfo(damageDef, (float)num3, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, false, false);
						dinfo.SetAllowDamagePropagation(false);
						DamageWorker.DamageResult damageResult = p.TakeDamage(dinfo);
						if (damageResult.hediffs != null)
						{
							foreach (Hediff hediff in damageResult.hediffs)
							{
								if (sourceDef != null)
								{
									hediff.sourceDef = sourceDef;
								}
								if (bodyGroupDef != null)
								{
									hediff.sourceBodyPartGroup = bodyGroupDef;
								}
							}
						}
					}
				}
			}
			if (p.Dead && !p.kindDef.forceDeathOnDowned)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = stringBuilder;
				Pawn p2 = p;
				stringBuilder2.AppendLine(((p2 != null) ? p2.ToString() : null) + " died during GiveInjuriesToForceDowned");
				for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					string str = "   -";
					Hediff hediff2 = p.health.hediffSet.hediffs[i];
					stringBuilder3.AppendLine(str + ((hediff2 != null) ? hediff2.ToString() : null));
				}
				Log.Error(stringBuilder.ToString());
			}
			p.health.forceDowned = false;
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000DBB84 File Offset: 0x000D9D84
		public static void DamageUntilDead(Pawn p, DamageDef damage = null, ThingDef sourceDef = null, BodyPartGroupDef bodyGroupDef = null)
		{
			HediffSet hediffSet = p.health.hediffSet;
			int num = 0;
			while (!p.Dead && num < 200 && HealthUtility.HittablePartsViolence(hediffSet).Any<BodyPartRecord>())
			{
				num++;
				BodyPartRecord bodyPartRecord = HealthUtility.HittablePartsViolence(hediffSet).RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				int num2 = Rand.RangeInclusive(8, 25);
				DamageDef def;
				if (damage != null)
				{
					def = damage;
				}
				else if (bodyPartRecord.depth == BodyPartDepth.Outside)
				{
					def = HealthUtility.RandomViolenceDamageType();
				}
				else
				{
					def = DamageDefOf.Blunt;
				}
				DamageInfo dinfo = new DamageInfo(def, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetIgnoreInstantKillProtection(true);
				foreach (Hediff hediff in p.TakeDamage(dinfo).hediffs)
				{
					if (sourceDef != null)
					{
						hediff.sourceDef = sourceDef;
					}
					if (bodyGroupDef != null)
					{
						hediff.sourceBodyPartGroup = bodyGroupDef;
					}
				}
			}
			if (!p.Dead)
			{
				Log.Error(((p != null) ? p.ToString() : null) + " not killed during GiveInjuriesToKill");
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000DBCC8 File Offset: 0x000D9EC8
		public static void SimulateKilled(Pawn p, DamageDef damage, ThingDef sourceDef = null, Tool sourceTool = null, BodyPartDef idealPart = null)
		{
			HediffSet hediffSet = p.health.hediffSet;
			int num = 0;
			while (!p.Dead && num < 200 && HealthUtility.HittablePartsViolence(hediffSet).Any<BodyPartRecord>())
			{
				num++;
				BodyPartRecord hitPart;
				if (idealPart == null || !hediffSet.TryGetBodyPartRecord(idealPart, out hitPart))
				{
					hitPart = HealthUtility.HittablePartsViolence(hediffSet).RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				}
				int num2 = Rand.RangeInclusive(8, 25);
				DamageInfo dinfo = new DamageInfo(damage, (float)num2, 999f, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetIgnoreInstantKillProtection(true);
				DamageWorker.DamageResult damageResult = p.TakeDamage(dinfo);
				if (!damageResult.hediffs.NullOrEmpty<Hediff>())
				{
					foreach (Hediff hediff in damageResult.hediffs)
					{
						if (sourceDef != null)
						{
							hediff.sourceDef = sourceDef;
							hediff.sourceLabel = sourceDef.label;
						}
						if (sourceTool != null)
						{
							hediff.sourceToolLabel = (sourceTool.labelNoLocation ?? sourceTool.label);
						}
					}
				}
			}
			if (!p.Dead)
			{
				p.Kill(null, null);
			}
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000DBE1C File Offset: 0x000DA01C
		public static void SimulateKilledByPawn(Pawn p, Pawn killer)
		{
			HediffSet hediffSet = p.health.hediffSet;
			int num = 0;
			while (!p.Dead && num < 200 && HealthUtility.HittablePartsViolence(hediffSet).Any<BodyPartRecord>())
			{
				num++;
				BodyPartRecord bodyPartRecord = HealthUtility.HittablePartsViolence(hediffSet).RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				int num2 = Rand.RangeInclusive(8, 25);
				Pawn_EquipmentTracker equipment = killer.equipment;
				bool? flag;
				if (equipment == null)
				{
					flag = null;
				}
				else
				{
					ThingWithComps primary = equipment.Primary;
					if (primary == null)
					{
						flag = null;
					}
					else
					{
						ThingDef def = primary.def;
						flag = ((def != null) ? new bool?(def.IsRangedWeapon) : null);
					}
				}
				bool? flag2 = flag;
				Verb verb;
				if (flag2.GetValueOrDefault() && Rand.Chance(0.75f))
				{
					verb = killer.equipment.PrimaryEq.PrimaryVerb;
				}
				else
				{
					Pawn_EquipmentTracker equipment2 = killer.equipment;
					Verb verb2;
					if (equipment2 == null)
					{
						verb2 = null;
					}
					else
					{
						CompEquippable primaryEq = equipment2.PrimaryEq;
						if (primaryEq == null)
						{
							verb2 = null;
						}
						else
						{
							List<Verb> allVerbs = primaryEq.AllVerbs;
							verb2 = ((allVerbs != null) ? allVerbs.RandomElement<Verb>() : null);
						}
					}
					verb = (verb2 ?? killer.meleeVerbs.TryGetMeleeVerb(p));
				}
				ThingWithComps equipmentSource = verb.EquipmentSource;
				ThingDef weapon = ((equipmentSource != null) ? equipmentSource.def : null) ?? killer.def;
				DamageDef def2 = verb.GetDamageDef() ?? DamageDefOf.Blunt;
				float amount = (float)num2;
				float armorPenetration = 999f;
				float angle = -1f;
				BodyPartRecord hitPart = bodyPartRecord;
				DamageInfo dinfo = new DamageInfo(def2, amount, armorPenetration, angle, killer, hitPart, weapon, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetIgnoreInstantKillProtection(true);
				p.TakeDamage(dinfo);
			}
			if (!p.Dead)
			{
				p.Kill(null, null);
			}
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000DBFC0 File Offset: 0x000DA1C0
		public static void DamageLegsUntilIncapableOfMoving(Pawn p, bool allowBleedingWounds = true)
		{
			int num = 0;
			p.health.forceDowned = true;
			Func<BodyPartRecord, bool> <>9__0;
			while (p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && num < 300)
			{
				num++;
				IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) && p.health.hediffSet.GetPartHealth(x) >= 2f));
				}
				IEnumerable<BodyPartRecord> source = notMissingParts.Where(predicate);
				if (!source.Any<BodyPartRecord>())
				{
					break;
				}
				BodyPartRecord bodyPartRecord = source.RandomElement<BodyPartRecord>();
				float maxHealth = bodyPartRecord.def.GetMaxHealth(p);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int minInclusive = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.12f), 1, (int)partHealth - 1);
				int maxInclusive = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.27f), 1, (int)partHealth - 1);
				int num2 = Rand.RangeInclusive(minInclusive, maxInclusive);
				DamageDef damageDef;
				if (!allowBleedingWounds && bodyPartRecord.def.bleedRate > 0f)
				{
					damageDef = DamageDefOf.Blunt;
				}
				else
				{
					damageDef = HealthUtility.RandomViolenceDamageType();
				}
				HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
				if (p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num2))
				{
					break;
				}
				DamageInfo dinfo = new DamageInfo(damageDef, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetAllowDamagePropagation(false);
				p.TakeDamage(dinfo);
			}
			p.health.forceDowned = false;
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000DC168 File Offset: 0x000DA368
		public static void DamageLimbsUntilIncapableOfManipulation(Pawn p, bool allowBleedingWounds = true)
		{
			int num = 0;
			p.health.forceDowned = true;
			Func<BodyPartRecord, bool> <>9__0;
			while (p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && num < 300)
			{
				num++;
				IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.ManipulationLimbCore) && p.health.hediffSet.GetPartHealth(x) >= 2f));
				}
				IEnumerable<BodyPartRecord> source = notMissingParts.Where(predicate);
				if (!source.Any<BodyPartRecord>())
				{
					break;
				}
				BodyPartRecord bodyPartRecord = source.RandomElement<BodyPartRecord>();
				float maxHealth = bodyPartRecord.def.GetMaxHealth(p);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int minInclusive = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.12f), 1, (int)partHealth - 1);
				int maxInclusive = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.27f), 1, (int)partHealth - 1);
				int num2 = Rand.RangeInclusive(minInclusive, maxInclusive);
				DamageDef damageDef = HealthUtility.RandomViolenceDamageType();
				HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
				if (p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num2))
				{
					break;
				}
				DamageInfo dinfo = new DamageInfo(damageDef, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetAllowDamagePropagation(false);
				p.TakeDamage(dinfo);
			}
			p.health.forceDowned = false;
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000DC2F4 File Offset: 0x000DA4F4
		public static DamageDef RandomViolenceDamageType()
		{
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				return DamageDefOf.Bullet;
			case 1:
				return DamageDefOf.Blunt;
			case 2:
				return DamageDefOf.Stab;
			case 3:
				return DamageDefOf.Scratch;
			case 4:
				return DamageDefOf.Cut;
			default:
				return null;
			}
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000DC344 File Offset: 0x000DA544
		public static DamageDef RandomPermanentInjuryDamageType(bool allowFrostbite)
		{
			switch (Rand.RangeInclusive(0, 3 + (allowFrostbite ? 1 : 0)))
			{
			case 0:
				return DamageDefOf.Bullet;
			case 1:
				return DamageDefOf.Scratch;
			case 2:
				return DamageDefOf.Bite;
			case 3:
				return DamageDefOf.Stab;
			case 4:
				return DamageDefOf.Frostbite;
			default:
				throw new Exception();
			}
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000DC3A0 File Offset: 0x000DA5A0
		public static HediffDef GetHediffDefFromDamage(DamageDef dam, Pawn pawn, BodyPartRecord part)
		{
			HediffDef result = dam.hediff;
			if (part.def.IsSkinCovered(part, pawn.health.hediffSet) && dam.hediffSkin != null)
			{
				result = dam.hediffSkin;
			}
			if (part.def.IsSolid(part, pawn.health.hediffSet.hediffs) && dam.hediffSolid != null)
			{
				result = dam.hediffSolid;
			}
			return result;
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000DC40C File Offset: 0x000DA60C
		public static bool TryAnesthetize(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return false;
			}
			pawn.health.forceDowned = true;
			pawn.health.AddHediff(HediffDefOf.Anesthetic, null, null, null);
			pawn.health.forceDowned = false;
			return true;
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000DC460 File Offset: 0x000DA660
		public static void AdjustSeverity(Pawn pawn, HediffDef hdDef, float sevOffset)
		{
			if (sevOffset == 0f)
			{
				return;
			}
			Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hdDef, false);
			if (hediff != null)
			{
				hediff.Severity += sevOffset;
				return;
			}
			if (sevOffset > 0f)
			{
				hediff = HediffMaker.MakeHediff(hdDef, pawn, null);
				hediff.Severity = sevOffset;
				pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x000DC4CC File Offset: 0x000DA6CC
		public static BodyPartRemovalIntent PartRemovalIntent(Pawn pawn, BodyPartRecord part)
		{
			if (pawn.health.hediffSet.hediffs.Any((Hediff d) => d.Visible && d.Part == part && d.def.isBad))
			{
				return BodyPartRemovalIntent.Amputate;
			}
			return BodyPartRemovalIntent.Harvest;
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000DC50C File Offset: 0x000DA70C
		public static int TicksUntilDeathDueToBloodLoss(Pawn pawn)
		{
			float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
			if (bleedRateTotal < 0.0001f)
			{
				return int.MaxValue;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			float num = (firstHediffOfDef != null) ? firstHediffOfDef.Severity : 0f;
			return (int)((1f - num) / bleedRateTotal * 60000f);
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000DC570 File Offset: 0x000DA770
		public static TaggedString FixWorstHealthCondition(Pawn pawn, params HediffDef[] exclude)
		{
			Hediff hediff;
			BodyPartRecord part;
			if (!HealthUtility.TryGetWorstHealthCondition(pawn, out hediff, out part, exclude))
			{
				return null;
			}
			if (hediff != null)
			{
				return HealthUtility.Cure(hediff);
			}
			return HealthUtility.Cure(part, pawn);
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000DC5A4 File Offset: 0x000DA7A4
		public static bool TryGetWorstHealthCondition(Pawn pawn, out Hediff hediff, out BodyPartRecord part, params HediffDef[] exclude)
		{
			part = null;
			hediff = HealthUtility.FindLifeThreateningHediff(pawn, exclude);
			if (hediff != null)
			{
				return true;
			}
			if (HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 2500)
			{
				hediff = HealthUtility.FindMostBleedingHediff(pawn, exclude);
				if (hediff != null)
				{
					return true;
				}
			}
			if (pawn.health.hediffSet.GetBrain() != null)
			{
				hediff = HealthUtility.FindPermanentInjury(pawn, Gen.YieldSingle<BodyPartRecord>(pawn.health.hediffSet.GetBrain()), exclude);
				if (hediff != null)
				{
					return true;
				}
			}
			float coverageAbsWithChildren = ThingDefOf.Human.race.body.GetPartsWithDef(BodyPartDefOf.Hand).First<BodyPartRecord>().coverageAbsWithChildren;
			part = HealthUtility.FindBiggestMissingBodyPart(pawn, coverageAbsWithChildren);
			if (part != null)
			{
				return true;
			}
			hediff = HealthUtility.FindPermanentInjury(pawn, from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.def == BodyPartDefOf.Eye
			select x, exclude);
			if (hediff != null)
			{
				return true;
			}
			hediff = HealthUtility.FindImmunizableHediffWhichCanKill(pawn, exclude);
			if (hediff != null)
			{
				return true;
			}
			hediff = HealthUtility.FindNonInjuryMiscBadHediff(pawn, true, false, exclude);
			if (hediff != null)
			{
				return true;
			}
			hediff = HealthUtility.FindNonInjuryMiscBadHediff(pawn, false, false, exclude);
			if (hediff != null)
			{
				return true;
			}
			if (pawn.health.hediffSet.GetBrain() != null)
			{
				hediff = HealthUtility.FindInjury(pawn, Gen.YieldSingle<BodyPartRecord>(pawn.health.hediffSet.GetBrain()), exclude);
				if (hediff != null)
				{
					return true;
				}
			}
			part = HealthUtility.FindBiggestMissingBodyPart(pawn, 0f);
			if (part != null)
			{
				return true;
			}
			hediff = HealthUtility.FindAddiction(pawn, exclude);
			if (hediff != null)
			{
				return true;
			}
			hediff = HealthUtility.FindPermanentInjury(pawn, null, exclude);
			if (hediff != null)
			{
				return true;
			}
			hediff = HealthUtility.FindInjury(pawn, null, exclude);
			if (hediff != null)
			{
				return true;
			}
			hediff = HealthUtility.FindNonInjuryMiscBadHediff(pawn, false, true, exclude);
			return hediff != null;
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000DC74C File Offset: 0x000DA94C
		private static Hediff FindLifeThreateningHediff(Pawn pawn, params HediffDef[] exclude)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem && !hediffs[i].FullyImmune() && (exclude == null || !exclude.Contains(hediffs[i].def)))
				{
					bool flag = hediffs[i].IsLethal && hediffs[i].Severity / hediffs[i].def.lethalSeverity >= 0.8f;
					if (hediffs[i].IsCurrentlyLifeThreatening || flag)
					{
						float num2 = (hediffs[i].Part != null) ? hediffs[i].Part.coverageAbsWithChildren : 999f;
						if (hediff == null || num2 > num)
						{
							hediff = hediffs[i];
							num = num2;
						}
					}
				}
			}
			return hediff;
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000DC864 File Offset: 0x000DAA64
		private static Hediff FindMostBleedingHediff(Pawn pawn, params HediffDef[] exclude)
		{
			float num = 0f;
			Hediff hediff = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem && (exclude == null || !exclude.Contains(hediffs[i].def)))
				{
					float bleedRate = hediffs[i].BleedRate;
					if (bleedRate > 0f && (bleedRate > num || hediff == null))
					{
						num = bleedRate;
						hediff = hediffs[i];
					}
				}
			}
			return hediff;
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x000DC900 File Offset: 0x000DAB00
		private static Hediff FindImmunizableHediffWhichCanKill(Pawn pawn, params HediffDef[] exclude)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem && hediffs[i].TryGetComp<HediffComp_Immunizable>() != null && !hediffs[i].FullyImmune() && (exclude == null || !exclude.Contains(hediffs[i].def)) && hediffs[i].CanEverKill())
				{
					float severity = hediffs[i].Severity;
					if (hediff == null || severity > num)
					{
						hediff = hediffs[i];
						num = severity;
					}
				}
			}
			return hediff;
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000DC9C4 File Offset: 0x000DABC4
		private static Hediff FindNonInjuryMiscBadHediff(Pawn pawn, bool onlyIfCanKill, bool checkDeprioritized, params HediffDef[] exclude)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if ((checkDeprioritized || !hediffs[i].def.deprioritizeHealing) && hediffs[i].Visible && hediffs[i].def.isBad && hediffs[i].def.everCurableByItem && !(hediffs[i] is Hediff_Injury) && !(hediffs[i] is Hediff_MissingPart) && !(hediffs[i] is Hediff_Addiction) && !(hediffs[i] is Hediff_AddedPart) && (!onlyIfCanKill || hediffs[i].CanEverKill()) && (exclude == null || !exclude.Contains(hediffs[i].def)))
				{
					float num2 = (hediffs[i].Part != null) ? hediffs[i].Part.coverageAbsWithChildren : 999f;
					if (hediff == null || num2 > num)
					{
						hediff = hediffs[i];
						num = num2;
					}
				}
			}
			return hediff;
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x000DCAF8 File Offset: 0x000DACF8
		private static BodyPartRecord FindBiggestMissingBodyPart(Pawn pawn, float minCoverage = 0f)
		{
			BodyPartRecord bodyPartRecord = null;
			foreach (Hediff_MissingPart hediff_MissingPart in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
			{
				if (hediff_MissingPart.Part.coverageAbsWithChildren >= minCoverage && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff_MissingPart.Part) && (bodyPartRecord == null || hediff_MissingPart.Part.coverageAbsWithChildren > bodyPartRecord.coverageAbsWithChildren))
				{
					bodyPartRecord = hediff_MissingPart.Part;
				}
			}
			return bodyPartRecord;
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x000DCB94 File Offset: 0x000DAD94
		private static Hediff_Addiction FindAddiction(Pawn pawn, params HediffDef[] exclude)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && hediff_Addiction.Visible && hediff_Addiction.def.everCurableByItem && (exclude == null || !exclude.Contains(hediffs[i].def)))
				{
					return hediff_Addiction;
				}
			}
			return null;
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x000DCC04 File Offset: 0x000DAE04
		private static Hediff_Injury FindPermanentInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null, params HediffDef[] exclude)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.IsPermanent() && hediff_Injury2.def.everCurableByItem && (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) && (exclude == null || !exclude.Contains(hediffs[i].def)) && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x000DCCA0 File Offset: 0x000DAEA0
		private static Hediff_Injury FindInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null, params HediffDef[] exclude)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.def.everCurableByItem && (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) && (exclude == null || !exclude.Contains(hediffs[i].def)) && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x000DCD34 File Offset: 0x000DAF34
		public static TaggedString Cure(Hediff hediff)
		{
			Pawn pawn = hediff.pawn;
			pawn.health.RemoveHediff(hediff);
			if (hediff.def.cureAllAtOnceIfCuredByItem)
			{
				int num = 0;
				for (;;)
				{
					num++;
					if (num > 10000)
					{
						break;
					}
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediff.def, false);
					if (firstHediffOfDef == null)
					{
						goto IL_63;
					}
					pawn.health.RemoveHediff(firstHediffOfDef);
				}
				Log.Error("Too many iterations.");
			}
			IL_63:
			return "HealingHealHediff".Translate(pawn, hediff.def.label);
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x000DCDC4 File Offset: 0x000DAFC4
		private static TaggedString Cure(BodyPartRecord part, Pawn pawn)
		{
			pawn.health.RestorePart(part, null, true);
			return "HealingRestoreBodyPart".Translate(pawn, part.Label);
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x000DCDF0 File Offset: 0x000DAFF0
		public static bool IsMissingSightBodyPart(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = hediffs[i] as Hediff_MissingPart;
				if (hediff_MissingPart != null && hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.SightSource))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x000DCE50 File Offset: 0x000DB050
		public static void AddStartingHediffs(Pawn pawn, List<StartingHediff> startingHediffs)
		{
			foreach (StartingHediff startingHediff in startingHediffs)
			{
				if (!startingHediff.HasHediff(pawn) && Rand.Chance(startingHediff.chance ?? 1f))
				{
					Hediff hediff = pawn.health.AddHediff(startingHediff.def, null, null, null);
					if (startingHediff.severity != null)
					{
						hediff.Severity = startingHediff.severity.Value;
					}
					if (startingHediff.durationTicksRange != null)
					{
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = startingHediff.durationTicksRange.Value.RandomInRange;
					}
				}
			}
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x000DCF38 File Offset: 0x000DB138
		public static TaggedString GetDiedLetterText(Pawn pawn, DamageInfo? dinfo, Hediff hediff)
		{
			if (dinfo != null)
			{
				return dinfo.Value.Def.deathMessage.Formatted(pawn.LabelShortCap, pawn.Named("PAWN"));
			}
			if (hediff != null)
			{
				return "PawnDiedBecauseOf".Translate(pawn.LabelShortCap, hediff.def.LabelCap, pawn.Named("PAWN"));
			}
			return "PawnDied".Translate(pawn.LabelShortCap, pawn.Named("PAWN"));
		}

		// Token: 0x0400220A RID: 8714
		public static readonly Color GoodConditionColor = new Color(0.6f, 0.8f, 0.65f);

		// Token: 0x0400220B RID: 8715
		public static readonly Color RedColor = ColorLibrary.RedReadable;

		// Token: 0x0400220C RID: 8716
		public static readonly Color ImpairedColor = new Color(0.9f, 0.35f, 0f);

		// Token: 0x0400220D RID: 8717
		public static readonly Color SlightlyImpairedColor = new Color(0.9f, 0.7f, 0f);

		// Token: 0x0400220E RID: 8718
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
