using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse;

public class Pawn_CarryTracker : IThingHolder, IExposable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass22_0
	{
		public CompDissolution compDissolution;

		internal void _003CGetGizmos_003Eb__1()
		{
			compDissolution.TriggerDissolutionEvent();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__22 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_CarryTracker _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__22(int _003C_003E1__state)
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
			Pawn_CarryTracker CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			_003C_003Ec__DisplayClass22_0 CS_0024_003C_003E8__locals11;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Gizmo gizmo = ContainingSelectionUtility.SelectCarriedThingGizmo(CS_0024_003C_003E8__locals8.pawn, CS_0024_003C_003E8__locals8.CarriedThing);
				if (gizmo != null)
				{
					_003C_003E2__current = gizmo;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0059;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_0059;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00e2;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0059:
				if (CS_0024_003C_003E8__locals8.pawn.Drafted && CS_0024_003C_003E8__locals8.CarriedThing is Pawn)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandDropPawn".Translate(CS_0024_003C_003E8__locals8.CarriedThing),
						defaultDesc = "CommandDropPawnDesc".Translate(),
						action = delegate
						{
							CS_0024_003C_003E8__locals8.pawn.carryTracker.TryDropCarriedThing(CS_0024_003C_003E8__locals8.pawn.Position, ThingPlaceMode.Near, out var _);
						},
						icon = TexCommand.DropCarriedPawn
					};
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_00e2;
				IL_00e2:
				if (!ModsConfig.BiotechActive || !DebugSettings.ShowDevGizmos)
				{
					break;
				}
				CS_0024_003C_003E8__locals11 = new _003C_003Ec__DisplayClass22_0
				{
					compDissolution = CS_0024_003C_003E8__locals8.CarriedThing.TryGetComp<CompDissolution>()
				};
				if (CS_0024_003C_003E8__locals11.compDissolution != null)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Dissolution event",
						action = delegate
						{
							CS_0024_003C_003E8__locals11.compDissolution.TriggerDissolutionEvent();
						}
					};
					_003C_003E1__state = 3;
					return true;
				}
				break;
			}
			return false;
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
			_003CGetGizmos_003Ed__22 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__22(0)
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

	public Pawn pawn;

	public ThingOwner<Thing> innerContainer;

	public Thing CarriedThing
	{
		get
		{
			if (innerContainer.Count == 0)
			{
				return null;
			}
			return innerContainer[0];
		}
	}

	public bool Full => AvailableStackSpace(CarriedThing.def) <= 0;

	public IThingHolder ParentHolder => pawn;

	public Pawn_CarryTracker(Pawn pawn)
	{
		this.pawn = pawn;
		innerContainer = new ThingOwner<Thing>(this, oneStackOnly: true);
	}

	public void ExposeData()
	{
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public int AvailableStackSpace(ThingDef td)
	{
		int num = MaxStackSpaceEver(td);
		if (CarriedThing != null)
		{
			num -= CarriedThing.stackCount;
		}
		return num;
	}

	public int MaxStackSpaceEver(ThingDef td)
	{
		int b = Mathf.RoundToInt(pawn.GetStatValue(StatDefOf.CarryingCapacity) / td.VolumePerUnit);
		return Mathf.Min(td.stackLimit, b);
	}

	public bool TryStartCarry(Thing item)
	{
		if (pawn.Dead || pawn.Downed)
		{
			Log.Error("Dead/downed/deathresting pawn " + pawn?.ToString() + " tried to start carry item.");
			return false;
		}
		if (innerContainer.TryAdd(item))
		{
			item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, pawn.Map));
			return true;
		}
		return false;
	}

	public int TryStartCarry(Thing item, int count, bool reserve = true)
	{
		if (pawn.Dead || pawn.Downed)
		{
			Log.Error("Dead/downed/deathresting pawn " + pawn?.ToString() + " tried to start carry " + item.ToStringSafe());
			return 0;
		}
		count = Mathf.Min(count, AvailableStackSpace(item.def));
		count = Mathf.Min(count, item.stackCount);
		bool flag = Find.Selector.IsSelected(item);
		Thing thing = item.SplitOff(count);
		int num = innerContainer.TryAdd(thing, count);
		if (num > 0 && thing != item)
		{
			TryUpdateTransferables(thing);
		}
		if (num > 0)
		{
			item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, pawn.Map));
			if (reserve)
			{
				pawn.Reserve(CarriedThing, pawn.CurJob);
			}
			if (flag)
			{
				if (!thing.Destroyed)
				{
					Find.Selector.Select(thing);
				}
				Find.Selector.Select(CarriedThing);
			}
			pawn.MapHeld.resourceCounter.UpdateResourceCounts();
		}
		return num;
	}

	private void TryUpdateTransferables(Thing splitStack)
	{
		if (splitStack != null && pawn.jobs?.curDriver is JobDriver_HaulToTransporter jobDriver_HaulToTransporter)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching(splitStack, jobDriver_HaulToTransporter.Transporter?.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay != null && !transferableOneWay.things.Contains(splitStack) && transferableOneWay.MaxCount + splitStack.stackCount <= transferableOneWay.CountToTransfer)
			{
				transferableOneWay.things.Add(splitStack);
			}
		}
	}

	public bool TryDropCarriedThing(IntVec3 dropLoc, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
	{
		if (innerContainer.TryDrop(CarriedThing, dropLoc, pawn.MapHeld, mode, out resultingThing, placedAction))
		{
			if (resultingThing != null && pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				resultingThing.SetForbidden(value: true, warnOnFail: false);
			}
			pawn.MapHeld.resourceCounter.UpdateResourceCounts();
			return true;
		}
		return false;
	}

	public bool TryDropCarriedThing(IntVec3 dropLoc, int count, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
	{
		if (innerContainer.TryDrop(CarriedThing, dropLoc, pawn.MapHeld, mode, count, out resultingThing, placedAction))
		{
			if (resultingThing != null && pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				resultingThing.SetForbidden(value: true, warnOnFail: false);
			}
			pawn.MapHeld.resourceCounter.UpdateResourceCounts();
			return true;
		}
		return false;
	}

	public int CarriedCount(ThingDef def)
	{
		int num = 0;
		foreach (Thing item in innerContainer)
		{
			if (item.def == def)
			{
				num += item.stackCount;
			}
		}
		return num;
	}

	public void DestroyCarriedThing()
	{
		innerContainer.ClearAndDestroyContents();
	}

	public void CarryHandsTickInterval(int delta)
	{
		if (CarriedThing is Pawn pawn && pawn.DevelopmentalStage.Baby())
		{
			pawn.ideo?.IncreaseIdeoExposureIfBabyTick(this.pawn.Ideo, delta);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__22))]
	public IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__22(-2)
		{
			_003C_003E4__this = this
		};
	}
}
