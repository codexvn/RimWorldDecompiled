using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Command_Hide_Plans : Command_Hide
{
	[CompilerGenerated]
	private sealed class _003CGetHideOptions_003Ed__3 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

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
		public _003CGetHideOptions_003Ed__3(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new FloatMenuOption("ShowAllZones".Translate(), delegate
				{
					Command_Hide.ToggleAll(hidden: false);
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new FloatMenuOption("HideAllZones".Translate(), delegate
				{
					Command_Hide.ToggleAll(hidden: true);
				});
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new FloatMenuOption("ShowAllZoneTypes".Translate("Planning".Translate()), delegate
				{
					Command_Hide.TogglePlans(hidden: false);
				}, PlanTex, Color.white);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new FloatMenuOption("HideAllZoneTypes".Translate("Planning".Translate()), delegate
				{
					Command_Hide.TogglePlans(hidden: true);
				}, PlanTex, Color.gray);
				_003C_003E1__state = 4;
				return true;
			case 4:
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
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetHideOptions_003Ed__3(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private static readonly Texture2D PlanTex = ContentFinder<Texture2D>.Get("UI/Designators/PlanOn");

	public Command_Hide_Plans(IHideable hideable)
		: base(hideable)
	{
	}

	protected override IEnumerable<FloatMenuOption> GetOptions()
	{
		return GetHideOptions();
	}

	[IteratorStateMachine(typeof(_003CGetHideOptions_003Ed__3))]
	public static IEnumerable<FloatMenuOption> GetHideOptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetHideOptions_003Ed__3(-2);
	}
}
