using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ScenPart_ConfigPage : ScenPart
{
	[CompilerGenerated]
	private sealed class _003CGetConfigPages_003Ed__0 : IEnumerable<Page>, IEnumerable, IEnumerator<Page>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Page _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ScenPart_ConfigPage _003C_003E4__this;

		Page IEnumerator<Page>.Current
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
		public _003CGetConfigPages_003Ed__0(int _003C_003E1__state)
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
			ScenPart_ConfigPage scenPart_ConfigPage = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = (Page)Activator.CreateInstance(scenPart_ConfigPage.def.pageClass);
				_003C_003E1__state = 1;
				return true;
			case 1:
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
		IEnumerator<Page> IEnumerable<Page>.GetEnumerator()
		{
			_003CGetConfigPages_003Ed__0 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConfigPages_003Ed__0(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Page>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetConfigPages_003Ed__0))]
	public override IEnumerable<Page> GetConfigPages()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConfigPages_003Ed__0(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void DoEditInterface(Listing_ScenEdit listing)
	{
	}
}
