using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_Toxifier : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_Toxifier _003C_003E4__this;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__4(int _003C_003E1__state)
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
			CompProperties_Toxifier compProperties_Toxifier = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (compProperties_Toxifier.pollutionIntervalTicks <= 0)
				{
					_003C_003E2__current = "Pollution interval ticks must be greater than zero.";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0051;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0051;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0075;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0075:
				if (compProperties_Toxifier.radius <= 0f)
				{
					_003C_003E2__current = "Radius to pollute must be greater than zero.";
					_003C_003E1__state = 3;
					return true;
				}
				break;
				IL_0051:
				if (compProperties_Toxifier.cellsToPollute <= 0)
				{
					_003C_003E2__current = "Cells to pollute must be greater than zero.";
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0075;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public float radius = 5.9f;

	public int pollutionIntervalTicks = 60000;

	public int cellsToPollute = 6;

	public CompProperties_Toxifier()
	{
		compClass = typeof(CompToxifier);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__4))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}
}
