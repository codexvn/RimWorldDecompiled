using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class Building_ArchonexusCore : Building
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__3 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_ArchonexusCore _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__3(int _003C_003E1__state)
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
				Building_ArchonexusCore building_ArchonexusCore = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)building_ArchonexusCore).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
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
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							action = building_ArchonexusCore.Activate,
							Disabled = !building_ArchonexusCore.CanActivateNow,
							defaultLabel = "DEV: Activate archonexus core"
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
			_003CGetGizmos_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__3(0)
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
	private sealed class _003CGetMultiSelectFloatMenuOptions_003Ed__7 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_ArchonexusCore _003C_003E4__this;

		private IEnumerable<Pawn> selPawns;

		public IEnumerable<Pawn> _003C_003E3__selPawns;

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
		public _003CGetMultiSelectFloatMenuOptions_003Ed__7(int _003C_003E1__state)
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
			Building_ArchonexusCore CS_0024_003C_003E8__locals17 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals17.CanActivateNow)
				{
					_003C_003E2__current = new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "AlreadyInvoked".Translate(), null);
					_003C_003E1__state = 1;
					return true;
				}
				CS_0024_003C_003E8__locals17.tmpPawnsCanReach.Clear();
				foreach (Pawn item in selPawns)
				{
					if (item.CanReach(CS_0024_003C_003E8__locals17, PathEndMode.InteractionCell, Danger.Deadly))
					{
						CS_0024_003C_003E8__locals17.tmpPawnsCanReach.Add(item);
					}
				}
				if (CS_0024_003C_003E8__locals17.tmpPawnsCanReach.NullOrEmpty())
				{
					_003C_003E2__current = new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = new FloatMenuOption("Invoke".Translate("Power".Translate()), delegate
				{
					CS_0024_003C_003E8__locals17.tmpPawnsCanReach[0].jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.ActivateArchonexusCore, CS_0024_003C_003E8__locals17), JobTag.Misc);
					CS_0024_003C_003E8__locals17.tmpObservationSpots.Clear();
					IntVec3 intVec = CS_0024_003C_003E8__locals17.Position + ((CS_0024_003C_003E8__locals17.InteractionCell - CS_0024_003C_003E8__locals17.Position).ToVector3().normalized * 8f).ToIntVec3();
					for (int i = 1; i < CS_0024_003C_003E8__locals17.tmpPawnsCanReach.Count; i++)
					{
						IntVec3 intVec2 = RCellFinder.BestOrderedGotoDestNear(intVec, CS_0024_003C_003E8__locals17.tmpPawnsCanReach[i], (IntVec3 c) => !CS_0024_003C_003E8__locals17.tmpObservationSpots.Contains(c));
						FloatMenuOptionProvider_DraftedMove.PawnGotoAction(intVec, CS_0024_003C_003E8__locals17.tmpPawnsCanReach[i], intVec2);
						CS_0024_003C_003E8__locals17.tmpObservationSpots.Add(intVec2);
					}
					CS_0024_003C_003E8__locals17.tmpObservationSpots.Clear();
				});
				_003C_003E1__state = 3;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetMultiSelectFloatMenuOptions_003Ed__7 _003CGetMultiSelectFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetMultiSelectFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetMultiSelectFloatMenuOptions_003Ed__ = new _003CGetMultiSelectFloatMenuOptions_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetMultiSelectFloatMenuOptions_003Ed__.selPawns = _003C_003E3__selPawns;
			return _003CGetMultiSelectFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private const int ObservationDistance = 8;

	private List<Pawn> tmpPawnsCanReach = new List<Pawn>();

	private List<IntVec3> tmpObservationSpots = new List<IntVec3>();

	public bool CanActivateNow => !ArchonexusCountdown.CountdownActivated;

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__3))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void Activate()
	{
		if (CanActivateNow)
		{
			ArchonexusCountdown.InitiateCountdown(this);
		}
	}

	[IteratorStateMachine(typeof(_003CGetMultiSelectFloatMenuOptions_003Ed__7))]
	public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetMultiSelectFloatMenuOptions_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawns = selPawns
		};
	}
}
