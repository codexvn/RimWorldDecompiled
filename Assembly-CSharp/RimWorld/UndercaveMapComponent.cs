using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001F7B RID: 8059
	public class UndercaveMapComponent : CustomMapComponent
	{
		// Token: 0x17001B15 RID: 6933
		// (get) Token: 0x0600B212 RID: 45586 RVA: 0x0032E37C File Offset: 0x0032C57C
		private static EffecterDef[] AmbientEffecters { get; } = new EffecterDef[]
		{
			EffecterDefOf.UndercaveMapAmbience,
			EffecterDefOf.UndercaveCeilingDebris
		};

		// Token: 0x17001B16 RID: 6934
		// (get) Token: 0x0600B213 RID: 45587 RVA: 0x0032E383 File Offset: 0x0032C583
		public Map SourceMap
		{
			get
			{
				PocketMapParent pocketMapParent = this.map.Parent as PocketMapParent;
				if (pocketMapParent == null)
				{
					return null;
				}
				return pocketMapParent.sourceMap;
			}
		}

		// Token: 0x0600B214 RID: 45588 RVA: 0x0032E3A0 File Offset: 0x0032C5A0
		public UndercaveMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x0600B215 RID: 45589 RVA: 0x0032E3B4 File Offset: 0x0032C5B4
		public override void MapGenerated()
		{
			Map sourceMap = this.SourceMap;
			object obj;
			if (sourceMap == null)
			{
				obj = null;
			}
			else
			{
				ListerThings listerThings = sourceMap.listerThings;
				obj = ((listerThings != null) ? listerThings.ThingsOfDef(ThingDefOf.PitGate).FirstOrDefault<Thing>() : null);
			}
			this.pitGate = (obj as PitGate);
			this.exit = (this.map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).FirstOrDefault<Thing>() as PocketMapExit);
			if (this.pitGate == null)
			{
				Log.Warning("Pit gate was not found after generating undercave, if this map was created via dev tools you can ignore this");
				return;
			}
			if (this.exit == null)
			{
				Log.Error("Pit gate exit was not found after generating undercave");
				return;
			}
			PitGate pitGate = this.pitGate;
			float num = (pitGate != null) ? pitGate.pointsMultiplier : 1f;
			List<Pawn> fleshbeastsForPoints = FleshbeastUtility.GetFleshbeastsForPoints(UndercaveMapComponent.ThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.SourceMap) * num), this.map, false);
			int num2 = 0;
			int randomInRange = UndercaveMapComponent.SpawnGroupSize.RandomInRange;
			IntVec3 root;
			CellFinder.TryFindRandomCell(this.map, (IntVec3 c) => c.Standable(this.map) && !c.InHorDistOf(this.exit.Position, 10f), out root);
			foreach (Pawn newThing in fleshbeastsForPoints)
			{
				IntVec3 loc;
				CellFinder.TryFindRandomCellNear(root, this.map, 3, (IntVec3 c) => c.Standable(this.map), out loc, -1);
				if (loc.IsValid)
				{
					GenSpawn.Spawn(newThing, loc, this.map, WipeMode.Vanish);
				}
				num2++;
				if (num2 >= randomInRange)
				{
					num2 = 0;
					randomInRange = UndercaveMapComponent.SpawnGroupSize.RandomInRange;
					CellFinder.TryFindRandomCell(this.map, (IntVec3 c) => c.Standable(this.map) && !c.InHorDistOf(this.exit.Position, 10f), out root);
				}
			}
		}

		// Token: 0x0600B216 RID: 45590 RVA: 0x0032E54C File Offset: 0x0032C74C
		public override void MapComponentTick()
		{
			if (Find.CurrentMap != this.map)
			{
				Sustainer sustainer = this.collapsingSustainer;
				if (sustainer != null)
				{
					sustainer.End();
				}
			}
			if (this.pitGate != null && Find.CurrentMap == this.map)
			{
				if (this.pitGate.IsCollapsing)
				{
					float mtb = UndercaveMapComponent.HoursToShakeMTBTicksCurve.Evaluate((float)this.pitGate.TicksUntilCollapse / 2500f);
					if (this.pitGate.CollapseStage == 1)
					{
						if (this.collapsingSustainer == null || this.collapsingSustainer.Ended)
						{
							this.collapsingSustainer = SoundDefOf.UndercaveCollapsingStage1.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerTick));
						}
						if (Find.CurrentMap == this.map && Rand.MTBEventOccurs(mtb, 1f, 1f))
						{
							this.TriggerCollapseFX(0.1f, UndercaveMapComponent.StageOneNumCollapseEffects.RandomInRange);
						}
					}
					else
					{
						if (this.collapsingSustainer == null || this.collapsingSustainer.Ended)
						{
							this.collapsingSustainer = SoundDefOf.UndercaveCollapsingStage2.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerTick));
						}
						if (Find.CurrentMap == this.map && Rand.MTBEventOccurs(mtb, 1f, 1f))
						{
							this.TriggerCollapseFX(0.2f, UndercaveMapComponent.StageTwoNumCollapseEffects.RandomInRange);
						}
					}
					this.collapsingSustainer.Maintain();
				}
				this.TriggerAmbientFX();
				this.ReadFxQueue();
			}
		}

		// Token: 0x0600B217 RID: 45591 RVA: 0x0032E6B0 File Offset: 0x0032C8B0
		private void TriggerAmbientFX()
		{
			if (Rand.MTBEventOccurs(60f, 1f, 1f))
			{
				IntVec3 cell;
				if (Find.CameraDriver.ZoomRootSize < 20f && CellFinderLoose.TryGetRandomCellWith((IntVec3 c) => c.Standable(this.map) && Find.CameraDriver.CurrentViewRect.Contains(c), this.map, 100, out cell))
				{
					this.fxQueue.Enqueue(new QueuedCellEffecter(EffecterDefOf.UndercaveMapAmbienceWater, cell, 0));
				}
				IntVec3 intVec;
				if (CellFinderLoose.TryGetRandomCellWith((IntVec3 c) => c.Standable(this.map), this.map, 100, out intVec))
				{
					if (intVec.Fogged(this.map))
					{
						return;
					}
					if (!Find.CameraDriver.CurrentViewRect.Contains(intVec))
					{
						return;
					}
					EffecterDef edef = UndercaveMapComponent.AmbientEffecters.RandomElementByWeight((EffecterDef p) => p.randomWeight);
					this.fxQueue.Enqueue(new QueuedCellEffecter(edef, intVec, 0));
				}
			}
		}

		// Token: 0x0600B218 RID: 45592 RVA: 0x0032E79C File Offset: 0x0032C99C
		private void TriggerCollapseFX(float shakeAmt, int numDustEffecters)
		{
			Find.CameraDriver.shaker.DoShake(shakeAmt);
			SoundDefOf.UndercaveRumble.PlayOneShotOnCamera(this.map);
			int randomInRange = UndercaveMapComponent.NumRockCollapsesRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 intVec;
				if (CellFinderLoose.TryGetRandomCellWith(delegate(IntVec3 c)
				{
					if (c.GetEdifice(this.map) != null)
					{
						return false;
					}
					using (List<Pawn>.Enumerator enumerator2 = this.map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.Position.InHorDistOf(c, 10f))
							{
								return false;
							}
						}
					}
					return true;
				}, this.map, 100, out intVec))
				{
					foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(intVec, this.map, UndercaveMapComponent.CollapsedRocksSizeRange.RandomInRange, (IntVec3 c) => c.GetEdifice(this.map) == null))
					{
						RoofCollapserImmediate.DropRoofInCells(c2, this.map, null);
					}
					EffecterDefOf.UndercaveCeilingDebris.SpawnMaintained(intVec, this.map, 1f);
				}
			}
			int num = Find.TickManager.TicksGame;
			for (int j = 0; j < numDustEffecters; j++)
			{
				IntVec3 cell;
				if (CellFinderLoose.TryGetRandomCellWith((IntVec3 c) => c.GetEdifice(this.map) == null, this.map, 100, out cell))
				{
					this.fxQueue.Enqueue(new QueuedCellEffecter(EffecterDefOf.UndercaveCeilingDebris, cell, num));
					num += UndercaveMapComponent.FXTriggerDelay.RandomInRange;
				}
			}
		}

		// Token: 0x0600B219 RID: 45593 RVA: 0x0032E8EC File Offset: 0x0032CAEC
		public void ReadFxQueue()
		{
			while (this.fxQueue.Count > 0 && Find.TickManager.TicksGame >= this.fxQueue.Peek().tick)
			{
				QueuedCellEffecter queuedCellEffecter = this.fxQueue.Dequeue();
				queuedCellEffecter.effecterDef.SpawnMaintained(queuedCellEffecter.cell, this.map, 1f);
			}
		}

		// Token: 0x0600B21A RID: 45594 RVA: 0x0032E950 File Offset: 0x0032CB50
		public void Notify_BeginCollapsing(int collapseDurationTicks)
		{
			FleshmassMapComponent component = this.map.GetComponent<FleshmassMapComponent>();
			if (component != null)
			{
				component.DestroyFleshmass(collapseDurationTicks, 5f, true, null);
			}
			SoundDefOf.UndercaveRumble.PlayOneShotOnCamera(this.map);
			Find.CameraDriver.shaker.DoShake(0.2f, 120);
		}

		// Token: 0x0600B21B RID: 45595 RVA: 0x0032E9A1 File Offset: 0x0032CBA1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PitGate>(ref this.pitGate, "pitGate", false);
			Scribe_References.Look<PocketMapExit>(ref this.exit, "exit", false);
		}

		// Token: 0x040079B9 RID: 31161
		private const int MinSpawnDistFromGateExit = 10;

		// Token: 0x040079BA RID: 31162
		private const float FleshmassCollapseRateExponent = 5f;

		// Token: 0x040079BB RID: 31163
		private static readonly IntRange SpawnGroupSize = new IntRange(2, 4);

		// Token: 0x040079BC RID: 31164
		private const int SpawnGroupRadius = 3;

		// Token: 0x040079BD RID: 31165
		private static readonly SimpleCurve ThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 250f),
				true
			},
			{
				new CurvePoint(500f, 800f),
				true
			},
			{
				new CurvePoint(1000f, 1500f),
				true
			},
			{
				new CurvePoint(2000f, 2000f),
				true
			},
			{
				new CurvePoint(8000f, 3000f),
				true
			}
		};

		// Token: 0x040079BE RID: 31166
		private static readonly IntRange NumRockCollapsesRange = new IntRange(0, 2);

		// Token: 0x040079BF RID: 31167
		private static readonly IntRange CollapsedRocksSizeRange = new IntRange(1, 5);

		// Token: 0x040079C0 RID: 31168
		private const float CollapsedRocksMinDistanceFromColonist = 10f;

		// Token: 0x040079C1 RID: 31169
		private const int InitialShakeDurationTicks = 120;

		// Token: 0x040079C2 RID: 31170
		private const float InitialShakeAmount = 0.2f;

		// Token: 0x040079C3 RID: 31171
		private const float StageOneShakeAmount = 0.1f;

		// Token: 0x040079C4 RID: 31172
		private const float StageTwoShakeAmount = 0.2f;

		// Token: 0x040079C5 RID: 31173
		private static readonly IntRange StageOneNumCollapseEffects = new IntRange(10, 15);

		// Token: 0x040079C6 RID: 31174
		private static readonly IntRange StageTwoNumCollapseEffects = new IntRange(15, 20);

		// Token: 0x040079C7 RID: 31175
		private static readonly IntRange FXTriggerDelay = new IntRange(10, 30);

		// Token: 0x040079C8 RID: 31176
		private static readonly SimpleCurve HoursToShakeMTBTicksCurve = new SimpleCurve
		{
			{
				new CurvePoint(14f, 2500f),
				true
			},
			{
				new CurvePoint(1f, 45f),
				true
			}
		};

		// Token: 0x040079C9 RID: 31177
		private const float AmbientFXMTB = 60f;

		// Token: 0x040079CB RID: 31179
		public PitGate pitGate;

		// Token: 0x040079CC RID: 31180
		public PocketMapExit exit;

		// Token: 0x040079CD RID: 31181
		private Queue<QueuedCellEffecter> fxQueue = new Queue<QueuedCellEffecter>();

		// Token: 0x040079CE RID: 31182
		private Sustainer collapsingSustainer;
	}
}
