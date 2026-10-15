using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_WorkTableAutonomous : Building_WorkTable, IThingHolder, INotifyHauledTo
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__21 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_WorkTableAutonomous _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__21(int _003C_003E1__state)
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
				Building_WorkTableAutonomous CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				Bill_Autonomous activeBill;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals5).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							CS_0024_003C_003E8__locals5.ActiveBill.formingTicks = 0f;
						},
						defaultLabel = "DEV: Complete cycle"
					};
					_003C_003E1__state = 3;
					return true;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!DebugSettings.ShowDevGizmos)
					{
						break;
					}
					activeBill = CS_0024_003C_003E8__locals5.ActiveBill;
					if (activeBill != null && activeBill.State == FormingState.Forming)
					{
						_003C_003E2__current = new Command_Action
						{
							action = delegate
							{
								CS_0024_003C_003E8__locals5.ActiveBill.formingTicks -= (float)CS_0024_003C_003E8__locals5.ActiveBill.recipe.formingTicks * 0.25f;
							},
							defaultLabel = "DEV: Forming cycle +25%"
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__21(0)
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

	protected Bill_Autonomous activeBill;

	public Bill_Autonomous ActiveBill
	{
		get
		{
			return activeBill;
		}
		set
		{
			if (activeBill != value)
			{
				activeBill = value;
			}
		}
	}

	public float CurrentBillFormingPercent
	{
		get
		{
			if (activeBill == null || activeBill.State != FormingState.Forming)
			{
				return 0f;
			}
			return 1f - activeBill.formingTicks / (float)activeBill.recipe.formingTicks;
		}
	}

	public GenDraw.FillableBarRequest BarDrawData => def.building.BarDrawDataFor(base.Rotation);

	public Building_WorkTableAutonomous()
	{
		innerContainer = new ThingOwner<Thing>(this);
	}

	public virtual void Notify_StartForming(Pawn billDoer)
	{
	}

	public virtual void Notify_FormingCompleted()
	{
		Thing thing = activeBill.CreateProducts();
		innerContainer.ClearAndDestroyContents();
		if (thing != null)
		{
			innerContainer.TryAdd(thing);
		}
	}

	public override void Notify_BillDeleted(Bill bill)
	{
		if (activeBill == bill)
		{
			EjectContents();
			activeBill = null;
		}
	}

	public virtual void Notify_HauledTo(Pawn hauler, Thing thing, int count)
	{
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		activeBill?.Reset();
		EjectContents();
		base.Destroy(mode);
	}

	public virtual void EjectContents()
	{
		innerContainer.TryDropAll(InteractionCell, base.Map, ThingPlaceMode.Near);
		activeBill?.Reset();
	}

	public virtual bool CanWork()
	{
		return true;
	}

	protected override void Tick()
	{
		base.Tick();
		if (activeBill != null && CanWork())
		{
			activeBill.BillTick();
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_References.Look(ref activeBill, "activeBill");
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		string inspectString = base.GetInspectString();
		if (!inspectString.NullOrEmpty())
		{
			stringBuilder.AppendLine(inspectString);
		}
		string inspectStringExtra = GetInspectStringExtra();
		if (!inspectStringExtra.NullOrEmpty())
		{
			stringBuilder.AppendLine(inspectStringExtra);
		}
		if (CanWork() && activeBill != null)
		{
			activeBill.AppendInspectionData(stringBuilder);
		}
		return stringBuilder.ToString().TrimEndNewlines();
	}

	protected virtual string GetInspectStringExtra()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__21))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}
}
