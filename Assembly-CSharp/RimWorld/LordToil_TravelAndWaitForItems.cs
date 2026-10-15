using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class LordToil_TravelAndWaitForItems : LordToil_Travel, IWaitForItemsLordToil
{
	[CompilerGenerated]
	private sealed class _003CExtraFloatMenuOptions_003Ed__9 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public LordToil_TravelAndWaitForItems _003C_003E4__this;

		private Pawn requester;

		public Pawn _003C_003E3__requester;

		private Pawn current;

		public Pawn _003C_003E3__current;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

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
		public _003CExtraFloatMenuOptions_003Ed__9(int _003C_003E1__state)
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
				LordToil_TravelAndWaitForItems lordToil_TravelAndWaitForItems = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
				}
				else
				{
					_003C_003E1__state = -1;
					if (lordToil_TravelAndWaitForItems.target != requester)
					{
						goto IL_00a3;
					}
					_003C_003E7__wrap1 = GiveItemsToPawnUtility.GetFloatMenuOptionsForPawn(requester, current, lordToil_TravelAndWaitForItems.requestedThingDef, lordToil_TravelAndWaitForItems.requestedThingCount).GetEnumerator();
					_003C_003E1__state = -3;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption floatMenuOption = _003C_003E7__wrap1.Current;
					_003C_003E2__current = floatMenuOption;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				goto IL_00a3;
				IL_00a3:
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
			_003CExtraFloatMenuOptions_003Ed__9 _003CExtraFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CExtraFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CExtraFloatMenuOptions_003Ed__ = new _003CExtraFloatMenuOptions_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CExtraFloatMenuOptions_003Ed__.requester = _003C_003E3__requester;
			_003CExtraFloatMenuOptions_003Ed__.current = _003C_003E3__current;
			return _003CExtraFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public Pawn target;

	public ThingDef requestedThingDef;

	public int requestedThingCount;

	public bool HasAllRequestedItems => CountRemaining <= 0;

	public int CountRemaining => GiveItemsToPawnUtility.GetCountRemaining(target, requestedThingDef, requestedThingCount);

	public LordToil_TravelAndWaitForItems(IntVec3 dest, Pawn target, ThingDef thingDef, int amount)
		: base(dest)
	{
		this.target = target;
		requestedThingDef = thingDef;
		requestedThingCount = amount;
	}

	public override void DrawPawnGUIOverlay(Pawn pawn)
	{
		if (pawn == target)
		{
			pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
		}
	}

	[IteratorStateMachine(typeof(_003CExtraFloatMenuOptions_003Ed__9))]
	public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn requester, Pawn current)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CExtraFloatMenuOptions_003Ed__9(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__requester = requester,
			_003C_003E3__current = current
		};
	}
}
