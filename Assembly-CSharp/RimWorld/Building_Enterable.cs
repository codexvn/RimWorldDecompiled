using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class Building_Enterable : Building, IThingHolder, ISuspendableThingHolder, ISearchableContents
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__24 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Enterable _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private IEnumerator<Thing> _003C_003E7__wrap2;

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
		public _003CGetGizmos_003Ed__24(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_Enterable building_Enterable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)building_Enterable).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = ((IEnumerable<Thing>)building_Enterable.innerContainer).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					Thing current2 = _003C_003E7__wrap2.Current;
					Gizmo gizmo = Building.SelectContainedItemGizmo(building_Enterable, current2);
					if (gizmo != null)
					{
						_003C_003E2__current = gizmo;
						_003C_003E1__state = 2;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
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

		private void _003C_003Em__Finally2()
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
			_003CGetGizmos_003Ed__24 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__24(0)
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

	public ThingOwner innerContainer;

	protected Pawn selectedPawn;

	protected int startTick = -1;

	public bool Working => startTick >= 0;

	public virtual bool IsContentsSuspended => true;

	public bool AnyAcceptablePawns
	{
		get
		{
			if (!base.Spawned)
			{
				return false;
			}
			return base.Map.mapPawns.AllPawnsSpawned.Any((Pawn x) => CanAcceptPawn(x));
		}
	}

	public ThingOwner SearchableContents => innerContainer;

	public Pawn SelectedPawn
	{
		get
		{
			return selectedPawn;
		}
		set
		{
			selectedPawn = value;
		}
	}

	public abstract Vector3 PawnDrawOffset { get; }

	public Building_Enterable()
	{
		innerContainer = new ThingOwner<Thing>(this);
	}

	public abstract AcceptanceReport CanAcceptPawn(Pawn p);

	public abstract void TryAcceptPawn(Pawn p);

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode != DestroyMode.WillReplace)
		{
			innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near);
		}
		base.DeSpawn(mode);
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	protected virtual void SelectPawn(Pawn pawn)
	{
		selectedPawn = pawn;
		if (!pawn.IsPrisonerOfColony && !pawn.Downed)
		{
			pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.EnterBuilding, this), JobTag.Misc);
		}
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (!Working && selectedPawn != null && selectedPawn.Map == base.Map)
		{
			GenDraw.DrawLineBetween(this.TrueCenter(), selectedPawn.TrueCenter());
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__24))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__24(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_Values.Look(ref startTick, "startTick", -1);
		Scribe_References.Look(ref selectedPawn, "selectedPawn");
	}
}
