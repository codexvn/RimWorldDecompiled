using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_SpawnWorldObject : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__5 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_SpawnWorldObject _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__5(int _003C_003E1__state)
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
				QuestPart_SpawnWorldObject questPart_SpawnWorldObject = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_SpawnWorldObject).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_SpawnWorldObject.worldObject != null)
					{
						_003C_003E2__current = questPart_SpawnWorldObject.worldObject;
						_003C_003E1__state = 2;
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public string inSignal;

	public WorldObject worldObject;

	public List<ThingDef> defsToExcludeFromHyperlinks;

	private bool spawned;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__5(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override bool IncreasesPopulation
	{
		get
		{
			if (worldObject is Site { IncreasesPopulation: not false })
			{
				return true;
			}
			return false;
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (!(signal.tag == inSignal) || spawned)
		{
			return;
		}
		PlanetTile result = worldObject.Tile;
		if (!result.Valid)
		{
			if (!TileFinder.TryFindNewSiteTile(out result))
			{
				result = PlanetTile.Invalid;
			}
		}
		else if (Find.WorldObjects.AnyWorldObjectAt(result) && !TileFinder.TryFindPassableTileWithTraversalDistance(result, 1, 50, out result, (PlanetTile x) => !Find.WorldObjects.AnyWorldObjectAt(x), ignoreFirstTilePassability: false, TileFinderMode.Near))
		{
			result = PlanetTile.Invalid;
		}
		if (result.Valid)
		{
			worldObject.Tile = result;
			Find.WorldObjects.Add(worldObject);
			spawned = true;
		}
	}

	public override void PostQuestAdded()
	{
		base.PostQuestAdded();
		if (!(worldObject is Site site))
		{
			return;
		}
		for (int i = 0; i < site.parts.Count; i++)
		{
			if (site.parts[i].things == null)
			{
				continue;
			}
			for (int j = 0; j < site.parts[i].things.Count; j++)
			{
				if (site.parts[i].things[j].def == ThingDefOf.PsychicAmplifier)
				{
					Find.History.Notify_PsylinkAvailable();
					return;
				}
			}
		}
	}

	public override void Cleanup()
	{
		if (!spawned)
		{
			worldObject.Destroy();
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.Saving && spawned && worldObject != null && (worldObject.Destroyed || !worldObject.Spawned))
		{
			worldObject = null;
		}
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Values.Look(ref spawned, "spawned", defaultValue: false);
		Scribe_Collections.Look(ref defsToExcludeFromHyperlinks, "defsToExcludeFromHyperlinks", LookMode.Def);
		if (spawned)
		{
			Scribe_References.Look(ref worldObject, "worldObject");
		}
		else
		{
			Scribe_Deep.Look(ref worldObject, "worldObject");
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		if (TileFinder.TryFindNewSiteTile(out var tile))
		{
			worldObject = SiteMaker.MakeSite((SitePartDef)null, tile, (Faction)null, ifHostileThenMustRemainHostile: true, (float?)null, (WorldObjectDef)null);
		}
	}
}
