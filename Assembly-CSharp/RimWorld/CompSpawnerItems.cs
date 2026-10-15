using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompSpawnerItems : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__5 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompSpawnerItems _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompSpawnerItems CS_0024_003C_003E8__locals1 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Spawn items",
					action = delegate
					{
						CS_0024_003C_003E8__locals1.SpawnItems();
					}
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
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
			_003CCompGetGizmosExtra_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__5(0)
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

	private int ticksPassed;

	public CompProperties_SpawnerItems Props => (CompProperties_SpawnerItems)props;

	public bool Active => parent.GetComp<CompCanBeDormant>()?.Awake ?? true;

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__5))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void SpawnItems()
	{
		if (Props.MatchingItems.TryRandomElement(out var result))
		{
			int stackCount = Mathf.CeilToInt(Props.approxMarketValuePerDay / result.BaseMarketValue);
			Thing thing = ThingMaker.MakeThing(result);
			thing.stackCount = stackCount;
			GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near, null, null, null);
		}
	}

	public override void CompTickRare()
	{
		if (Active)
		{
			ticksPassed += 250;
			if (ticksPassed >= Props.spawnInterval)
			{
				SpawnItems();
				ticksPassed -= Props.spawnInterval;
			}
		}
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref ticksPassed, "ticksPassed", 0);
	}

	public override string CompInspectStringExtra()
	{
		if (Active)
		{
			return "NextSpawnedResourceIn".Translate() + ": " + (Props.spawnInterval - ticksPassed).ToStringTicksToPeriod();
		}
		return null;
	}
}
