using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class PawnColumnWorker_ControlGroup : PawnColumnWorker
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public Pawn pawn;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_1
	{
		public MechanitorControlGroup localControlGroup;

		public _003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals1;

		internal void _003CButton_GenerateMenu_003Eb__0()
		{
			localControlGroup.Assign(CS_0024_003C_003E8__locals1.pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CButton_GenerateMenu_003Ed__3 : IEnumerable<Widgets.DropdownMenuElement<MechanitorControlGroup>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<MechanitorControlGroup>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<MechanitorControlGroup> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private _003C_003Ec__DisplayClass3_0 _003C_003E8__1;

		private MechanitorControlGroup _003CcurrentControlGroup_003E5__2;

		private int _003Ci_003E5__3;

		Widgets.DropdownMenuElement<MechanitorControlGroup> IEnumerator<Widgets.DropdownMenuElement<MechanitorControlGroup>>.Current
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
		public _003CButton_GenerateMenu_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003CcurrentControlGroup_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass3_0();
				_003C_003E8__1.pawn = pawn;
				Pawn overseer = _003C_003E8__1.pawn.GetOverseer();
				tmpControlGroups.Clear();
				tmpControlGroups.AddRange(overseer.mechanitor.controlGroups);
				_003CcurrentControlGroup_003E5__2 = _003C_003E8__1.pawn.GetMechControlGroup();
				_003Ci_003E5__3 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_01b4;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_01b4;
				}
				IL_01b4:
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < tmpControlGroups.Count)
			{
				_003C_003Ec__DisplayClass3_1 CS_0024_003C_003E8__locals7 = new _003C_003Ec__DisplayClass3_1
				{
					CS_0024_003C_003E8__locals1 = _003C_003E8__1,
					localControlGroup = tmpControlGroups[_003Ci_003E5__3]
				};
				if (_003CcurrentControlGroup_003E5__2 == CS_0024_003C_003E8__locals7.localControlGroup)
				{
					_003C_003E2__current = new Widgets.DropdownMenuElement<MechanitorControlGroup>
					{
						option = new FloatMenuOption("CannotAssignMechToControlGroup".Translate(CS_0024_003C_003E8__locals7.localControlGroup.LabelIndexWithWorkMode) + ": " + "AssignMechAlreadyAssigned".Translate(), null),
						payload = CS_0024_003C_003E8__locals7.localControlGroup
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new Widgets.DropdownMenuElement<MechanitorControlGroup>
				{
					option = new FloatMenuOption("AssignMechToControlGroup".Translate(CS_0024_003C_003E8__locals7.localControlGroup.LabelIndexWithWorkMode), delegate
					{
						CS_0024_003C_003E8__locals7.localControlGroup.Assign(CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.pawn);
					}),
					payload = CS_0024_003C_003E8__locals7.localControlGroup
				};
				_003C_003E1__state = 2;
				return true;
			}
			tmpControlGroups.Clear();
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
		IEnumerator<Widgets.DropdownMenuElement<MechanitorControlGroup>> IEnumerable<Widgets.DropdownMenuElement<MechanitorControlGroup>>.GetEnumerator()
		{
			_003CButton_GenerateMenu_003Ed__3 _003CButton_GenerateMenu_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CButton_GenerateMenu_003Ed__ = this;
			}
			else
			{
				_003CButton_GenerateMenu_003Ed__ = new _003CButton_GenerateMenu_003Ed__3(0);
			}
			_003CButton_GenerateMenu_003Ed__.pawn = _003C_003E3__pawn;
			return _003CButton_GenerateMenu_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Widgets.DropdownMenuElement<MechanitorControlGroup>>)this).GetEnumerator();
		}
	}

	private const int Width = 160;

	private static List<MechanitorControlGroup> tmpControlGroups = new List<MechanitorControlGroup>();

	public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
	{
		if (pawn.IsGestating())
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "Gestating".Translate().Colorize(ColoredText.SubtleGrayColor));
			Text.Anchor = TextAnchor.UpperLeft;
		}
		else if (pawn.GetOverseer() != null)
		{
			Widgets.Dropdown(rect, pawn, (Pawn p) => p.GetMechControlGroup(), Button_GenerateMenu, pawn.GetMechControlGroup().Index.ToString(), null, null, null, null, def.paintable);
		}
	}

	[IteratorStateMachine(typeof(_003CButton_GenerateMenu_003Ed__3))]
	private IEnumerable<Widgets.DropdownMenuElement<MechanitorControlGroup>> Button_GenerateMenu(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CButton_GenerateMenu_003Ed__3(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override int GetMinWidth(PawnTable table)
	{
		return Mathf.Max(base.GetMinWidth(table), 160);
	}

	public override int GetMaxWidth(PawnTable table)
	{
		return Mathf.Min(base.GetMaxWidth(table), GetMinWidth(table));
	}
}
