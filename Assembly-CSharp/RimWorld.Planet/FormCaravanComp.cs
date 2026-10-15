using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.Planet;

[StaticConstructorOnStartup]
public class FormCaravanComp : WorldObjectComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass18_0
	{
		public MapParent mapParent;

		internal void _003CGetGizmos_003Eb__0()
		{
			if (ModsConfig.OdysseyActive && mapParent.Map.listerThings.ThingsOfDef(ThingDefOf.GravEngine).Any())
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmLoseGravship".Translate(), Form));
			}
			else if (ModsConfig.OdysseyActive && mapParent.Map.listerThings.ThingsInGroup(ThingRequestGroup.PassengerShuttle).Any())
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmLoseShuttle".Translate(), Form));
			}
			else
			{
				Form();
			}
			void Form()
			{
				Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, reform: true, null, mapAboutToBeRemoved: false, null));
			}
		}

		internal void _003CGetGizmos_003Eb__2()
		{
			foreach (PlanetTile item in CaravanExitMapUtility.AvailableExitTilesAt(mapParent.Map))
			{
				Find.WorldDebugDrawer.FlashTile(item, 0f, null, 10);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__18 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FormCaravanComp _003C_003E4__this;

		private _003C_003Ec__DisplayClass18_0 _003C_003E8__1;

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
		public _003CGetGizmos_003Ed__18(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			FormCaravanComp formCaravanComp = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass18_0();
				_003C_003E8__1.mapParent = (MapParent)formCaravanComp.parent;
				if (!_003C_003E8__1.mapParent.HasMap)
				{
					break;
				}
				if (formCaravanComp.Reform && formCaravanComp.CanFormOrReformCaravanNow && _003C_003E8__1.mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "CommandReformCaravan".Translate(),
						defaultDesc = "CommandReformCaravanDesc".Translate(),
						icon = FormCaravanCommand,
						hotKey = KeyBindingDefOf.Misc2,
						tutorTag = "ReformCaravan",
						action = delegate
						{
							if (ModsConfig.OdysseyActive && _003C_003E8__1.mapParent.Map.listerThings.ThingsOfDef(ThingDefOf.GravEngine).Any())
							{
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmLoseGravship".Translate(), Form));
							}
							else if (ModsConfig.OdysseyActive && _003C_003E8__1.mapParent.Map.listerThings.ThingsInGroup(ThingRequestGroup.PassengerShuttle).Any())
							{
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmLoseShuttle".Translate(), Form));
							}
							else
							{
								Form();
							}
						}
					};
					if (GenHostility.AnyHostileActiveThreatToPlayer(_003C_003E8__1.mapParent.Map, countDormantPawnsAsHostile: true))
					{
						command_Action.Disable("CommandReformCaravanFailHostilePawns".Translate());
					}
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0141;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0141;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0141:
				if (!DebugSettings.ShowDevGizmos)
				{
					break;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Show available exits",
					action = delegate
					{
						foreach (PlanetTile item in CaravanExitMapUtility.AvailableExitTilesAt(_003C_003E8__1.mapParent.Map))
						{
							Find.WorldDebugDrawer.FlashTile(item, 0f, null, 10);
						}
					}
				};
				_003C_003E1__state = 2;
				return true;
			}
			return false;
			void Form()
			{
				Find.WindowStack.Add(new Dialog_FormCaravan(((_003C_003Ec__DisplayClass18_0)this).mapParent.Map, reform: true, null, mapAboutToBeRemoved: false, null));
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__18(0)
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

	public CellRect? foggedRoomsCheckRect;

	public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan");

	private bool anyActiveThreatLastTick;

	public WorldObjectCompProperties_FormCaravan Props => (WorldObjectCompProperties_FormCaravan)props;

	private MapParent MapParent => (MapParent)parent;

	public bool Reform
	{
		get
		{
			if (MapParent.HasMap)
			{
				return !MapParent.Map.IsPlayerHome;
			}
			return true;
		}
	}

	public bool CanFormOrReformCaravanNow
	{
		get
		{
			MapParent mapParent = MapParent;
			if (!mapParent.HasMap)
			{
				return false;
			}
			if (Reform)
			{
				if (AnyActiveThreatNow)
				{
					return false;
				}
				List<Pawn> freeColonistsSpawned = mapParent.Map.mapPawns.FreeColonistsSpawned;
				for (int i = 0; i < freeColonistsSpawned.Count; i++)
				{
					MentalState mentalState = freeColonistsSpawned[i].MentalState;
					if (mentalState != null && mentalState.def.category == MentalStateCategory.Aggro)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public bool AnyActiveThreatNow
	{
		get
		{
			if (MapParent.HasMap)
			{
				return GenHostility.AnyHostileActiveThreatToPlayer(MapParent.Map, countDormantPawnsAsHostile: true, !parent.CanReformFoggedEnemies);
			}
			return false;
		}
	}

	public bool AnyUnexploredFoggedRooms
	{
		get
		{
			if (!foggedRoomsCheckRect.HasValue)
			{
				return false;
			}
			MapParent mapParent = MapParent;
			if (!mapParent.HasMap)
			{
				return false;
			}
			IReadOnlyList<Room> allRooms = mapParent.Map.regionGrid.AllRooms;
			CellRect value = foggedRoomsCheckRect.Value;
			for (int i = 0; i < allRooms.Count; i++)
			{
				if (!allRooms[i].Fogged || !allRooms[i].ProperRoom)
				{
					continue;
				}
				foreach (IntVec3 cell in allRooms[i].Cells)
				{
					if (value.Contains(cell))
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public override void CompTickInterval(int delta)
	{
		base.CompTickInterval(delta);
		bool anyActiveThreatNow = AnyActiveThreatNow;
		if (!anyActiveThreatNow && anyActiveThreatLastTick && Reform && CanFormOrReformCaravanNow)
		{
			if (AnyUnexploredFoggedRooms)
			{
				Messages.Message("MessageCanReformCaravanNowNoMoreEnemiesButUnexploredAreas".Translate(), new LookTargets(parent), MessageTypeDefOf.SituationResolved, historical: false);
			}
			else
			{
				Messages.Message("MessageCanReformCaravanNowNoMoreEnemies".Translate(), new LookTargets(parent), MessageTypeDefOf.NeutralEvent, historical: false);
			}
		}
		anyActiveThreatLastTick = anyActiveThreatNow;
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref anyActiveThreatLastTick, "anyActiveThreatLastTick", defaultValue: false);
		Scribe_Values.Look(ref foggedRoomsCheckRect, "foggedRoomsCheckRect", null);
	}

	public bool CanReformNow()
	{
		MapParent mapParent = (MapParent)parent;
		if (MapParent.HasMap && Reform && CanFormOrReformCaravanNow)
		{
			return mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__18))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__18(-2)
		{
			_003C_003E4__this = this
		};
	}
}
