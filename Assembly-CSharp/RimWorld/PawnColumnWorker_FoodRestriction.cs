using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class PawnColumnWorker_FoodRestriction : PawnColumnWorker
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public Pawn pawn;

		internal void _003CButton_GenerateMenu_003Eb__1()
		{
			Find.WindowStack.Add(new Dialog_ManageFoodPolicies(pawn.foodRestriction.CurrentFoodPolicy));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_1
	{
		public FoodPolicy foodRestriction;

		public _003C_003Ec__DisplayClass4_0 CS_0024_003C_003E8__locals1;

		internal void _003CButton_GenerateMenu_003Eb__0()
		{
			CS_0024_003C_003E8__locals1.pawn.foodRestriction.CurrentFoodPolicy = foodRestriction;
		}
	}

	[CompilerGenerated]
	private sealed class _003CButton_GenerateMenu_003Ed__4 : IEnumerable<Widgets.DropdownMenuElement<FoodPolicy>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<FoodPolicy>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<FoodPolicy> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private _003C_003Ec__DisplayClass4_0 _003C_003E8__1;

		private List<FoodPolicy>.Enumerator _003C_003E7__wrap1;

		Widgets.DropdownMenuElement<FoodPolicy> IEnumerator<Widgets.DropdownMenuElement<FoodPolicy>>.Current
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
		public _003CButton_GenerateMenu_003Ed__4(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = default(List<FoodPolicy>.Enumerator);
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
					_003C_003E8__1 = new _003C_003Ec__DisplayClass4_0();
					_003C_003E8__1.pawn = pawn;
					_003C_003E7__wrap1 = Current.Game.foodRestrictionDatabase.AllFoodRestrictions.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00ee;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00ee;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_00ee:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003C_003Ec__DisplayClass4_1 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass4_1
						{
							CS_0024_003C_003E8__locals1 = _003C_003E8__1,
							foodRestriction = _003C_003E7__wrap1.Current
						};
						_003C_003E2__current = new Widgets.DropdownMenuElement<FoodPolicy>
						{
							option = new FloatMenuOption(CS_0024_003C_003E8__locals4.foodRestriction.label, delegate
							{
								CS_0024_003C_003E8__locals4.CS_0024_003C_003E8__locals1.pawn.foodRestriction.CurrentFoodPolicy = CS_0024_003C_003E8__locals4.foodRestriction;
							}),
							payload = CS_0024_003C_003E8__locals4.foodRestriction
						};
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<FoodPolicy>.Enumerator);
					_003C_003E2__current = new Widgets.DropdownMenuElement<FoodPolicy>
					{
						option = new FloatMenuOption($"{"AssignTabEdit".Translate()}...", delegate
						{
							Find.WindowStack.Add(new Dialog_ManageFoodPolicies(_003C_003E8__1.pawn.foodRestriction.CurrentFoodPolicy));
						})
					};
					_003C_003E1__state = 2;
					return true;
				}
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
		IEnumerator<Widgets.DropdownMenuElement<FoodPolicy>> IEnumerable<Widgets.DropdownMenuElement<FoodPolicy>>.GetEnumerator()
		{
			_003CButton_GenerateMenu_003Ed__4 _003CButton_GenerateMenu_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CButton_GenerateMenu_003Ed__ = this;
			}
			else
			{
				_003CButton_GenerateMenu_003Ed__ = new _003CButton_GenerateMenu_003Ed__4(0);
			}
			_003CButton_GenerateMenu_003Ed__.pawn = _003C_003E3__pawn;
			return _003CButton_GenerateMenu_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Widgets.DropdownMenuElement<FoodPolicy>>)this).GetEnumerator();
		}
	}

	private const int TopAreaHeight = 65;

	public const int ManageFoodRestrictionsButtonHeight = 32;

	public override void DoHeader(Rect rect, PawnTable table)
	{
		base.DoHeader(rect, table);
		MouseoverSounds.DoRegion(rect);
		if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageFoodPolicies".Translate(), drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			Find.WindowStack.Add(new Dialog_ManageFoodPolicies(null));
		}
	}

	public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
	{
		if (pawn.foodRestriction != null)
		{
			DoAssignFoodRestrictionButtons(rect, pawn);
		}
	}

	[IteratorStateMachine(typeof(_003CButton_GenerateMenu_003Ed__4))]
	private IEnumerable<Widgets.DropdownMenuElement<FoodPolicy>> Button_GenerateMenu(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CButton_GenerateMenu_003Ed__4(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override int GetMinWidth(PawnTable table)
	{
		return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
	}

	public override int GetOptimalWidth(PawnTable table)
	{
		return Mathf.Clamp(Mathf.CeilToInt(251f), GetMinWidth(table), GetMaxWidth(table));
	}

	public override int GetMinHeaderHeight(PawnTable table)
	{
		return Mathf.Max(base.GetMinHeaderHeight(table), 65);
	}

	public override int Compare(Pawn a, Pawn b)
	{
		return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
	}

	private int GetValueToCompare(Pawn pawn)
	{
		if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodPolicy != null)
		{
			return pawn.foodRestriction.CurrentFoodPolicy.id;
		}
		return int.MinValue;
	}

	private void DoAssignFoodRestrictionButtons(Rect rect, Pawn pawn)
	{
		Rect rect2 = rect.ContractedBy(0f, 2f);
		Widgets.Dropdown(rect2, pawn, (Pawn p) => p.foodRestriction.CurrentFoodPolicy, Button_GenerateMenu, pawn.foodRestriction.CurrentFoodPolicy.label.Truncate(rect2.width), null, pawn.foodRestriction.CurrentFoodPolicy.label, null, null, paintable: true);
	}
}
