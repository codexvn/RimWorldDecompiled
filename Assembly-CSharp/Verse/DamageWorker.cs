using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200023A RID: 570
	public class DamageWorker
	{
		// Token: 0x06000EC0 RID: 3776 RVA: 0x000448E4 File Offset: 0x00042AE4
		public virtual DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (victim.SpawnedOrAnyParentSpawned)
			{
				ImpactSoundUtility.PlayImpactSound(victim, dinfo.Def.impactSoundType, victim.MapHeld);
			}
			if (victim.def.useHitPoints && dinfo.Def.harmsHealth)
			{
				float num = dinfo.Amount;
				if (victim.def.category == ThingCategory.Building)
				{
					num *= dinfo.Def.buildingDamageFactor;
					if (victim.def.passability == Traversability.Impassable)
					{
						num *= dinfo.Def.buildingDamageFactorImpassable;
					}
					else
					{
						num *= dinfo.Def.buildingDamageFactorPassable;
					}
					if (dinfo.Def.scaleDamageToBuildingsBasedOnFlammability)
					{
						num *= Mathf.Max(0.05f, victim.GetStatValue(StatDefOf.Flammability, true, -1));
					}
					Pawn pawn = dinfo.Instigator as Pawn;
					if (pawn != null && pawn.IsShambler)
					{
						num *= 1.5f;
					}
					if (ModsConfig.BiotechActive && dinfo.Instigator != null && (dinfo.WeaponBodyPartGroup != null || (dinfo.Weapon != null && dinfo.Weapon.IsMeleeWeapon)) && victim.def.IsDoor)
					{
						num *= dinfo.Instigator.GetStatValue(StatDefOf.MeleeDoorDamageFactor, true, -1);
					}
				}
				if (victim.def.category == ThingCategory.Plant)
				{
					num *= dinfo.Def.plantDamageFactor;
				}
				else if (victim.def.IsCorpse)
				{
					num *= dinfo.Def.corpseDamageFactor;
				}
				damageResult.totalDamageDealt = (float)Mathf.Min(victim.HitPoints, GenMath.RoundRandom(num));
				victim.HitPoints -= Mathf.RoundToInt(damageResult.totalDamageDealt);
				if (victim.HitPoints <= 0)
				{
					victim.HitPoints = 0;
					victim.Kill(new DamageInfo?(dinfo), null);
				}
			}
			return damageResult;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00044AB4 File Offset: 0x00042CB4
		public virtual void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			if (this.def.explosionHeatEnergyPerCell > 1E-45f)
			{
				GenTemperature.PushHeat(explosion.Position, explosion.Map, this.def.explosionHeatEnergyPerCell * (float)cellsToAffect.Count);
			}
			if (explosion.doVisualEffects)
			{
				FleckMaker.Static(explosion.Position, explosion.Map, FleckDefOf.ExplosionFlash, explosion.radius * 6f);
				if (explosion.Map == Find.CurrentMap)
				{
					float magnitude = (explosion.Position.ToVector3Shifted() - Find.Camera.transform.position).magnitude;
					Find.CameraDriver.shaker.DoShake(4f * explosion.radius * explosion.screenShakeFactor / magnitude);
				}
				this.ExplosionVisualEffectCenter(explosion);
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00044B88 File Offset: 0x00042D88
		protected virtual void ExplosionVisualEffectCenter(Explosion explosion)
		{
			for (int i = 0; i < 4; i++)
			{
				FleckMaker.ThrowSmoke(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, explosion.radius * 0.6f);
			}
			if (this.def.explosionCenterFleck != null)
			{
				FleckMaker.Static(explosion.Position.ToVector3Shifted(), explosion.Map, this.def.explosionCenterFleck, 1f);
			}
			else if (this.def.explosionCenterMote != null)
			{
				MoteMaker.MakeStaticMote(explosion.Position.ToVector3Shifted(), explosion.Map, this.def.explosionCenterMote, 1f, false, 0f);
			}
			if (this.def.explosionCenterEffecter != null)
			{
				this.def.explosionCenterEffecter.Spawn(explosion.Position, explosion.Map, Vector3.zero, 1f);
			}
			if (this.def.explosionInteriorMote != null || this.def.explosionInteriorFleck != null || this.def.explosionInteriorEffecter != null)
			{
				int num = Mathf.RoundToInt(3.1415927f * explosion.radius * explosion.radius / 6f * this.def.explosionInteriorCellCountMultiplier);
				for (int j = 0; j < num; j++)
				{
					Vector3 b = Gen.RandomHorizontalVector(explosion.radius * this.def.explosionInteriorCellDistanceMultiplier);
					if (this.def.explosionInteriorEffecter != null)
					{
						Vector3 vect = explosion.Position.ToVector3Shifted() + b;
						this.def.explosionInteriorEffecter.Spawn(explosion.Position, vect.ToIntVec3(), explosion.Map, 1f);
					}
					else if (this.def.explosionInteriorFleck != null)
					{
						FleckMaker.ThrowExplosionInterior(explosion.Position.ToVector3Shifted() + b, explosion.Map, this.def.explosionInteriorFleck);
					}
					else
					{
						MoteMaker.ThrowExplosionInteriorMote(explosion.Position.ToVector3Shifted() + b, explosion.Map, this.def.explosionInteriorMote);
					}
				}
			}
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00044DBC File Offset: 0x00042FBC
		public virtual void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			if (explosion.doVisualEffects && (this.def.explosionCellMote != null || this.def.explosionCellFleck != null) && canThrowMotes)
			{
				float t = Mathf.Clamp01((explosion.Position - c).LengthHorizontal / explosion.radius);
				Color color = Color.Lerp(this.def.explosionColorCenter, this.def.explosionColorEdge, t);
				if (this.def.explosionCellMote != null)
				{
					Mote mote = c.GetFirstThing(explosion.Map, this.def.explosionCellMote) as Mote;
					if (mote != null)
					{
						mote.spawnTick = Find.TickManager.TicksGame;
					}
					else
					{
						MoteMaker.ThrowExplosionCell(c, explosion.Map, this.def.explosionCellMote, color);
					}
				}
				else
				{
					FleckMaker.ThrowExplosionCell(c, explosion.Map, this.def.explosionCellFleck, color);
				}
			}
			if (this.def.explosionCellEffecter != null && (this.def.explosionCellEffecterMaxRadius < 1E-45f || c.InHorDistOf(explosion.Position, this.def.explosionCellEffecterMaxRadius)) && Rand.Chance(this.def.explosionCellEffecterChance))
			{
				this.def.explosionCellEffecter.Spawn(explosion.Position, c, explosion.Map, 1f);
			}
			DamageWorker.thingsToAffect.Clear();
			float num = float.MinValue;
			bool flag = false;
			List<Thing> list = explosion.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.category != ThingCategory.Mote && thing.def.category != ThingCategory.Ethereal)
				{
					DamageWorker.thingsToAffect.Add(thing);
					if (thing.def.Fillage == FillCategory.Full && thing.def.Altitude > num)
					{
						flag = true;
						num = thing.def.Altitude;
					}
				}
			}
			for (int j = 0; j < DamageWorker.thingsToAffect.Count; j++)
			{
				if (DamageWorker.thingsToAffect[j].def.Altitude >= num)
				{
					this.ExplosionDamageThing(explosion, DamageWorker.thingsToAffect[j], damagedThings, ignoredThings, c);
				}
			}
			if (!flag)
			{
				this.ExplosionDamageTerrain(explosion, c);
			}
			if (this.def.explosionSnowMeltAmount > 0.0001f)
			{
				float lengthHorizontal = (c - explosion.Position).LengthHorizontal;
				float num2 = 1f - lengthHorizontal / explosion.radius;
				if (num2 > 0f)
				{
					explosion.Map.snowGrid.AddDepth(c, -num2 * this.def.explosionSnowMeltAmount);
				}
			}
			if (this.def == DamageDefOf.Bomb || this.def == DamageDefOf.Flame)
			{
				List<Thing> list2 = explosion.Map.listerThings.ThingsOfDef(ThingDefOf.RectTrigger);
				for (int k = 0; k < list2.Count; k++)
				{
					RectTrigger rectTrigger = (RectTrigger)list2[k];
					if (rectTrigger.activateOnExplosion && rectTrigger.Rect.Contains(c))
					{
						rectTrigger.ActivatedBy(null);
					}
				}
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x000450EC File Offset: 0x000432EC
		protected virtual void ExplosionDamageThing(Explosion explosion, Thing t, List<Thing> damagedThings, List<Thing> ignoredThings, IntVec3 cell)
		{
			if (t.def.category == ThingCategory.Mote || t.def.category == ThingCategory.Ethereal)
			{
				return;
			}
			if (damagedThings.Contains(t))
			{
				return;
			}
			damagedThings.Add(t);
			if (ignoredThings != null && ignoredThings.Contains(t))
			{
				return;
			}
			if (this.def == DamageDefOf.Bomb && t.def == ThingDefOf.Fire && !t.Destroyed)
			{
				t.Destroy(DestroyMode.Vanish);
				return;
			}
			float angle;
			if (t.Position == explosion.Position)
			{
				angle = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				angle = (t.Position - explosion.Position).AngleFlat;
			}
			DamageInfo dinfo = new DamageInfo(this.def, (float)explosion.GetDamageAmountAt(cell), explosion.GetArmorPenetrationAt(cell), angle, explosion.instigator, null, explosion.weapon, DamageInfo.SourceCategory.ThingOrUnknown, explosion.intendedTarget, true, true, QualityCategory.Normal, true, false);
			if (this.def.explosionAffectOutsidePartsOnly)
			{
				dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
			}
			BattleLogEntry_ExplosionImpact battleLogEntry_ExplosionImpact = null;
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				battleLogEntry_ExplosionImpact = new BattleLogEntry_ExplosionImpact(explosion.instigator, t, explosion.weapon, explosion.projectile, this.def);
				Find.BattleLog.Add(battleLogEntry_ExplosionImpact);
			}
			DamageWorker.DamageResult damageResult = t.TakeDamage(dinfo);
			damageResult.AssociateWithLog(battleLogEntry_ExplosionImpact);
			if (pawn != null && damageResult.wounded && pawn.stances != null)
			{
				pawn.stances.stagger.StaggerFor(95, 0.17f);
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00045260 File Offset: 0x00043460
		protected virtual void ExplosionDamageTerrain(Explosion explosion, IntVec3 c)
		{
			if (this.def != DamageDefOf.Bomb)
			{
				return;
			}
			if (!explosion.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(explosion.Map);
			if (terrain.destroyOnBombDamageThreshold < 0f)
			{
				return;
			}
			if ((float)explosion.GetDamageAmountAt(c) >= terrain.destroyOnBombDamageThreshold)
			{
				explosion.Map.terrainGrid.Notify_TerrainDestroyed(c);
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x000452CB File Offset: 0x000434CB
		public IEnumerable<IntVec3> ExplosionCellsToHit(Explosion explosion)
		{
			return this.ExplosionCellsToHit(explosion.Position, explosion.Map, explosion.radius, explosion.needLOSToCell1, explosion.needLOSToCell2, explosion.affectedAngle);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x000452F8 File Offset: 0x000434F8
		public virtual IEnumerable<IntVec3> ExplosionCellsToHit(IntVec3 center, Map map, float radius, IntVec3? needLOSToCell1 = null, IntVec3? needLOSToCell2 = null, FloatRange? affectedAngle = null)
		{
			DamageWorker.openCells.Clear();
			DamageWorker.adjWallCells.Clear();
			float num = (affectedAngle != null) ? affectedAngle.GetValueOrDefault().min : 0f;
			float num2 = (affectedAngle != null) ? affectedAngle.GetValueOrDefault().max : 0f;
			int num3 = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num3; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && GenSight.LineOfSight(center, intVec, map, true, null, 0, 0))
				{
					if (affectedAngle != null)
					{
						float lengthHorizontal = (intVec - center).LengthHorizontal;
						float num4 = lengthHorizontal / radius;
						if (lengthHorizontal <= 0.5f)
						{
							goto IL_16B;
						}
						float num5 = Mathf.Atan2((float)(-(float)(intVec.z - center.z)), (float)(intVec.x - center.x)) * 57.29578f;
						float num6 = num;
						float num7 = num2;
						if (num5 - num6 < -0.5f * num4 || num5 - num7 > 0.5f * num4)
						{
							goto IL_16B;
						}
					}
					if (needLOSToCell1 != null || needLOSToCell2 != null)
					{
						bool flag = needLOSToCell1 != null && GenSight.LineOfSight(needLOSToCell1.Value, intVec, map);
						bool flag2 = needLOSToCell2 != null && GenSight.LineOfSight(needLOSToCell2.Value, intVec, map);
						if (!flag && !flag2)
						{
							goto IL_16B;
						}
					}
					DamageWorker.openCells.Add(intVec);
				}
				IL_16B:;
			}
			for (int j = 0; j < DamageWorker.openCells.Count; j++)
			{
				IntVec3 intVec2 = DamageWorker.openCells[j];
				Building edifice = intVec2.GetEdifice(map);
				if (intVec2.Walkable(map))
				{
					if (edifice != null && edifice.def.Fillage == FillCategory.Full)
					{
						Building_Door building_Door = edifice as Building_Door;
						if (building_Door == null || !building_Door.Open)
						{
							goto IL_242;
						}
					}
					for (int k = 0; k < 4; k++)
					{
						IntVec3 intVec3 = intVec2 + GenAdj.CardinalDirections[k];
						if (intVec3.InHorDistOf(center, radius) && intVec3.InBounds(map) && !intVec3.Standable(map) && intVec3.GetEdifice(map) != null && !DamageWorker.openCells.Contains(intVec3) && !DamageWorker.adjWallCells.Contains(intVec3))
						{
							DamageWorker.adjWallCells.Add(intVec3);
						}
					}
				}
				IL_242:;
			}
			return DamageWorker.openCells.Concat(DamageWorker.adjWallCells);
		}

		// Token: 0x0400099F RID: 2463
		public DamageDef def;

		// Token: 0x040009A0 RID: 2464
		private const float ExplosionCamShakeMultiplier = 4f;

		// Token: 0x040009A1 RID: 2465
		private const float DamageToBuildingsFromFlammabilityMinFactor = 0.05f;

		// Token: 0x040009A2 RID: 2466
		private static List<Thing> thingsToAffect = new List<Thing>();

		// Token: 0x040009A3 RID: 2467
		private static List<IntVec3> openCells = new List<IntVec3>();

		// Token: 0x040009A4 RID: 2468
		private static List<IntVec3> adjWallCells = new List<IntVec3>();

		// Token: 0x0200023B RID: 571
		public class DamageResult
		{
			// Token: 0x17000265 RID: 613
			// (get) Token: 0x06000ECA RID: 3786 RVA: 0x0004558D File Offset: 0x0004378D
			public BodyPartRecord LastHitPart
			{
				get
				{
					if (this.parts == null)
					{
						return null;
					}
					if (this.parts.Count <= 0)
					{
						return null;
					}
					return this.parts[this.parts.Count - 1];
				}
			}

			// Token: 0x06000ECB RID: 3787 RVA: 0x000455C4 File Offset: 0x000437C4
			public void AddPart(Thing hitThing, BodyPartRecord part)
			{
				if (this.hitThing != null && this.hitThing != hitThing)
				{
					Log.ErrorOnce("Single damage worker referring to multiple things; will cause issues with combat log", 30667935);
				}
				this.hitThing = hitThing;
				if (this.parts == null)
				{
					this.parts = new List<BodyPartRecord>();
				}
				this.parts.Add(part);
			}

			// Token: 0x06000ECC RID: 3788 RVA: 0x00045617 File Offset: 0x00043817
			public void AddHediff(Hediff hediff)
			{
				if (this.hediffs == null)
				{
					this.hediffs = new List<Hediff>();
				}
				this.hediffs.Add(hediff);
			}

			// Token: 0x06000ECD RID: 3789 RVA: 0x00045638 File Offset: 0x00043838
			public void AssociateWithLog(LogEntry_DamageResult log)
			{
				if (log == null)
				{
					return;
				}
				Thing thing = this.hitThing;
				Pawn hitPawn = thing as Pawn;
				if (hitPawn != null)
				{
					List<BodyPartRecord> list = null;
					List<bool> recipientPartsDestroyed = null;
					if (!this.parts.NullOrEmpty<BodyPartRecord>())
					{
						list = this.parts.Distinct<BodyPartRecord>().ToList<BodyPartRecord>();
						recipientPartsDestroyed = (from part in list
						select hitPawn.health.hediffSet.GetPartHealth(part) <= 0f).ToList<bool>();
					}
					log.FillTargets(list, recipientPartsDestroyed, this.deflected);
				}
				if (this.hediffs != null)
				{
					for (int i = 0; i < this.hediffs.Count; i++)
					{
						this.hediffs[i].combatLogEntry = new WeakReference<LogEntry>(log);
						this.hediffs[i].combatLogText = log.ToGameStringFromPOV(null, false);
					}
				}
			}

			// Token: 0x040009A5 RID: 2469
			public bool wounded;

			// Token: 0x040009A6 RID: 2470
			public bool headshot;

			// Token: 0x040009A7 RID: 2471
			public bool deflected;

			// Token: 0x040009A8 RID: 2472
			public bool stunned;

			// Token: 0x040009A9 RID: 2473
			public bool deflectedByMetalArmor;

			// Token: 0x040009AA RID: 2474
			public bool diminished;

			// Token: 0x040009AB RID: 2475
			public bool diminishedByMetalArmor;

			// Token: 0x040009AC RID: 2476
			public Thing hitThing;

			// Token: 0x040009AD RID: 2477
			public List<BodyPartRecord> parts;

			// Token: 0x040009AE RID: 2478
			public List<Hediff> hediffs;

			// Token: 0x040009AF RID: 2479
			public float totalDamageDealt;
		}
	}
}
