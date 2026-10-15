using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public class PawnInventoryOption
{
	[CompilerGenerated]
	private sealed class _003CGenerateThings_003Ed__6 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PawnInventoryOption _003C_003E4__this;

		private List<PawnInventoryOption>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<Thing> _003C_003E7__wrap2;

		Thing IEnumerator<Thing>.Current
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
		public _003CGenerateThings_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 2:
				try
				{
					if (num == -4 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<PawnInventoryOption>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PawnInventoryOption pawnInventoryOption = _003C_003E4__this;
				PawnInventoryOption pawnInventoryOption2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (Rand.Value < pawnInventoryOption.skipChance)
					{
						return false;
					}
					if (pawnInventoryOption.thingDef != null && pawnInventoryOption.countRange.max > 0)
					{
						Thing thing = ThingMaker.MakeThing(pawnInventoryOption.thingDef);
						thing.stackCount = pawnInventoryOption.countRange.RandomInRange;
						_003C_003E2__current = thing;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0096;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0096;
				case 2:
					_003C_003E1__state = -4;
					goto IL_0110;
				case 3:
					{
						_003C_003E1__state = -5;
						goto IL_01c4;
					}
					IL_012a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						PawnInventoryOption current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.GenerateThings().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0110;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<PawnInventoryOption>.Enumerator);
					goto IL_0149;
					IL_0149:
					if (pawnInventoryOption.subOptionsChooseOne == null)
					{
						break;
					}
					pawnInventoryOption2 = pawnInventoryOption.subOptionsChooseOne.RandomElementByWeight((PawnInventoryOption o) => o.choiceChance);
					_003C_003E7__wrap2 = pawnInventoryOption2.GenerateThings().GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_01c4;
					IL_0110:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Thing current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_012a;
					IL_0096:
					if (pawnInventoryOption.subOptionsTakeAll != null)
					{
						_003C_003E7__wrap1 = pawnInventoryOption.subOptionsTakeAll.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_012a;
					}
					goto IL_0149;
					IL_01c4:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Thing current3 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap2 = null;
					break;
				}
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGenerateThings_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGenerateThings_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public ThingDef thingDef;

	public IntRange countRange = IntRange.One;

	public float choiceChance = 1f;

	public float skipChance;

	public List<PawnInventoryOption> subOptionsTakeAll;

	public List<PawnInventoryOption> subOptionsChooseOne;

	[IteratorStateMachine(typeof(_003CGenerateThings_003Ed__6))]
	public IEnumerable<Thing> GenerateThings()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateThings_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
