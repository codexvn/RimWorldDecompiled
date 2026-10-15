using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class MonumentMarker : Thing
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__29 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MonumentMarker _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private List<SketchEntity>.Enumerator _003C_003E7__wrap2;

		private SketchBuildable _003Cbuildable_003E5__4;

		private TerrainDef _003Cterrain_003E5__5;

		private IEnumerator<TerrainDef> _003C_003E7__wrap5;

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
		public _003CGetGizmos_003Ed__29(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -6:
			case -5:
			case 7:
			case 8:
			case 9:
			case 10:
				try
				{
					if (num == -6 || num == 8)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally4();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<SketchEntity>.Enumerator);
			_003Cbuildable_003E5__4 = null;
			_003Cterrain_003E5__5 = null;
			_003C_003E7__wrap5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MonumentMarker CS_0024_003C_003E8__locals31 = _003C_003E4__this;
				bool flag2;
				SketchEntity current6;
				Command_Action placeBlueprintsCommand;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Thing)CS_0024_003C_003E8__locals31).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0094;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0094;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0125;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0233;
				case 4:
					_003C_003E1__state = -4;
					goto IL_0279;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0366;
				case 6:
					_003C_003E1__state = -1;
					goto IL_03c1;
				case 7:
					_003C_003E1__state = -5;
					goto IL_05cb;
				case 8:
					_003C_003E1__state = -6;
					goto IL_05ae;
				case 9:
					_003C_003E1__state = -5;
					goto IL_0636;
				case 10:
					{
						_003C_003E1__state = -5;
						goto IL_0664;
					}
					IL_0094:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!CS_0024_003C_003E8__locals31.AllDone)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandCancelMonumentMarker".Translate(),
							defaultDesc = "CommandCancelMonumentMarkerDesc".Translate(),
							icon = CancelCommandTex,
							action = delegate
							{
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmCancelMonumentMarker".Translate(), delegate
								{
									QuestUtility.SendQuestTargetSignals(CS_0024_003C_003E8__locals31.questTags, "MonumentCancelled", CS_0024_003C_003E8__locals31.Named("SUBJECT"));
									CS_0024_003C_003E8__locals31.RemovePossiblyRelatedBlueprints();
									CS_0024_003C_003E8__locals31.Uninstall();
								}, destructive: true));
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0125;
					IL_0366:
					if (CS_0024_003C_003E8__locals31.AllDone && CS_0024_003C_003E8__locals31.AnyDisallowedBuilding && !CS_0024_003C_003E8__locals31.DisallowedBuildingTicksExpired)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Disallowed building ticks +6 hours",
							action = delegate
							{
								CS_0024_003C_003E8__locals31.ticksSinceDisallowedBuilding += 15000;
							}
						};
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_03c1;
					IL_05ae:
					while (_003C_003E7__wrap5.MoveNext())
					{
						TerrainDef current2 = _003C_003E7__wrap5.Current;
						if (!current2.BuildableByPlayer || current2.designatorDropdown != null)
						{
							continue;
						}
						bool flag = true;
						for (int i = 0; i < _003Cterrain_003E5__5.affordances.Count; i++)
						{
							if (!current2.affordances.Contains(_003Cterrain_003E5__5.affordances[i]))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							Command command = BuildCopyCommandUtility.BuildCommand(current2, null, null, null, styleOverridden: false, null, null, allowHotKey: false, null);
							if (command != null)
							{
								_003C_003E2__current = command;
								_003C_003E1__state = 8;
								return true;
							}
						}
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap5 = null;
					goto IL_05cb;
					IL_0125:
					flag2 = false;
					foreach (SketchEntity entity in CS_0024_003C_003E8__locals31.sketch.Entities)
					{
						if (entity is SketchBuildable sketchBuildable && !entity.IsSameSpawnedOrBlueprintOrFrame(entity.pos + CS_0024_003C_003E8__locals31.Position, CS_0024_003C_003E8__locals31.Map) && !entity.IsSpawningBlocked(entity.pos + CS_0024_003C_003E8__locals31.Position, CS_0024_003C_003E8__locals31.Map) && BuildCopyCommandUtility.FindAllowedDesignator(sketchBuildable.Buildable) != null)
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandPlaceBlueprints".Translate(),
							defaultDesc = "CommandPlaceBlueprintsDesc".Translate(),
							icon = PlaceBlueprintsCommandTex,
							action = delegate
							{
								IEnumerable<ThingDef> enumerable = CS_0024_003C_003E8__locals31.AllowedStuffs();
								if (!enumerable.Any())
								{
									CS_0024_003C_003E8__locals31.PlaceAllBlueprints(null);
									SoundDefOf.Click.PlayOneShotOnCamera();
								}
								else if (enumerable.Count() == 1)
								{
									CS_0024_003C_003E8__locals31.PlaceAllBlueprints(enumerable.First());
									SoundDefOf.Click.PlayOneShotOnCamera();
								}
								else
								{
									CS_0024_003C_003E8__locals31.ListFloatMenuOptions(enumerable, delegate(ThingDef stuff)
									{
										CS_0024_003C_003E8__locals31.PlaceAllBlueprints(stuff);
									});
								}
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0233;
					IL_0664:
					_003Cbuildable_003E5__4 = null;
					goto IL_066b;
					IL_03c1:
					tmpUniqueBuildableDefs.Clear();
					_003C_003E7__wrap2 = CS_0024_003C_003E8__locals31.sketch.Entities.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_066b;
					IL_0233:
					_003C_003E7__wrap1 = QuestUtility.GetQuestRelatedGizmos(CS_0024_003C_003E8__locals31).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0279;
					IL_0279:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						bool flag3 = false;
						foreach (SketchEntity entity2 in CS_0024_003C_003E8__locals31.sketch.Entities)
						{
							if (!entity2.IsSameSpawned(entity2.pos + CS_0024_003C_003E8__locals31.Position, CS_0024_003C_003E8__locals31.Map) && !entity2.IsSpawningBlocked(entity2.pos + CS_0024_003C_003E8__locals31.Position, CS_0024_003C_003E8__locals31.Map))
							{
								flag3 = true;
								break;
							}
						}
						if (flag3)
						{
							_003C_003E2__current = new Command_Action
							{
								defaultLabel = "DEV: Build all",
								action = delegate
								{
									CS_0024_003C_003E8__locals31.DebugBuildAll();
									SoundDefOf.Click.PlayOneShotOnCamera();
								}
							};
							_003C_003E1__state = 5;
							return true;
						}
						goto IL_0366;
					}
					goto IL_03c1;
					IL_066b:
					do
					{
						if (_003C_003E7__wrap2.MoveNext())
						{
							current6 = _003C_003E7__wrap2.Current;
							_003Cbuildable_003E5__4 = current6 as SketchBuildable;
							continue;
						}
						_003C_003Em__Finally3();
						_003C_003E7__wrap2 = default(List<SketchEntity>.Enumerator);
						tmpUniqueBuildableDefs.Clear();
						return false;
					}
					while (_003Cbuildable_003E5__4 == null || current6.IsSameSpawnedOrBlueprintOrFrame(current6.pos + CS_0024_003C_003E8__locals31.Position, CS_0024_003C_003E8__locals31.Map));
					if (tmpUniqueBuildableDefs.Add(new Pair<BuildableDef, ThingDef>(_003Cbuildable_003E5__4.Buildable, _003Cbuildable_003E5__4.Stuff)))
					{
						if (_003Cbuildable_003E5__4 is SketchTerrain { treatSimilarAsSame: not false })
						{
							_003Cterrain_003E5__5 = _003Cbuildable_003E5__4.Buildable as TerrainDef;
							if (_003Cterrain_003E5__5.designatorDropdown != null)
							{
								Designator designator = BuildCopyCommandUtility.FindAllowedDesignatorRoot(_003Cbuildable_003E5__4.Buildable);
								if (designator != null)
								{
									_003C_003E2__current = designator;
									_003C_003E1__state = 7;
									return true;
								}
								goto IL_05cb;
							}
							IEnumerable<TerrainDef> allDefs = DefDatabase<TerrainDef>.AllDefs;
							_003C_003E7__wrap5 = allDefs.GetEnumerator();
							_003C_003E1__state = -6;
							goto IL_05ae;
						}
						Command command2 = BuildCopyCommandUtility.BuildCommand(_003Cbuildable_003E5__4.Buildable, _003Cbuildable_003E5__4.Stuff, null, null, styleOverridden: false, current6.LabelCap, _003Cbuildable_003E5__4.Buildable.description, allowHotKey: false, null);
						if (command2 != null)
						{
							_003C_003E2__current = command2;
							_003C_003E1__state = 9;
							return true;
						}
						goto IL_0636;
					}
					goto IL_0664;
					IL_05cb:
					_003Cterrain_003E5__5 = null;
					goto IL_0636;
					IL_0636:
					placeBlueprintsCommand = CS_0024_003C_003E8__locals31.GetPlaceBlueprintsCommand(_003Cbuildable_003E5__4);
					if (placeBlueprintsCommand != null)
					{
						_003C_003E2__current = placeBlueprintsCommand;
						_003C_003E1__state = 10;
						return true;
					}
					goto IL_0664;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -5;
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
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
			_003CGetGizmos_003Ed__29 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__29(0)
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

	public Sketch sketch = new Sketch();

	public int ticksSinceDisallowedBuilding;

	public bool complete;

	private static readonly Texture2D PlaceBlueprintsCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints");

	private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	public const int DestroyAfterTicksSinceDisallowedBuilding = 60000;

	private const int MonumentCompletedCheckIntervalTicks = 177;

	private static List<ThingDef> tmpAllowedBuildings = new List<ThingDef>();

	private static HashSet<Pair<BuildableDef, ThingDef>> tmpUniqueBuildableDefs = new HashSet<Pair<BuildableDef, ThingDef>>();

	private static List<SketchBuildable> tmpBuildables = new List<SketchBuildable>();

	private static Dictionary<string, Pair<int, int>> tmpBuiltParts = new Dictionary<string, Pair<int, int>>();

	private static List<StuffCategoryDef> tmpStuffCategories = new List<StuffCategoryDef>();

	public override CellRect? CustomRectForSelector
	{
		get
		{
			if (!base.Spawned)
			{
				return null;
			}
			return sketch.OccupiedRect.MovedBy(base.Position);
		}
	}

	public bool AllDone
	{
		get
		{
			if (!base.Spawned)
			{
				return false;
			}
			foreach (SketchEntity entity in sketch.Entities)
			{
				if (!entity.IsSameSpawned(base.Position + entity.pos, base.Map))
				{
					return false;
				}
			}
			return true;
		}
	}

	public IntVec2 Size => sketch.OccupiedSize;

	public Thing FirstDisallowedBuilding
	{
		get
		{
			if (!base.Spawned)
			{
				return null;
			}
			List<SketchTerrain> terrain = sketch.Terrain;
			for (int i = 0; i < terrain.Count; i++)
			{
				tmpAllowedBuildings.Clear();
				sketch.ThingsAt(terrain[i].pos, out var singleResult, out var multipleResults);
				if (singleResult != null)
				{
					tmpAllowedBuildings.Add(singleResult.def);
				}
				if (multipleResults != null)
				{
					for (int j = 0; j < multipleResults.Count; j++)
					{
						tmpAllowedBuildings.Add(multipleResults[j].def);
					}
				}
				List<Thing> thingList = (terrain[i].pos + base.Position).GetThingList(base.Map);
				for (int k = 0; k < thingList.Count; k++)
				{
					if (thingList[k].def.IsBuildingArtificial && !thingList[k].def.IsBlueprint && !thingList[k].def.IsFrame && !thingList[k].def.building.isPowerConduit && !tmpAllowedBuildings.Contains(thingList[k].def))
					{
						return thingList[k];
					}
				}
			}
			return null;
		}
	}

	public bool AnyDisallowedBuilding => FirstDisallowedBuilding != null;

	public SketchEntity FirstEntityWithMissingBlueprint
	{
		get
		{
			if (!base.Spawned)
			{
				return null;
			}
			foreach (SketchEntity entity in sketch.Entities)
			{
				if (!entity.IsSameSpawnedOrBlueprintOrFrame(base.Position + entity.pos, base.Map))
				{
					return entity;
				}
			}
			return null;
		}
	}

	public bool DisallowedBuildingTicksExpired => ticksSinceDisallowedBuilding >= 60000;

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (!respawningAfterLoad)
		{
			sketch.Rotate(base.Rotation);
		}
		if (!ModLister.CheckRoyalty("Monument"))
		{
			Destroy();
		}
	}

	protected override void Tick()
	{
		if (!this.IsHashIntervalTick(177))
		{
			return;
		}
		bool allDone = AllDone;
		if (!complete && allDone)
		{
			complete = true;
			QuestUtility.SendQuestTargetSignals(questTags, "MonumentCompleted", this.Named("SUBJECT"));
		}
		if (complete && !allDone)
		{
			QuestUtility.SendQuestTargetSignals(questTags, "MonumentDestroyed", this.Named("SUBJECT"));
			if (!base.Destroyed)
			{
				Destroy();
			}
		}
		else
		{
			if (!allDone)
			{
				return;
			}
			if (AnyDisallowedBuilding)
			{
				ticksSinceDisallowedBuilding += 177;
				if (DisallowedBuildingTicksExpired)
				{
					Messages.Message("MessageMonumentDestroyedBecauseOfDisallowedBuilding".Translate(), new TargetInfo(base.Position, base.Map), MessageTypeDefOf.NegativeEvent);
					QuestUtility.SendQuestTargetSignals(questTags, "MonumentDestroyed", this.Named("SUBJECT"));
					if (!base.Destroyed)
					{
						Destroy();
					}
				}
			}
			else
			{
				ticksSinceDisallowedBuilding = 0;
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref sketch, "sketch");
		Scribe_Values.Look(ref ticksSinceDisallowedBuilding, "ticksSinceDisallowedBuilding", 0);
		Scribe_Values.Look(ref complete, "complete", defaultValue: false);
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		DrawGhost(drawLoc.ToIntVec3(), placingMode: false, base.Rotation);
	}

	public void DrawGhost(IntVec3 at, bool placingMode, Rot4 rotation)
	{
		CellRect rect = sketch.OccupiedRect.MovedBy(at);
		Blueprint_Install thingToIgnore = FindMyBlueprint(rect, Find.CurrentMap);
		sketch.Rotate(rotation);
		Func<SketchEntity, IntVec3, List<Thing>, Map, bool> validator = null;
		if (placingMode)
		{
			validator = (SketchEntity entity, IntVec3 offset, List<Thing> things, Map map) => MonumentMarkerUtility.GetFirstAdjacentBuilding(entity, offset, things, map) == null;
		}
		sketch.DrawGhost(at, Sketch.SpawnPosType.Unchanged, placingMode, thingToIgnore, validator);
	}

	public Blueprint_Install FindMyBlueprint(CellRect rect, Map map)
	{
		foreach (IntVec3 item in rect)
		{
			if (!item.InBounds(map))
			{
				continue;
			}
			List<Thing> thingList = item.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is Blueprint_Install blueprint_Install && blueprint_Install.ThingToInstall == this)
				{
					return blueprint_Install;
				}
			}
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__29))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__29(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Command_Action GetPlaceBlueprintsCommand(SketchBuildable buildable)
	{
		return new Command_Action
		{
			defaultLabel = "CommandPlaceBlueprintsSpecific".Translate(buildable.Label).CapitalizeFirst(),
			defaultDesc = "CommandPlaceBlueprintsSpecificDesc".Translate(buildable.Label).CapitalizeFirst(),
			icon = PlaceBlueprintsCommandTex,
			Order = 2000f,
			action = delegate
			{
				List<ThingDef> list = AllowedStuffsFor(buildable);
				if (!list.Any())
				{
					PlaceBlueprintsSimilarTo(buildable, null);
					SoundDefOf.Click.PlayOneShotOnCamera();
				}
				else if (list.Count() == 1)
				{
					PlaceBlueprintsSimilarTo(buildable, list.First());
					SoundDefOf.Click.PlayOneShotOnCamera();
				}
				else
				{
					ListFloatMenuOptions(list, delegate(ThingDef stuff)
					{
						PlaceBlueprintsSimilarTo(buildable, stuff);
					});
				}
			}
		};
	}

	private void ListFloatMenuOptions(IEnumerable<ThingDef> allowedStuff, Action<ThingDef> action)
	{
		List<FloatMenuOption> list = new List<FloatMenuOption>();
		bool flag = false;
		foreach (ThingDef item in allowedStuff)
		{
			if (base.Map.listerThings.ThingsOfDef(item).Count > 0)
			{
				flag = true;
				break;
			}
		}
		foreach (ThingDef item2 in allowedStuff)
		{
			if (!flag || base.Map.listerThings.ThingsOfDef(item2).Count != 0)
			{
				ThingDef stuffLocal = item2;
				list.Add(new FloatMenuOption(stuffLocal.LabelCap, delegate
				{
					action(stuffLocal);
				}, item2, null, forceBasicStyle: false, MenuOptionPriority.Default, null, null, 0f, null, null, playSelectionSound: true, 0, null));
			}
		}
		Find.WindowStack.Add(new FloatMenu(list));
	}

	public void DebugBuildAll()
	{
		sketch.Spawn(base.Map, base.Position, Faction.OfPlayer);
	}

	private void PlaceBlueprintsSimilarTo(SketchBuildable buildable, ThingDef preferredStuffIfNone)
	{
		bool flag = buildable is SketchTerrain;
		foreach (SketchBuildable buildable2 in sketch.Buildables)
		{
			if ((flag && buildable2 is SketchTerrain sketchTerrain && sketchTerrain.IsSameOrSimilar(buildable.Buildable)) || (!flag && buildable2 is SketchThing sketchThing && buildable.Buildable == sketchThing.def))
			{
				tmpBuildables.Add(buildable2);
			}
		}
		foreach (SketchBuildable tmpBuildable in tmpBuildables)
		{
			PlaceBlueprint(tmpBuildable, preferredStuffIfNone);
		}
		tmpBuildables.Clear();
	}

	private void PlaceAllBlueprints(ThingDef preferredStuffIfNone)
	{
		foreach (SketchEntity entity in sketch.Entities)
		{
			PlaceBlueprint(entity, preferredStuffIfNone);
		}
	}

	private void PlaceBlueprint(SketchEntity entity, ThingDef preferredStuffIfNone)
	{
		if (entity is SketchBuildable sketchBuildable && !entity.IsSameSpawnedOrBlueprintOrFrame(entity.pos + base.Position, base.Map) && !entity.IsSpawningBlocked(entity.pos + base.Position, base.Map) && BuildCopyCommandUtility.FindAllowedDesignator(sketchBuildable.Buildable) != null)
		{
			if (entity is SketchThing sketchThing && sketchThing.def.MadeFromStuff && sketchThing.stuff == null && preferredStuffIfNone != null && preferredStuffIfNone.stuffProps.CanMake(sketchThing.def))
			{
				sketchThing.stuff = preferredStuffIfNone;
				entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint);
				sketchThing.stuff = null;
			}
			else if (entity is SketchTerrain { stuffForComparingSimilar: null } sketchTerrain && preferredStuffIfNone != null)
			{
				sketchTerrain.stuffForComparingSimilar = preferredStuffIfNone;
				entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint);
				sketchTerrain.stuffForComparingSimilar = null;
			}
			else
			{
				entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint);
			}
		}
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		Quest quest = Find.QuestManager.QuestsListForReading.FirstOrDefault((Quest q) => q.QuestLookTargets.Contains(this));
		if (quest != null)
		{
			stringBuilder.Append("Quest".Translate() + ": " + quest.name);
		}
		QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
		if (base.Spawned && !AllDone)
		{
			tmpBuiltParts.Clear();
			foreach (SketchEntity entity in sketch.Entities)
			{
				if (!tmpBuiltParts.TryGetValue(entity.LabelCap, out var value))
				{
					value = default(Pair<int, int>);
				}
				value = ((!entity.IsSameSpawned(entity.pos + base.Position, base.Map)) ? new Pair<int, int>(value.First, value.Second + 1) : new Pair<int, int>(value.First + 1, value.Second + 1));
				tmpBuiltParts[entity.LabelCap] = value;
			}
			foreach (KeyValuePair<string, Pair<int, int>> tmpBuiltPart in tmpBuiltParts)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(tmpBuiltPart.Key.CapitalizeFirst() + ": " + tmpBuiltPart.Value.First + " / " + tmpBuiltPart.Value.Second);
			}
			tmpBuiltParts.Clear();
		}
		return stringBuilder.ToString();
	}

	private void RemovePossiblyRelatedBlueprints()
	{
		if (!base.Spawned)
		{
			return;
		}
		foreach (SketchBuildable buildable in sketch.Buildables)
		{
			if (buildable.GetSpawnedBlueprintOrFrame(base.Position + buildable.pos, base.Map) is Blueprint blueprint)
			{
				blueprint.Destroy();
			}
		}
	}

	public bool IsPart(Thing thing)
	{
		if (!base.Spawned)
		{
			return false;
		}
		if (!sketch.OccupiedRect.MovedBy(base.Position).Contains(thing.Position))
		{
			return false;
		}
		sketch.ThingsAt(thing.Position - base.Position, out var singleResult, out var multipleResults);
		if (singleResult != null && IsPartInternal(singleResult))
		{
			return true;
		}
		if (multipleResults != null)
		{
			for (int i = 0; i < multipleResults.Count; i++)
			{
				if (IsPartInternal(multipleResults[i]))
				{
					return true;
				}
			}
		}
		if (thing.def.entityDefToBuild != null)
		{
			SketchTerrain sketchTerrain = sketch.SketchTerrainAt(thing.Position - base.Position);
			if (sketchTerrain != null && IsPartInternal(sketchTerrain))
			{
				return true;
			}
		}
		return false;
		bool IsPartInternal(SketchBuildable b)
		{
			BuildableDef buildable = b.Buildable;
			if (thing.def != buildable && thing.def.entityDefToBuild != buildable)
			{
				return false;
			}
			if (b.GetSpawnedBlueprintOrFrame(b.pos + base.Position, base.Map) == thing)
			{
				return true;
			}
			if (b is SketchThing sketchThing && sketchThing.GetSameSpawned(sketchThing.pos + base.Position, base.Map) == thing)
			{
				return true;
			}
			return false;
		}
	}

	public bool AllowsPlacingBlueprint(BuildableDef buildable, IntVec3 pos, Rot4 rot, ThingDef stuff)
	{
		if (!base.Spawned)
		{
			return true;
		}
		CellRect newRect = GenAdj.OccupiedRect(pos, rot, buildable.Size);
		if (!sketch.OccupiedRect.MovedBy(base.Position).Overlaps(newRect))
		{
			return true;
		}
		bool collided = false;
		foreach (IntVec3 item in newRect)
		{
			sketch.ThingsAt(item - base.Position, out var singleResult, out var multipleResults);
			if (singleResult != null && CheckEntity(singleResult))
			{
				return true;
			}
			if (multipleResults != null)
			{
				for (int i = 0; i < multipleResults.Count; i++)
				{
					if (CheckEntity(multipleResults[i]))
					{
						return true;
					}
				}
			}
			SketchTerrain sketchTerrain = sketch.SketchTerrainAt(item - base.Position);
			if (sketchTerrain != null && CheckEntity(sketchTerrain))
			{
				return true;
			}
		}
		return !collided;
		bool CheckEntity(SketchBuildable entity)
		{
			if (entity.IsSameSpawned(entity.pos + base.Position, base.Map))
			{
				return false;
			}
			if (entity.OccupiedRect.MovedBy(base.Position).Overlaps(newRect))
			{
				collided = true;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (entity.OccupiedRect.MovedBy(base.Position).Equals(newRect) && IsSameOrSimilar(entity) && (stuff == null || entity.Stuff == null || stuff == entity.Stuff))
			{
				if (sketchThing != null && !(sketchThing.rot == rot) && !(sketchThing.rot == rot.Opposite))
				{
					return !sketchThing.def.rotatable;
				}
				return true;
			}
			return false;
		}
		bool IsSameOrSimilar(SketchBuildable entity)
		{
			if (buildable == entity.Buildable || (entity is SketchTerrain sketchTerrain2 && sketchTerrain2.IsSameOrSimilar(buildable)))
			{
				return true;
			}
			return false;
		}
	}

	public IEnumerable<ThingDef> AllowedStuffs()
	{
		tmpStuffCategories.Clear();
		bool flag = true;
		List<SketchThing> things = sketch.Things;
		for (int i = 0; i < things.Count; i++)
		{
			if (!things[i].def.MadeFromStuff || things[i].stuff != null)
			{
				continue;
			}
			if (flag)
			{
				flag = false;
				tmpStuffCategories.AddRange(things[i].def.stuffCategories);
				continue;
			}
			bool flag2 = false;
			for (int j = 0; j < things[i].def.stuffCategories.Count; j++)
			{
				if (tmpStuffCategories.Contains(things[i].def.stuffCategories[j]))
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				continue;
			}
			for (int num = tmpStuffCategories.Count - 1; num >= 0; num--)
			{
				if (!things[i].def.stuffCategories.Contains(tmpStuffCategories[num]))
				{
					tmpStuffCategories.RemoveAt(num);
				}
			}
		}
		return GenStuff.AllowedStuffs(tmpStuffCategories);
	}

	public List<ThingDef> AllowedStuffsFor(SketchBuildable buildable)
	{
		if (buildable.Buildable.MadeFromStuff && buildable.Stuff == null)
		{
			return GenStuff.AllowedStuffs(buildable.Buildable.stuffCategories).ToList();
		}
		if (buildable is SketchTerrain sketchTerrain)
		{
			List<ThingDef> list = new List<ThingDef>();
			{
				foreach (TerrainDef allDef in DefDatabase<TerrainDef>.AllDefs)
				{
					if (allDef.BuildableByPlayer && sketchTerrain.IsSameOrSimilar(allDef) && !allDef.CostList.NullOrEmpty())
					{
						list.Add(allDef.CostList.First().thingDef);
					}
				}
				return list;
			}
		}
		return null;
	}
}
