using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_MeditationFocus : CompProperties_StatOffsetBase
{
	[CompilerGenerated]
	private sealed class _003CGetExplanationAbstract_003Ed__2 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_MeditationFocus _003C_003E4__this;

		private ThingDef def;

		public ThingDef _003C_003E3__def;

		private int _003Ci_003E5__2;

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
		public _003CGetExplanationAbstract_003Ed__2(int _003C_003E1__state)
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
			CompProperties_MeditationFocus compProperties_MeditationFocus = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0063;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_0073;
			IL_0063:
			_003Ci_003E5__2++;
			goto IL_0073;
			IL_0073:
			if (_003Ci_003E5__2 < compProperties_MeditationFocus.offsets.Count)
			{
				string explanationAbstract = compProperties_MeditationFocus.offsets[_003Ci_003E5__2].GetExplanationAbstract(def);
				if (!explanationAbstract.NullOrEmpty())
				{
					_003C_003E2__current = explanationAbstract;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0063;
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
			_003CGetExplanationAbstract_003Ed__2 _003CGetExplanationAbstract_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetExplanationAbstract_003Ed__ = this;
			}
			else
			{
				_003CGetExplanationAbstract_003Ed__ = new _003CGetExplanationAbstract_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetExplanationAbstract_003Ed__.def = _003C_003E3__def;
			return _003CGetExplanationAbstract_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public List<MeditationFocusDef> focusTypes = new List<MeditationFocusDef>();

	public CompProperties_MeditationFocus()
	{
		compClass = typeof(CompMeditationFocus);
	}

	[IteratorStateMachine(typeof(_003CGetExplanationAbstract_003Ed__2))]
	public override IEnumerable<string> GetExplanationAbstract(ThingDef def)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetExplanationAbstract_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__def = def
		};
	}

	public override void ResolveReferences(ThingDef parent)
	{
		base.PostLoadSpecial(parent);
		for (int i = 0; i < offsets.Count; i++)
		{
			offsets[i].ResolveReferences();
		}
	}
}
