using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class PawnColumnWorker_Outfit : PawnColumnWorker
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public Pawn pawn;

		internal void _003CButton_GenerateMenu_003Eb__1()
		{
			Find.WindowStack.Add(new Dialog_ManageApparelPolicies(pawn.outfits.CurrentApparelPolicy));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_1
	{
		public ApparelPolicy outfit;

		public _003C_003Ec__DisplayClass4_0 CS_0024_003C_003E8__locals1;

		internal void _003CButton_GenerateMenu_003Eb__0()
		{
			CS_0024_003C_003E8__locals1.pawn.outfits.CurrentApparelPolicy = outfit;
		}
	}

	[CompilerGenerated]
	private sealed class _003CButton_GenerateMenu_003Ed__4 : IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<ApparelPolicy>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<ApparelPolicy> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private _003C_003Ec__DisplayClass4_0 _003C_003E8__1;

		private List<ApparelPolicy>.Enumerator _003C_003E7__wrap1;

		Widgets.DropdownMenuElement<ApparelPolicy> IEnumerator<Widgets.DropdownMenuElement<ApparelPolicy>>.Current
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
			_003C_003E7__wrap1 = default(List<ApparelPolicy>.Enumerator);
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
					_003C_003E7__wrap1 = Current.Game.outfitDatabase.AllOutfits.GetEnumerator();
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
							outfit = _003C_003E7__wrap1.Current
						};
						_003C_003E2__current = new Widgets.DropdownMenuElement<ApparelPolicy>
						{
							option = new FloatMenuOption(CS_0024_003C_003E8__locals4.outfit.label, delegate
							{
								CS_0024_003C_003E8__locals4.CS_0024_003C_003E8__locals1.pawn.outfits.CurrentApparelPolicy = CS_0024_003C_003E8__locals4.outfit;
							}),
							payload = CS_0024_003C_003E8__locals4.outfit
						};
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<ApparelPolicy>.Enumerator);
					_003C_003E2__current = new Widgets.DropdownMenuElement<ApparelPolicy>
					{
						option = new FloatMenuOption($"{"AssignTabEdit".Translate()}...", delegate
						{
							Find.WindowStack.Add(new Dialog_ManageApparelPolicies(_003C_003E8__1.pawn.outfits.CurrentApparelPolicy));
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
		IEnumerator<Widgets.DropdownMenuElement<ApparelPolicy>> IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>>.GetEnumerator()
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
			return ((IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>>)this).GetEnumerator();
		}
	}

	public const int TopAreaHeight = 65;

	public const int ManageOutfitsButtonHeight = 32;

	public override void DoHeader(Rect rect, PawnTable table)
	{
		base.DoHeader(rect, table);
		MouseoverSounds.DoRegion(rect);
		Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
		if (Widgets.ButtonText(rect2, "ManageApparelPolicies".Translate(), drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			Find.WindowStack.Add(new Dialog_ManageApparelPolicies(null));
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Outfits, KnowledgeAmount.Total);
		}
		UIHighlighter.HighlightOpportunity(rect2, "ManageOutfits");
	}

	public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
	{
		if (pawn.outfits == null)
		{
			return;
		}
		Rect rect2 = rect.ContractedBy(0f, 2f);
		bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
		Rect left = rect2;
		Rect right = default(Rect);
		if (somethingIsForced)
		{
			rect2.SplitVerticallyWithMargin(out left, out right, 4f);
		}
		if (pawn.IsQuestLodger())
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(left, "Unchangeable".Translate().Truncate(left.width));
			TooltipHandler.TipRegionByKey(left, "QuestRelated_Outfit");
			Text.Anchor = TextAnchor.UpperLeft;
		}
		else
		{
			Widgets.Dropdown(left, pawn, (Pawn p) => p.outfits.CurrentApparelPolicy, Button_GenerateMenu, pawn.outfits.CurrentApparelPolicy.label.Truncate(left.width), null, pawn.outfits.CurrentApparelPolicy.label, null, null, paintable: true);
		}
		if (!somethingIsForced)
		{
			return;
		}
		if (Widgets.ButtonText(right, "ClearForcedApparel".Translate(), drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			pawn.outfits.forcedHandler.Reset();
		}
		if (!Mouse.IsOver(right))
		{
			return;
		}
		TooltipHandler.TipRegion(right, new TipSignal(delegate
		{
			string text = "ForcedApparel".Translate() + ":\n";
			foreach (Apparel item in pawn.outfits.forcedHandler.ForcedApparel)
			{
				text = text + "\n   " + item.LabelCap;
			}
			return text;
		}, pawn.GetHashCode() * 612));
	}

	[IteratorStateMachine(typeof(_003CButton_GenerateMenu_003Ed__4))]
	private IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>> Button_GenerateMenu(Pawn pawn)
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
		if (pawn.outfits != null && pawn.outfits.CurrentApparelPolicy != null)
		{
			return pawn.outfits.CurrentApparelPolicy.id;
		}
		return int.MinValue;
	}
}
