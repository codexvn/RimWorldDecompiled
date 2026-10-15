using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

[StaticConstructorOnStartup]
public class WorldObject : IExposable, ILoadReferenceable, ISelectable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass111_0
	{
		public Quest quest;

		internal void _003CGetGizmos_003Eb__0()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests);
			((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetCaravanGizmos_003Ed__112 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldObject _003C_003E4__this;

		private Caravan caravan;

		public Caravan _003C_003E3__caravan;

		private int _003Ci_003E5__2;

		private IEnumerator<Gizmo> _003C_003E7__wrap2;

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
		public _003CGetCaravanGizmos_003Ed__112(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldObject worldObject = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_0086;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_00b2;
				IL_0086:
				if (_003C_003E7__wrap2.MoveNext())
				{
					Gizmo current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				_003Ci_003E5__2++;
				goto IL_00b2;
				IL_00b2:
				if (_003Ci_003E5__2 < worldObject.comps.Count)
				{
					_003C_003E7__wrap2 = worldObject.comps[_003Ci_003E5__2].GetCaravanGizmos(caravan).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0086;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
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
			_003CGetCaravanGizmos_003Ed__112 _003CGetCaravanGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCaravanGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetCaravanGizmos_003Ed__ = new _003CGetCaravanGizmos_003Ed__112(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetCaravanGizmos_003Ed__.caravan = _003C_003E3__caravan;
			return _003CGetCaravanGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__113 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldObject _003C_003E4__this;

		private Caravan caravan;

		public Caravan _003C_003E3__caravan;

		private int _003Ci_003E5__2;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap2;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetFloatMenuOptions_003Ed__113(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldObject worldObject = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_0086;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_00b2;
				IL_0086:
				if (_003C_003E7__wrap2.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				int num2 = _003Ci_003E5__2 + 1;
				_003Ci_003E5__2 = num2;
				goto IL_00b2;
				IL_00b2:
				if (_003Ci_003E5__2 < worldObject.comps.Count)
				{
					_003C_003E7__wrap2 = worldObject.comps[_003Ci_003E5__2].GetFloatMenuOptions(caravan).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0086;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__113 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__113(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.caravan = _003C_003E3__caravan;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__111 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldObject _003C_003E4__this;

		private List<Quest> _003Cquests_003E5__2;

		private int _003Ci_003E5__3;

		private IEnumerator<Gizmo> _003C_003E7__wrap3;

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
		public _003CGetGizmos_003Ed__111(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 3)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cquests_003E5__2 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldObject CS_0024_003C_003E8__locals14 = _003C_003E4__this;
				int num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals14.ShowRelatedQuests)
					{
						_003Cquests_003E5__2 = Find.QuestManager.QuestsListForReading;
						_003Ci_003E5__3 = 0;
						goto IL_015d;
					}
					goto IL_017a;
				case 1:
					_003C_003E1__state = -1;
					goto IL_014b;
				case 2:
					_003C_003E1__state = -1;
					goto IL_01d0;
				case 3:
					{
						_003C_003E1__state = -3;
						goto IL_022c;
					}
					IL_01d0:
					_003Ci_003E5__3 = 0;
					goto IL_0258;
					IL_015d:
					if (_003Ci_003E5__3 < _003Cquests_003E5__2.Count)
					{
						_003C_003Ec__DisplayClass111_0 CS_0024_003C_003E8__locals9 = new _003C_003Ec__DisplayClass111_0
						{
							quest = _003Cquests_003E5__2[_003Ci_003E5__3]
						};
						if (!CS_0024_003C_003E8__locals9.quest.hidden && !CS_0024_003C_003E8__locals9.quest.Historical && !CS_0024_003C_003E8__locals9.quest.dismissed && CS_0024_003C_003E8__locals9.quest.QuestLookTargets.Contains(CS_0024_003C_003E8__locals14))
						{
							_003C_003E2__current = new Command_Action
							{
								defaultLabel = "CommandViewQuest".Translate(CS_0024_003C_003E8__locals9.quest.name),
								defaultDesc = "CommandViewQuestDesc".Translate(),
								icon = ViewQuestCommandTex,
								Order = -1f,
								action = delegate
								{
									Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests);
									((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(CS_0024_003C_003E8__locals9.quest);
								}
							};
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_014b;
					}
					_003Cquests_003E5__2 = null;
					goto IL_017a;
					IL_014b:
					_003Ci_003E5__3++;
					goto IL_015d;
					IL_017a:
					if (DebugSettings.ShowDevGizmos && CS_0024_003C_003E8__locals14 is MapParent { HasMap: false })
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Generate",
							action = delegate
							{
								LongEventHandler.QueueLongEvent(delegate
								{
									CS_0024_003C_003E8__locals14.SetFaction(Faction.OfPlayer);
									Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(CS_0024_003C_003E8__locals14.Tile, new IntVec3(200, 1, 200), null);
									Current.Game.CurrentMap = orGenerateMap;
									CameraJumper.TryJump(orGenerateMap.Center, orGenerateMap);
								}, "GeneratingMap", doAsynchronously: false, GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap);
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_01d0;
					IL_0258:
					if (_003Ci_003E5__3 < CS_0024_003C_003E8__locals14.comps.Count)
					{
						_003C_003E7__wrap3 = CS_0024_003C_003E8__locals14.comps[_003Ci_003E5__3].GetGizmos().GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_022c;
					}
					return false;
					IL_022c:
					if (_003C_003E7__wrap3.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = null;
					num2 = _003Ci_003E5__3 + 1;
					_003Ci_003E5__3 = num2;
					goto IL_0258;
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
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
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
			_003CGetGizmos_003Ed__111 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__111(0)
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

	[CompilerGenerated]
	private sealed class _003CIncidentTargetTags_003Ed__85 : IEnumerable<IncidentTargetTagDef>, IEnumerable, IEnumerator<IncidentTargetTagDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IncidentTargetTagDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldObject _003C_003E4__this;

		private List<IncidentTargetTagDef>.Enumerator _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		private IEnumerator<IncidentTargetTagDef> _003C_003E7__wrap3;

		IncidentTargetTagDef IEnumerator<IncidentTargetTagDef>.Current
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
		public _003CIncidentTargetTags_003Ed__85(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
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
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<IncidentTargetTagDef>.Enumerator);
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldObject worldObject = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (worldObject.def.IncidentTargetTags != null)
					{
						_003C_003E7__wrap1 = worldObject.def.IncidentTargetTags.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0084;
					}
					goto IL_00a3;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0084;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00ff;
					}
					IL_012b:
					if (_003Ci_003E5__3 < worldObject.comps.Count)
					{
						_003C_003E7__wrap3 = worldObject.comps[_003Ci_003E5__3].IncidentTargetTags().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00ff;
					}
					return false;
					IL_0084:
					if (_003C_003E7__wrap1.MoveNext())
					{
						IncidentTargetTagDef current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<IncidentTargetTagDef>.Enumerator);
					goto IL_00a3;
					IL_00ff:
					if (_003C_003E7__wrap3.MoveNext())
					{
						IncidentTargetTagDef current2 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					_003Ci_003E5__3++;
					goto IL_012b;
					IL_00a3:
					_003Ci_003E5__3 = 0;
					goto IL_012b;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IncidentTargetTagDef> IEnumerable<IncidentTargetTagDef>.GetEnumerator()
		{
			_003CIncidentTargetTags_003Ed__85 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CIncidentTargetTags_003Ed__85(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IncidentTargetTagDef>)this).GetEnumerator();
		}
	}

	public WorldObjectDef def;

	public int ID = -1;

	private PlanetTile tile = PlanetTile.Invalid;

	private Faction factionInt;

	public int creationGameTicks = -1;

	public List<string> questTags;

	private bool destroyed;

	public ThingOwner<Thing> rewards;

	private int tickDelta;

	public bool isGeneratedLocation;

	private List<WorldObjectComp> comps = new List<WorldObjectComp>();

	private Material expandingMaterial;

	private int drawPosCacheTick = -1;

	private Vector3 drawPosCached;

	private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

	private const float BaseDrawSize = 0.7f;

	private const float DrawOffsetRange = 0.01f;

	private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest");

	private List<IThingHolder> tmpHolders;

	private bool cached;

	private bool cachedIsHolder;

	private IThingHolder cachedHolder;

	private IThingHolderTickable cachedTickable;

	public List<WorldObjectComp> AllComps => comps;

	public virtual bool ShowRelatedQuests => true;

	public virtual bool CanReformFoggedEnemies => false;

	public bool Destroyed => destroyed;

	public virtual bool VisibleInBackground
	{
		get
		{
			if (Tile.LayerDef.isSpace)
			{
				return Find.WorldSelector.SelectedLayer.Def.isSpace;
			}
			return false;
		}
	}

	public virtual bool RequiresSignalJammerToReach
	{
		get
		{
			if (def.requiresSignalJammerToReach)
			{
				return !(this is MapParent mapParent) || !mapParent.HasMap;
			}
			return false;
		}
	}

	public virtual float DrawAltitude => 0.03f;

	public PlanetTile Tile
	{
		get
		{
			return tile;
		}
		set
		{
			if (!(tile == value))
			{
				PlanetTile previous = tile;
				tile = value;
				if (Spawned && !def.useDynamicDrawer)
				{
					Find.World.renderer.Notify_StaticWorldObjectPosChanged();
				}
				if (previous.Valid)
				{
					previous.Tile.Layer.FastTileFinder.DirtyTile(previous);
				}
				tile.Tile.Layer.FastTileFinder.DirtyTile(tile);
				PositionChanged(previous, tile);
			}
		}
	}

	public bool Spawned => Find.WorldObjects.Contains(this);

	public virtual Vector3 DrawPos
	{
		get
		{
			if (Find.TickManager.TicksGame != drawPosCacheTick)
			{
				drawPosCached = Tile.Layer.Origin + Find.WorldGrid.GetTileCenter(Tile);
				drawPosCacheTick = Find.TickManager.TicksGame;
			}
			return drawPosCached;
		}
	}

	public Faction Faction => factionInt;

	public virtual string Label => def.label;

	public string LabelCap => Label.CapitalizeFirst(def);

	public virtual string LabelShort => Label;

	public virtual string LabelShortCap => LabelShort.CapitalizeFirst(def);

	public virtual bool HasName => false;

	public virtual Material Material => def.Material;

	public virtual Material ExpandingMaterial
	{
		get
		{
			if (def.expandingShader == null)
			{
				return null;
			}
			if (expandingMaterial != null)
			{
				return expandingMaterial;
			}
			MaterialRequest materialRequest = default(MaterialRequest);
			materialRequest.mainTex = def.ExpandingIconTexture;
			materialRequest.shader = def.expandingShader.Shader;
			materialRequest.color = Color.white;
			materialRequest.maskTex = def.ExpandingIconTextureMask;
			MaterialRequest req = materialRequest;
			return expandingMaterial = MaterialPool.MatFrom(req);
		}
	}

	public virtual bool SelectableNow => def.selectable;

	public virtual bool NeverMultiSelect => def.neverMultiSelect;

	public virtual Texture2D ExpandingIcon
	{
		get
		{
			if (def.ExpandingIconTexture != null)
			{
				return def.ExpandingIconTexture;
			}
			if (Material != null)
			{
				return (Texture2D)Material.mainTexture;
			}
			return BaseContent.BadTex;
		}
	}

	public virtual Color ExpandingIconColor => def.expandingIconColor ?? Material.color;

	public virtual float ExpandingIconPriority => def.expandingIconPriority;

	public virtual bool ExpandMore => def.expandMore;

	public virtual bool AppendFactionToInspectString => true;

	public IThingHolder ParentHolder
	{
		get
		{
			if (!Spawned)
			{
				return null;
			}
			return Find.World;
		}
	}

	public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats => Enumerable.Empty<StatDrawEntry>();

	public virtual BiomeDef Biome
	{
		get
		{
			if (!Spawned)
			{
				return null;
			}
			return Find.WorldGrid[Tile].PrimaryBiome;
		}
	}

	public virtual float ExpandingIconRotation => 0f;

	public virtual bool ExpandingIconFlipHorizontal => false;

	public virtual bool GravShipCanLandOn => false;

	protected virtual int UpdateRateTicks
	{
		get
		{
			if (!WorldRendererUtility.WorldSelected)
			{
				return 15;
			}
			return 1;
		}
	}

	protected virtual int UpdateRateTickOffset => this.HashOffset();

	[IteratorStateMachine(typeof(_003CIncidentTargetTags_003Ed__85))]
	public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CIncidentTargetTags_003Ed__85(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			InitializeComps();
		}
		Scribe_Values.Look(ref tile, "tile");
		Scribe_Values.Look(ref ID, "ID", -1);
		Scribe_Values.Look(ref creationGameTicks, "creationGameTicks", 0);
		Scribe_Values.Look(ref destroyed, "destroyed", defaultValue: false);
		Scribe_Values.Look(ref tickDelta, "tickDelta", 0);
		Scribe_Values.Look(ref isGeneratedLocation, "isGeneratedLocation", defaultValue: false);
		Scribe_References.Look(ref factionInt, "faction");
		Scribe_Collections.Look(ref questTags, "questTags", LookMode.Value);
		if (Scribe.mode != LoadSaveMode.Saving)
		{
			Scribe_Deep.Look(ref rewards, "rewards");
		}
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].PostExposeData();
		}
	}

	private void InitializeComps()
	{
		for (int i = 0; i < def.comps.Count; i++)
		{
			WorldObjectComp worldObjectComp = null;
			try
			{
				worldObjectComp = (WorldObjectComp)Activator.CreateInstance(def.comps[i].compClass);
				worldObjectComp.parent = this;
				comps.Add(worldObjectComp);
				worldObjectComp.Initialize(def.comps[i]);
			}
			catch (Exception ex)
			{
				Log.Error("Could not instantiate or initialize a WorldObjectComp: " + ex);
				comps.Remove(worldObjectComp);
			}
		}
	}

	public virtual void SetFaction(Faction newFaction)
	{
		if (!def.canHaveFaction && newFaction != null)
		{
			Log.Warning("Tried to set faction to " + newFaction?.ToString() + " but this world object (" + this?.ToString() + ") cannot have faction.");
		}
		else
		{
			factionInt = newFaction;
		}
	}

	public virtual string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (ModsConfig.OdysseyActive && def.requiresSignalJammerToReach)
		{
			stringBuilder.Append("RequiresSignalJammer".Translate());
		}
		if (Faction != null && AppendFactionToInspectString)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append($"{"Faction".Translate()}: {Faction.Name}");
		}
		for (int i = 0; i < comps.Count; i++)
		{
			string text = comps[i].CompInspectStringExtra();
			if (text.NullOrEmpty())
			{
				continue;
			}
			if (Prefs.DevMode)
			{
				string text2 = text;
				if (char.IsWhiteSpace(text2[text2.Length - 1]))
				{
					Log.ErrorOnce($"{comps[i].GetType()} CompInspectStringExtra ended with whitespace: {text}", 25612);
					text = text.TrimEndNewlines();
				}
			}
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append(text);
		}
		QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
		return stringBuilder.ToString();
	}

	public void DoTick()
	{
		using (ProfilerBlock.Scope("DoTick()"))
		{
			using (ProfilerBlock.Scope("Tick()"))
			{
				Tick();
			}
			tickDelta++;
			if (tickDelta > UpdateRateTicks || GenTicks.IsTickInterval(UpdateRateTickOffset, UpdateRateTicks))
			{
				using (ProfilerBlock.Scope("TickInterval()"))
				{
					TickInterval(tickDelta);
				}
				tickDelta = 0;
			}
		}
		if (Destroyed)
		{
			return;
		}
		if (!cached)
		{
			cached = true;
			cachedHolder = this as IThingHolder;
			cachedTickable = this as IThingHolderTickable;
			cachedIsHolder = cachedHolder != null;
		}
		if (!cachedIsHolder || (cachedTickable != null && !cachedTickable.ShouldTickContents))
		{
			return;
		}
		if (tmpHolders == null)
		{
			tmpHolders = new List<IThingHolder>(8);
		}
		tmpHolders.Add(cachedHolder);
		cachedHolder.GetChildHolders(tmpHolders);
		for (int i = 0; i < tmpHolders.Count; i++)
		{
			ThingOwner directlyHeldThings = tmpHolders[i].GetDirectlyHeldThings();
			bool flag = directlyHeldThings == null;
			if (!flag)
			{
				IThingHolder owner = directlyHeldThings.Owner;
				bool flag2 = ((owner is Map || owner is Caravan) ? true : false);
				flag = flag2;
			}
			if (!flag)
			{
				directlyHeldThings.DoTick();
				if (Destroyed)
				{
					break;
				}
			}
		}
		tmpHolders.Clear();
	}

	protected virtual void Tick()
	{
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].CompTick();
		}
	}

	protected virtual void TickInterval(int delta)
	{
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].CompTickInterval(delta);
		}
	}

	public virtual void ExtraSelectionOverlaysOnGUI()
	{
	}

	public virtual void DrawExtraSelectionOverlays()
	{
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].PostDrawExtraSelectionOverlays();
		}
	}

	public virtual void PostMake()
	{
		InitializeComps();
	}

	public virtual void PostAdd()
	{
		QuestUtility.SendQuestTargetSignals(questTags, "Spawned", this.Named("SUBJECT"));
	}

	protected virtual void PositionChanged(PlanetTile previous, PlanetTile current)
	{
	}

	public virtual void SpawnSetup()
	{
		if (!def.useDynamicDrawer)
		{
			Find.World.renderer.Notify_StaticWorldObjectPosChanged();
		}
		if (def.useDynamicDrawer)
		{
			Find.WorldDynamicDrawManager.RegisterDrawable(this);
		}
		Tile.Layer.FastTileFinder.DirtyTile(Tile);
	}

	public virtual void PostRemove()
	{
		if (!def.useDynamicDrawer)
		{
			Find.World.renderer.Notify_StaticWorldObjectPosChanged();
		}
		if (def.useDynamicDrawer)
		{
			Find.WorldDynamicDrawManager.DeRegisterDrawable(this);
		}
		Find.WorldSelector.Deselect(this);
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].PostPostRemove();
		}
		QuestUtility.SendQuestTargetSignals(questTags, "Despawned", this.Named("SUBJECT"));
		Tile.Layer.FastTileFinder.DirtyTile(Tile);
	}

	public virtual void Destroy()
	{
		if (Destroyed)
		{
			Log.Error("Tried to destroy already-destroyed world object " + this);
			return;
		}
		if (Spawned)
		{
			Find.WorldObjects.Remove(this);
		}
		destroyed = true;
		Find.FactionManager.Notify_WorldObjectDestroyed(this);
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].PostDestroy();
		}
		QuestUtility.SendQuestTargetSignals(questTags, "Destroyed", this.Named("SUBJECT"));
	}

	public virtual void Print(LayerSubMesh subMesh)
	{
		float averageTileSize = Tile.Layer.AverageTileSize;
		float num = Rand.RangeSeeded(0f, 0.01f, ID) + def.drawAltitudeOffset;
		WorldRendererUtility.PrintQuadTangentialToPlanet(DrawPos, 0.7f * averageTileSize, 0.03f + num, subMesh, counterClockwise: false, Rand.Range(0f, 360f));
	}

	public virtual void Draw()
	{
		float averageTileSize = Tile.Layer.AverageTileSize;
		float rawTransitionPct = ExpandableWorldObjectsUtility.RawTransitionPct;
		if (!Tile.LayerDef.isSpace && (bool)Material)
		{
			float num = Rand.RangeSeeded(0f, 0.01f, ID) + def.drawAltitudeOffset;
			if (def.expandingIcon && rawTransitionPct > 0f && !ExpandableWorldObjectsUtility.HiddenByRules(this))
			{
				Color color = Material.color;
				float num2 = 1f - rawTransitionPct;
				propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * num2));
				WorldRendererUtility.DrawQuadTangentialToPlanet(DrawPos, 0.7f * averageTileSize, DrawAltitude + num, Material, 0f, counterClockwise: false, useSkyboxLayer: false, propertyBlock);
			}
			else
			{
				WorldRendererUtility.DrawQuadTangentialToPlanet(DrawPos, 0.7f * averageTileSize, DrawAltitude + num, Material);
			}
		}
	}

	public T GetComponent<T>() where T : WorldObjectComp
	{
		for (int i = 0; i < comps.Count; i++)
		{
			if (comps[i] is T result)
			{
				return result;
			}
		}
		return null;
	}

	public bool TryGetComponent<T>(out T comp) where T : WorldObjectComp
	{
		comp = GetComponent<T>();
		return comp != null;
	}

	public WorldObjectComp GetComponent(Type type)
	{
		for (int i = 0; i < comps.Count; i++)
		{
			if (type.IsInstanceOfType(comps[i]))
			{
				return comps[i];
			}
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__111))]
	public virtual IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__111(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetCaravanGizmos_003Ed__112))]
	public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCaravanGizmos_003Ed__112(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__caravan = caravan
		};
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__113))]
	public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__113(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__caravan = caravan
		};
	}

	public virtual IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		return Enumerable.Empty<FloatMenuOption>();
	}

	public virtual IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		return Enumerable.Empty<FloatMenuOption>();
	}

	public virtual IEnumerable<InspectTabBase> GetInspectTabs()
	{
		return def.inspectorTabsResolved;
	}

	public virtual bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
	{
		return Faction == other.Faction;
	}

	public override string ToString()
	{
		return GetType().Name + " " + LabelCap + " (tile=" + Tile.ToString() + ")";
	}

	public override int GetHashCode()
	{
		return ID;
	}

	public string GetUniqueLoadID()
	{
		return "WorldObject_" + ID;
	}

	public virtual string GetDescription()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(def.description);
		for (int i = 0; i < comps.Count; i++)
		{
			string descriptionPart = comps[i].GetDescriptionPart();
			if (!descriptionPart.NullOrEmpty())
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(descriptionPart);
			}
		}
		return stringBuilder.ToString();
	}
}
