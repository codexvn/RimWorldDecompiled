using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Precept_RitualSeat : Precept_ThingDef
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public PreceptThingChance d;

		public Precept_RitualSeat _003C_003E4__this;

		internal void _003CEditFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.ThingDef = d.def;
		}
	}

	[CompilerGenerated]
	private sealed class _003CEditFloatMenuOptions_003Ed__2 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_RitualSeat _003C_003E4__this;

		private IEnumerator<PreceptThingChance> _003C_003E7__wrap1;

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
		public _003CEditFloatMenuOptions_003Ed__2(int _003C_003E1__state)
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
				Precept_RitualSeat precept_RitualSeat = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = precept_RitualSeat.def.Worker.ThingDefs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass2_0
					{
						_003C_003E4__this = precept_RitualSeat,
						d = _003C_003E7__wrap1.Current
					};
					if (CS_0024_003C_003E8__locals5.d.def != precept_RitualSeat.ThingDef)
					{
						_003C_003E2__current = new FloatMenuOption("PreceptReplaceWith".Translate() + ": " + CS_0024_003C_003E8__locals5.d.def.LabelCap, delegate
						{
							CS_0024_003C_003E8__locals5._003C_003E4__this.ThingDef = CS_0024_003C_003E8__locals5.d.def;
						}, CS_0024_003C_003E8__locals5.d.def, null, forceBasicStyle: false, MenuOptionPriority.Default, null, null, 0f, null, null, playSelectionSound: true, 0, null);
						_003C_003E1__state = 1;
						return true;
					}
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
			_003CEditFloatMenuOptions_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEditFloatMenuOptions_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public override string UIInfoSecondLine => base.ThingDef.LabelCap;

	[IteratorStateMachine(typeof(_003CEditFloatMenuOptions_003Ed__2))]
	public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEditFloatMenuOptions_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}
}
