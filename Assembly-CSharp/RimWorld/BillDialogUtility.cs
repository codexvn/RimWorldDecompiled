using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class BillDialogUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public Func<Pawn, bool> pawnValidator;

		public Bill bill;

		public WorkGiverDef workGiver;

		internal bool _003CGetPawnRestrictionOptionsForBill_003Eb__0(Pawn p)
		{
			if (pawnValidator != null)
			{
				return pawnValidator(p);
			}
			return true;
		}

		internal int _003CGetPawnRestrictionOptionsForBill_003Eb__2(Pawn pawn)
		{
			return pawn.skills.GetSkill(bill.recipe.workSkill).Level;
		}

		internal bool _003CGetPawnRestrictionOptionsForBill_003Eb__3(Pawn pawn)
		{
			return pawn.workSettings.WorkIsActive(workGiver.workType);
		}

		internal bool _003CGetPawnRestrictionOptionsForBill_003Eb__4(Pawn pawn)
		{
			return pawn.WorkTypeIsDisabled(workGiver.workType);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_1
	{
		public Pawn pawn;

		public _003C_003Ec__DisplayClass0_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetPawnRestrictionOptionsForBill_003Eb__5()
		{
			CS_0024_003C_003E8__locals1.bill.SetPawnRestriction(pawn);
		}

		internal void _003CGetPawnRestrictionOptionsForBill_003Eb__6()
		{
			CS_0024_003C_003E8__locals1.bill.SetPawnRestriction(pawn);
		}

		internal void _003CGetPawnRestrictionOptionsForBill_003Eb__7()
		{
			CS_0024_003C_003E8__locals1.bill.SetPawnRestriction(pawn);
		}

		internal void _003CGetPawnRestrictionOptionsForBill_003Eb__8()
		{
			CS_0024_003C_003E8__locals1.bill.SetPawnRestriction(pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetPawnRestrictionOptionsForBill_003Ed__0 : IEnumerable<Widgets.DropdownMenuElement<Pawn>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<Pawn>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<Pawn> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Func<Pawn, bool> pawnValidator;

		public Func<Pawn, bool> _003C_003E3__pawnValidator;

		private Bill bill;

		public Bill _003C_003E3__bill;

		private _003C_003Ec__DisplayClass0_0 _003C_003E8__1;

		private IEnumerator<Pawn> _003C_003E7__wrap1;

		Widgets.DropdownMenuElement<Pawn> IEnumerator<Widgets.DropdownMenuElement<Pawn>>.Current
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
		public _003CGetPawnRestrictionOptionsForBill_003Ed__0(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass0_0();
					_003C_003E8__1.pawnValidator = pawnValidator;
					_003C_003E8__1.bill = bill;
					SkillDef workSkill = _003C_003E8__1.bill.recipe.workSkill;
					IEnumerable<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
					allMaps_FreeColonists = from p in allMaps_FreeColonists
						where _003C_003E8__1.pawnValidator == null || _003C_003E8__1.pawnValidator(p)
						select p into pawn
						orderby pawn.LabelShortCap
						select pawn;
					if (workSkill != null)
					{
						allMaps_FreeColonists = allMaps_FreeColonists.OrderByDescending((Pawn pawn) => pawn.skills.GetSkill(_003C_003E8__1.bill.recipe.workSkill).Level);
					}
					_003C_003E8__1.workGiver = _003C_003E8__1.bill.billStack.billGiver.GetWorkgiver();
					if (_003C_003E8__1.workGiver == null)
					{
						Log.ErrorOnce("Generating pawn restrictions for a BillGiver without a Workgiver", 96455148);
						return false;
					}
					allMaps_FreeColonists = allMaps_FreeColonists.OrderByDescending((Pawn pawn) => pawn.workSettings.WorkIsActive(_003C_003E8__1.workGiver.workType));
					allMaps_FreeColonists = allMaps_FreeColonists.OrderBy((Pawn pawn) => pawn.WorkTypeIsDisabled(_003C_003E8__1.workGiver.workType));
					_003C_003E7__wrap1 = allMaps_FreeColonists.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
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
					_003C_003Ec__DisplayClass0_1 CS_0024_003C_003E8__locals33 = new _003C_003Ec__DisplayClass0_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						pawn = _003C_003E7__wrap1.Current
					};
					if (CS_0024_003C_003E8__locals33.pawn.WorkTypeIsDisabled(CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.workGiver.workType))
					{
						_003C_003E2__current = new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", CS_0024_003C_003E8__locals33.pawn.LabelShortCap, "WillNever".Translate(CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.workGiver.label)), null),
							payload = CS_0024_003C_003E8__locals33.pawn
						};
						_003C_003E1__state = 1;
						return true;
					}
					if (CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.recipe.workSkill != null && !CS_0024_003C_003E8__locals33.pawn.workSettings.WorkIsActive(CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.workGiver.workType))
					{
						_003C_003E2__current = new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1} {2}, {3})", CS_0024_003C_003E8__locals33.pawn.LabelShortCap, CS_0024_003C_003E8__locals33.pawn.skills.GetSkill(CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.recipe.workSkill).Level, CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.recipe.workSkill.label, "NotAssigned".Translate()), delegate
							{
								CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.SetPawnRestriction(CS_0024_003C_003E8__locals33.pawn);
							}),
							payload = CS_0024_003C_003E8__locals33.pawn
						};
						_003C_003E1__state = 2;
						return true;
					}
					if (!CS_0024_003C_003E8__locals33.pawn.workSettings.WorkIsActive(CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.workGiver.workType))
					{
						_003C_003E2__current = new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", CS_0024_003C_003E8__locals33.pawn.LabelShortCap, "NotAssigned".Translate()), delegate
							{
								CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.SetPawnRestriction(CS_0024_003C_003E8__locals33.pawn);
							}),
							payload = CS_0024_003C_003E8__locals33.pawn
						};
						_003C_003E1__state = 3;
						return true;
					}
					if (CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.recipe.workSkill != null)
					{
						_003C_003E2__current = new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption($"{CS_0024_003C_003E8__locals33.pawn.LabelShortCap} ({CS_0024_003C_003E8__locals33.pawn.skills.GetSkill(CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.recipe.workSkill).Level} {CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.recipe.workSkill.label})", delegate
							{
								CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.SetPawnRestriction(CS_0024_003C_003E8__locals33.pawn);
							}),
							payload = CS_0024_003C_003E8__locals33.pawn
						};
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003E2__current = new Widgets.DropdownMenuElement<Pawn>
					{
						option = new FloatMenuOption($"{CS_0024_003C_003E8__locals33.pawn.LabelShortCap}", delegate
						{
							CS_0024_003C_003E8__locals33.CS_0024_003C_003E8__locals1.bill.SetPawnRestriction(CS_0024_003C_003E8__locals33.pawn);
						}),
						payload = CS_0024_003C_003E8__locals33.pawn
					};
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
		IEnumerator<Widgets.DropdownMenuElement<Pawn>> IEnumerable<Widgets.DropdownMenuElement<Pawn>>.GetEnumerator()
		{
			_003CGetPawnRestrictionOptionsForBill_003Ed__0 _003CGetPawnRestrictionOptionsForBill_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPawnRestrictionOptionsForBill_003Ed__ = this;
			}
			else
			{
				_003CGetPawnRestrictionOptionsForBill_003Ed__ = new _003CGetPawnRestrictionOptionsForBill_003Ed__0(0);
			}
			_003CGetPawnRestrictionOptionsForBill_003Ed__.bill = _003C_003E3__bill;
			_003CGetPawnRestrictionOptionsForBill_003Ed__.pawnValidator = _003C_003E3__pawnValidator;
			return _003CGetPawnRestrictionOptionsForBill_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Widgets.DropdownMenuElement<Pawn>>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetPawnRestrictionOptionsForBill_003Ed__0))]
	public static IEnumerable<Widgets.DropdownMenuElement<Pawn>> GetPawnRestrictionOptionsForBill(Bill bill, Func<Pawn, bool> pawnValidator = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPawnRestrictionOptionsForBill_003Ed__0(-2)
		{
			_003C_003E3__bill = bill,
			_003C_003E3__pawnValidator = pawnValidator
		};
	}
}
