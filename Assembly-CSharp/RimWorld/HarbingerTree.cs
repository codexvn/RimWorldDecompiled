using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public class HarbingerTree : Plant
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass39_0
	{
		public int delay;

		public HarbingerTree _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__3()
		{
			_003C_003E4__this._003CGetGizmos_003Eg__DelayedSplatter_007C39_0(delay);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetAdditionalLeavings_003Ed__34 : IEnumerable<ThingDefCountClass>, IEnumerable, IEnumerator<ThingDefCountClass>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDefCountClass _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HarbingerTree _003C_003E4__this;

		private DestroyMode mode;

		public DestroyMode _003C_003E3__mode;

		private IEnumerator<ThingDefCountClass> _003C_003E7__wrap1;

		ThingDefCountClass IEnumerator<ThingDefCountClass>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetAdditionalLeavings_003Ed__34(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				HarbingerTree harbingerTree = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)harbingerTree).GetAdditionalLeavings(mode).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0075;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0075;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0075:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ThingDefCountClass current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new ThingDefCountClass(ThingDefOf.Meat_Twisted, Mathf.RoundToInt(30f * harbingerTree.Growth));
					_003C_003E1__state = 2;
					return true;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDefCountClass> IEnumerable<ThingDefCountClass>.GetEnumerator()
		{
			_003CGetAdditionalLeavings_003Ed__34 _003CGetAdditionalLeavings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAdditionalLeavings_003Ed__ = this;
			}
			else
			{
				_003CGetAdditionalLeavings_003Ed__ = new _003CGetAdditionalLeavings_003Ed__34(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetAdditionalLeavings_003Ed__.mode = _003C_003E3__mode;
			return _003CGetAdditionalLeavings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDefCountClass>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__39 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HarbingerTree _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetGizmos_003Ed__39(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				HarbingerTree CS_0024_003C_003E8__locals14 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Plant)CS_0024_003C_003E8__locals14).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					goto IL_013d;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Spawn new tree",
						action = CS_0024_003C_003E8__locals14.SpawnNewTree
					};
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Update Roots",
						action = CS_0024_003C_003E8__locals14.UpdateRoots
					};
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Blood Spatters (Delay)",
						action = delegate
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							for (int i = 1; i < 10; i++)
							{
								_003C_003Ec__DisplayClass39_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass39_0
								{
									_003C_003E4__this = CS_0024_003C_003E8__locals14,
									delay = i * 60
								};
								list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals5.delay.TicksToSeconds() + "s", delegate
								{
									CS_0024_003C_003E8__locals5._003C_003E4__this._003CGetGizmos_003Eg__DelayedSplatter_007C39_0(CS_0024_003C_003E8__locals5.delay);
								}));
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
					_003C_003E1__state = 6;
					return true;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!(CS_0024_003C_003E8__locals14.Map.zoneManager.ZoneAt(CS_0024_003C_003E8__locals14.Position) is Zone_Stockpile) || CS_0024_003C_003E8__locals14.Map.zoneManager.ZoneAt(CS_0024_003C_003E8__locals14.Position) != null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CreateCorpseStockpile".Translate(),
							defaultDesc = "CreateCorpseStockpileDesc".Translate(),
							icon = CreateCorpseStockpileIcon.Texture,
							action = CS_0024_003C_003E8__locals14.CreateCorpseStockpile
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_013d;
					IL_013d:
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Add 10 nutrition",
							action = delegate
							{
								CS_0024_003C_003E8__locals14.AddNutrition(10f);
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__39 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__39(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private const float ConsumingGrowthRateFactor = 1.5f;

	private const int TwistedFleshCountAtMaxGrowth = 30;

	private static readonly FloatRange NutritionForNewTreeRange = new FloatRange(65f, 95f);

	private static readonly CachedTexture CreateCorpseStockpileIcon = new CachedTexture("UI/Icons/CorpseStockpileZone");

	private static readonly SimpleCurve ConsumeRadiusByGrowthCurve = new SimpleCurve
	{
		new CurvePoint(0.15f, 3.9f),
		new CurvePoint(0.85f, 5.9f)
	};

	private float nutrition;

	private float nutritionForNextTree;

	private Dictionary<ThingWithComps, Mote> roots;

	private List<ThingWithComps> rootTargets;

	private Queue<ThingWithComps> deferredDestroy = new Queue<ThingWithComps>();

	private List<Thing> tmpThings = new List<Thing>();

	private List<IntVec3> tmpRadialCells = new List<IntVec3>();

	private bool ConsumableNearby => rootTargets.Count > 0;

	private int BloomLevel => Mathf.Max(Mathf.FloorToInt(Growth / 0.25f - 0.001f), 0);

	public float ConsumeRadius => ConsumeRadiusByGrowthCurve.Evaluate(Growth);

	public override float GrowthRate => base.GrowthRate * GrowthRateFactor_Consuming;

	private float GrowthRateFactor_Consuming
	{
		get
		{
			if (!ConsumableNearby)
			{
				return 1f;
			}
			return 1.5f;
		}
	}

	private IEnumerable<IntVec3> RadialCells => GenRadial.RadialCellsAround(base.Position, ConsumeRadius, useCenter: true);

	public override string GrowthRateCalcDesc
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder(base.GrowthRateCalcDesc);
			if (GrowthRateFactor_Consuming != 1f)
			{
				stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("FleshConsumption".Translate()) + ": " + GrowthRateFactor_Consuming.ToStringPercent());
			}
			return stringBuilder.ToString();
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref rootTargets, "rootTargets", LookMode.Reference);
		Scribe_Values.Look(ref nutrition, "nutrition", 0f);
		Scribe_Values.Look(ref nutritionForNextTree, "nutritionForNextTree", 0f);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			rootTargets.RemoveAll((ThingWithComps x) => x?.Destroyed ?? true);
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		if (!ModLister.CheckAnomaly("Harbinger tree"))
		{
			Destroy();
			return;
		}
		base.SpawnSetup(map, respawningAfterLoad);
		LongEventHandler.ExecuteWhenFinished(UpdateRoots);
		UpdateBloomingStage();
		if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
		{
			nutritionForNextTree = NutritionForNewTreeRange.RandomInRange;
		}
	}

	public override void TickLong()
	{
		base.TickLong();
		UpdateRoots();
		UpdateBloomingStage();
	}

	private void UpdateRoots()
	{
		if (roots == null)
		{
			roots = new Dictionary<ThingWithComps, Mote>();
		}
		if (rootTargets == null)
		{
			rootTargets = new List<ThingWithComps>();
		}
		tmpRadialCells.Clear();
		tmpRadialCells.AddRange(RadialCells.ToList());
		foreach (ThingWithComps rootTarget in rootTargets)
		{
			TryMakeRoot(rootTarget);
		}
		foreach (IntVec3 tmpRadialCell in tmpRadialCells)
		{
			if (!tmpRadialCell.InBounds(base.Map))
			{
				continue;
			}
			tmpThings.Clear();
			tmpThings.AddRange(tmpRadialCell.GetThingList(base.Map));
			foreach (Thing tmpThing in tmpThings)
			{
				if (tmpThing is ThingWithComps thing && thing.TryGetComp<CompHarbingerTreeConsumable>(out var comp) && comp.CanBeConsumed)
				{
					TryMakeRoot(thing);
				}
			}
		}
		foreach (ThingWithComps rootTarget2 in rootTargets)
		{
			if (rootTarget2.Destroyed || !tmpRadialCells.Contains(rootTarget2.PositionHeld))
			{
				deferredDestroy.Enqueue(rootTarget2);
			}
		}
		while (deferredDestroy.Count > 0)
		{
			ThingWithComps thingWithComps = deferredDestroy.Dequeue();
			if (roots.TryGetValue(thingWithComps, out var value))
			{
				value?.Destroy();
			}
			roots.Remove(thingWithComps);
			rootTargets.Remove(thingWithComps);
		}
	}

	private void TryMakeRoot(ThingWithComps thing)
	{
		if (!roots.ContainsKey(thing))
		{
			float exactRot = 0f;
			if (thing is Corpse corpse)
			{
				exactRot = corpse.InnerPawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None);
			}
			roots[thing] = MoteMaker.MakeStaticMote(thing.Position.ToVector3Shifted(), base.Map, ThingDefOf.Mote_HarbingerTreeRoots, 1f, makeOffscreen: false, exactRot);
		}
		if (!rootTargets.Contains(thing))
		{
			rootTargets.Add(thing);
		}
	}

	public void AddNutrition(float amt)
	{
		if (!(amt <= 0f))
		{
			nutrition += amt;
			if (nutrition >= nutritionForNextTree)
			{
				nutrition = 0f;
				nutritionForNextTree = NutritionForNewTreeRange.RandomInRange;
				SpawnNewTree();
			}
		}
	}

	private void SpawnNewTree()
	{
		if (IncidentWorker_HarbingerTreeSpawn.TryGetHarbingerTreeSpawnCell(base.Map, out var cell, base.Position))
		{
			cell.GetPlant(base.Map)?.Destroy();
			Plant plant = (Plant)ThingMaker.MakeThing(ThingDefOf.Plant_TreeHarbinger);
			plant.Growth = IncidentDefOf.HarbingerTreeSpawn.treeGrowth;
			GenSpawn.Spawn(plant, cell, base.Map);
			Messages.Message("HarbingerTreeSprouted".Translate(), plant, MessageTypeDefOf.NeutralEvent);
			Find.Anomaly.Notify_HarbingerTreeSpawned();
		}
	}

	private void UpdateBloomingStage()
	{
		overrideGraphicIndex = BloomLevel;
		DirtyMapMesh(base.Map);
	}

	[IteratorStateMachine(typeof(_003CGetAdditionalLeavings_003Ed__34))]
	public override IEnumerable<ThingDefCountClass> GetAdditionalLeavings(DestroyMode mode)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAdditionalLeavings_003Ed__34(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__mode = mode
		};
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		base.Destroy(mode);
		Find.Anomaly.Notify_HarbingerTreeDied();
	}

	private void CreateCorpseStockpile()
	{
		List<HarbingerTree> selectedTrees = Find.Selector.SelectedObjects.OfType<HarbingerTree>().ToList();
		if (base.Map.zoneManager.ZoneAt(base.Position) != null)
		{
			Zone zone = base.Map.zoneManager.ZoneAt(base.Position);
			Zone_Stockpile existing = zone as Zone_Stockpile;
			if (existing == null)
			{
				return;
			}
			base.Map.floodFiller.FloodFill(base.Position, (IntVec3 c) => selectedTrees.Any((HarbingerTree tree) => tree.RadialCells.Contains(c)) && (base.Map.zoneManager.ZoneAt(c) == null || base.Map.zoneManager.ZoneAt(c) == existing) && (bool)Designator_ZoneAdd.IsZoneableCell(c, base.Map), delegate(IntVec3 c)
			{
				if (!existing.ContainsCell(c))
				{
					existing.AddCell(c);
				}
			});
			return;
		}
		Zone_Stockpile stockpile = new Zone_Stockpile(StorageSettingsPreset.CorpseStockpile, base.Map.zoneManager);
		stockpile.settings.filter.SetAllow(ThingCategoryDefOf.CorpsesMechanoid, allow: false);
		stockpile.settings.filter.SetAllow(SpecialThingFilterDefOf.AllowCorpsesUnnatural, allow: false);
		base.Map.zoneManager.RegisterZone(stockpile);
		Zone_Stockpile existingStockpile = null;
		base.Map.floodFiller.FloodFill(base.Position, delegate(IntVec3 c)
		{
			if (base.Map.zoneManager.ZoneAt(c) is Zone_Stockpile zone_Stockpile)
			{
				existingStockpile = zone_Stockpile;
			}
			return selectedTrees.Any((HarbingerTree tree) => tree.RadialCells.Contains(c)) && base.Map.zoneManager.ZoneAt(c) == null && (bool)Designator_ZoneAdd.IsZoneableCell(c, base.Map);
		}, delegate(IntVec3 c)
		{
			stockpile.AddCell(c);
		});
		if (existingStockpile == null)
		{
			return;
		}
		List<IntVec3> list = stockpile.Cells.ToList();
		stockpile.Delete();
		foreach (IntVec3 item in list)
		{
			existingStockpile.AddCell(item);
		}
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		GenDraw.DrawRadiusRing(base.Position, ConsumeRadius);
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
		stringBuilder.AppendLineIfNotEmpty();
		if (DebugSettings.godMode)
		{
			stringBuilder.AppendLine("DEV: Nutrition: " + nutrition.ToString("F2"));
		}
		if (ConsumableNearby)
		{
			stringBuilder.AppendLine("HarbingerTreeConsuming".Translate());
		}
		else
		{
			stringBuilder.AppendLine("HarbingerTreeNotConsuming".Translate());
		}
		return stringBuilder.ToString().TrimEndNewlines();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__39))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__39(-2)
		{
			_003C_003E4__this = this
		};
	}

	[CompilerGenerated]
	private void _003CGetGizmos_003Eg__DelayedSplatter_007C39_0(int ticks)
	{
		EffecterDef effecterDef = new EffecterDef
		{
			maintainTicks = ticks + 5
		};
		SubEffecterDef item = new SubEffecterDef
		{
			subEffecterClass = typeof(SubEffecter_GroupedChance),
			chancePerTick = 1f,
			children = new List<SubEffecterDef>(EffecterDefOf.HarbingerTreeConsume.children),
			initialDelayTicks = ticks,
			subTriggerOnSpawn = false
		};
		effecterDef.children = new List<SubEffecterDef> { item };
		foreach (ThingWithComps rootTarget in rootTargets)
		{
			effecterDef.SpawnMaintained(rootTarget, rootTarget);
		}
	}
}
