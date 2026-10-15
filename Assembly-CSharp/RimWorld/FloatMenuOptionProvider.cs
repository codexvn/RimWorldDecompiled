using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003CGetOptions_003Ed__21 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FloatMenuOptionProvider _003C_003E4__this;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

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
		public _003CGetOptions_003Ed__21(int _003C_003E1__state)
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
			FloatMenuOptionProvider floatMenuOptionProvider = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				FloatMenuOption singleOption = floatMenuOptionProvider.GetSingleOption(context);
				if (singleOption != null)
				{
					_003C_003E2__current = singleOption;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptions_003Ed__21 _003CGetOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptions_003Ed__ = this;
			}
			else
			{
				_003CGetOptions_003Ed__ = new _003CGetOptions_003Ed__21(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetOptions_003Ed__.context = _003C_003E3__context;
			return _003CGetOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__22 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FloatMenuOptionProvider _003C_003E4__this;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

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
		public _003CGetOptionsFor_003Ed__22(int _003C_003E1__state)
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
			FloatMenuOptionProvider floatMenuOptionProvider = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				FloatMenuOption singleOptionFor = floatMenuOptionProvider.GetSingleOptionFor(clickedThing, context);
				if (singleOptionFor != null)
				{
					_003C_003E2__current = singleOptionFor;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__22 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__22(0)
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
	private sealed class _003CGetOptionsFor_003Ed__23 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FloatMenuOptionProvider _003C_003E4__this;

		private Pawn clickedPawn;

		public Pawn _003C_003E3__clickedPawn;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

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
		public _003CGetOptionsFor_003Ed__23(int _003C_003E1__state)
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
			FloatMenuOptionProvider floatMenuOptionProvider = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				FloatMenuOption singleOptionFor = floatMenuOptionProvider.GetSingleOptionFor(clickedPawn, context);
				if (singleOptionFor != null)
				{
					_003C_003E2__current = singleOptionFor;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__23 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__23(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetOptionsFor_003Ed__.clickedPawn = _003C_003E3__clickedPawn;
			_003CGetOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	protected abstract bool Drafted { get; }

	protected abstract bool Undrafted { get; }

	protected abstract bool Multiselect { get; }

	protected virtual bool RequiresManipulation => false;

	protected virtual bool MechanoidCanDo => false;

	protected virtual bool CanSelfTarget => false;

	public virtual bool CanTargetDespawned => false;

	protected virtual bool IgnoreFogged => true;

	public virtual bool SelectedPawnValid(Pawn pawn, FloatMenuContext context)
	{
		if (pawn.IsMutant && pawn.mutant.Def.whitelistedFloatMenuProviders != null && !pawn.mutant.Def.whitelistedFloatMenuProviders.Contains(FloatMenuMakerMap.currentProvider.GetType()))
		{
			return false;
		}
		if (!Drafted && pawn.Drafted)
		{
			return false;
		}
		if (!Undrafted && !pawn.Drafted)
		{
			return false;
		}
		if (!MechanoidCanDo && pawn.RaceProps.IsMechanoid)
		{
			return false;
		}
		if (RequiresManipulation && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			return false;
		}
		return true;
	}

	public virtual bool TargetThingValid(Thing thing, FloatMenuContext context)
	{
		if (!CanTargetDespawned && !thing.Spawned)
		{
			return false;
		}
		if (thing is Pawn pawn && !TargetPawnValid(pawn, context))
		{
			return false;
		}
		return true;
	}

	public virtual bool TargetPawnValid(Pawn pawn, FloatMenuContext context)
	{
		if (!CanSelfTarget && pawn == context.FirstSelectedPawn)
		{
			return false;
		}
		return true;
	}

	public virtual bool Applies(FloatMenuContext context)
	{
		if (!Multiselect && context.IsMultiselect)
		{
			return false;
		}
		if (IgnoreFogged && context.ClickedCell.Fogged(context.map))
		{
			return false;
		}
		if (!AppliesInt(context))
		{
			return false;
		}
		return true;
	}

	protected virtual bool AppliesInt(FloatMenuContext context)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetOptions_003Ed__21))]
	public virtual IEnumerable<FloatMenuOption> GetOptions(FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptions_003Ed__21(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__context = context
		};
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__22))]
	public virtual IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__22(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__23))]
	public virtual IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__23(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__clickedPawn = clickedPawn,
			_003C_003E3__context = context
		};
	}

	protected virtual FloatMenuOption GetSingleOption(FloatMenuContext context)
	{
		return null;
	}

	protected virtual FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
	{
		return null;
	}

	protected virtual FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
	{
		return null;
	}
}
