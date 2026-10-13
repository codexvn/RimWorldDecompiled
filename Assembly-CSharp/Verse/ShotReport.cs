using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D5F RID: 3423
	public struct ShotReport
	{
		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06005712 RID: 22290 RVA: 0x001C0300 File Offset: 0x001BE500
		private float FactorFromPosture
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance >= 4.5f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 0.5f;
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06005713 RID: 22291 RVA: 0x001C0350 File Offset: 0x001BE550
		private float FactorFromExecution
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance <= 3.9f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 7.5f;
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06005714 RID: 22292 RVA: 0x001C03A0 File Offset: 0x001BE5A0
		public float AimOnTargetChance_StandardTarget
		{
			get
			{
				float num = this.factorFromShooterAndDist * this.factorFromEquipment * this.factorFromWeather * this.factorFromCoveringGas * this.FactorFromExecution;
				num += this.offsetFromDarkness;
				if (num < 0.0201f)
				{
					num = 0.0201f;
				}
				return num;
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06005715 RID: 22293 RVA: 0x001C03E8 File Offset: 0x001BE5E8
		public float AimOnTargetChance_IgnoringPosture
		{
			get
			{
				return this.AimOnTargetChance_StandardTarget * this.factorFromTargetSize;
			}
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06005716 RID: 22294 RVA: 0x001C03F7 File Offset: 0x001BE5F7
		public float AimOnTargetChance
		{
			get
			{
				return this.AimOnTargetChance_IgnoringPosture * this.FactorFromPosture;
			}
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06005717 RID: 22295 RVA: 0x001C0406 File Offset: 0x001BE606
		public float PassCoverChance
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06005718 RID: 22296 RVA: 0x001C0414 File Offset: 0x001BE614
		public float TotalEstimatedHitChance
		{
			get
			{
				return Mathf.Clamp01(this.AimOnTargetChance * this.PassCoverChance);
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06005719 RID: 22297 RVA: 0x001C0428 File Offset: 0x001BE628
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x001C0430 File Offset: 0x001BE630
		public static ShotReport HitReportFor(Thing caster, Verb verb, LocalTargetInfo target)
		{
			IntVec3 cell = target.Cell;
			ShotReport shotReport;
			shotReport.distance = (cell - caster.Position).LengthHorizontal;
			shotReport.target = target.ToTargetInfo(caster.Map);
			CompUniqueWeapon compUniqueWeapon;
			bool flag = verb.EquipmentSource != null && verb.EquipmentSource.TryGetComp(out compUniqueWeapon) && compUniqueWeapon.IgnoreAccuracyMaluses;
			if (verb.verbProps.canGoWild)
			{
				shotReport.factorFromShooterAndDist = ShotReport.HitFactorFromShooter(caster, shotReport.distance, null);
			}
			else
			{
				shotReport.factorFromShooterAndDist = 1f;
			}
			shotReport.factorFromEquipment = verb.verbProps.GetHitChanceFactor(verb.EquipmentSource, shotReport.distance);
			shotReport.covers = CoverUtility.CalculateCoverGiverSet(target, caster.Position, caster.Map);
			shotReport.coversOverallBlockChance = CoverUtility.CalculateOverallBlockChance(target, caster.Position, caster.Map);
			shotReport.factorFromCoveringGas = 1f;
			if (verb.TryFindShootLineFromTo(verb.caster.Position, target, out shotReport.shootLine, false))
			{
				using (IEnumerator<IntVec3> enumerator = shotReport.shootLine.Points().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 cell2 = enumerator.Current;
						if (!flag && cell2.AnyGas(caster.Map, GasType.BlindSmoke))
						{
							shotReport.factorFromCoveringGas = 0.7f;
							break;
						}
					}
					goto IL_175;
				}
			}
			shotReport.shootLine = new ShootLine(IntVec3.Invalid, IntVec3.Invalid);
			IL_175:
			if (!flag && (!caster.Position.Roofed(caster.Map) || !target.Cell.Roofed(caster.Map)))
			{
				shotReport.factorFromWeather = caster.Map.weatherManager.CurWeatherAccuracyMultiplier;
			}
			else
			{
				shotReport.factorFromWeather = 1f;
			}
			if (target.HasThing)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					shotReport.factorFromTargetSize = pawn.BodySize;
				}
				else
				{
					shotReport.factorFromTargetSize = target.Thing.def.fillPercent * (float)target.Thing.def.size.x * (float)target.Thing.def.size.z * 2.5f;
				}
				shotReport.factorFromTargetSize = Mathf.Clamp(shotReport.factorFromTargetSize, 0.5f, 2f);
			}
			else
			{
				shotReport.factorFromTargetSize = 1f;
			}
			shotReport.forcedMissRadius = verb.verbProps.ForcedMissRadius;
			shotReport.offsetFromDarkness = 0f;
			if (ModsConfig.IdeologyActive && target.HasThing)
			{
				if (DarknessCombatUtility.IsOutdoorsAndLit(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyOutdoorsLitOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsOutdoorsAndDark(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyOutdoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndDark(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyIndoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndLit(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyIndoorsLitOffset, true, -1);
				}
			}
			return shotReport;
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x001C077C File Offset: 0x001BE97C
		public static float HitFactorFromShooter(Thing caster, float distance, float? acc = null)
		{
			float f;
			if (acc != null)
			{
				f = acc.Value;
			}
			else if (caster is Pawn)
			{
				f = caster.GetStatValue(StatDefOf.ShootingAccuracyPawn, true, -1);
			}
			else if (caster != null)
			{
				f = caster.GetStatValue(StatDefOf.ShootingAccuracyTurret, true, -1);
			}
			else
			{
				f = 1f;
			}
			float num = Mathf.Pow(f, distance);
			if (caster is Pawn)
			{
				if (distance <= 3f)
				{
					num *= caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Touch, true, -1);
				}
				else if (distance <= 12f)
				{
					num *= Mathf.Lerp(caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Touch, true, -1), caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Short, true, -1), (distance - 3f) / 12f - 3f);
				}
				else if (distance <= 25f)
				{
					num *= Mathf.Lerp(caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Short, true, -1), caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Medium, true, -1), (distance - 12f) / 25f - 12f);
				}
				else if (distance <= 40f)
				{
					num *= Mathf.Lerp(caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Medium, true, -1), caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Long, true, -1), (distance - 25f) / 40f - 25f);
				}
				else
				{
					num *= caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Long, true, -1);
				}
			}
			return Mathf.Max(num, 0.0201f);
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x001C08DC File Offset: 0x001BEADC
		public string GetTextReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.forcedMissRadius > 0.5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("WeaponMissRadius".Translate() + ": " + this.forcedMissRadius.ToString("F1"));
				stringBuilder.AppendLine("DirectHitChance".Translate() + ": " + (1f / (float)GenRadial.NumCellsInRadius(this.forcedMissRadius)).ToStringPercent());
			}
			else
			{
				stringBuilder.AppendLine(this.TotalEstimatedHitChance.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportShooterAbility".Translate() + ": " + this.factorFromShooterAndDist.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportWeapon".Translate() + ": " + this.factorFromEquipment.ToStringPercent());
				if (this.target.HasThing && this.factorFromTargetSize != 1f)
				{
					stringBuilder.AppendLine("   " + "TargetSize".Translate() + ": " + this.factorFromTargetSize.ToStringPercent());
				}
				if (this.factorFromWeather < 0.99f)
				{
					stringBuilder.AppendLine("   " + "Weather".Translate() + ": " + this.factorFromWeather.ToStringPercent());
				}
				if (this.factorFromCoveringGas < 0.99f)
				{
					stringBuilder.AppendLine("   " + "BlindSmoke".Translate().CapitalizeFirst() + ": " + this.factorFromCoveringGas.ToStringPercent());
				}
				if (this.FactorFromPosture < 0.9999f)
				{
					stringBuilder.AppendLine("   " + "TargetProne".Translate() + ": " + this.FactorFromPosture.ToStringPercent());
				}
				if (this.FactorFromExecution != 1f)
				{
					stringBuilder.AppendLine("   " + "Execution".Translate() + ": " + this.FactorFromExecution.ToStringPercent());
				}
				if (ModsConfig.IdeologyActive && this.target.HasThing && this.offsetFromDarkness != 0f)
				{
					if (DarknessCombatUtility.IsOutdoorsAndLit(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyOutdoorsLitOffset.LabelCap + ": " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsOutdoorsAndDark(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyOutdoorsDarkOffset.LabelCap + ": " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsIndoorsAndDark(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyIndoorsDarkOffset.LabelCap + ": " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsIndoorsAndLit(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyIndoorsLitOffset.LabelCap + "   " + this.offsetFromDarkness.ToStringPercent());
					}
				}
				if (this.PassCoverChance < 1f)
				{
					stringBuilder.AppendLine("   " + "ShootingCover".Translate() + ": " + this.PassCoverChance.ToStringPercent());
					for (int i = 0; i < this.covers.Count; i++)
					{
						CoverInfo coverInfo = this.covers[i];
						if (coverInfo.BlockChance > 0f)
						{
							stringBuilder.AppendLine("     " + "CoverThingBlocksPercentOfShots".Translate(coverInfo.Thing.LabelCap, coverInfo.BlockChance.ToStringPercent(), new NamedArgument(coverInfo.Thing.def, "COVER")).CapitalizeFirst());
						}
					}
				}
				else
				{
					stringBuilder.AppendLine("   (" + "NoCoverLower".Translate() + ")");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x001C0DF0 File Offset: 0x001BEFF0
		public Thing GetRandomCoverToMissInto()
		{
			CoverInfo coverInfo;
			if (this.covers.TryRandomElementByWeight((CoverInfo c) => c.BlockChance, out coverInfo))
			{
				return coverInfo.Thing;
			}
			return null;
		}

		// Token: 0x04003F75 RID: 16245
		private TargetInfo target;

		// Token: 0x04003F76 RID: 16246
		private float distance;

		// Token: 0x04003F77 RID: 16247
		private List<CoverInfo> covers;

		// Token: 0x04003F78 RID: 16248
		private float coversOverallBlockChance;

		// Token: 0x04003F79 RID: 16249
		private float factorFromShooterAndDist;

		// Token: 0x04003F7A RID: 16250
		private float factorFromEquipment;

		// Token: 0x04003F7B RID: 16251
		private float factorFromTargetSize;

		// Token: 0x04003F7C RID: 16252
		private float factorFromWeather;

		// Token: 0x04003F7D RID: 16253
		private float forcedMissRadius;

		// Token: 0x04003F7E RID: 16254
		private float offsetFromDarkness;

		// Token: 0x04003F7F RID: 16255
		private float factorFromCoveringGas;

		// Token: 0x04003F80 RID: 16256
		private ShootLine shootLine;
	}
}
