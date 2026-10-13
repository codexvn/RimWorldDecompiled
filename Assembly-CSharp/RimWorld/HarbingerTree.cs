using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B21 RID: 11041
	public class HarbingerTree : Plant
	{
		// Token: 0x170026A7 RID: 9895
		// (get) Token: 0x0600F6C1 RID: 63169 RVA: 0x0047A868 File Offset: 0x00478A68
		private bool ConsumableNearby
		{
			get
			{
				return this.rootTargets.Count > 0;
			}
		}

		// Token: 0x170026A8 RID: 9896
		// (get) Token: 0x0600F6C2 RID: 63170 RVA: 0x0047A878 File Offset: 0x00478A78
		private int BloomLevel
		{
			get
			{
				return Mathf.Max(Mathf.FloorToInt(this.Growth / 0.25f - 0.001f), 0);
			}
		}

		// Token: 0x170026A9 RID: 9897
		// (get) Token: 0x0600F6C3 RID: 63171 RVA: 0x0047A897 File Offset: 0x00478A97
		public float ConsumeRadius
		{
			get
			{
				return HarbingerTree.ConsumeRadiusByGrowthCurve.Evaluate(this.Growth);
			}
		}

		// Token: 0x170026AA RID: 9898
		// (get) Token: 0x0600F6C4 RID: 63172 RVA: 0x0047A8A9 File Offset: 0x00478AA9
		public override float GrowthRate
		{
			get
			{
				return base.GrowthRate * this.GrowthRateFactor_Consuming;
			}
		}

		// Token: 0x170026AB RID: 9899
		// (get) Token: 0x0600F6C5 RID: 63173 RVA: 0x0047A8B8 File Offset: 0x00478AB8
		private float GrowthRateFactor_Consuming
		{
			get
			{
				if (!this.ConsumableNearby)
				{
					return 1f;
				}
				return 1.5f;
			}
		}

		// Token: 0x170026AC RID: 9900
		// (get) Token: 0x0600F6C6 RID: 63174 RVA: 0x0047A8CD File Offset: 0x00478ACD
		private IEnumerable<IntVec3> RadialCells
		{
			get
			{
				return GenRadial.RadialCellsAround(base.Position, this.ConsumeRadius, true);
			}
		}

		// Token: 0x170026AD RID: 9901
		// (get) Token: 0x0600F6C7 RID: 63175 RVA: 0x0047A8E4 File Offset: 0x00478AE4
		public override string GrowthRateCalcDesc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(base.GrowthRateCalcDesc);
				if (this.GrowthRateFactor_Consuming != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("FleshConsumption".Translate()) + ": " + this.GrowthRateFactor_Consuming.ToStringPercent());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600F6C8 RID: 63176 RVA: 0x0047A950 File Offset: 0x00478B50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ThingWithComps>(ref this.rootTargets, "rootTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.nutrition, "nutrition", 0f, false);
			Scribe_Values.Look<float>(ref this.nutritionForNextTree, "nutritionForNextTree", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.rootTargets.RemoveAll((ThingWithComps x) => x == null || x.Destroyed);
			}
		}

		// Token: 0x0600F6C9 RID: 63177 RVA: 0x0047A9D8 File Offset: 0x00478BD8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Harbinger tree"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			LongEventHandler.ExecuteWhenFinished(new Action(this.UpdateRoots));
			this.UpdateBloomingStage();
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.nutritionForNextTree = HarbingerTree.NutritionForNewTreeRange.RandomInRange;
			}
		}

		// Token: 0x0600F6CA RID: 63178 RVA: 0x0047AA36 File Offset: 0x00478C36
		public override void TickLong()
		{
			base.TickLong();
			this.UpdateRoots();
			this.UpdateBloomingStage();
		}

		// Token: 0x0600F6CB RID: 63179 RVA: 0x0047AA4C File Offset: 0x00478C4C
		private void UpdateRoots()
		{
			if (this.roots == null)
			{
				this.roots = new Dictionary<ThingWithComps, Mote>();
			}
			if (this.rootTargets == null)
			{
				this.rootTargets = new List<ThingWithComps>();
			}
			this.tmpRadialCells.Clear();
			this.tmpRadialCells.AddRange(this.RadialCells.ToList<IntVec3>());
			foreach (ThingWithComps thing in this.rootTargets)
			{
				this.TryMakeRoot(thing);
			}
			foreach (IntVec3 c in this.tmpRadialCells)
			{
				if (c.InBounds(base.Map))
				{
					this.tmpThings.Clear();
					this.tmpThings.AddRange(c.GetThingList(base.Map));
					foreach (Thing thing2 in this.tmpThings)
					{
						ThingWithComps thingWithComps = thing2 as ThingWithComps;
						CompHarbingerTreeConsumable compHarbingerTreeConsumable;
						if (thingWithComps != null && thingWithComps.TryGetComp(out compHarbingerTreeConsumable) && compHarbingerTreeConsumable.CanBeConsumed)
						{
							this.TryMakeRoot(thingWithComps);
						}
					}
				}
			}
			using (List<ThingWithComps>.Enumerator enumerator = this.rootTargets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingWithComps thingWithComps2 = enumerator.Current;
					if (thingWithComps2.Destroyed || !this.tmpRadialCells.Contains(thingWithComps2.PositionHeld))
					{
						this.deferredDestroy.Enqueue(thingWithComps2);
					}
				}
				goto IL_1D8;
			}
			IL_192:
			ThingWithComps thingWithComps3 = this.deferredDestroy.Dequeue();
			Mote mote;
			if (this.roots.TryGetValue(thingWithComps3, out mote) && mote != null)
			{
				mote.Destroy(DestroyMode.Vanish);
			}
			this.roots.Remove(thingWithComps3);
			this.rootTargets.Remove(thingWithComps3);
			IL_1D8:
			if (this.deferredDestroy.Count <= 0)
			{
				return;
			}
			goto IL_192;
		}

		// Token: 0x0600F6CC RID: 63180 RVA: 0x0047AC74 File Offset: 0x00478E74
		private void TryMakeRoot(ThingWithComps thing)
		{
			if (!this.roots.ContainsKey(thing))
			{
				float exactRot = 0f;
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					exactRot = corpse.InnerPawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None);
				}
				this.roots[thing] = MoteMaker.MakeStaticMote(thing.Position.ToVector3Shifted(), base.Map, ThingDefOf.Mote_HarbingerTreeRoots, 1f, false, exactRot);
			}
			if (!this.rootTargets.Contains(thing))
			{
				this.rootTargets.Add(thing);
			}
		}

		// Token: 0x0600F6CD RID: 63181 RVA: 0x0047AD04 File Offset: 0x00478F04
		public void AddNutrition(float amt)
		{
			if (amt <= 0f)
			{
				return;
			}
			this.nutrition += amt;
			if (this.nutrition >= this.nutritionForNextTree)
			{
				this.nutrition = 0f;
				this.nutritionForNextTree = HarbingerTree.NutritionForNewTreeRange.RandomInRange;
				this.SpawnNewTree();
			}
		}

		// Token: 0x0600F6CE RID: 63182 RVA: 0x0047AD5C File Offset: 0x00478F5C
		private void SpawnNewTree()
		{
			IntVec3 intVec;
			if (IncidentWorker_HarbingerTreeSpawn.TryGetHarbingerTreeSpawnCell(base.Map, out intVec, new IntVec3?(base.Position)))
			{
				Plant plant = intVec.GetPlant(base.Map);
				if (plant != null)
				{
					plant.Destroy(DestroyMode.Vanish);
				}
				Plant plant2 = (Plant)ThingMaker.MakeThing(ThingDefOf.Plant_TreeHarbinger, null);
				plant2.Growth = IncidentDefOf.HarbingerTreeSpawn.treeGrowth;
				GenSpawn.Spawn(plant2, intVec, base.Map, WipeMode.Vanish);
				Messages.Message("HarbingerTreeSprouted".Translate(), plant2, MessageTypeDefOf.NeutralEvent, true);
				Find.Anomaly.Notify_HarbingerTreeSpawned();
			}
		}

		// Token: 0x0600F6CF RID: 63183 RVA: 0x0047ADF4 File Offset: 0x00478FF4
		private void UpdateBloomingStage()
		{
			this.overrideGraphicIndex = new int?(this.BloomLevel);
			base.DirtyMapMesh(base.Map);
		}

		// Token: 0x0600F6D0 RID: 63184 RVA: 0x0047AE13 File Offset: 0x00479013
		public override IEnumerable<ThingDefCountClass> GetAdditionalLeavings(DestroyMode mode)
		{
			foreach (ThingDefCountClass thingDefCountClass in base.GetAdditionalLeavings(mode))
			{
				yield return thingDefCountClass;
			}
			IEnumerator<ThingDefCountClass> enumerator = null;
			yield return new ThingDefCountClass(ThingDefOf.Meat_Twisted, Mathf.RoundToInt(30f * this.Growth));
			yield break;
			yield break;
		}

		// Token: 0x0600F6D1 RID: 63185 RVA: 0x0047AE2A File Offset: 0x0047902A
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			Find.Anomaly.Notify_HarbingerTreeDied();
		}

		// Token: 0x0600F6D2 RID: 63186 RVA: 0x0047AE40 File Offset: 0x00479040
		private void CreateCorpseStockpile()
		{
			List<HarbingerTree> selectedTrees = Find.Selector.SelectedObjects.OfType<HarbingerTree>().ToList<HarbingerTree>();
			if (base.Map.zoneManager.ZoneAt(base.Position) != null)
			{
				Zone zone = base.Map.zoneManager.ZoneAt(base.Position);
				Zone_Stockpile existing = zone as Zone_Stockpile;
				if (existing != null)
				{
					base.Map.floodFiller.FloodFill(base.Position, (IntVec3 c) => selectedTrees.Any((HarbingerTree tree) => tree.RadialCells.Contains(c)) && (this.Map.zoneManager.ZoneAt(c) == null || this.Map.zoneManager.ZoneAt(c) == existing) && Designator_ZoneAdd.IsZoneableCell(c, this.Map), delegate(IntVec3 c)
					{
						if (!existing.ContainsCell(c))
						{
							existing.AddCell(c);
						}
					}, int.MaxValue, false, null);
				}
				return;
			}
			Zone_Stockpile stockpile = new Zone_Stockpile(StorageSettingsPreset.CorpseStockpile, base.Map.zoneManager);
			stockpile.settings.filter.SetAllow(ThingCategoryDefOf.CorpsesMechanoid, false, null, null);
			stockpile.settings.filter.SetAllow(SpecialThingFilterDefOf.AllowCorpsesUnnatural, false);
			base.Map.zoneManager.RegisterZone(stockpile);
			Zone_Stockpile existingStockpile = null;
			base.Map.floodFiller.FloodFill(base.Position, delegate(IntVec3 c)
			{
				Zone_Stockpile zone_Stockpile = this.Map.zoneManager.ZoneAt(c) as Zone_Stockpile;
				if (zone_Stockpile != null)
				{
					existingStockpile = zone_Stockpile;
				}
				return selectedTrees.Any((HarbingerTree tree) => tree.RadialCells.Contains(c)) && this.Map.zoneManager.ZoneAt(c) == null && Designator_ZoneAdd.IsZoneableCell(c, this.Map);
			}, delegate(IntVec3 c)
			{
				stockpile.AddCell(c);
			}, int.MaxValue, false, null);
			if (existingStockpile != null)
			{
				List<IntVec3> list = stockpile.Cells.ToList<IntVec3>();
				stockpile.Delete();
				foreach (IntVec3 c2 in list)
				{
					existingStockpile.AddCell(c2);
				}
			}
		}

		// Token: 0x0600F6D3 RID: 63187 RVA: 0x0047B008 File Offset: 0x00479208
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			GenDraw.DrawRadiusRing(base.Position, this.ConsumeRadius);
		}

		// Token: 0x0600F6D4 RID: 63188 RVA: 0x0047B024 File Offset: 0x00479224
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			if (DebugSettings.godMode)
			{
				stringBuilder.AppendLine("DEV: Nutrition: " + this.nutrition.ToString("F2"));
			}
			if (this.ConsumableNearby)
			{
				stringBuilder.AppendLine("HarbingerTreeConsuming".Translate());
			}
			else
			{
				stringBuilder.AppendLine("HarbingerTreeNotConsuming".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600F6D5 RID: 63189 RVA: 0x0047B0AD File Offset: 0x004792AD
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!(base.Map.zoneManager.ZoneAt(base.Position) is Zone_Stockpile) || base.Map.zoneManager.ZoneAt(base.Position) != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "CreateCorpseStockpile".Translate(),
					defaultDesc = "CreateCorpseStockpileDesc".Translate(),
					icon = HarbingerTree.CreateCorpseStockpileIcon.Texture,
					action = new Action(this.CreateCorpseStockpile)
				};
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Add 10 nutrition",
					action = delegate()
					{
						this.AddNutrition(10f);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Spawn new tree",
					action = new Action(this.SpawnNewTree)
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Update Roots",
					action = new Action(this.UpdateRoots)
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Blood Spatters (Delay)",
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						for (int i = 1; i < 10; i++)
						{
							int delay = i * 60;
							list.Add(new FloatMenuOption(delay.TicksToSeconds().ToString() + "s", delegate()
							{
								this.<GetGizmos>g__DelayedSplatter|39_0(delay);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F6DA RID: 63194 RVA: 0x0047B164 File Offset: 0x00479364
		[CompilerGenerated]
		private void <GetGizmos>g__DelayedSplatter|39_0(int ticks)
		{
			EffecterDef effecterDef = new EffecterDef();
			effecterDef.maintainTicks = ticks + 5;
			SubEffecterDef subEffecterDef = new SubEffecterDef();
			subEffecterDef.subEffecterClass = typeof(SubEffecter_GroupedChance);
			subEffecterDef.chancePerTick = 1f;
			subEffecterDef.children = new List<SubEffecterDef>(EffecterDefOf.HarbingerTreeConsume.children);
			subEffecterDef.initialDelayTicks = ticks;
			subEffecterDef.subTriggerOnSpawn = false;
			effecterDef.children = new List<SubEffecterDef>
			{
				subEffecterDef
			};
			foreach (ThingWithComps t in this.rootTargets)
			{
				effecterDef.SpawnMaintained(t, t, 1f);
			}
		}

		// Token: 0x0400A260 RID: 41568
		private const float ConsumingGrowthRateFactor = 1.5f;

		// Token: 0x0400A261 RID: 41569
		private const int TwistedFleshCountAtMaxGrowth = 30;

		// Token: 0x0400A262 RID: 41570
		private static readonly FloatRange NutritionForNewTreeRange = new FloatRange(65f, 95f);

		// Token: 0x0400A263 RID: 41571
		private static readonly CachedTexture CreateCorpseStockpileIcon = new CachedTexture("UI/Icons/CorpseStockpileZone");

		// Token: 0x0400A264 RID: 41572
		private static readonly SimpleCurve ConsumeRadiusByGrowthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.15f, 3.9f),
				true
			},
			{
				new CurvePoint(0.85f, 5.9f),
				true
			}
		};

		// Token: 0x0400A265 RID: 41573
		private float nutrition;

		// Token: 0x0400A266 RID: 41574
		private float nutritionForNextTree;

		// Token: 0x0400A267 RID: 41575
		private Dictionary<ThingWithComps, Mote> roots;

		// Token: 0x0400A268 RID: 41576
		private List<ThingWithComps> rootTargets;

		// Token: 0x0400A269 RID: 41577
		private Queue<ThingWithComps> deferredDestroy = new Queue<ThingWithComps>();

		// Token: 0x0400A26A RID: 41578
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400A26B RID: 41579
		private List<IntVec3> tmpRadialCells = new List<IntVec3>();
	}
}
