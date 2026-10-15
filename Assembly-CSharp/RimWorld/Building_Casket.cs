using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Building_Casket : Building, IThingHolder, IOpenable, ISearchableContents
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__17 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Casket _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__17(int _003C_003E1__state)
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
				Building_Casket building_Casket = _003C_003E4__this;
				Gizmo gizmo;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)building_Casket).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00bb;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					gizmo = Building.SelectContainedItemGizmo(building_Casket, building_Casket.ContainedThing);
					if (gizmo != null)
					{
						_003C_003E2__current = gizmo;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00bb;
					IL_00bb:
					if (DebugSettings.ShowDevGizmos && building_Casket.CanOpen)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Open",
							action = building_Casket.Open
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
			_003CGetGizmos_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__17(0)
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

	protected ThingOwner innerContainer;

	protected bool contentsKnown;

	public string openedSignal;

	public virtual int OpenTicks => 300;

	public bool HasAnyContents => innerContainer.Count > 0;

	public Thing ContainedThing
	{
		get
		{
			if (innerContainer.Count != 0)
			{
				return innerContainer[0];
			}
			return null;
		}
	}

	public virtual bool CanOpen => HasAnyContents;

	public ThingOwner SearchableContents => innerContainer;

	public Building_Casket()
	{
		innerContainer = new ThingOwner<Thing>(this, oneStackOnly: false);
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public virtual void Open()
	{
		if (HasAnyContents)
		{
			EjectContents();
			if (!openedSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(openedSignal, this.Named("SUBJECT")));
			}
			DirtyMapMesh(base.Map);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__17))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_Values.Look(ref contentsKnown, "contentsKnown", defaultValue: false);
		Scribe_Values.Look(ref openedSignal, "openedSignal");
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (base.Faction != null && base.Faction.IsPlayer)
		{
			contentsKnown = true;
		}
	}

	public override AcceptanceReport ClaimableBy(Faction fac)
	{
		if (innerContainer.Any && !contentsKnown)
		{
			return false;
		}
		return base.ClaimableBy(fac);
	}

	public virtual bool Accepts(Thing thing)
	{
		return innerContainer.CanAcceptAnyOf(thing);
	}

	public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
	{
		if (!Accepts(thing))
		{
			return false;
		}
		bool flag;
		if (thing.holdingOwner != null)
		{
			thing.holdingOwner.TryTransferToContainer(thing, innerContainer, thing.stackCount);
			flag = true;
		}
		else
		{
			flag = innerContainer.TryAdd(thing);
		}
		if (flag)
		{
			if (thing.Faction != null && thing.Faction.IsPlayer)
			{
				contentsKnown = true;
			}
			return true;
		}
		return false;
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		Map map = base.Map;
		base.Destroy(mode);
		if (innerContainer.Count > 0 && (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize))
		{
			if (mode != DestroyMode.Deconstruct)
			{
				List<Pawn> list = new List<Pawn>();
				foreach (Thing item2 in (IEnumerable<Thing>)innerContainer)
				{
					if (item2 is Pawn item)
					{
						list.Add(item);
					}
				}
				foreach (Pawn item3 in list)
				{
					HealthUtility.DamageUntilDowned(item3);
				}
			}
			innerContainer.TryDropAll(base.Position, map, ThingPlaceMode.Near);
		}
		innerContainer.ClearAndDestroyContents();
	}

	public virtual void EjectContents()
	{
		innerContainer.TryDropAll(InteractionCell, base.Map, ThingPlaceMode.Near);
		contentsKnown = true;
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		string str = (contentsKnown ? innerContainer.ContentsString : ((string)"UnknownLower".Translate()));
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		return text + ("CasketContains".Translate() + ": " + str.CapitalizeFirst());
	}
}
