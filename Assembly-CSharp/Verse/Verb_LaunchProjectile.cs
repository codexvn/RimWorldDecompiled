using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D7D RID: 3453
	public class Verb_LaunchProjectile : Verb
	{
		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06005815 RID: 22549 RVA: 0x001C5DC3 File Offset: 0x001C3FC3
		public override float EffectiveRange
		{
			get
			{
				float effectiveRange = base.EffectiveRange;
				ThingWithComps equipmentSource = base.EquipmentSource;
				return effectiveRange * ((equipmentSource != null) ? equipmentSource.GetStatValue(StatDefOf.RangedWeapon_RangeMultiplier, true, -1) : 1f);
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06005816 RID: 22550 RVA: 0x001C5DEC File Offset: 0x001C3FEC
		public virtual ThingDef Projectile
		{
			get
			{
				ThingWithComps equipmentSource = base.EquipmentSource;
				CompChangeableProjectile compChangeableProjectile = (equipmentSource != null) ? equipmentSource.GetComp<CompChangeableProjectile>() : null;
				if (compChangeableProjectile != null && compChangeableProjectile.Loaded)
				{
					return compChangeableProjectile.Projectile;
				}
				return this.verbProps.defaultProjectile;
			}
		}

		// Token: 0x06005817 RID: 22551 RVA: 0x001C5E2C File Offset: 0x001C402C
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			BattleLog battleLog = Find.BattleLog;
			Thing caster = this.caster;
			Thing target = this.currentTarget.HasThing ? this.currentTarget.Thing : null;
			ThingWithComps equipmentSource = base.EquipmentSource;
			battleLog.Add(new BattleLogEntry_RangedFire(caster, target, (equipmentSource != null) ? equipmentSource.def : null, this.Projectile, this.ShotsPerBurst > 1));
		}

		// Token: 0x06005818 RID: 22552 RVA: 0x001C5E90 File Offset: 0x001C4090
		protected IntVec3 GetForcedMissTarget(float forcedMissRadius)
		{
			if (this.verbProps.forcedMissEvenDispersal)
			{
				if (this.forcedMissTargetEvenDispersalCache.Count <= 0)
				{
					this.forcedMissTargetEvenDispersalCache.AddRange(Verb_LaunchProjectile.GenerateEvenDispersalForcedMissTargets(this.currentTarget.Cell, forcedMissRadius, this.burstShotsLeft));
					this.forcedMissTargetEvenDispersalCache.SortByDescending((IntVec3 p) => p.DistanceToSquared(this.Caster.Position));
				}
				if (this.forcedMissTargetEvenDispersalCache.Count > 0)
				{
					return this.forcedMissTargetEvenDispersalCache.Pop<IntVec3>();
				}
			}
			int maxExclusive = GenRadial.NumCellsInRadius(forcedMissRadius);
			int num = Rand.Range(0, maxExclusive);
			return this.currentTarget.Cell + GenRadial.RadialPattern[num];
		}

		// Token: 0x06005819 RID: 22553 RVA: 0x001C5F35 File Offset: 0x001C4135
		private static IEnumerable<IntVec3> GenerateEvenDispersalForcedMissTargets(IntVec3 root, float radius, int count)
		{
			float randomRotationOffset = Rand.Range(0f, 360f);
			float goldenRatio = (1f + Mathf.Pow(5f, 0.5f)) / 2f;
			int num3;
			for (int i = 0; i < count; i = num3 + 1)
			{
				float f = 6.2831855f * (float)i / goldenRatio;
				float f2 = Mathf.Acos(1f - 2f * ((float)i + 0.5f) / (float)count);
				float num = (float)((int)(Mathf.Cos(f) * Mathf.Sin(f2) * radius));
				int num2 = (int)(Mathf.Cos(f2) * radius);
				Vector3 vect = new Vector3(num, 0f, (float)num2).RotatedBy(randomRotationOffset);
				yield return root + vect.ToIntVec3();
				num3 = i;
			}
			yield break;
		}

		// Token: 0x0600581A RID: 22554 RVA: 0x001C5F54 File Offset: 0x001C4154
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			ThingDef projectile = this.Projectile;
			if (projectile == null)
			{
				return false;
			}
			ShootLine shootLine;
			bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine, false);
			if (this.verbProps.stopBurstWithoutLos && !flag)
			{
				return false;
			}
			if (base.EquipmentSource != null)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompApparelVerbOwner_Charged comp2 = base.EquipmentSource.GetComp<CompApparelVerbOwner_Charged>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			this.lastShotTick = Find.TickManager.TicksGame;
			Thing thing = this.caster;
			Thing thing2 = base.EquipmentSource;
			CompMannable compMannable = this.caster.TryGetComp<CompMannable>();
			if (((compMannable != null) ? compMannable.ManningPawn : null) != null)
			{
				thing = compMannable.ManningPawn;
				thing2 = this.caster;
			}
			Vector3 drawPos = this.caster.DrawPos;
			Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, shootLine.Source, this.caster.Map, WipeMode.Vanish);
			CompUniqueWeapon compUniqueWeapon;
			if (thing2.TryGetComp(out compUniqueWeapon))
			{
				foreach (WeaponTraitDef weaponTraitDef in compUniqueWeapon.TraitsListForReading)
				{
					if (weaponTraitDef.damageDefOverride != null)
					{
						projectile2.damageDefOverride = weaponTraitDef.damageDefOverride;
					}
					if (!weaponTraitDef.extraDamages.NullOrEmpty<ExtraDamage>())
					{
						Projectile projectile3 = projectile2;
						if (projectile3.extraDamages == null)
						{
							projectile3.extraDamages = new List<ExtraDamage>();
						}
						projectile2.extraDamages.AddRange(weaponTraitDef.extraDamages);
					}
				}
			}
			if (this.verbProps.ForcedMissRadius > 0.5f)
			{
				float num = this.verbProps.ForcedMissRadius;
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					num *= this.verbProps.GetForceMissFactorFor(thing2, pawn);
				}
				float num2 = VerbUtility.CalculateAdjustedForcedMiss(num, this.currentTarget.Cell - this.caster.Position);
				if (num2 > 0.5f)
				{
					IntVec3 forcedMissTarget = this.GetForcedMissTarget(num2);
					if (forcedMissTarget != this.currentTarget.Cell)
					{
						this.ThrowDebugText("ToRadius");
						this.ThrowDebugText("Rad\nDest", forcedMissTarget);
						ProjectileHitFlags projectileHitFlags = ProjectileHitFlags.NonTargetWorld;
						if (Rand.Chance(0.5f))
						{
							projectileHitFlags = ProjectileHitFlags.All;
						}
						if (!this.canHitNonTargetPawnsNow)
						{
							projectileHitFlags &= ~ProjectileHitFlags.NonTargetPawns;
						}
						projectile2.Launch(thing, drawPos, forcedMissTarget, this.currentTarget, projectileHitFlags, this.preventFriendlyFire, thing2, null);
						return true;
					}
				}
			}
			ShotReport shotReport = ShotReport.HitReportFor(this.caster, this, this.currentTarget);
			Thing randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
			ThingDef targetCoverDef = (randomCoverToMissInto != null) ? randomCoverToMissInto.def : null;
			if (this.verbProps.canGoWild && !Rand.Chance(shotReport.AimOnTargetChance_IgnoringPosture))
			{
				bool flag2;
				if (projectile2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					ThingDef def = projectile2.def;
					flag2 = (((def != null) ? def.projectile : null) != null);
				}
				bool flyOverhead = flag2 && projectile2.def.projectile.flyOverhead;
				shootLine.ChangeDestToMissWild(shotReport.AimOnTargetChance_StandardTarget, flyOverhead, this.caster.Map);
				this.ThrowDebugText("ToWild" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
				this.ThrowDebugText("Wild\nDest", shootLine.Dest);
				ProjectileHitFlags projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
				if (Rand.Chance(0.5f) && this.canHitNonTargetPawnsNow)
				{
					projectileHitFlags2 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(thing, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags2, this.preventFriendlyFire, thing2, targetCoverDef);
				return true;
			}
			if (this.currentTarget.Thing != null && this.currentTarget.Thing.def.CanBenefitFromCover && !Rand.Chance(shotReport.PassCoverChance))
			{
				this.ThrowDebugText("ToCover" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
				this.ThrowDebugText("Cover\nDest", randomCoverToMissInto.Position);
				ProjectileHitFlags projectileHitFlags3 = ProjectileHitFlags.NonTargetWorld;
				if (this.canHitNonTargetPawnsNow)
				{
					projectileHitFlags3 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(thing, drawPos, randomCoverToMissInto, this.currentTarget, projectileHitFlags3, this.preventFriendlyFire, thing2, targetCoverDef);
				return true;
			}
			ProjectileHitFlags projectileHitFlags4 = ProjectileHitFlags.IntendedTarget;
			if (this.canHitNonTargetPawnsNow)
			{
				projectileHitFlags4 |= ProjectileHitFlags.NonTargetPawns;
			}
			if (!this.currentTarget.HasThing || this.currentTarget.Thing.def.Fillage == FillCategory.Full)
			{
				projectileHitFlags4 |= ProjectileHitFlags.NonTargetWorld;
			}
			this.ThrowDebugText("ToHit" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
			if (this.currentTarget.Thing != null)
			{
				projectile2.Launch(thing, drawPos, this.currentTarget, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, thing2, targetCoverDef);
				this.ThrowDebugText("Hit\nDest", this.currentTarget.Cell);
			}
			else
			{
				projectile2.Launch(thing, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, thing2, targetCoverDef);
				this.ThrowDebugText("Hit\nDest", shootLine.Dest);
			}
			return true;
		}

		// Token: 0x0600581B RID: 22555 RVA: 0x001C649C File Offset: 0x001C469C
		private void ThrowDebugText(string text)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, text, -1f);
			}
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x001C64C6 File Offset: 0x001C46C6
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), this.caster.Map, text, -1f);
			}
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x001C64EC File Offset: 0x001C46EC
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = true;
			ThingDef projectile = this.Projectile;
			if (projectile == null)
			{
				return 0f;
			}
			float num = projectile.projectile.explosionRadius + projectile.projectile.explosionRadiusDisplayPadding;
			float forcedMissRadius = this.verbProps.ForcedMissRadius;
			if (forcedMissRadius > 0f && base.BurstShotCount > 1)
			{
				num += forcedMissRadius;
			}
			return num;
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x001C6548 File Offset: 0x001C4748
		public override bool Available()
		{
			if (!base.Available())
			{
				return false;
			}
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Faction != Faction.OfPlayer && !this.verbProps.ai_ProjectileLaunchingIgnoresMeleeThreats && casterPawn.mindState.MeleeThreatStillThreat && casterPawn.mindState.meleeThreat.Position.AdjacentTo8WayOrInside(casterPawn.Position))
				{
					return false;
				}
			}
			return this.Projectile != null;
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x001C65BD File Offset: 0x001C47BD
		public override void Reset()
		{
			base.Reset();
			this.forcedMissTargetEvenDispersalCache.Clear();
		}

		// Token: 0x0400400B RID: 16395
		private List<IntVec3> forcedMissTargetEvenDispersalCache = new List<IntVec3>();
	}
}
