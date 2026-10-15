using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StatWorker_MeatAmount : StatWorker
{
	[CompilerGenerated]
	private sealed class _003CGetInfoCardHyperlinks_003Ed__1 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StatWorker_MeatAmount _003C_003E4__this;

		private StatRequest statRequest;

		public StatRequest _003C_003E3__statRequest;

		private IEnumerator<Dialog_InfoCard.Hyperlink> _003C_003E7__wrap1;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
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
		public _003CGetInfoCardHyperlinks_003Ed__1(int _003C_003E1__state)
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
				StatWorker_MeatAmount statWorker_MeatAmount = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((StatWorker)statWorker_MeatAmount).GetInfoCardHyperlinks(statRequest).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Dialog_InfoCard.Hyperlink current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!statRequest.HasThing || statRequest.Thing.def.race == null || statRequest.Thing.def.race.meatDef == null)
					{
						return false;
					}
					_003C_003E2__current = new Dialog_InfoCard.Hyperlink(statRequest.Thing.def.race.meatDef);
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
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003CGetInfoCardHyperlinks_003Ed__1 _003CGetInfoCardHyperlinks_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetInfoCardHyperlinks_003Ed__ = this;
			}
			else
			{
				_003CGetInfoCardHyperlinks_003Ed__ = new _003CGetInfoCardHyperlinks_003Ed__1(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetInfoCardHyperlinks_003Ed__.statRequest = _003C_003E3__statRequest;
			return _003CGetInfoCardHyperlinks_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	public override bool ShouldShowFor(StatRequest req)
	{
		if (req.Pawn?.RaceProps?.hasMeat == true)
		{
			return false;
		}
		return base.ShouldShowFor(req);
	}

	[IteratorStateMachine(typeof(_003CGetInfoCardHyperlinks_003Ed__1))]
	public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInfoCardHyperlinks_003Ed__1(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__statRequest = statRequest
		};
	}
}
