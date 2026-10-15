using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Utility;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_Reload : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public FloatMenuContext context;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_1
	{
		public IReloadableComp reloadable;

		public List<Thing> chosenAmmo;

		public _003C_003Ec__DisplayClass6_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetOptionsFor_003Eb__0()
		{
			CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobGiver_Reload.MakeReloadJob(reloadable, chosenAmmo), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__6 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		public FloatMenuOptionProvider_Reload _003C_003E4__this;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

		private IEnumerator<IReloadableComp> _003C_003E7__wrap1;

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
		public _003CGetOptionsFor_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 4u)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				FloatMenuOptionProvider_Reload floatMenuOptionProvider_Reload = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
					_003C_003E8__1.context = context;
					_003C_003E7__wrap1 = floatMenuOptionProvider_Reload.GetReloadablesUsingAmmo(_003C_003E8__1.context.FirstSelectedPawn, clickedThing).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				case 3:
					_003C_003E1__state = -3;
					break;
				case 4:
					_003C_003E1__state = -3;
					break;
				case 5:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass6_1 CS_0024_003C_003E8__locals16 = new _003C_003Ec__DisplayClass6_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						reloadable = _003C_003E7__wrap1.Current
					};
					ThingComp thingComp = CS_0024_003C_003E8__locals16.reloadable as ThingComp;
					string text = "Reload".Translate(thingComp.parent.Named("GEAR"), NamedArgumentUtility.Named(CS_0024_003C_003E8__locals16.reloadable.AmmoDef, "AMMO")) + " (" + CS_0024_003C_003E8__locals16.reloadable.LabelRemaining + ")";
					if (!CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly))
					{
						_003C_003E2__current = new FloatMenuOption(text + ": " + "NoPath".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 1;
						return true;
					}
					if (!CS_0024_003C_003E8__locals16.reloadable.NeedsReload(allowForceReload: true))
					{
						_003C_003E2__current = new FloatMenuOption(text + ": " + "ReloadFull".Translate(), null);
						_003C_003E1__state = 2;
						return true;
					}
					if ((CS_0024_003C_003E8__locals16.chosenAmmo = ReloadableUtility.FindEnoughAmmo(CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, clickedThing.Position, CS_0024_003C_003E8__locals16.reloadable, forceReload: true)) == null)
					{
						_003C_003E2__current = new FloatMenuOption(text + ": " + "ReloadNotEnough".Translate(), null);
						_003C_003E1__state = 3;
						return true;
					}
					if (CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.carryTracker.AvailableStackSpace(CS_0024_003C_003E8__locals16.reloadable.AmmoDef) < CS_0024_003C_003E8__locals16.reloadable.MinAmmoNeeded(allowForcedReload: true))
					{
						_003C_003E2__current = new FloatMenuOption(text + ": " + "ReloadCannotCarryEnough".Translate(NamedArgumentUtility.Named(CS_0024_003C_003E8__locals16.reloadable.AmmoDef, "AMMO")), null);
						_003C_003E1__state = 4;
						return true;
					}
					Action action = delegate
					{
						CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobGiver_Reload.MakeReloadJob(CS_0024_003C_003E8__locals16.reloadable, CS_0024_003C_003E8__locals16.chosenAmmo), JobTag.Misc);
					};
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action), CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, clickedThing);
					_003C_003E1__state = 5;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__6 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetOptionsFor_003Ed__.clickedThing = _003C_003E3__clickedThing;
			_003CGetOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetReloadablesUsingAmmo_003Ed__7 : IEnumerable<IReloadableComp>, IEnumerable, IEnumerator<IReloadableComp>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IReloadableComp _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		private List<Apparel>.Enumerator _003C_003E7__wrap1;

		IReloadableComp IEnumerator<IReloadableComp>.Current
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
		public _003CGetReloadablesUsingAmmo_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<Apparel>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (pawn.equipment?.PrimaryEq != null && pawn.equipment.PrimaryEq is IReloadableComp reloadableComp && clickedThing.def == reloadableComp.AmmoDef)
					{
						_003C_003E2__current = reloadableComp;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0088;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0088;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_00fe;
					}
					IL_00fe:
					while (_003C_003E7__wrap1.MoveNext())
					{
						IReloadableComp reloadableComp2 = _003C_003E7__wrap1.Current.TryGetComp<CompApparelReloadable>();
						if (reloadableComp2 != null && clickedThing.def == reloadableComp2.AmmoDef)
						{
							_003C_003E2__current = reloadableComp2;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Apparel>.Enumerator);
					break;
					IL_0088:
					if (pawn.apparel == null)
					{
						break;
					}
					_003C_003E7__wrap1 = pawn.apparel.WornApparel.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00fe;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IReloadableComp> IEnumerable<IReloadableComp>.GetEnumerator()
		{
			_003CGetReloadablesUsingAmmo_003Ed__7 _003CGetReloadablesUsingAmmo_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetReloadablesUsingAmmo_003Ed__ = this;
			}
			else
			{
				_003CGetReloadablesUsingAmmo_003Ed__ = new _003CGetReloadablesUsingAmmo_003Ed__7(0);
			}
			_003CGetReloadablesUsingAmmo_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetReloadablesUsingAmmo_003Ed__.clickedThing = _003C_003E3__clickedThing;
			return _003CGetReloadablesUsingAmmo_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IReloadableComp>)this).GetEnumerator();
		}
	}

	protected override bool Drafted => true;

	protected override bool Undrafted => true;

	protected override bool Multiselect => false;

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__6))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}

	[IteratorStateMachine(typeof(_003CGetReloadablesUsingAmmo_003Ed__7))]
	private IEnumerable<IReloadableComp> GetReloadablesUsingAmmo(Pawn pawn, Thing clickedThing)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetReloadablesUsingAmmo_003Ed__7(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__clickedThing = clickedThing
		};
	}
}
