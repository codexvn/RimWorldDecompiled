using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002ADD RID: 10973
	public class Fire : AttachableThing, ISizeReporter
	{
		// Token: 0x17002636 RID: 9782
		// (get) Token: 0x0600F482 RID: 62594 RVA: 0x00470D0E File Offset: 0x0046EF0E
		public int TicksSinceSpawn
		{
			get
			{
				return this.ticksSinceSpawn;
			}
		}

		// Token: 0x17002637 RID: 9783
		// (get) Token: 0x0600F483 RID: 62595 RVA: 0x00470D16 File Offset: 0x0046EF16
		public override string Label
		{
			get
			{
				if (this.parent != null)
				{
					return "FireOn".Translate(this.parent.LabelCap, this.parent);
				}
				return this.def.label;
			}
		}

		// Token: 0x17002638 RID: 9784
		// (get) Token: 0x0600F484 RID: 62596 RVA: 0x00470D58 File Offset: 0x0046EF58
		public override string InspectStringAddon
		{
			get
			{
				return "Burning".Translate() + " (" + "FireSizeLower".Translate((this.fireSize * 100f).ToString("F0")) + ")";
			}
		}

		// Token: 0x17002639 RID: 9785
		// (get) Token: 0x0600F485 RID: 62597 RVA: 0x00470DB8 File Offset: 0x0046EFB8
		private float SpreadInterval
		{
			get
			{
				float num = 150f - (this.fireSize - 1f) * 40f;
				if (num < 75f)
				{
					num = 75f;
				}
				return num;
			}
		}

		// Token: 0x1700263A RID: 9786
		// (get) Token: 0x0600F486 RID: 62598 RVA: 0x001304E5 File Offset: 0x0012E6E5
		public override int UpdateRateTicks
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x0600F487 RID: 62599 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ShouldBeLitNow()
		{
			return true;
		}

		// Token: 0x0600F488 RID: 62600 RVA: 0x00470DF0 File Offset: 0x0046EFF0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceSpawn, "ticksSinceSpawn", 0, false);
			Scribe_Values.Look<float>(ref this.fireSize, "fireSize", 0f, false);
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
		}

		// Token: 0x0600F489 RID: 62601 RVA: 0x00470E3C File Offset: 0x0046F03C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.RecalcPathsOnAndAroundMe(map);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.HomeArea, this, OpportunityType.Important);
			this.ticksSinceSpread = (int)(this.SpreadInterval * Rand.Value);
		}

		// Token: 0x0600F48A RID: 62602 RVA: 0x00470E6C File Offset: 0x0046F06C
		public float CurrentSize()
		{
			return this.fireSize;
		}

		// Token: 0x0600F48B RID: 62603 RVA: 0x00470E74 File Offset: 0x0046F074
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.sustainer != null)
			{
				if (this.sustainer.externalParams.sizeAggregator == null)
				{
					this.sustainer.externalParams.sizeAggregator = new SoundSizeAggregator();
				}
				this.sustainer.externalParams.sizeAggregator.RemoveReporter(this);
			}
			Map map = base.Map;
			base.DeSpawn(mode);
			this.RecalcPathsOnAndAroundMe(map);
		}

		// Token: 0x0600F48C RID: 62604 RVA: 0x00470EDC File Offset: 0x0046F0DC
		private void RecalcPathsOnAndAroundMe(Map map)
		{
			IntVec3[] adjacentCellsAndInside = GenAdj.AdjacentCellsAndInside;
			for (int i = 0; i < adjacentCellsAndInside.Length; i++)
			{
				IntVec3 c = base.Position + adjacentCellsAndInside[i];
				if (c.InBounds(map))
				{
					map.pathing.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x0600F48D RID: 62605 RVA: 0x00470F28 File Offset: 0x0046F128
		public override void AttachTo(Thing newParent)
		{
			base.AttachTo(newParent);
			Pawn pawn = newParent as Pawn;
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.WasOnFire, new object[]
				{
					pawn
				});
			}
		}

		// Token: 0x0600F48E RID: 62606 RVA: 0x00470F5B File Offset: 0x0046F15B
		protected override void Tick()
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
		}

		// Token: 0x0600F48F RID: 62607 RVA: 0x00470F80 File Offset: 0x0046F180
		protected override void TickInterval(int delta)
		{
			this.ticksSinceSpawn += delta;
			if (Fire.lastFireCountUpdateTick != Find.TickManager.TicksGame)
			{
				Fire.fireCount = base.Map.listerThings.ThingsOfDef(this.def).Count;
				Fire.lastFireCountUpdateTick = Find.TickManager.TicksGame;
			}
			if ((this.sustainer == null || this.sustainer.Ended) && !base.Position.Fogged(base.Map))
			{
				SoundInfo info = SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.PerTick);
				this.sustainer = SustainerAggregatorUtility.AggregateOrSpawnSustainerFor(this, SoundDefOf.FireBurning, info);
			}
			using (ProfilerBlock.Scope("Spawn particles"))
			{
				this.ticksUntilSmoke -= delta;
				if (this.ticksUntilSmoke <= 0)
				{
					this.SpawnSmokeParticles();
				}
				if (Fire.fireCount < 15 && this.fireSize > 0.7f && Rand.Value < this.fireSize * 0.01f)
				{
					FleckMaker.ThrowMicroSparks(this.DrawPos, base.Map);
				}
			}
			using (ProfilerBlock.Scope("Spread"))
			{
				if (this.fireSize > 1f)
				{
					this.ticksSinceSpread += delta;
					if ((float)this.ticksSinceSpread >= this.SpreadInterval)
					{
						this.TrySpread();
						this.ticksSinceSpread = 0;
					}
				}
			}
			if (this.IsHashIntervalTick(150, delta))
			{
				this.DoComplexCalcs();
			}
			if (this.ticksSinceSpawn >= 7500)
			{
				this.TryBurnFloor();
			}
		}

		// Token: 0x0600F490 RID: 62608 RVA: 0x00471138 File Offset: 0x0046F338
		private void SpawnSmokeParticles()
		{
			if (Fire.fireCount < 15)
			{
				FleckMaker.ThrowSmoke(this.DrawPos, base.Map, this.fireSize);
			}
			if (this.fireSize > 0.5f && this.parent == null)
			{
				FleckMaker.ThrowFireGlow(base.Position.ToVector3Shifted(), base.Map, this.fireSize);
			}
			float num = this.fireSize / 2f;
			if (num > 1f)
			{
				num = 1f;
			}
			num = 1f - num;
			this.ticksUntilSmoke = Fire.SmokeIntervalRange.Lerped(num) + (int)(10f * Rand.Value);
		}

		// Token: 0x0600F491 RID: 62609 RVA: 0x004711DC File Offset: 0x0046F3DC
		private void DoComplexCalcs()
		{
			bool flag = false;
			Fire.flammableList.Clear();
			this.flammabilityMax = 0f;
			if (!base.Position.GetTerrain(base.Map).extinguishesFire)
			{
				if (this.parent == null)
				{
					if (base.Position.TerrainFlammableNow(base.Map))
					{
						this.flammabilityMax = base.Position.TerrainFlammability(base.Map);
					}
					List<Thing> list = base.Map.thingGrid.ThingsListAt(base.Position);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing = list[i];
						if (thing is Building_Door)
						{
							flag = true;
						}
						float statValue = thing.GetStatValue(StatDefOf.Flammability, true, -1);
						if (statValue >= 0.01f)
						{
							Fire.flammableList.Add(list[i]);
							if (statValue > this.flammabilityMax)
							{
								this.flammabilityMax = statValue;
							}
							if (this.parent == null && this.fireSize > 0.4f && list[i].def.category == ThingCategory.Pawn && Rand.Chance(FireUtility.ChanceToAttachFireCumulative(list[i], 150f)))
							{
								list[i].TryAttachFire(this.fireSize * 0.2f, this.instigator);
							}
						}
					}
				}
				else
				{
					Fire.flammableList.Add(this.parent);
					this.flammabilityMax = this.parent.GetStatValue(StatDefOf.Flammability, true, -1);
				}
			}
			if (this.flammabilityMax < 0.01f)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Thing thing2;
			if (this.parent != null)
			{
				thing2 = this.parent;
			}
			else if (Fire.flammableList.Count > 0)
			{
				thing2 = Fire.flammableList.RandomElement<Thing>();
			}
			else
			{
				thing2 = null;
			}
			if (thing2 != null && (this.fireSize >= 0.4f || thing2 == this.parent || thing2.def.category != ThingCategory.Pawn))
			{
				this.DoFireDamage(thing2);
			}
			if (base.Spawned)
			{
				float num = this.fireSize * 160f;
				if (flag)
				{
					num *= 0.15f;
				}
				GenTemperature.PushHeat(base.Position, base.Map, num);
				if (Rand.Value < 0.4f)
				{
					float radius = this.fireSize * 3f;
					WeatherBuildupUtility.AddSnowRadial(base.Position, base.Map, radius, -(this.fireSize * 0.1f));
				}
				Building edifice = base.Position.GetEdifice(base.Map);
				float num2 = 0f;
				if (edifice != null && base.Map.Biome.inVacuum && edifice.def.passability == Traversability.Impassable && edifice.def.Fillage == FillCategory.Full)
				{
					if (base.Position.GetRoof(base.Map) == null)
					{
						num2 = 1f;
					}
					else
					{
						for (int j = 0; j < 4; j++)
						{
							if ((base.Position + GenAdj.CardinalDirections[j]).GetRoof(base.Map) == null)
							{
								num2 = 1f;
								break;
							}
							num2 = Mathf.Max(num2, base.Position.GetVacuum(base.Map));
						}
					}
				}
				else
				{
					num2 = base.Position.GetVacuum(base.Map);
				}
				this.fireSize += 0.00055f * this.flammabilityMax * 150f * (1f - num2);
				if (this.fireSize > 1.75f)
				{
					this.fireSize = 1.75f;
				}
				if (base.Map.weatherManager.RainRate > 0.01f && this.VulnerableToRain() && Rand.Value < 6f)
				{
					base.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}
				if (num2 > 0f)
				{
					base.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 20f * num2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}
			}
		}

		// Token: 0x0600F492 RID: 62610 RVA: 0x004715E6 File Offset: 0x0046F7E6
		private void TryBurnFloor()
		{
			if (this.parent != null || !base.Spawned)
			{
				return;
			}
			if (base.Position.TerrainFlammableNow(base.Map))
			{
				base.Map.terrainGrid.Notify_TerrainBurned(base.Position);
			}
		}

		// Token: 0x0600F493 RID: 62611 RVA: 0x00471624 File Offset: 0x0046F824
		private bool VulnerableToRain()
		{
			if (!base.Spawned)
			{
				return false;
			}
			RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
			if (roofDef == null)
			{
				return true;
			}
			if (roofDef.isThickRoof)
			{
				return false;
			}
			Thing edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.holdsRoof;
		}

		// Token: 0x0600F494 RID: 62612 RVA: 0x00471684 File Offset: 0x0046F884
		private void DoFireDamage(Thing targ)
		{
			int num = GenMath.RoundRandom(Mathf.Clamp(0.0125f + 0.0036f * this.fireSize, 0.0125f, 0.05f) * 150f);
			if (num < 1)
			{
				num = 1;
			}
			Pawn pawn = targ as Pawn;
			if (pawn != null)
			{
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_Fire, null);
				Find.BattleLog.Add(battleLogEntry_DamageTaken);
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator ?? this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
				targ.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_DamageTaken);
				Apparel apparel;
				if (pawn.apparel != null && pawn.apparel.WornApparel.TryRandomElement(out apparel))
				{
					apparel.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator ?? this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					return;
				}
			}
			else
			{
				targ.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator ?? this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x0600F495 RID: 62613 RVA: 0x004717AC File Offset: 0x0046F9AC
		protected void TrySpread()
		{
			IntVec3 intVec;
			bool flag;
			if (Rand.Chance(0.8f))
			{
				intVec = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(1, 8)];
				flag = true;
			}
			else
			{
				intVec = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(10, 20)];
				flag = false;
			}
			if (!intVec.InBounds(base.Map))
			{
				return;
			}
			if (Rand.Chance(FireUtility.ChanceToStartFireIn(intVec, base.Map, null)))
			{
				if (!flag)
				{
					CellRect startRect = CellRect.SingleCell(base.Position);
					CellRect endRect = CellRect.SingleCell(intVec);
					if (!GenSight.LineOfSight(base.Position, intVec, base.Map, startRect, endRect, null, false))
					{
						return;
					}
					((Spark)GenSpawn.Spawn(ThingDefOf.Spark, base.Position, base.Map, WipeMode.Vanish)).Launch(this, intVec, intVec, ProjectileHitFlags.All, false, null);
					return;
				}
				else
				{
					FireUtility.TryStartFireIn(intVec, base.Map, 0.1f, this.instigator, null);
				}
			}
		}

		// Token: 0x0400A11F RID: 41247
		private int ticksSinceSpawn;

		// Token: 0x0400A120 RID: 41248
		public float fireSize = 0.1f;

		// Token: 0x0400A121 RID: 41249
		private int ticksSinceSpread;

		// Token: 0x0400A122 RID: 41250
		private float flammabilityMax = 0.5f;

		// Token: 0x0400A123 RID: 41251
		public Thing instigator;

		// Token: 0x0400A124 RID: 41252
		private int ticksUntilSmoke;

		// Token: 0x0400A125 RID: 41253
		private Sustainer sustainer;

		// Token: 0x0400A126 RID: 41254
		private static List<Thing> flammableList = new List<Thing>();

		// Token: 0x0400A127 RID: 41255
		private static int fireCount;

		// Token: 0x0400A128 RID: 41256
		private static int lastFireCountUpdateTick;

		// Token: 0x0400A129 RID: 41257
		public const float MinFireSize = 0.1f;

		// Token: 0x0400A12A RID: 41258
		private const float MinSizeForSpark = 1f;

		// Token: 0x0400A12B RID: 41259
		private const float TicksBetweenSparksBase = 150f;

		// Token: 0x0400A12C RID: 41260
		private const float TicksBetweenSparksReductionPerFireSize = 40f;

		// Token: 0x0400A12D RID: 41261
		private const float MinTicksBetweenSparks = 75f;

		// Token: 0x0400A12E RID: 41262
		public const float MaxFireSize = 1.75f;

		// Token: 0x0400A12F RID: 41263
		private const int TicksToBurnFloor = 7500;

		// Token: 0x0400A130 RID: 41264
		private const int ComplexCalcsInterval = 150;

		// Token: 0x0400A131 RID: 41265
		private const float MinSizeForIgniteMovables = 0.4f;

		// Token: 0x0400A132 RID: 41266
		private const float FireBaseGrowthPerTick = 0.00055f;

		// Token: 0x0400A133 RID: 41267
		private static IntRange SmokeIntervalRange = new IntRange(130, 200);

		// Token: 0x0400A134 RID: 41268
		private const int SmokeIntervalRandomAddon = 10;

		// Token: 0x0400A135 RID: 41269
		private const float BaseSkyExtinguishChance = 0.04f;

		// Token: 0x0400A136 RID: 41270
		private const int BaseSkyExtinguishDamage = 10;

		// Token: 0x0400A137 RID: 41271
		private const int VacuumExtinguishDamage = 20;

		// Token: 0x0400A138 RID: 41272
		private const float HeatPerFireSizePerInterval = 160f;

		// Token: 0x0400A139 RID: 41273
		private const float HeatFactorWhenDoorPresent = 0.15f;

		// Token: 0x0400A13A RID: 41274
		private const float SnowClearRadiusPerFireSize = 3f;

		// Token: 0x0400A13B RID: 41275
		private const float SnowClearDepthFactor = 0.1f;

		// Token: 0x0400A13C RID: 41276
		private const int FireCountParticlesOff = 15;
	}
}
