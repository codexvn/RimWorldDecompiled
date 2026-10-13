using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000878 RID: 2168
	public class Explosion : Thing
	{
		// Token: 0x060036F2 RID: 14066 RVA: 0x00126860 File Offset: 0x00124A60
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.cellsToAffect = SimplePool<List<IntVec3>>.Get();
				this.cellsToAffect.Clear();
				this.damagedThings = SimplePool<List<Thing>>.Get();
				this.damagedThings.Clear();
				this.addedCellsAffectedOnlyByDamage = SimplePool<HashSet<IntVec3>>.Get();
				this.addedCellsAffectedOnlyByDamage.Clear();
			}
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x001268C4 File Offset: 0x00124AC4
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.cellsToAffect.Clear();
			SimplePool<List<IntVec3>>.Return(this.cellsToAffect);
			this.cellsToAffect = null;
			this.damagedThings.Clear();
			SimplePool<List<Thing>>.Return(this.damagedThings);
			this.damagedThings = null;
			this.addedCellsAffectedOnlyByDamage.Clear();
			SimplePool<HashSet<IntVec3>>.Return(this.addedCellsAffectedOnlyByDamage);
			this.addedCellsAffectedOnlyByDamage = null;
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x00126930 File Offset: 0x00124B30
		public virtual void StartExplosion(SoundDef explosionSound, List<Thing> ignoredThings)
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartExplosion() on unspawned thing.");
				return;
			}
			this.startTick = Find.TickManager.TicksGame;
			this.ignoredThings = ignoredThings;
			this.cellsToAffect.Clear();
			this.damagedThings.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			if (!this.overrideCells.NullOrEmpty<IntVec3>())
			{
				this.cellsToAffect.AddRange(this.overrideCells);
			}
			else
			{
				this.cellsToAffect.AddRange(this.damType.Worker.ExplosionCellsToHit(this));
			}
			if (this.applyDamageToExplosionCellsNeighbors)
			{
				this.AddCellsNeighbors(this.cellsToAffect);
			}
			this.damType.Worker.ExplosionStart(this, this.cellsToAffect);
			this.PlayExplosionSound(explosionSound);
			if (this.doVisualEffects)
			{
				FleckMaker.WaterSplash(base.Position.ToVector3Shifted(), base.Map, this.radius * 6f, 20f);
			}
			this.cellsToAffect.Sort((IntVec3 a, IntVec3 b) => this.GetCellAffectTick(b).CompareTo(this.GetCellAffectTick(a)));
			RegionTraverser.BreadthFirstTraverse(base.Position, base.Map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> allThings = x.ListerThings.AllThings;
				for (int i = allThings.Count - 1; i >= 0; i--)
				{
					if (allThings[i].Spawned)
					{
						allThings[i].Notify_Explosion(this);
					}
				}
				return false;
			}, 25, RegionType.Set_Passable);
			this.TrySpawnSingleThing(this.preExplosionSpawnSingleThingDef);
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x00126A8C File Offset: 0x00124C8C
		protected override void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			int num = this.cellsToAffect.Count - 1;
			while (num >= 0 && ticksGame >= this.GetCellAffectTick(this.cellsToAffect[num]))
			{
				try
				{
					this.AffectCell(this.cellsToAffect[num]);
				}
				catch (Exception ex)
				{
					string str = "Explosion could not affect cell ";
					string str2 = this.cellsToAffect[num].ToString();
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				this.cellsToAffect.RemoveAt(num);
				num--;
			}
			if (!this.cellsToAffect.Any<IntVec3>())
			{
				this.ExplosionEnded();
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x00126B5C File Offset: 0x00124D5C
		protected virtual void ExplosionEnded()
		{
			this.TrySpawnSingleThing(this.postExplosionSpawnSingleThingDef);
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x00126B6C File Offset: 0x00124D6C
		private void TrySpawnSingleThing(ThingDef thingDef)
		{
			if (thingDef == null)
			{
				return;
			}
			CellRect cellRect = base.Position.RectAbout(thingDef.Size);
			bool flag = false;
			if (thingDef.terrainAffordanceNeeded != null)
			{
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.GetAffordances(base.Map).Contains(thingDef.terrainAffordanceNeeded))
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				this.TrySpawnExplosionThing(thingDef, base.Position, 1);
			}
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x00126C00 File Offset: 0x00124E00
		public int GetDamageAmountAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.damAmount;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Max(GenMath.RoundRandom(Mathf.Lerp((float)this.damAmount, (float)this.damAmount * 0.2f, t)), 1);
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x00126C58 File Offset: 0x00124E58
		public float GetArmorPenetrationAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.armorPenetration;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Lerp(this.armorPenetration, this.armorPenetration * 0.2f, t);
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x00126CA0 File Offset: 0x00124EA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damType, "damType");
			Scribe_Values.Look<int>(ref this.damAmount, "damAmount", 0, false);
			Scribe_Values.Look<float>(ref this.armorPenetration, "armorPenetration", 0f, false);
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weapon, "weapon");
			Scribe_Defs.Look<ThingDef>(ref this.projectile, "projectile");
			Scribe_References.Look<Thing>(ref this.intendedTarget, "intendedTarget", false);
			Scribe_Values.Look<bool>(ref this.applyDamageToExplosionCellsNeighbors, "applyDamageToExplosionCellsNeighbors", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.preExplosionSpawnThingDef, "preExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.preExplosionSpawnChance, "preExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.preExplosionSpawnThingCount, "preExplosionSpawnThingCount", 1, false);
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDef, "postExplosionSpawnThingDef");
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDefWater, "postExplosionSpawnThingDefWater");
			Scribe_Values.Look<float>(ref this.postExplosionSpawnChance, "postExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.postExplosionSpawnThingCount, "postExplosionSpawnThingCount", 1, false);
			Scribe_Values.Look<GasType?>(ref this.postExplosionGasType, "postExplosionGasType", null, false);
			Scribe_Values.Look<float>(ref this.chanceToStartFire, "chanceToStartFire", 0f, false);
			Scribe_Deep.Look<SimpleCurve>(ref this.flammabilityChanceCurve, "flammabilityChanceCurve", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.damageFalloff, "dealMoreDamageAtCenter", false, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell1, "needLOSToCell1", null, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell2, "needLOSToCell2", null, false);
			Scribe_Values.Look<FloatRange?>(ref this.affectedAngle, "affectedAngle", null, false);
			Scribe_Values.Look<float>(ref this.propagationSpeed, "propagationSpeed", 0f, false);
			Scribe_Values.Look<float>(ref this.excludeRadius, "canTargetLocations", 0f, false);
			Scribe_Values.Look<bool>(ref this.doSoundEffects, "doSoundEffects", true, false);
			Scribe_Values.Look<bool>(ref this.doVisualEffects, "doVisualEffects", true, false);
			Scribe_Values.Look<float>(ref this.screenShakeFactor, "screenShakeFactor", 1f, false);
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnSingleThingDef, "postExplosionSpawnSingleThingDef");
			Scribe_Defs.Look<ThingDef>(ref this.preExplosionSpawnSingleThingDef, "preExplosionSpawnSingleThingDef");
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Collections.Look<IntVec3>(ref this.cellsToAffect, "cellsToAffect", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.damagedThings, "damagedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.ignoredThings, "ignoredThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.addedCellsAffectedOnlyByDamage, "addedCellsAffectedOnlyByDamage", LookMode.Value);
			Scribe_Collections.Look<IntVec3>(ref this.overrideCells, "overrideCells", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.damagedThings != null)
				{
					this.damagedThings.RemoveAll((Thing x) => x == null);
				}
				if (this.ignoredThings != null)
				{
					this.ignoredThings.RemoveAll((Thing x) => x == null);
				}
			}
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x00126FEC File Offset: 0x001251EC
		private int GetCellAffectTick(IntVec3 cell)
		{
			return this.startTick + (int)((cell - base.Position).LengthHorizontal * 1.5f / this.propagationSpeed);
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x00127024 File Offset: 0x00125224
		private void AffectCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return;
			}
			if (this.excludeRadius > 0f && (float)c.DistanceToSquared(base.Position) < this.excludeRadius * this.excludeRadius)
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(base.Map);
			bool flag = this.ShouldCellBeAffectedOnlyByDamage(c);
			if (!flag && Rand.Chance(this.preExplosionSpawnChance) && c.Walkable(base.Map))
			{
				this.TrySpawnExplosionThing(this.preExplosionSpawnThingDef, c, this.preExplosionSpawnThingCount);
			}
			this.damType.Worker.ExplosionAffectCell(this, c, this.damagedThings, this.ignoredThings, !flag);
			if (!flag)
			{
				if (Rand.Chance(this.postExplosionSpawnChance) && c.Walkable(base.Map))
				{
					ThingDef thingDef = terrain.IsWater ? (this.postExplosionSpawnThingDefWater ?? this.postExplosionSpawnThingDef) : this.postExplosionSpawnThingDef;
					this.TrySpawnExplosionThing(thingDef, c, this.postExplosionSpawnThingCount);
				}
				if (this.postExplosionGasType != null)
				{
					float num = this.postExplosionGasRadiusOverride ?? this.radius;
					float num2 = num * num;
					if ((float)c.DistanceToSquared(base.Position) <= num2)
					{
						GasUtility.AddGas(c, base.Map, this.postExplosionGasType.Value, this.postExplosionGasAmount);
					}
				}
			}
			float num3 = this.chanceToStartFire;
			if (this.damageFalloff)
			{
				num3 *= Mathf.Lerp(1f, 0.2f, c.DistanceTo(base.Position) / this.radius);
			}
			if (Rand.Chance(num3))
			{
				FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f), this.instigator, this.flammabilityChanceCurve);
			}
			if (terrain.temporary)
			{
				TempTerrainProps tempTerrain = terrain.tempTerrain;
				if (tempTerrain != null && tempTerrain.removedByExplosions)
				{
					base.Map.terrainGrid.RemoveTempTerrain(c, false, false);
				}
			}
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x00127214 File Offset: 0x00125414
		private void TrySpawnExplosionThing(ThingDef thingDef, IntVec3 c, int count)
		{
			if (thingDef == null)
			{
				return;
			}
			if (thingDef.IsFilth)
			{
				FilthMaker.TryMakeFilth(c, base.Map, thingDef, count, FilthSourceFlags.None, true);
				return;
			}
			Thing thing;
			if (GenSpawn.TrySpawn(thingDef, c, base.Map, out thing, WipeMode.Vanish, true))
			{
				thing.stackCount = count;
				CompReleaseGas compReleaseGas = thing.TryGetComp<CompReleaseGas>();
				if (compReleaseGas == null)
				{
					return;
				}
				compReleaseGas.StartRelease();
			}
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x0012726C File Offset: 0x0012546C
		private void PlayExplosionSound(SoundDef explosionSound)
		{
			if (!this.doSoundEffects)
			{
				return;
			}
			bool flag;
			if (Prefs.DevMode)
			{
				flag = (explosionSound != null);
			}
			else
			{
				flag = !explosionSound.NullOrUndefined();
			}
			if (flag)
			{
				explosionSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				return;
			}
			this.damType.soundExplosion.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x001272E4 File Offset: 0x001254E4
		private void AddCellsNeighbors(List<IntVec3> cells)
		{
			Explosion.tmpCells.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				Explosion.tmpCells.Add(cells[i]);
			}
			for (int j = 0; j < cells.Count; j++)
			{
				if (cells[j].Walkable(base.Map))
				{
					for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
					{
						IntVec3 intVec = cells[j] + GenAdj.AdjacentCells[k];
						if (intVec.InBounds(base.Map) && Explosion.tmpCells.Add(intVec))
						{
							this.addedCellsAffectedOnlyByDamage.Add(intVec);
						}
					}
				}
			}
			cells.Clear();
			foreach (IntVec3 item in Explosion.tmpCells)
			{
				cells.Add(item);
			}
			Explosion.tmpCells.Clear();
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x001273F8 File Offset: 0x001255F8
		private bool ShouldCellBeAffectedOnlyByDamage(IntVec3 c)
		{
			return this.applyDamageToExplosionCellsNeighbors && this.addedCellsAffectedOnlyByDamage.Contains(c);
		}

		// Token: 0x04002923 RID: 10531
		public float radius;

		// Token: 0x04002924 RID: 10532
		public DamageDef damType;

		// Token: 0x04002925 RID: 10533
		public int damAmount;

		// Token: 0x04002926 RID: 10534
		public float armorPenetration;

		// Token: 0x04002927 RID: 10535
		public Thing instigator;

		// Token: 0x04002928 RID: 10536
		public ThingDef weapon;

		// Token: 0x04002929 RID: 10537
		public ThingDef projectile;

		// Token: 0x0400292A RID: 10538
		public Thing intendedTarget;

		// Token: 0x0400292B RID: 10539
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x0400292C RID: 10540
		public FloatRange? affectedAngle;

		// Token: 0x0400292D RID: 10541
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x0400292E RID: 10542
		public float preExplosionSpawnChance;

		// Token: 0x0400292F RID: 10543
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04002930 RID: 10544
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04002931 RID: 10545
		public ThingDef postExplosionSpawnThingDefWater;

		// Token: 0x04002932 RID: 10546
		public float postExplosionSpawnChance;

		// Token: 0x04002933 RID: 10547
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04002934 RID: 10548
		public GasType? postExplosionGasType;

		// Token: 0x04002935 RID: 10549
		public int postExplosionGasAmount = 255;

		// Token: 0x04002936 RID: 10550
		public float? postExplosionGasRadiusOverride;

		// Token: 0x04002937 RID: 10551
		public float chanceToStartFire;

		// Token: 0x04002938 RID: 10552
		public SimpleCurve flammabilityChanceCurve;

		// Token: 0x04002939 RID: 10553
		public bool damageFalloff;

		// Token: 0x0400293A RID: 10554
		public IntVec3? needLOSToCell1;

		// Token: 0x0400293B RID: 10555
		public IntVec3? needLOSToCell2;

		// Token: 0x0400293C RID: 10556
		public bool doVisualEffects = true;

		// Token: 0x0400293D RID: 10557
		public float propagationSpeed = 1f;

		// Token: 0x0400293E RID: 10558
		public float excludeRadius;

		// Token: 0x0400293F RID: 10559
		public bool doSoundEffects = true;

		// Token: 0x04002940 RID: 10560
		public float screenShakeFactor = 1f;

		// Token: 0x04002941 RID: 10561
		public List<IntVec3> overrideCells;

		// Token: 0x04002942 RID: 10562
		public ThingDef preExplosionSpawnSingleThingDef;

		// Token: 0x04002943 RID: 10563
		public ThingDef postExplosionSpawnSingleThingDef;

		// Token: 0x04002944 RID: 10564
		private int startTick;

		// Token: 0x04002945 RID: 10565
		private List<IntVec3> cellsToAffect;

		// Token: 0x04002946 RID: 10566
		private List<Thing> damagedThings;

		// Token: 0x04002947 RID: 10567
		private List<Thing> ignoredThings;

		// Token: 0x04002948 RID: 10568
		private HashSet<IntVec3> addedCellsAffectedOnlyByDamage;

		// Token: 0x04002949 RID: 10569
		private const float DamageFactorAtEdge = 0.2f;

		// Token: 0x0400294A RID: 10570
		private static HashSet<IntVec3> tmpCells = new HashSet<IntVec3>();
	}
}
